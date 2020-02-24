using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Interceptor
{
    public abstract class AbstractAssemblyInterceptorAction : IAssemblyInterceptorAction
    {
        private NameValueCollection _parameters;
        public NameValueCollection Parameters
        {
            get
            {
                return _parameters;
            }

            set
            {
                _parameters = value;
            }
        }

        public abstract bool Execut();        
    }
}
