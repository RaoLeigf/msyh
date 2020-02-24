using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Expression
{
    /// <summary>
    /// 常量，变量表达式
    /// </summary>
    public class VariableExpression : IExpression
    {

        private string _identity;
        private bool _isStringToken;
        public VariableExpression(string var,bool isString)
        {
            _identity = var;
            _isStringToken = isString;
        }

        #region Field
        /// <summary>
        /// 计算参数
        /// </summary>
        public int Dimension
        {
            get
            {
                return 0;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string EvalClassName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string EvalMethodName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Identity
        {
            get
            {
                return _identity;
            }

            set
            {
                _identity = value;
            }
        }
        #endregion
        /// <summary>
        /// 进行变量替换
        /// </summary>
        /// <param name="Operands"></param>
        /// <returns></returns>
        public virtual object Eval(Operand[] operands)
        {
            if (_isStringToken)
            {
                return _identity;
            }
            else
            {
                //存在空值的可能性
                if (_identity == "null") return "null";

                double val;
                bool b = Operand.ToNumber(_identity, out val);
                if (!b) throw new AddinException("[" + _identity + "]转换成数值类型失败,如果值本身就是字符型请加单引号");
                return val;

            }
                

            

        }

      
        
       
    }
}
