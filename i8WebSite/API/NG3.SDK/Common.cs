using System;
using System.Collections.Generic;
using System.Web;

namespace NG3.SDK
{
    public class Common
    {
       
        /// <summary>
        /// 更新数据库链接
        /// </summary>
        /// <param name="builder">应用标识(AppKey)</param>
        /// <param name="values">约定的JSon字符串</param>
        /// <returns></returns>
        public static string UpdateDbConn(Builder builder,string url, string values)
        {
            
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoPost(builder, url, values);
        }

        /// <summary>
        /// 获取数据库链接
        /// </summary>
        /// <param name="builder">应用标识(AppKey)</param>
        /// <returns></returns>
        public static string GetDbConn(Builder builder)
        {

            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoGet(builder, new APIConfig().I6PServer + "dbstring.ashx");
        }

        /// <summary>
        /// 更新数据库链接
        /// </summary>
        /// <param name="builder">应用标识(AppKey)</param>
        /// <param name="values">约定的JSon字符串</param>
        /// <returns></returns>
        public static string BIPMainEntry(Builder builder, string url, string values)
        {

            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoPost(builder, url, values);
        }
    }
}
