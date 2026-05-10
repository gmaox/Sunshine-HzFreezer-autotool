using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Web.Script.Serialization;

namespace SunshineFreezer
{
    public partial class Form1 : Form
    {
        [DllImport("shell32.dll")]
        private static extern bool IsUserAnAdmin();

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_MINIMIZE = 6;

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private NotifyIcon notifyIcon;
        private System.Threading.Timer timer;
        private AppSettings settings;
        private bool isPaused = false;
        private int frozenPid = 0;
        private bool isFrozen = false;
        private bool isManualFreeze = false;
        private string pssuspendPath;
        private bool isSettingsMode;
        private Mutex mutex;
        private System.Windows.Forms.Timer portStatusTimer;
        private bool? lastPortOccupied = null; // null=首次, true=上次占用, false=上次释放

        public Form1()
        {
            InitializeComponent();
            pssuspendPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "res", "pssuspend64.exe");
            settings = AppSettings.Load();
            string[] args = Environment.GetCommandLineArgs();
            isSettingsMode = args.Contains("--settings");

            mutex = new Mutex(true, "Sunshine-HzFreezer-autotool", out bool created);
            if (!created && !isSettingsMode)
            {
                // 已有实例在运行，直接退出
                Environment.Exit(0);
                return;
            }

            if (isSettingsMode)
            {
                ShowSettings();
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                SetupTray();
                // 恢复暂停状态继承
                if (settings.text13 == "1" && settings.text14 == "1")
                {
                    isPaused = true;
                    SetTrayIcon("favicon_pause.ico");
                }
                StartMonitoring();
                // 检查是否启用了启动时显示通知
                if (settings.text13 == "1")
                {
                    string toastMsg = isPaused ? "但是是暂停状态，请点击系统托盘图标进行恢复\n" : "";
                    ShowToast("串流监听程序已启动", $"{toastMsg}右键系统托盘图标进行配置");
                    if (!IsUserAnAdmin())
                    {
                        ShowToast("串流监听程序启动(未使用管理员模式)", "部分游戏需用管理员身份运行工具\n不使用可能会无法冻结\n右键系统托盘图标进行配置");
                    }
                }
            }
        }

