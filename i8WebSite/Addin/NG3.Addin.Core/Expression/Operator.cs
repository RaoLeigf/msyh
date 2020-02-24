using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Expression
{
    /// <summary>
    /// 操作符
    /// </summary>
    public class Operator
    {

        public static OperatorType OpType(string text)
        {
            if (text == RegsitryManager.TOKEN_LEFT) return OperatorType.LEFT;
            if (text == RegsitryManager.TOKEN_RIGHT) return OperatorType.RIGHT;
            if (text == RegsitryManager.TOKEN_ADD) return OperatorType.ADD;
            if (text == RegsitryManager.TOKEN_SUB) return OperatorType.SUB;
            if (text == RegsitryManager.TOKEN_MUL) return OperatorType.MUL;
            if (text == RegsitryManager.TOKEN_DIV) return OperatorType.DIV;

            if (text == RegsitryManager.TOKEN_AND) return OperatorType.AND;
            if (text == RegsitryManager.TOKEN_OR) return OperatorType.OR;
            if (text == RegsitryManager.TOKEN_LT) return OperatorType.LT;
            if (text == RegsitryManager.TOKEN_GT) return OperatorType.GT;
            if (text == RegsitryManager.TOKEN_LE) return OperatorType.LE;

            if (text == RegsitryManager.TOKEN_GE) return OperatorType.GE;
            if (text == RegsitryManager.TOKEN_ET) return OperatorType.ET;
            if (text == RegsitryManager.TOKEN_UT) return OperatorType.UT;

            //返回函数
            if (RegsitryManager.IsFunction(text)) return OperatorType.FUNC;

    
            return OperatorType.ERR;

        }
        /// <summary>
        /// 操作符优化级比较
        /// 大于0.表示优先级大
        /// =0
        /// 小于0 表示优化级小
        /// </summary>
        /// <param name="op1"></param>
        /// <param name="op2"></param>
        /// <returns></returns>
        public static int CompareOperator(OperatorType op1, OperatorType op2)
        {
            return op1 - op2;
        }





    }
}
