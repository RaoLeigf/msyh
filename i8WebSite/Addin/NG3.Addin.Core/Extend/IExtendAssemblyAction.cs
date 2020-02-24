using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Extend
{
    /// <summary>
    /// 这个接口主要是给插件使用
    /// </summary>
    public interface IExtendAssemblyAction
    {

        NameValueCollection Parameters { set; get; }

        string Execut();
    }
}
