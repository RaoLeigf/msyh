#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectMstFacade
    * 命名空间：        GXM3.XM.Facade
    * 文 件 名：        ProjectMstFacade.cs
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
using SUP.Common.DataEntity;
using SUP.Common.DataAccess;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Facade;

using GXM3.XM.Facade.Interface;
using GXM3.XM.Rule.Interface;
using GXM3.XM.Model.Domain;
using GXM3.XM.Model.Enums;

using NG3.WorkFlow.Interfaces;
using NG3.WorkFlow.Rule;
using GYS3.YS.Rule.Interface;
using GQT3.QT.Rule.Interface;
using GQT3.QT.Model.Domain;
using SUP.Common.Base;
using Enterprise3.Common.Base.Criterion;
using Newtonsoft.Json.Linq;
using GSP3.SP.Model.Domain;

namespace GXM3.XM.Facade
{
	/// <summary>
	/// ProjectMst业务组装处理类
	/// </summary>
    public partial class ProjectMstFacade : EntFacadeBase<ProjectMstModel>, IProjectMstFacade, IWorkFlowPlugin
    {
		#region 类变量及属性
		/// <summary>
        /// ProjectMst业务逻辑处理对象
        /// </summary>
		IProjectMstRule ProjectMstRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IProjectMstRule;
            }
        }
		/// <summary>
        /// ProjectDtlImplPlan业务逻辑处理对象
        /// </summary>
		IProjectDtlImplPlanRule ProjectDtlImplPlanRule { get; set; }
		/// <summary>
        /// ProjectDtlTextContent业务逻辑处理对象
        /// </summary>
		IProjectDtlTextContentRule ProjectDtlTextContentRule { get; set; }
		/// <summary>
        /// ProjectDtlFundAppl业务逻辑处理对象
        /// </summary>
		IProjectDtlFundApplRule ProjectDtlFundApplRule { get; set; }
		/// <summary>
        /// ProjectDtlBudgetDtl业务逻辑处理对象
        /// </summary>
		IProjectDtlBudgetDtlRule ProjectDtlBudgetDtlRule { get; set; }
        /// <summary>
        /// ProjectDtlPurchaseDtl业务逻辑处理对象
        /// </summary>
        IProjectDtlPurchaseDtlRule ProjectDtlPurchaseDtlRule { get; set; }
        /// <summary>
        /// ProjectDtlPurDtl4SOF业务逻辑处理对象
        /// </summary>
        IProjectDtlPurDtl4SOFRule ProjectDtlPurDtl4SOFRule { get; set; }
        /// <summary>
        /// ProjectDtlPerformTarget业务逻辑处理对象
        /// </summary>
        IProjectDtlPerformTargetRule ProjectDtlPerformTargetRule { get; set; }

        IBudgetMstRule BudgetMstRule { get; set; }

        IQtOrgDygxRule QtOrgDygxRule { get; set; }

        ICorrespondenceSettings2Rule CorrespondenceSettings2Rule { get; set; }

        IBudgetAccountsRule BudgetAccountsRule { get; set; }

        IQtAccountRule QtAccountRule { get; set; }
        #endregion

        #region 重载方法
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValType"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public override FindedResult<ProjectMstModel> Find<TValType>(TValType id)
        {
            FindedResult<ProjectMstModel> Result = base.Find(id);
            #region 列表Grid代码转名称
            RichHelpDac helpdac = new RichHelpDac();
            //helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
            //helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            helpdac.CodeToName<ProjectMstModel>(Result.Data, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist");
            helpdac.CodeToName<ProjectMstModel>(Result.Data, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode");
            helpdac.CodeToName<ProjectMstModel>(Result.Data, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist");
            helpdac.CodeToName<ProjectMstModel>(Result.Data, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory");
            helpdac.CodeToName<ProjectMstModel>(Result.Data, "FApprover", "FApprover_EXName", "fg3_user");
            helpdac.CodeToName<ProjectMstModel>(Result.Data, "FPerformType", "FPerformType_EXName", "GHPerformEvalTargetTypeTree");
            helpdac.CodeToName<ProjectMstModel>(Result.Data, "FPerformEvalType", "FPerformEvalType_EXName", "GHPerformEvalType");
            //helpdac.CodeToName<ProjectMstModel>(Result.Data, "FAccount", "FAccount_EXName", "gh_Account");
            #endregion
            return Result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicWhere"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public override FindedResults<ProjectMstModel> Find(Dictionary<string, object> dicWhere, params string[] sorts)
        {
            FindedResults<ProjectMstModel> Result = base.Find(dicWhere, sorts);
            #region 列表Grid代码转名称
            RichHelpDac helpdac = new RichHelpDac();
            helpdac.CodeToName<ProjectMstModel>(Result.Data, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist", "");
            helpdac.CodeToName<ProjectMstModel>(Result.Data, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode", "");
            helpdac.CodeToName<ProjectMstModel>(Result.Data, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
            helpdac.CodeToName<ProjectMstModel>(Result.Data, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory", "");
            helpdac.CodeToName<ProjectMstModel>(Result.Data, "FApprover", "FApprover_EXName", "fg3_user", "");
            helpdac.CodeToName<ProjectMstModel>(Result.Data, "FPerformType", "FPerformType_EXName", "GHPerformEvalTargetTypeTree", "");
            helpdac.CodeToName<ProjectMstModel>(Result.Data, "FPerformEvalType", "FPerformEvalType_EXName", "GHPerformEvalType", "");
            #endregion
            return Result;
        }
        /// <summary>
        /// 分页获取全部集合
        /// </summary>
        /// <param name="pageNumber">页码(从1开始)</param>
        /// <param name="pageSize">每页大小(最大为200)</param>
        /// <param name="dic">查询条件字典（建议使用Enterprise3.Common.Base.CreateCriteria进行创建）</param>
        /// <param name="sorts">排序({属性名 desc,属性名,属性名 asc})</param>
        /// <returns>集合</returns>
        public override PagedResult<ProjectMstModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<ProjectMstModel> pageResult = base.LoadWithPage(pageNumber, pageSize, dic, sorts);


            pageResult = AddNextApproveName(pageResult, "GHProject");

            #region 列表Grid代码转名称
            RichHelpDac helpdac = new RichHelpDac();
            //helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
            //helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist", "");
            helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode", "");
            helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
            helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory", "");
            helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FApprover", "FApprover_EXName", "fg3_user", "");
            helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FPerformType", "FPerformType_EXName", "GHPerformEvalTargetTypeTree", "");
            helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FPerformEvalType", "FPerformEvalType_EXName", "GHPerformEvalType", "");
            //helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FAccount", "FAccount_EXName", "gh_Account", "");

            #endregion

            return pageResult;
        }

        /// <summary>
        /// 分页获取全部集合
        /// </summary>
        /// <param name="pageNumber">页码(从1开始)</param>
        /// <param name="pageSize">每页大小(最大为200)</param>
        /// <param name="nameSqlName">命名SQL名称</param>
        /// <param name="dic">查询条件字典（建议使用Enterprise3.Common.Base.CreateCriteria进行创建）</param>
        /// <param name="sorts">排序({属性名 desc,属性名,属性名 asc})</param>
        /// <returns>集合</returns>
        public PagedResult<ProjectMstModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<ProjectMstModel> pageResult = base.FacadeHelper.LoadWithPage(pageNumber, pageSize, nameSqlName, dic, sorts);

            pageResult = AddNextApproveName(pageResult, "GHProject");

            #region 列表Grid代码转名称
            RichHelpDac helpdac = new RichHelpDac();
            //helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
            //helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist", "");
            helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode", "");
            helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
            helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory", "");
            helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FApprover", "FApprover_EXName", "fg3_user", "");
            helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FPerformType", "FPerformType_EXName", "GHPerformEvalTargetTypeTree", "");
            helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FPerformEvalType", "FPerformEvalType_EXName", "GHPerformEvalType", "");
            //helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FAccount", "FAccount_EXName", "gh_Account", "");

            #endregion

            return pageResult;
        }

        /// <summary>
        /// 通过id，删除数据
        /// </summary>
        /// <param name="id">单主键id值</param>
        public override DeletedResult Delete<TValType>(TValType id)
        {
			ProjectDtlImplPlanRule.RuleHelper.DeleteByForeignKey(id);
			ProjectDtlTextContentRule.RuleHelper.DeleteByForeignKey(id);
			ProjectDtlFundApplRule.RuleHelper.DeleteByForeignKey(id);
			ProjectDtlBudgetDtlRule.RuleHelper.DeleteByForeignKey(id);

            ProjectDtlPurchaseDtlRule.RuleHelper.DeleteByForeignKey(id);
            ProjectDtlPurDtl4SOFRule.RuleHelper.DeleteByForeignKey(id);
            ProjectDtlPerformTargetRule.RuleHelper.DeleteByForeignKey(id);

            return base.Delete(id);
        }

		/// <summary>
        /// 通过ids，删除数据
        /// </summary>
        /// <param name="ids">单主键id集合</param>
        public override DeletedResult Delete<TValType>(IList<TValType> ids)
        {
			ProjectDtlImplPlanRule.RuleHelper.DeleteByForeignKey(ids);
			ProjectDtlTextContentRule.RuleHelper.DeleteByForeignKey(ids);
			ProjectDtlFundApplRule.RuleHelper.DeleteByForeignKey(ids);
			ProjectDtlBudgetDtlRule.RuleHelper.DeleteByForeignKey(ids);

            ProjectDtlPurchaseDtlRule.RuleHelper.DeleteByForeignKey(ids);
            ProjectDtlPurDtl4SOFRule.RuleHelper.DeleteByForeignKey(ids);
            ProjectDtlPerformTargetRule.RuleHelper.DeleteByForeignKey(ids);

            return base.Delete(ids);
        }


        /// <summary>
        /// 更改项目状态,项目状态更改成“单位备选”时,删除当前预算，并把对应项目的状态改为“单位备选”
        /// </summary>
        /// <param name="phid"></param>
        public void UpdateFProjStatus(long phid)
        {
            var model = base.Find(phid);
            model.Data.FProjStatus = 1;
            model.Data.FApproveStatus = "1";  //审批状态改为未上报
            CurrentRule.Update<Int64>(model.Data);

        }

        #endregion

        #region 实现 IProjectMstFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<ProjectMstModel> ExampleMethod<ProjectMstModel>(string param)
        //{
        //    //编写代码
        //}

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="projectMstEntity"></param>
        /// <param name="projectDtlImplPlanEntities"></param>
        /// <param name="projectDtlTextContentEntities"></param>
        /// <param name="projectDtlFundApplEntities"></param>
        /// <param name="projectDtlBudgetDtlEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveProjectMst(ProjectMstModel projectMstEntity, List<ProjectDtlImplPlanModel> projectDtlImplPlanEntities, List<ProjectDtlTextContentModel> projectDtlTextContentEntities, List<ProjectDtlFundApplModel> projectDtlFundApplEntities, List<ProjectDtlBudgetDtlModel> projectDtlBudgetDtlEntities)
        {
            SavedResult<Int64> savedResult = base.Save<Int64>(projectMstEntity);
            if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
            {
				if (projectDtlImplPlanEntities.Count > 0)
				{
					ProjectDtlImplPlanRule.Save(projectDtlImplPlanEntities, savedResult.KeyCodes[0]);
				}
				if (projectDtlTextContentEntities.Count > 0)
				{
					ProjectDtlTextContentRule.Save(projectDtlTextContentEntities, savedResult.KeyCodes[0]);
				}
				if (projectDtlFundApplEntities.Count > 0)
				{
					ProjectDtlFundApplRule.Save(projectDtlFundApplEntities, savedResult.KeyCodes[0]);
				}
				if (projectDtlBudgetDtlEntities.Count > 0)
				{
					ProjectDtlBudgetDtlRule.Save(projectDtlBudgetDtlEntities, savedResult.KeyCodes[0]);
				}
            }

			return savedResult;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="projectMstEntity"></param>
		/// <param name="projectDtlTextContentEntities"></param>
		/// <param name="projectDtlPurchaseDtlEntities"></param>
		/// <param name="projectDtlPurDtl4SOFEntities"></param>
		/// <param name="projectDtlPerformTargetEntities"></param>
		/// <param name="projectDtlFundApplEntities"></param>
		/// <param name="projectDtlBudgetDtlEntities"></param>
		/// <param name="projectDtlImplPlanEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveProjectMst(ProjectMstModel projectMstEntity, List<ProjectDtlTextContentModel> projectDtlTextContentEntities, List<ProjectDtlPurchaseDtlModel> projectDtlPurchaseDtlEntities, List<ProjectDtlPurDtl4SOFModel> projectDtlPurDtl4SOFEntities, List<ProjectDtlPerformTargetModel> projectDtlPerformTargetEntities, List<ProjectDtlFundApplModel> projectDtlFundApplEntities, List<ProjectDtlBudgetDtlModel> projectDtlBudgetDtlEntities, List<ProjectDtlImplPlanModel> projectDtlImplPlanEntities)
        {
            SavedResult<Int64> savedResult = base.Save<Int64>(projectMstEntity);
            if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
            {
                if (projectDtlTextContentEntities != null && projectDtlTextContentEntities.Count > 0)
                {
                    ProjectDtlTextContentRule.Save(projectDtlTextContentEntities, savedResult.KeyCodes[0]);
                }
                if (projectDtlPurchaseDtlEntities!=null && projectDtlPurchaseDtlEntities.Count > 0)
                {
                    ProjectDtlPurchaseDtlRule.Save(projectDtlPurchaseDtlEntities, savedResult.KeyCodes[0]);
                }
                if (projectDtlPurDtl4SOFEntities!=null && projectDtlPurDtl4SOFEntities.Count > 0)
                {
                    ProjectDtlPurDtl4SOFRule.Save(projectDtlPurDtl4SOFEntities, savedResult.KeyCodes[0]);
                }
                if (projectDtlPerformTargetEntities!=null && projectDtlPerformTargetEntities.Count > 0)
                {
                    ProjectDtlPerformTargetRule.Save(projectDtlPerformTargetEntities, savedResult.KeyCodes[0]);
                }
                if (projectDtlFundApplEntities != null && projectDtlFundApplEntities.Count > 0)
                {
                    ProjectDtlFundApplRule.Save(projectDtlFundApplEntities, savedResult.KeyCodes[0]);
                }
                if (projectDtlBudgetDtlEntities != null && projectDtlBudgetDtlEntities.Count > 0)
                {
                    ProjectDtlBudgetDtlRule.Save(projectDtlBudgetDtlEntities, savedResult.KeyCodes[0]);
                }
                if (projectDtlImplPlanEntities != null && projectDtlImplPlanEntities.Count > 0)
                {
                    ProjectDtlImplPlanRule.Save(projectDtlImplPlanEntities, savedResult.KeyCodes[0]);
                }
            }

            return savedResult;
        }
       

        #endregion


        #region 工作流接口
        /// <summary>
        /// 流程发起时调用（一般用于修改表单状态为送审中、或是维护表单已挂工作流）
        /// </summary>
        /// <param name="ec"></param>
        public void FlowStart(WorkFlowExecutionContext ec)
        {
            long phid = Convert.ToInt64(ec.BillInfo.PK1);
            var mst = base.Find(phid);

            //更改状态为：审批中
            mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.IsPending).ToString();
            mst.Data.FDateofDeclaration = DateTime.Now;
            CurrentRule.Update<Int64>(mst.Data);
        }

        /// <summary>
        /// 在审批任务执行前调用，在这里判断是否允许执行审批操作（现在流程中没有判断杜绝多个审批节点执行，所以单据状态为已审批也允许再次审批）
        /// </summary>
        /// <param name="ec"></param>
        /// <returns></returns>
        public ApproveValidResult CheckApproveValid(WorkFlowExecutionContext ec)
        {
            return NG3.WorkFlow.Interfaces.ApproveValidResult.Create(ApproveValidType.Yes, string.Empty);
        }

        /// <summary>
        ///  审批组件任务办理时调用（现在流程中没有判断杜绝多个审批节点执行，所以如果单据已审批就修改审批人、审批时间）
        /// </summary>
        /// <param name="ec"></param>
        public void Approve(WorkFlowExecutionContext ec)
        {

            long phid = Convert.ToInt64(ec.BillInfo.PK1);
            var mst = base.Find(phid);
            //更新状态为已审批
            if (mst.Data.FApproveStatus != Convert.ToInt32(EnumApproveStatus.Approved).ToString())
            {
                //20190107 审批完成时改为已审批，流程结束时才改项目状态
                //审批完成时,项目状态为预立项,则项目状态改为立项,审批状态改为未审批
                //if (mst.Data.FProjStatus == 1)
                //{
                //    mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.ToBeRepored).ToString();
                //    mst.Data.FProjStatus = 2;
                //}
                //else
                //{
                    mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.Approved).ToString();
                //}
                
            }
            

            ////用 FlowEnd(), 在流程结束时进行操作(approve 方法 在进行审批节点后就会调用,可能存在多个审批节点)
            //long phid = Convert.ToInt64(ec.BillInfo.PK1);
            //var mst = base.Find(phid);
            ////更新状态为已审批
            //if (mst.Data.FApproveStatus != Convert.ToInt32(EnumApproveStatus.Approved).ToString())
            //{
            //    //审批完成时,项目状态为预立项,则项目状态改为立项,审批状态改为未审批
            //    if (mst.Data.FProjStatus == 1)
            //    {
            //        mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.ToBeRepored).ToString();
            //        mst.Data.FProjStatus = 2;
            //    }
            //    else
            //    {
            //        mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.Approved).ToString();
            //    }

            //    mst.Data.FApproveDate = DateTime.Now;
            //    mst.Data.FApprover = base.UserID;
            //    CurrentRule.Update<Int64>(mst.Data);
            //}

            ProjectMstModel projectMst = base.Find(phid).Data;
            //Dictionary<string, object> conndic = new Dictionary<string, object>();
            //new CreateCriteria(conndic)
            //    .Add(ORMRestrictions<string>.Eq("Dylx", "config"))
            //    .Add(ORMRestrictions<string>.Eq("DefStr1", projectMst.FDeclarationUnit));
            //IList<CorrespondenceSettings2Model> CorrespondenceSettings2s = CorrespondenceSettings2Rule.RuleHelper.Find(conndic);

            //连接串更改为从基础数据-账套中取  QtAccountRule
            if (!string.IsNullOrEmpty(projectMst.FAccount))
            {
                Dictionary<string, object> conndic = new Dictionary<string, object>();
                new CreateCriteria(conndic).Add(ORMRestrictions<string>.Eq("Dm", projectMst.FAccount));
                var Accounts = QtAccountRule.Find(conndic);
                if (Accounts.Count > 0 &&!string.IsNullOrEmpty(Accounts[0].FConn))
                {
                    if (projectMst.FSaveToOldG6h == 1)
                    {
                        try
                        {
                            List<string> valuesqlList = new List<string>();
                            List<DateTime?> DJRQList = new List<DateTime?>();
                            List<DateTime?> DT1List = new List<DateTime?>();
                            List<DateTime?> DT2List = new List<DateTime?>();
                            List<string> dtlSqlList = new List<string>();
                            List<string> dtlcodeList = new List<string>();

                            string userConn = Accounts[0].FConn;
                            string zbly_dm = Accounts[0].Dm;
                            int ID = BudgetMstRule.GetId(userConn);
                            string ZY;
                            string DWDM;
                            string DEF_STR7;
                            //单位转换
                            IList<QtOrgDygxModel> OrgDygx = QtOrgDygxRule.findByXmorg(projectMst.FDeclarationUnit);
                            if (OrgDygx.Count > 0)
                            {
                                DWDM = OrgDygx[0].Oldorg;
                            }
                            else
                            {
                                DWDM = projectMst.FDeclarationUnit;
                            }
                            //部门转换
                            IList<QtOrgDygxModel> OrgDygx2 = QtOrgDygxRule.findByXmorg(projectMst.FBudgetDept);
                            if (OrgDygx2.Count > 0)
                            {
                                ZY = OrgDygx2[0].Oldorg;
                                DEF_STR7 = OrgDygx2[0].Oldorg;
                            }
                            else
                            {
                                ZY = projectMst.FBudgetDept;
                                DEF_STR7 = projectMst.FBudgetDept;
                            }
                            string DJH;
                            DJH = ZY + "c" + projectMst.FProjCode + "0001";
                            IList<ProjectDtlBudgetDtlModel> ProjectDtlBudgetDtlList = ProjectDtlBudgetDtlRule.FindByForeignKey(projectMst.PhId);
                            if (ProjectDtlBudgetDtlList.Count > 0)
                            {
                                for (var j = 0; j < ProjectDtlBudgetDtlList.Count; j++)
                                {
                                    ID += 1;

                                    DateTime? DJRQ = projectMst.FDateofDeclaration;
                                    DateTime? DT1 = projectMst.FStartDate;
                                    DateTime? DT2 = projectMst.FEndDate;
                                    string YSKM_DM = ProjectDtlBudgetDtlList[j].FBudgetAccounts;
                                    string JFQD_DM = ProjectDtlBudgetDtlList[j].FSourceOfFunds;
                                    decimal PAY_JE = ProjectDtlBudgetDtlList[j].FAmount;
                                    string DEF_STR1 = projectMst.FProjCode;
                                    string DEF_STR4 = projectMst.FExpenseCategory;
                                    int DEF_INT1 = int.Parse(projectMst.FProjAttr);
                                    int DEF_INT2 = int.Parse(projectMst.FDuration);
                                    string MXXM = ProjectDtlBudgetDtlList[j].FDtlCode;
                                    string DEF_STR8 = "";
                                    //支出渠道转换
                                    if (ProjectDtlBudgetDtlList[j].FExpensesChannel != null && ProjectDtlBudgetDtlList[j].FExpensesChannel != "")
                                    {
                                        IList<QtOrgDygxModel> OrgZCQD = QtOrgDygxRule.findByXmorg(ProjectDtlBudgetDtlList[j].FExpensesChannel);
                                        if (OrgZCQD.Count > 0)
                                        {
                                            DEF_STR8 = OrgZCQD[0].Oldorg;
                                        }
                                        else
                                        {
                                            DEF_STR8 = ProjectDtlBudgetDtlList[j].FExpensesChannel;
                                        }
                                    }
                                    int year = int.Parse(projectMst.FYear);
                                    int xmzt = 3;
                                    int int1;
                                    if (projectMst.FIfPerformanceAppraisal == EnumYesNo.Yes)
                                    {
                                        int1 = 1;
                                    }
                                    else
                                    {
                                        int1 = 2;
                                    }
                                    int int2;
                                    if (projectMst.FIfKeyEvaluation == EnumYesNo.Yes)
                                    {
                                        int2 = 1;
                                    }
                                    else
                                    {
                                        int2 = 2;
                                    }
                                    string ACCNO1;
                                    Dictionary<string, object> dicYSKM = new Dictionary<string, object>();
                                    new CreateCriteria(dicYSKM)
                                        .Add(ORMRestrictions<String>.Eq("KMDM", ProjectDtlBudgetDtlList[j].FBudgetAccounts));
                                    IList<BudgetAccountsModel> budgetAccounts = BudgetAccountsRule.Find(dicYSKM);
                                    if (budgetAccounts.Count > 0 && budgetAccounts[0].HB == "1")
                                    {
                                        ACCNO1 = "1";
                                    }
                                    else
                                    {
                                        ACCNO1 = "0";
                                    }
                                    string valuesql = "(" + ID + ",'" + DJH + "','" + DWDM + "','" + YSKM_DM + "','" + JFQD_DM + "'," + PAY_JE + ",'" + ZY + "','"
                                            + DEF_STR1 + "','" + DEF_STR4 + "'," + DEF_INT1 + "," + DEF_INT2 + ",'" + MXXM + "','" + DEF_STR7 + "','" + DEF_STR8 + "',"
                                            + year + "," + xmzt + "," + int1 + "," + int2 + ",'" + ACCNO1 + "'";

                                    string dtlSql = "('jj','" + projectMst.FProjCode + "','" + ProjectDtlBudgetDtlList[j].FDtlCode + "','" + ProjectDtlBudgetDtlList[j].FName + "','xm')";
                                    valuesqlList.Add(valuesql);
                                    DJRQList.Add(DJRQ);
                                    DT1List.Add(DT1);
                                    DT2List.Add(DT2);

                                    if (!dtlcodeList.Contains(ProjectDtlBudgetDtlList[j].FDtlCode))
                                    {
                                        dtlSqlList.Add(dtlSql);
                                        dtlcodeList.Add(ProjectDtlBudgetDtlList[j].FDtlCode);
                                    }
                                }
                            }
                            string mstSql = "('" + projectMst.FProjCode + "','" + projectMst.FProjName + "','xm')";

                            ProjectMstRule.ApproveAddData(userConn, zbly_dm, valuesqlList, mstSql, dtlSqlList, DJRQList, "xm", projectMst.FProjCode, dtlcodeList, DJH,DT1List,DT2List);
                        }
                        catch
                        {

                        }
                    }
                }

            }
            else
            {
                mst.Data.FSaveToOldG6h = 0;
            }
            mst.Data.FApproveDate = DateTime.Now;
            mst.Data.FApprover = base.UserID;
            CurrentRule.Update<Int64>(mst.Data);
        }

        /// <summary>
        /// 审批节点回退前判断是否允许取消审批
        /// </summary>
        /// <param name="ec"></param>
        /// <returns></returns>
        public ApproveValidResult CheckCancelApproveValid(WorkFlowExecutionContext ec)
        {
            return NG3.WorkFlow.Interfaces.ApproveValidResult.Create(ApproveValidType.Yes, string.Empty);
        }

        /// <summary>
        /// 审批节点回退时调用进行单据取消审批操作
        /// </summary>
        /// <param name="ec"></param>
        public void CancelApprove(WorkFlowExecutionContext ec)
        {
            long phid = Convert.ToInt64(ec.BillInfo.PK1);
            var mst = base.Find(phid);
            //更新状态为审批中
            mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.IsPending).ToString();
            mst.Data.FDateofDeclaration = new Nullable<DateTime>();
            CurrentRule.Update<Int64>(mst.Data);

            //throw new NotImplementedException();
        }

        /// <summary>
        /// 流程被终止时调用
        /// </summary>
        /// <param name="ec"></param>
        public void FlowAbort(WorkFlowExecutionContext ec)
        {
            long phid = Convert.ToInt64(ec.BillInfo.PK1);
            var mst = base.Find(phid);
            //更新状态为待上报
            mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.ToBeRepored).ToString();
            //mst.Data.FProjStatus = 1;//项目立项审批驳回-->从预立项从新修改，发起；状态默认预立项；
            mst.Data.FDateofDeclaration = new Nullable<DateTime>();
            CurrentRule.Update<Int64>(mst.Data);
        }

        /// <summary>
        /// 流程结束时调用
        /// </summary>
        /// <param name="ec"></param>
        public void FlowEnd(WorkFlowExecutionContext ec)
        {

            long phid = Convert.ToInt64(ec.BillInfo.PK1);
            var mst = base.Find(phid);
            //更新状态为已审批
            if (mst.Data.FApproveStatus != Convert.ToInt32(EnumApproveStatus.Approved).ToString())
            {
                
               
                mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.Approved).ToString();
                if (mst.Data.FProjStatus == 1)
                {
                    //审批完成时,项目状态为预立项,则项目状态改为立项,审批状态改为未审批
                    mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.ToBeRepored).ToString();
                    mst.Data.FProjStatus = 2;
                }


            }
            else if(mst.Data.FProjStatus == 1 && mst.Data.FApproveStatus == Convert.ToInt32(EnumApproveStatus.Approved).ToString())
            {
                //审批完成时,项目状态为预立项,则项目状态改为立项,审批状态改为未审批
                mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.ToBeRepored).ToString();
                mst.Data.FProjStatus = 2;
            }

            mst.Data.FApproveDate = DateTime.Now;
            mst.Data.FApprover = base.UserID;
            CurrentRule.Update<Int64>(mst.Data);

            //浙江因流程结束后需要回测操作，故更改审批标志放在审批节点走后
            //long phid = Convert.ToInt64(ec.BillInfo.PK1);
            //var mst = base.Find(phid);
            ////更新状态为已审批
            //if (mst.Data.FApproveStatus != Convert.ToInt32(EnumApproveStatus.Approved).ToString())
            //{
            //    //审批完成时,项目状态为预立项,则项目状态改为立项,审批状态改为未审批
            //    if (mst.Data.FProjStatus == 1)
            //    {
            //        mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.ToBeRepored).ToString();
            //        mst.Data.FProjStatus = 2;
            //    }
            //    else
            //    {
            //        mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.Approved).ToString();
            //    }

            //    mst.Data.FApproveDate = DateTime.Now;
            //    mst.Data.FApprover = base.UserID;
            //    CurrentRule.Update<Int64>(mst.Data);
            //}
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 新增、编辑\审核类组件任务执行时调用,方法参数中包含组件id
        /// </summary>
        /// <param name="compId"></param>
        /// <param name="ec"></param>
        public void EditUserTaskComplete(string compId, WorkFlowExecutionContext ec)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 获取单据转pdf所需的套打模块、及数据，用于APP端
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public BizToPdfEntity GetBizToPdfEntity(WorkFlowExecutionContext executionContext)
        {
           
            return null;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 获取单据附用（用于App端）
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public List<BizAttachment> GetBizAttachment(WorkFlowExecutionContext executionContext)
        {
            return new List<BizAttachment>();
        }

        /// <summary>
        /// app办理时如果单据有修改则调用该方法判断是否允许保存修改
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public ApproveValidResult CheckBizSaveByMobileApp(WorkFlowExecutionContext executionContext, Dictionary<string, string> jsonData)
        {
            return ApproveValidResult.DefaultValue;
        }

        /// <summary>
        /// app端办理时如果修改了单据内容则调用该方法进行单据保存。
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public bool SaveBizDataByMobileApp(WorkFlowExecutionContext executionContext, Dictionary<string, string> jsonData)
        {
            return true;
            //throw new NotImplementedException();
        }


       

        /// <summary>
        /// 项目同步数据到老G6H
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string AddData(string[] ids)
        {
            string result = "";
            List<ProjectMstModel> projectMstList = base.FacadeHelper.Find(ids).Data.ToList();
            
            
            if (projectMstList.Count > 0)
            {
                string userConn;
                string zbly_dm;
                List<string> AccountList = projectMstList.Where(x => !string.IsNullOrEmpty(x.FAccount)).Select(x => x.FAccount).Distinct().ToList();
                foreach (var Account in AccountList)
                {
                    //连接串更改为从基础数据-账套中取  QtAccountRule
                    Dictionary<string, object> conndic = new Dictionary<string, object>();
                    new CreateCriteria(conndic).Add(ORMRestrictions<string>.Eq("Dm", Account));
                    var Accounts = QtAccountRule.Find(conndic);
                    if (Accounts.Count > 0 && !string.IsNullOrEmpty(Accounts[0].FConn))
                    {
                        userConn = Accounts[0].FConn;
                        zbly_dm = Accounts[0].Dm;
                        List<string> valuesqlList = new List<string>();
                        List<DateTime?> DJRQList = new List<DateTime?>();
                        List<DateTime?> DT1List = new List<DateTime?>();
                        List<DateTime?> DT2List = new List<DateTime?>();
                        List<string> mstSqlList = new List<string>();
                        List<string> dtlSqlList = new List<string>();
                        int ID = BudgetMstRule.GetId(userConn);
                        var projectMstList2 = projectMstList.FindAll(x => x.FAccount == Account);
                        for (var i = 0; i < projectMstList2.Count; i++)
                        {
                            if (projectMstList2[i].FSaveToOldG6h == 0)
                            {


                                //projectMstList[i].FSaveToOldG6h = 1;
                                string ZY;
                                string DWDM;
                                string DEF_STR7;
                                //单位转换
                                IList<QtOrgDygxModel> OrgDygx = QtOrgDygxRule.findByXmorg(projectMstList2[i].FDeclarationUnit);
                                if (OrgDygx.Count > 0)
                                {
                                    DWDM = OrgDygx[0].Oldorg;
                                }
                                else
                                {
                                    DWDM = projectMstList2[i].FDeclarationUnit;
                                }
                                //部门转换
                                IList<QtOrgDygxModel> OrgDygx2 = QtOrgDygxRule.findByXmorg(projectMstList2[i].FBudgetDept);
                                if (OrgDygx2.Count > 0)
                                {
                                    ZY = OrgDygx2[0].Oldorg;
                                    DEF_STR7 = OrgDygx2[0].Oldorg;
                                }
                                else
                                {
                                    ZY = projectMstList2[i].FBudgetDept;
                                    DEF_STR7 = projectMstList2[i].FBudgetDept;
                                }

                                IList<ProjectDtlBudgetDtlModel> ProjectDtlBudgetDtlList = ProjectDtlBudgetDtlRule.FindByForeignKey(projectMstList2[i].PhId);
                                if (ProjectDtlBudgetDtlList.Count > 0)
                                {
                                    for (var j = 0; j < ProjectDtlBudgetDtlList.Count; j++)
                                    {

                                        ID += 1;
                                        //IDs[userConns.IndexOf(userConn)] += 1;
                                        string DJH;
                                        DJH = ZY + "c" + projectMstList2[i].FProjCode + "0001";
                                        DateTime? DJRQ = projectMstList2[i].FDateofDeclaration;
                                        DateTime? DT1 = projectMstList2[i].FStartDate;
                                        DateTime? DT2 = projectMstList2[i].FEndDate;
                                        string YSKM_DM = ProjectDtlBudgetDtlList[j].FBudgetAccounts;
                                        string JFQD_DM = ProjectDtlBudgetDtlList[j].FSourceOfFunds;
                                        decimal PAY_JE = ProjectDtlBudgetDtlList[j].FAmount;
                                        string DEF_STR1 = projectMstList2[i].FProjCode;
                                        string DEF_STR4 = projectMstList2[i].FExpenseCategory;
                                        int DEF_INT1 = int.Parse(projectMstList2[i].FProjAttr);
                                        int DEF_INT2 = int.Parse(projectMstList2[i].FDuration);
                                        string MXXM = ProjectDtlBudgetDtlList[j].FDtlCode;
                                        string DEF_STR8 = "";
                                        //支出渠道转换
                                        if (ProjectDtlBudgetDtlList[j].FExpensesChannel != null && ProjectDtlBudgetDtlList[j].FExpensesChannel != "")
                                        {
                                            IList<QtOrgDygxModel> OrgZCQD = QtOrgDygxRule.findByXmorg(ProjectDtlBudgetDtlList[j].FExpensesChannel);
                                            if (OrgZCQD.Count > 0)
                                            {
                                                DEF_STR8 = OrgZCQD[0].Oldorg;
                                            }
                                            else
                                            {
                                                DEF_STR8 = ProjectDtlBudgetDtlList[j].FExpensesChannel;
                                            }
                                        }
                                        int year = int.Parse(projectMstList2[i].FYear);
                                        int xmzt = 3;
                                        int int1;
                                        if (projectMstList2[i].FIfPerformanceAppraisal == EnumYesNo.Yes)
                                        {
                                            int1 = 1;
                                        }
                                        else
                                        {
                                            int1 = 2;
                                        }
                                        int int2;
                                        if (projectMstList2[i].FIfKeyEvaluation == EnumYesNo.Yes)
                                        {
                                            int2 = 1;
                                        }
                                        else
                                        {
                                            int2 = 2;
                                        }
                                        //允许预备费抵扣
                                        string ACCNO1;
                                        Dictionary<string, object> dicYSKM = new Dictionary<string, object>();
                                        new CreateCriteria(dicYSKM)
                                            .Add(ORMRestrictions<String>.Eq("KMDM", ProjectDtlBudgetDtlList[j].FBudgetAccounts));
                                        IList<BudgetAccountsModel> budgetAccounts = BudgetAccountsRule.Find(dicYSKM);
                                        if (budgetAccounts.Count > 0 && budgetAccounts[0].HB == "1")
                                        {
                                            ACCNO1 = "1";
                                        }
                                        else
                                        {
                                            ACCNO1 = "0";
                                        }

                                        string valuesql = "(" + ID + ",'" + DJH + "','" + DWDM + "','" + YSKM_DM + "','" + JFQD_DM + "'," + PAY_JE + ",'" + ZY + "','"
                                                + DEF_STR1 + "','" + DEF_STR4 + "'," + DEF_INT1 + "," + DEF_INT2 + ",'" + MXXM + "','" + DEF_STR7 + "','" + DEF_STR8 + "',"
                                                + year + "," + xmzt + "," + int1 + "," + int2 + ",'" + ACCNO1 + "'";

                                        string dtlSql = "('jj','" + projectMstList2[i].FProjCode + "','" + ProjectDtlBudgetDtlList[j].FDtlCode + "','" + ProjectDtlBudgetDtlList[j].FName + "','xm')";
                                        valuesqlList.Add(valuesql);
                                        DJRQList.Add(DJRQ);
                                        DT1List.Add(DT1);
                                        DT2List.Add(DT2);

                                        if (!dtlSqlList.Contains(dtlSql))
                                        {
                                            dtlSqlList.Add(dtlSql);
                                        }
                                    }
                                }
                                string mstSql = "('" + projectMstList2[i].FProjCode + "','" + projectMstList2[i].FProjName + "','xm')";
                                if (!mstSqlList.Contains(mstSql))
                                {
                                    mstSqlList.Add(mstSql);
                                }
                                projectMstList2[i].FSaveToOldG6h = 1;
                                projectMstList2[i].PersistentState = PersistentState.Modified;
                            }
                        }
                        try
                        {
                            BudgetMstRule.AddData(userConn, zbly_dm, valuesqlList, mstSqlList, dtlSqlList, DJRQList, "xm",DT1List,DT2List);
                            
                        }
                        catch (Exception e)
                        {
                            result = result + Account + ",";
                        }
                    }
                }
                /*if (result == 1)
                {
                    foreach (ProjectMstModel projectMst in projectMstList)
                    {
                        projectMst.FSaveToOldG6h = 1;
                        projectMst.PersistentState = PersistentState.Modified;
                    }
                    base.Save<Int64>(projectMstList);
                }*/
                if (result != "")
                {
                    result = result.Substring(0, result.Length - 1);
                    result = result + "同步失败";
                }
                else
                {
                    base.Save<Int64>(projectMstList);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取审批中的单据id
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public List<string> GetApproveList(PagedResult<ProjectMstModel> pageResult)
        {
            var idList = new List<string>();
            foreach(var item in pageResult.Results)
            {
                if(item.FApproveStatus == "2")
                {
                    idList.Add(item.PhId.ToString());
                }
            }
            return idList;
        }

        /// <summary>
        /// 增加单据中下一审批节点名称
        /// </summary>
        /// <param name="pageResult"></param>
        /// <param name="BizType"> 审批流业务类型</param>
        /// <returns></returns>
        public PagedResult<ProjectMstModel> AddNextApproveName(PagedResult<ProjectMstModel> pageResult,string BizType)
        {
            var approveListId = GetApproveList(pageResult);
            if(approveListId.Count == 0)
            {
                foreach (var item in pageResult.Results)
                {
                    item.FNextApprove = "无";
                }
                return pageResult;
            }
            var Next_approve = NG3.WorkFlow.Rule.WorkFlowHelper.GetFlowInfoByBizList(BizType, approveListId);
           
            foreach (var item in pageResult.Results)
            {
                if (item.FApproveStatus == "2")
                {
                   for(var i = 0; i < Next_approve.Count; i++)
                    {
                        if(Next_approve[i]["pk1"].ToString() == item.PhId.ToString() && Next_approve[i]["flow_status_name"].ToString() == "运行中")
                        {
                            item.FNextApprove = Next_approve[i]["curnodes"].ToString();
                            break;
                        }
                    }
                }
                else
                {
                    item.FNextApprove = "无";
                }
            }

            return pageResult;
        }

        #endregion

        #region //新的工作流，类似审批流

        /// <summary>
        /// 修改项目审批状态
        /// </summary>
        /// <param name="recordModel">审批对象</param>
        /// <param name="fApproval">审批字段</param>
        /// <returns></returns>
        public SavedResult<long> UpdateProject(GAppvalRecordModel recordModel, string fApproval)
        {
            if (recordModel.RefbillPhid == 0)
                return null;

            ProjectMstModel projectMst = this.ProjectMstRule.Find(recordModel.RefbillPhid);
            if(projectMst.FProjStatus == (int)EnumProjStatus.Alternative && fApproval == ((int)EnumApproveStatus.Approved).ToString())
            {
                projectMst.FProjStatus = (int)EnumProjStatus.InBudget;
                projectMst.FApproveStatus = ((int)EnumApproveStatus.ToBeRepored).ToString();
            }
            else
            {
                projectMst.FApproveStatus = fApproval;
            }
            
            projectMst.FApproveDate = DateTime.Now;
            projectMst.FApprover = recordModel.OperaPhid;
            projectMst.FApprover_EXName = recordModel.OperaName;
            projectMst.PersistentState = PersistentState.Modified;

            return ProjectMstRule.Save<Int64>(projectMst);
        }

        /// <summary>
        /// 跨审批流退回时修改项目状态以及审批状态
        /// </summary>
        /// <param name="recordModel">审批对象</param>
        /// <param name="fApproval">审批字段</param>
        /// <returns></returns>
        public SavedResult<long> UpdateProject2(GAppvalRecordModel recordModel, string fApproval)
        {
            if (recordModel.RefbillPhid == 0)
                return null;

            ProjectMstModel projectMst = this.ProjectMstRule.Find(recordModel.RefbillPhid);
            if (projectMst.FProjStatus == (int)EnumProjStatus.InBudget)
            {
                projectMst.FProjStatus = (int)EnumProjStatus.Alternative;
                projectMst.FApproveStatus = fApproval;
            }
            else
            {
                throw new Exception("只有项目立项才能跨审批流退回！");
            }

            projectMst.FApproveDate = DateTime.Now;
            projectMst.FApprover = recordModel.OperaPhid;
            projectMst.FApprover_EXName = recordModel.OperaName;
            projectMst.PersistentState = PersistentState.Modified;

            return ProjectMstRule.Save<Int64>(projectMst);
        }
        #endregion
    }
}

