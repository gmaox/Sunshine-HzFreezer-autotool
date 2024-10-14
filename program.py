import json
import os
import subprocess
import tkinter as tk
from tkinter import messagebox
import webbrowser
import keyboard
from win10toast import ToastNotifier
from pystray import Icon, MenuItem, Menu
from PIL import Image
import sys
import psutil
import time
import threading
import win32event,win32api,time
import ctypes
#初始化通知
toaster = ToastNotifier()
#确保只有一个程序运行
if __name__ == '__main__':
    mutex = win32event.CreateMutex(None, False, 'Sunshine-HzFreezer-autotool')
    if win32api.GetLastError() > 0:
        toaster.show_toast("​不许调戏心海酱(￣Д ￣)", "​工具已在后台运行", icon_path='',duration=0.01)
        os._exit(0)
if ctypes.windll.shell32.IsUserAnAdmin()==0:
    toaster.show_toast("​串流监听程序启动(未使用管理员模式)", "部分游戏需用管理员身份运行工具\n不使用可能会无法冻结\n右键系统托盘图标进行配置", icon_path='',duration=0.01)
elif ctypes.windll.shell32.IsUserAnAdmin()==1:
    toaster.show_toast("​串流监听程序已启动", "右键系统托盘图标进行配置", icon_path='',duration=0.01)
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
        return {"text1": "ctrl+b", "text2": "ctrl+m", "text3": "48000", "text4": "3", "text5": "0", "text6": "0"}
    except json.JSONDecodeError:
        print("Error decoding JSON.")
        return {"text1": "ctrl+b", "text2": "ctrl+m", "text3": "48000", "text4": "3", "text5": "0", "text6": "0"}
