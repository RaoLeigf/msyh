#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Facade
    * 类 名 称：			GAppvalRecordFacade
    * 文 件 名：			GAppvalRecordFacade.cs
    * 创建时间：			2019/5/20 
    * 作    者：			李明    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using SUP.Common.DataEntity;
using SUP.Common.DataAccess;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Facade;

using GSP3.SP.Facade.Interface;
using GSP3.SP.Rule.Interface;
using GSP3.SP.Model.Domain;
using Enterprise3.WebApi.GSP3.SP.Model.Request;
using Enterprise3.WebApi.GSP3.SP.Model.Common;
using Enterprise3.WebApi.GSP3.SP.Model.Response;
using SUP.Common.Base;
using GSP3.SP.Model.Enums;
using Enterprise3.Common.Base.Criterion;
using GBK3.BK.Model.Enums;
using GGK3.GK.Rule.Interface;
using GGK3.GK.Model.Domain;
using GBK3.BK.Rule.Interface;
using GBK3.BK.Model.Domain;
using GQT3.QT.Model.Domain;
using GQT3.QT.Rule.Interface;
using System.IO;
using System.Web.Http;
using System.Web;
using GYS3.YS.Rule.Interface;
using GXM3.XM.Rule.Interface;
using GXM3.XM.Model.Domain;
using GYS3.YS.Model.Domain;

namespace GSP3.SP.Facade
{
    /// <summary>
    /// GAppvalRecord业务组装处理类
    /// </summary>
    public partial class GAppvalRecordFacade : EntFacadeBase<GAppvalRecordModel>, IGAppvalRecordFacade
    {
        #region 类变量及属性
        /// <summary>
        /// GAppvalRecord业务逻辑处理对象
        /// </summary>
        IGAppvalRecordRule GAppvalRecordRule
        {
            get
            {
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IGAppvalRecordRule;
            }
        }

        IGKPaymentMstRule GKPaymentMstRule { get; set; }

        IGKPaymentDtlRule GKPaymentDtlRule { get; set; }

        IPaymentMstRule PaymentMstRule { get; set; }

        IPaymentDtlRule PaymentDtlRule { get; set; }

        IGAppvalPost4OperRule GAppvalPost4OperRule { get; set; }

        IGAppvalProc4PostRule GAppvalProc4PostRule { get; set; }

        IGAppvalPostRule GAppvalPostRule { get; set; }

        IUserRule UserRule { get; set; }

        IQtAttachmentRule QtAttachmentRule { get; set; }

        IBudgetMstRule BudgetMstRule { get; set; }

        IProjectMstRule ProjectMstRule { get; set; }

        IOrganizationRule OrganizationRule { get; set; }

        IExpenseMstRule ExpenseMstRule { get; set; }

        IYsIncomeMstRule YsIncomeMstRule { get; set; }
        #endregion


        #region 实现 IGAppvalRecordFacade 业务添加的成员

        /// <summary>
        /// 获取当前用户的待审批记录
        /// </summary>
        /// <param name="billRequest"></param>
        /// <param name="total">总条数</param>
        /// <returns></returns>
        public List<AppvalRecordVo> GetUnDoRecords(BillRequestModel billRequest, out int total) {

            SqlDao sqlDao = new SqlDao();
            List<AppvalRecordVo> appvalRecords = sqlDao.GetRecords(billRequest.Year, billRequest.Uid, billRequest.BType, billRequest.OrgCode, "1", billRequest.Splx_Phid);

            if (appvalRecords == null || appvalRecords.Count == 0) {
                total = 0;
                return new List<AppvalRecordVo>();
            }

            //筛选数据
            try
            {
                //申报单名称或编号查询
                if (!string.IsNullOrEmpty(billRequest.BName))
                {
                    appvalRecords = appvalRecords.FindAll(t => (t.BName != null && t.BName.IndexOf(billRequest.BName) > -1) || (t.BNum != null && t.BNum.IndexOf(billRequest.BName) > -1));
                }
                //申报日期
                if (billRequest.BStartDate != null && billRequest.BStartDate.Year > 1900)
                {
                    appvalRecords = appvalRecords.FindAll(t => t.BDate >= billRequest.BStartDate);
                }
                if (billRequest.BEndTime != null && billRequest.BEndTime.Year > 1900)
                {
                    appvalRecords = appvalRecords.FindAll(t => t.BDate <= billRequest.BEndTime);
                }
                //停留时长
                if (billRequest.Operator > 0)
                {
                    if (billRequest.Operator == 1)
                    {
                        appvalRecords = appvalRecords.FindAll(t => t.StopHour == billRequest.StopHour);
                    }
                    else if (billRequest.Operator == 2)
                    {
                        appvalRecords = appvalRecords.FindAll(t => t.StopHour > billRequest.StopHour);
                    }
                    else if (billRequest.Operator == 3)
                    {
                        appvalRecords = appvalRecords.FindAll(t => t.StopHour < billRequest.StopHour);
                    }
                }
            }
            catch (Exception e) {
                throw new Exception("根据条件筛选数据报错！");
            }

            //根据申请日期排序
            //appvalRecords.Sort((AppvalRecordVo appvalRecordVo1, AppvalRecordVo appvalRecordVo2) => -appvalRecordVo1.BDate.CompareTo(appvalRecordVo2.BDate));
            appvalRecords = appvalRecords.OrderByDescending(t => t.BDate).ToList();
            total = appvalRecords.Count;
            //分页
            appvalRecords = appvalRecords.Skip((billRequest.PageIndex - 1) * billRequest.PageSize).Take(billRequest.PageSize).ToList();

            return appvalRecords;
        }


