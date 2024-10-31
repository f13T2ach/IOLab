import os
projectName = "IONet"
class ServerConfig:
    port = 9000
    name = "主服务器"
    area = "广州"
class DirConfig:
    logDir = os.getcwd() + "\\log\\"
    sqlDir = os.getcwd() + "\\sql\\"
class SQLConfig:
    DB_NAME = DirConfig.sqlDir + 'users.db'