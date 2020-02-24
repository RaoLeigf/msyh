using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Expression
{
    public interface IExpression
    {

        //标识符
        string Identity{set;get;}
        //表达式计算参数个数
        int Dimension { set; get; }

        string EvalClassName { set; get; }
        
        string EvalMethodName { set; get; } 

        object Eval(Operand[] operends);
    }
}
