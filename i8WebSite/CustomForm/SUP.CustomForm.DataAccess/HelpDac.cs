using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Web.Caching;
using NG3;
using NG3.Data;
using NG3.Data.Service;
using SUP.CustomForm.DataEntity;
using SUP.Common.Base;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace SUP.CustomForm.DataAccess
{
    public class HelpDac : HelpBase
    {
        public const string HELPKEY = "HELPKEY";
        public const string RICHHELPKEY = "RICHHELPKEY";
        private const string HelpXML = "SUP_CommoHelpConfig";
        private const string HelpRich = "SUP_RichHelpConfig";

        //add by ljy 20160303，增加或获取help帮助缓存，这些帮助列来自设计器左边拖出来的帮助列，列名和helpid对应关系存在xml文件内，xtype类型是ngRichHelp
        public static DataTable XmlHelpDT
        {
            get
            {
                DataSet ds = HttpRuntime.Cache[HelpXML] as DataSet;
                if (ds == null)
                {
                    string configPath = AppDomain.CurrentDomain.BaseDirectory + "NG3Config\\EFormHelp.xml";
                    ds = new DataSet();
                    ds.ReadXml(configPath);

                    //缓存到进程Cache
                    HttpRuntime.Cache.Insert(HelpXML, ds, null,
                        System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(1, 0, 0), System.Web.Caching.CacheItemPriority.NotRemovable, null);
                }
                return ds.Tables["Node"];
            }
        }

        //设计器后期扩展某些帮助列可以选richhelp的helpid，所以从数据库取出所有richhelp信息，xtype类型是ngRichHelp
        public static DataTable RichHelpDT
        {
            get
            {
                DataTable dt = HttpRuntime.Cache[HelpRich] as DataTable;
                if (dt == null)
                {
                    dt = DbHelper.GetDataTable("select * from fg_helpinfo_master");

                    //缓存到进程Cache
                    HttpRuntime.Cache.Insert(HelpRich, dt, null,
                        System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(1, 0, 0), System.Web.Caching.CacheItemPriority.NotRemovable, null);
                }
                return dt;
            }
        }

        /// <summary>
        /// 获得help数据列表;
        /// </summary>
        /// <param name="helpId">帮助id;</param>
        /// <param name="pageSize">分页大小;</param>
        /// <param name="pageIndex">页码;</param>
        /// <param name="totalRecord">总记录长度;</param>
        /// <param name="clientFilter">过滤条件;</param>
        /// <returns></returns>
        public DataTable GetHelpList(string helpId, int pageSize, int pageIndex, ref int totalRecord, string clientFilter, bool isAutoComplete, string outJsonQuery)
        {
            HelpEntity item = GetHelpItem(helpId);
            DataTable dt = null;

            /*如果没有sql则返回;*/
            if (item.FromSql == "1")
            {
                string sortField = string.Empty;
                string sql = item.Sql;

                //填充“:参数”
                if (sql.IndexOf(":") > 0)
                {
                    if (!string.IsNullOrEmpty(outJsonQuery))
                    {
                        JObject jo = JObject.Parse(outJsonQuery.ToString());

                        foreach (var it in jo)
                        {
                            if (string.IsNullOrWhiteSpace(it.Value.ToString()))
                            {
                                sql = sql.Replace(":" + it.Key.ToString(), "''");
                            }
                            else
                            {
                                sql = sql.Replace(":" + it.Key.ToString(), it.Value.ToString());
                            }
                        }
                    }
                }

                int n = sql.ToUpper().IndexOf("ORDER BY", StringComparison.OrdinalIgnoreCase);
                if (n > 0)
                {
                    sortField = sql.Substring(n);
                    sql = sql.Substring(0, n - 1);
                }
                else
                {
                    sortField = item.CodeField + " asc";
                }

                if (!string.IsNullOrEmpty(clientFilter))
                {
                    IDataParameter[] p;
                    string joinStr = string.Empty;

                    //sql中有无where
                    if (sql.IndexOf("where", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        joinStr = " and ";
                    }
                    else
                    {
                        joinStr = " where ";
                    }

                    if (isAutoComplete)
                    {
                        string query = string.Empty;
                        p = BuildInputQuery(helpId, clientFilter, string.Empty, string.Empty, ref query);

                        if (!string.IsNullOrEmpty(query))
                        {
                            sql += joinStr + query;
                        }
                    }
                    else
                    {
                        string query = string.Empty;
                        p = DataConverterHelper.BuildQueryWithParam(clientFilter, string.Empty, ref query);

                        if (!string.IsNullOrEmpty(query))
                        {
                            sql += joinStr + query;
                        }
                    }

                    string sqlstr = PaginationAdapter.GetPageDataSql(sql, pageSize, ref pageIndex, ref totalRecord, sortField, p);
                    dt = DbHelper.GetDataTable(sqlstr, p);
                }
                else
                {

                    string sqlstr = PaginationAdapter.GetPageDataSql(sql, pageSize, ref pageIndex, ref totalRecord, sortField);
                    dt = DbHelper.GetDataTable(sqlstr);
                }
            }
            else
            {
                dt = Common.GetHelpInfo(item.HelpId);
            }

            return dt;
        }

        /// <summary>
        /// 获得帮助信息;
        /// </summary>
        /// <param name="helpId">帮助id;</param>
        /// <returns></returns>
        public override HelpEntity GetHelpItem(string helpId)
        {
            if (string.IsNullOrEmpty(helpId)) throw new ArgumentException("helpid is null");

            //根据richhelp帮助表中是否存在判断是哪种帮助
            DataRow[] drs = HelpDac.RichHelpDT.Select("helpid='" + helpId + "'");
            if (drs.Length > 0)
            {
                return GetRichHelpItem(helpId);    //系统内置的通用帮助
            }
            else
            {
                return GetCustomFormHelpItem(helpId);  //设计器上自己定义的帮助
            }
        }

        /// <summary>
        /// 获得通用帮助信息;
        /// </summary>
        /// <param name="helpId">帮助id;</param>
        /// <returns></returns>
        public HelpEntity GetCustomFormHelpItem(string helpId)
        {
            if (string.IsNullOrEmpty(helpId)) throw new ArgumentException("helpid is null");
            var item = HttpRuntime.Cache.Get(HELPKEY + helpId) as HelpEntity;

            if (item == null)
            {
                item = new HelpEntity();
                string fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NG3Config" + Path.DirectorySeparatorChar + "EFormHelp.xml");

                var sql = new StringBuilder();
                sql.Append("select * from p_form_base ").Append("where phid = \'" + helpId + "\'");

                var helpdt = new DataTable();
                helpdt = DbHelper.GetDataTable(sql.ToString());

                if (helpdt == null || helpdt.Rows.Count < 1)
                {
                    item.HelpId = helpId;
                    item.FromSql = "0";
                    item.Sql = string.Empty;
                    return item;
                }
                else
                {
                    /*获得帮助表字段信息;*/
                    item.HelpId = helpId;
                    item.Title = GetFieldValue(helpdt, "base_name");
                    item.FromSql = GetFieldValue(helpdt, "fromsql");

                    //帮助数据来自sql
                    if (item.FromSql == "1")
                    {
                        item.CodeField = GetFieldValue(helpdt, "col_data");
                        item.NoField = GetFieldValue(helpdt, "col_filter");
                        item.NameField = GetFieldValue(helpdt, "col_view");
                        item.AllField = item.NoField + "," + item.NameField + "," + item.CodeField;
                        item.HeadText = GetFieldValue(helpdt, "filtertitle") + "," + GetFieldValue(helpdt, "viewtitle");

                        item.Sql = GetFieldValue(helpdt, "sql_str");
                        item.DynamicSql = item.Sql;

                        //获取表名和转换动态sql
                        if (!string.IsNullOrEmpty(item.Sql))
                        {
                            int spacePos;
                            do
                            {
                                item.Sql = item.Sql.Replace("  ", " ");
                                spacePos = item.Sql.IndexOf("  ");

                            } while (spacePos != (-1));

                            int fromPos = item.Sql.IndexOf("from", StringComparison.OrdinalIgnoreCase);
                            int onePos = item.Sql.IndexOf(" ", fromPos + 4);
                            int twoPos = item.Sql.IndexOf(" ", onePos + 1);

                            if (twoPos == (-1))
                            {
                                item.TableName = item.Sql.Substring(fromPos + 4).Trim();
                            }
                            else
                            {
                                item.TableName = item.Sql.Substring(onePos, twoPos - onePos).Trim();
                            }

                            //带：动态sql转换
                            if (item.Sql.IndexOf(":") > 0)
                            {
                                //得到sql中:和空格之间的串
                                string start = @"\:";
                                string end = @"\ ";
                                Regex rg = new Regex("(?<=(" + start + "))[.\\s\\S]*?(?=(" + end + "))", RegexOptions.Multiline | RegexOptions.Singleline);
                                MatchCollection macths = rg.Matches(item.DynamicSql + " ");
                                int bracketPos = 0;
                                string tempValue = string.Empty;

                                if (macths.Count > 0)
                                {
                                    for (int i = 0; i < macths.Count; i++)
                                    {
                                        tempValue = macths[i].Value;

                                        //去掉可能存在的小括号符
                                        do
                                        {
                                            tempValue = tempValue.Replace(")", "");
                                            bracketPos = tempValue.IndexOf(")");

                                        } while (bracketPos != (-1));

                                        item.DynamicSql = item.DynamicSql.Replace(":" + tempValue.Trim(), " null or 1=1 ");
                                    }
                                }
                            }
                        }
                    }
                    else  //帮助数据是手工录入的，不来自sql
                    {
                        item.CodeField = "phid";
                        item.NoField = "base_code";
                        item.NameField = "base_name";
                        item.AllField = item.NoField + "," + item.NameField + "," + item.CodeField;
                        item.HeadText = "代码,名称";
                    }
                }

                HttpRuntime.Cache.Add(HELPKEY + helpId,
                                      item,
                                      new CacheDependency(fullpath),
                                      DateTime.Now.AddHours(2),
                                      Cache.NoSlidingExpiration,
                                      CacheItemPriority.NotRemovable,
                                      null);
            }

            return item;
        }

        /// <summary>
        /// 获得系统帮助信息;
        /// </summary>
        /// <param name="helpId">帮助id;</param>
        /// <returns></returns>
        public HelpEntity GetRichHelpItem(string helpId)
        {
            if (string.IsNullOrEmpty(helpId)) throw new ArgumentException("helpid is null");
            var item = HttpRuntime.Cache.Get(HELPKEY + helpId) as HelpEntity;

            if (item == null)
            {
                item = new HelpEntity();
                string fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NG3Config" + Path.DirectorySeparatorChar + "EFormHelp.xml");

                var sql = new StringBuilder();
                sql.Append("select * from fg_helpinfo_master ")
                    .Append("where helpid = \'" + helpId + "\'");

                var helpdt = new DataTable();
                helpdt = DbHelper.GetDataTable(sql.ToString());

                if (helpdt == null || helpdt.Rows.Count < 1)
                {
                    item.HelpId = helpId;
                    item.FromSql = "0";
                    item.Sql = string.Empty;
                    return item;
                }
                else
                {
                    /*获得帮助表字段信息;*/
                    item.HelpId = helpId;
                    item.Title = GetFieldValue(helpdt, "title");
                    item.CodeField = GetFieldValue(helpdt, "codefield");
                    item.NameField = GetFieldValue(helpdt, "namefield");
                    item.AllField = item.CodeField + "," + item.NameField;
                    item.HeadText = "代码,名称";
                    item.FromSql = "1";
                    item.TableName = GetFieldValue(helpdt, "tablename");
                    item.Sql = "select " + item.AllField + " from " + item.TableName;
                    item.DynamicSql = item.Sql;
                }

                HttpRuntime.Cache.Add(HELPKEY + helpId,
                                      item,
                                      new CacheDependency(fullpath),
                                      DateTime.Now.AddHours(2),
                                      Cache.NoSlidingExpiration,
                                      CacheItemPriority.NotRemovable,
                                      null);
            }

            return item;
        }

        /// <summary>
        /// 获得字段的值;
        /// </summary>
        private static string GetFieldValue(DataTable dt, string field)
        {
            var value = string.Empty;
            if (dt.Rows.Count > 0)
            {
                value = dt.Rows[0][field].ToString();
            }
            return value;
        }

    }
}
