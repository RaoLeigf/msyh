using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Security;

namespace NG3.SDK
{
    internal class HttpClientHelp
    {
        private static int _timeout = 100000;

        /// <summary>
        /// 请求与响应的超时时间
        /// </summary>
        public static int Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }
        /// <summary>
        /// 给请求签名。
        /// </summary>
        /// <param name="parameters">所有字符型的请求参数</param>
        /// <param name="secret">签名密钥</param>
        /// <returns>签名</returns>
        public static string SignNGRequest(SecurityEntity entity)
        {
            var strparams = string.Empty;

            strparams += entity.TokenIdentity;
            strparams += entity.AppKey;
            strparams += entity.Format;
            strparams += entity.Method;
            strparams += entity.TimeStamp;
            strparams += entity.Version;
            strparams += entity.Secret;

            if (strparams.Length == 0)
                throw new Exception("当前请求的参数签名不正确！");
            return FormsAuthentication.HashPasswordForStoringInConfigFile(strparams.ToLower(), "md5").ToLower();
        }

        /// <summary>
        /// 执行HTTP POST请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>HTTP响应</returns>
        public static string DoPost(string url, IDictionary<string, string> parameters)
        {
            var req = GetWebRequest(url, "POST");
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            byte[] postData = Encoding.UTF8.GetBytes(BuildQuery(parameters));
            var reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            var rsp = (HttpWebResponse)req.GetResponse();
            var encoding = Encoding.GetEncoding(rsp.CharacterSet);
            return GetResponseAsString(rsp, encoding);
        }

        /// <summary>
        /// 执行HTTP POST请求。
        /// </summary>
        /// <param name="builder">SDK调用者信息</param>
        /// <param name="url">请求地址</param>
        /// <param name="values">请求业务参数</param>
        /// <returns>HTTP响应</returns>
        public static string Do(Builder builder, string url, string values,string method)
        {
            string msg=string.Empty;
            if (new APIConfig().Debug)
            {
                msg = "开始请求:" + url;
               // LogManager.Instance.Write(msg, true);
            }
            try
            {
                string secvalues = GetSecurityValues(builder);
                if (new APIConfig().Debug)
                {
                    msg = "系统参数:" + secvalues;
                  //  LogManager.Instance.Write(msg, true);

                    msg = "业务参数:" + values;
                   // LogManager.Instance.Write(msg, true);
                }
              
                var req = GetWebRequest(url, method);
                req.ContentType = "application/json";
                req.Headers.Add("SecurityKey", secvalues);
                
                //byte[] postData = Encoding.UTF8.GetBytes(BuildQuery(parameters));
                byte[] postData = Encoding.UTF8.GetBytes(values);
                var reqStream = req.GetRequestStream();
                reqStream.Write(postData, 0, postData.Length);
                reqStream.Close();

                var rsp = (HttpWebResponse)req.GetResponse();
                var encoding = Encoding.GetEncoding(rsp.CharacterSet);

                string rst = GetResponseAsString(rsp, encoding);
                
                if (new APIConfig().Debug)
                {
                    msg = "请求:" + url + "结束" + rst;
                   // LogManager.Instance.Write(msg, true);
                }

                return rst;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.Source;
               // LogManager.Instance.Write(msg, true);
                //LogManager.Instance.Write("请求失败:" + url, true);
                return msg;
            }
        }
        public static string DoPost(Builder builder, string url, string values)
        {
            return Do(builder, url, values, "POST");
        }

        public static string DoPut(Builder builder, string url, string values)
        {
            return Do(builder, url, values, "PUT");
        }

