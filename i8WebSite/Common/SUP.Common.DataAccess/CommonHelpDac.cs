using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Web;
using System.Web.Caching;
using System.Data;
using System.Reflection;

using NG3;
using NG3.Base;
using NG3.Data;
using NG3.Data.Service;

using SUP.Common.DataEntity;
using SUP.Common.Base;
using Newtonsoft.Json;


namespace SUP.Common.DataAccess
{
    public class CommonHelpDac : HelpBase
    {

        private const string COMMONHELPKEY = "COMMONHELPKEY";
        private const string COMMONHELPORMKEY = "COMMONHELPORMKEY";
        static string[] splitStr = new string[] { "\r\n" };

        public CommonHelpDac()
        {
 
        }

        /// <summary>
        /// 获取查询区模板
        /// </summary>
        /// <param name="helpid"></param>
        /// <returns></returns>
        public string GetQueryTemplate(string helpid)
        {
            CommonHelpEntity item = this.GetCommonHelpItem(helpid);

            //return item.Query.ToTemplate();

            return GetHelpTemplate(item.QueryTemplate);

        }

        /// <summary>
        /// 取得列表的模板信息
        /// </summary>
        /// <param name="helpid"></param>
        /// <returns></returns>
        public string GetListTemplate(string helpid)
        {
            CommonHelpEntity item = this.GetCommonHelpItem(helpid);

            //return string.Format("<TreeList>{0}</TreeList>", item.List.InnerXml);

            return GetHelpTemplate(item.ListTemplate);
        }

        public string GetJsonTemplate(string helpid)
        {
            CommonHelpEntity item = this.GetCommonHelpItem(helpid);

            return GetXmlContent(item.JsonTemplate,"gb2312");
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="helpid">帮助标记</param>
        /// <param name="clientQuery">客户端查询条件</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <param name="clientSqlFilter">客户端传来的sql查询条件</param>
        /// <param name="isAutoComplete">联想搜索</param>
        /// <returns></returns>
        public DataTable GetList(string helpid, int pageSize, int pageIndex, ref int totalRecord, string clientQuery, string outJsonQuery, string leftLikeJsonQuery, string clientSqlFilter,bool isAutoComplete,bool ormMode)
        {
            DataTable dt = null;
            CommonHelpEntity item = ormMode ? this.GetHelpItem(helpid) : this.GetCommonHelpItem(helpid);

            //拼装sql语句方式
            if(item.Mode == HelpMode.Default)
            {
                StringBuilder sql = new StringBuilder();

                if (item.AllField.Length > 0)
                {
                    sql.Append("select " + item.Distinct + item.AllField)
                        .Append(" from " + item.TableName)
                        .Append(" where ");
                }
                else
                {
                    sql.Append("select " + item.Distinct + item.CodeField + " " + item.CodeProperty + " , " 
                                        + item.NameField + " " + item.NameProperty)
                        .Append(" from " + item.TableName)
                        .Append(" where ");
                }
                
                #region where     
                     
            
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

                //if(!string.IsNullOrEmpty(clientQuery))
                //{
                //    sql.Append(" and " + clientQuery);
                //}
                

                #endregion

                #region 分页

                string sortString = string.Empty;

                if (!string.IsNullOrEmpty(item.SortField))
                {
                    sortString = item.SortField.Trim().IndexOf(" ") > 0 ? item.SortField : item.SortField + " asc ";
                }
                else
                {
                    sortString = item.CodeField + " asc ";
                }


                if (string.IsNullOrEmpty(clientQuery) && string.IsNullOrEmpty(outJsonQuery))
                {
                    string strSql = PaginationAdapter.GetPageDataSql(sql.ToString(), pageSize, ref pageIndex, ref totalRecord, sortString, null);

                    dt = DbHelper.GetDataTable(strSql);
                }
                else
                {
                    string query = string.Empty;

                    IDataParameter[] p= null;
                    if (isAutoComplete)
                    {
                        p = this.BuildInputQuery(helpid, clientQuery,item.PYField,outJsonQuery,leftLikeJsonQuery,ref query);
                    }
                    else
                    {
                        p = DataConverterHelper.BuildQueryWithParam(clientQuery,outJsonQuery,leftLikeJsonQuery, ref query);
                    }

                    if (!string.IsNullOrEmpty(query))
                    {
                        sql.Append(" and " + query);
                    }

                    string strSql = PaginationAdapter.GetPageDataSql(sql.ToString(), pageSize, ref pageIndex, ref totalRecord, sortString, p);

                    dt = DbHelper.GetDataTable(strSql,p);
                }

                #endregion

            }
            else if (item.Mode == HelpMode.GetHelpResult)//插件模式通过反射获取帮助列表
            {                
                string fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "I6Rules" + Path.DirectorySeparatorChar + item.Assembly);
                if (!File.Exists(fullpath))
                {
                    fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Rules" + Path.DirectorySeparatorChar + item.Assembly);
                }
                if (File.Exists(fullpath))
                {
                    Assembly assem = Assembly.LoadFile(fullpath);
                    object instance = assem.CreateInstance(item.ClassName);

                    ICommonHelp help = instance as ICommonHelp;

                    if (help != null)
                    {
                        dt = help.GetHelpList(pageSize, pageIndex,ref totalRecord, clientQuery, outJsonQuery, isAutoComplete);
                    }
                    else
                    {
                        throw new Exception(string.Format("程序集{0}中的{1}类未实现SUP.Common.Base.ICommonHelp接口", item.Assembly, item.ClassName));
                    }
                }
                else
                {
                    throw new Exception(string.Format("服务端Rules目录下找不到程序集{0}", item.Assembly));
                }
            }

            if (ormMode)
            {
                dt.TableName = item.TableName;
                return ConvertFieldColToPropertyCol(dt,item.FieldPropertyDic,item.FieldDic);
            }

            return dt;
        }


