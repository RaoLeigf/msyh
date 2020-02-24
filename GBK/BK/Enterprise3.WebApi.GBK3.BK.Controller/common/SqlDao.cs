using Enterprise3.Common.Base.Helpers;
using GData3.Common.Utils;
using NG3.Data.Service;
using SUP.Common.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Enterprise3.WebApi.GBK3.BK.Controller.Common
{
    /// <summary>
    /// sql查询
    /// </summary>
    public class SqlDao
    {
        /// <summary>
        /// 获取资金拨付主列表
        /// </summary>
        public readonly string select_payment_sql = "SELECT pay.f_year FYear,pay.f_code FCode,pay.f_name FName,pay.f_date FDate,pay.f_approval FApproval,pay.is_pay IsPay,pay.f_describe FDescribe,sum(isnull(pay.f_amount_total,0)) FAmountTotal"
                                                    + " FROM bk3_payment_mst pay"
                                                    + " WHERE 1=1 {0}"
                                                    + " GROUP BY pay.f_year,pay.f_code,pay.f_name,pay.f_date,pay.f_approval,pay.is_pay,pay.f_describe";
        /// <summary>
        /// 获取资金拨付主列表
        /// </summary>
        public readonly string select_payment_Oracle = "SELECT pay.f_year FYear,pay.f_code FCode,pay.f_name FName,pay.f_date FDate,pay.f_approval FApproval,pay.is_pay IsPay,pay.f_describe FDescribe,sum(nvl(pay.f_amount_total,0)) FAmountTotal"
                                                    + " FROM bk3_payment_mst pay"
                                                    + " WHERE 1=1 {0}"
                                                    + " GROUP BY pay.f_year,pay.f_code,pay.f_name,pay.f_date,pay.f_approval,pay.is_pay,pay.f_describe";
        /// <summary>
        /// 获取资金拨付主列表
        /// </summary>
        /// <param name="fPhid">部门主键</param>
        /// <returns></returns>
        public DataTable GetPaymentList(string fPhid)
        {
            /*
            string userConn = ConfigHelper.GetString("DBTG6H");
            DataTable dataTable = null;
            DbHelper.Open(userConn);

            if (!String.IsNullOrEmpty(fPhid))
            {
                fPhid = " and phid = " + fPhid + "";
            }
            if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
            {
                dataTable = DbHelper.GetDataTable(userConn, string.Format(select_payment_Oracle, fPhid));
            }
            else
            {
                dataTable = DbHelper.GetDataTable(userConn, string.Format(select_payment_sql, fPhid));
            }
            */
            DataTable dataTable = null;
            if (!String.IsNullOrEmpty(fPhid))
            {
                fPhid = " and phid = " + fPhid + "";
            }
            if (CommonUtils.IsOracleDB())
            {
                dataTable = DbHelper.GetDataTable( string.Format(select_payment_Oracle, fPhid));
            }
            else
            {
                dataTable = DbHelper.GetDataTable( string.Format(select_payment_sql, fPhid));
            }

            return dataTable;
        }
    }
}