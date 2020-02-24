using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Expression
{
    /// <summary>
    /// 函数表达式
    /// </summary>
    public class FuncExpression : IExpression
    {
        private string _identity;
        private int _dimension; //计算参数个数
        private string _evalCalssName;
        private string _evalMethodName;


        #region Constructor
        public FuncExpression(string funcName)
        {
            _identity = funcName;
        }

        public FuncExpression(string funcName,int argscount):this(funcName)
        {            
            _dimension = argscount;

        }

        public FuncExpression(string funcName, int argscount, string evalClassName, string evalMethodName):this(funcName,argscount)
        {       
            _evalCalssName = evalClassName;
            _evalMethodName = evalMethodName;
        }

        #endregion

        #region Field
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

        //标志符
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



        public object Eval(Operand[] operands)
        {
            object result = null;

            FuncExpression opExpression = RegsitryManager.GetFunctionEval(this) as FuncExpression;


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
    }
}
