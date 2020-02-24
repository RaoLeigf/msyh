using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Interceptor
{
    public interface IAssemblyInterceptorAction
    {
        NameValueCollection Parameters { set; get; }

        bool Execut();
    }
}
