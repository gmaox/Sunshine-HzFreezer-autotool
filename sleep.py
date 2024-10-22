import ctypes
import os
import sys
import tkinter as tk

# 全局变量，控制窗口状态
if len(sys.argv) > 1:
    TIMENUM = int(sys.argv[1])  # 获取第一个参数
    SLEEPTYPE = int(sys.argv[2])
    print(f"传递的参数: {TIMENUM,SLEEPTYPE}")
else:
    print("没有传递参数。")
    TIMENUM = 10
    SLEEPTYPE = 0
    
if SLEEPTYPE == 0:
    SLEEPTYPEM = "睡眠"
elif SLEEPTYPE == 1:
    SLEEPTYPEM = "休眠"
def timeover():
    # 创建Tkinter窗口
    window = tk.Tk()
    window.title("")
    # 获取屏幕宽度和高度
    screen_width = window.winfo_screenwidth()
    screen_height = window.winfo_screenheight()
    # 计算窗口位置
    x = (screen_width // 2) - (200 // 2)
    y = (screen_height // 2) - (100 // 2) - 150
    # 设置窗口位置
    window.geometry(f"200x100+{x}+{y}")
    tk.Label(window, text=f"倒计时结束后\n系统将会尝试进行->{SLEEPTYPEM}").pack()
    timenum = TIMENUM  # 初始计时值
    label = tk.Label(window, text=f"{timenum}")  # 创建标签显示计时
    label.pack()

    # 定义一个函数来处理窗口关闭事件
    def on_closing():
        window.destroy()
        os._exit(0)


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
            print("倒计时结束，开始休眠")
            #----------------
            on_closing()
    # 绑定窗口关闭事件
    window.protocol("WM_DELETE_WINDOW", on_closing)

    # 启动计时
    update_timer()  # 开始计时
    tk.Button(window, text="单击按钮或关闭窗口\n取消休眠", command=on_closing).pack()

    # 主循环
    window.mainloop()

timeover()
