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
using SUP.Common.DataEntity;
using SUP.Common.Base;

namespace SUP.Common.DataAccess
{
    public class CustomHelpDac : CustomHelpBase
    {
        public const string HELPKEY = "HELPKEY";
        private const string HelpXML = "SUP_CommoHelpConfig";

        //add by ljy 20160303，增加或获取help帮助缓存
        public static DataTable DTHelp
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

        /// <summary>
        /// 获得help数据列表;
        /// </summary>
        /// <param name="helpId">帮助id;</param>
        /// <param name="pageSize">分页大小;</param>
        /// <param name="pageIndex">页码;</param>
        /// <param name="totalRecord">总记录长度;</param>
        /// <param name="clientFilter">过滤条件;</param>
        /// <returns></returns>
        public DataTable GetHelpList(string helpId, int pageSize, int pageIndex, ref int totalRecord, string clientFilter, bool isAutoComplete)
        {
            CustomHelpEntity item = GetHelpItem(helpId);
            DataTable dt;

            /*如果没有sql则返回;*/
            if (item.FromSql != "1")
                return new DataTable();

            string sortField = string.Empty;
            string sql = item.Sql;

            int n = sql.ToUpper().IndexOf("ORDER BY");
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
                if (isAutoComplete)
                {
                    string query = string.Empty;
                    p = BuildInputQuery(helpId, clientFilter, string.Empty, string.Empty, ref query);

                    if (!string.IsNullOrEmpty(query))
                    {
                        sql += " where " + query;
                    }
                }
                else
                {
                    string query = string.Empty;
                    p = DataConverterHelper.BuildQueryWithParam(clientFilter, string.Empty, ref query);

                    if (!string.IsNullOrEmpty(query))
                    {
                        sql += " where " + query;
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

            return dt;

        }

        /// <summary>
        /// 获得帮助信息;
        /// </summary>
        /// <param name="helpId">帮助id;</param>
        /// <returns></returns>
        public override CustomHelpEntity GetHelpItem(string helpId)
        {
            if (string.IsNullOrEmpty(helpId)) throw new ArgumentException("helpid is null");


            //系统帮助字段，helpid根据配置文件去读NG3Config\\EFormHelp.xml
            DataRow[] drs = DTHelp.Select("HelpId='" + helpId + "'");

            //系统内置的通用帮助
            if (drs.Length > 0)
            {
                return GetRichHelpItem(helpId);
            }
            else
            {
                return GetCommonHelpItem(helpId);
            }
        }

        /// <summary>
        /// 获得通用帮助信息;
        /// </summary>
        /// <param name="helpId">帮助id;</param>
        /// <returns></returns>
        public CustomHelpEntity GetCommonHelpItem(string helpId)
        {
            if (string.IsNullOrEmpty(helpId)) throw new ArgumentException("helpid is null");
            var item = HttpRuntime.Cache.Get(HELPKEY + helpId) as CustomHelpEntity;

            if (item == null)
            {
                item = new CustomHelpEntity();
                string fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                               "NG3Config" + Path.DirectorySeparatorChar + "CommonHelp.xml");

                var sql = new StringBuilder();
                sql.Append("select * from p_form_base ")
                    .Append("where code = \'" + helpId + "\'");

                var helpdt = new DataTable();
                helpdt = DbHelper.GetDataTable(sql.ToString());

                /*获得帮助表字段信息;*/
                item.HelpId = helpId;
                item.Title = GetFieldValue(helpdt, "base_name");
                item.CodeField = GetFieldValue(helpdt, "col_data");
                item.NameField = GetFieldValue(helpdt, "col_view");
                item.AllField = item.CodeField + "," + item.NameField;
                item.HeadText = GetFieldValue(helpdt, "datetitle") + "," + GetFieldValue(helpdt, "viewtitle");
                item.FromSql = GetFieldValue(helpdt, "fromsql");
                item.Sql = GetFieldValue(helpdt, "sql_str");
                if (item.Sql != "")
                {
                    int spacePos;
                    do
                    {
                        item.Sql = item.Sql.Replace("  ", " ");
                        spacePos = item.Sql.IndexOf("  ", System.StringComparison.Ordinal);

                    } while (spacePos != (-1));

                    item.Sql = item.Sql.Replace("  ", " ");
                    int fromPos = item.Sql.IndexOf("from", System.StringComparison.Ordinal);
                    int onePos = item.Sql.IndexOf(" ", fromPos + 4, System.StringComparison.Ordinal);
                    int twoPos = item.Sql.IndexOf(" ", onePos + 1, System.StringComparison.Ordinal);

                    if (twoPos == (-1))
                    {
                        item.TableName = item.Sql.Substring(fromPos + 4).Trim();
                    }
                    else
                    {
                        item.TableName = item.Sql.Substring(onePos, twoPos - onePos).Trim();
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
        public CustomHelpEntity GetRichHelpItem(string helpId)
        {
            if (string.IsNullOrEmpty(helpId)) throw new ArgumentException("helpid is null");
            var item = HttpRuntime.Cache.Get(HELPKEY + helpId) as CustomHelpEntity;

            if (item == null)
            {
                item = new CustomHelpEntity();
                string fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                               "NG3Config" + Path.DirectorySeparatorChar + "EFormHelp.xml");

                var sql = new StringBuilder();
                sql.Append("select * from fg_helpinfo_master ")
                    .Append("where helpid = \'" + helpId + "\'");

                var helpdt = new DataTable();
                helpdt = DbHelper.GetDataTable(sql.ToString());

                /*获得帮助表字段信息;*/
                item.HelpId = helpId;
                item.Title = GetFieldValue(helpdt, "title");
                item.CodeField = GetFieldValue(helpdt, "codefield");
                item.NameField = GetFieldValue(helpdt, "namefield");
                item.AllField = item.CodeField + "," + item.NameField;
                item.HeadText = "代码,名称";
                item.FromSql = "1";
                item.Sql = "select " + item.AllField + " from " + GetFieldValue(helpdt, "tablename");
                if (item.Sql != "")
                {
                    item.TableName =
                        item.Sql.Substring(item.Sql.IndexOf("from", System.StringComparison.Ordinal) + 4).Trim();
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
