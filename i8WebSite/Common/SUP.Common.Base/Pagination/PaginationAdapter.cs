using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NG3.Data.Service;
using System.Data;

namespace SUP.Common.Base
{
    public class PaginationAdapter
    {

        private static PaginationSqlHelper  helper = new PaginationSqlHelper();


        /// <summary>
        ///  获取 分页 查询串
        /// </summary>
        /// <param name="sql">原始查询语句</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页号</param>
        /// <param name="rowCount">总页数</param>
        /// <param name="sortField">排序字段</param>
        /// <returns></returns>
        public static string GetPageDataSql(string sql, int pageSize, ref int pageIndex, ref int rowCount, string sortField)
        {
            return GetPageDataSql(sql, pageSize, ref pageIndex, ref rowCount, sortField,null);
        }

        public static string GetPageDataSql(string sql, int pageSize, ref int pageIndex, ref int rowCount, string sortField, IDataParameter[] parameter)
        {
            return GetPageDataSql(sql, pageSize, ref pageIndex, ref rowCount, sortField, parameter, string.Empty);
        }

        /// <summary>
        /// 获取 分页 查询串
        /// </summary>
        /// <param name="sql">业务sql语句</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页号</param>
        /// <param name="rowCount">总行数</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="parameter">参数化查询的参数列表</param>
        /// <returns></returns>
        public static string GetPageDataSql(string sql, int pageSize, ref int pageIndex, ref int rowCount, string sortField, IDataParameter[] parameter,string connStr="")
        {

            #region 计算总行数
            string tmpSql = sql; 
            int orderIndex = sql.ToLower().LastIndexOf(" order ");//TODO:不支持多个 order 本身的select 语句串 
            if (orderIndex > -1)
            {
                tmpSql = sql.Substring(0, orderIndex);
            }

            tmpSql = helper.GetQueryVirtualCountSql(tmpSql);

            string strCount = string.Empty;

            if (parameter == null)
            {
                if (string.IsNullOrEmpty(connStr))
                {
                    strCount = Convert.ToString(DbHelper.ExecuteScalar(tmpSql));//得到当前总行数
                }
                else
                {
                    strCount = Convert.ToString(DbHelper.ExecuteScalar(connStr, tmpSql));//得到当前总行数
                }
            }
            else
            {
                if (string.IsNullOrEmpty(connStr))
                {
                    strCount = Convert.ToString(DbHelper.ExecuteScalar(tmpSql, parameter));//得到当前总行数
                }
                else
                {
                    strCount = Convert.ToString(DbHelper.ExecuteScalar(connStr,tmpSql, parameter));//得到当前总行数
                }
                
            }
            rowCount = (strCount.Length == 0) ? 0 : int.Parse(strCount);

            #endregion

            #region 计算总页数,页号

            int pageCount = (int)Math.Ceiling(rowCount / (double)pageSize);//总页数

            if (pageIndex >= pageCount) //调整当前页号值
            {
                pageIndex = (pageCount == 0) ? 0 : pageCount - 1;
            }
            if (pageIndex < 0)
            {
                pageIndex = 0;
            }

            #endregion

            return helper.GetPageDataSql(sql, pageSize, pageIndex, sortField, rowCount);
        }
        
    }
}
