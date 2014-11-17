using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace WebAlertSys
{
    /// <summary>
    /// 业务控制层
    /// 1、控制开机自自启动软件
    /// 2、控制开市后自启动监控
    /// </summary>
    class CWorkFlow
    {
        private static DateTime[] MARKET_TIMES_AM = { DateTime.Today.Add(new TimeSpan(9, 30, 0)), DateTime.Today.Add(new TimeSpan(11, 30, 0)) };  
        private static DateTime[] MARKET_TIMES_PM = { DateTime.Today.Add(new TimeSpan(13, 0, 0)), DateTime.Today.Add(new TimeSpan(15, 0, 0)) };       
 
        /// <summary>
        /// 设置开机自启动
        /// </summary>
        public static void SetAutoRun()
        {           
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);//打开注册表子项
            key.SetValue("WebAlert", System.Reflection.Assembly.GetExecutingAssembly().Location);
        }
        /// <summary>
        /// 判定今天是否为交易日
        /// </summary>
        /// <returns></returns>
        public static bool MarketDayOK()
        {
            bool res = true;
            DayOfWeek today = DateTime.Now.DayOfWeek;
            if (today == DayOfWeek.Saturday || today == DayOfWeek.Sunday)
                res = false;
            
            return res;
        }

        /// <summary>
        /// 判断现在是否处于哪个时段
        /// </summary>
        /// <returns>0:开盘前 1：交易中；2：收盘后</returns>
        public static int MonitorTimeOK()
        {  
            int res=-1;
            if(DateTime.Now <MARKET_TIMES_AM[0])
                res=0;
            else if(DateTime.Now < MARKET_TIMES_AM[1])
                res=1;
            else if(DateTime.Now < MARKET_TIMES_PM[0])
                res =0;
            else if(DateTime.Now < MARKET_TIMES_PM[1])
                res=1;
            else
                res =2;

            return res;     
        }

    }
}
