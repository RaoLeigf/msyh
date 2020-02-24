using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NG3.Data;
using SUP.Common.Base;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Text.RegularExpressions;

namespace SUP.Common.Base
{
    public class DataConverterHelper
    {
        private static NGJsonConverter converter = new NGJsonConverter();
        private static NGJsonEntityConverter entityConverter = new NGJsonEntityConverter();

        public DataConverterHelper()
        {
 
        }

        /// <summary>
        /// 把json数据转换成DataTable
        /// </summary>
        /// <param name="json">json串</param>
        /// <param name="selectSql">取表结构的sql语句</param>
        /// <returns></returns>
        public static DataTable ToDataTable(string json, string selectSql)
        {
            return converter.ToDataTable(json, selectSql);
        }

        /// <summary>
        /// 把json数据转换成DataTable
        /// </summary>
        /// <param name="json">json串</param>
        /// <param name="selectSql">取表结构的sql语句</param>
        /// <returns></returns>
        public static DataTable ToDataTable(string json, string selectSql,Dictionary<string,string> fieldmapping)
        {
            if (fieldmapping != null)
            {
                converter.FieldMapping = fieldmapping;
                converter.NeedMapping = true;
            }
            return converter.ToDataTable(json, selectSql);
        }

        /// <summary>
        /// 把json数据转换成DataTable
        /// </summary>
        /// <param name="jo">json对象</param>
        /// <param name="selectSql">取表结构的sql语句</param>
        /// <returns></returns>
        public static DataTable ToDataTable(JObject jo, string selectSql)
        {
            return converter.ToDataTable(jo, selectSql);
        }


        /// <summary>
        /// 把DataTable转成json数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public static string ToJson(DataTable dt, int totalRecord)
        {
            return converter.ToJson(dt, totalRecord);
        }

        /// <summary>
        /// format为false返回原始数组
        /// format为true返回格式化json格式，提供给easyui的datagrid使用，格式：{total:100,rows:[{},{}]}
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public static string ToJsonData(DataTable dt, int totalRecord,bool format)
        {
            return converter.ToJsonData(dt, totalRecord, format);
        }
               

        /// <summary>
        /// json字符串转成实体类信息集合
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="json">客户端传入的json串</param>
        /// <returns></returns>
        public static EntityInfo<T> JsonToEntity<T>(string json)
        {
            return entityConverter.JsonToEntity<T>(json);
        }

        /// <summary>
        /// jobject转成实体类信息集合
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="jo">jobect对象</param>
        /// <returns></returns>
        public static EntityInfo<T> JsonToEntity<T>(JObject jo)
        {
            return entityConverter.JsonToEntity<T>(jo);
        }

        public static string EntityListToJson<T>(IList<T> entityList, long totalRecord)
        {
            return entityConverter.EntityListToJson<T>(entityList,totalRecord);
        }

        public static string EntityListToJson(System.Collections.IList entityList, long totalRecord)
        {
            return entityConverter.EntityListToJson(entityList, totalRecord);
        }

        public static string EntityListToJson<T>(IList<T> entityList, string status, string msg)
        {
            return entityConverter.EntityListToJson(entityList, status,msg);
        }

        public static string EntityToJson<T>(T entity)
        {
            return entityConverter.EntityToJson<T>(entity);
        }

        public static string ResponseResultToJson(object result)
        {
           JObject jo = entityConverter.ResponseResultToJson(result, false);
            //return JsonConvert.SerializeObject(jo,new LongJsonConverter());// long兼容int32,LongJsonConverter把int32也变成string,枚举和金额会变成string前端会出问题 
            return JsonConvert.SerializeObject(jo);

            //下面的代码ngdate会多出时分秒6个0

            //IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();//解决日期问题,原来调用的方法entityConverter.ResponseResultToJson废弃
            //timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            //return JsonConvert.SerializeObject(result, Formatting.Indented, timeConverter);
        }

        public static string SerializeObject(object obj)
        {
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();//解决日期问题
            timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(obj, new LongJsonConverter(),timeConverter);
        }

        /// <summary>
        /// 根据json串构建查询条件语句
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string BuildQuery(string json)
        {
            if (string.IsNullOrEmpty(json)) return string.Empty;

            Dictionary<string, object> d = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            int i = 0;
            StringBuilder strb = new StringBuilder();
            foreach (KeyValuePair<string, object> item in d)
            {                

                if (item.Value is string)
                {
                    if (!string.IsNullOrEmpty(item.Value.ToString()))
                    {
                        if (i > 0)
                        {
                            strb.Append(" and ");
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
                            strb.Append(item.Key + " like '%" + item.Value + "%'");
                        }

                        i++;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.Value.ToString()))
                    {
                        if (i > 0)
                        {
                            strb.Append(" and ");
                        }

                        if (item.Key.ToString().EndsWith("*ngLow"))//下限
                        {
                            string[] arr = item.Key.Split('*');

                            if (arr[1] == "num")//数字字段
                            {
                                strb.Append(arr[0] + " >= " + item.Value);
                            }
                            else//日期
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
                            strb.Append(item.Key + "=" + item.Value);
                        }

                        i++;
                    }
                }

               
            }

            return strb.ToString();
            
        }


        //注册内嵌查询条件生成
        public static IDataParameter[] BuildQueryWithParam(string json, ref string where)
        {
            if (!string.IsNullOrEmpty(json))
            {
                Dictionary<string, object> d = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                return BuildQueryWithParamForCmd(d, ref where);
            }
            else
            {
                StringBuilder strb = new StringBuilder();
                List<NGDataParameter> paramList = new List<NGDataParameter>();
                where = strb.ToString();

                return paramList.ToArray();
            }
        }

