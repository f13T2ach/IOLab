import sqlite3
import os
import hashlib
import app
import uuid
import config

class UserDatabase:
    def __init__(self):
        self.conn = None
        self.cursor = None

    def start(self):
        if not os.path.exists(config.SQLConfig.DB_NAME):
            app.logger.info("数据库不存在 正在创建...")
        self.conn = sqlite3.connect(config.SQLConfig.DB_NAME)
        self.cursor = self.conn.cursor()
        self.cursor.execute('''CREATE TABLE IF NOT EXISTS users (
                                id INTEGER PRIMARY KEY,
                                username TEXT NOT NULL,
                                email TEXT NOT NULL UNIQUE,
                                hashedpass TEXT NOT NULL,
                                token TEXT UNIQUE NOT NULL)''')
        self.conn.commit()
        app.logger.info("数据库连接已启动")

    # 生成用户 token（不带连字符）
    def generate_token(self):
        return str(uuid.uuid4()).replace("-", "")  # 生成唯一的 UUID 并去掉连字符

    # 添加用户
    def add_user(self, username, email, password):
        try:
            if not self.conn:
                app.logger.info("请先启动数据库连接")
                return
            
            # 获取下一个用户 ID
            self.cursor.execute("SELECT MAX(id) FROM users")
            max_id = self.cursor.fetchone()[0]
            next_id = 10000000 if max_id is None else max_id + 1  # 从 10000000 开始

            hashedpass = self.hash_password(password)  # 对密码进行 MD5 加密
            token = self.generate_token()  # 生成用户 token
            
            self.cursor.execute("INSERT INTO users (id, username, email, hashedpass, token) VALUES (?, ?, ?, ?, ?)", 
                                (next_id, username, email, hashedpass, token))
            self.conn.commit()
            app.logger.info("%s 添加成功: 用户 ID: %d, Token: %s",username, next_id, token)
        except sqlite3.IntegrityError:
            app.logger.info("%s 已存在或邮箱已被使用",username)
        except sqlite3.Error as e:
            app.logger.info("添加 %s 用户失败: {e}",username)

    # 生成 MD5 哈希密码
    def hash_password(self, password):
        return hashlib.md5(password.encode()).hexdigest()
    
    # 关闭数据库连接
    def close(self):
        if self.conn:
            self.conn.close()
            app.logger.info("数据库连接已关闭")