import os

def count_code_lines(file_path, file_ext):
    """Counts the number of non-empty, non-comment lines in a given file."""
    count = 0
    with open(file_path, 'r') as file:
        lines = file.readlines()

    if file_ext in ['.cpp', '.h']:
        in_block_comment = False
        for line in lines:
            line = line.strip()
            if not line:
                continue
            if '/*' in line:
                in_block_comment = True
            if '*/' in line:
                in_block_comment = False
                continue
            if in_block_comment:
                continue
            if not line.startswith('//'):
                count += 1
    elif file_ext == '.cs':
        in_block_comment = False
        for line in lines:
            line = line.strip()
            if not line:
                continue
            if '/*' in line:
                in_block_comment = True
            if '*/' in line:
                in_block_comment = False
                continue
            if in_block_comment:
                continue
            if not line.startswith('//'):
                count += 1
    elif file_ext == '.py':
        for line in lines:
            line = line.strip()
            if not line:
                continue
            if line.startswith('#'):
                continue
            count += 1
    elif file_ext == '.xaml':
            for line in lines:
                line = line.strip()
                if not line:
                    continue
                if line.startswith('<!'):
                    continue
                count += 1
    return count

def scan_directory(directory):
    """Scans the directory and counts the effective lines of code, skipping certain folders."""
    code_exts = ['.cpp', '.h', '.cs', '.py','.xaml']
    total_lines = {ext: 0 for ext in code_exts}

    for root, dirs, files in os.walk(directory):
        # Modify the dirs in-place to skip certain directories
        dirs[:] = [d for d in dirs if not (d.startswith('.') or d in ['bin', 'obj'])]

        for file in files:
            file_ext = os.path.splitext(file)[1]
            if file_ext in code_exts:
                file_path = os.path.join(root, file)
                num_lines = count_code_lines(file_path, file_ext)
                total_lines[file_ext] += num_lines

    return total_lines

if __name__ == "__main__":
    directory = input("Enter the directory path to scan: ")
    lines_count = scan_directory(directory)
    print("Effective lines of code:")
    for ext, count in lines_count.items():
        print(f"{ext} files: {count} lines")
