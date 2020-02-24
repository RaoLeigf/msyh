using Enterprise3.Common.Model.Attributes;
using NG3.Addin.Model.Domain.BusinessModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NG3.Addin.Core.Cfg
{
    public static class InterceptedServiceScanner
    {
        public static IList<InterceptedServiceBizModel> GetInterceptedService()
        {
            List<InterceptedServiceBizModel> services = new List<InterceptedServiceBizModel>();

            string path = string.Empty;

            path = HttpRuntime.AppDomainAppPath;
            path = path + @"\i6Rules";

            DirectoryInfo TheFolder = new DirectoryInfo(path);
           

            foreach (var file in TheFolder.GetFiles())
            {
                if (file.Extension != ".dll") continue;
                //加载DLL
                try
                {
                    Assembly assembly = Assembly.LoadFrom(file.FullName);


                    var types = assembly.GetExportedTypes();


                    foreach (var clazz in types)
                    {
                        var methdoInfos = clazz.GetMethods();
                        foreach (var method in methdoInfos)
                        {
                            var attributes = method.GetCustomAttributes(true);
                            foreach (var attr in attributes)
                            {
                                if (attr.GetType().ToString() == "Enterprise3.Common.Model.Attributes.AddinAttribute")
                                {

                                    AddinAttribute attribute = attr as AddinAttribute;
                                    if (attribute !=null)
                                    {
                                        //插入
                                        InterceptedServiceBizModel service = new InterceptedServiceBizModel();
                                        service.TargetAssemblyName = file.Name;
                                        service.TargetClassName = clazz.Name;
                                        service.TargetMethodName = method.Name;

                                        service.ServiceFuncName = attribute.ServiceMethodDescription;
                                        service.ServiceName = attribute.ServiceDescription;
                                        service.MatchCondition = attribute.MatchCondition;

                                        services.Add(service);
                                    }
                                   

                                    
                                }
                            }
                        }


                    }

                }
                catch (Exception)
                {

                    continue;
                }
            }


            return services; 
        }
    }
}
