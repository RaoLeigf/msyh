#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectMstDac
    * 命名空间：        GXM3.XM.Dac
    * 文 件 名：        ProjectMstDac.cs
    * 创建时间：        2018/8/28 
    * 作    者：        李明    
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

using GXM3.XM.Model.Domain;
using GXM3.XM.Dac.Interface;
using Enterprise3.Common.Base.Helpers;
using NG3.Data.Service;

namespace GXM3.XM.Dac
{
	/// <summary>
	/// ProjectMst数据访问处理类
	/// </summary>
    public partial class ProjectMstDac : EntDacBase<ProjectMstModel>, IProjectMstDac
    {
        #region 实现 IProjectMstDac 业务添加的成员

        public readonly string insert_sql = @"INSERT INTO z_qtltzd(ID,DJH,DWDM,YSKM_DM,JFQD_DM,PAY_JE,ZY,DEF_STR1,DEF_STR4,DEF_INT1,DEF_INT2,MXXM,DEF_STR7,DEF_STR8,year,xmzt,int1,int2,ACCNO1,zbly_dm,DJRQ,DEF_BZ1,ZGY,spgw,DT1,DT2) 
            VALUES ";
        public readonly string insert_Oracle = @"INSERT INTO z_qtltzd(ID,DJH,DWDM,YSKM_DM,JFQD_DM,PAY_JE,ZY,DEF_STR1,DEF_STR4,DEF_INT1,DEF_INT2,MXXM,DEF_STR7,DEF_STR8,year,xmzt,int1,int2,ACCNO1,zbly_dm,DJRQ,DEF_BZ1,ZGY,spgw,DT1,DT2) 
            VALUES ";
        //({0},'{1}','{2}','{3}','{4}','{5}',{6},'{7}','{8}','{9}',{10},{11},'{12}','{13}','{14}',{15},{16},{17},{18})

        public readonly string insertmst_sql = @"INSERT INTO z_qtgkxm(DM,MC,DEF_STR1) VALUES ";
        public readonly string insertmst_Oracle = @"INSERT INTO z_qtgkxm(DM,MC,DEF_STR1) VALUES ";

        public readonly string insertdtl_sql = @"INSERT INTO JJ_FXGL(MK,XMDM,DM,MC,STR1) VALUES ";
        public readonly string insertdtl_Oracle = @"INSERT INTO JJ_FXGL(MK,XMDM,DM,MC,STR1) VALUES ";

        public readonly string delete_Oracle = @"DELETE FROM z_qtltzd WHERE DEF_STR1=";
        public readonly string delete_sql = @"DELETE FROM z_qtltzd WHERE DEF_STR1=";

        public readonly string deletemst_sql = @"DELETE FROM z_qtgkxm WHERE DM=";
        public readonly string deletemst_Oracle = @"DELETE FROM z_qtgkxm WHERE DM=";

        public readonly string deletedtl_sql = @"DELETE FROM JJ_FXGL WHERE DM=";
        public readonly string deletedtl_Oracle = @"DELETE FROM JJ_FXGL WHERE DM=";

        public readonly string updateZGY = @"UPDATE z_qtltzd SET ZGY='0' WHERE DJH = ";

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<ProjectMstModel> ExampleMethod<ProjectMst>(string param)
        //{
        //    //编写代码
        //}

