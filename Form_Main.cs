using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;

namespace WebAlertSys
{
    /// <summary>
    /// 主窗体类
    /// 隐藏窗体
    /// </summary>
    public partial class Form_Main : Form
    {
        //早盘交易起止时间9:30-11:30
        private static DateTime[] MARKET_TIMES_AM = { DateTime.Today.Add(new TimeSpan(9, 30, 0)), DateTime.Today.Add(new TimeSpan(11, 30, 0)) };
        //午盘交易起止时间1:00:-3:00
        private static DateTime[] MARKET_TIMES_PM = { DateTime.Today.Add(new TimeSpan(13, 0, 0)), DateTime.Today.Add(new TimeSpan(15, 0, 0)) };

        private static float[] VReal = { 999999, 0, -999999 };  //当天股票的极小值，实时值，极大值
        private static Icon[] myIcon = { Properties.Resources.icon_heart, Properties.Resources.icon_man };

        private List<string> web_address = new List<string>();  //股票网址       

        private static string IniFileName = "param.ini";//配置文件名
        INIClass ini = new INIClass(IniFileName); //配置文件类

        private string code;                    //股票代码 
        private float VFloor, VCeil;            //最高价、最低价报警值
        private int nInterval_CheckTimer;       //股票的巡检周期(秒)

        //定时器1：巡检定时器
        private bool bCheckTimerStatus = false;//巡检定时器运行标志
        private bool bAlerting = false;         //当前报警状态

        //定时器2:图标闪烁定时器
        private bool bRunning_IconTimer = false;    //闪烁图标定时器是否在运行？
        private int nSplashCounter = 0;             //闪烁计数器(奇偶数切换图标)

        /// <summary>
        /// 主窗体构造函数
        /// </summary>
        public Form_Main()
        {
            InitializeComponent();

            this.Hide();//隐藏主窗体
            this.ShowInTaskbar = false;     //隐藏任务栏图标
            this.notifyIcon1.Visible = true;//显示托盘图标
        }

        /// <summary>
        /// 主窗体加载
        /// 隐藏主窗口和任务栏图标，显示托盘图标，自动启动定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Main_Load(object sender, EventArgs e)
        {
            //从ini读控制参数
            this.ReadParamFromIniFile();

            //根据code构建网址           
            this.web_address.Add(string.Format("http://q.stock.sohu.com/cn/{0}/index.shtml", this.code)); //sohu网
            this.web_address.Add(string.Format("http://quote.eastmoney.com/{0}.html", this.code)); //东方财富网

            //初始化webBrowser控件
            this.webBrowser1.Url = new Uri(this.web_address[0]);//默认选择sohu网
            this.webBrowser1.ScriptErrorsSuppressed = true; //禁止弹出错误信息窗口     
        }

        //双击图标可打开网页
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("IEXPLORE.EXE", this.web_address[0]);
        }

        /// <summary>
        /// 巡检定时器触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_check_Tick(object sender, EventArgs e)
        {
            //发布时这里写入运行日志

            //检索网页内容
            Debug.Assert(this.webBrowser1.Document != null, "无法取得网站内容，请检查网址和代码！");
            //检索网页标题
            string title = this.webBrowser1.Document.Title;
            Debug.Assert(title != "", "无法取得网页标题");

            //解析sohu网页标题的股价
            float realvalue = this.GetRealValue_Sohu(title);
            if (realvalue == -1)
            {
                MessageBox.Show(string.Format("检索不到{0}的股价！\n标题：{1}", this.code, title));
                return;
            }
            VReal[0] = realvalue < VReal[0] ? realvalue : VReal[0]; //当日最低价
            VReal[1] = realvalue;                                   //当日实时价
            VReal[2] = realvalue > VReal[2] ? realvalue : VReal[2]; //当日最高价

            //显示即时价格
            this.notifyIcon1.Text = string.Format(@"{0}: {1} [{2},{3}]",
                this.code,
                VReal[1].ToString(), 
                VReal[0].ToString(), 
                VReal[2].ToString());

            //判定是否报警
            this.bAlerting = this.IfAlertByValue(realvalue);//报警标志
            if (this.bAlerting)    //如果报警...
            {
                //输出界面图标的报警信息
                this.notifyIcon1.BalloonTipTitle = "神马都是浮云";
                this.notifyIcon1.BalloonTipText = "有情况...";
                this.notifyIcon1.ShowBalloonTip(500);   //显示时长ms

                //开启图标报警闪烁
                if (!this.bRunning_IconTimer)
                {
                    this.bRunning_IconTimer = true; //置闪烁标志
                    this.timer_icon.Start();        //开启图标闪烁
                }
            }
            else  //如果未报警...
            {
                //关闭报警闪烁
                if (this.bRunning_IconTimer)
                {
                    this.bRunning_IconTimer = false;//置闪烁标志
                    this.timer_icon.Stop();         //关闭图标闪烁
                }
            }

            return;

        }

