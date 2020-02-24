using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NG3.Data;
using NG3.Data.Service;

namespace SUP.Common.Base
{
    public class PaginationSqlHelper
    {

        #region private

        private string _sortField;			//排序表达式
        private string _queryCommand;		//查询条件


        #endregion

        public PaginationSqlHelper()
        {
 
        }

        
        /// <summary>
        /// 获取 分页 查询串，
        /// 不支持多个Order by 的语句，
        /// union 查询串需要程序员自己套一个 select * from (union 语句串) table order by sortfield
        /// </summary>
        /// <param name="sql">原始查询串</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageNo">页号</param>
        /// <param name="sortFields">排序字段</param>
        /// <param name="totalRowCount">总行数</param>
        /// <returns></returns>
        public  string GetPageDataSql(string sql, int pageSize, int pageNo, string sortFields, int totalRowCount)
        {
            if (string.IsNullOrEmpty(sql) || pageSize < 1)
            {
                return sql;
            }
            if (string.IsNullOrWhiteSpace(sortFields)) 
            {
                throw new ArgumentException("the argument sortFields can not be empty!");
            }

            string newsql = string.Empty;

            int begin = 0, end = 0;    
            switch (DbHelper.Vendor)
            {
                //----以下分页写法，oracle数据库分页后，某些列的值为空了-------
                //case DbVendor.Oracle:
                //    #region Oralce
                //    int begin = 0, end = 0;

                //    begin = pageNo * pageSize + 1;
                //    end = (pageNo + 1) * pageSize;

                //    //若为ORACLE数据库，则采用嵌套3层的查询语句结合rownum来实现分页（疑惑：目前似乎１层或２层也支持order by）
                //    newsql = "select * from ( select mytable.*, rownum rownumber from ( ";
                //    newsql += sql;
                //    //				sNewsql += " ) row_ where rownum <= {0}) where rownum_ >= {1}";
                //    //modified by huds on 2011-06-14 for oracle排序
                //    //此代码实为想当然sql中不存在order by子句，但船大不调，他日问题
                //    //呈现，再图良策
                //    if (sortFields.Length > 0 && sql.ToLower().LastIndexOf("order") < 0)
                //    {
                //        if (sortFields.ToLower().IndexOf("order") >= 0)
                //        {
                //            newsql += sortFields;
                //        }
                //        else
                //        {
                //            newsql += " order by " + sortFields;
                //        }
                //    }
                //    //end add
                //    //newsql += " ) mytable ) where rownumber >= {1} and rownumber<= {0}";//原来的分页有问题，更改如此

                //    newsql += " ) mytable ) where rownumber >= " + begin.ToString() + " and rownumber<= " + end.ToString();

                //    //newsql = string.Format(newsql, end, begin);
                //    #endregion
                //    break;
                case DbVendor.ASE:
                    #region ASE
                    int iRowcount = pageSize * (pageNo + 1);
                    newsql = " set rowcount " + iRowcount;
                    newsql = newsql + " " + sql;
                    #endregion
                    break;
                case DbVendor.MySql:
                    #region MySql

                    begin = 0;
                    begin = pageNo * pageSize;

                    newsql = "select mytable.* from ( ";
                    newsql += sql;
                    if (sortFields.Length > 0 && sql.ToLower().LastIndexOf("order") < 0)
                    {
                        if (sortFields.ToLower().IndexOf("order") >= 0)
                        {
                            newsql += sortFields;
                        }
                        else
                        {
                            newsql += " order by " + sortFields;
                        }
                    }

                    //newsql += " ) mytable limit {0},{1}";
                    //newsql = string.Format(newsql,begin,pageSize);
                    newsql += " ) mytable limit " + begin.ToString() + "," + pageSize.ToString();

                    #endregion
                    break;
                case DbVendor.Oracle:
                default:
                    #region SQL Server

                    //sqlserver2005以上数据库，使用ROW_NUMBER方式提高性能                 
                    //if (DbHelper.VendorDetail == DbVendor.SQLServer2005 || DbHelper.VendorDetail == DbVendor.SQLServer2008)
                    {
                        string temp = sql;
                        int indexoforderby = temp.ToUpper().LastIndexOf("ORDER BY");
                        if (string.IsNullOrEmpty(sortFields))
                        {

                            if (indexoforderby < 0)
                            {
                                throw new Exception("请传入order by 字段");
                            }
                            else
                            {
                                sortFields = sql.Substring(indexoforderby);

                                //sql = sql.Substring(0, indexoforderby);//去掉原始sql语句中的order by语句
                            }
                        }

                        //处理order by语句
                        if (sortFields.ToUpper().IndexOf("ORDER BY") > -1)
                        {
                            sortFields = sortFields.ToUpper().Replace("ORDER BY", "");//截取order by字段                            
                        }
                        sortFields = GetNoTableSortField(sortFields);

                        if (indexoforderby > 0)
                        {
                            //去掉原来sql中的order by语句                                     
                            sql = sql.Substring(0, indexoforderby);//去掉原始sql语句中的order by语句
                        }

                        int beginindex = 0, endindex = 0;
                        beginindex = pageNo * pageSize + 1;
                        endindex = (pageNo + 1) * pageSize;

                        StringBuilder strbuilder = new StringBuilder();
                        strbuilder.Append("select * from ");
                        strbuilder.Append(" ( ");
                        //strbuilder.Append(" select row_number() over(order by {0} ) as 'rownumber', table1.* ");//sqlserver可以，oracle不行
                        strbuilder.Append(" select row_number() over(order by {0} ) rownumber, table1.* ");//
                        strbuilder.Append(" from ");
                        strbuilder.Append(" ( {1} ) table1 ");
                        strbuilder.Append(" ) temp_table ");
                        strbuilder.Append("where temp_table.rownumber >= {2} and temp_table.rownumber <= {3}");

                        newsql = string.Format(strbuilder.ToString(), sortFields, sql, beginindex, endindex);
                    }
                    //else
                    //{
                    //    #region SQL中插入top的位置
                    //    int indexInsertTopKeyword = 0;	//SQL中插入top的位置
                    //    //先找select 
                    //    string trimSql = sql.Trim().ToLower();
                    //    if (trimSql.StartsWith("select "))
                    //    {
                    //        //说明正确的传入了select语句，那就可以在未经过Trim()的字符串中查找第一个"select"就可以了
                    //        //select 后的第一个空白的索引值
                    //        indexInsertTopKeyword = sql.IndexOf("select", StringComparison.OrdinalIgnoreCase) + 6;

                    //        //再找select后是否直接跟着distinct，有就改变索引
                    //        string afterTruncateSelectSql = trimSql.Substring(6).Trim();
                    //        if (afterTruncateSelectSql.StartsWith("distinct "))
                    //        {
                    //            indexInsertTopKeyword = sql.IndexOf(" distinct ", StringComparison.OrdinalIgnoreCase) + 10;
                    //        }
                    //    }
                    //    #endregion

                    //    #region 得到 (原始和反转) 排序表达式
                    //    if (!string.IsNullOrEmpty(sortFields))
                    //    {
                    //        int orderIndex = sortFields.ToLower().LastIndexOf(" by ");//TODO:不支持多个Order by 的语句
                    //        if (orderIndex > -1)
                    //        {
                    //            //sortFields = sortFields.Substring(orderIndex + 2);
                    //            sortFields = sortFields.Substring(orderIndex + 3);
                    //        }
                    //    }
                    //    this.AdjustSelectCommandEx(sql, sortFields);
                    //    if (string.IsNullOrEmpty(this._sortField) && string.IsNullOrEmpty(sortFields))   //原始查询语句中无排序字段，参数也未指定排序字段
                    //    {
                    //        return sql;
                    //    }

                    //    string originalSortExpression = " order by " + this._sortField;//原始 排序表达式 
                    //    string originalSortExpressionNoTable = " order by " + this.GetNoTableSortField();
                    //    string reverseSortExpressionNoTable = " order by " + this.GetSortFieldDESC();//反转 排序表达式   

                    //    #endregion

                    //    #region  整理请求的行数量
                    //    int lastPageCount = totalRowCount % pageSize;//最后一页数量
                    //    int pageCount;//页数
                    //    int requestRowCount = pageSize;//请求的行数量

                    //    if (lastPageCount > 0)
                    //    {
                    //        pageCount = totalRowCount / pageSize + 1;
                    //    }
                    //    else
                    //    {
                    //        pageCount = totalRowCount / pageSize;
                    //        lastPageCount = pageSize;
                    //    }

                    //    if (pageNo + 1 == pageCount)//是否最后一页
                    //        requestRowCount = lastPageCount;
                    //    #endregion

                    //    #region 拼装 分页 SQL
                    //    newsql = this._queryCommand.Insert(indexInsertTopKeyword, " top " + (pageSize * (pageNo + 1)).ToString() + " ") + originalSortExpression; //例如，select top 193000 * from c_ba_status_his order by c_code asc
                    //    newsql = string.Format("select * from ( select top {0}  * from ( {1} )  t1  {2} ) t2  {3}", requestRowCount.ToString(), newsql,
                    //        reverseSortExpressionNoTable, originalSortExpressionNoTable);
                    //    #endregion
                    //}


                    #endregion
                    break;
            }
            return newsql;
        }
        
