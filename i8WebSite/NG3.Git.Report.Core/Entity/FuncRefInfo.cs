using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Report.Func.Core.Entity
{
    /// <summary>
    /// 函数的反射信息
    /// </summary>
    public class FuncRefInfo
    {
        
        public FuncRefInfo(string name,string assembly,string clazz,string mname)
        {
            FuncName = name;
            AssemblyName = assembly;
            ClassName = clazz;
            MethodName = mname;
        }

        public FuncRefInfo() { }
        //函数名称
        public string FuncName { set; get; }

        //程序集名称
        public string AssemblyName { set; get; }
        //类名
        public string ClassName { set; get; }

        //方法名
        public string MethodName { set; get; }

    }
}
