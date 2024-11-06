import os
projectName = "IONet"
class ServerConfig:
    port = 9000
    name = "主服务器"
    area = "广州"
class DirConfig:
    logDir = os.getcwd() + "\\log\\"
    sqlDir = os.getcwd() + "\\sql\\"
    uploadDir_relative= "\\userfile\\"
    uploadDir = os.getcwd() + uploadDir_relative
class SQLConfig:
    DB_NAME = DirConfig.sqlDir + 'users.db'
    WORKSHOPDB_NAME = DirConfig.sqlDir + 'workshop.db'
    dailyReward = 1