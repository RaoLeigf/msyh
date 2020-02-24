using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;

namespace SUP.CustomForm.Rule.Expression
{
    /// <summary>
    /// keyup事件
    ///p_form0000000083_m.numericcol_1= (p_form0000000083_m.numericcol_2+ p_form0000000083_m.numericcol_3)*2+3
    /// </summary>
    public class ComputeFuncFillSpecificColExp : ExpressionBase
    {

        public ComputeFuncFillSpecificColExp()
        {

        }

        public ComputeFuncFillSpecificColExp(PbExpressionImp pbImp)
        {
            if (pbImp.ExpressionType != PbExpressionType.ComputeFuncAndFillSpecificCol)
            {
                throw new Exception("PbExpressionType is not ComputeFuncAndFillSpecificCol !");
            }
            this.Expression = pbImp.Expression;
            this.Initial(pbImp.Expression);
        }

        private void Initial(string exp)
        {
            //string exp = "p_form0000000083_m.numericcol_1= (p_form0000000083_m.numericcol_2+ p_form0000000083_m.numericcol_3)*2+3";

            string[] s = exp.Split('=');
            string left = s[0];//左值
            string right = s[1];//右值
            string rexp = right.Replace("=", ",").Replace("+", ",").Replace("-", ",").Replace("*", ",").Replace("/", ",").Replace("(", ",").Replace(")", ",");

            string[] vs = rexp.Split(',');

            this.Left.Add(left);

            for (int i = 0; i < vs.Length; i++)
            {
                if (vs[i].IndexOf(".") > 0)
                {
                    this.Right.Add(vs[i].Trim());
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

            string exp = Expression;
            string s = string.Empty;
            string tablename = string.Empty;//转grid名称
            string funcname = string.Empty;            

            foreach (var left in Left)
            {
                exp = string.Format(exp.Replace(left, string.Format("Ext.getCmp('{0}').setValue", left.Split('.')[1].Trim())));
            }

            foreach (var right in Right)
            {
                exp = string.Format(exp.Replace(right, string.Format("Ext.getCmp('{0}').getValue()", right.Split('.')[1].Trim())));
            }
            exp = exp.Replace("=", "(");
            exp += ");";

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

        /// <summary>
        /// 表达式转换成脚本代码
        /// </summary>
        /// <returns></returns>
        public string GetCodeFromExpGrid(Dictionary<String, String> dic, Dictionary<string, Col> col, string colname, string tablename)
        {
            if (string.IsNullOrEmpty(Expression))
                return string.Empty;

            string exp = Expression;
            string s = string.Empty;
            string funcname = string.Empty;

            //下一次触发字段事件
            string changeCol = Left[0].Split('.')[1].Trim();
            if (col[changeCol].Xtype == "ngCustomFormHelp" || col[changeCol].Xtype == "ngRichHelp")
            {
                funcname = "helpselected";
            }
            else
            {
                funcname = "itemchanged";
            }

            var isform = false;   //赋值字段是form列则公式值循环累加
            foreach (var left in Left)
            {
                if (!left.Contains(tablename))
                {
                    isform = true;
                    break;
                }
            }

            //左值右值格式转化
            foreach (var left in Left)
            {
                if (left.Contains(tablename))
                {
                    exp = exp.Replace(left, string.Format("record.set('{0}',", left.Split('.')[1].Trim()));
                }
                else
                {
                    exp = exp.Replace(left, string.Format("Ext.getCmp('{0}').setValue(", left.Split('.')[1].Trim()));
                }
            }

            foreach (var right in Right)
            {
                exp = exp.Replace(right, string.Format("Ext.Number.from(record.get('{0}'),0)", right.Split('.')[1].Trim()));
            }

            //exp = exp.Replace("=", "");
            //exp += ");";

            if (isform == false)
            {
                //p_form0000000130_d.amt = p_form0000000130_d.qty * p_form0000000130_d.prc
                //
                //Ext.getCmp('p_form0000000130_dgrid').getColumn('qty').getEditor().on('itemchanged',function(me, newValue, oldValue, eOpts){
                //    Ext.Array.each(p_form0000000130_dstore.data.items, function (record) {
                //        record.set('amt', record.get('qty') * record.get('prc');
                //    });
                //  });                 

                s = string.Format("if (e.field == '{0}')", colname);
                s += "{";
                s += string.Format("\r\n\t" + "var record = e.record;");
                s += string.Format("\r\n\t" + exp.Replace("=", "") + ");");
                s += string.Format("\r\n");
                s += "};";

                //s = string.Format(
                //        @"Ext.getCmp('{0}grid').getColumn('{1}').getEditor().on('{2}',function(me, newValue, oldValue, eOpts)",
                //        tablename, colname, funcname);
                //s += "{";
                //s += string.Format("\r\n\t");
                //s += string.Format("Ext.Array.each({0}store.data.items, function (record)", tablename);
                //s += "{";
                //s += string.Format("\r\n\t");
                //s += string.Format("\r\n\t\t\t" + exp );
                //s += string.Format("\r\n");
                //s += "    });";
                //s += string.Format("\r\n");
                //s += "});";
            }
            else
            {
                //p_form0000000130_m.amt = p_form0000000130_d.qty * p_form0000000130_d.prc
                //
                //if (e.field == 'qty') {
                //    var sum = 0;
                //    Ext.Array.each(p_form0000000130_dstore.data.items, function (record) {
                //        sum += record.get('qty') * record.get('prc');
                //        
                //    });
                //    Ext.getCmp('amt').setValue(sum);
                //}; 

                s = string.Format("if (e.field == '{0}')", colname);
                s += "{";
                s += string.Format("\r\n\t" + "var sum = 0;");
                s += string.Format("\r\n\t" + "Ext.Array.each({0}store.data.items, function (record)", tablename);
                s += "{";
                s += string.Format("\r\n\t\t\t" + "sum += " + exp.Split('=')[1].Trim() + ";");
                s += string.Format("\r\n");
                s += "    });";
                s += string.Format("\r\n" + exp.Split('=')[0].Trim() + "sum);");
                s += string.Format("\r\n");
                s += string.Format("Ext.getCmp('{0}').fireEvent('{1}');", changeCol, funcname);
                s += string.Format("\r\n");
                s += "};";
            }

            return s;
        }

        public override string GetCodeFromExp(Dictionary<string, string> dic)
        {
            return string.Empty;
        }

    }
}
