using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;

namespace SUP.CustomForm.Rule.Expression
{
    /// <summary>
    /// 全局事件：BillBeforeSaveEvent
    /// 对应类型：NG3.Metadata.UI.PowserBuilder.Events.Implementation.PbExpressionType.BillValidation
    /// 
    /// 表达式示例1:p_form0000000083_m.numericcol_1>100,金额不能小于100,1
    /// 功能：验证布尔表达式，为false，弹出"金额不能小于100"，最后一个数据项为1表示在changed事件中验证，为0表示提交时才验证
    /// 
    /// </summary>
    public class BillValidationExp : ExpressionBase
    {

        private string boolExp = string.Empty;//左侧布尔表达式

        private string hint = string.Empty;//错误提示信息

        private bool immediateCheck = false;//是否changed实时验证,false是提交时验证

        public BillValidationExp()
        {

        }

        public BillValidationExp(PbExpressionImp pbImp)
        {
            //去掉运算符中空格,然后=替换成==
            pbImp.Expression = pbImp.Expression.Replace("< >", "<>").Replace("<>", "!=").Replace("> =", ">=").Replace("< =", "<=");
            pbImp.Expression = pbImp.Expression.Replace("!=", "*#1").Replace(">=", "*#2").Replace("<=", "*#3");
            pbImp.Expression = pbImp.Expression.Replace("=", "==").Replace(" and ", " && ").Replace(" or ", " || ");
            pbImp.Expression = pbImp.Expression.Replace("*#1", "!=").Replace("*#2", ">=").Replace("*#3", "<=");

            this.Expression = pbImp.Expression;
            this.Initial(this.Expression);
        }

        /// <summary>
        /// 左侧布尔表达式
        /// </summary>
        public string BoolExp
        {
            get { return boolExp; }
            set { boolExp = value; }
        }

        /// <summary>
        /// 错误提示信息
        /// </summary>
        public string Hint
        {
            get { return hint; }
            set { hint = value; }
        }

        /// <summary>
        /// 是否changed实时验证
        /// </summary>
        public bool ImmediateCheck
        {
            get { return immediateCheck; }
            set { immediateCheck = value; }
        }


        private void Initial(string exp)
        {
            string[] s = exp.Split(',');

            this.BoolExp = s[0];//左表达式
            this.Hint = s[1];//提示信息

            if (s[2] == "1")//检测时刻标记
            {
                this.ImmediateCheck = true;
            }

            //p_form0000000131_d.numericcol_1 - p_form0000000131_d.numericcol_2 >@ 0
            string left = this.BoolExp.Replace("=", ",").Replace(">", ",").Replace("<", ",").Replace(">=", ",").Replace("<=", ",")
                                               .Replace("<>", ",").Replace(" and ", ",").Replace(" or ", ",").Replace("+", ",").Replace("-", ",")
                                               .Replace("*", ",").Replace("/", ",").Replace("!", ",").Replace("(", ",").Replace(")", ",")
                                               .Replace("&&", ",").Replace("||", ",");

            string[] leftStr = left.Split(',');

            for (int i = 0; i < leftStr.Length; i++)
            {
                if (leftStr[i].IndexOf(".") > 0)
                {
                    this.Left.Add(leftStr[i].Trim());
                }
            }

        }

        public string GetCodeFromExp(Dictionary<string, string> dic, Dictionary<string, Col> col)
        {
            //NG3.Metadata.UI.PowserBuilder.Events.Implementation.PbExpressionType.BillValidation
            //p_form0000000083_m.numericcol_1>100,金额不能小于100,1
            //Ext.getCmp('numericcol_1').minValue=100;
            //1 change事件    0  提交时


            //if(!(Ext.getCmp('numericcol_1').getValue() > 100)){
            //    Ext.MessageBox.alert('提示', '金额不能小于100');
            //    return;
            //}

            string condition = string.Empty;
            string tips = string.Empty;
            string status = string.Empty;
            string funcname = string.Empty;
            string colname = string.Empty;
            string tablename = string.Empty;
            string s = string.Empty;

            var isform = true;   //是否全是form字段
            foreach (var left in Left)
            {
                if (left.Split('.')[0].Trim().Contains("_d"))
                {
                    tablename = left.Split('.')[0].Trim();
                    isform = false;
                    break;
                }
            }

            condition = Expression.Split(',')[0];
            tips = Expression.Split(',')[1];
            status = Expression.Split(',')[2];

            //用lambda表达式按字符串长度给list排序
            Left.Sort((a, b) => a.Length - b.Length);
            Left.Reverse();

            foreach (var left in Left)
            {
                colname = left.Split('.')[1].Trim();

                if (col[colname].Xtype == "ngCheckbox")
                {
                    funcname = "getCustomValue";
                }
                else
                {
                    funcname = "getValue";
                }

                if (left.Split('.')[0].Trim().Contains("_d"))
                {
                    condition = condition.Replace(left, string.Format("record.get('{0}')", colname));
                }
                else
                {
                    condition = condition.Replace(left, string.Format("Ext.getCmp('{0}').{1}()", colname, funcname));
                }                
            }

            //所有列都是form字段
            if (isform == true)
            {
                if (status == "0")
                {
                    s = "if(!(" + condition + ")){" + string.Format("\r\n");
                    s += string.Format("\tExt.MessageBox.alert('提示', '{0}');\r\n", tips);
                    s += "  return false;" + string.Format("\r\n");
                    s += "}" + string.Format("\r\n");
                    s += "else {" + string.Format("\r\n");
                    s += "  return true;" + string.Format("\r\n");
                    s += "}";
                }
                else
                {
                    condition = condition.Replace(">", "minValue=").Replace("<", "maxValue=");
                    s = condition + string.Format(";\r\n");
                    s += string.Format("Ext.getCmp('numericcol_1').blankText = \"{0}\";", tips);
                }
            }
            else
            {
                s = "var res = true;" + string.Format("\r\n");
                s += string.Format("Ext.Array.each({0}store.data.items, function (record) ", tablename);
                s += "{" + string.Format("\r\n");
                s += string.Format("\t") + "if(!(" + condition + ")) {" + string.Format("\r\n");
                s += string.Format("\t") + string.Format("Ext.MessageBox.alert('提示', '第' + (record.index + 1 ) + '行: ' + '{0}');", tips) + string.Format("\r\n");
                s += string.Format("\t\t") + "res = false;" + string.Format("\r\n");
                s += string.Format("\t\t") + "return false;" + string.Format("\r\n");
                s += string.Format("\t") + "}" + string.Format("\r\n");
                s += "});" + string.Format("\r\n");
                s += "return res;" + string.Format("\r\n");
            }

            return s;
        }

        public override string GetCodeFromExp(Dictionary<string, string> dic)
        {
            return string.Empty;
        }
    }
}
