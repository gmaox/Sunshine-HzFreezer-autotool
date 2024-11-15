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
def trytoastshow():
    try:
        toaster.show_toast("​串流监听程序已启动", f"{TOASTPLUS}右键系统托盘图标进行配置", icon_path='',duration=0.01)
        return
    except:
        time.sleep(5)
        trytoastshow()
# 从 JSON 文件读取数据
def read_from_json(filename="1.json"):
    try:
        with open(filename, "r", encoding="utf-8") as file:
            return json.load(file)
    except FileNotFoundError:
        return {"text1": "ctrl+b", "text2": "ctrl+m", "text3": "48000", "text4": "3", "text5": "0", "text6": "0","text7": "0"," text8": "120","text9": "0","text10": "0","text11": "输入冻结时的命令","text12": "输入解冻时的命令","text13": "1","text14": "0"}
    except json.JSONDecodeError:
        print("Error decoding JSON.")
        return {"text1": "ctrl+b", "text2": "ctrl+m", "text3": "48000", "text4": "3", "text5": "0", "text6": "0","text7": "0"," text8": "120","text9": "0","text10": "0","text11": "输入冻结时的命令","text12": "输入解冻时的命令","text13": "1","text14": "0"}

data = read_from_json()
TEXT13 = data.get("text13", "0")
if TEXT13 == "1" and int(data.get("text14", "0")) == 1:
    TOASTPLUS = "但是是暂停状态，请点击系统托盘图标进行恢复\n"
else:
    TOASTPLUS = ""
if ctypes.windll.shell32.IsUserAnAdmin()==0:
    toaster.show_toast("​串流监听程序启动(未使用管理员模式)", "部分游戏需用管理员身份运行工具\n不使用可能会无法冻结\n右键系统托盘图标进行配置", icon_path='',duration=0.01)
    ADMIN = False
elif ctypes.windll.shell32.IsUserAnAdmin()==1:
    trytoastshow()
    ADMIN = True
# 保存数据到 JSON 文件
def save_to_json(data, filename="1.json"):
    try:
        with open(filename, "w", encoding="utf-8") as file:
            json.dump(data, file, ensure_ascii=False, indent=4)
    except Exception as e:
        print(f"Error saving to JSON: {e}")
        messagebox.showerror("Error", f"Failed to save data: {e}")
