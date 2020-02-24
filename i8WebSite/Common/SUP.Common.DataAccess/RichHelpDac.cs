using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Caching;

using Newtonsoft.Json;
using NG3;
using NG3.Data;
using NG3.Data.Service;
using SUP.Common.Base;
using SUP.Common.DataEntity;

namespace SUP.Common.DataAccess
{


    public class RichHelpDac : HelpBase
    {
        private const string COMMONHELPKEY = "RICH_HELP_KEY";


        public int DeleteCommonUseData(string helpid, string codeValue, string logid, string org)
        {
            string sql = "delete from fg_help_commonuse where helpid={0} and ocode={1} and logid={2} and strvalue={3}";
            IDataParameter[] p = new NGDataParameter[] { new NGDataParameter("helpid", helpid), new NGDataParameter("ocode", org), new NGDataParameter("logid", logid), new NGDataParameter("strvalue", codeValue) };
            return DbHelper.ExecuteNonQuery(sql, p);
        }

        /// <summary>
        /// 是否存在用户定义的查询条件
        /// </summary>
        /// <param name="helpid"></param>
        /// <param name="ocode"></param>
        /// <param name="logid"></param>
        /// <returns></returns>
        private bool ExistsUserDefine(string helpid, string ocode, string logid)
        {
            string sql = "select COUNT(*) from fg_helprichquery_master where helpid={0} and userid={1}";
            IDataParameter[] p = new NGDataParameter[2];
            p[0] = new NGDataParameter("helpid", helpid);
            p[1] = new NGDataParameter("userid", logid);

            return (DbHelper.GetString(sql, p) != "0");
        }

        public override CommonHelpEntity GetCommonHelpItem(string helpid)
        {
            if (string.IsNullOrEmpty(helpid)) throw new ArgumentException("helpid is null");

            string cachedKey = COMMONHELPKEY + AppInfoBase.UCode + LangInfo.Lang + helpid;
            CommonHelpEntity item = HttpRuntime.Cache.Get(cachedKey) as CommonHelpEntity;            

            if (item == null)
            {
                DataRow dr = this.GetMasterInfo(helpid).Rows[0];
                item = new CommonHelpEntity();

                item.CodeField = dr["codefield"].ToString();//系统编码列
                if (dr["usercodefield"] != null && dr["usercodefield"] != DBNull.Value)
                {
                    item.UserCodeField = dr["usercodefield"].ToString();//用户编码列
                }
                else
                {
                    item.UserCodeField = item.CodeField;
                }

                item.NameField = dr["namefield"].ToString();
                item.TableName = dr["tablename"].ToString();
                item.CodeProperty = DataConverterHelper.FieldToProperty(item.TableName, item.CodeField);
                item.UserCodeProperty = DataConverterHelper.FieldToProperty(item.TableName, item.UserCodeField);
                item.NameProperty = DataConverterHelper.FieldToProperty(item.TableName, item.NameField);
                item.PYField = dr["pyfield"].ToString();//拼音首字母

                string allField = this.GetFields(helpid, false);
                bool hasAllField = (!string.IsNullOrEmpty(allField));
                item.AllField = hasAllField ? allField : (item.CodeField+"," +item.NameField);//excel导入没配置sys表
                item.AllFieldWithTableName = hasAllField ? this.GetFields(helpid, true) : item.AllField;
                item.AllProperty = this.GetProperties(helpid, item.FieldPropertyDic,item.FieldDic);
                item.DetailTableFields = this.GetDetailTableFields(helpid);
                item.DetailTablePropertys = this.GetDetailTableProperties(helpid);
                item.HeadText = hasAllField ? this.GetHeader(helpid) : ("代 码,名 称");
                item.DetailTableHeaders = this.GetDetailTableHeaders(helpid);


                if (dr["busphid"] != null && dr["busphid"] != DBNull.Value)
                {
                    long busphid = Convert.ToInt64(dr["busphid"].ToString());
                    item.BusPhid = busphid;
                    //item.BusType = DbHelper.GetString(string.Format("select code FROM  metadata_bustree where phid={0}", busphid));
                }

                string mode = dr["modetype"].ToString();
                switch (mode)
                {
                    case "0":
                        item.Mode = HelpMode.Default;
                        break;
                    case "1":
                        item.Mode = HelpMode.GetHelpResult;
                        break;
                    default:
                        item.Mode = HelpMode.Default;
                        break;
                }
                
                //明细表
                item.DetailTable = dr["detailtable"].ToString();
                item.MasterTableKey = dr["mastertablekey"].ToString();
                item.MasterTableKeyProperty = DataConverterHelper.FieldToProperty(item.DetailTable, item.MasterTableKey);
                item.MasterID = dr["masterid"].ToString();
                item.DetailSqlFilter = dr["detailsqlfilter"].ToString();

                if (item.Mode == HelpMode.Default)
                {
                    item.SqlFilter = HttpUtility.UrlDecode(dr["sqlfilter"].ToString());
                    item.SqlFilterWithMacro = item.SqlFilter;
                    item.SortField = dr["sortfield"].ToString();
                    item.SortProperty = DataConverterHelper.FieldToProperty(item.TableName, dr["sortfield"].ToString());
                    string distinct = dr["needdistinct"].ToString();
                    if (distinct == "1")
                    {
                        item.Distinct = " DISTINCT ";
                    }

                }
                else
                {
                    item.Assembly = dr["assemblyname"].ToString();
                    item.ClassName = dr["classname"].ToString();
                }

                item.Title = dr["title"].ToString();
                var langDic = LangInfo.GetLabelLang(helpid);
                if (langDic.ContainsKey("richhelp_title") && !string.IsNullOrWhiteSpace(langDic["richhelp_title"]))
                {
                    item.Title = langDic["richhelp_title"];//多语言
                }
               
                item.ShowTree = dr["showtree"].ToString();

                item.TreePid = dr["treepid"].ToString();
                item.TreeChildId = dr["treechildid"].ToString();

                bool isOutDatastource = false;
                if (dr["outdatasource"] != null && dr["outdatasource"] != DBNull.Value)
                {
                    if (Convert.ToInt16(dr["outdatasource"]) == 1)
                    {
                        isOutDatastource = true;
                    }
                }
                item.OutDataSource = isOutDatastource;//是否外部数据源，数据不在当前账套的

                bool exitsQueryProp = false;

                item.QueryPropertyItem = this.GetQeuryPropertyItem(helpid, ref exitsQueryProp);
                item.ExistQueryProperty = exitsQueryProp;

                if (!string.IsNullOrEmpty(item.CodeProperty) && !string.IsNullOrEmpty(item.NameProperty))
                {
                    ////缓存起来
                    HttpRuntime.Cache.Add(cachedKey,
                                          item,
                                          null,
                                          DateTime.Now.AddDays(1),
                                          Cache.NoSlidingExpiration,
                                          CacheItemPriority.NotRemovable,
                                          null);
                }
            }

            //过滤条件宏定义替换，不能再缓存里面做，否则组织切换了，缓存还在数据就不正确了
            string filter = item.SqlFilterWithMacro;
            if (filter != null)
            {
                if (filter.IndexOf("@ocode@") > 0)
                {
                    item.SqlFilter = filter.Replace("@ocode@", AppInfoBase.OCode);
                }
                if (filter.IndexOf("@orgid@") > 0)
                {
                    item.SqlFilter = filter.Replace("@orgid@", AppInfoBase.OrgID.ToString());
                }
            }
            return item;
        }

