using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NG3.Data;
using NG3.Data.Service;
using SUP.CustomForm.DataEntity;
using Newtonsoft.Json;
using SUP.Common.DataEntity;

namespace SUP.CustomForm.DataAccess
{
    public abstract class HelpBase
    {
        public abstract HelpEntity GetHelpItem(string helpId);

        public IDataParameter[] BuildInputQuery(string helpid, string queryValue, string outJsonQuery, string leftLikeJsonQuery, ref string query)
        {
            HelpEntity helpitem = this.GetHelpItem(helpid);
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
            strb.Append(helpitem.CodeField);
            strb.Append(codeLikeStr);
            strb.Append(" or ");
            strb.Append(helpitem.NameField);
            strb.Append(nameLikeStr);
            strb.Append(" or ");
            strb.Append(functionName + "(");
            strb.Append(helpitem.NameField);
            strb.Append(") like '%");
            strb.Append(queryValue);
            strb.Append("%' )");


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
                paramList.Add(p[1]);
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
                    if (item.Value is string)
                    {
                        if (!string.IsNullOrEmpty(item.Value.ToString()))
                        {

                            strb.Append(" and ");

                            string columnName = string.Empty;
                            DbType dbtype = DbType.AnsiString;

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

                    if (item.Value is string)
                    {
                        if (!string.IsNullOrEmpty(item.Value.ToString()))
                        {

                            strb.Append(" and ");


                            string columnName = string.Empty;
                            DbType dbtype = DbType.AnsiString;

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

        #region 代码转名称;
        /// <summary>
        /// 转换某一个code的代码转名称
        /// </summary>
        /// <param name="helpId"></param>
        /// <param name="codeValue"></param>
        /// <returns></returns>
        public string GetName(string helpId, string codeValue)
        {
            return GetName(helpId, codeValue, "Single");
        }

        /// <summary>
        /// 转换某一个code的代码转名称，支持多选帮助列;
        /// </summary>
        /// <param name="helpId"></param>
        /// <param name="codeValue"></param>
        /// <param name="selectMode"></param>
        /// <returns></returns>
        public string GetName(string helpId, string codeValue, string selectMode)
        {
            string result = string.Empty;

            if (helpId == "") return string.Empty;
            HelpEntity item = this.GetHelpItem(helpId);
            if (item.FromSql != "1") return string.Empty;

            //project_table.phid，如果带表名则去掉表名
            string codeStr = item.CodeField;
            string nameStr = item.NameField;
            if (item.CodeField.IndexOf(".") > 0)
            {
                codeStr = item.CodeField.Substring(item.CodeField.IndexOf(".") + 1).Trim();
            }
            if (item.NameField.IndexOf(".") > 0)
            {
                nameStr = item.NameField.Substring(item.NameField.IndexOf(".") + 1).Trim();
            }

            //如果sql语句带：，说明是动态sql，动态替换很麻烦，暂时截断sql到表名
            var sqlStr = item.Sql;
            if (item.Sql.IndexOf(":") > 0)
            {
                sqlStr = item.DynamicSql;
            }

            DataTable helpdt = DbHelper.GetDataTable(sqlStr);
            if (helpdt == null || helpdt.Rows.Count == 0) return string.Empty;

            if (SelectMode.Multi.ToString().ToUpper() == selectMode.ToUpper())  //自定义复选帮助
            {
                DataRow[] helpdrs = helpdt.Select(codeStr + " in (" + codeValue + ")");

                if (helpdrs.Length > 0)
                {
                    StringBuilder buildstr = new StringBuilder();
                    int count = helpdrs.Length - 1;

                    for (int i = 0; i < helpdrs.Length; i++)
                    {
                        buildstr.Append(helpdrs[i][nameStr]);
                        if (i < count)
                        {
                            buildstr.Append(",");
                        }
                    }

                    result = buildstr.ToString();  //代码转成名称;
                }
            }
            else
            {
                DataRow[] helpdrs = helpdt.Select(codeStr + " = '" + codeValue + "'");

                if (helpdrs.Length > 0)
                {
                    result = helpdrs[0][nameStr].ToString();  //代码转成名称;
                }
            }
           
            return result;
        }

        /// <summary>
        /// DataTable代码转名称;
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="codeField"></param>
        /// <param name="helpId"></param>
        public void CodeToName(DataTable dt, string codeField, string helpId)
        {
            CodeToName(dt, codeField, helpId, "Single");
        }

        /// <summary>
        /// DataTable代码转名称，动态where条件;
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="codeField"></param>
        /// <param name="helpId"></param>
        /// <param name="selectMode"></param>
        public void CodeToName(DataTable dt, string codeField, string helpId, string selectMode)
        {
            if (helpId == "") return;
            HelpEntity item = this.GetHelpItem(helpId);

            if (item.FromSql != "1") return;
            if (string.IsNullOrEmpty(item.Sql)) return;
            if (item.Sql.Length < 15) return; //如select , from

            //project_table.phid，如果带表名则去掉表名
            string codeStr = item.CodeField;
            string nameStr = item.NameField;
            if (item.CodeField.IndexOf(".") > 0)
            {
                codeStr = item.CodeField.Substring(item.CodeField.IndexOf(".") + 1).Trim();
            }
            if (item.NameField.IndexOf(".") > 0)
            {
                nameStr = item.NameField.Substring(item.NameField.IndexOf(".") + 1).Trim();
            }

            //如果sql语句带：，说明是动态sql，动态替换很麻烦，暂时截断sql到表名
            var sqlStr = item.Sql;
            if (item.Sql.IndexOf(":") > 0)
            {
                sqlStr = item.DynamicSql;
            }

            DataTable helpdt = DbHelper.GetDataTable(sqlStr);

            if (helpdt == null || helpdt.Rows.Count == 0) return;

            foreach (DataRow dr in dt.Rows)
            {
                string codeValue = (dr[codeField] == null || dr[codeField] == DBNull.Value) ? string.Empty : dr[codeField].ToString();

                if (!string.IsNullOrWhiteSpace(codeValue))
                {
                    if (SelectMode.Multi.ToString().ToUpper() == selectMode.ToUpper())  //自定义复选帮助
                    {
                        DataRow[] helpdrs = helpdt.Select(codeStr + " in (" + codeValue + ")");

                        if (helpdrs.Length > 0)
                        {
                            StringBuilder buildstr = new StringBuilder();
                            int count = helpdrs.Length - 1;

                            for (int i = 0; i < helpdrs.Length; i++)
                            {
                                buildstr.Append(helpdrs[i][nameStr]);

                                if (i < count)
                                {
                                    buildstr.Append(",");
                                }
                            }

                            dr[codeField] = buildstr.ToString();  //代码转成名称;
                        }
                    }
                    else
                    {
                        DataRow[] helpdrs = helpdt.Select(codeStr + " = '" + codeValue + "'");

                        if (helpdrs.Length > 0)
                        {
                            dr[codeField] = helpdrs[0][nameStr];  //代码转成名称;                         
                        }
                    }
                }
            }
        }
        #endregion 代码转名称;

        /// <summary>
        /// 验证用户输入数据的合法性;
        /// </summary>
        /// <param name="helpId"></param>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public bool ValidateData(string helpId, string inputValue)
        {
            HelpEntity item = this.GetHelpItem(helpId);
            string CodeFlag = "0", FieldFlag = "0";
            StringBuilder strb = new StringBuilder();

            try
            {
                strb.Append("select COUNT(*) from ");
                strb.Append(item.TableName);
                strb.Append(" where ");
                strb.Append(item.CodeField + " ={0}");
                IDataParameter[] p = new NGDataParameter[] { new NGDataParameter(item.CodeField, inputValue) };
                CodeFlag = DbHelper.GetString(strb.ToString(), p);
            }
            catch (Exception e)
            {

            }

            try
            {
                strb.Clear();
                strb.Append("select COUNT(*) from ");
                strb.Append(item.TableName);
                strb.Append(" where ");
                strb.Append(item.NameField + " ={0}");

                IDataParameter[] p = new NGDataParameter[] { new NGDataParameter(item.NameField, inputValue) };
                FieldFlag = DbHelper.GetString(strb.ToString(), p);
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
    }
}
