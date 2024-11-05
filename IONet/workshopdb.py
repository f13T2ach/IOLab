import sqlite3
import os
import config
from datetime import datetime

class WorkshopDatabase:
    def __init__(self):
        self.conn = None
        self.cursor = None

    def start(self):
        """启动数据库连接，创建表（如果不存在）"""
        if not os.path.exists(config.DirConfig.sqlDir):
            os.makedirs(config.DirConfig.sqlDir)
            return "创建 SQL 目录: {}".format(config.DirConfig.sqlDir)
        
        # 连接到 SQLite 数据库
        self.conn = sqlite3.connect(config.SQLConfig.DB_NAME)
        self.cursor = self.conn.cursor()

        # 创建 workshop 表
        self.cursor.execute('''
            CREATE TABLE IF NOT EXISTS workshop (
                id INTEGER PRIMARY KEY,
                user_id INTEGER NOT NULL,
                content TEXT NOT NULL,
                json_file TEXT,
                upload_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            )
        ''')
        self.conn.commit()

    def add_workshop_item(self, user_id, content, json_file=None, logger=None):
        """添加新文案"""
        try:
            if not self.conn:
                if logger:
                    logger.info("请先启动数据库")
                return
            
            self.cursor.execute('''
                INSERT INTO workshop (user_id, content, json_file) 
                VALUES (?, ?, ?)
            ''', (user_id, content, json_file))
            self.conn.commit()
            
            if logger:
                logger.info(f"用户 {user_id} 的文案上传成功")
            return (0, "文案上传成功")
        
        except sqlite3.Error as e:
            if logger:
                logger.error(f"添加文案失败: {e}")
            return (-1, "文案上传失败")

    def get_workshop_items(self, page=1, per_page=10, logger=None):
        """分页浏览文案"""
        try:
            offset = (page - 1) * per_page
            self.cursor.execute('''
                SELECT id, user_id, content, json_file, upload_time 
                FROM workshop 
                ORDER BY upload_time DESC 
                LIMIT ? OFFSET ?
            ''', (per_page, offset))
            items = self.cursor.fetchall()
            
            result = [
                {
                    "id": item[0],
                    "user_id": item[1],
                    "content": item[2],
                    "json_file": item[3],
                    "upload_time": item[4]
                }
                for item in items
            ]
            
            if logger:
                logger.info(f"文案分页浏览成功，当前页: {page}")
            return result

        except sqlite3.Error as e:
            if logger:
                logger.error(f"分页浏览文案失败: {e}")
            return []

    def get_workshop_item_by_id(self, item_id, logger=None):
        """根据文案ID获取文案内容和 JSON 文件路径"""
        try:
            self.cursor.execute('''
                SELECT id, user_id, content, json_file, upload_time 
                FROM workshop 
                WHERE id = ?
            ''', (item_id,))
            item = self.cursor.fetchone()

            if item:
                return {
                    "id": item[0],
                    "user_id": item[1],
                    "content": item[2],
                    "json_file": item[3],
                    "upload_time": item[4]
                }
            else:
                if logger:
                    logger.info(f"未找到ID为 {item_id} 的文案")
                return None

        except sqlite3.Error as e:
            if logger:
                logger.error(f"根据ID获取文案失败: {e}")
            return None

    # 关闭数据库连接
    def close(self):
        if self.conn:
            self.conn.close()
