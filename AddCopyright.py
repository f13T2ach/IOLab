import os

# 注释模板
header_template = """/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * {filename} is part of the IOLab project.
 *
 * IOLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * IOLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with IOLab. If not, see <https://www.gnu.org/licenses/>.
 */
"""

# 需要处理的文件扩展名
file_extensions = ('.py', '.cs', '.cpp', '.c', '.h')

def add_header_to_file(filepath):
    filename = os.path.basename(filepath)
    with open(filepath, 'r+', encoding='utf-8') as file:
        content = file.read()
        # 跳过已包含GPLv3许可信息的文件
        if "GNU General Public License" in content:
            return
        # 根据文件名生成注释
        header = header_template.format(filename=filename)
        # 添加注释到文件头部
        file.seek(0)
        file.write(header + "\n" + content)

def process_directory(directory):
    for root, dirs, files in os.walk(directory):
        for file in files:
            if file.endswith(file_extensions):
                file_path = os.path.join(root, file)
                add_header_to_file(file_path)

if __name__ == "__main__":
    directory_to_process = "."  # 目标目录，"."表示当前目录
    process_directory(directory_to_process)
