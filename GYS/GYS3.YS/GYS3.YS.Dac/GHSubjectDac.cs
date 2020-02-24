#region Summary
/**************************************************************************************
    * 类 名 称：        GHSubjectDac
    * 命名空间：        GYS3.YS.Dac
    * 文 件 名：        GHSubjectDac.cs
    * 创建时间：        2018/11/26 
    * 作    者：        董泉伟    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enterprise3.NHORM.Dac;

using GYS3.YS.Model.Domain;
using GYS3.YS.Dac.Interface;
using Enterprise3.Common.Base.Helpers;
using NG3.Data.Service;

namespace GYS3.YS.Dac
{
	/// <summary>
	/// GHSubject数据访问处理类
	/// </summary>
    public partial class GHSubjectDac : EntDacBase<GHSubjectModel>, IGHSubjectDac
    {
        #region 实现 IGHSubjectDac 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<GHSubjectModel> ExampleMethod<GHSubject>(string param)
        //{
        //    //编写代码
        //}

        public readonly string insert_sql = @"INSERT INTO z_qtltzd(ID,DJH,DWDM,YSKM_DM,PAY_JE,ZY,MXXM,DEF_STR7,DEF_STR8,year,DEF_STR1,zbly_dm,DJRQ,DEF_BZ1,xmzt,ZGY,spgw) 
            VALUES ";

        public readonly string insert_Oracle = @"INSERT INTO z_qtltzd(ID,DJH,DWDM,YSKM_DM,PAY_JE,ZY,MXXM,DEF_STR7,DEF_STR8,year,DEF_STR1,zbly_dm,DJRQ,DEF_BZ1,xmzt,ZGY,spgw) 
            VALUES ";

        public readonly string insertmst_sql = "INSERT INTO z_qtgkxm(DM,MC,DEF_STR1) VALUES ";

        public readonly string insertmst_Oracle = "INSERT INTO z_qtgkxm(DM,MC,DEF_STR1) VALUES ";

        public readonly string insertdtl_sql = "INSERT INTO JJ_FXGL(MK,XMDM,DM,MC,STR1) VALUES ";

        public readonly string insertdtl_Oracle = "INSERT INTO JJ_FXGL(MK,XMDM,DM,MC,STR1) VALUES ";

        public readonly string delete_sql = @"DELETE FROM z_qtltzd WHERE DEF_BZ1='zc'";
        public readonly string delete_Oracle = @"DELETE FROM z_qtltzd WHERE DEF_BZ1='zc'";
        public readonly string deletemst_sql = @"DELETE FROM z_qtgkxm WHERE DEF_STR1='zc'";
        public readonly string deletemst_Oracle = @"DELETE FROM z_qtgkxm WHERE DEF_STR1='zc'";
        public readonly string deletedtl_sql = @"DELETE FROM JJ_FXGL WHERE STR1='zc'";
        public readonly string deletedtl_Oracle = @"DELETE FROM JJ_FXGL WHERE STR1='zc'";

        /// <summary>
        /// 同步数据到老G6H数据库
        /// </summary>
        /// <param name="userConn"></param>
        /// <param name="zbly_dm"></param>
        /// <param name="valuesqlList"></param>
        /// <param name="mstSqlList"></param>
        /// <param name="dtlSqlList"></param>
        /// <param name="DJRQList"></param>
        /// <returns></returns>
        public int AddData(string userConn, string zbly_dm,List<string> valuesqlList, List<string> mstSqlList, List<string> dtlSqlList, List<DateTime?> DJRQList)
        {
            //string userConn = ConfigHelper.GetString("DBG6H");
            //string zbly_dm = ConfigHelper.GetString("DBG6H_ZT");
            string insertSql = "";
            string insertSql2 = "";
            string insertSql3 = "";

            var result = 0;
            //try
            //{
                DbHelper.Open(userConn);
                //DbHelper.BeginTran(userConn);
                for (var i = 0; i < valuesqlList.Count; i++)
                {
                    if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        insertSql = insert_Oracle + valuesqlList[i] + ",'" + zbly_dm + "',to_date('" + Convert.ToDateTime(DJRQList[i]).ToString("yyyy-MM-dd") + "','yyyy/mm/dd hh24:mi:ss'),'zc',3,'1','over')";
                        //+ insertmst_Oracle+ mstSqlList[i]+";"+ insertdtl_Oracle+ dtlSqlList[i];
                    }
                    else
                    {
                        insertSql = insert_sql + valuesqlList[i] + ",'" + zbly_dm + "','" + Convert.ToDateTime(DJRQList[i]).ToString("yyyy-MM-dd") + "','zc',3,'1','over')";
                        //+ insertmst_sql+ mstSqlList[i] + ";" + insertdtl_sql+ dtlSqlList[i];
                        //insertSql = string.Format(insert_sql, ID, DJH, DJRQ, DWDM, YSKM_DM, JFQD_DM, PAY_JE, ZY, DEF_STR1,
                        //DEF_STR4, DEF_INT1, DEF_INT2, MXXM, DEF_STR7, DEF_STR8, year, xmzt, int1, int2);
                    }
                    result = DbHelper.ExecuteNonQuery(userConn, insertSql);
                }
                for (var j = 0; j < mstSqlList.Count; j++)
                {
                    if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        insertSql2 = insertmst_Oracle + mstSqlList[j];
                    }
                    else
                    {
                        insertSql2 = insertmst_Oracle + mstSqlList[j];
                    }

                    result = DbHelper.ExecuteNonQuery(userConn, insertSql2);
                }
                for (var x = 0; x < dtlSqlList.Count; x++)
                {
                    if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        insertSql3 = insertdtl_Oracle + dtlSqlList[x];
                    }
                    else
                    {
                        insertSql3 = insertdtl_Oracle + dtlSqlList[x];
                    }
                    result = DbHelper.ExecuteNonQuery(userConn, insertSql3);
                }
            //    DbHelper.CommitTran(userConn);
            //}
            //catch(Exception e)
            //{
            //    result = 0;
            //    DbHelper.RollbackTran(userConn);
            //}
            //finally
            //{
            //    DbHelper.Close(userConn);
            //}
            DbHelper.Close(userConn);
            return result;
        }

        /// <summary>
        /// 基本支出审批同步数据到老G6H数据库
        /// </summary>
        /// <param name="userConn"></param>
        /// <param name="zbly_dm"></param>
        /// <param name="valuesqlList"></param>
        /// <param name="mstSqlList"></param>
        /// <param name="dtlSqlList"></param>
        /// <param name="DJRQList"></param>
        /// <returns></returns>
        public int AddDataSP(string userConn, string zbly_dm, List<string> valuesqlList, List<string> mstSqlList, List<string> dtlSqlList, List<DateTime?> DJRQList)
        {
            //string userConn = ConfigHelper.GetString("DBG6H");
            //string zbly_dm = ConfigHelper.GetString("DBG6H_ZT");
            string insertSql = "";
            string insertSql2 = "";
            string insertSql3 = "";
            string deleteSql = "";
            string deleteSql2 = "";
            string deleteSql3 = "";

            var result = 0;
            DbHelper.Open(userConn);
            if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
            {
                deleteSql = delete_Oracle;
                deleteSql2 = deletemst_Oracle;
                deleteSql3 = deletedtl_Oracle;
            }
            else
            {
                deleteSql = delete_sql;
                deleteSql2 = deletemst_sql;
                deleteSql3 = deletedtl_sql;
            }
            result = DbHelper.ExecuteNonQuery(userConn, deleteSql);
            result = DbHelper.ExecuteNonQuery(userConn, deleteSql2);
            result = DbHelper.ExecuteNonQuery(userConn, deleteSql3);
            for (var i = 0; i < valuesqlList.Count; i++)
            {
                if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    insertSql = insert_Oracle + valuesqlList[i] + ",'" + zbly_dm + "',to_date('" + Convert.ToDateTime(DJRQList[i]).ToString("yyyy-MM-dd") + "','yyyy/mm/dd hh24:mi:ss'),'zc',3,'1','over')";
                    //+ insertmst_Oracle+ mstSqlList[i]+";"+ insertdtl_Oracle+ dtlSqlList[i];
                }
                else
                {
                    insertSql = insert_sql + valuesqlList[i] + ",'" + zbly_dm + "','" + Convert.ToDateTime(DJRQList[i]).ToString("yyyy-MM-dd") + "','zc',3,'1','over')";
                    //+ insertmst_sql+ mstSqlList[i] + ";" + insertdtl_sql+ dtlSqlList[i];
                    //insertSql = string.Format(insert_sql, ID, DJH, DJRQ, DWDM, YSKM_DM, JFQD_DM, PAY_JE, ZY, DEF_STR1,
                    //DEF_STR4, DEF_INT1, DEF_INT2, MXXM, DEF_STR7, DEF_STR8, year, xmzt, int1, int2);
                }

                result = DbHelper.ExecuteNonQuery(userConn, insertSql);
            }
            for (var j = 0; j < mstSqlList.Count; j++)
            {
                if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    insertSql2 = insertmst_Oracle + mstSqlList[j];
                }
                else
                {
                    insertSql2 = insertmst_Oracle + mstSqlList[j];
                }
                result = DbHelper.ExecuteNonQuery(userConn, insertSql2);
            }
            for (var x = 0; x < dtlSqlList.Count; x++)
            {
                if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    insertSql3 = insertdtl_Oracle + dtlSqlList[x];
                }
                else
                {
                    insertSql3 = insertdtl_Oracle + dtlSqlList[x];
                }
                result = DbHelper.ExecuteNonQuery(userConn, insertSql3);
            }


            DbHelper.Close(userConn);
            return result;
        }

        #endregion
    }
}

