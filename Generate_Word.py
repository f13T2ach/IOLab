import os
from docx import Document
from docx.oxml import OxmlElement
from docx.oxml.ns import qn
from docx.shared import Pt, RGBColor

def add_hyperlink(paragraph, url, text):
    # 添加超链接
    part = paragraph.part
    r_id = part.relate_to(url, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/hyperlink", is_external=True)
    hyperlink = OxmlElement("w:hyperlink")
    hyperlink.set(qn("r:id"), r_id)
    run = OxmlElement("w:r")
    rPr = OxmlElement("w:rPr")
    run.append(rPr)
    text_element = OxmlElement("w:t")
    text_element.text = text
    run.append(text_element)
    hyperlink.append(run)
    paragraph._p.append(hyperlink)

def add_bookmark(paragraph, bookmark_name):
    # 使用 Word 的书签方式创建书签
    bookmark_start = OxmlElement("w:bookmarkStart")
    bookmark_start.set(qn("w:id"), "0")
    bookmark_start.set(qn("w:name"), bookmark_name)

    bookmark_end = OxmlElement("w:bookmarkEnd")
    bookmark_end.set(qn("w:id"), "0")

    paragraph._p.append(bookmark_start)
    paragraph._p.append(bookmark_end)

def format_code_line(paragraph, line):
    # 设置代码的字体为五号字体，并高亮关键字
    run = paragraph.add_run(line)
    run.font.size = Pt(10.5)  # 五号字体
    keywords = ["def", "class", "import", "from", "for", "while", "if", "else", "elif", "return", "try", "except", "finally"]
    for keyword in keywords:
        if keyword in line:
            keyword_index = line.find(keyword)
            if keyword_index != -1:
                start = keyword_index
                end = start + len(keyword)
                run.font.color.rgb = RGBColor(255, 0, 0)  # 红色标注关键字

def traverse_and_save_to_word_with_toc(directory, output_word):
    # 创建一个 Word 文档
    document = Document()

    # 添加目录页
    document.add_heading("目录", level=1)
    toc_paragraph = document.add_paragraph()

    # 遍历指定目录下的所有文件
    for root, dirs, files in os.walk(directory):
        for file in files:
            
            file_path = os.path.join(root, file)
            print("正在处理:",file_path)
            # 仅处理代码文件（包括 xaml 文件）
            if file.endswith(('.py', '.java', '.cpp', '.js', '.cs', '.html', '.css', '.c', '.h', '.sh', '.rb', '.go', '.xaml')):
                # 添加到目录
                paragraph = toc_paragraph
                file_title = os.path.relpath(file_path, directory)
                add_hyperlink(paragraph, f"#{file_title}", file_title)
                paragraph.add_run("\n")

                # 添加文件名作为标题
                document.add_page_break()
                heading = document.add_heading(file_title, level=2)
                add_bookmark(heading, file_title)

                # 读取代码内容
                try:
                    with open(file_path, 'r', encoding='utf-8') as f:
                        lines = f.readlines()

                    # 分页逻辑，每 50 行作为一页
                    for i in range(0, len(lines), 50):
                        page_lines = lines[i:i+50]
                        for line in page_lines:
                            paragraph = document.add_paragraph(style="NoSpacing")  # 避免每行之间的空行
                            format_code_line(paragraph, line)
                        if i + 50 < len(lines):
                            document.add_page_break()

                except Exception as e:
                    document.add_paragraph(f"Error reading file {file}: {str(e)}")

    # 保存 Word 文档
    document.save(output_word)

if __name__ == "__main__":
    # 指定要遍历的目录
    input_directory = input("请输入要遍历的目录路径: ").strip()

    # 指定输出 Word 文件
    output_file = input("请输入输出的 Word 文件名(包含 .docx): ").strip()

    if not output_file.endswith('.docx'):
        print("输出文件名必须以 .docx 结尾！")
    else:
        traverse_and_save_to_word_with_toc(input_directory, output_file)
        print(f"文件已保存到 {output_file}")
