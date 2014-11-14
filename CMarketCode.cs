using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data;

namespace WebAlertSys
{
    /// <summary>
    /// 根据股票特征符，枚举所有可能的股票代码
    /// </summary>
    class CMarketCode
    {
        //代码左侧补零规则
        private static string[] label = new string[] { "{0:0}", "{0:00}", "{0:000}", "{0:0000}", "{0:00000}", "{0:000000}" };

        public List<string> sCode_All = new List<string>();

        public CMarketCode(string[] sCodePattern)
        {
            for (int i = 0; i < sCodePattern.Length; i++)
            {
                Debug.Assert(sCodePattern[i].Length == 8, "股票特征码错误");
                sCode_All.AddRange(this.EnumAllCodeBak(sCodePattern[i]));
            }
        }

        /// <summary>
        /// 根据股票特征字符串，枚举所有可能的股票代码
        /// </summary>
        /// <param name="sCodeType">8位的股票特征码，如sz000XXX</param>
        /// <returns>股票代码列表</returns>
        private List<string> EnumAllCodeBak(string sCodeType)
        {
            List<string> res = new List<string>();

            string head = sCodeType.TrimEnd('X');//前缀字符

            //解析特征码为备选代码集合
            int num = (sCodeType.Split('X')).Length - 1;
            int max = (int)Math.Pow(10.0, double.Parse(num.ToString()));
            for (int i = 1; i < max; i++)
            {
                string tmp = head + string.Format(label[num - 1], i);
                res.Add(tmp);
            }
            return res;
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
