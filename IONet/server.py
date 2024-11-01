from flask import Flask, jsonify, request
import config
from gevent import pywsgi, spawn
import app
import usrdb

service = Flask(__name__)

def launchServer():
    try:
        # 设置日志记录器
        service.logger = app.logging.getLogger('global')
        service.logger.info("正在启动服务器...")
        
        # 使用 gevent 的 spawn 启动服务器
        server = pywsgi.WSGIServer(('0.0.0.0', 9000), service)
        server.serve_forever()  # 非阻塞地启动服务器
        
        service.logger.info("服务器启动成功")
    except Exception as e:
        service.logger.error(f"服务器启动失败: {e}，实例即将退出")
        quit()

@service.route('/api/server', methods=['POST'])
def getServer():
    return jsonify(
        {"name": config.ServerConfig.name,"area":config.ServerConfig.area},
    )
@service.route('/api/auth/login', methods=['POST'])
def getUserData():
    data = request.get_json()
    username = data.get('username')
    password = data.get('password')
    app.logger.info(f"用户 {username} 请求以密码 {password} 登录")

    app.sql.start()
    value = app.sql.get_user_token(username,password)
    app.sql.close()

    if value == -1:  # 检查是否返回 -1
        return jsonify(
            {"code": -1, "msg": "用户名或密码错误", "data": {}}
        ), 401  # 返回未授权状态码
    else:
        return jsonify(
            {"code": 0, "msg": "登录成功", "data": value}  # 这里的 value 是字典
        ), 200  # 返回成功状态码

@service.route('/api/auth/regist',methods=['POST'])
def tryRegistUser():
    data = request.get_json()
    username = data.get('username')
    password = data.get('password')
    email = data.get('email')

    app.sql.start()
    value = app.sql.add_user(username,email,password)
    app.sql.close()
    return jsonify(
        {"code": value[0],"msg":value[1]},
    )



@service.route('/api/auth/logout', methods=['POST'])
def logout_user():
    data = request.get_json()
    token = data.get('token')
    
    if not token:
        return jsonify({"code":-1,"msg":"token 是必须的"}), 400
    app.sql.start()
    # 调用数据库的删除用户方法
    value = app.sql.delete_user_by_token(token)
    app.sql.close()
    
    return jsonify({"code":value[0],"msg":value[1]}), 200
# 启动服务器
if __name__ == '__main__':
    launchServer()