        /// <summary>
        /// 获取安全认证的字段和值
        /// </summary>
        /// <param name="builder">SDK调用者信息</param>
        /// <returns></returns>
        private static string GetSecurityValues(Builder builder)
        {
            string str=string.Empty;
            str = "{";
            str += "\"Join\":\"" + builder.SecurityEntity.Join.ToString() + "\"";
            str += ",";
            str += "\"AppKey\":\"" + builder.SecurityEntity.AppKey.ToString() + "\"";
            str += ",";
            str += "\"TokenIdentity\":\"" + builder.SecurityEntity.TokenIdentity.ToString() + "\"";
            str += ",";
            str += "\"Format\":\"" + builder.SecurityEntity.Format.ToString() + "\"";
            str += ",";
            str += "\"Method\":\"" + builder.SecurityEntity.Method.ToString() + "\"";
            str += ",";
            str += "\"TimeStamp\":\"" + builder.SecurityEntity.TimeStamp.ToString() + "\"";
            str += ",";
            str += "\"Version\":\"" + builder.SecurityEntity.Version.ToString() + "\"";
            str += ",";
            str += "\"Secret\":\"" + builder.SecurityEntity.Secret.ToString() + "\"";
            str += "}";
            return str;
        }

        /// <summary>
        /// 执行HTTP GET请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>HTTP响应</returns>
        public static string DoGet(string url, IDictionary<string, string> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                url = url.Contains("?") ? String.Format("{0}&{1}", url, BuildQuery(parameters)) : String.Format("{0}?{1}", url, BuildQuery(parameters));
            }
            string msg = string.Empty;
            if (new APIConfig().Debug)
            {
                msg = "开始请求:" + url;
                //LogManager.Instance.Write(msg, true);
            }
            try
            {
                var req = GetWebRequest(url, "GET");
                req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                
                var rsp = (HttpWebResponse)req.GetResponse();
                var encoding = Encoding.GetEncoding(rsp.CharacterSet);
                
                string rst = GetResponseAsString(rsp, encoding);
                if (new APIConfig().Debug)
                {
                    msg = "请求:" + url + "结束" + rst.Substring(0, 300);
                    //LogManager.Instance.Write(msg, true);
                }
                return rst;

            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.Source;
                //LogManager.Instance.Write(msg, true);
                //LogManager.Instance.Write("请求失败:" + url, true);
                return msg;
            }
        }

        public static string DoGet(Builder builder, string url)
        {
            string msg = string.Empty;
            if (new APIConfig().Debug)
            {
                msg = "开始请求:" + url;
                //LogManager.Instance.Write(msg, true);
            }
            try
            {

                var req = GetWebRequest(url, "GET");
                req.ContentType = "application/json";
                req.Headers.Add("SecurityKey", GetSecurityValues(builder));

                var rsp = (HttpWebResponse)req.GetResponse();
                var encoding = Encoding.GetEncoding(rsp.CharacterSet);
                
                string rst = GetResponseAsString(rsp, encoding);
                if (new APIConfig().Debug)
                {
                    msg = "请求:" + url + "结束.";
                    msg += rst.Length>300?rst.Substring(0, 300):rst;
                    //LogManager.Instance.Write(msg, true);
                }
                return rst;

            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.Source;
               // LogManager.Instance.Write(msg, true);
               // LogManager.Instance.Write("请求失败:" + url, true);
                return msg;
            }
        }

        

        public static HttpWebRequest GetWebRequest(string url, string method)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.ServicePoint.Expect100Continue = false;
            req.Method = method;
            req.KeepAlive = true;
            req.UserAgent = "NG4Net";
            req.Timeout = _timeout;
            
            return req;
        }/// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        public static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            var result = new StringBuilder();
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);

                // 按字符读取并写入字符串缓冲
                int ch = -1;
                while ((ch = reader.Read()) > -1)
                {
                    // 过滤结束符
                    char c = (char)ch;
                    if (String.Compare(c.ToString(), '\0'.ToString(), false) != 0)
                    {
                        result.Append(c);
                    }
                }
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }

            return result.ToString();
        }
        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        public static string BuildQuery(IDictionary<string, string> parameters)
        {
            var postData = new StringBuilder();
            bool hasParam = false;

            var dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }

                    postData.Append(name);
                    postData.Append("=");
                    postData.Append(Uri.EscapeDataString(value));
                    hasParam = true;
                }
            }

            return postData.ToString();
        }
    
    }
}
