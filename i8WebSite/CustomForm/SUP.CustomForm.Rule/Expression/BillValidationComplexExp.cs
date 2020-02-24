using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;
using System.Text.RegularExpressions;

namespace SUP.CustomForm.Rule.Expression
{
    /// <summary>
    /// 全局事件：BillBeforeSaveEvent
    /// 对应类型：NG3.Metadata.UI.PowserBuilder.Events.Implementation.PbExpressionType.BillValidation
    /// 
    /// 表达式示例2:select itemname from itemdata where itemno =  getitemvalue( p_form0000000083_m.userdefine_1),内容不相等,0,=,p_form0000000083_m.textcol_1,p_form0000000083_m.code&p_form0000000083_m.longcol_1&p_form0000000083_m.longcol_2
    /// 功能： sql语句获得一个值，跟后面的sum(textcol_1)比较，运算符有">,<,=,=>,<=",
    /// textcol_1后面的表达式表示:从表p_form0000000083_m中获取code,longcol_1,longcol_2三个字段相等的那些行，把获取到的这些行的textcol_1字段汇总的值和sql语句得到的值比较,
    /// 即：判断 getValue(sql) == sum(select textcol_1 from p_form0000000083_m where code= longcol_1 and code=longcol_2) 
    /// </summary>
    public class BillValidationComplexExp : ExpressionBase
    {
        private string sql = string.Empty;//表达式左sql语句
        private string hint = string.Empty;//错误提示信息 
        private bool immediateCheck = false;//是否changed实时验证,false是提交时验证
        private string opera = string.Empty;//运算符
        private string sumColumn = string.Empty;//汇总运算字段
        private string whereExp = string.Empty;//右边sql语句的where语句


        public BillValidationComplexExp()
        {

        }

        public BillValidationComplexExp(PbExpressionImp pbImp)
        {
            this.Expression = pbImp.Expression;
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
        /// 错误提示信息
        /// </summary>
        public string Hint
        {
            get { return hint; }
            set { hint = value; }
        }

        /// <summary>
        /// 是否changed实时验证,false是提交时验证
        /// </summary>
        public bool ImmediateCheck
        {
            get { return immediateCheck; }
            set { immediateCheck = value; }
        }

        /// <summary>
        /// 运算符
        /// </summary>
        public string Opera
        {
            get { return opera; }
            set { opera = value; }
        }

        /// <summary>
        /// 汇总运算字段
        /// </summary>
        public string SumColumn
        {
            get { return sumColumn; }
            set { sumColumn = value; }
        }

        public void Initial(string exp)
        {
            string[] s = exp.Split(',');

            this.Sql = s[0];//sql
            this.Hint = s[1];//错误提示
            if (s[2] == "1")//是否changed时提醒
            {
                this.ImmediateCheck = true;
            }

            this.Opera = s[3];//运算符
            this.SumColumn = s[4];//汇总字段
            this.whereExp = s[5];//where

            SetSqlParam(this.Sql, @"\(", @"\)");//放入Left

        }

        private void SetSqlParam(string str, string start, string end)
        {
            Regex rg = new Regex("(?<=(" + start + "))[.\\s\\S]*?(?=(" + end + "))", RegexOptions.Multiline | RegexOptions.Singleline);

            MatchCollection macths = rg.Matches(str);
            List<string> ls = new List<string>();
            for (int i = 0; i < macths.Count; i++)
            {
                this.Left.Add(macths[i].Value);
            }

        }

        public string GetCodeFromExp(Dictionary<string, string> dic, Dictionary<string, Col> col, string colname)
        {
            //var leftsql = "select itemname from itemdata  where itemno= '" + Ext.getCmp('userdefine_1').getValue() + "' )";
            //var sum = "p_form0000000083_m.textcol_1";
            //var where = "p_form0000000083_m.code&p_form0000000083_m.longcol_1&p_form0000000083_m.longcol_2";
            //var opera= "=";
            //var tablename = "p_form0000000083_m";

            //Ext.Ajax.request({
            //    params: { 'leftsql': leftsql,'sum': sum,'opera': opera, 'where': where, 'tablename': tablename },      
            //    url: C_ROOT + 'SUP/CustomExpression/ValidationComplex',      
            //    success: function (response) {          
            //        var resp = Ext.JSON.decode(response.responseText);
            //        if(resp.status == "true")
            //            return true;
            //        else{
            //            Ext.MessageBox.alert('提示', '内容不等相等');
            //            return false;
            //        }
            //    }
            //});	

            foreach (var left in Left)
            {
                Sql = Sql.Replace("getitemvalue(" + left + ")", string.Format("'\" + Ext.getCmp('{0}').getValue() + \"'", left.Split('.')[1].Trim()));
            }

            string[] wherestr = whereExp.Split('&');
            foreach (var str in wherestr)
            {
                whereExp = whereExp.Replace(str,
                                            string.Format("{0}='\" + Ext.getCmp('{1}').getValue() + \"'", str,
                                                          str.Split('.')[1]));
            }

            var s = string.Empty;
            var expression_string = string.Empty;

            s += string.Format("var leftsql = \"{0}\";\r\n", Sql);
            s += string.Format("var sum = \"{0}\";\r\n", sumColumn);
            s += string.Format("var where = \"{0}\";\r\n", whereExp);
            s += string.Format("var opera= \"{0}\";\r\n", Opera);
            s += string.Format("var tablename = \"{0}\";\r\n", sumColumn.Split('.')[0].Trim());
            s += string.Format("var res =false;\r\n\r\n");
            s += "Ext.Ajax.request({" + string.Format("\r\n");
            s +=
                "    params: { 'leftsql': leftsql,'sum': sum,'opera': opera, 'where': where, 'tablename': tablename }," +
                string.Format("\r\n") + string.Format("\r\n");
            s += "    url: C_ROOT + 'SUP/CustomExpression/ValidationComplex', " + string.Format("\r\n");
            s += "    async: false," + string.Format("\r\n");
            s += "    success: function (response) {" + string.Format("\r\n");
            s += "        var resp = Ext.JSON.decode(response.responseText);" + string.Format("\r\n");
            s += "        if(resp.status == \"true\")" + string.Format("\r\n");
            s += "            res = true;" + string.Format("\r\n");
            s += "        else{" + string.Format("\r\n");
            s += "            Ext.MessageBox.alert('提示', '内容不等相等');" + string.Format("\r\n");
            s += "            res = false;" + string.Format("\r\n");
            s += "        }" + string.Format("\r\n");
            s += "    }" + string.Format("\r\n");
            s += "});" + string.Format("\r\n");
            s += string.Format("return res;\r\n");


            if (!immediateCheck)
            {
                return s;
            }
            else
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

                expression_string = string.Format("Ext.getCmp('{0}').addListener('{1}',function()", Left[0].Split('.')[1].Trim(), funcname);
                expression_string += "{" + string.Format("\r\n");
                expression_string += s;
                expression_string += "});" + string.Format("\r\n");

                return expression_string;
            }
        }

        public override string GetCodeFromExp(Dictionary<string, string> dic)
        {
            return string.Empty;
        }
    }
}
