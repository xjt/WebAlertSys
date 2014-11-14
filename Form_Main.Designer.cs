﻿namespace WebAlertSys
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
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_配置 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItem_启动或停止 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_退出 = new System.Windows.Forms.ToolStripMenuItem();
            this.timer_Monitor = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.timer_IconSplash = new System.Windows.Forms.Timer(this.components);
            this.timer_MarketTime = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_配置,
            this.toolStripMenuItem1,
            this.ToolStripMenuItem_启动或停止,
            this.ToolStripMenuItem_退出});
            this.contextMenuStrip.Name = "contextMenuStrip2";
            this.contextMenuStrip.Size = new System.Drawing.Size(101, 76);
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
            // timer_Monitor
            // 
            this.timer_Monitor.Interval = 10000;
            this.timer_Monitor.Tick += new System.EventHandler(this.timer_Monitor_Tick);
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "WebAlert";
            this.notifyIcon.Visible = true;
            // 
            // timer_IconSplash
            // 
            this.timer_IconSplash.Interval = 500;
            this.timer_IconSplash.Tick += new System.EventHandler(this.timer_IconSplash_Tick);
            // 
            // timer_MarketTime
            // 
            this.timer_MarketTime.Enabled = true;
            this.timer_MarketTime.Interval = 60000;
            this.timer_MarketTime.Tick += new System.EventHandler(this.timer_MarketTime_Tick);
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
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// 巡检定时器周期
        /// </summary>
        private System.Windows.Forms.Timer timer_Monitor;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        /// <summary>
        /// 图标闪烁周期
        /// </summary>
        private System.Windows.Forms.Timer timer_IconSplash;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_配置;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_启动或停止;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_退出;
        private System.Windows.Forms.Timer timer_MarketTime;
    }
}

