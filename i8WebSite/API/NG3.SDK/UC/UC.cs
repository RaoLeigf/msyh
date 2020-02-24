using System;
using System.Collections.Generic;
using System.Web;

namespace NG3.SDK
{
    public class UC
    {
        

        /// <summary>
        /// UC平台Rest服务调用
        /// </summary>
        /// <param name="builder">系统参数</param>       
        /// <param name="values">见文档参数说明</param>
        /// <returns></returns>
        public static string Send(Builder builder, string values)
        {
            builder.SecurityEntity.Join = true;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoPost(builder, new APIConfig().I6PServer + "SendMsg", values);
        }

       
    }
}
