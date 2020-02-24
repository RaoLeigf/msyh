using System;
using System.Collections.Generic;
using System.Web;

namespace NG3.SDK
{
    public class Rights
    {
        /// <summary>
        /// 按操作员获取指定账套组织下的页面按钮权限集
        /// </summary>
        /// <param name="builder">系统参数</param>
        /// <param name="ucode">账套</param>
        /// <param name="ocode">组织</param>
        /// <param name="loginid">操作员</param>
        /// <param name="pagename">页面</param>
        /// <param name="funcname">功能</param>
        /// <returns></returns>
        public static string GetPageButtons(Builder builder, string ucode, string ocode, string loginid, string pagename, string funcname)
        {
            builder.SecurityEntity.Join = true;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";
           string str = string.Format("ucode={0}&ocode={1}&loginid={2}&pagename={3}&funcname={4}", ucode, ocode, loginid, pagename, funcname);
           return HttpClientHelp.DoGet(builder, new APIConfig().I6PServer + "PageButtonsRights.ashx?" + str);
           //return HttpClientHelp.DoGet(builder, "http://localhost/wkf/PageButtonsRights.ashx?" + str);
        }

        /// <summary>
        /// 判断页面是否有入口权限
        /// </summary>
        /// <param name="builder">系统参数</param>
        /// <param name="ocode">组织</param>
        /// <param name="loginid">操作员</param>
        /// <param name="pagename">页面</param>
        /// <param name="funcname">功能</param>
        /// <returns></returns>
        public static string IsHaveRight(Builder builder, string ucode, string ocode, string loginid, string rightname, string funcname)
        {
            builder.SecurityEntity.Join = true;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";
            string str = string.Format("ucode={0}&ocode={1}&loginid={2}&pagename={3}&funcname={4}",ucode,ocode, loginid, rightname, funcname);
            return HttpClientHelp.DoGet(builder, new APIConfig().I6PServer + "PageRights.ashx?" + str);
            //return HttpClientHelp.DoGet(builder, "http://localhost/wkf/PageButtonsRights.ashx?" + str);
        }
        
        /// <summary>
        /// 信息域
        /// </summary>
        /// <param name="builder">系统参数</param>
        /// <param name="values">{"UserId":"","Ocode":"","TableName":""}</param>
        /// <returns></returns>
        public static string LimitgetPerporty(Builder builder, string values)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoPost(builder, new APIConfig().W3APIServer + "integrationapi/LimitgetPerporty", values);
        }
        
        /// <summary>
        /// 信息域
        /// </summary>
        /// <param name="builder">系统参数</param>
        /// <param name="values">{"UserId":"","Ocode":"","LimitSQL":""}</param>
        /// <returns></returns>
        public static string LimitSql(Builder builder, string values)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoPost(builder, new APIConfig().W3APIServer + "integrationapi/limitsql", values);
        }
        
        /// <summary>
        /// 信息域
        /// </summary>
        /// <param name="builder">系统参数</param>
        /// <param name="values">{"TableName": "" ,"UserId":"","Ocode":"","LimitSQL":""}</param>
        /// <returns></returns>
        public static string LimitFilter(Builder builder, string values)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoPost(builder, new APIConfig().W3APIServer + "integrationapi/limitfilter", values);
        }
    }
}
