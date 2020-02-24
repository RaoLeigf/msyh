using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Expression
{
    public class BasicOperatorEval
    {
        /// <summary>
        /// 加法
        /// </summary>
        /// <param name="operands"></param>
        /// <returns></returns>
        public object Add(Operand[] operands)
        {
            object result;
            double op1 = 0, op2 = 0;
            Operand.GetDoubles(operands, out op1, out op2);
            result = op1 + op2;

            return result;
        }

        public object Sub(Operand[] operands)
        {
            object result;
            double op1 = 0, op2 = 0;
            Operand.GetDoubles(operands, out op1, out op2);
            result = op1 - op2;

            return result;
        }

        public object Mul(Operand[] operands)
        {
            object result;
            double op1 = 0, op2 = 0;
            Operand.GetDoubles(operands, out op1, out op2);
            result = op1 * op2;

            return result;
        }
        public object Div(Operand[] operands)
        {
            object result;
            double op1 = 0, op2 = 0;
            Operand.GetDoubles(operands, out op1, out op2);
            if (op2 == 0) throw new ArithmeticException();
            result = op1 / op2;

            return result;
        }

        public object And(Operand[] operands)
        {
            object result;
            bool op1 = false, op2 = false;
            Operand.GetBools(operands, out op1, out op2);
            result = op1 && op2;

            return result;
        }

        public object Or(Operand[] operands)
        {
            object result;
            bool op1 = false, op2 = false;
            Operand.GetBools(operands, out op1, out op2);
            result = op1 || op2;

            return result;
        }
        public object Lt(Operand[] operands)
        {
            object result;
            double op1 = 0, op2 = 0;
            Operand.GetDoubles(operands, out op1, out op2);
            result = op1 < op2;

            return result;
        }

        public object Gt(Operand[] operands)
        {
            object result;
            double op1 = 0, op2 = 0;
            Operand.GetDoubles(operands, out op1, out op2);
            result = op1 > op2;

            return result;
        }

        public object Le(Operand[] operands)
        {
            object result;
            double op1 = 0, op2 = 0;
            Operand.GetDoubles(operands, out op1, out op2);
            result = op1 <= op2;

            return result;
        }

        public object Ge(Operand[] operands)
        {
            object result;
            double op1 = 0, op2 = 0;
            Operand.GetDoubles(operands, out op1, out op2);
            result = op1 >= op2; 

            return result;
        }

        public object Et(Operand[] operands)
        {
            object result;


            //判断是否字符串
           if(operands[0].IsString()|| operands[1].IsString())
            {
                return operands[0].Value.Equals(operands[1].Value);
            } 

            double op1 = 0, op2 = 0;
            Operand.GetDoubles(operands, out op1, out op2);
            result = op1 == op2;

            return result;
        }

        public object Ut(Operand[] operands)
        {
            object result;

            //判断是否字符串
            if (operands[0].IsString() || operands[1].IsString())
            {
                return (!(operands[0].Value.Equals(operands[1].Value)));
            }


            double op1 = 0, op2 = 0;
            Operand.GetDoubles(operands, out op1, out op2);
            result = op1 != op2;

            return result;
        }



    }
}
