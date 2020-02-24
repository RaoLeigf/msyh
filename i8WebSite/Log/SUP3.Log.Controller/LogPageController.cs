using Enterprise3.NHORM.Controller;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NG3.Log.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SUP3.Log.Controller
{
    /// <summary>
	/// LogPage控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class LogPageController: AFCommonController
    {
        /// <summary>
        /// 将
        /// </summary>
        public void  PushPageLog()
        {
            try
            {
                string logData = System.Web.HttpContext.Current.Request.Form["data"];

                if (string.IsNullOrEmpty(logData)) return;

                //将LogData进行JSON串解析 
                NameValueCollection times = new NameValueCollection();
                JObject obj = JsonConvert.DeserializeObject(logData) as JObject;
                if (obj == null) return;
                //去除掉
                string url = obj.Property("url").Value.ToString();
                if(!string.IsNullOrEmpty(url) && url.IndexOf("?")>0)
                {
                    url = url.Substring(0, url.IndexOf("?"));
                }
                times.Add("url", url);
                times.Add("requesttime", obj.Property("requestTime").Value.ToString());
                times.Add("commonjstime", obj.Property("commonJsTime").Value.ToString());
                times.Add("loadpagetime", obj.Property("loadPageTime").Value.ToString());
                times.Add("tcptime", obj.Property("tcpTime").Value.ToString());
                times.Add("lookupdnstime", obj.Property("lookupDnsTime").Value.ToString());
                times.Add("ttfb", obj.Property("ttfb").Value.ToString());
                times.Add("redirecttime", obj.Property("redirectTime").Value.ToString());
                //存入到数据库中
                NG3LoggerManager.LogPage(times);
            }
            catch (Exception)
            {

                
            }
                
        }
    }
}
