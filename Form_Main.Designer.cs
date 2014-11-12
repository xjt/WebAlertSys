namespace WebAlertSys
{
    /// <summary>
    /// 隐藏的主窗口
    /// </summary>
    partial class Form_Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_配置 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItem_启动或停止 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_退出 = new System.Windows.Forms.ToolStripMenuItem();
            this.timer_check = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.timer_icon = new System.Windows.Forms.Timer(this.components);
            this.timer_Market = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_配置,
            this.toolStripMenuItem1,
            this.ToolStripMenuItem_启动或停止,
            this.ToolStripMenuItem_退出});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(101, 76);
            // 
            // ToolStripMenuItem_配置
            // 
            this.ToolStripMenuItem_配置.Name = "ToolStripMenuItem_配置";
            this.ToolStripMenuItem_配置.Size = new System.Drawing.Size(100, 22);
            this.ToolStripMenuItem_配置.Text = "配置";
            this.ToolStripMenuItem_配置.Click += new System.EventHandler(this.ToolStripMenuItem_配置_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(97, 6);
            // 
            // ToolStripMenuItem_启动或停止
            // 
            this.ToolStripMenuItem_启动或停止.Name = "ToolStripMenuItem_启动或停止";
            this.ToolStripMenuItem_启动或停止.Size = new System.Drawing.Size(100, 22);
            this.ToolStripMenuItem_启动或停止.Text = "启动";
            this.ToolStripMenuItem_启动或停止.Click += new System.EventHandler(this.ToolStripMenuItem_启动或停止_Click);
            // 
            // ToolStripMenuItem_退出
            // 
            this.ToolStripMenuItem_退出.Name = "ToolStripMenuItem_退出";
            this.ToolStripMenuItem_退出.Size = new System.Drawing.Size(100, 22);
            this.ToolStripMenuItem_退出.Text = "退出";
            this.ToolStripMenuItem_退出.Click += new System.EventHandler(this.ToolStripMenuItem_退出_Click);
            // 
            // timer_check
            // 
            this.timer_check.Interval = 10000;
            this.timer_check.Tick += new System.EventHandler(this.timer_check_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.notifyIcon1.BalloonTipText = "....";
            this.notifyIcon1.BalloonTipTitle = "WebAlert";
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip2;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "WebAlert";    
            // 
            // timer_icon
            // 
            this.timer_icon.Interval = 500;
            this.timer_icon.Tick += new System.EventHandler(this.timer_icon_Tick);
            // 
            // timer_Market
            // 
            this.timer_Market.Enabled = true;
            this.timer_Market.Interval = 300000;
            this.timer_Market.Tick += new System.EventHandler(this.timer_Market_Tick);
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 363);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_Main";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "stock price monitor";     
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// 巡检定时器周期
        /// </summary>
        private System.Windows.Forms.Timer timer_check;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        /// <summary>
        /// 图标闪烁周期
        /// </summary>
        private System.Windows.Forms.Timer timer_icon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_配置;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_启动或停止;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_退出;
        private System.Windows.Forms.Timer timer_Market;
    }
}

