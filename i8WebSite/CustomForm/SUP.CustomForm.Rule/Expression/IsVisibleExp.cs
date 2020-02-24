using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;

namespace SUP.CustomForm.Rule.Expression
{
    /// <summary>
    /// itemchanged事件 
    /// textcol_2.visible=1	if( ddlbcol_1='2', 0, 1 ) or numericcol_5.visible=1 0
    /// </summary>
    public class IsVisibleExp : ExpressionBase
    {

        public IsVisibleExp()
        { 
        }

        public IsVisibleExp(PbExpressionImp pbImp)
        {
            if (pbImp.ExpressionType != PbExpressionType.IsVisible)
            {
                throw new Exception("PbExpressionType is not IsVisible!");
            }
            this.Expression = pbImp.Expression;
            this.Initial(pbImp.Expression);
        }

        private void Initial(string exp)
        {
            string[] s = exp.Split('.');
            string left = s[0];          //左值
            string right = s[1].Trim();  //右值

            //固定不可见
            if (right.Substring(right.Length - 1, 1) == "0")
            {
                this.Left.Add(left);
                this.Right.Add("0");
                return;
            }

            int leftBracket = right.IndexOf('(');
            int rightBracket = right.IndexOf(')');
            right = right.Substring(leftBracket + 1, rightBracket - leftBracket - 1);

            string[] rightexp = right.Split(',');

            this.Left.Add(left);

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
            //textcol_2.visible=1	if( ddlbcol_1='2', 0, 1 ) or numericcol_5.visible=1 0

            if (string.IsNullOrEmpty(Expression))
                return string.Empty;

            string exp = string.Empty;
            string s = string.Empty;
            string ifexp = string.Empty;

            //固定不可见
            if (Right[0] == "0")
            {
                return string.Format("\t\t" + "Ext.getCmp('{0}').hide();\r\n", colname);                
            }
           
            //Right[0] 判断条件中的运算符格式化
            ifexp = Right[0];
            ifexp = ifexp.Replace("< >", "<>").Replace("<>", "!=").Replace("> =", ">=").Replace("< =", "<=");
            ifexp = ifexp.Replace("!=", "*#1").Replace(">=", "*#2").Replace("<=", "*#3");
            ifexp = ifexp.Replace("=", "==").Replace(" and ", " && ").Replace(" or ", " || ");
            ifexp = ifexp.Replace("*#1", "!=").Replace("*#2", ">=").Replace("*#3", "<=");

            char[] operatorArr = {'=','>','<','!'};
            string changeCol = ifexp.Split(operatorArr)[0].Trim();
            ifexp = ifexp.Replace(changeCol, string.Format("Ext.getCmp('{0}').getValue()", changeCol));

            //Right[1] 根据右边第二个参数判断是hide还是show
            string[] judgeArr = new string[2];
            if (Right[1] == "1")
            {
                judgeArr[0] = "show";
                judgeArr[1] = "hide";
            }
            else
            {
                judgeArr[0] = "hide";
                judgeArr[1] = "show";
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

            exp += "\r\n" + "\t" + "if (" + ifexp + ") {";
            exp += string.Format("\r\n" + "\t\t" + "Ext.getCmp('{0}').{1}();", colname, judgeArr[0]);
            exp += "\r\n" + "\t" + "}";
            exp += "\r\n" + "\t" + "else {";
            exp += string.Format("\r\n" + "\t\t" + "Ext.getCmp('{0}').{1}();", colname, judgeArr[1]);
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
            //textcol_2.visible=1	if( ddlbcol_1='2', 0, 1 ) or numericcol_5.visible=1 0

            if (string.IsNullOrEmpty(Expression))
                return string.Empty;

            string exp = string.Empty;
            string s = string.Empty;
            string ifexp = string.Empty;

            //物资代码隐藏的是c_name物资列
            if (colname == "res_code")
            {
                colname = "c_name";
            }
            else
            {
                //帮助列隐藏的是名称列
                if (col[colname].Xtype == "ngCustomFormHelp" || col[colname].Xtype == "ngRichHelp")
                {
                    colname += "_name";
                }
            }

            //固定不可见
            if (Right[0] == "0")
            {
                return string.Format("\t\t" + "{0}grid.hideColumn('{1}', true);\r\n", tablename, colname);
            }

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
                judgeArr[0] = "false";
                judgeArr[1] = "true";
            }
            else
            {
                judgeArr[0] = "true";
                judgeArr[1] = "false";
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

            exp += "\r\n" + "\t" + "if (" + ifexp + ") {";
            exp += string.Format("\r\n" + "\t\t" + "{0}grid.hideColumn('{1}', {2});", tablename, colname, judgeArr[0]);
            exp += "\r\n" + "\t" + "}";
            exp += "\r\n" + "\t" + "else {";
            exp += string.Format("\r\n" + "\t\t" + "{0}grid.hideColumn('{1}', {2});", tablename, colname, judgeArr[1]);
            exp += "\r\n" + "\t" + "}\r\n";

            s += string.Format("Ext.getCmp('{0}').addListener('{1}', function()", changeCol, funcname);
            s += "{";
            s += exp;
            s += "});\r\n";

            return s;

        }

        public override string GetCodeFromExp(Dictionary<string, string> dic)
        {
            return string.Empty;
        }

    }
}
