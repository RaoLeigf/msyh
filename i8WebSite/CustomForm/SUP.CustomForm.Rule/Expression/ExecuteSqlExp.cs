using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;
using System.Text.RegularExpressions;

namespace SUP.CustomForm.Rule.Expression
{
    /// <summary>
    /// 保存时在同一事务中执行的sql或者存储过程
    /// 1、sql语句：update testtable1 set name=getitemvalue(p_form0000000083_m.userdefine_1) where id=getitemvalue(p_form0000000083_d.textcol_1)
    /// 2、存储过程: exec mytestprocedure1(string|p_form0000000083_d.textcol_1,string|p_form0000000083_m.textcol_1,string|p_form0000000083_m.userdefine_1)
    /// </summary>
    public class ExecuteSqlExp : ExpressionBase
    {

        private List<string> paramType = new List<string>();//参数类型 
        private SqlExpType sqlExpType;

        public SqlExpType SqlExpType
        {
            get { return sqlExpType; }
            set { sqlExpType = value; }
        }

        public List<string> ParamType
        {
            get { return paramType; }
            set { paramType = value; }
        }

        public ExecuteSqlExp()
        {

        }

        public ExecuteSqlExp(PbExpressionImp pbImp)
        {
            this.Expression = pbImp.Expression;
            this.Expression = Expression.Replace("&", ",");
            this.Initial(pbImp.Expression);
        }


        private void Initial(string exp)
        {
            //string exp = "update testtable1 set name=getitemvalue( p_form0000000083_m.userdefine_1 ) where id=getitemvalue(p_form0000000083_d.textcol_1)";
            if (exp.Trim().StartsWith("exec"))
            {
                this.SqlExpType = Rule.Expression.SqlExpType.Procedure;
                SetProcessParam(exp, @"\(", @"\)");
            }
            else
            {
                this.SqlExpType = Rule.Expression.SqlExpType.SqlExp;
                SetSqlParam(exp, @"\(", @"\)");
            }
        }

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

        private void SetProcessParam(string str, string start, string end)
        {
            Regex rg = new Regex("(?<=(" + start + "))[.\\s\\S]*?(?=(" + end + "))", RegexOptions.Multiline | RegexOptions.Singleline);

            Match macth = rg.Match(str);
            string param = macth.Value;
            string[] ps = param.Split(new char[] { '|', ',' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < ps.Length; i++)
            {
                if (i % 2 == 0)
                {
                    this.ParamType.Add(ps[i]);
                }
                else
                {
                    this.Right.Add(ps[i]);
                }

            }
        }

        public override string GetCodeFromExp(Dictionary<String, String> dic)
        {
        //    update testtable1 set name=getitemvalue(p_form0000000083_m.userdefine_1) where id=getitemvalue(p_form0000000083_d.textcol_1)
        //    Ext.Array.each(listdwstore.data.items, function (item) {
        //        sqlstr += "update testtable1 set name='"+item.data['userdefine_1']+"' where id='"+item.data['textcol_1']+"';";
        //    });
            string s = string.Empty;
            if (String.IsNullOrEmpty(Expression))
                return string.Empty;

            string sqlstring = Expression;

            if (!sqlstring.StartsWith("exec"))
            {
                string curcolumn = string.Empty;

                //sql语句
                //更新form列
                foreach (var right in Right)
                {
                    if (sqlstring.Contains("getitemvalue") == true)
                    {
                        curcolumn = string.Format("Ext.getCmp('{0}').getValue()", right.Split('.')[1].Trim());

                        //where条件前的参数则为空的话置为null
                        if (sqlstring.IndexOf("getitemvalue", StringComparison.OrdinalIgnoreCase) < sqlstring.IndexOf("where", StringComparison.OrdinalIgnoreCase))
                        {
                            sqlstring = sqlstring.Replace("getitemvalue(" + right + ")", string.Format("\" + ({0}==''?'null':{0}) + \"", curcolumn));
                        }
                        else
                        {
                            sqlstring = sqlstring.Replace("getitemvalue(" + right + ")", string.Format("'\" + {0} + \"'", curcolumn));
                        }                    
                    }

                    ////参与sql的变量值不能为空，否则sql会语法错误
                    //s += "if (" + string.Format("Ext.getCmp('{0}').getValue()", right.Split('.')[1].Trim()) + "== '') { \r\n";
                    //s += "  Ext.MessageBox.alert('提示', '参与sql的变量:" + "" +"值不能为空!'); \r\n";
                    //s += "} \r\n";
                }

                s += "        sqlstr += \"";
                s += sqlstring;
                s += "\";";
                s += string.Format("\r\n");

                /*
                //更新grid列
                foreach (var right in Right)
                {
                    sqlstring = sqlstring.Replace("getitemvalue(" + right + ")", string.Format("'\"+item.data['{0}']+\"'", right.Split('.')[1].Trim()));

                }
                s = "    Ext.Array.each(listdwstore.data.items, function (item){";
                s += string.Format("\r\n");
                s += "        sqlstr += \"";
                s += sqlstring;
                s += "\";";
                s += string.Format("\r\n");
                s += "    });";
                */
            }
            else
            { //exec
                //exec mytestprocedure1(string|p_form0000000083_d.textcol_1|id,string|p_form0000000083_m.textcol_1|,string|p_form0000000083_m.userdefine_1)
                //    var execstr = "mytestprocedure1(string|"+Ext.getCmp('textcol_1').getValue()+",string|"+Ext.getCmp('textcol_1').getValue()+",string|"+Ext.getCmp('userdefine_1').getValue()+")";   
                //取下类型

                DataAccess.Common common = new DataAccess.Common();  //查帮助用的
                DataTable dt = common.GetProcedureParams("mytestprocedure1");  //存储过程名称暂时固定，待修改

                if (dt == null || dt.Rows.Count == 0)
                    return "";

                sqlstring = sqlstring.Substring(sqlstring.IndexOf(" ") + 1);

                int index = 0;
                foreach (var right in Right)
                {
                    sqlstring = sqlstring.Replace(right,
                                                  string.Format("\" + Ext.getCmp('{0}').getValue() + \"|{1}",
                                                                right.Split('.')[1].Trim(),
                                                                dt.Rows[index]["name"].ToString().StartsWith("@")
                                                                    ? dt.Rows[index]["name"].ToString().Substring(1)
                                                                    : dt.Rows[index]["name"].ToString()));
                    index++;
                }
                s = "    execstr = \"";
                s += sqlstring;
                s += "\";";
            }

            return s;
        }
    }


    /// <summary>
    /// sql语句类型
    /// </summary>
    public enum SqlExpType
    {
        SqlExp,//普通sql语句
        Procedure//存储过程
    }
}