        //取得字段信息
        private string GetFields(string helpid, bool withTableName)
        {
            StringBuilder sb = new StringBuilder();
           // sb.Append("select * from ( ");
            sb.Append("select fg_helpinfo_sys.tablename,fieldname,fieldsource,seq ,fg_helpinfo_sys.showheader from fg_helpinfo_master,fg_helpinfo_sys  ");
            sb.Append(" where fg_helpinfo_master.code = fg_helpinfo_sys.masterid and fg_helpinfo_master.helpid ={0} ");
            //sb.Append(" union");
            //sb.Append(" select fg_helpinfo_user.tablename,fieldname,fieldsource,seq ,fg_helpinfo_user.showheader from fg_helpinfo_master,fg_helpinfo_user ");
            //sb.Append(" where fg_helpinfo_master.code = fg_helpinfo_user.masterid and fg_helpinfo_master.helpid = {0} ");
            //sb.Append(" ) t ");
            sb.Append(" order by showheader desc , seq ");
            IDataParameter[] p = new NGDataParameter[] { new NGDataParameter("helpid", helpid) };
            DataTable dt = DbHelper.GetDataTable(sb.ToString(), p);
            string fields = string.Empty;
            int last = dt.Rows.Count - 1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string temp = string.Empty;
                if (withTableName)//带表名，帮助的常用功能需要
                {
                    string tableName = dt.Rows[i]["tablename"].ToString();

                    if (string.IsNullOrEmpty(tableName))
                    {
                        temp = dt.Rows[i]["fieldname"].ToString();
                    }
                    else
                    {
                        temp = tableName + "." + dt.Rows[i]["fieldname"].ToString();
                    }
                }
                else
                {
                    temp = dt.Rows[i]["fieldname"].ToString();//不带表名，原先带表名是commonhelp的查询字段名可能是一样的,richhelp不需要
                }


                if (i == last)
                {
                    fields = fields + temp.Trim().ToLower();
                }
                else
                {
                    fields = fields + temp.Trim().ToLower() + ",";
                }
            }
            return fields;
        }

