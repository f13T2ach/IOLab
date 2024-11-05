from flask import Blueprint, request, jsonify
from werkzeug.utils import secure_filename
import os
import workshopdb  # 引入新建的 workshopdb 模块
import config
import usrdb  # 假设 usrdb 仍然负责处理用户数据库

# 创建 Blueprint 对象
workshop_bp = Blueprint('workshop', __name__)
workshop_db = workshopdb.WorkshopDatabase()  # 实例化 workshop 数据库
user_db = usrdb.UserDatabase()  # 实例化用户数据库

# 启动数据库
workshop_db.start()
user_db.start()

# 文案上传功能
@workshop_bp.route('/upload', methods=['POST'])
def upload_content():
    token = request.headers.get("Authorization")
    content = request.form.get("content")
    json_file = request.files.get("json_file")
    
    # 校验用户身份
    user_info = user_db.get_user_info_by_token(token)
    if not user_info:
        return jsonify({"error": "无效的 token 或用户不存在"}), 403
    if user_info["groupid"] == -1:
        return jsonify({"error": "您的账户已被封禁，无法上传文案"}), 403

    # 处理上传的 JSON 文件
    json_filename = None
    if json_file:
        filename = secure_filename(json_file.filename)
        json_filename = os.path.join(config.DirConfig.uploadDir, filename)
        json_file.save(json_filename)

    # 添加文案到数据库
    result = workshop_db.add_workshop_item(user_id=user_info["username"], content=content, json_file=json_filename)
    if result[0] == 0:
        return jsonify({"message": result[1]}), 200
    else:
        return jsonify({"error": result[1]}), 500

# 分页浏览文案
@workshop_bp.route('/browse', methods=['GET'])
def browse_content():
    page = request.args.get('page', default=1, type=int)
    per_page = request.args.get('per_page', default=10, type=int)

    # 获取文案列表
    items = workshop_db.get_workshop_items(page=page, per_page=per_page)
    return jsonify(items), 200

# 下载 JSON 文件
@workshop_bp.route('/download/<int:item_id>', methods=['GET'])
def download_json(item_id):
    item = workshop_db.get_workshop_item_by_id(item_id)
    if not item:
        return jsonify({"error": "文案不存在"}), 404

    if not item["json_file"]:
        return jsonify({"error": "该文案无 JSON 文件"}), 404

    try:
        return jsonify({"json_file": item["json_file"]}), 200
    except FileNotFoundError:
        return jsonify({"error": "文件未找到"}), 404
