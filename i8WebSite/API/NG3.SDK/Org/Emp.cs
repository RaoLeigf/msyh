using System;
using System.Collections.Generic;
using System.Web;

namespace NG3.SDK
{
    public class Emp
    {
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="builder">系统参数</param>
        /// <param name="pagenum">页面数</param>
        /// <param name="itemcount">一页行数</param>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public static string GetAllUsers(Builder builder, string pagenum, string itemcount, string where)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            SecurityEntity ent = new SecurityEntity();
            ent.AppKey = builder.SecurityEntity.AppKey;
            ent.Format = "xml";
            ent.Method = "ng.usermanage.getlistbypage";
            ent.Secret = builder.SecurityEntity.Secret;
            ent.TimeStamp = builder.SecurityEntity.TimeStamp;
            ent.TokenIdentity = builder.SecurityEntity.TokenIdentity;
            ent.Version = "1.0";

            // 添加协议级请求参数
            Dictionary<string, string> reqParams = new Dictionary<string, string>();
            reqParams.Add("tokenidentity", ent.TokenIdentity);
            reqParams.Add("method", ent.Method);
            reqParams.Add("v", ent.Version);
            reqParams.Add("format", ent.Format);
            reqParams.Add("timestamp", ent.TimeStamp);
            reqParams.Add("appkey", ent.AppKey);
            reqParams.Add("sign",HttpClientHelp.SignNGRequest(ent));
            reqParams.Add("pagenum", pagenum);
            reqParams.Add("itemcount", itemcount);
            reqParams.Add("where", where);

            string rst = HttpClientHelp.DoPost(new APIConfig().SSOServer, reqParams);

            return rst;
        }

        /// <summary>
        /// 获取指定账套下的所有用户
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="values">账套号{"unitID":"$"}</param>
        /// <returns></returns>
        public static string UsersByUnit(Builder builder, string values)
        {

            builder.SecurityEntity.Join = true;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            string rst = HttpClientHelp.DoPost(builder, new APIConfig().W3APIServer + "integrationapi/UsersByUnit", values);

            return rst;
        }
    }
}
