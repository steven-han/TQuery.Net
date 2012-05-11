/**  功能：采集操作类
 *   作者：Steven_Han 
 *   MSN： Steven_Han@live.com
**/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;

namespace TQuery.Net
{
    public class GatherHelper
    {
        /// <summary>
        /// 获取远程页面代码（如果此方法获取不到请用GetStringByUrl）
        /// </summary>
        /// <param name="Url">页面地址</param>
        /// <param name="Charset">编码</param>
        /// <returns></returns>
        public static string GetHtmlSource(string Url, Encoding Charset)
        {
            string html = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream Stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(Stream, Charset);
                html = reader.ReadToEnd();
                Stream.Close();
            }
            catch
            {
                return null;
            }
            return html;
        }

        /// <summary>
        /// 获取远程页面代码
        /// </summary>
        /// <param name="Url">页面地址</param>
        /// <param name="Charset">编码</param>
        /// <returns></returns>
        public static string GetStringByUrl(string Url, Encoding Charset)
        {
            if (Url.Equals("about:blank")) return null; ;
            if (!Url.StartsWith("http://") && !Url.StartsWith("https://")) { Url = "http://" + Url; }
            int dialCount = 0;
        loop:
            StreamReader sreader = null;
            string html = string.Empty;
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
                httpWebRequest.UserAgent = "User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
                httpWebRequest.Accept = "*/*";
                httpWebRequest.KeepAlive = true;
                httpWebRequest.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    sreader = new StreamReader(httpWebResponse.GetResponseStream(), Charset);
                    char[] cCont = new char[256];
                    int count = sreader.Read(cCont, 0, 256);
                    while (count > 0)
                    {
                        String str = new String(cCont, 0, count);
                        html += str;
                        count = sreader.Read(cCont, 0, 256);
                    }
                }
                if (null != httpWebResponse) { httpWebResponse.Close(); }
                return html;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ConnectFailure) { dialCount++; }
                if (dialCount < 5) { goto loop; }
                return null;
            }
            finally { if (sreader != null) { sreader.Close(); } }
        }

        /// <summary>
        /// 提取代码（如果有多条返回一条）
        /// </summary>
        /// <param name="Code">原代码</param>
        /// <param name="WordsBegin">开始标记</param>
        /// <param name="WordsEnd">结束标记</param>
        /// <returns></returns>
        public static string SniffString(string Code, string WordsBegin, string WordsEnd)
        {
            string _return = string.Empty;
            Regex regex = new Regex("" + WordsBegin + @"(?<title>[\s\S]+?)" + WordsEnd + "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            for (Match match = regex.Match(Code); match.Success; match = match.NextMatch())
            {
                _return = match.Groups["title"].ToString();
            }
            return _return;
        }

        /// <summary>
        /// 提取代码（适用多条返回列表）
        /// </summary>
        /// <param name="Code">原代码</param>
        /// <param name="WordsBegin">开始标记</param>
        /// <param name="WordsEnd">结束标记</param>
        /// <returns></returns>
        public static IList<KeyValuePair<int, string>> SniffStringList(string Code, string WordsBegin, string WordsEnd)
        {
            IList<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
            MatchCollection mc = Regex.Matches(Code, "" + WordsBegin + @"(?<title>[\s\S]+?)" + WordsEnd + "");
            int i = 0;
            foreach (Match m in mc)
            {
                list.Add(new KeyValuePair<int, string>(i, m.Groups["title"].Value));
                i++;
            }
            return list;
        }

        /// <summary>
        /// 提取代码（适用多条返回第几条）
        /// </summary>
        /// <param name="Code">原代码</param>
        /// <param name="WordsBegin">开始标记</param>
        /// <param name="WordsEnd">结束标记</param>
        /// <param name="Number">第几条</param>
        /// <returns></returns>
        public static string SniffString(string Code, string WordsBegin, string WordsEnd, int Number)
        {
            IList<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
            MatchCollection mc = Regex.Matches(Code, "" + WordsBegin + @"(?<title>[\s\S]+?)" + WordsEnd + "");
            int i = 0;
            foreach (Match m in mc)
            {
                list.Add(new KeyValuePair<int, string>(i, m.Groups["title"].Value));
                i++;
            }
            return list.Where(L => L.Key == Number).SingleOrDefault().Value;
        }

        /// <summary>
        /// 过滤HTML
        /// </summary>
        /// <param name="html">原HTML</param>
        /// <returns></returns>
        public static string CheckString(string html)
        {
            Regex regex = new Regex(@"<(.|\n)+?>");
            html = regex.Replace(html, "");
            html = html.Replace("&nbsp;", "");
            return html;
        }

        /// <summary>
        /// 过滤换行符
        /// </summary>
        /// <param name="html">原HTML</param>
        /// <returns></returns>
        public static string CleanString(string html)
        {
            string tempStr = html.Replace((char)13, (char)0);
            return tempStr.Replace((char)10, (char)0);
        }

        /// <summary>
        /// 取所有URL
        /// </summary>
        /// <param name="html">原HTML</param>
        /// <returns></returns>
        public static IList<KeyValuePair<string, string>> FetchUrl(string html)
        {
            IList<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            string pattern = @"<a\s*href=(""|')(?<href>[\s\S.]*?)(""|').*?>\s*(?<name>[\s\S.]*?)</a>";
            MatchCollection mc = Regex.Matches(html, pattern);
            foreach (Match m in mc)
            {
                list.Add(new KeyValuePair<string, string>(m.Groups["href"].Value, m.Groups["name"].Value));
            }
            return list;
        }
    }
}