        /// <summary>
        /// 得到查询的虚拟结果数量的Sql(asSql的select列必须保证唯一性)
        /// </summary>
        /// <param name="asSql">取数原始sql</param>
        /// <returns></returns>
        public  string GetQueryVirtualCountSql(string asSql)
        {
            //modified by weizj, 2010.9.21 只处理sql2005及以上 for W300040378
            //if (DbHelper.VendorDetail == DbVendor.SQLServer2008 || DbHelper.VendorDetail == DbVendor.SQLServer2005)
            if (DbHelper.Vendor == DbVendor.SQLServer)
            {
                //去掉最后的那个order by语句
                string temp = asSql.ToUpper();
                int indexoforderby = temp.LastIndexOf("ORDER BY");
                if (indexoforderby > 0)
                {
                    asSql = asSql.Substring(0, indexoforderby);
                }
            }

            string strQuery = string.Empty;
            //string strCount = "0";

            string pagingCommandText = "SELECT COUNT(*) FROM ({0}) table1";

            switch (DbHelper.Vendor)
            {
                case DbVendor.Oracle:
                case DbVendor.Oracle9:// Orcale数据源
                    strQuery = string.Format(pagingCommandText.Replace(" table1", ""), asSql);
                    break;
                case DbVendor.SQLServer:// 非Orcale数据源
                    strQuery = string.Format(pagingCommandText, asSql);
                    break;
                case DbVendor.ASE:// 非Orcale数据源
                    if (asSql.IndexOf("from") < 0)
                    {
                        strQuery = "select count(*) " + asSql.Substring(asSql.IndexOf("FROM"));
                    }
                    else
                    {
                        strQuery = "select count(*) " + asSql.Substring(asSql.IndexOf("from"));
                    }
                    break;
                default:
                    strQuery = string.Format(pagingCommandText, asSql);
                    break;
            }

            //strCount = this.GetString(strQuery);
            //int iRet = int.Parse(strCount);

            return strQuery;
        }
        