        private void ShowSettings()
        {
            textBox1.Text = settings.text3;
            textBox2.Text = settings.text4;
            checkBox1.Checked = settings.text7 == "1";
            textBox5.Text = settings.text8;
            comboBox1.SelectedIndex = int.Parse(settings.text9);
            checkBox2.Checked = settings.text10 == "1";
            textBox6.Text = settings.text11;
            textBox7.Text = settings.text12;
            checkBox3.Checked = settings.text13 == "1";
            checkBox4.Checked = settings.text15 == "1";
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Right - this.Width,
                Screen.PrimaryScreen.WorkingArea.Bottom - this.Height);
            this.Show();
            UpdatePortStatus();
            // 设置页面每秒刷新端口状态
            portStatusTimer = new System.Windows.Forms.Timer();
            portStatusTimer.Interval = 1000;
            portStatusTimer.Tick += (s, e) => UpdatePortStatus();
            portStatusTimer.Start();
        }

        private void SetupTray()
        {
            notifyIcon = new NotifyIcon();
            SetTrayIcon("favicon.ico");
            notifyIcon.Text = "串流自动冻结小工具(精简版v1)";
            notifyIcon.MouseClick += NotifyIcon_MouseClick;
            notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
            ContextMenuStrip menu = new ContextMenuStrip();
            // menu.Items.Add("调试(精简版无效)", null, (s, e) => ShowConsole());
            menu.Items.Add("Github/使用说明", null, (s, e) => OpenGithub());
            menu.Items.Add("程序设置", null, (s, e) => OpenSettings());
            ToolStripMenuItem startupItem = new ToolStripMenuItem("开机自启动");
            startupItem.CheckOnClick = true;
            startupItem.Checked = IsStartupEnabled();
            startupItem.Click += (s, e) => ToggleStartup();
            menu.Items.Add(startupItem);
            menu.Items.Add("Quit", null, (s, e) => Application.Exit());
            notifyIcon.ContextMenuStrip = menu;
            notifyIcon.Visible = true;
        }

        private void SetTrayIcon(string iconName)
        {
            string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "res", iconName);
            if (File.Exists(iconPath))
            {
                notifyIcon.Icon = new Icon(iconPath);
            }
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ShowProcessList();
            }
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private ProcessListForm processListForm;

        private void ShowProcessList()
        {
            if (processListForm == null || processListForm.IsDisposed)
            {
                processListForm = new ProcessListForm(this);
                processListForm.FormClosed += (s, e) => processListForm = null;
            }
            processListForm.Show();
            processListForm.Activate();
        }

        public AppSettings GetSettings()
        {
            return settings;
        }

        public void PublicFreeze(int pid)
        {
            isManualFreeze = true;
            FreezeProcess(pid);
        }

        public void PublicUnfreeze(int pid)
        {
            isManualFreeze = false;
            UnfreezeProcess(pid);
        }

        public int GetFrozenPid()
        {
            return frozenPid;
        }

        public bool GetIsPaused()
        {
            return isPaused;
        }

        public void TogglePause()
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                SetTrayIcon("favicon_pause.ico");
            }
            else
            {
                SetTrayIcon("favicon.ico");
            }
            if (settings.text13 == "1")
            {
                settings.text14 = isPaused ? "1" : "0";
                settings.Save();
            }
        }

        public bool GetIsFrozen()
        {
            return isFrozen;
        }

        private void StartMonitoring()
        {
            int interval = int.Parse(settings.text4) * 1000;
            timer = new System.Threading.Timer(CheckPort, null, 0, interval);
        }

        private void CheckPort(object state)
        {
            if (isPaused) return;
            if (settings.text15 == "1" && !IsFullscreen()) return;
            int port = int.Parse(settings.text3);
            int pid = GetPidForPort(port);
            bool portOccupied = pid != 0;

            // 首次运行，只记录状态不执行动作
            if (lastPortOccupied == null)
            {
                lastPortOccupied = portOccupied;
                return;
            }

            // 状态未变化，不执行动作
            if (lastPortOccupied == portOccupied)
            {
                return;
            }

            // 更新状态记录
            lastPortOccupied = portOccupied;

            if (portOccupied)
            {
                // 端口释放→占用：解冻（如果有已冻结的进程）
                if (isFrozen)
                {
                    string processName = "";
                    try
                    {
                        var proc = Process.GetProcessById(frozenPid);
                        processName = proc.ProcessName;
                    }
                    catch { }
                    UnfreezeProcess(frozenPid);
                    ShowTooltipOnUIThread($"端口被占用，已解冻进程: {processName}");
                }
            }
            else
            {
                // 端口占用→释放：冻结前台窗口进程
                if (!isFrozen)
                {
                    IntPtr hwnd = GetForegroundWindow();
                    if (hwnd != IntPtr.Zero)
                    {
                        GetWindowThreadProcessId(hwnd, out uint foregroundPid);
                        if (foregroundPid > 0)
                        {
                            string windowTitle = GetWindowTitle(hwnd);
                            string processName = "";
                            try
                            {
                                var proc = Process.GetProcessById((int)foregroundPid);
                                processName = proc.ProcessName;
                            }
                            catch { }

                            // 检查白名单
                            if (!string.IsNullOrEmpty(processName) && settings.IsInWhitelist(processName))
                            {
                                return;
                            }

                            FreezeProcess((int)foregroundPid);
                            ShowTooltipOnUIThread($"端口已释放，已冻结进程: {processName}\n窗口: {windowTitle}");
                        }
                    }
                }
            }
        }

        private string GetWindowTitle(IntPtr hwnd)
        {
            StringBuilder sb = new StringBuilder(256);
            GetWindowText(hwnd, sb, sb.Capacity);
            return sb.ToString();
        }

        private void ShowTooltipOnUIThread(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(ShowTooltipOnUIThread), message);
                return;
            }
            ShowTooltip(message);
        }

        private int GetPidForPort(int port)
        {
            Process p = new Process();
            p.StartInfo.FileName = "netstat";
            p.StartInfo.Arguments = "-ano";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            string[] lines = output.Split('\n');
            string portStr = ":" + port.ToString();
            foreach (string line in lines)
            {
                if (!line.Contains(portStr)) continue;

                string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2) continue;

                // TCP LISTENING 状态
                if (parts[1].Contains(portStr) && line.Contains("LISTENING"))
                {
                    int pid;
                    if (int.TryParse(parts[parts.Length - 1], out pid))
                        return pid;
                }
                // UDP 端口（netstat 中 UDP 行没有 LISTENING 状态）
                if (parts[0].ToUpper().StartsWith("UDP") && parts[1].Contains(portStr))
                {
                    int pid;
                    if (int.TryParse(parts[parts.Length - 1], out pid))
                        return pid;
                }
            }
            return 0;
        }

        private void FreezeProcess(int pid)
        {
            if (!File.Exists(pssuspendPath))
            {
                MessageBox.Show("pssuspend64.exe not found in application directory.");
                return;
            }

            string processName = "";
            try
            {
                var proc = Process.GetProcessById(pid);
                processName = proc.ProcessName;
            }
            catch { }

            // 最小化被冻结的窗口
            MinimizeProcessWindows(pid);
            System.Threading.Thread.Sleep(300);

            Process p = new Process();
            p.StartInfo.FileName = pssuspendPath;
            p.StartInfo.Arguments = $"{pid}";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            isFrozen = true;
            frozenPid = pid;

            // 记录历史
            if (!string.IsNullOrEmpty(processName))
            {
                settings.AddHistoryRecord(processName, pid, "冻结");
            }

            if (settings.text10 == "1")
            {
                ExecuteCommand(settings.text11);
            }
            if (settings.text7 == "1")
            {
                StartSleepTimer(int.Parse(settings.text8), int.Parse(settings.text9));
            }
        }

        private void MinimizeProcessWindows(int pid)
        {
            try
            {
                var proc = Process.GetProcessById(pid);
                // 枚举进程的主窗口句柄并最小化
                if (proc.MainWindowHandle != IntPtr.Zero)
                {
                    ShowWindow(proc.MainWindowHandle, SW_MINIMIZE);
                }
            }
            catch { }
        }

        private void UnfreezeProcess(int pid)
        {
            string processName = "";
            try
            {
                var proc = Process.GetProcessById(pid);
                processName = proc.ProcessName;
            }
            catch { }

            Process p = new Process();
            p.StartInfo.FileName = pssuspendPath;
            p.StartInfo.Arguments = $"-r {pid}";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            isFrozen = false;
            frozenPid = 0;

            // 记录历史
            if (!string.IsNullOrEmpty(processName))
            {
                settings.AddHistoryRecord(processName, pid, "解冻");
            }

            if (settings.text10 == "1")
            {
                ExecuteCommand(settings.text12);
            }
        }

        private void ExecuteCommand(string command)
        {
            Process.Start("cmd.exe", "/c " + command);
        }

        private void ShowConsole()
        {
            AllocConsole();
        }

        private void OpenGithub()
        {
            Process.Start("https://github.com/gmaox/Sunshine-HzFreezer-autotool/tree/NET");
        }

        private void OpenSettings()
        {
            Process.Start(Application.ExecutablePath, "--settings");
        }

        private void ShowToast(string title, string message)
        {
            notifyIcon.ShowBalloonTip(1000, title, message, ToolTipIcon.Info);
        }

        private void StartSleepTimer(int timeNum, int sleepType)
        {
            CountdownForm countdown = new CountdownForm(timeNum, sleepType);
            countdown.ShowDialog();
        }

        private bool IsFullscreen()
        {
            IntPtr hwnd = GetForegroundWindow();
            RECT rect;
            GetWindowRect(hwnd, out rect);
            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;
            Rectangle screenBounds = Screen.PrimaryScreen.Bounds;
            return width >= screenBounds.Width && height >= screenBounds.Height;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            settings.text3 = textBox1.Text;
            settings.text4 = textBox2.Text;
            settings.text7 = checkBox1.Checked ? "1" : "0";
            settings.text8 = textBox5.Text;
            settings.text9 = comboBox1.SelectedIndex.ToString();
            settings.text10 = checkBox2.Checked ? "1" : "0";
            settings.text11 = textBox6.Text;
            settings.text12 = textBox7.Text;
            settings.text13 = checkBox3.Checked ? "1" : "0";
            settings.text15 = checkBox4.Checked ? "1" : "0";
            settings.Save();
            ShowTooltip("保存成功");
        }

        private void ShowTooltip(string message)
        {
            Form tooltipForm = new Form()
            {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = Color.FromArgb(50, 50, 50),
                Size = new Size(300, 50),
                TopMost = true,
                ShowInTaskbar = false
            };
            Label label = new Label()
            {
                Text = message,
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            tooltipForm.Controls.Add(label);
            tooltipForm.Show();
            System.Windows.Forms.Timer closeTimer = new System.Windows.Forms.Timer();
            closeTimer.Interval = 2000;
            closeTimer.Tick += (s, ev) =>
            {
                closeTimer.Stop();
                tooltipForm.Close();
            };
            closeTimer.Start();
        }

        private void SetStartup(bool enable)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (enable)
            {
                rk.SetValue("SunshineFreezer", Application.ExecutablePath);
            }
            else
            {
                rk.DeleteValue("SunshineFreezer", false);
            }
        }

        private bool IsStartupEnabled()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false);
            return rk?.GetValue("SunshineFreezer") != null;
        }

        private void ToggleStartup()
        {
            bool enabled = IsStartupEnabled();
            SetStartup(!enabled);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isSettingsMode)
            {
                e.Cancel = true;
                this.Hide();
            }
            else
            {
                portStatusTimer?.Stop();
                portStatusTimer?.Dispose();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdatePortStatus();
        }

        private void UpdatePortStatus()
        {
            if (!isSettingsMode) return;

            int port;
            if (!int.TryParse(textBox1.Text, out port) || port <= 0 || port > 65535)
            {
                SetPortStatusUI(Color.Gray, "无效端口");
                return;
            }

            int pid = GetPidForPort(port);
            if (pid == 0)
            {
                SetPortStatusUI(Color.Gray, "空闲");
            }
            else
            {
                string processName = "";
                try
                {
                    var proc = Process.GetProcessById(pid);
                    processName = proc.ProcessName;
                }
                catch { processName = $"PID:{pid}"; }

                SetPortStatusUI(Color.LimeGreen, processName);
            }
        }

        private void SetPortStatusUI(Color dotColor, string statusText)
        {
            if (pictureBoxPortStatus.InvokeRequired)
            {
                pictureBoxPortStatus.Invoke(new Action<Color, string>(SetPortStatusUI), dotColor, statusText);
                return;
            }

            // 释放旧图片
            if (pictureBoxPortStatus.Image != null)
            {
                pictureBoxPortStatus.Image.Dispose();
                pictureBoxPortStatus.Image = null;
            }

            Bitmap bmp = new Bitmap(12, 12);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                using (SolidBrush brush = new SolidBrush(dotColor))
                {
                    g.FillEllipse(brush, 0, 0, 12, 12);
                }
            }
            pictureBoxPortStatus.Image = bmp;
            labelPortStatus.Text = statusText;
            labelPortStatus.ForeColor = dotColor == Color.Gray ? SystemColors.GrayText : Color.Green;
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
