using System;
using System.Collections.Generic;
using System.Web;

namespace NG3.SDK
{
    public class Log
    {
        /// <summary>
        /// 日志写入
        /// </summary>
        /// <param name="appKey">应用标识(AppKey)</param>
        /// <param name="values">约定的JSon字符串</param>
        /// <returns></returns>
        public static string LogInPut(Builder builder, string values)
        {
            
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoPost(builder, new APIConfig().LogAPIServer + "api/Log", values);
        }
    }
}
