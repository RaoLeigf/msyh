using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Security;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace NGWebSite
{
    public class MessageHandler : DelegatingHandler
    {
        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        { 
            //add by xumj
            //按照传递进来的key 去统一身份认证里面进行验证，如果验证失败直接拒绝请求
            string strSysparms = ((string[])(request.Headers.GetValues("SecurityKey")))[0];
            object tmp = JsonConvert.DeserializeObject(strSysparms, typeof(SecurityEntity));
            SecurityEntity entity = tmp as SecurityEntity;
            if (entity.Join)
            {
                string rst = GetTokenIdentityInfo(entity);
                if(rst.ToLower().Contains("err")){
                    //TODO:返回
                   Task.Factory.StartNew<HttpResponseMessage>(()=>{
                   return new HttpResponseMessage(HttpStatusCode.Forbidden);});                   
                }

                return base.SendAsync(request, cancellationToken);
            }
            else
            {
                return base.SendAsync(request, cancellationToken);
            }
        }

        private string GetTokenIdentityInfo(SecurityEntity entity) {
            SecurityEntity ent = new SecurityEntity();
            ent.AppKey = entity.AppKey;
            ent.Format = "xml";
            ent.Method = "ng.tokeninfo.get";
            ent.Secret = entity.Secret;
            ent.TimeStamp = entity.TimeStamp;
            ent.TokenIdentity = entity.TokenIdentity;
            ent.Version = "1.0";
            
            // 添加协议级请求参数
            Dictionary<string, string> reqParams = new Dictionary<string, string>();
            reqParams.Add("tokenidentity", ent.TokenIdentity);
            reqParams.Add("method", ent.Method);
            reqParams.Add("v", ent.Version);
            reqParams.Add("format", ent.Format);
            reqParams.Add("timestamp", ent.TimeStamp);
            reqParams.Add("appkey", ent.AppKey);
            reqParams.Add("sign", SignNGRequest(ent));

            string rst = DoPost(new APIConfig().SecurityServer, reqParams);
            return rst;
        }

        /// <summary>
        /// 给请求签名。
        /// </summary>
        /// <param name="parameters">所有字符型的请求参数</param>
        /// <param name="secret">签名密钥</param>
        /// <returns>签名</returns>
        internal static string SignNGRequest(SecurityEntity entity)
        {
            var strparams = string.Empty;

            strparams +=entity.TokenIdentity;
            strparams +=entity.AppKey;
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
        public string DoPost(string url, IDictionary<string, string> parameters)
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
        /// 执行HTTP GET请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>HTTP响应</returns>
        public string DoGet(string url, IDictionary<string, string> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                url = url.Contains("?") ? String.Format("{0}&{1}", url, BuildQuery(parameters)) : String.Format("{0}?{1}", url, BuildQuery(parameters));
            }

            var req = GetWebRequest(url, "GET");
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            var rsp = (HttpWebResponse)req.GetResponse();
            var encoding = Encoding.GetEncoding(rsp.CharacterSet);
            return GetResponseAsString(rsp, encoding);
        }

        public HttpWebRequest GetWebRequest(string url, string method)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.ServicePoint.Expect100Continue = false;
            req.Method = method;
            req.KeepAlive = true;
            req.UserAgent = "NG4Net";
            //req.Timeout = _timeout;
            return req;
        }

        /// <summary>
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

    /// <summary>
    /// 安全验证实体
    /// </summary>
    internal class SecurityEntity
    {
        /// <summary>
        /// 是否参与安全验证
        /// </summary>
        public bool Join
        {
            get { return join; }
            set { join = value; }
        }
        private bool join = true;

        /// <summary>
        /// 请求的应用程序
        /// </summary>
        public string AppKey
        {
            get { return appKey; }
            set { appKey = value; }
        }
        private string appKey = string.Empty;

        /// <summary>
        /// 令牌标识
        /// </summary>
        public string TokenIdentity
        {
            get { return tokenidentity; }
            set { tokenidentity = value; }
        }
        private string tokenidentity = string.Empty;

        /// <summary>
        /// 传递的参数的格式
        /// </summary>
        public string Format
        {
            get { return format; }
            set { format = value; }
        }
        private string format = string.Empty;

        /// <summary>
        /// 请求提交方式
        /// </summary>
        public string Method
        {
            get { return method; }
            set { method = value; }
        }
        private string method = string.Empty;

        /// <summary>
        /// 请求时间
        /// </summary>
        public string TimeStamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }
        private string timestamp = string.Empty;

        /// <summary>
        /// SDK版本
        /// </summary>
        public string Version
        {
            get { return version; }
            set { version = value; }
        }
        private string version = string.Empty;

        /// <summary>
        /// 应用密钥
        /// </summary>
        public string Secret
        {
            get { return secret; }
            set { secret = value; }
        }
        private string secret = string.Empty;

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return productname; }
            set { productname = value; }
        }
        private string productname = string.Empty;

        /// <summary>
        /// 当前账套
        /// </summary>
        public string Account
        {
            get { return account; }
            set { account = value; }
        }
        private string account = string.Empty;

        /// <summary>
        /// 当前登录的UserID
        /// </summary>
        public string UserID
        {
            get { return userid; }
            set { userid = value; }
        }
        private string userid = string.Empty;

        /// <summary>
        /// 当前登录的组织
        /// </summary>
        public string Org
        {
            get { return org; }
            set { org = value; }
        }
        private string org = string.Empty; 
    }

}