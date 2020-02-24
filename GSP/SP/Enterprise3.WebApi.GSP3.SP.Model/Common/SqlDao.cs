using Enterprise3.Common.Base.Helpers;
using Enterprise3.WebApi.GSP3.SP.Model.Response;
using GData3.Common.Utils;
using GSP3.SP.Model.Domain;
using NG3.Data.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GSP3.SP.Model.Common
{
    /// <summary>
    /// 审批相关的sql语句
    /// </summary>
    public class SqlDao
    {

        /// <summary>
        /// 获取当前用户的审批记录
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="uid">用户id</param>
        /// <param name="bType">单据类型</param>
        /// <param name="orgIds">组织id集合</param>
        /// <param name="approval">审批状态</param>
        /// <param name="splx_phid">审批类型id</param>
        /// <returns></returns>
        public List<AppvalRecordVo> GetRecords2(string year, long uid, string bType, string orgIds, string approval, long splx_phid)
        {
            List<AppvalRecordVo> appvalRecords = null;
            bool isOralce = CommonUtils.IsOracleDB();
            string sql = GetUnDoRecordsSQL2(bType, isOralce);
            DataTable dataTable = null;
            DbHelper.Open();
            try
            {
                if (isOralce)
                {
                    dataTable = DbHelper.GetDataTable(string.Format(sql, uid, year, orgIds, approval, bType, splx_phid));
                }
                else
                {
                    dataTable = DbHelper.GetDataTable(string.Format(sql, uid, year, orgIds, approval, bType, splx_phid));
                }
            }
            catch (Exception e)
            {
                throw new Exception("执行SQL语句报错！");
            }
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return appvalRecords;
            }
            try
            {
                appvalRecords = DCHelper.DataTable2List<AppvalRecordVo>(dataTable).ToList();
            }
            catch (Exception e)
            {
                throw new Exception("DataTable转List报错！");
            }

            //计算停留时间
            try
            {
                foreach (AppvalRecordVo model in appvalRecords)
                {
                    if (model.FSendDate != null)
                    {
                        model.StopHour = Math.Round((DateTime.Now - model.FSendDate.Value).TotalHours, 2);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("计算停留时长报错！");
            }

            return appvalRecords;
        }
        /// <summary>
        /// 根据类型与数据库类型获取不同的sql语句
        /// </summary>
        /// <param name="bType"></param>
        /// <param name="isOracle"></param>
        /// <returns></returns>
        private string GetUnDoRecordsSQL2(string bType, bool isOracle)
        {
            string sql = "";
            if (string.IsNullOrEmpty(bType))
            {
                return sql;
            }

            switch (bType)
            {
                case "001":
                    sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,bk.f_orgphid OrgId,bk.f_orgcode OrgCode,bk.f_orgname OrgName,bk.f_code BNum,bk.F_DEPPHID DepId,bk.F_DEPCODE DepCode, bk.F_DEPNAME DepName,
                            bk.f_name BName,bk.f_amount_total BAccount,bk.f_date BDate,isnull(bk.f_approval,0) BStatus,bk.f_describe BDescribe,sp.f_billtype BBilltype,bk.F_BUDCODE BudgetDept, bk.F_BUDNAME BudgetName
                            from sp3_appval_record sp
                            left join BK3_PAYMENT_MST bk on sp.refbill_phid=bk.phid
                            where sp.opera_phid='{0}' and bk.f_year='{1}' and bk.f_orgphid in ({2}) and sp.f_approval in ('{3}') and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                    break;
                case "002":
                    sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,bk.f_orgphid OrgId,bk.f_orgcode OrgCode,bk.f_orgname OrgName,bk.f_code BNum,
                            bk.f_name BName,bk.f_amount_total BAccount,bk.f_date BDate,isnull(gk.f_approval,0) BStatus,bk.f_describe BDescribe,gk.f_code PayNum,gk.f_amount_total PayAccount,sp.f_billtype BBilltype
                            from sp3_appval_record sp
                            LEFT JOIN gk3_payment_mst gk ON sp.refbill_phid=gk.phid
                            left join bK3_PAYMENT_MST bk on gk.REFBILL_PHID=bk.phid
                            where sp.opera_phid='{0}' and bk.f_year='{1}' and bk.f_orgphid in ({2}) and sp.f_approval in ('{3}') and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                    break;
                case "003":
                    sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,ys.F_DECLARATIONUNIT OrgCode,ys.F_PROJCODE BNum,ys.F_DECLARATIONDEPT DepCode,
                            ys.F_PROJNAME BName,ys.F_PROJAMOUNT BAccount,ys.F_DATEOFDECLARATION BDate,isnull(ys.F_APPROVESTATUS, '0') BStatus,sp.f_billtype BBilltype,
                            ys.F_PERFORMTYPE PerformType, ys.F_TYPE djType,ys.F_PROJSTATUS ProjStatus,ys.F_BUDGETDEPT BudgetDept,ys.F_DECLARER Declarer, ys.F_VERNO djVerno  
                            from sp3_appval_record sp
                            left join YS3_EXPENSEMST ys on sp.refbill_phid=ys.phid
                            where sp.opera_phid='{0}' and ys.f_year='{1}' and ys.F_DECLARATIONDEPT in ({2}) and sp.f_approval in ('{3}') and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                    break;
                case "004":
                case "005":
                    sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,xm.F_DECLARATIONUNIT OrgCode,xm.F_PROJCODE BNum,xm.F_DECLARATIONDEPT DepCode,
                            xm.F_PROJNAME BName,xm.F_PROJAMOUNT BAccount,xm.F_DATEOFDECLARATION BDate,isnull(xm.F_APPROVESTATUS, '0') BStatus,sp.f_billtype BBilltype,
                            xm.F_IFPURCHASE IfPurchase, xm.F_PERFORMTYPE PerformType, xm.F_TYPE djType,xm.F_PROJSTATUS ProjStatus,xm.F_BUDGETDEPT BudgetDept,
                            xm.F_DECLARER Declarer 
                            from sp3_appval_record sp
                            left join XM3_PROJECTMST xm on sp.refbill_phid=xm.phid
                            where sp.opera_phid='{0}' and xm.f_year='{1}' and xm.F_DECLARATIONDEPT in ({2}) and sp.f_approval in ('{3}') and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                    break;
                case "006":
                case "007":
                case "008":
                case "009":
                    sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,ys.F_DECLARATIONUNIT OrgCode,ys.F_PROJCODE BNum,ys.F_DECLARATIONDEPT DepCode,
                            ys.F_PROJNAME BName,ys.F_PROJAMOUNT BAccount,ys.F_DATEOFDECLARATION BDate,isnull(ys.F_APPROVESTATUS, '0') BStatus,sp.f_billtype BBilltype,
                            ys.F_PERFORMTYPE PerformType, ys.F_TYPE djType,ys.F_PROJSTATUS ProjStatus,ys.F_BUDGETDEPT BudgetDept,ys.F_DECLARER Declarer, ys.F_VERNO djVerno  
                            from sp3_appval_record sp
                            left join YS3_BUDGETMST ys on sp.refbill_phid=ys.phid
                            where sp.opera_phid='{0}' and ys.f_year='{1}' and ys.F_DECLARATIONDEPT in ({2}) and sp.f_approval in ('{3}') and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                    break;
                case "010":
                    sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,ys.f_orgid OrgId,ys.f_orgcode OrgCode,ys.F_DeclareTime BDate, isnull(ys.F_DeclareAmount, 0) BAccount,ys.F_YEAR Byear,
                            sp.f_billtype BBilltype
                            from sp3_appval_record sp
                            left join ys3_incomemst ys on sp.refbill_phid=ys.phid
                            where sp.opera_phid='{0}' and ys.f_year='{1}' and ys.f_orgid in ({2}) and sp.f_approval in ('{3}') and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                    break;
                default:
                    break;
            }

            if (isOracle)
            {
                switch (bType)
                {
                    case "001":
                        sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,bk.f_orgphid OrgId,bk.f_orgcode OrgCode,bk.f_orgname OrgName,bk.f_code BNum,bk.F_DEPPHID DepId,bk.F_DEPCODE DepCode, bk.F_DEPNAME DepName,
                            bk.f_name BName,bk.f_amount_total BAccount,bk.f_date BDate,bk.f_approval BStatus,bk.f_describe BDescribe,sp.f_billtype BBilltype,bk.F_BUDCODE BudgetDept, bk.F_BUDNAME BudgetName
                            from sp3_appval_record sp
                            left join BK3_PAYMENT_MST bk on sp.refbill_phid=bk.phid
                            where sp.opera_phid='{0}' and bk.f_year='{1}' and bk.f_orgphid in ({2}) and sp.f_approval in ('{3}') and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                        break;
                    case "002":
                        sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,bk.f_orgphid OrgId,bk.f_orgcode OrgCode,bk.f_orgname OrgName,bk.f_code BNum,
                            bk.f_name BName,bk.f_amount_total BAccount,bk.f_date BDate,gk.f_approval BStatus,bk.f_describe BDescribe,gk.f_code PayNum,gk.f_amount_total PayAccount,sp.f_billtype BBilltype
                            from sp3_appval_record sp
                            LEFT JOIN gk3_payment_mst gk ON sp.refbill_phid=gk.phid
                            left join bK3_PAYMENT_MST bk on gk.REFBILL_PHID=bk.phid
                            where sp.opera_phid='{0}' and bk.f_year='{1}' and bk.f_orgphid in ({2}) and sp.f_approval in ('{3}') and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                        break;
                    case "003":
                        sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,ys.F_DECLARATIONUNIT OrgCode,ys.F_PROJCODE BNum,ys.F_DECLARATIONDEPT DepCode,
                            ys.F_PROJNAME BName,ys.F_PROJAMOUNT BAccount,ys.F_DATEOFDECLARATION BDate,nvl(ys.F_APPROVESTATUS, '0') BStatus,sp.f_billtype BBilltype,
                            ys.F_PERFORMTYPE PerformType, ys.F_TYPE djType,ys.F_PROJSTATUS ProjStatus,ys.F_BUDGETDEPT BudgetDept,ys.F_DECLARER Declarer, ys.F_VERNO djVerno  
                            from sp3_appval_record sp
                            left join YS3_EXPENSEMST ys on sp.refbill_phid=ys.phid
                            where sp.opera_phid='{0}' and ys.f_year='{1}' and ys.F_DECLARATIONDEPT in ({2}) and sp.f_approval in ('{3}') and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                        break;
                    case "004":
                    case "005":
                        sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,xm.F_DECLARATIONUNIT OrgCode,xm.F_PROJCODE BNum,xm.F_DECLARATIONDEPT DepCode,
                            xm.F_PROJNAME BName,xm.F_PROJAMOUNT BAccount,xm.F_DATEOFDECLARATION BDate,nvl(xm.F_APPROVESTATUS, '0') BStatus,sp.f_billtype BBilltype,
                            xm.F_IFPURCHASE IfPurchase, xm.F_PERFORMTYPE PerformType, xm.F_TYPE djType,xm.F_PROJSTATUS ProjStatus,xm.F_BUDGETDEPT BudgetDept,
                            xm.F_DECLARER Declarer  
                            from sp3_appval_record sp
                            left join XM3_PROJECTMST xm on sp.refbill_phid=xm.phid
                            where sp.opera_phid='{0}' and xm.f_year='{1}' and xm.F_DECLARATIONDEPT in ({2}) and sp.f_approval in ('{3}') and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                        break;
                    case "006":
                    case "007":
                    case "008":
                    case "009":
                        sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,ys.F_DECLARATIONUNIT OrgCode,ys.F_PROJCODE BNum,ys.F_DECLARATIONDEPT DepCode,
                            ys.F_PROJNAME BName,ys.F_PROJAMOUNT BAccount,ys.F_DATEOFDECLARATION BDate,nvl(ys.F_APPROVESTATUS, '0') BStatus,sp.f_billtype BBilltype,
                            ys.F_PERFORMTYPE PerformType, ys.F_TYPE djType,ys.F_PROJSTATUS ProjStatus,ys.F_BUDGETDEPT BudgetDept,ys.F_DECLARER Declarer, ys.F_VERNO djVerno  
                            from sp3_appval_record sp
                            left join YS3_BUDGETMST ys on sp.refbill_phid=ys.phid
                            where sp.opera_phid='{0}' and ys.f_year='{1}' and ys.F_DECLARATIONDEPT in ({2}) and sp.f_approval in ('{3}') and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                        break;
                    case "010":
                        sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,ys.f_orgid OrgId,ys.f_orgcode OrgCode,ys.F_DeclareTime BDate, nvl(ys.F_DeclareAmount,0) BAccount,ys.F_YEAR Byear,
                            sp.f_billtype BBilltype
                            from sp3_appval_record sp
                            left join ys3_incomemst ys on sp.refbill_phid=ys.phid
                            where sp.opera_phid='{0}' and ys.f_year='{1}' and ys.f_orgid in ({2}) and sp.f_approval in ('{3}') and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                        break;
                    default:
                        break;
                }
            }

            return sql;
        }

        /// <summary>
        /// 获取当前用户的审批记录
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="uid">用户id</param>
        /// <param name="bType">单据类型</param>
        /// <param name="orgcode">组织编码</param>
        /// <param name="approval">审批状态</param>
        /// <param name="splx_phid">审批类型id</param>
        /// <returns></returns>
        public List<AppvalRecordVo> GetRecords(string year,long uid,string bType,string orgcode,string approval,long splx_phid) {

            List<AppvalRecordVo> appvalRecords = null;
            /*
            string userConn = ConfigHelper.GetString("DBTG6H");

            bool isOralce = false;
            if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0) {
                isOralce = true;
            }
            string sql = GetUnDoRecordsSQL(bType, isOralce);

            
            DataTable dataTable = null;

            DbHelper.Open(userConn);
            try
            {
                if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    dataTable = DbHelper.GetDataTable(userConn, string.Format(sql, uid, year, orgcode, approval,splx_phid));
                }
                else
                {
                    dataTable = DbHelper.GetDataTable(userConn, string.Format(sql, uid, year, orgcode, approval, splx_phid));
                }
            }
            catch (Exception e)
            {
                throw new Exception("执行SQL语句报错！");
            }
            */
            bool isOralce = CommonUtils.IsOracleDB();
 
            string sql = GetUnDoRecordsSQL(bType, isOralce);


            DataTable dataTable = null;

            DbHelper.Open();
            try
            {
                if (isOralce)
                {
                    dataTable = DbHelper.GetDataTable( string.Format(sql, uid, year, orgcode, approval, bType, splx_phid));
                }
                else
                {
                    dataTable = DbHelper.GetDataTable( string.Format(sql, uid, year, orgcode, approval, bType, splx_phid));
                }
            }
            catch (Exception e)
            {
                throw new Exception("执行SQL语句报错！");
            }


            if (dataTable == null || dataTable.Rows.Count == 0) {
                return appvalRecords;
            }
            try
            {
                appvalRecords = DCHelper.DataTable2List<AppvalRecordVo>(dataTable).ToList();
            }
            catch (Exception e) {
                throw new Exception("DataTable转List报错！" + e.Message);
            }

            //计算停留时间
            try
            {
                foreach (AppvalRecordVo model in appvalRecords)
                {
                    if (model.FSendDate != null)
                    {
                        model.StopHour = Math.Round((DateTime.Now - model.FSendDate.Value).TotalHours, 2);
                    }
                }
            }
            catch (Exception e) {
                throw new Exception("计算停留时长报错！");
            }

            return appvalRecords;
        }

        private string GetUnDoRecordsSQL(string bType,bool isOracle) {
            string sql = "";
            if (string.IsNullOrEmpty(bType)) {
                return sql;
            }

            switch (bType) {
                case "001":
                    sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,bk.f_orgphid OrgId,bk.f_orgcode OrgCode,bk.f_orgname OrgName,bk.f_code BNum,bk.F_DEPPHID DepId,bk.F_DEPCODE DepCode, bk.F_DEPNAME DepName,
                            bk.f_name BName,bk.f_amount_total BAccount,bk.f_date BDate,isnull(bk.f_approval,0) BStatus,bk.f_describe BDescribe,sp.f_billtype BBilltype
                            from sp3_appval_record sp
                            left join BK3_PAYMENT_MST bk on sp.refbill_phid=bk.phid
                            where sp.opera_phid='{0}' and bk.f_year='{1}' and bk.f_orgcode like '{2}%' and sp.f_approval in ('{3}') and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                    break;
                case "002":
                    sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,bk.f_orgphid OrgId,bk.f_orgcode OrgCode,bk.f_orgname OrgName,bk.f_code BNum,
                            bk.f_name BName,bk.f_amount_total BAccount,bk.f_date BDate,isnull(gk.f_approval,0) BStatus,bk.f_describe BDescribe,gk.f_code PayNum,gk.f_amount_total PayAccount,sp.f_billtype BBilltype
                            from sp3_appval_record sp
                            LEFT JOIN gk3_payment_mst gk ON sp.refbill_phid=gk.phid
                            left join bK3_PAYMENT_MST bk on gk.REFBILL_PHID=bk.phid
                            where sp.opera_phid={0} and bk.f_year='{1}' and bk.f_orgcode like '{2}%' and sp.f_approval in ({3}) and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                    break;
                case "003":
                    sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,ys.F_DECLARATIONUNIT OrgCode,ys.F_PROJCODE BNum,ys.F_DECLARATIONDEPT DepCode,
                            ys.F_PROJNAME BName,ys.F_PROJAMOUNT BAccount,ys.F_DATEOFDECLARATION BDate,isnull(ys.F_APPROVESTATUS, '0') BStatus,sp.f_billtype BBilltype,
                            ys.F_PERFORMTYPE PerformType, ys.F_TYPE djType,ys.F_PROJSTATUS ProjStatus,ys.F_BUDGETDEPT BudgetDept,ys.F_DECLARER Declarer, ys.F_VERNO djVerno  
                            from sp3_appval_record sp
                            left join YS3_EXPENSEMST ys on sp.refbill_phid=ys.phid
                            where sp.opera_phid='{0}' and ys.f_year='{1}' and ys.F_DECLARATIONUNIT like '{2}%' and sp.f_approval in ('{3}') and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                    break;
                case "004":
                case "005":
                    sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,xm.F_DECLARATIONUNIT OrgCode,xm.F_PROJCODE BNum,xm.F_DECLARATIONDEPT DepCode,
                            xm.F_PROJNAME BName,xm.F_PROJAMOUNT BAccount,xm.F_DATEOFDECLARATION BDate,isnull(xm.F_APPROVESTATUS, '0') BStatus,sp.f_billtype BBilltype,
                            xm.F_IFPURCHASE IfPurchase, xm.F_PERFORMTYPE PerformType, xm.F_TYPE djType,xm.F_PROJSTATUS ProjStatus,xm.F_BUDGETDEPT BudgetDept,
                            xm.F_DECLARER Declarer  
                            from sp3_appval_record sp
                            left join XM3_PROJECTMST xm on sp.refbill_phid=xm.phid
                            where sp.opera_phid='{0}' and xm.f_year='{1}' and xm.F_DECLARATIONUNIT like '{2}%' and sp.f_approval in ('{3}') and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                    break;
                case "006":
                case "007":
                case "008":
                case "009":
                    sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,ys.F_DECLARATIONUNIT OrgCode,ys.F_PROJCODE BNum,ys.F_DECLARATIONDEPT DepCode,
                            ys.F_PROJNAME BName,ys.F_PROJAMOUNT BAccount,ys.F_DATEOFDECLARATION BDate,isnull(ys.F_APPROVESTATUS, '0') BStatus,sp.f_billtype BBilltype,
                            ys.F_PERFORMTYPE PerformType, ys.F_TYPE djType,ys.F_PROJSTATUS ProjStatus,ys.F_BUDGETDEPT BudgetDept,ys.F_DECLARER Declarer, ys.F_VERNO djVerno  
                            from sp3_appval_record sp
                            left join YS3_BUDGETMST ys on sp.refbill_phid=ys.phid
                            where sp.opera_phid='{0}' and ys.f_year='{1}' and ys.F_DECLARATIONUNIT like '{2}%' and sp.f_approval in ('{3}') and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                    break;
                case "010":
                    sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,ys.f_orgid OrgId,ys.f_orgcode OrgCode,ys.F_DeclareTime BDate, isnull(ys.F_DeclareAmount, 0) BAccount,ys.F_YEAR Byear,
                            sp.f_billtype BBilltype
                            from sp3_appval_record sp
                            left join ys3_incomemst ys on sp.refbill_phid=ys.phid
                            where sp.opera_phid='{0}' and ys.f_year='{1}' and ys.f_orgcode like '{2}%' and sp.f_approval in ('{3}') and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                    break;
                default:
                    break;
            }

            if (isOracle) {
                switch (bType)
                {
                    case "001":
                        sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,bk.f_orgphid OrgId,bk.f_orgcode OrgCode,bk.f_orgname OrgName,bk.f_code BNum,bk.F_DEPPHID DepId,bk.F_DEPCODE DepCode, bk.F_DEPNAME DepName,
                            bk.f_name BName,bk.f_amount_total BAccount,bk.f_date BDate,bk.f_approval BStatus,bk.f_describe BDescribe,sp.f_billtype BBilltype
                            from sp3_appval_record sp
                            left join BK3_PAYMENT_MST bk on sp.refbill_phid=bk.phid
                            where sp.opera_phid='{0}' and bk.f_year='{1}' and bk.f_orgcode like '{2}%' and sp.f_approval in ({3}) and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                        break;
                    case "002":
                        sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,bk.f_orgphid OrgId,bk.f_orgcode OrgCode,bk.f_orgname OrgName,bk.f_code BNum,
                            bk.f_name BName,bk.f_amount_total BAccount,bk.f_date BDate,gk.f_approval BStatus,bk.f_describe BDescribe,gk.f_code PayNum,gk.f_amount_total PayAccount,sp.f_billtype BBilltype
                            from sp3_appval_record sp
                            LEFT JOIN gk3_payment_mst gk ON sp.refbill_phid=gk.phid
                            left join bK3_PAYMENT_MST bk on gk.REFBILL_PHID=bk.phid
                            where sp.opera_phid='{0}' and bk.f_year='{1}' and bk.f_orgcode like '{2}%' and sp.f_approval in ({3}) and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                        break;
                    case "003":
                        sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                                sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                                sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                                sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,ys.F_DECLARATIONUNIT OrgCode,ys.F_PROJCODE BNum,ys.F_DECLARATIONDEPT DepCode,
                                ys.F_PROJNAME BName,ys.F_PROJAMOUNT BAccount,ys.F_DATEOFDECLARATION BDate,nvl(ys.F_APPROVESTATUS, '0') BStatus,sp.f_billtype BBilltype,
                                ys.F_PERFORMTYPE PerformType, ys.F_TYPE djType,ys.F_PROJSTATUS ProjStatus,ys.F_BUDGETDEPT BudgetDept,ys.F_DECLARER Declarer, ys.F_VERNO djVerno 
                                from sp3_appval_record sp
                                left join YS3_EXPENSEMST ys on sp.refbill_phid=ys.phid
                                where sp.opera_phid='{0}' and ys.f_year='{1}' and ys.F_DECLARATIONUNIT like '{2}%' and sp.f_approval in ('{3}') and sp.f_billtype='{4}'
                                and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                                order by sp.f_date desc";
                        break;
                    case "004":
                    case "005":
                        sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,xm.F_DECLARATIONUNIT OrgCode,xm.F_PROJCODE BNum,xm.F_DECLARATIONDEPT DepCode,
                            xm.F_PROJNAME BName,xm.F_PROJAMOUNT BAccount,xm.F_DATEOFDECLARATION BDate,nvl(xm.F_APPROVESTATUS, '0') BStatus,sp.f_billtype BBilltype,
                            xm.F_IFPURCHASE IfPurchase, xm.F_PERFORMTYPE PerformType, xm.F_TYPE djType,xm.F_PROJSTATUS ProjStatus,xm.F_BUDGETDEPT BudgetDept,
                            xm.F_DECLARER Declarer 
                            from sp3_appval_record sp
                            left join XM3_PROJECTMST xm on sp.refbill_phid=xm.phid
                            where sp.opera_phid='{0}' and xm.f_year='{1}' and xm.F_DECLARATIONUNIT like '{2}%' and sp.f_approval in ({3}) and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                        break;
                    case "006":
                    case "007":
                    case "008":
                    case "009":
                        sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,ys.F_DECLARATIONUNIT OrgCode,ys.F_PROJCODE BNum,ys.F_DECLARATIONDEPT DepCode,
                            ys.F_PROJNAME BName,ys.F_PROJAMOUNT BAccount,ys.F_DATEOFDECLARATION BDate,nvl(ys.F_APPROVESTATUS, '0') BStatus,sp.f_billtype BBilltype,
                            ys.F_PERFORMTYPE PerformType, ys.F_TYPE djType,ys.F_PROJSTATUS ProjStatus,ys.F_BUDGETDEPT BudgetDept,ys.F_DECLARER Declarer, ys.F_VERNO djVerno 
                            from sp3_appval_record sp
                            left join YS3_BUDGETMST ys on sp.refbill_phid=ys.phid
                            where sp.opera_phid='{0}' and ys.f_year='{1}' and ys.F_DECLARATIONUNIT like '{2}%' and sp.f_approval in ('{3}') and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                        break;
                    case "010":
                        sql = @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                            sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                            sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,sp.f_billtype FBilltype,
                            sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,ys.f_orgid OrgId,ys.f_orgcode OrgCode,ys.F_DeclareTime BDate, nvl(ys.F_DeclareAmount,0) BAccount,ys.F_YEAR Byear,
                            sp.f_billtype BBilltype
                            from sp3_appval_record sp
                            left join ys3_incomemst ys on sp.refbill_phid=ys.phid
                            where sp.opera_phid='{0}' and ys.f_year='{1}' and ys.f_orgcode like '{2}%' and sp.f_approval in ('{3}') and sp.f_billtype='{4}'
                            and sp.proc_phid in (select phid from sp3_appval_proc where splx_phid='{5}') and sp.POST_PHID !=0
                            order by sp.f_date desc";
                        break;
                    default:
                        break;
                }
            }

            return sql;
        }

        private readonly string FindByRelId_SQL =
                                @"select sp.phid PhId,sp.refbill_phid RefbillPhid,sp.proc_phid ProcPhid,sp.post_phid PostPhid,sp.opera_phid OperaPhid,
                                sp.operator_code OperatorCode,sp.f_seq FSeq,sp.f_send_date FSendDate,sp.f_date FDate,sp.f_approval FApproval,
                                sp.f_opinion FOpinion,sp.creator Creator,sp.editor Editor,sp.cur_orgid CurOrgId,sp.ng_insert_dt NgInsertDt,
                                sp.ng_update_dt NgUpdateDt,sp.ng_record_ver NgRecordVer,us.username OperaName,sp.f_billtype FBilltype
                                from sp3_appval_record sp
                                left join fg3_user us on sp.opera_phid=us.phid
                                where sp.refbill_phid={0} and sp.f_billtype='{1}'";

        /// <summary>
        /// 根据关联单据id,单据类型查找审批记录
        /// </summary>
        /// <param name="phid">单据id</param>
        /// <param name="billType">单据类型</param>
        /// <returns></returns>
        public List<GAppvalRecordModel> FindByRelId(long phid,string billType) {

            List<GAppvalRecordModel> recordModels = null;

            if (phid == 0) {
                return new List<GAppvalRecordModel>();
            }

            /*
            string userConn = ConfigHelper.GetString("DBTG6H");

            DataTable dataTable = null;

            DbHelper.Open(userConn);
            try
            {
                if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    dataTable = DbHelper.GetDataTable(userConn, string.Format(FindByRelId_SQL, phid, billType));
                }
                else
                {
                    dataTable = DbHelper.GetDataTable(userConn, string.Format(FindByRelId_SQL, phid, billType));
                }
            }
            catch (Exception e) {
                throw new Exception("执行SQL语句报错！");
            }

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return recordModels;
            }
            try
            {
                recordModels = DCHelper.DataTable2List<GAppvalRecordModel>(dataTable).ToList();
            }
            catch (Exception e)
            {
                throw new Exception("DataTable转List报错！");
            }
            */
            DataTable dataTable = null;
            try
            {
                if (CommonUtils.IsOracleDB())
                {
                    dataTable = DbHelper.GetDataTable( string.Format(FindByRelId_SQL, phid, billType));
                }
                else
                {
                    dataTable = DbHelper.GetDataTable( string.Format(FindByRelId_SQL, phid, billType));
                }
            }
            catch (Exception e)
            {
                throw new Exception("执行SQL语句报错！");
            }

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return recordModels;
            }
            try
            {
                recordModels = DCHelper.DataTable2List<GAppvalRecordModel>(dataTable).ToList();
            }
            catch (Exception e)
            {
                throw new Exception("DataTable转List报错！");
            }

            return recordModels;
        }


        private readonly string FindAppvalPostByProcID_SQL =
                                @"select b.phid PhId,b.org_phid OrgPhid,b.org_code OrgCode,b.f_code FCode,b.f_name FName,b.f_describe FDescribe,
                                b.f_enable FEnable,b.creator Creator,b.editor Editor,b.cur_orgid CurOrgId,b.ng_insert_dt NgInsertDt,
                                b.ng_update_dt NgUpdateDt,b.ng_record_ver NgRecordVer,a.f_seq Seq,a.f_mode FMode
                                from sp3_appval_proc4post a
                                left join sp3_appval_post b on a.post_phid=b.phid
                                where a.proc_phid={0} ORDER BY a.f_seq";

        /// <summary>
        /// 根据审批流程的id查找审批岗位
        /// </summary>
        /// <param name="phid">审批岗位id</param>
        /// <returns></returns>
        public List<GAppvalPostModel> FindAppvalPostByProcID(long phid) {

            List<GAppvalPostModel> postModels = null;

            if (phid == 0) {
                return new List<GAppvalPostModel>();
            }

            /*
            string userConn = ConfigHelper.GetString("DBTG6H");

            DataTable dataTable = null;

            DbHelper.Open(userConn);
            try
            {
                if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    dataTable = DbHelper.GetDataTable(userConn, string.Format(FindAppvalPostByProcID_SQL, phid));
                }
                else
                {
                    dataTable = DbHelper.GetDataTable(userConn, string.Format(FindAppvalPostByProcID_SQL, phid));
                }
            }
            catch (Exception e)
            {
                throw new Exception("执行SQL语句报错！");
            }
            */
            DataTable dataTable = null;
            try
            {
                if (CommonUtils.IsOracleDB())
                {
                    dataTable = DbHelper.GetDataTable( string.Format(FindAppvalPostByProcID_SQL, phid));
                }
                else
                {
                    dataTable = DbHelper.GetDataTable( string.Format(FindAppvalPostByProcID_SQL, phid));
                }
            }
            catch (Exception e)
            {
                throw new Exception("执行SQL语句报错！");
            }


            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return postModels;
            }
            try
            {
                postModels = DCHelper.DataTable2List<GAppvalPostModel>(dataTable).ToList();
            }
            catch (Exception e)
            {
                throw new Exception("DataTable转List报错！");
            }

            return postModels;

        }

        private readonly string GetOperatorsByPostID_SQL =
                                @"select op.phid PhId,op.post_phid PostPhid,op.operator_phid OperatorPhid,op.operator_code OperatorCode,
                                op.f_seq FSeq,op.creator Creator,op.editor Editor,op.cur_orgid CurOrgId,op.ng_insert_dt NgInsertDt,
                                op.ng_update_dt NgUpdateDt,op.ng_record_ver NgRecordVer,us.username OperatorName
                                from sp3_appval_post4oper op
                                left join fg3_user us on op.operator_phid=us.phid
                                where op.post_phid={0}";

        /// <summary>
        /// 根据岗位id查找所有的操作员
        /// </summary>
        /// <param name="postId">岗位id</param>
        /// <returns></returns>
        public List<GAppvalPost4OperModel> GetOperatorsByPostID(long postId) {
            List<GAppvalPost4OperModel> operModels = null;

            if (postId == 0) {
                return new List<GAppvalPost4OperModel>();
            }

            /*
            string userConn = ConfigHelper.GetString("DBTG6H");

            DataTable dataTable = null;

            DbHelper.Open(userConn);
            try
            {
                if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    dataTable = DbHelper.GetDataTable(userConn, string.Format(GetOperatorsByPostID_SQL, postId));
                }
                else
                {
                    dataTable = DbHelper.GetDataTable(userConn, string.Format(GetOperatorsByPostID_SQL, postId));
                }
            }
            catch (Exception e)
            {
                throw new Exception("执行SQL语句报错！");
            }
            */
            DataTable dataTable = null;
            try
            {
                if (CommonUtils.IsOracleDB())
                {
                    dataTable = DbHelper.GetDataTable( string.Format(GetOperatorsByPostID_SQL, postId));
                }
                else
                {
                    dataTable = DbHelper.GetDataTable( string.Format(GetOperatorsByPostID_SQL, postId));
                }
            }
            catch (Exception e)
            {
                throw new Exception("执行SQL语句报错！");
            }

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return operModels;
            }
            try
            {
                operModels = DCHelper.DataTable2List<GAppvalPost4OperModel>(dataTable).ToList();
            }
            catch (Exception e)
            {
                throw new Exception("DataTable转List报错！");
            }

            return operModels;
        }

        private readonly string ProcIsUsed_SQL = @"SELECT * FROM sp3_appval_record WHERE proc_phid IN 
                                                (SELECT phid FROM sp3_appval_proc WHERE splx_phid={0})";

        /// <summary>
        /// 判断审批流程是否被引用
        /// </summary>
        /// <param name="splx_phid">审批类型id</param>
        /// <returns></returns>
        public DataTable ProcIsUsed(long splx_phid) {
            /*
            string userConn = ConfigHelper.GetString("DBTG6H");

            DataTable dataTable = null;

            DbHelper.Open(userConn);
            try
            {
                if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    dataTable = DbHelper.GetDataTable(userConn, string.Format(ProcIsUsed_SQL, splx_phid));
                }
                else
                {
                    dataTable = DbHelper.GetDataTable(userConn, string.Format(ProcIsUsed_SQL, splx_phid));
                }
            }
            catch (Exception e)
            {
                throw new Exception("执行SQL语句报错！");
            }
            */
   
            DataTable dataTable = null;

            DbHelper.Open();
            try
            {
                if (CommonUtils.IsOracleDB())
                {
                    dataTable = DbHelper.GetDataTable( string.Format(ProcIsUsed_SQL, splx_phid));
                }
                else
                {
                    dataTable = DbHelper.GetDataTable( string.Format(ProcIsUsed_SQL, splx_phid));
                }
            }
            catch (Exception e)
            {
                throw new Exception("执行SQL语句报错！");
            }

            return dataTable;
        }


        private readonly string GetAppvalProc_SQL = @"SELECT p.phid PhId,p.org_phid OrgPhid,p.org_code OrgCode,p.f_code FCode,p.f_name FName,p.f_billtype FBilltype,
                                                    p.f_enable FEnable,p.f_describe FDescribe,p.splx_phid SPLXPhid,p.splx_code SPLXCode,org.oname OrgName,
                                                    p.creator creator,p.editor editor,p.cur_orgid CurOrgId,p.ng_insert_dt NgInsertDt,p.ng_update_dt NgUpdateDt,p.ng_record_ver NgRecordVer,p.issystem IsSystem
                                                    FROM sp3_appval_proc p
                                                    LEFT JOIN fg_orglist org ON org.phid=p.org_phid
                                                    WHERE p.org_phid in ({0}) AND p.f_billtype='{1}' {2}
                                                    ORDER BY p.issystem desc, p.f_code asc";

        /// <summary>
        /// 根据组织id，单据类型，审批类型获取所有的审批流程
        /// </summary>
        /// <param name="orgid">组织id</param>
        /// <param name="bType">单据类型</param>
        /// <param name="splx_phid">审批类型id</param>
        /// <returns></returns>
        public DataTable GetAppvalProc(string orgid, string bType, long splx_phid) {

            /*
            string userConn = ConfigHelper.GetString("DBTG6H");

            DataTable dataTable = null;

            string whereSPLX = "";
            if (splx_phid > 0) {
                whereSPLX = " AND p.splx_phid =" + splx_phid;
            }

            DbHelper.Open(userConn);
            try
            {
                if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    dataTable = DbHelper.GetDataTable(userConn, string.Format(GetAppvalProc_SQL, orgid, bType, whereSPLX));
                }
                else
                {
                    dataTable = DbHelper.GetDataTable(userConn, string.Format(GetAppvalProc_SQL, orgid, bType, whereSPLX));
                }
            }
            catch (Exception e)
            {
                throw new Exception("执行SQL语句报错！");
            }
            */
            DataTable dataTable = null;
            string whereSPLX = "";
            if (splx_phid > 0)
            {
                whereSPLX = " AND p.splx_phid =" + splx_phid;
            }
            
            try
            {
                if (CommonUtils.IsOracleDB())
                {                    
                    dataTable = DbHelper.GetDataTable(string.Format(GetAppvalProc_SQL, orgid, bType, whereSPLX));
                }
                else
                {
                    //dataTable = DbHelper.GetDataTable(userConn, string.Format(GetAppvalProc_SQL, orgid, bType, whereSPLX));
                    dataTable = DbHelper.GetDataTable(string.Format(GetAppvalProc_SQL, orgid, bType, whereSPLX));
                }
            }
            catch (Exception e)
            {
                throw new Exception("执行SQL语句报错！");
            }

            return dataTable;
        }

        private readonly string GetAppvalProc_SQL2 = @"SELECT p.phid PhId,p.org_phid OrgPhid,p.org_code OrgCode,p.f_code FCode,p.f_name FName,p.f_billtype FBilltype,
                                                    p.f_enable FEnable,p.f_describe FDescribe,p.splx_phid SPLXPhid,p.splx_code SPLXCode,org.oname OrgName,
                                                    p.creator creator,p.editor editor,p.cur_orgid CurOrgId,p.ng_insert_dt NgInsertDt,p.ng_update_dt NgUpdateDt,p.ng_record_ver NgRecordVer,p.issystem IsSystem
                                                    FROM sp3_appval_proc p
                                                    LEFT JOIN fg_orglist org ON org.phid=p.org_phid
                                                    WHERE p.f_code='{0}' AND p.f_billtype='{1}' AND p.splx_phid={2}
                                                    ORDER BY p.issystem desc, p.f_code asc";

        /// <summary>
        /// 根据审批类型，单据类型，审批流程编码获取审批流程
        /// </summary>
        /// <param name="approvalTypeId">审批类型id</param>
        /// <param name="bType">单据类型</param>
        /// <param name="procCode">审批流程编码</param>
        /// <returns></returns>
        public DataTable GetAppvalProc(long approvalTypeId, string bType, string procCode) {
            
            if (approvalTypeId == 0 || string.IsNullOrEmpty(bType) || string.IsNullOrEmpty(procCode)) {
                return null;
            }

            /*
            string userConn = ConfigHelper.GetString("DBTG6H");

            DataTable dataTable = null;

            DbHelper.Open(userConn);
            try
            {
                if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    dataTable = DbHelper.GetDataTable(userConn, string.Format(GetAppvalProc_SQL2, procCode, bType, approvalTypeId));
                }
                else
                {
                    dataTable = DbHelper.GetDataTable(userConn, string.Format(GetAppvalProc_SQL2, procCode, bType, approvalTypeId));
                }
            }
            catch (Exception e)
            {
                throw new Exception("执行SQL语句报错！");
            }
            */

            DataTable dataTable = null;
            try
            {
                if (CommonUtils.IsOracleDB())
                {
                    dataTable = DbHelper.GetDataTable( string.Format(GetAppvalProc_SQL2, procCode, bType, approvalTypeId));
                }
                else
                {
                    dataTable = DbHelper.GetDataTable( string.Format(GetAppvalProc_SQL2, procCode, bType, approvalTypeId));
                }
            }
            catch (Exception e)
            {
                throw new Exception("执行SQL语句报错！");
            }

            return dataTable;
        }
    }
}
