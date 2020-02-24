using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;

namespace SUP.CustomForm.Rule.Expression
{

    /// <summary>
    /// 初始化时根据某些条件隐藏grid的列,更新后隐藏grid的某些列'，表达式的值为true则隐藏列
    /// p_form0000000083_m.numericcol_2 <>0  and  p_form0000000083_m.numericcol_3 <>0;p_form0000000083_d.datetimecol_1&p_form0000000083_d.numericcol_2&
    /// "<>"为"!=", and 为"&&", or为"||"
    /// </summary>
    public class ComputeExpDisBodyColExp : ExpressionBase
    {

        public ComputeExpDisBodyColExp()
        {

        }

        public ComputeExpDisBodyColExp(PbExpressionImp pbImp)
        {
            this.Expression = pbImp.Expression;
            this.Initial(pbImp.Expression);
        }

        private void Initial(string exp)
        {
            //string exp = "p_form0000000083_m.numericcol_2 <>0  and  p_form0000000083_m.numericcol_3 <>0;p_form0000000083_d.datetimecol_1&p_form0000000083_d.numericcol_2&";

            string[] s = exp.Split(';');
            string leftExp = s[0];//左表达式
            string righExp = s[1];//右表达式

            //List<string> left = new List<string>();
            //List<string> right = new List<string>();

            //左
            string sLeft = leftExp.Replace("=", ",").Replace("+", ",").Replace("*", ",").Replace("and", ",").Replace("and", ",")
                                .Replace("/", ",").Replace("(", ",").Replace(")", ",").Replace("<>", ",");

            string[] vs = sLeft.Split(',');
            for (int i = 0; i < vs.Length; i++)
            {
                if (vs[i].IndexOf(".") > 0)
                {
                    this.Left.Add(vs[i].Trim());
                }
            }

            //右
            string[] rightStrs = righExp.Split('&');
            for (int i = 0; i < rightStrs.Length; i++)
            {
                if (rightStrs[i].IndexOf(".") > 0)
                {
                    this.Right.Add(rightStrs[i].Trim());
                }
            }

        }

        /// <summary>
        /// 表达式转换成脚本代码
        /// </summary>
        /// <returns></returns>
        public string GetCodeFromExp(Dictionary<String, String> dic, Dictionary<string, Col> col, string colname)
        {
            if (string.IsNullOrEmpty(Expression))
                return string.Empty;

            string exp_condition = Expression.Split(';')[0];
            string exp_result = Expression.Split(';')[1];

            //string exp = "p_form0000000083_m.numericcol_2 <>0  and  p_form0000000083_m.numericcol_3 <>0;
            //p_form0000000083_d.datetimecol_1&p_form0000000083_d.numericcol_2&";
            // <>  -> !=    and -> &&   or  -> || 
            foreach (var left in Left)
            {
                exp_condition = exp_condition.Replace(left,
                                                      string.Format("Ext.getCmp('{0}').getValue()", left.Split('.')[1].Trim()));
            }
            exp_condition = exp_condition.Replace("<>", "!=").Replace("and", "&&").Replace("or", "||");


            //for(var i=0; i<listdwgrid.columns.length; i++){
            //    if(listdwgrid.columns[i].dataIndex == 'datetimecol_1'){
            //        listdwgrid.columns[i].hide();
            //    }
            //    if(listdwgrid.columns[i].dataIndex == 'numericcol_2'){
            //        listdwgrid.columns[i].hide();
            //    }
            //}
            string exp_tablename = dic[exp_result.Split('.')[0]];  //转table名称

            string s = string.Empty;
            s = string.Format("if({0})", exp_condition);
            s += "{\r\n";
            s += string.Format("\tfor(var i=0; i<{0}grid.columns.length; i++)", exp_tablename);
            s += "{\r\n";
            foreach (var right in Right)
            {
                s += "\t\t";
                s += string.Format(@"if({0}grid.columns[i].dataIndex == '{1}')", exp_tablename, right.Split('.')[1].Trim());
                s += "{\r\n\t\t\t";
                s += string.Format("{0}grid.columns[i].hide();\r\n", exp_tablename);
                s += "\t\t}\r\n";
            }
            s += "\t}\t\r\n}";

            string result = string.Empty;
            string funcname = string.Empty;
            if (col[colname].Xtype == "ngCustomFormHelp" || col[colname].Xtype == "ngRichHelp")
            {
                funcname = "helpselected";
            }
            else
            {
                funcname = "itemchanged";
            }

            foreach (var left in Left)
            {
                result += string.Format(@"Ext.getCmp('{0}').addListener('{1}', function()", left.Split('.')[1].Trim(), funcname);
                result += "{";
                result += s;
                result += "});\r\n";
            }


            return result;
        }

        public override string GetCodeFromExp(Dictionary<string, string> dic)
        {
            return string.Empty;
        }
    }
}
