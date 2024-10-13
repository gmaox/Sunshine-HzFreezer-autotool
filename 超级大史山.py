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
        return {"text1": "ctrl+b", "text2": "ctrl+m", "text3": "48000", "text4": "3", "text5": "0", "text6": "0","text7": "0"," text8": "120","text9": "0","text10": "0","text11": "输入冻结时的命令","text12": "输入解冻时的命令"}
    except json.JSONDecodeError:
        print("Error decoding JSON.")
        return {"text1": "ctrl+b", "text2": "ctrl+m", "text3": "48000", "text4": "3", "text5": "0", "text6": "0","text7": "0"," text8": "120","text9": "0","text10": "0","text11": "输入冻结时的命令","text12": "输入解冻时的命令"}
# 自定义窗口的回调函数
def on_custom_input():
    global SLEEPVALUE,entry_var8
    data = read_from_json()
    root = tk.Tk()
    root.title("若无法输入请尝试选中窗口")
    entry_var1 = tk.StringVar(value=data.get("text1", "ctrl+b"))
    entry_var2 = tk.StringVar(value=data.get("text2", "ctrl+m"))
    entry_var3 = tk.StringVar(value=data.get("text3", "48000"))
    entry_var4 = tk.StringVar(value=data.get("text4", "3"))
    entry_var5 = tk.StringVar(value=data.get("text5", "0"))
    entry_var6 = tk.StringVar(value=data.get("text6", "0"))
    entry_var7 = tk.StringVar(value=data.get("text7", "0"))
    entry_var8 = tk.StringVar(value=data.get("text8", "120"))
    SLEEPVALUE = data.get("text9", 0)
    entry_var10 = tk.StringVar(value=data.get("text10", "0"))
    entry_var11 = tk.StringVar(value=data.get("text11", "输入冻结时的命令"))
    entry_var12 = tk.StringVar(value=data.get("text12", "输入解冻时的命令"))
    tk.Label(root, text="冻结光标程序快捷键:").grid(row=0, column=0)
    entry1 = tk.Entry(root, textvariable=entry_var1)
    entry1.grid(row=0, column=1)
    tk.Label(root, text="解冻快捷键:").grid(row=1, column=0)
    entry2 = tk.Entry(root, textvariable=entry_var2)
    entry2.grid(row=1, column=1)
    tk.Label(root, text="↑确保按钮点击后有雪藏有反应↑\n").grid(row=3, column=0, columnspan=2)
    tk.Label(root, text="端口号 (默认48000):").grid(row=4, column=0)
    entry3 = tk.Entry(root, textvariable=entry_var3)
    entry3.grid(row=4, column=1)
    tk.Label(root, text="监听间隔 (默认3):").grid(row=5, column=0)
    entry4 = tk.Entry(root, textvariable=entry_var4)
    entry4.grid(row=5, column=1)
    tk.Label(root, text="以下两参数为虚拟显示器准备\n解冻前等待 (默认0):").grid(row=6, column=0)
    entry5 = tk.Entry(root, textvariable=entry_var5)
    entry5.grid(row=6, column=1)
    tk.Label(root, text="冻结前等待 (默认0):").grid(row=7, column=0)
    entry6 = tk.Entry(root, textvariable=entry_var6)
    entry6.grid(row=7, column=1)
    def save_action():
        global SLEEPVALUE
        text1 = entry1.get()
        text2 = entry2.get()
        text3 = entry3.get()
        text4 = entry4.get()
        text5 = entry5.get()
        text6 = entry6.get()
        text7 = entry_var7.get()
        try:
            text8 = entry8.get()
            if selected_option.get()=="睡眠":
                SLEEPVALUE=0
            else:
                SLEEPVALUE=1
            text9 = SLEEPVALUE
        except:
            text8=120
            text9=0
        text10 = entry_var10.get()
        try:
            text11 = entry11.get()
            text12 = entry12.get()
        except:
            text11="输入冻结时的命令"
            text12="输入解冻时的命令"
        new_data = {"text1": text1, "text2": text2, "text3": text3, "text4": text4, "text5": text5, "text6": text6,"text7": text7,"text8": text8,"text9": text9,"text10": text10,"text11": text11,"text12": text12}
        save_to_json(new_data)
        messagebox.showinfo("Data saved successfully!", "保存成功\n若选中了底下两个多选框导致的保存\n请重新打开设置查看更多配置项")
        python = sys.executable
        os.execl(python, python, *sys.argv)
    save_button = tk.Button(root, text="--保存全部修改--", command=save_action)
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
        task_name = "MysunAppAutoStart"  # 任务名称
        app_path = psutil.Process(os.getpid()).exe()  # 可执行文件路径
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
    checkbox1 = tk.Checkbutton(root, text="启用休眠倒计时", variable=entry_var7, command=save_action)
    checkbox1.grid(row=11, column=0)
    if int(entry_var7.get())==1:
        tk.Label(root,text="断连后倒计时多久休眠？(秒)").grid(row=12, column=0)
        entry8 = tk.Entry(root, textvariable=entry_var8)
        entry8.grid(row=12, column=1)
        selected_option = tk.StringVar(root)
        if data.get("text9", 0) == 0:
            selected_option.set("睡眠") 
            options = ["睡眠", "休眠"]
        else:
            selected_option.set("休眠")
            options = ["休眠", "睡眠"]
        option_menu = tk.OptionMenu(root, selected_option, *options)
        tk.Label(root,text="选择以何种方式执行").grid(row=13, column=0)
        option_menu.grid(row=13, column=1)
    checkbox1 = tk.Checkbutton(root, text="启用命令接口", variable=entry_var10, command=save_action)
    checkbox1.grid(row=11, column=1)
    if int(entry_var10.get())==1:
        entry11 = tk.Entry(root, textvariable=entry_var11)
        entry12 = tk.Entry(root, textvariable=entry_var12)
        if int(entry_var7.get())==1:
            entry11.grid(row=14, column=0)
            entry12.grid(row=14, column=1)
        else:
            entry11.grid(row=12, column=0)
            entry12.grid(row=12, column=1)
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

