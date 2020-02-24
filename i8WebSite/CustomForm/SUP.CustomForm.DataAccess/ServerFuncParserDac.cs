using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NG3.Data.Service;
using SUP.Common.Base;

namespace SUP.CustomForm.DataAccess
{
    public class ServerFuncParserDac
    {
        /// <summary>
        /// 获得sql语句的结果;
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string GetSqlValue(string sql)
        {
            if (sql == string.Empty) return string.Empty;

            var value = DbHelper.GetString(sql);
            return value;
        }

        /// <summary>
        /// 取DataTable数据;
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            if (sql == string.Empty) return null;

            DataTable dt = DbHelper.GetDataTable(sql);
            return dt;
        }

        /// <summary>
        /// 执行sql语句;
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteSql(string sql)
        {
            if (sql == string.Empty) return -1;

            var value = DbHelper.ExecuteNonQuery(sql);
            return value;
        }
    }
}
