using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AopAlliance.Intercept;

namespace NG3.Addin.Core.Interceptor
{
    public class MethodAfterExpInterceptor : AbstractMethodAfterInterceptor
    {
        public override bool After(object returnObject, IMethodInvocation invocation)
        {
            try
            {
                var exps = ConfigureEntity.ExpModels;

                if (exps == null) return true;


                bool rtn = ExpressionUtils.Eval(Session, invocation, exps, ConfigureEntity.ExpVarModels);
                return rtn;

                //var exps = ConfigureEntity.ExpModels;
                //string expression = string.Empty;
                //if (exps!=null && exps.Count >0)
                //{
                //    expression = ConfigureEntity.ExpModels[0].ExpText;
                //}
                //if (!string.IsNullOrWhiteSpace(expression)) return true;
                
                //bool rtn = ExpressionUtils.Eval(Session, invocation, expression, ConfigureEntity.ExpVarModels);
                //return rtn;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
