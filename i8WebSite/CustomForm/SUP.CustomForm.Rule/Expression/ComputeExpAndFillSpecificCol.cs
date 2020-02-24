using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;
using System.Text.RegularExpressions;

namespace SUP.CustomForm.Rule.Expression
{
    /// <summary>
    /// 对应:NG3.Metadata.UI.PowserBuilder.Events.Implementation.PbExpressionType.ComputeExpAndFillSpecificCol
    /// 调用楼工控件解析pb表达式
    /// p_form0000000083_m.longcol_1 = year( bill_dt )
    /// </summary>
    public class ComputeExpAndFillSpecificCol : ExpressionBase
    {
        public ComputeExpAndFillSpecificCol()
        {

        }

        public ComputeExpAndFillSpecificCol(PbExpressionImp pbImp)
        {
            if (pbImp.ExpressionType != PbExpressionType.ComputeExpAndFillSpecificCol)
            {
                throw new Exception("PbExpressionType is not ComputeExpAndFillSpecificCol !");
            }
            this.Expression = pbImp.Expression;
            this.Initial(pbImp.Expression);
        }

        private void Initial(string exp)
        {
            //this.SetSqlParam(s[1], @"\(", @"\)");//

            string[] s = exp.Split('=');

            string left = s[0];  //左值
            this.Left.Add(left);

            string right = s[1]; //右值
            string rexp = right.Replace("=", ",").Replace("+", ",").Replace("-", ",").Replace("*", ",").Replace("/", ",").Replace("(", ",").Replace(")", ",");
            string[] vs = rexp.Split(',');

            for (int i = 0; i < vs.Length; i++)
            {
                if (vs[i].IndexOf(".") > 0)
                {
                    this.Right.Add(vs[i].Trim());
                }
            }
        }

        /// <summary>
        /// 截取()之间的参数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void SetSqlParam(string str, string start, string end)
        {
            Regex rg = new Regex("(?<=(" + start + "))[.\\s\\S]*?(?=(" + end + "))", RegexOptions.Multiline | RegexOptions.Singleline);

            MatchCollection macths = rg.Matches(str);
            List<string> ls = new List<string>();
            for (int i = 0; i < macths.Count; i++)
            {
                this.Right.Add(macths[i].Value);
            }

        }


        public string GetCodeFromExp(Dictionary<string, string> dic, Dictionary<string, Col> col, string colname)
        {
            //p_form0000000083_m.longcol_1 = getYear( bill_dt )
            //Ext.getCmp('longcol_1').setValue(getYear(Ext.getCmp('bill_dt').getValue()));
            //string s = string.Empty;
            //string ori = RightExp;

            //s = "@opt@.setValue(@ori@);";

            //foreach (var right in Right)
            //{
            //    ori = ori.Replace(right,
            //                      string.Format("Ext.getCmp('{0}').getValue()",
            //                                    right.Split('.').Length > 1 ? right.Split('.')[1].Trim() : right.Trim()));
            //}

            //s = s.Replace("@opt@", string.Format("Ext.getCmp('{0}')", Left[0].Split('.')[1].Trim()));
            //s = s.Replace("@ori@", ori);


            /* 不支持公式前先注释掉
            //Ext.getCmp('longcol_1').setValue(AF.func("year",Ext.getCmp('bill_dt').getValue()));
            string s = string.Empty;
            string func = string.Empty;
            string ori = RightExp;
            ori = ori.Substring(ori.IndexOf("(") + 1, ori.LastIndexOf(")") - ori.IndexOf("(") - 1);

            func = Expression.Substring(Expression.IndexOf("=") + 1,
                                        Expression.IndexOf("(") - Expression.IndexOf("=") - 1);

            s = "@opt@.setValue(AF.func(\"@func@\",@ori@));";

            foreach (var right in Right)
            {
                ori = ori.Replace(right,
                                  string.Format("Ext.getCmp('{0}').getValue()",
                                                right.Split('.').Length > 1 ? right.Split('.')[1].Trim() : right.Trim()));
            }

            s = s.Replace("@opt@", string.Format("Ext.getCmp('{0}')", Left[0].Split('.')[1].Trim()));
            s = s.Replace("@func@", func.Trim());
            s = s.Replace("@ori@", ori);
            */


            //p_form0000000110_m.textcol_3 = p_form0000000110_m.textcol_1 + p_form0000000110_m.textcol_2
            string s = string.Empty;
            string exp = string.Empty;

            foreach (var left in Left)
            {
                exp = string.Format(Expression.Replace(left,
                                                     string.Format("Ext.getCmp('{0}').setValue",
                                                                   left.Split('.')[1].Trim())));
            }

            foreach (var right in Right)
            {
                exp = string.Format(exp.Replace(right, string.Format("Ext.getCmp('{0}').getValue()", right.Split('.')[1].Trim())));
            }

            exp = exp.Replace("=", "(");
            exp += ");";

            string funcname = string.Empty;
            if (col[colname].Xtype == "ngCustomFormHelp" || col[colname].Xtype == "ngRichHelp")
            {
                funcname = "helpselected";
            }
            else
            {
                funcname = "itemchanged";
            }

            s += string.Format(@"Ext.getCmp('{0}').addListener('{1}', function()", colname, funcname);
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
