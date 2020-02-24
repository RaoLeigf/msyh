using NG3.Addin.Core.Interceptor;
using NG3.Addin.Model.Domain.BusinessModel;
using NG3.Addin.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Cfg
{
    public class AddinEnvironment
    {
        /// <summary>
        /// 取得请求的参数，不包含服务端参灵敏
        /// </summary>
        /// <returns></returns>
        public static NameValueCollection RequestParams()
        {
            NameValueCollection requestParams = new NameValueCollection();

            NameValueCollection nvs = System.Web.HttpContext.Current.Request.Params;
            string[] serverVarKeys = System.Web.HttpContext.Current.Request.ServerVariables.AllKeys;

            foreach (var item in nvs.AllKeys)
            { 
                if (Array.IndexOf(serverVarKeys, item) > 0) continue;
                requestParams.Add(item, nvs[item]);
            }
            return requestParams;
        }

        public static void SaveServiceRequestParam(AddinMethodInvocation invocation)
        {
            var requestParams = RequestParams();
            string className = invocation.Target.ToString();
            string method = invocation.Method.Name;

            AddinConfigureEntityKey entityKey = new AddinConfigureEntityKey(className, method, EnumInterceptorType.None);

            foreach (string param in requestParams)
            {
                //key
                string key = entityKey.GetKey() + "_" + param;
                ServiceUIParamBizModel model = new ServiceUIParamBizModel { ClassName = className, MethodName = method, ParamName = param, ParamValue = requestParams[param] };
                AddinCache.AddUIParam(key, model);
            }

        }


        /// <summary>
        /// 当前已经运行过的参数集合
        /// </summary>
        /// <returns></returns>
        public static IList<ServiceUIParamBizModel> GetServiceUIParams()
        {
            return AddinCache.GetUIParams();
        }




    }
}
