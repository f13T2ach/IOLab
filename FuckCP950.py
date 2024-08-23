import os
import codecs

def convert_file_encoding(file_path, src_encoding, dest_encoding='utf-8'):
    """Converts the encoding of a file from src_encoding to dest_encoding."""
    try:
        # Read the file with the source encoding
        with codecs.open(file_path, 'r', encoding=src_encoding) as file:
            content = file.read()
        
        # Write the file with the destination encoding
        with codecs.open(file_path, 'w', encoding=dest_encoding) as file:
            file.write(content)
        
        print(f"Converted {file_path} from {src_encoding} to {dest_encoding}")
    except UnicodeDecodeError:
        print(f"Cannot decode {file_path} with {src_encoding}")
    except UnicodeEncodeError:
        print(f"Cannot encode {file_path} with {dest_encoding}")

def scan_and_convert(directory):
    """Scans the directory and converts files with cp950 encoding to utf-8."""
    for root, _, files in os.walk(directory):
        for file in files:
            file_path = os.path.join(root, file)
            try:
                with open(file_path, 'r', encoding='cp950') as _:
                    pass  # If it opens successfully, the file is encoded with cp950
            except (UnicodeDecodeError, FileNotFoundError):
                continue  # Ignore files that can't be decoded as cp950 or not found
            
            # Convert the file encoding to UTF-8
            convert_file_encoding(file_path, 'cp950')

if __name__ == "__main__":
    directory = input("Enter the directory path to scan and convert: ")
    scan_and_convert(directory)
