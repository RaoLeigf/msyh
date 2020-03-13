#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Service
    * 类 名 称：			GAppvalRecordService
    * 文 件 名：			GAppvalRecordService.cs
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
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Service;

using GSP3.SP.Service.Interface;
using GSP3.SP.Facade.Interface;
using GSP3.SP.Model.Domain;
using Enterprise3.WebApi.GSP3.SP.Model.Response;
using Enterprise3.WebApi.GSP3.SP.Model.Request;
using GBK3.BK.Facade.Interface;
using GSP3.SP.Model.Enums;
using SUP.Common.Base;
using GGK3.GK.Facade.Interface;
using Enterprise3.Common.Base.Criterion;
using GQT3.QT.Facade.Interface;
using GQT3.QT.Model.Domain;
using GBK3.BK.Model.Enums;
using GXM3.XM.Facade.Interface;
using GXM3.XM.Model.Domain;
using GYS3.YS.Facade.Interface;
using GYS3.YS.Model.Domain;

namespace GSP3.SP.Service
{
	/// <summary>
	/// GAppvalRecord服务组装处理类
	/// </summary>
    public partial class GAppvalRecordService : EntServiceBase<GAppvalRecordModel>, IGAppvalRecordService
    {
		#region 类变量及属性
		/// <summary>
        /// GAppvalRecord业务外观处理对象
        /// </summary>
		IGAppvalRecordFacade GAppvalRecordFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IGAppvalRecordFacade;
            }
        }

        private IGAppvalPostFacade GAppvalPostFacade { get; set; }

        private IPaymentMstFacade PaymentMstFacade { get; set; }

        private IGAppvalPost4OperFacade GAppvalPost4OperFacade { get; set; }

        private IGKPaymentMstFacade GKPaymentMstFacade { get; set; }

        private IOrganizationFacade OrganizationFacade { get; set; }

        private IQtAttachmentFacade QtAttachmentFacade { get; set; }

        private IProjectMstFacade ProjectMstFacade { get; set; }

        private IBudgetMstFacade BudgetMstFacade { get; set; }

        private IExpenseMstFacade ExpenseMstFacade { get; set; }

        private IBudgetProcessCtrlFacade BudgetProcessCtrlFacade { get; set; }

        private IYsIncomeMstFacade YsIncomeMstFacade { get; set; }
        #endregion

        #region 实现 IGAppvalRecordService 业务添加的成员

        /// <summary>
        /// 分页获取待我审批单据数据
        /// </summary>
        /// <param name="billRequest"></param>
        /// <param name="total">总条数</param>
        /// <returns></returns>
        public List<AppvalRecordVo> GetUnDoRecordList(BillRequestModel billRequest, out int total) {
            List<AppvalRecordVo> recordVos = new List<AppvalRecordVo>();
            OrganizeModel Org = OrganizationFacade.Find(billRequest.Orgid).Data;
            billRequest.OrgCode = Org.OCode;
            //recordVos = GAppvalRecordFacade.GetUnDoRecords(billRequest,out total);
            recordVos = GAppvalRecordFacade.GetUnDoRecords2(billRequest, out total);
            return recordVos;
        }

        /// <summary>
        /// 分页获取已审批单据数据
        /// </summary>
        /// <param name="billRequest"></param>
        /// <param name="total">总条数</param>
        /// <returns></returns>
        public List<AppvalRecordVo> GetDoneRecordList(BillRequestModel billRequest, out int total) {
            List<AppvalRecordVo> recordVos = new List<AppvalRecordVo>();
            OrganizeModel Org = OrganizationFacade.Find(billRequest.Orgid).Data;
            billRequest.OrgCode = Org.OCode;
            //recordVos = GAppvalRecordFacade.GetDoneRecordList(billRequest,out total);
            recordVos = GAppvalRecordFacade.GetDoneRecordList2(billRequest, out total);
            return recordVos;
        }

        /// <summary>
        /// 审批流查看
        /// </summary>
        /// <param name="phid">单据phid</param>
        /// <param name="proc_phid">审批流程id</param>
        /// <param name="billType">单据类型</param>
        /// <returns></returns>
        public List<GAppvalRecordModel> GetAppvalRecord(long phid,long proc_phid,string billType) {
            List<GAppvalRecordModel> recordModels = new List<GAppvalRecordModel>();

            if (phid == 0)
                return recordModels;

            //根据关联单据id查询所有的审批记录
            List<GAppvalRecordModel> findedRecords = GAppvalRecordFacade.FindByRelId(phid,billType);
            //根据审批流程找到所有的审批岗位
            List<GAppvalPostModel> postModels = GAppvalPostFacade.FindAppvalPostByProcID(proc_phid);
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
                    recordModels.Add(recordModel);
                }
            }
            else {
                //将已经审批的审批记录，按照审批时间升序排序，插入到返回结果集中
                recordModels.AddRange(findedRecords.FindAll(t => t.FApproval == 2 || t.FApproval == 9).OrderBy(t => t.FDate));

                List<GAppvalRecordModel> unDoRecords = findedRecords.FindAll(t => t.FApproval == 1);
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
                        recordModel.FSeq = presentPost.Seq;
                        recordModels.Add(recordModel);
                    }
                }
            }

            return recordModels;
        }

        /// <summary>
        /// 审批流查看
        /// </summary>
        /// <param name="phid">单据主键</param>
        /// <param name="billType">单据类型</param>
        /// <returns></returns>
        public List<GAppvalRecordModel> GetAppvalRecordList(long phid, string billType)
        {
            List<GAppvalRecordModel> recordModels = new List<GAppvalRecordModel>();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if(billType == BillType.FundsPay)
            {
                IList<GAppvalRecordModel> records = GAppvalRecordFacade.Find(t => t.RefbillPhid == phid && t.FBilltype == billType).Data.OrderByDescending(t => t.FSendDate).ToList();
                if(records!= null && records.Count > 0)
                {
                    var result = this.GAppvalRecordFacade.GetAppvalRecordList(phid, records[0].ProcPhid, billType);
                    if(result.Count > 0)
                    {
                        foreach(var record in result)
                        {
                            recordModels.Add(record);
                        }
                    }
                }
                dic.Clear();
                new CreateCriteria(dic).Add(ORMRestrictions<long>.Eq("RefbillPhid", phid));
                var gKpayments = this.GKPaymentMstFacade.Find(dic, new string[] { "NgInsertDt Asc" }).Data;
                if(gKpayments.Count > 0)
                {
                    foreach(var gKpayment in gKpayments)
                    {
                        IList<GAppvalRecordModel> records2 = GAppvalRecordFacade.Find(t => t.RefbillPhid == gKpayment.PhId && t.FBilltype == BillType.PayMent).Data.OrderByDescending(t => t.FSendDate).ToList();
                        if (records2 != null && records2.Count > 0)
                        {
                            var result = this.GAppvalRecordFacade.GetAppvalRecordList(gKpayment.PhId, records2[0].ProcPhid, BillType.PayMent);
                            if (result.Count > 0)
                            {
                                foreach (var record in result)
                                {
                                    recordModels.Add(record);
                                }
                            }
                        }
                    }

                }
            }
            if (billType == BillType.PayMent)
            {
                dic.Clear();
                new CreateCriteria(dic).Add(ORMRestrictions<long>.Eq("PhId", phid));
                var gKpayment = this.GKPaymentMstFacade.Find(dic).Data;
                if (gKpayment.Count > 0)
                {
                    IList<GAppvalRecordModel> records2 = GAppvalRecordFacade.Find(t => t.RefbillPhid == gKpayment[0].RefbillPhid && t.FBilltype == gKpayment[0].FBilltype).Data.OrderByDescending(t => t.FSendDate).ToList();
                    if (records2 != null && records2.Count > 0)
                    {
                        var result = this.GAppvalRecordFacade.GetAppvalRecordList(gKpayment[0].RefbillPhid, records2[0].ProcPhid, gKpayment[0].FBilltype);
                        if (result.Count > 0)
                        {
                            foreach (var record in result)
                            {
                                recordModels.Add(record);
                            }
                        }
                    }
                }
                IList<GAppvalRecordModel> records = GAppvalRecordFacade.Find(t => t.RefbillPhid == phid && t.FBilltype == billType).Data.OrderByDescending(t=>t.FSendDate).ToList();
                if (records != null && records.Count > 0)
                {
                    var result = this.GAppvalRecordFacade.GetAppvalRecordList(phid, records[0].ProcPhid, billType);
                    if (result.Count > 0)
                    {
                        foreach (var record in result)
                        {
                            recordModels.Add(record);
                        }
                    }
                    //var procPhids = records.GroupBy(t=>t.ProcPhid);
                    //if(procPhids.Count() > 0)
                    //{
                    //    foreach(var item in procPhids)
                    //    {
                    //        var result = this.GAppvalRecordFacade.GetAppvalRecordList(phid, item.Select(t=>t.ProcPhid).First(), billType);
                    //        if (result.Count > 0)
                    //        {
                    //            foreach (var record in result)
                    //            {
                    //                recordModels.Add(record);
                    //            }
                    //        }
                    //    }
                    //}

                }
            }
            if(billType == BillType.BeginProject || billType == BillType.MiddleProject || billType == BillType.ExpendBudeget)
            {
                dic.Clear();
                new CreateCriteria(dic).Add(ORMRestrictions<long>.Eq("PhId", phid));
                var gKpayment = this.ProjectMstFacade.Find(dic).Data;
                if (gKpayment.Count > 0)
                {
                    IList<GAppvalRecordModel> records2 = GAppvalRecordFacade.Find(t => t.RefbillPhid == gKpayment[0].PhId && t.FBilltype == BillType.BeginProject).Data.OrderByDescending(t => t.FSendDate).ToList();
                    if (records2 != null && records2.Count > 0)
                    {
                        var result = this.GAppvalRecordFacade.GetAppvalRecordList(gKpayment[0].PhId, records2[0].ProcPhid, BillType.BeginProject);
                        if (result.Count > 0)
                        {
                            foreach (var record in result)
                            {
                                recordModels.Add(record);
                            }
                        }
                    }
                    IList<GAppvalRecordModel> records = GAppvalRecordFacade.Find(t => t.RefbillPhid == gKpayment[0].PhId && t.FBilltype == BillType.MiddleProject).Data.OrderByDescending(t => t.FSendDate).ToList();
                    if (records != null && records.Count > 0)
                    {
                        var result = this.GAppvalRecordFacade.GetAppvalRecordList(gKpayment[0].PhId, records[0].ProcPhid, BillType.MiddleProject);
                        if (result.Count > 0)
                        {
                            foreach (var record in result)
                            {
                                recordModels.Add(record);
                            }
                        }
                    }
                    recordModels = recordModels.OrderBy(t => t.FSeq).OrderBy(t=>t.FSendDate).ToList();
                }

            }
            if(billType == BillType.MiddleAddBudget || billType == BillType.MiddleUpdateBudget|| billType == BillType.MiddleDtlBudget || billType == BillType.MiddleBudget)
            {
                dic.Clear();
                new CreateCriteria(dic).Add(ORMRestrictions<long>.Eq("PhId", phid));
                var gKpayment = this.BudgetMstFacade.Find(dic).Data;
                if (gKpayment.Count > 0)
                {
                    IList<GAppvalRecordModel> records2 = GAppvalRecordFacade.Find(t => t.RefbillPhid == gKpayment[0].PhId && t.FBilltype == billType).Data.OrderByDescending(t => t.FSendDate).ToList();
                    if (records2 != null && records2.Count > 0)
                    {
                        var result = this.GAppvalRecordFacade.GetAppvalRecordList(gKpayment[0].PhId, records2[0].ProcPhid, billType);
                        if (result.Count > 0)
                        {
                            foreach (var record in result)
                            {
                                recordModels.Add(record);
                            }
                        }
                    }
                }
            }
            if(billType == BillType.Expense)
            {
                dic.Clear();
                new CreateCriteria(dic).Add(ORMRestrictions<long>.Eq("PhId", phid));
                var gKpayment = this.ExpenseMstFacade.Find(dic).Data;
                if (gKpayment.Count > 0)
                {
                    IList<GAppvalRecordModel> records2 = GAppvalRecordFacade.Find(t => t.RefbillPhid == gKpayment[0].PhId && t.FBilltype == billType).Data.OrderByDescending(t => t.FSendDate).ToList();
                    if (records2 != null && records2.Count > 0)
                    {
                        var result = this.GAppvalRecordFacade.GetAppvalRecordList(gKpayment[0].PhId, records2[0].ProcPhid, billType);
                        if (result.Count > 0)
                        {
                            foreach (var record in result)
                            {
                                recordModels.Add(record);
                            }
                        }
                    }
                }
            }
            if (billType == BillType.InComeBudget)
            {
                dic.Clear();
                new CreateCriteria(dic).Add(ORMRestrictions<long>.Eq("PhId", phid));
                var gKpayment = this.YsIncomeMstFacade.Find(dic).Data;
                if (gKpayment.Count > 0)
                {
                    IList<GAppvalRecordModel> records2 = GAppvalRecordFacade.Find(t => t.RefbillPhid == gKpayment[0].PhId && t.FBilltype == billType).Data.OrderByDescending(t => t.FSendDate).ToList();
                    if (records2 != null && records2.Count > 0)
                    {
                        var result = this.GAppvalRecordFacade.GetAppvalRecordList(gKpayment[0].PhId, records2[0].ProcPhid, billType);
                        if (result.Count > 0)
                        {
                            foreach (var record in result)
                            {
                                recordModels.Add(record);
                            }
                        }
                    }
                }
            }
            return recordModels;
        }

        /// <summary>
        /// 审批
        /// </summary>
        /// <param name="recordModel"></param>
        public void PostApprovalRecord(GAppvalRecordModel recordModel) {
            if (recordModel == null) {
                throw new Exception("审批数据为空！");
            }
            
            /*审批的步骤为：
             * ①、更新审批记录表的审批状态，审批的说明，审批时间等字段，
             * 如果当前审批岗位为非会签模式，那么更新完当前审批人的审批记录，删除当前岗位其他审批人的审批记录
             * 如果当前岗位为会签模式，分两种情况：如果审批通过，那么保留当前岗位其他审批人的记录；如果审批不通过，删除当前岗位其他还未审批的审批人的审批记录
             * ②、审批通过，分两种情况，当前审批流程还有下一审批岗位或者当前审批流程已经结束
             * 1).如果当前审批流程还有下一个审批岗位，那么判断当前岗位是否是会签模式，如果是会签模式，只有当前岗位的所有审批人都审批通过，才生成下一岗位审批人的审批记录
             * 如果当前岗位是非会签模式，那么直接生成下一岗位审批人的审批记录
             * 2).如果当前岗位已经是审批流程的最后一个岗位,岗位为非会签模式或者 岗位为会签模式并且所有审批人已经审批通过，才修改关联单据的审批状态为审批通过
             * ③审批不通过，分两种情况，回退到发起人和回退到之前的审批岗位
             * 1).回退到发起人,修改关联单据的审批状态为审批不通过
             * 2).回退到之前的岗位，生成回退岗位审批人的审批记录
             */

            //当前审批流程的审批岗位
            List<GAppvalPostModel> postModels = GAppvalPostFacade.FindAppvalPostByProcID(recordModel.ProcPhid).OrderBy(t => t.Seq).ToList();
            int max_seq = postModels.Select(t => t.Seq).Max();
            int min_seq = postModels.Select(t => t.Seq).Min();
            //当前审批岗位
            GAppvalPostModel presentPost = postModels.Find(t => t.PhId == recordModel.PostPhid);
            //当前岗位的审批记录
            List<GAppvalRecordModel> recordModels = GAppvalRecordFacade.Find(t => t.RefbillPhid == recordModel.RefbillPhid && t.FBilltype == recordModel.FBilltype && t.PostPhid == presentPost.PhId).Data.ToList();
            //当前岗位审批通过的审批记录
            List<GAppvalRecordModel> passRecords = recordModels.FindAll(t => t.FApproval == (byte)Approval.Pass);

            //更新审批记录
            SavedResult<Int64> savedResult = GAppvalRecordFacade.UpdateApprovalRecord(recordModel, presentPost, recordModels);
            if (savedResult == null || savedResult.SaveRows == 0) {
                throw new Exception("更新审批记录失败！");
            }

            if (recordModel.FApproval == (byte)Approval.UnPass)
            {
                #region 审批不通过
                
                //如果退回到发起人(也就是岗位id为0),修改单据的审批状态为不通过
                if (recordModel.BackPostPhid == 0)
                {
                    if (BillType.FundsPay == recordModel.FBilltype)
                    {
                        PaymentMstFacade.UpdatePayment(recordModel.RefbillPhid, (byte)Approval.UnPass);
                    }
                    if (BillType.PayMent == recordModel.FBilltype) {
                        GKPaymentMstFacade.UpdatePaymentApprovalState(recordModel.RefbillPhid, (byte)Approval.UnPass);
                    }
                    if (BillType.BeginProject == recordModel.FBilltype || BillType.MiddleProject == recordModel.FBilltype)
                    {
                        this.ProjectMstFacade.UpdateProject(recordModel, "4");
                    }
                    if (BillType.MiddleAddBudget == recordModel.FBilltype || BillType.MiddleUpdateBudget == recordModel.FBilltype || BillType.MiddleDtlBudget == recordModel.FBilltype || recordModel.FBilltype == BillType.MiddleBudget)
                    {
                        this.BudgetMstFacade.UpdateBudget(recordModel, "4");
                    }
                    if (BillType.Expense == recordModel.FBilltype)//用款计划
                    {
                        this.ExpenseMstFacade.UpdateExpense(recordModel, "4");
                    }
                }
                else {
                    //回退给之前的岗位
                    GAppvalPostModel backApprovalPost = GAppvalPostFacade.Find(recordModel.BackPostPhid).Data;
                    if (backApprovalPost == null)
                    {
                        throw new Exception("回退岗位不存在！");
                    }
                    if (recordModel.NextOperators == null || recordModel.NextOperators.Count == 0)
                    {
                        throw new Exception("未选择回退审批人！");
                    }
                    //回退岗位的操作员
                    List<GAppvalPost4OperModel> operators = GAppvalPost4OperFacade.Find(t => t.PostPhid == recordModel.BackPostPhid).Data.ToList();

                    if (operators.Count > recordModel.NextOperators.Count && backApprovalPost.FMode == 1) {
                        throw new Exception("岗位为会签模式,必须选择所有审批人！");
                    }

                    operators = operators.FindAll(t => recordModel.NextOperators.Contains(t.OperatorPhid));

                    List<GAppvalRecordModel> addRecords = new List<GAppvalRecordModel>();
                    foreach (GAppvalPost4OperModel model in operators)
                    {
                        addRecords.Add(new GAppvalRecordModel
                        {
                            RefbillPhid = recordModel.RefbillPhid,
                            FBilltype = recordModel.FBilltype,
                            ProcPhid = recordModel.ProcPhid,
                            PostPhid = backApprovalPost.PhId,
                            OperaPhid = model.OperatorPhid,
                            OperatorCode = model.OperatorCode,
                            FSeq = backApprovalPost.Seq,
                            FSendDate = DateTime.Now,
                            FApproval = (Byte)Approval.Wait,
                            PersistentState = PersistentState.Added
                        });
                    }
                    GAppvalRecordFacade.Save<Int64>(addRecords);
                }

                #endregion
            }
            else if (recordModel.FApproval == (byte)Approval.Pass) {
                #region 审批通过

                
                if (recordModels.Count == 0) {
                    throw new Exception("审批记录不存在！");
                }

                //如果当前岗位是最后一个岗位,判断是否更新单据的审批状态,
                //如果当前岗位不是最后一个岗位,生成下一个审批岗位的审批记录
                if (max_seq == presentPost.Seq)
                {
                    if (presentPost.FMode == (Byte)ModeType.No || (presentPost.FMode == (Byte)ModeType.Yes && passRecords.Count == recordModels.Count-1)) {
                        if (BillType.FundsPay == recordModel.FBilltype)
                        {
                            PaymentMstFacade.UpdatePayment(recordModel.RefbillPhid, (byte)Approval.Pass);
                        }
                        if (BillType.PayMent == recordModel.FBilltype)
                        {
                            GKPaymentMstFacade.UpdatePaymentApprovalState(recordModel.RefbillPhid, (byte)Approval.Pass);
                        }
                        if (BillType.BeginProject == recordModel.FBilltype || BillType.MiddleProject == recordModel.FBilltype)
                        {
                            this.ProjectMstFacade.UpdateProject(recordModel, "3");
                        }
                        if(BillType.ExpendBudeget == recordModel.FBilltype)
                        {
                            this.ProjectMstFacade.UpdateExpenProject(recordModel, "3");
                        }
                        //if(BillType.ExpendBudeget == recordModel.FBilltype)
                        if (BillType.MiddleAddBudget == recordModel.FBilltype || BillType.MiddleUpdateBudget == recordModel.FBilltype || BillType.MiddleDtlBudget == recordModel.FBilltype || recordModel.FBilltype == BillType.MiddleBudget)
                        {
                            this.BudgetMstFacade.UpdateBudget(recordModel, "3");
                        }
                        if (BillType.Expense == recordModel.FBilltype)//用款计划
                        {
                            this.ExpenseMstFacade.UpdateExpense(recordModel, "3");
                        }
                    }
                }
                else {
                    //如果是工作流审批，要把审批人信息同步到主表内
                    if (BillType.BeginProject == recordModel.FBilltype || BillType.MiddleProject == recordModel.FBilltype)
                    {
                        this.ProjectMstFacade.UpdateProject(recordModel, "2");
                    }
                    if (BillType.MiddleAddBudget == recordModel.FBilltype || BillType.MiddleUpdateBudget == recordModel.FBilltype || BillType.MiddleDtlBudget == recordModel.FBilltype || recordModel.FBilltype == BillType.MiddleBudget)
                    {
                        this.BudgetMstFacade.UpdateBudget(recordModel, "2");
                    }
                    if (BillType.Expense == recordModel.FBilltype)//用款计划
                    {
                        this.ExpenseMstFacade.UpdateExpense(recordModel, "2");
                    }

                    //下一审批岗位
                    GAppvalPostModel nextPost = postModels.Find(t => t.Seq > presentPost.Seq);

                    if (recordModel.NextOperators == null || recordModel.NextOperators.Count == 0) {
                        throw new Exception("未选择下一审批人！");
                    }
                    //下一审批岗位的审批人
                    List<GAppvalPost4OperModel> operators = GAppvalPost4OperFacade.Find(t => t.PostPhid == nextPost.PhId).Data.ToList();
                    
                    //当前岗位为非会签模式,或者当前岗位的审批人都审批通过,才生成下一审批人的待审批记录
                    if (presentPost.FMode == (Byte)ModeType.No || (presentPost.FMode == (Byte)ModeType.Yes && passRecords.Count == recordModels.Count - 1))
                    {
                        operators = operators.FindAll(t => recordModel.NextOperators.Contains(t.OperatorPhid));

                        List<GAppvalRecordModel> addRecords = new List<GAppvalRecordModel>();
                        foreach (GAppvalPost4OperModel model in operators)
                        {
                            addRecords.Add(new GAppvalRecordModel
                            {
                                RefbillPhid = recordModel.RefbillPhid,
                                FBilltype = recordModel.FBilltype,
                                ProcPhid = recordModel.ProcPhid,
                                PostPhid = nextPost.PhId,
                                OperaPhid = model.OperatorPhid,
                                OperatorCode = model.OperatorCode,
                                FSeq = nextPost.Seq,
                                FSendDate = DateTime.Now,
                                FApproval = (Byte)Approval.Wait,
                                PersistentState = PersistentState.Added
                            });
                        }
                        GAppvalRecordFacade.Save<Int64>(addRecords);
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// 审批(带附件)
        /// </summary>
        /// <param name="recordModel">审批记录对象</param>
        /// <param name="qtAttachments">附件集合</param>
        public void PostApprovalRecordList(GAppvalRecordModel recordModel, List<QtAttachmentModel> qtAttachments)
        {
            if (recordModel == null)
            {
                throw new Exception("审批数据为空！");
            }

            /*审批的步骤为：
             * ①、更新审批记录表的审批状态，审批的说明，审批时间等字段，
             * 如果当前审批岗位为非会签模式，那么更新完当前审批人的审批记录，删除当前岗位其他审批人的审批记录
             * 如果当前岗位为会签模式，分两种情况：如果审批通过，那么保留当前岗位其他审批人的记录；如果审批不通过，删除当前岗位其他还未审批的审批人的审批记录
             * ②、审批通过，分两种情况，当前审批流程还有下一审批岗位或者当前审批流程已经结束
             * 1).如果当前审批流程还有下一个审批岗位，那么判断当前岗位是否是会签模式，如果是会签模式，只有当前岗位的所有审批人都审批通过，才生成下一岗位审批人的审批记录
             * 如果当前岗位是非会签模式，那么直接生成下一岗位审批人的审批记录
             * 2).如果当前岗位已经是审批流程的最后一个岗位,岗位为非会签模式或者 岗位为会签模式并且所有审批人已经审批通过，才修改关联单据的审批状态为审批通过
             * ③审批不通过，分两种情况，回退到发起人和回退到之前的审批岗位
             * 1).回退到发起人,修改关联单据的审批状态为审批不通过
             * 2).回退到之前的岗位，生成回退岗位审批人的审批记录
             */

            //当前审批流程的审批岗位
            List<GAppvalPostModel> postModels = GAppvalPostFacade.FindAppvalPostByProcID(recordModel.ProcPhid).OrderBy(t => t.Seq).ToList();
            int max_seq = postModels.Select(t => t.Seq).Max();
            int min_seq = postModels.Select(t => t.Seq).Min();
            //当前审批岗位
            GAppvalPostModel presentPost = postModels.Find(t => t.PhId == recordModel.PostPhid);
            //当前岗位的审批记录
            List<GAppvalRecordModel> recordModels = GAppvalRecordFacade.Find(t => t.RefbillPhid == recordModel.RefbillPhid && t.FBilltype == recordModel.FBilltype && t.PostPhid == presentPost.PhId && t.ProcPhid == recordModel.ProcPhid && t.FApproval == (byte)Approval.Wait).Data.ToList();
            //当前岗位审批通过的审批记录
            List<GAppvalRecordModel> passRecords = recordModels.FindAll(t => t.FApproval == (byte)Approval.Pass);

            //更新审批记录
            SavedResult<Int64> savedResult = GAppvalRecordFacade.UpdateApprovalRecord(recordModel, presentPost, recordModels);
            if (savedResult == null || savedResult.SaveRows == 0)
            {
                throw new Exception("更新审批记录失败！");
            }
            //保存附件
            SavedResult<long> savedResultRec = new SavedResult<long>();
            if(qtAttachments != null && qtAttachments.Count > 0)
            {
                foreach (var attach in qtAttachments)
                {
                    attach.BTable = "SP3_APPVAL_RECORD";
                    attach.RelPhid = recordModel.PhId;
                }
                savedResultRec = this.QtAttachmentFacade.Save<long>(qtAttachments);
            }           

            if (recordModel.FApproval == (byte)Approval.UnPass)
            {
                #region 审批不通过

                //如果退回到发起人(也就是岗位id为0),修改单据的审批状态为不通过
                if (recordModel.BackPostPhid == 0)
                {
                    if (BillType.FundsPay == recordModel.FBilltype)
                    {
                        PaymentMstFacade.UpdatePayment(recordModel.RefbillPhid, (byte)Approval.UnPass);
                    }
                    if (BillType.PayMent == recordModel.FBilltype)
                    {
                        GKPaymentMstFacade.UpdatePaymentApprovalState(recordModel.RefbillPhid, (byte)Approval.UnPass);
                    }
                    if (BillType.BeginProject == recordModel.FBilltype )
                    {
                        this.ProjectMstFacade.UpdateProject(recordModel, "4");
                    }
                    if(BillType.ExpendBudeget == recordModel.FBilltype)
                    {
                        this.ProjectMstFacade.UpdateExpenProject(recordModel, "4");
                    }
                    if (BillType.MiddleProject == recordModel.FBilltype)
                    {
                        //跨审批流回退
                        if(recordModel.IsSpanBack == 1)
                        {
                            this.ProjectMstFacade.UpdateProject2(recordModel, "4");
                        }
                        else
                        {
                            this.ProjectMstFacade.UpdateProject(recordModel, "4");
                        }
                    }
                    if (BillType.MiddleAddBudget == recordModel.FBilltype || BillType.MiddleUpdateBudget == recordModel.FBilltype || BillType.MiddleDtlBudget == recordModel.FBilltype || recordModel.FBilltype == BillType.MiddleBudget)
                    {
                        this.BudgetMstFacade.UpdateBudget(recordModel, "4");
                    }
                    if (BillType.Expense == recordModel.FBilltype)//用款计划
                    {
                        this.ExpenseMstFacade.UpdateExpense(recordModel, "4");
                    }
                    //收入预算审批
                    if (BillType.InComeBudget == recordModel.FBilltype)
                    {
                        this.YsIncomeMstFacade.UpdateInCome(recordModel.RefbillPhid, (byte)Approval.UnPass);
                    }
                }
                else
                {
                    //回退给之前的岗位
                    GAppvalPostModel backApprovalPost = GAppvalPostFacade.Find(recordModel.BackPostPhid).Data;
                    if (backApprovalPost == null)
                    {
                        throw new Exception("回退岗位不存在！");
                    }
                    if (recordModel.NextOperators == null || recordModel.NextOperators.Count == 0)
                    {
                        throw new Exception("未选择回退审批人！");
                    }
                    //回退岗位的操作员
                    List<GAppvalPost4OperModel> operators = GAppvalPost4OperFacade.Find(t => t.PostPhid == recordModel.BackPostPhid).Data.ToList();

                    if (operators.Count > recordModel.NextOperators.Count && backApprovalPost.FMode == 1)
                    {
                        throw new Exception("岗位为会签模式,必须选择所有审批人！");
                    }

                    operators = operators.FindAll(t => recordModel.NextOperators.Contains(t.OperatorPhid));

                    List<GAppvalRecordModel> addRecords = new List<GAppvalRecordModel>();
                    //若是跨审批流回退
                    if(recordModel.IsSpanBack == 1)
                    {
                        //把项目改成预立项
                        this.ProjectMstFacade.UpdateProject2(recordModel, "2");
                        foreach (GAppvalPost4OperModel model in operators)
                        {
                            addRecords.Add(new GAppvalRecordModel
                            {
                                RefbillPhid = recordModel.RefbillPhid,
                                FBilltype = BillType.BeginProject,
                                ProcPhid = recordModel.BackProcPhid,
                                PostPhid = backApprovalPost.PhId,
                                OperaPhid = model.OperatorPhid,
                                OperatorCode = model.OperatorCode,
                                FSeq = backApprovalPost.Seq,
                                FSendDate = DateTime.Now,
                                FApproval = (Byte)Approval.Wait,
                                PersistentState = PersistentState.Added
                            });
                        }
                    }
                    else
                    {
                        foreach (GAppvalPost4OperModel model in operators)
                        {
                            addRecords.Add(new GAppvalRecordModel
                            {
                                RefbillPhid = recordModel.RefbillPhid,
                                FBilltype = recordModel.FBilltype,
                                ProcPhid = recordModel.ProcPhid,
                                PostPhid = backApprovalPost.PhId,
                                OperaPhid = model.OperatorPhid,
                                OperatorCode = model.OperatorCode,
                                FSeq = backApprovalPost.Seq,
                                FSendDate = DateTime.Now,
                                FApproval = (Byte)Approval.Wait,
                                PersistentState = PersistentState.Added
                            });
                        }
                    }

                    GAppvalRecordFacade.Save<Int64>(addRecords);
                }

                #endregion
            }
            else if (recordModel.FApproval == (byte)Approval.Pass)
            {
                #region 审批通过


                if (recordModels.Count == 0)
                {
                    throw new Exception("审批记录不存在！");
                }

                //如果当前岗位是最后一个岗位,判断是否更新单据的审批状态,
                //如果当前岗位不是最后一个岗位,生成下一个审批岗位的审批记录
                if (max_seq == presentPost.Seq)
                {
                    if (presentPost.FMode == (Byte)ModeType.No || (presentPost.FMode == (Byte)ModeType.Yes && passRecords.Count == recordModels.Count - 1))
                    {
                        if (BillType.FundsPay == recordModel.FBilltype)
                        {
                            PaymentMstFacade.UpdatePayment(recordModel.RefbillPhid, (byte)Approval.Pass);
                        }
                        if (BillType.PayMent == recordModel.FBilltype)
                        {
                            GKPaymentMstFacade.UpdatePaymentApprovalState(recordModel.RefbillPhid, (byte)Approval.Pass);
                        }
                        if (BillType.BeginProject == recordModel.FBilltype || BillType.MiddleProject == recordModel.FBilltype)
                        {
                            this.ProjectMstFacade.UpdateProject(recordModel, "3");
                        }
                        if (BillType.ExpendBudeget == recordModel.FBilltype)
                        {
                            this.ProjectMstFacade.UpdateExpenProject(recordModel, "3");
                        }
                        if (BillType.MiddleAddBudget == recordModel.FBilltype || BillType.MiddleUpdateBudget == recordModel.FBilltype || BillType.MiddleDtlBudget == recordModel.FBilltype || recordModel.FBilltype == BillType.MiddleBudget)
                        {
                            this.BudgetMstFacade.UpdateBudget(recordModel, "3");
                        }
                        if (BillType.Expense == recordModel.FBilltype)//用款计划
                        {
                            this.ExpenseMstFacade.UpdateExpense(recordModel, "3");
                        }
                        if (BillType.InComeBudget == recordModel.FBilltype)
                        {
                            this.YsIncomeMstFacade.UpdateInCome(recordModel.RefbillPhid, (byte)Approval.Pass);
                        }
                    }
                }
                else
                {
                    //如果是工作流审批，要把审批人信息同步到主表内
                    if (BillType.BeginProject == recordModel.FBilltype || BillType.MiddleProject == recordModel.FBilltype)
                    {
                        this.ProjectMstFacade.UpdateProject(recordModel, "2");
                    }
                    if (BillType.MiddleAddBudget == recordModel.FBilltype || BillType.MiddleUpdateBudget == recordModel.FBilltype || BillType.MiddleDtlBudget == recordModel.FBilltype || recordModel.FBilltype == BillType.MiddleBudget)
                    {
                        this.BudgetMstFacade.UpdateBudget(recordModel, "2");
                    }
                    if (BillType.Expense == recordModel.FBilltype)//用款计划
                    {
                        this.ExpenseMstFacade.UpdateExpense(recordModel, "2");
                    }

                    //下一审批岗位
                    GAppvalPostModel nextPost = postModels.Find(t => t.Seq > presentPost.Seq);

                    //if (recordModel.NextOperators == null || recordModel.NextOperators.Count == 0)
                    //{
                    //    throw new Exception("未选择下一审批人！");
                    //}
                    //下一审批岗位的审批人
                    List<GAppvalPost4OperModel> operators = GAppvalPost4OperFacade.Find(t => t.PostPhid == nextPost.PhId).Data.ToList();

                    //当前岗位为非会签模式,或者当前岗位的审批人都审批通过,才生成下一审批人的待审批记录
                    if (presentPost.FMode == (Byte)ModeType.No || (presentPost.FMode == (Byte)ModeType.Yes && passRecords.Count == recordModels.Count - 1))
                    {
                        if (recordModel.NextOperators == null || recordModel.NextOperators.Count == 0)
                        {
                            throw new Exception("10086");
                        }
                        operators = operators.FindAll(t => recordModel.NextOperators.Contains(t.OperatorPhid));

                        List<GAppvalRecordModel> addRecords = new List<GAppvalRecordModel>();
                        foreach (GAppvalPost4OperModel model in operators)
                        {
                            addRecords.Add(new GAppvalRecordModel
                            {
                                RefbillPhid = recordModel.RefbillPhid,
                                FBilltype = recordModel.FBilltype,
                                ProcPhid = recordModel.ProcPhid,
                                PostPhid = nextPost.PhId,
                                OperaPhid = model.OperatorPhid,
                                OperatorCode = model.OperatorCode,
                                FSeq = nextPost.Seq,
                                FSendDate = DateTime.Now,
                                FApproval = (Byte)Approval.Wait,
                                PersistentState = PersistentState.Added
                            });
                        }
                        GAppvalRecordFacade.Save<Int64>(addRecords);
                    }
                }
                #endregion
            }

        }




        /// <summary>
        /// 把数据保存到审批记录表中
        /// </summary>
        /// <param name="gAppval">审批发起记录</param>
        /// <returns></returns>
        public SavedResult<long> AddAppvalRecord(GAppvalRecordModel gAppval)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            if(gAppval.RefbillPhidList != null && gAppval.RefbillPhidList.Count > 0)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                List<byte> approvalList = new List<byte>();
                approvalList.Add((byte)Approval.Send);
                approvalList.Add((byte)Approval.UnPass);
                new CreateCriteria(dic)
                    .Add(ORMRestrictions<List<long>>.In("PhId", gAppval.RefbillPhidList))
                    .Add(ORMRestrictions<List<byte>>.NotIn("FApproval", approvalList));
                if(gAppval.FBilltype == BillType.FundsPay)
                {
                    var result = this.PaymentMstFacade.Find(dic).Data;
                    if(result.Count > 0)
                    {
                        throw new Exception("送审的单据应满足未送审或者已退回状态！");
                    }
                }             
                else if(gAppval.FBilltype == BillType.PayMent)
                {
                    var result = this.GKPaymentMstFacade.Find(dic).Data;
                    if (result.Count > 0)
                    {
                        throw new Exception("送审的单据应满足未送审或者已退回状态！");
                    }
                }
                else if (gAppval.FBilltype == BillType.BeginProject || gAppval.FBilltype== BillType.MiddleProject || gAppval.FBilltype == BillType.ExpendBudeget)
                {
                    dic.Clear();
                    List<string> approvals = new List<string>();
                    approvals.Clear();
                    approvals.Add("1");//项目相关的审批（1-未送审，2-审批中，3-审批通过，4-已退回）
                    approvals.Add("4");
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<List<long>>.In("PhId", gAppval.RefbillPhidList))
                        .Add(ORMRestrictions<List<string>>.NotIn("FApproveStatus", approvals));
                    var result = this.ProjectMstFacade.Find(dic).Data;
                    if (result.Count > 0)
                    {
                        throw new Exception("送审的单据应满足未送审或者已退回状态！");
                    }
                }
                else if (gAppval.FBilltype == BillType.MiddleAddBudget || gAppval.FBilltype==BillType.MiddleUpdateBudget || gAppval.FBilltype == BillType.MiddleDtlBudget || gAppval.FBilltype == BillType.MiddleBudget)
                {
                    dic.Clear();
                    List<string> approvals = new List<string>();
                    approvals.Clear();
                    approvals.Add("1");//预算相关的审批（1-未送审，2-审批中，3-审批通过，4-已退回）
                    approvals.Add("4");
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<List<long>>.In("PhId", gAppval.RefbillPhidList))
                        .Add(ORMRestrictions<List<string>>.NotIn("FApproveStatus", approvals));
                    var result = this.BudgetMstFacade.Find(dic).Data;
                    if (result.Count > 0)
                    {
                        throw new Exception("送审的单据应满足未送审或者已退回状态！");
                    }
                }
                else if (gAppval.FBilltype == BillType.Expense)
                {
                    dic.Clear();
                    List<string> approvals = new List<string>();
                    approvals.Clear();
                    approvals.Add("1");//预算相关的审批（1-未送审，2-审批中，3-审批通过，4-已退回）
                    approvals.Add("4");
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<List<long>>.In("PhId", gAppval.RefbillPhidList))
                        .Add(ORMRestrictions<List<string>>.NotIn("FApprovestatus", approvals));
                    var result = this.ExpenseMstFacade.Find(dic).Data;
                    if (result.Count > 0)
                    {
                        throw new Exception("送审的单据应满足未送审或者已退回状态！");
                    }
                }
                else if(gAppval.FBilltype == BillType.InComeBudget)
                {
                    var result = this.YsIncomeMstFacade.Find(dic).Data;
                    if (result.Count > 0)
                    {
                        throw new Exception("送审的单据应满足未送审或者已退回状态！");
                    }
                }
                else
                {
                    throw new Exception("送审的单据类型传递不正确！");
                }
                dic.Clear();
                new CreateCriteria(dic)
                    .Add(ORMRestrictions<List<long>>.In("PhId", gAppval.RefbillPhidList))
                    .Add(ORMRestrictions<byte>.Eq("FDelete", (byte)DeleteType.Yes));
                if (gAppval.FBilltype == BillType.FundsPay)
                {
                    var payments = this.PaymentMstFacade.Find(dic).Data;
                    if(payments.Count > 0)
                    {
                        throw new Exception("作废的单据不能进行送审！");
                    }
                }
                else if(gAppval.FBilltype == BillType.PayMent)
                {
                    var payments = this.GKPaymentMstFacade.Find(dic).Data;
                    if (payments.Count > 0)
                    {
                        throw new Exception("作废的单据不能进行送审！");
                    }
                }
                else if (gAppval.FBilltype == BillType.BeginProject || gAppval.FBilltype == BillType.MiddleProject || gAppval.FBilltype == BillType.ExpendBudeget)
                {
                    dic.Clear();
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<List<long>>.In("PhId", gAppval.RefbillPhidList))
                        .Add(ORMRestrictions<int>.NotEq("FLifeCycle", 0));
                    var payments = this.ProjectMstFacade.Find(dic).Data;
                    if (payments != null && payments.Count > 0)
                    {
                        throw new Exception("作废的单据不能进行送审！");
                    }
                    //立项与预立项的单据要进行进度控制的判断
                    dic.Clear();
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<List<long>>.In("PhId", gAppval.RefbillPhidList))
                        .Add(ORMRestrictions<int>.Eq("FLifeCycle", 0));
                    var paymentList = this.ProjectMstFacade.Find(dic).Data;
                    if(paymentList != null && paymentList.Count > 0)
                    {
                        List<string> budList = paymentList.Select(t => t.FBudgetDept).Distinct().ToList();
                        List<string> yearList = paymentList.Select(t => t.FYear).Distinct().ToList();
                        if(budList!=null && budList.Count > 0 && yearList != null && yearList.Count > 0)
                        {
                            var budProcess = this.BudgetProcessCtrlFacade.Find(t => budList.Contains(t.FDeptCode) && yearList.Contains(t.FYear)).Data;
                            if(budProcess != null && budProcess.Count > 0)
                            {
                                foreach(var payM in paymentList)
                                {
                                    var process = budProcess.ToList().Find(t => t.FDeptCode == payM.FBudgetDept && t.FYear == payM.FYear);
                                    if (process != null && process.FProcessStatus != "1")
                                    {
                                        throw new Exception("有单据的预算部门的进度已不在年初申报，因此无法送审！");
                                    }
                                }
                            }
                        }
                    }
                }
                else if (gAppval.FBilltype == BillType.MiddleAddBudget || gAppval.FBilltype == BillType.MiddleUpdateBudget || gAppval.FBilltype == BillType.MiddleDtlBudget || gAppval.FBilltype ==BillType.MiddleBudget)
                {
                    dic.Clear();
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<List<long>>.In("PhId", gAppval.RefbillPhidList))
                        .Add(ORMRestrictions<int>.NotEq("FLifeCycle", 0));

                    var payments = this.BudgetMstFacade.Find(dic).Data;
                    if (payments.Count > 0)
                    {
                        throw new Exception("作废的单据不能进行送审！");
                    }

                    //年中调整的单据要进行进度控制的判断
                    dic.Clear();
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<List<long>>.In("PhId", gAppval.RefbillPhidList))
                        .Add(ORMRestrictions<int>.Eq("FLifeCycle", 0));
                    var paymentList = this.BudgetMstFacade.Find(dic).Data;
                    if (paymentList != null && paymentList.Count > 0)
                    {
                        List<string> budList = paymentList.Select(t => t.FBudgetDept).Distinct().ToList();
                        List<string> yearList = paymentList.Select(t => t.FYear).Distinct().ToList();
                        if (budList != null && budList.Count > 0 && yearList != null && yearList.Count > 0)
                        {
                            var budProcess = this.BudgetProcessCtrlFacade.Find(t => budList.Contains(t.FDeptCode) && yearList.Contains(t.FYear)).Data;
                            if (budProcess != null && budProcess.Count > 0)
                            {
                                foreach (var payM in paymentList)
                                {
                                    var process = budProcess.ToList().Find(t => t.FDeptCode == payM.FBudgetDept && t.FYear == payM.FYear);
                                    if (process != null && process.FProcessStatus != "3")
                                    {
                                        throw new Exception("有单据的预算部门的进度已不在年中调整，因此无法送审！");
                                    }
                                }
                            }
                        }
                    }
                }
                else if (gAppval.FBilltype == BillType.Expense)
                {
                    dic.Clear();
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<List<long>>.In("PhId", gAppval.RefbillPhidList))
                        .Add(ORMRestrictions<int>.NotEq("FLifeCycle", 0));

                    var payments = this.ExpenseMstFacade.Find(dic).Data;
                    if (payments.Count > 0)
                    {
                        throw new Exception("作废的单据不能进行送审！");
                    }
                }
                else if (gAppval.FBilltype == BillType.InComeBudget)
                {
                    //dic.Clear();
                    //new CreateCriteria(dic)
                    //    .Add(ORMRestrictions<List<long>>.In("PhId", gAppval.RefbillPhidList))
                    //    .Add(ORMRestrictions<int>.NotEq("FLifeCycle", 0));

                    //var payments = this.ExpenseMstFacade.Find(dic).Data;
                    //if (payments.Count > 0)
                    //{
                    //    throw new Exception("作废的单据不能进行送审！");
                    //}
                }
                else
                {
                    throw new Exception("送审的单据类型传递不正确！");
                }
                savedResult = this.GAppvalRecordFacade.AddAppvalRecord(gAppval);
                //long postPhid = gAppval.PostPhid;
                //byte fApproval = gAppval.FApproval;
                //foreach (var billPhid in gAppval.RefbillPhidList)
                //{
                //    GAppvalRecordModel gAppvalRecord = gAppval;
                //    gAppvalRecord.RefbillPhid = billPhid;
                //    gAppvalRecord.PostPhid = postPhid;
                //    gAppvalRecord.FApproval = fApproval;
                //    savedResult = this.GAppvalRecordFacade.AddAppvalRecord(gAppvalRecord);
                //}

            }
            else
            {
                throw new Exception("单据主键列表不能为空！");
            }
            return savedResult;
        }

        /// <summary>
        /// 生成支付单
        /// </summary>
        /// <param name="recordModel"></param>
        public SavedResult<Int64> PostAddPayMent(GAppvalRecordModel recordModel) {
            if (recordModel == null || recordModel.RefbillPhid == 0)
                throw new Exception("单据id为空！");
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic).Add(ORMRestrictions<long>.Eq("PhId", recordModel.RefbillPhid));
            var result = this.PaymentMstFacade.Find(dic).Data;
            if(result.Count >0)
            {
                if(result[0].FApproval == (byte)Approval.Pass)
                {
                    return GAppvalRecordFacade.PostAddPayMent(recordModel.RefbillPhid);
                }
                else
                {
                    throw new Exception("审批已成功，但您不是该审批流的最后一个岗位的操作员，无权进行生成支付单操作！");
                }
            }
            else
            {
                throw new Exception("选择的资金拨付单不存在！");
            }
            
        }


        /// <summary>
        /// 批量生成支付单
        /// </summary>
        /// <param name="recordModel"></param>
        public SavedResult<Int64> PostAddPayMents(GAppvalRecordModel recordModel)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic).Add(ORMRestrictions<List<long>>.In("PhId", recordModel.RefbillPhidList));
            var result = this.PaymentMstFacade.Find(dic).Data;
            if(result.Count > 0)
            {
                foreach(var pay in result)
                {
                    if(pay.FDelete == (byte)DeleteType.Yes)
                    {
                        throw new Exception("选择的资金拨付单存在作废单据，无法生成支付单！");
                    }
                }
                dic.Clear();
                new CreateCriteria(dic)
                    .Add(ORMRestrictions<List<long>>.In("RefbillPhid", recordModel.RefbillPhidList))
                    .Add(ORMRestrictions<string>.Eq("FBilltype", BillType.FundsPay));
                var records = this.GAppvalRecordFacade.Find(dic).Data;
                if(records != null && records.Count > 0)
                {
                    foreach (var payment in result)
                    {
                        if (payment.FApproval != (byte)Approval.Pass)
                        {
                            throw new Exception("选择的资金拨付单没有符合都是审批通过条件！");
                        }
                    }
                }
                else
                {
                    foreach (var payment in result)
                    {
                        if(payment.FApproval != (byte)Approval.Send)
                        {
                            throw new Exception("选择的资金拨付单没有符合都是待送审条件！");
                        }
                    }
                }
                return GAppvalRecordFacade.PostAddPayMents(recordModel.RefbillPhidList);
            }
            else
            {
                throw new Exception("选择的资金拨付单不存在！");
            }
        }


        /// <summary>
        /// 根据流程获取所有岗位以及操作员名字
        /// </summary>
        /// <param name="procPhid">流程主键</param>
        /// <returns></returns>
        public List<GAppvalRecordModel> GetAllPostsAndOpersByProc(long procPhid)
        {
            return this.GAppvalRecordFacade.GetAllPostsAndOpersByProc(procPhid);
        }

        /// <summary>
        /// 根据单据号与单据类型取消单据送审
        /// </summary>
        /// <param name="gAppval"></param>
        /// <returns></returns>
        public SavedResult<long> PostCancelAppvalRecord(GAppvalRecordModel gAppval)
        {
            return this.GAppvalRecordFacade.PostCancelAppvalRecord(gAppval);
        }
        #endregion
    }
}