        /// <summary>
        /// 调整Select 语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="appendSortFild"></param>
        /// <returns></returns>
        private void AdjustSelectCommandEx(string sql, string appendSortFild)
        {
            string tmpSql = sql.ToLower();
            string newSortFild = string.Empty;
            string[] sortFilds = null;//查询串中的排序字段数组

            int orderIndex = tmpSql.LastIndexOf(" order ");
            if (orderIndex > -1)
            {
                this._queryCommand = sql.Substring(0, orderIndex);
                newSortFild = sql.Substring(orderIndex - 1); //去掉order by 关键字
                orderIndex = newSortFild.IndexOf(" by ");
                newSortFild = newSortFild.Substring(orderIndex + 4).Trim();
                sortFilds = newSortFild.Split(',');//取得查询串中的排序字段数组
            }
            else
            {
                this._queryCommand = tmpSql;
            }

            //
            //判断appdSortFilds中的字段在sortFild中否存在，不存在要加入
            //
            string[] appdSortFilds = appendSortFild.Split(',');
            if (appdSortFilds.Length > 0)
            {
                if (newSortFild.Length == 0)
                    newSortFild = " " + appendSortFild + " ";
                else
                {
                    string addSort = "";
                    for (int i = 0; i < appdSortFilds.Length; i++)
                    {
                        int j;
                        for (j = 0; j < sortFilds.Length; j++)
                        {
                            if (sortFilds[j].IndexOf(appdSortFilds[i], StringComparison.OrdinalIgnoreCase) >= 0)
                                break;
                        }
                        if (j == sortFilds.Length) //说明没找到
                        {
                            addSort += ("," + appdSortFilds[i]);
                        }

                    }
                    newSortFild += addSort;
                }
            }

            this._sortField = newSortFild;
        }
        
