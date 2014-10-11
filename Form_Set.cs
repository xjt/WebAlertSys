using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WebAlertSys
{
    /// <summary>
    /// 参数设置窗体类
    /// 保存窗体设置参数到ini文件
    /// </summary>
    public partial class Form_Set : Form
    {
        //配置文件名
        private static string IniFileName = "param.ini";
        private INIClass ini = new INIClass(IniFileName);

        /// <summary>
        /// 构造函数
        /// 从INI文件读数据，初始化窗体
        /// </summary>
        public Form_Set()
        {
            InitializeComponent();

            //用ini文件初始化控件值
            this.textBox_code.Text = ini.IniReadValue("股票", "代码");
            if (string.IsNullOrEmpty(this.textBox_code.Text)) //ini不存在或者还没有被初始化
            {
                return;
            }
            this.numericUpDown_max.Value = decimal.Parse(ini.IniReadValue("报警参数", "上限"));
            this.numericUpDown_min.Value = decimal.Parse(ini.IniReadValue("报警参数", "下限"));
            this.numericUpDown_Timer_Value.Value = int.Parse(ini.IniReadValue("定时器", "周期"));
            this.comboBox_timer_unit.SelectedIndex = 0;   //默认为秒
        }

        /// <summary>
        /// 写配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSaveClick(object sender, EventArgs e)
        {
            //保存代码
            if (this.radioButton_sh.Checked)//沪市
            {
                ini.IniWriteValue("股票", "代码", this.radioButton_sh.Text + this.textBox_code.Text);
            }
            else//深市
            {
                ini.IniWriteValue("股票", "代码", this.radioButton_sz.Text + this.textBox_code.Text);
            }
              
            //保存报警上下限
            ini.IniWriteValue("报警参数", "上限", this.numericUpDown_max.Value.ToString());
            ini.IniWriteValue("报警参数", "下限", this.numericUpDown_min.Value.ToString());

            //保存报警周期（转化为秒）
            Decimal time_interval = this.numericUpDown_Timer_Value.Value;
            switch (this.comboBox_timer_unit.SelectedIndex)
            {
                case 0:  //秒
                    break;

                case 1:  //分钟
                    time_interval *= 60;
                    break;

                case 2:  //小时
                    time_interval *= 3600;
                    break;
            }
            ini.IniWriteValue("定时器", "周期", time_interval.ToString());

            AutoRunMethod();

            //关闭
            this.Close();
        }

        /// <summary>
        /// 写注册表开机启动
        /// </summary>
        private static void AutoRunMethod()
        {
            //设置为开机自启动
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);//打开注册表子项
            key.SetValue("WebAlert", System.Reflection.Assembly.GetExecutingAssembly().Location);
        }
    }
}