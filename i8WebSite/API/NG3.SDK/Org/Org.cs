using System;
using System.Collections.Generic;
using System.Web;

namespace NG3.SDK
{
    public class Org
    {
        /// <summary>
        /// 按操作员获取指定账套下的有权限的所有下级组织信息
        /// </summary>
        /// <param name="builder">系统参数</param>
        /// <param name="values">约定的JSon字符串</param>
        /// <returns></returns>
        public static string AllOrgs(Builder builder, string values)
        {
            builder.SecurityEntity.Join = true;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoPost(builder, new APIConfig().W3APIServer + "integrationapi/AllOrgs", values);
        }
        
        /// <summary>
        /// 获取指定帐套下的组织关系
        /// </summary>
        /// <param name="builder">系统参数</param>
        /// <param name="values">约定的JSon字符串</param>
        /// <returns></returns>
        public static string GetOrgRelate(Builder builder, string values)
        {
            builder.SecurityEntity.Join = true;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoPost(builder, new APIConfig().W3APIServer + "integrationapi/orgrelate", values);
        }
        
        /// <summary>
        /// 根据指定帐套号以及组织关系代码获取指定组织数据
        /// </summary>
        /// <param name="builder">系统参数</param>
        /// <param name="values">约定的JSon字符串</param>
        /// <returns></returns>
        public static string GetOrgTree(Builder builder, string values)
        {
            builder.SecurityEntity.Join = true;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoPost(builder, new APIConfig().W3APIServer + "integrationapi/orgTree", values);
        }
        
        /// <summary>
        /// 根据指定操作员组织数据
        /// </summary>
        /// <param name="builder">系统参数</param>
        /// <param name="values">约定的JSon字符串</param>
        /// <returns></returns>
        public static string GetUserBlongOrg(Builder builder, string values)
        {
            builder.SecurityEntity.Join = true;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoPost(builder, new APIConfig().W3APIServer + "integrationapi/UserBlongOrg", values);
        }

        /// <summary>
        /// 获取注册的套件的IP
        /// </summary>
        /// <param name="builder">系统参数</param>
        /// <param name="suite">套件</param>
        /// <returns></returns>
        public static string GetSuiteIP(Builder builder, string suite)
        {
            builder.SecurityEntity.Join = true;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";
            
            string str = string.Format("suite={0}", suite);
            return HttpClientHelp.DoGet(builder, new APIConfig().I6PServer + "SuiteIP.ashx?" + str);
        }

        /// <summary>
        /// 获取注册的套件
        /// </summary>
        /// <param name="builder">系统参数</param>
        /// <returns></returns>
        public static string GetSuites(Builder builder)
        {
            builder.SecurityEntity.Join = true;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoGet(builder, new APIConfig().I6PServer + "SuiteList.ashx" );
        }

        
        public static string GetDept(Builder builder)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoGet(builder, new APIConfig().LogAPIServer + "api/Dept/5");
        }
    }
}
