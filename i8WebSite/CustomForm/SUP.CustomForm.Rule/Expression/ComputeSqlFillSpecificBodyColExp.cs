using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;
using gudusoft.gsqlparser;

namespace SUP.CustomForm.Rule.Expression
{
      /// <summary>
     /// select itemname as  textcol_1,ordercycle  as longcol_1 from itemdata where itemno= p_form0000000083_m.userdefine_1;10;p_form0000000083_d
     /// 注：获取数据填入p_form0000000083_d对应的grid
    ///表达式类型部分:00表示覆盖，01表示追加；11表示弹出帮助供用户选择再追加到grid；10表示弹出帮助供用户选择后覆盖到grid。
    ///弹出帮助的列跟p_form0000000083_d表 
    /// </summary>
    public class ComputeSqlFillSpecificBodyColExp : ExpressionBase
    {

        private string sql = string.Empty;
        private ActionType actionType = ActionType.Append;
        private string tableName = string.Empty;
        private List<string> helpFields = new List<string>();
       
        public ComputeSqlFillSpecificBodyColExp(PbExpressionImp pbImp)
        {
            this.Expression = pbImp.Expression;
            this.Expression = Expression.Replace("&", ",");
            this.Initial(pbImp.Expression);            
        }

        /// <summary>
        /// sql语句
        /// </summary>
        public string Sql
        {
            get { return sql; }
            set { sql = value; }
        }

        /// <summary>
        /// 执行的动作类型
        /// </summary>
        public ActionType ActionType
        {
            get { return actionType; }
            set { actionType = value; }
        }

        /// <summary>
        /// 表名，用于定位grid
        /// </summary>
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }

        public List<string> HelpFields
        {
            get { return helpFields; }
            set { helpFields = value; }
        }


        private void Initial(string exp)
        {
            //string exp = "select itemname as  textcol_1,ordercycle  as longcol_1 from itemdata where itemno= p_form0000000083_m.userdefine_1;10;p_form0000000083_d";

            string[] s = exp.Split(';');

            string sql = s[0];
            string action = s[1];
            string tableName = s[2];

            this.Sql = sql;
            //处理sql语句          
            TGSqlParser sqlparser = new TGSqlParser(TDbVendor.DbVMssql);
            sqlparser.SqlText.Text = sql;
            int i = sqlparser.Parse();
            if (i == 0)
            {
                WhereCondition w = new WhereCondition(sqlparser.SqlStatements[0].WhereClause);
                this.Right = w.getControlIds();
            }
            
            //处理action
            switch (action)
            {
                case "00": this.ActionType = Rule.Expression.ActionType.Override;
                    break;
                case "01": this.ActionType = Rule.Expression.ActionType.Append;
                    break;
                case "11": this.ActionType = Rule.Expression.ActionType.SelectAppend;
                    break;
                case "10": this.ActionType = Rule.Expression.ActionType.SelectOverride;
                    break;
                default: this.ActionType = Rule.Expression.ActionType.Append;
                    break;
            }

            //处理表名
            this.TableName = tableName;

            //帮助的字段
            this.HelpFields = GetColumnAlia(sql);
        }

        private  List<string> GetColumnAlia(string exp)
        {

            //string exp = "select itemname as  textcol_1,ordercycle  as longcol_1, name myname from itemdata where itemno= p_form0000000083_m.userdefine_1";
            List<string> list = new List<string>();
            TGSqlParser sqlparser = new TGSqlParser(TDbVendor.DbVMssql);
            sqlparser.SqlText.Text = exp;
            int ret = sqlparser.Parse();
            if (ret == 0)
            {
                TSelectSqlStatement sqlstmt;
                sqlstmt = (TSelectSqlStatement)sqlparser.SqlStatements[0];
                //sqlstmt.Fields
                foreach (TLzField field in sqlstmt.Fields)
                {
                    //Console.WriteLine("field alia: " + field.FieldAlias);
                    if (string.IsNullOrEmpty(field.FieldAlias))
                    {
                        list.Add(field.Name);
                    }
                    else
                    {
                        list.Add(field.FieldAlias);
                    }
                }
            }

            return list;
        }

        public override string GetCodeFromExp(Dictionary<string, string> dic)
        {
            return string.Empty;
        }