# 自定义窗口的回调函数
def on_custom_input(icon, item):
    data = read_from_json()
    root = tk.Tk()
    root.title("若无法输入请尝试选中窗口")
    entry_var1 = tk.StringVar(value=data.get("text1", "ctrl+b"))
    entry_var2 = tk.StringVar(value=data.get("text2", "ctrl+m"))
    entry_var3 = tk.StringVar(value=data.get("text3", "48000"))
    entry_var4 = tk.StringVar(value=data.get("text4", "3"))
    entry_var5 = tk.StringVar(value=data.get("text5", "0"))
    entry_var6 = tk.StringVar(value=data.get("text6", "0"))
    tk.Label(root, text="冻结光标程序快捷键:").grid(row=0, column=0)
    entry1 = tk.Entry(root, textvariable=entry_var1)
    entry1.grid(row=0, column=1)
    tk.Label(root, text="解冻快捷键:").grid(row=1, column=0)
    entry2 = tk.Entry(root, textvariable=entry_var2)
    entry2.grid(row=1, column=1)
    tk.Label(root, text="↑确保按钮点击后有雪藏有反应↑\n").grid(row=3, column=0, columnspan=2)
    tk.Label(root, text="端口号(默认48000):").grid(row=4, column=0)
    entry3 = tk.Entry(root, textvariable=entry_var3)
    entry3.grid(row=4, column=1)
    tk.Label(root, text="监听间隔(默认3):").grid(row=5, column=0)
    entry4 = tk.Entry(root, textvariable=entry_var4)
    entry4.grid(row=5, column=1)
    tk.Label(root, text="以下两参数为虚拟显示器准备\n连接与解冻间隔:").grid(row=6, column=0)
    entry5 = tk.Entry(root, textvariable=entry_var5)
    entry5.grid(row=6, column=1)
    tk.Label(root, text="暂停串流与冻结间隔:").grid(row=7, column=0)
    entry6 = tk.Entry(root, textvariable=entry_var6)
    entry6.grid(row=7, column=1)
    def save_action():
        text1 = entry1.get()
        text2 = entry2.get()
        text3 = entry3.get()
        text4 = entry4.get()
        text5 = entry5.get()
        text6 = entry6.get()
        new_data = {"text1": text1, "text2": text2, "text3": text3, "text4": text4, "text5": text5, "text6": text6}
        save_to_json(new_data)
        messagebox.showinfo("Success", "Data saved successfully!")
        python = sys.executable
        os.execl(python, python, *sys.argv)
    save_button = tk.Button(root, text="--保存以上修改--", command=save_action)
    save_button.grid(row=8, column=0, columnspan=2)
    def startuprun():
        def check_task_exists(task_name):
            """检查任务是否存在"""
            command = ['schtasks', '/query', '/tn', task_name]
            try:
                # 如果任务存在，将返回0
                subprocess.run(command, check=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE)
                return True
            except subprocess.CalledProcessError:
                return False

        def delete_task(task_name):
            """删除指定的任务"""
            command = ['schtasks', '/delete', '/tn', task_name, '/f']
            subprocess.run(command, check=True)
            messagebox.showinfo('删除成功',f"任务 {task_name} 已删除。")

        def create_startup_task(task_name, app_path):
            """创建开机自启任务"""
            command = [
                'schtasks', '/create', '/tn', task_name, '/tr', f'"{app_path}"',
                '/sc', 'onlogon', '/rl', 'highest', '/f'
            ]

            try:
                # 调用 schtasks 命令
                subprocess.run(command, check=True)
                messagebox.showinfo('创建成功',f"开机自启任务 {task_name} 创建成功！")
            except subprocess.CalledProcessError as e:
                messagebox.showinfo('错误',f"任务创建失败，或许是没有管理员权限: {e}")

        with open("MysunAppAutoStart.bat", "w", encoding="utf-8") as file:
            file.write(f'@echo off\ncd /d "{os.path.dirname(psutil.Process(os.getpid()).exe())}"\nstart {os.path.basename(psutil.Process(os.getpid()).exe())}')
        task_name = "MysunAppAutoStart"  # 任务名称
        app_path = os.path.dirname(psutil.Process(os.getpid()).exe())+"\\MysunAppAutoStart.bat"  # 可执行文件路径
        

        # 检查任务是否存在
        if check_task_exists(task_name):
            delete_task(task_name)  # 删除现有任务
        else:
            create_startup_task(task_name, app_path)# 创建新任务

    save_button = tk.Button(root, text="开启或关闭开机自启", command=startuprun)
    save_button.grid(row=10, column=0, columnspan=2)
    tk.Label(root, text="不喜欢通知可以在win设置中关闭").grid(row=9, column=0, columnspan=2)
    tk.Label(root, text="\n输入后可测试：       \n").grid(row=2, column=0)
    test1_button = tk.Button(root, text="模拟冻结按键", command=lambda: keyboard.press_and_release(entry1.get()))
    test1_button.grid(row=2, column=0, columnspan=4)
    test2_button = tk.Button(root, text="模拟解冻按键", command=lambda: keyboard.press_and_release(entry2.get()))
    test2_button.grid(row=2, column=1)
    root.mainloop()
# 从 favicon.ico 加载图标
def create_icon_image():
    return Image.open(os.path.join(os.path.dirname(__file__), "favicon.ico"))
# 退出函数
def on_quit(icon, item):
    os._exit(0)
def github():
    webbrowser.open('https://github.com/gmaox/Sunshine-HzFreezer-autotool')
def console():
    ctypes.windll.kernel32.AllocConsole()
    sys.stdout = open("CONOUT$", "w")
# 初始化托盘图标
icon = Icon("test", create_icon_image(), menu=Menu(
    MenuItem("调试", console),
    MenuItem("github/使用说明", github),
    MenuItem("程序设置", on_custom_input),
    MenuItem("Quit", on_quit)
))
def start_icon():
    icon.run()
# 启动托盘图标线程
icon_thread = threading.Thread(target=start_icon)
icon_thread.daemon = True
icon_thread.start()
# 设置要监听的端口和检查时间间隔
SUN = False
data = read_from_json()
KEY1 = data.get("text1", "ctrl+b")
KEY2 = data.get("text2", "ctrl+m")
PORT = int(data.get("text3", "48000"))
INTERVAL = int(data.get("text4","3"))
TIMESLEEP1 =int(data.get("text5","0"))
TIMESLEEP2 =int(data.get("text6","0"))
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
                    time.sleep(TIMESLEEP1)
                    keyboard.press_and_release(KEY2)
            break
    if not port_in_use:
        print(f"Port {PORT} is free.")
        if SUN == True:
            SUN = False
            print("sunshine.exe is close----------------"+KEY1)
            time.sleep(TIMESLEEP2)
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

