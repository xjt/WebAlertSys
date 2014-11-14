using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Data;
using System.Threading;

namespace WebAlertSys
{
    /// <summary>
    /// 报警参数结构
    /// </summary>
    public struct MON_PARAM_STRUCT
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
        private static string IniFileName = "param.ini"; //配置文件名
        INIClass ini = new INIClass(IniFileName);        //配置文件类   

        private static Icon[] myIcon = { Properties.Resources.icon_heart,
                                         Properties.Resources.icon_man }; //图标资源     

        private bool bMonitoring = false;       //监控中标志  
        private bool bIconSplashing = false;    //图标闪烁标志
        private int nSplashCounter = 0;         //闪烁计数(奇偶数切换图标)

        List<MON_PARAM_STRUCT> List_MonPar = new List<MON_PARAM_STRUCT>(); //监控参数集合

        /// <summary>
        /// 主窗体构造函数
        /// </summary>
        public Form_Main()
        {
            InitializeComponent();

            //非交易日退出
            if (!CWorkFlow.MarketDayOK())
                this.Close();

            this.ShowInTaskbar = false;     //隐藏任务栏图标
            this.notifyIcon.Visible = true;//显示托盘图标  

            SQLiteHelper.ConnSqlLiteDbPath = @"E:\My Projects\WebAlertSys\HisDB";
            //Initial_TB_CODE_INFO();       //初始化股票代码库            

            //现在是交易时段，自动启动监控
            int label = CWorkFlow.MonitorTimeOK();
            if (1 == label)
            {
                StartOrStopMonitor(true);//启动巡检         
            }
            else if (label == 2)
            {
                this.Close();
            }
        }

        /// <summary>
        /// 触发股价监控定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Monitor_Tick(object sender, EventArgs e)
        {
            //读出最新监控参数
            this.ReadMonitorParamFromDB();

            //取出监控代码
            int num = this.List_MonPar.Count;
            List<string> codes = new List<string>();
            for (int i = 0; i < num; i++)
            {
                codes.Add(this.List_MonPar[i].code);
            }

            //获取实时股价
            sina_stock sina = new sina_stock(codes);

            bool bWarnning = false;//股价报警标志

            string info_warning = "WebAlert";//图标文本

            //判定是否报警
            for (int i = 0; i < num; i++)
            {
                real_data rd = sina.data_array[i];
                if (rd.当前价 <= this.List_MonPar[i].VFloor || rd.当前价 >= this.List_MonPar[i].VCeil)
                {
                    string BalloonTipTitle = string.Format(@"{0} {1}", 
                        rd.股票名, 
                        (rd.当前价 <= this.List_MonPar[i].VFloor) ? "止损" : "止盈");
                    string BalloonTipText = string.Format(@"{0} {1:0.00}% [{2}-{3}]", 
                        rd.当前价, 
                        100 * ((rd.当前价 / rd.昨收盘价) - 1),
                        this.List_MonPar[i].VFloor,
                        this.List_MonPar[i].VCeil);
                    this.notifyIcon.ShowBalloonTip(500, BalloonTipTitle, BalloonTipText, ToolTipIcon.Warning);//气球显示持续时间
                    Thread.Sleep(1000);             
                    bWarnning = true;                    
                }             
            }

            //这是个特殊操作，解决不能关闭气泡提示的问题
            this.notifyIcon.Visible = false;
            this.notifyIcon.Visible = true;       

            if (bWarnning)//至少有一个报警
            {
                info_warning="操作纪律：\n严格止损，防亏损扩大\n严格止盈，将盈利固化";

                //开启报警图标闪烁
                if (!this.bIconSplashing)
                {
                    this.timer_IconSplash.Start();  //开启图标闪烁   
                    this.bIconSplashing = true;     //置闪烁标志
                }
            }
            else
            {        
                //关闭报警图标闪烁
                if (this.bIconSplashing)
                {
                    this.timer_IconSplash.Stop();     //关闭图标闪烁
                    this.bIconSplashing = false;//置闪烁标志  
                }
            }

            this.notifyIcon.Text = info_warning;

            return;
        }

        /// <summary>
        /// 触发图标闪烁定时器
        /// 每个周期换一次图标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_IconSplash_Tick(object sender, EventArgs e)
        {
            //用计数器的奇偶性来切换图标
            this.notifyIcon.Icon = Convert.ToBoolean(nSplashCounter++ % 2) ?
                Properties.Resources.icon_heart : Properties.Resources.icon_man;
      
            if (this.nSplashCounter >= 10)
            {
                this.nSplashCounter = 0;
            }

            return;
        }

