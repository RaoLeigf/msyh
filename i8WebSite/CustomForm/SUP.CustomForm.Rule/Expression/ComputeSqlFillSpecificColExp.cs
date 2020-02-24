using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;
using gudusoft.gsqlparser;

namespace SUP.CustomForm.Rule.Expression
{

    /// <summary>
    ///change事件
    ///如：p_form0000000083_m.textcol_1 = (select itemname from itemdata where itemno= p_form0000000083_m.userdefine_1 )
    /// </summary>
    public class ComputeSqlFillSpecificColExp : ExpressionBase
    {

        public ComputeSqlFillSpecificColExp()
        {

        }

        public ComputeSqlFillSpecificColExp(PbExpressionImp pbImp)
        {
            this.Expression = pbImp.Expression;
            this.Expression = Expression.Replace("&", ",");
            this.Initial(pbImp.Expression);
        }

        private void Initial(string exp)
        {
            //string exp = "p_form0000000083_m.textcol_1 = (select itemname from itemdata where itemno= p_form0000000083_m.userdefine_1 )";

            int index = exp.IndexOf('=');
            string left = exp.Substring(0, index);
            string right = exp.Substring(index + 1);

            this.Left.Add(left);
            //List<string> ls = null;
            TGSqlParser sqlparser = new TGSqlParser(TDbVendor.DbVMssql);
            sqlparser.SqlText.Text = right;
            int i = sqlparser.Parse();
            if (i == 0)
            {
                WhereCondition w = new WhereCondition(sqlparser.SqlStatements[0].WhereClause);
                //ls = w.getControlIds();
                this.Right = w.getControlIds();
            }
        }

        public override string GetCodeFromExp(Dictionary<string, string> dic)
        {
            return string.Empty;
        }

        /// <summary>
        /// 表达式转换成脚本代码
        /// </summary>
        /// <returns></returns>
        public string GetCodeFromExp(Dictionary<String, String> dic, Dictionary<string, Col> col, string colname, string loctype)
        {
            /*
            1、p_form0000000083_m.textcol_1 = (select itemname from itemdata where itemno = p_form0000000083_m.userdefine_1 )
            2、p_form0000000083_m.textcol_1 = (select itemname from itemdata where itemno = 'xyp' )
            
            Ext.getCmp('userdefine_1').addListener('change',function(){
                var sqlstr = " (select itemname from itemdata where itemno= '"+ Ext.getCmp('userdefine_1').getValue() + "' )";
                Ext.Ajax.request({
                    params: { 'sql': sqlstr },
                    url: C_ROOT + 'SUP/CustomExpression/GetSqlValue',
                    success: function (response) {
                        var resp = Ext.JSON.decode(response.responseText);
                        if(resp.status == "ok"){
                            Ext.getCmp('textcol_1').setValue(resp.value);
                        }
                     }
                });
            });

            string s = string.Empty;
            s += string.Format("Ext.getCmp('{0}').addListener('change',function()", Left[0].Split('.')[1].Trim());
            s += "{";
            s += string.Format("\r\n");
            s += "      var sqlstr = \"" + sqlstring.Substring(sqlstring.IndexOf('=') + 1) + "\";";
            s += string.Format("\r\n");
            s += @"     Ext.Ajax.request({
        params: { 'sql': sqlstr },";
            s += "      url: C_ROOT + 'SUP/CustomExpression/GetSqlValue',";
            s += "      success: function (response) {";
            s += "          var resp = Ext.JSON.decode(response.responseText);";
            s += string.Format("\r\n\t\tExt.getCmp('{0}').setValue(resp.value);\r\n", Left[0].Split('.')[1].Trim());
            s += @"         }
    });
});";
            */

            if (string.IsNullOrEmpty(Expression))
                return string.Empty;

            string sqlstring = Expression;
            //foreach (var right in Right)
            //{
            //    if (right.Contains("p_form") == true && right.Contains(".") == true)
            //    {
            //        sqlstring = sqlstring.Replace(right, string.Format("'\" + Ext.getCmp('{0}').getValue() + \"'", right.Split('.')[1].Trim()));
            //    }
            //}

            string tablename = Left[0].Split('.')[0].Trim();

            foreach (var vv in col)
            {
                string fullname = tablename + "." + vv.Key;

                do
                {
                    if (sqlstring.Contains(fullname))
                    {
                        sqlstring = sqlstring.Replace(fullname, string.Format("'\" + Ext.getCmp('{0}').getValue() + \"'", fullname.Split('.')[1].Trim()));
                    }
                    else
                    {
                        break;
                    }

                } while (true);
            }

            string s = string.Empty;
            s += "      var sqlstr = \"" + sqlstring.Substring(sqlstring.IndexOf('=') + 1) + "\";";
            s += string.Format("\r\n");
            s += @"     Ext.Ajax.request({
        params: { 'sql': sqlstr },";
            s += "      url: C_ROOT + 'SUP/CustomExpression/GetSqlValue',";
            s += "      success: function (response) {";
            s += "          var resp = Ext.JSON.decode(response.responseText);";
            s += string.Format("\r\n\t\tExt.getCmp('{0}').setValue(resp.value);\r\n", Left[0].Split('.')[1].Trim());
            s += @"         }
    });";

