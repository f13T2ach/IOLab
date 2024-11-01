import os
import logging
from datetime import datetime
import time
import config
from usrdb import UserDatabase


from flask import Flask
from gevent import pywsgi

logger = logging.getLogger('global')
sql = UserDatabase()
service = Flask(__name__)


# 定义 ANSI 转义序列
RESET_SEQ = "\033[0m"
COLOR_SEQ = "\033[1;%dm"
BOLD_SEQ = "\033[1m"

# 日志颜色定义
WARNING_COLOR = 33  # 黄色
ERROR_COLOR = 31    # 红色
INFO_COLOR = 37     # 白色

class ColoredFormatter(logging.Formatter):
    def format(self, record):
        original_msg = super().format(record)
        if record.levelno == logging.WARNING:
            return COLOR_SEQ % WARNING_COLOR + original_msg + RESET_SEQ
        elif record.levelno == logging.ERROR or record.levelno == logging.CRITICAL:
            return COLOR_SEQ % ERROR_COLOR + original_msg + RESET_SEQ
        else:
            return original_msg

def main():
    print("IOLab - Intelligent Experiment Platform")
    print("v1.0.0 Debug Mode | DO NOT PUBLISH")
    
    
    logger.warning("此实例为测试框架，请勿在工厂环境使用")
    
    logger.info("正在启动 主SQL 服务")
    sql.start()
    logger.info("主SQL 启动成功，尝试写入测试用户 1 密码 1")
    sql.add_user("1", "1@iolab.com", "1",logger=logger)
    sql.close()
    
    # 设置日志记录器

    service.logger = logger

    logger.info("正在导入业务路由")
    # 导入路由
    from routes import auth, server_info

    # 注册蓝图
    service.register_blueprint(auth.auth_bp)
    service.register_blueprint(server_info.server_info_bp)
    logger.info(f"正在启动服务器，监听 {config.ServerConfig.port}")
    server = pywsgi.WSGIServer(('0.0.0.0', config.ServerConfig.port), service)
    logger.info("服务器启动成功")
    server.serve_forever()  # 非阻塞地启动服务器
    


if __name__ == '__main__':
    log_dir = config.DirConfig.logDir
    if not os.path.exists(log_dir):
        os.makedirs(log_dir)
        print(f"Created log directory at {log_dir}")
    else:
        print("Already have log Dir")
        
    # 构造日志文件名
    logname = f"{log_dir}iolab_{datetime.now().strftime('%Y%m%d_%H%M%S')}_{int(time.time())}.txt"

    # 创建一个日志记录器
    logger.setLevel(logging.DEBUG)

    # 设置日志格式
    formatter = logging.Formatter('%(asctime)s - %(filename)s[line:%(lineno)d] - %(levelname)s: %(message)s')
    colored_formatter = ColoredFormatter('%(asctime)s - %(filename)s[line:%(lineno)d] - %(levelname)s: %(message)s')

    # 添加文件处理器
    file_handler = logging.FileHandler(logname)
    file_handler.setFormatter(formatter)
    logger.addHandler(file_handler)

    # 添加控制台处理器
    console_handler = logging.StreamHandler()
    console_handler.setFormatter(colored_formatter)
    logger.addHandler(console_handler)

    

    main()

    # 移除处理器（可选）
    logger.removeHandler(file_handler)
    logger.removeHandler(console_handler)

    