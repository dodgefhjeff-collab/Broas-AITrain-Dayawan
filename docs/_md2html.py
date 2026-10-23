# -*- coding: utf-8 -*-
import pathlib
import markdown

root = pathlib.Path(__file__).resolve().parent
md_path = root / "客户操作手册.md"
html_path = root / "客户操作手册.html"
text = md_path.read_text(encoding="utf-8")
body = markdown.markdown(
    text,
    extensions=["tables", "sane_lists", "nl2br"],
)
html = f"""<!DOCTYPE html>
<html lang="zh-CN">
<head>
<meta charset="utf-8">
<title>室内训练机操作手册</title>
<style>
body {{ font-family: "微软雅黑", "Microsoft YaHei", sans-serif; font-size: 12pt; line-height: 1.5; }}
h1 {{ font-size: 22pt; }}
h2 {{ font-size: 16pt; margin-top: 18pt; }}
h3 {{ font-size: 14pt; }}
table {{ border-collapse: collapse; width: 100%; margin: 8pt 0; }}
th, td {{ border: 1px solid #666; padding: 4pt 6pt; vertical-align: top; }}
th {{ background: #1a658f; color: white; }}
img {{ width: 16cm; height: auto; }}
blockquote {{ color: #333; }}
</style>
</head>
<body>
{body}
</body>
</html>
"""
html_path.write_text(html, encoding="utf-8")
print(html_path)
