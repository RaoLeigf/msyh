using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Expression
{
    /// <summary>
    /// 操作数表达式
    /// </summary>
    public class OperatorExprssion : IExpression
    {

        private string _identity;
        private int _dimension;
        private string _evalCalssName;
        private string _evalMethodName;

        #region Constructor
        public OperatorExprssion(string op)
        {
            _identity = op;
        }
        public OperatorExprssion(string op, int argscount):this(op)
        {
            _dimension = argscount;
        }

        public OperatorExprssion(string op, int argscount,string evalClassName,string evalMethodName):this(op,argscount)
        {
            _evalCalssName = evalClassName;
            _evalMethodName = evalMethodName;           
        }
        #endregion

        #region Field
        public int Dimension
        {
            get
            {
                return _dimension;
            }

            set
            {
                _dimension = value;
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
                throw new NotImplementedException();
            }
        }

        public string EvalClassName
        {
            get
            {
                return _evalCalssName;
            }

            set
            {
                _evalCalssName = value;
            }
        }

        public string EvalMethodName
        {
            get
            {
                return _evalMethodName;
            }

            set
            {
                _evalMethodName = value;
            }
        }

        #endregion

        public object Eval(Operand[] operands)
        {
            object result = null;
            
            OperatorExprssion opExpression = RegsitryManager.GetOperatorEval(this) as OperatorExprssion;

            try
            {
                if (opExpression != null)
                {
                    string className = opExpression.EvalClassName;
                    string methodName = opExpression.EvalMethodName;

                    Type t = Type.GetType(className);
                    var target = Activator.CreateInstance(t);
                    if (target != null)
                    {
                        MethodInfo method = t.GetMethod(methodName);
                        result = method.Invoke(target, new object[] { operands });
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                LogHelper<OperatorExprssion>.Error("表达式运算符计算出错!" + e.Message);
                throw;
            }
           

           
           
        }
    }

       
}
