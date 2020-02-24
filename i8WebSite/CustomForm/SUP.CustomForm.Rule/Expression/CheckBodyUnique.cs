using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;

namespace SUP.CustomForm.Rule.Expression
{
    /// <summary>
    /// 全局事件：BillBeforeSaveEvent
    /// 对应类型：NG3.Metadata.UI.PowserBuilder.Events.Implementation.PbExpressionType.CheckBodyUnique
    /// p_form0000000083_d.numericcol_1,p_form0000000083_d.longcol_1;0
    ///  
    /// </summary>
    public  class CheckBodyUnique : ExpressionBase
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

        public CheckBodyUnique()
        {
 
        }

        public CheckBodyUnique(PbExpressionImp pbImp)
        {
            this.Expression = pbImp.Expression;
            this.Initial(pbImp.Expression);
        }

        private void Initial(string exp)
        {
            string[] s = exp.Split(';');
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
            //    p_form0000000083_d.numericcol_1,p_form0000000083_d.longcol_1;0
            //    select numericcol_1,longcol_1 from p_form0000000083_d where numericcol_1 = 'xxx' or longcol_1 = 'xxx' 
            //    ……
            //    var TableName = "p_form0000000083_m";
            //    var Fields = "numericcol_1:'"+Ext.getCmp('userdefine_1').getValue()+"',longcol_1:'"+Ext.getCmp('longcol_1').getValue()+"'"
            //前台     + "|" + "numericcol_1:'"+Ext.getCmp('userdefine_1').getValue()+"',longcol_1:'"+Ext.getCmp('longcol_1').getValue()+"'";
            //    var Together = "1";
            //    Ext.Ajax.request({
            //       params: { 'TableName': TableName,'Fields': Fields, 'Together': Together },
            //       url: C_ROOT + 'SUP/CustomExpression/GetSqlValue',
            //       success: function (response) {    
            //           var resp = Ext.JSON.decode(response.responseText);
            //           if(resp.status == "ok"){
            //           }
            //        }
            //    });
            if (string.IsNullOrEmpty(Expression))
                return string.Empty;

            string s = string.Empty;
            string TableName = Expression.Split('.')[0];
            string Together = Expression.Split(';')[1];
            string Fields = Expression.Split(';')[0];
            foreach (var right in Right)
            {
                Fields = Fields.Replace(right, string.Format("{0}:'\" + item.data.{0} + \"'", right.Split('.')[1].Trim()));
            }

            s += string.Format("var TableName = \"{0}\";\r\n", TableName);
            s += string.Format("var Fields = \"\";\r\n");
            s += string.Format("var res = false;\r\n");

            s += "Ext.Array.each(listdwstore.data.items, function (item){";
            s += string.Format("\r\n");
            s += "    Fields += \"";
            s += Fields;
            s += "|\";";
            s += string.Format("\r\n");
            s += "});" + string.Format("\r\n");

            s += "Fields = Fields.substring(0,Fields.length-1);";
            s += string.Format("var Together = \"{0}\";\r\n", Together);
            s += "Ext.Ajax.request({" + string.Format("\r\n");
            s += "    params: { 'TableName': TableName,'Fields': Fields, 'Together': Together }," + string.Format("\r\n");
            s += "    url: C_ROOT + 'SUP/CustomExpression/UniqueCheck'," + string.Format("\r\n");
            s += "    async: false," + string.Format("\r\n");
            s += "    success: function (response) {" + string.Format("\r\n");
            s += "        var resp = Ext.JSON.decode(response.responseText);" + string.Format("\r\n");
            s += "        if(resp.status == \"ok\"){" + string.Format("\r\n");
            s += "          	res=true;" + string.Format("\r\n");
            s += "        }else{" + string.Format("\r\n");
            s += "              Ext.MessageBox.alert('提示','表单中第'+resp.message+'行数据已存在');" + string.Format("\r\n");
            s += "			    res=false;" + string.Format("\r\n");
            s += "        }" + string.Format("\r\n");
            s += "     }" + string.Format("\r\n");
            s += "});" + string.Format("\r\n");
            s += "return res;";

            return s;
        }

    }
}
