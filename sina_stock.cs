using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAlertSys
{
    public struct real_data
    {
        public string code;
        public string 股票名;
        public float 开盘价;
        public float 昨收盘价;
        public float 当前价;
        public float 最高价;
        public float 最低价;
        public float 买入价;
        public float 卖出价;
        public float 成交量;//单位手；
        public float 成交额;//单位"万元"
        public float 买一量;//手；
        public float 买一价;//"买一"报价；
        public float 买二量;//手；
        public float 买二价;//"买一"报价；
        public float 买三量;//手；
        public float 买三价;//"买一"报价；
        public float 买四量;//手；
        public float 买四价;//"买一"报价；
        public float 买五量;//手；
        public float 买五价;//"买一"报价；
        public float 卖一量;
        public float 卖一价;
        public float 卖二量;
        public float 卖二价;
        public float 卖三量;
        public float 卖三价;
        public float 卖四量;
        public float 卖四价;
        public float 卖五量;
        public float 卖五价;
        public DateTime 时间;
    }
    class sina_stock
    {
        public List<real_data> data_array = new List<real_data>();

        public sina_stock(List<String> codes)
        {
            int num = codes.Count;
            //组装url,参考 http://www.xuebuyuan.com/333713.html
            string url_sina = "http://hq.sinajs.cn/list=";
            foreach(string code in codes)
            {
                url_sina += code+ ((--num!=0)?",":"");             
            }

            //web请求
            string info = CByWebRequest.getPage(url_sina);
            /*
            //方法一：
            CByWebClient myweb = new CByWebClient();
            myweb.url = url_sina;
            myweb.encoding = "GB2312";
            string content = myweb.getbody(1024000);

            //方法二：
            string content2 = CByWebRequest.getPage(url_sina);//完整数据

            //方法三：
            string content3 = CByHttpRespone.CheckTeamSiteUrl(url_sina);
             */

            //解析
            convert_msg(codes, info);

        }

        private void convert_msg(List<String> codes,string info)
        {
            data_array.Clear();

            string[] data_lines = info.Split("\r\n".ToCharArray());

            for (int i = 0; i < data_lines.Length; i++)
            {
                if (data_lines[i].Length == 0)
                    continue;
             
                real_data this_stock = new real_data();
                this_stock.code = codes[i]; //代码                      
   
                string[] msg = data_lines[i].Split(',');//解析股票名
                string[] name = msg[0].Split('"');
                msg[0] = name[1];
                if (msg[0] != "")//股票名可能无法解析
                {                    
                    this_stock.股票名 = msg[0];
                    this_stock.开盘价 = float.Parse(msg[1]);
                    this_stock.昨收盘价 = float.Parse(msg[2]);
                    this_stock.当前价 = float.Parse(msg[3]);
                    this_stock.最高价 = float.Parse(msg[4]);
                    this_stock.最低价 = float.Parse(msg[5]);
                    this_stock.买入价 = float.Parse(msg[6]);
                    this_stock.卖出价 = float.Parse(msg[7]);
                    this_stock.成交量 = float.Parse(msg[8]);
                    this_stock.成交额 = float.Parse(msg[9]);
                    this_stock.买一量 = float.Parse(msg[10]);
                    this_stock.买一价 = float.Parse(msg[11]);
                    this_stock.买二量 = float.Parse(msg[12]);
                    this_stock.买二价 = float.Parse(msg[13]);
                    this_stock.买三量 = float.Parse(msg[14]);
                    this_stock.买三价 = float.Parse(msg[15]);
                    this_stock.买四量 = float.Parse(msg[16]);
                    this_stock.买四价 = float.Parse(msg[17]);
                    this_stock.买五量 = float.Parse(msg[18]);
                    this_stock.买五价 = float.Parse(msg[19]);
                    this_stock.卖一量 = float.Parse(msg[20]);
                    this_stock.卖一价 = float.Parse(msg[21]);
                    this_stock.卖二量 = float.Parse(msg[22]);
                    this_stock.卖二价 = float.Parse(msg[23]);
                    this_stock.卖三量 = float.Parse(msg[24]);
                    this_stock.卖三价 = float.Parse(msg[25]);
                    this_stock.卖四量 = float.Parse(msg[26]);
                    this_stock.卖四价 = float.Parse(msg[27]);
                    this_stock.卖五量 = float.Parse(msg[28]);
                    this_stock.卖五价 = float.Parse(msg[29]);
                    string[] tm = msg[31].Split(':');
                    this_stock.时间 = DateTime.Parse(msg[30]).AddHours(double.Parse(tm[0])).AddMinutes(double.Parse(tm[1])).AddSeconds(double.Parse(tm[2]));
                }

                data_array.Add(this_stock);
            }

        }

    }

}
