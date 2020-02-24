using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using gudusoft.gsqlparser;

namespace SUP.CustomForm.Rule.Expression
{
    public  class WhereCondition
    {
        private List<string> ls = new List<string>();

        private TLzCustomExpression condition;

        public WhereCondition(TLzCustomExpression expr)
        {
            this.condition = expr;
        }

        public List<string> getControlIds()
        {
            this.condition.InOrderTraverse(searchValue);
            return this.ls;
        }

        public bool searchValue(TLz_Node pnode, Boolean pIsLeafNode)
        {

            TLzCustomExpression lcexpr = (TLzCustomExpression)pnode;
            if (lcexpr.lexpr is TLzCustomExpression)
            {
                ls.Clear();
                TLzCustomExpression leftExpr = (TLzCustomExpression)lcexpr.lexpr;
                if (leftExpr.oper == TLzOpType.Expr_Attr)
                {
                    //Console.WriteLine("column: {0}", leftExpr.AsText);
                    //if (lcexpr.opname != null)
                    //{
                    //    Console.WriteLine("Operator: {0}", lcexpr.opname.AsText);
                    //}
                    //Console.WriteLine("value: {0}", lcexpr.rexpr.AsText);
                    //Console.WriteLine("");

                    ls.Add(lcexpr.rexpr.AsText);
                }
            }

            return true;
        }
    }
}
