using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Expression
{
    /// <summary>
    /// 表达式计算函数
    /// </summary>
    public class BasicFunctionEval
    {

        #region 条件表达式
        //IF函数，函数格式类似于if(bool expression,true,false)
        public object If(Operand[] operands)
        {
            if (operands == null) return null;

            if (operands.Length < 3)
            {

                throw new AddinException("IF 函数参数个数不对！");
            }

            bool op1 = false;
            Operand.GetBools(operands, out op1);
            Operand op = op1 ? operands[1] : operands[2];

            return op.Value;



        }

        //判断表达式是否为空
        public object IsNull(Operand[] operands)
        {
            if (operands == null) return null;

            if (operands.Length < 2) 
           {                
                throw new Exception("IsNull 函数参数个数不对！");
            }
                    

            if (operands[0].Value == null || operands[0].Value.ToString()=="null")
                return operands[1].Value;
            else
                return operands[0].Value;
        }
        #endregion


        #region  提供一些日期函数
        public int Year(Operand[] operands)
        {

            if (operands == null) return -1;

            DateTime dt;
            Operand.GetDateTimes(operands, out dt);
            return dt.Year;


        }
        public int Month(Operand[] operands)
        {
            if (operands == null) return -1;

            DateTime dt;
            Operand.GetDateTimes(operands, out dt);
            return dt.Month;


        }
        public int Day(Operand[] operands)
        {
            if (operands == null) return -1;
            DateTime dt;
            Operand.GetDateTimes(operands, out dt);
            return dt.Day;

        }
        #endregion

    }
}
