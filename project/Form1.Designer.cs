namespace SunshineFreezer
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.pictureBoxPortStatus = new System.Windows.Forms.PictureBox();
            this.labelPortStatus = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPortStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "端口号 (默认48000,不懂别改):";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(220, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(200, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "检查时间间隔(秒):";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(222, 69);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 21);
            this.textBox2.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(200, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "睡眠前等待时间(秒):";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(222, 99);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 21);
            this.textBox3.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(200, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "睡眠后等待时间(秒):";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(222, 129);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 21);
            this.textBox4.TabIndex = 7;
            // 
            // checkBox1
            // 
            this.checkBox1.Location = new System.Drawing.Point(12, 159);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(200, 20);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "冻结后自动睡眠";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 189);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(200, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "睡眠倒计时(秒):";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(222, 189);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 21);
            this.textBox5.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(12, 219);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(200, 20);
            this.label6.TabIndex = 11;
            this.label6.Text = "睡眠类型:";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Items.AddRange(new object[] {
            "睡眠",
            "休眠"});
            this.comboBox1.Location = new System.Drawing.Point(222, 219);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(100, 20);
            this.comboBox1.TabIndex = 12;
            // 
            // checkBox2
            // 
            this.checkBox2.Location = new System.Drawing.Point(12, 249);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(200, 20);
            this.checkBox2.TabIndex = 13;
            this.checkBox2.Text = "启用自定义命令";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(12, 279);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(200, 20);
            this.label7.TabIndex = 14;
            this.label7.Text = "冻结时命令:";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(222, 279);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(300, 21);
            this.textBox6.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(12, 309);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(200, 20);
            this.label8.TabIndex = 16;
            this.label8.Text = "解冻时命令:";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(222, 309);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(300, 21);
            this.textBox7.TabIndex = 17;
            // 
            // checkBox3
            // 
            this.checkBox3.Location = new System.Drawing.Point(12, 339);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(133, 20);
            this.checkBox3.TabIndex = 18;
            this.checkBox3.Text = "启动时显示通知";
            // 
            // checkBox4
            // 
            this.checkBox4.Location = new System.Drawing.Point(12, 369);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(133, 20);
            this.checkBox4.TabIndex = 19;
            this.checkBox4.Text = "仅在全屏时冻结";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(168, 339);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(152, 50);
            this.buttonSave.TabIndex = 21;
            this.buttonSave.Text = "保存设置";
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // pictureBoxPortStatus
            // 
            this.pictureBoxPortStatus.Location = new System.Drawing.Point(98, 39);
            this.pictureBoxPortStatus.Name = "pictureBoxPortStatus";
            this.pictureBoxPortStatus.Size = new System.Drawing.Size(12, 12);
            this.pictureBoxPortStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxPortStatus.TabIndex = 22;
            this.pictureBoxPortStatus.TabStop = false;
            // 
            // labelPortStatus
            // 
            this.labelPortStatus.AutoSize = true;
            this.labelPortStatus.Location = new System.Drawing.Point(116, 39);
            this.labelPortStatus.Name = "labelPortStatus";
            this.labelPortStatus.Size = new System.Drawing.Size(29, 12);
            this.labelPortStatus.TabIndex = 23;
            this.labelPortStatus.Text = "空闲";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(10, 39);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 20);
            this.label9.TabIndex = 24;
            this.label9.Text = "端口状态：";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 402);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox4);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.labelPortStatus);
            this.Controls.Add(this.pictureBoxPortStatus);
            this.Name = "Form1";
            this.Text = "自动冻结设置";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPortStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBoxStartup;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.PictureBox pictureBoxPortStatus;
        private System.Windows.Forms.Label labelPortStatus;
        private System.Windows.Forms.Label label9;
    }
}

