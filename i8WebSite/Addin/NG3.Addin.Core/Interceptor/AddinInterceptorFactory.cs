using AopAlliance.Intercept;
using NG3.Addin.Core.Cfg;
using NG3.Addin.Model.Domain.BusinessModel;
using NG3.Addin.Model.Enums;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Interceptor
{
    public static class AddinInterceptorFactory
    {
        public static IBeforeInterceptor[] GetBeforeExecutor(MethodAroundBizModel entity, ISession session)
        {

            List<IBeforeInterceptor> executors = new List<IBeforeInterceptor>();

            
            if (entity != null)
            {
                //方法前只支持表达式
                foreach (var item in entity.ExpModels)
                {
                    IBeforeInterceptor executor =  new MethodBeforeExpInterceptor();
                    executor.Session = session;
                    executor.ConfigureEntity = entity;
                    executors.Add(executor);
                }

                ////再计算SQL
                //foreach (var item in entity.SqlModels)
                //{
                //    IBeforeInterceptor executor = new MethodBeforeSqlInterceptor();
                //    executor.Session = session;
                //    executor.ConfigureEntity = entity;
                //    executors.Add(executor);
                //}
                ////再调用插件
                //foreach (var item in entity.AssemblyModels)
                //{
                //    IBeforeInterceptor executor = new MethodBeforeAssemblyInterceptor();
                //    executor.Session = session;
                //    executor.ConfigureEntity = entity;
                //    executors.Add(executor);
                //}

            }
            return executors.ToArray();
        }


        public static IAfterInterceptor[] GetAfterExecutor(MethodAroundBizModel entity, ISession session)
        {
            //方法后支持SQL语句与插件

            List<IAfterInterceptor> executors = new List<IAfterInterceptor>();


            if (entity != null)
            {
                ////先计算表达式
                //foreach (var item in entity.ExpModels)
                //{
                //    IAfterInterceptor executor = new MethodAfterExpInterceptor();
                //    executor.Session = session;
                //    executor.ConfigureEntity = entity;
                //    executors.Add(executor);
                //}

                //再计算SQL
                foreach (var item in entity.SqlModels)
                {
                    IAfterInterceptor executor = new MethodAfterSqlInterceptor();
                    executor.Session = session;
                    executor.ConfigureEntity = entity;
                    executors.Add(executor);
                }
                //再调用插件
                foreach (var item in entity.AssemblyModels)
                {
                    IAfterInterceptor executor = new MethodAfterAssemblyInterceptor();
                    executor.Session = session;
                    executor.ConfigureEntity = entity;
                    executors.Add(executor);
                }

            }
            return executors.ToArray();
        }

    }
}
