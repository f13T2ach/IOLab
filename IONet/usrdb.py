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
        if not os.path.exists(config.DirConfig.sqlDir):
            os.makedirs(config.DirConfig.sqlDir)
            app.logger.info(f"创建 SQL 目录: {config.DirConfig.sqlDir}")
         # 连接到 SQLite 数据库（如果数据库不存在，它将被自动创建）
        self.conn = sqlite3.connect(config.SQLConfig.DB_NAME)
        self.cursor = self.conn.cursor()

        # 创建用户表（如果不存在）
        self.cursor.execute('''CREATE TABLE IF NOT EXISTS users (
                                id INTEGER PRIMARY KEY,
                                username TEXT NOT NULL,
                                email TEXT NOT NULL UNIQUE,
                                hashedpass TEXT NOT NULL,
                                exp INTERGER NOT NULL,
                                groupid INTERGER NOT NULL,
                                token TEXT UNIQUE NOT NULL)''')
        self.conn.commit()

        #app.logger.info("用户数据库启动成功")

    # 生成用户 token（不带连字符）
    def generate_token(self):
        return str(uuid.uuid4()).replace("-", "")  # 生成唯一的 UUID 并去掉连字符

    # 添加用户
    def add_user(self, username, email, password):
        try:
            if not self.conn:
                app.logger.info("请先启动用户数据库")
                return
            
            # 获取下一个用户 ID
            self.cursor.execute("SELECT MAX(id) FROM users")
            max_id = self.cursor.fetchone()[0]
            next_id = 10000000 if max_id is None else max_id + 1  # 从 10000000 开始

            hashedpass = self.hash_password(password)  # 对密码进行 MD5 加密
            token = self.generate_token()  # 生成用户 token
            
            self.cursor.execute("INSERT INTO users (id, username, email, hashedpass,exp,groupid, token) VALUES (?, ?, ?, ?,?,?, ?)", 
                                (next_id, username, email, hashedpass, 0,0, token))
            self.conn.commit()
            app.logger.info("%s 添加成功: 用户 ID: %d, Token: %s",username, next_id, token)
            return (0,"注册成功")
        except sqlite3.IntegrityError:
            app.logger.info("%s 已存在或邮箱已被使用",username)
            return (-1,"邮箱已绑定在其他账户上")
        except sqlite3.Error as e:
            app.logger.info("添加 %s 用户失败: {e}",username)
            return (-1,"服务器异常")
    
    def get_user_token(self, nameOrEmail, password):
        """根据用户名或电子邮件和密码检索用户的 token"""
        try:
            if not nameOrEmail or not password:
                app.logger.info("用户名或电子邮件与密码均未提供")
                return -1

            # 先获取 hashed 密码
            hashedpass = self.hash_password(password)

            self.cursor.execute("SELECT token FROM users WHERE (username = ? OR email = ?) AND hashedpass = ?", 
                                (nameOrEmail, nameOrEmail, hashedpass))
            result = self.cursor.fetchone()

            if result:
                return result[0]  # 返回 token
            else:
                return -1  # 用户未找到或密码不正确
        except sqlite3.Error as e:
            app.logger.info(f"检索用户 {nameOrEmail} token 失败: {e}")
            return -1
        
    def delete_user_by_token(self, token):
        try:
            if not self.conn:
                app.logger.info("请先启动数据库连接")
                return
            
            # 根据 token 删除用户
            self.cursor.execute("DELETE FROM users WHERE token = ?", (token,))
            self.conn.commit()
            
            if self.cursor.rowcount > 0:
                app.logger.info(f"用户的 token {token} 注销成功")
                return (0,"注销成功")
            else:
                app.logger.info(f"未找到与 token {token} 关联的用户，注销失败")
                return (-1,"用户不存在")
        except sqlite3.Error as e:
            app.logger.error(f"注销用户失败: {e}")
            return (-1,"服务器异常")


    # 生成 MD5 哈希密码
    def hash_password(self, password):
        return hashlib.md5(password.encode()).hexdigest()
    
    # 关闭数据库连接
    def close(self):
        if self.conn:
            self.conn.close()