root = None
# 自定义窗口的回调函数
def on_custom_input():
    global SLEEPVALUE,entry_var8
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
    entry_var13 = tk.StringVar(value=data.get("text13", "1"))
    tk.Label(root, text="冻结光标程序快捷键:").grid(row=0, column=0)
    entry1 = tk.Entry(root, textvariable=entry_var1)
    entry1.grid(row=0, column=1)
    tk.Label(root, text="解冻快捷键:").grid(row=1, column=0)
    entry2 = tk.Entry(root, textvariable=entry_var2)
    entry2.grid(row=1, column=1)
    tk.Label(root, text="↑确保按钮点击后有雪藏有反应↑\n").grid(row=3, column=0, columnspan=2)
    tk.Label(root, text="端口号 (默认48000,不懂别改):").grid(row=4, column=0)
    entry3 = tk.Entry(root, textvariable=entry_var3)
    entry3.grid(row=4, column=1)
    tk.Label(root, text="监听间隔 (默认3,也可以填1):").grid(row=5, column=0)
    entry4 = tk.Entry(root, textvariable=entry_var4)
    entry4.grid(row=5, column=1)
    tk.Label(root, text="解冻前等待 (默认0):").grid(row=6, column=0)
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
        text13 = entry_var13.get()
        if text3 == "" or text4 == "" or text5 == "" or text6 == "" or text7 == "" or text8 == "" or text9 == "" or text10 == "" or text11 == "" or text12 == "":
            messagebox.showerror("Error", "请输入完整数据")
            return
        # if text7 == '1':
            # if getattr(sys, 'frozen', False):
                # base_path = sys._MEIPASS  # 获取资源的临时目录
            # else:
                # base_path = os.path.dirname(__file__)
            # resource_path = os.path.join(base_path, 'sleep.exe')
            # try:
                # shutil.copy(resource_path, 'sleeptimerun.exe')
            # except:
                # print("复制失败")
        new_data = {"text1": text1, "text2": text2, "text3": text3, "text4": text4, "text5": text5, "text6": text6,"text7": text7,"text8": text8,"text9": text9,"text10": text10,"text11": text11,"text12": text12,"text13": text13,"text14": "0"}
        save_to_json(new_data)
        messagebox.showinfo("Data saved successfully!", "保存成功\n程序即将重启")
        python = sys.executable
        os.execl(python, python, *sys.argv)
    save_button = tk.Button(root, text="--保存全部修改--", command=save_action)
    save_button.grid(row=8, column=0, columnspan=2)
    def startuprun():
        if ADMIN==False:
            messagebox.showerror("错误", "请使用管理员权限设置开机自启")
            return
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
                messagebox.showinfo('错误',f"任务创建失败: {e}")

        with open("MysunAppAutoStart.bat", "w", encoding="utf-8") as file:
            file.write(f'@echo off\nif "%1"=="hide" goto Begin\nstart mshta vbscript:createobject("wscript.shell").run("""%~0"" hide",0)(window.close)&&exit\n:Begin\ntimeout /t 4 /nobreak >nul\ncd /d "{os.path.dirname(psutil.Process(os.getpid()).exe())}"\nstart {os.path.basename(psutil.Process(os.getpid()).exe())}')
        task_name = "MysunAppAutoStart"  # 任务名称
        app_path = os.path.dirname(psutil.Process(os.getpid()).exe())+"\\MysunAppAutoStart.bat"  # 可执行文件路径
        

        # 检查任务是否存在
        if check_task_exists(task_name):
            delete_task(task_name)  # 删除现有任务
        else:
            create_startup_task(task_name, app_path)# 创建新任务

    save_button = tk.Button(root, text="开启或关闭开机自启", command=startuprun)
    save_button.grid(row=10, column=0, columnspan=2)
    #tk.Label(root, text="点击任务栏图标可以暂停侦听").grid(row=9, column=0, columnspan=2)
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
    checkbox1 = tk.Checkbutton(root, text="点击系统托盘暂停状态是否继承至下次启动", variable=entry_var13, command=save_action)
    checkbox1.grid(row=9, column=0, columnspan=2)
    root.mainloop()
# 从 favicon.ico 加载图标
def create_icon_image():
    if SLEEPBUTTON == True:
        return Image.open(os.path.join(os.path.dirname(__file__), "favicon_sleep.ico"))
    else:
        return Image.open(os.path.join(os.path.dirname(__file__), "favicon.ico"))
# 退出函数
def on_quit():
    os._exit(0)
def github():
    webbrowser.open('https://github.com/gmaox/Sunshine-HzFreezer-autotool')
def console():
    ctypes.windll.kernel32.AllocConsole()
    sys.stdout = open("CONOUT$", "w")
    # 退出时记录数据

def on_click(icon, item):
    global ITEMCLICK
    print("点击了菜单项：", item)
    if ITEMCLICK:
        ITEMCLICK=False
        icon.icon=create_icon_image()
    else:
        ITEMCLICK=True
        icon.icon=Image.open(os.path.join(os.path.dirname(__file__), "favicon_pause.ico"))
    if TEXT13 == "0":
        return
    if ITEMCLICK:
        data["text14"] = "1"  # 更新字典中的数据
        save_to_json(data)  # 保存更新后的字典
    else:
        data["text14"] = "0"  # 更新字典中的数据
        save_to_json(data)  # 保存更新后的字典

KEY1 = data.get("text1", "ctrl+b")
KEY2 = data.get("text2", "ctrl+m")
SLEEPBUTTON = int(data.get("text7", "0"))
def on_pause():
    time.sleep(0.8)
    keyboard.press_and_release(KEY1)
def on_resume():
    time.sleep(0.8)
    keyboard.press_and_release(KEY2)
# 初始化托盘图标
if TEXT13 == "1" and data.get("text14", "0") == "1":
    ITEMCLICK=True
    ICONIMAGE = Image.open(os.path.join(os.path.dirname(__file__), "favicon_pause.ico"))
else:
    ITEMCLICK=False
    ICONIMAGE = create_icon_image()
