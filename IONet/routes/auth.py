from flask import Blueprint, jsonify, request
import app

auth_bp = Blueprint('auth', __name__)

@auth_bp.route('/api/auth/login', methods=['POST'])
def getUserData():
    data = request.get_json()
    username = data.get('username')
    password = data.get('password')
    app.logger.info(f"用户 {username} 请求以密码登录")

    app.sql.start()
    value = app.sql.get_user_token(username, password,app.logger)
    app.sql.close()

    if value == -1:
        return jsonify({"code": -1, "msg": "用户名或密码错误", "data": {}}), 401
    else:
        return jsonify({"code": 0, "msg": "登录成功", "data": value}), 200

@auth_bp.route('/api/auth/regist', methods=['POST'])
def tryRegistUser():
    data = request.get_json()
    username = data.get('username')
    password = data.get('password')
    email = data.get('email')

    app.sql.start()
    value = app.sql.add_user(username, email, password,logger=app.logger)
    app.sql.close()
    return jsonify({"code": value[0], "msg": value[1]}), 200

@auth_bp.route('/api/auth/logout', methods=['POST'])
def logout_user():
    data = request.get_json()
    token = data.get('token')
    
    if not token:
        return jsonify({"code": -1, "msg": "token 是必须的"}), 400
    
    app.sql.start()
    value = app.sql.delete_user_by_token(token,logger=app.logger)
    app.sql.close()
    
    return jsonify({"code": value[0], "msg": value[1]})

@auth_bp.route('/api/auth/privateFetch', methods=['POST'])
def private_fetch():
    data = request.get_json()
    token = data.get('token')

    if not token:
        return jsonify({"code": -1, "msg": "token 是必须的"}), 400

    app.sql.start()
    user_info = app.sql.get_user_info_by_token(token,logger=app.logger)
    app.sql.close()

    if user_info:
        return jsonify({"code": 0, "msg": "用户信息获取成功", "data": user_info}), 200
    else:
        return jsonify({"code": -1, "msg": "无效的 token 或用户不存在"}), 404
    
@auth_bp.route('/api/auth/checkin', methods=['POST'])
def check_in_user():
    data = request.get_json()
    token = data.get('token')

    if not token:
        return jsonify({"code": -1, "msg": "token 是必须的"}), 400

    app.sql.start()
    result = app.sql.check_in(token)
    app.sql.close()

    if result[0] == 0:
        # 返回成功状态码和最新的经验值
        return jsonify({"code": 0, "msg": result[1], "exp": result[1].get("exp", 0)}), 200
    else:
        return jsonify({"code": result[0], "msg": result[1]}), 400
