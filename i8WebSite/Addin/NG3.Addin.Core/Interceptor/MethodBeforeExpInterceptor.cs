using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AopAlliance.Intercept;

namespace NG3.Addin.Core.Interceptor
{
    /// <summary>
    /// 表过式执行
    /// </summary>
    public class MethodBeforeExpInterceptor : AbstractMethodBeforeInterceptor
    {
        public override bool Before(IMethodInvocation invocation)
        {
            try
            {
                var exps = ConfigureEntity.ExpModels;

                if (exps == null) return true;

            
                bool rtn = ExpressionUtils.Eval(Session, invocation, exps, ConfigureEntity.ExpVarModels);
                return rtn;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
