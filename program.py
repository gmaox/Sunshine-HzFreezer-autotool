import json
import os
import tkinter as tk
from tkinter import messagebox
import keyboard
from pystray import Icon, MenuItem, Menu
from PIL import Image
import sys
import psutil
import time
import threading

# 保存数据到 JSON 文件
def save_to_json(data, filename="1.json"):
    try:
        with open(filename, "w", encoding="utf-8") as file:
            json.dump(data, file, ensure_ascii=False, indent=4)
    except Exception as e:
        print(f"Error saving to JSON: {e}")
        messagebox.showerror("Error", f"Failed to save data: {e}")

# 从 JSON 文件读取数据
def read_from_json(filename="1.json"):
    try:
        with open(filename, "r", encoding="utf-8") as file:
            return json.load(file)
    except FileNotFoundError:
        return {"text1": "Default Text 1", "text2": "Default Text 2"}
    except json.JSONDecodeError:
        print("Error decoding JSON.")
        return {"text1": "Default Text 1", "text2": "Default Text 2"}

# 自定义窗口的回调函数
def on_custom_input(icon, item):
    data = read_from_json()

    root = tk.Tk()
    root.title("同步雪藏快捷键")

    entry_var1 = tk.StringVar(value=data.get("text1", "alt+win+z"))
    entry_var2 = tk.StringVar(value=data.get("text2", "alt+win+c"))

    tk.Label(root, text="冻结光标程序快捷键:").grid(row=0, column=0)
    entry1 = tk.Entry(root, textvariable=entry_var1)
    entry1.grid(row=0, column=1)

    tk.Label(root, text="解冻快捷键:").grid(row=1, column=0)
    entry2 = tk.Entry(root, textvariable=entry_var2)
    entry2.grid(row=1, column=1)

    def save_action():
        text1 = entry1.get()
        text2 = entry2.get()
        new_data = {"text1": text1, "text2": text2}
        save_to_json(new_data)
        messagebox.showinfo("Success", "Data saved successfully!")
        python = sys.executable
        os.execl(python, python, *sys.argv)

    save_button = tk.Button(root, text="Save", command=save_action)
    save_button.grid(row=2, column=0, columnspan=2)

    root.mainloop()

# 从 favicon.ico 加载图标
def create_icon_image():
    return Image.open(os.path.join(os.path.dirname(__file__), "favicon.ico"))

# 退出函数
def on_quit(icon, item):
    os._exit(0)

# 初始化托盘图标
icon = Icon("test", create_icon_image(), menu=Menu(
    MenuItem("同步雪藏快捷键", on_custom_input),
    MenuItem("Quit", on_quit)
))

def start_icon():
    icon.run()

# 启动托盘图标线程
icon_thread = threading.Thread(target=start_icon)
icon_thread.daemon = True
icon_thread.start()

# 设置要监听的端口和检查时间间隔
PORT = 48000
INTERVAL = 3
SUN = False
data = read_from_json()
KEY1 = data.get("text1", "alt+win+z")
KEY2 = data.get("text2", "alt+win+c")
# 检查端口占用情况
def check_port_usage():
    global SUN
    port_in_use = False
    for conn in psutil.net_connections(kind='inet'):
        if conn.laddr.port == PORT:
            port_in_use = True
            print(f"Port {PORT} is in use by PID {conn.pid}, program: {psutil.Process(conn.pid).name()}")
            if psutil.Process(conn.pid).name() == "sunshine.exe":
                if SUN == False:
                    SUN = True
                    print("sunshine.exe is running--------------"+KEY2)
                    time.sleep(3)
                    keyboard.press_and_release(KEY2)
            break
    
    if not port_in_use:
        print(f"Port {PORT} is free.")
        if SUN == True:
            SUN = False
            print("sunshine.exe is close----------------"+KEY1)
            time.sleep(3)
            keyboard.press_and_release(KEY1)


def generate_report():
    while True:
        print(f"\nChecking port {PORT} usage at {time.strftime('%Y-%m-%d %H:%M:%S')}")
        check_port_usage()
        time.sleep(INTERVAL)

# 启动定时生成报告的线程
report_thread = threading.Thread(target=generate_report)
report_thread.daemon = True
report_thread.start()

# 主线程等待
try:
    while True:
        time.sleep(1)
except KeyboardInterrupt:
    print("Program interrupted and stopping...")
