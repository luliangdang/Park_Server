namespace 汽车仓储管理系统
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lbOnline = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.BeginListen = new System.Windows.Forms.Button();
            this.CloseListen = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.RichTextBox();
            this.txtParkaddr = new System.Windows.Forms.RichTextBox();
            this.Park_get = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.Park_out = new System.Windows.Forms.Button();
            this.Park_check = new System.Windows.Forms.Button();
            this.dbPark = new System.Windows.Forms.DataGridView();
            this.lbHeadcount = new System.Windows.Forms.Label();
            this.uParkAddr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ustate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.utime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dbPark)).BeginInit();
            this.SuspendLayout();
            // 
            // lbOnline
            // 
            this.lbOnline.FormattingEnabled = true;
            this.lbOnline.ItemHeight = 12;
            this.lbOnline.Location = new System.Drawing.Point(489, 186);
            this.lbOnline.Name = "lbOnline";
            this.lbOnline.Size = new System.Drawing.Size(120, 88);
            this.lbOnline.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(431, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(431, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "端口号";
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(489, 47);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(100, 21);
            this.txtIp.TabIndex = 4;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(489, 79);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(100, 21);
            this.txtPort.TabIndex = 5;
            // 
            // BeginListen
            // 
            this.BeginListen.Location = new System.Drawing.Point(489, 117);
            this.BeginListen.Name = "BeginListen";
            this.BeginListen.Size = new System.Drawing.Size(75, 23);
            this.BeginListen.TabIndex = 6;
            this.BeginListen.Text = "启动服务器";
            this.BeginListen.UseVisualStyleBackColor = true;
            this.BeginListen.Click += new System.EventHandler(this.button1_Click);
            // 
            // CloseListen
            // 
            this.CloseListen.Location = new System.Drawing.Point(489, 157);
            this.CloseListen.Name = "CloseListen";
            this.CloseListen.Size = new System.Drawing.Size(75, 23);
            this.CloseListen.TabIndex = 7;
            this.CloseListen.Text = "关闭服务器";
            this.CloseListen.UseVisualStyleBackColor = true;
            this.CloseListen.Click += new System.EventHandler(this.CloseListen_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(314, 280);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ReadOnly = true;
            this.txtMsg.Size = new System.Drawing.Size(295, 122);
            this.txtMsg.TabIndex = 8;
            this.txtMsg.Text = "";
            // 
            // txtParkaddr
            // 
            this.txtParkaddr.Location = new System.Drawing.Point(16, 288);
            this.txtParkaddr.Name = "txtParkaddr";
            this.txtParkaddr.Size = new System.Drawing.Size(240, 38);
            this.txtParkaddr.TabIndex = 9;
            this.txtParkaddr.Text = "";
            // 
            // Park_get
            // 
            this.Park_get.Location = new System.Drawing.Point(16, 332);
            this.Park_get.Name = "Park_get";
            this.Park_get.Size = new System.Drawing.Size(75, 23);
            this.Park_get.TabIndex = 10;
            this.Park_get.Text = "车位已用";
            this.Park_get.UseVisualStyleBackColor = true;
            this.Park_get.Click += new System.EventHandler(this.Park_get_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 270);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "车位地址";
            // 
            // Park_out
            // 
            this.Park_out.Location = new System.Drawing.Point(16, 361);
            this.Park_out.Name = "Park_out";
            this.Park_out.Size = new System.Drawing.Size(75, 23);
            this.Park_out.TabIndex = 12;
            this.Park_out.Text = "车位待用";
            this.Park_out.UseVisualStyleBackColor = true;
            this.Park_out.Click += new System.EventHandler(this.Park_out_Click);
            // 
            // Park_check
            // 
            this.Park_check.Location = new System.Drawing.Point(16, 390);
            this.Park_check.Name = "Park_check";
            this.Park_check.Size = new System.Drawing.Size(75, 23);
            this.Park_check.TabIndex = 13;
            this.Park_check.Text = "检测车位";
            this.Park_check.UseVisualStyleBackColor = true;
            this.Park_check.Click += new System.EventHandler(this.Park_check_Click);
            // 
            // dbPark
            // 
            this.dbPark.AllowUserToAddRows = false;
            this.dbPark.AllowUserToDeleteRows = false;
            this.dbPark.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dbPark.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.uParkAddr,
            this.ustate,
            this.utime});
            this.dbPark.Location = new System.Drawing.Point(14, 46);
            this.dbPark.Name = "dbPark";
            this.dbPark.ReadOnly = true;
            this.dbPark.RowTemplate.Height = 23;
            this.dbPark.Size = new System.Drawing.Size(411, 216);
            this.dbPark.TabIndex = 14;
            // 
            // lbHeadcount
            // 
            this.lbHeadcount.AutoSize = true;
            this.lbHeadcount.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbHeadcount.Location = new System.Drawing.Point(13, 18);
            this.lbHeadcount.Name = "lbHeadcount";
            this.lbHeadcount.Size = new System.Drawing.Size(109, 20);
            this.lbHeadcount.TabIndex = 15;
            this.lbHeadcount.Text = "总车位数：";
            // 
            // uParkAddr
            // 
            this.uParkAddr.HeaderText = "车位地址";
            this.uParkAddr.Name = "uParkAddr";
            this.uParkAddr.ReadOnly = true;
            // 
            // ustate
            // 
            this.ustate.HeaderText = "车位状态";
            this.ustate.Name = "ustate";
            this.ustate.ReadOnly = true;
            // 
            // utime
            // 
            this.utime.HeaderText = "刷新时间";
            this.utime.Name = "utime";
            this.utime.ReadOnly = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 417);
            this.Controls.Add(this.lbHeadcount);
            this.Controls.Add(this.dbPark);
            this.Controls.Add(this.Park_check);
            this.Controls.Add(this.Park_out);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Park_get);
            this.Controls.Add(this.txtParkaddr);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.CloseListen);
            this.Controls.Add(this.BeginListen);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtIp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbOnline);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "汽车仓储管理系统";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dbPark)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox lbOnline;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button BeginListen;
        private System.Windows.Forms.Button CloseListen;
        private System.Windows.Forms.RichTextBox txtMsg;
        private System.Windows.Forms.RichTextBox txtParkaddr;
        private System.Windows.Forms.Button Park_get;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Park_out;
        private System.Windows.Forms.Button Park_check;
        private System.Windows.Forms.DataGridView dbPark;
        private System.Windows.Forms.Label lbHeadcount;
        private System.Windows.Forms.DataGridViewTextBoxColumn uParkAddr;
        private System.Windows.Forms.DataGridViewTextBoxColumn ustate;
        private System.Windows.Forms.DataGridViewTextBoxColumn utime;
    }
}

