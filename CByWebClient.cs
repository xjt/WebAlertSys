using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace WebAlertSys
{

    //webclient
    class CByWebClient
    {
        public string url { get; set; }
        public string encoding { get; set; }

        public string getbody(int length)
        {
            try
            {
                WebClient wb = new WebClient();
                Stream response = wb.OpenRead(url);
                StreamReader reader = new StreamReader(response, getEncodeing(encoding), true, 256000);
                char[] a = new char[length];
                int i = reader.Read(a, 0, length);
                reader.Close();
                return new string(a);
            }
            catch (Exception e)
            {
                return e.Message;
                //return null;  
            }
        }
        private Encoding getEncodeing(string str_encoding)
        {
            Encoding res;
            switch (str_encoding)
            {
                case "UTF-8":
                    res = Encoding.UTF8;
                    break;
                case "GB2312":
                    res = Encoding.GetEncoding("GB2312");
                    break;
                case "ASCII":
                    res = Encoding.ASCII;
                    break;
                default:
                    res = Encoding.GetEncoding(str_encoding);
                    break;
            }
            return res;
        }
    }

    class CByWebRequest
    {
        public static string getPage(String url)
        {
            WebResponse result = null;
            string resultstring = "";
            try
            {
                WebRequest req = WebRequest.Create(url);
                req.Timeout = 30000;
                result = req.GetResponse();
                Stream ReceiveStream = result.GetResponseStream();

                //read the stream into a string  
                //StreamReader sr = new StreamReader(ReceiveStream, System.Text.Encoding.UTF8);  
                string strEncod = result.ContentType;
                StreamReader sr;
                if (strEncod.ToLower().IndexOf("utf") != -1)
                {
                    sr = new StreamReader(ReceiveStream, System.Text.Encoding.UTF8);
                }
                else
                {
                    sr = new StreamReader(ReceiveStream, System.Text.Encoding.Default);
                }
                resultstring = sr.ReadToEnd();
                //js.alert(resultstring);
                Console.WriteLine(resultstring);
            }
            catch
            {
                throw new Exception();
            }
            finally
            {
                if (result != null)
                {
                    result.Close();
                }
            }
            return resultstring;
        }
    }

    class CByHttpRespone
    {
        public static string CheckTeamSiteUrl(string url)
        {
            string response = "";
            HttpWebResponse httpResponse = null;

            //assert: user have access to URL   
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Headers.Set("Pragma", "no-cache"); //不缓存
                

                // request.Headers.Set("KeepAlive", "true");  

                httpRequest.CookieContainer = new CookieContainer();

                httpRequest.Referer = url;

                httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";



                httpRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            }
            catch (Exception ex)
            {
                throw new ApplicationException("HTTP 403 Access denied, URL: " + url, ex);
            }

            //if here, the URL is correct and the user has access   
            try
            {
                string strEncod = httpResponse.ContentType;
                StreamReader stream;
                if (strEncod.ToLower().IndexOf("utf") != -1)
                {
                    stream = new StreamReader(httpResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                }
                else
                {
                    stream = new StreamReader(httpResponse.GetResponseStream(), System.Text.Encoding.Default);
                }

                char[] buff = new char[4000];
                stream.ReadBlock(buff, 0, 4000);
                response = new string(buff);
                stream.Close();
                httpResponse.Close();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("HTTP 404 Page not found, URL: " + url, ex);
            }
            return response;
        }
    }

}