        public string GetCodeFromExp(Dictionary<string, string> dic, Dictionary<string, Col> col, string colname)
        {
            /* 无选择覆盖
            Ext.getCmp('textcol_2').addListener('itemchanged', function () {
                var textcol_2help = Ext.create('Ext.ng.CustFormMultiHelp', {
                    valueField: 'textcol_2',
                    helpFieldslist: 'textcol_1',
                    helpHeadTextList: '11文本'
                });
                textcol_2help.sqldt = "(select cname as textcol_1 from hr_epm_main where cno =  '" + Ext.getCmp('textcol_2').getValue() + "')";
                Ext.Ajax.request({
                    url: C_ROOT + 'SUP/CustomExpression/GetSqlValue?sqldt=' + textcol_2help.sqldt,
                    async: false,
                    success: function (response) {
                        var resp = Ext.JSON.decode(response.responseText);
                        p_form0000000110_dstore.removeAll();
                        p_form0000000110_dstore.add(resp.Record);
                    }
                });
            });                          
            */

            /* 选择后覆盖
            Ext.getCmp('textcol_2').addListener('itemchanged', function () {
                var textcol_2help = Ext.create('Ext.ng.CustFormMultiHelp', {
                    valueField: 'textcol_2',
                    helpFieldslist: 'textcol_1',
                    helpHeadTextList: '11文本',
                    listeners: {
                        helpselected: function (obj) {
                            p_form0000000110_dstore.removeAll();
                            p_form0000000110_dstore.add(obj.data);
                        }
                    }
                });
                textcol_2help.sqldt = "(select cname as textcol_1 from hr_epm_main where cno =  '" + Ext.getCmp('textcol_2').getValue() + "')";
                textcol_2help.showHelp();
            });
            */

            string s = string.Empty;

            if (col[colname].Xtype == "ngCustomFormHelp" || col[colname].Xtype == "ngRichHelp")
            {
                s = string.Format("Ext.getCmp('{0}').addListener('helpselected',function()", colname);
            }
            else
            {
                s = string.Format("Ext.getCmp('{0}').addListener('itemchanged',function()", colname);
            }
            s += "{" + string.Format("\r\n");

            string sqlstring = Sql;
            foreach (var right in Right)
            {
                //where 条件值是常量的话就不用替换
                if (right.Contains(".") == true)
                {
                    sqlstring = sqlstring.Replace(right, string.Format("'\" + Ext.getCmp('{0}').getValue() + \"'", right.Split('.')[1].Trim()));
                }
            }
            sqlstring = colname + "help.sqldt = \"(" + sqlstring + ")\";";

            string helpstring = string.Empty;
            helpstring = "var " + colname + "help = Ext.create('Ext.ng.CustFormMultiHelp', {" +
                             string.Format("\r\n");
            helpstring += string.Format("    valueField: '{0}',\r\n", colname);

            //HelpFields
            string helpfieldsstr = string.Empty;
            string textstr = string.Empty;
            foreach (var str in HelpFields)
            {
                helpfieldsstr += str + ",";
                textstr += col[str].Name + ",";
            }

            helpstring += string.Format("    helpFieldslist:'{0}',\r\n",
                                        helpfieldsstr.Substring(0, helpfieldsstr.Length - 1));
            helpstring += string.Format("    helpHeadTextList:'{0}'", textstr.Substring(0, textstr.Length - 1));

            if (actionType == ActionType.Override || actionType == ActionType.Append)
            {
                helpstring += string.Format("\r\n");
                helpstring += "});";

                string requeststr = string.Empty;
                requeststr = "    Ext.Ajax.request({" + string.Format("\r\n");
                requeststr += "        url: C_ROOT + 'SUP/CustomExpression/GetSqlValue?sqldt='+" + colname + "help.sqldt," +
                              string.Format("\r\n");
                requeststr += "        async: false," + string.Format("\r\n");
                requeststr += "        success: function (response) {" + string.Format("\r\n");
                requeststr += "            var resp = Ext.JSON.decode(response.responseText);" + string.Format("\r\n");

                if (actionType == ActionType.Override)
                {
                    requeststr += string.Format("            {0}store.removeAll();\r\n", tableName); //所有地方原来都是dic[tableName]
                }

                requeststr += string.Format("            {0}store.add(resp.Record);\r\n", tableName); 

                requeststr += "         }" + string.Format("\r\n");
                requeststr += "    });";

                s += helpstring + string.Format("\r\n");
                s += sqlstring + string.Format("\r\n");
                s += requeststr + string.Format("\r\n");
            }
            else
            {
                helpstring += string.Format(",\r\n");
                helpstring += "    listeners: {" + string.Format("\r\n");
                helpstring += "        helpselected: function (obj) {" + string.Format("\r\n");

                if (actionType == ActionType.SelectOverride)
                {
                    helpstring += string.Format("            {0}store.removeAll();\r\n", tableName);
                }

                helpstring += "            Ext.Array.each(obj.data, function (record) {" + string.Format("\r\n");
                helpstring += string.Format(
                    "                {0}store.add(record.data);\r\n", tableName);
                helpstring += "            });" + string.Format("\r\n");

                helpstring += "        }" + string.Format("\r\n");
                helpstring += "    }" + string.Format("\r\n");
                helpstring += "});";
                
                s += helpstring + string.Format("\r\n");
                s += sqlstring + string.Format("\r\n");
                s += colname + "help.showHelp();" + string.Format("\r\n");
            }

            s += "});" + string.Format("\r\n");

            return s;
        }
    }

    public enum ActionType
    {
        Override,//覆盖00
        Append,//追加01
        SelectAppend,//11,弹出帮助供用户选择再追加到grid
        SelectOverride//10,弹出帮助供用户选择后覆盖到grid
    }
}
