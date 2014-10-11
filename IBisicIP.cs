namespace WebAlertSys
{
    /// <summary>
    /// 抽象类，定义基础网站信息和方法
    /// </summary>
    public abstract class BisicIp
    {
    //   001×××国债现货；201×××国债回购；110×××120×××企业债券；129×××100×××可转换债券；310×××国债期货；500×××550×××基金；600×××A股；700×××配股；710×××转配股；701×××转配股再配股；711×××转配股再转配股；720×××红利；730×××新股申购；735×××新基金申购；900×××B股；737×××新股配售
        private readonly string[] _shHead = { "600" };//A股

        //0×××A股；1×××企业债券、国债回购、国债现货；2×××B股及B股权证；3×××转配股权证；4×××基金；5×××可转换债券；6×××国债期货；7×××期权；8×××配股权证；9×××新股配售 
        private readonly string[] _szHead = { "0" };//A股
        

        //http://stockhtm.finance.qq.com/sstock/ggcx/600401.shtml?pgv_ref=fi_tips
 

        /// <summary>
        /// 抽象方法：根据code和ip装配网址
        /// </summary>
        /// <param name="code"></param>
        /// <param name="ipUnfitted"></param>
        /// <returns></returns>
        public abstract string CreateIp(string code, string ipUnfitted);

        /// <summary>
        /// 返回带市场类型的股票代码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string AddMarketToCode(string code)
        {
            
            foreach (string t in _shHead)
            {
                if (code.StartsWith(t))
                {
                    return "sh" + code;
                }
            }

            foreach (string t in _szHead)
            {
                if(code.StartsWith(t))
                {
                    return "sz" + code;
                }
            }

            return code;
        }
    }

    internal class BisicIpImplSina : BisicIp
    {
        public override string CreateIp(string code, string ipUnfitted)
        {
            return string.Empty;
        }
    }

    public class BisicIpImplSohu : BisicIp
    {
        public override string CreateIp(string code, string ipUnfitted)
        {
            return string.Empty;
        }
    }
}