        /// <summary>
        /// 获取当前用户的待审批记录
        /// </summary>
        /// <param name="billRequest"></param>
        /// <param name="total">总条数</param>
        /// <returns></returns>
        public List<AppvalRecordVo> GetUnDoRecords2(BillRequestModel billRequest, out int total)
        {

            SqlDao sqlDao = new SqlDao();
            List<AppvalRecordVo> appvalRecords = new List<AppvalRecordVo>();


            //if (billRequest.OrgIds != null && billRequest.OrgIds.Count > 0)
            //{
            //    var orgStr = "";
            //    foreach(var orgid in billRequest.OrgIds)
            //    {
            //        orgStr = orgStr + orgid.ToString() + ",";
            //    }
            //    appvalRecords = sqlDao.GetRecords2(billRequest.Year, billRequest.Uid, billRequest.BType,orgStr.Remove(orgStr.Length-1,1), "1", billRequest.Splx_Phid);
            //}
            //else
            //{
            //    appvalRecords = sqlDao.GetRecords(billRequest.Year, billRequest.Uid, billRequest.BType, billRequest.OrgCode, "1", billRequest.Splx_Phid);
            //}
            if (billRequest.BType == BillType.FundsPay || billRequest.BType == BillType.PayMent)
            {
                if (billRequest.OrgIds != null && billRequest.OrgIds.Count > 0)
                {
                    var orgStr = "";
                    foreach (var orgid in billRequest.OrgIds)
                    {
                        orgStr = orgStr + "'"+orgid.ToString() + "',";
                    }
                    appvalRecords = sqlDao.GetRecords2(billRequest.Year, billRequest.Uid, billRequest.BType, orgStr.Remove(orgStr.Length - 1, 1), "1", billRequest.Splx_Phid);
                }
                else
                {
                    appvalRecords = sqlDao.GetRecords(billRequest.Year, billRequest.Uid, billRequest.BType, billRequest.OrgCode, "1", billRequest.Splx_Phid);
                }
            }
            else if(billRequest.BType == BillType.Expense)
            {
                if (billRequest.OrgCodes != null && billRequest.OrgCodes.Count > 0)
                {
                    var orgStr = "";
                    foreach (var orgcode in billRequest.OrgCodes)
                    {
                        orgStr = orgStr +"'" + orgcode.ToString() + "',";
                    }
                    appvalRecords = sqlDao.GetRecords2(billRequest.Year, billRequest.Uid, billRequest.BType, orgStr.Remove(orgStr.Length - 1, 1), "1", billRequest.Splx_Phid);
                }
                else
                {
                    appvalRecords = sqlDao.GetRecords(billRequest.Year, billRequest.Uid, billRequest.BType, billRequest.OrgCode, "1", billRequest.Splx_Phid);
                }
                if (appvalRecords != null && appvalRecords.Count > 0)
                {
                    IList<OrganizeModel> orgs = this.OrganizationRule.Find(t => t.PhId != 0);
                    if (orgs != null && orgs.Count > 0)
                    {
                        foreach (var app in appvalRecords)
                        {
                            app.OrgName = orgs.ToList().Find(t => t.OCode == app.OrgCode) == null ? "" : orgs.ToList().Find(t => t.OCode == app.OrgCode).OName;
                            app.DepName = orgs.ToList().Find(t => t.OCode == app.DepCode) == null ? "" : orgs.ToList().Find(t => t.OCode == app.DepCode).OName;
                            app.BudgetName = orgs.ToList().Find(t => t.OCode == app.BudgetDept) == null ? "" : orgs.ToList().Find(t => t.OCode == app.BudgetDept).OName;
                        }
                    }
                }
            }
            else if (billRequest.BType == BillType.BeginProject || billRequest.BType == BillType.MiddleProject)
            {
                if (billRequest.OrgCodes != null && billRequest.OrgCodes.Count > 0)
                {
                    var orgStr = "";
                    foreach (var orgcode in billRequest.OrgCodes)
                    {
                        orgStr = orgStr + "'" + orgcode.ToString() + "',";
                    }
                    appvalRecords = sqlDao.GetRecords2(billRequest.Year, billRequest.Uid, billRequest.BType, orgStr.Remove(orgStr.Length - 1, 1), "1", billRequest.Splx_Phid);
                }
                else
                {
                    appvalRecords = sqlDao.GetRecords(billRequest.Year, billRequest.Uid, billRequest.BType, billRequest.OrgCode, "1", billRequest.Splx_Phid);
                }
                if (appvalRecords != null && appvalRecords.Count > 0)
                {
                    if (billRequest.BType == BillType.BeginProject)
                    {
                        appvalRecords = appvalRecords.FindAll(t => t.ProjStatus == "1");
                    }
                    else
                    {
                        appvalRecords = appvalRecords.FindAll(t => t.ProjStatus == "2" || t.ProjStatus == "9");
                    }
                }
                if (appvalRecords != null && appvalRecords.Count > 0)
                {
                    IList<OrganizeModel> orgs = this.OrganizationRule.Find(t => t.PhId != 0);
                    if (orgs != null && orgs.Count > 0)
                    {
                        foreach (var app in appvalRecords)
                        {
                            app.OrgName = orgs.ToList().Find(t => t.OCode == app.OrgCode) == null ? "" : orgs.ToList().Find(t => t.OCode == app.OrgCode).OName;
                            app.DepName = orgs.ToList().Find(t => t.OCode == app.DepCode) == null ? "" : orgs.ToList().Find(t => t.OCode == app.DepCode).OName;
                            app.BudgetName = orgs.ToList().Find(t => t.OCode == app.BudgetDept) == null ? "" : orgs.ToList().Find(t => t.OCode == app.BudgetDept).OName;
                        }
                    }
                }
            }
            else if (billRequest.BType == BillType.MiddleAddBudget || billRequest.BType == BillType.MiddleUpdateBudget || billRequest.BType == BillType.MiddleDtlBudget || billRequest.BType == BillType.MiddleBudget)
            {
                if (billRequest.OrgCodes != null && billRequest.OrgCodes.Count > 0)
                {
                    var orgStr = "";
                    foreach (var orgcode in billRequest.OrgCodes)
                    {
                        orgStr = orgStr +"'"+ orgcode.ToString() + "',";
                    }
                    appvalRecords = sqlDao.GetRecords2(billRequest.Year, billRequest.Uid, billRequest.BType, orgStr.Remove(orgStr.Length - 1, 1), "1", billRequest.Splx_Phid);
                }
                else
                {
                    appvalRecords = sqlDao.GetRecords(billRequest.Year, billRequest.Uid, billRequest.BType, billRequest.OrgCode, "1", billRequest.Splx_Phid);
                }
                if (appvalRecords != null && appvalRecords.Count > 0)
                {
                    IList<OrganizeModel> orgs = this.OrganizationRule.Find(t => t.PhId != 0);
                    if (orgs != null && orgs.Count > 0)
                    {
                        foreach (var app in appvalRecords)
                        {
                            app.OrgName = orgs.ToList().Find(t => t.OCode == app.OrgCode) == null ? "" : orgs.ToList().Find(t => t.OCode == app.OrgCode).OName;
                            app.DepName = orgs.ToList().Find(t => t.OCode == app.DepCode) == null ? "" : orgs.ToList().Find(t => t.OCode == app.DepCode).OName;
                            app.BudgetName = orgs.ToList().Find(t => t.OCode == app.BudgetDept) == null ? "" : orgs.ToList().Find(t => t.OCode == app.BudgetDept).OName;
                        }
                    }
                }
            }
            else if (billRequest.BType == BillType.InComeBudget)
            {
                if (billRequest.OrgIds != null && billRequest.OrgIds.Count > 0)
                {
                    var orgStr = "";
                    foreach (var orgid in billRequest.OrgIds)
                    {
                        orgStr = orgStr + "'" + orgid.ToString() + "',";
                    }
                    appvalRecords = sqlDao.GetRecords2(billRequest.Year, billRequest.Uid, billRequest.BType, orgStr.Remove(orgStr.Length - 1, 1), "1", billRequest.Splx_Phid);
                }
                else
                {
                    appvalRecords = sqlDao.GetRecords(billRequest.Year, billRequest.Uid, billRequest.BType, billRequest.OrgCode, "1", billRequest.Splx_Phid);
                }

                if (appvalRecords != null && appvalRecords.Count > 0)
                {
                    IList<OrganizeModel> orgs = this.OrganizationRule.Find(t => t.PhId != 0);
                    if (orgs != null && orgs.Count > 0)
                    {
                        foreach (var app in appvalRecords)
                        {
                            app.OrgName = orgs.ToList().Find(t => t.OCode == app.OrgCode) == null ? "" : orgs.ToList().Find(t => t.OCode == app.OrgCode).OName;
                            app.BName = app.BYear + app.OrgName + "收入预算";
                            //app.DepName = orgs.ToList().Find(t => t.OCode == app.DepCode) == null ? "" : orgs.ToList().Find(t => t.OCode == app.DepCode).OName;
                            //app.BudgetName = orgs.ToList().Find(t => t.OCode == app.BudgetDept) == null ? "" : orgs.ToList().Find(t => t.OCode == app.BudgetDept).OName;
                        }
                    }
                }
            }
            if (appvalRecords == null || appvalRecords.Count == 0)
            {
                total = 0;
                return new List<AppvalRecordVo>();
            }

            //筛选数据
            try
            {
                //申报单名称或编号查询
                if (!string.IsNullOrEmpty(billRequest.BName))
                {
                    appvalRecords = appvalRecords.FindAll(t => (t.BName != null && t.BName.IndexOf(billRequest.BName) > -1) || (t.BNum != null && t.BNum.IndexOf(billRequest.BName) > -1));
                }
                //申报日期
                if (billRequest.BStartDate != null && billRequest.BStartDate.Year > 1900)
                {
                    appvalRecords = appvalRecords.FindAll(t => t.BDate >= billRequest.BStartDate);
                }
                if (billRequest.BEndTime != null && billRequest.BEndTime.Year > 1900)
                {
                    appvalRecords = appvalRecords.FindAll(t => t.BDate <= billRequest.BEndTime);
                }
                //停留时长
                if (billRequest.Operator > 0)
                {
                    if (billRequest.Operator == 1)
                    {
                        appvalRecords = appvalRecords.FindAll(t => t.StopHour == billRequest.StopHour);
                    }
                    else if (billRequest.Operator == 2)
                    {
                        appvalRecords = appvalRecords.FindAll(t => t.StopHour > billRequest.StopHour);
                    }
                    else if (billRequest.Operator == 3)
                    {
                        appvalRecords = appvalRecords.FindAll(t => t.StopHour < billRequest.StopHour);
                    }
                }
                //根据审批流程与审批岗位进行筛选
                if(billRequest.ProcPhid != 0 && billRequest.PostPhid != 0)
                {
                    appvalRecords = appvalRecords.FindAll(t => t.ProcPhid == billRequest.ProcPhid && t.PostPhid == billRequest.PostPhid);
                }
            }
            catch (Exception e)
            {
                throw new Exception("根据条件筛选数据报错！" + e.Message);
            }

            //根据申请日期排序
            //appvalRecords.Sort((AppvalRecordVo appvalRecordVo1, AppvalRecordVo appvalRecordVo2) => -appvalRecordVo1.BDate.CompareTo(appvalRecordVo2.BDate));
            appvalRecords = appvalRecords.OrderByDescending(t => t.BDate).ToList();
            total = appvalRecords.Count;
            //分页
            appvalRecords = appvalRecords.Skip((billRequest.PageIndex - 1) * billRequest.PageSize).Take(billRequest.PageSize).ToList();

            return appvalRecords;
        }

