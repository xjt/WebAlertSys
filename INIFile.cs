using System.IO;
using System.Runtime.InteropServices;
using System.Text;


namespace WebAlertSys
{

    /// <summary>
    /// 负责配置文件的读写
    /// 默认配置文件放在执行文件的同级目录下
    /// </summary>
    /// <remarks>公共基础类</remarks>
    public class INIClass
    {
        private string inifilename;

        //调用kernel32动态库函数
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);
        

        /// <summary>
        /// 构造函数，传入ini配置文件名
        /// </summary>
        public INIClass(string sFileName_INI)
        {
            this.inifilename = sFileName_INI;
        
        }          
      
        /// <summary>
        ///  执行程序路径，含"\"。
        /// </summary>
        private string installDirectory = null;
        private string InstallDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(this.installDirectory))
                {
                    string sPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    this.installDirectory = Path.GetDirectoryName(sPath) + @"\";
                }
                return this.installDirectory;
            }
        }

        /// <summary>
        /// 配置文件全路径（在执行程序的同级目录）
        /// </summary>   
        private string iniFileName_All = null;
        private string IniFileName_All
        {
            get
            {
                if (string.IsNullOrEmpty(this.iniFileName_All))
                {
                    this.iniFileName_All = this.InstallDirectory + this.inifilename;
                }

                return this.iniFileName_All;
            }
        }


        /// <summary>
        /// 从Ini文件获取数据
        /// </summary>
        /// <param name="section">应用程序</param>
        /// <param name="key">键的名称</param>
        /// <returns>键的值</returns>
        public string IniReadValue(string section, string key)
        {
            int nCapacity = 255;
            StringBuilder temp = new StringBuilder(nCapacity);
            int i = GetPrivateProfileString(section, key, "", temp, nCapacity, this.IniFileName_All);

            if (i < 0)
                return "";

            return temp.ToString();
        }


        /// <summary>
        /// 向Ini文件中写入值
        /// </summary>
        /// <param name="section">应用程序</param>
        /// <param name="key">键的名称</param>
        /// <param name="value">键的值</param>
        /// <returns>执行成功为True，失败为False。</returns>
        public bool IniWriteValue(string section, string key, string value)

        {
            if (section.Trim().Length <= 0 || key.Trim().Length <= 0 || value.Trim().Length <= 0)
                return false;

            return (0!=WritePrivateProfileString(section, key, value, this.IniFileName_All));
        }

    

    }

}
