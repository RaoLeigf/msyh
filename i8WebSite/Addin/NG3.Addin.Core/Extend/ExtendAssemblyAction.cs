using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Extend
{
    class ExtendAssemblyAction : AbstractExtendAction
    {
        public override string Execute()
        {

            string rtn = string.Empty;

            foreach (var item in ConfigureEntity.AssemblyModels)
            {
                var assembly = Assembly.Load(item.AssemblyName);
                var clazz = assembly.GetType(item.ClassName);

                //var obj = (AbstractExtendAction)Activator.CreateInstance(clazz);
                //执行
                // rtn = obj.Execute();

                //使用新封装的接口，所有的参数都是通过request取
                var obj = (AbstractExtendAssemblyAction)Activator.CreateInstance(clazz);

                //初始化插件参数
                NameValueCollection nvcols = System.Web.HttpContext.Current.Request.Params;

                obj.Parameters = nvcols;

                //初始化Dbheper
                var trans = ((NHibernate.Transaction.AdoTransaction)Session.Transaction).NGTrans;
                NG3.Data.Service.ConnectionInfoService.InitialDbHelper(trans);

                //执行插件
                rtn = obj.Execut();



                //成功还是失败记录日志

            }
            return rtn;

        }

        
    }
}
