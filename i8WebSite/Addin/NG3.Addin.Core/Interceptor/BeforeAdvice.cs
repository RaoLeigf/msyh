using Spring.Aop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using NG3.Addin.Core.Cfg;
using NG3.Addin.Model.Enums;

namespace NG3.Addin.Core.Interceptor
{
    public class BeforeAdvice : IMethodBeforeAdvice
    {
        private InterceptorExecutor executor;
        public void Before(MethodInfo method, object[] args, object target)
        {

            //打出环境参数
            LogHelper<BeforeAdvice>.PrintEvn();
            AddinMethodInvocation invocation = new AddinMethodInvocation(method, args, target);
            //请求的参数保存到当前的服务器内存中
            AddinEnvironment.SaveServiceRequestParam(invocation);

            bool rtn = false;
            try
            {
                
                rtn = executor.ExecuteBeforeMethod(method,args,target);
                //是否要判断返回值来处理
                LogHelper<BeforeAdvice>.Error("功能注入方法前方法执行结果:" + rtn);
            }
            catch (Exception e)
            {
                LogHelper<BeforeAdvice>.Error("功能注入执行方法前方法出错:" + e.StackTrace);
                throw new AddinException("功能注入提示信息:" + e.Message);
            }
        }
    }
}
