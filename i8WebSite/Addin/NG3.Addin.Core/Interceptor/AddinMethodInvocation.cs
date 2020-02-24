using AopAlliance.Intercept;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace NG3.Addin.Core.Interceptor
{
    public class AddinMethodInvocation : IMethodInvocation
    {
        private MethodInfo _method;
        private object[] _args;
        private object _target;
        public AddinMethodInvocation(MethodInfo method, object[] args, object target)
        {
            _method = method;
            _args = args;
            _target = target;
        }

        public object[] Arguments
        {
            get
            {
                return _args;
            }
        }

        public MethodInfo Method
        {
            get
            {
                return _method;
            }
        }

        public object Proxy
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public MemberInfo StaticPart
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object Target
        {
            get
            {
                return _target;
            }
        }

        public Type TargetType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object This
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object Proceed()
        {
            throw new NotImplementedException();
        }
    }
}
