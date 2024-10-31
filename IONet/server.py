from flask import Flask, jsonify
import config
from gevent import pywsgi, spawn
import app

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
        {"name": config.ServerConfig.name},
        {"area": config.ServerConfig.area}
    )

# 启动服务器
if __name__ == '__main__':
    launchServer()
