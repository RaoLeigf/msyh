using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;

namespace NG3.Metadata.UI.PowserBuilder.Events.Implementation
{
    /// <summary>
    /// 内置事件实现-表达式
    /// </summary>
    public class PbExpressionImp:PbBaseImp
    {
        private string _expression = string.Empty;
        private PbExpressionType _expressionType = PbExpressionType.Other;

        /// <summary>
        /// 表达式文本
        /// </summary>
        public string Expression
        {
            get { return _expression; }
            set { _expression = value; }
        }

        /// <summary>
        /// 表达式类型
        /// </summary>
        public PbExpressionType ExpressionType
        {
            get { return _expressionType; }
            set { _expressionType = value; }
        }

        public PbExpressionImp()
        {
            EventImpType = PbEventImpType.Expression;
        }
    }
}