icon = Icon("test", ICONIMAGE, menu=Menu(
    MenuItem('暂停程序', on_click, default=True ,visible=False), 
    MenuItem("调试", console),
    MenuItem("Github/使用说明", github),
    MenuItem("程序设置", on_custom_input),
    MenuItem("Quit", on_quit),
    Menu.SEPARATOR,
    MenuItem('              🧊', on_pause),
    Menu.SEPARATOR,
    MenuItem('              🌞', on_resume)
))
def start_icon():
    icon.run()
# 启动托盘图标线程
icon_thread = threading.Thread(target=start_icon)
icon_thread.daemon = True
icon_thread.start()
# 设置要监听的端口和检查时间间隔
SUN = False
PORT = int(data.get("text3", "48000"))
INTERVAL = int(data.get("text4","3"))
TIMESLEEP1 =int(data.get("text5","0"))
TIMESLEEP2 =int(data.get("text6","0"))
SLEEPTYPE = int(data.get("text9","0"))
TIMENUM = int(data.get("text8", "120"))

# 命令相关
DATA1 = int(data.get("text10", "0"))
DATA2 = data.get("text11", "0")
DATA3 = data.get("text12", "0")
def shell_command(command):
    if DATA1 == False:
        return
    if command ==1:
        os.system(DATA2)
    else:
        os.system(DATA3)

# 检查端口是否被占用（重点）
pid = 1145141919810
pidtime = 0
PIDTIME = (TIMENUM/INTERVAL)+3
def check_port_usage():
    global SUN,pid,pidtime,ITEMCLICK
    if ITEMCLICK:
        print("程序暂停，请点击任务栏图标继续运行")
        return
    if pid != 1145141919810:
        pidtime += 1
        if pidtime > PIDTIME:
            pid = 1145141919810
            pidtime = 0
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
                    try:
                        keyboard.press_and_release(KEY2)
                    except:
                        pass
                    if SLEEPBUTTON == True and pid != 1145141919810:
                        try:
                            process = psutil.Process(pid)
                            # process.terminate()
                            # process.wait()
                                    # 遍历所有子进程并终止它们
                            for child in process.children(recursive=True):
                                child.kill()
                                print(f"已结束子进程 PID: {child.pid}")

                            # 最后结束父进程
                            process.kill()
                            print(f"已关闭 PID {pid} 的程序。")
                            pid = 1145141919810
                            pidtime = 0
                        except psutil.NoSuchProcess:
                            print(f"PID {pid} 的程序未找到。")
                            pid = 1145141919810
                            pidtime = 0
                        except Exception as e:
                            print(f"关闭程序时发生错误: {e}")
                    shell_command(0)
            break
    if not port_in_use:
        print(f"Port {PORT} is free.")
        if SUN == True:
            SUN = False
            print("sunshine.exe is close----------------"+KEY1)
            time.sleep(TIMESLEEP2)
            try:
                keyboard.press_and_release(KEY1)
            except:
                pass
            if SLEEPBUTTON == True:
                try:
                    process = subprocess.Popen(["sleeptimerun.exe", str(TIMENUM), str(SLEEPTYPE)]) #, creationflags=subprocess.CREATE_NEW_CONSOLE
                    pid = process.pid
                except:
                    toaster.show_toast("错误", "无法启动计时弹窗,请检查sleeptimerun.exe是否被杀毒软件误杀", icon_path='',duration=0.01)
                    # if getattr(sys, 'frozen', False):
                    #     base_path = sys._MEIPASS  # 获取资源的临时目录
                    # else:
                    #     base_path = os.path.dirname(__file__)
                    # resource_path = os.path.join(base_path, 'sleep.exe')
                    # try:
                    #     shutil.copy(resource_path, 'sleeptimerun.exe')
                    #     process = subprocess.Popen(["sleeptimerun.exe", str(TIMENUM), str(SLEEPTYPE)])
                    #     pid = process.pid
                    # except Exception as e:
                    #     toaster.show_toast("错误", f"复制失败{e}", icon_path='',duration=0.01)
                print(f"启动的程序PID: {pid}")
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

try:
    while True:
        time.sleep(1)
except KeyboardInterrupt:
    print("Program interrupted and stopping...")