            //如果是按钮更新事件，添加itemchanged事件
            if (loctype == "Normal")
            {
                string funcname = string.Empty;
                if (col[colname].Xtype == "ngCustomFormHelp" || col[colname].Xtype == "ngRichHelp")
                {
                    funcname = "helpselected";
                }
                else
                {
                    funcname = "itemchanged";
                }


                s = string.Format("Ext.getCmp('{0}').addListener('{1}',function() ", colname, funcname) + "{ \r\n" + s;
                s += "\r\n});" + string.Format("\r\n");
            }

            return s;
        }

        /// <summary>
        /// 表达式转换成脚本代码
        /// </summary>
        /// <returns></returns>
        public string GetCodeFromExpGrid(Dictionary<String, String> dic, Dictionary<string, Col> col, string colname, string tablename)
        {
            /*
            1、p_form0000000083_d.textcol_1 = (select itemname from itemdata where itemno = p_form0000000083_d.userdefine_1 )
            
            if (e.field == 'userdefine_1')"
            {
                var data = p_form0000000083_d.getSelectionModel().getSelection();

                var sqlstr = " (select itemname from itemdata where itemno= '"+ data[0].get('userdefine_1') + "' )";
                Ext.Ajax.request({
                    params: { 'sql': sqlstr },
                    url: C_ROOT + 'SUP/CustomExpression/GetSqlValue',
                    success: function (response) {
                        var resp = Ext.JSON.decode(response.responseText);
                        if(resp.status == "ok"){
                            data[0].set('textcol_1', resp.value);
                        }
                    }
                });
            }

            */

            if (string.IsNullOrEmpty(Expression))
                return string.Empty;

            string sqlstring = Expression;

            foreach (var vv in col)
            {
                string fullname = tablename + "." + vv.Key;

                do
                {
                    if (sqlstring.Contains(fullname))
                    {
                        sqlstring = sqlstring.Replace(fullname, string.Format("'\" + e.record.get('{0}') + \"'", fullname.Split('.')[1].Trim()));
                    }
                    else
                    {
                        break;
                    }

                } while (true);
            }

            string s = string.Empty;
            s += string.Format("\r\n" + "\t" + "if (e.field == '{0}' || e.field == '{0}_name') ", colname) + "{";
            s += "\r\n" + "\t\t" + "var sqlstr = \"" + sqlstring.Substring(sqlstring.IndexOf('=') + 1) + "\";";
            s += string.Format("\r\n");
            s += @"     Ext.Ajax.request({
                        params: { 'sql': sqlstr },";
            s += "      url: C_ROOT + 'SUP/CustomExpression/GetSqlValue',";
            s += "      success: function (response) {";
            s += "          var resp = Ext.JSON.decode(response.responseText);";
            s += string.Format("\r\n\t\te.record.set('{0}', resp.value);\r\n", Left[0].Split('.')[1].Trim());
            s += @"         }
                 });";
            s += "\r\n" + "}";


            ////如果是按钮更新事件，添加itemchanged事件
            //if (loctype == "Normal")
            //{
            //    string funcname = string.Empty;
            //    if (col[colname].Xtype == "ngCustomFormHelp" || col[colname].Xtype == "ngRichHelp")
            //    {
            //        funcname = "helpselected";
            //    }
            //    else
            //    {
            //        funcname = "itemchanged";
            //    }


            //    s = string.Format("Ext.getCmp('{0}').addListener('{1}',function() ", colname, funcname) + "{ \r\n" + s;
            //    s += "\r\n});" + string.Format("\r\n");
            //}

            return s;
        }
    }

}
