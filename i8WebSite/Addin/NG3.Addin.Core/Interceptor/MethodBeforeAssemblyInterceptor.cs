using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AopAlliance.Intercept;
using System.Reflection;
using System.Collections.Specialized;

namespace NG3.Addin.Core.Interceptor
{
    public class MethodBeforeAssemblyInterceptor : AbstractMethodBeforeInterceptor
    {
        public override bool Before(IMethodInvocation invocation)
        {
            try
            {
                foreach (var item in ConfigureEntity.AssemblyModels)
                {
                    var assembly = Assembly.Load(item.AssemblyName);
                    var clazz = assembly.GetType(item.ClassName);

                    //var obj = (AbstractMethodBeforeInterceptor)Activator.CreateInstance(clazz);
                    // bool rtn = obj.Before(invocation);

                    //使用新封装的接口，所有的参数都是通过request取
                    var obj = (AbstractAssemblyInterceptorAction)Activator.CreateInstance(clazz);

                    //初始化插件参数
                    NameValueCollection nvcols = System.Web.HttpContext.Current.Request.Params;

                    obj.Parameters = nvcols;

                    //初始化Dbheper
                    var trans = ((NHibernate.Transaction.AdoTransaction)Session.Transaction).NGTrans;
                    NG3.Data.Service.ConnectionInfoService.InitialDbHelper(trans);

                    //执行插件
                    bool rtn = obj.Execut();

                    //成功还是失败记录日志

                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