        /// <summary>
        /// 获取当前用户的待审批记录(不分页)
        /// </summary>
        /// <param name="billRequest"></param>
        /// <returns></returns>
        public List<AppvalRecordVo> GetUnDoRecords3(BillRequestModel billRequest)
        {
            SqlDao sqlDao = new SqlDao();
            List<AppvalRecordVo> appvalRecords = new List<AppvalRecordVo>();           
            if (billRequest.OrgIds != null && billRequest.OrgIds.Count > 0)
            {
                var orgStr = "";
                foreach (var orgid in billRequest.OrgIds)
                {
                    orgStr = orgStr +"'"+ orgid.ToString() + "',";
                }
                appvalRecords = sqlDao.GetRecords2(billRequest.Year, billRequest.Uid, billRequest.BType, orgStr.Remove(orgStr.Length - 1, 1), "1", billRequest.Splx_Phid);
            }
            else
            {
                appvalRecords = sqlDao.GetRecords(billRequest.Year, billRequest.Uid, billRequest.BType, billRequest.OrgCode, "1", billRequest.Splx_Phid);
            }

            if (appvalRecords == null || appvalRecords.Count == 0)
            {
                return new List<AppvalRecordVo>();
            }

            //筛选数据
            try
            {
                //申报单名称或编号查询
                if (!string.IsNullOrEmpty(billRequest.BName))
                {
                    appvalRecords = appvalRecords.FindAll(t => (t.BName != null && t.BName.IndexOf(billRequest.BName) > -1) || (t.BNum != null && t.BNum.IndexOf(billRequest.BName) > -1));
                }
                //申报日期
                if (billRequest.BStartDate != null && billRequest.BStartDate.Year > 1900)
                {
                    appvalRecords = appvalRecords.FindAll(t => t.BDate >= billRequest.BStartDate);
                }
                if (billRequest.BEndTime != null && billRequest.BEndTime.Year > 1900)
                {
                    appvalRecords = appvalRecords.FindAll(t => t.BDate <= billRequest.BEndTime);
                }
                //停留时长
                if (billRequest.Operator > 0)
                {
                    if (billRequest.Operator == 1)
                    {
                        appvalRecords = appvalRecords.FindAll(t => t.StopHour == billRequest.StopHour);
                    }
                    else if (billRequest.Operator == 2)
                    {
                        appvalRecords = appvalRecords.FindAll(t => t.StopHour > billRequest.StopHour);
                    }
                    else if (billRequest.Operator == 3)
                    {
                        appvalRecords = appvalRecords.FindAll(t => t.StopHour < billRequest.StopHour);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("根据条件筛选数据报错！");
            }
            return appvalRecords;
        }

        /// <summary>
        /// 分页获取已审批单据数据
        /// </summary>
        /// <param name="billRequest"></param>
        /// <param name="total">总条数</param>
        /// <returns></returns>
        public List<AppvalRecordVo> GetDoneRecordList(BillRequestModel billRequest, out int total) {
            SqlDao sqlDao = new SqlDao();
            List<AppvalRecordVo> appvalRecords = sqlDao.GetRecords(billRequest.Year, billRequest.Uid, billRequest.BType, billRequest.OrgCode, "9", billRequest.Splx_Phid);

            if (appvalRecords == null || appvalRecords.Count == 0)
            {
                total = 0;
                return new List<AppvalRecordVo>();
            }

            //去除重复数据(如果单据审批被退回过，可能存在同一张单据，同一个审批人的多条审批记录)
            Dictionary<long, AppvalRecordVo> map = new Dictionary<long, AppvalRecordVo>();
            foreach (AppvalRecordVo model in appvalRecords) {
                if (!map.ContainsKey(model.RefbillPhid)) {
                    map.Add(model.RefbillPhid, model);
                }
            }
            appvalRecords = map.Values.ToList();

            //筛选数据
            try
            {
                //申报单名称或编号查询
                if (!string.IsNullOrEmpty(billRequest.BName))
                {
                    appvalRecords = appvalRecords.FindAll(t => (t.BName != null && t.BName.IndexOf(billRequest.BName) > -1) || (t.BNum != null && t.BNum.IndexOf(billRequest.BName) > -1));
                }
                //申报日期
                if (billRequest.BStartDate != null && billRequest.BStartDate.Year > 1900)
                {
                    appvalRecords = appvalRecords.FindAll(t => t.BDate >= billRequest.BStartDate);
                }
                if (billRequest.BEndTime != null && billRequest.BEndTime.Year > 1900)
                {
                    appvalRecords = appvalRecords.FindAll(t => t.BDate <= billRequest.BEndTime);
                }
                //停留时长
                if (billRequest.Operator > 0)
                {
                    if (billRequest.Operator == 1)
                    {
                        appvalRecords = appvalRecords.FindAll(t => t.StopHour == billRequest.StopHour);
                    }
                    else if (billRequest.Operator == 2)
                    {
                        appvalRecords = appvalRecords.FindAll(t => t.StopHour > billRequest.StopHour);
                    }
                    else if (billRequest.Operator == 3)
                    {
                        appvalRecords = appvalRecords.FindAll(t => t.StopHour < billRequest.StopHour);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("根据条件筛选数据报错！");
            }
            //根据申请日期排序
            //appvalRecords.Sort((AppvalRecordVo appvalRecordVo1, AppvalRecordVo appvalRecordVo2) => -appvalRecordVo1.BDate.CompareTo(appvalRecordVo2.BDate));
            appvalRecords = appvalRecords.OrderByDescending(t => t.BDate).ToList();
            total = appvalRecords.Count;
            //分页
            appvalRecords = appvalRecords.Skip((billRequest.PageIndex - 1) * billRequest.PageSize).Take(billRequest.PageSize).ToList();

            return appvalRecords;
        }
        /// <summary>
        /// 分页获取已审批单据数据
        /// </summary>
        /// <param name="billRequest"></param>
        /// <param name="total">总条数</param>
        /// <returns></returns>
        public List<AppvalRecordVo> GetDoneRecordList2(BillRequestModel billRequest, out int total)
        {
            SqlDao sqlDao = new SqlDao();
            //List<AppvalRecordVo> appvalRecords = sqlDao.GetRecords(billRequest.Year, billRequest.Uid, billRequest.BType, billRequest.OrgCode, "9", billRequest.Splx_Phid);

            List<AppvalRecordVo> appvalRecords = new List<AppvalRecordVo>();
            if(billRequest.BType == BillType.FundsPay || billRequest.BType == BillType.PayMent)
            {
                if (billRequest.OrgIds != null && billRequest.OrgIds.Count > 0)
                {
                    var orgStr = "";
                    foreach (var orgid in billRequest.OrgIds)
                    {
                        orgStr = orgStr +"'"+ orgid.ToString() + "',";
                    }
                    appvalRecords = sqlDao.GetRecords2(billRequest.Year, billRequest.Uid, billRequest.BType, orgStr.Remove(orgStr.Length-1, 1), "9", billRequest.Splx_Phid);
                }
                else
                {
                    appvalRecords = sqlDao.GetRecords(billRequest.Year, billRequest.Uid, billRequest.BType, billRequest.OrgCode, "9", billRequest.Splx_Phid);
                }
            }
            else if (billRequest.BType == BillType.BeginProject || billRequest.BType == BillType.MiddleProject)
            {
                if (billRequest.OrgCodes != null && billRequest.OrgCodes.Count > 0)
                {
                    var orgStr = "";
                    foreach (var orgcode in billRequest.OrgCodes)
                    {
                        orgStr = orgStr +"'"+ orgcode.ToString() + "',";
                    }
                    appvalRecords = sqlDao.GetRecords2(billRequest.Year, billRequest.Uid, billRequest.BType, orgStr.Remove(orgStr.Length - 1, 1), "9", billRequest.Splx_Phid);
                }
                else
                {
                    appvalRecords = sqlDao.GetRecords(billRequest.Year, billRequest.Uid, billRequest.BType, billRequest.OrgCode, "9", billRequest.Splx_Phid);
                }
                //无需筛选项目状态
                //if (appvalRecords != null && appvalRecords.Count > 0)
                //{
                //    if (billRequest.BType == BillType.BeginProject)
                //    {
                //        appvalRecords = appvalRecords.FindAll(t => t.ProjStatus == "1");
                //    }
                //    else
                //    {
                //        appvalRecords = appvalRecords.FindAll(t => t.ProjStatus == "2"|| t.ProjStatus == "9");
                //    }
                //}
                if(appvalRecords != null && appvalRecords.Count > 0)
                {
                    IList<OrganizeModel> orgs = this.OrganizationRule.Find(t => t.PhId != 0);
                    if(orgs != null && orgs.Count > 0)
                    {
                        foreach(var app in appvalRecords)
                        {
                            app.OrgName = orgs.ToList().Find(t => t.OCode == app.OrgCode) == null ? "" : orgs.ToList().Find(t => t.OCode == app.OrgCode).OName;
                            app.DepName = orgs.ToList().Find(t => t.OCode == app.DepCode) == null ? "" : orgs.ToList().Find(t => t.OCode == app.DepCode).OName;
                            app.BudgetName = orgs.ToList().Find(t => t.OCode == app.BudgetDept) == null ? "" : orgs.ToList().Find(t => t.OCode == app.BudgetDept).OName;
                        }
                    }
                }
            }
            else if (billRequest.BType == BillType.Expense)
            {
                if (billRequest.OrgCodes != null && billRequest.OrgCodes.Count > 0)
                {
                    var orgStr = "";
                    foreach (var orgcode in billRequest.OrgCodes)
                    {
                        orgStr = orgStr + "'" + orgcode.ToString() + "',";
                    }
                    appvalRecords = sqlDao.GetRecords2(billRequest.Year, billRequest.Uid, billRequest.BType, orgStr.Remove(orgStr.Length - 1, 1), "9", billRequest.Splx_Phid);
                }
                else
                {
                    appvalRecords = sqlDao.GetRecords(billRequest.Year, billRequest.Uid, billRequest.BType, billRequest.OrgCode, "9", billRequest.Splx_Phid);
                }
                if (appvalRecords != null && appvalRecords.Count > 0)
                {
                    IList<OrganizeModel> orgs = this.OrganizationRule.Find(t => t.PhId != 0);
                    if (orgs != null && orgs.Count > 0)
                    {
                        foreach (var app in appvalRecords)
                        {
                            app.OrgName = orgs.ToList().Find(t => t.OCode == app.OrgCode) == null ? "" : orgs.ToList().Find(t => t.OCode == app.OrgCode).OName;
                            app.DepName = orgs.ToList().Find(t => t.OCode == app.DepCode) == null ? "" : orgs.ToList().Find(t => t.OCode == app.DepCode).OName;
                            app.BudgetName = orgs.ToList().Find(t => t.OCode == app.BudgetDept) == null ? "" : orgs.ToList().Find(t => t.OCode == app.BudgetDept).OName;
                        }
                    }
                }
            }
            else if (billRequest.BType == BillType.MiddleAddBudget || billRequest.BType == BillType.MiddleUpdateBudget || billRequest.BType == BillType.MiddleDtlBudget || billRequest.BType == BillType.MiddleBudget)
            {
                if (billRequest.OrgCodes != null && billRequest.OrgCodes.Count > 0)
                {
                    var orgStr = "";
                    foreach (var orgcode in billRequest.OrgCodes)
                    {
                        orgStr = orgStr + "'" + orgcode.ToString() + "',";
                    }
                    appvalRecords = sqlDao.GetRecords2(billRequest.Year, billRequest.Uid, billRequest.BType, orgStr.Remove(orgStr.Length - 1, 1), "9", billRequest.Splx_Phid);
                }
                else
                {
                    appvalRecords = sqlDao.GetRecords(billRequest.Year, billRequest.Uid, billRequest.BType, billRequest.OrgCode, "9", billRequest.Splx_Phid);
                }
                if (appvalRecords != null && appvalRecords.Count > 0)
                {
                    IList<OrganizeModel> orgs = this.OrganizationRule.Find(t => t.PhId != 0);
                    if (orgs != null && orgs.Count > 0)
                    {
                        foreach (var app in appvalRecords)
                        {
                            app.OrgName = orgs.ToList().Find(t => t.OCode == app.OrgCode) == null ? "" : orgs.ToList().Find(t => t.OCode == app.OrgCode).OName;
                            app.DepName = orgs.ToList().Find(t => t.OCode == app.DepCode) == null ? "" : orgs.ToList().Find(t => t.OCode == app.DepCode).OName;
                            app.BudgetName = orgs.ToList().Find(t => t.OCode == app.BudgetDept) == null ? "" : orgs.ToList().Find(t => t.OCode == app.BudgetDept).OName;
                        }
                    }
                }
            }
            else if (billRequest.BType == BillType.InComeBudget)
            {
                if (billRequest.OrgIds != null && billRequest.OrgIds.Count > 0)
                {
                    var orgStr = "";
                    foreach (var orgid in billRequest.OrgIds)
                    {
                        orgStr = orgStr + "'" + orgid.ToString() + "',";
                    }
                    appvalRecords = sqlDao.GetRecords2(billRequest.Year, billRequest.Uid, billRequest.BType, orgStr.Remove(orgStr.Length - 1, 1), "9", billRequest.Splx_Phid);
                }
                else
                {
                    appvalRecords = sqlDao.GetRecords(billRequest.Year, billRequest.Uid, billRequest.BType, billRequest.OrgCode, "9", billRequest.Splx_Phid);
                }

                if (appvalRecords != null && appvalRecords.Count > 0)
                {
                    IList<OrganizeModel> orgs = this.OrganizationRule.Find(t => t.PhId != 0);
                    if (orgs != null && orgs.Count > 0)
                    {
                        foreach (var app in appvalRecords)
                        {
                            app.OrgName = orgs.ToList().Find(t => t.OCode == app.OrgCode) == null ? "" : orgs.ToList().Find(t => t.OCode == app.OrgCode).OName;
                            app.BName = app.BYear + app.OrgName + "收入预算";
                            //app.DepName = orgs.ToList().Find(t => t.OCode == app.DepCode) == null ? "" : orgs.ToList().Find(t => t.OCode == app.DepCode).OName;
                            //app.BudgetName = orgs.ToList().Find(t => t.OCode == app.BudgetDept) == null ? "" : orgs.ToList().Find(t => t.OCode == app.BudgetDept).OName;
                        }
                    }
                }
            }


            if (appvalRecords == null || appvalRecords.Count == 0)
            {
                total = 0;
                return new List<AppvalRecordVo>();
            }

            //去除重复数据(如果单据审批被退回过，可能存在同一张单据，同一个审批人的多条审批记录)
            Dictionary<long, AppvalRecordVo> map = new Dictionary<long, AppvalRecordVo>();
            foreach (AppvalRecordVo model in appvalRecords)
            {
                if (!map.ContainsKey(model.RefbillPhid))
                {
                    map.Add(model.RefbillPhid, model);
                }
            }
            appvalRecords = map.Values.ToList();

            //筛选数据
            try
            {
                //申报单名称或编号查询
                if (!string.IsNullOrEmpty(billRequest.BName))
                {
                    appvalRecords = appvalRecords.FindAll(t => (t.BName != null && t.BName.IndexOf(billRequest.BName) > -1) || (t.BNum != null && t.BNum.IndexOf(billRequest.BName) > -1));
                }
                //申报日期
                if (billRequest.BStartDate != null && billRequest.BStartDate.Year > 1900)
                {
                    appvalRecords = appvalRecords.FindAll(t => t.BDate >= billRequest.BStartDate);
                }
                if (billRequest.BEndTime != null && billRequest.BEndTime.Year > 1900)
                {
                    appvalRecords = appvalRecords.FindAll(t => t.BDate <= billRequest.BEndTime);
                }
                //停留时长
                if (billRequest.Operator > 0)
                {
                    if (billRequest.Operator == 1)
                    {
                        appvalRecords = appvalRecords.FindAll(t => t.StopHour == billRequest.StopHour);
                    }
                    else if (billRequest.Operator == 2)
                    {
                        appvalRecords = appvalRecords.FindAll(t => t.StopHour > billRequest.StopHour);
                    }
                    else if (billRequest.Operator == 3)
                    {
                        appvalRecords = appvalRecords.FindAll(t => t.StopHour < billRequest.StopHour);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("根据条件筛选数据报错！");
            }
            //根据申请日期排序
            //appvalRecords.Sort((AppvalRecordVo appvalRecordVo1, AppvalRecordVo appvalRecordVo2) => -appvalRecordVo1.BDate.CompareTo(appvalRecordVo2.BDate));
            appvalRecords = appvalRecords.OrderByDescending(t => t.BDate).ToList();
            total = appvalRecords.Count;
            //分页
            appvalRecords = appvalRecords.Skip((billRequest.PageIndex - 1) * billRequest.PageSize).Take(billRequest.PageSize).ToList();

            return appvalRecords;
        }
        /// <summary>
        /// 根据关联单据id,单据类型查找审批记录
        /// </summary>
        /// <param name="phid">单据id</param>
        /// <param name="billType">单据类型</param>
        /// <returns></returns>
        public List<GAppvalRecordModel> FindByRelId(long phid, string billType) {

            SqlDao sqlDao = new SqlDao();
            List<GAppvalRecordModel> recordModels = sqlDao.FindByRelId(phid, billType);

            if (recordModels == null) {
                return new List<GAppvalRecordModel>();
            }

            return recordModels;
        }

        /// <summary>
        /// 更新审批记录
        /// </summary>
        /// <param name="recordModel"></param>
        /// <param name="presentPost">当前审批岗位</param>
        /// <param name="models">当前审批岗位的审批记录</param>
        /// <returns></returns>
        public SavedResult<Int64> UpdateApprovalRecord(GAppvalRecordModel recordModel, GAppvalPostModel presentPost, List<GAppvalRecordModel> models) {
            if (recordModel == null || recordModel.PhId == 0 || presentPost == null || models == null) {
                return null;
            }

            //判断当前岗位是否是会签
            foreach (GAppvalRecordModel model in models) {
                if (model.PhId == recordModel.PhId)
                {
                    model.FApproval = recordModel.FApproval;
                    model.FDate = DateTime.Now;
                    model.FOpinion = recordModel.FOpinion;
                    model.PersistentState = PersistentState.Modified;
                }
                else {
                    if (presentPost.FMode == (Byte)ModeType.Yes)
                    {
                        model.PersistentState = PersistentState.UnChanged;
                        //如果当前岗位为会签模式，并且审批不通过，那么删除其他未审批的审批记录
                        if (recordModel.FApproval == (byte)Approval.UnPass && model.FApproval != (byte)Approval.Pass) {
                            model.PersistentState = PersistentState.Deleted;
                        }
                    }
                    else {
                        model.PersistentState = PersistentState.Deleted;
                    }
                }

            }

            try
            {
                return GAppvalRecordRule.Save<Int64>(models);
            }
            catch (Exception e) {
                throw new Exception("更新审批记录失败！" + e.Message);
            }
        }

        /// <summary>
        /// 把数据保存到审批记录表中
        /// </summary>
        /// <param name="gAppvalRecord">审批发起记录</param>
        /// <returns></returns>
        public SavedResult<long> AddAppvalRecord(GAppvalRecordModel gAppvalRecord)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            //新增审批记录（多条）
            Dictionary<string, object> dic = new Dictionary<string, object>();

            long postPhid = gAppvalRecord.PostPhid;
            byte fApproval = gAppvalRecord.FApproval;
            IList<GAppvalPost4OperModel> operators = new List<GAppvalPost4OperModel>();
            IList<GAppvalProc4PostModel> procPost = new List<GAppvalProc4PostModel>();
            if (gAppvalRecord.NextOperators != null && gAppvalRecord.NextOperators.Count > 0)
            {
                dic.Clear();
                new CreateCriteria(dic)
                    .Add(ORMRestrictions<List<long>>.In("PhId", gAppvalRecord.NextOperators));
                operators = this.GAppvalPost4OperRule.Find(dic);
                if (operators.Count > 0)
                {
                    dic.Clear();
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<long>.Eq("ProcPhid", gAppvalRecord.ProcPhid))
                        .Add(ORMRestrictions<long>.Eq("PostPhid", gAppvalRecord.PostPhid));
                    procPost = this.GAppvalProc4PostRule.Find(dic);
                    if (procPost.Count <= 0)
                    {
                        throw new Exception("传递的岗位与流程不正确！");
                    }
                }
                else
                {
                    throw new Exception("下一岗位的审批人信息获取失败！");
                }
                List<GAppvalRecordModel> addRecords = new List<GAppvalRecordModel>();
                List<GAppvalRecordModel> addFirst = new List<GAppvalRecordModel>();
                List<PaymentMstModel> updatePayments = new List<PaymentMstModel>();
                List<GKPaymentMstModel> updateGks = new List<GKPaymentMstModel>();
                List<ProjectMstModel> updatePro = new List<ProjectMstModel>();
                List<BudgetMstModel> updateBud = new List<BudgetMstModel>();
                List<ExpenseMstModel> updateExp = new List<ExpenseMstModel>();
                List<YsIncomeMstModel> updateIn = new List<YsIncomeMstModel>();
                foreach (var billPhid in gAppvalRecord.RefbillPhidList)
                {
                    GAppvalRecordModel gAppval = gAppvalRecord;
                    gAppval.RefbillPhid = billPhid;
                    gAppval.PostPhid = postPhid;
                    gAppval.FApproval = fApproval;
                    foreach (var oper in operators)
                    {
                        addRecords.Add(new GAppvalRecordModel
                        {
                            RefbillPhid = gAppval.RefbillPhid,
                            FBilltype = gAppval.FBilltype,
                            ProcPhid = gAppval.ProcPhid,
                            PostPhid = gAppval.PostPhid,
                            OperaPhid = oper.OperatorPhid,
                            OperatorCode = oper.OperatorCode,
                            FSeq = procPost[0].FSeq,
                            FSendDate = DateTime.Now,
                            FApproval = (Byte)Approval.Wait,
                            FOpinion = gAppval.FOpinion,
                            PersistentState = PersistentState.Added
                        });
                    }
                    //savedResult = this.GAppvalRecordRule.Save<long>(addRecords);       
                    //加一条发起人的记录
                    //dic.Clear();
                    //new CreateCriteria(dic)
                    //    .Add(ORMRestrictions<long>.Eq("RefbillPhid", gAppval.RefbillPhid))
                    //    .Add(ORMRestrictions<long>.Eq("PostPhid", (long)0))
                    //    .Add(ORMRestrictions<string>.Eq("FBilltype", gAppval.FBilltype));
                    //var record = this.GAppvalRecordRule.Find(dic);

                    //if (record.Count > 0)
                    //{
                    //    gAppval.PostPhid = record[0].PostPhid;
                    //    gAppval.FApproval = record[0].FApproval;
                    //    gAppval.FSendDate = record[0].FSendDate;
                    //    gAppval.FDate = record[0].FDate;
                    //    gAppval.PhId = record[0].PhId; 
                    //    gAppval.OperaPhid = gAppvalRecord.OperaPhid;
                    //    gAppval.OperatorCode = gAppvalRecord.OperatorCode;
                    //    gAppval.NgRecordVer = record[0].NgRecordVer;
                    //    gAppval.PersistentState = PersistentState.Modified;
                    //}
                    //else
                    //{
                    //    gAppval.PostPhid = (long)0;
                    //    gAppval.FApproval = (byte)9;
                    //    gAppval.FSendDate = DateTime.Now;
                    //    gAppval.FDate = DateTime.Now;
                    //    gAppval.OperaPhid = gAppvalRecord.OperaPhid;
                    //    gAppval.OperatorCode = gAppvalRecord.OperatorCode;
                    //    gAppval.PersistentState = PersistentState.Added;
                    //}
                    gAppval.PostPhid = (long)0;
                    gAppval.FApproval = (byte)9;
                    gAppval.FSendDate = DateTime.Now;
                    gAppval.FDate = DateTime.Now;
                    gAppval.OperaPhid = gAppvalRecord.OperaPhid;
                    gAppval.OperatorCode = gAppvalRecord.OperatorCode;
                    gAppval.PersistentState = PersistentState.Added;
                    gAppval.PhId = (long)0;
                    //addFirst.Add(gAppval);
                    addFirst.Add(new GAppvalRecordModel
                    {
                        RefbillPhid = billPhid,
                        FBilltype = gAppval.FBilltype,
                        ProcPhid = gAppval.ProcPhid,
                        PostPhid = (long)0,
                        OperaPhid = gAppvalRecord.OperaPhid,
                        OperatorCode = gAppvalRecord.OperatorCode,
                        FSeq = 0,
                        FSendDate = DateTime.Now,
                        FApproval = (byte)9,
                        FDate = DateTime.Now,
                        PersistentState = PersistentState.Added
                    });
                    //savedResult = this.GAppvalRecordRule.Save<long>(gAppval);
                    //修改主表审批状态
                    if (gAppval.FBilltype == BillType.FundsPay)
                    {
                        dic.Clear();
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<long>.Eq("PhId", gAppval.RefbillPhid));
                        var paymentMst = this.PaymentMstRule.Find(dic);
                        if (paymentMst.Count > 0)
                        {
                            paymentMst[0].FApproval = (byte)ApprovalType.pend;
                            paymentMst[0].PersistentState = PersistentState.Modified;
                            //savedResult = this.PaymentMstRule.Save<long>(paymentMst[0]);
                            updatePayments.Add(paymentMst[0]);
                        }
                        else
                        {
                            throw new Exception("此申请单据记录有误！");
                        }
                    }
                    else if (gAppval.FBilltype == BillType.PayMent)
                    {
                        dic.Clear();
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<long>.Eq("PhId", gAppval.RefbillPhid));
                        var paymentMst = this.GKPaymentMstRule.Find(dic);
                        if (paymentMst.Count > 0)
                        {
                            paymentMst[0].FApproval = (byte)ApprovalType.pend;
                            paymentMst[0].PersistentState = PersistentState.Modified;
                            //savedResult = this.GKPaymentMstRule.Save<long>(paymentMst[0]);
                            updateGks.Add(paymentMst[0]);
                        }
                        else
                        {
                            throw new Exception("此申请单据记录有误！");
                        }
                    }
                    else if (gAppval.FBilltype == BillType.BeginProject || gAppval.FBilltype == BillType.MiddleProject || gAppval.FBilltype == BillType.ExpendBudeget) 
                    {
                        dic.Clear();
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<long>.Eq("PhId", gAppval.RefbillPhid));
                        var payments = this.ProjectMstRule.Find(dic);
                        if (payments.Count > 0)
                        {
                            payments[0].FApproveStatus = "2";
                            payments[0].FApproveDate = DateTime.Now;
                            payments[0].FApprover = gAppval.OperaPhid;
                            payments[0].FApprover_EXName = gAppval.OperaName;
                            payments[0].PersistentState = PersistentState.Modified;
                            //点击的是生成草案(单据的状态也要改成项目草案)
                            if (gAppval.FBilltype == BillType.MiddleProject)
                            {
                                payments[0].FProjStatus = 2;//把项目状态改成项目草案
                            }
                            //savedResult = this.GKPaymentMstRule.Save<long>(paymentMst[0]);
                            updatePro.Add(payments[0]);
                        }
                        else
                        {
                            throw new Exception("此申请单据记录有误！");
                        }
                    }
                    else if (gAppval.FBilltype == BillType.MiddleAddBudget || gAppval.FBilltype == BillType.MiddleUpdateBudget || gAppval.FBilltype == BillType.MiddleDtlBudget || gAppval.FBilltype == BillType.MiddleBudget)
                    {
                        dic.Clear();
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<long>.Eq("PhId", gAppval.RefbillPhid));
                        var payments = this.BudgetMstRule.Find(dic);
                        if (payments.Count > 0)
                        {
                            payments[0].FApproveStatus = "2";
                            payments[0].FApproveDate = DateTime.Now;
                            payments[0].FApprover = gAppval.OperaPhid;
                            payments[0].FApprover_EXName = gAppval.OperaName;
                            payments[0].PersistentState = PersistentState.Modified;
                            //savedResult = this.GKPaymentMstRule.Save<long>(paymentMst[0]);
                            updateBud.Add(payments[0]);
                        }
                        else
                        {
                            throw new Exception("此申请单据记录有误！");
                        }
                    }
                    else if (gAppval.FBilltype == BillType.Expense)
                    {
                        dic.Clear();
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<long>.Eq("PhId", gAppval.RefbillPhid));
                        var payments = this.ExpenseMstRule.Find(dic);
                        if (payments.Count > 0)
                        {
                            payments[0].FApprovestatus = "2";
                            payments[0].FApprovedate = DateTime.Now;
                            payments[0].FApprover = gAppval.OperaPhid;
                            payments[0].FApprover_EXName = gAppval.OperaName;
                            payments[0].PersistentState = PersistentState.Modified;
                            //savedResult = this.GKPaymentMstRule.Save<long>(paymentMst[0]);
                            updateExp.Add(payments[0]);
                        }
                        else
                        {
                            throw new Exception("此申请单据记录有误！");
                        }
                    }
                    else if (gAppval.FBilltype == BillType.InComeBudget)
                    {
                        dic.Clear();
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<long>.Eq("PhId", gAppval.RefbillPhid));
                        var payments = this.YsIncomeMstRule.Find(dic);
                        if (payments.Count > 0)
                        {
                            payments[0].FApproval = 1;
                            payments[0].FReporter = gAppval.OperaPhid;
                            payments[0].FReporttime = DateTime.Now;
                            //payments[0].f = ;
                            //payments[0].FApprover = gAppval.OperaPhid;
                            //payments[0].FApprover_EXName = gAppval.OperaName;
                            payments[0].PersistentState = PersistentState.Modified;
                            //savedResult = this.GKPaymentMstRule.Save<long>(paymentMst[0]);
                            updateIn.Add(payments[0]);
                        }
                        else
                        {
                            throw new Exception("此申请单据记录有误！");
                        }
                    }
                }
                if (addRecords.Count > 0)
                {
                    savedResult = this.GAppvalRecordRule.Save<long>(addRecords);
                }
                if (addFirst.Count > 0)
                {
                    savedResult = this.GAppvalRecordRule.Save<long>(addFirst);
                    if(gAppvalRecord.QtAttachments != null && gAppvalRecord.QtAttachments.Count > 0 && savedResult.KeyCodes.Count > 0)
                    {
                        foreach(var attach in gAppvalRecord.QtAttachments)
                        {
                            attach.RelPhid = savedResult.KeyCodes[0];
                            attach.BTable = "SP3_APPVAL_RECORD";
                            attach.PersistentState = PersistentState.Added;
                        }
                        savedResult = this.QtAttachmentRule.Save<long>(gAppvalRecord.QtAttachments);
                    }
                }
                if (updatePayments.Count > 0)
                {
                    savedResult = this.PaymentMstRule.Save<long>(updatePayments);
                }
                if (updateGks.Count > 0)
                {
                    savedResult = this.GKPaymentMstRule.Save<long>(updateGks);
                }
                if(updatePro.Count > 0)
                {
                    savedResult = this.ProjectMstRule.Save<long>(updatePro);
                }
                if(updateBud.Count > 0)
                {
                    savedResult = this.BudgetMstRule.Save<long>(updateBud);
                }
                if(updateExp.Count > 0)
                {
                    savedResult = this.ExpenseMstRule.Save<long>(updateExp);
                }
                if(updateIn.Count > 0)
                {
                    savedResult = this.YsIncomeMstRule.Save<long>(updateIn);
                }
            }
            else
            {
                throw new Exception("下一岗位的审批人不能为空！");
            }
            return savedResult;
        }

        /// <summary>
        /// 生成支付单
        /// </summary>
        /// <param name="bill_phid">资金拨付单id</param>
        /// <returns></returns>
        public SavedResult<Int64> PostAddPayMent(long bill_phid) {
            if (bill_phid == 0)
                throw new Exception("单据id为空！");

            IList<GKPaymentMstModel> mstModels = GKPaymentMstRule.Find(t => t.RefbillPhid == bill_phid && t.FBilltype == BillType.FundsPay);

            if (mstModels != null && mstModels.Count > 0) {
                throw new Exception("支付单已经生成！");
            }

            //资金拨付单数据
            PaymentMstModel paymentMst = PaymentMstRule.Find(bill_phid);
            IList<PaymentDtlModel> paymentDtls = PaymentMstRule.GetPaymentDtlsByMstPhid(bill_phid);
            if (paymentDtls == null || paymentDtls.Count == 0) {
                throw new Exception("单据明细不存在！");
            }

            //支付单主表
            GKPaymentMstModel paymentMstModel = new GKPaymentMstModel();
            paymentMstModel.OrgPhid = paymentMst.FOrgphid;
            long currentTicks = DateTime.Now.AddHours(-8).Ticks;
            DateTime dtFrom = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long currentMillis = (currentTicks - dtFrom.Ticks) / 10000;
            paymentMstModel.FCode = "P" + currentMillis.ToString();
            paymentMstModel.OrgCode = paymentMst.FOrgcode;
            paymentMstModel.RefbillPhid = bill_phid;
            paymentMstModel.RefbillCode = paymentMst.FCode;
            paymentMstModel.FAmountTotal = paymentDtls.Sum(t => t.FAmount);
            paymentMstModel.FBilltype = BillType.FundsPay;
            paymentMstModel.FYear = paymentMst.FYear;
            paymentMstModel.PersistentState = PersistentState.Added;

            SavedResult<Int64> savedResult = GKPaymentMstRule.Save<Int64>(paymentMstModel);
            if(savedResult.KeyCodes.Count > 0)
            {
                //支付单明细
                List<GKPaymentDtlModel> paymentDtlModels = new List<GKPaymentDtlModel>();
                foreach (PaymentDtlModel model in paymentDtls) {
                    GKPaymentDtlModel paymentDtlModel = new GKPaymentDtlModel();
                    paymentDtlModel.MstPhid = savedResult.KeyCodes[0];
                    paymentDtlModel.OrgPhid = paymentMst.FOrgphid;
                    paymentDtlModel.OrgCode = paymentMst.FOrgcode;
                    paymentDtlModel.RefbillPhid = bill_phid;
                    paymentDtlModel.RefbillCode = paymentMst.FCode;
                    paymentDtlModel.RefbillDtlPhid = model.PhId;
                    paymentDtlModel.FAmount = model.FAmount;
                    paymentDtlModel.FRecAcnt = model.FAccount;
                    paymentDtlModel.FRecBankcode = model.FBankcode;
                    paymentDtlModel.FRecAcntname = model.FBankname;
                    paymentDtlModel.QtKmmc = model.QtKmmc;
                    paymentDtlModel.QtKmdm = model.QtKmdm;
                    paymentDtlModel.RefbillDtlPhid2 = model.BudgetdtlPhid;
                    paymentDtlModel.FSamebank = 1;
                    paymentDtlModel.PersistentState = PersistentState.Added;
                    paymentDtlModels.Add(paymentDtlModel);
                }

                GKPaymentDtlRule.Save(paymentDtlModels, savedResult.KeyCodes[0]);
            }
            return savedResult;
        }


        /// <summary>
        /// 批量生成支付单
        /// </summary>
        /// <param name="bill_phids">资金拨付单id集合</param>
        /// <returns></returns>
        public SavedResult<Int64> PostAddPayMents(List<long> bill_phids)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            SavedResult<long> savedResult2 = new SavedResult<long>();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<List<long>>.In("RefbillPhid", bill_phids))
                .Add(ORMRestrictions<string>.Eq("FBilltype", BillType.FundsPay));
            var mstModels = this.GKPaymentMstRule.Find(dic);
            if (mstModels != null && mstModels.Count > 0)
            {
                throw new Exception("支付单已经生成！");
            }

            //资金拨付单主表数据
            dic.Clear();
            new CreateCriteria(dic)
                    .Add(ORMRestrictions<List<long>>.In("PhId", bill_phids));
            var paymentMsts = PaymentMstRule.Find(dic);
            if (paymentMsts == null || paymentMsts.Count <= 0)
            {
                throw new Exception("单据主表数据不存在！");
            }
            foreach (var payment in paymentMsts)
            {
                dic.Clear();
                new CreateCriteria(dic)
                    .Add(ORMRestrictions<long>.Eq("MstPhid", payment.PhId));
                //资金拨付单子表
                IList<PaymentDtlModel> paymentDtls = this.PaymentDtlRule.Find(dic);
                if (paymentDtls == null || paymentDtls.Count <= 0)
                {
                    throw new Exception("单据子表数据不存在！");
                }
                //支付单主表
                GKPaymentMstModel paymentMstModel = new GKPaymentMstModel();
                paymentMstModel.OrgPhid = payment.FOrgphid;
                long currentTicks = DateTime.Now.Ticks;
                DateTime dtFrom = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                long currentMillis = (currentTicks - dtFrom.Ticks) / 10000;
                paymentMstModel.FCode = "P" + currentMillis.ToString();
                paymentMstModel.OrgCode = payment.FOrgcode;
                paymentMstModel.RefbillPhid = payment.PhId;
                paymentMstModel.RefbillCode = payment.FCode;
                paymentMstModel.FAmountTotal = paymentDtls.Sum(t => t.FAmount);
                paymentMstModel.FBilltype = BillType.FundsPay;
                paymentMstModel.FYear = payment.FYear;
                paymentMstModel.PersistentState = PersistentState.Added;

                savedResult = GKPaymentMstRule.Save<Int64>(paymentMstModel);
                if(savedResult.KeyCodes.Count > 0)
                {
                    //支付单明细
                    List<GKPaymentDtlModel> paymentDtlModels = new List<GKPaymentDtlModel>();
                    foreach (PaymentDtlModel model in paymentDtls)
                    {
                        GKPaymentDtlModel paymentDtlModel = new GKPaymentDtlModel();
                        paymentDtlModel.MstPhid = savedResult.KeyCodes[0];
                        paymentDtlModel.OrgPhid = payment.FOrgphid;
                        paymentDtlModel.OrgCode = payment.FOrgcode;
                        paymentDtlModel.RefbillPhid = payment.PhId;
                        paymentDtlModel.RefbillCode = payment.FCode;
                        paymentDtlModel.RefbillDtlPhid = model.PhId;
                        paymentDtlModel.FAmount = model.FAmount;
                        paymentDtlModel.FRecAcnt = model.FAccount;
                        paymentDtlModel.FRecBankcode = model.FBankcode;
                        paymentDtlModel.FRecAcntname = model.FBankname;
                        paymentDtlModel.QtKmmc = model.QtKmmc;
                        paymentDtlModel.QtKmdm = model.QtKmdm;
                        paymentDtlModel.RefbillDtlPhid2 = model.BudgetdtlPhid;
                        paymentDtlModel.FSamebank = 1;

                        paymentDtlModel.PersistentState = PersistentState.Added;
                        paymentDtlModels.Add(paymentDtlModel);
                    }
                    GKPaymentDtlRule.Save(paymentDtlModels, savedResult.KeyCodes[0]);
                }               
            }
            //修改资金拨付单据审批状态
            foreach (var paym in paymentMsts)
            {
                paym.FApproval = (byte)Approval.Pass;
                paym.PersistentState = PersistentState.Modified;
            }
            savedResult2 = this.PaymentMstRule.Save<long>(paymentMsts);
            return savedResult;
        }


        /// <summary>
        /// 审批流查看
        /// </summary>
        /// <param name="phid">单据主键</param>
        /// <param name="proc_phid">流程主键</param>
        /// <param name="billType">单据类型</param>
        /// <returns></returns>
        public List<GAppvalRecordModel> GetAppvalRecordList(long phid,long proc_phid, string billType)
        {
            List<GAppvalRecordModel> recordModels = new List<GAppvalRecordModel>();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (phid == 0)
                return recordModels;

            //根据关联单据id查询所有的审批记录
            List<GAppvalRecordModel> findedRecords = FindByRelId(phid, billType);
            //根据审批流程找到所有的审批岗位
            List<GAppvalPostModel> postModels = GAppvalPostRule.FindAppvalPostByProcID(proc_phid);
            if(postModels==null || postModels.Count <= 0)
            {
                throw new Exception("该单据对应流程设置不正确！");
            }
            int maxSeq = postModels.Select(t => t.Seq).Max();

            if (findedRecords == null || findedRecords.Count == 0)
            {
                int minSeq = postModels.Select(t => t.Seq).Min();
                for (int i = minSeq; i <= maxSeq; i++)
                {
                    //还未审批的岗位，审批人显示的是审批岗位的名称，不是这个岗位的一个或多个审批人的名字
                    GAppvalRecordModel recordModel = new GAppvalRecordModel();
                    recordModel.FApproval = 1;
                    recordModel.PostName = postModels.Find(t => t.Seq == i).FName;
                    recordModel.FSeq = i;
                    recordModel.FBilltype = billType;
                    recordModels.Add(recordModel);
                }
            }
            else
            {
                //将已经审批的审批记录，按照审批时间升序排序，插入到返回结果集中
                recordModels.AddRange(findedRecords.FindAll(t => t.FApproval == 2 || t.FApproval == 9).OrderBy(t => t.FDate));

                foreach(var rec in findedRecords)
                {
                    if (rec.PostPhid == 0)
                    {
                        rec.JudgeRefer = 1;
                    }
                    else
                    {
                        rec.JudgeRefer = 0;
                    }
                }
                //审批过的记录增加附件信息
                foreach(var rec in recordModels)
                {
                    dic.Clear();
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<string>.Eq("BTable", "SP3_APPVAL_RECORD"))
                        .Add(ORMRestrictions<long>.Eq("RelPhid", rec.PhId));
                    var attachs = this.QtAttachmentRule.Find(dic);
                    if (attachs.Count > 0)
                    {
                        rec.QtAttachments = attachs.ToList();
                    }
                }

                List<GAppvalRecordModel> unDoRecords = findedRecords.FindAll(t => t.FApproval == 1 && t.ProcPhid == proc_phid);
                if (unDoRecords.Count > 0)
                {
                    int minSeq = postModels.Find(t => t.PhId == unDoRecords[0].PostPhid).Seq;
                    for (int i = minSeq; i <= maxSeq; i++)
                    {
                        //还未审批的岗位，审批人显示的是审批岗位的名称，不是这个岗位的一个或多个审批人的名字
                        GAppvalRecordModel recordModel = new GAppvalRecordModel();
                        GAppvalPostModel presentPost = postModels.Find(t => t.Seq == i);
                        recordModel.FApproval = 1;
                        recordModel.PostName = presentPost.FName;
                        recordModel.PostPhid = presentPost.PhId;
                        recordModel.ProcPhid = proc_phid;
                        recordModel.RefbillPhid = phid;
                        recordModel.FBilltype = billType;
                        recordModel.FSendDate = DateTime.Now;
                        recordModel.FSeq = presentPost.Seq;
                        recordModels.Add(recordModel);
                    }
                }
            }
            if(billType == BillType.PayMent)
            {
                GAppvalRecordModel recordModel = new GAppvalRecordModel();
                //GKPaymentMstModel gKPayment = new GKPaymentMstModel();               
                dic.Clear();
                new CreateCriteria(dic)
                    .Add(ORMRestrictions<long>.Eq("PhId", phid));
                var gKPayments = this.GKPaymentMstRule.Find(dic);
                if(gKPayments.Count > 0)
                {
                    if(gKPayments[0].FSubmitterId != 0)
                    {
                        recordModel.FBilltype = billType;
                        recordModel.JudgeRefer = 2;                        
                        recordModel.IsPay = gKPayments[0].FState;
                        if(gKPayments[0].FState == (byte)0)
                        {
                            recordModel.FApproval = (byte)1;
                        }
                        else if (gKPayments[0].FState == (byte)1)
                        {
                            recordModel.FApproval = (byte)9;
                        }
                        else if (gKPayments[0].FState == (byte)2)
                        {
                            recordModel.FApproval = (byte)2;
                        }
                        else
                        {
                            recordModel.FApproval = (byte)1;
                        }
                        dic.Clear();
                        new CreateCriteria(dic)
                                .Add(ORMRestrictions<long>.Eq("PhId", gKPayments[0].FSubmitterId));
                        IList<User2Model> users = UserRule.Find(dic);
                        if(users.Count > 0)
                        {
                            recordModel.OperaName = users[0].UserName;
                        }
                        recordModels.Add(recordModel);
                    }
                }
            }
            foreach(var rm in recordModels)
            {
                var pm = postModels.Find(t => t.PhId == rm.PostPhid);
                if (pm != null)
                {
                    rm.IsMode = pm.FMode;
                }
                else
                {
                    rm.IsMode = 0;
                }
            }
            return recordModels;
        }

        /// <summary>
        /// 根据流程获取所有岗位以及操作员名字
        /// </summary>
        /// <param name="procPhid">流程主键</param>
        /// <returns></returns>
        public List<GAppvalRecordModel> GetAllPostsAndOpersByProc(long procPhid)
        {
            List<GAppvalRecordModel> gAppvalRecords = new List<GAppvalRecordModel>();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic)
                    .Add(ORMRestrictions<long>.Eq("ProcPhid", procPhid));
            //根据流程获取所有的流程岗位集合
            var procPosts = this.GAppvalProc4PostRule.Find(dic, new string[] { " FSeq Asc" });
            if(procPosts.Count > 0)
            {
                //获取所有岗位的主键集合
                var postIds = procPosts.ToList().Select(t => t.PostPhid).ToList();
                dic.Clear();
                new CreateCriteria(dic)
                        .Add(ORMRestrictions<List<long>>.In("PhId", postIds));
                var posts = this.GAppvalPostRule.Find(dic);
                if(posts.Count > 0)
                {
                    dic.Clear();
                    new CreateCriteria(dic)
                            .Add(ORMRestrictions<List<long>>.In("PostPhid", postIds));
                    //所有岗位操作员列表集合
                    var postOpers = this.GAppvalPost4OperRule.Find(dic);
                    foreach(var post in posts)
                    {
                        //单个岗位所对应的操作员逐渐集合
                        var operIds = postOpers.ToList().FindAll(t=>t.PostPhid == post.PhId).Select(t =>t.OperatorPhid).ToList();
                        dic.Clear();
                        new CreateCriteria(dic)
                                .Add(ORMRestrictions<List<long>>.In("PhId", operIds));
                        IList<User2Model> users = UserRule.Find(dic);
                        GAppvalRecordModel gAppval = new GAppvalRecordModel();
                        gAppval.OperaName = "";
                        if (users.Count > 0)
                        {
                            foreach(var user in users)
                            {
                                if(gAppval.OperaName == "")
                                {
                                    gAppval.OperaName = gAppval.OperaName + user.UserName;
                                }
                                else
                                {
                                    gAppval.OperaName = gAppval.OperaName + ", " + user.UserName;
                                }
                                
                            }
                        }
                        gAppval.PostPhid = post.PhId;
                        gAppval.PostName = post.FName;
                        gAppval.IsMode = procPosts.ToList().Find(t => t.PostPhid == post.PhId).FMode;
                        gAppval.FSeq = procPosts.ToList().Find(t => t.PostPhid == post.PhId).FSeq;
                        gAppvalRecords.Add(gAppval);
                    }
                }
            }
            gAppvalRecords = gAppvalRecords.OrderBy(t => t.FSeq).ToList();
            return gAppvalRecords;
        }

        /// <summary>
        /// 根据单据号与单据类型取消单据送审
        /// </summary>
        /// <param name="gAppval"></param>
        /// <returns></returns>
        public SavedResult<long> PostCancelAppvalRecord(GAppvalRecordModel gAppval)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            //取消送审，1、把审批记录删除，2、相关附件删除，3、主表状态改变
            //先判断所选单据是否有资格取消送审
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic).Add(ORMRestrictions<List<long>>.In("PhId", gAppval.RefbillPhidList));
            if(gAppval.FBilltype == BillType.FundsPay)
            {
                var payments = this.PaymentMstRule.Find(dic);
                if(payments.Count > 0)
                {
                    foreach(var payment in payments)
                    {
                        if(payment.FApproval == (byte)Approval.Pass)
                        {
                            throw new Exception(payment.FName+"单据已审批完成，无法取消送审！");
                        }
                        if (payment.FApproval == (byte)Approval.Send)
                        {
                            throw new Exception(payment.FName + "单据未送审，无法取消送审！");
                        }
                        if (payment.FApproval == (byte)Approval.UnPass)
                        {
                            throw new Exception(payment.FName + "单据未通过，无法取消送审！");
                        }
                        if(payment.FDelete == (byte)DeleteType.Yes)
                        {
                            throw new Exception(payment.FName + "单据已作废，无法取消送审！");
                        }
                    }
                }
            }
            else if (gAppval.FBilltype == BillType.PayMent)
            {
                var gkPayments = this.GKPaymentMstRule.Find(dic);
                if (gkPayments.Count > 0)
                {
                    foreach (var gkPayment in gkPayments)
                    {
                        if (gkPayment.FApproval == (byte)Approval.Pass)
                        {
                            throw new Exception("支付单编号为" + gkPayment.FCode + "的单据已审批完成，无法取消送审！");
                        }
                        if (gkPayment.FApproval == (byte)Approval.Send)
                        {
                            throw new Exception("支付单编号为" + gkPayment.FCode + "的单据未送审，无法取消送审！");
                        }
                        if (gkPayment.FApproval == (byte)Approval.UnPass)
                        {
                            throw new Exception("支付单编号为" + gkPayment.FCode + "的单据未通过，无法取消送审！");
                        }
                        if (gkPayment.FDelete == (byte)DeleteType.Yes)
                        {
                            throw new Exception("支付单编号为" + gkPayment.FCode + "的单据已作废，无法取消送审！");
                        }
                    }
                }
            }
            else if (gAppval.FBilltype == BillType.BeginProject || gAppval.FBilltype==BillType.MiddleProject)
            {                
                var projects = this.ProjectMstRule.Find(dic);
                if (projects.Count > 0)
                {
                    foreach (var pro in projects)
                    {
                        if (pro.FApproveStatus == "3")
                        {
                            throw new Exception("项目单编号为" + pro.FProjCode + "的单据已审批完成，无法取消送审！");
                        }
                        if (pro.FApproveStatus == "1")
                        {
                            throw new Exception("项目单编号为" + pro.FProjCode + "的单据未送审，无法取消送审！");
                        }
                        if (pro.FApproveStatus == "4")
                        {
                            throw new Exception("项目单编号为" + pro.FProjCode + "的单据未通过，无法取消送审！");
                        }
                        if (pro.FLifeCycle != 0)
                        {
                            throw new Exception("项目单编号为" + pro.FProjCode + "的单据已作废，无法取消送审！");
                        }
                    }
                }
            }
            else if (gAppval.FBilltype == BillType.MiddleAddBudget || gAppval.FBilltype == BillType.MiddleUpdateBudget || gAppval.FBilltype == BillType.MiddleDtlBudget || gAppval.FBilltype == BillType.MiddleBudget)
            {
                var budgets = this.BudgetMstRule.Find(dic);
                if (budgets.Count > 0)
                {
                    foreach (var bud in budgets)
                    {
                        if (bud.FApproveStatus == "3")
                        {
                            throw new Exception("预算单编号为" + bud.FProjCode + "的单据已审批完成，无法取消送审！");
                        }
                        if (bud.FApproveStatus == "1")
                        {
                            throw new Exception("预算单编号为" + bud.FProjCode + "的单据未送审，无法取消送审！");
                        }
                        if (bud.FApproveStatus == "4")
                        {
                            throw new Exception("预算单编号为" + bud.FProjCode + "的单据未通过，无法取消送审！");
                        }
                        if (bud.FLifeCycle != 0)
                        {
                            throw new Exception("预算单编号为" + bud.FProjCode + "的单据已作废，无法取消送审！");
                        }
                    }
                }
            }
            else if (gAppval.FBilltype == BillType.Expense)//用款计划
            {
                var expenses = this.ExpenseMstRule.Find(dic);
                if (expenses.Count > 0)
                {
                    foreach (var exp in expenses)
                    {
                        if (exp.FApprovestatus == "3")
                        {
                            throw new Exception("预算单编号为" + exp.FProjcode + "的单据已审批完成，无法取消送审！");
                        }
                        if (exp.FApprovestatus == "1")
                        {
                            throw new Exception("预算单编号为" + exp.FProjcode + "的单据未送审，无法取消送审！");
                        }
                        if (exp.FApprovestatus == "4")
                        {
                            throw new Exception("预算单编号为" + exp.FProjcode + "的单据未通过，无法取消送审！");
                        }
                        if (exp.FLifeCycle != 0)
                        {
                            throw new Exception("预算单编号为" + exp.FProjcode + "的单据已作废，无法取消送审！");
                        }
                    }
                }
            }
            else if (gAppval.FBilltype == BillType.InComeBudget)
            {
                var payments = this.YsIncomeMstRule.Find(dic);
                if (payments.Count > 0)
                {
                    foreach (var payment in payments)
                    {
                        if (payment.FApproval == (byte)Approval.Pass)
                        {
                            throw new Exception("单据已审批完成，无法取消送审！");
                        }
                        if (payment.FApproval == (byte)Approval.Send)
                        {
                            throw new Exception("单据未送审，无法取消送审！");
                        }
                        if (payment.FApproval == (byte)Approval.UnPass)
                        {
                            throw new Exception("单据未通过，无法取消送审！");
                        }
                        //if (payment.FDelete == (byte)DeleteType.Yes)
                        //{
                        //    throw new Exception(payment.FName + "单据已作废，无法取消送审！");
                        //}
                    }
                }
            }
            else
            {
                throw new Exception("传递的单据类型有误！");
            }

            //先判断只有送审人可以取消送审
            dic.Clear();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<List<long>>.In("RefbillPhid", gAppval.RefbillPhidList))
                .Add(ORMRestrictions<string>.Eq("FBilltype", gAppval.FBilltype))
                .Add(ORMRestrictions<long>.Eq("PostPhid", (long)0));
            var appvals2 = this.GAppvalRecordRule.Find(dic);
            GAppvalRecordModel newGAppval = new GAppvalRecordModel();
            if (appvals2 != null && appvals2.Count > 0)
            {
                newGAppval = appvals2.OrderByDescending(t => t.FSendDate).ToList()[0];
                //foreach (var appval in appvals2)
                //{
                //    if (appval.OperaPhid != gAppval.OperaPhid)
                //    {
                //        throw new Exception("您不是所选单据的送审人，所以不能取消送审!");
                //    }
                //}
                if (newGAppval.OperaPhid != gAppval.OperaPhid)
                {
                    throw new Exception("您不是所选单据的送审人，所以不能取消送审!");
                }
            }



            dic.Clear();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<List<long>>.In("RefbillPhid", gAppval.RefbillPhidList))
                .Add(ORMRestrictions<string>.Eq("FBilltype", gAppval.FBilltype))
                .Add(ORMRestrictions<long>.NotEq("PostPhid", (long)0))
                .Add(ORMRestrictions<DateTime>.Ge("FSendDate", (DateTime)newGAppval.FSendDate));
            var appvals = this.GAppvalRecordRule.Find(dic);
            if(appvals.Count > 0)
            {
                foreach(var appval in appvals)
                {
                    if(appval.FApproval != (byte)Approval.Wait)
                    {
                        throw new Exception("所选单据已被审批，无法取消送审！");
                    }
                }
            }


            //先删除审批记录
            dic.Clear();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<List<long>>.In("RefbillPhid", gAppval.RefbillPhidList))
                .Add(ORMRestrictions<string>.Eq("FBilltype", gAppval.FBilltype));
            var appvalRecords = this.GAppvalRecordRule.Find(dic);
            if(appvalRecords.Count > 0)
            {
                //删审批记录
                this.GAppvalRecordRule.Delete(dic);
                //再删除附件
                var firstList = appvalRecords.ToList().FindAll(t => t.PostPhid == (long)0);
                if(firstList.Count >0)
                {
                    var firstPhids = firstList.Select(t => t.PhId).ToList();
                    dic.Clear();
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<List<long>>.In("RelPhid", firstPhids))
                        .Add(ORMRestrictions<string>.Eq("BTable", "SP3_APPVAL_RECORD"));
                    var attachs = this.QtAttachmentRule.Find(dic);
                    if(attachs.Count > 0)
                    {
                        var paths = attachs.Select(t => t.BUrlpath).ToList();
                        foreach(var path in paths)
                        {
                            var allPath = HttpContext.Current.Request.MapPath("~" + path);
                            //删除文件
                            if (File.Exists(allPath))
                            {
                                File.Delete(allPath);
                            }
                            else
                            {
                                throw new Exception("附件不存在！");
                            }
                        }
                        //删附件表数据
                        this.QtAttachmentRule.Delete(dic);
                    }
                }                
            }
            //修改主表状态
            dic.Clear();
            new CreateCriteria(dic).Add(ORMRestrictions<List<long>>.In("PhId", gAppval.RefbillPhidList));
            if (gAppval.FBilltype == BillType.FundsPay)
            {
                var updatePayments = this.PaymentMstRule.Find(dic);
                if(updatePayments.Count > 0)
                {
                    foreach(var updatePayment in updatePayments)
                    {
                        updatePayment.FApproval = (byte)Approval.Send;
                        updatePayment.PersistentState = PersistentState.Modified;
                    }
                    savedResult = this.PaymentMstRule.Save<long>(updatePayments);
                }
            }
            else if (gAppval.FBilltype == BillType.PayMent)
            {
                var updateGkPayments = this.GKPaymentMstRule.Find(dic);
                if(updateGkPayments.Count > 0)
                {
                    foreach(var updateGkPayment in updateGkPayments)
                    {
                        updateGkPayment.FApproval = (byte)Approval.Send;
                        updateGkPayment.PersistentState = PersistentState.Modified;
                    }
                    savedResult = this.GKPaymentMstRule.Save<long>(updateGkPayments);
                }
            }
            else if (gAppval.FBilltype == BillType.BeginProject || gAppval.FBilltype == BillType.MiddleProject)
            {
                var updateProjects = this.ProjectMstRule.Find(dic);
                if (updateProjects.Count > 0)
                {
                    foreach (var updateProject in updateProjects)
                    {
                        updateProject.FApproveStatus = "1";
                        updateProject.PersistentState = PersistentState.Modified;
                    }
                    savedResult = this.ProjectMstRule.Save<long>(updateProjects);
                }
            }
            else if (gAppval.FBilltype == BillType.MiddleAddBudget || gAppval.FBilltype == BillType.MiddleUpdateBudget || gAppval.FBilltype == BillType.MiddleDtlBudget || gAppval.FBilltype == BillType.MiddleBudget)
            {
                var updateBudgets = this.BudgetMstRule.Find(dic);
                if (updateBudgets.Count > 0)
                {
                    foreach (var updateBudget in updateBudgets)
                    {
                        updateBudget.FApproveStatus = "1";
                        updateBudget.PersistentState = PersistentState.Modified;
                    }
                    savedResult = this.BudgetMstRule.Save<long>(updateBudgets);
                }
            }
            else if (gAppval.FBilltype == BillType.Expense)//用款计划
            {
                var updateExpenses = this.ExpenseMstRule.Find(dic);
                if (updateExpenses.Count > 0)
                {
                    foreach (var updateExpense in updateExpenses)
                    {
                        updateExpense.FApprovestatus = "1";
                        updateExpense.PersistentState = PersistentState.Modified;
                    }
                    savedResult = this.ExpenseMstRule.Save<long>(updateExpenses);
                }
            }
            else if (gAppval.FBilltype == BillType.InComeBudget)
            {
                var updateInComes = this.YsIncomeMstRule.Find(dic);
                if(updateInComes != null && updateInComes.Count > 0)
                {
                    foreach(var inCome in updateInComes)
                    {
                        inCome.FApproval = 0;
                        inCome.PersistentState = PersistentState.Modified;
                    }
                    savedResult = this.YsIncomeMstRule.Save<long>(updateInComes);
                }
            }
            return savedResult;
        }
        #endregion
    }
}

