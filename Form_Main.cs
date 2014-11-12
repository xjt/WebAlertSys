using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Data;

namespace WebAlertSys
{
    /// <summary>
    /// 报警参数结构
    /// </summary>
    public struct INFO_WARN
    {
        public string code;            //股票代码 
        public float VFloor;           //止损价
        public float VCeil;            //止盈价
    }

    /// <summary>
    /// 主窗体类
    /// 隐藏窗体
    /// </summary>
    public partial class Form_Main : Form
    {       
        private static DateTime[] MARKET_TIMES_AM = { DateTime.Today.Add(new TimeSpan(9, 30, 0)), DateTime.Today.Add(new TimeSpan(11, 30, 0)) };  //交易时段   
        private static DateTime[] MARKET_TIMES_PM = { DateTime.Today.Add(new TimeSpan(13, 0, 0)), DateTime.Today.Add(new TimeSpan(15, 0, 0)) };       
        private static Icon[] myIcon = { Properties.Resources.icon_heart, Properties.Resources.icon_man }; //图标资源    
        private static string IniFileName = "param.ini";//配置文件名
        INIClass ini = new INIClass(IniFileName); //配置文件类   
        List<INFO_WARN> par_set = new List<INFO_WARN>(); //报警参数结构体集合

        //定时器1：巡检定时器
        private bool bCheckTimerStatus = false;//巡检定时器运行标志    

        //定时器2:图标闪烁节奏器
        private bool bRunning_IconTimer = false;    //是否在运行？
        private int nSplashCounter = 0;             //闪烁计数(奇偶数切换图标)

        /// <summary>
        /// 主窗体构造函数
        /// </summary>
        public Form_Main()
        {
            InitializeComponent();          

            SQLiteHelper.ConnSqlLiteDbPath = @"E:\My Projects\WebAlertSys\HisDB";   
    
            this.ShowInTaskbar = false;     //隐藏任务栏图标
            this.notifyIcon1.Visible = true;//显示托盘图标  
          
            //从ini读控制参数
            this.ReadParamFromIniFile();  

            //初始化股票代码库
            Initial_TB_CODE_INFO();
        }

        /// <summary>
        /// 巡检定时器触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_check_Tick(object sender, EventArgs e)
        {
            //获取实时价格
            int num = this.par_set.Count;
            List<string> codes = new List<string>();
            for (int i = 0; i < num; i++)
            {
                codes.Add(this.par_set[i].code);
            }
            sina_stock sina = new sina_stock(codes);      
 
            //判定是否报警
            for (int i = 0; i < num; i++)
            {
                real_data rd = sina.data_array[i];
                if (rd.当前价 <= this.par_set[i].VFloor || rd.当前价 >= this.par_set[i].VCeil)
                {
                    //报警 
                    //输出界面图标的报警信息
                    this.notifyIcon1.BalloonTipTitle = "神马都是浮云";
                    this.notifyIcon1.BalloonTipText = "有情况...";
                    this.notifyIcon1.ShowBalloonTip(500);   //显示时长ms

                    //开启报警图标闪烁
                    if (!this.bRunning_IconTimer)
                    {
                        this.bRunning_IconTimer = true; //置闪烁标志
                        this.timer_icon.Start();        //开启图标闪烁
                    }
                }
                else
                {
                    //关闭报警图标闪烁
                    if (this.bRunning_IconTimer)
                    {
                        this.bRunning_IconTimer = false;//置闪烁标志
                        this.timer_icon.Stop();         //关闭图标闪烁
                    }
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

                //如果巡检定时器已经在运行，重启
                if (this.bCheckTimerStatus)
                {
                    this.timer_check.Stop();                 
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
        /// 从ini读取监控代码列表、报警值）
        /// </summary>
        /// <returns>par_set</returns>
        private bool ReadParamFromIniFile()
        {
            this.par_set.Clear();

            //取得股票代码列表
            string codes_line = ini.IniReadValue("股票列表","codes");
            if (codes_line == string.Empty) //ini文件不存在，或者还没有初始化
            {
                return false;
            }
            string[] tmp = codes_line.Split(',');

            for (int i = 0; i < tmp.Length; i++)
            {
                string tmp1 = ini.IniReadValue("报警参数", tmp[i]);                

                INFO_WARN inf = new INFO_WARN();
                inf.code = tmp[i];
                inf.VFloor = float.Parse((tmp1.Split(','))[0]);//取得报警特征值           
                inf.VCeil = float.Parse((tmp1.Split(','))[1]);//取得报警特征值  
                this.par_set.Add(inf);           
            }      

            return true;
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
            this.timer_check.Interval = 1000;
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



        //初始化沪深股市代码库
        private bool Initial_TB_CODE_INFO()
        {
            //股市代码定义 http://baike.baidu.com/view/965844.htm?fr=aladdin
                        
       
            //备选code股票代码集合
            CMarketCode bak = new CMarketCode();
            List<string> code_list = bak.sCode_All;
           
            
            //发web请求，并解析返回结果
            int id = 10; //每10个一组发送web请求    
            List<string> code_group = new List<string>();
            foreach (string code in code_list)
            {
                code_group.Add(code);
                if (--id > 0)
                    continue;

                //发送web请求，并解析
                sina_stock sina = new sina_stock(code_group);
           

                //根据结果插入数据库
                foreach (real_data stock in sina.data_array)
                {
                    if (stock.股票名 == null)
                        continue;

                    string sError = string.Empty;

                    //查询当前代码是否存在
                    string sSql = string.Format("SELECT * FROM TB_CODE_INFO WHERE (code = '{0}')", stock.code);
                    DataTable dt = SQLiteHelper.GetDataTable(out sError, sSql);
                    if (dt==null || dt.Rows.Count == 0)   //插入
                    {
                        sSql = string.Format(
                               @"INSERT INTO TB_CODE_INFO (code, name) VALUES ('{0}','{1}')",
                               stock.code,
                              stock.股票名);
                        bool res = SQLiteHelper.UpdateData(out sError, sSql);
                    }
                }

                //后续处理
                code_group.Clear();
                id = 10;
            }


              

                return true;
        }
    
    }
}