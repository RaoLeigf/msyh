using NG3.Addin.Model.Domain.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Expression
{
    public class RegsitryManager
    {
        #region 运算符
        public const string TOKEN_LEFT = "(";
        public const string TOKEN_RIGHT = ")";
        public const string TOKEN_ADD = "+";
        public const string TOKEN_SUB = "-";
        public const string TOKEN_MUL = "*";
        public const string TOKEN_DIV = "/";
        public const string TOKEN_AND = "&&";
        public const string TOKEN_OR = "||";
        public const string TOKEN_LT = "<";
        public const string TOKEN_GT = ">";
        public const string TOKEN_LE = "<=";
        public const string TOKEN_GE = ">=";
        public const string TOKEN_ET = "==";
        public const string TOKEN_UT = "<>";

        public const string TOKEN_COMMA = ",";
        #endregion

        private static string[] Operators = { TOKEN_LEFT, TOKEN_RIGHT , TOKEN_ADD ,TOKEN_SUB,TOKEN_MUL,
                                              TOKEN_DIV,TOKEN_AND,TOKEN_OR,TOKEN_LT,TOKEN_GT,TOKEN_LE,
                                              TOKEN_GE,TOKEN_ET,TOKEN_UT,TOKEN_COMMA};
        #region 操作符表达式注册
        //操作符表达式注册
        private static OperatorExprssion[] OpFuntions = {
        new OperatorExprssion(TOKEN_ADD,2,"NG3.Addin.Core.Expression.BasicOperatorEval","Add"),
        new OperatorExprssion(TOKEN_SUB,2,"NG3.Addin.Core.Expression.BasicOperatorEval","Sub"),
        new OperatorExprssion(TOKEN_MUL,2,"NG3.Addin.Core.Expression.BasicOperatorEval","Mul"),
        new OperatorExprssion(TOKEN_DIV,2,"NG3.Addin.Core.Expression.BasicOperatorEval","Div"),
        new OperatorExprssion(TOKEN_AND,2,"NG3.Addin.Core.Expression.BasicOperatorEval","And"),
        new OperatorExprssion(TOKEN_OR,2,"NG3.Addin.Core.Expression.BasicOperatorEval","Or"),
        new OperatorExprssion(TOKEN_LT,2,"NG3.Addin.Core.Expression.BasicOperatorEval","Lt"),
        new OperatorExprssion(TOKEN_GT,2,"NG3.Addin.Core.Expression.BasicOperatorEval","Gt"),
        new OperatorExprssion(TOKEN_LE,2,"NG3.Addin.Core.Expression.BasicOperatorEval","Le"),
        new OperatorExprssion(TOKEN_GE,2,"NG3.Addin.Core.Expression.BasicOperatorEval","Ge"),
        new OperatorExprssion(TOKEN_ET,2,"NG3.Addin.Core.Expression.BasicOperatorEval","Et"),
        new OperatorExprssion(TOKEN_UT,2,"NG3.Addin.Core.Expression.BasicOperatorEval","Ut")
                };
        #endregion

        #region 支持的函数注册
        private static FuncExpression[] Functions = 
            {
                 new FuncExpression("If",3,"NG3.Addin.Core.Expression.BasicFunctionEval","If"),
                 new FuncExpression("IsNull",2,"NG3.Addin.Core.Expression.BasicFunctionEval","IsNull"),
                 new FuncExpression("Year",1,"NG3.Addin.Core.Expression.BasicFunctionEval","Year"),
                 new FuncExpression("Month",1,"NG3.Addin.Core.Expression.BasicFunctionEval","Month"),
                 new FuncExpression("Day",1,"NG3.Addin.Core.Expression.BasicFunctionEval","Day")



        };

        public static SupportFunctionBizModel[] SupportFuncs =
        {            
            new SupportFunctionBizModel("If(e,t,f)","If函数,e表达式为真时，值为t,否则为f"),
            new SupportFunctionBizModel("IsNull(e,t)","IsNull函数,e值为NULL时，值为t"),
            new SupportFunctionBizModel("Year(s)","s为日期字符，取字符串的年度"),
            new SupportFunctionBizModel("Month(s)","s为日期字符，取字符串的月份"),
            new SupportFunctionBizModel("Day(s)","s为日期字符，取字符串的天")
        };

        #endregion



        //判断是不是函数名
        public static bool IsFunction(string name)
        {

            if (string.IsNullOrEmpty(name)) return false;

            foreach (var item in Functions)
            {
                if (name.Equals(item.Identity,StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 判断是否有指定的操作符
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static bool ISOperator(string op)
        {

            if (Array.IndexOf(Operators, op) >= 0) return true;
      
            return false;

        }

        /// <summary>
        /// 得到操作符或者是函数的操作数的个数
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static int GetDimension(IExpression exp)
        {
            if (exp == null) return -1;
            //查找运算符
            foreach (var item in OpFuntions)
            {
                if(exp.Identity.Equals(item.Identity,StringComparison.OrdinalIgnoreCase))
                {
                    return item.Dimension;
                }
            }

            //查找自定义函数
            foreach (var item in Functions)
            {
                if (exp.Identity.Equals(item.Identity, StringComparison.OrdinalIgnoreCase))
                {
                    return item.Dimension;
                }
            }

            return exp.Dimension;
            
                         
        }

        //取得表过式运算信息类
        public static IExpression GetOperatorEval(IExpression exp)
        {
            if (exp == null) return null;
            //查找运算符
            foreach (var item in OpFuntions)
            {
                if (item.Identity == exp.Identity)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// 取得函数运算信息类
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static IExpression GetFunctionEval(IExpression exp)
        {
            if (exp == null) return null;
            //查找运算符
            foreach (var item in Functions)
            {
                if (exp.Identity.Equals(item.Identity, StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }
            return null;
        }

    }
}