#------------定时休眠---------------
SLEEPBUTTON = data.get("text7", "0")
SLEEPTYPE = int(data.get("text9","0"))
TIMENUM = int(data.get("text8", "120"))
WOPEN = False
def start_sleep():
    if SLEEPBUTTON == False:
        return
    windowsleeprun = threading.Thread(target=timeover)
    windowsleeprun.daemon = True
    windowsleeprun.start()
def timeover():
    global WOPEN
    global WOPENA
    WOPENA = False
    WOPEN = True
    time.sleep(3)
    # 创建Tkinter窗口
    window = tk.Tk()
    window.wm_attributes('-topmost', 1)
    window.title("")
    # 获取屏幕宽度和高度
    screen_width = window.winfo_screenwidth()
    screen_height = window.winfo_screenheight()
    # 计算窗口位置
    x = (screen_width // 2) - (200 // 2)
    y = (screen_height // 2) - (100 // 2) - 150
    # 设置窗口位置
    window.geometry(f"200x100+{x}+{y}")
    tk.Label(window, text="倒计时结束后，系统将会尝试休眠").pack()
    timenum = TIMENUM  # 初始计时值
    label = tk.Label(window, text=f"{timenum}")  # 创建标签显示计时
    label.pack()

    # 定义一个函数来处理窗口关闭事件
    def on_closing():
        global WOPEN
        WOPEN = False
        window.destroy()

    # 更新计时的函数
    def update_timer():
        nonlocal timenum  # 使用nonlocal来修改外部变量
        if timenum > 0:  # 计时未结束
            timenum -= 1
            label.config(text=f"{timenum}")  # 更新标签内容
            window.after(1000, update_timer)  # 每1000毫秒（1秒）调用自己
        else:
            label.config(text="时间到!")  # 计时结束时的显示
            #----------------
            ctypes.windll.powrprof.SetSuspendState(SLEEPTYPE, 1, 0)
            on_closing()
            print("倒计时结束，开始休眠")
            #----------------
            
        if WOPENA:  # 如果窗口已关闭，则不继续执行
            on_closing()
    # 绑定窗口关闭事件
    window.protocol("WM_DELETE_WINDOW", on_closing)

    # 启动计时
    update_timer()  # 开始计时
    tk.Button(window, text="单击按钮或关闭窗口\n取消休眠", command=on_closing).pack()

    # 主循环
    window.mainloop()
def close_sleep():
    global WOPENA
    WOPENA = True
#---------------------------------------------
# 命令相关
def shell_command(command):
    if int(data.get("text10", "0")) == 0:
        return
    if command ==1:
        os.system(data.get("text11", "0"))
    else:
        os.system(data.get("text12", "0"))
# 检查端口是否被占用（重点）
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
                    close_sleep()
                    shell_command(0)
            break
    if not port_in_use:
        print(f"Port {PORT} is free.")
        if SUN == True:
            SUN = False
            print("sunshine.exe is close----------------"+KEY1)
            time.sleep(TIMESLEEP2)
            keyboard.press_and_release(KEY1)
            start_sleep()
            shell_command(1)
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
        time.sleep(10000)
except KeyboardInterrupt:
    print("Program interrupted and stopping...")

