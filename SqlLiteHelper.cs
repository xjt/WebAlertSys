using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.Diagnostics;
using System.Data.Common;

namespace WebAlertSys
{

    public class SQLiteHelper
    {
       /// <summary>
       /// 数据库文件路径
       /// </summary>
       public static string ConnSqlLiteDbPath = string.Empty;

       /// <summary>
       /// 数据库链接字符串
       /// </summary>    
       public static string ConnString
       {
           get
           {
               return string.Format(@"Data Source={0}", ConnSqlLiteDbPath);
           }
       } 

        /// <summary>
        /// 执行一条查询指令，结果返回DataTable
        /// </summary>
        /// <param name="sError">输出的错误信息；成功返回空</param>
        /// <param name="sSQL">查询指令</param>
        /// <returns>数据表</returns>
       public static DataTable GetDataTable(out string sError,string sSQL)
       {
           DataTable dt = null;
           sError = string.Empty;

           try
           {
               SQLiteConnection conn = new SQLiteConnection(ConnString);
               conn.Open();
               SQLiteCommand cmd = new SQLiteCommand();
               cmd.CommandText = sSQL;
               cmd.Connection = conn;
               SQLiteDataAdapter dao = new SQLiteDataAdapter(cmd);
               dt = new DataTable();
               dao.Fill(dt);
           }
           catch (Exception ex)
           {
               sError = ex.Message;
           } 

           return dt;
       }
         
       /// <summary>
       /// 根据查询结果，取出首行首列的元素
       /// </summary>
       /// <param name="sError"></param>
       /// <param name="sSQL"></param>
       /// <returns></returns>
       public static object GetSingle(out string sError, string sSQL,int nRowID=0, int nColumnID=0)
       {
           DataTable dt = GetDataTable(out sError, sSQL);
           Debug.Assert(nRowID < dt.Rows.Count && nColumnID < dt.Columns.Count,"输入的行或者列的索引号过界！");
           if (dt != null && dt.Rows.Count > 0)
           {
               return dt.Rows[nRowID][nColumnID];
           } 

           return null;
       }
        
       /// <summary>
       /// 取出表中某个字段的极大值对象
       /// </summary>
       /// <param name="sError"></param>
       /// <param name="sKeyField">字段</param>
       /// <param name="sTableName">表</param>
       /// <returns>最大值</returns>
       public static Int32 GetMaxID(out string sError, string sKeyField,string sTableName)
       {

           DataTable dt = GetDataTable(out sError, "select ifnull(max([" + sKeyField + "]),0) as MaxID from [" + sTableName + "]");
           if (dt != null && dt.Rows.Count > 0)
           {
               return Convert.ToInt32(dt.Rows[0][0].ToString());
           }

           return 0;
       }
         
       /// <summary>
       /// 执行insert,update,delete 动作，也可以使用事务(错误后回滚)
       /// </summary>
       /// <param name="sError"></param>
       /// <param name="sSQL"></param>
       /// <param name="bUseTransaction"></param>
       /// <returns></returns>
       public static bool UpdateData(out string sError, string sSQL,bool bUseTransaction=false)
       {
           int iResult = 0;
           sError = string.Empty;

           if (!bUseTransaction)
           {
               try
               {
                   SQLiteConnection conn = new SQLiteConnection(ConnString);
                   conn.Open();
                   SQLiteCommand comm = new SQLiteCommand(conn);
                   comm.CommandText = sSQL;
                   iResult = comm.ExecuteNonQuery();
               }
               catch (Exception ex)
               {
                   sError = ex.Message;
                   iResult = -1;
               }
           }
           else // 使用事务
           {
               DbTransaction trans =null;
               try
               {
                   SQLiteConnection conn = new SQLiteConnection(ConnString);
                   conn.Open();
                   trans = conn.BeginTransaction();
                   SQLiteCommand comm = new SQLiteCommand(conn);
                   comm.CommandText = sSQL;
                   iResult = comm.ExecuteNonQuery();
                   trans.Commit();
               }
               catch (Exception ex)
               {
                   sError = ex.Message;
                   iResult = -1;
                   trans.Rollback();
               }
           } 

           return iResult >0;
       }
        
    }
}