        //取得列表头信息
        private string GetHeader(string helpid)
        {
            

            StringBuilder sb = new StringBuilder();
            //sb.Append("select * from ( ");
            sb.Append("select fieldproperty,fg_helpinfo_sys.langkey,seq from fg_helpinfo_master,fg_helpinfo_sys ");
            sb.Append(" where fg_helpinfo_master.code = fg_helpinfo_sys.masterid and showheader='1' and fg_helpinfo_master.helpid ={0} ");
            //sb.Append(" union");
            //sb.Append(" select fieldproperty,seq from fg_helpinfo_master,fg_helpinfo_user ");
            //sb.Append(" where fg_helpinfo_master.code = fg_helpinfo_user.masterid and showheader='1' and fg_helpinfo_master.helpid = {0} ");
            //sb.Append(" ) t ");
            sb.Append(" order by seq ");
            IDataParameter[] p = new NGDataParameter[] { new NGDataParameter("helpid", helpid) };
            DataTable dt = DbHelper.GetDataTable(sb.ToString(), p);

            var langDic = LangInfo.GetLang(helpid);//多语言

            string header = string.Empty;
            int last = dt.Rows.Count - 1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string temp = dt.Rows[i]["fieldproperty"].ToString();                                
                if (dt.Rows[i]["langkey"] != null && dt.Rows[i]["langkey"] != DBNull.Value)
                {
                    string langkey = dt.Rows[i]["langkey"].ToString();
                    if (langDic != null)
                    {
                        if (langDic.ContainsKey(langkey) && !string.IsNullOrWhiteSpace(langDic[langkey]))
                        {
                            temp = langDic[langkey];//替换多语言
                        }
                    }                
                }

                if (i == last)
                {
                    header = header + temp;
                }
                else
                {
                    header = header + temp + ",";
                }
            }
            return header;
        }

        //获取明细表的字段信息
        private string GetDetailTableFields(string helpid)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select fg_helpinfo_detailtable.tablename,fieldname,seq ,showheader from fg_helpinfo_master,fg_helpinfo_detailtable  ");
            sb.Append(" where fg_helpinfo_master.code = fg_helpinfo_detailtable.masterid and fg_helpinfo_master.helpid ={0} ");
            sb.Append(" order by seq asc  ");

            IDataParameter[] p = new NGDataParameter[] { new NGDataParameter("helpid", helpid) };
            DataTable dt = DbHelper.GetDataTable(sb.ToString(), p);
            string fields = string.Empty;
            int last = dt.Rows.Count - 1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string tableName = dt.Rows[i]["tablename"].ToString();
                string temp = string.Empty;
                if (string.IsNullOrEmpty(tableName))
                {
                    temp = dt.Rows[i]["fieldname"].ToString();
                }
                else
                {
                    temp = tableName + "." + dt.Rows[i]["fieldname"].ToString();
                }

                if (i == last)
                {
                    fields = fields + temp;
                }
                else
                {
                    fields = fields + temp + ",";
                }
            }
            return fields;

        }

        //获取明细表的头信息
        private string GetDetailTableHeaders(string helpid)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select fieldproperty,seq from fg_helpinfo_master,fg_helpinfo_detailtable  ");
            sb.Append(" where fg_helpinfo_master.code = fg_helpinfo_detailtable.masterid and fg_helpinfo_master.helpid ={0} ");
            sb.Append(" order by seq asc  ");

            IDataParameter[] p = new NGDataParameter[] { new NGDataParameter("helpid", helpid) };
            DataTable dt = DbHelper.GetDataTable(sb.ToString(), p);
            string header = string.Empty;
            int last = dt.Rows.Count - 1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string temp = dt.Rows[i]["fieldproperty"].ToString();
                if (i == last)
                {
                    header = header + temp;
                }
                else
                {
                    header = header + temp + ",";
                }
            }
            return header;

        }

        /// <summary>
        /// 列表信息
        /// </summary>
        /// <param name="helpid">帮助id</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页号</param>
        /// <param name="totalRecord">总记录数</param>
        /// <param name="keys">记忆查询，在结果中搜索</param>
        /// <param name="treerefkey">树对应的段</param>
        /// <param name="treesearchkey">树上的值</param>
        /// <param name="outJsonQuery">外部条件</param>
        /// <param name="leftLikeJsonQuery">*%左匹配条件</param>
        /// <param name="clientSqlFilter">sql查询条件</param>
        /// <param name="isAutoComplete">是否智能搜索</param>
        /// <returns></returns>
        public object GetList(RichHelpListArgEntity entity, ref int totalRecord, bool ormMode)
        {
            int pageIndex = entity.PageIndex;
            CommonHelpEntity item = this.GetCommonHelpItem(entity.Helpid);
            string extfields = this.GetExtendFields(entity.QueryPropertyCode);
            if (item.Mode == 0)
            {
                StringBuilder sql = new StringBuilder();
                if (item.AllField.Length > 0)
                {
                    if (string.IsNullOrWhiteSpace(extfields))
                    {
                        sql.Append("select ");
                        sql.Append(item.Distinct);
                        //sql.Append(item.AllField);
                        sql.Append(item.AllFieldWithTableName);
                    }
                    else
                    {
                        sql.Append("select ");
                        sql.Append(item.Distinct);
                        sql.Append(" ");
                        //sql.Append(item.AllField);
                        sql.Append(item.AllFieldWithTableName);
                        sql.Append(",");
                        sql.Append(extfields);
                    }
                    sql.Append(" from " + item.TableName).Append(" where ");
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(extfields))
                    {
                        sql.Append("select ");
                        sql.Append(item.Distinct);
                        sql.Append(item.CodeField);
                        sql.Append(",");
                        sql.Append(item.NameField);
                    }
                    else
                    {
                        sql.Append("select ");
                        sql.Append(item.Distinct);
                        sql.Append(" ");
                        sql.Append(item.CodeField);
                        sql.Append(",");
                        sql.Append(item.NameField);
                        sql.Append(",");
                        sql.Append(extfields);
                    }
                    sql.Append(" from " + item.TableName);
                    sql.Append(" where ");
                }
                if (!string.IsNullOrEmpty(item.SqlFilter))
                {
                    sql.Append(item.SqlFilter);
                }
                else
                {
                    sql.Append(" 1=1 ");
                }
                if (!string.IsNullOrEmpty(entity.ClientSqlFilter))
                {
                    sql.Append(" and " + entity.ClientSqlFilter);
                }
                if (!(string.IsNullOrWhiteSpace(entity.Treerefkey) || string.IsNullOrWhiteSpace(entity.Treesearchkey)))
                {
                    sql.Append(" and " + entity.Treerefkey + "='" + entity.Treesearchkey + "'");
                }
                string sortString = string.Empty;
                if (!string.IsNullOrEmpty(item.SortField))
                {
                    sortString = (item.SortField.Trim().IndexOf(" ") > 0) ? item.SortField : (item.SortField + " asc ");
                }
                else
                {
                    sortString = item.CodeField + " asc ";
                }

                Int64 busPhid = item.BusPhid;//配置在帮助上的
                if (!string.IsNullOrWhiteSpace(entity.BusCode))//前端传入的
                {
                    string phid = DbHelper.GetString(string.Format("SELECT phid FROM metadata_bustree where code='{0}'", entity.BusCode));
                    if (string.IsNullOrWhiteSpace(phid))
                    {
                        throw new Exception(string.Format("业务类型代码[{0}]的phid获取失败！", entity.BusCode));
                    }
                    busPhid = Convert.ToInt64(phid);
                }
                //信息权限处理
                if (busPhid != 0)
                {
                    //var infoRight = new Enterprise3.Rights.AnalyticEngine.InfoRightControl(item.BusType, AppInfoBase.UserID);
                    var infoRight = new Enterprise3.Rights.AnalyticEngine.InfoRightControl(busPhid,
                        entity.InfoRightUIContainerID, AppInfoBase.UserID);
                    string infoRightQuery = infoRight.GetSqlWhere();

                    if (!string.IsNullOrEmpty(infoRightQuery))
                    {
                        sql.Append(" and (" + infoRightQuery + ")");
                    }
                }

                if (!entity.IsAutoComplete && (entity.Keys.Length == 0) && string.IsNullOrEmpty(entity.OutJsonQuery))
                {
                    DataTable tempDt = DbHelper.GetDataTable(PaginationAdapter.GetPageDataSql(sql.ToString(), entity.PageSize, ref pageIndex, ref totalRecord, sortString, null));
                    if (ormMode)
                    {
                        tempDt.TableName = item.TableName;
                        return ConvertFieldColToPropertyCol(tempDt, item.FieldPropertyDic, item.FieldDic);
                    }
                    return tempDt;
                }
                string query = string.Empty;
                IDataParameter[] p = null;
                if (entity.IsAutoComplete && !string.IsNullOrWhiteSpace(entity.ClientQuery))//敲空格智能搜索，取全部数据
                {
                    p = base.BuildInputQuery(entity.Helpid, entity.ClientQuery, item.PYField, entity.OutJsonQuery, string.Empty, ref query);
                }
                else
                {
                    string code = string.IsNullOrEmpty(item.UserCodeField) ? item.CodeField : item.UserCodeField;//优先搜索用户代码列
                    p = DataConverterHelper.BuildQueryWithParam(code, item.NameField, item.PYField, entity.Keys, entity.OutJsonQuery, string.Empty, ref query);
                }
                if (!string.IsNullOrEmpty(query))
                {
                    sql.Append(" and (" + query + ")");
                }
             

                string strSql = PaginationAdapter.GetPageDataSql(sql.ToString(), entity.PageSize, ref pageIndex, ref totalRecord, sortString, p);

                DataTable tDt = DbHelper.GetDataTable(strSql, p);
                if (ormMode)
                {
                    tDt.TableName = item.TableName;
                    return ConvertFieldColToPropertyCol(tDt, item.FieldPropertyDic, item.FieldDic);
                }
                return tDt;
            }
            else
            {
                string fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "I6Rules" + Path.DirectorySeparatorChar + item.Assembly);
                if (!File.Exists(fullpath))
                {
                    throw new Exception(string.Format("服务端Rules目录下找不到程序集{0}", item.Assembly));
                }
                Assembly assemb = Assembly.LoadFile(fullpath);
                object obj = assemb.CreateInstance(item.ClassName);

                if (ormMode)//ORM实现不同的接口
                {
                    IRichHelpListORM help = obj as IRichHelpListORM;
                    if (help == null)
                    {
                        throw new Exception(string.Format("程序集{0}中的{1}类未实现SUP.Common.Base.IRichHelpListORM", item.Assembly, item.ClassName));
                    }
                    return help.GetHelpList(entity.PageSize, entity.PageIndex, ref totalRecord, entity.Keys, entity.Treesearchkey,entity.ClientQuery,entity.OutJsonQuery, entity.IsAutoComplete);
                }
                else
                {

                    IRichHelpList help = obj as IRichHelpList;
                    if (help == null)
                    {
                        throw new Exception(string.Format("程序集{0}中的{1}类未实现SUP.Common.Base.IRichHelpList接口", item.Assembly, item.ClassName));
                    }
                    return help.GetHelpList(entity.PageSize, entity.PageIndex, ref totalRecord, entity.Keys, entity.Treesearchkey, entity.ClientQuery,entity.OutJsonQuery, entity.IsAutoComplete);
                }
            }
        }

        //获取明细
        public DataTable GetDetailList(string helpid, string masterCode, bool ormMode)
        {
            CommonHelpEntity item = this.GetCommonHelpItem(helpid);

            StringBuilder sql = new StringBuilder();

            sql.Append("select ");
            sql.Append(item.DetailTableFields);
            sql.Append(" from " + item.DetailTable).Append(" where ");

            if (!string.IsNullOrWhiteSpace(item.DetailSqlFilter))
            {
                sql.Append(item.DetailSqlFilter);
                sql.Append(" and " + item.MasterID + " = '" + masterCode + "'");
            }
            else
            {
                sql.Append(item.MasterID + " = '" + masterCode + "'");
            }

            DataTable dt = DbHelper.GetDataTable(sql.ToString());
            if (ormMode)
            {
                dt.TableName = item.DetailTable;//显示指定表名，字段转属性用得到表名
                return ConvertFieldColToPropertyCol(dt, item.FieldPropertyDic, item.FieldDic);
            }

            return dt;
        }

        /// <summary>
        /// 获取主信息
        /// </summary>
        /// <param name="helpid"></param>
        /// <returns></returns>
        private DataTable GetMasterInfo(string helpid)
        {
            string sql = "select * from fg_helpinfo_master where helpid={0}";
            IDataParameter[] p = new NGDataParameter[] { new NGDataParameter("helpid", helpid) };
            DataTable dt = DbHelper.GetDataTable(sql, p);
            if (dt.Rows.Count == 0)
            {
                throw new Exception("helpid:" + helpid + " is not found");
            }
            return dt;
        }

        private QueryPropertyItem[] GetQeuryPropertyItem(string helpid, ref bool existQueryProperty)
        {
            DataTable dt = DbHelper.GetDataTable(" select fg_helpinfo_queryprop.* from fg_helpinfo_queryprop,fg_helpinfo_master  where fg_helpinfo_master.code = fg_helpinfo_queryprop.masterid  and fg_helpinfo_master.helpid ='" + helpid + "'");
            existQueryProperty = dt.Rows.Count > 0;
            int count = dt.Rows.Count + 1;
            QueryPropertyItem[] arr = new QueryPropertyItem[count];

            QueryPropertyItem allItem = new QueryPropertyItem();
            allItem.code = "all";
            allItem.boxLabel = "无分类";
            allItem.name = "property";
            allItem.inputValue = "all";
            //allItem.@checked = true;
            arr[0] = allItem;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                QueryPropertyItem item = new QueryPropertyItem();
                item.code = dr["code"].ToString();
                item.boxLabel = dr["propertyname"].ToString();
                item.name = "property";
                item.inputValue = dr["propertyid"].ToString();
                arr[i + 1] = item;
            }
            return arr;
        }

        /// <summary>
        /// 构建查询字典
        /// </summary>
        /// <param name="helpid"></param>
        /// <param name="clientQuery">json格式数据</param>
        /// <returns></returns>
        private Dictionary<string, object> GetQueryDictionary(string helpid, string clientQuery)
        {
            DataTable dt = this.GetRichQueryUIInfo(helpid, AppInfoBase.OCode, AppInfoBase.LoginID);
            Dictionary<string, object> queryDic = new Dictionary<string, object>();
            Dictionary<string, object> d = JsonConvert.DeserializeObject<Dictionary<string, object>>(clientQuery);
            foreach (KeyValuePair<string, object> item in d)
            {
                string fiedlname = item.Key;
                if (item.Key.IndexOf(".") > 0)
                {
                    fiedlname = item.Key.Split(new char[] { '.' })[1];//去掉表名
                }
                DataRow[] arr = dt.Select(string.Format("field='{0}'", fiedlname));
                if (arr.Length > 0)
                {
                    string key = item.Key + "*" + arr[0]["operator"].ToString();
                    queryDic.Add(key, item.Value);
                }
            }
            return queryDic;
        }

        /// <summary>
        /// 获取记忆的查询条件按
        /// </summary>
        /// <param name="helpid"></param>
        /// <returns></returns>
        public string GetQueryFilter(string helpid)
        {
            string sql = "select rememberstr from fg_helprichquery_master where helpid={0} and userid={1}";
            IDataParameter[] p = new NGDataParameter[] { new NGDataParameter("helpid", helpid), new NGDataParameter("userid", AppInfoBase.LoginID) };
            return DbHelper.GetString(sql, p);
        }


        /// <summary>
        /// 查询属性树
        /// </summary>
        /// <param name="code">主键</param>
        /// <param name="porpertyInfodt">查询属性信息表</param>
        /// <returns></returns>
        public DataTable GetQueryProTree(string code, ref DataTable porpertyInfodt)
        {
            string sql = " select modetype,tree_sql,treeshow,tree_id,tree_pid,tree_text,tree_searchkey,tree_lazyload,list_treerefkey,assemblyname,classname  from fg_helpinfo_queryprop where code ='" + code + "'";
            DataTable treedt = new DataTable();
            DataTable dt = DbHelper.GetDataTable(sql);
            porpertyInfodt = dt;
            if (dt.Rows.Count <= 0)
            {
                return treedt;
            }
            DataRow dr = dt.Rows[0];

            if ("0" == dr["modetype"].ToString())//sql模式
            {
                return DbHelper.GetDataTable(dr["tree_sql"].ToString());
            }
            else//插件模式
            {
                string assembly = dr["assemblyname"].ToString();
                string classname = dr["classname"].ToString();
                string fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "I6Rules" + Path.DirectorySeparatorChar + assembly);
                if (!File.Exists(fullpath))
                {
                    throw new Exception(string.Format("服务端Rules目录下找不到程序集{0}", assembly));
                }
                IRichHelpProTree help = Assembly.LoadFile(fullpath).CreateInstance(classname) as IRichHelpProTree;
                if (help == null)
                {
                    throw new Exception(string.Format("程序集{0}中的{1}类未实现SUP.Common.Base.IRichHelpProTree接口", assembly, classname));
                }
                return help.GetPropertyTreeList(code);
            }
        }

        /// <summary>
        /// 高级查询元素
        /// </summary>
        /// <param name="helpid">帮助id</param>
        /// <returns></returns>
        public DataTable GetRichQueryItems(string helpid)
        {
            string sql = "select * from fg_helpinfo_richquery where isdisplay='1' and helpid={0}";
            IDataParameter[] p = new NGDataParameter[] { new NGDataParameter("helpid", helpid) };
            return DbHelper.GetDataTable(sql, p);
        }

        /// <summary>
        /// 高级查询列表
        /// </summary>
        /// <param name="helpid">帮助id</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页号</param>
        /// <param name="totalRecord">总行数</param>
        /// <param name="clientQuery">客户端json查询条件</param>
        /// <param name="outJsonQuery">外部查询条件</param>
        /// <param name="leftLikeJsonQuery">左匹配查询条件</param>
        /// <param name="clientSqlFilter">客户端注入的sql条件语句</param>
        /// <returns></returns>
        //public Object GetRichQueryList(string helpid, int pageSize, int pageIndex, ref int totalRecord, string clientQuery, string outJsonQuery, string leftLikeJsonQuery, string clientSqlFilter, bool ormMode)
        public Object GetRichQueryList(RichHelpListArgEntity entity, ref int totalRecord, bool ormMode)
        {
            int pageIndex = entity.PageIndex;
            CommonHelpEntity item = this.GetCommonHelpItem(entity.Helpid);
            DataTable richQueryItems = this.GetRichQueryItems(entity.Helpid);
            string filter = string.Empty;
            string tables = item.TableName;
            string pluginQuery = string.Empty;

            if (!string.IsNullOrWhiteSpace(entity.ClientQuery))
            {
                string tablelist = this.GetTableList(richQueryItems, entity.ClientQuery, ref filter);
                if (!string.IsNullOrWhiteSpace(tablelist))
                {
                    tables = tables + tablelist;
                }
            }


            //if (item.Mode == HelpMode.GetHelpResult)
            if (item.OutDataSource)
            {

                string fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "I6Rules" + Path.DirectorySeparatorChar + item.Assembly);
                if (!File.Exists(fullpath))
                {
                    fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Rules" + Path.DirectorySeparatorChar + item.Assembly);
                }
                if (!File.Exists(fullpath))
                {
                    throw new Exception(string.Format("服务端Rules目录下找不到程序集{0}", item.Assembly));
                }
                Assembly assembly = Assembly.LoadFile(fullpath);
                object obj = assembly.CreateInstance(item.ClassName);

                if (ormMode)
                {
                    IRichHelpRichQueryListORM help = obj as IRichHelpRichQueryListORM;
                    if (help == null)
                    {
                        throw new Exception(string.Format("程序集{0}中的{1}类未实现SUP.Common.Base.IRichHelpList接口", item.Assembly, item.ClassName));
                    }
                    //help.GetRichQueryInfo(ref tables, ref pluginQuery);//插件内部组装表，查询条件 

                    return help.GetRichQueryList(entity.Helpid, entity.PageSize, entity.PageIndex, ref totalRecord, entity.ClientQuery, entity.OutJsonQuery);
                }
                else
                {
                    IRichHelpRichQueryList help = obj as IRichHelpRichQueryList;
                    if (help == null)
                    {
                        throw new Exception(string.Format("程序集{0}中的{1}类未实现SUP.Common.Base.IRichHelpList接口", item.Assembly, item.ClassName));
                    }
                    //help.GetRichQueryInfo(ref tables, ref pluginQuery);//插件内部组装表，查询条件 

                    return help.GetRichQueryList(entity.Helpid, entity.PageSize, entity.PageIndex, ref totalRecord, entity.ClientQuery, entity.OutJsonQuery);
                }

            }
            else
            {
                //组装sql语句
                StringBuilder sql = new StringBuilder();
                if (item.AllField.Length > 0)
                {
                    sql.Append("select " + item.Distinct + item.AllFieldWithTableName);
                    sql.Append(" from " + tables);
                    sql.Append(" where ");
                }
                else
                {
                    sql.Append("select " + item.Distinct + item.CodeField + " , " + item.NameField);
                    sql.Append(" from " + tables);
                    sql.Append(" where ");
                }

                //组装条件
                if (!string.IsNullOrEmpty(item.SqlFilter))
                {
                    sql.Append(item.SqlFilter);
                }
                else if (!string.IsNullOrWhiteSpace(pluginQuery))
                {
                    sql.Append(pluginQuery);
                }
                else
                {
                    sql.Append(" 1=1 ");
                }
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    sql.Append(filter);
                }
                if (!string.IsNullOrEmpty(entity.ClientSqlFilter))
                {
                    sql.Append(" and " + entity.ClientSqlFilter);
                }
                string sortString = string.Empty;
                if (!string.IsNullOrEmpty(item.SortField))
                {
                    sortString = (item.SortField.Trim().IndexOf(" ") > 0) ? item.SortField : (item.SortField + " asc ");
                }
                else
                {
                    sortString = item.CodeField + " asc ";
                }

                Int64 busPhid = item.BusPhid;//配置在帮助上的
                if (!string.IsNullOrWhiteSpace(entity.BusCode))//前端传入的
                {
                    string phid = DbHelper.GetString(string.Format("SELECT phid FROM metadata_bustree where code='{0}'", entity.BusCode));
                    if (string.IsNullOrWhiteSpace(phid))
                    {
                        throw new Exception(string.Format("业务类型代码[{0}]的phid获取失败！",entity.BusCode));
                    }
                    busPhid = Convert.ToInt64(phid);
                }
                //信息权限处理
                if (busPhid != 0)
                {
                    //var infoRight = new Enterprise3.Rights.AnalyticEngine.InfoRightControl(item.BusType, AppInfoBase.UserID);
                    var infoRight = new Enterprise3.Rights.AnalyticEngine.InfoRightControl(busPhid, 
                        entity.InfoRightUIContainerID, AppInfoBase.UserID);
                    string infoRightQuery = infoRight.GetSqlWhere();

                    if (!string.IsNullOrEmpty(infoRightQuery))
                    {
                        sql.Append(" and (" + infoRightQuery + ")");
                    }
                }

                if (string.IsNullOrEmpty(entity.ClientQuery) && string.IsNullOrEmpty(entity.OutJsonQuery))
                {
                    string strSql = PaginationAdapter.GetPageDataSql(sql.ToString(), entity.PageSize, ref pageIndex, ref totalRecord, sortString, null);
                    DataTable dt = DbHelper.GetDataTable(strSql);
                    if (ormMode)
                    {
                        dt.TableName = item.TableName;
                        return ConvertFieldColToPropertyCol(dt, item.FieldPropertyDic, item.FieldDic);
                    }
                    return dt;
                }
                string query = string.Empty;
                IDataParameter[] p = null;
             
                string[] keys = new string[0];
                p = DataConverterHelper.BuildQueryWithParam(item.CodeField, item.NameField,item.PYField, keys,entity.OutJsonQuery, string.Empty, ref query);
                if (!string.IsNullOrEmpty(query))
                {
                    sql.Append(" and " + query);
                }

              

                string str = PaginationAdapter.GetPageDataSql(sql.ToString(), entity.PageSize, ref pageIndex, ref totalRecord, sortString, p);

                DataTable tempDt = DbHelper.GetDataTable(str, p);
                if (ormMode)
                {
                    tempDt.TableName = item.TableName;
                    return ConvertFieldColToPropertyCol(tempDt, item.FieldPropertyDic, item.FieldDic);
                }
                return tempDt;
            }

        }

        /// <summary>
        /// 获取关联表
        /// </summary>
        /// <param name="richQueryItems">高级查询</param>
        /// <param name="clientQuery">客户端查询条件json格式</param>
        /// <param name="where">sql过滤条件</param>
        /// <returns></returns>
        private string GetTableList(DataTable richQueryItems, string clientQuery, ref string where)
        {
            StringBuilder strb = new StringBuilder();
            List<string> list = new List<string>();
            Dictionary<string, object> d = JsonConvert.DeserializeObject<Dictionary<string, object>>(clientQuery);
            foreach (KeyValuePair<string, object> item in d)
            {
                if (!string.IsNullOrEmpty(item.Value.ToString()))
                {
                    string str = item.Key;
                    string field = string.Empty;
                    if (str.IndexOf(".") > 0)
                    {
                        field = str.Split(new char[] { '.' })[1];
                    }
                    DataRow[] dr = richQueryItems.Select("field='" + field + "' and isrelateproperty='1'");
                    if (dr.Length > 0)
                    {
                        list.Add(dr[0]["tablename"].ToString());

                        strb.Append(" and ");
                        strb.Append(dr[0]["tablename"].ToString());
                        strb.Append("." + dr[0]["joinfield"].ToString());
                        strb.Append(" = ");
                        strb.Append(dr[0]["basetable"].ToString());
                        strb.Append("." + dr[0]["basefield"].ToString());
                    }
                }
            }
            where = strb.ToString();
            string tables = string.Empty;
            for (int i = 0; i < list.Count; i++)
            {
                tables = tables + "," + list[i];
            }
            return tables;
        }

        /// <summary>
        /// 树形列表数据
        /// </summary>
        /// <param name="helpid"></param>
        /// <param name="clientQuery"></param>
        /// <param name="outJsonQuery"></param>
        /// <param name="leftLikeJsonQuery"></param>
        /// <param name="clientSqlFilter"></param>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        public DataTable GetTreeList(string helpid, string clientQuery, string outJsonQuery, string leftLikeJsonQuery, string clientSqlFilter, string nodeid, bool ormMode)
        {
            CommonHelpEntity item = this.GetCommonHelpItem(helpid);
            if (item.Mode == HelpMode.Default)
            {
                string fields;
                StringBuilder sql = new StringBuilder();
                if (item.AllField.Length > 0)
                {
                    fields = item.AllField;
                    if (!(!(item.ShowTree == "1") || string.IsNullOrWhiteSpace(item.TreePid)))
                    {
                        fields = item.AllField + "," + item.TreePid;
                    }
                    sql.Append("select " + item.Distinct + fields).Append(" from " + item.TableName).Append(" where ");
                }
                else
                {
                    fields = item.CodeField + " , " + item.NameField;
                    if (!(!(item.ShowTree == "1") || string.IsNullOrWhiteSpace(item.TreePid)))
                    {
                        fields = item.CodeField + " , " + item.NameField + "," + item.TreePid;
                    }
                    sql.Append("select " + item.Distinct + fields).Append(" from " + item.TableName).Append(" where ");
                }
                if (!string.IsNullOrEmpty(item.SqlFilter))
                {
                    sql.Append(item.SqlFilter);
                }
                else
                {
                    sql.Append(" 1=1 ");
                }
                if (!string.IsNullOrEmpty(clientSqlFilter))
                {
                    sql.Append(" and " + clientSqlFilter);
                }

                //排序
                string sortString = string.Empty;
                if (!string.IsNullOrEmpty(item.SortField))
                {
                    sortString = (item.SortField.Trim().IndexOf(" ") > 0) ? item.SortField : (item.SortField + " asc ");
                }
                else
                {
                    sortString = item.CodeField + " asc ";
                }
                //无查询条件
                if (string.IsNullOrEmpty(clientQuery) && string.IsNullOrEmpty(outJsonQuery))
                {
                    if (!string.IsNullOrWhiteSpace(sortString))
                    {
                        sql.Append(" order by " + sortString);
                    }

                    DataTable dt = DbHelper.GetDataTable(sql.ToString());
                    if (ormMode)
                    {
                        dt.TableName = item.TableName;
                        return ConvertFieldColToPropertyCol(dt, item.FieldPropertyDic, item.FieldDic);
                    }
                    return dt;
                }

                string query = string.Empty;
                IDataParameter[] p = null;
                p = DataConverterHelper.BuildQueryWithParam(clientQuery, outJsonQuery, leftLikeJsonQuery, ref query);
                if (!string.IsNullOrEmpty(query))
                {
                    sql.Append(" and " + query);
                }
                if (!string.IsNullOrWhiteSpace(sortString))
                {
                    sql.Append(" order by " + sortString);
                }
                
                DataTable tempDt = DbHelper.GetDataTable(sql.ToString(), p);
                if (ormMode)
                {
                    tempDt.TableName = item.TableName;
                    return ConvertFieldColToPropertyCol(tempDt, item.FieldPropertyDic, item.FieldDic);
                }
                return tempDt;               
            }


            string fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "I6Rules" + Path.DirectorySeparatorChar + item.Assembly);
            if (!File.Exists(fullpath))
            {
                throw new Exception(string.Format("服务端Rules目录下找不到程序集{0}", item.Assembly));
            }

            Assembly assembly = Assembly.LoadFile(fullpath);
            object obj = assembly.CreateInstance(item.ClassName);

            IRichHelpTreeList help = obj as IRichHelpTreeList;
            if (help == null)
            {
                throw new Exception(string.Format("程序集{0}中的{1}类未实现SUP.Common.Base.IRichHelpTreeList接口", item.Assembly, item.ClassName));
            }
            return help.GetTreeList(helpid, clientQuery, outJsonQuery, leftLikeJsonQuery, clientSqlFilter, nodeid);

        }

        /// <summary>
        /// 树记忆
        /// </summary>
        /// <param name="type"></param>
        /// <param name="BussType"></param>
        /// <returns></returns>
        public TreeMemoryEntity GetTreeMemory(string type, string BussType)
        {
            string sql = string.Format("SELECT * from fg_comhelp_treememo where logid='{0}' and ocode='{1}' and comhelptype='{2}' and busstype='{3}' and ismemo='1'", new object[] { AppInfoBase.LoginID, AppInfoBase.OCode, type, BussType });

            DataTable tmpDT = DbHelper.GetDataTable(sql);
            TreeMemoryEntity treeMemoryEntity = new TreeMemoryEntity(AppInfoBase.LoginID, AppInfoBase.OCode, type, BussType);
            if ((tmpDT == null) || (tmpDT.Rows.Count == 0))
            {
                treeMemoryEntity.IsMemo = false;
                return treeMemoryEntity;
            }
            treeMemoryEntity.FoucedNodeValue = tmpDT.Rows[0]["FoucedNodeValue"].ToString();
            return treeMemoryEntity;
        }

        /// <summary>
        /// 获取常用数据列表
        /// </summary>
        /// <param name="helpid"></param>
        /// <param name="logid"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public DataTable GetCommonUseList(string helpid, string logid, string org, bool ormMode)
        {
            CommonHelpEntity item = this.GetCommonHelpItem(helpid);
            StringBuilder sql = new StringBuilder();

            if (item.TableName.IndexOf("=") > 0 || item.TableName.IndexOf(",") > 0)//多表关联
            {
                sql.Append("select table1.* from ( ");
                sql.Append("select " + item.Distinct + item.AllFieldWithTableName);
                sql.Append(" from " + item.TableName);
                sql.Append(" where  1=1 ");
                if (!string.IsNullOrEmpty(item.SqlFilter))
                {
                    sql.Append(" and " + item.SqlFilter);
                }
                sql.Append(" ) table1 ,fg_help_commonuse ");

                string codeField = item.CodeField;
                if (item.CodeField.IndexOf(".") > 0)
                {
                    codeField = codeField.Split('.')[1];//去掉表名
                }
                if (DbHelper.Vendor == DbVendor.Oracle)
                {
                    sql.Append(" where to_char(table1." + codeField + ") =fg_help_commonuse.strvalue ");
                }
                else
                {
                    sql.Append(" where cast(table1." + codeField + " as varchar) =fg_help_commonuse.strvalue ");
                }
            }
            else
            {

                if (item.AllFieldWithTableName.Length > 0)
                {
                    sql.Append("select " + item.Distinct + item.AllFieldWithTableName);
                    sql.Append(" from " + item.TableName + ",fg_help_commonuse ");
                    sql.Append(" where " + item.TableName + "." + item.CodeField + "=fg_help_commonuse.strvalue ");
                }
                else
                {
                    sql.Append("select " + item.Distinct + item.CodeField + " , " + item.NameField);
                    sql.Append(" from " + item.TableName + ",fg_help_commonuse ");
                    sql.Append(" where " + item.TableName + "." + item.CodeField + "=fg_help_commonuse.strvalue ");
                }
                if (!string.IsNullOrEmpty(item.SqlFilter))
                {
                    sql.Append(" and " + item.SqlFilter);
                }
            }

            sql.Append(string.Format(" and fg_help_commonuse.helpid='{0}' ", helpid));
            sql.Append(string.Format(" and fg_help_commonuse.ocode='{0}' ", NG3.AppInfoBase.OCode));
            sql.Append(string.Format(" and fg_help_commonuse.logid='{0}' ", NG3.AppInfoBase.LoginID));
         

            DataTable dt = DbHelper.GetDataTable(sql.ToString());
            if (ormMode)
            {
                dt.TableName = item.TableName;
                return ConvertFieldColToPropertyCol(dt, item.FieldPropertyDic, item.FieldDic);
            }
            return dt;
        }

        /// <summary>
        /// 保存常用数据
        /// </summary>
        /// <param name="helpid"></param>
        /// <param name="codeValue"></param>
        /// <param name="logid"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public int SaveCommonUseData(string helpid, string codeValue, string logid, string org)
        {
            if (IsExistCommonUseData(helpid, codeValue, logid, org)) return -2;

            string sql = "insert into fg_help_commonuse (code,helpid,ocode,logid,strvalue) values ({0},{1},{2},{3},{4})";
            IDataParameter[] p = new NGDataParameter[5];
            p[0] = new NGDataParameter("code", Guid.NewGuid().ToString());
            p[1] = new NGDataParameter("helpid", helpid);
            p[2] = new NGDataParameter("ocode", org);
            p[3] = new NGDataParameter("logid", logid);
            p[4] = new NGDataParameter("strvalue", codeValue);

            return DbHelper.ExecuteNonQuery(sql, p);
        }

        /// <summary>
        /// 判断是否已经存在常用数据
        /// </summary>
        /// <param name="helpid"></param>
        /// <param name="codeValue"></param>
        /// <param name="logid"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        private bool IsExistCommonUseData(string helpid, string codeValue, string logid, string org)
        {
            bool flag = true;
            string sql = string.Format("select count(*) from fg_help_commonuse where helpid='{0}' and ocode='{1}' and logid='{2}' and strvalue='{3}' ", 
                helpid, org,logid,codeValue);
            string str = DbHelper.GetString(sql);
            if (str == "0")
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// 保存查询串
        /// </summary>
        /// <param name="helpid"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public int SaveQueryFilter(string helpid, string json)
        {
            if (IsExistQueryFilter(helpid))
            {
                string sql = "update fg_helprichquery_master set rememberstr={0} where helpid={1} and userid={2}";
                IDataParameter[] p = new NGDataParameter[3];
                p[0] = new NGDataParameter("rememberstr", json);
                p[1] = new NGDataParameter("helpid", helpid);
                p[2] = new NGDataParameter("userid", AppInfoBase.LoginID);

                return DbHelper.ExecuteNonQuery(sql, p);
            }
            else
            {
                string sql = "insert into fg_helprichquery_master (code,helpid,userid,rememberstr) values({0},{1},{2},{3})";
                IDataParameter[] p = new NGDataParameter[4];
                p[0] = new NGDataParameter("code", Guid.NewGuid().ToString());
                p[1] = new NGDataParameter("helpid", helpid);
                p[2] = new NGDataParameter("userid", AppInfoBase.LoginID);
                p[3] = new NGDataParameter("rememberstr", json);

                return DbHelper.ExecuteNonQuery(sql, p);
            }
        }

        private bool IsExistQueryFilter(string helpid)
        {
            bool flag = true;
            string sql = string.Format("select count(*) from fg_helprichquery_master where helpid='{0}' and userid='{1}' ", helpid, AppInfoBase.LoginID);
            string str = DbHelper.GetString(sql);
            if (str == "0")
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// 高级查询UI信息，设置运算符，默认值
        /// </summary>
        /// <param name="helpid">帮助id</param>
        /// <param name="ocode">组织号</param>
        /// <param name="logid">操作员</param>
        /// <returns></returns>
        public DataTable GetRichQueryUIInfo(string helpid, string ocode, string logid)
        {
            IDataParameter[] p;
            //if (this.ExistsUserDefine(helpid, ocode, logid))
            //{
            StringBuilder strb = new StringBuilder();
            strb.Append(" select fg_helpinfo_richquery.code,definetype,tablename,field,fname_chn,fieldtype,fg_helprichquery_detail.operator,");
            strb.Append(" fg_helprichquery_detail.defaultdata,fg_helprichquery_detail.displayindex ");
            strb.Append(" from fg_helprichquery_detail,fg_helpinfo_richquery,fg_helprichquery_master ");
            strb.Append(" where fg_helpinfo_richquery.code = fg_helprichquery_detail.syscode ");
            strb.Append(" and fg_helprichquery_detail.masterid = fg_helprichquery_master.code ");
            strb.Append(" and fg_helpinfo_richquery.isdisplay = '1' ");
            strb.Append(" and fg_helprichquery_master.helpid={0} ");
            strb.Append(" and fg_helprichquery_master.userid={1}");
            p = new NGDataParameter[] { new NGDataParameter("helpid", helpid), new NGDataParameter("userid", logid) };

            DataTable dt = DbHelper.GetDataTable(strb.ToString(), p);
            //}

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                string sql = "select code,definetype,tablename,field,fname_chn,fieldtype,defaultdata,operator,isdisplay,displayindex from fg_helpinfo_richquery where isdisplay = '1' and helpid={0} ";
                p = new NGDataParameter[] { new NGDataParameter("helpid", helpid) };
                return DbHelper.GetDataTable(sql, p);
            }
        }

        /// <summary>
        /// 保存查询条件设置信息
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="helpid"></param>
        /// <param name="ocode"></param>
        /// <param name="logid"></param>
        /// <returns></returns>
        public int SaveQueryInfo(DataTable dt, string helpid, string ocode, string logid)
        {
            if (dt.Rows.Count == 0) return 1;//空的表，不处理，也算保存成功

            DataTable detaildt;
            int iret = 0;
            if (this.ExistsUserDefine(helpid, ocode, logid))
            {
                StringBuilder strb = new StringBuilder();
                int count = dt.Rows.Count;
                int index = count - 1;
                for (int i = 0; i < count; i++)
                {
                    string val = dt.Rows[i]["code"].ToString();
                    if (i == index)
                    {
                        strb.Append("'" + val + "'");
                    }
                    else
                    {
                        strb.Append("'" + val + "',");
                    }
                }
                detaildt = DbHelper.GetDataTable("select * from fg_helprichquery_detail where syscode in (" + strb.ToString() + ")");
                foreach (DataRow dr in detaildt.Rows)
                {
                    DataRow[] drs = dt.Select(" code='" + dr["syscode"].ToString() + "'");
                    if (drs.Length > 0)
                    {
                        dr["operator"] = drs[0]["operator"].ToString();
                        dr["defaultdata"] = drs[0]["defaultdata"].ToString();
                        dr["displayindex"] = drs[0]["displayindex"].ToString();
                    }
                }
                return DbHelper.Update(detaildt, "select * from fg_helprichquery_detail");
            }

            string sql = "select * from fg_helprichquery_master where 1=0";
            DataTable masterdt = DbHelper.GetDataTable(sql);
            DataRow masterdr = masterdt.NewRow();
            string code = Guid.NewGuid().ToString();
            masterdr.BeginEdit();
            masterdr["code"] = code;
            masterdr["helpid"] = helpid;
            masterdr["userid"] = logid;
            masterdr.EndEdit();
            masterdt.Rows.Add(masterdr);
            iret = DbHelper.Update(masterdt, "select * from fg_helprichquery_master");

            sql = "select * from fg_helprichquery_detail where 1=0";
            detaildt = DbHelper.GetDataTable(sql);
            foreach (DataRow row in dt.Rows)
            {
                DataRow detailRow = detaildt.NewRow();
                detailRow.BeginEdit();
                detailRow["code"] = Guid.NewGuid().ToString();
                detailRow["masterid"] = code;
                detailRow["syscode"] = row["code"].ToString();
                detailRow["operator"] = row["operator"].ToString();
                detailRow["defaultdata"] = row["defaultdata"].ToString();
                detailRow["displayindex"] = row["displayindex"].ToString();
                detailRow.EndEdit();
                detaildt.Rows.Add(detailRow);
            }
            return (iret + DbHelper.Update(detaildt, "select * from fg_helprichquery_detail"));
        }

        /// <summary>
        /// 更新数记忆信息
        /// </summary>
        /// <param name="treeMemoryEntity"></param>
        /// <returns></returns>
        public int UpdataTreeMemory(TreeMemoryEntity treeMemoryEntity)
        {
            DataRow dr;
            string sql = string.Format("SELECT * from fg_comhelp_treememo where logid='{0}' and ocode='{1}' and comhelptype='{2}' and busstype='{3}'", new object[] { treeMemoryEntity.LogId, treeMemoryEntity.OCode, treeMemoryEntity.Type, treeMemoryEntity.BussType });
            DataTable tmpDT = DbHelper.GetDataTable(sql);
            if (tmpDT.Rows.Count == 0)
            {
                dr = tmpDT.NewRow();
                dr["id"] = Guid.NewGuid().ToString();
                tmpDT.Rows.Add(dr);
            }
            else
            {
                dr = tmpDT.Rows[0];
            }
            dr["logid"] = treeMemoryEntity.LogId;
            dr["ocode"] = treeMemoryEntity.OCode;
            dr["comhelptype"] = treeMemoryEntity.Type;
            dr["foucednodevalue"] = treeMemoryEntity.FoucedNodeValue;
            dr["ismemo"] = treeMemoryEntity.IsMemo ? 1 : 0;
            dr["busstype"] = treeMemoryEntity.BussType;
            return DbHelper.Update(tmpDT, sql);
        }

        /// <summary>
        /// 获取列表添加的字段，列头信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetListExtendInfo(string code)
        {
            string sql = "select list_extfields,list_extheader from fg_helpinfo_queryprop where code='" + code + "'";

            return DbHelper.GetDataTable(sql);
        }

        /// <summary>
        /// 获取额外的字段信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetExtendFields(string code)
        {
            string sql = "select list_extfields from fg_helpinfo_queryprop where code='" + code + "'";
            return DbHelper.GetString(sql);
        }

        public override CommonHelpEntity GetHelpItem(string helpid)
        {
            return GetCommonHelpItem(helpid);
        }

        /// <summary>
        /// 获取帮助结果集,Excel导出平台使用，列顺序为item.CodeField|UserCodeField|item.NameField
        /// </summary>
        /// <param name="helpid"></param>
        /// <returns></returns>
        public DataTable GetHelpResult(string helpid)
        {
            CommonHelpEntity item = this.GetCommonHelpItem(helpid);

            StringBuilder sql = new StringBuilder();

            //if (item.AllField.Length > 0)
            //{
            //    sql.Append("select ");
            //    sql.Append(item.Distinct);
            //    //sql.Append(item.AllField);
            //    sql.Append(item.AllFieldWithTableName);
            //    sql.Append(" from " + item.TableName);
            //}
            //else
            
            sql.Append("select ");
            sql.Append(item.Distinct);
            sql.Append(item.CodeField);
            sql.Append(",");
            if (!string.IsNullOrEmpty(item.UserCodeField))
            {
                sql.Append(item.UserCodeField);
                sql.Append(",");
            }
            sql.Append(item.NameField);
            sql.Append(" from " + item.TableName);
            
            if (!string.IsNullOrEmpty(item.SqlFilter))
            {
                sql.Append(" where ");
                sql.Append(item.SqlFilter);
            }

            DataTable dt = DbHelper.GetDataTable(sql.ToString());
            return dt;
        }


    }
}

