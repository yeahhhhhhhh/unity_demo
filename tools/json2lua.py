import os
import json
import re
from pathlib import Path
from typing import Any, Dict

def escape_lua_string(s: str) -> str:
    """
    转义 Lua 字符串中的特殊字符
    """
    if not isinstance(s, str):
        return str(s)
    
    s = s.replace('\\', '\\\\')
    s = s.replace('"', '\\"')
    s = s.replace('\n', '\\n')
    s = s.replace('\r', '\\r')
    s = s.replace('\t', '\\t')
    
    return s

def is_lua_keyword(s: str) -> bool:
    """
    检查字符串是否为 Lua 关键字
    """
    lua_keywords = {
        'and', 'break', 'do', 'else', 'elseif', 'end', 'false', 'for',
        'function', 'if', 'in', 'local', 'nil', 'not', 'or', 'repeat',
        'return', 'then', 'true', 'until', 'while'
    }
    return s in lua_keywords

def format_lua_table_value(value: Any, indent_level: int = 0) -> str:
    """
    将 Python 值格式化为 Lua 表格式
    """
    indent = "  " * indent_level
    
    if isinstance(value, dict):
        if not value:
            return "{}"
        
        lines = ["{"]
        for k, v in value.items():
            if not isinstance(k, str) or not k.isidentifier() or re.match(r'^\d', k) or is_lua_keyword(k):
                key_str = f'["{escape_lua_string(str(k))}"]'
            else:
                key_str = k
            value_str = format_lua_table_value(v, indent_level + 1)
            lines.append(f"{indent}  {key_str} = {value_str},")
        lines.append(f"{indent}}}")
        return "\n".join(lines)
    
    elif isinstance(value, (list, tuple)):
        if not value:
            return "{}"
        
        lines = ["{"]
        for item in value:
            value_str = format_lua_table_value(item, indent_level + 1)
            lines.append(f"{indent}  {value_str},")
        lines.append(f"{indent}}}")
        return "\n".join(lines)
    
    elif isinstance(value, str):
        return f'"{escape_lua_string(value)}"'
    
    elif isinstance(value, bool):
        return "true" if value else "false"
    
    elif isinstance(value, (int, float)):
        return str(value)
    
    elif value is None:
        return "nil"
    
    else:
        return f'"{escape_lua_string(str(value))}"'

def convert_json_to_lua(json_file: str, output_file: str) -> None:
    """
    将单个 JSON 文件转换为 Lua 数据文件
    """
    try:
        with open(json_file, 'r', encoding='utf-8') as f:
            data = json.load(f)
        
        print(f"正在转换: {json_file} -> {output_file}")
        
        # 创建输出目录
        output_dir = os.path.dirname(output_file)
        if output_dir and not os.path.exists(output_dir):
            os.makedirs(output_dir)
        
        # 生成 Lua 文件内容
        lua_content = []
        lua_content.append("-- 此文件由 JSON 自动转换生成，请勿手动修改")
        lua_content.append("local data = " + format_lua_table_value(data))
        lua_content.append("")
        lua_content.append("return data")
        
        # 写入 Lua 文件
        with open(output_file, 'w', encoding='utf-8') as f:
            f.write("\n".join(lua_content))
        
        print(f"✓ 成功生成: {output_file}")
        
    except Exception as e:
        print(f"✗ 转换失败 {json_file}: {e}")

def batch_convert_json_to_lua(input_dir: str, output_dir: str):
    """
    批量转换输入目录中的所有 JSON 文件到输出目录中的 Lua 文件
    保持原始目录结构
    """
    input_path = Path(input_dir)
    output_path = Path(output_dir)
    
    # 确保输出目录存在
    output_path.mkdir(parents=True, exist_ok=True)
    
    # 递归查找所有 JSON 文件
    json_files = list(input_path.rglob("*.json"))
    
    if not json_files:
        print(f"在目录 {input_dir} 中未找到 JSON 文件")
        return 0
    
    print(f"找到 {len(json_files)} 个 JSON 文件，开始转换...")
    print("-" * 50)
    
    success_count = 0
    for json_file in json_files:
        try:
            # 计算相对路径，以便在输出目录中保持相同结构
            relative_path = json_file.relative_to(input_path)
            # 更改扩展名为 .lua
            lua_file = output_path / relative_path.with_suffix('.lua')
            
            # 调用单个转换函数
            convert_json_to_lua(str(json_file), str(lua_file))
            success_count += 1
            
        except Exception as e:
            print(f"✗ 处理文件 {json_file} 时出错: {e}")
            continue
    
    print("-" * 50)
    print(f"转换完成! 成功: {success_count}/{len(json_files)} 个文件")
    return success_count

def main():
    """主函数"""
    # 设置输入和输出目录
    current_dir = os.path.dirname(os.path.abspath(__file__))
    input_dir = os.path.join(current_dir, "json")    # JSON 文件所在目录
    output_dir = os.path.join(current_dir, "lua")     # Lua 文件输出目录
    
    print("=" * 50)
    print("JSON 到 Lua 批量转换工具")
    print("=" * 50)
    print(f"输入目录: {input_dir}")
    print(f"输出目录: {output_dir}")
    print()
    
    # 检查输入目录是否存在
    if not os.path.exists(input_dir):
        print(f"错误: 输入目录不存在: {input_dir}")
        print("请创建 'json' 文件夹并放入 JSON 文件")
        return
    
    # 执行批量转换
    success_count = batch_convert_json_to_lua(input_dir, output_dir)
    
    if success_count > 0:
        print(f"\n✅ 所有文件已转换到: {output_dir}")
    else:
        print("\n❌ 转换失败，请检查错误信息")

if __name__ == "__main__":
    main()