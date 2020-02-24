#region Summary
/**************************************************************************************
    * 类 名 称：        BudgetMstDac
    * 命名空间：        GYS3.YS.Dac
    * 文 件 名：        BudgetMstDac.cs
    * 创建时间：        2018/8/30 
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
using System.Data;
using GData3.Common.Utils;

namespace GYS3.YS.Dac
{
	/// <summary>
	/// BudgetMst数据访问处理类
	/// </summary>
    public partial class BudgetMstDac : EntDacBase<BudgetMstModel>, IBudgetMstDac
    {
        #region 实现 IBudgetMstDac 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<BudgetMstModel> ExampleMethod<BudgetMst>(string param)
        //{
        //    //编写代码
        //}

        public readonly string insert_sql = @"INSERT INTO z_qtltzd(ID,DJH,DWDM,YSKM_DM,JFQD_DM,PAY_JE,ZY,DEF_STR1,DEF_STR4,DEF_INT1,DEF_INT2,MXXM,DEF_STR7,DEF_STR8,year,xmzt,int1,int2,ACCNO1,zbly_dm,DJRQ,DEF_BZ1,ZGY,spgw,DT1,DT2) 
            VALUES ";

        public readonly string insert_Oracle = @"INSERT INTO z_qtltzd(ID,DJH,DWDM,YSKM_DM,JFQD_DM,PAY_JE,ZY,DEF_STR1,DEF_STR4,DEF_INT1,DEF_INT2,MXXM,DEF_STR7,DEF_STR8,year,xmzt,int1,int2,ACCNO1,zbly_dm,DJRQ,DEF_BZ1,ZGY,spgw,DT1,DT2) 
            VALUES ";
        //({0},'{1}','{2}','{3}','{4}','{5}',{6},'{7}','{8}','{9}',{10},{11},'{12}','{13}','{14}',{15},{16},{17},{18})

        public readonly string select_Id = @"SELECT ID FROM z_qtltzd ORDER BY ID DESC";

        public readonly string insertmst_sql = "INSERT INTO z_qtgkxm(DM,MC,DEF_STR1) VALUES ";

        public readonly string insertmst_Oracle = "INSERT INTO z_qtgkxm(DM,MC,DEF_STR1) VALUES ";

        public readonly string insertdtl_sql = "INSERT INTO JJ_FXGL(MK,XMDM,DM,MC,STR1) VALUES ";

        public readonly string insertdtl_Oracle = "INSERT INTO JJ_FXGL(MK,XMDM,DM,MC,STR1) VALUES ";

        /// <summary>
        /// 同步数据到老G6H数据库
        /// </summary>
        /// <param name="userConn"></param>
        /// <param name="zbly_dm"></param>
        /// <param name="valuesqlList"></param>
        /// <param name="mstSqlList"></param>
        /// <param name="dtlSqlList"></param>
        /// <param name="DJRQList"></param>
        /// <param name="DEF_BZ1"></param>
        /// <param name="DT1List"></param>
        /// <param name="DT2List"></param>
        /// <returns></returns>
        public int AddData(string userConn, string zbly_dm, List<string> valuesqlList, List<string> mstSqlList, List<string> dtlSqlList, List<DateTime?> DJRQList, string DEF_BZ1, List<DateTime?> DT1List, List<DateTime?> DT2List)
        {
            //string userConn = ConfigHelper.GetString("DBG6H");
            //string zbly_dm = ConfigHelper.GetString("DBG6H_ZT");
            //DataTable dataTable = null;
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
                    insertSql = insert_Oracle + valuesqlList[i] + ",'" + zbly_dm + "',to_date('" + Convert.ToDateTime(DJRQList[i]).ToString("yyyy-MM-dd") + "','yyyy/mm/dd hh24:mi:ss'),'" + DEF_BZ1 + "','1','over'" +
                    ",to_date('" + Convert.ToDateTime(DT1List[i]).ToString("yyyy-MM-dd") + "','yyyy/mm/dd hh24:mi:ss')" +
                    ",to_date('" + Convert.ToDateTime(DT2List[i]).ToString("yyyy-MM-dd") + "','yyyy/mm/dd hh24:mi:ss') )";
                        //+ insertmst_Oracle+ mstSqlList[i]+";"+ insertdtl_Oracle+ dtlSqlList[i];
                    }
                    else
                    {
                    insertSql = insert_sql + valuesqlList[i] + ",'" + zbly_dm + "','" + Convert.ToDateTime(DJRQList[i]).ToString("yyyy-MM-dd") + "','" + DEF_BZ1 + "','1','over'," +
                    Convert.ToDateTime(DT1List[i]).ToString("yyyy-MM-dd") + "," + Convert.ToDateTime(DT2List[i]).ToString("yyyy-MM-dd") + ")";
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
        /// 取最大ID值
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        public int GetId(string userConn)
        {
            //string userConn = ConfigHelper.GetString("DBG6H");
            DataTable dataTable = null;
            DbHelper.Open(userConn);
            if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
            {
                dataTable = DbHelper.GetDataTable(userConn, string.Format(select_Id));
            }
            else
            {
                dataTable = DbHelper.GetDataTable(userConn, string.Format(select_Id));
            }
            DbHelper.Close(userConn);
            if (dataTable.Rows.Count == 0)
            {
                return 0;
            }
            else
            {
                int ID = int.Parse(dataTable.Rows[0]["ID"].ToString());
                return ID;
            }

        }

        /// <summary>
        /// 允许预备费抵扣
        /// </summary>
        /// <param name="userConn"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public int AddYBF(string userConn,string code)
        {
            var result = 0;
            var sqlStr= "UPDATE z_qtltzd SET ACCNO1='1' WHERE DEF_STR1='"+ code + "'";
            DbHelper.Open(userConn);
            result = DbHelper.ExecuteNonQuery(userConn, sqlStr);
            DbHelper.Close(userConn);
            return result;
        }

        //isnull(z_qtltzd.TZXH,0) FApprover,NVL(z_qtltzd.TZXH,0) FApprover,
        public readonly string SelectMst_Oracle = @"SELECT z_qtltzd.DEF_STR1 FProjCode,z_qtgkxm.MC FProjName, 
        z_qtltzd.DJRQ FDateofDeclaration, z_qtltzd.DWDM FDeclarationUnit,z_qtltzd.SHR3 FDeclarer, z_qtltzd.ZY FBudgetDept, 
        z_qtltzd.DEF_STR7 FDeclarationDept,z_qtltzd.DEF_STR4 FExpenseCategory,z_qtltzd.DEF_INT1 FDuration,
        z_qtltzd.DT1 FStartDate, z_qtltzd.DT2 FEndDate,z_qtltzd.DEF_INT2 FProjAttr,
        sum(NVL(z_qtltzd.PAY_JE,0)) FProjAmount,sum(NVL(z_qtltzd.PAY_JE,0)) FBudgetAmount,
        z_qtltzd.YEAR FYear
        FROM z_qtltzd 
        JOIN z_qtgkxm ON z_qtltzd.DEF_STR1=z_qtgkxm.DM 
        JOIN JJ_FXGL ON z_qtltzd.MXXM=JJ_FXGL.DM
        WHERE z_qtltzd.DEF_BZ1 IS NULL  
        GROUP BY z_qtltzd.DEF_STR1,z_qtgkxm.MC,z_qtltzd.DJRQ,z_qtltzd.DWDM,z_qtltzd.SHR3,z_qtltzd.ZY ,z_qtltzd.DEF_STR7,z_qtltzd.DEF_STR4,z_qtltzd.DEF_INT1,
        z_qtltzd.DT1,z_qtltzd.DT2,z_qtltzd.DEF_INT2,z_qtltzd.TZXH,z_qtltzd.YEAR";

        public readonly string SelectMst_sql = @"SELECT z_qtltzd.DEF_STR1 FProjCode,z_qtgkxm.MC FProjName, 
        z_qtltzd.DJRQ FDateofDeclaration, z_qtltzd.DWDM FDeclarationUnit,z_qtltzd.SHR3 FDeclarer, z_qtltzd.ZY FBudgetDept, 
        z_qtltzd.DEF_STR7 FDeclarationDept,z_qtltzd.DEF_STR4 FExpenseCategory,z_qtltzd.DEF_INT1 FDuration,
        z_qtltzd.DT1 FStartDate, z_qtltzd.DT2 FEndDate,z_qtltzd.DEF_INT2 FProjAttr,
        sum(isnull(z_qtltzd.PAY_JE,0)) FProjAmount,sum(isnull(z_qtltzd.PAY_JE,0)) FBudgetAmount,
        z_qtltzd.YEAR FYear
        FROM z_qtltzd 
        JOIN z_qtgkxm ON z_qtltzd.DEF_STR1=z_qtgkxm.DM 
        JOIN JJ_FXGL ON z_qtltzd.MXXM=JJ_FXGL.DM
        WHERE z_qtltzd.DEF_BZ1 IS NULL  
        GROUP BY z_qtltzd.DEF_STR1,z_qtgkxm.MC,z_qtltzd.DJRQ,z_qtltzd.DWDM,z_qtltzd.SHR3,z_qtltzd.ZY ,z_qtltzd.DEF_STR7,z_qtltzd.DEF_STR4,z_qtltzd.DEF_INT1,
        z_qtltzd.DT1,z_qtltzd.DT2,z_qtltzd.DEF_INT2,z_qtltzd.TZXH,z_qtltzd.YEAR";
        /// <summary>
        /// 获取老g6h预算数据主表
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        public DataTable GetOldMstList(string userConn)
        {
            DataTable dataTable = null;
            DbHelper.Open(userConn);
            if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
            {
                dataTable = DbHelper.GetDataTable(userConn, string.Format(SelectMst_Oracle));
            }
            else
            {
                dataTable = DbHelper.GetDataTable(userConn, string.Format(SelectMst_sql));
            }
            DbHelper.Close(userConn);
            return dataTable;
            //return DCHelper.DataTable2List<BudgetMstModel>(dataTable);
        }

        public readonly string SelectDtl_Oracle = @"SELECT 
        z_qtltzd.YSKM_DM FBudgetAccounts, 
        z_qtltzd.JFQD_DM FSourceOfFunds, NVL(z_qtltzd.PAY_JE,0) FAmount,NVL(z_qtltzd.PAY_JE,0) FBudgetAmount,
        z_qtltzd.BZ FOtherInstructions,
        z_qtltzd.DEF_STR1 FQtZcgnfl,
        z_qtltzd.DEF_STR2 FMeasUnit, z_qtltzd.MXXM FDtlCode,JJ_FXGL.MC FName,
        NVL(z_qtltzd.DEF_NUM1,0) FNum,NVL(z_qtltzd.DEF_NUM2,0) FPrice
        FROM z_qtltzd 
        JOIN z_qtgkxm ON z_qtltzd.DEF_STR1=z_qtgkxm.DM 
        JOIN JJ_FXGL ON z_qtltzd.MXXM=JJ_FXGL.DM
        WHERE z_qtltzd.DEF_BZ1 IS NULL  
        ";

        public readonly string SelectDtl_sql = @"SELECT 
        z_qtltzd.YSKM_DM FBudgetAccounts, 
        z_qtltzd.JFQD_DM FSourceOfFunds, isnull(z_qtltzd.PAY_JE,0) FAmount,isnull(z_qtltzd.PAY_JE,0) FBudgetAmount,
        z_qtltzd.BZ FOtherInstructions,
        z_qtltzd.DEF_STR1 FQtZcgnfl,
        z_qtltzd.DEF_STR2 FMeasUnit, z_qtltzd.MXXM FDtlCode,JJ_FXGL.MC FName,
        isnull(z_qtltzd.DEF_NUM1,0) FNum,isnull(z_qtltzd.DEF_NUM2,0) FPrice
        FROM z_qtltzd 
        JOIN z_qtgkxm ON z_qtltzd.DEF_STR1=z_qtgkxm.DM 
        JOIN JJ_FXGL ON z_qtltzd.MXXM=JJ_FXGL.DM
        WHERE z_qtltzd.DEF_BZ1 IS NULL  
        ";
        /// <summary>
        /// 获取老g6h预算数据明细表(FQtZcgnfl存的是主单据代码FProjCode)
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        public DataTable GetOldDtlList(string userConn)
        {
            DataTable dataTable = null;
            DbHelper.Open(userConn);
            if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
            {
                dataTable = DbHelper.GetDataTable(userConn, string.Format(SelectDtl_Oracle));
            }
            else
            {
                dataTable = DbHelper.GetDataTable(userConn, string.Format(SelectDtl_sql));
            }
            DbHelper.Close(userConn);
            return dataTable;
            //return DCHelper.DataTable2List<BudgetDtlBudgetDtlModel>(dataTable);
        }

        public readonly string SelectText = @"SELECT  z_qtltzd.INSTRUCT FProjOverview,
        z_qtltzd.XG_FILE FAnnualPerformGoal , z_qtltzd.DEF_STR1 FLTPerformGoal
        FROM z_qtltzd 
        JOIN z_qtgkxm ON z_qtltzd.DEF_STR1=z_qtgkxm.DM 
        JOIN JJ_FXGL ON z_qtltzd.MXXM=JJ_FXGL.DM
        WHERE z_qtltzd.DEF_BZ1 IS NULL  ";
        /// <summary>
        /// 获取老g6h预算数据text表(FLTPerformGoal存的是主单据代码FProjCode)
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        public DataTable GetOldTextList(string userConn)
        {
            DataTable dataTable = null;
            DbHelper.Open(userConn);
            
            dataTable = DbHelper.GetDataTable(userConn, string.Format(SelectText));
            
            DbHelper.Close(userConn);
            return dataTable;
            //return DCHelper.DataTable2List<BudgetDtlTextContentModel>(dataTable);
        }
        #endregion
    }
}