        /// <summary>
        /// 取得帮助节点信息
        /// </summary>
        /// <param name="helpid"></param>
        /// <returns></returns>
        public override CommonHelpEntity GetCommonHelpItem(string helpid)
        {
            if (string.IsNullOrEmpty(helpid)) throw new ArgumentException("helpid is null");

            CommonHelpEntity item = HttpRuntime.Cache.Get(COMMONHELPKEY + helpid) as CommonHelpEntity;

            if (item == null)
            {
                string fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                               "NG3Config" + Path.DirectorySeparatorChar + "NG3CommonHelp.xml");
                //@"Config\CommonHelp.xml");
                //读取通用帮助文档
                XmlDocument doc = HttpRuntime.Cache.Get(COMMONHELPKEY) as XmlDocument;
                if (doc == null)
                {
                    try
                    {
                        doc = new XmlDocument();
                        doc.Load(fullpath);

                        HttpRuntime.Cache.Add(COMMONHELPKEY,
                                              doc,
                                              new CacheDependency(fullpath),
                                              DateTime.Now.AddDays(30),
                                              Cache.NoSlidingExpiration,
                                              CacheItemPriority.NotRemovable,
                                              null);

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                //取配置节
                string xPath = string.Format("/CommonHelp/Help[HelpID='{0}']", helpid);

                try
                {
                    DbHelper.Open();
                    if (DbHelper.Vendor == DbVendor.Oracle)
                    {
                        xPath = string.Format("/CommonHelp/Help[HelpID='{0}']", helpid + "_orcale");

                        //先尝试找oracle节点
                        XmlNode tempnode = doc.DocumentElement.SelectSingleNode(xPath);
                        if (tempnode == null)
                        {
                            xPath = string.Format("/CommonHelp/Help[HelpID='{0}']", helpid); //找不到就找正常节点
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    DbHelper.Close();
                }


                XmlNode node = doc.DocumentElement.SelectSingleNode(xPath);

                if (node == null)
                {
                    throw new Exception(string.Format("commonhelp.xml文件中无法找到{0}配置节,请注意区分大小写", helpid));
                }

                item = new CommonHelpEntity();

                string mode = GetSubNode(node, "Mode", string.Empty);
                item.AllField = GetSubNode(node, "AllField", string.Empty);
                item.HeadText = GetSubNode(node, "HeadText", string.Empty);

                switch (mode)
                {
                    case "0":
                        item.Mode = HelpMode.Default;
                        break;
                    case "1":
                        item.Mode = HelpMode.GetHelpSql;
                        break;
                    case "2":
                        item.Mode = HelpMode.GetHelpResult;
                        break;
                    default:
                        item.Mode = HelpMode.Default;
                        break;
                }

                item.CodeField = GetSubNode(node, "CodeField", string.Empty);
                item.NameField = GetSubNode(node, "NameField", string.Empty);
                item.TableName = GetSubNode(node, "TableName", string.Empty);
                if (item.Mode == HelpMode.Default)
                {
                    //item.ID = helpflag;
                  
                    item.SqlFilter = GetSubNode(node, "SqlFilter", string.Empty);
                    item.SortField = GetSubNode(node, "SortField", string.Empty);


                    string distinct = GetSubNode(node, "Distinct", string.Empty);
                    if (distinct == "1")
                    {
                        item.Distinct = " DISTINCT ";
                    }

                }
                else if (item.Mode == HelpMode.GetHelpSql)
                {
                    item.Assembly = GetSubNode(node, "Assembly", string.Empty);
                    item.ClassName = GetSubNode(node, "ClassName", string.Empty);
                }
                else
                {
                    item.Assembly = GetSubNode(node, "Assembly", string.Empty);
                    item.ClassName = GetSubNode(node, "ClassName", string.Empty);
                    item.GetListMethod = GetSubNode(node, "GetListMethod", string.Empty);
                    item.GetNameMethod = GetSubNode(node, "GetNameMethod", string.Empty);
                    item.CodeToNameMethod = GetSubNode(node, "CodeToNameMethod", string.Empty);
                }

                item.Title = GetSubNode(node, "Title", string.Empty);
                item.QueryTemplate = GetSubNode(node, "QueryTemplate", string.Empty);
                item.ListTemplate = GetSubNode(node, "ListTemplate", string.Empty);
                item.JsonTemplate = GetSubNode(node, "JsonTemplate", string.Empty);
                //var queryNode = node.SelectSingleNode("Query");
                //if (queryNode != null)
                //{
                //    item.Query = new CommHelpQuery(queryNode);
                //}
                //item.List = node.SelectSingleNode("List");

                //缓存起来
                HttpRuntime.Cache.Add(COMMONHELPKEY + helpid,
                                      item,
                                      new CacheDependency(fullpath),
                                      DateTime.Now.AddDays(30),
                                      Cache.NoSlidingExpiration,
                                      CacheItemPriority.NotRemovable,
                                      null);
            }

            //过滤条件宏定义替换，不能再缓存里面做，否则组织切换了，缓存还在数据就不正确了
            string filter = item.SqlFilter;
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
                if (filter.IndexOf("@version@") > 0)//fastdp用到
                {
                    item.SqlFilter = filter.Replace("@version@", AppInfoBase.Version);
                }
            }
            return item;
        }

        private string GetSubNode(XmlNode node, string pname, string defV)
        {
            string v = null;
            var t = node.SelectSingleNode(pname);
            if (t != null)
            {
                v = t.InnerXml;
            }

            return v ?? defV;
        }

        public string GetHelpTemplate(string templateName)
        {       
            string content = GetXmlContent(templateName,"utf-8");

            StringBuilder sb = new StringBuilder();

            var lines = content.Split(splitStr, StringSplitOptions.None);

            // sb.Append("\'");

            foreach (var line in lines)
            {
                if (!line.IsNullOrWhiteSpace())
                {
                    sb.Append(line.Trim());
                    sb.Append("\\\n");
                }
            }

            //sb.Append("\'");

            return (sb.ToString());
        }        

        public string GetXmlContent(string templateName,string codingtype)
        {
            //string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config\HelpTemplate\" + templateName);

            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config" + Path.DirectorySeparatorChar + "HelpTemplate" + Path.DirectorySeparatorChar + templateName);

            string content = GetValueByPath(fullPath,codingtype);

            return content;
        }

        private string GetValueByPath(string fullpath, string codingtype)
        {
            FileStream fs = new FileStream(fullpath, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (fs.Length <= 3) return string.Empty;

            byte[] bomData = new byte[3];
            int offset = 0;
            fs.Read(bomData, 0, 3);
            //验证是否存在Utf8的 bom,如果有则忽略
            if (bomData[0] == 0xEF && bomData[1] == 0xBB && bomData[2] == 0xBF)
            {
                offset = 3;
            }
            else
            {
                fs.Seek(0, SeekOrigin.Begin);
            }

            byte[] data = new byte[fs.Length - offset];

            int pos = fs.Read(data, 0, data.Length);

            if (pos <= 0)
            {
                fs.Close();
                throw new EndOfStreamException(string.Format("SUPTemplateItem.GetValueByPath Error: FullPath={0}!", fullpath));
            }

            fs.Close();
            if ("gb2312" == codingtype)
            {
                return System.Text.Encoding.GetEncoding("gb2312").GetString(data);                
            }
            else
            {
                return System.Text.Encoding.UTF8.GetString(data);
            }
        }

        //----------新实现,注册信息放在数据库中-------

        public override CommonHelpEntity GetHelpItem(string helpid)
        {
            if (string.IsNullOrEmpty(helpid)) throw new ArgumentException("helpid is null");

            CommonHelpEntity item = HttpRuntime.Cache.Get(COMMONHELPORMKEY + helpid) as CommonHelpEntity;
            if (item == null)
            {
                DataTable dt = this.GetMasterInfo(helpid);
                DataRow dr = dt.Rows[0];

                item = new CommonHelpEntity();

                string mode = dr["modetype"].ToString();
                item.AllField = this.GetFields(helpid);
                item.AllProperty = this.GetProperties(helpid, item.FieldPropertyDic,item.FieldDic);
                item.HeadText = this.GetHeader(helpid);

                switch (mode)
                {
                    case "0": item.Mode = HelpMode.Default;
                        break;                  
                    case "1": item.Mode = HelpMode.GetHelpResult;
                        break;
                    default: item.Mode = HelpMode.Default;
                        break;
                }

                item.CodeField = dr["codefield"].ToString();
                item.CodeProperty = DataConverterHelper.FieldToProperty(dr["tablename"].ToString(), item.CodeField);
                item.NameField = dr["namefield"].ToString();
                item.NameProperty = DataConverterHelper.FieldToProperty(dr["tablename"].ToString(), item.NameField);
                 
                if (item.Mode == HelpMode.Default)
                {
                    //item.ID = helpflag;
                    item.TableName = dr["tablename"].ToString();
                    item.SqlFilter = dr["sqlfilter"].ToString();
                    item.SortField = dr["sortfield"].ToString();


                    string distinct = dr["needdistinct"].ToString();
                    if (distinct == "1")
                    {
                        item.Distinct = " DISTINCT ";
                    }

                }
                else if (item.Mode == HelpMode.GetHelpResult)
                {
                    item.Assembly = dr["assemblyname"].ToString();
                    item.ClassName = dr["classname"].ToString(); 
                }

                item.Title = dr["title"].ToString();
                item.ShowTree = dr["showtree"].ToString();
                item.TreePid = dr["treepid"].ToString();
                item.TreeChildId = dr["treechildid"].ToString(); 

                //缓存起来
                HttpRuntime.Cache.Add(COMMONHELPKEY + helpid,
                                             item,
                                             null,
                                             DateTime.Now.AddDays(1),
                                             Cache.NoSlidingExpiration,
                                             CacheItemPriority.NotRemovable,
                                             null);
            }

            return item;
        }

        private DataTable GetMasterInfo(string helpid)
        {
            string sql = "select * from fg_helpinfo_master where helpid={0}";
            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("helpid", helpid);
            DataTable dt = DbHelper.GetDataTable(sql, p);

            if (dt.Rows.Count == 0) throw new Exception("helpid:" + helpid + " is not found");

            return dt;
        }

        private string GetFields(string helpid)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select fg_helpinfo_sys.tablename,fieldname,fieldsource,seq,showheader from fg_helpinfo_master,fg_helpinfo_sys ");
            sb.Append(" where fg_helpinfo_master.code = fg_helpinfo_sys.masterid and fg_helpinfo_master.helpid ={0} ORDER BY showheader desc , seq");
            //sb.Append(" union");
            //sb.Append(" select fg_helpinfo_user.tablename,fieldname,fieldsource,seq,showheader from fg_helpinfo_master,fg_helpinfo_user ");
            //sb.Append(" where fg_helpinfo_master.code = fg_helpinfo_user.masterid and fg_helpinfo_master.helpid = {0} ORDER BY showheader desc , seq");

            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("helpid", helpid);
            DataTable dt = DbHelper.GetDataTable(sb.ToString(), p);

            string fields = string.Empty;
            int last = dt.Rows.Count - 1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string temp = dt.Rows[i]["tablename"].ToString() + "." + dt.Rows[i]["fieldname"].ToString();
                //string temp = dt.Rows[i]["tablename"].ToString() + "." + DataConverterHelper.FieldToProperty(dt.Rows[i]["tablename"].ToString(), dt.Rows[i]["fieldname"].ToString());

                if (i == last)
                {
                    fields += temp;
                }
                else
                {
                    fields += temp + ",";
                }
            }

            return fields;
        }

        private string GetHeader(string helpid)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select fieldproperty from fg_helpinfo_master,fg_helpinfo_sys ");
            sb.Append(" where fg_helpinfo_master.code = fg_helpinfo_sys.masterid and showheader='1' and fg_helpinfo_master.helpid ={0} ");
            //sb.Append(" union");
            //sb.Append(" select fieldproperty from fg_helpinfo_master,fg_helpinfo_user ");
            //sb.Append(" where fg_helpinfo_master.code = fg_helpinfo_user.masterid and showheader='1' and fg_helpinfo_master.helpid = {0} ");

            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("helpid", helpid);
            DataTable dt = DbHelper.GetDataTable(sb.ToString(), p);

            string header = string.Empty;
            int last = dt.Rows.Count - 1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string temp = dt.Rows[i]["fieldproperty"].ToString();
                if (i == last)
                {
                    header += temp;
                }
                else
                {
                    header += temp + ",";
                }
            }

