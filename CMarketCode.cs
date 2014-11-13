using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace WebAlertSys
{
    class CBMarketCode_BAK
    {          
        public List<string> sCodes = new List<string>();

        //代码左侧补零规则
        private static string[] label = new string[] { "{0:0}", "{0:00}", "{0:000}", "{0:0000}", "{0:00000}", "{0:000000}" };
        private string sCodeType; //股票的特征码，如sz000XXX

        public CBMarketCode_BAK(string _sCodeType)
        {
            Debug.Assert(_sCodeType.Length == 8, "股票特征代码错误");

            this.sCodeType = _sCodeType;
            string head = this.sCodeType.TrimEnd('X');//前缀字符

            //解析特征码为备选代码集合
            int num = (this.sCodeType.Split('X')).Length - 1;
            int max = (int)Math.Pow(10.0, double.Parse(num.ToString()));
            for (int i = 1; i < max; i++)
            {
                string tmp = head + string.Format(label[num - 1], i);
                sCodes.Add(tmp);
            }
        }
    }

    //根据沪深交易所规则，定义所有可能出现的备选代码
    class CMarketCode
    {
        public List<string> sCode_All = new List<string>();

        public CMarketCode(string[] sCodePattern)
        {
            for(int i=0;i<sCodePattern.Length;i++)
            {
                CBMarketCode_BAK tmp = new CBMarketCode_BAK(sCodePattern[i]);
                sCode_All.AddRange(tmp.sCodes);
            }
        }
    }
}
