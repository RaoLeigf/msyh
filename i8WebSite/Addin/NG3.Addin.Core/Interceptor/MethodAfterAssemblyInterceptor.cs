using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AopAlliance.Intercept;
using System.Reflection;
using System.Collections.Specialized;
using SUP.Common.Base;

namespace NG3.Addin.Core.Interceptor
{
    class MethodAfterAssemblyInterceptor : AbstractMethodAfterInterceptor
    {
        public  override bool After(object returnObject, IMethodInvocation invocation)
        {
            try
            {
                              
                //循环执行多个插件
                foreach (var item in ConfigureEntity.AssemblyModels)
                {
                    var assembly = Assembly.Load(item.AssemblyName);
                    var clazz = assembly.GetType(item.ClassName);

                    //插件直接继承抽象类
                    //var obj = (AbstractMethodAfterInterceptor)Activator.CreateInstance(clazz);
                    //bool rtn = obj.After(returnObject, invocation);


                    //使用新封装的接口，所有的参数都是通过request取
                    var obj = (AbstractAssemblyInterceptorAction)Activator.CreateInstance(clazz);

                    //初始化插件参数
                    NameValueCollection nvcols = System.Web.HttpContext.Current.Request.Params;

                    //增加结果集参数
                    string methodResult = DataConverterHelper.SerializeObject(returnObject);
                    nvcols.Add("methodResult", methodResult);

                    obj.Parameters = nvcols;

                    //初始化Dbheper
                    var trans = ((NHibernate.Transaction.AdoTransaction)Session.Transaction).NGTrans;
                    NG3.Data.Service.ConnectionInfoService.InitialDbHelper(trans);

                    //执行插件
                    bool rtn = obj.Execut();

                    //记录日志，执行成功还是失败

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
