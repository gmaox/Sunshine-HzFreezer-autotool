import ctypes
import sys
from ctypes import wintypes

# 加载user32.dll和kernel32.dll
user32 = ctypes.windll.user32
kernel32 = ctypes.windll.kernel32

# 定义所需的Windows API函数和类型
EnumWindows = user32.EnumWindows
EnumWindowsProc = ctypes.WINFUNCTYPE(wintypes.BOOL, wintypes.HWND, wintypes.LPARAM)
GetWindowTextLength = user32.GetWindowTextLengthW
GetWindowText = user32.GetWindowTextW
GetWindowThreadProcessId = user32.GetWindowThreadProcessId
SetForegroundWindow = user32.SetForegroundWindow
ShowWindow = user32.ShowWindow
IsIconic = user32.IsIconic
AttachThreadInput = user32.AttachThreadInput
GetForegroundWindow = user32.GetForegroundWindow

def find_window_by_title(title):
    """通过窗口标题查找窗口句柄"""
    hwnds = []
    
    def enum_callback(hwnd, lParam):
        # 获取窗口标题长度
        length = GetWindowTextLength(hwnd)
        if length > 0:
            # 创建缓冲区并获取标题
            buffer = ctypes.create_unicode_buffer(length + 1)
            GetWindowText(hwnd, buffer, length + 1)
            # 检查标题是否匹配
            if buffer.value == title:
                hwnds.append(hwnd)
        return True  # 继续枚举
    
    # 枚举所有顶层窗口
    EnumWindows(EnumWindowsProc(enum_callback), 0)
    return hwnds

def set_foreground_window(title):
    """将指定标题的窗口设为焦点"""
    # 查找所有匹配的窗口句柄
    hwnds = find_window_by_title(title)
    if not hwnds:
        print(f"未找到标题为 '{title}' 的窗口")
        return False
    
    hwnd = hwnds[0]  # 取第一个匹配的句柄
    
    # 如果窗口最小化，则恢复
    if IsIconic(hwnd):
        ShowWindow(hwnd, 9)  # SW_RESTORE
    
    # 获取当前线程ID
    current_tid = kernel32.GetCurrentThreadId()
    # 获取目标窗口的线程ID
    target_tid = GetWindowThreadProcessId(hwnd, None)
    
    # 附加线程输入以允许设置前台窗口
    if current_tid != target_tid:
        AttachThreadInput(current_tid, target_tid, True)
        SetForegroundWindow(hwnd)
        AttachThreadInput(current_tid, target_tid, False)
    else:
        SetForegroundWindow(hwnd)
    
    # 验证是否成功
    foreground_hwnd = GetForegroundWindow()
    return foreground_hwnd == hwnd

if __name__ == "__main__":
    window_title = "自动冻结设置"
    if set_foreground_window(window_title):
        print(f"窗口 '{window_title}' 已置前")
    else:
        print(f"无法将窗口 '{window_title}' 置前")