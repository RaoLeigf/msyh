using Spring.Aop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using NG3.Addin.Core.Cfg;
using NG3.Addin.Model.Enums;
using NG3.Addin.Model.Domain.BusinessModel;

namespace NG3.Addin.Core.Interceptor
{
    public class AfterAdvice : IAfterReturningAdvice
    {
        private InterceptorExecutor executor;
        public void AfterReturning(object returnValue, MethodInfo method, object[] args, object target)
        {
            //打出环境参数
            LogHelper<AfterAdvice>.PrintEvn();
            AddinMethodInvocation invocation = new AddinMethodInvocation(method, args, target);
            AddinEnvironment.SaveServiceRequestParam(invocation);
          
            
            try
            {
                LogHelper<AfterAdvice>.Info("方法执行的结果是：" + (returnValue == null ? "null":returnValue.ToString()));                
                bool rtn = executor.ExecuteAfterMethod(returnValue, method,args,target);
                //是否要判断返回值来处理
                LogHelper<AfterAdvice>.Info("功能注入方法执行的结果是："+rtn);
            }
            catch (Exception e)
            {
                LogHelper<AfterAdvice>.Error("执行方法后方法出错:" + e.StackTrace);
                throw new AddinException("功能注入提示信息:" + e.Message);
            }
        }
    }
}