            return header;
        }

        public DataTable GetListData(string helpid, int pageSize, int pageIndex, ref int totalRecord, string clientQuery, string outJsonQuery, string leftLikeJsonQuery, string clientSqlFilter, bool isAutoComplete)
        {
            DataTable dt = null;

            CommonHelpEntity item = this.GetHelpItem(helpid);

            //拼装sql语句方式
            if (item.Mode == HelpMode.Default)
            {
                StringBuilder sql = new StringBuilder();

                if (item.AllField.Length > 0)
                {
                    sql.Append("select " + item.Distinct + item.AllField)
                        .Append(" from " + item.TableName)
                        .Append(" where ");
                }
                else
                {
                    sql.Append("select " + item.Distinct + item.CodeField + " , " + item.NameField)
                        .Append(" from " + item.TableName)
                        .Append(" where ");
                }


                #region where
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
                #endregion

                #region 分页

                string sortString = string.Empty;
                if (!string.IsNullOrEmpty(item.SortField))
                {
                    sortString = item.SortField.Trim().IndexOf(" ") > 0 ? item.SortField : item.SortField + " asc ";
                }
                else
                {
                    sortString = item.CodeField + " asc ";
                }


                if (string.IsNullOrEmpty(clientQuery) && string.IsNullOrEmpty(outJsonQuery))
                {
                    string strSql = PaginationAdapter.GetPageDataSql(sql.ToString(), pageSize, ref pageIndex, ref totalRecord, sortString, null);

                    dt = DbHelper.GetDataTable(strSql);
                }
                else
                {
                    string query = string.Empty;

                    IDataParameter[] p = null;
                    if (isAutoComplete)
                    {
                        p = this.BuildInputQuery(helpid, clientQuery,item.PYField, outJsonQuery, leftLikeJsonQuery, ref query);
                    }
                    else
                    {
                        p = DataConverterHelper.BuildQueryWithParam(clientQuery, outJsonQuery, leftLikeJsonQuery, ref query);
                    }

                    if (!string.IsNullOrEmpty(query))
                    {
                        sql.Append(" and " + query);
                    }

                    string strSql = PaginationAdapter.GetPageDataSql(sql.ToString(), pageSize, ref pageIndex, ref totalRecord, sortString, p);

                    dt = DbHelper.GetDataTable(strSql, p);
                }

                #endregion

            }
            else //插件模式通过反射获取帮助列表
            {
                string fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "I6Rules" + Path.DirectorySeparatorChar + item.Assembly);

                if (File.Exists(fullpath))
                {
                    Assembly assem = Assembly.LoadFile(fullpath);
                    object instance = assem.CreateInstance(item.ClassName);

                    ICommonHelp help = instance as ICommonHelp;

                    if (help != null)
                    {
                        dt = help.GetHelpList(pageSize, pageIndex, ref totalRecord, clientQuery, outJsonQuery, isAutoComplete);
                    }
                    else
                    {
                        throw new Exception(string.Format("程序集{0}中的{1}类未实现SUP.Common.Base.ICommonHelp接口", item.Assembly, item.ClassName));
                    }
                }
                else
                {
                    throw new Exception(string.Format("服务端Rules目录下找不到程序集{0}", item.Assembly));
                }
            }
            return dt;
            
        }

        
    }

}