        /// <summary>
        /// 触发交易时间有效性判定
        /// 如果到了交易时间，则自动启动股价监控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_MarketTime_Tick(object sender, EventArgs e)
        {
            if (1 == CWorkFlow.MonitorTimeOK()) //如果现在交易时段
            {
                if (!this.bMonitoring)
                {
                    StartOrStopMonitor(true); //自动启动巡检                    
                }
            }
            else
            {
                if (this.bMonitoring)     //自动停止巡检
                {
                    StartOrStopMonitor(false);
                }
            }
            return;
        }

        #region 右键菜单

        /// <summary>
        /// 右键弹出配置窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_配置_Click(object sender, EventArgs e)
        {
            Form_Set fm = new Form_Set();
            fm.ShowDialog();
        }

        /// <summary>
        /// 右键启动或停止监控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_启动或停止_Click(object sender, EventArgs e)
        {
            if (!this.bMonitoring)
            {
                StartOrStopMonitor(true);
            }
            else//如果巡检定时器已经运行中，停止它...
            {
                StartOrStopMonitor(false);

                this.timer_MarketTime.Stop();   //停止交易时间监控器
                this.timer_IconSplash.Stop();   //停止图标闪烁定时器
                this.bIconSplashing = false;
            }
        }

        /// <summary>
        /// 右键：退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_退出_Click(object sender, EventArgs e)
        {
            this.timer_Monitor.Stop();
            this.timer_Monitor.Stop();
            this.timer_IconSplash.Stop();
            this.Close();
        }

        #endregion 右键菜单

        /// <summary>
        /// 从DB读取监控代码列表、报警值
        /// 给List_MonPar赋值
        /// </summary>
        /// <returns>List_MonPar</returns>
        private bool ReadMonitorParamFromDB()
        {
            //报警数据库   
            string sError;
            string sSql = string.Format("SELECT * FROM TB_WARN_INFO WHERE (validate = 1)");
            DataTable dt = SQLiteHelper.GetDataTable(out sError, sSql);
            Debug.Assert(sError == "");

            //导出
            this.List_MonPar.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                MON_PARAM_STRUCT tmp = new MON_PARAM_STRUCT();
                tmp.code = (dt.Rows[i][0]).ToString();
                tmp.VFloor = float.Parse((dt.Rows[i][1]).ToString());
                tmp.VCeil = float.Parse((dt.Rows[i][2]).ToString());
                this.List_MonPar.Add(tmp);
            }

            return true;
        }

        /// <summary>
        /// 启动或停止股价监控
        /// </summary>
        private void StartOrStopMonitor(bool bStart)
        {
            if (bStart)
            {
                this.timer_Monitor.Start();//启动巡检
            }
            else
            {
                this.timer_Monitor.Stop(); //停止巡检
            }

            this.bMonitoring = bStart;
            this.ToolStripMenuItem_启动或停止.Text = bStart ? "停止" : "启动";
        }

        //初始化沪深股市代码库
        private bool Initial_TB_CODE_INFO()
        {
            //股市代码定义 http://baike.baidu.com/view/965844.htm?fr=aladdin
            //目前只关注：上交所指数sh000XXX 基金sh500XXX sh500XXX A股sh600XXX 
            //临时代码：新股申购sh730XXX 新基金申购sh735XXX   
            //目前只关注：综合指数sz399XXX sz395XXX  A股证券sz000XXX sz001XXX 中小板sz002XXX 创业板sz300XXX 国债逆回购sz131XXX 封闭式基金sz150XXX
            //临时代码： 新股发行申购代码为sz730XXX          
            string[] stock_all ={ "sh000XXX", "sh500XXX", "sh550XXX", "sh60XXXX" ,
                                  "sz399XXX", "sz395XXX", "sz000XXX", "sz001XXX", "sz002XXX", "sz300XXX", "sz131XXX", "sz150XXX"};
            //备选code股票代码集合
            CMarketCode bak = new CMarketCode(stock_all);
            List<string> code_list = bak.sCode_All;

            //发web请求，并解析返回结果
            int id = 10; //每10个一组发送web请求    
            List<string> code_group = new List<string>();
            foreach (string code in code_list)
            {
                code_group.Add(code);
                if (--id > 0)
                    continue;

                sina_stock sina = new sina_stock(code_group);    //发送web请求，并解析   
                foreach (real_data stock in sina.data_array) //根据结果插入数据库
                {
                    if (stock.股票名 == null)
                        continue;

                    //查询当前代码是否存在
                    string sError = string.Empty;
                    string sSql = string.Format("SELECT * FROM TB_CODE_INFO WHERE (code = '{0}')", stock.code);
                    DataTable dt = SQLiteHelper.GetDataTable(out sError, sSql);
                    if (dt == null || dt.Rows.Count == 0)   //插入
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