using System;
using System.Collections.Generic;
using System.Text;

namespace SUP.CustomForm.ServerGen
{
    internal class Common
    {
        /// <summary>
        /// 返回Tabs个数;
        /// </summary>
        private static string GetTabS(int iCount)
        {
            var tabs = string.Empty;
            for (int i = 0; i < iCount; i++)
            {
                tabs += "\t";
            }
            return tabs;
        }

        /// <summary>
        /// 代码转名称的方法;
        /// </summary>
        public static string GetCodeToNameDac(IList<CodeToNameInfo> info, string sql)
        {
            if (string.IsNullOrEmpty(sql))  //没有单据体，sql为空
            {
                return string.Empty;
            }
            int index = sql.IndexOf("WHERE", StringComparison.OrdinalIgnoreCase);

            if (index > 0)
            {
                sql = sql.Substring(0, index) + " where 1=2";
            }
            else
            {
                sql = sql + " where 1=2";
            }

            /*
            DataTable dt;
            dt = DbHelper.GetDataTable(sql);

            var strResults = new StringBuilder();
            if (info.Count > 0)
            {
                strResults.Append("HelpDac helpdac=new HelpDac();\r\n");
                for (int i = 0; i < info.Count; i++)
                {
                    if (dt.Columns.Contains(info[i].CodeName))
                    {
                        strResults.Append(string.Format(GetTabS(3) + "dt.Columns.Add(\"{0}\");\r\n", info[i].CodeName + "_name"));
                    }
                }

                strResults.Append(GetTabS(3) + "foreach (DataRow dr in dt.Rows)\r\n");
                strResults.Append(GetTabS(3) + "{\r\n");

                for (int i = 0; i < info.Count; i++)
                {
                    if (dt.Columns.Contains(info[i].CodeName))
                    {
                        strResults.Append(string.Format(GetTabS(4) + "dr[\"{0}\"] = dr[\"{1}\"];\r\n", info[i].CodeName + "_name", info[i].CodeName));
                    }
                }

                strResults.Append(GetTabS(3) + "}\r\n");

                for (int i = 0; i < info.Count; i++)
                {
                    if (dt.Columns.Contains(info[i].CodeName))
                    {
                        strResults.Append(string.Format(GetTabS(3) + "helpdac.CodeToName(dt, \"{0}\", \"{1}\");\r\n", info[i].CodeName + "_name", info[i].HelpId));
                    }
                }
            }

            return strResults.ToString();
            */


            var strResults = new StringBuilder();
            if (info.Count > 0)
            {
                strResults.Append("HelpDac helpdac=new HelpDac();\r\n");
                for (int i = 0; i < info.Count; i++)
                {
                    if (sql.Contains(info[i].CodeName)&&info[i].CodeName!="s_tree_name") //if (sql.Contains(info[i].TableName + "." + info[i].CodeName))
                    {
                        strResults.Append(string.Format(GetTabS(3) + "dt.Columns.Add(\"{0}\");\r\n", info[i].CodeName + "_name"));
                    }
                }

                strResults.Append(GetTabS(3) + "foreach (DataRow dr in dt.Rows)\r\n");
                strResults.Append(GetTabS(3) + "{\r\n");

                for (int i = 0; i < info.Count; i++)
                {
                    if (sql.Contains(info[i].CodeName) && info[i].CodeName != "s_tree_name")
                    {
                        strResults.Append(string.Format(GetTabS(4) + "dr[\"{0}\"] = dr[\"{1}\"];\r\n", info[i].CodeName + "_name", info[i].CodeName));
                    }
                }

                strResults.Append(GetTabS(3) + "}\r\n");

                for (int i = 0; i < info.Count; i++)
                {
                    if (sql.Contains(info[i].CodeName) && info[i].CodeName != "s_tree_name")
                    {
                        if (info[i].MultiSelect)
                        {
                            strResults.Append(string.Format(GetTabS(3) + "helpdac.CodeToName(dt, \"{0}\", \"{1}\", \"{2}\");\r\n", info[i].CodeName + "_name", info[i].HelpId, "Multi"));
                        }
                        else
                        {
                            strResults.Append(string.Format(GetTabS(3) + "helpdac.CodeToName(dt, \"{0}\", \"{1}\", \"{2}\");\r\n", info[i].CodeName + "_name", info[i].HelpId, "Single"));
                        }

                    }
                }
            }

            return strResults.ToString();
        }

        public static string GetCodeToNameDac(IList<CodeToNameInfo> info, string sql, string tablename)
        {
            List<CodeToNameInfo> cn = new List<CodeToNameInfo>();

            //生成当前grid中需代码转名称字段
            foreach (CodeToNameInfo code in info)
            {
                if (code.TableName == tablename)
                {
                    cn.Add(code);
                }
            }

            return GetCodeToNameDac(cn, sql);
        }

        /// <summary>
        /// 获取分组列从dr取值的串;
        /// </summary>
        public static string GetGroupFieldCol(Dictionary<string, string> Groupfield)
        {
            string groupfieldcol = string.Empty;

            foreach (KeyValuePair<string, string> col in Groupfield)
            {
                //键值不同说明是帮助列
                if (col.Key != col.Value)
                {
                    groupfieldcol += string.Format("dr[\"{0}\"]+\"(\"+dr[\"{1}\"]+\")\"+\"|\"+", col.Value, col.Key);
                }
                else
                {
                    groupfieldcol += string.Format("dr[\"{0}\"]+\"|\"+", col.Value);
                }

            }

            if (!string.IsNullOrEmpty(groupfieldcol))
            {
                groupfieldcol = groupfieldcol.Substring(0, groupfieldcol.Length - 5);
            }

            return groupfieldcol;
        }
    }
}
