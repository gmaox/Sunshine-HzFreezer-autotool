using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SunshineFreezer
{
    public partial class ProcessListForm : Form
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventHook, WinEventDelegate dele, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        private const int SW_MINIMIZE = 6;
        private const uint EVENT_SYSTEM_FOREGROUND = 3;
        private const uint WINEVENT_OUTOFCONTEXT = 0;

        private Form1 mainForm;
        private ListView listViewFrozen;
        private Timer refreshTimer;
        private Button freezeForegroundButton;
        private Button pauseButton;
        private Button whitelistButton;
        private Button historyButton;
        private IntPtr winEventHook = IntPtr.Zero;
        private int currentProcessId;
        private bool isFreezing = false;

        public ProcessListForm(Form1 mainForm)
        {
            this.mainForm = mainForm;
            this.Text = "进程列表";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Right - this.Width, Screen.PrimaryScreen.WorkingArea.Bottom - this.Height);
            this.BackColor = Color.LightBlue;
            this.Font = new Font("Arial", 12, FontStyle.Regular);
            InitializeComponents();
            LoadProcesses();
            refreshTimer = new Timer();
            refreshTimer.Interval = 1000;
            refreshTimer.Tick += (s, e) => UpdateList();
            refreshTimer.Start();
        }

        private void InitializeComponents()
        {
            listViewFrozen = new ListView();
            listViewFrozen.View = View.Details;
            listViewFrozen.Columns.Add("进程名", 200);
            listViewFrozen.Columns.Add("PID", 100);
            listViewFrozen.FullRowSelect = true;
            listViewFrozen.Location = new Point(20, 20);
            listViewFrozen.Size = new Size(560, 100);
            listViewFrozen.Font = new Font("Arial", 10, FontStyle.Regular);
            listViewFrozen.ItemActivate += ListView_ItemActivate;
            this.Controls.Add(listViewFrozen);

            Label labelFrozen = new Label();
            labelFrozen.Text = "冻结中的程序：";
            labelFrozen.Location = new Point(20, 130);
            labelFrozen.Font = new Font("Arial", 14, FontStyle.Bold);
            labelFrozen.AutoSize = true;
            this.Controls.Add(labelFrozen);

            freezeForegroundButton = new Button();
            freezeForegroundButton.Text = "冻结前台窗口";
            freezeForegroundButton.Location = new Point(150, 160);
            freezeForegroundButton.Size = new Size(300, 50);
            freezeForegroundButton.Font = new Font("Arial", 16, FontStyle.Bold);
            freezeForegroundButton.BackColor = Color.LightGreen;
            freezeForegroundButton.Click += FreezeForegroundButton_Click;
            this.Controls.Add(freezeForegroundButton);

            // 白名单按钮（左边）
            whitelistButton = new Button();
            whitelistButton.Text = "白名单";
            whitelistButton.Location = new Point(20, 220);
            whitelistButton.Size = new Size(120, 40);
            whitelistButton.Font = new Font("Arial", 12, FontStyle.Bold);
            whitelistButton.BackColor = Color.LightYellow;
            whitelistButton.Click += WhitelistButton_Click;
            this.Controls.Add(whitelistButton);

            // 暂停按钮（中间）
            pauseButton = new Button();
            pauseButton.Text = mainForm.GetIsPaused() ? "继续监听" : "暂停监听";
            pauseButton.Location = new Point(150, 220);
            pauseButton.Size = new Size(150, 40);
            pauseButton.Font = new Font("Arial", 14, FontStyle.Bold);
            pauseButton.BackColor = mainForm.GetIsPaused() ? Color.LightGreen : Color.LightCoral;
            pauseButton.Click += PauseButton_Click;
            this.Controls.Add(pauseButton);

            // 历史按钮（右边）
            historyButton = new Button();
            historyButton.Text = "历史";
            historyButton.Location = new Point(310, 220);
            historyButton.Size = new Size(120, 40);
            historyButton.Font = new Font("Arial", 12, FontStyle.Bold);
            historyButton.BackColor = Color.LightBlue;
            historyButton.Click += HistoryButton_Click;
            this.Controls.Add(historyButton);

            Button closeButton = new Button();
            closeButton.Text = "关闭";
            closeButton.Location = new Point(250, 280);
            closeButton.Size = new Size(100, 40);
            closeButton.Font = new Font("Arial", 12, FontStyle.Regular);
            closeButton.Click += (s, e) => this.Close();
            this.Controls.Add(closeButton);
        }

        private void FreezeForegroundButton_Click(object sender, EventArgs e)
        {
            // 防止重复点击
            if (isFreezing || winEventHook != IntPtr.Zero)
            {
                return;
            }
            freezeForegroundButton.Enabled = false;
            isFreezing = true;
            currentProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
            WinEventDelegate dele = new WinEventDelegate(WinEventHook);
            // 保持委托引用，防止被垃圾回收
            this.dele = dele;
            winEventHook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, dele, 0, 0, WINEVENT_OUTOFCONTEXT);
            ShowTooltip("请点击要冻结的窗口");
        }

        private WinEventDelegate dele; // 保持委托引用

        private void PauseButton_Click(object sender, EventArgs e)
        {
            mainForm.TogglePause();
            bool isPaused = mainForm.GetIsPaused();
            pauseButton.Text = isPaused ? "继续监听" : "暂停监听";
            pauseButton.BackColor = isPaused ? Color.LightGreen : Color.LightCoral;
        }

        private void WhitelistButton_Click(object sender, EventArgs e)
        {
            WhitelistForm form = new WhitelistForm(mainForm.GetSettings());
            form.ShowDialog();
        }

        private void HistoryButton_Click(object sender, EventArgs e)
        {
            HistoryForm form = new HistoryForm(mainForm.GetSettings());
            form.ShowDialog();
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
            Timer closeTimer = new Timer();
            closeTimer.Interval = 2000;
            closeTimer.Tick += (s, ev) =>
            {
                closeTimer.Stop();
                tooltipForm.Close();
            };
            closeTimer.Start();
        }

        public void WinEventHook(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (!isFreezing) return;

            try
            {
                uint pid;
                GetWindowThreadProcessId(hwnd, out pid);
                if (pid != 0 && pid != (uint)currentProcessId)
                {
                    var proc = System.Diagnostics.Process.GetProcessById((int)pid);

                    // 检查白名单
                    if (mainForm.GetSettings().IsInWhitelist(proc.ProcessName))
                    {
                        ShowTooltip($"{proc.ProcessName} 在白名单中，跳过冻结");
                        UnhookWinEvent(winEventHook);
                        winEventHook = IntPtr.Zero;
                        isFreezing = false;
                        freezeForegroundButton.Enabled = true;
                        return;
                    }

                    // 避免冻结 explorer.exe
                    if (proc.ProcessName.ToLower() == "explorer")
                    {
                        return;
                    }

                    UnhookWinEvent(winEventHook);
                    winEventHook = IntPtr.Zero;
                    isFreezing = false;
                    System.Threading.Thread.Sleep(300);
                    ShowWindow(hwnd, SW_MINIMIZE);
                    System.Threading.Thread.Sleep(300);
                    mainForm.PublicFreeze((int)pid);
                    UpdateList();
                    freezeForegroundButton.Enabled = true;
                }
            }
            catch { }
        }

        private void ListView_ItemActivate(object sender, EventArgs e)
        {
            ListView listView = sender as ListView;
            if (listView.SelectedItems.Count > 0)
            {
                int pid = int.Parse(listView.SelectedItems[0].SubItems[1].Text);
                mainForm.PublicUnfreeze(pid);
                UpdateList();
            }
        }

        private void LoadProcesses()
        {
            UpdateList();
        }

        private void UpdateList()
        {
            listViewFrozen.Items.Clear();
            if (mainForm.GetIsFrozen() && mainForm.GetFrozenPid() != 0)
            {
                try
                {
                    var proc = System.Diagnostics.Process.GetProcessById(mainForm.GetFrozenPid());
                    listViewFrozen.Items.Add(new ListViewItem(new string[] { proc.ProcessName, mainForm.GetFrozenPid().ToString() }));
                }
                catch { }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            refreshTimer.Stop();
            base.OnFormClosing(e);
        }
    }
}
