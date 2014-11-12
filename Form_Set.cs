using System;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Data.SQLite;
using System.Data;

namespace WebAlertSys
{ 
    /// <summary>
    /// 监控参数设置类
    /// </summary>
    public partial class Form_Set : Form
    {        
        /// <summary>
        /// 构造函数
        /// 从INI文件读数据，初始化窗体
        /// </summary>
        public Form_Set()
        {
            InitializeComponent();
            SQLiteHelper.ConnSqlLiteDbPath = @"E:\My Projects\WebAlertSys\HisDB";      
        }

        /// <summary>
        /// 写注册表开机启动
        /// </summary>
        private void button_OK_Click(object sender, EventArgs e)
        {
            //设置为开机自启动
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);//打开注册表子项
            key.SetValue("WebAlert", System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        //添加/更新一条报警记录到SQLite
        private void button_Add_Click(object sender, EventArgs e)
        {  
            string sError = string.Empty;     
            
            //股票代码
            if(this.textBox_code.Text=="")
                return;
            string sCode =(this.radioButton_sh.Checked)?"sh":"sz";
            sCode +=this.textBox_code.Text;

            //查询当前代码是否存在
            string sSql = string.Format("SELECT * FROM TB_WARN_INFO WHERE (code = '{0}')", sCode);
            DataTable dt = SQLiteHelper.GetDataTable(out sError, sSql);

            if (dt.Rows.Count != 0) //更新
            {
                sSql = string.Format(
                    @"UPDATE  TB_WARN_INFO SET vfloor ={0}, vceil ={1}, validate ={2} where code='{3}'",
                    this.numericUpDown_min.Value,
                    this.numericUpDown_max.Value,
                    this.checkBox_Validate.Checked?1:0,
                    sCode);
            }
            else//插入
            {
                sSql = string.Format(
                       @"INSERT INTO TB_WARN_INFO (code, vfloor, vceil,validate) VALUES ('{0}',{1},{2},{3})",
                       sCode,
                       this.numericUpDown_min.Value,
                       this.numericUpDown_max.Value,
                       1);

            }

            bool res = SQLiteHelper.UpdateData(out sError, sSql);   

        }

      
       
    }
}