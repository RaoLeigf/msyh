using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.Rule.Expression
{
    public abstract class ExpressionBase
    {
        private string expression = string.Empty;
        private string loctype;

        private List<string> left = new List<string>();
        private List<string> right = new List<string>();

        public ExpressionBase()
        {

        }


        /// <summary>
        /// 原始表达式
        /// </summary>
        public string Expression
        {
            get { return expression; }
            set { expression = value; }
        }

        /// <summary>
        /// 表达式位置
        /// </summary>
        public string LocType
        {
            get { return loctype; }
            set { loctype = value; }
        }

        /// <summary>
        /// 左值
        /// </summary>
        public List<string> Left
        {
            get { return left; }
            set { left = value; }
        }

        /// <summary>
        /// 右值
        /// </summary>
        public List<string> Right
        {
            get { return right; }
            set { right = value; }
        }


        /// <summary>
        /// 表达式转换成脚本代码
        /// </summary>
        /// <returns></returns>
        public abstract string GetCodeFromExp(Dictionary<String, String> dic);


    }
}
