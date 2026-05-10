using System;
using System.Drawing;
using System.Windows.Forms;

namespace SunshineFreezer
{
    public partial class HistoryForm : Form
    {
        private AppSettings settings;

        public HistoryForm(AppSettings settings)
        {
            this.settings = settings;
            this.Text = "冻结历史记录";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.LightBlue;
            InitializeComponents();
            LoadHistory();
        }

        private void InitializeComponents()
        {
            ListView listView = new ListView();
            listView.View = View.Details;
            listView.Columns.Add("时间", 150);
            listView.Columns.Add("进程名", 150);
            listView.Columns.Add("PID", 80);
            listView.Columns.Add("操作", 100);
            listView.FullRowSelect = true;
            listView.Location = new Point(20, 20);
            listView.Size = new Size(540, 280);
            listView.Font = new Font("Arial", 10, FontStyle.Regular);
            listView.Name = "listViewHistory";
            this.Controls.Add(listView);

            Button clearButton = new Button();
            clearButton.Text = "清空历史";
            clearButton.Location = new Point(150, 320);
            clearButton.Size = new Size(120, 35);
            clearButton.Font = new Font("Arial", 12, FontStyle.Bold);
            clearButton.BackColor = Color.LightCoral;
            clearButton.Click += ClearButton_Click;
            this.Controls.Add(clearButton);

            Button closeButton = new Button();
            closeButton.Text = "关闭";
            closeButton.Location = new Point(330, 320);
            closeButton.Size = new Size(120, 35);
            closeButton.Font = new Font("Arial", 12, FontStyle.Regular);
            closeButton.Click += (s, e) => this.Close();
            this.Controls.Add(closeButton);
        }

        private void LoadHistory()
        {
            var listView = this.Controls["listViewHistory"] as ListView;
            listView.Items.Clear();

            // 倒序显示，最新的在前面
            for (int i = settings.History.Count - 1; i >= 0; i--)
            {
                var record = settings.History[i];
                listView.Items.Add(new ListViewItem(new string[]
                {
                    record.Time,
                    record.ProcessName,
                    record.Pid.ToString(),
                    record.Action
                }));
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("确定要清空所有历史记录吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                settings.History.Clear();
                settings.Save();
                LoadHistory();
            }
        }
    }
}
