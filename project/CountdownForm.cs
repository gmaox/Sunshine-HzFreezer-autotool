using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SunshineFreezer
{
    public partial class CountdownForm : Form
    {
        private int timeLeft;
        private int sleepType;

        [DllImport("powrprof.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

        public CountdownForm(int timeNum, int sleepType)
        {
            this.timeLeft = timeNum;
            this.sleepType = sleepType;
            this.Text = "";
            this.TopMost = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(200, 120);

            Label label1 = new Label();
            label1.Text = $"倒计时结束后\n系统将会尝试进行->{(sleepType == 0 ? "睡眠" : "休眠")}";
            label1.Location = new Point(10, 10);
            label1.AutoSize = true;
            this.Controls.Add(label1);

            Label label2 = new Label();
            label2.Text = timeLeft.ToString();
            label2.Location = new Point(10, 50);
            label2.AutoSize = true;
            this.Controls.Add(label2);

            Button button = new Button();
            button.Text = "单击按钮或关闭窗口\n取消休眠";
            button.Location = new Point(10, 70);
            button.Size = new Size(180, 40);
            button.Click += (s, e) => this.Close();
            this.Controls.Add(button);

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += (s, e) =>
            {
                timeLeft--;
                label2.Text = timeLeft.ToString();
                if (timeLeft <= 0)
                {
                    timer.Stop();
                    SetSuspendState(sleepType == 1, true, false);
                    this.Close();
                }
            };
            timer.Start();

            this.FormClosing += (s, e) => timer.Stop();
        }
    }
}