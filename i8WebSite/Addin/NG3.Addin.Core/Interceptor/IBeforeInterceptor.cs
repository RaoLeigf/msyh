using AopAlliance.Intercept;
using NG3.Addin.Core.Cfg;
using NG3.Addin.Model.Domain.BusinessModel;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Interceptor
{
    public interface IBeforeInterceptor
    {
        ISession Session{ set; get; }
        MethodAroundBizModel ConfigureEntity {set;get;}
        bool Before(IMethodInvocation invocation); 
    }
}
