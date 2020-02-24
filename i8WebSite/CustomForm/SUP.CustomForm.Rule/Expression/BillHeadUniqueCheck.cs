using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;

namespace SUP.CustomForm.Rule.Expression
{
    /// <summary>
    /// BillBeforeSaveEvent
    /// 对应类型：NG3.Metadata.UI.PowserBuilder.Events.Implementation.PbExpressionType.BillHeadUniqueCheck
    /// 表头字段唯一性验证，最后的表达式为1表示两个字段组合起来做唯一性验证，为0表示单独验证
    /// 表达式：p_form0000000083_m.numericcol_1,p_form0000000083_m.longcol_1;1
    /// 
    /// </summary>
    public class BillHeadUniqueCheck : ExpressionBase
    {

        private bool isUnion = false;

        /// <summary>
        /// 多字段联合判断唯一性
        /// </summary>
        public bool IsUnion
        {
            get { return isUnion; }
            set { isUnion = value; }
        }

        public BillHeadUniqueCheck()
        {
 
        }

        public BillHeadUniqueCheck(PbExpressionImp pbImp)
        {
            this.Expression = pbImp.Expression;
            this.Initial(pbImp.Expression);
        }

        private void Initial(string exp)
        {

            string [] s = exp.Split(';');

            string param = s[0];
            string flag = s[1];

            if (flag == "1")
            {
                this.IsUnion = true;
            }

            string[] ps = param.Split(',');

            this.Right.AddRange(ps);
        }

        public override string GetCodeFromExp(Dictionary<string, string> dic)
        {
            return string.Empty;
        }

        public  string GetCodeFromExp(Dictionary<string, string> dic,Dictionary<string,Col> col)
        {
            //    p_form0000000083_m.numericcol_1,p_form0000000083_m.longcol_1;1
            //    select numericcol_1,longcol_1 from p_form0000000083_m where numericcol_1 = 'xxx' and longcol_1 = 'xxx' 
            //    TableName ;  				Fields ;   							Together
            //    p_form0000000083_m         numericcol_1:'xxx';longcol_1:'aaa'      1

            //var TableName = "p_form0000000083_m";
            //var Fields = "numericcol_1:'"+Ext.getCmp('userdefine_1').getValue()+"',longcol_1:'"+Ext.getCmp('longcol_1').getValue()+"'";
            //var Together = "1";
            //Ext.Ajax.request({
            //    params: { 'TableName': TableName,'Fields': Fields, 'Together': Together },
            //    url: C_ROOT + 'SUP/CustomExpression/GetSqlValue',
            //    success: function (response) {    
            //        var resp = Ext.JSON.decode(response.responseText);
            //        if(resp.status == "ok"){

            //        }
            //     }
            //});
            if (string.IsNullOrEmpty(Expression))
                return string.Empty;

            string s = string.Empty;
            string TableName = Expression.Split('.')[0];
            string Together = Expression.Split(';')[1];
            string Fields = Expression.Split(';')[0];
            string tip = string.Empty;

            foreach (var right in Right)
            {
                tip = tip + col[right.Split('.')[1]].Name.Split(':')[0] + string.Format("' + Ext.getCmp('{0}').getValue() + '", right.Split('.')[1].Trim()) + (Together.Equals("1") ? " 和" : " 或");
            }
            tip = tip.Substring(0, tip.Length - 1);

            foreach (var right in Right)
            {
                Fields = Fields.Replace(right, string.Format("{0}:'\" + Ext.getCmp('{0}').getValue() + \"'", right.Split('.')[1].Trim()));
            }

            s += string.Format("var TableName = \"{0}\";\r\n", TableName);
            s += string.Format("var Fields = \"{0}\";\r\n", Fields);
            s += string.Format("var Together = \"{0}\";\r\n",Together);
            s += string.Format("var res = false;\r\n");
            s += "Ext.Ajax.request({" + string.Format("\r\n");
            s += "    params: { 'TableName': TableName,'Fields': Fields, 'Together': Together }," + string.Format("\r\n");
            s += "    url: C_ROOT + 'SUP/CustomExpression/UniqueCheck'," + string.Format("\r\n");
            s += "    async: false," + string.Format("\r\n");
            s += "    success: function (response) {" + string.Format("\r\n");
            s += "        var resp = Ext.JSON.decode(response.responseText);" + string.Format("\r\n");
            s += "        if(resp.status == \"ok\"){" + string.Format("\r\n");
            s += "          	res = true;" + string.Format("\r\n");
            s += "        }else{" + string.Format("\r\n");
            s += string.Format("              Ext.MessageBox.alert('提示', '{0}已存在');\r\n",tip);
            s += "			    res = false;" + string.Format("\r\n");
            s += "        }" + string.Format("\r\n");
            s += "     }" + string.Format("\r\n");
            s += "});" + string.Format("\r\n");
            s += "return res;" + string.Format("\r\n");
            
            return s;
        }
    }
}
