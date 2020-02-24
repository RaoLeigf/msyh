using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;

namespace SUP.CustomForm.Rule.Expression
{
    /// <summary>
    /// itemchanged事件
    /// 0	if( ddlbcol_1='9', 0, 1 )  or 0 1
    /// </summary>
    public class IsReadOnlyExp : ExpressionBase
    {

        public IsReadOnlyExp()
        {
        }

        public IsReadOnlyExp(PbExpressionImp pbImp)
        {
            if (pbImp.ExpressionType != PbExpressionType.IsReadOnly)
            {
                throw new Exception("PbExpressionType is not IsReadOnly!");
            }
            this.Expression = pbImp.Expression;
            this.Initial(pbImp.Expression);
        }

        private void Initial(string exp)
        {
            string right = exp; //右值

            //固定不可见
            if (right.Substring(right.Length - 1, 1) == "1")
            {
                this.Right.Add("1");
                return;
            }

            int leftBracket = right.IndexOf('(');
            int rightBracket = right.IndexOf(')');
            right = right.Substring(leftBracket + 1, rightBracket - leftBracket - 1);

            string[] rightexp = right.Split(',');

            for (int i = 0; i < rightexp.Length; i++)
            {
                this.Right.Add(rightexp[i].Trim());
            }
        }

        /// <summary>
        /// 表达式转换成脚本代码
        /// </summary>
        /// <returns></returns>
        public string GetCodeFromExp(Dictionary<string, string> dic, Dictionary<string, Col> col, string colname)
        {
            //protect="0	if( ddlbcol_1='9', 0, 1 )"

            if (string.IsNullOrEmpty(Expression))
                return string.Empty;

            string exp = string.Empty;
            string s = string.Empty;

            string ifexp = string.Empty;

            //Right[0] 判断条件中的运算符格式化
            ifexp = Right[0];
            ifexp = ifexp.Replace("< >", "<>").Replace("<>", "!=").Replace("> =", ">=").Replace("< =", "<=");
            ifexp = ifexp.Replace("!=", "*#1").Replace(">=", "*#2").Replace("<=", "*#3");
            ifexp = ifexp.Replace("=", "==").Replace(" and ", " && ").Replace(" or ", " || ");
            ifexp = ifexp.Replace("*#1", "!=").Replace("*#2", ">=").Replace("*#3", "<=");

            char[] operatorArr = { '=', '>', '<', '!' };
            string changeCol = ifexp.Split(operatorArr)[0].Trim();
            ifexp = ifexp.Replace(changeCol, string.Format("Ext.getCmp('{0}').getValue()", changeCol));

            //Right[1] 根据右边第二个参数判断是hide还是show
            string[] judgeArr = new string[2];
            if (Right[1] == "1")
            {
                judgeArr[0] = "true";
                judgeArr[1] = "false";
            }
            else
            {
                judgeArr[0] = "false";
                judgeArr[1] = "true";
            }

            //判断事件类型
            string funcname = string.Empty;
            if (col[changeCol].Xtype == "ngCustomFormHelp" || col[changeCol].Xtype == "ngRichHelp" || col[changeCol].Xtype == "ngComboBox")
            {
                funcname = "helpselected";
            }
            else
            {
                funcname = "itemchanged";
            }

            exp += "\r\n" + "\t" + "if (otype == $Otype.VIEW) return;";
            exp += "\r\n" + "\t" + "if (" + ifexp + ") {";
            exp += string.Format("\r\n" + "\t\t" + "Ext.getCmp('{0}').userSetReadOnly({1});", colname, judgeArr[0]);
            exp += "\r\n" + "\t" + "}";
            exp += "\r\n" + "\t" + "else {";
            exp += string.Format("\r\n" + "\t\t" + "Ext.getCmp('{0}').userSetReadOnly({1});", colname, judgeArr[1]);
            exp += "\r\n" + "\t" + "}\r\n";

            s += string.Format("Ext.getCmp('{0}').addListener('{1}', function()", changeCol, funcname);
            s += "{";
            s += exp;
            s += "});\r\n";

            return s;
        }

        /// <summary>
        /// 表达式转换成脚本代码
        /// </summary>
        /// <returns></returns>
        public string GetCodeFromExpGrid(Dictionary<string, string> dic, Dictionary<string, Col> col, string colname, string tablename)
        {
            //protect="0	if(ddlbcol_1='9',0,1)"   or 0	1

            if (string.IsNullOrEmpty(Expression))
                return string.Empty;

            string exp = string.Empty;
            string s = string.Empty;
            string ifexp = string.Empty;

            //固定只读
            if (Right[0] == "1")
            {
                exp += string.Format("\r\n" + "{0}grid.addListener('beforeedit', function(editor, e, eOpts) ", tablename) + "{";
                exp += string.Format("\r\n" + "\t" + "if (e.field == '{0}') ", colname) + "{";
                exp += string.Format("\r\n" + "\t\t" + "return false;");
                exp += "\r\n" + "\t" + "}";
                exp += "\r\n" + "});\r\n";

                s += exp;

                return s;
            }

            //Right[0] 判断条件中的运算符格式化
            ifexp = Right[0];
            ifexp = ifexp.Replace("< >", "<>").Replace("<>", "!=").Replace("> =", ">=").Replace("< =", "<=");
            ifexp = ifexp.Replace("!=", "*#1").Replace(">=", "*#2").Replace("<=", "*#3");
            ifexp = ifexp.Replace("=", "==").Replace(" and ", " && ").Replace(" or ", " || ");
            ifexp = ifexp.Replace("*#1", "!=").Replace("*#2", ">=").Replace("*#3", "<=");

            char[] operatorArr = { '=', '>', '<', '!' };
            string changeCol = ifexp.Split(operatorArr)[0].Trim();
            ifexp = ifexp.Replace(changeCol, string.Format("data[0].get('{0}')", changeCol));

            //Right[1] 根据右边第二个参数判断是hide还是show
            string[] judgeArr = new string[2];
            if (Right[1] == "1")
            {
                judgeArr[0] = "return false;";
            }
            else
            {
                judgeArr[0] = "return true;";
            }

            exp += string.Format("\r\n" + "{0}grid.addListener('beforeedit', function(editor, e, eOpts) ", tablename) + "{";
            exp += string.Format("\r\n" + "\t" + "var data = {0}grid.getSelectionModel().getSelection();", tablename);
            exp += string.Format("\r\n" + "\t" + "if (e.field == '{0}' && {1}) ", colname, ifexp) + "{";
            exp += string.Format("\r\n" + "\t\t" + "{0}", judgeArr[0]);
            exp += "\r\n" + "\t" + "}";
            exp += "\r\n" + "});\r\n";

            s += exp;

            return s;
        }

        public override string GetCodeFromExp(Dictionary<string, string> dic)
        {
            return string.Empty;
        }

    }
}
