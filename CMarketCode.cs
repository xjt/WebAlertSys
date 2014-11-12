using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace WebAlertSys
{
    class CBMarketCode_BAK
    {
        //代码左侧补零规则
        private static string[] label = new string[] { "{0:0}", "{0:00}", "{0:000}", "{0:0000}", "{0:00000}", "{0:000000}"};

        public List<string> sCodes = new List<string>();
        private string sCodeType; //股票的特征码，如sz000XXX
        public CBMarketCode_BAK(string _sCodeType)
        {
            Debug.Assert(_sCodeType.Length == 8, "股票特征代码错误");

            this.sCodeType = _sCodeType;
            string head = this.sCodeType.TrimEnd('X');//前缀字符

            //解析特征码为备选代码集合
            int num = (this.sCodeType.Split('X')).Length-1;
            int max=(int)Math.Pow(10.0,double.Parse(num.ToString()));
            for (int i = 1; i < max; i++)
            {
                string tmp = head +string.Format(label[num-1], i);
                sCodes.Add(tmp);
            }

        }
      

    }
    //根据沪深交易所规则，定义所有可能出现的备选代码
    class CMarketCode
    {

        public List<string> sCode_All= new List<string>(); 

        public CMarketCode()
        {

            /*
             * 在上海证券交易所上市的证券，根据上交所“证券编码实施方案”，采用6位数编制方法，
             * 前3位数为区别证券品种，具体见下表所列：
             * 000××× 上证指数
             * 001×××国债现货；
             * 110×××120×××企业债券；
             * 129×××100×××可转换债券；
             * 201×××国债回购；
             * 310×××国债期货；
             * 500×××550×××基金；
             * 600×××A股；
             * 700×××配股；
             * 710×××转配股；
             * 701×××转配股再配股；
             * 711×××转配股再转配股；
             * 720×××红利；
             * 730×××新股申购；
             * 735×××新基金申购；
             * 737×××新股配售；
             * 900×××B股。
             */
            //目前只关注：上交所指数sh000XXX 基金sh500XXX sh500XXX A股sh600XXX 
            //临时代码：新股申购sh730XXX 新基金申购sh735XXX   
            string[] stock_sh = new string[] { "sh000XXX", "sh500XXX", "sh550XXX", "sh600XXX"};
            for (int i = 0; i < stock_sh.Length; i++)
            {
                CBMarketCode_BAK tmp = new CBMarketCode_BAK(stock_sh[i]);
                sCode_All.AddRange(tmp.sCodes);
            }


        /*
         * 深圳证券市场的证券代码由原来的4位长度统一升为6位长度。
         * 1、新证券代码编码规则升位后的证券代码采用6位数字编码，编码规则定义如下：
         * 顺序编码区：6位代码中的第3位到第6位，取值范围为0001-9999。
         * 证券种类标识区：6位代码中的最左两位，其中第1位标识证券大类，第2位标识该大类下的衍生证券。
         * 第1位 第2位 第3-6位 定义
         * 0 0 xxxx A股证券(其中，中小板是002XXX)
         * 3 xxxx A股A2权证
         * 7 xxxx A股增发
         * 8 xxxx A股A1权证
         * 9 xxxx A股转配
         * 1 0 xxxx 国债现货
         * 1 xxxx 债券
         * 2 xxxx 可转换债券
         * 3 xxxx 国债回购
         * 1 7 xxxx 原有投资基金
         * 8 xxxx 证券投资基金
         * 2 0 xxxx B股证券
         * 7 xxxx B股增发
         * 8 xxxx B股权证
         * 3 0 xxxx 创业板证券
         * 7 xxxx 创业板增发
         * 8 xxxx 创业板权证
         * 3 9 xxxx 综合指数/成份指数         * 
         * 2、新旧证券代码转换此次A股证券代码升位方法为原代码前加“00”，但有两个A股股票升位方法特殊，分别是“0696 ST联益”和“0896 豫能控股”，升位后股票代码分别为“001696”和“001896”。
         * 股票代码中的临时代码和特殊符号临时代码新股：
         * 新股发行申购代码为730***，
         * 新股申购款代码为740***，
         * 新股配号代码为741***；
         * 新股配售代码为737***，
         * 新股配售的配号（又称“新股值号”）为747***；
         * 可转换债券发行申购代码为733***；
         */
        //目前只关注：综合指数sz399XXX sz395XXX  A股证券sz000XXX sz001XXX 中小板sz002XXX 创业板sz300XXX 国债逆回购sz131XXX 封闭式基金sz150XXX
         //临时代码： 新股发行申购代码为sz730XXX  

            string[] stock_sz = new string[] { "sz399XXX", "sz395XXX", "sz000XXX", "sz001XXX", "sz002XXX", "sz300XXX", "sz131XXX", "sz150XXX" };
           
            for (int i = 0; i < stock_sz.Length; i++)
        {
            CBMarketCode_BAK tmp = new CBMarketCode_BAK(stock_sz[i]);
            sCode_All.AddRange(tmp.sCodes);
        }        
        

        }
    }
}
