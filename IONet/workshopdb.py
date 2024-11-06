import sqlite3
import os
import config
from datetime import datetime

class WorkshopDatabase:
    def __init__(self):
        self.conn = None
        self.cursor = None

    def start(self):
        import app
        """启动数据库连接并创建表（如果不存在）"""
        if not os.path.exists(config.DirConfig.sqlDir):
            os.makedirs(config.DirConfig.sqlDir)
            app.logger.info("创建 文案SQL 目录: {}".format(config.DirConfig.uploadDir))
        
        # 用户文件目录
        if not os.path.exists(config.DirConfig.uploadDir):
            os.makedirs(config.DirConfig.uploadDir)
            app.logger.info("创建 用户文件 目录: {}".format(config.DirConfig.uploadDir))
        
        # 连接到 SQLite 数据库
        self.conn = sqlite3.connect(config.SQLConfig.WORKSHOPDB_NAME)
        self.cursor = self.conn.cursor()

        # 创建 workshop 表，使用 author_id 代替 author
        self.cursor.execute('''
            CREATE TABLE IF NOT EXISTS workshop (
                id INTEGER PRIMARY KEY,
                author_id INTEGER NOT NULL,
                title TEXT NOT NULL,
                content TEXT NOT NULL,
                json_file TEXT,
                created_at TIMESTAMP,
                tags TEXT,
                upload_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (author_id) REFERENCES users(id)  -- 关联用户表
            )
        ''')
        self.conn.commit()

    def add_workshop_item(self, author_id, title, content, created_at, tags, json_file=None, logger=None):
        """添加新文案，使用 author_id"""
        try:
            if not self.conn:
                if logger:
                    logger.info("请先启动数据库")
                return (-1, "数据库未启动")
            
            # 将 tags 转换为字符串格式以便存储
            tags_str = ",".join(tags) if isinstance(tags, list) else tags

            self.cursor.execute('''
                INSERT INTO workshop (author_id, title, content, json_file, created_at, tags) 
                VALUES (?, ?, ?, ?, ?, ?)
            ''', (author_id, title, content, json_file, created_at, tags_str))
            self.conn.commit()
            
            if logger:
                logger.info(f"用户 {author_id} 的文案上传成功")
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
                SELECT id, author_id, title, content, json_file, created_at, tags, upload_time 
                FROM workshop 
                ORDER BY upload_time DESC 
                LIMIT ? OFFSET ?
            ''', (per_page, offset))
            items = self.cursor.fetchall()
            
            result = [
                {
                    "id": item[0],
                    "author_id": item[1],
                    "title": item[2],
                    "content": item[3],
                    "json_file": item[4],
                    "created_at": item[5],
                    "tags": item[6].split(",") if item[6] else [],
                    "upload_time": item[7]
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
                SELECT id, author_id, title, content, json_file, created_at, tags, upload_time 
                FROM workshop 
                WHERE id = ?
            ''', (item_id,))
            item = self.cursor.fetchone()

            if item:
                return {
                    "id": item[0],
                    "author_id": item[1],
                    "title": item[2],
                    "content": item[3],
                    "json_file": item[4],
                    "created_at": item[5],
                    "tags": item[6].split(",") if item[6] else [],
                    "upload_time": item[7]
                }
            else:
                if logger:
                    logger.info(f"未找到ID为 {item_id} 的文案")
                return None

        except sqlite3.Error as e:
            if logger:
                logger.error(f"根据ID获取文案失败: {e}")
            return None

    def close(self):
        """关闭数据库连接"""
        if self.conn:
            self.conn.close()
