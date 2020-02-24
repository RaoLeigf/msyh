using NG3.Report.Func.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Report.Func.Core.Interface
{
    public interface IFunction
    {
        //函数预处理，主要是公式参数的填充
        FuncInfo PreHandle();
        FuncInfo Func { set; get; }
    }
}
