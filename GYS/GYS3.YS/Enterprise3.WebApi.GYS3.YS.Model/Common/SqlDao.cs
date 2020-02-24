using Enterprise3.WebApi.GYS3.YS.Model.Response;
using GData3.Common.Utils;
using NG3.Data.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Enterprise3.WebApi.GYS3.YS.Model.Common
{
    /// <summary>
    /// 预算项目调整分析相关sql
    /// </summary>
    public class SqlDao
    {
        //sql语句
        private readonly string GetBudgetAdjustAnalyses_SQL =
                        @"SELECT mst.F_YEAR FYEAR,mst.PHID FProjPhId, mst.F_PROJCODE FPROJCODE, mst.F_PROJNAME FPROJNAME, mst.F_DECLARATIONUNIT FDECLARATIONUNIT,mst.F_DECLARATIONDEPT FDECLARATIONDEPT, mst.F_BUDGETDEPT FBUDGETDEPT,
                            mst.F_ACCOUNT FACCOUNT, mst.F_BUDGETAMOUNT FBUDGETAMOUNT, dtl.F_DTLCODE FDTLCODE,dtl.F_NAME FDTLNAME,dtl.F_BUDGETACCOUNTS FBUDGETACCOUNTS,
                            nvl(dtl.F_AMOUNT, 0) FAMOUNT,(CASE WHEN dtl.F_AMOUNTEDIT >= 0 THEN dtl.F_AMOUNTEDIT ELSE 0 END) FZAMOUNT,(CASE WHEN dtl.F_AMOUNTEDIT < 0 THEN dtl.F_AMOUNTEDIT ELSE 0 END) FJAMOUNT,
                            nvl(dtl.F_AMOUNTEDIT, 0) FAMOUNTEDIT, nvl(dtl.F_AMOUNTAFTEREDIT,0) FAMOUNTAFTEREDIT
                            FROM YS3_BUDGETMST mst 
                            LEFT JOIN YS3_BUDGETDTL_BUDGETDTL dtl ON mst.PHID = dtl.MST_PHID
                            WHERE mst.F_YEAR='{0}' AND mst.F_DECLARATIONUNIT= '{1}'  AND mst.F_LIFECYCLE= '0' AND mst.F_MIDYEARCHANGE = '0'
                            ORDER BY mst.F_DECLARATIONUNIT,mst.F_DECLARATIONDEPT, mst.F_BUDGETDEPT, mst.F_PROJCODE, dtl.F_DTLCODE";

        /// <summary>
        /// 根据年份与组织编码获取所有项目明细的预算分析数据
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="orgCode">组织编码</param>
        /// <returns></returns>
        public List<BudgetAdjustAnalyseModel> GetBudgetAdjustAnalyses(string year, string orgCode)
        {
            List<BudgetAdjustAnalyseModel> budgetAdjustAnalyses = new List<BudgetAdjustAnalyseModel>();
         
            DataTable dataTable = null;

            if (CommonUtils.IsOracleDB())
            {
                dataTable = DbHelper.GetDataTable(string.Format(GetBudgetAdjustAnalyses_SQL, year, orgCode));
            }
            else
            {
                dataTable = DbHelper.GetDataTable(string.Format(GetBudgetAdjustAnalyses_SQL, year, orgCode));
            }

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return budgetAdjustAnalyses;
            }
            else
            {
                budgetAdjustAnalyses = DCHelper.DataTable2List<BudgetAdjustAnalyseModel>(dataTable).ToList();
            }

            return budgetAdjustAnalyses;
        }

    }
}