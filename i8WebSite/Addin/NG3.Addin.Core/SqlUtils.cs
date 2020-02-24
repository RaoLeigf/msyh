using AopAlliance.Intercept;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NG3.Addin.Model.Domain;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Transform;
using NHibernate.Type;
using Spring.Util;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core
{
    public class SqlUtils
    {

        public static bool ExecuteUpdate(ISession session, string sql)
        {
            string ReplacedSQL = sql;

            int affectrows = 0;
            try
            {
                if (string.IsNullOrEmpty(sql)) throw new AddinException("SQL语句为空");
                if (session == null) throw new AddinException("Session 为空");

                LogHelper<SqlUtils>.Info("执行SQL为：" + ReplacedSQL);

                var query = session.CreateSQLQuery(ReplacedSQL);
                affectrows = query.ExecuteUpdate();
                return true;
            }
            catch (Exception ex)
            {
                LogHelper<SqlUtils>.Error(sql + "语句执行出错!原因："+ex.Message);
                throw;
            }

        }

        /// <summary>
        /// 计算标量
        /// </summary>
        /// <param name="session"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object ExecuteScalar(ISession session, string sql)
        {
            object value = null;
            //进行UI变量替换
            string ReplacedSQL = sql;

            try
            {
                if (string.IsNullOrEmpty(sql)) throw new AddinException("SQL语句为空");
                if (session == null) throw new AddinException("Session 为空");


                LogHelper<SqlUtils>.Info("执行SQL为：" + ReplacedSQL);

                var query = session.CreateSQLQuery(ReplacedSQL);
                value = query.UniqueResult();

                LogHelper<SqlUtils>.Info("生成的结果集为：" + value);

                return value;
            }
            catch (Exception ex)
            {
                LogHelper<SqlUtils>.Error(sql + "语句执行出错!原因：" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 返回JSON结果集
        /// </summary>
        /// <param name="session"></param>
        /// <param name="sql"></param>
        /// <param name="pageNo"></param>
        /// <param name="pagerows"></param>
        /// <returns></returns>
        public static string Execute(ISession session, string sql, int pageNo,int pagerows)
        {
            try
            {
                if (string.IsNullOrEmpty(sql)) throw new AddinException("SQL语句为空");
                if (session == null) throw new AddinException("Session 为空");

                string ReplacedSQL = sql;

                int startRow;
                pageNo = pageNo - 1;
                if (pageNo < 0) pageNo = 0;
                startRow = pageNo * pagerows;


                var query = session.CreateSQLQuery(ReplacedSQL);


                var listdatas = query.SetFirstResult(startRow)
                    .SetMaxResults(pagerows)
                    .List();
                LogHelper<SqlUtils>.Info("执行SQL为：" + ReplacedSQL);

                //解析成Json字符串
                string result = SqlUtils.ToJson(ReplacedSQL, listdatas);

                LogHelper<SqlUtils>.Info("生成的结果集为：" + result);

                return result;
            }
            catch (Exception ex)
            {
                LogHelper<SqlUtils>.Error(sql+"语句执行出错!原因："+ex.Message);
                throw;
            }
           
        }
            
        /// <summary>
        /// 执行存储过程，存储过程暂时不支持返回值
        /// </summary>
        /// <param name="session"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ExecuteSP(ISession session, string sql)
        {

            //object value = null;暂时不支持返回值
            //进行UI变量替换
            string ReplacedSP = sql;
            try
            {
                if (string.IsNullOrEmpty(sql)) throw new AddinException("SQL语句为空");
                if (session == null) throw new AddinException("Session 为空");

                //统一存储过程格式调用up_update_a1(1,'jjjjsd')
                if (GetDBMSType(session)=="SQL")
                {
                    //语法类似于  EXEC up_update_a1 1,'jjjjsd'
                    ReplacedSP = ReplacedSP.Replace("(", " ");
                    ReplacedSP = ReplacedSP.Replace(")", "");

                    if(!ReplacedSP.StartsWith("exec",StringComparison.OrdinalIgnoreCase))
                    {
                        ReplacedSP = "exec " + ReplacedSP;
                    }
                    
                }
                else if (GetDBMSType(session) == "ORA")
                {
                    // oracle语法"begin   up_updata_a1(1); end;";
                    ReplacedSP = ReplacedSP.Trim();
                    if(!ReplacedSP.EndsWith(";"))
                    {
                        ReplacedSP = ReplacedSP + ";";
                        ReplacedSP = "begin " + ReplacedSP + " end;";
                    }
                    
                }
                LogHelper<SqlUtils>.Info("执行的存储过程为："+ReplacedSP);
                //执行引擎
                var query = session.CreateSQLQuery(ReplacedSP);
                //执行SQL语句
                query.UniqueResult();
                
               
                return true;
            }
            catch (Exception ex)
            {
                LogHelper<SqlUtils>.Error(ReplacedSP + "语句执行出错!原因：" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 执行数据库函数
        /// </summary>
        /// <param name="session"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object ExecuteFunc(ISession session, string sql)
        {            
            //进行UI变量替换
            string ReplacedFunc = sql;
            //前端配置的时候，只要书写 SELECT dbo.up_getname_a1(1),返回的是字符串

            //函数变量调用
            //dbo.up_getname_a1(1)
            int pos = 0, pos2 = 0; ;
            try
            {
                if (string.IsNullOrEmpty(sql)) throw new AddinException("SQL语句为空");
                if (session == null) throw new AddinException("Session 为空");
                if (GetDBMSType(session) == "SQL")
                {
                    //判断是否带有schema
                    pos = ReplacedFunc.IndexOf(".");
                   if (pos > 0)
                    {
                        pos2 = ReplacedFunc.IndexOf("(");
                        if (pos > pos2)
                        {
                            //.在（中表示.是在数组中的
                            ReplacedFunc = "dbo." + ReplacedFunc;
                        }
           
                    }else
                    {
                        ReplacedFunc = "dbo." + ReplacedFunc;
                    }
                    //SQLSERVER 语法类似于  SELECT dbo.up_getname_a1(1) as #value#    
                    ReplacedFunc = "select " + ReplacedFunc + " as value ";
                    
                }
                else if (GetDBMSType(session) == "ORA")
                {
                    // oracle语法 select up_get_name(1) from dual
                    ReplacedFunc = "select " + ReplacedFunc + " as value from dual";

                }
                LogHelper<SqlUtils>.Info("执行的函数为：" + ReplacedFunc);

                var query = session.CreateSQLQuery(ReplacedFunc);
                //执行SQL语句                
                query = session.CreateSQLQuery(ReplacedFunc).AddScalar("value", NHibernateUtil.String);
                object str = query.UniqueResult();


                LogHelper<SqlUtils>.Info("执行的函数结果为：" + ReplacedFunc);
                return str;
            }
            catch (Exception ex)
            {
                LogHelper<SqlUtils>.Error(ReplacedFunc + "语句执行出错!原因："+ex.Message);
                throw;
            }
        }
 




        private static string GetDBMSType(ISession session)
        {
            if(session==null ) throw new AddinException("当前的Session为空");

            string DbmsType = string.Empty;
            var sf = session.SessionFactory as ISessionFactoryImplementor;
            if (sf != null)
            {
                string dialectName = string.Empty;
                if(sf.Dialect!=null)
                    dialectName = sf.Dialect.GetType().ToString();
                if (dialectName.IndexOf("mssql", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    DbmsType = "SQL";
                }
                else if (dialectName.IndexOf("oracle", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    DbmsType = "ORA";
                }
            }
            if (string.IsNullOrEmpty(DbmsType)) throw new AddinException("无法判断当前连接数据库的类型");

            return DbmsType;
            
        }

        private static string ToJson(string sql,IList objects)
        {
            var data = ToSqlResultList(sql, objects);
            if(data==null)
            {
                return DataConverterHelper.EntityListToJson<SqlResultEntity>(data,ResponseStatus.Success,"Has no data!");
            }
            //返回字符串
            return DataConverterHelper.EntityListToJson<SqlResultEntity>(data, ResponseStatus.Success, "");

        }
        /// <summary>
        /// 实体转换成JSON
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        private static IList<SqlResultEntity> ToSqlResultList(string sql,IList objects)
        {
            if (string.IsNullOrEmpty(sql)) throw new AddinException("Sql语句为空");

            if (objects == null) return null;
            if (objects.Count < 1) return null;



            //通过SQL语句转解析出列名,SQL语句不支持字查询
            string queryString = sql.Trim();
            int pos1=0,pos2 = 0;
            pos1 = "select ".Length; //
            pos2 = queryString.IndexOf(" from", StringComparison.OrdinalIgnoreCase);
            string colString = queryString.Substring(pos1, pos2 - pos1);
            //所有的列
            string[] cols = colString.Split(new string[] { "," },StringSplitOptions.None);


            //判断列是否与结果集能够正确对应
            if(objects[0].GetType().IsArray)
            {
                var e = objects[0] as object[];
                if (cols.Length != e.Length) throw new AddinException("列数目与结果集列数目无法对应!");
            }
            else
            {
                //就一个值的情况
                if (cols.Length != 1) throw new AddinException("列数目与结果集列数目无法对应!");
            }

            //提取判断别名
            for (int i = 0; i < cols.Length; i++)
            {
                string item = cols[i].Trim();
                //取空格的位置
                int pos = item.LastIndexOf(" "); //空格来判断别名
                if(pos >0)
                {
                    //存在别名
                    string alias = item.Substring(pos + 1);
                    cols[i] = alias;
                }
            }

            //object是个2维数组
            List<SqlResultEntity> results = new List<SqlResultEntity>();
            //结果集循环
            foreach (var item in objects)
            {
                object[] row;
                if (item.GetType().IsArray)
                {
                    row = (object[])item;
                }
                else
                {
                    row = new object[] { item };
                }
                
                //行集
                Dictionary<string, object> dic = new Dictionary<string, object>();
                for (int i = 0; i < row.Length; i++)
                {

                    dic.Add(cols[i], row[i]);
                }
                SqlResultEntity entity = new SqlResultEntity();
                entity.ExtendObjects = dic;
                results.Add(entity);
            }

            return results;

        }
  
    }


}
