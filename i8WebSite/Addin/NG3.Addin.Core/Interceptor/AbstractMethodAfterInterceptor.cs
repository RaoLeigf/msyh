using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AopAlliance.Intercept;
using NHibernate;
using NG3.Addin.Core.Cfg;
using NG3.Addin.Model.Domain.BusinessModel;

namespace NG3.Addin.Core.Interceptor
{
    public abstract class AbstractMethodAfterInterceptor : IAfterInterceptor
    {
        private ISession _session;

        private MethodAroundBizModel _configureEntity;

        public ISession Session
        {
            set{ _session = value;}
            get { return _session; }
        }

        public MethodAroundBizModel ConfigureEntity
        {
            set { _configureEntity = value; }
            get { return _configureEntity; }

        }


        public abstract bool After(object returnObject, IMethodInvocation invocation);
        
    }
}
