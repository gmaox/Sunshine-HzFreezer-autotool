using System;
using System.Drawing;
using System.Windows.Forms;

namespace SunshineFreezer
{
    public partial class WhitelistForm : Form
    {
        private AppSettings settings;
        private ListBox listBox;

        public WhitelistForm(AppSettings settings)
        {
            this.settings = settings;
            this.Text = "白名单管理";
            this.Size = new Size(500, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.LightBlue;
            InitializeComponents();
            LoadWhitelist();
        }

        private void InitializeComponents()
        {
            Label label = new Label();
            label.Text = "以下程序不会被自动冻结：";
            label.Location = new Point(20, 15);
            label.Font = new Font("Arial", 12, FontStyle.Bold);
            label.AutoSize = true;
            this.Controls.Add(label);

            listBox = new ListBox();
            listBox.Location = new Point(20, 45);
            listBox.Size = new Size(440, 250);
            listBox.Font = new Font("Arial", 11, FontStyle.Regular);
            this.Controls.Add(listBox);

            TextBox addTextBox = new TextBox();
            addTextBox.Location = new Point(20, 310);
            addTextBox.Size = new Size(280, 25);
            addTextBox.Font = new Font("Arial", 11, FontStyle.Regular);
            addTextBox.Name = "addTextBox";
            this.Controls.Add(addTextBox);

            Button addButton = new Button();
            addButton.Text = "添加";
            addButton.Location = new Point(310, 308);
            addButton.Size = new Size(70, 30);
            addButton.Font = new Font("Arial", 11, FontStyle.Bold);
            addButton.BackColor = Color.LightGreen;
            addButton.Click += AddButton_Click;
            this.Controls.Add(addButton);

            Button removeButton = new Button();
            removeButton.Text = "删除选中";
            removeButton.Location = new Point(20, 360);
            removeButton.Size = new Size(120, 35);
            removeButton.Font = new Font("Arial", 11, FontStyle.Bold);
            removeButton.BackColor = Color.LightCoral;
            removeButton.Click += RemoveButton_Click;
            this.Controls.Add(removeButton);

            Button closeButton = new Button();
            closeButton.Text = "关闭";
            closeButton.Location = new Point(340, 360);
            closeButton.Size = new Size(120, 35);
            closeButton.Font = new Font("Arial", 11, FontStyle.Regular);
            closeButton.Click += (s, e) => this.Close();
            this.Controls.Add(closeButton);
        }

        private void LoadWhitelist()
        {
            listBox.Items.Clear();
            foreach (var item in settings.Whitelist)
            {
                listBox.Items.Add(item);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var textBox = this.Controls["addTextBox"] as TextBox;
            string processName = textBox.Text.Trim();

            if (string.IsNullOrEmpty(processName))
            {
                MessageBox.Show("请输入进程名", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 移除 .exe 后缀（如果有）
            if (processName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                processName = processName.Substring(0, processName.Length - 4);
            }

            if (settings.IsInWhitelist(processName))
            {
                MessageBox.Show("该程序已在白名单中", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            settings.AddToWhitelist(processName);
            LoadWhitelist();
            textBox.Text = "";
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedItem == null)
            {
                MessageBox.Show("请先选择要删除的项", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string processName = listBox.SelectedItem.ToString();
            settings.RemoveFromWhitelist(processName);
            LoadWhitelist();
        }
    }
}
