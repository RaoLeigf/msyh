using System;
using System.Collections.Generic;
using System.Web;

namespace NG3.SDK
{
    public class Account
    {
        /// <summary>
        /// 根据操作员ID获取该操作员对应的所有账套
        /// </summary>
        /// <param name="appKey">应用标识(AppKey)</param>
        /// <param name="values">约定的JSon字符串</param>
        /// <returns></returns>
        public static string AllUnits(Builder builder, string values)
        {

            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoPost(builder, new APIConfig().W3APIServer + "integrationapi/AllUnits", values);
        }

        /// <summary>
        /// 获取数据库连接信息
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static string GetDBString(Builder builder)
        {

            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoGet(builder, new APIConfig().I6PServer + "DBString.ashx");
        }
    }
}
