from flask import Blueprint, request, jsonify, send_from_directory
from werkzeug.utils import secure_filename
import os
import workshopdb
import config
import usrdb
from datetime import datetime
import uuid
from flask import make_response

# 创建 Blueprint 对象
workshop_bp = Blueprint('workshop', __name__)
workshop_db = workshopdb.WorkshopDatabase()  # 实例化 workshop 数据库
user_db = usrdb.UserDatabase()  # 实例化用户数据库

# 文案上传功能
@workshop_bp.route('/api/workshop/upload', methods=['POST'])
def upload_content():
    try:
        # 获取表单数据
        token = request.form["token"]
        title = request.form["title"]
        content = request.form["content"]
        created_at = request.form.get("created_at", datetime.now().isoformat())
        tags = request.form.getlist("tags[]")  # 获取 tags 数组

        # 校验用户身份
        user_info = user_db.get_user_info_by_token(token)
        if not user_info:
            return jsonify({"code": -1, "msg": "无效的 token 或用户不存在"}), 403
        if user_info["groupid"] == -1:
            return jsonify({"code": -1, "msg": "该账户已被封禁，无法上传文案"}), 403

        # 处理上传的 JSON 文件
        json_file = request.files.get("json_file")
        # 文件上传部分
        json_filename = None
        if json_file:
            # 使用 uuid 生成唯一的文件名
            file_extension = os.path.splitext(json_file.filename)[1]  # 获取文件的扩展名
            unique_filename = str(uuid.uuid4()).replace("-", "") + file_extension  # 去掉 UUID 中的 "-" 并加上扩展名

            json_filename = "." + config.DirConfig.uploadDir_relative + unique_filename

            # 保存文件
            json_file.save(json_filename)

        # 添加文案到数据库
        result = workshop_db.add_workshop_item(
            author_id=user_info["id"],
            title=title,
            content=content,
            created_at=created_at,
            tags=tags,
            json_file=unique_filename
        )
        if result[0] == 0:
            return jsonify({"code": 0, "msg": result[1]}), 200
        else:
            return jsonify({"code": -1, "msg": result[1]}), 500

    except KeyError as e:
        return jsonify({"code": -1, "msg": f"缺少必需的参数: {e}"}), 400
    except Exception as e:
        return jsonify({"code": -1, "msg": f"服务器内部错误: {e}"}), 500


@workshop_bp.route('/api/workshop/browse', methods=['GET'])
def browse_content():
    page = request.args.get('page', default=1, type=int)
    per_page = request.args.get('per_page', default=10, type=int)

    # 获取文案列表
    items = workshop_db.get_workshop_items(page=page, per_page=per_page)

    # 查询每个文案的作者名称以及 JSON 文件的 ID
    result_items = []
    for item in items:
        author_id = item.get('author_id')  # 获取作者 ID

        # 查询作者名称
        user_info = user_db.get_user_info_by_id(author_id)  # 通过 author_id 获取作者信息
        author_name = user_info['username'] if user_info else '未知作者'  # 如果用户不存在，则显示'未知作者'

        # 构建新的文案字典
        result_item = {
            'title': item.get('title'),  # 文案标题
            'content': item.get('content'),  # 文案内容
            'author_name': author_name,  # 作者名称
            'created_at': item.get('created_at'),  # 创建时间
            'id': item.get('id') # JSON 文件 ID
        }

        # 添加到结果列表
        result_items.append(result_item)

    return jsonify({"code": 0, "items": result_items}), 200


@workshop_bp.route('/api/workshop/download/<int:item_id>', methods=['GET'])
def download_json(item_id):
    item = workshop_db.get_workshop_item_by_id(item_id)
    if not item:
        return jsonify({"code": -1, "msg": "文案不存在"}), 404

    json_filename = item["json_file"]
    if not json_filename:
        return jsonify({"code": -1, "msg": "该文案无 JSON 文件"}), 404
        
    try:
        # 检查文件是否存在
        full_path = json_filename
        
        #print(full_path)

        if not os.path.exists(config.DirConfig.uploadDir + full_path):
            return jsonify({"code": -1, "msg": "文件未找到"}), 404
        
        #print(config.DirConfig.uploadDir)
        #print(json_filename)
        return send_from_directory(config.DirConfig.uploadDir, json_filename, as_attachment=True)
        #return jsonify({"code": 0, "msg": f"文件找到{full_path}"}), 200
    except FileNotFoundError:
        return jsonify({"code": -1, "msg": "文件未找到"}), 404
    except Exception as e:
        return jsonify({"code": -1, "msg": f"发生错误: {str(e)}"}), 500