        /// <summary>
        /// 图标闪烁定时器触发
        /// 每个周期换一次图标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_icon_Tick(object sender, EventArgs e)
        {
            //用计数器的奇偶性来切换图标
            this.notifyIcon1.Icon = Convert.ToBoolean(this.nSplashCounter % 2) ?
                Properties.Resources.icon_heart : Properties.Resources.icon_man;
           
            this.nSplashCounter++;
            if (this.nSplashCounter++ >= 10)
            {
                this.nSplashCounter = 0;
            }

            return;
        }

        #region 右键菜单

        /// <summary>
        /// 右键菜单：弹出配置窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_配置_Click(object sender, EventArgs e)
        {
            Form_Set fm = new Form_Set();
            if (DialogResult.OK == fm.ShowDialog())   //设置窗口完成后保存配置文件
            {
                //从ini读出最新控制参数
                this.ReadParamFromIniFile();

                //如果巡检定时器已经在运行
                if (this.bCheckTimerStatus)
                {
                    this.timer_check.Stop();
                    this.timer_check.Interval = this.nInterval_CheckTimer;//重设巡检周期后
                    this.timer_check.Start();
                }
            }
        }

        /// <summary>
        /// 启动或停止监控状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_启动或停止_Click(object sender, EventArgs e)
        {
            if (!this.bCheckTimerStatus)   //如果巡检定时器未运行，运行它...
            {
                this.bCheckTimerStatus = true;
                this.timer_check.Interval = this.nInterval_CheckTimer * 1000;
                this.timer_check.Start();//启动巡检
                this.ToolStripMenuItem_启动或停止.Text = "停止";
            }
            else//如果巡检定时器已经运行中，停止它...
            {
                this.bCheckTimerStatus = false;
                this.timer_check.Stop();//停止巡检
                this.ToolStripMenuItem_启动或停止.Text = "启动";

                //无论是否图标闪烁（报警），先停止它
                this.timer_icon.Stop();   //停止图标闪烁定时器
                this.bRunning_IconTimer = false;
            }
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_退出_Click(object sender, EventArgs e)
        {
            this.timer_check.Stop();
            this.timer_icon.Stop();
            this.Close();
        }

        #endregion 右键菜单

        #region 与业务交互层无关的一些数据处理函数

        /// <summary>
        /// 从ini读控制参数（代码、报警值、巡检周期）
        /// </summary>
        private bool ReadParamFromIniFile()
        {
            //取得监控股票代码
            this.code = ini.IniReadValue("股票", "代码");
            if (this.code == string.Empty) //ini文件不存在，或者还没有初始化
            {
                return false;
            }

            //取得报警特征值（上下限、闪烁周期）
            this.VCeil = float.Parse(ini.IniReadValue("报警参数", "上限"));
            this.VFloor = float.Parse(ini.IniReadValue("报警参数", "下限"));
            this.nInterval_CheckTimer = int.Parse(ini.IniReadValue("定时器", "周期"));

            return true;
        }

        /// <summary>
        /// 解析sohu股票web窗体标题，取得实时股价
        /// </summary>
        /// <param name="content">窗体标题字符串，如“双环科技:9.58 2.90% +0.27 - 000707 - 搜狐证券”</param>
        /// <returns>实时股价,错误返回-1</returns>
        private float GetRealValue_Sohu(string content)
        {
            float value = -1;
            try
            {
                string[] array = content.Split(" ".ToCharArray()); //解析出“双环科技:9.58”
                value = float.Parse(array[0].Split(':')[1]); //解析出“9.58”
            }
            catch
            {
            }

            return value;
        }

        /// <summary>
        /// 根据实时股价和参数判定是否需要报警
        /// </summary>
        /// <param name="realValue">实时股价</param>
        /// <returns>是否报警</returns>
        private bool IfAlertByValue(float realValue)
        {
            if (realValue == -1)
            {
                return false;
            }

            return (realValue <= this.VFloor || realValue >= this.VCeil);
        }

        #endregion 与业务交互层无关的一些数据处理函数

        /// <summary>
        /// 定时器3：交易时间的有效性判定，现在是否为交易时间？
        /// 定时器周期固定为5分钟
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Market_Tick(object sender, EventArgs e)
        {   
            //如果现在已经在交易时间中：
            if ((DateTime.Now > MARKET_TIMES_AM[0] && DateTime.Now < MARKET_TIMES_AM[1]) ||
            (DateTime.Now > MARKET_TIMES_PM[0] && DateTime.Now < MARKET_TIMES_PM[1]))
            {
                if (!this.bCheckTimerStatus)    //自动启动巡检
                {
                    StartOrStopMethod(true);
                }
            }
            else
            {
                if (this.bCheckTimerStatus)     //自动停止巡检
                {
                    StartOrStopMethod(false);
                }
            }
        }

        /// <summary>
        /// 启动或停止监控
        /// </summary>
        private void StartOrStopMethod(bool bStart)
        {
            this.bCheckTimerStatus = bStart;
            this.timer_check.Interval = this.nInterval_CheckTimer * 1000;
            if (bStart)
            {
                this.timer_check.Start();//启动巡检
            }
            else
            {
                this.timer_check.Stop(); //停止巡检
            }

            //后处理
            this.ToolStripMenuItem_启动或停止.Text = bStart ? "停止" : "启动";
        }

    
    }
}