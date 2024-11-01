import sqlite3
import os
import hashlib
import uuid
import config
from datetime import datetime

class UserDatabase:
    def __init__(self):
        self.conn = None
        self.cursor = None

    def start(self):
        if not os.path.exists(config.DirConfig.sqlDir):
            os.makedirs(config.DirConfig.sqlDir)
            return "创建 SQL 目录: {}".format(config.DirConfig.sqlDir)
        # 连接到 SQLite 数据库（如果数据库不存在，它将被自动创建）
        self.conn = sqlite3.connect(config.SQLConfig.DB_NAME)
        self.cursor = self.conn.cursor()

        # 创建用户表（如果不存在）
        self.cursor.execute('''CREATE TABLE IF NOT EXISTS users (
                                id INTEGER PRIMARY KEY,
                                username TEXT NOT NULL,
                                email TEXT NOT NULL UNIQUE,
                                hashedpass TEXT NOT NULL,
                                exp INTEGER NOT NULL,
                                groupid INTEGER NOT NULL,
                                token TEXT UNIQUE NOT NULL,
                                last_check_in DATE)''')
        self.conn.commit()

    # 生成用户 token（不带连字符）
    def generate_token(self):
        return str(uuid.uuid4()).replace("-", "")  # 生成唯一的 UUID 并去掉连字符

    # 添加用户
    def add_user(self, username, email, password, group=0, logger=None):
        try:
            if not self.conn:
                if logger:
                    logger.info("请先启动用户数据库")
                return
            
            # 获取下一个用户 ID
            self.cursor.execute("SELECT MAX(id) FROM users")
            max_id = self.cursor.fetchone()[0]
            next_id = 10000000 if max_id is None else max_id + 1  # 从 10000000 开始

            basicExp = 0
            hashedpass = self.hash_password(password)  # 对密码进行 MD5 加密
            token = self.generate_token()  # 生成用户 token
            
            self.cursor.execute("INSERT INTO users (id, username, email, hashedpass, exp, groupid, token, last_check_in) VALUES (?, ?, ?, ?, ?, ?, ?, ?)", 
                                (next_id, username, email, hashedpass, basicExp, group, token, None))
            self.conn.commit()
            if logger:
                logger.info("%s 添加成功: 用户 ID: %d, Token: %s", username, next_id, token)
            return (0, "注册成功")
        except sqlite3.IntegrityError:
            if logger:
                logger.info("%s 已存在或邮箱已被使用", username)
            return (-1, "邮箱已绑定在其他账户上")
        except sqlite3.Error as e:
            if logger:
                logger.info("添加 %s 用户失败: {e}", username)
            return (-1, "服务器异常")
    
    def get_user_token(self, nameOrEmail, password, logger=None):
        """根据用户名或电子邮件和密码检索用户的 token"""
        try:
            if not nameOrEmail or not password:
                if logger:
                    logger.info("用户名或电子邮件与密码均未提供")
                return -1

            # 先获取 hashed 密码
            hashedpass = self.hash_password(password)

            self.cursor.execute("SELECT token, email, exp, groupid, username FROM users WHERE (username = ? OR email = ?) AND hashedpass = ?", 
                                (nameOrEmail, nameOrEmail, hashedpass))
            result = self.cursor.fetchone()

            if result:
                # 返回一个包含 token、email、exp 和 groupid 的字典
                return {
                    "token": result[0],   # token
                    "email": result[1],   # email
                    "exp": result[2],     # exp
                    "groupid": result[3],  # groupid
                    "username": result[4]  # username
                }
            
            else:
                return -1  # 用户未找到或密码不正确
        except sqlite3.Error as e:
            if logger:
                logger.info(f"检索用户 {nameOrEmail} token 失败: {e}")
            return -1
        
    def delete_user_by_token(self, token, logger=None):
        try:
            if not self.conn:
                if logger:
                    logger.info("请先启动数据库连接")
                return
            
            # 根据 token 删除用户
            self.cursor.execute("DELETE FROM users WHERE token = ?", (token,))
            self.conn.commit()
            
            if self.cursor.rowcount > 0:
                if logger:
                    logger.info(f"用户的 token {token} 注销成功")
                return (0, "注销成功")
            else:
                if logger:
                    logger.info(f"未找到与 token {token} 关联的用户，注销失败")
                return (-1, "用户不存在")
        except sqlite3.Error as e:
            if logger:
                logger.error(f"注销用户失败: {e}")
            return (-1, "服务器异常")

    def check_in(self, token, logger=None):
        """根据用户 token 增加经验值"""
        try:
            if not self.conn:
                if logger:
                    logger.info("请先启动用户数据库")
                return

            # 获取当前日期
            today = datetime.now().date()

            # 检查最后签到日期
            self.cursor.execute("SELECT last_check_in, exp FROM users WHERE token = ?", (token,))
            result = self.cursor.fetchone()

            if result:
                last_check_in, exp = result
                
                # 检查用户的最后签到日期是否存在，并与今天的日期比较
                if last_check_in is not None:
                    last_check_in = datetime.strptime(last_check_in, "%Y-%m-%d").date()
                    if last_check_in == today:
                        return (-1, "今天已经签到过，请明天再来签到")
                # 增加经验值
                self.cursor.execute("UPDATE users SET exp = exp + 5, last_check_in = ? WHERE token = ?", (today, token))
                self.conn.commit()

                if self.cursor.rowcount > 0:
                    if logger:
                        logger.info(f"用户的 token {token} 签到成功，经验值增加 5")
                    return (0, "签到成功，经验值增加 5")
            else:
                if logger:
                    logger.info(f"未找到与 token {token} 关联的用户，签到失败")
                return (-1, "用户不存在或未签到")
        except sqlite3.Error as e:
            if logger:
                logger.error(f"签到失败: {e}")
            return (-1, "服务器异常")

    def get_user_info_by_token(self, token, logger=None):
        """根据用户 token 检索用户信息"""
        try:
            if not self.conn:
                if logger:
                    logger.info("请先启动用户数据库")
                return None

            self.cursor.execute("SELECT username, email, exp, groupid FROM users WHERE token = ?", (token,))
            result = self.cursor.fetchone()

            if result:
                return {
                    "username": result[0],
                    "email": result[1],
                    "exp": result[2],
                    "groupid": result[3]
                }
            else:
                return None  # 用户未找到
        except sqlite3.Error as e:
            if logger:
                logger.error(f"根据 token 获取用户信息失败: {e}")
            return None

    # 生成 MD5 哈希密码
    def hash_password(self, password):
        return hashlib.md5(password.encode()).hexdigest()
    
    # 关闭数据库连接
    def close(self):
        if self.conn:
            self.conn.close()
