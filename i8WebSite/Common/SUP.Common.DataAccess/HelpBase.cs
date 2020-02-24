using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.DataEntity;
using System.Data;
using SUP.Common.Base;
using NG3.Data.Service;
using System.IO;
using System.Reflection;
using NG3.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace SUP.Common.DataAccess
{
    public abstract class HelpBase
    {
        public abstract CommonHelpEntity GetCommonHelpItem(string helpid);
        public abstract CommonHelpEntity GetHelpItem(string helpid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helpid"></param>
        /// <param name="queryValue">搜索内容</param>
        /// <param name="pyField">拼音首字母字段（助记码字段）</param>
        /// <param name="outJsonQuery"></param>
        /// <param name="leftLikeJsonQuery"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDataParameter[] BuildInputQuery(string helpid, string queryValue, string pyField, string outJsonQuery, string leftLikeJsonQuery, ref string query)
        {
            CommonHelpEntity helpitem = this.GetCommonHelpItem(helpid);
            StringBuilder strb = new StringBuilder();

            int paramCount = 0;
            bool valueflg = false;
            bool nameflg = false;

            //无奈
            NG3.Data.Service.DbHelper.Open();
            NG3.Data.DbVendor vender = NG3.Data.Service.DbHelper.Vendor;
            NG3.Data.Service.DbHelper.Close();

            string codeLikeStr = string.Empty;
            string nameLikeStr = string.Empty;

            if (helpitem.CodeField.IndexOf(".") > 0)
            {
                codeLikeStr = " like '%" + queryValue + "%'";
            }
            else
            {
                valueflg = true;
                //codeLikeStr = " like '%'+{" + paramCount.ToString() + "}+'%'";
                codeLikeStr = " like {" + paramCount.ToString() + "}";
                paramCount++;
            }

            if (helpitem.NameField.IndexOf(".") > 0)
            {
                nameLikeStr = " like '%" + queryValue + "%'";
            }
            else
            {
                nameflg = true;
                //nameLikeStr = " like '%'+{" + paramCount + "}+'%'";
                nameLikeStr = " like {" + paramCount + "}";
                paramCount++;
            }

            //获取汉字拼音首字母函数
            string functionName = "dbo.fun_getPY";
            //if (vender == NG3.Data.DbVendor.Oracle)
            if (vender == NG3.Data.DbVendor.Oracle || vender == NG3.Data.DbVendor.MySql)
            {
                functionName = "fun_getPY";
            }

            strb.Append(" ( ");
            //strb.Append(helpitem.CodeField);
            if (!string.IsNullOrWhiteSpace(helpitem.UserCodeField))
            {
                strb.Append(helpitem.UserCodeField);
            }
            else
            {
                strb.Append(helpitem.CodeField);
            }

            strb.Append(codeLikeStr);
            strb.Append(" or ");
            if (helpitem.NameField != helpitem.CodeField && helpitem.NameField != helpitem.UserCodeField)//代码列和名称列一样的
            {
                strb.Append(helpitem.NameField);
                strb.Append(nameLikeStr);
                //strb.Append(" or ");
            }

            if (IsAlphabet(queryValue))
            {
                strb.Append(" or ");
                if (string.IsNullOrWhiteSpace(pyField))
                {
                    strb.Append(functionName + "(");
                    strb.Append(helpitem.NameField);
                    strb.Append(") ");
                }
                else
                {
                    strb.Append(pyField);
                }
                strb.Append(" like '%");
                strb.Append(queryValue);
                strb.Append("%' )");
            }
            else
            {
                strb.Append(")");
            }


            List<NGDataParameter> paramList = new List<NGDataParameter>();
            if (valueflg && nameflg)
            {
                NGDataParameter[] p = new NGDataParameter[2];
                p[0] = new NGDataParameter(helpitem.CodeField, DbType.AnsiString);
                //p[0].Value = queryValue;
                p[0].Value = "%" + queryValue + "%";

                p[1] = new NGDataParameter(helpitem.NameField, DbType.AnsiString);
                //p[1].Value = queryValue;
                p[1].Value = "%" + queryValue + "%";
                paramList.Add(p[0]);

                if (helpitem.NameField != helpitem.CodeField && helpitem.NameField != helpitem.UserCodeField)//代码列和名称列一样的
                {
                    paramList.Add(p[1]);
                }
            }
            else if (valueflg && !nameflg)
            {
                NGDataParameter[] p = new NGDataParameter[1];
                p[0] = new NGDataParameter(helpitem.CodeField, DbType.AnsiString);
                //p[0].Value = queryValue;
                p[0].Value = "%" + queryValue + "%";
                paramList.Add(p[0]);
            }
            else if (!valueflg && nameflg)
            {
                NGDataParameter[] p = new NGDataParameter[1];
                p[0] = new NGDataParameter(helpitem.NameField, DbType.AnsiString);
                //p[0].Value = queryValue;
                p[0].Value = "%" + queryValue + "%";
                paramList.Add(p[0]);
            }


            if (!string.IsNullOrEmpty(outJsonQuery))//通用帮助外部条件处理
            {
                Dictionary<string, object> outFilter = JsonConvert.DeserializeObject<Dictionary<string, object>>(outJsonQuery);//通用帮助
                foreach (KeyValuePair<string, object> item in outFilter)
                {
                    //if (item.Value is string)
                    if (item.Value != null)
                    {
                        if (!string.IsNullOrEmpty(item.Value.ToString()))
                        {

                            strb.Append(" and ");

                            string columnName = string.Empty;
                            DbType dbtype = DbType.AnsiString;
                            if (item.Value is Int64)
                            {
                                dbtype = DbType.Int64;
                            }

                            if (item.Key.ToString().EndsWith("*ngLow"))//下限
                            {
                                string[] arr = item.Key.Split('*');

                                if (arr[1] == "num")//数字字段
                                {
                                    strb.Append(arr[0] + " >= " + item.Value);
                                }
                                else//日期字段
                                {
                                    strb.Append(arr[0] + " >= '" + item.Value + "'");
                                }
                            }
                            else if (item.Key.ToString().EndsWith("*ngUP"))//上限
                            {
                                string[] arr = item.Key.Split('*');

                                if (arr[1] == "num")//数字字段
                                {
                                    strb.Append(arr[0] + " <= " + item.Value);
                                }
                                else//日期
                                {
                                    strb.Append(arr[0] + " <= '" + item.Value + "'");
                                }
                            }
                            else
                            {
                                columnName = item.Key;

                                if (columnName.IndexOf(".") > 0)
                                {
                                    strb.Append(item.Key + "='" + item.Value + "'");//带表名不参数化
                                }
                                else
                                {
                                    strb.Append(item.Key + "={" + paramCount.ToString() + "}");//外部条件用"="

                                    NGDataParameter param = new NGDataParameter(columnName, dbtype);
                                    param.Value = item.Value;
                                    paramList.Add(param);

                                    paramCount++;
                                }
                            }
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(leftLikeJsonQuery))
            {
                Dictionary<string, object> d = JsonConvert.DeserializeObject<Dictionary<string, object>>(leftLikeJsonQuery);

                foreach (KeyValuePair<string, object> item in d)
                {

                    //if (item.Value is string)
                    if (item.Value != null)
                    {
                        if (!string.IsNullOrEmpty(item.Value.ToString()))
                        {

                            strb.Append(" and ");


                            string columnName = string.Empty;
                            DbType dbtype = DbType.AnsiString;
                            if (item.Value is Int64)
                            {
                                dbtype = DbType.Int64;
                            }

                            columnName = item.Key;

                            if (columnName.IndexOf(".") > 0)//字段带点号，无法参数化
                            {
                                strb.Append(item.Key + " like '" + item.Value + "%'");
                            }
                            else
                            {

                                //匹配like value%
                                //strb.Append(item.Key + " like '' +{" + paramCount.ToString() + "}+ '%'");
                                strb.Append(item.Key + " like  +{" + paramCount.ToString() + "}");

                                //判断是否带表名，如果带表名，则去掉，表名不能参数化
                                string[] cols = columnName.Split('.');

                                if (cols.Length > 1)
                                {
                                    columnName = cols[1];
                                }
                                NGDataParameter p = new NGDataParameter(columnName, dbtype);
                                //p.Value = item.Value;
                                p.Value = item.Value + "%";
                                paramList.Add(p);

                                paramCount++;
                            }
                        }
                    }
                }
            }

            query = strb.ToString();
            return paramList.ToArray();
        }

        //判断查询是否是全字母
        private bool IsAlphabet(string queryValue)
        {
            string pattern = @"^[A-Za-z]+$";
            Regex regex = new Regex(pattern);
            bool flg = regex.IsMatch(queryValue);
            return flg;
        }

        #region 代码转名称


        /// <summary>
        ///根据代码获取名称
        /// </summary>
        /// <param name="helpid">帮助标记</param>
        /// <param name="code">值</param>
        /// <param name="selectMode">多选还是单选</param>
        /// <param name="clientQuery">查询条件</param>
        /// <param name="outJsonQuery">外部注入条件</param>
        /// <param name="helpType">帮助类型</param>
        /// <returns></returns>
        public string GetName(string helpid, string code, string selectMode, string clientQuery, string outJsonQuery, string helpType)
        {
            CommonHelpEntity item = this.GetCommonHelpItem(helpid);//this.GetHelpItem(helpid);

            string result = string.Empty;

            string table = item.TableName;
            if (helpType.ToLower() == HelpType.NGmshelp.ToString().ToLower())//主从明细帮助
            {
                table = item.DetailTable;//明细表
            }

            //if (item.Mode == HelpMode.Default)
            if (!item.OutDataSource)
            {
                StringBuilder strbsql = new StringBuilder();

                strbsql.Append("select DISTINCT ");//不用过滤条件，会出现重复
                strbsql.Append(item.NameField);
                strbsql.Append(" from ");
                strbsql.Append(table);
                strbsql.Append(" where ");

                if (SelectMode.Multi.ToString().ToUpper() == selectMode.ToUpper())
                {
                    string inStatement = GetInStatement(code);//拼装in语句
                    strbsql.Append(item.CodeField);
                    strbsql.Append(" in ");
                    strbsql.Append("(" + inStatement + ")");
                }
                else
                {
                    strbsql.Append(item.CodeField);
                    strbsql.Append("=");
                    strbsql.Append("'" + code + "'");
                }

                string query = string.Empty;

                IDataParameter[] p = DataConverterHelper.BuildQueryWithParam(clientQuery, outJsonQuery, ref query);

                //if (!string.IsNullOrEmpty(item.SqlFilter))
                //{
                //    strbsql.Append(" and " + item.SqlFilter);
                //}

                if (!string.IsNullOrEmpty(query))
                {
                    strbsql.Append(" and " + query);
                }


                if (SelectMode.Multi.ToString().ToUpper() == selectMode.ToUpper())
                {
                    StringBuilder buildstr = new StringBuilder();
                    DataTable dt = DbHelper.GetDataTable(strbsql.ToString(), p);
                    int count = dt.Rows.Count - 1;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        string nameField = item.NameField;
                        if (item.NameField.IndexOf('.') > 0)
                        {
                            nameField = item.NameField.Split('.')[1];//去掉表名
                        }
                        buildstr.Append(dr[nameField]);
                        if (i < count)
                        {
                            buildstr.Append(",");
                        }
                    }
                    result = buildstr.ToString();
                }
                else
                {
                    object obj = DbHelper.ExecuteScalar(strbsql.ToString(), p);
                    result = (obj == null ? string.Empty : obj.ToString());
                }
            }
            else //插件模式通过反射获取帮助名称值
            {
                string fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "I6Rules" + Path.DirectorySeparatorChar + item.Assembly);

                if (File.Exists(fullpath))
                {
                    Assembly assem = Assembly.LoadFile(fullpath);
                    object instance = assem.CreateInstance(item.ClassName);

                    IHelp help = instance as IHelp;

                    if (help != null)
                    {
                        result = help.GetName(helpid, code, selectMode, clientQuery, outJsonQuery);
                    }
                    else
                    {
                        throw new Exception(string.Format("程序集{0}中的{1}类未实现SUP.Common.Base.IHelp接口", item.Assembly, item.ClassName));
                    }

                }
                else
                {
                    throw new Exception(string.Format("服务端Rules目录下找不到程序集{0}", item.Assembly));
                }
            }

            return result;

        }

        private static string GetInStatement(string code)
        {
            StringBuilder sb = new StringBuilder();
            string s = code;
            if (code.StartsWith("["))
            {
                s = code.Split('\"')[1];
            }

            string[] fileds = s.Split(',');
            foreach (string str in fileds)
            {
                sb.Append("'");
                sb.Append(str);
                sb.Append("'");
                sb.Append(",");
            }
            return sb.ToString().TrimEnd(',');//移除最后一个逗号
        }

        /// <summary>
        /// 代码转名称,此方法，名称值会直接替换代码字段的值，代码字段是string类型可以使用
        /// </summary>
        /// <param name="dt">需要代码转名称的数据集</param>
        /// <param name="codeField">代码字段</param>
        /// <param name="helpid">通用帮助id</param>
        /// <param name="dataInDetail">数据在明细表中</param>
        public void CodeToName(DataTable dt, string codeField, string helpid, bool dataInDetail = false)
        {
            this.CodeToName(dt, codeField, codeField, helpid, string.Empty, dataInDetail);
        }

        /// <summary>
        /// 代码转名称,此方法，名称值会写入modifyField字段,代码字段不会被覆盖
        /// </summary>
        /// <param name="codeField">dt中需要转名称的代码字段</param>
        /// <param name="modifyField">需要修改值的字段</param>
        /// <param name="helpid">通用帮助id</param>
        /// <param name="dt">需要代码转名称的数据集</param>
        /// <param name="dataInDetail">数据在明细表中</param>
        public void CodeToName(string codeField, string modifyField, string helpid, DataTable dt, bool dataInDetail = false)
        {
            this.CodeToName(dt, codeField, modifyField, helpid, string.Empty, dataInDetail);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeField">dt中需要转名称的代码字段</param>
        /// <param name="modifyField">需要修改值的字段</param>
        /// <param name="helpid">通用帮助id</param>
        /// <param name="filter">过滤条件</param>
        /// <param name="dt">需要代码转名称的数据集</param>
        public void CodeToName(string codeField, string modifyField, string helpid, string filter, DataTable dt, bool dataInDetail = false)
        {
            this.CodeToName(dt, codeField, modifyField, helpid, filter, dataInDetail);
        }

        /// <summary>
        /// 代码转名称
        /// </summary>
        /// <param name="dt">需要代码转名称的数据集</param>
        /// <param name="codeField">dt中需要转名称的代码字段</param>
        /// <param name="helpid">通用帮助id</param>
        /// <param name="filter">过滤条件</param>
        public void CodeToName(DataTable dt, string codeField, string helpid, string filter, bool dataInDetail = false)
        {
            this.CodeToName(dt, codeField, codeField, helpid, filter, dataInDetail);
        }

        /// <summary>
        /// 代码转名称
        /// </summary>
        /// <param name="dt">需要代码转名称的数据集</param>
        /// <param name="codeField">dt中需要转名称的代码字段</param>
        /// <param name="modifyfield">修改的字段名称</param>
        /// <param name="helpid">通用帮助id</param>
        /// <param name="filter">过滤条件</param>
        public void CodeToName(DataTable dt, string codeField, string modifyField, string helpid, string filter, bool isMulti, bool dataInDetail = false)
        {
            CommonHelpEntity item = this.GetCommonHelpItem(helpid);

            DataTable helpdt = GetHelpDt(dt, item, codeField, filter, isMulti,dataInDetail);
            if (helpdt == null) return;

            //去掉表名
            string codeF = item.CodeField;
            if (item.CodeField.IndexOf(".") > 0)
            {
                codeF = item.CodeField.Split('.')[1];
            }

            string nameF = item.NameField;
            if (item.NameField.IndexOf(".") > 0)
            {
                nameF = item.NameField.Split('.')[1];
            }

            foreach (DataRow dr in dt.Rows)
            {
                string value = (dr[codeField] == null || dr[codeField] == DBNull.Value) ? string.Empty : dr[codeField].ToString();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    //DataRow[] helpdrs = helpdt.Select(item.CodeField + "='" + value + "'");

                    if (isMulti)//多选
                    {
                        DataRow[] helpdrs = helpdt.Select(codeF + " in (" + GetInStatement(value) + ")");
                        if (helpdrs.Length > 0)
                        {
                            StringBuilder strb = new StringBuilder();
                            foreach (DataRow innerdr in helpdrs)
                            {
                                strb.Append(innerdr[nameF].ToString());
                                strb.Append(",");
                            }
                            if (helpdrs.Length > 0)
                            {                                
                                dr[modifyField] = strb.ToString().TrimEnd(',');//代码转成名称
                            }
                        }
                    }
                    else
                    {
                        DataRow[] helpdrs = helpdt.Select(codeF + "='" + value + "'");

                        if (helpdrs.Length > 0)
                        {                           
                            dr[modifyField] = helpdrs[0][nameF];//代码转成名称
                        }
                    }
                }
            }

        }

        public JArray CodeToName(JArray ja, string codeField, string helpid)
        {
            return CodeToName(ja, codeField, codeField, helpid, string.Empty);
        }

        public JArray CodeToName(JArray ja, string codeField, string helpid, string filter)
        {
            return CodeToName(ja, codeField, codeField, helpid, filter);
        }
        public JArray CodeToName(JArray ja, string codeField, string modifyField, string helpid, string filter)
        {

            CommonHelpEntity item = this.GetCommonHelpItem(helpid);

            DataTable helpdt = new DataTable();

            int incount = 0;
            int count = ja.Count;
            int upindex = count - 1;
            StringBuilder instrb = new StringBuilder();
            //处理in过滤条件  
            instrb.Append("(");
            for (int i = 0; i < count; i++)
            {
                if (ja[i][codeField] == null || String.IsNullOrWhiteSpace(ja[i][codeField].ToString())) continue;

                instrb.Append("'" + ja[i][codeField] + "'");
                instrb.Append(",");
                incount++;
            }

            if (incount == 0) return ja;//一个都不需要转换

            instrb.Append(")");

            string instr = instrb.ToString();
            if (instr.IndexOf(",)") > 0)
            {
                instr = instr.Replace(",)", ")");//处理最后一个","
            }

            if (item.Mode == HelpMode.Default)
            {

                StringBuilder strb = new StringBuilder();
                strb.Append("select ");
                strb.Append(item.CodeField);
                strb.Append("," + item.NameField);
                strb.Append(" from ");
                strb.Append(item.TableName);
                strb.Append(" where ");
                strb.Append(item.CodeField + " in ");
                strb.Append(instr);
                //if (!string.IsNullOrWhiteSpace(item.SqlFilter))
                //{
                //    strb.Append(" and " + item.SqlFilter);
                //}
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    strb.Append(" and " + filter);
                }

                helpdt = DbHelper.GetDataTable(strb.ToString());

            }
            else if (item.Mode == HelpMode.GetHelpResult)
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

                    IHelp help = instance as IHelp;

                    if (help != null)
                    {
                        helpdt = help.CodeToName(instr);
                    }
                    else
                    {
                        throw new Exception(string.Format("程序集{0}中的{1}类未实现SUP.Common.Base.IHelp接口", item.Assembly, item.ClassName));
                    }
                }
                else
                {
                    throw new Exception(string.Format("服务端Rules目录下找不到程序集{0}", item.Assembly));
                }

            }

            //去掉表名
            string codeF = item.CodeField;
            if (item.CodeField.IndexOf(".") > 0)
            {
                codeF = item.CodeField.Split('.')[1];
            }
            string nameF = item.NameField;
            if (item.NameField.IndexOf(".") > 0)
            {
                nameF = item.NameField.Split('.')[1];
            }

            for (int i = 0; i < count; i++)
            {
                string value = (ja[i][codeField] == null) ? string.Empty : ja[i][codeField].ToString();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    //DataRow[] helpdrs = helpdt.Select(item.CodeField + "='" + value + "'");
                    DataRow[] helpdrs = helpdt.Select(codeF + "='" + value + "'");

                    if (helpdrs.Length > 0)
                    {
                        //dr[codeField] = helpdrs[0][item.NameField];//代码转成名称
                        ja[i][modifyField] = helpdrs[0][nameF].ToString();//代码转成名称
                    }
                }
            }

            return ja;

        }


        /// <summary>
        /// 单独修改一个对象的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="codeProperty"></param>
        /// <param name="helpid"></param>
        /// <param name="dataInDetail"></param>
        public void CodeToName<T>(T item, string codeProperty, string helpid, bool dataInDetail = false)
        {
            List<T> ls = new List<T>();
            ls.Add(item);

            CodeToName<T>(ls, codeProperty, helpid, dataInDetail);
        }

        public void CodeToName<T>(T item, string codeProperty, string modifyProperty, string helpid, bool dataInDetail = false)
        {
            List<T> ls = new List<T>();
            ls.Add(item);

            CodeToName<T>(ls, codeProperty, modifyProperty, helpid, string.Empty, dataInDetail);
        }


        public void CodeToName<T>(T item, string codeProperty, string modifyProperty, string helpid, string filter, bool isMulti, bool dataInDetail = false)
        {
            List<T> ls = new List<T>();
            ls.Add(item);

            CodeToName<T>(ls, codeProperty, modifyProperty, string.Empty, helpid, filter, isMulti, dataInDetail);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">业务数据集</param>
        /// <param name="codeProperty">代码属性</param>
        /// <param name="helpid">帮助标记</param>
        /// <param name="dataInDetail">是否为明细表的数据</param>
        public void CodeToName<T>(IList<T> list, string codeProperty, string helpid, bool dataInDetail = false)
        {
            CodeToName<T>(list, codeProperty, codeProperty, helpid, string.Empty, dataInDetail);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">业务数据集</param>
        /// <param name="codeProperty">代码属性</param>
        /// <param name="helpid">帮助标记</param>
        /// <param name="filter">过滤条件</param>
        /// <param name="dataInDetail">是否为明细表的数据</param>
        public void CodeToName<T>(IList<T> list, string codeProperty, string helpid, string filter, bool dataInDetail = false)
        {
            CodeToName<T>(list, codeProperty, codeProperty, helpid, filter, dataInDetail);
        }

        /// <summary>
        /// 实体集合代码转名称
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="list">业务数据集</param>
        /// <param name="codeProperty">代码属性</param>
        /// <param name="modifyProperty">待修改的属性(名称列的属性)</param>
        /// <param name="helpid">帮助标记</param>
        /// <param name="filter">过滤条件</param>
        /// <param name="dataInDetail">是否为明细表的数据</param>
        public void CodeToName<T>(IList<T> list, string codeProperty, string modifyProperty, string helpid, string filter, bool dataInDetail = false)
        {
            CodeToName<T>(list, codeProperty, modifyProperty, string.Empty, helpid, filter, false, dataInDetail);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">业务数据集</param>
        /// <param name="codeProperty">代码属性</param>
        /// <param name="modifyProperty">想修改的属性</param>
        /// <param name="youCodeField">代码列(有时候代码属性和代码列不一定一致)</param>
        /// <param name="helpid">帮助标记</param>
        /// <param name="filter">过滤条件</param>
        /// <param name="dataInDetail">是否为明细表的数据</param>
        public void CodeToName<T>(IList<T> list, string codeProperty, string modifyProperty, string youCodeField, string helpid, string filter, bool isMulti, bool dataInDetail = false)
        {
            if (list.Count == 0) return;//一个都不需要转换

            CommonHelpEntity item = this.GetCommonHelpItem(helpid);

            DataTable helpdt = new DataTable();

            string table = item.TableName;
            if (dataInDetail)
            {
                table = item.DetailTable;//转明细表
            }

            int incount;
            string codefield = item.CodeField;
            if (!string.IsNullOrWhiteSpace(youCodeField))
            {
                codefield = youCodeField;//一般是phid
            }

            string instr = GetInStatement(list, codeProperty, codefield, isMulti, out incount);
            if (incount == 0) return;//一个都不需要转换

            //if (item.Mode == HelpMode.Default)
            if (!item.OutDataSource)
            {

                StringBuilder strb = new StringBuilder();
                strb.Append("select ");
                //strb.Append(item.CodeField);
                strb.Append(codefield);
                strb.Append("," + item.NameField);
                strb.Append(" from ");
                strb.Append(table);
                strb.Append(" where ");
                //strb.Append(item.CodeField + " in ");
                //strb.Append(codefield + " in ");
                strb.Append(instr);
                //if (!string.IsNullOrWhiteSpace(item.SqlFilter))
                //{
                //    strb.Append(" and " + item.SqlFilter);
                //}
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    strb.Append(" and " + filter);
                }

                helpdt = DbHelper.GetDataTable(strb.ToString());

            }
            else
            {
                string fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "I6Rules" + Path.DirectorySeparatorChar + item.Assembly);

                if (File.Exists(fullpath))
                {
                    Assembly assem = Assembly.LoadFile(fullpath);
                    object instance = assem.CreateInstance(item.ClassName);

                    IHelp help = instance as IHelp;

                    if (help != null)
                    {
                        helpdt = help.CodeToName(instr);
                    }
                    else
                    {
                        throw new Exception(string.Format("程序集{0}中的{1}类未实现SUP.Common.Base.IHelp接口", item.Assembly, item.ClassName));
                    }
                }
                else
                {
                    throw new Exception(string.Format("服务端Rules目录下找不到程序集{0}", item.Assembly));
                }

            }

            if (helpdt == null || helpdt.Rows.Count==0) return;

            //带表名
            if (codefield.IndexOf(".") > 0)
            {
                codefield = codefield.Split('.')[1];
            }
            string nameF = item.NameField;
            if (item.NameField.IndexOf(".") > 0)
            {
                nameF = item.NameField.Split('.')[1];
            }

            foreach (T entity in list)
            {
                //string value = (dr[codeField] == null || dr[codeField] == DBNull.Value) ? string.Empty : dr[codeField].ToString();
                Type t = entity.GetType();
                object obj = t.GetProperty(codeProperty).GetValue(entity, null);
                if (obj == null || String.IsNullOrWhiteSpace(obj.ToString())) continue;
                string value = obj.ToString();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (isMulti)//多选
                    {
                        DataRow[] helpdrs = helpdt.Select(codefield + " in (" + GetInStatement(value) + ")");
                        if (helpdrs.Length > 0)
                        {
                            StringBuilder strb = new StringBuilder();
                            foreach (DataRow dr in helpdrs)
                            {
                                strb.Append(dr[nameF].ToString());
                                strb.Append(",");
                            }
                            if (string.IsNullOrWhiteSpace(modifyProperty))
                            {
                                t.GetProperty(codeProperty).SetValue(entity, strb.ToString().TrimEnd(','), null);
                            }
                            else
                            {
                                t.GetProperty(modifyProperty).SetValue(entity, strb.ToString().TrimEnd(','), null);
                            }
                        }
                    }
                    else
                    {
                        DataRow[] helpdrs = helpdt.Select(codefield + "='" + value + "'");
                        if (helpdrs.Length > 0)
                        {
                            if (string.IsNullOrWhiteSpace(modifyProperty))
                            {
                                t.GetProperty(codeProperty).SetValue(entity, helpdrs[0][nameF].ToString(), null);
                            }
                            else
                            {
                                t.GetProperty(modifyProperty).SetValue(entity, helpdrs[0][nameF].ToString(), null);
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 拼接in语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">待转集合</param>
        /// <param name="codeProperty">属性名</param>
        /// <param name="codefield">字段名</param>
        /// <param name="isMulti">是否多选</param>
        /// <param name="incount">in个数</param>
        /// <returns></returns>
        private string GetInStatement<T>(IList<T> list, string codeProperty, string codefield, bool isMulti, out int incount)
        {
            int count = list.Count;
            incount = 0;
            bool isNum = false;//数据类型是否为数字

            List<string> codeList = new List<string>();
            //处理in过滤条件           
            for (int i = 0; i < count; i++)
            {
                Type t = list[i].GetType();
                object obj = t.GetProperty(codeProperty).GetValue(list[i], null);
                if (obj == null || String.IsNullOrWhiteSpace(obj.ToString())) continue;

                Type type = t.GetProperty(codeProperty).PropertyType;
                if (type == typeof(Int64) || type == typeof(Int32))
                {
                    isNum = true;
                }

                if (isMulti)
                {
                    string str = obj.ToString();
                    if (str.StartsWith("["))//有些多选保存带[]符号
                    {
                        str = str.Split('\"')[1];
                    }
                    string[] arrStr = str.Split(',');
                    //int lastindex = arrStr.Length - 1;
                    for (int j = 0; j < arrStr.Length; j++)
                    {
                        string codeVal = string.Empty;
                        if (isNum)
                        {
                            codeVal = arrStr[j];
                        }
                        else
                        {
                            codeVal = "'" + arrStr[j] + "'";
                        }

                        if (!codeList.Contains(codeVal))
                        {
                            codeList.Add(codeVal);
                        }
                    }

                }
                else
                {
                    string codeVal = string.Empty;
                    if (isNum)
                    {
                        codeVal = obj.ToString();
                    }
                    else
                    {
                        codeVal = "'" + obj.ToString() + "'";
                    }
                    if (!codeList.Contains(codeVal))
                    {
                        codeList.Add(codeVal);
                    }
                }
                incount++;
            }

            //分组
            List<List<string>> groupList = new List<List<string>>();
            for (int i = 0; i < codeList.Count; i++)
            {
                int groupIndex = i / 800;//oracle in 语句不能超过1000个
                if (i % 800 == 0)
                {
                    groupList.Add(new List<string>());
                }
                List<string> tempList = groupList[groupIndex];
                tempList.Add(codeList[i]);
            }

            StringBuilder strb = new StringBuilder();
            int upIndex = groupList.Count - 1;
            for (int i = 0; i < groupList.Count; i++)
            {
                List<string> ls = groupList[i];
                string instr = string.Join(",", ls.ToArray());
                if (i < upIndex)
                {
                    strb.Append(codefield + " in (" + instr + ") or ");
                }
                else
                {
                    strb.Append(codefield + " in (" + instr + ") ");
                }
            }

            return strb.ToString();
        }

        /// <summary>
        /// 批量代码转名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">业务数据集</param>
        /// <param name="infoList">帮助信息列表</param>
        public void BatchCodeToName<T>(IList<T> list, List<CodeToNameInfoEntity> infoList)
        {
            foreach (CodeToNameInfoEntity item in infoList)
            {
                CodeToName<T>(list, item.CodeProperty, item.ModifyProperty, item.HelpID, item.Filter);
            }
        }

        private DataTable GetHelpDt(DataTable dt, CommonHelpEntity item, string codeField, string filter, bool isMulti, bool dataInDetail = false)
        {
            DataTable helpdt = new DataTable();

            string table = item.TableName;
            if (dataInDetail)
            {
                table = item.DetailTable;
            }

            int incount = 0;
            int count = dt.Rows.Count;
            int upindex = count - 1;
            //处理in过滤条件  
            List<string> codeList = new List<string>();
            for (int i = 0; i < count; i++)
            {
                if (dt.Rows[i][codeField] == null || dt.Rows[i][codeField] == DBNull.Value || String.IsNullOrWhiteSpace(dt.Rows[i][codeField].ToString())) continue;

                if (isMulti)//多选
                {
                    string str = dt.Rows[i][codeField].ToString();
                    if (str.StartsWith("["))//有些多选保存带[]符号
                    {
                        str = str.Split('\"')[1];
                    }
                    string[] arrStr = str.Split(',');
                    for (int j = 0; j < arrStr.Length; j++)
                    {
                        string codeVal = arrStr[j];
                        if (!codeList.Contains(codeVal))
                        {
                            codeList.Add("'" + codeVal + "'");
                            incount++;
                        }
                    }
                }
                else
                {
                    codeList.Add("'" + dt.Rows[i][codeField] + "'");                   
                    incount++;
                }
            }
            if (incount == 0) return null;//一个都不需要转换
           
            string instr = "(" + string.Join(",", codeList.ToArray()) + ")";
           
            if (!item.OutDataSource)
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("select ");
                strb.Append(item.AllFieldWithTableName);
                //strb.Append(item.AllField);               
                strb.Append(" from ");
                strb.Append(table);
                strb.Append(" where ");
                strb.Append(item.CodeField + " in ");
                strb.Append(instr);
                //if (!string.IsNullOrWhiteSpace(item.SqlFilter))
                //{
                //    strb.Append(" and " + item.SqlFilter);
                //}
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    strb.Append(" and " + filter);
                }

                helpdt = DbHelper.GetDataTable(strb.ToString());

            }
            else
            {
                string fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "I6Rules" + Path.DirectorySeparatorChar + item.Assembly);

                if (File.Exists(fullpath))
                {
                    Assembly assem = Assembly.LoadFile(fullpath);
                    object instance = assem.CreateInstance(item.ClassName);

                    IHelp help = instance as IHelp;

                    if (help != null)
                    {
                        helpdt = help.CodeToName(instr);
                    }
                    else
                    {
                        throw new Exception(string.Format("程序集{0}中的{1}类未实现SUP.Common.Base.IHelp接口", item.Assembly, item.ClassName));
                    }
                }
                else
                {
                    throw new Exception(string.Format("服务端Rules目录下找不到程序集{0}", item.Assembly));
                }
            }

            return helpdt;
        }

        private DataTable GetHelpDt<T>(IList<T> list, CommonHelpEntity item, string codeProperty, string filter)
        {
            
            DataTable helpdt = new DataTable();
            if (list.Count == 0) return helpdt;//一个都不需要转换

            int incount = 0;
            //int count = list.Count;
            //int upindex = count - 1;
            //StringBuilder instrb = new StringBuilder();
            ////处理in过滤条件  
            //instrb.Append("(");
            //for (int i = 0; i < count; i++)
            //{
            //    Type t = list[i].GetType();
            //    object obj = t.GetProperty(codeProperty).GetValue(list[i], null);
            //    if (obj == null || String.IsNullOrWhiteSpace(obj.ToString())) continue;

            //    instrb.Append("'" + obj.ToString() + "'");
            //    instrb.Append(",");
            //    incount++;
            //}

            //if (incount == 0) return helpdt;//一个都不需要转换

            //instrb.Append(")");

            //string instr = instrb.ToString();
            //if (instr.IndexOf(",)") > 0)
            //{
            //    instr = instr.Replace(",)", ")");//处理最后一个","
            //}

            string codefield = item.CodeField;
            string instr = GetInStatement(list, codeProperty, codefield, false, out incount);
            //if (item.Mode == HelpMode.Default)
            if (!item.OutDataSource)
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("select ");
                //strb.Append(item.AllField);
                strb.Append(item.AllFieldWithTableName);
                strb.Append(" from ");
                strb.Append(item.TableName);
                strb.Append(" where ");
                //strb.Append(item.CodeField + " in ");
                strb.Append(instr);
                //if (!string.IsNullOrWhiteSpace(item.SqlFilter))
                //{
                //    strb.Append(" and " + item.SqlFilter);
                //}
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    strb.Append(" and " + filter);
                }

                helpdt = DbHelper.GetDataTable(strb.ToString());

            }
            else
            {
                string fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "I6Rules" + Path.DirectorySeparatorChar + item.Assembly);

                if (File.Exists(fullpath))
                {
                    Assembly assem = Assembly.LoadFile(fullpath);
                    object instance = assem.CreateInstance(item.ClassName);

                    IHelp help = instance as IHelp;

                    if (help != null)
                    {
                        helpdt = help.CodeToName(instr);
                    }
                    else
                    {
                        throw new Exception(string.Format("程序集{0}中的{1}类未实现SUP.Common.Base.IHelp接口", item.Assembly, item.ClassName));
                    }
                }
                else
                {
                    throw new Exception(string.Format("服务端Rules目录下找不到程序集{0}", item.Assembly));
                }
            }

            return helpdt;
        }

        /// <summary>
        /// 获取多选帮助已经选中的值
        /// </summary>
        /// <param name="helpid"></param>
        /// <param name="codes"></param>
        /// <returns></returns>
        public DataTable GetSelectedData(string helpid, string codes, bool mode)
        {
            CommonHelpEntity item = this.GetCommonHelpItem(helpid);
            StringBuilder builder = new StringBuilder();

            string temp = codes.Replace("[\"", "").Replace("\"]", "");//["1101,1201"]
            string[] fields = temp.Split(',');
            int positon = fields.Length - 1;
            for (int i = 0; i < fields.Length; i++)
            {
                string s = fields[i].Replace("\"", "");
                if (i == positon)
                {
                    builder.Append("'" + s + "'");//["1101","1201"]
                }
                else
                {
                    builder.Append("'" + s + "',");
                }
            }

            StringBuilder strb = new StringBuilder();
            strb.Append("select ");
            //strb.Append(item.AllField);
            strb.Append(item.AllFieldWithTableName);
            strb.Append(" from ");
            strb.Append(item.TableName);
            strb.Append(" where ");
            strb.Append(item.CodeField);
            strb.Append(" in (");
            strb.Append(builder.ToString());
            strb.Append(")");

            DataTable dt = DbHelper.GetDataTable(strb.ToString());

            if (mode)
            {
                dt.TableName = item.TableName;
                return ConvertFieldColToPropertyCol(dt, item.FieldPropertyDic, item.FieldDic);
            }

            return dt;
        }


        #endregion

        #region 根据帮助获取其他额外信息
        public void ConvertDataByHelpInfo(DataTable dt, Dictionary<string, string> dic, string codeField, string helpid, string filter, bool isMulti)
        {
            CommonHelpEntity helpitem = this.GetCommonHelpItem(helpid);
            DataTable helpdt = GetHelpDt(dt, helpitem, codeField, filter,isMulti);

            if (helpdt == null) return;

            string str = helpitem.CodeField;
            if (helpitem.CodeField.IndexOf(".") > 0)
            {
                str = helpitem.CodeField.Split('.')[1];//去掉表名
            }

            foreach (DataRow dr in dt.Rows)
            {
                string code = dr[codeField].ToString();

                DataRow[] arr = helpdt.Select(string.Format(str + "='{0}'", code));
                if (arr.Length > 0)
                {
                    foreach (KeyValuePair<string, string> item in dic)
                    {
                        dr[item.Key] = arr[0][item.Value];
                    }
                }
            }

        }


        /// <summary>
        /// 单条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="dic"></param>
        /// <param name="codeProperty"></param>
        /// <param name="helpid"></param>
        /// <param name="filter"></param>
        public void GetDataByHelpInfo<T>(T item, Dictionary<string, string> dic, string codeProperty, string helpid, string filter)
        {
            List<T> ls = new List<T>();
            ls.Add(item);

            GetDataByHelpInfo<T>(ls, dic, codeProperty, helpid, filter);
        }

        /// <summary>
        /// 根据帮助代码获取其他额外信息
        /// </summary>
        /// <param name="list">业务列表</param>
        /// <param name="dic">业务属性名和帮助列属性名对应：key是业务表属性名，val是帮助字段对应属性名(fg_columns中对应)</param>
        /// <param name="codeProperty">代码列的属性</param>
        /// <param name="helpid">帮助id</param>
        /// <param name="filter">过滤条件</param>
        public void GetDataByHelpInfo<T>(IList<T> list, Dictionary<string, string> dic, string codeProperty, string helpid, string filter)
        {
            CommonHelpEntity helpitem = this.GetCommonHelpItem(helpid);
            DataTable helpdt = GetHelpDt(list, helpitem, codeProperty, filter);

            if (helpdt == null) return;

            //属性转换成字段<property,property> 转成 <property,field>
            Dictionary<string, string> fieldDic = this.GetFields(dic, helpid);

            //去掉表名
            string codeField = helpitem.CodeField;
            if (helpitem.CodeField.IndexOf(".") > 0)
            {
                codeField = helpitem.CodeField.Split('.')[1];
            }

            foreach (T entity in list)
            {
                Type t = entity.GetType();
                object obj = t.GetProperty(codeProperty).GetValue(entity, null);
                if (obj == null || String.IsNullOrWhiteSpace(obj.ToString())) continue;
                string value = obj.ToString();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    //DataRow[] helpdrs = helpdt.Select(helpitem.CodeField + "='" + value + "'");
                    DataRow[] helpdrs = helpdt.Select(codeField + "='" + value + "'");
                    if (helpdrs.Length > 0)
                    {
                        foreach (KeyValuePair<string, string> item in fieldDic)
                        {
                            //t.GetProperty(item.Key).SetValue(entity, helpdrs[0][item.Value].ToString(), null);
                            t.GetProperty(item.Key).SetValue(entity, Convert.ChangeType(helpdrs[0][item.Value], t.GetProperty(item.Key).PropertyType), null);
                        }
                    }
                }
            }

        }

        /// <summary>
        // <property,property> 转成 <property,field>
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="helpid"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetFields(Dictionary<string, string> dic, string helpid)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select fg_helpinfo_sys.tablename,fieldname,seq,showheader from fg_helpinfo_master,fg_helpinfo_sys ");
            sb.Append(" where fg_helpinfo_master.code = fg_helpinfo_sys.masterid and fg_helpinfo_master.helpid ={0} ORDER BY showheader desc , seq");
            //sb.Append(" union");
            //sb.Append(" select fg_helpinfo_user.tablename,fieldname,seq,showheader from fg_helpinfo_master,fg_helpinfo_user ");
            //sb.Append(" where fg_helpinfo_master.code = fg_helpinfo_user.masterid and fg_helpinfo_master.helpid = {0} ORDER BY showheader desc , seq");

            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("helpid", helpid);
            DataTable dt = DbHelper.GetDataTable(sb.ToString(), p);

            Dictionary<string, string> temDic = new Dictionary<string, string>();

            string tables = GetTableNames(dt, "tablename");
            DataTable PropertyDt = new QueryPanelDac().GetPropertyDt(tables);//获取属性

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                string fieldName = dt.Rows[i]["fieldname"].ToString().Trim();
                string fName = fieldName;
                if (fieldName.IndexOf(" ") > 0)
                {//取别名
                    string[] arr = fieldName.Split(' ');
                    fName = arr[arr.Length - 1];
                }

                //string propertyName = DataConverterHelper.FieldToProperty(dt.Rows[i]["tablename"].ToString(), fName);
                string propertyName = GetPropertyName(PropertyDt, dt.Rows[i]["tablename"].ToString(), fName);

                foreach (KeyValuePair<string, string> item in dic)
                {
                    if (item.Value == propertyName)
                    {
                        //dic[item.Key] = fieldName;
                        temDic.Add(item.Key, fName);
                    }
                }
            }

            return temDic;

        }

        #endregion

        /// <summary>
        /// 验证用户输入数据的合法性
        /// </summary>
        /// <param name="helpid"></param>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public bool ValidateData(string helpid, string inputValue, string clientSqlFilter, string selectMode, string helptype)
        {
            CommonHelpEntity item = this.GetCommonHelpItem(helpid);
            string CodeFlag = "0", FieldFlag = "0";

            string table = item.TableName;
            if (helptype.ToLower() == HelpType.NGmshelp.ToString().ToLower())//主从明细帮助
            {
                table = item.DetailTable;//明细表
            }

            StringBuilder strb = new StringBuilder();
            if (HelpMode.Default == item.Mode)
            {
                try
                {

                    if (SelectMode.Multi.ToString() == selectMode)
                    {
                        string inclourse = GetInStatement(inputValue);
                        strb.Append("select COUNT(*) from ");
                        strb.Append(table);
                        strb.Append(" where ");
                        strb.Append(item.CodeField);
                        strb.Append(" in (" + inclourse + ")");

                        int count = Convert.ToInt32(DbHelper.ExecuteScalar(strb.ToString()));
                        CodeFlag = count.ToString();
                        //if (count == inputValue.Split(',').Length)
                        //{
                        //    CodeFlag = count.ToString();
                        //}

                    }
                    else
                    {
                        strb.Append("select COUNT(*) from ");
                        strb.Append(table);
                        strb.Append(" where ");
                        strb.Append(item.CodeField + " ='" + inputValue + "'");
                        if (!String.IsNullOrEmpty(clientSqlFilter))
                        {
                            strb.Append(" and " + clientSqlFilter);
                        }

                        CodeFlag = DbHelper.ExecuteScalar(strb.ToString()).ToString();
                    }


                }
                catch (Exception e)
                {

                }

                if (CodeFlag != "0") return true;//直接返回

                try
                {
                    strb.Clear();

                    if (SelectMode.Multi.ToString() == selectMode)
                    {
                        string inclourse = GetInStatement(inputValue);
                        strb.Append("select COUNT(*) from ");
                        strb.Append(table);
                        strb.Append(" where ");
                        strb.Append(item.NameField);
                        strb.Append(" in (" + inclourse + ")");

                        int count = Convert.ToInt32(DbHelper.ExecuteScalar(strb.ToString()));
                        FieldFlag = count.ToString();
                        //if (count == inputValue.Split(',').Length)
                        //{
                        //    FieldFlag = count.ToString();
                        //}
                        //else
                        //{
                        //}
                    }
                    else
                    {
                        strb.Append("select COUNT(*) from ");
                        strb.Append(table);
                        strb.Append(" where ");
                        strb.Append(item.NameField + " ='" + inputValue + "'");
                        if (!String.IsNullOrEmpty(clientSqlFilter))
                        {
                            strb.Append(" and " + clientSqlFilter);
                        }
                        FieldFlag = DbHelper.ExecuteScalar(strb.ToString()).ToString();
                    }
                }
                catch (Exception e)
                {

                }

                if (CodeFlag == "0" && FieldFlag == "0")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else//插件模式
            {
                bool flag = false;
                string fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "I6Rules" + Path.DirectorySeparatorChar + item.Assembly);
                if (File.Exists(fullpath))
                {
                    Assembly assem = Assembly.LoadFile(fullpath);
                    object instance = assem.CreateInstance(item.ClassName);

                    IValidate help = instance as IValidate;
                    if (help != null)
                    {
                        flag = help.ValidateData(helpid, inputValue, clientSqlFilter, selectMode);
                    }
                    else
                    {
                        flag = true;//没有实现IValidate的先不验证
                        //throw new Exception(string.Format("程序集{0}中的{1}类未实现SUP.Common.Base.IValidate接口", item.Assembly, item.ClassName));
                    }
                }
                else
                {
                    throw new Exception(string.Format("服务端Rules目录下找不到程序集{0}", item.Assembly));
                }

                return flag;
            }//else
        }

        /// <summary>
        /// 获取属性名
        /// </summary>
        /// <param name="helpid"></param>
        /// <returns></returns>
        public string GetProperties(string helpid, Dictionary<string, string> fieldPropertyDic, Dictionary<string, string> fieldDic)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select fg_helpinfo_sys.tablename,fieldname,fieldsource,seq,fg_helpinfo_sys.showheader from fg_helpinfo_master,fg_helpinfo_sys ");
            sb.Append(" where fg_helpinfo_master.code = fg_helpinfo_sys.masterid  and fg_helpinfo_master.helpid ={0} ORDER BY  showheader desc , seq");           

            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("helpid", helpid);
            DataTable dt = DbHelper.GetDataTable(sb.ToString(), p);

            string tables = GetTableNames(dt, "tablename");
            DataTable PropertyDt = new QueryPanelDac().GetPropertyDt(tables);//获取属性

            string properties = string.Empty;

            if (dt.Rows.Count > 0)
            {
                //取类名
                string str = @"SELECT modelname FROM fg_table WHERE  c_name = {0}";
                p[0] = new NGDataParameter("c_name", dt.Rows[0]["tablename"].ToString());
                DataTable ClassDt = DbHelper.GetDataTable(str, p);
                string className = ClassDt.Rows.Count == 0 ? string.Empty : ClassDt.Rows[0]["modelname"].ToString();

                int last = dt.Rows.Count - 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string temp = GetPropertyName(PropertyDt, dt.Rows[i]["tablename"].ToString(), dt.Rows[i]["fieldname"].ToString());

                    string key = dt.Rows[i]["fieldname"].ToString().Trim().ToLower();
                    if (!fieldPropertyDic.ContainsKey(key))//可能帮助注册有重复字段
                    {
                        fieldPropertyDic.Add(key, temp.Trim());//列字段属性对应信息
                        fieldDic.Add(key, dt.Rows[i]["fieldname"].ToString().Trim());//key小写->value大写,恢复datatable的列
                    }
                    else {
                        throw new Exception(string.Format("通用帮助[{0}]的字段[{1}]重复注册了,请及时修复！",helpid,key));
                    }                   

                    if (i == last)
                    {
                        properties += temp;
                    }
                    else
                    {
                        properties += temp + ",";
                    }
                }
            }

            return properties;
        }

        /// <summary>
        /// 获取属性名
        /// </summary>
        /// <param name="helpid"></param>
        /// <returns></returns>
        public string GetDetailTableProperties(string helpid)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select fg_helpinfo_detailtable.tablename,fieldname,seq ,showheader from fg_helpinfo_master,fg_helpinfo_detailtable  ");
            sb.Append(" where fg_helpinfo_master.code = fg_helpinfo_detailtable.masterid and fg_helpinfo_master.helpid ={0} ");
            sb.Append(" order by seq asc  ");

            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("helpid", helpid);
            DataTable dt = DbHelper.GetDataTable(sb.ToString(), p);

            string tables = GetTableNames(dt, "tablename");
            DataTable PropertyDt = new QueryPanelDac().GetPropertyDt(tables);//获取属性
            string properties = string.Empty;

            if (dt.Rows.Count > 0)
            {
                //取类名
                string str = @"SELECT modelname FROM fg_table WHERE  c_name = {0}";
                p[0] = new NGDataParameter("c_name", dt.Rows[0]["tablename"].ToString());
                DataTable ClassDt = DbHelper.GetDataTable(str, p);
                string className = ClassDt.Rows.Count == 0 ? string.Empty : ClassDt.Rows[0]["modelname"].ToString();

                int last = dt.Rows.Count - 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //string temp = dt.Rows[i]["tablename"].ToString() + "." + dt.Rows[i]["fieldname"].ToString();
                    string temp = string.Empty;
                    if (string.IsNullOrEmpty(className))//帮助是插件模式，注册的表名是空的
                    {
                        //temp = DataConverterHelper.FieldToProperty(dt.Rows[i]["tablename"].ToString(),
                        //                                               dt.Rows[i]["fieldname"].ToString());
                        temp = GetPropertyName(PropertyDt, dt.Rows[i]["tablename"].ToString(), dt.Rows[i]["fieldname"].ToString());
                    }
                    else
                    {
                        temp = className + "." +
                                   GetPropertyName(PropertyDt, dt.Rows[i]["tablename"].ToString(), dt.Rows[i]["fieldname"].ToString());
                    }

                    //可以不带类名，之前的写法都用了带类名前缀，继续使用
                    //string temp = DataConverterHelper.FieldToProperty(dt.Rows[i]["tablename"].ToString(),
                    //                                                 dt.Rows[i]["fieldname"].ToString());

                    if (i == last)
                    {
                        properties += temp;
                    }
                    else
                    {
                        properties += temp + ",";
                    }
                }
            }

            return properties;
        }

        /// <summary>
        /// 字段列转属性列
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable ConvertFieldColToPropertyCol(DataTable dt, Dictionary<string, string> fieldPropertyDic, Dictionary<string, string> fieldDic)
        {
            // 字段列转属性列
            foreach (DataColumn dataColumn in dt.Columns)
            {
                if (fieldPropertyDic.ContainsKey(dataColumn.ColumnName.ToLower()))
                {
                    dataColumn.ColumnName = fieldPropertyDic[dataColumn.ColumnName.ToLower()];
                }
                else//按照注册的列的大小写来
                {
                    if (fieldDic.ContainsKey(dataColumn.ColumnName.ToLower()))
                    {
                        dataColumn.ColumnName = fieldDic[dataColumn.ColumnName.ToLower()];
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// 组装表的in语句
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="tableNameField"></param>
        /// <returns></returns>
        private static string GetTableNames(DataTable dt, string tableNameField)
        {
            List<string> tableList = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                string tname = row[tableNameField].ToString().Trim();
                tname = "'" + tname + "'";
                if (!tableList.Contains(tname))
                {
                    tableList.Add(tname);
                }
            }
            string tables = string.Join(",", tableList.ToArray());
            return tables;
        }
        /// <summary>
        /// 获取属性名
        /// </summary>
        /// <param name="PropertyDt"></param>
        /// <param name="table"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private static string GetPropertyName(DataTable PropertyDt, string table, string field)
        {
            if (string.IsNullOrWhiteSpace(table)) return field;

            string fieldname = field;
            DataRow[] drs = PropertyDt.Select(string.Format("c_bname='{0}' and c_name='{1}'", table, field));
            if (drs.Length > 0)
            {
                if (drs[0]["propertyname"] != null && drs[0]["propertyname"] != DBNull.Value)
                {
                    fieldname = drs[0]["propertyname"].ToString();
                }
            }

            return fieldname;
        }
        
    }
}
