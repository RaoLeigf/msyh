using AopAlliance.Intercept;
using NHibernate;
using Spring.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Interceptor
{ 
    class AroundInterceptor : IMethodInterceptor
    {
        private InterceptorExecutor executor;
      
        public object Invoke(IMethodInvocation invocation)
        {
            //打出环境参数
            LogHelper<AroundInterceptor>.PrintEvn();

            bool rtn = false;
            try
            {
                //rtn = executor.ExecuteBeforeMethod(invocation);
                //是否要判断返回值来处理
            }
            catch (Exception e)
            {
                LogHelper<AroundInterceptor>.Error("执行方法前环绕方法出错:"+e.Message);
                throw;
            }
            

            object obj = invocation.Proceed();

            //事务是否存在问题
            try
            {
                //rtn = executor.ExecuteAfterMethod(obj,invocation);
                //是否要判断返回值来处理

            }
            catch (Exception e)
            {
                LogHelper<AroundInterceptor>.Error("执行方法后环绕方法出错:" + e.Message);
                throw;
            }

            return obj;
        }
    }
}