        /// <summary>
        /// 得到没有表名的排序串
        /// </summary>
        /// <returns></returns>
        private string GetNoTableSortField()
        {
            string[] sArrSort = this._sortField.Split(',');
            if (sArrSort.Length == 0) return "";
            int indx = -1;

            for (int i = 0; i < sArrSort.Length; i++)
            {
                indx = sArrSort[i].IndexOf(".");
                if (indx > -1)
                {
                    sArrSort[i] = sArrSort[i].Substring(indx + 1).Trim();
                }

            }

            return string.Join(",", sArrSort);

        }

        /// <summary>
        /// 得到没有表名的排序串
        /// </summary>
        /// <returns></returns>
        private  string GetNoTableSortField(string sortField)
        {
            string[] sArrSort = sortField.Split(',');
            if (sArrSort.Length == 0) return "";
            int indx = -1;

            for (int i = 0; i < sArrSort.Length; i++)
            {
                indx = sArrSort[i].IndexOf(".");
                if (indx > -1)
                {
                    sArrSort[i] = sArrSort[i].Substring(indx + 1).Trim();
                }

            }
            return string.Join(",", sArrSort);
        }

        /// <summary>
        /// 根据SortField得到反向的排序串
        /// </summary>
        /// <returns>排序串</returns>
        private string GetSortFieldDESC()
        {
            string retString = string.Empty;
            string[] sArrSort = this.GetNoTableSortField().Split(',');

            if (sArrSort.Length == 0) return "";

            string sTemp = string.Empty;
            for (int i = 0; i < sArrSort.Length; i++)
            {
                sArrSort[i] = sArrSort[i].Trim().ToLower();
                if (sArrSort[i].IndexOf("asc") >= 1)
                {
                    // 替换排序方式
                    sTemp = sArrSort[i].Replace(" asc", "###");
                    sTemp = sTemp.Replace(" desc", " asc");
                    sTemp = sTemp.Replace("###", " desc");
                }
                else if (sArrSort[i].IndexOf("desc") >= 1)
                {
                    // 替换排序方式
                    sTemp = sArrSort[i].Replace(" desc", "###");
                    sTemp = sTemp.Replace(" asc", " desc");
                    sTemp = sTemp.Replace("###", " asc");
                }
                else
                    sTemp = sArrSort[i] + " desc";
                retString = (retString == "" ? retString = sTemp : retString + "," + sTemp);
            }
            return retString;
        } 

    }
}
