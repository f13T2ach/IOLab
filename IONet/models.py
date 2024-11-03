import re

def is_sql_injection_risk(input_string):
    """
    检查给定的字符串是否有潜在的SQL注入风险。
    
    参数:
        input_string (str): 需要检查的字符串。
    
    返回:
        bool: 如果存在潜在的SQL注入风险，则返回True；否则返回False。
    """
    # 定义可能引起SQL注入的关键字和模式
    patterns = [
        r'select.*from',  # SELECT ... FROM ...
        r'update.*set',   # UPDATE ... SET ...
        r'delete.*from',  # DELETE ... FROM ...
        r'insert.*into',  # INSERT ... INTO ...
        r'union.*select',  # UNION ... SELECT ...
        r'--',            # 单行注释符号
        r'/\*',           # 多行注释开始符号
        r'\*/',           # 多行注释结束符号
        r';',             # 分号，常用于终止SQL语句
        r'\bOR\b',        # OR关键字
        r'\bAND\b',       # AND关键字
        r'\bWHERE\b',     # WHERE关键字
        r'\bDROP\b',      # DROP关键字
        r'\bEXEC\b',      # EXEC关键字
        r'\bCREATE\b',    # CREATE关键字
        r'\bALTER\b',     # ALTER关键字
        r'\bGRANT\b',     # GRANT关键字
        r'\bREVOKE\b',    # REVOKE关键字
    ]
    
    for pattern in patterns:
        if re.search(pattern, input_string, re.IGNORECASE):
            return True
    
    return False