        public static IDataParameter[] BuildQueryWithParam(string json, string outFilterJson, ref string where)
        {
            return BuildQueryWithParam(json, outFilterJson, string.Empty, ref where);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="outFilterJson">通用帮助的外部查询条件</param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static IDataParameter[] BuildQueryWithParam(string json, string outFilterJson,string leftLikeJsonQuery, ref string where)
        {
            int i = 0;
            int paramCout = 0;
            StringBuilder strb = new StringBuilder();
            List<NGDataParameter> paramList = new List<NGDataParameter>();

            if (!string.IsNullOrEmpty(json))
            {
                Dictionary<string, object> d = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                foreach (KeyValuePair<string, object> item in d)
                {
                    //if (item.Value is string)
                    if (item.Value != null)
                    {
                        if (!string.IsNullOrEmpty(item.Value.ToString()))
                        {
                            if (i > 0)
                            {
                                strb.Append(" and ");
                            }

                            string columnName = string.Empty;
                            DbType dbtype = DbType.AnsiString;
                            if (item.Value is Int64)
                            {
                                dbtype = DbType.Int64;
                            }
                            if (item.Key.ToString().IndexOf("*ngLow") > 0)//下限
                            {
                                string[] arr = item.Key.Split('*');

                                if (arr[1] == "num")//数字字段
                                {
                                    strb.Append(arr[0] + " >= " + item.Value);
                                }
                                else//日期、字符字段
                                {
                                    strb.Append(arr[0] + " >= '" + item.Value + "'");
                                }
                            }
                            else if (item.Key.ToString().IndexOf("*ngUP") > 0)//上限
                            {
                                string[] arr = item.Key.Split('*');

                                if (arr[1] == "num")//数字字段
                                {
                                    strb.Append(arr[0] + " <= " + item.Value);
                                }
                                else//日期、字符字段
                                {
                                    strb.Append(arr[0] + " <= '" + item.Value + "'");
                                }
                            }
                            else
                            {
                                columnName = item.Key;
                                string colname = columnName;
                                //if (columnName.IndexOf(".") > 0)//字段带点号，无法参数化
                                //{
                                //    strb.Append(item.Key + " like '%" + item.Value + "%'");
                                //}
                                //else
                                //{
                                //strb.Append(item.Key + " like '%' +{" + paramCout.ToString() + "}+ '%'");
                                if (columnName.IndexOf("*LLike") > 0)//匹配like value%                                {
                                {
                                    string[] arr = columnName.Split('*');
                                    //strb.Append(arr[0] + " like '{" + paramCout.ToString() + "}%'");
                                    strb.Append(arr[0] + " like {" + paramCout.ToString() + "}");
                                }
                                else if (columnName.IndexOf("*RLike") > 0) //匹配like %value
                                {
                                    string[] arr = columnName.Split('*');
                                    //strb.Append(arr[0] + " like '%{" + paramCout.ToString() + "}' ");
                                    strb.Append(arr[0] + " like {" + paramCout.ToString() + "} ");
                                }
                                else if (columnName.IndexOf("*gt") > 0)
                                {
                                    dbtype = BuildSql(strb, item.Key, item.Value, ">", paramCout, ref colname);
                                }
                                else if (columnName.IndexOf("*ge") > 0)
                                {
                                    dbtype = BuildSql(strb, item.Key, item.Value, ">=", paramCout, ref colname);
                                }
                                else if (columnName.IndexOf("*lt") > 0)
                                {
                                    dbtype = BuildSql(strb, item.Key, item.Value, "<", paramCout, ref colname);
                                }
                                else if (columnName.IndexOf("*le") > 0)
                                {
                                    dbtype = BuildSql(strb, item.Key, item.Value, "<=", paramCout, ref colname);
                                }
                                else if (columnName.IndexOf("*eq") > 0)
                                {
                                    dbtype = BuildSql(strb, item.Key, item.Value, "=", paramCout, ref colname);
                                }
                                else
                                {
                                    string[] arr = columnName.Split('*');
                                    if (arr.Length > 2)//  column*type*compare
                                    {
                                        switch (arr[1])
                                        {
                                            case "num":
                                                dbtype = DbType.Int32;
                                                break;
                                            case "date":
                                                dbtype = DbType.DateTime;
                                                break;
                                            default:
                                                dbtype = DbType.AnsiString;
                                                break;
                                        }
                                    }
                                    strb.Append(arr[0] + " like {" + paramCout.ToString() + "}");
                                }

                                //判断是否带表名，如果带表名，则去掉，表名不能参数化
                                string[] cols = columnName.Split('.');

                                if (cols.Length > 1)
                                {
                                    colname = cols[1];
                                }

                                string pname = "P" + paramCout.ToString();
                                NGDataParameter p = new NGDataParameter(pname, dbtype);//new NGDataParameter(colname, dbtype);
                                //p.Value = item.Value;
                                if (columnName.IndexOf("*LLike") > 0)//匹配like value%                                {
                                {
                                    p.Value = item.Value + "%";
                                }
                                else if (columnName.IndexOf("*RLike") > 0) //匹配like %value
                                {
                                    p.Value = "%" + item.Value;
                                }
                                else if (columnName.IndexOf("*eq") > 0) //匹配 value 
                                {
                                    p.Value = item.Value;
                                }
                                else
                                {
                                    if (dbtype == DbType.AnsiString)
                                    {
                                        p.Value = "%" + item.Value + "%";                                        
                                    }
                                    else
                                    {
                                        p.Value = item.Value;//数字，日期
                                    }
                                }
                                paramList.Add(p);

                                paramCout++;
                                //}
                            }

                            i++;
                        }
                    }
                }
            }

            //
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
                            if (i > 0)
                            {
                                strb.Append(" and ");
                            }

                            string columnName = string.Empty;
                            DbType dbtype = DbType.AnsiString;
                            if (item.Value is Int64)
                            {
                                dbtype = DbType.Int64;
                            }
                            
                            columnName = item.Key;

                            //if (columnName.IndexOf(".") > 0)//字段带点号，无法参数化
                            //{
                            //    strb.Append(item.Key + " like '" + item.Value + "%'");
                            //}
                            //else
                            //{
                                    
                                //匹配like value%
                                //strb.Append(item.Key + " like '' +{" + paramCout.ToString() + "}+ '%'");
                                strb.Append(item.Key + " like +{" + paramCout.ToString() + "}");

                                //判断是否带表名，如果带表名，则去掉，表名不能参数化
                                //string[] cols = columnName.Split('.');
                                //if (cols.Length > 1)
                                //{
                                //    columnName = cols[1];
                                //}

                                string pname = "P" + paramCout.ToString();
                                NGDataParameter p = new NGDataParameter(pname, dbtype);//NGDataParameter(columnName, dbtype);
                                p.Value = item.Value + "%";
                                paramList.Add(p);
                                paramCout++;
                            //}
                            i++;
                        }
                    }
                }
            }


            if (!string.IsNullOrEmpty(outFilterJson))//通用帮助外部条件处理
            {
                Dictionary<string, object> outFilter = JsonConvert.DeserializeObject<Dictionary<string, object>>(outFilterJson);//通用帮助
                foreach (KeyValuePair<string, object> item in outFilter)
                {
                    //if (item.Value is string)
                    if (item.Value != null)
                    {
                        if (!string.IsNullOrEmpty(item.Value.ToString()))
                        {
                            if (i > 0)
                            {
                                strb.Append(" and ");
                            }

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
                                else//日期或者字符
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
                                else//日期或者字符
                                {
                                    strb.Append(arr[0] + " <= '" + item.Value + "'");
                                }
                            }
                            else
                            {
                                columnName = item.Key;
                                //if (columnName.IndexOf(".") > 0)//字段带点号，无法参数化
                                //{
                                //    strb.Append(item.Key + "='"+ item.Value +"'");
                                //}
                                //else
                                //{
                                strb.Append(item.Key + "={" + paramCout.ToString() + "}");//外部条件用"="

                                string pname = "P" + paramCout.ToString();
                                NGDataParameter p = new NGDataParameter(pname, dbtype);
                                p.Value = item.Value;
                                paramList.Add(p);

                                paramCout++;
                                //}
                            }
                            i++;
                        }
                    }
                }
            }
        

            where = strb.ToString();
            return paramList.ToArray();
        }

        /// <summary>
        /// 构建查询条件，查询参数
        /// </summary>
        /// <param name="codeField">代码字段</param>
        /// <param name="nameField">名称字段</param>
        /// <param name="pyField">拼音首字母字段</param>
        /// <param name="searchkey">搜索条件</param>
        /// <param name="outFilterJson">通用帮助的外部查询条件</param>
        /// <param name="leftLikeJsonQuery">左匹配条件</param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static IDataParameter[] BuildQueryWithParam(string codeField,string nameField,string pyField,string [] searchkey, string outFilterJson, string leftLikeJsonQuery, ref string where)
        {
            int i = 0;
            int paramCount = 0;
            StringBuilder strb = new StringBuilder();
            List<NGDataParameter> paramList = new List<NGDataParameter>();

            
            for (int j = 0; j < searchkey.Length; j++)
            {
                if (j == 0)
                {
                    strb.Append(" ( ");//加(
                }
                else
                {
                    strb.Append(" and ( ");//加空格
                }
                string queryValue = searchkey[j];
                //无奈
                NG3.Data.Service.DbHelper.Open();
                NG3.Data.DbVendor vender = NG3.Data.Service.DbHelper.Vendor;
                NG3.Data.Service.DbHelper.Close();

                string codeLikeStr = string.Empty;
                string nameLikeStr = string.Empty;

                //在结果中搜索，新的oracle驱动参数化有问题（并非所有变量都已绑定）
                if (codeField.IndexOf(".") > 0 || (vender == DbVendor.Oracle && searchkey.Length >1))
                {
                    codeLikeStr = " like '%" + queryValue + "%'";
                }
                else
                {                    
                    //codeLikeStr = " like '%'+{" + paramCount.ToString() + "}+'%'";
                    codeLikeStr = " like {" + paramCount.ToString() + "}";
                }

                //在结果中搜索，新的oracle驱动参数化有问题（并非所有变量都已绑定）
                if (nameField.IndexOf(".") > 0 || (vender == DbVendor.Oracle && searchkey.Length > 1))
                {
                    nameLikeStr = " like '%" + queryValue + "%'";
                }
                else
                {                    
                    //nameLikeStr = " like '%'+{" + paramCount.ToString() + "}+'%'";  
                    nameLikeStr = " like {" + paramCount.ToString() + "}";
                }

                //获取汉字拼音首字母函数
                string functionName = "dbo.fun_getPY";
                if (vender == NG3.Data.DbVendor.Oracle||vender == NG3.Data.DbVendor.MySql)
                {
                    functionName = "fun_getPY";
                }

                //if (!string.IsNullOrEmpty(strb.ToString()))
                //{
                //    strb.Append(") and ( ");
                //}


                strb.Append(codeField);
                strb.Append(codeLikeStr);
                strb.Append(" or ");
                strb.Append(nameField);
                strb.Append(nameLikeStr);

                if (IsAlphabet(queryValue))
                {
                    strb.Append(" or ");
                    if (string.IsNullOrWhiteSpace(pyField))
                    {
                        strb.Append(functionName + "(");
                        strb.Append(nameField);
                        strb.Append(") ");
                        strb.Append(" like {" + paramCount.ToString() + "}");
                    }
                    else
                    {
                        strb.Append(pyField + " like '%" + queryValue + "%' ");//拼音首字母字段（可能带表名），智能搜索优化
                    }
                }              
                strb.Append(")");               

                string pname = "P" + paramCount.ToString();
                NGDataParameter p = new NGDataParameter(pname,NGDbType.VarChar);
                //p.Value = queryValue;
                p.Value = "%" + queryValue + "%";
                paramList.Add(p);
                paramCount++;
            }



            StringBuilder strb2 = new StringBuilder();
            //
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
                            //if (i > 0)
                            //if(strb.Length > 0)
                            //{
                            //    strb2.Append(" and ");
                            //}

                            string columnName = string.Empty;
                            DbType dbtype = DbType.AnsiString;
                            if (item.Value is Int64)
                            {
                                dbtype = DbType.Int64;
                            }

                            columnName = item.Key;

                            if (columnName.IndexOf(".") > 0)//字段带点号，无法参数化
                            {
                                strb2.Append(item.Key + " like '" + item.Value + "%'");
                            }
                            else
                            {

                                //匹配like value%
                                //strb.Append(item.Key + " like '' +{" + paramCount.ToString() + "}+ '%'");
                                strb2.Append(item.Key + " like '' +{" + paramCount.ToString() + "}");
                                //判断是否带表名，如果带表名，则去掉，表名不能参数化
                                string[] cols = columnName.Split('.');

                                //if (cols.Length > 1)
                                //{
                                //    columnName = cols[1];
                                //}
                                //NGDataParameter p = new NGDataParameter(columnName, dbtype);
                                columnName = "P" + paramCount.ToString();
                                NGDataParameter p = new NGDataParameter(columnName, dbtype);
                                p.Value = item.Value + "%";
                                //p.Value = item.Value;
                                paramList.Add(p);

                                paramCount++;
                            }

                            i++;
                        }
                    }
                }
            }

            StringBuilder strb3 = new StringBuilder();
            if (!string.IsNullOrEmpty(outFilterJson))//通用帮助外部条件处理
            {
                Dictionary<string, object> outFilter = JsonConvert.DeserializeObject<Dictionary<string, object>>(outFilterJson);//通用帮助
                foreach (KeyValuePair<string, object> item in outFilter)
                {
                    //if (item.Value is string)
                    if (item.Value != null)
                    {
                        if (!string.IsNullOrEmpty(item.Value.ToString()))
                        {
                            if (strb3.Length > 0)
                            {
                                strb3.Append(" and ");
                            }

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
                                    strb3.Append(arr[0] + " >= " + item.Value);
                                }
                                else//日期或者字符
                                {
                                    strb3.Append(arr[0] + " >= '" + item.Value + "'");
                                }
                            }
                            else if (item.Key.ToString().EndsWith("*ngUP"))//上限
                            {
                                string[] arr = item.Key.Split('*');

                                if (arr[1] == "num")//数字字段
                                {
                                    strb3.Append(arr[0] + " <= " + item.Value);
                                }
                                else//日期或者字符
                                {
                                    strb3.Append(arr[0] + " <= '" + item.Value + "'");
                                }
                            }
                            else
                            {
                                columnName = item.Key;
                                if (columnName.IndexOf(".") > 0 || NG3.Data.Service.DbHelper.Vendor == DbVendor.Oracle)//字段带点号，无法参数化，oracle也不参数化
                                {
                                    strb3.Append(item.Key + "='" + item.Value + "'");
                                }
                                else
                                {
                                    strb3.Append(item.Key + "={" + paramCount.ToString() + "}");//外部条件用"="

                                    string pname = "P" + paramCount.ToString();
                                    NGDataParameter p = new NGDataParameter(pname, dbtype);
                                    //NGDataParameter p = new NGDataParameter(columnName, dbtype);
                                    p.Value = item.Value;
                                    paramList.Add(p);

                                    paramCount++;
                                }
                            }
                            i++;
                        }
                    }
                }

            }

            //where = strb.ToString();
            if (strb.Length > 0 && strb2.Length == 0 && strb3.Length == 0)
            {
                where = strb.ToString();
            }
            else if (strb.Length == 0 && strb2.Length > 0 && strb3.Length == 0)
            {
                where = strb2.ToString();
            }
            else if (strb.Length == 0 && strb2.Length == 0 && strb3.Length > 0)
            {
                where = strb3.ToString();
            }
            else if (strb.Length > 0 && strb2.Length > 0 && strb3.Length == 0)
            {
                where = "(" + strb.ToString() + ") and (" + strb2.ToString() + ")";
            }
            else if (strb.Length > 0 && strb2.Length == 0 && strb3.Length > 0)
            {
                where = "(" + strb.ToString() + ") and (" + strb3.ToString() + ")";
            }
            else if (strb.Length == 0 && strb2.Length > 0 && strb3.Length > 0)
            {
                where = "(" + strb2.ToString() + ") and (" + strb3.ToString() + ")";
            }
            else if (strb.Length > 0 && strb2.Length > 0 && strb3.Length > 0)
            {
                where = "(" + strb.ToString() + ") and (" + strb2.ToString() + ") and (" + strb3.ToString() + ")";
            }
           
            return paramList.ToArray();
        }

        //判断查询是否是全字母
        private static bool IsAlphabet(string queryValue)
        {
            string pattern = @"^[A-Za-z]+$";
            Regex regex = new Regex(pattern);
            bool flg = regex.IsMatch(queryValue);
            return flg;
        }

        /// <summary>
        ///eq              ＝         
        ///gt              ＞
        ///ge              ＞＝
        ///lt              ＜
        ///le              ＜＝
        ///between     BETWEEN，闭区间xy中的任意值
        ///like            LIKE
        ///LLike          LIKE    '%asdf'
        ///RLike          LIKE    'asdf%'
        ///in              in
        /// </summary>
        /// <param name="d"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static IDataParameter[] BuildQueryWithParam(Dictionary<string, object> d, ref string where)
        {
            int i = 0;
            int paramCout = 0;
            StringBuilder strb = new StringBuilder();
            List<NGDataParameter> paramList = new List<NGDataParameter>();
                       
            //Dictionary<string, object> d = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            foreach (KeyValuePair<string, object> item in d)
            {
                //if (item.Value is string)
                if (item.Value != null)
                {
                    if (!string.IsNullOrEmpty(item.Value.ToString()))
                    {
                        if (i > 0)
                        {
                            strb.Append(" and ");
                        }

                        string columnName = string.Empty;
                        DbType dbtype = DbType.AnsiString;
                        if (item.Value is Int64)
                        {
                            dbtype = DbType.Int64;
                        }

                        if (item.Key.ToString().EndsWith("*ngLow"))//下限,不需要参数化
                        {
                            string[] arr = item.Key.Split('*');
                            if (arr[1] == "num")//数字字段
                            {
                                strb.Append(arr[0] + " >= " + item.Value);
                            }
                            else//日期、字符字段
                            {
                                strb.Append(arr[0] + " >= '" + item.Value + "'");
                            }
                        }
                        else if (item.Key.EndsWith("*ngUP"))//上限，不需要参数化
                        {
                            string[] arr = item.Key.Split('*');

                            if (arr[1] == "num")//数字字段
                            {
                                strb.Append(arr[0] + " <= " + item.Value);
                            }
                            else//日期、字符字段
                            {
                                strb.Append(arr[0] + " <= '" + item.Value + "'");
                            }
                        }
                        else if (columnName.EndsWith("*in"))//无法参数化
                        {
                            strb.Append(item.Key + " in (" + item.Value + ")");
                        }
                        else//参数化
                        {
                            columnName = item.Key;
                            string colname = columnName;

                            //if (columnName.IndexOf(".") > 0)//字段带点号，无法参数化
                            //{
                            //    if (columnName.IndexOf("*") > 0)
                            //    {
                            //        string[] arr = columnName.Split('*');
                            //        strb.Append(arr[0] + " like '%" + item.Value + "%'");
                            //    }
                            //    else
                            //    {
                            //        strb.Append(item.Key + " like '%" + item.Value + "%'");
                            //    }
                            //}
                            //else
                            //{
                                if (columnName.EndsWith("*LLike"))//匹配like value%                                {
                                {
                                    string[] arr = columnName.Split('*');
                                    //strb.Append(arr[0] + " like '{" + paramCout.ToString() + "}%'");
                                    strb.Append(arr[0] + " like {" + paramCout.ToString() + "}"); 
                                }
                                else if (columnName.EndsWith("*RLike")) //匹配like %value
                                {
                                    string[] arr = columnName.Split('*');
                                    //strb.Append(arr[0] + " like '%{" + paramCout.ToString() + "}' ");
                                    strb.Append(arr[0] + " like {" + paramCout.ToString() + "} ");
                                }
                                else if (columnName.EndsWith("*gt"))
                                {
                                    dbtype = BuildSql(strb, item.Key, item.Value, ">", paramCout, ref colname);
                                }
                                else if (columnName.EndsWith("*ge"))
                                {
                                    dbtype = BuildSql(strb, item.Key, item.Value, ">=", paramCout, ref colname);
                                }
                                else if (columnName.EndsWith("*lt"))
                                {
                                    dbtype = BuildSql(strb, item.Key, item.Value, "<", paramCout, ref colname);
                                }
                                else if (columnName.EndsWith("*le"))
                                {
                                    dbtype = BuildSql(strb, item.Key, item.Value, "<=", paramCout, ref colname);
                                }
                                else if (columnName.EndsWith("*eq"))
                                {
                                    dbtype = BuildSql(strb, item.Key, item.Value, "=", paramCout, ref colname);
                                }
                                else
                                {
                                    string[] arr = columnName.Split('*');
                                    if (arr.Length > 2)//  column*type*compare
                                    {
                                        switch (arr[1])
                                        {
                                            case "num":
                                                dbtype = DbType.Int32;
                                                break;
                                            case "date":
                                                dbtype = DbType.DateTime;
                                                break;
                                            default:
                                                dbtype = DbType.AnsiString;
                                                break;
                                        }
                                    }
                                    strb.Append(arr[0] + " like {" + paramCout.ToString() + "}");
                                }

                                //判断是否带表名，如果带表名，则去掉，表名不能参数化
                                string[] cols = columnName.Split('.');

                                if (cols.Length > 1)
                                {
                                    colname = cols[1];
                                }

                                string pname = "P" + paramCout.ToString();
                                NGDataParameter p = new NGDataParameter(pname, dbtype);//new NGDataParameter(colname, dbtype);
                                //p.Value = item.Value;
                                if (columnName.EndsWith("*LLike"))//匹配like value%                                {
                                {
                                    p.Value = item.Value + "%";
                                }
                                else if (columnName.EndsWith("*RLike")) //匹配like %value
                                {
                                    p.Value = "%" + item.Value;
                                }
                                else if (columnName.EndsWith("*eq")) //匹配 value 
                                {
                                    p.Value = item.Value;
                                }
                                else
                                {
                                    p.Value = "%" + item.Value + "%"; ;
                                }
                                paramList.Add(p);

                                paramCout++;
                            //}
                        }

                        i++;
                    }
                }
            }//foreach

            where = strb.ToString();
            return paramList.ToArray();
        }

        private static DbType BuildSql(StringBuilder strb, string key, object value, string expression,int count,ref string colname)
        {
            DbType dbtype = DbType.AnsiString;
            string[] arr = key.Split('*');

            if (arr.Length > 2) //  column*type*compare
            {
                switch (arr[1])
                {
                    case "num":
                        dbtype = DbType.Int32;
                        strb.Append(arr[0] + expression + "{" + count + "}");
                        return dbtype;
                    case "date":
                        dbtype = DbType.DateTime;
                        break;
                    default:
                        dbtype = DbType.AnsiString;
                        break;
                }
            }
            else
            {

                switch (value.GetType().FullName)
                {
                    case "System.Int16":
                        dbtype = DbType.Int16;
                        break;
                    case "System.Int32":
                        dbtype = DbType.Int32;
                        break;
                    case "System.Int64":
                        dbtype = DbType.Int64;
                        break;
                    case "System.Single":
                        dbtype = DbType.Single;
                        break;
                    case "System.UInt16":
                        dbtype = DbType.UInt16;
                        break;
                    case "System.UInt32":
                        dbtype = DbType.UInt32;
                        break;
                    case "System.UInt64":
                        dbtype = DbType.UInt64;
                        break;
                    case "System.Double":
                        dbtype = DbType.Decimal;
                        break;
                    case "System.Decimal":
                        dbtype = DbType.Decimal;
                        break;
                    case "System.Date":
                        dbtype = DbType.Date;
                        break;
                    case "System.DateTime":
                        dbtype = DbType.DateTime;
                        break;
                }
            }

            colname = arr[0];
            //if (value.GetType().FullName == "System.String")
            if (dbtype == DbType.AnsiString)
            {
                strb.Append(arr[0] + expression + "'{" + count + "}'");
            }   
            else
            {
                strb.Append(arr[0] + expression + "{" + count + "}");
            }

            return dbtype;
        }

        /// <summary>
        /// sql语句NGDataParam
        /// </summary>
        /// <param name="d"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static IDataParameter[] BuildQueryWithParamForCmd(Dictionary<string, object> d, ref string where)
        {
            int i = 0;
            int paramCout = 0;
            StringBuilder strb = new StringBuilder();
            List<NGDataParameter> paramList = new List<NGDataParameter>();

            foreach (KeyValuePair<string, object> item in d)
            {
                if (!string.IsNullOrEmpty(item.Value.ToString()))
                {
                    if (i > 0)
                    {
                        strb.Append(" and ");
                    }

                    string columnName = string.Empty;
                    DbType dbtype = DbType.AnsiString;
                    columnName = item.Key;
                    string colname = columnName;
                    string[] arr = columnName.Split('*');
                    string[] cols = arr[0].Split('.');
                    //判断是否带表名，如果带表名，则去掉，表名不能参数化
                    if (cols.Length > 1)
                    {
                        colname = cols[1];
                    }
                    string expression = "";
                    if (columnName.EndsWith("*ngLow") || columnName.IndexOf("*ge") > 0)//下限,不需要参数化
                    {
                        expression = ">=";
                    }
                    else if (columnName.EndsWith("*ngUP") || columnName.IndexOf("*le") > 0)//上限，不需要参数化
                    {
                        expression = "<=";
                    }
                    else if (columnName.IndexOf("*lt") > 0)
                    {
                        expression = "<";
                    }
                    else if (columnName.IndexOf("*gt") > 0)
                    {
                        expression = ">";
                    }
                    else if (columnName.IndexOf("*eq") > 0)
                    {
                        expression = "=";
                    }
                    else if (columnName.IndexOf("*NotEq") > 0)
                    {
                        expression = "!=";
                    }
                    if (!string.IsNullOrEmpty(expression))
                    {
                        dbtype = GetDataType(arr, item.Value);
                        strb.Append(arr[0] + expression + "{" + paramCout.ToString() + "}");
                        string pname = "P" + paramCout.ToString();
                        NGDataParameter p = new NGDataParameter(pname, dbtype);
                        p.Value = item.Value;
                        paramList.Add(p);
                        paramCout++;


                    }
                    else if (columnName.EndsWith("*in"))//无法参数化
                    {
                        strb.Append(item.Key + " in (" + item.Value + ")");
                    }
                    else if (columnName.EndsWith("*NotIn"))
                    {
                        strb.Append(item.Key + "not in (" + item.Value + ")");
                    }
                    else
                    {
                        if (columnName.EndsWith("*LLike"))//匹配like value%                                {
                        {
                            strb.Append(arr[0] + " like {" + paramCout.ToString() + "}");
                        }
                        else if (columnName.EndsWith("*RLike")) //匹配like %value
                        {
                            strb.Append(arr[0] + " like {" + paramCout.ToString() + "} ");
                        }
                        else
                        {
                            strb.Append(arr[0] + " like {" + paramCout.ToString() + "} ");
                        }
                        string pname = "P" + paramCout.ToString();
                        NGDataParameter p = new NGDataParameter(pname, dbtype);
                        if (columnName.EndsWith("*LLike"))//匹配like value%                                {
                        {
                            p.Value = item.Value + "%";
                        }
                        else if (columnName.EndsWith("*RLike")) //匹配like %value
                        {
                            p.Value = "%" + item.Value;
                        }
                        else
                        {
                            p.Value = "%" + item.Value + "%"; ;
                        }
                        paramList.Add(p);
                        paramCout++;
                    }
                    i++;
                }
            }
            where = strb.ToString();
            return paramList.ToArray();
        }

        /// <summary>
        /// 获取DbType
        /// </summary>
        /// <param name="datatype"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static DbType GetDataType(string[] arr, object value)
        {
            DbType dbtype = DbType.AnsiString;
            if (arr.Length > 2)
            {
                switch (arr[1])
                {
                    case "num":
                        dbtype = DbType.Decimal;
                        break;
                    case "date":
                        dbtype = DbType.DateTime;
                        break;
                    default:
                        dbtype = DbType.AnsiString;
                        break;

                }
            }
            else
            {
                switch (value.GetType().FullName)
                {
                    case "System.Int16":
                        dbtype = DbType.Int16;
                        break;
                    case "System.Int32":
                        dbtype = DbType.Int32;
                        break;
                    case "System.Int64":
                        dbtype = DbType.Int64;
                        break;
                    case "System.Single":
                        dbtype = DbType.Single;
                        break;
                    case "System.UInt16":
                        dbtype = DbType.UInt16;
                        break;
                    case "System.UInt32":
                        dbtype = DbType.UInt32;
                        break;
                    case "System.UInt64":
                        dbtype = DbType.UInt64;
                        break;
                    case "System.Double":
                        dbtype = DbType.Decimal;
                        break;
                    case "System.Decimal":
                        dbtype = DbType.Decimal;
                        break;
                    case "System.Date":
                        dbtype = DbType.Date;
                        break;
                    case "System.DateTime":
                        dbtype = DbType.DateTime;
                        break;
                }
            }
            return dbtype;
        }


        /// <summary>
        /// 系统定义字段转调整后的字段[暂未考虑变更记录]
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">系统定义字段名</param>
        /// <returns></returns>
        public static string FieldToAdjustedField(string tableName, string fieldName)
        {
            return fieldName;
        }

        /// <summary>
        /// 字段名转属性名[暂未考虑变更记录]
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public static string FieldToProperty(string tableName, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return fieldName;//ORM的插件模式，直接注册属性，表名为空，直接返回属性
            }

            string sql = "select propertyname from fg_columns where c_bname={0} and c_name={1} ";

            IDataParameter[] p = new NGDataParameter[2];
            p[0] = new NGDataParameter("c_bname", DbType.AnsiString);
            p[0].Value = tableName;
            p[1] = new NGDataParameter("c_name", DbType.AnsiString);
            p[1].Value = fieldName;

            object obj = NG3.Data.Service.DbHelper.ExecuteScalar(sql, p);

            string str = fieldName;//string.Empty;默认是fieldName，如果找不到就返回，插件时字段注册成属性
            if (obj != null && obj != DBNull.Value)
            {
                str = obj.ToString();
            }

            return str;
        }

        /// <summary>
        /// 字段名转属性名[暂未考虑变更记录]
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static Dictionary<string, string> FieldToProperty(string tableName)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            string sql = "select c_bname,c_name,propertyname from fg_columns where c_bname={0} ";

            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("c_bname", DbType.AnsiString);
            p[0].Value = tableName;

            DataTable dt = NG3.Data.Service.DbHelper.GetDataTable(sql, p);

            foreach (DataRow dr in dt.Rows)
            {
                dic.Add(dr["c_name"].ToString(), dr["propertyname"].ToString());
            }

            return dic;
        }

        /// <summary>
        /// 字段名转属性名[暂未考虑变更记录]
        /// </summary>
        /// <param name="list">表名.字段名</param>
        /// <returns></returns>
        public static Dictionary<string, string> FieldToProperty(IList<string> list)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (string s in list)
            {
                string[] ss = s.Split('.');
                if (ss == null || ss.Length != 2) continue;
                if (string.IsNullOrEmpty(ss[0]) || string.IsNullOrEmpty(ss[1])) continue;

                string sql = "select propertyname from fg_columns where c_bname={0} and c_name={1} ";

                IDataParameter[] p = new NGDataParameter[2];
                p[0] = new NGDataParameter("c_bname", DbType.AnsiString);
                p[0].Value = ss[0];
                p[1] = new NGDataParameter("c_name", DbType.AnsiString);
                p[1].Value = ss[1];

                object obj = NG3.Data.Service.DbHelper.ExecuteScalar(sql, p);

                if (obj != null && obj != DBNull.Value)
                {
                    dic.Add(s, obj.ToString());
                }
            }

            return dic;
        }

        /// <summary>
        /// 属性名转字段名[暂未考虑变更记录]
        /// </summary>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="className">类名</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static string PropertyToField(string nameSpace, string className, string propertyName)
        {
            string sReturn = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append("select fg_table.c_name tablename,fg_table.modelname,fg_table.namespaceprefix,fg_table.namespacesuffix,");
            sb.Append("fg_columns.c_name fieldname,propertyname from fg_columns ");
            sb.Append("left join fg_table on fg_columns.c_bname=fg_table.c_name ");
            sb.Append("where modelname={0} and propertyname={1} ");

            IDataParameter[] p = new NGDataParameter[2];
            p[0] = new NGDataParameter("modelname", DbType.AnsiString);
            p[0].Value = className;
            p[1] = new NGDataParameter("propertyname", DbType.AnsiString);
            p[1].Value = propertyName;

            DataTable dt = NG3.Data.Service.DbHelper.GetDataTable(sb.ToString(), p);

            foreach (DataRow dr in dt.Rows)
            {
                string namespaceprefix = Convert.ToString(dr["namespaceprefix"]);
                string namespacesuffix = Convert.ToString(dr["namespacesuffix"]);
                string sNameSpace = namespaceprefix + ".Model";
                if (!string.IsNullOrEmpty(namespacesuffix))
                {
                    sNameSpace += "." + namespacesuffix;
                }

                if (sNameSpace == nameSpace)
                {
                    sReturn = dr["fieldname"].ToString();
                    break;
                }
            }

            return sReturn;
        }

        /// <summary>
        /// 属性名转字段名[暂未考虑变更记录]
        /// </summary>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="className">类名</param>
        /// <returns></returns>
        public static Dictionary<string, string> PropertyToField(string nameSpace, string className)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            StringBuilder sb = new StringBuilder();
            sb.Append("select fg_table.c_name tablename,fg_table.modelname,fg_table.namespaceprefix,fg_table.namespacesuffix,");
            sb.Append("fg_columns.c_name fieldname,propertyname from fg_columns ");
            sb.Append("left join fg_table on fg_columns.c_bname=fg_table.c_name ");
            sb.Append("where modelname={0} and propertyname={1} ");

            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("modelname", DbType.AnsiString);
            p[0].Value = className;

            DataTable dt = NG3.Data.Service.DbHelper.GetDataTable(sb.ToString(), p);

            foreach (DataRow dr in dt.Rows)
            {
                string namespaceprefix = Convert.ToString(dr["namespaceprefix"]);
                string namespacesuffix = Convert.ToString(dr["namespacesuffix"]);
                string sNameSpace = namespaceprefix + ".Model";
                if (!string.IsNullOrEmpty(namespacesuffix))
                {
                    sNameSpace += "." + namespacesuffix;
                }

                if (sNameSpace == nameSpace)
                {
                    dic.Add(dr["propertyname"].ToString(), dr["fieldname"].ToString());
                }
            }

            return dic;
        }

        /// <summary>
        /// IDataParameter拼查询串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertQueryString(string str)
        {
            //方式一： return DataConverterHelper.BuildQuery(str);

            //方式二:
            if (!string.IsNullOrEmpty(str))
            {
                Dictionary<string, object> dic =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(str);

                IDataParameter[] p = BuildQueryWithParam(dic, ref str);

                var s = new object[p.Count()];
                for (int i = 0; i < p.Count(); i++)
                {
                    s[i] = p[i].Value;
                }
                str = string.Format(str, s);
                return str;
            }
            return string.Empty;
        }

        /// <summary>
        /// Json to Dictionary
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ConvertToDic(string str,bool normal=true)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            Dictionary<string, object> dic2 = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(str))
            {
                dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(str);

                foreach (var item in dic)
                {
                    string[] arr = item.Key.Split('*');
                    if (arr.Length == 4)
                    {
                        string isAddToWhere = arr[3].ToLower();//是否直接参与查询

                        if (normal)//取直接参与查询的字段
                        {
                            if (isAddToWhere == "0") continue;
                        }
                        else//取不能直接参与查询的字段
                        {
                            if (isAddToWhere == "1") continue;
                        }

                        if (item.Value != null && !string.IsNullOrWhiteSpace(item.Value.ToString()))
                        {
                            string datatype = arr[1].ToLower();
                            string opration = arr[2].ToLower();
                            if (opration == "in")//in特殊处理，值转换为ArrayList
                            {
                                if (item.Value is JArray)
                                {

                                    ArrayList list = new ArrayList();
                                    JArray jarr = item.Value as JArray;
                                    AddValToList(datatype, list, jarr);
                                    dic2.Add(item.Key, list);
                                }
                                else
                                {
                                    ArrayList list = new ArrayList();
                                    switch (datatype)
                                    {
                                        case "byte":
                                            list.Add(Convert.ToByte(item.Value));
                                            break;
                                        case "int16":
                                            list.Add(Convert.ToInt16(item.Value));
                                            break;
                                        case "enum":
                                        case "int32":
                                            list.Add(Convert.ToInt32(item.Value));
                                            break;
                                        case "int64":
                                            list.Add(Convert.ToInt64(item.Value));
                                            break;
                                        case "number":
                                            list.Add(Convert.ToDecimal(item.Value));
                                            break;
                                        case "date":
                                            list.Add(Convert.ToDateTime(item.Value));
                                            break;
                                        default:
                                            list.Add(item.Value);
                                            break;
                                    }
                                    dic2.Add(item.Key, list);
                                }
                            }
                            else
                            {
                                AddValue(dic2, item, datatype);
                            }                          
                        }
                        else
                        {
                            //条件为空，忽略
                            //dic2.Add(item.Key, item.Value);
                        }
                    }
                    else
                    {
                        if (item.Value != null && !string.IsNullOrWhiteSpace(item.Value.ToString()))
                        {
                            dic2.Add(item.Key, item.Value);
                        }
                    }
                }
            }
            return dic2;
        }

        private static void AddValToList(string datatype, ArrayList list, JArray jarr)
        {
            foreach (JValue jval in jarr)
            {
                switch (datatype)
                {
                    case "byte":
                        list.Add(Convert.ToByte(jval.Value));
                        break;
                    case "int16":
                        list.Add(Convert.ToInt16(jval.Value));
                        break;
                    case "enum":
                    case "int32":
                        list.Add(Convert.ToInt32(jval.Value));
                        break;
                    case "int64":
                        list.Add(Convert.ToInt64(jval.Value));
                        break;
                    case "number":
                        list.Add(Convert.ToDecimal(jval.Value));
                        break;
                    case "date":
                        list.Add(Convert.ToDateTime(jval.Value));
                        break;
                    default:
                        list.Add(jval.Value);
                        break;
                }
            }
        }

        private static void AddValue(Dictionary<string, object> dic2, KeyValuePair<string, object> item, string datatype)
        {
            switch (datatype)
            {
                case "byte":
                    dic2.Add(item.Key, Convert.ToByte(item.Value));
                    break;
                case "int16":
                    dic2.Add(item.Key, Convert.ToInt16(item.Value));
                    break;
                case "enum":
                case "int32":
                    dic2.Add(item.Key, Convert.ToInt32(item.Value));
                    break;
                case "int64":
                    dic2.Add(item.Key, Convert.ToInt64(item.Value));
                    break;
                case "number":
                    dic2.Add(item.Key, Convert.ToDecimal(item.Value));
                    break;
                case "date":
                    dic2.Add(item.Key, Convert.ToDateTime(item.Value));
                    break;
                default:
                    dic2.Add(item.Key, item.Value);
                    break;
            }
        }
    }
}