        /// <summary>
        /// 审批时同步项目数据
        /// </summary>
        /// <param name="userConn"></param>
        /// <param name="zbly_dm"></param>
        /// <param name="valuesqlList"></param>
        /// <param name="mstSql"></param>
        /// <param name="dtlSqlList"></param>
        /// <param name="DJRQList"></param>
        /// <param name="DEF_BZ1"></param>
        /// <param name="mstCode"></param>
        /// <param name="dtlcodeList"></param>
        /// <param name="DJH"></param>
        /// <param name="DT1List"></param>
        /// <param name="DT2List"></param>
        /// <returns></returns>
        public int ApproveAddData(string userConn, string zbly_dm,List<string> valuesqlList, string mstSql, List<string> dtlSqlList, List<DateTime?> DJRQList, string DEF_BZ1, string mstCode, List<string> dtlcodeList, string DJH, List<DateTime?> DT1List, List<DateTime?> DT2List)
        {
            //string userConn = ConfigHelper.GetString("DBG6H");
            //string zbly_dm = ConfigHelper.GetString("DBG6H_ZT");
            string insertSql = "";
            string insertSql2 = "";
            string insertSql3 = "";
            string deleteSql = "";
            string deleteSql2 = "";
            string deleteSql3 = "";
            string UPDATESql = "";
            var result = 0;
            //try
            //{
            DbHelper.Open(userConn);
            //DbHelper.BeginTran(userConn);
            if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
            {
                deleteSql = delete_Oracle + "'" + mstCode + "' AND DJH = '" + DJH + "'";
                deleteSql2 = deletemst_Oracle + "'" + mstCode + "'";
                insertSql2 = insertmst_Oracle + mstSql;
            }
            else
            {
                deleteSql = delete_sql + "'" + mstCode + "' AND DJH = '" + DJH + "'";
                deleteSql2 = deletemst_sql + "'" + mstCode + "'";
                insertSql2 = insertmst_sql + mstSql;
            }
            result = DbHelper.ExecuteNonQuery(userConn, deleteSql);
            result = DbHelper.ExecuteNonQuery(userConn, deleteSql2);
            result = DbHelper.ExecuteNonQuery(userConn, insertSql2);
            for (var i = 0; i < dtlcodeList.Count; i++)
            {
                if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    deleteSql3 = deletedtl_Oracle + "'" + dtlcodeList[i] + "'";
                }
                else
                {
                    deleteSql3 = deletedtl_sql + "'" + dtlcodeList[i] + "'";
                }
                result = DbHelper.ExecuteNonQuery(userConn, deleteSql3);
            }
            for (var j = 0; j < valuesqlList.Count; j++)
            {
                if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    insertSql = insert_Oracle + valuesqlList[j] + ",'" + zbly_dm + "',to_date('" + Convert.ToDateTime(DJRQList[j]).ToString("yyyy-MM-dd") + "','yyyy/mm/dd hh24:mi:ss'),'" + DEF_BZ1 + "','1','over'" +
                    ",to_date('" + Convert.ToDateTime(DT1List[j]).ToString("yyyy-MM-dd") + "','yyyy/mm/dd hh24:mi:ss')" +
                    ",to_date('" + Convert.ToDateTime(DT2List[j]).ToString("yyyy-MM-dd") + "','yyyy/mm/dd hh24:mi:ss') )";
                }
                else
                {
                    insertSql = insert_sql + valuesqlList[j] + ",'" + zbly_dm + "','" + Convert.ToDateTime(DJRQList[j]).ToString("yyyy-MM-dd") + "','" + DEF_BZ1 + "','1','over'," +
                    Convert.ToDateTime(DT1List[j]).ToString("yyyy-MM-dd") + "," + Convert.ToDateTime(DT2List[j]).ToString("yyyy-MM-dd") + ")";
                }
                result = DbHelper.ExecuteNonQuery(userConn, insertSql);
                if (DJH.Substring(DJH.Length - 4, 4) == "0002")
                {
                    UPDATESql = updateZGY + "'" + DJH.Substring(0, DJH.Length - 4) + "0001" + "'";
                    result = DbHelper.ExecuteNonQuery(userConn, UPDATESql);
                }
                
            }
            for (var x = 0; x < dtlSqlList.Count; x++)
            {
                if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    insertSql3 = insertdtl_Oracle + dtlSqlList[x];
                }
                else
                {
                    insertSql3 = insertdtl_sql + dtlSqlList[x];
                }
                result = DbHelper.ExecuteNonQuery(userConn, insertSql3);
            }
            //    DbHelper.CommitTran(userConn);
            //}
            //catch (Exception e)
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
        /// 年中调整预执行时同步项目数据
        /// </summary>
        /// <param name="userConn"></param>
        /// <param name="zbly_dm"></param>
        /// <param name="valuesqlList"></param>
        /// <param name="mstSql"></param>
        /// <param name="dtlSqlList"></param>
        /// <param name="DJRQList"></param>
        /// <param name="DEF_BZ1"></param>
        /// <param name="mstCode"></param>
        /// <param name="dtlcodeList"></param>
        /// <param name="DJH"></param>
        /// <returns></returns>
        public int ApproveAddData2(string userConn, string zbly_dm, List<string>[] valuesqlList, List<string> mstSql, List<string> dtlSqlList, List<DateTime?>[] DJRQList, string DEF_BZ1, List<string> mstCode, List<string> dtlcodeList, List<string> DJH)
        {
            //string userConn = ConfigHelper.GetString("DBG6H");
            //string zbly_dm = ConfigHelper.GetString("DBG6H_ZT");
            string insertSql = "";
            string insertSql2 = "";
            string insertSql3 = "";
            string deleteSql = "";
            string deleteSql2 = "";
            string deleteSql3 = "";
            string UPDATESql = "";
            var result = 0;
            //try
            //{
            DbHelper.Open(userConn);
            //DbHelper.BeginTran(userConn);
            for(int i=0; i< mstCode.Count; i++)
            {
                if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    deleteSql = delete_Oracle + "'" + mstCode + "' AND DJH = '" + DJH + "'";
                    deleteSql2 = deletemst_Oracle + "'" + mstCode + "'";
                }
                else
                {
                    deleteSql = delete_sql + "'" + mstCode + "' AND DJH = '" + DJH + "'";
                    deleteSql2 = deletemst_sql + "'" + mstCode + "'";
                }
                result = DbHelper.ExecuteNonQuery(userConn, deleteSql);
                result = DbHelper.ExecuteNonQuery(userConn, deleteSql2);
            }
            for(int i=0;i< mstSql.Count; i++)
            {
                if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    insertSql2 = insertmst_Oracle + mstSql;
                }
                else
                {
                    insertSql2 = insertmst_sql + mstSql;
                }
                result = DbHelper.ExecuteNonQuery(userConn, insertSql2);
            }

            //result = DbHelper.ExecuteNonQuery(userConn, deleteSql);
            //result = DbHelper.ExecuteNonQuery(userConn, deleteSql2);
            //result = DbHelper.ExecuteNonQuery(userConn, insertSql2);
            for (var i = 0; i < dtlcodeList.Count; i++)
            {
                if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    deleteSql3 = deletedtl_Oracle + "'" + dtlcodeList[i] + "'";
                }
                else
                {
                    deleteSql3 = deletedtl_sql + "'" + dtlcodeList[i] + "'";
                }
                result = DbHelper.ExecuteNonQuery(userConn, deleteSql3);
            }
            for (var j = 0; j < valuesqlList.Length; j++)
            {
                for(var k=0; k < valuesqlList[j].Count; k++)
                {
                    if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        insertSql = insert_Oracle + valuesqlList[j][k] + ",'" + zbly_dm + "',to_date('" + Convert.ToDateTime(DJRQList[j][k]).ToString("yyyy-MM-dd") + "','yyyy/mm/dd hh24:mi:ss'),'" + DEF_BZ1 + "','1','over')";
                    }
                    else
                    {
                        insertSql = insert_sql + valuesqlList[j][k] + ",'" + zbly_dm + "','" + Convert.ToDateTime(DJRQList[j][k]).ToString("yyyy-MM-dd") + "','" + DEF_BZ1 + "','1','over')";
                    }
                    result = DbHelper.ExecuteNonQuery(userConn, insertSql);
                    if (DJH[j].Substring(DJH[j].Length - 4, 4) == "0002")
                    {
                        UPDATESql = updateZGY + "'" + DJH[j].Substring(0, DJH[j].Length - 4) + "0001" + "'";
                        result = DbHelper.ExecuteNonQuery(userConn, UPDATESql);
                    }
                }


            }
            for (var x = 0; x < dtlSqlList.Count; x++)
            {
                if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    insertSql3 = insertdtl_Oracle + dtlSqlList[x];
                }
                else
                {
                    insertSql3 = insertdtl_sql + dtlSqlList[x];
                }
                result = DbHelper.ExecuteNonQuery(userConn, insertSql3);
            }
            //    DbHelper.CommitTran(userConn);
            //}
            //catch (Exception e)
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
        #endregion
    }
}

