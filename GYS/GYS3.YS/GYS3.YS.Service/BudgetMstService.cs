#region Summary
/**************************************************************************************
    * 类 名 称：        BudgetMstService
    * 命名空间：        GYS3.YS.Service
    * 文 件 名：        BudgetMstService.cs
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
using SUP.Common.DataEntity;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Service;
using Enterprise3.Common.Base.Criterion;

using GYS3.YS.Service.Interface;
using GYS3.YS.Facade.Interface;
using GYS3.YS.Model.Domain;
using GQT3.QT.Facade.Interface;
using System.Reflection;
using GQT3.QT.Model.Domain;
using System.Net;
using SUP.Common.Base;
using GXM3.XM.Model.Domain;
using GXM3.XM.Facade.Interface;
using Enterprise3.WebApi.GYS3.YS.Model.Request;
using System.Data;
using NG3.Data.Service;
using NG3;
using GYS3.YS.Facade;
using Enterprise3.WebApi.GYS3.YS.Model.Response;
using GYS3.YS.Model.Extra;

namespace GYS3.YS.Service
{
    /// <summary>
    /// BudgetMst服务组装处理类
    /// </summary>
    public partial class BudgetMstService : EntServiceBase<BudgetMstModel>, IBudgetMstService
    {
        #region 类变量及属性
        /// <summary>
        /// BudgetMst业务外观处理对象
        /// </summary>
        IBudgetMstFacade BudgetMstFacade
        {
            get
            {
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IBudgetMstFacade;
            }
        }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
        private IBudgetDtlImplPlanFacade BudgetDtlImplPlanFacade { get; set; }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
        private IBudgetDtlTextContentFacade BudgetDtlTextContentFacade { get; set; }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
        private IBudgetDtlFundApplFacade BudgetDtlFundApplFacade { get; set; }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
        private IBudgetDtlBudgetDtlFacade BudgetDtlBudgetDtlFacade { get; set; }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IBudgetDtlPerformTargetFacade BudgetDtlPerformTargetFacade { get; set; }

        /// <summary>
        /// 集中采购明细
        /// </summary>
        private IBudgetDtlPurchaseDtlFacade BudgetDtlPurchaseDtlFacade { get; set; }

        /// <summary>
        /// 集中采购资金来源
        /// </summary>
        private IBudgetDtlPurDtl4SOFFacade BudgetDtlPurDtl4SOFFacade { get; set; }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IJxTrackingFacade JxTrackingFacade { get; set; }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
        private IBudgetDtlPersonnelFacade BudgetDtlPersonnelFacade { get; set; }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
        private IBudgetDtlPersonNameFacade BudgetDtlPersonNameFacade { get; set; }

        private ICorrespondenceSettingsFacade CorrespondenceSettingsFacade { get; set; }

        private IQTModifyFacade QTModifyFacade { get; set; }

        private IUserFacade UserFacade { get; set; }

        private IExpenseCategoryFacade ExpenseCategoryFacade { get; set; }

        private IOrganizationFacade OrganizationFacade { get; set; }

        private IPerformEvalTargetTypeFacade PerformEvalTargetTypeFacade { get; set; }

        private IPerformEvalTypeFacade PerformEvalTypeFacade { get; set; }

        private ISourceOfFundsFacade SourceOfFundsFacade { get; set; }

        private IBudgetAccountsFacade BudgetAccountsFacade { get; set; }

        private IPaymentMethodFacade PaymentMethodFacade { get; set; }

        private IQtZcgnflFacade QtZcgnflFacade { get; set; }

        private IProcurementCatalogFacade ProcurementCatalogFacade { get; set; }

        private IProcurementTypeFacade ProcurementTypeFacade { get; set; }

        private IProcurementProceduresFacade ProcurementProceduresFacade { get; set; }

        private IQTSysSetFacade QTSysSetFacade { get; set; }

        private ICorrespondenceSettings2Facade CorrespondenceSettings2Facade { get; set; }

        private IProjectMstFacade ProjectMstFacade { get; set; }

        private IYsAccountFacade YsAccountFacade { get; set; }

        private IQtAttachmentFacade QtAttachmentFacade { get; set; }

        private IQtAccountFacade QtAccountFacade { get; set; }
        #endregion

        #region 实现 IBudgetMstService 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<BudgetMstModel> ExampleMethod<BudgetMstModel>(string param)
        //{
        //    //编写代码
        //}

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="budgetMstEntity"></param>
        /// <param name="budgetDtlImplPlanEntities"></param>
        /// <param name="budgetDtlTextContentEntities"></param>
        /// <param name="budgetDtlFundApplEntities"></param>
        /// <param name="budgetDtlBudgetDtlEntities"></param>
        /// <param name="budgetDtlPerformTargetEntities"></param>
        /// <param name="budgetDtlPurchaseDtlEntities"></param>
        /// <param name="budgetDtlPurDtl4SOFEntities"></param>
        /// <param name="budgetDtlPersonnels"></param>
        /// <param name="budgetDtlPersonNames"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveBudgetMst(BudgetMstModel budgetMstEntity, List<BudgetDtlImplPlanModel> budgetDtlImplPlanEntities, List<BudgetDtlTextContentModel> budgetDtlTextContentEntities, List<BudgetDtlFundApplModel> budgetDtlFundApplEntities, List<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtlEntities, List<BudgetDtlPerformTargetModel> budgetDtlPerformTargetEntities, List<BudgetDtlPurchaseDtlModel> budgetDtlPurchaseDtlEntities, List<BudgetDtlPurDtl4SOFModel> budgetDtlPurDtl4SOFEntities, List<BudgetDtlPersonnelModel> budgetDtlPersonnels, List<BudgetDtlPersonNameModel> budgetDtlPersonNames= null)
        {
            return BudgetMstFacade.SaveBudgetMst(budgetMstEntity, budgetDtlImplPlanEntities, budgetDtlTextContentEntities, budgetDtlFundApplEntities, budgetDtlBudgetDtlEntities, budgetDtlPerformTargetEntities, budgetDtlPurchaseDtlEntities, budgetDtlPurDtl4SOFEntities, budgetDtlPersonnels, budgetDtlPersonNames);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="budgetMstEntity"></param>
        /// <param name="budgetDtlImplPlanEntities"></param>
        /// <param name="budgetDtlTextContentEntities"></param>
        /// <param name="budgetDtlFundApplEntities"></param>
        /// <param name="budgetDtlBudgetDtlEntities"></param>
        /// <param name="budgetDtlPerformTargetEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveBudgetMst(BudgetMstModel budgetMstEntity, List<BudgetDtlImplPlanModel> budgetDtlImplPlanEntities, List<BudgetDtlTextContentModel> budgetDtlTextContentEntities, List<BudgetDtlFundApplModel> budgetDtlFundApplEntities, List<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtlEntities, List<BudgetDtlPerformTargetModel> budgetDtlPerformTargetEntities)
        {
            return BudgetMstFacade.SaveBudgetMst(budgetMstEntity, budgetDtlImplPlanEntities, budgetDtlTextContentEntities, budgetDtlFundApplEntities, budgetDtlBudgetDtlEntities, budgetDtlPerformTargetEntities);
        }

        /// <summary>
        /// 通过外键值获取BudgetDtlImplPlan明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<BudgetDtlImplPlanModel> FindBudgetDtlImplPlanByForeignKey<TValType>(TValType id)
        {
            return BudgetDtlImplPlanFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 通过外键值获取BudgetDtlTextContent明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<BudgetDtlTextContentModel> FindBudgetDtlTextContentByForeignKey<TValType>(TValType id)
        {
            return BudgetDtlTextContentFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 通过外键值获取BudgetDtlFundAppl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<BudgetDtlFundApplModel> FindBudgetDtlFundApplByForeignKey<TValType>(TValType id)
        {
            return BudgetDtlFundApplFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 通过外键值获取BudgetDtlBudgetDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<BudgetDtlBudgetDtlModel> FindBudgetDtlBudgetDtlByForeignKey<TValType>(TValType id)
        {
            return BudgetDtlBudgetDtlFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 通过外键值获取BudgetDtlPerformTarget明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<BudgetDtlPerformTargetModel> FindBudgetDtlPerformTargetByForeignKey<TValType>(TValType id)
        {
            return BudgetDtlPerformTargetFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 通过外键值获取BudgetDtlDtlPurchaseDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<BudgetDtlPurchaseDtlModel> FindBudgetDtlPurchaseDtlByForeignKey<TValType>(TValType id)
        {
            return BudgetDtlPurchaseDtlFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 通过外键值获取BudgetDtlPurDtl4SOF明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<BudgetDtlPurDtl4SOFModel> FindBudgetDtlPurDtl4SOFByForeignKey<TValType>(TValType id)
        {
            return BudgetDtlPurDtl4SOFFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 通过外键值获取BudgetDtlPersonnel明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<BudgetDtlPersonnelModel> FindBudgetDtlPersonnelByForeignKey<TValType>(TValType id)
        {
            return BudgetDtlPersonnelFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 通过外键值获取BudgetDtlPersonName明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<BudgetDtlPersonNameModel> FindBudgetDtlPersonNameByForeignKey<TValType>(TValType id)
        {
            return BudgetDtlPersonNameFacade.FindByForeignKey(id);
        }


        /// <summary>
        /// 通过外键值获取JxTracking明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<JxTrackingModel> FindJxTrackingByForeignKey<TValType>(TValType id)
        {
            return JxTrackingFacade.FindByForeignKey(id, new string[] { "FTime" });
        }


        /// <summary>
        /// 通过科目代码获取BudgetDtlBudgetDtl明细数据
        /// </summary>
        /// <param name="kmdm">科目代码</param>
        /// <returns></returns>
        public FindedResults<BudgetDtlBudgetDtlModel> FindBudgeAccount(string kmdm)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("FBudgetAccounts", kmdm));
            return BudgetDtlBudgetDtlFacade.Find(dicWhere);
        }

        /// <summary>
        /// 通过资金来源代码获取BudgetDtlBudgetDtl明细数据
        /// </summary>
        /// <param name="ZJDM">资金来源代码</param>
        /// <returns></returns>
        public FindedResults<BudgetDtlBudgetDtlModel> FindBudgeAccountByZJDM(string ZJDM)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("FSourceOfFunds", ZJDM));
            return BudgetDtlBudgetDtlFacade.Find(dicWhere);
        }

        /// <summary>
        /// 通过代码获取BudgetMstModel
        /// </summary>
        /// <param name="dm">代码</param>
        /// <returns></returns>
        public FindedResults<BudgetMstModel> FindBudgetMst(string dm)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("FExpenseCategory", dm));
            return base.Find(dicWhere);
        }

        /// <summary>
        /// 通过代码获取BudgetDtlBudgetDtlModel
        /// </summary>
        /// <param name="dm">代码</param>
        /// <returns></returns>
        public FindedResults<BudgetDtlBudgetDtlModel> FindPaymentMethod(string dm)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("FPaymentMethod", dm));
            FindedResults<BudgetDtlBudgetDtlModel> results = BudgetDtlBudgetDtlFacade.Find(dicWhere);
            if (results != null && results.Data.Count > 0)
            {
                results.Status = ResponseStatus.Error;
                results.Msg = "该支付方式已被引用，无法删除！";
            }
            return results;
        }

        ///// <summary>
        ///// 预算科目删除时查找是否引用
        ///// </summary>
        ///// <param name="dicWhere">查询条件</param>
        ///// <returns></returns>
        ////public FindedResults<BudgetDtlBudgetDtlModel> FindBudgetDtlBudgetDtlByCode(Dictionary<string, object> dicWhere)
        ////{
        ////    return BudgetDtlBudgetDtlFacade.FindByCode(dicWhere);
        ////}
        ///// <summary>

        //public long FindBudgetDtlBudgetDtlByCode(Dictionary<string, object> dicWhere)
        //{
        //    return BudgetDtlBudgetDtlFacade.GetCount(dicWhere);
        //}

        /// <summary>
        ///  获取已经完成的预算（相同的项目编码 取最新的一条）
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="dicWhere"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public PagedResult<BudgetMstModel> BudgetLoadWithPage(int pageIndex, int pageSize = 20, Dictionary<string, object> dicWhere = null, params string[] sorts)
        {
            return BudgetMstFacade.FacadeHelper.LoadWithPage(pageIndex, pageSize, "GYS3.YS.BudgetMst", dicWhere, sorts);
        }

        /// <summary>
        /// 删除集中采购明细和资金来源
        /// </summary>
        /// <param name="id"></param>
        public void DeleteProjectDtlPurchase(Int64 id)
        {
            BudgetDtlPurchaseDtlFacade.FacadeHelper.DeleteByForeignKey<Int64>(id);
            BudgetDtlPurDtl4SOFFacade.FacadeHelper.DeleteByForeignKey<Int64>(id);
        }

        /// <summary>
        /// 同步数据到老G6H数据库
        /// </summary>
        /// <returns></returns>
        public string AddData()
        {
            return BudgetMstFacade.AddData();
        }

        /// <summary>
        /// 根据预算单据主键集合同步数据到老G6H数据库
        /// </summary>
        /// <param name="phids">预算主键集合</param>
        /// <returns></returns>
        public string AddData2(List<long> phids)
        {
            return BudgetMstFacade.AddData2(phids);
        }

        /// <summary>
        /// 项目生成预算时同步数据到老G6H数据库
        /// </summary>
        /// <param name="budgetMstEntity"></param>
        /// <param name="budgetDtlBudgetDtlEntities"></param>
        /// <returns></returns>
        public string AddDataInSaveBudgetMst(BudgetMstModel budgetMstEntity, List<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtlEntities)
        {
            return BudgetMstFacade.AddDataInSaveBudgetMst(budgetMstEntity, budgetDtlBudgetDtlEntities);
        }


        /// <summary>
        /// 查明细通过明细主键
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FindedResult<BudgetDtlBudgetDtlModel> FindDtlByPhid(long id)
        {
            return BudgetDtlBudgetDtlFacade.Find(id);
        }

        /// <summary>
        /// 允许预备费抵扣
        /// </summary>
        /// <returns></returns>
        public string AddYBF(long id)
        {
            return BudgetMstFacade.AddYBF(id);
        }

        /// <summary>
        /// 项目支出预算情况查询(已编报：FProjAmount，剩余可编报：FBillNO，账务实际：FDeclarationDept，年初预算：FBudgetAmount)
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagedResult<BudgetMstModel> GetXmZcYs(string userID, int pageIndex, int pageSize)
        {
            var dicWhereDept = new Dictionary<string, object>();
            new CreateCriteria(dicWhereDept)
                .Add(ORMRestrictions<string>.Eq("Dwdm", userID)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
            var deptList = CorrespondenceSettingsFacade.Find(dicWhereDept);
            List<string> deptL = new List<string>();
            for (var i = 0; i < deptList.Data.Count; i++)
            {
                deptL.Add(deptList.Data[i].Dydm);
            }
            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                   .Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL))
                   .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"))
                   .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0));
            //new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", DateTime.Now.Year.ToString()));
            PagedResult<BudgetMstModel> BudgetMsts = BudgetMstFacade.LoadWithPage(pageIndex, pageSize, "GYS.YS.getZCbudget2", dicWhere);
            PagedResult<BudgetMstModel> BudgetMsts2 = BudgetMstFacade.GetSJFSS(userID, BudgetMsts);
            return BudgetMsts2;
        }

        /// <summary>
        /// 项目支出预算情况查询(已编报：FProjAmount，剩余可编报：FSurplusAmount，账务实际：FHappenAmount，年初预算：FBudgetAmount)
        /// </summary>
        /// <param name="userID">用户主键</param>
        /// <returns></returns>
        public PagedResult<BudgetMstModel> GetXmZcYs2(string userID)
        {
            var dicWhereDept = new Dictionary<string, object>();
            new CreateCriteria(dicWhereDept)
                .Add(ORMRestrictions<string>.Eq("Dwdm", userID)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
            var deptList = CorrespondenceSettingsFacade.Find(dicWhereDept);
            List<string> deptL = new List<string>();
            for (var i = 0; i < deptList.Data.Count; i++)
            {
                deptL.Add(deptList.Data[i].Dydm);
            }
            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                   .Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL))
                   .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"))
                   .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0));
            //new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", DateTime.Now.Year.ToString()));
            //PagedResult<BudgetMstModel> BudgetMsts = BudgetMstFacade.LoadWithPage(0, 20, "GYS.YS.getZCbudget2", dicWhere);
            PagedResult<BudgetMstModel> BudgetMsts = BudgetMstFacade.LoadWithPageInfinity("GYS.YS.getZCbudget2", dicWhere);
            PagedResult<BudgetMstModel> BudgetMsts2 = BudgetMstFacade.GetSJFSS2(userID, BudgetMsts);
            return BudgetMsts2;
        }

        /// <summary>
        /// 保存预算单据修改记录
        /// </summary>
        /// <param name="AfterbudgetMst"></param>
        /// <param name="budgetDtlImpls"></param>
        /// <param name="budgetDtlTexts"></param>
        /// <param name="budgetDtlFunds"></param>
        /// <param name="budgetDtlBudgets"></param>
        /// <param name="budgetDtlPerforms"></param>
        /// <param name="budgetDtlPurchases"></param>
        /// <param name="budgetDtlPurDtl4s"></param>
        /// <returns></returns>
        public CommonResult SaveModify(BudgetMstModel AfterbudgetMst, EntityInfo<BudgetDtlImplPlanModel> budgetDtlImpls, EntityInfo<BudgetDtlTextContentModel> budgetDtlTexts, EntityInfo<BudgetDtlFundApplModel> budgetDtlFunds, EntityInfo<BudgetDtlBudgetDtlModel> budgetDtlBudgets, EntityInfo<BudgetDtlPerformTargetModel> budgetDtlPerforms, EntityInfo<BudgetDtlPurchaseDtlModel> budgetDtlPurchases, EntityInfo<BudgetDtlPurDtl4SOFModel> budgetDtlPurDtl4s)
        {
            CommonResult result = new CommonResult();
            //获取IP
            string IP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    IP = _IPAddress.ToString();
                }
            }
            //long UserPhid = AfterbudgetMst.Creator;
            long UserPhid = AppInfoBase.UserID;
            User2Model User = UserFacade.Find(UserPhid).Data;
            List<QTModifyModel> modifyList = new List<QTModifyModel>();

            //列属性->中文
            Dictionary<string, string> colums = new Dictionary<string, string> {  { "FYear", "项目年度" },
            { "FProjName", "项目名称" },{ "FDeclarationUnit", "申报单位" },{ "FDeclarationDept", "申报部门" },{ "FProjAttr", "项目属性" },{ "FDuration", "存续期限" },
            { "FExpenseCategory", "支出类别" },{ "FStartDate", "开始日期" },{ "FEndDate", "结束日期" },
            { "FProjAmount", "项目金额" },{ "FIfPerformanceAppraisal", "是否绩效评价" },{ "FIfKeyEvaluation", "是否重点评价" },{ "FMeetingTime", "会议时间" },
            { "FMeetiingSummaryNo", "会议纪要编号" },
            { "FBudgetDept", "预算部门" },{ "FBudgetAmount", "预算金额" },
            { "FIfPurchase", "是否集中采购" },{ "FPerformType", "绩效项目类型代码" },{ "FPerformEvalType", "绩效评价类型" }
            };
            //项目属性
            Dictionary<string, string> FProjAttrDic = new Dictionary<string, string> { { "1", "延续项目" }, { "2", "新增项目" } };
            //存续期限
            Dictionary<string, string> FDurationDic = new Dictionary<string, string> { { "1", "一次性项目" }, { "2", "经常性项目" }, { "3", "跨年度项目" } };
            //项目状态
            /*Dictionary<string, string> FProjStatusDic = new Dictionary<string, string> { { "1", "预立项" }, { "2", "项目立项" }, { "3", "项目执行" }, { "4", "项目调整" },
            { "5", "项目暂停" }, { "6", "项目终止" }, { "7", "项目关闭" }, { "8", "调整项目执行" }};*/
            //是否EnumYesNO
            Dictionary<string, string> EnumYesNODic = new Dictionary<string, string> { { "1", "是" }, { "2", "否" }, { "Yes", "是" }, { "No", "否" } };
            //单据类型
            //Dictionary<string, string> FTypeDic = new Dictionary<string, string> { { "c", "年初" }, { "z", "年中" }, { "x", "专项" } };
            //审批状态
            //Dictionary<string, string> FApproveStatusDic = new Dictionary<string, string> { { "1", "待上报" }, { "2", "审批中" }, { "3", "审批通过" }, { "4", "已退回" } };
            //版本标识
            //Dictionary<string, string> FLifeCycleDic = new Dictionary<string, string> { { "0", "正常" }, { "1", "作废" } };
            //单据调整判断
            //Dictionary<string, string> FMidYearChangeDic = new Dictionary<string, string> { { "0", "正常" }, { "1", "调整" } };
            //生成到老G6H记录
            //Dictionary<string, string> FSaveToOldG6hDic = new Dictionary<string, string> { { "0", "否" }, { "1", "是" } };

            BudgetMstModel budgetMst = BudgetMstFacade.Find(AfterbudgetMst.PhId).Data;
            PropertyInfo[] properties = typeof(BudgetMstModel).GetProperties();//取BudgetMstModel的所有属性
            foreach (PropertyInfo info in properties)
            {
                if (info.Name != "PersistentState" && info.Name != "ListNotEvaluateProerty" && info.Name != "ExtendObjects" && info.Name != "NgRecordVer" && !info.Name.EndsWith("EXName"))
                {
                    //Type type = budgetMst.GetPropertyType(info.Name);//取属性的值类型
                    object beforevalue = budgetMst.GetPropertyValue(info.Name) ?? "";
                    //object beforevalue2 = Convert.ChangeType(beforevalue, type)??"";

                    object aftervalue = AfterbudgetMst.GetPropertyValue(info.Name) ?? "";
                    //object aftervaluee2 = Convert.ChangeType(aftervalue, type)??"";

                    if (!beforevalue.Equals(aftervalue))
                    {
                        QTModifyModel qTModify = new QTModifyModel();
                        qTModify.DEFSTR1 = budgetMst.FProjStatus.ToString();//项目状态
                        qTModify.UserCode = User.UserNo;
                        qTModify.UserName = User.UserName;
                        qTModify.IP = IP;
                        if (!colums.ContainsKey(info.Name))
                        {
                            break;
                        }
                        qTModify.ModifyField = colums[info.Name];

                        qTModify.FProjCode = budgetMst.FProjCode;
                        //qTModify.FProjName
                        //qTModify.TabName = info.Name;
                        qTModify.PersistentState = PersistentState.Added;

                        switch (info.Name)
                        {
                            case "FProjAttr":
                                qTModify.BeforeValue = FProjAttrDic[beforevalue.ToString()];
                                qTModify.AfterValue = FProjAttrDic[aftervalue.ToString()];
                                break;
                            case "FDuration":
                                qTModify.BeforeValue = FDurationDic[beforevalue.ToString()];
                                qTModify.AfterValue = FDurationDic[aftervalue.ToString()];
                                break;
                            case "FExpenseCategory":
                                qTModify.BeforeValue = ExpenseCategoryFacade.FindMcByDm(beforevalue.ToString());
                                qTModify.AfterValue = ExpenseCategoryFacade.FindMcByDm(aftervalue.ToString());
                                break;
                            /* case "FProjStatus":
                                 qTModify.BeforeValue = FProjStatusDic[beforevalue.ToString()];
                                 qTModify.AfterValue = FProjStatusDic[aftervalue.ToString()];
                                 break;*/
                            case "FIfPerformanceAppraisal":
                                qTModify.BeforeValue = EnumYesNODic[beforevalue.ToString()];
                                qTModify.AfterValue = EnumYesNODic[aftervalue.ToString()];
                                break;
                            case "FIfKeyEvaluation":
                                qTModify.BeforeValue = EnumYesNODic[beforevalue.ToString()];
                                qTModify.AfterValue = EnumYesNODic[aftervalue.ToString()];
                                break;
                            /* case "FType":
                                 qTModify.BeforeValue = FTypeDic[beforevalue.ToString()];
                                 qTModify.AfterValue = FTypeDic[aftervalue.ToString()];
                                 break;*/
                            /*case "FApproveStatus":
                                qTModify.BeforeValue = FApproveStatusDic[beforevalue.ToString()];
                                qTModify.AfterValue = FApproveStatusDic[aftervalue.ToString()];
                                break;*/
                            /*case "FLifeCycle":
                                qTModify.BeforeValue = FLifeCycleDic[beforevalue.ToString()];
                                qTModify.AfterValue = FLifeCycleDic[aftervalue.ToString()];
                                break;*/
                            case "FBudgetDept":
                                qTModify.BeforeValue = OrganizationFacade.FindMcByDm(beforevalue.ToString());
                                qTModify.AfterValue = OrganizationFacade.FindMcByDm(aftervalue.ToString());
                                break;
                            /*case "FApprover":
                                qTModify.BeforeValue = UserFacade.FindMcByDm(beforevalue.ToString());
                                qTModify.AfterValue = UserFacade.FindMcByDm(aftervalue.ToString());
                                break;*/
                            /*case "FMidYearChange":
                                qTModify.BeforeValue = FMidYearChangeDic[beforevalue.ToString()];
                                qTModify.AfterValue = FMidYearChangeDic[aftervalue.ToString()];
                                break;*/
                            case "FIfPurchase":
                                qTModify.BeforeValue = EnumYesNODic[beforevalue.ToString()];
                                qTModify.AfterValue = EnumYesNODic[aftervalue.ToString()];
                                break;
                            case "FPerformType":
                                qTModify.BeforeValue = PerformEvalTargetTypeFacade.FindMcByDm(beforevalue.ToString());
                                qTModify.AfterValue = PerformEvalTargetTypeFacade.FindMcByDm(aftervalue.ToString());
                                break;
                            case "FPerformEvalType":
                                qTModify.BeforeValue = PerformEvalTypeFacade.FindMcByDm(beforevalue.ToString());
                                qTModify.AfterValue = PerformEvalTypeFacade.FindMcByDm(aftervalue.ToString());
                                break;
                            /*case "FSaveToOldG6h":
                                qTModify.BeforeValue = FSaveToOldG6hDic[beforevalue.ToString()];
                                qTModify.AfterValue = FSaveToOldG6hDic[aftervalue.ToString()];
                                break;*/
                            /*case "FBillNO":
                                qTModify.BeforeValue = FSaveToOldG6hDic[beforevalue.ToString()];
                                qTModify.AfterValue = FSaveToOldG6hDic[aftervalue.ToString()];
                                break;*/
                            default:
                                qTModify.BeforeValue = beforevalue.ToString();
                                qTModify.AfterValue = aftervalue.ToString();
                                break;
                        }
                        modifyList.Add(qTModify);

                    }
                }
            }

            //实施计划（BudgetDtlImplPlanModel）
            Dictionary<string, string> colums_Impls = new Dictionary<string, string> { { "FImplContent", "实施内容" } ,
            { "FStartDate", "开始日期" },{ "FEndDate", "结束日期" }};
            List<BudgetDtlImplPlanModel> implPlanModels = BudgetDtlImplPlanFacade.FindByForeignKey(AfterbudgetMst.PhId).Data.ToList();//原数据
            List<long> implPlanindex = new List<long>();
            //Dictionary<string, string> implPlanRow = new Dictionary<string, string>();//之前的行号
            for (var i = 0; i < implPlanModels.Count; i++)
            {
                //implPlanRow.Add(implPlanModels[i].PhId.ToString(), (i + 1).ToString());
                implPlanindex.Add(implPlanModels[i].PhId);
            }
            /*Dictionary<string, string> implPlanRow2 = new Dictionary<string, string>();//现在的行号
            for (var i = 0; i < budgetDtlImpls.AllRow.Count; i++)
            {
                implPlanRow2.Add(budgetDtlImpls.AllRow[i].PhId.ToString(), (i + 1).ToString());
            }*/
            if (budgetDtlImpls.DeleteRow.Count > 0)
            {
                for (var i = 0; i < budgetDtlImpls.DeleteRow.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = budgetMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "实施计划-行号" + (implPlanindex.FindIndex(item => item.Equals(budgetDtlImpls.DeleteRow[i].PhId)) + 1).ToString();
                    qTModify.FProjCode = budgetMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "删除";
                    modifyList.Add(qTModify);
                }
                for (var i = 0; i < budgetDtlImpls.DeleteRow.Count; i++)
                {
                    implPlanindex.Remove(budgetDtlImpls.DeleteRow[i].PhId);
                }
            }
            if (budgetDtlImpls.NewRow.Count > 0)
            {
                for (var i = 0; i < budgetDtlImpls.NewRow.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = budgetMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "实施计划-行号" + (implPlanindex.Count + 1).ToString();
                    qTModify.FProjCode = budgetMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "新增";
                    modifyList.Add(qTModify);
                }
            }
            if (budgetDtlImpls.ModifyRow.Count > 0)
            {
                PropertyInfo[] properties_Impls = typeof(BudgetDtlImplPlanModel).GetProperties();//取BudgetDtlImplPlanModel的所有属性
                for (var i = 0; i < budgetDtlImpls.ModifyRow.Count; i++)
                {
                    foreach (PropertyInfo info in properties_Impls)
                    {
                        if (info.Name != "PersistentState" && info.Name != "ListNotEvaluateProerty" && info.Name != "ExtendObjects" && info.Name != "NgRecordVer" && !info.Name.EndsWith("EXName") && info.Name != "ForeignKeys")
                        {
                            //object beforevalue = implPlanModels[int.Parse(implPlanRow[budgetDtlImpls.ModifyRow[i].PhId.ToString()])-1].GetPropertyValue(info.Name) ?? "";
                            object beforevalue = implPlanModels[implPlanModels.FindIndex(item => item.PhId.Equals(budgetDtlImpls.ModifyRow[i].PhId))].GetPropertyValue(info.Name) ?? "";
                            object aftervalue = budgetDtlImpls.ModifyRow[i].GetPropertyValue(info.Name) ?? "";
                            if (!beforevalue.Equals(aftervalue))
                            {
                                QTModifyModel qTModify = new QTModifyModel();
                                qTModify.DEFSTR1 = budgetMst.FProjStatus.ToString();
                                qTModify.UserCode = User.UserNo;
                                qTModify.UserName = User.UserName;
                                qTModify.IP = IP;
                                if (!colums_Impls.ContainsKey(info.Name))
                                {
                                    break;
                                }
                                qTModify.ModifyField = "实施计划-行号" + (implPlanindex.FindIndex(item => item.Equals(budgetDtlImpls.ModifyRow[i].PhId)) + 1).ToString() + "-" + colums_Impls[info.Name]; ;
                                qTModify.FProjCode = budgetMst.FProjCode;
                                //qTModify.FProjName
                                //qTModify.TabName = info.Name;
                                qTModify.PersistentState = PersistentState.Added;
                                qTModify.BeforeValue = beforevalue.ToString();
                                qTModify.AfterValue = aftervalue.ToString();
                                modifyList.Add(qTModify);
                            }
                        }
                    }
                }
            }

            //BudgetDtlTextContentModel 只可能有一行数据
            Dictionary<string, string> colums_textContent = new Dictionary<string, string> { { "FFunctionalOverview", "部门职能概述" } ,
            { "FProjOverview", "项目概况" },{ "FProjBasis", "立项依据" },{ "FFeasibility", "可行性" },{ "FNecessity", "必要性" },{ "FLTPerformGoal", "总体绩效目标" },{ "FAnnualPerformGoal", "年度绩效目标" }};
            BudgetDtlTextContentModel textContentModel = BudgetDtlTextContentFacade.FindByForeignKey(AfterbudgetMst.PhId).Data[0];//原数据
            PropertyInfo[] properties_textContent = typeof(BudgetDtlTextContentModel).GetProperties();//取BudgetDtlTextContentModel的所有属性
            foreach (PropertyInfo info in properties_textContent)
            {
                if (info.Name != "PersistentState" && info.Name != "ListNotEvaluateProerty" && info.Name != "ExtendObjects" && info.Name != "NgRecordVer" && info.Name != "ForeignKeys")
                {
                    object beforevalue = textContentModel.GetPropertyValue(info.Name) ?? "";
                    object aftervalue = budgetDtlTexts.AllRow[0].GetPropertyValue(info.Name) ?? "";
                    if (!beforevalue.Equals(aftervalue))
                    {
                        QTModifyModel qTModify = new QTModifyModel();
                        qTModify.DEFSTR1 = budgetMst.FProjStatus.ToString();
                        qTModify.UserCode = User.UserNo;
                        qTModify.UserName = User.UserName;
                        qTModify.IP = IP;
                        if (!colums_textContent.ContainsKey(info.Name))
                        {
                            break;
                        }
                        qTModify.ModifyField = colums_textContent[info.Name]; ;
                        qTModify.FProjCode = budgetMst.FProjCode;
                        //qTModify.FProjName
                        //qTModify.TabName = info.Name;
                        qTModify.PersistentState = PersistentState.Added;
                        qTModify.BeforeValue = beforevalue.ToString();
                        qTModify.AfterValue = aftervalue.ToString();
                        modifyList.Add(qTModify);
                    }
                }
            }

            ////项目资金申请BudgetDtlFundApplModel
            //Dictionary<string, string> colums_fundAppl = new Dictionary<string, string> { { "FSourceOfFunds", "资金来源" } ,
            //{ "FAmount", "金额" }};
            //List<BudgetDtlFundApplModel> fundApplModels = BudgetDtlFundApplFacade.FindByForeignKey(AfterbudgetMst.PhId).Data.ToList();//原数据
            //List<long> fundApplindex = new List<long>();
            ////Dictionary<string, string> fundApplRow = new Dictionary<string, string>();//之前的行号
            //for (var i = 0; i < fundApplModels.Count; i++)
            //{
            //    //fundApplRow.Add(fundApplModels[i].PhId.ToString(), (i + 1).ToString());
            //    fundApplindex.Add(fundApplModels[i].PhId);
            //}
            //Dictionary<string, string> fundApplRow2 = new Dictionary<string, string>();//现在的行号
            //for (var i = 0; i < budgetDtlFunds.AllRow.Count; i++)
            //{
            //    fundApplRow2.Add(budgetDtlFunds.AllRow[i].PhId.ToString(), (i + 1).ToString());
            //}
            //if (budgetDtlFunds.DeleteRow.Count > 0)
            //{
            //    for (var i = 0; i < budgetDtlFunds.DeleteRow.Count; i++)
            //    {
            //        QTModifyModel qTModify = new QTModifyModel();
            //        qTModify.DEFSTR1 = budgetMst.FProjStatus.ToString();
            //        qTModify.UserCode = User.UserNo;
            //        qTModify.UserName = User.UserName;
            //        qTModify.IP = IP;
            //        qTModify.ModifyField = "项目资金申请-行号" + (fundApplindex.FindIndex(item => item.Equals(budgetDtlBudgets.DeleteRow[i].PhId)) + 1).ToString();
            //        qTModify.FProjCode = budgetMst.FProjCode;
            //        //qTModify.FProjName
            //        //qTModify.TabName = info.Name;
            //        qTModify.PersistentState = PersistentState.Added;
            //        qTModify.AfterValue = "删除";
            //        modifyList.Add(qTModify);
            //    }
            //    for (var i = 0; i < budgetDtlFunds.DeleteRow.Count; i++)
            //    {
            //        fundApplindex.Remove(budgetDtlFunds.DeleteRow[i].PhId);
            //    }
            //}
            //if (budgetDtlFunds.NewRow.Count > 0)
            //{
            //    for (var i = 0; i < budgetDtlFunds.NewRow.Count; i++)
            //    {
            //        QTModifyModel qTModify = new QTModifyModel();
            //        qTModify.DEFSTR1 = budgetMst.FProjStatus.ToString();
            //        qTModify.UserCode = User.UserNo;
            //        qTModify.UserName = User.UserName;
            //        qTModify.IP = IP;
            //        qTModify.ModifyField = "项目资金申请-行号" + (fundApplindex.Count + 1).ToString();
            //        qTModify.FProjCode = budgetMst.FProjCode;
            //        //qTModify.FProjName
            //        //qTModify.TabName = info.Name;
            //        qTModify.PersistentState = PersistentState.Added;
            //        qTModify.AfterValue = "新增";
            //        modifyList.Add(qTModify);
            //    }
            //}
            //if (budgetDtlFunds.ModifyRow.Count > 0)
            //{
            //    PropertyInfo[] properties_fundAppl = typeof(BudgetDtlFundApplModel).GetProperties();//取BudgetDtlFundApplModel的所有属性
            //    for (var i = 0; i < budgetDtlFunds.ModifyRow.Count; i++)
            //    {
            //        foreach (PropertyInfo info in properties_fundAppl)
            //        {
            //            if (info.Name != "PersistentState" && info.Name != "ListNotEvaluateProerty" && info.Name != "ExtendObjects" && info.Name != "NgRecordVer" && !info.Name.EndsWith("EXName") && info.Name != "ForeignKeys")
            //            {
            //                //object beforevalue = fundApplModels[int.Parse(fundApplRow[budgetDtlFunds.ModifyRow[i].PhId.ToString()]) - 1].GetPropertyValue(info.Name) ?? "";
            //                object beforevalue = fundApplModels[fundApplModels.FindIndex(item => item.PhId.Equals(budgetDtlFunds.ModifyRow[i].PhId))].GetPropertyValue(info.Name) ?? "";

            //                object aftervalue = budgetDtlFunds.ModifyRow[i].GetPropertyValue(info.Name) ?? "";
            //                if (!beforevalue.Equals(aftervalue))
            //                {
            //                    QTModifyModel qTModify = new QTModifyModel();
            //                    qTModify.DEFSTR1 = budgetMst.FProjStatus.ToString();
            //                    qTModify.UserCode = User.UserNo;
            //                    qTModify.UserName = User.UserName;
            //                    qTModify.IP = IP;
            //                    if (!colums_fundAppl.ContainsKey(info.Name))
            //                    {
            //                        break;
            //                    }
            //                    qTModify.ModifyField = "项目资金申请-行号" + (fundApplindex.FindIndex(item => item.Equals(budgetDtlFunds.ModifyRow[i].PhId)) + 1).ToString() + "-" + colums_fundAppl[info.Name]; ;
            //                    qTModify.FProjCode = budgetMst.FProjCode;
            //                    //qTModify.FProjName
            //                    //qTModify.TabName = info.Name;
            //                    qTModify.PersistentState = PersistentState.Added;
            //                    switch (info.Name)
            //                    {
            //                        case "FSourceOfFunds":
            //                            qTModify.BeforeValue = SourceOfFundsFacade.FindMcByDm(beforevalue.ToString());
            //                            qTModify.AfterValue = SourceOfFundsFacade.FindMcByDm(aftervalue.ToString());
            //                            break;
            //                        default:
            //                            qTModify.BeforeValue = beforevalue.ToString();
            //                            qTModify.AfterValue = aftervalue.ToString();
            //                            break;
            //                    }
            //                    modifyList.Add(qTModify);
            //                }
            //            }
            //        }
            //    }
            //}

            //EntityInfo<BudgetDtlBudgetDtlModel> budgetDtlBudgets 预算明细
            Dictionary<string, string> colums_budgetDtl = new Dictionary<string, string> {
            { "FName", "名称" },{ "FMeasUnit", "计量单位" },{ "FQty", "天数" },{ "FQty2", "人数" },{ "FPrice", "单价" },{ "FAmount", "金额" },{ "FSourceOfFunds", "资金来源" },{ "FBudgetAccounts", "预算科目" },
            { "FOtherInstructions", "其他说明" },{ "FPaymentMethod", "支付方式" },{ "FExpensesChannel", "支出渠道" },{ "FFeedback", "反馈意见" },{ "Xm3_DtlPhid", "明细来源" },{ "FBudgetAmount", "预算金额" },
            { "FIfPurchase", "是否集中采购" },{ "FQtZcgnfl", "支出功能分类科目" }};
            List<BudgetDtlBudgetDtlModel> budgetDtlModels = BudgetDtlBudgetDtlFacade.FindByForeignKey(AfterbudgetMst.PhId).Data.ToList();//原数据
            List<long> budgetDtlindex = new List<long>();
            //Dictionary<string, string> budgetDtlRow = new Dictionary<string, string>();//之前的行号
            for (var i = 0; i < budgetDtlModels.Count; i++)
            {
                //budgetDtlRow.Add(budgetDtlModels[i].PhId.ToString(), (i + 1).ToString());
                budgetDtlindex.Add(budgetDtlModels[i].PhId);
            }
            /*Dictionary<string, string> budgetDtlRow2 = new Dictionary<string, string>();//现在的行号
            for (var i = 0; i < budgetDtlBudgets.AllRow.Count; i++)
            {
                budgetDtlRow2.Add(budgetDtlBudgets.AllRow[i].PhId.ToString(), (i + 1).ToString());
            }*/
            if (budgetDtlBudgets.DeleteRow.Count > 0)
            {
                for (var i = 0; i < budgetDtlBudgets.DeleteRow.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = budgetMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "预算明细-行号" + (budgetDtlindex.FindIndex(item => item.Equals(budgetDtlBudgets.DeleteRow[i].PhId)) + 1).ToString();
                    qTModify.FProjCode = budgetMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "删除";
                    modifyList.Add(qTModify);

                }
                for (var i = 0; i < budgetDtlBudgets.DeleteRow.Count; i++)
                {
                    budgetDtlindex.Remove(budgetDtlBudgets.DeleteRow[i].PhId);
                }
            }
            if (budgetDtlBudgets.NewRow.Count > 0)
            {
                for (var i = 0; i < budgetDtlBudgets.NewRow.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = budgetMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "预算明细-行号" + (budgetDtlindex.Count + 1).ToString();
                    qTModify.FProjCode = budgetMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "新增";
                    modifyList.Add(qTModify);
                    budgetDtlindex.Add(budgetDtlBudgets.NewRow[i].PhId);
                }
            }
            if (budgetDtlBudgets.ModifyRow.Count > 0)
            {
                PropertyInfo[] properties_budgetDtl = typeof(BudgetDtlBudgetDtlModel).GetProperties();//取BudgetDtlBudgetDtlModel的所有属性
                for (var i = 0; i < budgetDtlBudgets.ModifyRow.Count; i++)
                {
                    foreach (PropertyInfo info in properties_budgetDtl)
                    {
                        if (info.Name != "PersistentState" && info.Name != "ListNotEvaluateProerty" && info.Name != "ExtendObjects" && info.Name != "NgRecordVer" && !info.Name.EndsWith("EXName") && info.Name != "ForeignKeys")
                        {
                            object beforevalue = budgetDtlModels[budgetDtlModels.FindIndex(item => item.PhId.Equals(budgetDtlBudgets.ModifyRow[i].PhId))].GetPropertyValue(info.Name) ?? "";

                            object aftervalue = budgetDtlBudgets.ModifyRow[i].GetPropertyValue(info.Name) ?? "";
                            if (!beforevalue.Equals(aftervalue))
                            {
                                QTModifyModel qTModify = new QTModifyModel();
                                qTModify.DEFSTR1 = budgetMst.FProjStatus.ToString();
                                qTModify.UserCode = User.UserNo;
                                qTModify.UserName = User.UserName;
                                qTModify.IP = IP;
                                if (!colums_budgetDtl.ContainsKey(info.Name))
                                {
                                    break;
                                }
                                qTModify.ModifyField = "预算明细-行号" + (budgetDtlindex.FindIndex(item => item.Equals(budgetDtlBudgets.ModifyRow[i].PhId)) + 1).ToString() + "-" + colums_budgetDtl[info.Name]; ;
                                qTModify.FProjCode = budgetMst.FProjCode;
                                //qTModify.FProjName
                                //qTModify.TabName = info.Name;
                                qTModify.PersistentState = PersistentState.Added;
                                switch (info.Name)
                                {
                                    case "FSourceOfFunds":
                                        qTModify.BeforeValue = SourceOfFundsFacade.FindMcByDm(beforevalue.ToString());
                                        qTModify.AfterValue = SourceOfFundsFacade.FindMcByDm(aftervalue.ToString());
                                        break;
                                    case "FBudgetAccounts":
                                        qTModify.BeforeValue = BudgetAccountsFacade.FindMcByDm(beforevalue.ToString());
                                        qTModify.AfterValue = BudgetAccountsFacade.FindMcByDm(aftervalue.ToString());
                                        break;
                                    case "FPaymentMethod":
                                        qTModify.BeforeValue = PaymentMethodFacade.FindMcByDm(beforevalue.ToString());
                                        qTModify.AfterValue = PaymentMethodFacade.FindMcByDm(aftervalue.ToString());
                                        break;
                                    case "FExpensesChannel":
                                        qTModify.BeforeValue = OrganizationFacade.FindMcByDm(beforevalue.ToString());
                                        qTModify.AfterValue = OrganizationFacade.FindMcByDm(aftervalue.ToString());
                                        break;
                                    case "FIfPurchase":
                                        qTModify.BeforeValue = EnumYesNODic[beforevalue.ToString()];
                                        qTModify.AfterValue = EnumYesNODic[aftervalue.ToString()];
                                        break;
                                    case "FQtZcgnfl":
                                        qTModify.BeforeValue = QtZcgnflFacade.FindMcByDm(beforevalue.ToString());
                                        qTModify.AfterValue = QtZcgnflFacade.FindMcByDm(aftervalue.ToString());
                                        break;
                                    default:
                                        qTModify.BeforeValue = beforevalue.ToString();
                                        qTModify.AfterValue = aftervalue.ToString();
                                        break;
                                }
                                modifyList.Add(qTModify);
                            }
                        }
                    }
                }
            }

            //EntityInfo<BudgetDtlPerformTargetModel> budgetDtlPerforms
            //BudgetDtlPurchaseDtlModel 集中采购 EntityInfo<BudgetDtlPurchaseDtlModel> budgetDtlPurchases
            Dictionary<string, string> colums_purchaseDtl = new Dictionary<string, string> {
            { "FName", "名称" },{ "FContent", "采购内容" },{ "FCatalogCode", "采购目录" },{ "FTypeCode", "采购类型" },{ "FProcedureCode", "采购程序" },{ "FMeasUnit", "计量单位" },
            { "FQty", "数量" },{ "FPrice", "预计单价" },{ "FAmount", "总计金额" },{ "FSpecification", "技术参数及配置标准" },{ "FRemark", "备注" },{ "FEstimatedPurTime", "预计采购时间" },
            { "FIfPerformanceAppraisal", "是否绩效评价" }};
            List<BudgetDtlPurchaseDtlModel> purchaseDtlModels = BudgetDtlPurchaseDtlFacade.FindByForeignKey(AfterbudgetMst.PhId).Data.ToList();//原数据
            Dictionary<string, string> budgetDtlNameRow = new Dictionary<string, string>();//之前的行号
            for (var i = 0; i < budgetDtlModels.Count; i++)
            {
                if (!budgetDtlNameRow.ContainsKey(budgetDtlModels[i].FName))
                {
                    budgetDtlNameRow.Add(budgetDtlModels[i].FName, i.ToString());
                }
            }
            /*Dictionary<string, string> budgetDtlNameRow2 = new Dictionary<string, string>();//现在的行号
            for (var i = 0; i < budgetDtlBudgets.AllRow.Count; i++)
            {
                budgetDtlNameRow2.Add(budgetDtlBudgets.AllRow[i].FName, i.ToString());
            }*/
            if (budgetDtlPurchases.DeleteRow.Count > 0)
            {
                for (var i = 0; i < budgetDtlPurchases.DeleteRow.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = budgetMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "预算明细-名称-" + budgetDtlPurchases.DeleteRow[i].FName + "-集中采购";
                    qTModify.FProjCode = budgetMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "删除";
                    modifyList.Add(qTModify);
                }
            }
            if (budgetDtlPurchases.NewRow.Count > 0)
            {
                for (var i = 0; i < budgetDtlPurchases.NewRow.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = budgetMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "预算明细-名称-" + budgetDtlPurchases.NewRow[i].FName + "-集中采购";
                    qTModify.FProjCode = budgetMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "新增";
                    modifyList.Add(qTModify);
                }
            }
            if (budgetDtlPurchases.ModifyRow.Count > 0)
            {
                PropertyInfo[] properties_Purchases = typeof(BudgetDtlPurchaseDtlModel).GetProperties();//取BudgetDtlPurchaseDtlModel的所有属性
                for (var i = 0; i < budgetDtlPurchases.ModifyRow.Count; i++)
                {
                    foreach (PropertyInfo info in properties_Purchases)
                    {
                        if (info.Name != "PersistentState" && info.Name != "ListNotEvaluateProerty" && info.Name != "ExtendObjects" && info.Name != "NgRecordVer" && !info.Name.EndsWith("EXName") && info.Name != "ForeignKeys")
                        {
                            object beforevalue = purchaseDtlModels[int.Parse(budgetDtlNameRow[budgetDtlPurchases.ModifyRow[i].FName])].GetPropertyValue(info.Name) ?? "";
                            object aftervalue = budgetDtlPurchases.ModifyRow[i].GetPropertyValue(info.Name) ?? "";
                            if (!beforevalue.Equals(aftervalue))
                            {
                                QTModifyModel qTModify = new QTModifyModel();
                                qTModify.DEFSTR1 = budgetMst.FProjStatus.ToString();
                                qTModify.UserCode = User.UserNo;
                                qTModify.UserName = User.UserName;
                                qTModify.IP = IP;
                                if (!colums_purchaseDtl.ContainsKey(info.Name))
                                {
                                    break;
                                }
                                qTModify.ModifyField = "预算明细-名称-" + budgetDtlPurchases.ModifyRow[i].FName + "-" + colums_purchaseDtl[info.Name]; ;
                                qTModify.FProjCode = budgetMst.FProjCode;
                                //qTModify.FProjName
                                //qTModify.TabName = info.Name;
                                qTModify.PersistentState = PersistentState.Added;
                                switch (info.Name)
                                {
                                    case "FCatalogCode":
                                        qTModify.BeforeValue = ProcurementCatalogFacade.FindMcByDm(beforevalue.ToString());
                                        qTModify.AfterValue = ProcurementCatalogFacade.FindMcByDm(aftervalue.ToString());
                                        break;
                                    case "FTypeCode":
                                        qTModify.BeforeValue = ProcurementTypeFacade.FindMcByDm(beforevalue.ToString());
                                        qTModify.AfterValue = ProcurementTypeFacade.FindMcByDm(aftervalue.ToString());
                                        break;
                                    case "FProcedureCode":
                                        qTModify.BeforeValue = ProcurementProceduresFacade.FindMcByDm(beforevalue.ToString());
                                        qTModify.AfterValue = ProcurementProceduresFacade.FindMcByDm(aftervalue.ToString());
                                        break;
                                    case "FIfPerformanceAppraisal":
                                        qTModify.BeforeValue = EnumYesNODic[beforevalue.ToString()];
                                        qTModify.AfterValue = EnumYesNODic[aftervalue.ToString()];
                                        break;
                                    default:
                                        qTModify.BeforeValue = beforevalue.ToString();
                                        qTModify.AfterValue = aftervalue.ToString();
                                        break;
                                }
                                modifyList.Add(qTModify);
                            }
                        }
                    }
                }
            }

            //BudgetDtlPurDtl4SOFModel 集中采购资金来源 EntityInfo<BudgetDtlPurDtl4SOFModel> budgetDtlPurDtl4s
            Dictionary<string, string> colums_purDtl4SOF = new Dictionary<string, string> {
            { "FName", "明细项目名称" },{ "FSourceOfFunds", "资金来源" },{ "FAmount", "金额" }};
            List<BudgetDtlPurDtl4SOFModel> purDtl4SOFModels = BudgetDtlPurDtl4SOFFacade.FindByForeignKey(AfterbudgetMst.PhId).Data.ToList();//原数据
            /*Dictionary<string, string> budgetDtlCodeRow = new Dictionary<string, string>();//之前的行号
            for (var i = 0; i < budgetDtlModels.Count; i++)
            {
                budgetDtlCodeRow.Add(budgetDtlModels[i].FDtlCode.ToString(), (i + 1).ToString());
            }
            Dictionary<string, string> budgetDtlCodeRow2 = new Dictionary<string, string>();//现在的行号
            for (var i = 0; i < budgetDtlBudgets.AllRow.Count; i++)
            {
                budgetDtlCodeRow2.Add(budgetDtlBudgets.AllRow[i].FDtlCode.ToString(), (i + 1).ToString());
            }*/
            if (budgetDtlPurDtl4s.DeleteRow.Count > 0)
            {
                for (var i = 0; i < budgetDtlPurDtl4s.DeleteRow.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = budgetMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "预算明细-名称-" + budgetDtlPurDtl4s.DeleteRow[i].FName + "-集中采购资金来源";
                    qTModify.FProjCode = budgetMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "删除";
                    modifyList.Add(qTModify);
                }
            }
            if (budgetDtlPurDtl4s.NewRow.Count > 0)
            {
                for (var i = 0; i < budgetDtlPurDtl4s.NewRow.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = budgetMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "预算明细-名称-" + budgetDtlPurDtl4s.NewRow[i].FName + "-集中采购资金来源";
                    qTModify.FProjCode = budgetMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "新增";
                    modifyList.Add(qTModify);
                }
            }
            if (budgetDtlPurDtl4s.ModifyRow.Count > 0)
            {
                PropertyInfo[] properties_Purchases = typeof(BudgetDtlPurDtl4SOFModel).GetProperties();//取BudgetDtlPurDtl4SOFModel的所有属性
                for (var i = 0; i < budgetDtlPurDtl4s.ModifyRow.Count; i++)
                {
                    foreach (PropertyInfo info in properties_Purchases)
                    {
                        if (info.Name != "PersistentState" && info.Name != "ListNotEvaluateProerty" && info.Name != "ExtendObjects" && info.Name != "NgRecordVer" && !info.Name.EndsWith("EXName") && info.Name != "ForeignKeys")
                        {
                            object beforevalue = purDtl4SOFModels[int.Parse(budgetDtlNameRow[budgetDtlPurDtl4s.ModifyRow[i].FName])].GetPropertyValue(info.Name) ?? "";
                            object aftervalue = budgetDtlPurDtl4s.ModifyRow[i].GetPropertyValue(info.Name) ?? "";
                            if (!beforevalue.Equals(aftervalue))
                            {
                                QTModifyModel qTModify = new QTModifyModel();
                                qTModify.DEFSTR1 = budgetMst.FProjStatus.ToString();
                                qTModify.UserCode = User.UserNo;
                                qTModify.UserName = User.UserName;
                                qTModify.IP = IP;
                                if (!colums_purDtl4SOF.ContainsKey(info.Name))
                                {
                                    break;
                                }
                                qTModify.ModifyField = "预算明细-名称-" + budgetDtlPurDtl4s.ModifyRow[i].FName + "-" + colums_purDtl4SOF[info.Name]; ;
                                qTModify.FProjCode = budgetMst.FProjCode;
                                //qTModify.FProjName
                                //qTModify.TabName = info.Name;
                                qTModify.PersistentState = PersistentState.Added;
                                switch (info.Name)
                                {
                                    case "FSourceOfFunds":
                                        qTModify.BeforeValue = SourceOfFundsFacade.FindMcByDm(beforevalue.ToString());
                                        qTModify.AfterValue = SourceOfFundsFacade.FindMcByDm(aftervalue.ToString());
                                        break;
                                    default:
                                        qTModify.BeforeValue = beforevalue.ToString();
                                        qTModify.AfterValue = aftervalue.ToString();
                                        break;
                                }
                                modifyList.Add(qTModify);
                            }
                        }
                    }
                }
            }


            if (modifyList.Count > 0)
            {
                result = QTModifyFacade.Save<Int64>(modifyList, "");
            }
            return result;
        }

        /// <summary>
        /// 取符合对下补助的项目
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public object GetDxbzList(BudgetMstRequestModel param)
        {
            OrganizeModel Unit = OrganizationFacade.Find(long.Parse(param.UnitId)).Data;
            OrganizeModel Dept = OrganizationFacade.Find(long.Parse(param.DeptId)).Data;
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", param.Year))
                .Add(ORMRestrictions<string>.Eq("FDeclarationUnit", Unit.OCode))
                .Add(ORMRestrictions<string>.Eq("FBudgetDept", Dept.OCode))
                .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
                .Add(ORMRestrictions<String>.Eq("FMidYearChange", "0"));
            List<BudgetMstModel> budgetMsts = BudgetMstFacade.Find(dicWhere).Data.ToList();
            decimal FAmount = 0;
            List<BudgetMstModel> data = new List<BudgetMstModel>();
            for (var i = 0; i < budgetMsts.Count; i++)
            {
                object Dtl = GetDxbzDtl(budgetMsts[i].PhId, Unit.PhId);
                Type type = Dtl.GetType();
                var list = (List<BudgetDtlBudgetDtlModel>)type.GetProperty("Dtl").GetValue(Dtl);
                if (list.Count > 0)
                {
                    FAmount = FAmount + (decimal)type.GetProperty("FAmount").GetValue(Dtl);
                    budgetMsts[i].FProjAmount = (decimal)type.GetProperty("FAmount").GetValue(Dtl);
                    data.Add(budgetMsts[i]);
                }

            }
            var result = new
            {
                Mst = data,
                FAmount = FAmount
            };
            return result;
        }


        /// <summary>
        /// 根据单据主键取对下补助的明细
        /// </summary>
        /// <param name="id"></param>
        /// <param name="OrgId"></param>
        /// <returns></returns>
        public object GetDxbzDtl(long id, long OrgId)
        {
            var code = "";//对下补助代码
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic)
                       .Add(ORMRestrictions<String>.Eq("DicType", "DxbzCode"))
                       .Add(ORMRestrictions<long>.Eq("Orgid", OrgId));
            IList<QTSysSetModel> qTSysSets = QTSysSetFacade.Find(dic).Data;
            if (qTSysSets.Count > 0)
            {
                code = qTSysSets[0].Value;
            }
            List<BudgetDtlBudgetDtlModel> result = new List<BudgetDtlBudgetDtlModel>();
            decimal FAmount = 0;
            IList<BudgetDtlBudgetDtlModel> budgetDtl = BudgetDtlBudgetDtlFacade.FindByForeignKey(id).Data;
            foreach (BudgetDtlBudgetDtlModel data in budgetDtl)
            {
                if (data.FPaymentMethod == code)
                {
                    //data.FBudgetAccounts_EXName = BudgetAccountsFacade.FindMcByDm(data.FBudgetAccounts);//代码转名称
                    if (data.FAmountAfterEdit != 0)
                    {
                        FAmount += data.FAmountAfterEdit;
                    }
                    else
                    {
                        FAmount += data.FAmount;
                    }
                    result.Add(data);
                }
            }
            var value = new
            {
                FAmount = FAmount,
                Dtl = result
            };
            return value;
        }

        /// <summary>
        /// 取符合对下补助的项目
        /// </summary>
        /// <param name="UnitId"></param>
        /// <param name="DeptId"></param>
        /// <param name="Year"></param>
        /// <returns></returns>
        public List<BudgetMstModel> GetYsList(long UnitId, long DeptId, string Year)
        {
            OrganizeModel Unit = OrganizationFacade.Find(UnitId).Data;
            OrganizeModel Dept = OrganizationFacade.Find(DeptId).Data;
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", Year))
                .Add(ORMRestrictions<string>.Eq("FDeclarationUnit", Unit.OCode))
                .Add(ORMRestrictions<string>.Eq("FBudgetDept", Dept.OCode))
                .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
                .Add(ORMRestrictions<String>.Eq("FMidYearChange", "0"));
            List<BudgetMstModel> budgetMsts = BudgetMstFacade.Find(dicWhere).Data.ToList();
            return budgetMsts;
        }

        /// <summary>
        /// 数据迁移到项目库
        /// </summary>
        /// <param name="userID"></param>
        public string SaveSynOld(string userID)
        {

            string FDeclarationUnit = "";//组织
            var dicWhereUnit = new Dictionary<string, object>();
            new CreateCriteria(dicWhereUnit).Add(ORMRestrictions<string>.Eq("Dylx", "08"))
                .Add(ORMRestrictions<Int32>.Eq("DefInt1", 1)).Add(ORMRestrictions<string>.Eq("Dwdm", userID));
            var CorrespondenceSettings = CorrespondenceSettingsFacade.Find(dicWhereUnit).Data;
            if (CorrespondenceSettings.Count > 0)
            {
                FDeclarationUnit = CorrespondenceSettings[0].Dydm;
            }

            string userConn = "";
            Dictionary<string, object> conndic = new Dictionary<string, object>();
            new CreateCriteria(conndic)
                .Add(ORMRestrictions<string>.Eq("Dylx", "config"))
                .Add(ORMRestrictions<string>.Eq("DefStr1", FDeclarationUnit));
            IList<CorrespondenceSettings2Model> CorrespondenceSettings2s = CorrespondenceSettings2Facade.Find(conndic).Data;
            if (CorrespondenceSettings2s.Count > 0 && CorrespondenceSettings2s[0].DefStr2 != null)
            {
                userConn = CorrespondenceSettings2s[0].DefStr2;
            }
            else
            {
                return "老G6H数据库配置有误！";
            }
            List<ProjectMstModel> projectMsts = BudgetMstFacade.GetOldMstListXM(userConn).ToList();
            List<ProjectDtlBudgetDtlModel> projectDtls = BudgetMstFacade.GetOldDtlListXM(userConn).ToList();
            List<ProjectDtlTextContentModel> projectTexts = BudgetMstFacade.GetOldTextListXM(userConn).ToList();
            List<ProjectDtlBudgetDtlModel> projectDtlsBycode = new List<ProjectDtlBudgetDtlModel>();
            List<ProjectDtlTextContentModel> projectTextsBycode = new List<ProjectDtlTextContentModel>();

            BudgetMstModel YsMst = new BudgetMstModel();
            List<BudgetMstModel> budgetMsts = BudgetMstFacade.GetOldMstList(userConn).ToList();
            List<BudgetDtlBudgetDtlModel> budgetDtls = BudgetMstFacade.GetOldDtlList(userConn).ToList();
            List<BudgetDtlTextContentModel> budgetTexts = BudgetMstFacade.GetOldTextList(userConn).ToList();
            List<BudgetDtlBudgetDtlModel> budgetDtlsBycode = new List<BudgetDtlBudgetDtlModel>();
            List<BudgetDtlTextContentModel> budgetTextsBycode = new List<BudgetDtlTextContentModel>();
            SavedResult<Int64> savedMstResult = new SavedResult<Int64>();

            for (var i = 0; i < projectMsts.Count; i++)
            {
                //存项目
                projectMsts[i].PersistentState = PersistentState.Added;
                projectMsts[i].FProjStatus = 2;
                projectMsts[i].FApproveStatus = "3";
                projectMsts[i].FVerNo = "0001";
                projectMsts[i].FSaveToOldG6h = 1;
                //存预算明细表
                projectDtlsBycode = projectDtls.FindAll(t => t.FQtZcgnfl == projectMsts[i].FProjCode);
                if (projectDtlsBycode.Count > 0)
                {
                    foreach (ProjectDtlBudgetDtlModel dtl in projectDtlsBycode)
                    {
                        projectDtls.Remove(dtl);
                        //dtl.MstPhid = savedMstResult.KeyCodes[0];
                        dtl.FQtZcgnfl = "";
                        dtl.PersistentState = PersistentState.Added;
                    }
                    //BudgetDtlBudgetDtlFacade.Save<Int64>(budgetDtlsBycode);
                }
                //存预算Text表
                projectTextsBycode = projectTexts.FindAll(t => t.FLTPerformGoal == projectMsts[i].FProjCode);
                if (projectTextsBycode.Count > 0)
                {
                    foreach (ProjectDtlTextContentModel text in projectTextsBycode)
                    {
                        projectTexts.Remove(text);
                        //text.MstPhid = savedMstResult.KeyCodes[0];
                        text.FLTPerformGoal = "";
                        text.PersistentState = PersistentState.Added;
                    }
                    //BudgetDtlTextContentFacade.Save<Int64>(budgetTextsBycode);
                }
                savedMstResult = ProjectMstFacade.SaveProjectMst(projectMsts[i], new List<ProjectDtlImplPlanModel>(), projectTextsBycode, new List<ProjectDtlFundApplModel>(), projectDtlsBycode);

                //存预算
                if (savedMstResult.Status == ResponseStatus.Success && savedMstResult.KeyCodes.Count > 0)
                {
                    YsMst = budgetMsts.ToList().Find(t => t.FProjCode == projectMsts[i].FProjCode);
                    YsMst.PersistentState = PersistentState.Added;
                    YsMst.FProjStatus = 3;
                    YsMst.FApproveStatus = "3";
                    YsMst.FVerNo = "0001";
                    YsMst.FSaveToOldG6h = 1;
                    YsMst.XmMstPhid = savedMstResult.KeyCodes[0];
                    //savedMstResult = BudgetMstFacade.Save<Int64>(budgetMsts[i], "");
                    //存预算明细表
                    budgetDtlsBycode = budgetDtls.FindAll(t => t.FQtZcgnfl == YsMst.FProjCode);
                    if (budgetDtlsBycode.Count > 0)
                    {
                        foreach (BudgetDtlBudgetDtlModel dtl in budgetDtlsBycode)
                        {
                            budgetDtls.Remove(dtl);
                            //dtl.MstPhid = savedMstResult.KeyCodes[0];
                            dtl.FQtZcgnfl = "";
                            dtl.PersistentState = PersistentState.Added;
                        }
                        //BudgetDtlBudgetDtlFacade.Save<Int64>(budgetDtlsBycode);
                    }
                    //存预算Text表
                    budgetTextsBycode = budgetTexts.FindAll(t => t.FLTPerformGoal == YsMst.FProjCode);
                    if (budgetTextsBycode.Count > 0)
                    {
                        foreach (BudgetDtlTextContentModel text in budgetTextsBycode)
                        {
                            budgetTexts.Remove(text);
                            //text.MstPhid = savedMstResult.KeyCodes[0];
                            text.FLTPerformGoal = "";
                            text.PersistentState = PersistentState.Added;
                        }
                        //BudgetDtlTextContentFacade.Save<Int64>(budgetTextsBycode);
                    }
                    BudgetMstFacade.SaveBudgetMst(budgetMsts[i], new List<BudgetDtlImplPlanModel>(), budgetTextsBycode, new List<BudgetDtlFundApplModel>(), budgetDtlsBycode, new List<BudgetDtlPerformTargetModel>());
                }
            }

            return "数据迁移成功！";
        }

        /// <summary>
        /// 获取已使用数
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="ProjCode"></param>
        /// <returns></returns>
        public string GetUseAmount(string usercode, string ProjCode)
        {
            string userConn = "";
            string select_DM;
            string UseAmount = "0";

            string FDeclarationUnit = "";//组织
            var dicWhereUnit = new Dictionary<string, object>();
            new CreateCriteria(dicWhereUnit).Add(ORMRestrictions<string>.Eq("Dylx", "08"))
                .Add(ORMRestrictions<Int32>.Eq("DefInt1", 1)).Add(ORMRestrictions<string>.Eq("Dwdm", usercode));
            var CorrespondenceSettings = CorrespondenceSettingsFacade.Find(dicWhereUnit).Data;
            if (CorrespondenceSettings.Count > 0)
            {
                FDeclarationUnit = CorrespondenceSettings[0].Dydm;
            }
            Dictionary<string, object> conndic = new Dictionary<string, object>();
            new CreateCriteria(conndic)
                .Add(ORMRestrictions<string>.Eq("Dylx", "config"))
                .Add(ORMRestrictions<string>.Eq("DefStr1", FDeclarationUnit));
            IList<CorrespondenceSettings2Model> CorrespondenceSettings2s = CorrespondenceSettings2Facade.Find(conndic).Data;
            if (CorrespondenceSettings2s.Count > 0 && CorrespondenceSettings2s[0].DefStr2 != null)
            {
                userConn = CorrespondenceSettings2s[0].DefStr2;
            }
            else
            {
                return "0.00";//return "老G6H数据库配置有误！";
            }



            if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
            {
                select_DM = @"SELECT sum(nvl(j,0))-sum(nvl(d,0)) FROM v_zw_pzhz WHERE zxyt= ";
            }
            else
            {
                select_DM = @"SELECT sum(isnull(j,0))-sum(isnull(d,0)) FROM v_zw_pzhz WHERE zxyt= ";
            }
            DataTable dataTable = null;
            DbHelper.Open(userConn);

            dataTable = DbHelper.GetDataTable(userConn, select_DM + "'" + ProjCode + "'");
            if (dataTable.Rows.Count != 0 && !string.IsNullOrEmpty(dataTable.Rows[0][0].ToString()))
            {
                UseAmount = decimal.Parse(dataTable.Rows[0][0].ToString()).ToString();
            }
            DbHelper.Close(userConn);

            return UseAmount;
        }

        /// <summary>
        /// 获取实际发生数
        /// </summary>
        /// <param name="FAccount"></param>
        /// <param name="XMCode"></param>
        /// <returns></returns>
        public string GetSJFSSbyXMCode(string FAccount, string XMCode)
        {
            string userConn = "";
            string select_DM;
            string UseAmount = "0";

            /*string FDeclarationUnit = "";//组织
            var dicWhereUnit = new Dictionary<string, object>();
            new CreateCriteria(dicWhereUnit).Add(ORMRestrictions<string>.Eq("Dylx", "08"))
                .Add(ORMRestrictions<Int32>.Eq("DefInt1", 1)).Add(ORMRestrictions<string>.Eq("Dwdm", usercode));
            var CorrespondenceSettings = CorrespondenceSettingsFacade.Find(dicWhereUnit).Data;
            if (CorrespondenceSettings.Count > 0)
            {
                FDeclarationUnit = CorrespondenceSettings[0].Dydm;
            }*/
            /*Dictionary<string, object> conndic = new Dictionary<string, object>();
            new CreateCriteria(conndic)
                .Add(ORMRestrictions<string>.Eq("Dylx", "config"))
                .Add(ORMRestrictions<string>.Eq("DefStr1", FDeclarationUnit));
            IList<CorrespondenceSettings2Model> CorrespondenceSettings2s = CorrespondenceSettings2Facade.Find(conndic).Data;
            if (CorrespondenceSettings2s.Count > 0 && CorrespondenceSettings2s[0].DefStr2 != null)
            {
                userConn = CorrespondenceSettings2s[0].DefStr2;
            }
            else
            {
                return "0.00";//return "老G6H数据库配置有误！";
            }*/
            //连接串更改为从基础数据-账套中取  QtAccountRule
            Dictionary<string, object> conndic = new Dictionary<string, object>();
            new CreateCriteria(conndic).Add(ORMRestrictions<string>.Eq("Dm", FAccount));
            var Accounts = QtAccountFacade.Find(conndic).Data;
            if (Accounts.Count > 0 && !string.IsNullOrEmpty(Accounts[0].FConn))
            {
                userConn = Accounts[0].FConn;
            }
            else
            {
                return "0.00";//return "老G6H数据库配置有误！";
            }


            if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
            {
                select_DM = @"SELECT sum(nvl(j,0))-sum(nvl(d,0)) FROM v_zw_pzhz WHERE zxyt= '{0}' ";
            }
            else
            {
                select_DM = @"SELECT sum(isnull(j,0))-sum(isnull(d,0)) FROM v_zw_pzhz WHERE zxyt= '{0}' ";
            }

            DataTable dataTable = null;
            try
            {
                DbHelper.Open(userConn);

                dataTable = DbHelper.GetDataTable(userConn, string.Format(select_DM, XMCode));
                if (dataTable.Rows.Count != 0 && !string.IsNullOrEmpty(dataTable.Rows[0][0].ToString()))
                {
                    UseAmount = decimal.Parse(dataTable.Rows[0][0].ToString()).ToString();
                }
                DbHelper.Close(userConn);

            }
            catch (Exception ex)
            {

            }
            return UseAmount;
        }

        /// <summary>
        /// 获取实际发生数
        /// </summary>
        /// <param name="FAccount"></param>
        /// <param name="DtlCode"></param>
        /// <returns></returns>
        public string GetSJFSSbyMXCode(string FAccount, string DtlCode)
        {
            string userConn = "";
            string select_DM;
            string UseAmount = "0";

            /*string FDeclarationUnit = "";//组织
            var dicWhereUnit = new Dictionary<string, object>();
            new CreateCriteria(dicWhereUnit).Add(ORMRestrictions<string>.Eq("Dylx", "08"))
                .Add(ORMRestrictions<Int32>.Eq("DefInt1", 1)).Add(ORMRestrictions<string>.Eq("Dwdm", usercode));
            var CorrespondenceSettings = CorrespondenceSettingsFacade.Find(dicWhereUnit).Data;
            if (CorrespondenceSettings.Count > 0)
            {
                FDeclarationUnit = CorrespondenceSettings[0].Dydm;
            }*/
            /*Dictionary<string, object> conndic = new Dictionary<string, object>();
            new CreateCriteria(conndic)
                .Add(ORMRestrictions<string>.Eq("Dylx", "config"))
                .Add(ORMRestrictions<string>.Eq("DefStr1", FDeclarationUnit));
            IList<CorrespondenceSettings2Model> CorrespondenceSettings2s = CorrespondenceSettings2Facade.Find(conndic).Data;
            if (CorrespondenceSettings2s.Count > 0 && CorrespondenceSettings2s[0].DefStr2 != null)
            {
                userConn = CorrespondenceSettings2s[0].DefStr2;
            }
            else
            {
                return "0.00";//return "老G6H数据库配置有误！";
            }*/
            //连接串更改为从基础数据-账套中取  QtAccountRule
            Dictionary<string, object> conndic = new Dictionary<string, object>();
            new CreateCriteria(conndic).Add(ORMRestrictions<string>.Eq("Dm", FAccount));
            var Accounts = QtAccountFacade.Find(conndic).Data;
            if (Accounts.Count > 0 && !string.IsNullOrEmpty(Accounts[0].FConn))
            {
                userConn = Accounts[0].FConn;
            }
            else
            {
                return "0.00";//return "老G6H数据库配置有误！";
            }


            if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
            {
                select_DM = @"SELECT sum(nvl(j,0))-sum(nvl(d,0)) FROM v_zw_pzhz WHERE mxxm= '{0}' ";
            }
            else
            {
                select_DM = @"SELECT sum(isnull(j,0))-sum(isnull(d,0)) FROM v_zw_pzhz WHERE mxxm= '{0}' ";
            }

            DataTable dataTable = null;
            try
            {
                DbHelper.Open(userConn);

                dataTable = DbHelper.GetDataTable(userConn, string.Format(select_DM, DtlCode));
                if (dataTable.Rows.Count != 0 && !string.IsNullOrEmpty(dataTable.Rows[0][0].ToString()))
                {
                    UseAmount = decimal.Parse(dataTable.Rows[0][0].ToString()).ToString();
                }
                DbHelper.Close(userConn);
            }
            catch (Exception ex)
            {

            }

            return UseAmount;
        }
        #endregion


        /// <summary>
        /// 回撤
        /// </summary>
        /// <param name="approveCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public PagedResult<BudgetMstModel> FindUnvalidPiid(string approveCode, string userId)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();

            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("Bizid", approveCode))
                .Add(ORMRestrictions<string>.Eq("Pk1", userId));
            var result = base.ServiceHelper.LoadWithPageInfinity("GXM3.XM.APPROVE", dicWhere);
            return result;
        }


        /// <summary>
        /// 项目预算调整分析表
        /// </summary>
        /// <param name="dicWhere"></param>
        /// <returns></returns>
        public List<BudgetTZModel> GetBudgetTZList(string userID, Dictionary<string, object> dicWhere)
        {
            List<BudgetTZModel> BudgetTZList = new List<BudgetTZModel>();

            BudgetTZList = BudgetMstFacade.GetBudgetTZList(userID, dicWhere);

            return BudgetTZList;
        }

        /// <summary>
        /// 根据主键集合获取打印数据
        /// </summary>
        /// <param name="phids">主键集合</param>
        /// <returns></returns>
        public List<object> PostPrintData(string[] phids)
        {
            List<object> result = new List<object>();
            foreach (var phid in phids)
            {
                BudgetMstModel projectMst = BudgetMstFacade.Find(long.Parse(phid)).Data;

                var FDeclarationDept = projectMst.FDeclarationDept;
                var FDeclarationDeptList = OrganizationFacade.Find(x => x.OCode == projectMst.FDeclarationDept).Data;
                if (FDeclarationDeptList != null && FDeclarationDeptList.Count > 0)
                {
                    FDeclarationDept = FDeclarationDeptList[0].OName;
                }

                var FProjAttr = projectMst.FProjAttr;
                var FProjAttrList = QTSysSetFacade.Find(x => x.DicType == "ProjectProper" && x.Orgcode == projectMst.FDeclarationUnit && x.TypeCode == projectMst.FProjAttr).Data;
                if (FProjAttrList != null && FProjAttrList.Count > 0)
                {
                    FProjAttr = FProjAttrList[0].TypeName;
                }

                var FDuration = projectMst.FDuration;
                var FDurationList = QTSysSetFacade.Find(x => x.DicType == "TimeLimit" && x.Orgcode == projectMst.FDeclarationUnit && x.TypeCode == projectMst.FDuration).Data;
                if (FDurationList != null && FDurationList.Count > 0)
                {
                    FDuration = FDurationList[0].TypeName;
                }
                BudgetDtlTextContentModel projectDtlText = BudgetDtlTextContentFacade.FindByForeignKey(long.Parse(phid)).Data[0];

                List<object> Budgets = new List<object>();
                IList<BudgetDtlBudgetDtlModel> projectDtlBudgets = BudgetDtlBudgetDtlFacade.FindByForeignKey(long.Parse(phid)).Data;
                foreach (var projectDtlBudget in projectDtlBudgets)
                {
                    var FPaymentMethod = projectDtlBudget.FPaymentMethod;
                    //支付方式代码转名称
                    var FPaymentMethodList = QTSysSetFacade.Find(x => x.DicType == "PayMethodTwo" && x.Orgcode == projectMst.FDeclarationUnit && x.TypeCode == projectDtlBudget.FPaymentMethod).Data;
                    if (FPaymentMethodList.Count > 0)
                    {
                        FPaymentMethod = FPaymentMethodList[0].TypeName;
                    }
                    Budgets.Add(new
                    {
                        FName = projectDtlBudget.FName,
                        FBudgetAccounts = projectDtlBudget.FBudgetAccounts_EXName,
                        FAmount = projectDtlBudget.FAmount,
                        FAmountEdit = projectDtlBudget.FAmountEdit,
                        FAmountAfterEdit = projectDtlBudget.FAmountAfterEdit,
                        FPaymentMethod = FPaymentMethod,
                        FExpensesChannel = projectDtlBudget.FExpensesChannel_EXName,
                        FOtherInstructions = projectDtlBudget.FOtherInstructions,
                        FSourceOfFunds = projectDtlBudget.FSourceOfFunds_EXName,
                        FQtZcgnfl = projectDtlBudget.FQtZcgnfl_EXName
                    });
                }
                IList<QtAttachmentModel> qtAttachments = new List<QtAttachmentModel>();
                qtAttachments = this.QtAttachmentFacade.Find(t => t.BTable == "YS3_BUDGETMST" && t.RelPhid == long.Parse(phid)).Data;

                object data = new
                {
                    DJH = projectMst.PhId,
                    FProjCode = projectMst.FProjCode,
                    FDeclarationDept = FDeclarationDept,
                    FDateofDeclaration = projectMst.FDateofDeclaration,
                    FDeclarer = projectMst.FDeclarer,
                    FProjName = projectMst.FProjName,
                    FProjAttr = FProjAttr,
                    FDuration = FDuration,
                    FExpenseCategory = projectMst.FExpenseCategory_EXName,
                    FStartDate = projectMst.FStartDate,
                    FEndDate = projectMst.FEndDate,
                    FFunctionalOverview = projectDtlText.FFunctionalOverview,
                    FProjBasis = projectDtlText.FProjBasis,
                    FFeasibility = projectDtlText.FFeasibility,
                    FNecessity = projectDtlText.FNecessity,
                    FLTPerformGoal = projectDtlText.FLTPerformGoal,
                    FAnnualPerformGoal = projectDtlText.FAnnualPerformGoal,
                    Budgets = Budgets,
                    FundAppl = BudgetDtlFundApplFacade.FindByForeignKey(long.Parse(phid)).Data,
                    ImplPlan = BudgetDtlImplPlanFacade.FindByForeignKey(long.Parse(phid)).Data,
                    FMeetiingSummaryNo = projectMst.FMeetiingSummaryNo,
                    FMeetingTime = projectMst.FMeetingTime,
                    FLeadingOpinions = projectDtlText.FLeadingOpinions,
                    FChairmanOpinions = projectDtlText.FChairmanOpinions,
                    FBz = projectDtlText.FBz,
                    FDeptOpinions = projectDtlText.FDeptOpinions,
                    FDeptOpinions2 = projectDtlText.FDeptOpinions2,
                    FResolution = projectDtlText.FResolution,
                    FadjustText = projectDtlText.FadjustText,
                    FVerNo = projectMst.FVerNo,
                    FType = projectMst.FType,
                    QtAttachments = qtAttachments
                };
                result.Add(data);
            }
            return result;
        }

        /// <summary>
        /// 保存绩效跟踪数据
        /// </summary>
        /// <param name="adddata"></param>
        /// <param name="updatedata"></param>
        /// <param name="deletedata"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveTracking(List<JxTrackingModel> adddata, List<JxTrackingModel> updatedata, List<string> deletedata)
        {
            SavedResult<Int64> result = new SavedResult<Int64>();
            List<JxTrackingModel> data = new List<JxTrackingModel>();
            if (adddata != null && adddata.Count > 0)
            {
                for (var i = 0; i < adddata.Count; i++)
                {
                    JxTrackingModel a = adddata[i];
                    a.PersistentState = PersistentState.Added;
                    data.Add(a);
                }
            }
            if (updatedata != null && updatedata.Count > 0)
            {
                for (var j = 0; j < updatedata.Count; j++)
                {
                    JxTrackingModel b = updatedata[j];
                    JxTrackingModel c = JxTrackingFacade.Find(b.PhId).Data;
                    c.FTime = b.FTime;
                    c.FText = b.FText;
                    c.FDeclarationUnit = b.FDeclarationUnit;
                    c.FProjName = b.FProjName;
                    c.FProjAmount = b.FProjAmount;
                    c.FActualAmount = b.FActualAmount;
                    c.FBalanceAmount = b.FBalanceAmount;
                    c.FImplRate = b.FImplRate;
                    c.PersistentState = PersistentState.Modified;
                    data.Add(c);
                }

            }
            if (deletedata != null && deletedata.Count > 0)
            {
                for (var x = 0; x < deletedata.Count; x++)
                {
                    JxTrackingModel d = JxTrackingFacade.Find(long.Parse(deletedata[x])).Data;
                    d.PersistentState = PersistentState.Deleted;
                    data.Add(d);
                }
            }
            result = JxTrackingFacade.Save<Int64>(data, "");
            return result;
        }

        #region 取金格控件标签值
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="arcID"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetKingGridTagRelateData(string type, long arcID)
        {
            return BudgetMstFacade.GetKingGridTagRelateData(type, arcID);
        }


        /// <summary>
        /// 根据组织获取该组织的上年与本年决算
        /// </summary>
        /// <param name="orgId">组织ID</param>
        /// <param name="orgCode">组织Code</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public IList<YsAccountModel> GetYsAccounts(string orgId, string orgCode, string year)
        {
            //取本年预算的所有明细数据
            IList<BudgetMstModel> budgetMsts = new List<BudgetMstModel>();//用来存主表数据
            IList<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtls = new List<BudgetDtlBudgetDtlModel>();//用来存明细数据
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic).Add(ORMRestrictions<string>.Eq("FYear", year))
               .Add(ORMRestrictions<string>.Eq("FDeclarationUnit", orgCode))
               .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
               .Add(ORMRestrictions<string>.Eq("FApproveStatus", "3"))
               .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));
            budgetMsts = this.BudgetMstFacade.Find(dic).Data;
            if (budgetMsts != null && budgetMsts.Count > 0)
            {
                var phids = budgetMsts.Select(t => t.PhId).Distinct().ToList();
                if (phids != null && phids.Count > 0)
                {
                    dic.Clear();
                    new CreateCriteria(dic).Add(ORMRestrictions<List<long>>.In("MstPhid", phids));
                    budgetDtlBudgetDtls = this.BudgetDtlBudgetDtlFacade.Find(dic).Data;
                }
            }

            //获取本年决算与上年决算
            var result = this.YsAccountFacade.GetYsAccounts(orgId, orgCode, year);
            result = result.OrderBy(t => t.SUBJECTCODE).ToList();

            IList<YsAccountModel> ysAccounts = new List<YsAccountModel>();
            if (result != null && result.Count > 0)
            {
                //加入本年预算数据
                foreach (var res in result)
                {
                    if (budgetDtlBudgetDtls != null && budgetDtlBudgetDtls.Count > 0)
                    {
                        //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts.StartsWith(res.SUBJECTCODE)) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts.StartsWith(res.SUBJECTCODE)).Sum(t => t.FAmountAfterEdit);
                        res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && t.FBudgetAccounts.StartsWith(res.SUBJECTCODE)) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && t.FBudgetAccounts.StartsWith(res.SUBJECTCODE)));
                        if (res.SUBJECTCODE == "4BNHJSR")
                        {
                            //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts.StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts.StartsWith("4")).Sum(t => t.FAmountAfterEdit);
                            res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && t.FBudgetAccounts.StartsWith("4")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && t.FBudgetAccounts.StartsWith("4")));
                        }
                        if (res.SUBJECTCODE == "5BNHJZC")
                        {
                            //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts.StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts.StartsWith("5")).Sum(t => t.FAmountAfterEdit);
                            res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && t.FBudgetAccounts.StartsWith("5")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && t.FBudgetAccounts.StartsWith("5")));
                        }
                        if (res.SUBJECTCODE == "5QM1")
                        {
                            res.BUDGETTOTAL = (result.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : result.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").BUDGETTOTAL) + (result.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : result.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").BUDGETTOTAL);
                        }
                        if (res.SUBJECTCODE == "5QM2")
                        {
                            //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts.StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts.StartsWith("331")).Sum(t => t.FAmountAfterEdit);
                            res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && t.FBudgetAccounts.StartsWith("331")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && t.FBudgetAccounts.StartsWith("331")));
                        }
                        if (res.SUBJECTCODE == "5QM3")
                        {
                            //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts.StartsWith("311")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts.StartsWith("311")).Sum(t => t.FAmountAfterEdit);
                            res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && t.FBudgetAccounts.StartsWith("311")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && t.FBudgetAccounts.StartsWith("311")));
                        }
                        if (res.SUBJECTCODE == "5QM6")
                        {
                            res.BUDGETTOTAL = (result.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : result.ToList().Find(t => t.SUBJECTCODE == "5QM1").BUDGETTOTAL) + (result.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : result.ToList().Find(t => t.SUBJECTCODE == "5QM2").BUDGETTOTAL)
                                    + (result.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : result.ToList().Find(t => t.SUBJECTCODE == "5QM3").BUDGETTOTAL) - (result.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : result.ToList().Find(t => t.SUBJECTCODE == "5QM4").BUDGETTOTAL)
                                    - (result.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : result.ToList().Find(t => t.SUBJECTCODE == "5QM5").BUDGETTOTAL);
                        }
                    }
                }
                foreach (var res in result)
                {
                    if (res.SUBJECTCODE.Length == 3)
                    {
                        YsAccountModel ys = new YsAccountModel();
                        ysAccounts.Add(GetCompleteYsAccount(result, res.SUBJECTCODE, ys));
                    }
                    else
                    {
                        if (res.SUBJECTCODE == "4BNHJSR" || res.SUBJECTCODE == "5BNHJZC" || res.SUBJECTCODE.StartsWith("5QM"))
                        {
                            ysAccounts.Add(res);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            return ysAccounts;
        }

        /// <summary>
        /// 根据组织获取该组织的上年与本年决算
        /// </summary>
        /// <param name="orgId">组织ID</param>
        /// <param name="orgCode">组织Code</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public IList<YsAccountModel> GetYsAccounts2(string orgId, string orgCode, string year)
        {
            //取本年预算的所有明细数据
            IList<BudgetMstModel> budgetMsts = new List<BudgetMstModel>();//用来存主表数据
            IList<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtls = new List<BudgetDtlBudgetDtlModel>();//用来存明细数据
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic).Add(ORMRestrictions<string>.Eq("FYear", year))
               .Add(ORMRestrictions<string>.Eq("FDeclarationUnit", orgCode))
               .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
               .Add(ORMRestrictions<string>.Eq("FApproveStatus", "3"))
               .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));
            budgetMsts = this.BudgetMstFacade.Find(dic).Data;
            if (budgetMsts != null && budgetMsts.Count > 0)
            {
                var phids = budgetMsts.Select(t => t.PhId).Distinct().ToList();
                if (phids != null && phids.Count > 0)
                {
                    dic.Clear();
                    new CreateCriteria(dic).Add(ORMRestrictions<List<long>>.In("MstPhid", phids));
                    budgetDtlBudgetDtls = this.BudgetDtlBudgetDtlFacade.Find(dic).Data;
                }
            }

            //获取本年决算与上年决算
            var result = this.YsAccountFacade.GetYsAccounts(orgId, orgCode, year);
            result = result.OrderBy(t => t.SUBJECTCODE).ToList();

            IList<YsAccountModel> ysAccounts = new List<YsAccountModel>();
            if (result != null && result.Count > 0)
            {
                //加入本年预算数据
                foreach (var res in result)
                {
                    if (budgetDtlBudgetDtls != null && budgetDtlBudgetDtls.Count > 0)
                    {
                        //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts.StartsWith(res.SUBJECTCODE)) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts.StartsWith(res.SUBJECTCODE)).Sum(t => t.FAmountAfterEdit);
                        res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && t.FBudgetAccounts.StartsWith(res.SUBJECTCODE)) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && t.FBudgetAccounts.StartsWith(res.SUBJECTCODE)));
                        if (res.SUBJECTCODE == "4BNHJSR")
                        {
                            //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts.StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts.StartsWith("4")).Sum(t => t.FAmountAfterEdit);
                            res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && t.FBudgetAccounts.StartsWith("4")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && t.FBudgetAccounts.StartsWith("4")));
                        }
                        if (res.SUBJECTCODE == "5BNHJZC")
                        {
                            //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts.StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts.StartsWith("5")).Sum(t => t.FAmountAfterEdit);
                            res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && t.FBudgetAccounts.StartsWith("5")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && t.FBudgetAccounts.StartsWith("5")));
                        }
                        if (res.SUBJECTCODE == "5QM1")
                        {
                            res.BUDGETTOTAL = (result.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : result.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").BUDGETTOTAL) - (result.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : result.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").BUDGETTOTAL);
                        }
                        if (res.SUBJECTCODE == "5QM2")
                        {
                            //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts.StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts.StartsWith("331")).Sum(t => t.FAmountAfterEdit);
                            res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && t.FBudgetAccounts.StartsWith("331")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && t.FBudgetAccounts.StartsWith("331")));
                        }
                        if (res.SUBJECTCODE == "5QM3")
                        {
                            //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts.StartsWith("311")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts.StartsWith("311")).Sum(t => t.FAmountAfterEdit);
                            res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && t.FBudgetAccounts.StartsWith("311")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && t.FBudgetAccounts.StartsWith("311")));
                        }
                        if (res.SUBJECTCODE == "5QM6")
                        {
                            res.BUDGETTOTAL = (result.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : result.ToList().Find(t => t.SUBJECTCODE == "5QM1").BUDGETTOTAL) + (result.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : result.ToList().Find(t => t.SUBJECTCODE == "5QM2").BUDGETTOTAL)
                                    + (result.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : result.ToList().Find(t => t.SUBJECTCODE == "5QM3").BUDGETTOTAL) - (result.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : result.ToList().Find(t => t.SUBJECTCODE == "5QM4").BUDGETTOTAL)
                                    - (result.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : result.ToList().Find(t => t.SUBJECTCODE == "5QM5").BUDGETTOTAL);
                        }
                    }
                }
                //foreach (var res in result)
                //{
                //    if (res.SUBJECTCODE.Length == 3)
                //    {
                //        YsAccountModel ys = new YsAccountModel();
                //        ysAccounts.Add(GetCompleteYsAccount(result, res.SUBJECTCODE, ys));
                //    }
                //    else
                //    {
                //        if (res.SUBJECTCODE == "4BNHJSR" || res.SUBJECTCODE == "5BNHJZC" || res.SUBJECTCODE.StartsWith("5QM"))
                //        {
                //            ysAccounts.Add(res);
                //        }
                //        else
                //        {
                //            continue;
                //        }
                //    }
                //}
            }
            return ysAccounts;
        }

        /// <summary>
        /// 根据明细获取金额总值
        /// </summary>
        /// <param name="budgetDtlBudgetDtls">明细集合</param>
        /// <returns></returns>
        public decimal GetSumAmount(List<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtls)
        {
            decimal sum = 0;
            if (budgetDtlBudgetDtls != null && budgetDtlBudgetDtls.Count > 0)
            {
                foreach (var budget in budgetDtlBudgetDtls)
                {
                    //if(budget.FAmountAfterEdit.Equals(null))
                    //{
                    //    if(budget.FAmount.Equals(null))
                    //    {
                    //        continue;
                    //    }
                    //    else
                    //    {
                    //        sum += budget.FAmount;
                    //    }
                    //}
                    //else
                    //{
                    //    sum += budget.FAmountAfterEdit;
                    //}
                    if (budget.FAmount.Equals(null))
                    {
                        continue;
                    }
                    else
                    {
                        sum += budget.FAmount;
                    }
                }
            }
            return sum;
        }

        /// <summary>
        /// 返回一个有下级科目的科目
        /// </summary>
        /// <param name="ysAccounts">总的集合</param>
        /// <param name="subjectCode">科目编码</param>
        /// <param name="ys">得到的科目</param>
        /// <returns></returns>
        public YsAccountModel GetCompleteYsAccount(IList<YsAccountModel> ysAccounts, string subjectCode, YsAccountModel ys)
        {
            ys = ysAccounts.ToList().Find(t => t.SUBJECTCODE == subjectCode);
            var result = ysAccounts.ToList().FindAll(t => t.SUBJECTCODE.StartsWith(subjectCode) && t.SUBJECTCODE != subjectCode && t.SUBJECTCODE.Length == (subjectCode.Length + 2));
            if (result != null && result.Count > 0)
            {
                ys.Childrens = result;
                foreach (var res in ys.Childrens)
                {
                    GetCompleteYsAccount(ysAccounts, res.SUBJECTCODE, res);
                }
            }
            return ys;
        }


        /// <summary>
        /// 修改预决算数据
        /// </summary>
        /// <param name="ysAccounts">列表集合</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public SavedResult<long> SaveAccountList(List<YsAccountModel> ysAccounts, string orgId, string orgCode, string year)
        {
            return this.YsAccountFacade.SaveAccountList(ysAccounts, orgId, orgCode, year);
        }
        #endregion

        #region//组织用户信息，用controller服务
        /// <summary>
        /// 根据用户主键获取用户信息
        /// </summary>
        /// <param name="phid"></param>
        /// <returns></returns>
        public User2Model GetUser(long phid)
        {
            User2Model User = this.UserFacade.Find(phid).Data;
            return User;
        }

        /// <summary>
        /// 根据组织主键获取组织信息
        /// </summary>
        /// <param name="phid"></param>
        /// <returns></returns>
        public OrganizeModel GetOrganize(long phid)
        {
            OrganizeModel organize = this.OrganizationFacade.Find(phid).Data;
            return organize;
        }

        /// <summary>
        /// 根据组织编码获取组织信息
        /// </summary>
        /// <param name="code">组织编码</param>
        /// <returns></returns>
        public OrganizeModel GetOrganizeByCode(string code)
        {
            OrganizeModel organize = new OrganizeModel();
            var organizes = this.OrganizationFacade.Find(t => t.OCode == code).Data;
            if (organizes != null && organizes.Count > 0)
            {
                organize = organizes[0];
            }
            return organize;
        }

        #endregion

        #region//加入绩效

        /// <summary>
        /// 获取新的项目绩效集合
        /// </summary>
        /// <param name="budgetDtlPerformTargets">项目带的绩效集合</param>
        /// <param name="targetTypeCode">父级节点</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        public List<BudgetDtlPerformTargetModel> GetNewBudPerformTargets(List<BudgetDtlPerformTargetModel> budgetDtlPerformTargets, string targetTypeCode, long orgId, string orgCode)
        {
            if (budgetDtlPerformTargets != null && budgetDtlPerformTargets.Count > 0)
            {
                //该组织下所有的指标类型集合
                IList<PerformEvalTargetTypeModel> performEvalTargetTypes = this.PerformEvalTargetTypeFacade.Find(t => t.Orgcode == orgCode && t.Orgid == orgId).Data;

                int max = 0;
                foreach (var proPer in budgetDtlPerformTargets)
                {
                    if (proPer.FTargetTypeCode.Length / 2 > max)
                    {
                        max = proPer.FTargetTypeCode.Length / 2;
                    }
                }
                foreach (var per in budgetDtlPerformTargets)
                {
                    per.FTargetTypeName = performEvalTargetTypes.ToList().Find(t => t.FCode == per.FTargetTypeCode) == null ? "" : performEvalTargetTypes.ToList().Find(t => t.FCode == per.FTargetTypeCode).FName;
                    per.FTargetTypePerantCode = performEvalTargetTypes.ToList().Find(t => t.FCode == per.FTargetTypeCode) == null ? "" : performEvalTargetTypes.ToList().Find(t => t.FCode == per.FTargetTypeCode).FCode;
                    if (max - per.FTargetTypeCode.Length / 2 == 0)
                    {
                        per.TypeCount = 1;
                        per.FTargetTypeCode1 = per.FTargetTypeCode;
                        per.FTargetTypeName1 = per.FTargetTypeName;
                        GetNewBudPerformTarget(performEvalTargetTypes, per, targetTypeCode, 1);
                    }
                    else if (max - per.FTargetTypeCode.Length / 2 == 1)
                    {
                        per.TypeCount = 2;
                        per.FTargetTypeCode2 = per.FTargetTypeCode;
                        per.FTargetTypeName2 = per.FTargetTypeName;
                        GetNewBudPerformTarget(performEvalTargetTypes, per, targetTypeCode, 1);
                    }
                    else if (max - per.FTargetTypeCode.Length / 2 == 2)
                    {
                        per.TypeCount = 3;
                        per.FTargetTypeCode3 = per.FTargetTypeCode;
                        per.FTargetTypeName3 = per.FTargetTypeName;
                        GetNewBudPerformTarget(performEvalTargetTypes, per, targetTypeCode, 1);
                    }
                    else if (max - per.FTargetTypeCode.Length / 2 == 3)
                    {
                        per.TypeCount = 4;
                        per.FTargetTypeCode4 = per.FTargetTypeCode;
                        per.FTargetTypeName4 = per.FTargetTypeName;
                        GetNewBudPerformTarget(performEvalTargetTypes, per, targetTypeCode, 1);
                    }
                    else if (max - per.FTargetTypeCode.Length / 2 == 4)
                    {
                        per.TypeCount = 5;
                        per.FTargetTypeCode5 = per.FTargetTypeCode;
                        per.FTargetTypeName5 = per.FTargetTypeName;
                        GetNewBudPerformTarget(performEvalTargetTypes, per, targetTypeCode, 1);
                    }
                    else
                    {
                        continue;
                    }
                }

                budgetDtlPerformTargets = budgetDtlPerformTargets.OrderBy(t => t.FTargetTypeCode5).OrderBy(t => t.FTargetTypeCode4).OrderBy(t => t.FTargetTypeCode3).OrderBy(t => t.FTargetTypeCode2).OrderBy(t => t.FTargetTypeCode1).ToList();
            }
            return budgetDtlPerformTargets;
        }

        /// <summary>
        /// 获取新的绩效明细
        /// </summary>
        /// <param name="performEvalTargetTypes">该组织绩效指标类型集合</param>
        /// <param name="budgetDtlPerformTarget">单个绩效明细</param>
        /// <param name="targetTypeCode">父节点</param>
        /// <param name="num">数量</param>
        /// <returns></returns>
        public BudgetDtlPerformTargetModel GetNewBudPerformTarget(IList<PerformEvalTargetTypeModel> performEvalTargetTypes, BudgetDtlPerformTargetModel budgetDtlPerformTarget, string targetTypeCode, int num)
        {
            if (budgetDtlPerformTarget != null && performEvalTargetTypes != null && performEvalTargetTypes.Count > 0 && budgetDtlPerformTarget.FTargetTypePerantCode != targetTypeCode)
            {
                if (budgetDtlPerformTarget.FTargetTypeCode.Length - num * 2 >= 0)
                {
                    var type = performEvalTargetTypes.ToList().Find(t => t.FCode == budgetDtlPerformTarget.FTargetTypeCode.Substring(0, budgetDtlPerformTarget.FTargetTypeCode.Length - num * 2));
                    if (type != null)
                    {
                        if (budgetDtlPerformTarget.TypeCount == 1)
                        {
                            budgetDtlPerformTarget.FTargetTypeCode2 = type.FCode;
                            budgetDtlPerformTarget.FTargetTypeName2 = type.FName;
                            budgetDtlPerformTarget.TypeCount++;
                            budgetDtlPerformTarget.FTargetTypePerantCode = type.FCode;
                            num++;
                            GetNewBudPerformTarget(performEvalTargetTypes, budgetDtlPerformTarget, targetTypeCode, num);
                        }
                        else if (budgetDtlPerformTarget.TypeCount == 2)
                        {
                            budgetDtlPerformTarget.FTargetTypeCode3 = type.FCode;
                            budgetDtlPerformTarget.FTargetTypeName3 = type.FName;
                            budgetDtlPerformTarget.TypeCount++;
                            budgetDtlPerformTarget.FTargetTypePerantCode = type.FCode;
                            num++;
                            GetNewBudPerformTarget(performEvalTargetTypes, budgetDtlPerformTarget, targetTypeCode, num);
                        }
                        else if (budgetDtlPerformTarget.TypeCount == 3)
                        {
                            budgetDtlPerformTarget.FTargetTypeCode4 = type.FCode;
                            budgetDtlPerformTarget.FTargetTypeName4 = type.FName;
                            budgetDtlPerformTarget.TypeCount++;
                            budgetDtlPerformTarget.FTargetTypePerantCode = type.FCode;
                            num++;
                            GetNewBudPerformTarget(performEvalTargetTypes, budgetDtlPerformTarget, targetTypeCode, num);
                        }
                        else if (budgetDtlPerformTarget.TypeCount == 4)
                        {
                            budgetDtlPerformTarget.FTargetTypeCode5 = type.FCode;
                            budgetDtlPerformTarget.FTargetTypeName5 = type.FName;
                            budgetDtlPerformTarget.TypeCount++;
                            budgetDtlPerformTarget.FTargetTypePerantCode = type.FCode;
                            num++;
                            GetNewBudPerformTarget(performEvalTargetTypes, budgetDtlPerformTarget, targetTypeCode, num);
                        }
                        else
                        {

                        }
                    }
                }

            }
            return budgetDtlPerformTarget;
        }


        /// <summary>
        /// 根据组织id获取组织列表
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<OrganizeModel> GetOrganizeByList(string code)
        {
            OrganizeModel organize = new OrganizeModel();
            var Query = this.OrganizationFacade.Find(t => t.OCode.StartsWith(code)).Data;
            List<OrganizeModel> list = Query.ToList();
            return list;
        }
        /// <summary>
        /// 根据字典取数据
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public List<BudgetDtlBudgetDtlModel> FindProjectDtlBudgetDtl(Dictionary<string, object> dictionary)
        {
            var Query = BudgetDtlBudgetDtlFacade.Find(dictionary);
            return Query.Data.ToList();
        }

        #endregion

        #region//vue绩效跟踪

        /// <summary>
        /// 保存绩效跟踪集合
        /// </summary>
        /// <param name="jxTrackings">绩效跟踪集合</param>
        /// <returns></returns>
        public SavedResult<long> SaveJxTracks(List<JxTrackingModel> jxTrackings)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            savedResult = this.JxTrackingFacade.Save<long>(jxTrackings);
            return savedResult;
        }
        #endregion

        #region//项目调整分析表

        /// <summary>
        /// 根据年份与组织编码获取相应的项目预算调整分析表
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="orgCode">组织编码</param>
        /// <returns></returns>
        public List<BudgetAdjustAnalyseModel> GetBudgetAdjustAnalyseList(string year, string orgCode)
        {
            return this.BudgetMstFacade.GetBudgetAdjustAnalyseList(year, orgCode);
        }
        #endregion

        #region//民生银行

        /// <summary>
        /// 根据预算主表数据补充明细数据
        /// </summary>
        /// <param name="budgetMsts">主表数据集合</param>
        /// <returns></returns>
        public List<BudgetAllDataModel> GetBudegtAllDataModels(List<BudgetMstModel> budgetMsts)
        {
            List<BudgetAllDataModel> budgetAllDatas = new List<BudgetAllDataModel>();
            if (budgetMsts != null && budgetMsts.Count > 0)
            {
                List<long> mstPhids = new List<long>();
                mstPhids = budgetMsts.Select(t => t.PhId).ToList();
                if (mstPhids != null && mstPhids.Count > 0)
                {
                    //主表对应附件集合
                    List<QtAttachmentModel> qtAttachments = QtAttachmentFacade.Find(t => t.BTable == "YS3_BUDGETMST" && mstPhids.Contains(t.RelPhid)).Data.ToList();
                    //主表对应明细所有数据
                    List<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtls = BudgetDtlBudgetDtlFacade.Find(t => mstPhids.Contains(t.MstPhid)).Data.ToList();
                    List<BudgetDtlFundApplModel> budgetDtlFundAppls = BudgetDtlFundApplFacade.Find(t => mstPhids.Contains(t.MstPhid)).Data.ToList();
                    List<BudgetDtlImplPlanModel> budgetDtlImplPlans = BudgetDtlImplPlanFacade.Find(t => mstPhids.Contains(t.MstPhid)).Data.ToList();
                    List<BudgetDtlPerformTargetModel> budgetDtlPerformTargets = BudgetDtlPerformTargetFacade.Find(t => mstPhids.Contains(t.MstPhId)).Data.ToList();
                    List<BudgetDtlPersonNameModel> budgetDtlPersonNames = BudgetDtlPersonNameFacade.Find(t => mstPhids.Contains(t.MstPhid)).Data.ToList();
                    List<BudgetDtlPersonnelModel> budgetDtlPersonnels = BudgetDtlPersonnelFacade.Find(t => mstPhids.Contains(t.MstPhid)).Data.ToList();
                    List<BudgetDtlPurchaseDtlModel> budgetDtlPurchaseDtls = BudgetDtlPurchaseDtlFacade.Find(t => mstPhids.Contains(t.MstPhid)).Data.ToList();
                    List<BudgetDtlPurDtl4SOFModel> budgetDtlPurDtl4SOFs = BudgetDtlPurDtl4SOFFacade.Find(t => mstPhids.Contains(t.MstPhid)).Data.ToList();
                    List<BudgetDtlTextContentModel> budgetDtlTextContents = BudgetDtlTextContentFacade.Find(t => mstPhids.Contains(t.MstPhid)).Data.ToList();

                    //基础数据集合（为转名称做准备）
                    IList<QTSysSetModel> allSysSets = this.QTSysSetFacade.Find(t => t.DicType == "ZcfxName" && t.Orgcode == budgetMsts[0].FDeclarationUnit).Data;

                    //科目集合（为转名称做准备）
                    IList<BudgetAccountsModel> allBudgetAccounts = this.BudgetAccountsFacade.Find(t => t.PhId != 0).Data;

                    //for循环逐个加入数据
                    foreach (var mst in budgetMsts)
                    {
                        BudgetAllDataModel budgetAllData = new BudgetAllDataModel();
                        budgetAllData.BudgetMst = mst;
                        if (qtAttachments != null && qtAttachments.Count > 0)
                        {
                            budgetAllData.BudgetAttachments = qtAttachments.FindAll(t => t.RelPhid == mst.PhId);
                        }
                        if (budgetDtlBudgetDtls != null && budgetDtlBudgetDtls.Count > 0)
                        {
                            foreach (var dtl in budgetDtlBudgetDtls)
                            {
                                if (allBudgetAccounts != null && allBudgetAccounts.Count > 0)
                                {
                                    dtl.FBudgetAccounts_EXName = allBudgetAccounts.ToList().Find(t => t.KMDM == dtl.FBudgetAccounts) == null ? "" : allBudgetAccounts.ToList().Find(t => t.KMDM == dtl.FBudgetAccounts).KMMC;
                                }
                                if (allSysSets != null && allSysSets.Count > 0)
                                {
                                    dtl.FSubitemName = allSysSets.ToList().Find(t => t.TypeCode == dtl.FSubitemCode) == null ? "" : allSysSets.ToList().Find(t => t.TypeCode == dtl.FSubitemCode).TypeName;
                                }
                            }
                            budgetAllData.BudgetDtlBudgetDtls = budgetDtlBudgetDtls.FindAll(t => t.MstPhid == mst.PhId).OrderBy(t => t.FDtlCode).ToList();
                            budgetAllData.BudgetDtlBudgetDtls2 = budgetAllData.BudgetDtlBudgetDtls;
                        }
                        if (budgetDtlFundAppls != null && budgetDtlFundAppls.Count > 0)
                        {
                            budgetAllData.BudgetDtlFundAppls = budgetDtlFundAppls.FindAll(t => t.MstPhid == mst.PhId);
                        }
                        if (budgetDtlImplPlans != null && budgetDtlImplPlans.Count > 0)
                        {
                            budgetAllData.BudgetDtlImplPlans = budgetDtlImplPlans.FindAll(t => t.MstPhid == mst.PhId);
                        }
                        if (budgetDtlPerformTargets != null && budgetDtlPerformTargets.Count > 0)
                        {
                            budgetAllData.BudgetDtlPerformTargets = budgetDtlPerformTargets.FindAll(t => t.MstPhId == mst.PhId);
                        }
                        if (budgetDtlPersonNames != null && budgetDtlPersonNames.Count > 0)
                        {
                            budgetAllData.BudgetDtlPersonNames = budgetDtlPersonNames.FindAll(t => t.MstPhid == mst.PhId);
                        }
                        if (budgetDtlPersonnels != null && budgetDtlPersonnels.Count > 0)
                        {
                            budgetAllData.BudgetDtlPersonnels = budgetDtlPersonnels.FindAll(t => t.MstPhid == mst.PhId);
                        }
                        if (budgetDtlPurchaseDtls != null && budgetDtlPurchaseDtls.Count > 0)
                        {
                            budgetAllData.BudgetDtlPurchaseDtls = budgetDtlPurchaseDtls.FindAll(t => t.MstPhid == mst.PhId);
                        }
                        if (budgetDtlPurDtl4SOFs != null && budgetDtlPurDtl4SOFs.Count > 0)
                        {
                            budgetAllData.BudgetDtlPurDtl4SOFs = budgetDtlPurDtl4SOFs.FindAll(t => t.MstPhid == mst.PhId);
                        }
                        if (budgetDtlTextContents != null && budgetDtlTextContents.Count > 0)
                        {
                            budgetAllData.BudgetDtlTextContents = budgetDtlTextContents.FindAll(t => t.MstPhid == mst.PhId);
                        }
                        budgetAllDatas.Add(budgetAllData);
                    }
                }
            }
            return budgetAllDatas;
        }
        #endregion
    }
}

