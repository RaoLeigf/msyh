#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectMstService
    * 命名空间：        GXM3.XM.Service
    * 文 件 名：        ProjectMstService.cs
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
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.Common.Base.Criterion;
using Enterprise3.NHORM.Service;

using SUP.Common.Base;

using GXM3.XM.Service.Interface;
using GXM3.XM.Facade.Interface;
using GXM3.XM.Model.Domain;
using GYS3.YS.Facade.Interface;
using GXM3.XM.Model;
using GQT3.QT.Facade.Interface;
using GQT3.QT.Service.Interface;
using GQT3.QT.Model;
using GQT3.QT.Model.Domain;
using GYS3.YS.Model.Enums;
using System.Reflection;
using System.Net;
using NG3;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using Newtonsoft.Json;
using System.IO;
using GData3.Common.Utils;
using GYS3.YS.Model.Domain;
using SUP.Common.DataAccess;
using GXM3.XM.Model.Extra;
using GYS3.YS.Service.Interface;

namespace GXM3.XM.Service
{
	/// <summary>
	/// ProjectMst服务组装处理类
	/// </summary>
    public partial class ProjectMstService : EntServiceBase<ProjectMstModel>, IProjectMstService
    {
		#region 类变量及属性
		/// <summary>
        /// ProjectMst业务外观处理对象
        /// </summary>
		IProjectMstFacade ProjectMstFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IProjectMstFacade;
            }
        }
        
        IQtSysCodeSeqService SysCodeSeqService { get; set; }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
        private IProjectDtlImplPlanFacade ProjectDtlImplPlanFacade { get; set; }

		/// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IProjectDtlTextContentFacade ProjectDtlTextContentFacade { get; set; }

		/// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IProjectDtlFundApplFacade ProjectDtlFundApplFacade { get; set; }

		/// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IProjectDtlBudgetDtlFacade ProjectDtlBudgetDtlFacade { get; set; }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
        private IProjectDtlPersonnelFacade ProjectDtlPersonnelFacade { get; set; }

        // private IProjectMstApproveFacade ProjectMstApproveFacade { get; set; }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
        private IProjectDtlPersonNameFacade ProjectDtlPersonNameFacade { get; set; }


        /// <summary>
        /// 预算
        /// </summary>
        private IBudgetMstFacade BudgetMstFacade { get; set; }

        private IQtSysCodeSeqFacade QtSysCodeSeqFacade { get; set; }

        private IProjLibProjFacade ProjLibProjFacade { get; set; }

        private ICorrespondenceSettingsFacade CorrespondenceSettingsFacade { get; set; }

        private ICorrespondenceSettings2Facade CorrespondenceSettings2Facade { get; set; }

        private IQTControlSetFacade QTControlSetFacade { get; set; }

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

        private IQTModifyFacade QTModifyFacade { get; set; }

        private IQTIndividualInfoFacade QTIndividualInfoFacade { get; set; }

        private IQTSysSetFacade QTSysSetFacade { get; set; }

        private IUserOrgFacade UserOrgFacade { get; set; }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
        private IProjectDtlPerformTargetFacade ProjectDtlPerformTargetFacade { get; set; }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
        private IProjectDtlPurchaseDtlFacade ProjectDtlPurchaseDtlFacade { get; set; }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
        private IProjectDtlPurDtl4SOFFacade ProjectDtlPurDtl4SOFFacade { get; set; }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
        private IQtAttachmentFacade QtAttachmentFacade { get; set; }

        private IBudgetDtlTextContentFacade BudgetDtlTextContentFacade { get; set; }

        private IBudgetDtlBudgetDtlFacade BudgetDtlBudgetDtlFacade { get; set; }

        private IBudgetDtlFundApplFacade BudgetDtlFundApplFacade { get; set; }

        private IBudgetDtlImplPlanFacade BudgetDtlImplPlanFacade { get; set; }

        #endregion

        #region 实现 IProjectMstService 业务添加的成员

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
            return ProjectMstFacade.SaveProjectMst(projectMstEntity, projectDtlImplPlanEntities, projectDtlTextContentEntities, projectDtlFundApplEntities, projectDtlBudgetDtlEntities);
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
            return ProjectMstFacade.SaveProjectMst(projectMstEntity, projectDtlTextContentEntities, projectDtlPurchaseDtlEntities, projectDtlPurDtl4SOFEntities, projectDtlPerformTargetEntities, projectDtlFundApplEntities, projectDtlBudgetDtlEntities, projectDtlImplPlanEntities);
        }
        

        /// <summary>
        /// 通过外键值获取ProjectDtlImplPlan明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<ProjectDtlImplPlanModel> FindProjectDtlImplPlanByForeignKey<TValType>(TValType id)
        {
            return ProjectDtlImplPlanFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 通过外键值获取ProjectDtlTextContent明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<ProjectDtlTextContentModel> FindProjectDtlTextContentByForeignKey<TValType>(TValType id)
        {
            return ProjectDtlTextContentFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 通过外键值获取ProjectDtlPersonnel明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<ProjectDtlPersonnelModel> FindProjectDtlPersonnelByForeignKey<TValType>(TValType id)
        {
            return ProjectDtlPersonnelFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 通过外键值获取ProjectDtlPersonName明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<ProjectDtlPersonNameModel> FindProjectDtlPersonNameByForeignKey<TValType>(TValType id)
        {
            return ProjectDtlPersonNameFacade.FindByForeignKey(id);
        }


        /// <summary>
        /// 通过外键值获取ProjectDtlFundAppl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<ProjectDtlFundApplModel> FindProjectDtlFundApplByForeignKey<TValType>(TValType id)
        {
            return ProjectDtlFundApplFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 通过外键值获取ProjectDtlBudgetDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<ProjectDtlBudgetDtlModel> FindProjectDtlBudgetDtlByForeignKey<TValType>(TValType id)
        {
            return ProjectDtlBudgetDtlFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 返回对Phid的明细记录
        /// </summary>
        /// <typeparam name="Int64"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public FindedResult<ProjectDtlBudgetDtlModel> FindProjectDtlBudgetDtlByPhID<Int64>(Int64 id)
        {
            return ProjectDtlBudgetDtlFacade.Find(id);
        }

        /// <summary>
        /// 获取首页页面的 条数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="ng3_logid">用户主键</param>
        /// <param name="usercode">账号</param>
        /// <returns></returns>
        public ProjectCountModel GetDataCount(int pageIndex, int pageSize, long ng3_logid,string usercode, string sessionFYear)
        {


            Dictionary<string, object> dicWhere1 = new Dictionary<string, object>();
            Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
            Dictionary<string, object> dicWhere3 = new Dictionary<string, object>();

            //获取操作员对应预算部门
            var userDeptList = CorrespondenceSettingsFacade.GetUserDepementList(usercode);
            var arrydep = new List<string>();

            foreach (var item in userDeptList)
            {
                arrydep.Add(item.Dydm);
            }

            new CreateCriteria(dicWhere1).Add(ORMRestrictions<string>.Eq("FApproveStatus", "1"))
                .Add(ORMRestrictions<IList<string>>.In("FBudgetDept", arrydep))
                .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0));//操作员对应预算部门

            new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FApproveStatus", "2"))
                .Add(ORMRestrictions<IList<string>>.In("FBudgetDept", arrydep))
                .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0));//操作员对应预算部门

            new CreateCriteria(dicWhere3).Add(ORMRestrictions<string>.Eq("FApproveStatus", "1"))
                .Add(ORMRestrictions<Int32>.Eq("FProjStatus", 1))
                .Add(ORMRestrictions<IList<string>>.In("FBudgetDept", arrydep))
                .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0));//操作员对应预算部门

            if (sessionFYear != "")
            {
                new CreateCriteria(dicWhere1).Add(ORMRestrictions<string>.Eq("FYear", sessionFYear));
                new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FYear", sessionFYear));
                new CreateCriteria(dicWhere3).Add(ORMRestrictions<string>.Eq("FYear", sessionFYear));
            }

            var resultProject = ProjectMstFacade.LoadWithPage(0, 1, dicWhere3);
            var resultBudget = BudgetMstFacade.LoadWithPage(0, 1, dicWhere1);

            var resultProject2 = ProjectMstFacade.LoadWithPage(0, 1, dicWhere2);
            var resultBudget2 = BudgetMstFacade.LoadWithPage(0, 1, dicWhere2);


            ProjectCountModel model = new ProjectCountModel
            {
                ProjectCount = resultProject.TotalItems,
                BudgetCount = resultBudget.TotalItems,
                ProjectApproval= resultProject2.TotalItems,
                BudgetApproval= resultBudget2.TotalItems
            };

            return model;
        }

        /// <summary>
        /// 通过代码获取ProjectMstModel
        /// </summary>
        /// <param name="dm">代码</param>
        /// <returns></returns>
        public FindedResults<ProjectMstModel> FindProjectMst(string dm) {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("FExpenseCategory",dm));
            return base.Find(dicWhere);
        }
        public FindedResults<ProjectMstModel> FindProjectMstByProperty<T>(T values,String propertyName) {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<T>.In(propertyName, values));
            return this.Find(dicWhere);
        }


        public FindedResults<ProjectDtlPurchaseDtlModel> FindProjectDtlPurchaseDtlByAnyCode<TValType>(TValType values, string Pname) {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<TValType>.In(Pname, values));
            return ProjectDtlPurchaseDtlFacade.Find(dicWhere);
        }
        /// <summary>
        /// ProjectDtlBudgetDtlModel
        /// </summary>
        /// <param name="dm">代码</param>
        /// <returns></returns>
        public FindedResults<ProjectDtlBudgetDtlModel> FindPaymentMethod(string dm) {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FPaymentMethod", dm));
            FindedResults<ProjectDtlBudgetDtlModel> results = ProjectDtlBudgetDtlFacade.Find(dicWhere);
            if (results != null && results.Data.Count > 0) {
                results.Status = ResponseStatus.Error;
                results.Msg = "该支付方式已被引用，无法删除！";
            }
            return results;
        }

        /// <summary>
        /// 通过资金来源代码获取ProjectDtlBudgetDtl明细数据
        /// </summary>
        /// <param name="ZJDM">资金来源代码</param>
        /// <returns></returns>
        public FindedResults<ProjectDtlBudgetDtlModel> FindProjectDtlBudgetDtlMstByZJDM(string ZJDM)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("FSourceOfFunds", ZJDM));
            return ProjectDtlBudgetDtlFacade.Find(dicWhere);
        }


        /// <summary>
        /// 更改项目状态,项目状态更改成“单位备选”时,删除当前预算，并把对应项目的状态改为“单位备选”
        /// </summary>
        /// <param name="phid"></param>
        public void UpdateFProjStatus(long phid)
        {
            ProjectMstFacade.UpdateFProjStatus(phid);

        }
        /// <summary>
        /// 预算根据明细表主键回填预算金额
        /// </summary>
        /// <param name="phid"></param>
        /// <param name="FAmount"></param>
        public void UpdateDtlFBudgetAmount(long[] phid, decimal[] FAmount)
        {
            ProjectDtlBudgetDtlFacade.UpdateDtlFBudgetAmount(phid, FAmount);
            long id = 0;
            decimal budgetAmount = 0 ;
            for (var i = 0; i < phid.Length; i++)
            {
                if (phid[i] == 0)
                {
                    continue;
                }
                id = phid[i];
                budgetAmount += FAmount[i];
            }
            if(id != 0)
            {
                var dtl = ProjectDtlBudgetDtlFacade.Find(id);
                var mstPhid = dtl.Data.MstPhid;
                var mstProject = ProjectMstFacade.Find(mstPhid).Data;
                mstProject.FBudgetAmount = budgetAmount;
                mstProject.PersistentState = PersistentState.Modified;
                ProjectMstFacade.Save<Int64>(mstProject); //回填预算金额总额
            }
           
            //ProjectDtlBudgetDtlFacade.
        }

        #endregion


        /// <summary>
        /// 获取最大项目库编码
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public string CreateOrGetMaxProjCode(string year)
        {
            string projCode = "";
            QtSysCodeSeqModel seqM = null;

            var dicWhere = new Dictionary<string, object>(); 
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", year))
                .Add(ORMRestrictions<string>.Eq("FCode", "ProjCode")).Add(ORMRestrictions<string>.Eq("FTname", "xm3_projectmst"));
            
            FindedResults<QtSysCodeSeqModel> sysCodeSeqResult = QtSysCodeSeqFacade.Find(dicWhere);
            // FindedResults<QtSysCodeSeqModel> sysCodeSeqResult = QtSysCodeSeqFacade.Find(t => t.FYear == year && t.FCode== "ProjCode" && t.FTname== "xm3_projectmst");

            if (sysCodeSeqResult.Status == ResponseStatus.Success)
            {    
                //插入或更新项目代码编码序号                    
                if (sysCodeSeqResult.Data.Count > 0)
                {
                    seqM = sysCodeSeqResult.Data[0];
                    if (string.IsNullOrWhiteSpace(seqM.FSeqNO))
                    {
                        projCode = year + string.Format("{0:D8}", 1);
                    }
                    else
                    {
                        var max = Int64.Parse(seqM.FSeqNO.Substring(4));
                        max = max + 1;
                        projCode = year + string.Format("{0:D8}", max);
                    }

                    seqM.FSeqNO = projCode; //更新代码，访问一次后就加1，后续不还原，一直累加
                    seqM.PersistentState = PersistentState.Modified;
                }
                else
                {
                    //系统编码不存在 
                    projCode = year + string.Format("{0:D8}", 1);

                    seqM = new QtSysCodeSeqModel
                    {
                        FYear = year,
                        FCode = "ProjCode",
                        FName = "项目代码编码序号",
                        FTname = "xm3_projectmst",
                        FSeqNO = projCode,
                        PersistentState = SUP.Common.Base.PersistentState.Added
                    };
                }
                SavedResult<Int64> saveResult = QtSysCodeSeqFacade.Save<Int64>(seqM);
            }
            else
            {
                projCode = year + string.Format("{0:D8}", 1);
            }

            return projCode;
        }

       /// <summary>
       /// 生成预算时回填明细
       /// </summary>
       /// <param name="oldxm3BudgetDtl"></param>
        public void UpdateBudgetDtlList(List<ProjectDtlBudgetDtlModel> oldxm3BudgetDtl)
        {
            ProjectDtlBudgetDtlFacade.UpdateBudgetDtlList( oldxm3BudgetDtl);
        }

        /// <summary>
        /// 通过外键值获取ProjectDtlPerformTarget明细数据
        /// </summary>
        /// <typeparam name="TValType"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public FindedResults<ProjectDtlPerformTargetModel> FindProjectDtlPerformTargetByForeignKey<TValType>(TValType id)
        {
            return ProjectDtlPerformTargetFacade.FindByForeignKey(id,new string[] { "FTargetClassCode" });
        }

        /// <summary>
        /// 通过外键值获取ProjectDtlPurchaseDtl明细数据
        /// </summary>
        /// <typeparam name="TValType"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public FindedResults<ProjectDtlPurchaseDtlModel> FindProjectDtlPurchaseDtlByForeignKey<TValType>(TValType id)
        {
            return ProjectDtlPurchaseDtlFacade.FindByForeignKey(id);
        }
        /// <summary>
        /// 通过采购目录代码获取ProjectDtlPurchaseDtl明细数据
        /// </summary>
        /// <param name="codes">采购目录代码</param>
        /// <returns></returns>
        public FindedResults<ProjectDtlPurchaseDtlModel> FindProjectDtlPurchaseDtlByCatalogCode<TValType>(TValType codes)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<TValType>.In("FCatalogCode", codes));
            return ProjectDtlPurchaseDtlFacade.Find(dicWhere);
        }

        /// <summary>
        /// 通过外键值获取FindProjectDtlPurDtl4SOF明细数据
        /// </summary>
        /// <typeparam name="TValType"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public FindedResults<ProjectDtlPurDtl4SOFModel> FindProjectDtlPurDtl4SOFByForeignKey<TValType>(TValType id)
        {
            return ProjectDtlPurDtl4SOFFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 通过指标代码获取ProjectDtlPerformTarget明细数据
        /// </summary>
        /// <param name="FTargetCode">指标代码</param>
        /// <returns></returns>
        public FindedResults<ProjectDtlPerformTargetModel> FindProjectDtlPerformTargetByFTargetCode(string FTargetCode)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("FTargetCode", FTargetCode));
            return ProjectDtlPerformTargetFacade.Find(dicWhere);
        }

        /// <summary>
        /// 通过采购类型代码获取ProjectDtlPurchaseDtl明细数据
        /// </summary>
        /// <param name="FTypeCode">指标代码</param>
        /// <returns></returns>
        public FindedResults<ProjectDtlPurchaseDtlModel> FindProjectDtlPurchaseDtlByFTypeCode(string FTypeCode)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("FTypeCode", FTypeCode));
            return ProjectDtlPurchaseDtlFacade.Find(dicWhere);
        }

        /// <summary>
        /// 删除集中采购明细和资金来源
        /// </summary>
        /// <param name="id"></param>
        public void DeleteProjectDtlPurchase(Int64 id)
        {
            ProjectDtlPurchaseDtlFacade.FacadeHelper.DeleteByForeignKey<Int64>(id);
            ProjectDtlPurDtl4SOFFacade.FacadeHelper.DeleteByForeignKey<Int64>(id);
        }


        /// <summary>
        /// 回撤
        /// </summary>
        /// <param name="approveCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public PagedResult<ProjectMstModel> FindUnvalidPiid( string approveCode,string userId)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("Bizid", approveCode))
                .Add(ORMRestrictions<string>.Eq("Pk1", userId));
            var result = base.ServiceHelper.LoadWithPageInfinity("GXM3.XM.APPROVE", dicWhere);
            return result;
        }

        /// <summary>
        /// 项目同步数据到老G6H
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string AddData(string[] ids)
        {
            return ProjectMstFacade.AddData(ids);
        }

        /// <summary>
        /// 查找是否有设置功能控制
        /// </summary>
        /// <param name="BZ"></param>
        /// <param name="DWDM"></param>
        /// <returns></returns>
        public List<CorrespondenceSettings2Model> FindQTControlSet(string BZ,string DWDM)
        {
            List<CorrespondenceSettings2Model> result=new List<CorrespondenceSettings2Model>();
            Dictionary<string, object> dicWhereSet = new Dictionary<string, object>();
            new CreateCriteria(dicWhereSet)
               .Add(ORMRestrictions<string>.Eq("BZ", BZ));

            IList<QTControlSetModel> SetResult = QTControlSetFacade.Find(dicWhereSet).Data;
            if (SetResult.Count > 0) {
                QTControlSetModel qTControlSet = SetResult[0];
                if (qTControlSet.ControlOrNot == "1")
                {
                    Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<string>.Eq("Dylx", "button"))
                        .Add(ORMRestrictions<string>.Eq("DefStr1", BZ)).Add(ORMRestrictions<string>.Eq("Dydm", DWDM));
                    result = CorrespondenceSettings2Facade.FacadeHelper.Find(dicWhere).Data.ToList();
                    //result = CorrespondenceSettingsFacade.FacadeHelper.LoadWithPageInfinity("GHQTControlSet", dicWhere, false, new string[] { });
                }
            }
            return result;
        }

        /// <summary>
        /// 操作员默认部门
        /// </summary>
        /// <param name="userPhid"></param>
        /// <returns></returns>
        public string GetDefaultDept(long userPhid)
        {
            User2Model user = UserFacade.Find(userPhid).Data;
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("Dwdm", user.UserNo))
                .Add(ORMRestrictions<string>.Eq("Dylx", "08"))
                .Add(ORMRestrictions<System.Int32>.Eq("DefInt1", 1));
            IList<CorrespondenceSettingsModel> correspondences = CorrespondenceSettingsFacade.Find(dicWhere).Data;
            if (correspondences.Count > 0)
            {
                return correspondences[0].DefStr3;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 保存预算单据修改记录
        /// </summary>
        /// <param name="AfterProjectMst"></param>
        /// <param name="projectDtlImpls"></param>
        /// <param name="projectDtlTexts"></param>
        /// <param name="projectDtlFunds"></param>
        /// <param name="projectDtlBudgets"></param>
        /// <param name="projectDtlPerforms"></param>
        /// <param name="projectDtlPurchases"></param>
        /// <param name="projectDtlPurDtl4s"></param>
        /// <returns></returns>
        public CommonResult SaveModify2(ProjectMstModel AfterProjectMst, ProjectDtlImplPlanModel projectDtlImpls, ProjectDtlTextContentModel projectDtlTexts, ProjectDtlFundApplModel projectDtlFunds, ProjectDtlBudgetDtlModel projectDtlBudgets, ProjectDtlPerformTargetModel projectDtlPerforms, ProjectDtlPurchaseDtlModel projectDtlPurchases, ProjectDtlPurDtl4SOFModel projectDtlPurDtl4s)
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
            //long UserPhid = AfterProjectMst.Creator;// 
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
            { "FIfPurchase", "是否集中采购" },{ "FPerformType", "绩效项目类型" },{ "FPerformEvalType", "绩效评价类型" }
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
            // Dictionary<string, string> FApproveStatusDic = new Dictionary<string, string> { { "1", "待上报" }, { "2", "审批中" }, { "3", "审批通过" }, { "4", "已退回" } };
            //版本标识
            //Dictionary<string, string> FLifeCycleDic = new Dictionary<string, string> { { "0", "正常" }, { "1", "作废" } };
            //单据调整判断
            //Dictionary<string, string> FMidYearChangeDic = new Dictionary<string, string> { { "0", "正常" }, { "1", "调整" } };
            //生成到老G6H记录
            //Dictionary<string, string> FSaveToOldG6hDic = new Dictionary<string, string> { { "0", "否" }, { "1", "是" } };

            ProjectMstModel projectMst = ProjectMstFacade.Find(AfterProjectMst.PhId).Data;
            PropertyInfo[] properties = typeof(ProjectMstModel).GetProperties();//取ProjectMstModel的所有属性
            foreach (PropertyInfo info in properties)
            {
                if (info.Name != "PersistentState" && info.Name != "ListNotEvaluateProerty" && info.Name != "ExtendObjects" && info.Name != "NgRecordVer" && !info.Name.EndsWith("EXName"))
                {
                    //Type type = budgetMst.GetPropertyType(info.Name);//取属性的值类型
                    object beforevalue = projectMst.GetPropertyValue(info.Name) ?? "";
                    //object beforevalue2 = Convert.ChangeType(beforevalue, type)??"";

                    object aftervalue = AfterProjectMst.GetPropertyValue(info.Name) ?? "";
                    //object aftervaluee2 = Convert.ChangeType(aftervalue, type)??"";

                    if (!beforevalue.Equals(aftervalue))
                    {
                        QTModifyModel qTModify = new QTModifyModel();
                        qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();//项目状态
                        qTModify.UserCode = User.UserNo;
                        qTModify.UserName = User.UserName;
                        qTModify.IP = IP;
                        if (!colums.ContainsKey(info.Name))
                        {
                            break;
                        }
                        qTModify.ModifyField = colums[info.Name];

                        qTModify.FProjCode = projectMst.FProjCode;
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
                            /*case "FProjStatus":
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
                            case "FBudgetDept":
                                qTModify.BeforeValue = OrganizationFacade.FindMcByDm(beforevalue.ToString());
                                qTModify.AfterValue = OrganizationFacade.FindMcByDm(aftervalue.ToString());
                                break;
                            case "FApprover":
                                qTModify.BeforeValue = UserFacade.FindMcByDm(beforevalue.ToString());
                                qTModify.AfterValue = UserFacade.FindMcByDm(aftervalue.ToString());
                                break;
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
                                break;
                            case "FBillNO":
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

            //实施计划（ProjectDtlImplPlanModel）
            Dictionary<string, string> colums_Impls = new Dictionary<string, string> {{ "FImplContent", "实施内容" } ,
            { "FStartDate", "开始日期" },{ "FEndDate", "结束日期" }};
            List<ProjectDtlImplPlanModel> implPlanModels = ProjectDtlImplPlanFacade.FindByForeignKey(AfterProjectMst.PhId).Data.ToList();//原数据
            List<long> implPlanindex = new List<long>();
            for (var i = 0; i < implPlanModels.Count; i++)
            {
                implPlanindex.Add(implPlanModels[i].PhId);
            }

            if (projectDtlImpls.ProjectDtlImplPlansDel.Count > 0)
            {
                for (var i = 0; i < projectDtlImpls.ProjectDtlImplPlansDel.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "实施计划-行号" + (implPlanindex.FindIndex(item => item.Equals(projectDtlImpls.ProjectDtlImplPlansDel[i].PhId)) + 1).ToString();
                    qTModify.FProjCode = projectMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "删除";
                    modifyList.Add(qTModify);
                }
                for (var i = 0; i < projectDtlImpls.ProjectDtlImplPlansDel.Count; i++)
                {
                    implPlanindex.Remove(projectDtlImpls.ProjectDtlImplPlansDel[i].PhId);
                }
            }
            if (projectDtlImpls.ProjectDtlImplPlansAdd.Count > 0)
            {
                for (var i = 0; i < projectDtlImpls.ProjectDtlImplPlansAdd.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "实施计划-行号" + (implPlanindex.Count + 1).ToString();
                    qTModify.FProjCode = projectMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "新增";
                    modifyList.Add(qTModify);
                }
            }
            if (projectDtlImpls.ProjectDtlImplPlansMid.Count > 0)
            {
                PropertyInfo[] properties_Impls = typeof(ProjectDtlImplPlanModel).GetProperties();//取ProjectDtlImplPlanModel的所有属性
                for (var i = 0; i < projectDtlImpls.ProjectDtlImplPlansMid.Count; i++)
                {
                    foreach (PropertyInfo info in properties_Impls)
                    {
                        if (info.Name != "PersistentState" && info.Name != "ListNotEvaluateProerty" && info.Name != "ExtendObjects" && info.Name != "NgRecordVer" && !info.Name.EndsWith("EXName") && info.Name != "ForeignKeys")
                        {
                            //object beforevalue = implPlanModels[int.Parse(implPlanRow[budgetDtlImpls.ModifyRow[i].PhId.ToString()])-1].GetPropertyValue(info.Name) ?? "";
                            object beforevalue = implPlanModels[implPlanModels.FindIndex(item => item.PhId.Equals(projectDtlImpls.ProjectDtlImplPlansMid[i].PhId))].GetPropertyValue(info.Name) ?? "";
                            object aftervalue = projectDtlImpls.ProjectDtlImplPlansMid[i].GetPropertyValue(info.Name) ?? "";
                            if (!beforevalue.Equals(aftervalue))
                            {
                                QTModifyModel qTModify = new QTModifyModel();
                                qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                                qTModify.UserCode = User.UserNo;
                                qTModify.UserName = User.UserName;
                                qTModify.IP = IP;
                                if (!colums_Impls.ContainsKey(info.Name))
                                {
                                    break;
                                }
                                qTModify.ModifyField = "实施计划-行号" + (implPlanindex.FindIndex(item => item.Equals(projectDtlImpls.ProjectDtlImplPlansMid[i].PhId)) + 1).ToString() + "-" + colums_Impls[info.Name]; ;
                                qTModify.FProjCode = projectMst.FProjCode;
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
            Dictionary<string, string> colums_textContent = new Dictionary<string, string> {  { "FFunctionalOverview", "部门职能概述" } ,
            { "FProjOverview", "项目概况" },{ "FProjBasis", "立项依据" },{ "FFeasibility", "可行性" },{ "FNecessity", "必要性" },{ "FLTPerformGoal", "总体绩效目标" },{ "FAnnualPerformGoal", "年度绩效目标" }};
            ProjectDtlTextContentModel textContentModel = ProjectDtlTextContentFacade.FindByForeignKey(AfterProjectMst.PhId).Data[0];//原数据
            PropertyInfo[] properties_textContent = typeof(ProjectDtlTextContentModel).GetProperties();//取ProjectDtlTextContentModel的所有属性
            foreach (PropertyInfo info in properties_textContent)
            {
                if (info.Name != "PersistentState" && info.Name != "ListNotEvaluateProerty" && info.Name != "ExtendObjects" && info.Name != "NgRecordVer" && info.Name != "ForeignKeys")
                {
                    object beforevalue = textContentModel.GetPropertyValue(info.Name) ?? "";
                    object aftervalue = projectDtlTexts.GetPropertyValue(info.Name) ?? "";
                    if (!beforevalue.Equals(aftervalue))
                    {
                        QTModifyModel qTModify = new QTModifyModel();
                        qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                        qTModify.UserCode = User.UserNo;
                        qTModify.UserName = User.UserName;
                        qTModify.IP = IP;
                        if (!colums_textContent.ContainsKey(info.Name))
                        {
                            break;
                        }
                        qTModify.ModifyField = colums_textContent[info.Name]; ;
                        qTModify.FProjCode = projectMst.FProjCode;
                        //qTModify.FProjName
                        //qTModify.TabName = info.Name;
                        qTModify.PersistentState = PersistentState.Added;
                        qTModify.BeforeValue = beforevalue.ToString();
                        qTModify.AfterValue = aftervalue.ToString();
                        modifyList.Add(qTModify);
                    }
                }
            }

            //EntityInfo<ProjectDtlBudgetDtlModel> projectDtlBudgets 预算明细
            Dictionary<string, string> colums_budgetDtl = new Dictionary<string, string> {
            { "FName", "名称" },{ "FMeasUnit", "计量单位" },{ "FQty", "天数" },{ "FQty2", "人数" },{ "FPrice", "单价" },{ "FAmount", "金额" },{ "FSourceOfFunds", "资金来源" },{ "FBudgetAccounts", "预算科目" },
            { "FOtherInstructions", "其他说明" },{ "FPaymentMethod", "支付方式" },{ "FExpensesChannel", "支出渠道" },{ "FFeedback", "反馈意见" },{ "Xm3_DtlPhid", "明细来源" },{ "FBudgetAmount", "预算金额" },
            { "FIfPurchase", "是否集中采购" },{ "FQtZcgnfl", "支出功能分类科目" }};
            List<ProjectDtlBudgetDtlModel> budgetDtlModels = ProjectDtlBudgetDtlFacade.FindByForeignKey(AfterProjectMst.PhId).Data.ToList();//原数据
            List<long> budgetDtlindex = new List<long>();
            for (var i = 0; i < budgetDtlModels.Count; i++)
            {
                budgetDtlindex.Add(budgetDtlModels[i].PhId);
            }
            if (projectDtlBudgets.ProjectDtlBudgetDtlsDel.Count > 0)
            {
                for (var i = 0; i < projectDtlBudgets.ProjectDtlBudgetDtlsDel.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "预算明细-行号" + (budgetDtlindex.FindIndex(item => item.Equals(projectDtlBudgets.ProjectDtlBudgetDtlsDel[i].PhId)) + 1).ToString();
                    qTModify.FProjCode = projectMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "删除";
                    modifyList.Add(qTModify);

                }
                for (var i = 0; i < projectDtlBudgets.ProjectDtlBudgetDtlsDel.Count; i++)
                {
                    budgetDtlindex.Remove(projectDtlBudgets.ProjectDtlBudgetDtlsDel[i].PhId);
                }
            }
            if (projectDtlBudgets.ProjectDtlBudgetDtlsAdd.Count > 0)
            {
                for (var i = 0; i < projectDtlBudgets.ProjectDtlBudgetDtlsAdd.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "预算明细-行号" + (budgetDtlindex.Count + 1).ToString();
                    qTModify.FProjCode = projectMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "新增";
                    modifyList.Add(qTModify);
                    budgetDtlindex.Add(projectDtlBudgets.ProjectDtlBudgetDtlsAdd[i].PhId);
                }
            }
            if (projectDtlBudgets.ProjectDtlBudgetDtlsMid.Count > 0)
            {
                PropertyInfo[] properties_budgetDtl = typeof(ProjectDtlBudgetDtlModel).GetProperties();//取ProjectDtlBudgetDtlModel的所有属性
                for (var i = 0; i < projectDtlBudgets.ProjectDtlBudgetDtlsMid.Count; i++)
                {
                    foreach (PropertyInfo info in properties_budgetDtl)
                    {
                        if (info.Name != "PersistentState" && info.Name != "ListNotEvaluateProerty" && info.Name != "ExtendObjects" && info.Name != "NgRecordVer" && !info.Name.EndsWith("EXName") && info.Name != "ForeignKeys")
                        {
                            object beforevalue = budgetDtlModels[budgetDtlModels.FindIndex(item => item.PhId.Equals(projectDtlBudgets.ProjectDtlBudgetDtlsMid[i].PhId))].GetPropertyValue(info.Name) ?? "";

                            object aftervalue = projectDtlBudgets.ProjectDtlBudgetDtlsMid[i].GetPropertyValue(info.Name) ?? "";
                            if (!beforevalue.Equals(aftervalue))
                            {
                                QTModifyModel qTModify = new QTModifyModel();
                                qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                                qTModify.UserCode = User.UserNo;
                                qTModify.UserName = User.UserName;
                                qTModify.IP = IP;
                                if (!colums_budgetDtl.ContainsKey(info.Name))
                                {
                                    break;
                                }
                                qTModify.ModifyField = "预算明细-行号" + (budgetDtlindex.FindIndex(item => item.Equals(projectDtlBudgets.ProjectDtlBudgetDtlsMid[i].PhId)) + 1).ToString() + "-" + colums_budgetDtl[info.Name]; ;
                                qTModify.FProjCode = projectMst.FProjCode;
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

            //EntityInfo<ProjectDtlPerformTargetModel> projectDtlPerforms 未做记录修改保存
            //ProjectDtlPurchaseDtlModel 集中采购 EntityInfo<ProjectDtlPurchaseDtlModel> projectDtlPurchases
            Dictionary<string, string> colums_purchaseDtl = new Dictionary<string, string> {
            { "FName", "名称" },{ "FContent", "采购内容" },{ "FCatalogCode", "采购目录" },{ "FTypeCode", "采购类型" },{ "FProcedureCode", "采购程序" },{ "FMeasUnit", "计量单位" },
            { "FQty", "数量" },{ "FPrice", "预计单价" },{ "FAmount", "总计金额" },{ "FSpecification", "技术参数及配置标准" },{ "FRemark", "备注" },{ "FEstimatedPurTime", "预计采购时间" },
            { "FIfPerformanceAppraisal", "是否绩效评价" }};
            List<ProjectDtlPurchaseDtlModel> purchaseDtlModels = ProjectDtlPurchaseDtlFacade.FindByForeignKey(AfterProjectMst.PhId).Data.ToList();//原数据
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
            if (projectDtlPurchases.ProjectDtlPurchaseDtlsDel.Count > 0)
            {
                for (var i = 0; i < projectDtlPurchases.ProjectDtlPurchaseDtlsDel.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "预算明细-名称-" + projectDtlPurchases.ProjectDtlPurchaseDtlsDel[i].FName + "-集中采购";
                    qTModify.FProjCode = projectMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "删除";
                    modifyList.Add(qTModify);
                }
            }
            if (projectDtlPurchases.ProjectDtlPurchaseDtlsAdd.Count > 0)
            {
                for (var i = 0; i < projectDtlPurchases.ProjectDtlPurchaseDtlsAdd.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "预算明细-名称-" + projectDtlPurchases.ProjectDtlPurchaseDtlsAdd[i].FName + "-集中采购";
                    qTModify.FProjCode = projectMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "新增";
                    modifyList.Add(qTModify);
                }
            }
            if (projectDtlPurchases.ProjectDtlPurchaseDtlsMid.Count > 0)
            {
                PropertyInfo[] properties_Purchases = typeof(ProjectDtlPurchaseDtlModel).GetProperties();//取ProjectDtlPurchaseDtlModel的所有属性
                for (var i = 0; i < projectDtlPurchases.ProjectDtlPurchaseDtlsMid.Count; i++)
                {
                    foreach (PropertyInfo info in properties_Purchases)
                    {
                        if (info.Name != "PersistentState" && info.Name != "ListNotEvaluateProerty" && info.Name != "ExtendObjects" && info.Name != "NgRecordVer" && !info.Name.EndsWith("EXName") && info.Name != "ForeignKeys")
                        {
                            object beforevalue = purchaseDtlModels[int.Parse(budgetDtlNameRow[projectDtlPurchases.ProjectDtlPurchaseDtlsMid[i].FName])].GetPropertyValue(info.Name) ?? "";
                            object aftervalue = projectDtlPurchases.ProjectDtlPurchaseDtlsMid[i].GetPropertyValue(info.Name) ?? "";
                            if (!beforevalue.Equals(aftervalue))
                            {
                                QTModifyModel qTModify = new QTModifyModel();
                                qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                                qTModify.UserCode = User.UserNo;
                                qTModify.UserName = User.UserName;
                                qTModify.IP = IP;
                                if (!colums_purchaseDtl.ContainsKey(info.Name))
                                {
                                    break;
                                }
                                qTModify.ModifyField = "预算明细-名称-" + projectDtlPurchases.ProjectDtlPurchaseDtlsMid[i].FName + "-" + colums_purchaseDtl[info.Name]; ;
                                qTModify.FProjCode = projectMst.FProjCode;
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

            //ProjectDtlPurDtl4SOFModel 集中采购资金来源 EntityInfo<ProjectDtlPurDtl4SOFModel> projectDtlPurDtl4s
            Dictionary<string, string> colums_purDtl4SOF = new Dictionary<string, string> {
            { "FName", "明细项目名称" },{ "FSourceOfFunds", "资金来源" },{ "FAmount", "金额" }};
            List<ProjectDtlPurDtl4SOFModel> purDtl4SOFModels = ProjectDtlPurDtl4SOFFacade.FindByForeignKey(AfterProjectMst.PhId).Data.ToList();//原数据

            if (projectDtlPurDtl4s.ProjectDtlPurDtl4SOFsDel.Count > 0)
            {
                for (var i = 0; i < projectDtlPurDtl4s.ProjectDtlPurDtl4SOFsDel.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "预算明细-名称-" + projectDtlPurDtl4s.ProjectDtlPurDtl4SOFsDel[i].FName + "-集中采购资金来源";
                    qTModify.FProjCode = projectMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "删除";
                    modifyList.Add(qTModify);
                }
            }
            if (projectDtlPurDtl4s.ProjectDtlPurDtl4SOFsAdd.Count > 0)
            {
                for (var i = 0; i < projectDtlPurDtl4s.ProjectDtlPurDtl4SOFsAdd.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "预算明细-名称-" + projectDtlPurDtl4s.ProjectDtlPurDtl4SOFsAdd[i].FName + "-集中采购资金来源";
                    qTModify.FProjCode = projectMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "新增";
                    modifyList.Add(qTModify);
                }
            }
            if (projectDtlPurDtl4s.ProjectDtlPurDtl4SOFsMid.Count > 0)
            {
                PropertyInfo[] properties_Purchases = typeof(ProjectDtlPurDtl4SOFModel).GetProperties();//取BudgetDtlPurDtl4SOFModel的所有属性
                for (var i = 0; i < projectDtlPurDtl4s.ProjectDtlPurDtl4SOFsMid.Count; i++)
                {
                    foreach (PropertyInfo info in properties_Purchases)
                    {
                        if (info.Name != "PersistentState" && info.Name != "ListNotEvaluateProerty" && info.Name != "ExtendObjects" && info.Name != "NgRecordVer" && !info.Name.EndsWith("EXName") && info.Name != "ForeignKeys")
                        {
                            object beforevalue = purDtl4SOFModels[int.Parse(budgetDtlNameRow[projectDtlPurDtl4s.ProjectDtlPurDtl4SOFsMid[i].FName])].GetPropertyValue(info.Name) ?? "";
                            object aftervalue = projectDtlPurDtl4s.ProjectDtlPurDtl4SOFsMid[i].GetPropertyValue(info.Name) ?? "";
                            if (!beforevalue.Equals(aftervalue))
                            {
                                QTModifyModel qTModify = new QTModifyModel();
                                qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                                qTModify.UserCode = User.UserNo;
                                qTModify.UserName = User.UserName;
                                qTModify.IP = IP;
                                if (!colums_purDtl4SOF.ContainsKey(info.Name))
                                {
                                    break;
                                }
                                qTModify.ModifyField = "预算明细-名称-" + projectDtlPurDtl4s.ProjectDtlPurDtl4SOFsMid[i].FName + "-" + colums_purDtl4SOF[info.Name]; ;
                                qTModify.FProjCode = projectMst.FProjCode;
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
        /// 保存预算单据修改记录
        /// </summary>
        /// <param name="AfterProjectMst"></param>
        /// <param name="projectDtlImpls"></param>
        /// <param name="projectDtlTexts"></param>
        /// <param name="projectDtlFunds"></param>
        /// <param name="projectDtlBudgets"></param>
        /// <param name="projectDtlPerforms">暂时未做修改记录的保存</param>
        /// <param name="projectDtlPurchases"></param>
        /// <param name="projectDtlPurDtl4s"></param>
        /// <returns></returns>
        public CommonResult SaveModify(ProjectMstModel AfterProjectMst, EntityInfo<ProjectDtlImplPlanModel> projectDtlImpls, EntityInfo<ProjectDtlTextContentModel> projectDtlTexts, EntityInfo<ProjectDtlFundApplModel> projectDtlFunds, EntityInfo<ProjectDtlBudgetDtlModel> projectDtlBudgets, EntityInfo<ProjectDtlPerformTargetModel> projectDtlPerforms, EntityInfo<ProjectDtlPurchaseDtlModel> projectDtlPurchases, EntityInfo<ProjectDtlPurDtl4SOFModel> projectDtlPurDtl4s)
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
            //long UserPhid = AfterProjectMst.Creator;// 
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
            { "FIfPurchase", "是否集中采购" },{ "FPerformType", "绩效项目类型" },{ "FPerformEvalType", "绩效评价类型" }
            };
            //项目属性
            Dictionary<string, string> FProjAttrDic = new Dictionary<string, string> { { "1", "延续项目" }, { "2", "新增项目" } };
            //存续期限
            Dictionary<string, string> FDurationDic = new Dictionary<string, string> { { "1", "一次性项目" }, { "2", "经常性项目" }, { "3", "跨年度项目" } };
            //项目状态
            /*Dictionary<string, string> FProjStatusDic = new Dictionary<string, string> { { "1", "预立项" }, { "2", "项目立项" }, { "3", "项目执行" }, { "4", "项目调整" },
            { "5", "项目暂停" }, { "6", "项目终止" }, { "7", "项目关闭" }, { "8", "调整项目执行" }};*/
            //是否EnumYesNO
            Dictionary<string, string> EnumYesNODic = new Dictionary<string, string> { { "1", "是" }, { "2", "否" } , { "Yes", "是" }, { "No", "否" } };
            //单据类型
            //Dictionary<string, string> FTypeDic = new Dictionary<string, string> { { "c", "年初" }, { "z", "年中" }, { "x", "专项" } };
            //审批状态
           // Dictionary<string, string> FApproveStatusDic = new Dictionary<string, string> { { "1", "待上报" }, { "2", "审批中" }, { "3", "审批通过" }, { "4", "已退回" } };
            //版本标识
            //Dictionary<string, string> FLifeCycleDic = new Dictionary<string, string> { { "0", "正常" }, { "1", "作废" } };
            //单据调整判断
            //Dictionary<string, string> FMidYearChangeDic = new Dictionary<string, string> { { "0", "正常" }, { "1", "调整" } };
            //生成到老G6H记录
            //Dictionary<string, string> FSaveToOldG6hDic = new Dictionary<string, string> { { "0", "否" }, { "1", "是" } };

            ProjectMstModel projectMst = ProjectMstFacade.Find(AfterProjectMst.PhId).Data;
            PropertyInfo[] properties = typeof(ProjectMstModel).GetProperties();//取ProjectMstModel的所有属性
            foreach (PropertyInfo info in properties)
            {
                if (info.Name != "PersistentState" && info.Name != "ListNotEvaluateProerty" && info.Name != "ExtendObjects" && info.Name != "NgRecordVer" && !info.Name.EndsWith("EXName"))
                {
                    //Type type = budgetMst.GetPropertyType(info.Name);//取属性的值类型
                    object beforevalue = projectMst.GetPropertyValue(info.Name) ?? "";
                    //object beforevalue2 = Convert.ChangeType(beforevalue, type)??"";

                    object aftervalue = AfterProjectMst.GetPropertyValue(info.Name) ?? "";
                    //object aftervaluee2 = Convert.ChangeType(aftervalue, type)??"";

                    if (!beforevalue.Equals(aftervalue))
                    {
                        QTModifyModel qTModify = new QTModifyModel();
                        qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();//项目状态
                        qTModify.UserCode = User.UserNo;
                        qTModify.UserName = User.UserName;
                        qTModify.IP = IP;
                        if (!colums.ContainsKey(info.Name))
                        {
                            break;
                        }
                        qTModify.ModifyField = colums[info.Name];

                        qTModify.FProjCode = projectMst.FProjCode;
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
                            /*case "FProjStatus":
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
                            /*case "FType":
                                qTModify.BeforeValue = FTypeDic[beforevalue.ToString()];
                                qTModify.AfterValue = FTypeDic[aftervalue.ToString()];
                                break;
                            case "FApproveStatus":
                                qTModify.BeforeValue = FApproveStatusDic[beforevalue.ToString()];
                                qTModify.AfterValue = FApproveStatusDic[aftervalue.ToString()];
                                break;
                            case "FLifeCycle":
                                qTModify.BeforeValue = FLifeCycleDic[beforevalue.ToString()];
                                qTModify.AfterValue = FLifeCycleDic[aftervalue.ToString()];
                                break;*/
                            case "FBudgetDept":
                                qTModify.BeforeValue = OrganizationFacade.FindMcByDm(beforevalue.ToString());
                                qTModify.AfterValue = OrganizationFacade.FindMcByDm(aftervalue.ToString());
                                break;
                            case "FApprover":
                                qTModify.BeforeValue = UserFacade.FindMcByDm(beforevalue.ToString());
                                qTModify.AfterValue = UserFacade.FindMcByDm(aftervalue.ToString());
                                break;
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
                                break;
                            case "FBillNO":
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

            //实施计划（ProjectDtlImplPlanModel）
            Dictionary<string, string> colums_Impls = new Dictionary<string, string> {{ "FImplContent", "实施内容" } ,
            { "FStartDate", "开始日期" },{ "FEndDate", "结束日期" }};
            List<ProjectDtlImplPlanModel> implPlanModels = ProjectDtlImplPlanFacade.FindByForeignKey(AfterProjectMst.PhId).Data.ToList();//原数据
            List<long> implPlanindex = new List<long>();
            for (var i = 0; i < implPlanModels.Count; i++)
            {
                implPlanindex.Add(implPlanModels[i].PhId);
            }
            
            if (projectDtlImpls.DeleteRow.Count > 0)
            {
                for (var i = 0; i < projectDtlImpls.DeleteRow.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "实施计划-行号" + (implPlanindex.FindIndex(item => item.Equals(projectDtlImpls.DeleteRow[i].PhId)) + 1).ToString();
                    qTModify.FProjCode = projectMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "删除";
                    modifyList.Add(qTModify);
                }
                for (var i = 0; i < projectDtlImpls.DeleteRow.Count; i++)
                {
                    implPlanindex.Remove(projectDtlImpls.DeleteRow[i].PhId);
                }
            }
            if (projectDtlImpls.NewRow.Count > 0)
            {
                for (var i = 0; i < projectDtlImpls.NewRow.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "实施计划-行号" + (implPlanindex.Count + 1).ToString();
                    qTModify.FProjCode = projectMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "新增";
                    modifyList.Add(qTModify);
                }
            }
            if (projectDtlImpls.ModifyRow.Count > 0)
            {
                PropertyInfo[] properties_Impls = typeof(ProjectDtlImplPlanModel).GetProperties();//取ProjectDtlImplPlanModel的所有属性
                for (var i = 0; i < projectDtlImpls.ModifyRow.Count; i++)
                {
                    foreach (PropertyInfo info in properties_Impls)
                    {
                        if (info.Name != "PersistentState" && info.Name != "ListNotEvaluateProerty" && info.Name != "ExtendObjects" && info.Name != "NgRecordVer" && !info.Name.EndsWith("EXName") && info.Name != "ForeignKeys")
                        {
                            //object beforevalue = implPlanModels[int.Parse(implPlanRow[budgetDtlImpls.ModifyRow[i].PhId.ToString()])-1].GetPropertyValue(info.Name) ?? "";
                            object beforevalue = implPlanModels[implPlanModels.FindIndex(item => item.PhId.Equals(projectDtlImpls.ModifyRow[i].PhId))].GetPropertyValue(info.Name) ?? "";
                            object aftervalue = projectDtlImpls.ModifyRow[i].GetPropertyValue(info.Name) ?? "";
                            if (!beforevalue.Equals(aftervalue))
                            {
                                QTModifyModel qTModify = new QTModifyModel();
                                qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                                qTModify.UserCode = User.UserNo;
                                qTModify.UserName = User.UserName;
                                qTModify.IP = IP;
                                if (!colums_Impls.ContainsKey(info.Name))
                                {
                                    break;
                                }
                                qTModify.ModifyField = "实施计划-行号" + (implPlanindex.FindIndex(item => item.Equals(projectDtlImpls.ModifyRow[i].PhId)) + 1).ToString() + "-" + colums_Impls[info.Name]; ;
                                qTModify.FProjCode = projectMst.FProjCode;
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
            Dictionary<string, string> colums_textContent = new Dictionary<string, string> {  { "FFunctionalOverview", "部门职能概述" } ,
            { "FProjOverview", "项目概况" },{ "FProjBasis", "立项依据" },{ "FFeasibility", "可行性" },{ "FNecessity", "必要性" },{ "FLTPerformGoal", "总体绩效目标" },{ "FAnnualPerformGoal", "年度绩效目标" }};
            ProjectDtlTextContentModel textContentModel = ProjectDtlTextContentFacade.FindByForeignKey(AfterProjectMst.PhId).Data[0];//原数据
            PropertyInfo[] properties_textContent = typeof(ProjectDtlTextContentModel).GetProperties();//取ProjectDtlTextContentModel的所有属性
            foreach (PropertyInfo info in properties_textContent)
            {
                if (info.Name != "PersistentState" && info.Name != "ListNotEvaluateProerty" && info.Name != "ExtendObjects" && info.Name != "NgRecordVer" && info.Name != "ForeignKeys")
                {
                    object beforevalue = textContentModel.GetPropertyValue(info.Name) ?? "";
                    object aftervalue = projectDtlTexts.AllRow[0].GetPropertyValue(info.Name) ?? "";
                    if (!beforevalue.Equals(aftervalue))
                    {
                        QTModifyModel qTModify = new QTModifyModel();
                        qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                        qTModify.UserCode = User.UserNo;
                        qTModify.UserName = User.UserName;
                        qTModify.IP = IP;
                        if (!colums_textContent.ContainsKey(info.Name))
                        {
                            break;
                        }
                        qTModify.ModifyField = colums_textContent[info.Name]; ;
                        qTModify.FProjCode = projectMst.FProjCode;
                        //qTModify.FProjName
                        //qTModify.TabName = info.Name;
                        qTModify.PersistentState = PersistentState.Added;
                        qTModify.BeforeValue = beforevalue.ToString();
                        qTModify.AfterValue = aftervalue.ToString();
                        modifyList.Add(qTModify);
                    }
                }
            }

            //项目资金申请BudgetDtlFundApplModel
            //Dictionary<string, string> colums_fundAppl = new Dictionary<string, string> { { "FSourceOfFunds", "资金来源" } ,
            //{ "FAmount", "金额" }};
            //List<ProjectDtlFundApplModel> fundApplModels = ProjectDtlFundApplFacade.FindByForeignKey(AfterProjectMst.PhId).Data.ToList();//原数据
            //List<long> fundApplindex = new List<long>();
            //for (var i = 0; i < fundApplModels.Count; i++)
            //{
            //    fundApplindex.Add(fundApplModels[i].PhId);
            //}
            //if (projectDtlFunds.DeleteRow.Count > 0)
            //{
            //    for (var i = 0; i < projectDtlFunds.DeleteRow.Count; i++)
            //    {
            //        QTModifyModel qTModify = new QTModifyModel();
            //        qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
            //        qTModify.UserCode = User.UserNo;
            //        qTModify.UserName = User.UserName;
            //        qTModify.IP = IP;
            //        qTModify.ModifyField = "项目资金申请-行号" + (fundApplindex.FindIndex(item => item.Equals(projectDtlBudgets.DeleteRow[i].PhId)) + 1).ToString();
            //        qTModify.FProjCode = projectMst.FProjCode;
            //        //qTModify.FProjName
            //        //qTModify.TabName = info.Name;
            //        qTModify.PersistentState = PersistentState.Added;
            //        qTModify.AfterValue = "删除";
            //        modifyList.Add(qTModify);
            //    }
            //    for (var i = 0; i < projectDtlFunds.DeleteRow.Count; i++)
            //    {
            //        fundApplindex.Remove(projectDtlFunds.DeleteRow[i].PhId);
            //    }
            //}
            //if (projectDtlFunds.NewRow.Count > 0)
            //{
            //    for (var i = 0; i < projectDtlFunds.NewRow.Count; i++)
            //    {
            //        QTModifyModel qTModify = new QTModifyModel();
            //        qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
            //        qTModify.UserCode = User.UserNo;
            //        qTModify.UserName = User.UserName;
            //        qTModify.IP = IP;
            //        qTModify.ModifyField = "项目资金申请-行号" + (fundApplindex.Count + 1).ToString();
            //        qTModify.FProjCode = projectMst.FProjCode;
            //        //qTModify.FProjName
            //        //qTModify.TabName = info.Name;
            //        qTModify.PersistentState = PersistentState.Added;
            //        qTModify.AfterValue = "新增";
            //        modifyList.Add(qTModify);
            //    }
            //}
            //if (projectDtlFunds.ModifyRow.Count > 0)
            //{
            //    PropertyInfo[] properties_fundAppl = typeof(ProjectDtlFundApplModel).GetProperties();//取ProjectDtlFundApplModel的所有属性
            //    for (var i = 0; i < projectDtlFunds.ModifyRow.Count; i++)
            //    {
            //        foreach (PropertyInfo info in properties_fundAppl)
            //        {
            //            if (info.Name != "PersistentState" && info.Name != "ListNotEvaluateProerty" && info.Name != "ExtendObjects" && info.Name != "NgRecordVer" && !info.Name.EndsWith("EXName") && info.Name != "ForeignKeys")
            //            {
            //                //object beforevalue = fundApplModels[int.Parse(fundApplRow[budgetDtlFunds.ModifyRow[i].PhId.ToString()]) - 1].GetPropertyValue(info.Name) ?? "";
            //                object beforevalue = fundApplModels[fundApplModels.FindIndex(item => item.PhId.Equals(projectDtlFunds.ModifyRow[i].PhId))].GetPropertyValue(info.Name) ?? "";

            //                object aftervalue = projectDtlFunds.ModifyRow[i].GetPropertyValue(info.Name) ?? "";
            //                if (!beforevalue.Equals(aftervalue))
            //                {
            //                    QTModifyModel qTModify = new QTModifyModel();
            //                    qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
            //                    qTModify.UserCode = User.UserNo;
            //                    qTModify.UserName = User.UserName;
            //                    qTModify.IP = IP;
            //                    if (!colums_fundAppl.ContainsKey(info.Name))
            //                    {
            //                        break;
            //                    }
            //                    qTModify.ModifyField = "项目资金申请-行号" + (fundApplindex.FindIndex(item => item.Equals(projectDtlFunds.ModifyRow[i].PhId)) + 1).ToString() + "-" + colums_fundAppl[info.Name]; ;
            //                    qTModify.FProjCode = projectMst.FProjCode;
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

            //EntityInfo<ProjectDtlBudgetDtlModel> projectDtlBudgets 预算明细
            Dictionary<string, string> colums_budgetDtl = new Dictionary<string, string> { 
            { "FName", "名称" },{ "FMeasUnit", "计量单位" },{ "FQty", "天数" },{ "FQty2", "人数" },{ "FPrice", "单价" },{ "FAmount", "金额" },{ "FSourceOfFunds", "资金来源" },{ "FBudgetAccounts", "预算科目" },
            { "FOtherInstructions", "其他说明" },{ "FPaymentMethod", "支付方式" },{ "FExpensesChannel", "支出渠道" },{ "FFeedback", "反馈意见" },{ "Xm3_DtlPhid", "明细来源" },{ "FBudgetAmount", "预算金额" },
            { "FIfPurchase", "是否集中采购" },{ "FQtZcgnfl", "支出功能分类科目" }};
            List<ProjectDtlBudgetDtlModel> budgetDtlModels = ProjectDtlBudgetDtlFacade.FindByForeignKey(AfterProjectMst.PhId).Data.ToList();//原数据
            List<long> budgetDtlindex = new List<long>();
            for (var i = 0; i < budgetDtlModels.Count; i++)
            {
                budgetDtlindex.Add(budgetDtlModels[i].PhId);
            }
            if (projectDtlBudgets.DeleteRow.Count > 0)
            {
                for (var i = 0; i < projectDtlBudgets.DeleteRow.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "预算明细-行号" + (budgetDtlindex.FindIndex(item => item.Equals(projectDtlBudgets.DeleteRow[i].PhId)) + 1).ToString();
                    qTModify.FProjCode = projectMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "删除";
                    modifyList.Add(qTModify);

                }
                for (var i = 0; i < projectDtlBudgets.DeleteRow.Count; i++)
                {
                    budgetDtlindex.Remove(projectDtlBudgets.DeleteRow[i].PhId);
                }
            }
            if (projectDtlBudgets.NewRow.Count > 0)
            {
                for (var i = 0; i < projectDtlBudgets.NewRow.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "预算明细-行号" + (budgetDtlindex.Count + 1).ToString();
                    qTModify.FProjCode = projectMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "新增";
                    modifyList.Add(qTModify);
                    budgetDtlindex.Add(projectDtlBudgets.NewRow[i].PhId);
                }
            }
            if (projectDtlBudgets.ModifyRow.Count > 0)
            {
                PropertyInfo[] properties_budgetDtl = typeof(ProjectDtlBudgetDtlModel).GetProperties();//取ProjectDtlBudgetDtlModel的所有属性
                for (var i = 0; i < projectDtlBudgets.ModifyRow.Count; i++)
                {
                    foreach (PropertyInfo info in properties_budgetDtl)
                    {
                        if (info.Name != "PersistentState" && info.Name != "ListNotEvaluateProerty" && info.Name != "ExtendObjects" && info.Name != "NgRecordVer" && !info.Name.EndsWith("EXName") && info.Name != "ForeignKeys")
                        {
                            object beforevalue = budgetDtlModels[budgetDtlModels.FindIndex(item => item.PhId.Equals(projectDtlBudgets.ModifyRow[i].PhId))].GetPropertyValue(info.Name) ?? "";

                            object aftervalue = projectDtlBudgets.ModifyRow[i].GetPropertyValue(info.Name) ?? "";
                            if (!beforevalue.Equals(aftervalue))
                            {
                                QTModifyModel qTModify = new QTModifyModel();
                                qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                                qTModify.UserCode = User.UserNo;
                                qTModify.UserName = User.UserName;
                                qTModify.IP = IP;
                                if (!colums_budgetDtl.ContainsKey(info.Name))
                                {
                                    break;
                                }
                                qTModify.ModifyField = "预算明细-行号" + (budgetDtlindex.FindIndex(item => item.Equals(projectDtlBudgets.ModifyRow[i].PhId)) + 1).ToString() + "-" + colums_budgetDtl[info.Name]; ;
                                qTModify.FProjCode = projectMst.FProjCode;
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

            //EntityInfo<ProjectDtlPerformTargetModel> projectDtlPerforms 未做记录修改保存
            //ProjectDtlPurchaseDtlModel 集中采购 EntityInfo<ProjectDtlPurchaseDtlModel> projectDtlPurchases
            Dictionary<string, string> colums_purchaseDtl = new Dictionary<string, string> {  
            { "FName", "名称" },{ "FContent", "采购内容" },{ "FCatalogCode", "采购目录" },{ "FTypeCode", "采购类型" },{ "FProcedureCode", "采购程序" },{ "FMeasUnit", "计量单位" },
            { "FQty", "数量" },{ "FPrice", "预计单价" },{ "FAmount", "总计金额" },{ "FSpecification", "技术参数及配置标准" },{ "FRemark", "备注" },{ "FEstimatedPurTime", "预计采购时间" },
            { "FIfPerformanceAppraisal", "是否绩效评价" }};
            List<ProjectDtlPurchaseDtlModel> purchaseDtlModels = ProjectDtlPurchaseDtlFacade.FindByForeignKey(AfterProjectMst.PhId).Data.ToList();//原数据
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
            if (projectDtlPurchases.DeleteRow.Count > 0)
            {
                for (var i = 0; i < projectDtlPurchases.DeleteRow.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "预算明细-名称-" + projectDtlPurchases.DeleteRow[i].FName + "-集中采购";
                    qTModify.FProjCode = projectMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "删除";
                    modifyList.Add(qTModify);
                }
            }
            if (projectDtlPurchases.NewRow.Count > 0)
            {
                for (var i = 0; i < projectDtlPurchases.NewRow.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "预算明细-名称-" + projectDtlPurchases.NewRow[i].FName + "-集中采购";
                    qTModify.FProjCode = projectMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "新增";
                    modifyList.Add(qTModify);
                }
            }
            if (projectDtlPurchases.ModifyRow.Count > 0)
            {
                PropertyInfo[] properties_Purchases = typeof(ProjectDtlPurchaseDtlModel).GetProperties();//取ProjectDtlPurchaseDtlModel的所有属性
                for (var i = 0; i < projectDtlPurchases.ModifyRow.Count; i++)
                {
                    foreach (PropertyInfo info in properties_Purchases)
                    {
                        if (info.Name != "PersistentState" && info.Name != "ListNotEvaluateProerty" && info.Name != "ExtendObjects" && info.Name != "NgRecordVer" && !info.Name.EndsWith("EXName") && info.Name != "ForeignKeys")
                        {
                            object beforevalue = purchaseDtlModels[int.Parse(budgetDtlNameRow[projectDtlPurchases.ModifyRow[i].FName])].GetPropertyValue(info.Name) ?? "";
                            object aftervalue = projectDtlPurchases.ModifyRow[i].GetPropertyValue(info.Name) ?? "";
                            if (!beforevalue.Equals(aftervalue))
                            {
                                QTModifyModel qTModify = new QTModifyModel();
                                qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                                qTModify.UserCode = User.UserNo;
                                qTModify.UserName = User.UserName;
                                qTModify.IP = IP;
                                if (!colums_purchaseDtl.ContainsKey(info.Name))
                                {
                                    break;
                                }
                                qTModify.ModifyField = "预算明细-名称-" + projectDtlPurchases.ModifyRow[i].FName + "-" + colums_purchaseDtl[info.Name]; ;
                                qTModify.FProjCode = projectMst.FProjCode;
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

            //ProjectDtlPurDtl4SOFModel 集中采购资金来源 EntityInfo<ProjectDtlPurDtl4SOFModel> projectDtlPurDtl4s
            Dictionary<string, string> colums_purDtl4SOF = new Dictionary<string, string> { 
            { "FName", "明细项目名称" },{ "FSourceOfFunds", "资金来源" },{ "FAmount", "金额" }};
            List<ProjectDtlPurDtl4SOFModel> purDtl4SOFModels = ProjectDtlPurDtl4SOFFacade.FindByForeignKey(AfterProjectMst.PhId).Data.ToList();//原数据
            
            if (projectDtlPurDtl4s.DeleteRow.Count > 0)
            {
                for (var i = 0; i < projectDtlPurDtl4s.DeleteRow.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "预算明细-名称-" + projectDtlPurDtl4s.DeleteRow[i].FName + "-集中采购资金来源";
                    qTModify.FProjCode = projectMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "删除";
                    modifyList.Add(qTModify);
                }
            }
            if (projectDtlPurDtl4s.NewRow.Count > 0)
            {
                for (var i = 0; i < projectDtlPurDtl4s.NewRow.Count; i++)
                {
                    QTModifyModel qTModify = new QTModifyModel();
                    qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                    qTModify.UserCode = User.UserNo;
                    qTModify.UserName = User.UserName;
                    qTModify.IP = IP;
                    qTModify.ModifyField = "预算明细-名称-" + projectDtlPurDtl4s.NewRow[i].FName + "-集中采购资金来源";
                    qTModify.FProjCode = projectMst.FProjCode;
                    //qTModify.FProjName
                    //qTModify.TabName = info.Name;
                    qTModify.PersistentState = PersistentState.Added;
                    qTModify.AfterValue = "新增";
                    modifyList.Add(qTModify);
                }
            }
            if (projectDtlPurDtl4s.ModifyRow.Count > 0)
            {
                PropertyInfo[] properties_Purchases = typeof(ProjectDtlPurDtl4SOFModel).GetProperties();//取BudgetDtlPurDtl4SOFModel的所有属性
                for (var i = 0; i < projectDtlPurDtl4s.ModifyRow.Count; i++)
                {
                    foreach (PropertyInfo info in properties_Purchases)
                    {
                        if (info.Name != "PersistentState" && info.Name != "ListNotEvaluateProerty" && info.Name != "ExtendObjects" && info.Name != "NgRecordVer" && !info.Name.EndsWith("EXName") && info.Name != "ForeignKeys")
                        {
                            object beforevalue = purDtl4SOFModels[int.Parse(budgetDtlNameRow[projectDtlPurDtl4s.ModifyRow[i].FName])].GetPropertyValue(info.Name) ?? "";
                            object aftervalue = projectDtlPurDtl4s.ModifyRow[i].GetPropertyValue(info.Name) ?? "";
                            if (!beforevalue.Equals(aftervalue))
                            {
                                QTModifyModel qTModify = new QTModifyModel();
                                qTModify.DEFSTR1 = projectMst.FProjStatus.ToString();
                                qTModify.UserCode = User.UserNo;
                                qTModify.UserName = User.UserName;
                                qTModify.IP = IP;
                                if (!colums_purDtl4SOF.ContainsKey(info.Name))
                                {
                                    break;
                                }
                                qTModify.ModifyField = "预算明细-名称-" + projectDtlPurDtl4s.ModifyRow[i].FName + "-" + colums_purDtl4SOF[info.Name]; ;
                                qTModify.FProjCode = projectMst.FProjCode;
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
        /// 判断当前选则的模本金额跟实际录入金额的大小比较
        /// </summary>
        /// <param name="busType"></param>
        /// <param name="IndividualInfoId"></param>
        /// <param name="projAmount"></param>
        /// <param name="OrgCode"></param>
        /// <returns></returns>
        public string FindIndividualInfo(string busType,long IndividualInfoId,decimal projAmount,string OrgCode)
        {
            string dataid="";
            if (busType== "GHProjectBegin")
            {
                dataid = BudgetMstFacade.Find(IndividualInfoId).Data.FIndividualinfophid;
            }
            else
            {
                dataid = ProjectMstFacade.Find(IndividualInfoId).Data.FIndividualinfophid;
            }
            if (!string.IsNullOrEmpty(dataid))
            {
                //先增加控制，如果基本基础金额区间设置为0~0，直接返回空
                var individuals = this.QTIndividualInfoFacade.Find(long.Parse(dataid)).Data;
                if (individuals != null)
                {
                    if (individuals.IndividualinfoAmount1 == 0 && individuals.IndividualinfoAmount2 == 0)
                    {
                        return "";
                    }
                }
            }

            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("IndividualinfoBustype", busType));
            var IndividualInfoList = QTIndividualInfoFacade.FacadeHelper.Find(dicWhere).Data.ToList();
            if (!string.IsNullOrEmpty(OrgCode))
            {
                IndividualInfoList = IndividualInfoList.FindAll(x => !string.IsNullOrEmpty(x.DEFSTR9) && x.DEFSTR9.Split(',').ToList().Contains(OrgCode));
            }
            var new_id = "";

            foreach(var item in IndividualInfoList)
            {
                if(projAmount > item.IndividualinfoAmount1 && projAmount <= item.IndividualinfoAmount2)
                {
                    new_id = item.PhId.ToString();
                    break;
                }
            }

            return new_id;
        }
        /// <summary>
        /// 根据项目id获取符合条件的表单
        /// </summary>
        /// <param name="busType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string FindIndividualInfoById(string busType, long id)
        {
            var xm = ProjectMstFacade.Find(id).Data;
            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("IndividualinfoBustype", busType));
            var IndividualInfoList = QTIndividualInfoFacade.FacadeHelper.Find(dicWhere);
            var new_id = "";

            foreach (var item in IndividualInfoList.Data)
            {
                if (xm.FProjAmount > item.IndividualinfoAmount1 && xm.FProjAmount <= item.IndividualinfoAmount2)
                {
                    new_id = item.PhId.ToString();
                    xm.FIndividualinfophid = new_id;
                    xm.PersistentState = PersistentState.Modified;
                    ProjectMstFacade.Save<Int64>(xm, "");
                    break;
                }
            }
            
            return new_id;
        }
        

        #region //新增接口-wgg

        /// <summary>
        /// 根据主键集合修改作废状态
        /// </summary>
        /// <param name="phids">主键集合</param>
        /// <returns></returns>
        public SavedResult<long> PostCancetProjectList(List<long> phids)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic).
                    Add(ORMRestrictions<List<long>>.In("PhId", phids));
            var payments = this.ProjectMstFacade.Find(dic).Data;
            if (payments.Count > 0)
            {
                foreach (var payment in payments)
                {
                    if (payment.FApproveStatus != "1" && payment.FApproveStatus != "4")
                    {
                        throw new Exception("只有未送审与未通过的单据可以作废！");
                    }
                    //payment.FLifeCycle = 1;
                    payment.FDeleteMark = 1;
                    payment.PersistentState = PersistentState.Modified;
                }
                savedResult = this.ProjectMstFacade.Save<long>(payments);
                //dic.Clear();
                //new CreateCriteria(dic).
                //        Add(ORMRestrictions<List<long>>.In("MstPhid", phids));
                //var paymentXms = this.PaymentXmRule.Find(dic);
                //if (paymentXms.Count > 0)
                //{
                //    foreach (var paymentXm in paymentXms)
                //    {
                //        paymentXm.FDelete = (byte)1;
                //        paymentXm.PersistentState = PersistentState.Modified;
                //    }
                //    savedResult = this.PaymentXmRule.Save<long>(paymentXms);
                //}
                //dic.Clear();
                //new CreateCriteria(dic).
                //        Add(ORMRestrictions<List<long>>.In("MstPhid", phids));
                //var paymentDtls = this.PaymentDtlRule.Find(dic);
                //if (paymentDtls.Count > 0)
                //{
                //    foreach (var paymentDtl in paymentDtls)
                //    {
                //        paymentDtl.FDelete = (byte)1;
                //        paymentDtl.PersistentState = PersistentState.Modified;
                //    }
                //    savedResult = this.PaymentDtlRule.Save<long>(paymentDtls);
                //}
            }
            return savedResult;
        }

        /// <summary>
        /// 根据主键集合获取打印数据
        /// </summary>
        /// <param name="phids">主键集合</param>
        /// <returns></returns>
        public List<object> PostPrintData(string[] phids)
        {
            List<object> result = new List<object>();
            foreach(var phid in phids)
            {
                ProjectMstModel projectMst = ProjectMstFacade.Find(long.Parse(phid)).Data;

                var FDeclarationDept = projectMst.FDeclarationDept;
                var FDeclarationDeptList = OrganizationFacade.Find(x=>x.OCode == projectMst.FDeclarationDept).Data;
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
                ProjectDtlTextContentModel projectDtlText = ProjectDtlTextContentFacade.FindByForeignKey(long.Parse(phid)).Data[0];

                List<object> Budgets = new List<object>();
                IList<ProjectDtlBudgetDtlModel> projectDtlBudgets = ProjectDtlBudgetDtlFacade.FindByForeignKey(long.Parse(phid)).Data;
                foreach(var projectDtlBudget in projectDtlBudgets)
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
                        FName=projectDtlBudget.FName,
                        FBudgetAccounts=projectDtlBudget.FBudgetAccounts_EXName,
                        FAmount=projectDtlBudget.FAmount,
                        FPaymentMethod= FPaymentMethod,
                        FExpensesChannel =projectDtlBudget.FExpensesChannel_EXName,
                        FOtherInstructions=projectDtlBudget.FOtherInstructions,
                        FSourceOfFunds = projectDtlBudget.FSourceOfFunds_EXName,
                        FQtZcgnfl = projectDtlBudget.FQtZcgnfl_EXName
                    });
                }

                IList<QtAttachmentModel> qtAttachments = new List<QtAttachmentModel>();
                qtAttachments = this.QtAttachmentFacade.Find(t => t.BTable == "XM3_PROJECTMST" && t.RelPhid == long.Parse(phid)).Data;
                object data = new
                {
                    DJH = projectMst.PhId,
                    FProjCode =projectMst.FProjCode,
                    FDeclarationDept = FDeclarationDept,
                    FDateofDeclaration=projectMst.FDateofDeclaration,
                    FDeclarer=projectMst.FDeclarer,
                    FProjName=projectMst.FProjName,
                    FProjAttr= FProjAttr,
                    FDuration= FDuration,
                    FExpenseCategory=projectMst.FExpenseCategory_EXName,
                    FStartDate=projectMst.FStartDate,
                    FEndDate=projectMst.FEndDate,
                    FFunctionalOverview=projectDtlText.FFunctionalOverview,
                    FProjBasis= projectDtlText.FProjBasis,
                    FFeasibility= projectDtlText.FFeasibility,
                    FNecessity= projectDtlText.FNecessity,
                    FLTPerformGoal= projectDtlText.FLTPerformGoal,
                    FAnnualPerformGoal= projectDtlText.FAnnualPerformGoal,
                    Budgets= Budgets,
                    FundAppl= ProjectDtlFundApplFacade.FindByForeignKey(long.Parse(phid)).Data,
                    ImplPlan= ProjectDtlImplPlanFacade.FindByForeignKey(long.Parse(phid)).Data,
                    FMeetiingSummaryNo = projectMst.FMeetiingSummaryNo,
                    FMeetingTime = projectMst.FMeetingTime,
                    FLeadingOpinions = projectDtlText.FLeadingOpinions,
                    FChairmanOpinions = projectDtlText.FChairmanOpinions,
                    FBz = projectDtlText.FBz,
                    FDeptOpinions = projectDtlText.FDeptOpinions,
                    FDeptOpinions2 = projectDtlText.FDeptOpinions2,
                    FResolution = projectDtlText.FResolution,
                    QtAttachments = qtAttachments
                };
                result.Add(data);
            }
            return result;
        }

        /// <summary>
        /// 获取新的项目绩效集合
        /// </summary>
        /// <param name="projectDtlPerformTargets">项目带的绩效集合</param>
        /// <param name="targetTypeCode">父级节点</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        public List<ProjectDtlPerformTargetModel> GetNewProPerformTargets(List<ProjectDtlPerformTargetModel> projectDtlPerformTargets, string targetTypeCode, long orgId, string orgCode)
        {
            if(projectDtlPerformTargets != null && projectDtlPerformTargets.Count > 0)
            {
                //该组织下所有的指标类型集合
                IList<PerformEvalTargetTypeModel> performEvalTargetTypes = this.PerformEvalTargetTypeFacade.Find(t => t.Orgcode == orgCode && t.Orgid == orgId).Data;

                int max = 0;
                foreach (var proPer in projectDtlPerformTargets)
                {
                    if (proPer.FTargetTypeCode.Length / 2 > max)
                    {
                        max = proPer.FTargetTypeCode.Length / 2;
                    }
                }
                foreach (var per in projectDtlPerformTargets)
                {
                    per.FTargetTypeName = performEvalTargetTypes.ToList().Find(t => t.FCode == per.FTargetTypeCode) == null ? "" : performEvalTargetTypes.ToList().Find(t => t.FCode == per.FTargetTypeCode).FName;
                    per.FTargetTypePerantCode = performEvalTargetTypes.ToList().Find(t => t.FCode == per.FTargetTypeCode) == null ? "" : performEvalTargetTypes.ToList().Find(t => t.FCode == per.FTargetTypeCode).FCode;
                    if (max - per.FTargetTypeCode.Length / 2 == 0)
                    {
                        per.TypeCount = 1;
                        per.FTargetTypeCode1 = per.FTargetTypeCode;
                        per.FTargetTypeName1 = per.FTargetTypeName;
                        GetNewProPerformTarget(performEvalTargetTypes, per, targetTypeCode, 1);
                    }
                    else if (max - per.FTargetTypeCode.Length / 2 == 1)
                    {
                        per.TypeCount = 2;
                        per.FTargetTypeCode2 = per.FTargetTypeCode;
                        per.FTargetTypeName2 = per.FTargetTypeName;
                        GetNewProPerformTarget(performEvalTargetTypes, per, targetTypeCode, 1);
                    }
                    else if (max - per.FTargetTypeCode.Length / 2 == 2)
                    {
                        per.TypeCount = 3;
                        per.FTargetTypeCode3 = per.FTargetTypeCode;
                        per.FTargetTypeName3 = per.FTargetTypeName;
                        GetNewProPerformTarget(performEvalTargetTypes, per, targetTypeCode, 1);
                    }
                    else if (max - per.FTargetTypeCode.Length / 2 == 3)
                    {
                        per.TypeCount = 4;
                        per.FTargetTypeCode4 = per.FTargetTypeCode;
                        per.FTargetTypeName4 = per.FTargetTypeName;
                        GetNewProPerformTarget(performEvalTargetTypes, per, targetTypeCode, 1);
                    }
                    else if (max - per.FTargetTypeCode.Length / 2 == 4)
                    {
                        per.TypeCount = 5;
                        per.FTargetTypeCode5 = per.FTargetTypeCode;
                        per.FTargetTypeName5 = per.FTargetTypeName;
                        GetNewProPerformTarget(performEvalTargetTypes, per, targetTypeCode, 1);
                    }
                    else
                    {
                        continue;
                    }
                }

                projectDtlPerformTargets = projectDtlPerformTargets.OrderBy(t => t.FTargetTypeCode5).OrderBy(t => t.FTargetTypeCode4).OrderBy(t => t.FTargetTypeCode3).OrderBy(t => t.FTargetTypeCode2).OrderBy(t => t.FTargetTypeCode1).ToList();
            }
            return projectDtlPerformTargets;
        }

        /// <summary>
        /// 获取新的绩效明细
        /// </summary>
        /// <param name="performEvalTargetTypes">该组织绩效指标类型集合</param>
        /// <param name="projectDtlPerformTarget">单个绩效明细</param>
        /// <param name="targetTypeCode">父节点</param>
        /// <param name="num">数量</param>
        /// <returns></returns>
        public ProjectDtlPerformTargetModel GetNewProPerformTarget(IList<PerformEvalTargetTypeModel> performEvalTargetTypes, ProjectDtlPerformTargetModel projectDtlPerformTarget, string targetTypeCode, int num)
        {
            if (projectDtlPerformTarget != null && performEvalTargetTypes != null && performEvalTargetTypes.Count > 0 && projectDtlPerformTarget.FTargetTypePerantCode != targetTypeCode)
            {
                if (projectDtlPerformTarget.FTargetTypeCode.Length - num * 2 >= 0)
                {
                    var type = performEvalTargetTypes.ToList().Find(t => t.FCode == projectDtlPerformTarget.FTargetTypeCode.Substring(0, projectDtlPerformTarget.FTargetTypeCode.Length - num * 2));
                    if (type != null)
                    {
                        if (projectDtlPerformTarget.TypeCount == 1)
                        {
                            projectDtlPerformTarget.FTargetTypeCode2 = type.FCode;
                            projectDtlPerformTarget.FTargetTypeName2 = type.FName;
                            projectDtlPerformTarget.TypeCount++;
                            projectDtlPerformTarget.FTargetTypePerantCode = type.FCode;
                            num++;
                            GetNewProPerformTarget(performEvalTargetTypes, projectDtlPerformTarget, targetTypeCode, num);
                        }
                        else if (projectDtlPerformTarget.TypeCount == 2)
                        {
                            projectDtlPerformTarget.FTargetTypeCode3 = type.FCode;
                            projectDtlPerformTarget.FTargetTypeName3 = type.FName;
                            projectDtlPerformTarget.TypeCount++;
                            projectDtlPerformTarget.FTargetTypePerantCode = type.FCode;
                            num++;
                            GetNewProPerformTarget(performEvalTargetTypes, projectDtlPerformTarget, targetTypeCode, num);
                        }
                        else if (projectDtlPerformTarget.TypeCount == 3)
                        {
                            projectDtlPerformTarget.FTargetTypeCode4 = type.FCode;
                            projectDtlPerformTarget.FTargetTypeName4 = type.FName;
                            projectDtlPerformTarget.TypeCount++;
                            projectDtlPerformTarget.FTargetTypePerantCode = type.FCode;
                            num++;
                            GetNewProPerformTarget(performEvalTargetTypes, projectDtlPerformTarget, targetTypeCode, num);
                        }
                        else if (projectDtlPerformTarget.TypeCount == 4)
                        {
                            projectDtlPerformTarget.FTargetTypeCode5 = type.FCode;
                            projectDtlPerformTarget.FTargetTypeName5 = type.FName;
                            projectDtlPerformTarget.TypeCount++;
                            projectDtlPerformTarget.FTargetTypePerantCode = type.FCode;
                            num++;
                            GetNewProPerformTarget(performEvalTargetTypes, projectDtlPerformTarget, targetTypeCode, num);
                        }
                        else
                        {

                        }
                    }
                }

            }
            return projectDtlPerformTarget;
        }

        #endregion

        #region//汇总与申请表导出

        /// <summary>
        /// 预立项汇总打印(省总)
        /// </summary>
        /// <param name="projectMsts">项目集合</param>
        /// <param name="qtCoverUpForOrg">套打格式</param>
        /// <param name="organize">组织</param>
        /// <param name="user">人员</param>
        /// <returns></returns>
        public string ExportSummaryExcelSZ1(IList<ProjectMstModel> projectMsts, QtCoverUpForOrgModel qtCoverUpForOrg, OrganizeModel organize, User2Model user)
        {

            string[] head = { "序号", "项目编码", "项目名称", "支出类别", "存续期限", "项目属性", "单据状态", "绩效评价", "项目金额", "开始日期", "结束日期", "申报日期", "申报人" };

            //行索引
            int rowNumber = 0;

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("工会预立项项目列表汇总表");
            sheet.DefaultRowHeight = 18 * 20;
            sheet.DefaultColumnWidth = 12;
            sheet.SetColumnWidth(0, 4800);
            sheet.SetColumnWidth(1, 4800);
            sheet.SetColumnWidth(2, 4800);
            sheet.SetColumnWidth(3, 4800);
            sheet.SetColumnWidth(4, 4800);
            sheet.SetColumnWidth(5, 4800);
            sheet.SetColumnWidth(6, 4800);
            sheet.SetColumnWidth(7, 4800);
            sheet.SetColumnWidth(8, 4800);
            sheet.SetColumnWidth(9, 4800);
            sheet.SetColumnWidth(10, 4800);
            sheet.SetColumnWidth(11, 4800);
            sheet.SetColumnWidth(12, 4800);
            sheet.SetColumnWidth(13, 4800);
            sheet.SetColumnWidth(14, 4800);

            //合并单元格
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 14));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 5));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 6, 11));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 12, 14));


            //标题单元格样式
            ICellStyle cellTitleStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 500, 14, false);
            ICellStyle cellTitleStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
            ICellStyle cellTitleStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Right, VerticalAlignment.Center, 400, 12, false);
            //表头单元格样式
            ICellStyle cellHeadStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
            //内容单元格样式
            ICellStyle cellStyle1 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
            ICellStyle cellStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
            ICellStyle cellStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
            //数字内容格式
            ICellStyle cellStyle4 = ExcelHelper.CreateStyle2(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);

            //标题
            IRow row = sheet.CreateRow(rowNumber);
            row.Height = 20 * 20;
            ICell cell = row.CreateCell(0);
            cell.SetCellValue(qtCoverUpForOrg.TempLateName + "工会预立项项目汇总表");
            cell.CellStyle = cellTitleStyle;
            rowNumber++;
            row = sheet.CreateRow(rowNumber);
            cell = row.CreateCell(0);
            cell.SetCellValue("申报部门：" + organize.OName);
            cell.CellStyle = cellTitleStyle2;
            cell = row.CreateCell(6);
            cell.SetCellValue("制表日期：" + DateTime.Now.ToString("yyyy年MM月dd日"));
            cell.CellStyle = cellTitleStyle2;
            cell = row.CreateCell(12);
            cell.SetCellValue("单位：元");
            cell.CellStyle = cellTitleStyle2;
            rowNumber++;

            //表头
            row = sheet.CreateRow(rowNumber);
            cell = row.CreateCell(0);
            cell.SetCellValue("序号");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(1);
            cell.SetCellValue("项目编码");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(2);
            cell.SetCellValue("项目名称");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(3);
            cell.SetCellValue("支出类别");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(4);
            cell.SetCellValue("存续期限");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(5);
            cell.SetCellValue("项目属性");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(6);
            cell.SetCellValue("单据状态");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(7);
            cell.SetCellValue("绩效评价");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(8);
            cell.SetCellValue("项目金额");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(9);
            cell.SetCellValue("开始日期");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(10);
            cell.SetCellValue("结束日期");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(11);
            cell.SetCellValue("申报日期");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(12);
            cell.SetCellValue("申报人");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(13);
            cell.SetCellValue("审批人");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(14);
            cell.SetCellValue("审批时间");
            cell.CellStyle = cellHeadStyle;
            rowNumber++;

            double sum = 0;
            //表格内容
            for (int i = 1; i <= projectMsts.Count; i++)
            {
                row = sheet.CreateRow(rowNumber);
                //每行
                cell = row.CreateCell(0);
                cell.SetCellValue(i);
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(1);
                cell.SetCellValue(projectMsts[i - 1].FProjCode);
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(2);
                cell.SetCellValue(projectMsts[i - 1].FProjName);
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(3);
                cell.SetCellValue(projectMsts[i - 1].FExpenseCategory_EXName);
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(4);
                cell.SetCellValue(projectMsts[i - 1].FDuration_EXName);
                cell.CellStyle = cellStyle2;

                cell = row.CreateCell(5);
                cell.SetCellValue(projectMsts[i - 1].FProjAttr_EXName);
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(6);
                cell.SetCellValue(GetApproveName(projectMsts[i - 1].FApproveStatus));
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(7);
                cell.SetCellValue(GetIfPerformance((int)projectMsts[i - 1].FIfPerformanceAppraisal));
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(8);
                cell.SetCellValue((double)projectMsts[i - 1].FProjAmount);
                cell.CellStyle = cellStyle4;
                cell = row.CreateCell(9);
                cell.SetCellValue(projectMsts[i - 1].FStartDate == null ? "":((DateTime)projectMsts[i - 1].FStartDate).ToString("yyyy-MM-dd"));
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(10);
                cell.SetCellValue(projectMsts[i - 1].FEndDate == null ? "" : ((DateTime)projectMsts[i - 1].FEndDate).ToString("yyyy-MM-dd"));
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(11);
                cell.SetCellValue(projectMsts[i - 1].FDateofDeclaration == null ? "" : ((DateTime)projectMsts[i - 1].FDateofDeclaration).ToString("yyyy-MM-dd"));
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(12);
                cell.SetCellValue(projectMsts[i - 1].FDeclarer);
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(13);
                cell.SetCellValue(projectMsts[i - 1].FApproveDate == null ? "" : ((DateTime)projectMsts[i - 1].FApproveDate).ToString("yyyy-MM-dd"));
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(14);
                cell.SetCellValue(projectMsts[i - 1].FApprover_EXName);
                cell.CellStyle = cellStyle2;
                rowNumber++;
                sum += (double)projectMsts[i - 1].FProjAmount;
            }
            //合并单元格
            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 0, 14));
            //土地部分
            row = sheet.CreateRow(rowNumber);
            cell = row.CreateCell(0);
            cell.SetCellValue("项目金额合计：" + sum);
            cell.CellStyle = cellHeadStyle;
            rowNumber++;

            //sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 0, 3));
            //sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 4, 8));
            //sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 9, 12));
            //row = sheet.CreateRow(rowNumber);
            //cell = row.CreateCell(0);
            //cell.SetCellValue("制表人：");
            //cell.CellStyle = cellTitleStyle2;
            //cell = row.CreateCell(4);
            //cell.SetCellValue("部门领导审核：");
            //cell.CellStyle = cellTitleStyle2;
            //cell = row.CreateCell(9);
            //cell.SetCellValue("分管领导审核：");
            ////cell.SetCellValue("制表人："+"                            "+"部门领导审核："+"                            "+"分管领导审核：");
            //cell.CellStyle = cellTitleStyle2;

            //合并单元格
            //sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 2));
            //row = sheet.CreateRow(rowNumber);
            //cell = row.CreateCell(0);
            //cell.SetCellValue("工会主席:" + sysOrganize.Chairman);
            //cell.CellStyle = cellTitleStyle2;
            //cell = row.CreateCell(1);
            //cell.SetCellValue("财务负责人:" + sysOrganize.Treasurer);
            //cell.CellStyle = cellTitleStyle2;
            //cell = row.CreateCell(3);
            //cell.SetCellValue("复核:");
            //cell.CellStyle = cellTitleStyle2;
            //cell = row.CreateCell(4);
            //cell.SetCellValue("制表:" + sysUser.RealName);
            //cell.CellStyle = cellTitleStyle2;
            rowNumber++;

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            var buf = ms.ToArray();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\\DownLoadFiles\\YProjectMst";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            using (FileStream fs = File.OpenWrite(path + "\\" + filename))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
                fs.Close();
            }
            workbook = null;
            ms.Close();
            ms.Dispose();

            return JsonConvert.SerializeObject(new { path = "YProjectMst", filename = filename });
        }

        /// <summary>
        /// 预立项汇总打印(明细区域的)
        /// </summary>
        /// <param name="projectMsts">项目集合</param>
        /// <param name="qtCoverUpForOrg">套打格式</param>
        /// <param name="organize">组织</param>
        /// <param name="user">人员</param>
        /// <returns></returns>
        public string ExportSummaryExcel1(IList<ProjectMstModel> projectMsts, QtCoverUpForOrgModel qtCoverUpForOrg, OrganizeModel organize, User2Model user)
        {

            string[] head = { "序号", "项目编码", "项目名称", "支出类别", "存续期限","项目属性","单据状态","绩效评价","项目金额","开始日期","结束日期","申报日期","申报人"};

            //行索引
            int rowNumber = 0;

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("工会预立项项目列表汇总表");
            sheet.DefaultRowHeight = 18 * 20;
            sheet.DefaultColumnWidth = 12;
            sheet.SetColumnWidth(0, 4800);
            sheet.SetColumnWidth(1, 4800);
            sheet.SetColumnWidth(2, 4800);
            sheet.SetColumnWidth(3, 4800);
            sheet.SetColumnWidth(4, 4800);
            sheet.SetColumnWidth(5, 4800);
            sheet.SetColumnWidth(6, 4800);
            sheet.SetColumnWidth(7, 4800);
            sheet.SetColumnWidth(8, 4800);
            sheet.SetColumnWidth(9, 4800);
            sheet.SetColumnWidth(10, 4800);
            sheet.SetColumnWidth(11, 4800);
            sheet.SetColumnWidth(12, 4800);


            //合并单元格
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 12));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 4));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 5, 10));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 11, 12));


            //标题单元格样式
            ICellStyle cellTitleStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 500, 14, false);
            ICellStyle cellTitleStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
            ICellStyle cellTitleStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Right, VerticalAlignment.Center, 400, 12, false);
            //表头单元格样式
            ICellStyle cellHeadStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
            //内容单元格样式
            ICellStyle cellStyle1 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
            ICellStyle cellStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
            ICellStyle cellStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
            //数字内容格式
            ICellStyle cellStyle4 = ExcelHelper.CreateStyle2(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);

            //标题
            IRow row = sheet.CreateRow(rowNumber);
            row.Height = 20 * 20;
            ICell cell = row.CreateCell(0);
            cell.SetCellValue(qtCoverUpForOrg.TempLateName + "工会预立项项目汇总表");
            cell.CellStyle = cellTitleStyle;
            rowNumber++;
            row = sheet.CreateRow(rowNumber);
            cell = row.CreateCell(0);
            cell.SetCellValue("申报部门：" + organize.OName);
            cell.CellStyle = cellTitleStyle2;
            cell = row.CreateCell(5);
            cell.SetCellValue("制表日期：" + DateTime.Now.ToString("yyyy年MM月dd日"));
            cell.CellStyle = cellTitleStyle2;
            cell = row.CreateCell(11);
            cell.SetCellValue("单位：元");
            cell.CellStyle = cellTitleStyle2;
            rowNumber++;

            //表头
            row = sheet.CreateRow(rowNumber);
            cell = row.CreateCell(0);
            cell.SetCellValue("序号");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(1);
            cell.SetCellValue("项目编码");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(2);
            cell.SetCellValue("项目名称");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(3);
            cell.SetCellValue("支出类别");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(4);
            cell.SetCellValue("存续期限");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(5);
            cell.SetCellValue("项目属性");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(6);
            cell.SetCellValue("单据状态");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(7);
            cell.SetCellValue("绩效评价");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(8);
            cell.SetCellValue("项目金额");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(9);
            cell.SetCellValue("开始日期");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(10);
            cell.SetCellValue("结束日期");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(11);
            cell.SetCellValue("申报日期");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(12);
            cell.SetCellValue("申报人");
            cell.CellStyle = cellHeadStyle;
            rowNumber++;

            double sum = 0;
            //表格内容
            for (int i = 1; i <= projectMsts.Count; i++)
            {
                row = sheet.CreateRow(rowNumber);                
                //每行
                cell = row.CreateCell(0);
                cell.SetCellValue(i);
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(1);
                cell.SetCellValue(projectMsts[i-1].FProjCode);
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(2);
                cell.SetCellValue(projectMsts[i - 1].FProjName);
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(3);
                cell.SetCellValue(projectMsts[i - 1].FExpenseCategory_EXName);
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(4);
                cell.SetCellValue(projectMsts[i - 1].FDuration_EXName);
                cell.CellStyle = cellStyle2;

                cell = row.CreateCell(5);
                cell.SetCellValue(projectMsts[i - 1].FProjAttr_EXName);
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(6);
                cell.SetCellValue(GetApproveName(projectMsts[i - 1].FApproveStatus));
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(7);
                cell.SetCellValue(GetIfPerformance((int)projectMsts[i - 1].FIfPerformanceAppraisal));
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(8);
                cell.SetCellValue((double)projectMsts[i - 1].FProjAmount);
                cell.CellStyle = cellStyle4;
                cell = row.CreateCell(9);
                cell.SetCellValue(projectMsts[i - 1].FStartDate == null ? "" : ((DateTime)projectMsts[i - 1].FStartDate).ToString("yyyy-MM-dd"));
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(10);
                cell.SetCellValue(projectMsts[i - 1].FEndDate == null ? "" : ((DateTime)projectMsts[i - 1].FEndDate).ToString("yyyy-MM-dd"));
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(11);
                cell.SetCellValue(projectMsts[i - 1].FDateofDeclaration == null ? "" : ((DateTime)projectMsts[i - 1].FDateofDeclaration).ToString("yyyy-MM-dd"));
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(12);
                cell.SetCellValue(projectMsts[i - 1].FDeclarer);
                cell.CellStyle = cellStyle2;
                rowNumber++;
                sum += (double)projectMsts[i-1].FProjAmount;
            }
            //合并单元格
            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 0, 12));           
            //土地部分
            row = sheet.CreateRow(rowNumber);
            cell = row.CreateCell(0);
            cell.SetCellValue("项目金额合计：" + sum);
            cell.CellStyle = cellHeadStyle;
            rowNumber++;

            sheet.AddMergedRegion(new CellRangeAddress(rowNumber , rowNumber, 0, 3));
            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 4, 8));
            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 9, 12));
            row = sheet.CreateRow(rowNumber);
            cell = row.CreateCell(0);
            cell.SetCellValue("制表人：");
            cell.CellStyle = cellTitleStyle2;
            cell = row.CreateCell(4);
            cell.SetCellValue("部门领导审核：");
            cell.CellStyle = cellTitleStyle2;
            cell = row.CreateCell(9);
            cell.SetCellValue("分管领导审核：");
            //cell.SetCellValue("制表人："+"                            "+"部门领导审核："+"                            "+"分管领导审核：");
            cell.CellStyle = cellTitleStyle2;
           
            //合并单元格
            //sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 2));
            //row = sheet.CreateRow(rowNumber);
            //cell = row.CreateCell(0);
            //cell.SetCellValue("工会主席:" + sysOrganize.Chairman);
            //cell.CellStyle = cellTitleStyle2;
            //cell = row.CreateCell(1);
            //cell.SetCellValue("财务负责人:" + sysOrganize.Treasurer);
            //cell.CellStyle = cellTitleStyle2;
            //cell = row.CreateCell(3);
            //cell.SetCellValue("复核:");
            //cell.CellStyle = cellTitleStyle2;
            //cell = row.CreateCell(4);
            //cell.SetCellValue("制表:" + sysUser.RealName);
            //cell.CellStyle = cellTitleStyle2;
            rowNumber++;

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            var buf = ms.ToArray();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\\DownLoadFiles\\YProjectMst";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            using (FileStream fs = File.OpenWrite(path + "\\" + filename))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
                fs.Close();
            }
            workbook = null;
            ms.Close();
            ms.Dispose();

            return JsonConvert.SerializeObject(new { path = "YProjectMst", filename = filename });
        }


        /// <summary>
        /// 立项汇总打印(省总)
        /// </summary>
        /// <param name="projectMsts">项目集合</param>
        /// <param name="qtCoverUpForOrg">套打格式</param>
        /// <param name="organize">组织</param>
        /// <param name="user">人员</param>
        /// <returns></returns>
        public string ExportSummaryExcelSZ2(IList<ProjectMstModel> projectMsts, QtCoverUpForOrgModel qtCoverUpForOrg, OrganizeModel organize, User2Model user)
        {

            string[] head = { "序号", "项目编码", "项目名称", "支出类别", "存续期限", "项目属性", "单据状态", "绩效评价", "项目金额", "开始日期", "结束日期", "申报日期", "申报人" };

            //行索引
            int rowNumber = 0;

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("工会立项项目列表汇总表");
            sheet.DefaultRowHeight = 18 * 20;
            sheet.DefaultColumnWidth = 12;
            sheet.SetColumnWidth(0, 4800);
            sheet.SetColumnWidth(1, 4800);
            sheet.SetColumnWidth(2, 4800);
            sheet.SetColumnWidth(3, 4800);
            sheet.SetColumnWidth(4, 4800);
            sheet.SetColumnWidth(5, 4800);
            sheet.SetColumnWidth(6, 4800);
            sheet.SetColumnWidth(7, 4800);
            sheet.SetColumnWidth(8, 4800);
            sheet.SetColumnWidth(9, 4800);
            sheet.SetColumnWidth(10, 4800);
            sheet.SetColumnWidth(11, 4800);
            sheet.SetColumnWidth(12, 4800);
            sheet.SetColumnWidth(13, 4800);
            sheet.SetColumnWidth(14, 4800);

            //合并单元格
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 14));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 5));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 6, 11));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 12, 14));


            //标题单元格样式
            ICellStyle cellTitleStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 500, 14, false);
            ICellStyle cellTitleStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
            ICellStyle cellTitleStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Right, VerticalAlignment.Center, 400, 12, false);
            //表头单元格样式
            ICellStyle cellHeadStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
            //内容单元格样式
            ICellStyle cellStyle1 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
            ICellStyle cellStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
            ICellStyle cellStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
            //数字内容格式
            ICellStyle cellStyle4 = ExcelHelper.CreateStyle2(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);

            //标题
            IRow row = sheet.CreateRow(rowNumber);
            row.Height = 20 * 20;
            ICell cell = row.CreateCell(0);
            cell.SetCellValue(qtCoverUpForOrg.TempLateName + "工会立项项目汇总表");
            cell.CellStyle = cellTitleStyle;
            rowNumber++;
            row = sheet.CreateRow(rowNumber);
            cell = row.CreateCell(0);
            cell.SetCellValue("申报部门：" + organize.OName);
            cell.CellStyle = cellTitleStyle2;
            cell = row.CreateCell(6);
            cell.SetCellValue("制表日期：" + DateTime.Now.ToString("yyyy年MM月dd日"));
            cell.CellStyle = cellTitleStyle2;
            cell = row.CreateCell(12);
            cell.SetCellValue("单位：元");
            cell.CellStyle = cellTitleStyle2;
            rowNumber++;

            //表头
            row = sheet.CreateRow(rowNumber);
            cell = row.CreateCell(0);
            cell.SetCellValue("序号");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(1);
            cell.SetCellValue("项目编码");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(2);
            cell.SetCellValue("项目名称");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(3);
            cell.SetCellValue("支出类别");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(4);
            cell.SetCellValue("存续期限");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(5);
            cell.SetCellValue("项目属性");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(6);
            cell.SetCellValue("单据状态");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(7);
            cell.SetCellValue("绩效评价");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(8);
            cell.SetCellValue("项目金额");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(9);
            cell.SetCellValue("开始日期");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(10);
            cell.SetCellValue("结束日期");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(11);
            cell.SetCellValue("申报日期");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(12);
            cell.SetCellValue("申报人");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(13);
            cell.SetCellValue("审批人");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(14);
            cell.SetCellValue("审批时间");
            cell.CellStyle = cellHeadStyle;
            rowNumber++;

            double sum = 0;
            //表格内容
            for (int i = 1; i <= projectMsts.Count; i++)
            {
                row = sheet.CreateRow(rowNumber);
                //每行
                cell = row.CreateCell(0);
                cell.SetCellValue(i);
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(1);
                cell.SetCellValue(projectMsts[i - 1].FProjCode);
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(2);
                cell.SetCellValue(projectMsts[i - 1].FProjName);
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(3);
                cell.SetCellValue(projectMsts[i - 1].FExpenseCategory_EXName);
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(4);
                cell.SetCellValue(projectMsts[i - 1].FDuration_EXName);
                cell.CellStyle = cellStyle2;

                cell = row.CreateCell(5);
                cell.SetCellValue(projectMsts[i - 1].FProjAttr_EXName);
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(6);
                cell.SetCellValue(GetApproveName(projectMsts[i - 1].FApproveStatus));
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(7);
                cell.SetCellValue(GetIfPerformance((int)projectMsts[i - 1].FIfPerformanceAppraisal));
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(8);
                cell.SetCellValue((double)projectMsts[i - 1].FProjAmount);
                cell.CellStyle = cellStyle4;
                cell = row.CreateCell(9);
                cell.SetCellValue(projectMsts[i - 1].FStartDate == null ? "" : ((DateTime)projectMsts[i - 1].FStartDate).ToString("yyyy-MM-dd"));
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(10);
                cell.SetCellValue(projectMsts[i - 1].FEndDate == null ? "" : ((DateTime)projectMsts[i - 1].FEndDate).ToString("yyyy-MM-dd"));
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(11);
                cell.SetCellValue(projectMsts[i - 1].FDateofDeclaration == null ? "" : ((DateTime)projectMsts[i - 1].FDateofDeclaration).ToString("yyyy-MM-dd"));
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(12);
                cell.SetCellValue(projectMsts[i - 1].FDeclarer);
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(13);
                cell.SetCellValue(projectMsts[i - 1].FApproveDate == null ? "" : ((DateTime)projectMsts[i - 1].FApproveDate).ToString("yyyy-MM-dd"));
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(14);
                cell.SetCellValue(projectMsts[i - 1].FApprover_EXName);
                cell.CellStyle = cellStyle2;
                rowNumber++;
                sum += (double)projectMsts[i - 1].FProjAmount;
            }
            //合并单元格
            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 0, 14));
            //土地部分
            row = sheet.CreateRow(rowNumber);
            cell = row.CreateCell(0);
            cell.SetCellValue("项目金额合计：" + sum);
            cell.CellStyle = cellHeadStyle;
            rowNumber++;

            //sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 0, 3));
            //sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 4, 8));
            //sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 9, 12));
            //row = sheet.CreateRow(rowNumber);
            //cell = row.CreateCell(0);
            //cell.SetCellValue("制表人：");
            //cell.CellStyle = cellTitleStyle2;
            //cell = row.CreateCell(4);
            //cell.SetCellValue("部门领导审核：");
            //cell.CellStyle = cellTitleStyle2;
            //cell = row.CreateCell(9);
            //cell.SetCellValue("分管领导审核：");
            ////cell.SetCellValue("制表人："+"                            "+"部门领导审核："+"                            "+"分管领导审核：");
            //cell.CellStyle = cellTitleStyle2;

            //合并单元格
            //sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 2));
            //row = sheet.CreateRow(rowNumber);
            //cell = row.CreateCell(0);
            //cell.SetCellValue("工会主席:" + sysOrganize.Chairman);
            //cell.CellStyle = cellTitleStyle2;
            //cell = row.CreateCell(1);
            //cell.SetCellValue("财务负责人:" + sysOrganize.Treasurer);
            //cell.CellStyle = cellTitleStyle2;
            //cell = row.CreateCell(3);
            //cell.SetCellValue("复核:");
            //cell.CellStyle = cellTitleStyle2;
            //cell = row.CreateCell(4);
            //cell.SetCellValue("制表:" + sysUser.RealName);
            //cell.CellStyle = cellTitleStyle2;
            rowNumber++;

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            var buf = ms.ToArray();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\\DownLoadFiles\\ProjectMst";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            using (FileStream fs = File.OpenWrite(path + "\\" + filename))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
                fs.Close();
            }
            workbook = null;
            ms.Close();
            ms.Dispose();

            return JsonConvert.SerializeObject(new { path = "ProjectMst", filename = filename });
        }

        /// <summary>
        /// 立项汇总打印(明细区域的)
        /// </summary>
        /// <param name="projectMsts">项目集合</param>
        /// <param name="qtCoverUpForOrg">套打格式</param>
        /// <param name="organize">组织</param>
        /// <param name="user">人员</param>
        /// <returns></returns>
        public string ExportSummaryExcel2(IList<ProjectMstModel> projectMsts, QtCoverUpForOrgModel qtCoverUpForOrg, OrganizeModel organize, User2Model user)
        {

            string[] head = { "序号", "项目编码", "项目名称", "支出类别", "存续期限", "项目属性", "单据状态", "绩效评价", "项目金额", "开始日期", "结束日期", "申报日期", "申报人" };

            //行索引
            int rowNumber = 0;

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("工会立项项目列表汇总表");
            sheet.DefaultRowHeight = 18 * 20;
            sheet.DefaultColumnWidth = 12;
            sheet.SetColumnWidth(0, 4800);
            sheet.SetColumnWidth(1, 4800);
            sheet.SetColumnWidth(2, 4800);
            sheet.SetColumnWidth(3, 4800);
            sheet.SetColumnWidth(4, 4800);
            sheet.SetColumnWidth(5, 4800);
            sheet.SetColumnWidth(6, 4800);
            sheet.SetColumnWidth(7, 4800);
            sheet.SetColumnWidth(8, 4800);
            sheet.SetColumnWidth(9, 4800);
            sheet.SetColumnWidth(10, 4800);
            sheet.SetColumnWidth(11, 4800);
            sheet.SetColumnWidth(12, 4800);


            //合并单元格
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 12));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 4));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 5, 10));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 11, 12));


            //标题单元格样式
            ICellStyle cellTitleStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 500, 14, false);
            ICellStyle cellTitleStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
            ICellStyle cellTitleStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Right, VerticalAlignment.Center, 400, 12, false);
            //表头单元格样式
            ICellStyle cellHeadStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
            //内容单元格样式
            ICellStyle cellStyle1 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
            ICellStyle cellStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
            ICellStyle cellStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
            //数字内容格式
            ICellStyle cellStyle4 = ExcelHelper.CreateStyle2(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);

            //标题
            IRow row = sheet.CreateRow(rowNumber);
            row.Height = 20 * 20;
            ICell cell = row.CreateCell(0);
            cell.SetCellValue(qtCoverUpForOrg.TempLateName + "工会立项项目汇总表");
            cell.CellStyle = cellTitleStyle;
            rowNumber++;
            row = sheet.CreateRow(rowNumber);
            cell = row.CreateCell(0);
            cell.SetCellValue("申报部门：" + organize.OName);
            cell.CellStyle = cellTitleStyle2;
            cell = row.CreateCell(5);
            cell.SetCellValue("制表日期：" + DateTime.Now.ToString("yyyy年MM月dd日"));
            cell.CellStyle = cellTitleStyle2;
            cell = row.CreateCell(11);
            cell.SetCellValue("单位：元");
            cell.CellStyle = cellTitleStyle2;
            rowNumber++;

            //表头
            row = sheet.CreateRow(rowNumber);
            cell = row.CreateCell(0);
            cell.SetCellValue("序号");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(1);
            cell.SetCellValue("项目编码");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(2);
            cell.SetCellValue("项目名称");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(3);
            cell.SetCellValue("支出类别");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(4);
            cell.SetCellValue("存续期限");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(5);
            cell.SetCellValue("项目属性");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(6);
            cell.SetCellValue("单据状态");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(7);
            cell.SetCellValue("绩效评价");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(8);
            cell.SetCellValue("项目金额");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(9);
            cell.SetCellValue("开始日期");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(10);
            cell.SetCellValue("结束日期");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(11);
            cell.SetCellValue("申报日期");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(12);
            cell.SetCellValue("申报人");
            cell.CellStyle = cellHeadStyle;
            rowNumber++;

            double sum = 0;
            //表格内容
            for (int i = 1; i <= projectMsts.Count; i++)
            {
                row = sheet.CreateRow(rowNumber);
                //每行
                cell = row.CreateCell(0);
                cell.SetCellValue(i);
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(1);
                cell.SetCellValue(projectMsts[i - 1].FProjCode);
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(2);
                cell.SetCellValue(projectMsts[i - 1].FProjName);
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(3);
                cell.SetCellValue(projectMsts[i-1].FExpenseCategory_EXName);
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(4);
                cell.SetCellValue(projectMsts[i - 1].FDuration_EXName);
                cell.CellStyle = cellStyle2;

                cell = row.CreateCell(5);
                cell.SetCellValue(projectMsts[i - 1].FProjAttr_EXName);
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(6);
                cell.SetCellValue(GetApproveName(projectMsts[i - 1].FApproveStatus));
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(7);
                cell.SetCellValue(GetIfPerformance((int)projectMsts[i - 1].FIfPerformanceAppraisal));
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(8);
                cell.SetCellValue((double)projectMsts[i - 1].FProjAmount);
                cell.CellStyle = cellStyle4;
                cell = row.CreateCell(9);
                cell.SetCellValue( projectMsts[i - 1].FStartDate == null ? "" : ((DateTime)projectMsts[i - 1].FStartDate).ToString("yyyy-MM-dd"));
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(10);
                cell.SetCellValue(projectMsts[i - 1].FEndDate == null ? "" : ((DateTime)projectMsts[i - 1].FEndDate).ToString("yyyy-MM-dd"));
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(11);
                cell.SetCellValue( projectMsts[i - 1].FDateofDeclaration == null ? "" : ((DateTime)projectMsts[i - 1].FDateofDeclaration).ToString("yyyy-MM-dd"));
                cell.CellStyle = cellStyle2;
                cell = row.CreateCell(12);
                cell.SetCellValue(projectMsts[i - 1].FDeclarer);
                cell.CellStyle = cellStyle2;
                rowNumber++;
                sum += (double)projectMsts[i - 1].FProjAmount;
            }
            //合并单元格
            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 0, 12));
            //土地部分
            row = sheet.CreateRow(rowNumber);
            cell = row.CreateCell(0);
            cell.SetCellValue("项目金额合计：" + sum);
            cell.CellStyle = cellHeadStyle;
            rowNumber++;

            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 0, 3));
            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 4, 8));
            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 9, 12));
            row = sheet.CreateRow(rowNumber);
            cell = row.CreateCell(0);
            cell.SetCellValue("制表人：");
            cell.CellStyle = cellTitleStyle2;
            cell = row.CreateCell(4);
            cell.SetCellValue("部门领导审核：");
            cell.CellStyle = cellTitleStyle2;
            cell = row.CreateCell(9);
            cell.SetCellValue("分管领导审核：");
            //cell.SetCellValue("制表人："+"                            "+"部门领导审核："+"                            "+"分管领导审核：");
            cell.CellStyle = cellTitleStyle2;

            //合并单元格
            //sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 2));
            //row = sheet.CreateRow(rowNumber);
            //cell = row.CreateCell(0);
            //cell.SetCellValue("工会主席:" + sysOrganize.Chairman);
            //cell.CellStyle = cellTitleStyle2;
            //cell = row.CreateCell(1);
            //cell.SetCellValue("财务负责人:" + sysOrganize.Treasurer);
            //cell.CellStyle = cellTitleStyle2;
            //cell = row.CreateCell(3);
            //cell.SetCellValue("复核:");
            //cell.CellStyle = cellTitleStyle2;
            //cell = row.CreateCell(4);
            //cell.SetCellValue("制表:" + sysUser.RealName);
            //cell.CellStyle = cellTitleStyle2;
            rowNumber++;

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            var buf = ms.ToArray();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\\DownLoadFiles\\ProjectMst";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            using (FileStream fs = File.OpenWrite(path + "\\" + filename))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
                fs.Close();
            }
            workbook = null;
            ms.Close();
            ms.Dispose();

            return JsonConvert.SerializeObject(new { path = "ProjectMst", filename = filename });
        }

        /// <summary>
        /// 预立项申报表打印
        /// </summary>
        /// <param name="projectMsts">项目集合</param>
        /// <param name="qtCoverUpForOrg">套打格式</param>
        /// <param name="organize">组织</param>
        /// <param name="user">人员</param>
        /// <returns></returns>
        public string ExportDeclareExcel1(IList<ProjectMstModel> projectMsts, QtCoverUpForOrgModel qtCoverUpForOrg, OrganizeModel organize, User2Model user)
        {
            //行索引
            int rowNumber = 0;
            HSSFWorkbook workbook = new HSSFWorkbook();
            if (projectMsts != null && projectMsts.Count > 0)
            {
                var syssets = QTSysSetFacade.Find(t => t.PhId != 0).Data.ToList();
                int count = 0;
                foreach(var project in projectMsts)
                {
                    rowNumber = 0;
                    ISheet sheet;
                    if (count > 0)
                    {
                        sheet = workbook.CreateSheet("工会预立项项目列表申报表" + count);
                    }
                    else
                    {
                        sheet = workbook.CreateSheet("工会预立项项目列表申报表");
                    }
                    sheet.DefaultRowHeight = 18 * 20;
                    sheet.DefaultColumnWidth = 12;
                    sheet.SetColumnWidth(0, 6400);
                    sheet.SetColumnWidth(1, 4800);
                    sheet.SetColumnWidth(2, 4800);
                    sheet.SetColumnWidth(3, 4800);
                    sheet.SetColumnWidth(4, 4800);
                    sheet.SetColumnWidth(5, 4800);
                    sheet.SetColumnWidth(6, 4800);
                    sheet.SetColumnWidth(7, 4800);
                    sheet.SetColumnWidth(8, 4800);
                    sheet.SetColumnWidth(9, 4800);



                    //合并单元格
                    sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 5));
                    sheet.AddMergedRegion(new CellRangeAddress(1, 1, 7, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 1, 2));
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 4, 5));
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 7, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(3, 3, 1, 5));
                    sheet.AddMergedRegion(new CellRangeAddress(3, 3, 7, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(4, 4, 1, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(5, 5, 1, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(6, 6, 1, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(7, 7, 1, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(8, 8, 1, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(9, 9, 1, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(10, 11, 0, 0));
                    sheet.AddMergedRegion(new CellRangeAddress(10, 10, 1, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(11, 11, 1, 9));


                    //标题单元格样式
                    ICellStyle cellTitleStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 500, 14, false);
                    ICellStyle cellTitleStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                    ICellStyle cellTitleStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Right, VerticalAlignment.Center, 400, 12, false);
                    //表头单元格样式
                    ICellStyle cellHeadStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
                    //内容单元格样式
                    ICellStyle cellStyle1 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                    ICellStyle cellStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
                    ICellStyle cellStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                    //数字内容格式
                    ICellStyle cellStyle4 = ExcelHelper.CreateStyle2(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);

                    //标题
                    IRow row = sheet.CreateRow(rowNumber);
                    row.Height = 20 * 20;
                    ICell cell = row.CreateCell(0);
                    cell.SetCellValue(qtCoverUpForOrg.TempLateName + "工会预立项项目申报表");
                    cell.CellStyle = cellTitleStyle;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(6);
                    cell.SetCellValue("单据号");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(7);
                    cell.SetCellValue(project.FBillNo);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;

                    //先获取项目可研
                    IList<ProjectDtlTextContentModel> projectDtlTextContents = new List<ProjectDtlTextContentModel>();
                    projectDtlTextContents = this.ProjectDtlTextContentFacade.FindByForeignKey(project.PhId).Data;
                    ProjectDtlTextContentModel projectDtlTextContent = new ProjectDtlTextContentModel();
                    if(projectDtlTextContents != null && projectDtlTextContents.Count > 0)
                    {
                        projectDtlTextContent = projectDtlTextContents[0];
                    }
                    else
                    {
                        projectDtlTextContent = null;
                    }


                    //表头
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("申报部门");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(project.FDeclarationDept_EXName);
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(3);
                    cell.SetCellValue("申报日期");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(4);
                    cell.SetCellValue(project.FDateofDeclaration == null ? "" : ((DateTime)project.FDateofDeclaration).ToString("yyyy-MM-dd"));
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(6);
                    cell.SetCellValue("填表人");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(7);
                    cell.SetCellValue(project.FDeclarer);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("项目名称");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(project.FProjName);
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(6);
                    cell.SetCellValue("项目编码");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(7);
                    cell.SetCellValue(project.FProjCode);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("项目属性");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(project.FProjAttr_EXName);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("存续期限");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(project.FDuration_EXName);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("支出类别");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(project.FExpenseCategory_EXName);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("项目起止时间");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(project.FStartDate == null ? "" : ((DateTime)project.FStartDate).ToString("yyyy-MM-dd") + "-" + project.FEndDate == null ? "" : ((DateTime)project.FEndDate).ToString("yyyy-MM-dd"));
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("部门职能概述");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(projectDtlTextContent == null ? "": projectDtlTextContent.FFunctionalOverview);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("项目申报依据(需上传附件)");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(projectDtlTextContent == null ? "" : projectDtlTextContent.FProjBasis);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("项目可行性和必要性");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(projectDtlTextContent == null ? "" : projectDtlTextContent.FFeasibility);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(1);
                    cell.SetCellValue(projectDtlTextContent == null ? "" : projectDtlTextContent.FNecessity);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;

                    //项目预算明细
                    IList<ProjectDtlBudgetDtlModel> projectDtlBudgetDtls = this.ProjectDtlBudgetDtlFacade.FindByForeignKey(project.PhId).Data;
                    if(projectDtlBudgetDtls != null && projectDtlBudgetDtls.Count > 0)
                    {
                        
                        //明细表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlBudgetDtls.Count+ 1, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 3));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 9));
                        //明细表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目预算明细(万元)");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("序号");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(2);
                        cell.SetCellValue("明细项目名称");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(4);
                        cell.SetCellValue("金额");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(5);
                        cell.SetCellValue("支付方式");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(6);
                        cell.SetCellValue("测算过程或其他需要说明的事项");
                        cell.CellStyle = cellHeadStyle;
                        rowNumber++;
                        double dtlSum = 0;
                        for (int j = 1; j <= projectDtlBudgetDtls.Count;j++)
                        {
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 3));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 9));
                            //支付方式代码转名称
                            var syssetProjectDtlBudgetDtl = syssets.FindAll(x => x.DicType == "PayMethodTwo" && x.Orgcode == project.FDeclarationUnit && x.TypeCode == projectDtlBudgetDtls[j-1].FPaymentMethod);
                            if (syssetProjectDtlBudgetDtl.Count > 0)
                            {
                                projectDtlBudgetDtls[j - 1].FPaymentMethod_EXName = syssetProjectDtlBudgetDtl[0].TypeName;
                            }
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(1);
                            cell.SetCellValue(j);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(2);
                            cell.SetCellValue(projectDtlBudgetDtls[j - 1].FName);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(4);
                            cell.SetCellValue((double)(projectDtlBudgetDtls[j - 1].FAmount/10000));
                            cell.CellStyle = cellStyle4;
                            cell = row.CreateCell(5);
                            cell.SetCellValue(projectDtlBudgetDtls[j - 1].FPaymentMethod_EXName);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(6);
                            cell.SetCellValue(projectDtlBudgetDtls[j - 1].FOtherInstructions);
                            cell.CellStyle = cellHeadStyle;
                            rowNumber++;
                            dtlSum += (double)(projectDtlBudgetDtls[j - 1].FAmount / 10000);
                        }

                        //合计
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 3));
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(1);
                        cell.SetCellValue("合计");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(4);
                        cell.SetCellValue(dtlSum);
                        cell.CellStyle = cellStyle4;
                        rowNumber++;
                    }
                    else
                    {
                        //明细表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + 1, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 3));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 9));
                        //明细表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目预算明细(万元)");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("序号");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(2);
                        cell.SetCellValue("明细项目名称");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(4);
                        cell.SetCellValue("金额");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(5);
                        cell.SetCellValue("支付方式");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(6);
                        cell.SetCellValue("测算过程或其他需要说明的事项");
                        cell.CellStyle = cellHeadStyle;
                        rowNumber++;
                        double dtlSum = 0;                       

                        //合计
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 3));
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(1);
                        cell.SetCellValue("合计");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(4);
                        cell.SetCellValue(dtlSum);
                        cell.CellStyle = cellStyle4;
                        rowNumber++;
                    }

                    //项目资金申请
                    IList<ProjectDtlFundApplModel> projectDtlFundAppls = this.ProjectDtlFundApplFacade.FindByForeignKey(project.PhId).Data;
                    if(projectDtlFundAppls != null && projectDtlFundAppls.Count > 0)
                    {
                        //项目资金申请表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count + 1, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 6));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 7, 9));
                        //项目资金申请表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目资金申请(万元)");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("序号");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(2);
                        cell.SetCellValue("资金来源");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(7);
                        cell.SetCellValue("金额");
                        cell.CellStyle = cellHeadStyle;                       
                        rowNumber++;
                        double fundSum = 0;
                        for(int j=1;j<= projectDtlFundAppls.Count; j++)
                        {
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 6));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 7, 9));
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(1);
                            cell.SetCellValue(j);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(2);
                            cell.SetCellValue(projectDtlFundAppls[j - 1].FSourceOfFunds_EXName);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(7);
                            cell.SetCellValue((double)(projectDtlFundAppls[j - 1].FAmount / 10000));
                            cell.CellStyle = cellStyle4;
                            
                            rowNumber++;
                            fundSum += (double)(projectDtlFundAppls[j - 1].FAmount / 10000);
                        }

                        //合计
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 6));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 7, 9));
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(1);
                        cell.SetCellValue("合计");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(7);
                        cell.SetCellValue(fundSum);
                        cell.CellStyle = cellStyle4;
                        rowNumber++;
                    }
                    else
                    {
                        //项目资金申请表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + 1, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 6));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 7, 9));
                        //项目资金申请表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目资金申请(万元)");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("序号");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(2);
                        cell.SetCellValue("资金来源");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(7);
                        cell.SetCellValue("金额");
                        cell.CellStyle = cellHeadStyle;
                        rowNumber++;
                        double fundSum = 0;
                       
                        //合计
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 6));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 7, 9));
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(1);
                        cell.SetCellValue("合计");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(7);
                        cell.SetCellValue(fundSum);
                        cell.CellStyle = cellStyle4;
                        rowNumber++;
                    }

                    //项目实施进度计划
                    IList<ProjectDtlImplPlanModel> projectDtlImplPlans = this.ProjectDtlImplPlanFacade.FindByForeignKey(project.PhId).Data;
                    if(projectDtlImplPlans != null && projectDtlImplPlans.Count > 0)
                    {
                        //项目实施进度计划表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 5));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 7));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 9));
                        //项目实施进度计划表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目实施进度计划");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("项目实施内容");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(6);
                        cell.SetCellValue("开始时间");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(8);
                        cell.SetCellValue("完成时间");
                        cell.CellStyle = cellHeadStyle;
                        rowNumber++;

                        for (int j = 1; j <= projectDtlImplPlans.Count; j++)
                        {
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 5));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 7));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 9));
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(1);
                            cell.SetCellValue(projectDtlImplPlans[j - 1].FImplContent);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(6);
                            cell.SetCellValue(projectDtlImplPlans[j - 1].FStartDate == null ? "" : ((DateTime)projectDtlImplPlans[j - 1].FStartDate).ToString("yyyy-MM-dd"));
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(8);
                            cell.SetCellValue(projectDtlImplPlans[j - 1].FEndDate == null ? "" : ((DateTime)projectDtlImplPlans[j - 1].FEndDate).ToString("yyyy-MM-dd"));
                            cell.CellStyle = cellHeadStyle;
                            rowNumber++;
                        }
                    }
                    else
                    {
                        //项目实施进度计划表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 5));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 7));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 9));
                        //项目实施进度计划表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目实施进度计划");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("项目实施内容");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(6);
                        cell.SetCellValue("开始时间");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(8);
                        cell.SetCellValue("完成时间");
                        cell.CellStyle = cellHeadStyle;
                        rowNumber++;
                    }

                    //绩效目标
                    //绩效目标表合并单元格
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + 1, 0, 0));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                    //绩效目标表表头
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("绩效目标");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue("年度目标");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(5);
                    cell.SetCellValue("长期目标");
                    cell.CellStyle = cellHeadStyle;
                    rowNumber++;
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                    //绩效目标表表头
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(1);
                    cell.SetCellValue(projectDtlTextContent== null?"": projectDtlTextContent.FAnnualPerformGoal);
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(5);
                    cell.SetCellValue(projectDtlTextContent == null ? "" : projectDtlTextContent.FLTPerformGoal);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    #region
                    //IList<ProjectDtlPerformTargetModel> projectDtlPerformTargets = this.ProjectDtlPerformTargetFacade.FindByForeignKey(project.PhId).Data;
                    //if(projectDtlPerformTargets != null && projectDtlPerformTargets.Count > 0)
                    //{
                    //    //绩效目标表合并单元格
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count, 0, 0));
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                    //    //绩效目标表表头
                    //    row = sheet.CreateRow(rowNumber);
                    //    cell = row.CreateCell(0);
                    //    cell.SetCellValue("绩效目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    cell = row.CreateCell(1);
                    //    cell.SetCellValue("年度目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    cell = row.CreateCell(5);
                    //    cell.SetCellValue("长期目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    rowNumber++;

                    //}
                    //else
                    //{
                    //    //绩效目标表合并单元格
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count, 0, 0));
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                    //    //绩效目标表表头
                    //    row = sheet.CreateRow(rowNumber);
                    //    cell = row.CreateCell(0);
                    //    cell.SetCellValue("绩效目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    cell = row.CreateCell(1);
                    //    cell.SetCellValue("年度目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    cell = row.CreateCell(5);
                    //    cell.SetCellValue("长期目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    rowNumber++;
                    //}
                    #endregion

                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 9));
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("部门领导意见");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(projectDtlTextContent == null ? "" : projectDtlTextContent.FDeptOpinions);
                    cell.CellStyle = cellStyle2;

                    cell = row.CreateCell(5);
                    cell.SetCellValue("部门分管领导意见");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(6);
                    cell.SetCellValue(projectDtlTextContent == null ? "" : projectDtlTextContent.FDeptOpinions2);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber+1, 0, 0));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 4));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber + 1, rowNumber + 1, 2, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber + 2, rowNumber + 2, 1, 9));

                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("预算编审小组意见");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue("会议时间");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(2);
                    cell.SetCellValue(project.FMeetingTime == null ? "" : ((DateTime)project.FMeetingTime).ToString("yyyy-MM-dd"));
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(5);
                    cell.SetCellValue("会议纪要编号");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(6);
                    cell.SetCellValue(project.FMeetiingSummaryNo);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(1);
                    cell.SetCellValue("会议决议");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(2);
                    cell.SetCellValue("");
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("备注");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(projectDtlTextContent == null ? "" : projectDtlTextContent.FBz);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    count++;
                }
                
            }
                      
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            var buf = ms.ToArray();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\\DownLoadFiles\\YProjectMstSB";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            using (FileStream fs = File.OpenWrite(path + "\\" + filename))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
                fs.Close();
            }
            workbook = null;
            ms.Close();
            ms.Dispose();

            return JsonConvert.SerializeObject(new { path = "YProjectMstSB", filename = filename });
        }

        /// <summary>
        /// 立项申报表打印
        /// </summary>
        /// <param name="projectMsts">项目集合</param>
        /// <param name="qtCoverUpForOrg">套打格式</param>
        /// <param name="organize">组织</param>
        /// <param name="user">人员</param>
        /// <returns></returns>
        public string ExportDeclareExcel2(IList<ProjectMstModel> projectMsts, QtCoverUpForOrgModel qtCoverUpForOrg, OrganizeModel organize, User2Model user)
        {
            //行索引
            int rowNumber = 0;
            HSSFWorkbook workbook = new HSSFWorkbook();
            if (projectMsts != null && projectMsts.Count > 0)
            {
                var syssets = QTSysSetFacade.Find(t => t.PhId != 0).Data.ToList();
                int count = 0;
                foreach (var project in projectMsts)
                {
                    rowNumber = 0;
                    ISheet sheet;
                    if (count > 0)
                    {
                        sheet = workbook.CreateSheet("工会立项项目列表申报表" + count);
                    }
                    else
                    {
                        sheet = workbook.CreateSheet("工会立项项目列表申报表");
                    }
                    sheet.DefaultRowHeight = 18 * 20;
                    sheet.DefaultColumnWidth = 12;
                    sheet.SetColumnWidth(0, 6400);
                    sheet.SetColumnWidth(1, 4800);
                    sheet.SetColumnWidth(2, 4800);
                    sheet.SetColumnWidth(3, 4800);
                    sheet.SetColumnWidth(4, 4800);
                    sheet.SetColumnWidth(5, 4800);
                    sheet.SetColumnWidth(6, 4800);
                    sheet.SetColumnWidth(7, 4800);
                    sheet.SetColumnWidth(8, 4800);
                    sheet.SetColumnWidth(9, 4800);



                    //合并单元格
                    sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 5));
                    sheet.AddMergedRegion(new CellRangeAddress(1, 1, 7, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 1, 2));
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 4, 5));
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 7, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(3, 3, 1, 5));
                    sheet.AddMergedRegion(new CellRangeAddress(3, 3, 7, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(4, 4, 1, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(5, 5, 1, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(6, 6, 1, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(7, 7, 1, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(8, 8, 1, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(9, 9, 1, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(10, 11, 0, 0));
                    sheet.AddMergedRegion(new CellRangeAddress(10, 10, 1, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(11, 11, 1, 9));


                    //标题单元格样式
                    ICellStyle cellTitleStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 500, 14, false);
                    ICellStyle cellTitleStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                    ICellStyle cellTitleStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Right, VerticalAlignment.Center, 400, 12, false);
                    //表头单元格样式
                    ICellStyle cellHeadStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
                    //内容单元格样式
                    ICellStyle cellStyle1 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                    ICellStyle cellStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
                    ICellStyle cellStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                    //数字内容格式
                    ICellStyle cellStyle4 = ExcelHelper.CreateStyle2(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);

                    //标题
                    IRow row = sheet.CreateRow(rowNumber);
                    row.Height = 20 * 20;
                    ICell cell = row.CreateCell(0);
                    cell.SetCellValue(qtCoverUpForOrg.TempLateName + "工会立项项目申报表");
                    cell.CellStyle = cellTitleStyle;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(6);
                    cell.SetCellValue("单据号");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(7);
                    cell.SetCellValue(project.FBillNo);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;

                    //先获取项目可研
                    IList<ProjectDtlTextContentModel> projectDtlTextContents = new List<ProjectDtlTextContentModel>();
                    projectDtlTextContents = this.ProjectDtlTextContentFacade.FindByForeignKey(project.PhId).Data;
                    ProjectDtlTextContentModel projectDtlTextContent = new ProjectDtlTextContentModel();
                    if (projectDtlTextContents != null && projectDtlTextContents.Count > 0)
                    {
                        projectDtlTextContent = projectDtlTextContents[0];
                    }
                    else
                    {
                        projectDtlTextContent = null;
                    }


                    //表头
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("申报部门");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(project.FDeclarationDept_EXName);
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(3);
                    cell.SetCellValue("申报日期");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(4);
                    cell.SetCellValue(project.FDateofDeclaration == null ? "" : ((DateTime)project.FDateofDeclaration).ToString("yyyy-MM-dd"));
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(6);
                    cell.SetCellValue("填表人");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(7);
                    cell.SetCellValue(project.FDeclarer);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("项目名称");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(project.FProjName);
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(6);
                    cell.SetCellValue("项目编码");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(7);
                    cell.SetCellValue(project.FProjCode);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("项目属性");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(project.FProjAttr_EXName);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("存续期限");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(project.FDuration_EXName);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("支出类别");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(project.FExpenseCategory_EXName);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("项目起止时间");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(project.FStartDate == null ? "" : ((DateTime)project.FStartDate).ToString("yyyy-MM-dd") + "-" + project.FEndDate == null ? "" : ((DateTime)project.FEndDate).ToString("yyyy-MM-dd"));
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("部门职能概述");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(projectDtlTextContent == null ? "" : projectDtlTextContent.FFunctionalOverview);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("项目申报依据(需上传附件)");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(projectDtlTextContent == null ? "" : projectDtlTextContent.FProjBasis);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("项目可行性和必要性");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(projectDtlTextContent == null ? "" : projectDtlTextContent.FFeasibility);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(1);
                    cell.SetCellValue(projectDtlTextContent == null ? "" : projectDtlTextContent.FNecessity);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;

                    //项目预算明细
                    IList<ProjectDtlBudgetDtlModel> projectDtlBudgetDtls = this.ProjectDtlBudgetDtlFacade.FindByForeignKey(project.PhId).Data;
                    if (projectDtlBudgetDtls != null && projectDtlBudgetDtls.Count > 0)
                    {

                        //明细表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlBudgetDtls.Count + 1, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 3));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 9));
                        //明细表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目预算明细(万元)");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("序号");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(2);
                        cell.SetCellValue("明细项目名称");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(4);
                        cell.SetCellValue("金额");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(5);
                        cell.SetCellValue("支付方式");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(6);
                        cell.SetCellValue("测算过程或其他需要说明的事项");
                        cell.CellStyle = cellHeadStyle;
                        rowNumber++;
                        double dtlSum = 0;
                        for (int j = 1; j <= projectDtlBudgetDtls.Count; j++)
                        {
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 3));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 9));
                            //支付方式代码转名称
                            var syssetProjectDtlBudgetDtl = syssets.FindAll(x => x.DicType == "PayMethodTwo" && x.Orgcode == project.FDeclarationUnit && x.TypeCode == projectDtlBudgetDtls[j - 1].FPaymentMethod);
                            if (syssetProjectDtlBudgetDtl.Count > 0)
                            {
                                projectDtlBudgetDtls[j - 1].FPaymentMethod_EXName = syssetProjectDtlBudgetDtl[0].TypeName;
                            }
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(1);
                            cell.SetCellValue(j);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(2);
                            cell.SetCellValue(projectDtlBudgetDtls[j - 1].FName);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(4);
                            cell.SetCellValue((double)(projectDtlBudgetDtls[j - 1].FAmount / 10000));
                            cell.CellStyle = cellStyle4;
                            cell = row.CreateCell(5);
                            cell.SetCellValue(projectDtlBudgetDtls[j - 1].FPaymentMethod_EXName);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(6);
                            cell.SetCellValue(projectDtlBudgetDtls[j - 1].FOtherInstructions);
                            cell.CellStyle = cellHeadStyle;
                            rowNumber++;
                            dtlSum += (double)(projectDtlBudgetDtls[j - 1].FAmount / 10000);
                        }

                        //合计
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 3));
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(1);
                        cell.SetCellValue("合计");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(4);
                        cell.SetCellValue(dtlSum);
                        cell.CellStyle = cellStyle4;
                        rowNumber++;
                    }
                    else
                    {
                        //明细表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + 1, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 3));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 9));
                        //明细表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目预算明细(万元)");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("序号");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(2);
                        cell.SetCellValue("明细项目名称");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(4);
                        cell.SetCellValue("金额");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(5);
                        cell.SetCellValue("支付方式");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(6);
                        cell.SetCellValue("测算过程或其他需要说明的事项");
                        cell.CellStyle = cellHeadStyle;
                        rowNumber++;
                        double dtlSum = 0;

                        //合计
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 3));
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(1);
                        cell.SetCellValue("合计");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(4);
                        cell.SetCellValue(dtlSum);
                        cell.CellStyle = cellStyle4;
                        rowNumber++;
                    }

                    //项目资金申请
                    IList<ProjectDtlFundApplModel> projectDtlFundAppls = this.ProjectDtlFundApplFacade.FindByForeignKey(project.PhId).Data;
                    if (projectDtlFundAppls != null && projectDtlFundAppls.Count > 0)
                    {
                        //项目资金申请表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count + 1, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 6));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 7, 9));
                        //项目资金申请表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目资金申请(万元)");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("序号");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(2);
                        cell.SetCellValue("资金来源");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(7);
                        cell.SetCellValue("金额");
                        cell.CellStyle = cellHeadStyle;
                        rowNumber++;
                        double fundSum = 0;
                        for (int j = 1; j <= projectDtlFundAppls.Count; j++)
                        {
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 6));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 7, 9));
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(1);
                            cell.SetCellValue(j);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(2);
                            cell.SetCellValue(projectDtlFundAppls[j - 1].FSourceOfFunds_EXName);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(7);
                            cell.SetCellValue((double)(projectDtlFundAppls[j - 1].FAmount / 10000));
                            cell.CellStyle = cellStyle4;

                            rowNumber++;
                            fundSum += (double)(projectDtlFundAppls[j - 1].FAmount / 10000);
                        }

                        //合计
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 6));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 7, 9));
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(1);
                        cell.SetCellValue("合计");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(7);
                        cell.SetCellValue(fundSum);
                        cell.CellStyle = cellStyle4;
                        rowNumber++;
                    }
                    else
                    {
                        //项目资金申请表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + 1, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 6));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 7, 9));
                        //项目资金申请表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目资金申请(万元)");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("序号");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(2);
                        cell.SetCellValue("资金来源");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(7);
                        cell.SetCellValue("金额");
                        cell.CellStyle = cellStyle4;
                        rowNumber++;
                        double fundSum = 0;

                        //合计
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 6));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 7, 9));
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(1);
                        cell.SetCellValue("合计");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(7);
                        cell.SetCellValue(fundSum);
                        cell.CellStyle = cellStyle4;
                        rowNumber++;
                    }

                    //项目实施进度计划
                    IList<ProjectDtlImplPlanModel> projectDtlImplPlans = this.ProjectDtlImplPlanFacade.FindByForeignKey(project.PhId).Data;
                    if (projectDtlImplPlans != null && projectDtlImplPlans.Count > 0)
                    {
                        //项目实施进度计划表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 5));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 7));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 9));
                        //项目实施进度计划表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目实施进度计划");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("项目实施内容");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(6);
                        cell.SetCellValue("开始时间");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(8);
                        cell.SetCellValue("完成时间");
                        cell.CellStyle = cellHeadStyle;
                        rowNumber++;

                        for (int j = 1; j <= projectDtlImplPlans.Count; j++)
                        {
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 5));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 7));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 9));
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(1);
                            cell.SetCellValue(projectDtlImplPlans[j - 1].FImplContent);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(6);
                            cell.SetCellValue(projectDtlImplPlans[j - 1].FStartDate == null ? "" : ((DateTime)projectDtlImplPlans[j - 1].FStartDate).ToString("yyyy-MM-dd"));
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(8);
                            cell.SetCellValue(projectDtlImplPlans[j - 1].FEndDate == null ? "" : ((DateTime)projectDtlImplPlans[j - 1].FEndDate).ToString("yyyy-MM-dd"));
                            cell.CellStyle = cellHeadStyle;
                            rowNumber++;
                        }
                    }
                    else
                    {
                        //项目实施进度计划表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 5));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 7));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 9));
                        //项目实施进度计划表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目实施进度计划");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("项目实施内容");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(6);
                        cell.SetCellValue("开始时间");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(8);
                        cell.SetCellValue("完成时间");
                        cell.CellStyle = cellHeadStyle;
                        rowNumber++;
                    }

                    //绩效目标
                    //绩效目标表合并单元格
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + 1, 0, 0));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                    //绩效目标表表头
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("绩效目标");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue("年度目标");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(5);
                    cell.SetCellValue("长期目标");
                    cell.CellStyle = cellHeadStyle;
                    rowNumber++;
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                    //绩效目标表表头
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(1);
                    cell.SetCellValue(projectDtlTextContent == null ? "" : projectDtlTextContent.FAnnualPerformGoal);
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(5);
                    cell.SetCellValue(projectDtlTextContent == null ? "" : projectDtlTextContent.FLTPerformGoal);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    #region
                    //IList<ProjectDtlPerformTargetModel> projectDtlPerformTargets = this.ProjectDtlPerformTargetFacade.FindByForeignKey(project.PhId).Data;
                    //if(projectDtlPerformTargets != null && projectDtlPerformTargets.Count > 0)
                    //{
                    //    //绩效目标表合并单元格
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count, 0, 0));
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                    //    //绩效目标表表头
                    //    row = sheet.CreateRow(rowNumber);
                    //    cell = row.CreateCell(0);
                    //    cell.SetCellValue("绩效目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    cell = row.CreateCell(1);
                    //    cell.SetCellValue("年度目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    cell = row.CreateCell(5);
                    //    cell.SetCellValue("长期目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    rowNumber++;

                    //}
                    //else
                    //{
                    //    //绩效目标表合并单元格
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count, 0, 0));
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                    //    //绩效目标表表头
                    //    row = sheet.CreateRow(rowNumber);
                    //    cell = row.CreateCell(0);
                    //    cell.SetCellValue("绩效目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    cell = row.CreateCell(1);
                    //    cell.SetCellValue("年度目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    cell = row.CreateCell(5);
                    //    cell.SetCellValue("长期目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    rowNumber++;
                    //}
                    #endregion

                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 9));
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("部门领导意见");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(projectDtlTextContent == null ? "" : projectDtlTextContent.FDeptOpinions);
                    cell.CellStyle = cellStyle2;

                    cell = row.CreateCell(5);
                    cell.SetCellValue("部门分管领导意见");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(6);
                    cell.SetCellValue(projectDtlTextContent == null ? "" : projectDtlTextContent.FDeptOpinions2);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + 1, 0, 0));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 4));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber + 1, rowNumber + 1, 2, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber + 2, rowNumber + 2, 1, 9));

                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("预算编审小组意见");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue("会议时间");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(2);
                    cell.SetCellValue(project.FMeetingTime == null ? "" : ((DateTime)project.FMeetingTime).ToString("yyyy-MM-dd"));
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(5);
                    cell.SetCellValue("会议纪要编号");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(6);
                    cell.SetCellValue(project.FMeetiingSummaryNo);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(1);
                    cell.SetCellValue("会议决议");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(2);
                    cell.SetCellValue(GetResolutionName(projectDtlTextContent == null ? "" : projectDtlTextContent.FResolution));
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("备注");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(projectDtlTextContent == null ? "" : projectDtlTextContent.FBz);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    count++;
                }

            }

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            var buf = ms.ToArray();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\\DownLoadFiles\\ProjectMstSB";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            using (FileStream fs = File.OpenWrite(path + "\\" + filename))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
                fs.Close();
            }
            workbook = null;
            ms.Close();
            ms.Dispose();

            return JsonConvert.SerializeObject(new { path = "ProjectMstSB", filename = filename });
        }


        /// <summary>
        /// 导出年中新增申报表
        /// </summary>
        /// <param name="budgetMsts">年中新增集合</param>
        /// <param name="qtCoverUpForOrg">套打格式</param>
        /// <param name="organize">组织</param>
        /// <param name="user">人员</param>
        /// <returns></returns>
        public string ExportDeclareExcelXZ(IList<BudgetMstModel> budgetMsts, QtCoverUpForOrgModel qtCoverUpForOrg, OrganizeModel organize, User2Model user)
        {
            //行索引
            int rowNumber = 0;
            HSSFWorkbook workbook = new HSSFWorkbook();
            if (budgetMsts != null && budgetMsts.Count > 0)
            {
                var syssets = QTSysSetFacade.Find(t => t.PhId != 0).Data.ToList();
                int count = 0;
                foreach (var budget in budgetMsts)
                {
                    rowNumber = 0;
                    ISheet sheet;
                    if (count > 0)
                    {
                        sheet = workbook.CreateSheet("年中新增申报表" + count);
                    }
                    else
                    {
                        sheet = workbook.CreateSheet("年中新增申报表");
                    }
                    sheet.DefaultRowHeight = 18 * 20;
                    sheet.DefaultColumnWidth = 12;
                    sheet.SetColumnWidth(0, 6400);
                    sheet.SetColumnWidth(1, 4800);
                    sheet.SetColumnWidth(2, 4800);
                    sheet.SetColumnWidth(3, 4800);
                    sheet.SetColumnWidth(4, 4800);
                    sheet.SetColumnWidth(5, 4800);
                    sheet.SetColumnWidth(6, 4800);
                    sheet.SetColumnWidth(7, 4800);
                    sheet.SetColumnWidth(8, 4800);
                    sheet.SetColumnWidth(9, 4800);



                    //合并单元格
                    sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 5));
                    sheet.AddMergedRegion(new CellRangeAddress(1, 1, 7, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 1, 2));
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 4, 5));
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 7, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(3, 3, 1, 5));
                    sheet.AddMergedRegion(new CellRangeAddress(3, 3, 7, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(4, 4, 1, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(5, 5, 1, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(6, 6, 1, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(7, 7, 1, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(8, 8, 1, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(9, 9, 1, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(10, 11, 0, 0));
                    sheet.AddMergedRegion(new CellRangeAddress(10, 10, 1, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(11, 11, 1, 9));


                    //标题单元格样式
                    ICellStyle cellTitleStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 500, 14, false);
                    ICellStyle cellTitleStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                    ICellStyle cellTitleStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Right, VerticalAlignment.Center, 400, 12, false);
                    //表头单元格样式
                    ICellStyle cellHeadStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
                    //内容单元格样式
                    ICellStyle cellStyle1 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                    ICellStyle cellStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
                    ICellStyle cellStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);

                    //标题
                    IRow row = sheet.CreateRow(rowNumber);
                    row.Height = 20 * 20;
                    ICell cell = row.CreateCell(0);
                    cell.SetCellValue(qtCoverUpForOrg.TempLateName + "工会年中新增项目申报表");
                    cell.CellStyle = cellTitleStyle;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(6);
                    cell.SetCellValue("单据号");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(7);
                    cell.SetCellValue(budget.FBillNO);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;

                    //先获取项目可研
                    IList<BudgetDtlTextContentModel> budgetDtlTextContents = new List<BudgetDtlTextContentModel>();
                    budgetDtlTextContents = this.BudgetDtlTextContentFacade.FindByForeignKey(budget.PhId).Data;
                    BudgetDtlTextContentModel budgetDtlTextContent  = new BudgetDtlTextContentModel();
                    if (budgetDtlTextContents != null && budgetDtlTextContents.Count > 0)
                    {
                        budgetDtlTextContent = budgetDtlTextContents[0];
                    }
                    else
                    {
                        budgetDtlTextContent = null;
                    }


                    //表头
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("申报部门");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(budget.FDeclarationDept_EXName);
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(3);
                    cell.SetCellValue("申报日期");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(4);
                    cell.SetCellValue(budget.FDateofDeclaration.ToString());
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(6);
                    cell.SetCellValue("填表人");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(7);
                    cell.SetCellValue(budget.FDeclarer);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("项目名称");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(budget.FProjName);
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(6);
                    cell.SetCellValue("项目编码");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(7);
                    cell.SetCellValue(budget.FProjCode);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("项目属性");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(budget.FProjAttr_EXName);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("存续期限");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(budget.FDuration_EXName);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("支出类别");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(budget.FExpenseCategory_EXName);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("项目起止时间");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(budget.FStartDate.ToString() + "-" + budget.FEndDate.ToString());
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("部门职能概述");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FFunctionalOverview);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("项目申报依据(需上传附件)");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FProjBasis);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("项目可行性和必要性");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FFeasibility);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(1);
                    cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FNecessity);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;

                    //项目预算明细
                    IList<BudgetDtlBudgetDtlModel> projectDtlBudgetDtls = this.BudgetDtlBudgetDtlFacade.FindByForeignKey(budget.PhId).Data;
                    if (projectDtlBudgetDtls != null && projectDtlBudgetDtls.Count > 0)
                    {

                        //明细表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlBudgetDtls.Count + 1, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 3));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 9));
                        //明细表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目预算明细(万元)");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("序号");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(2);
                        cell.SetCellValue("明细项目名称");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(4);
                        cell.SetCellValue("金额");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(5);
                        cell.SetCellValue("支付方式");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(6);
                        cell.SetCellValue("测算过程或其他需要说明的事项");
                        cell.CellStyle = cellHeadStyle;
                        rowNumber++;
                        double dtlSum = 0;
                        for (int j = 1; j <= projectDtlBudgetDtls.Count; j++)
                        {
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 3));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 9));
                            //支付方式代码转名称
                            var syssetProjectDtlBudgetDtl = syssets.FindAll(x => x.DicType == "PayMethodTwo" && x.Orgcode == budget.FDeclarationUnit && x.TypeCode == projectDtlBudgetDtls[j - 1].FPaymentMethod);
                            if (syssetProjectDtlBudgetDtl.Count > 0)
                            {
                                projectDtlBudgetDtls[j - 1].FPaymentMethod_EXName = syssetProjectDtlBudgetDtl[0].TypeName;
                            }
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(1);
                            cell.SetCellValue(j);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(2);
                            cell.SetCellValue(projectDtlBudgetDtls[j - 1].FName);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(4);
                            cell.SetCellValue((double)(projectDtlBudgetDtls[j - 1].FAmount / 10000));
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(5);
                            cell.SetCellValue(projectDtlBudgetDtls[j - 1].FPaymentMethod_EXName);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(6);
                            cell.SetCellValue(projectDtlBudgetDtls[j - 1].FOtherInstructions);
                            cell.CellStyle = cellHeadStyle;
                            rowNumber++;
                            dtlSum += (double)(projectDtlBudgetDtls[j - 1].FAmount / 10000);
                        }

                        //合计
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 3));
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(1);
                        cell.SetCellValue("合计");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(4);
                        cell.SetCellValue(dtlSum);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                    }
                    else
                    {
                        //明细表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + 1, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 3));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 9));
                        //明细表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目预算明细(万元)");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("序号");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(2);
                        cell.SetCellValue("明细项目名称");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(4);
                        cell.SetCellValue("金额");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(5);
                        cell.SetCellValue("支付方式");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(6);
                        cell.SetCellValue("测算过程或其他需要说明的事项");
                        cell.CellStyle = cellHeadStyle;
                        rowNumber++;
                        double dtlSum = 0;

                        //合计
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 3));
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(1);
                        cell.SetCellValue("合计");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(4);
                        cell.SetCellValue(dtlSum);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                    }

                    //项目资金申请
                    IList<BudgetDtlFundApplModel> projectDtlFundAppls = this.BudgetDtlFundApplFacade.FindByForeignKey(budget.PhId).Data;
                    if (projectDtlFundAppls != null && projectDtlFundAppls.Count > 0)
                    {
                        //项目资金申请表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count + 1, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 6));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 7, 9));
                        //项目资金申请表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目资金申请(万元)");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("序号");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(2);
                        cell.SetCellValue("资金来源");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(7);
                        cell.SetCellValue("金额");
                        cell.CellStyle = cellHeadStyle;
                        rowNumber++;
                        double fundSum = 0;
                        for (int j = 1; j <= projectDtlFundAppls.Count; j++)
                        {
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 6));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 7, 9));
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(1);
                            cell.SetCellValue(j);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(2);
                            cell.SetCellValue(projectDtlFundAppls[j - 1].FSourceOfFunds_EXName);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(7);
                            cell.SetCellValue((double)(projectDtlFundAppls[j - 1].FAmount / 10000));
                            cell.CellStyle = cellHeadStyle;

                            rowNumber++;
                            fundSum += (double)(projectDtlFundAppls[j - 1].FAmount / 10000);
                        }

                        //合计
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 6));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 7, 9));
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(1);
                        cell.SetCellValue("合计");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(7);
                        cell.SetCellValue(fundSum);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                    }
                    else
                    {
                        //项目资金申请表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + 1, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 6));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 7, 9));
                        //项目资金申请表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目资金申请(万元)");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("序号");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(2);
                        cell.SetCellValue("资金来源");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(7);
                        cell.SetCellValue("金额");
                        cell.CellStyle = cellHeadStyle;
                        rowNumber++;
                        double fundSum = 0;

                        //合计
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 6));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 7, 9));
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(1);
                        cell.SetCellValue("合计");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(7);
                        cell.SetCellValue(fundSum);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                    }

                    //项目实施进度计划
                    IList<BudgetDtlImplPlanModel> projectDtlImplPlans = this.BudgetDtlImplPlanFacade.FindByForeignKey(budget.PhId).Data;
                    if (projectDtlImplPlans != null && projectDtlImplPlans.Count > 0)
                    {
                        //项目实施进度计划表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 5));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 7));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 9));
                        //项目实施进度计划表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目实施进度计划");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("项目实施内容");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(6);
                        cell.SetCellValue("开始时间");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(8);
                        cell.SetCellValue("完成时间");
                        cell.CellStyle = cellHeadStyle;
                        rowNumber++;

                        for (int j = 1; j <= projectDtlImplPlans.Count; j++)
                        {
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 5));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 7));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 9));
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(1);
                            cell.SetCellValue(projectDtlImplPlans[j - 1].FImplContent);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(6);
                            cell.SetCellValue(projectDtlImplPlans[j - 1].FStartDate.ToString());
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(8);
                            cell.SetCellValue(projectDtlImplPlans[j - 1].FEndDate.ToString());
                            cell.CellStyle = cellHeadStyle;
                            rowNumber++;
                        }
                    }
                    else
                    {
                        //项目实施进度计划表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 5));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 7));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 9));
                        //项目实施进度计划表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目实施进度计划");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("项目实施内容");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(6);
                        cell.SetCellValue("开始时间");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(8);
                        cell.SetCellValue("完成时间");
                        cell.CellStyle = cellHeadStyle;
                        rowNumber++;
                    }

                    //绩效目标
                    //绩效目标表合并单元格
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + 1, 0, 0));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                    //绩效目标表表头
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("绩效目标");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue("年度目标");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(5);
                    cell.SetCellValue("长期目标");
                    cell.CellStyle = cellHeadStyle;
                    rowNumber++;
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                    //绩效目标表表头
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(1);
                    cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FAnnualPerformGoal);
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(5);
                    cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FLTPerformGoal);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    #region
                    //IList<ProjectDtlPerformTargetModel> projectDtlPerformTargets = this.ProjectDtlPerformTargetFacade.FindByForeignKey(project.PhId).Data;
                    //if(projectDtlPerformTargets != null && projectDtlPerformTargets.Count > 0)
                    //{
                    //    //绩效目标表合并单元格
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count, 0, 0));
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                    //    //绩效目标表表头
                    //    row = sheet.CreateRow(rowNumber);
                    //    cell = row.CreateCell(0);
                    //    cell.SetCellValue("绩效目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    cell = row.CreateCell(1);
                    //    cell.SetCellValue("年度目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    cell = row.CreateCell(5);
                    //    cell.SetCellValue("长期目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    rowNumber++;

                    //}
                    //else
                    //{
                    //    //绩效目标表合并单元格
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count, 0, 0));
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                    //    //绩效目标表表头
                    //    row = sheet.CreateRow(rowNumber);
                    //    cell = row.CreateCell(0);
                    //    cell.SetCellValue("绩效目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    cell = row.CreateCell(1);
                    //    cell.SetCellValue("年度目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    cell = row.CreateCell(5);
                    //    cell.SetCellValue("长期目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    rowNumber++;
                    //}
                    #endregion

                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 9));
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("部门领导意见");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FDeptOpinions);
                    cell.CellStyle = cellStyle2;

                    cell = row.CreateCell(5);
                    cell.SetCellValue("部门分管领导意见");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(6);
                    cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FDeptOpinions2);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + 1, 0, 0));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 4));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber + 1, rowNumber + 1, 2, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber + 2, rowNumber + 2, 1, 9));

                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("预算编审小组意见");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue("会议时间");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(2);
                    cell.SetCellValue(budget.FMeetingTime.ToString());
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(5);
                    cell.SetCellValue("会议纪要编号");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(6);
                    cell.SetCellValue(budget.FMeetiingSummaryNo);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(1);
                    cell.SetCellValue("会议决议");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(2);
                    cell.SetCellValue(GetResolutionName(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FResolution));
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("备注");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FBz);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    count++;
                }

            }

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            var buf = ms.ToArray();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\\DownLoadFiles\\BudgetMstXZ";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            using (FileStream fs = File.OpenWrite(path + "\\" + filename))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
                fs.Close();
            }
            workbook = null;
            ms.Close();
            ms.Dispose();

            return JsonConvert.SerializeObject(new { path = "BudgetMstXZ", filename = filename });
        }

        /// <summary>
        /// 导出年中调整申报表
        /// </summary>
        /// <param name="budgetMsts">年中新增集合</param>
        /// <param name="qtCoverUpForOrg">套打格式</param>
        /// <param name="organize">组织</param>
        /// <param name="user">人员</param>
        /// <returns></returns>
        public string ExportDeclareExcelTZ(IList<BudgetMstModel> budgetMsts, QtCoverUpForOrgModel qtCoverUpForOrg, OrganizeModel organize, User2Model user)
        {
            //行索引
            int rowNumber = 0;
            HSSFWorkbook workbook = new HSSFWorkbook();
            if (budgetMsts != null && budgetMsts.Count > 0)
            {
                var syssets = QTSysSetFacade.Find(t => t.PhId != 0).Data.ToList();
                int count = 0;
                foreach (var budget in budgetMsts)
                {
                    rowNumber = 0;
                    ISheet sheet;
                    if (count > 0)
                    {
                        sheet = workbook.CreateSheet("年中调整申报表" + count);
                    }
                    else
                    {
                        sheet = workbook.CreateSheet("年中调整申报表");
                    }
                    sheet.DefaultRowHeight = 18 * 20;
                    sheet.DefaultColumnWidth = 12;
                    sheet.SetColumnWidth(0, 6400);
                    sheet.SetColumnWidth(1, 4800);
                    sheet.SetColumnWidth(2, 4800);
                    sheet.SetColumnWidth(3, 4800);
                    sheet.SetColumnWidth(4, 4800);
                    sheet.SetColumnWidth(5, 4800);
                    sheet.SetColumnWidth(6, 4800);
                    sheet.SetColumnWidth(7, 4800);
                    sheet.SetColumnWidth(8, 4800);
                    sheet.SetColumnWidth(9, 4800);
                    sheet.SetColumnWidth(10, 4800);
                    sheet.SetColumnWidth(11, 4800);
                    sheet.SetColumnWidth(12, 4800);


                    //合并单元格
                    sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 12));
                    sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 7));
                    sheet.AddMergedRegion(new CellRangeAddress(1, 1, 9, 12));
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 1, 3));
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 5, 7));
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 9, 12));
                    sheet.AddMergedRegion(new CellRangeAddress(3, 3, 1, 7));
                    sheet.AddMergedRegion(new CellRangeAddress(3, 3, 9, 12));
                    sheet.AddMergedRegion(new CellRangeAddress(4, 4, 1, 12));
                    //sheet.AddMergedRegion(new CellRangeAddress(5, 5, 1, 9));
                    //sheet.AddMergedRegion(new CellRangeAddress(6, 6, 1, 9));
                    //sheet.AddMergedRegion(new CellRangeAddress(7, 7, 1, 9));
                    //sheet.AddMergedRegion(new CellRangeAddress(8, 8, 1, 9));
                    //sheet.AddMergedRegion(new CellRangeAddress(9, 9, 1, 9));
                    //sheet.AddMergedRegion(new CellRangeAddress(10, 11, 0, 0));
                    //sheet.AddMergedRegion(new CellRangeAddress(10, 10, 1, 9));
                    //sheet.AddMergedRegion(new CellRangeAddress(11, 11, 1, 9));


                    //标题单元格样式
                    ICellStyle cellTitleStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 500, 14, false);
                    ICellStyle cellTitleStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                    ICellStyle cellTitleStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Right, VerticalAlignment.Center, 400, 12, false);
                    //表头单元格样式
                    ICellStyle cellHeadStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
                    //内容单元格样式
                    ICellStyle cellStyle1 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                    ICellStyle cellStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
                    ICellStyle cellStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);

                    //标题
                    IRow row = sheet.CreateRow(rowNumber);
                    row.Height = 20 * 20;
                    ICell cell = row.CreateCell(0);
                    cell.SetCellValue(qtCoverUpForOrg.TempLateName + "工会预算年中调整申报表");
                    cell.CellStyle = cellTitleStyle;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(8);
                    cell.SetCellValue("单据号");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(9);
                    cell.SetCellValue(budget.FBillNO);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("申报部门");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(budget.FDeclarationDept_EXName);
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(4);
                    cell.SetCellValue("申报日期");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(5);
                    cell.SetCellValue(budget.FDateofDeclaration.ToString());
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(8);
                    cell.SetCellValue("申报人");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(9);
                    cell.SetCellValue(budget.FDeclarer);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("项目名称");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(budget.FProjName);
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(8);
                    cell.SetCellValue("项目编码");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(9);
                    cell.SetCellValue(budget.FProjCode);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;

                    //先获取项目可研
                    IList<BudgetDtlTextContentModel> budgetDtlTextContents = new List<BudgetDtlTextContentModel>();
                    budgetDtlTextContents = this.BudgetDtlTextContentFacade.FindByForeignKey(budget.PhId).Data;
                    BudgetDtlTextContentModel budgetDtlTextContent = new BudgetDtlTextContentModel();
                    if (budgetDtlTextContents != null && budgetDtlTextContents.Count > 0)
                    {
                        budgetDtlTextContent = budgetDtlTextContents[0];
                    }
                    else
                    {
                        budgetDtlTextContent = null;
                    }


                    //表头                    
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("预算调整理由和依据(需要上传附件)");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FProjBasis);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;

                    //项目预算明细
                    IList<BudgetDtlBudgetDtlModel> projectDtlBudgetDtls = this.BudgetDtlBudgetDtlFacade.FindByForeignKey(budget.PhId).Data;
                    if (projectDtlBudgetDtls != null && projectDtlBudgetDtls.Count > 0)
                    {

                        //明细表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlBudgetDtls.Count + 1, 0, 0));

                        //明细表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目预算明细(万元)");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("序号");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(2);
                        cell.SetCellValue("明细项目名称");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(3);
                        cell.SetCellValue("会计科目");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(4);
                        cell.SetCellValue("资金来源");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(5);
                        cell.SetCellValue("功能科目");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(6);
                        cell.SetCellValue("支付方式");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(7);
                        cell.SetCellValue("支出渠道");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(8);
                        cell.SetCellValue("支出类别");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(9);
                        cell.SetCellValue("年初预算");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(10);
                        cell.SetCellValue("调整金额");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(11);
                        cell.SetCellValue("调整后项目预算金额");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(12);
                        cell.SetCellValue("年中调整测算说明");
                        cell.CellStyle = cellHeadStyle;
                        rowNumber++;
                        double dtlSum1 = 0, dtlSum2 = 0, dtlSum3 = 0;
                        for (int j = 1; j <= projectDtlBudgetDtls.Count; j++)
                        {
                            //支付方式代码转名称
                            var syssetProjectDtlBudgetDtl = syssets.FindAll(x => x.DicType == "PayMethodTwo" && x.Orgcode == budget.FDeclarationUnit && x.TypeCode == projectDtlBudgetDtls[j - 1].FPaymentMethod);
                            if (syssetProjectDtlBudgetDtl.Count > 0)
                            {
                                projectDtlBudgetDtls[j - 1].FPaymentMethod_EXName = syssetProjectDtlBudgetDtl[0].TypeName;
                            }
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(1);
                            cell.SetCellValue(j);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(2);
                            cell.SetCellValue(projectDtlBudgetDtls[j - 1].FName);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(3);
                            cell.SetCellValue(projectDtlBudgetDtls[j - 1].FBudgetAccounts_EXName);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(4);
                            cell.SetCellValue(projectDtlBudgetDtls[j - 1].FSourceOfFunds_EXName);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(5);
                            cell.SetCellValue(projectDtlBudgetDtls[j - 1].FQtZcgnfl_EXName);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(6);
                            cell.SetCellValue(projectDtlBudgetDtls[j - 1].FPaymentMethod_EXName);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(7);
                            cell.SetCellValue(projectDtlBudgetDtls[j - 1].FExpensesChannel_EXName);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(8);
                            cell.SetCellValue(budget.FExpenseCategory_EXName);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(9);
                            cell.SetCellValue((double)(projectDtlBudgetDtls[j - 1].FAmount / 10000));
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(10);
                            cell.SetCellValue((double)(projectDtlBudgetDtls[j - 1].FAmountEdit / 10000));
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(11);
                            cell.SetCellValue((double)((projectDtlBudgetDtls[j - 1].FAmountEdit + projectDtlBudgetDtls[j - 1].FAmount) / 10000));
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(12);
                            cell.SetCellValue(projectDtlBudgetDtls[j - 1].FOtherInstructions);
                            cell.CellStyle = cellHeadStyle;
                            rowNumber++;
                            dtlSum1 += (double)(projectDtlBudgetDtls[j - 1].FAmount / 10000);
                            dtlSum2 += (double)(projectDtlBudgetDtls[j - 1].FAmountEdit / 10000);
                            dtlSum3 += (double)((projectDtlBudgetDtls[j - 1].FAmountEdit + projectDtlBudgetDtls[j - 1].FAmount) / 10000);
                        }

                        //合计
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 8));
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(1);
                        cell.SetCellValue("合计");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(9);
                        cell.SetCellValue(dtlSum1);
                        cell.CellStyle = cellStyle2;
                        cell = row.CreateCell(10);
                        cell.SetCellValue(dtlSum2);
                        cell.CellStyle = cellStyle2;
                        cell = row.CreateCell(11);
                        cell.SetCellValue(dtlSum3);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                    }
                    else
                    {
                        //明细表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlBudgetDtls.Count + 1, 0, 0));

                        //明细表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目预算明细(万元)");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("序号");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(2);
                        cell.SetCellValue("明细项目名称");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(3);
                        cell.SetCellValue("会计科目");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(4);
                        cell.SetCellValue("资金来源");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(5);
                        cell.SetCellValue("功能科目");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(6);
                        cell.SetCellValue("支付方式");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(7);
                        cell.SetCellValue("支出渠道");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(8);
                        cell.SetCellValue("支出类别");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(9);
                        cell.SetCellValue("年初预算");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(10);
                        cell.SetCellValue("调整金额");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(11);
                        cell.SetCellValue("调整后项目预算金额");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(12);
                        cell.SetCellValue("年中调整测算说明");
                        cell.CellStyle = cellHeadStyle;
                        rowNumber++;

                        //合计
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 8));
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(1);
                        cell.SetCellValue("合计");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(9);
                        cell.SetCellValue("");
                        cell.CellStyle = cellStyle2;
                        cell = row.CreateCell(10);
                        cell.SetCellValue("");
                        cell.CellStyle = cellStyle2;
                        cell = row.CreateCell(11);
                        cell.SetCellValue("");
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                    }

                    //项目资金申请
                    IList<BudgetDtlFundApplModel> projectDtlFundAppls = this.BudgetDtlFundApplFacade.FindByForeignKey(budget.PhId).Data;
                    if (projectDtlFundAppls != null && projectDtlFundAppls.Count > 0)
                    {
                        //项目资金申请表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count + 1, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 7));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 12));
                        //项目资金申请表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目资金申请(万元)");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("序号");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(2);
                        cell.SetCellValue("资金来源");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(8);
                        cell.SetCellValue("金额");
                        cell.CellStyle = cellHeadStyle;
                        rowNumber++;
                        double fundSum = 0;
                        for (int j = 1; j <= projectDtlFundAppls.Count; j++)
                        {
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 7));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 12));
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(1);
                            cell.SetCellValue(j);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(2);
                            cell.SetCellValue(projectDtlFundAppls[j - 1].FSourceOfFunds_EXName);
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(8);
                            cell.SetCellValue((double)(projectDtlFundAppls[j - 1].FAmount / 10000));
                            cell.CellStyle = cellHeadStyle;

                            rowNumber++;
                            fundSum += (double)(projectDtlFundAppls[j - 1].FAmount / 10000);
                        }

                        //合计
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 7));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 12));
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(1);
                        cell.SetCellValue("资金总额");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(8);
                        cell.SetCellValue(fundSum);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                    }
                    else
                    {
                        //项目资金申请表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count + 1, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 7));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 12));
                        //项目资金申请表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目资金申请(万元)");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("序号");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(2);
                        cell.SetCellValue("资金来源");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(8);
                        cell.SetCellValue("金额");
                        cell.CellStyle = cellHeadStyle;
                        rowNumber++;
                        double fundSum = 0;

                        //合计
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 7));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 12));
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(1);
                        cell.SetCellValue("资金总额");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(8);
                        cell.SetCellValue(fundSum);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                    }

                    #region //项目实施进度计划
                    //项目实施进度计划
                    //IList<BudgetDtlImplPlanModel> projectDtlImplPlans = this.BudgetDtlImplPlanFacade.FindByForeignKey(budget.PhId).Data;
                    //if (projectDtlImplPlans != null && projectDtlImplPlans.Count > 0)
                    //{
                    //    //项目实施进度计划表合并单元格
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count, 0, 0));
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 5));
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 7));
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 9));
                    //    //项目实施进度计划表表头
                    //    row = sheet.CreateRow(rowNumber);
                    //    cell = row.CreateCell(0);
                    //    cell.SetCellValue("项目实施进度计划");
                    //    cell.CellStyle = cellHeadStyle;
                    //    cell = row.CreateCell(1);
                    //    cell.SetCellValue("项目实施内容");
                    //    cell.CellStyle = cellHeadStyle;
                    //    cell = row.CreateCell(6);
                    //    cell.SetCellValue("开始时间");
                    //    cell.CellStyle = cellHeadStyle;
                    //    cell = row.CreateCell(8);
                    //    cell.SetCellValue("完成时间");
                    //    cell.CellStyle = cellHeadStyle;
                    //    rowNumber++;

                    //    for (int j = 1; j <= projectDtlImplPlans.Count; j++)
                    //    {
                    //        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 5));
                    //        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 7));
                    //        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 9));
                    //        row = sheet.CreateRow(rowNumber);
                    //        cell = row.CreateCell(1);
                    //        cell.SetCellValue(projectDtlImplPlans[j - 1].FImplContent);
                    //        cell.CellStyle = cellHeadStyle;
                    //        cell = row.CreateCell(6);
                    //        cell.SetCellValue(projectDtlImplPlans[j - 1].FStartDate.ToString());
                    //        cell.CellStyle = cellHeadStyle;
                    //        cell = row.CreateCell(8);
                    //        cell.SetCellValue(projectDtlImplPlans[j - 1].FEndDate.ToString());
                    //        cell.CellStyle = cellHeadStyle;
                    //        rowNumber++;
                    //    }
                    //}
                    //else
                    //{
                    //    //项目实施进度计划表合并单元格
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 0, 0));
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 5));
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 7));
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 9));
                    //    //项目实施进度计划表表头
                    //    row = sheet.CreateRow(rowNumber);
                    //    cell = row.CreateCell(0);
                    //    cell.SetCellValue("项目实施进度计划");
                    //    cell.CellStyle = cellHeadStyle;
                    //    cell = row.CreateCell(1);
                    //    cell.SetCellValue("项目实施内容");
                    //    cell.CellStyle = cellHeadStyle;
                    //    cell = row.CreateCell(6);
                    //    cell.SetCellValue("开始时间");
                    //    cell.CellStyle = cellHeadStyle;
                    //    cell = row.CreateCell(8);
                    //    cell.SetCellValue("完成时间");
                    //    cell.CellStyle = cellHeadStyle;
                    //    rowNumber++;
                    //}
                    #endregion

                    #region//绩效目标
                    ////绩效目标
                    ////绩效目标表合并单元格
                    //sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + 1, 0, 0));
                    //sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                    //sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                    ////绩效目标表表头
                    //row = sheet.CreateRow(rowNumber);
                    //cell = row.CreateCell(0);
                    //cell.SetCellValue("绩效目标");
                    //cell.CellStyle = cellHeadStyle;
                    //cell = row.CreateCell(1);
                    //cell.SetCellValue("年度目标");
                    //cell.CellStyle = cellHeadStyle;
                    //cell = row.CreateCell(5);
                    //cell.SetCellValue("长期目标");
                    //cell.CellStyle = cellHeadStyle;
                    //rowNumber++;
                    //sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                    //sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                    ////绩效目标表表头
                    //row = sheet.CreateRow(rowNumber);
                    //cell = row.CreateCell(1);
                    //cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FAnnualPerformGoal);
                    //cell.CellStyle = cellStyle2;
                    //cell = row.CreateCell(5);
                    //cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FLTPerformGoal);
                    //cell.CellStyle = cellStyle2;
                    //rowNumber++;
                    #endregion

                    #region
                    //IList<ProjectDtlPerformTargetModel> projectDtlPerformTargets = this.ProjectDtlPerformTargetFacade.FindByForeignKey(project.PhId).Data;
                    //if(projectDtlPerformTargets != null && projectDtlPerformTargets.Count > 0)
                    //{
                    //    //绩效目标表合并单元格
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count, 0, 0));
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                    //    //绩效目标表表头
                    //    row = sheet.CreateRow(rowNumber);
                    //    cell = row.CreateCell(0);
                    //    cell.SetCellValue("绩效目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    cell = row.CreateCell(1);
                    //    cell.SetCellValue("年度目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    cell = row.CreateCell(5);
                    //    cell.SetCellValue("长期目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    rowNumber++;

                    //}
                    //else
                    //{
                    //    //绩效目标表合并单元格
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count, 0, 0));
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                    //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                    //    //绩效目标表表头
                    //    row = sheet.CreateRow(rowNumber);
                    //    cell = row.CreateCell(0);
                    //    cell.SetCellValue("绩效目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    cell = row.CreateCell(1);
                    //    cell.SetCellValue("年度目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    cell = row.CreateCell(5);
                    //    cell.SetCellValue("长期目标");
                    //    cell.CellStyle = cellHeadStyle;
                    //    rowNumber++;
                    //}
                    #endregion

                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 6));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 12));
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("部门领导意见");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FDeptOpinions);
                    cell.CellStyle = cellStyle2;

                    cell = row.CreateCell(7);
                    cell.SetCellValue("部门分管领导意见");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(8);
                    cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FDeptOpinions2);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + 1, 0, 0));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 6));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 12));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber + 1, rowNumber + 1, 2, 12));


                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("项目审核领导小组意见");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue("会议时间");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(2);
                    cell.SetCellValue(budget.FMeetingTime.ToString());
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(7);
                    cell.SetCellValue("会议纪要编号");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(8);
                    cell.SetCellValue(budget.FMeetiingSummaryNo);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(1);
                    cell.SetCellValue("会议决议");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(2);
                    cell.SetCellValue(GetResolutionName(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FResolution));
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 6));
                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 12));
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("党组会议意见");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FLeadingOpinions);
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(7);
                    cell.SetCellValue("主席办公会议意见");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(8);
                    cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FChairmanOpinions);
                    cell.CellStyle = cellStyle2;


                    rowNumber++;

                    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 12));
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("备注");
                    cell.CellStyle = cellHeadStyle;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FBz);
                    cell.CellStyle = cellStyle2;
                    rowNumber++;

                    count++;
                }

            }

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            var buf = ms.ToArray();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\\DownLoadFiles\\BudgetMstTZ";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            using (FileStream fs = File.OpenWrite(path + "\\" + filename))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
                fs.Close();
            }
            workbook = null;
            ms.Close();
            ms.Dispose();

            return JsonConvert.SerializeObject(new { path = "BudgetMstTZ", filename = filename });
        }


        /// <summary>
        /// 导出年中调整申报表(年中新增与年中调整放在同一个execl)
        /// </summary>
        /// <param name="budgetMsts">年中新增集合</param>
        /// <param name="qtCoverUpForOrg">套打格式</param>
        /// <param name="organize">组织</param>
        /// <param name="user">人员</param>
        /// <returns></returns>
        public string ExportDeclareExcelTZ2(IList<BudgetMstModel> budgetMsts, QtCoverUpForOrgModel qtCoverUpForOrg, OrganizeModel organize, User2Model user)
        {
            //行索引
            int rowNumber = 0;
            HSSFWorkbook workbook = new HSSFWorkbook();
            if (budgetMsts != null && budgetMsts.Count > 0)
            {
                var syssets = QTSysSetFacade.Find(t => t.PhId != 0).Data.ToList();
                int count = 0, count1 = 0;
                foreach (var budget in budgetMsts)
                {
                    rowNumber = 0;
                    ISheet sheet;
                    if(budget.FVerNo=="0001" && budget.FType == "z")
                    {
                        if (count1 > 0)
                        {
                            sheet = workbook.CreateSheet("年中新增申报表" + count);
                        }
                        else
                        {
                            sheet = workbook.CreateSheet("年中新增申报表");
                        }
                        sheet.DefaultRowHeight = 18 * 20;
                        sheet.DefaultColumnWidth = 12;
                        sheet.SetColumnWidth(0, 6400);
                        sheet.SetColumnWidth(1, 4800);
                        sheet.SetColumnWidth(2, 4800);
                        sheet.SetColumnWidth(3, 4800);
                        sheet.SetColumnWidth(4, 4800);
                        sheet.SetColumnWidth(5, 4800);
                        sheet.SetColumnWidth(6, 4800);
                        sheet.SetColumnWidth(7, 4800);
                        sheet.SetColumnWidth(8, 4800);
                        sheet.SetColumnWidth(9, 4800);



                        //合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 9));
                        sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 5));
                        sheet.AddMergedRegion(new CellRangeAddress(1, 1, 7, 9));
                        sheet.AddMergedRegion(new CellRangeAddress(2, 2, 1, 2));
                        sheet.AddMergedRegion(new CellRangeAddress(2, 2, 4, 5));
                        sheet.AddMergedRegion(new CellRangeAddress(2, 2, 7, 9));
                        sheet.AddMergedRegion(new CellRangeAddress(3, 3, 1, 5));
                        sheet.AddMergedRegion(new CellRangeAddress(3, 3, 7, 9));
                        sheet.AddMergedRegion(new CellRangeAddress(4, 4, 1, 9));
                        sheet.AddMergedRegion(new CellRangeAddress(5, 5, 1, 9));
                        sheet.AddMergedRegion(new CellRangeAddress(6, 6, 1, 9));
                        sheet.AddMergedRegion(new CellRangeAddress(7, 7, 1, 9));
                        sheet.AddMergedRegion(new CellRangeAddress(8, 8, 1, 9));
                        sheet.AddMergedRegion(new CellRangeAddress(9, 9, 1, 9));
                        sheet.AddMergedRegion(new CellRangeAddress(10, 11, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(10, 10, 1, 9));
                        sheet.AddMergedRegion(new CellRangeAddress(11, 11, 1, 9));


                        //标题单元格样式
                        ICellStyle cellTitleStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 500, 14, false);
                        ICellStyle cellTitleStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                        ICellStyle cellTitleStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Right, VerticalAlignment.Center, 400, 12, false);
                        //表头单元格样式
                        ICellStyle cellHeadStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
                        //内容单元格样式
                        ICellStyle cellStyle1 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                        ICellStyle cellStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
                        ICellStyle cellStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                        //数字内容格式
                        ICellStyle cellStyle4 = ExcelHelper.CreateStyle2(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);

                        //标题
                        IRow row = sheet.CreateRow(rowNumber);
                        row.Height = 20 * 20;
                        ICell cell = row.CreateCell(0);
                        cell.SetCellValue(qtCoverUpForOrg.TempLateName + "工会年中新增项目申报表");
                        cell.CellStyle = cellTitleStyle;
                        rowNumber++;
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(6);
                        cell.SetCellValue("单据号");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(7);
                        cell.SetCellValue(budget.FBillNO);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;

                        //先获取项目可研
                        IList<BudgetDtlTextContentModel> budgetDtlTextContents = new List<BudgetDtlTextContentModel>();
                        budgetDtlTextContents = this.BudgetDtlTextContentFacade.FindByForeignKey(budget.PhId).Data;
                        BudgetDtlTextContentModel budgetDtlTextContent = new BudgetDtlTextContentModel();
                        if (budgetDtlTextContents != null && budgetDtlTextContents.Count > 0)
                        {
                            budgetDtlTextContent = budgetDtlTextContents[0];
                        }
                        else
                        {
                            budgetDtlTextContent = null;
                        }


                        //表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("申报部门");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue(budget.FDeclarationDept_EXName);
                        cell.CellStyle = cellStyle2;
                        cell = row.CreateCell(3);
                        cell.SetCellValue("申报日期");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(4);
                        cell.SetCellValue(budget.FDateofDeclaration == null ? "" : ((DateTime)budget.FDateofDeclaration).ToString("yyyy-MM-dd"));
                        cell.CellStyle = cellStyle2;
                        cell = row.CreateCell(6);
                        cell.SetCellValue("填表人");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(7);
                        cell.SetCellValue(budget.FDeclarer);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目名称");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue(budget.FProjName);
                        cell.CellStyle = cellStyle2;
                        cell = row.CreateCell(6);
                        cell.SetCellValue("项目编码");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(7);
                        cell.SetCellValue(budget.FProjCode);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目属性");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue(budget.FProjAttr_EXName);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("存续期限");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue(budget.FDuration_EXName);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("支出类别");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue(budget.FExpenseCategory_EXName);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目起止时间");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue(budget.FStartDate == null ? "" : ((DateTime)budget.FStartDate).ToString("yyyy-MM-dd") + "-" + budget.FEndDate == null ? "" : ((DateTime)budget.FEndDate).ToString("yyyy-MM-dd"));
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("部门职能概述");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FFunctionalOverview);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目申报依据(需上传附件)");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FProjBasis);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目可行性和必要性");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FFeasibility);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(1);
                        cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FNecessity);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;

                        //项目预算明细
                        IList<BudgetDtlBudgetDtlModel> projectDtlBudgetDtls = this.BudgetDtlBudgetDtlFacade.FindByForeignKey(budget.PhId).Data;
                        if (projectDtlBudgetDtls != null && projectDtlBudgetDtls.Count > 0)
                        {

                            //明细表合并单元格
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlBudgetDtls.Count + 1, 0, 0));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 3));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 9));
                            //明细表表头
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(0);
                            cell.SetCellValue("项目预算明细(万元)");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(1);
                            cell.SetCellValue("序号");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(2);
                            cell.SetCellValue("明细项目名称");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(4);
                            cell.SetCellValue("金额");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(5);
                            cell.SetCellValue("支付方式");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(6);
                            cell.SetCellValue("测算过程或其他需要说明的事项");
                            cell.CellStyle = cellHeadStyle;
                            rowNumber++;
                            double dtlSum = 0;
                            for (int j = 1; j <= projectDtlBudgetDtls.Count; j++)
                            {
                                sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 3));
                                sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 9));
                                //支付方式代码转名称
                                var syssetProjectDtlBudgetDtl = syssets.FindAll(x => x.DicType == "PayMethodTwo" && x.Orgcode == budget.FDeclarationUnit && x.TypeCode == projectDtlBudgetDtls[j - 1].FPaymentMethod);
                                if (syssetProjectDtlBudgetDtl.Count > 0)
                                {
                                    projectDtlBudgetDtls[j - 1].FPaymentMethod_EXName = syssetProjectDtlBudgetDtl[0].TypeName;
                                }
                                row = sheet.CreateRow(rowNumber);
                                cell = row.CreateCell(1);
                                cell.SetCellValue(j);
                                cell.CellStyle = cellHeadStyle;
                                cell = row.CreateCell(2);
                                cell.SetCellValue(projectDtlBudgetDtls[j - 1].FName);
                                cell.CellStyle = cellHeadStyle;
                                cell = row.CreateCell(4);
                                cell.SetCellValue((double)(projectDtlBudgetDtls[j - 1].FAmount / 10000));
                                cell.CellStyle = cellStyle4;
                                cell = row.CreateCell(5);
                                cell.SetCellValue(projectDtlBudgetDtls[j - 1].FPaymentMethod_EXName);
                                cell.CellStyle = cellHeadStyle;
                                cell = row.CreateCell(6);
                                cell.SetCellValue(projectDtlBudgetDtls[j - 1].FOtherInstructions);
                                cell.CellStyle = cellHeadStyle;
                                rowNumber++;
                                dtlSum += (double)(projectDtlBudgetDtls[j - 1].FAmount / 10000);
                            }

                            //合计
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 3));
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(1);
                            cell.SetCellValue("合计");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(4);
                            cell.SetCellValue(dtlSum);
                            cell.CellStyle = cellStyle4;
                            rowNumber++;
                        }
                        else
                        {
                            //明细表合并单元格
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + 1, 0, 0));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 3));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 9));
                            //明细表表头
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(0);
                            cell.SetCellValue("项目预算明细(万元)");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(1);
                            cell.SetCellValue("序号");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(2);
                            cell.SetCellValue("明细项目名称");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(4);
                            cell.SetCellValue("金额");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(5);
                            cell.SetCellValue("支付方式");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(6);
                            cell.SetCellValue("测算过程或其他需要说明的事项");
                            cell.CellStyle = cellHeadStyle;
                            rowNumber++;
                            double dtlSum = 0;

                            //合计
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 3));
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(1);
                            cell.SetCellValue("合计");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(4);
                            cell.SetCellValue(dtlSum);
                            cell.CellStyle = cellStyle4;
                            rowNumber++;
                        }

                        //项目资金申请
                        IList<BudgetDtlFundApplModel> projectDtlFundAppls = this.BudgetDtlFundApplFacade.FindByForeignKey(budget.PhId).Data;
                        if (projectDtlFundAppls != null && projectDtlFundAppls.Count > 0)
                        {
                            //项目资金申请表合并单元格
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count + 1, 0, 0));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 6));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 7, 9));
                            //项目资金申请表表头
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(0);
                            cell.SetCellValue("项目资金申请(万元)");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(1);
                            cell.SetCellValue("序号");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(2);
                            cell.SetCellValue("资金来源");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(7);
                            cell.SetCellValue("金额");
                            cell.CellStyle = cellHeadStyle;
                            rowNumber++;
                            double fundSum = 0;
                            for (int j = 1; j <= projectDtlFundAppls.Count; j++)
                            {
                                sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 6));
                                sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 7, 9));
                                row = sheet.CreateRow(rowNumber);
                                cell = row.CreateCell(1);
                                cell.SetCellValue(j);
                                cell.CellStyle = cellHeadStyle;
                                cell = row.CreateCell(2);
                                cell.SetCellValue(projectDtlFundAppls[j - 1].FSourceOfFunds_EXName);
                                cell.CellStyle = cellHeadStyle;
                                cell = row.CreateCell(7);
                                cell.SetCellValue((double)(projectDtlFundAppls[j - 1].FAmount / 10000));
                                cell.CellStyle = cellStyle4;

                                rowNumber++;
                                fundSum += (double)(projectDtlFundAppls[j - 1].FAmount / 10000);
                            }

                            //合计
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 6));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 7, 9));
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(1);
                            cell.SetCellValue("合计");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(7);
                            cell.SetCellValue(fundSum);
                            cell.CellStyle = cellStyle4;
                            rowNumber++;
                        }
                        else
                        {
                            //项目资金申请表合并单元格
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + 1, 0, 0));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 6));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 7, 9));
                            //项目资金申请表表头
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(0);
                            cell.SetCellValue("项目资金申请(万元)");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(1);
                            cell.SetCellValue("序号");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(2);
                            cell.SetCellValue("资金来源");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(7);
                            cell.SetCellValue("金额");
                            cell.CellStyle = cellHeadStyle;
                            rowNumber++;
                            double fundSum = 0;

                            //合计
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 6));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 7, 9));
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(1);
                            cell.SetCellValue("合计");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(7);
                            cell.SetCellValue(fundSum);
                            cell.CellStyle = cellStyle4;
                            rowNumber++;
                        }

                        //项目实施进度计划
                        IList<BudgetDtlImplPlanModel> projectDtlImplPlans = this.BudgetDtlImplPlanFacade.FindByForeignKey(budget.PhId).Data;
                        if (projectDtlImplPlans != null && projectDtlImplPlans.Count > 0)
                        {
                            //项目实施进度计划表合并单元格
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count, 0, 0));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 5));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 7));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 9));
                            //项目实施进度计划表表头
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(0);
                            cell.SetCellValue("项目实施进度计划");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(1);
                            cell.SetCellValue("项目实施内容");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(6);
                            cell.SetCellValue("开始时间");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(8);
                            cell.SetCellValue("完成时间");
                            cell.CellStyle = cellHeadStyle;
                            rowNumber++;

                            for (int j = 1; j <= projectDtlImplPlans.Count; j++)
                            {
                                sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 5));
                                sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 7));
                                sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 9));
                                row = sheet.CreateRow(rowNumber);
                                cell = row.CreateCell(1);
                                cell.SetCellValue(projectDtlImplPlans[j - 1].FImplContent);
                                cell.CellStyle = cellHeadStyle;
                                cell = row.CreateCell(6);
                                cell.SetCellValue(projectDtlImplPlans[j - 1].FStartDate == null ? "" : ((DateTime)projectDtlImplPlans[j - 1].FStartDate).ToString("yyyy-MM-dd"));
                                cell.CellStyle = cellHeadStyle;
                                cell = row.CreateCell(8);
                                cell.SetCellValue(projectDtlImplPlans[j - 1].FEndDate == null ? "" : ((DateTime)projectDtlImplPlans[j - 1].FEndDate).ToString("yyyy-MM-dd"));
                                cell.CellStyle = cellHeadStyle;
                                rowNumber++;
                            }
                        }
                        else
                        {
                            //项目实施进度计划表合并单元格
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 0, 0));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 5));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 7));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 9));
                            //项目实施进度计划表表头
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(0);
                            cell.SetCellValue("项目实施进度计划");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(1);
                            cell.SetCellValue("项目实施内容");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(6);
                            cell.SetCellValue("开始时间");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(8);
                            cell.SetCellValue("完成时间");
                            cell.CellStyle = cellHeadStyle;
                            rowNumber++;
                        }

                        //绩效目标
                        //绩效目标表合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + 1, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                        //绩效目标表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("绩效目标");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("年度目标");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(5);
                        cell.SetCellValue("长期目标");
                        cell.CellStyle = cellHeadStyle;
                        rowNumber++;
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                        //绩效目标表表头
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(1);
                        cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FAnnualPerformGoal);
                        cell.CellStyle = cellStyle2;
                        cell = row.CreateCell(5);
                        cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FLTPerformGoal);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                        #region
                        //IList<ProjectDtlPerformTargetModel> projectDtlPerformTargets = this.ProjectDtlPerformTargetFacade.FindByForeignKey(project.PhId).Data;
                        //if(projectDtlPerformTargets != null && projectDtlPerformTargets.Count > 0)
                        //{
                        //    //绩效目标表合并单元格
                        //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count, 0, 0));
                        //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                        //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                        //    //绩效目标表表头
                        //    row = sheet.CreateRow(rowNumber);
                        //    cell = row.CreateCell(0);
                        //    cell.SetCellValue("绩效目标");
                        //    cell.CellStyle = cellHeadStyle;
                        //    cell = row.CreateCell(1);
                        //    cell.SetCellValue("年度目标");
                        //    cell.CellStyle = cellHeadStyle;
                        //    cell = row.CreateCell(5);
                        //    cell.SetCellValue("长期目标");
                        //    cell.CellStyle = cellHeadStyle;
                        //    rowNumber++;

                        //}
                        //else
                        //{
                        //    //绩效目标表合并单元格
                        //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count, 0, 0));
                        //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                        //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                        //    //绩效目标表表头
                        //    row = sheet.CreateRow(rowNumber);
                        //    cell = row.CreateCell(0);
                        //    cell.SetCellValue("绩效目标");
                        //    cell.CellStyle = cellHeadStyle;
                        //    cell = row.CreateCell(1);
                        //    cell.SetCellValue("年度目标");
                        //    cell.CellStyle = cellHeadStyle;
                        //    cell = row.CreateCell(5);
                        //    cell.SetCellValue("长期目标");
                        //    cell.CellStyle = cellHeadStyle;
                        //    rowNumber++;
                        //}
                        #endregion

                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 9));
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("部门领导意见");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FDeptOpinions);
                        cell.CellStyle = cellStyle2;

                        cell = row.CreateCell(5);
                        cell.SetCellValue("部门分管领导意见");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(6);
                        cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FDeptOpinions2);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + 1, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 4));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 9));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber + 1, rowNumber + 1, 2, 9));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber + 2, rowNumber + 2, 1, 9));

                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("预算编审小组意见");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("会议时间");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(2);
                        cell.SetCellValue(budget.FMeetingTime == null ? "" : ((DateTime)budget.FMeetingTime).ToString("yyyy-MM-dd"));
                        cell.CellStyle = cellStyle2;
                        cell = row.CreateCell(5);
                        cell.SetCellValue("会议纪要编号");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(6);
                        cell.SetCellValue(budget.FMeetiingSummaryNo);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(1);
                        cell.SetCellValue("会议决议");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(2);
                        cell.SetCellValue(GetResolutionName(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FResolution));
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("备注");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FBz);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                        count1++;
                    }
                    else
                    {
                        if (count > 0)
                        {
                            sheet = workbook.CreateSheet("年中调整申报表" + count);
                        }
                        else
                        {
                            sheet = workbook.CreateSheet("年中调整申报表");
                        }
                        sheet.DefaultRowHeight = 18 * 20;
                        sheet.DefaultColumnWidth = 12;
                        sheet.SetColumnWidth(0, 6400);
                        sheet.SetColumnWidth(1, 4800);
                        sheet.SetColumnWidth(2, 4800);
                        sheet.SetColumnWidth(3, 4800);
                        sheet.SetColumnWidth(4, 4800);
                        sheet.SetColumnWidth(5, 4800);
                        sheet.SetColumnWidth(6, 4800);
                        sheet.SetColumnWidth(7, 4800);
                        sheet.SetColumnWidth(8, 4800);
                        sheet.SetColumnWidth(9, 4800);
                        sheet.SetColumnWidth(10, 4800);
                        sheet.SetColumnWidth(11, 4800);
                        sheet.SetColumnWidth(12, 4800);


                        //合并单元格
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 12));
                        sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 7));
                        sheet.AddMergedRegion(new CellRangeAddress(1, 1, 9, 12));
                        sheet.AddMergedRegion(new CellRangeAddress(2, 2, 1, 3));
                        sheet.AddMergedRegion(new CellRangeAddress(2, 2, 5, 7));
                        sheet.AddMergedRegion(new CellRangeAddress(2, 2, 9, 12));
                        sheet.AddMergedRegion(new CellRangeAddress(3, 3, 1, 7));
                        sheet.AddMergedRegion(new CellRangeAddress(3, 3, 9, 12));
                        sheet.AddMergedRegion(new CellRangeAddress(4, 4, 1, 12));
                        //sheet.AddMergedRegion(new CellRangeAddress(5, 5, 1, 9));
                        //sheet.AddMergedRegion(new CellRangeAddress(6, 6, 1, 9));
                        //sheet.AddMergedRegion(new CellRangeAddress(7, 7, 1, 9));
                        //sheet.AddMergedRegion(new CellRangeAddress(8, 8, 1, 9));
                        //sheet.AddMergedRegion(new CellRangeAddress(9, 9, 1, 9));
                        //sheet.AddMergedRegion(new CellRangeAddress(10, 11, 0, 0));
                        //sheet.AddMergedRegion(new CellRangeAddress(10, 10, 1, 9));
                        //sheet.AddMergedRegion(new CellRangeAddress(11, 11, 1, 9));


                        //标题单元格样式
                        ICellStyle cellTitleStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 500, 14, false);
                        ICellStyle cellTitleStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                        ICellStyle cellTitleStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Right, VerticalAlignment.Center, 400, 12, false);
                        //表头单元格样式
                        ICellStyle cellHeadStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
                        //内容单元格样式
                        ICellStyle cellStyle1 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                        ICellStyle cellStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
                        ICellStyle cellStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                        //数字内容格式
                        ICellStyle cellStyle4 = ExcelHelper.CreateStyle2(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);

                        //标题
                        IRow row = sheet.CreateRow(rowNumber);
                        row.Height = 20 * 20;
                        ICell cell = row.CreateCell(0);
                        cell.SetCellValue(qtCoverUpForOrg.TempLateName + "工会预算年中调整申报表");
                        cell.CellStyle = cellTitleStyle;
                        rowNumber++;
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(8);
                        cell.SetCellValue("单据号");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(9);
                        cell.SetCellValue(budget.FBillNO);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("申报部门");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue(budget.FDeclarationDept_EXName);
                        cell.CellStyle = cellStyle2;
                        cell = row.CreateCell(4);
                        cell.SetCellValue("申报日期");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(5);
                        cell.SetCellValue(budget.FDateofDeclaration == null ? "" : ((DateTime)budget.FDateofDeclaration).ToString("yyyy-MM-dd"));
                        cell.CellStyle = cellStyle2;
                        cell = row.CreateCell(8);
                        cell.SetCellValue("申报人");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(9);
                        cell.SetCellValue(budget.FDeclarer);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目名称");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue(budget.FProjName);
                        cell.CellStyle = cellStyle2;
                        cell = row.CreateCell(8);
                        cell.SetCellValue("项目编码");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(9);
                        cell.SetCellValue(budget.FProjCode);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;

                        //先获取项目可研
                        IList<BudgetDtlTextContentModel> budgetDtlTextContents = new List<BudgetDtlTextContentModel>();
                        budgetDtlTextContents = this.BudgetDtlTextContentFacade.FindByForeignKey(budget.PhId).Data;
                        BudgetDtlTextContentModel budgetDtlTextContent = new BudgetDtlTextContentModel();
                        if (budgetDtlTextContents != null && budgetDtlTextContents.Count > 0)
                        {
                            budgetDtlTextContent = budgetDtlTextContents[0];
                        }
                        else
                        {
                            budgetDtlTextContent = null;
                        }


                        //表头                    
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("预算调整理由和依据(需要上传附件)");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FProjBasis);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;

                        //项目预算明细
                        IList<BudgetDtlBudgetDtlModel> projectDtlBudgetDtls = this.BudgetDtlBudgetDtlFacade.FindByForeignKey(budget.PhId).Data;
                        if (projectDtlBudgetDtls != null && projectDtlBudgetDtls.Count > 0)
                        {

                            //明细表合并单元格
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlBudgetDtls.Count + 1, 0, 0));

                            //明细表表头
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(0);
                            cell.SetCellValue("项目预算明细(万元)");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(1);
                            cell.SetCellValue("序号");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(2);
                            cell.SetCellValue("明细项目名称");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(3);
                            cell.SetCellValue("会计科目");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(4);
                            cell.SetCellValue("资金来源");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(5);
                            cell.SetCellValue("功能科目");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(6);
                            cell.SetCellValue("支付方式");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(7);
                            cell.SetCellValue("支出渠道");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(8);
                            cell.SetCellValue("支出类别");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(9);
                            cell.SetCellValue("年初预算");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(10);
                            cell.SetCellValue("调整金额");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(11);
                            cell.SetCellValue("调整后项目预算金额");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(12);
                            cell.SetCellValue("年中调整测算说明");
                            cell.CellStyle = cellHeadStyle;
                            rowNumber++;
                            double dtlSum1 = 0, dtlSum2 = 0, dtlSum3 = 0;
                            for (int j = 1; j <= projectDtlBudgetDtls.Count; j++)
                            {
                                //支付方式代码转名称
                                var syssetProjectDtlBudgetDtl = syssets.FindAll(x => x.DicType == "PayMethodTwo" && x.Orgcode == budget.FDeclarationUnit && x.TypeCode == projectDtlBudgetDtls[j - 1].FPaymentMethod);
                                if (syssetProjectDtlBudgetDtl.Count > 0)
                                {
                                    projectDtlBudgetDtls[j - 1].FPaymentMethod_EXName = syssetProjectDtlBudgetDtl[0].TypeName;
                                }
                                row = sheet.CreateRow(rowNumber);
                                cell = row.CreateCell(1);
                                cell.SetCellValue(j);
                                cell.CellStyle = cellHeadStyle;
                                cell = row.CreateCell(2);
                                cell.SetCellValue(projectDtlBudgetDtls[j - 1].FName);
                                cell.CellStyle = cellHeadStyle;
                                cell = row.CreateCell(3);
                                cell.SetCellValue(projectDtlBudgetDtls[j - 1].FBudgetAccounts_EXName);
                                cell.CellStyle = cellHeadStyle;
                                cell = row.CreateCell(4);
                                cell.SetCellValue(projectDtlBudgetDtls[j - 1].FSourceOfFunds_EXName);
                                cell.CellStyle = cellHeadStyle;
                                cell = row.CreateCell(5);
                                cell.SetCellValue(projectDtlBudgetDtls[j - 1].FQtZcgnfl_EXName);
                                cell.CellStyle = cellHeadStyle;
                                cell = row.CreateCell(6);
                                cell.SetCellValue(projectDtlBudgetDtls[j - 1].FPaymentMethod_EXName);
                                cell.CellStyle = cellHeadStyle;
                                cell = row.CreateCell(7);
                                cell.SetCellValue(projectDtlBudgetDtls[j - 1].FExpensesChannel_EXName);
                                cell.CellStyle = cellHeadStyle;
                                cell = row.CreateCell(8);
                                cell.SetCellValue(budget.FExpenseCategory_EXName);
                                cell.CellStyle = cellHeadStyle;
                                cell = row.CreateCell(9);
                                cell.SetCellValue((double)(projectDtlBudgetDtls[j - 1].FAmount / 10000));
                                cell.CellStyle = cellStyle4;
                                cell = row.CreateCell(10);
                                cell.SetCellValue((double)(projectDtlBudgetDtls[j - 1].FAmountEdit / 10000));
                                cell.CellStyle = cellStyle4;
                                cell = row.CreateCell(11);
                                cell.SetCellValue((double)((projectDtlBudgetDtls[j - 1].FAmountEdit + projectDtlBudgetDtls[j - 1].FAmount) / 10000));
                                cell.CellStyle = cellStyle4;
                                cell = row.CreateCell(12);
                                cell.SetCellValue(projectDtlBudgetDtls[j - 1].FOtherInstructions);
                                cell.CellStyle = cellHeadStyle;
                                rowNumber++;
                                dtlSum1 += (double)(projectDtlBudgetDtls[j - 1].FAmount / 10000);
                                dtlSum2 += (double)(projectDtlBudgetDtls[j - 1].FAmountEdit / 10000);
                                dtlSum3 += (double)((projectDtlBudgetDtls[j - 1].FAmountEdit + projectDtlBudgetDtls[j - 1].FAmount) / 10000);
                            }

                            //合计
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 8));
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(1);
                            cell.SetCellValue("合计");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(9);
                            cell.SetCellValue(dtlSum1);
                            cell.CellStyle = cellStyle4;
                            cell = row.CreateCell(10);
                            cell.SetCellValue(dtlSum2);
                            cell.CellStyle = cellStyle4;
                            cell = row.CreateCell(11);
                            cell.SetCellValue(dtlSum3);
                            cell.CellStyle = cellStyle4;
                            rowNumber++;
                        }
                        else
                        {
                            //明细表合并单元格
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlBudgetDtls.Count + 1, 0, 0));

                            //明细表表头
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(0);
                            cell.SetCellValue("项目预算明细(万元)");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(1);
                            cell.SetCellValue("序号");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(2);
                            cell.SetCellValue("明细项目名称");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(3);
                            cell.SetCellValue("会计科目");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(4);
                            cell.SetCellValue("资金来源");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(5);
                            cell.SetCellValue("功能科目");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(6);
                            cell.SetCellValue("支付方式");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(7);
                            cell.SetCellValue("支出渠道");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(8);
                            cell.SetCellValue("支出类别");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(9);
                            cell.SetCellValue("年初预算");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(10);
                            cell.SetCellValue("调整金额");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(11);
                            cell.SetCellValue("调整后项目预算金额");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(12);
                            cell.SetCellValue("年中调整测算说明");
                            cell.CellStyle = cellHeadStyle;
                            rowNumber++;

                            //合计
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 8));
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(1);
                            cell.SetCellValue("合计");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(9);
                            cell.SetCellValue("");
                            cell.CellStyle = cellStyle2;
                            cell = row.CreateCell(10);
                            cell.SetCellValue("");
                            cell.CellStyle = cellStyle2;
                            cell = row.CreateCell(11);
                            cell.SetCellValue("");
                            cell.CellStyle = cellStyle2;
                            rowNumber++;
                        }

                        //项目资金申请
                        IList<BudgetDtlFundApplModel> projectDtlFundAppls = this.BudgetDtlFundApplFacade.FindByForeignKey(budget.PhId).Data;
                        if (projectDtlFundAppls != null && projectDtlFundAppls.Count > 0)
                        {
                            //项目资金申请表合并单元格
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count + 1, 0, 0));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 7));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 12));
                            //项目资金申请表表头
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(0);
                            cell.SetCellValue("项目资金申请(万元)");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(1);
                            cell.SetCellValue("序号");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(2);
                            cell.SetCellValue("资金来源");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(8);
                            cell.SetCellValue("金额");
                            cell.CellStyle = cellHeadStyle;
                            rowNumber++;
                            double fundSum = 0;
                            for (int j = 1; j <= projectDtlFundAppls.Count; j++)
                            {
                                sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 7));
                                sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 12));
                                row = sheet.CreateRow(rowNumber);
                                cell = row.CreateCell(1);
                                cell.SetCellValue(j);
                                cell.CellStyle = cellHeadStyle;
                                cell = row.CreateCell(2);
                                cell.SetCellValue(projectDtlFundAppls[j - 1].FSourceOfFunds_EXName);
                                cell.CellStyle = cellHeadStyle;
                                cell = row.CreateCell(8);
                                cell.SetCellValue((double)(projectDtlFundAppls[j - 1].FAmount / 10000));
                                cell.CellStyle = cellStyle4;

                                rowNumber++;
                                fundSum += (double)(projectDtlFundAppls[j - 1].FAmount / 10000);
                            }

                            //合计
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 7));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 12));
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(1);
                            cell.SetCellValue("资金总额");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(8);
                            cell.SetCellValue(fundSum);
                            cell.CellStyle = cellStyle4;
                            rowNumber++;
                        }
                        else
                        {
                            //项目资金申请表合并单元格
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count + 1, 0, 0));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 7));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 12));
                            //项目资金申请表表头
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(0);
                            cell.SetCellValue("项目资金申请(万元)");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(1);
                            cell.SetCellValue("序号");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(2);
                            cell.SetCellValue("资金来源");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(8);
                            cell.SetCellValue("金额");
                            cell.CellStyle = cellHeadStyle;
                            rowNumber++;
                            double fundSum = 0;

                            //合计
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 7));
                            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 12));
                            row = sheet.CreateRow(rowNumber);
                            cell = row.CreateCell(1);
                            cell.SetCellValue("资金总额");
                            cell.CellStyle = cellHeadStyle;
                            cell = row.CreateCell(8);
                            cell.SetCellValue(fundSum);
                            cell.CellStyle = cellStyle4;
                            rowNumber++;
                        }

                        #region //项目实施进度计划
                        //项目实施进度计划
                        //IList<BudgetDtlImplPlanModel> projectDtlImplPlans = this.BudgetDtlImplPlanFacade.FindByForeignKey(budget.PhId).Data;
                        //if (projectDtlImplPlans != null && projectDtlImplPlans.Count > 0)
                        //{
                        //    //项目实施进度计划表合并单元格
                        //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count, 0, 0));
                        //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 5));
                        //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 7));
                        //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 9));
                        //    //项目实施进度计划表表头
                        //    row = sheet.CreateRow(rowNumber);
                        //    cell = row.CreateCell(0);
                        //    cell.SetCellValue("项目实施进度计划");
                        //    cell.CellStyle = cellHeadStyle;
                        //    cell = row.CreateCell(1);
                        //    cell.SetCellValue("项目实施内容");
                        //    cell.CellStyle = cellHeadStyle;
                        //    cell = row.CreateCell(6);
                        //    cell.SetCellValue("开始时间");
                        //    cell.CellStyle = cellHeadStyle;
                        //    cell = row.CreateCell(8);
                        //    cell.SetCellValue("完成时间");
                        //    cell.CellStyle = cellHeadStyle;
                        //    rowNumber++;

                        //    for (int j = 1; j <= projectDtlImplPlans.Count; j++)
                        //    {
                        //        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 5));
                        //        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 7));
                        //        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 9));
                        //        row = sheet.CreateRow(rowNumber);
                        //        cell = row.CreateCell(1);
                        //        cell.SetCellValue(projectDtlImplPlans[j - 1].FImplContent);
                        //        cell.CellStyle = cellHeadStyle;
                        //        cell = row.CreateCell(6);
                        //        cell.SetCellValue(projectDtlImplPlans[j - 1].FStartDate.ToString());
                        //        cell.CellStyle = cellHeadStyle;
                        //        cell = row.CreateCell(8);
                        //        cell.SetCellValue(projectDtlImplPlans[j - 1].FEndDate.ToString());
                        //        cell.CellStyle = cellHeadStyle;
                        //        rowNumber++;
                        //    }
                        //}
                        //else
                        //{
                        //    //项目实施进度计划表合并单元格
                        //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 0, 0));
                        //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 5));
                        //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 6, 7));
                        //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 9));
                        //    //项目实施进度计划表表头
                        //    row = sheet.CreateRow(rowNumber);
                        //    cell = row.CreateCell(0);
                        //    cell.SetCellValue("项目实施进度计划");
                        //    cell.CellStyle = cellHeadStyle;
                        //    cell = row.CreateCell(1);
                        //    cell.SetCellValue("项目实施内容");
                        //    cell.CellStyle = cellHeadStyle;
                        //    cell = row.CreateCell(6);
                        //    cell.SetCellValue("开始时间");
                        //    cell.CellStyle = cellHeadStyle;
                        //    cell = row.CreateCell(8);
                        //    cell.SetCellValue("完成时间");
                        //    cell.CellStyle = cellHeadStyle;
                        //    rowNumber++;
                        //}
                        #endregion

                        #region//绩效目标
                        ////绩效目标
                        ////绩效目标表合并单元格
                        //sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + 1, 0, 0));
                        //sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                        //sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                        ////绩效目标表表头
                        //row = sheet.CreateRow(rowNumber);
                        //cell = row.CreateCell(0);
                        //cell.SetCellValue("绩效目标");
                        //cell.CellStyle = cellHeadStyle;
                        //cell = row.CreateCell(1);
                        //cell.SetCellValue("年度目标");
                        //cell.CellStyle = cellHeadStyle;
                        //cell = row.CreateCell(5);
                        //cell.SetCellValue("长期目标");
                        //cell.CellStyle = cellHeadStyle;
                        //rowNumber++;
                        //sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                        //sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                        ////绩效目标表表头
                        //row = sheet.CreateRow(rowNumber);
                        //cell = row.CreateCell(1);
                        //cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FAnnualPerformGoal);
                        //cell.CellStyle = cellStyle2;
                        //cell = row.CreateCell(5);
                        //cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FLTPerformGoal);
                        //cell.CellStyle = cellStyle2;
                        //rowNumber++;
                        #endregion

                        #region
                        //IList<ProjectDtlPerformTargetModel> projectDtlPerformTargets = this.ProjectDtlPerformTargetFacade.FindByForeignKey(project.PhId).Data;
                        //if(projectDtlPerformTargets != null && projectDtlPerformTargets.Count > 0)
                        //{
                        //    //绩效目标表合并单元格
                        //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count, 0, 0));
                        //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                        //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                        //    //绩效目标表表头
                        //    row = sheet.CreateRow(rowNumber);
                        //    cell = row.CreateCell(0);
                        //    cell.SetCellValue("绩效目标");
                        //    cell.CellStyle = cellHeadStyle;
                        //    cell = row.CreateCell(1);
                        //    cell.SetCellValue("年度目标");
                        //    cell.CellStyle = cellHeadStyle;
                        //    cell = row.CreateCell(5);
                        //    cell.SetCellValue("长期目标");
                        //    cell.CellStyle = cellHeadStyle;
                        //    rowNumber++;

                        //}
                        //else
                        //{
                        //    //绩效目标表合并单元格
                        //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + projectDtlFundAppls.Count, 0, 0));
                        //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 4));
                        //    sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
                        //    //绩效目标表表头
                        //    row = sheet.CreateRow(rowNumber);
                        //    cell = row.CreateCell(0);
                        //    cell.SetCellValue("绩效目标");
                        //    cell.CellStyle = cellHeadStyle;
                        //    cell = row.CreateCell(1);
                        //    cell.SetCellValue("年度目标");
                        //    cell.CellStyle = cellHeadStyle;
                        //    cell = row.CreateCell(5);
                        //    cell.SetCellValue("长期目标");
                        //    cell.CellStyle = cellHeadStyle;
                        //    rowNumber++;
                        //}
                        #endregion

                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 6));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 12));
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("部门领导意见");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FDeptOpinions);
                        cell.CellStyle = cellStyle2;

                        cell = row.CreateCell(7);
                        cell.SetCellValue("部门分管领导意见");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(8);
                        cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FDeptOpinions2);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber + 1, 0, 0));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 2, 6));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 12));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber + 1, rowNumber + 1, 2, 12));


                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("项目审核领导小组意见");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("会议时间");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(2);
                        cell.SetCellValue(budget.FMeetingTime == null ? "" : ((DateTime)budget.FMeetingTime).ToString("yyyy-MM-dd"));
                        cell.CellStyle = cellStyle2;
                        cell = row.CreateCell(7);
                        cell.SetCellValue("会议纪要编号");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(8);
                        cell.SetCellValue(budget.FMeetiingSummaryNo);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(1);
                        cell.SetCellValue("会议决议");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(2);
                        cell.SetCellValue(GetResolutionName(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FResolution));
                        cell.CellStyle = cellStyle2;
                        rowNumber++;
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 6));
                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 8, 12));
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("党组会议意见");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FLeadingOpinions);
                        cell.CellStyle = cellStyle2;
                        cell = row.CreateCell(7);
                        cell.SetCellValue("主席办公会议意见");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(8);
                        cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FChairmanOpinions);
                        cell.CellStyle = cellStyle2;


                        rowNumber++;

                        sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 12));
                        row = sheet.CreateRow(rowNumber);
                        cell = row.CreateCell(0);
                        cell.SetCellValue("备注");
                        cell.CellStyle = cellHeadStyle;
                        cell = row.CreateCell(1);
                        cell.SetCellValue(budgetDtlTextContent == null ? "" : budgetDtlTextContent.FBz);
                        cell.CellStyle = cellStyle2;
                        rowNumber++;

                        count++;
                    }
                    
                }

            }

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            var buf = ms.ToArray();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\\DownLoadFiles\\BudgetMstTZS";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            using (FileStream fs = File.OpenWrite(path + "\\" + filename))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
                fs.Close();
            }
            workbook = null;
            ms.Close();
            ms.Dispose();

            return JsonConvert.SerializeObject(new { path = "BudgetMstTZS", filename = filename });
        }

        /// <summary>
        /// 年中新增汇总表的（按明细来显示）
        /// </summary>
        /// <param name="projectMsts">项目集合</param>
        /// <param name="qtCoverUpForOrg">套打格式</param>
        /// <param name="organize">组织</param>
        /// <param name="user">人员</param>
        /// <returns></returns>
        public string ExportSummaryExcelXZ(IList<BudgetMstModel> projectMsts, QtCoverUpForOrgModel qtCoverUpForOrg, OrganizeModel organize, User2Model user)
        {

            string[] head = { "序号", "项目编码", "项目名称", "支出类别", "存续期限", "项目属性", "单据状态", "绩效评价", "项目金额", "开始日期", "结束日期", "申报日期", "申报人" };

            //行索引
            int rowNumber = 0;
            
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("工会年中新增列表汇总表");
            sheet.DefaultRowHeight = 18 * 20;
            sheet.DefaultColumnWidth = 12;
            sheet.SetColumnWidth(0, 4800);
            sheet.SetColumnWidth(1, 4800);
            sheet.SetColumnWidth(2, 4800);
            sheet.SetColumnWidth(3, 4800);
            sheet.SetColumnWidth(4, 4800);
            sheet.SetColumnWidth(5, 4800);
            sheet.SetColumnWidth(6, 4800);
            sheet.SetColumnWidth(7, 4800);
            sheet.SetColumnWidth(8, 4800);
            sheet.SetColumnWidth(9, 4800);
            sheet.SetColumnWidth(10, 4800);
            sheet.SetColumnWidth(11, 4800);
            sheet.SetColumnWidth(12, 4800);
            sheet.SetColumnWidth(13, 4800);

            //合并单元格
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 13));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 5));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 6, 11));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 12, 13));


            //标题单元格样式
            ICellStyle cellTitleStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 500, 14, false);
            ICellStyle cellTitleStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
            ICellStyle cellTitleStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Right, VerticalAlignment.Center, 400, 12, false);
            //表头单元格样式
            ICellStyle cellHeadStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
            //内容单元格样式
            ICellStyle cellStyle1 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
            ICellStyle cellStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
            ICellStyle cellStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);

            //标题
            IRow row = sheet.CreateRow(rowNumber);
            row.Height = 20 * 20;
            ICell cell = row.CreateCell(0);
            cell.SetCellValue(qtCoverUpForOrg.TempLateName + "工会部门年中新增项目汇总表");
            cell.CellStyle = cellTitleStyle;
            rowNumber++;
            row = sheet.CreateRow(rowNumber);
            cell = row.CreateCell(0);
            cell.SetCellValue("申报部门：（盖章）" + organize.OName);
            cell.CellStyle = cellTitleStyle2;
            cell = row.CreateCell(6);
            cell.SetCellValue("制表日期：" + DateTime.Now.ToString("yyyy年MM月dd日"));
            cell.CellStyle = cellTitleStyle2;
            cell = row.CreateCell(12);
            cell.SetCellValue("单位：元");
            cell.CellStyle = cellTitleStyle2;
            rowNumber++;

            //表头
            row = sheet.CreateRow(rowNumber);
            cell = row.CreateCell(0);
            cell.SetCellValue("序号");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(1);
            cell.SetCellValue("项目编码");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(2);
            cell.SetCellValue("项目名称");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(3);
            cell.SetCellValue("明细项目名称");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(4);
            cell.SetCellValue("会计科目");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(5);
            cell.SetCellValue("资金来源");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(6);
            cell.SetCellValue("功能科目");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(7);
            cell.SetCellValue("支付方式");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(8);
            cell.SetCellValue("支出渠道");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(9);
            cell.SetCellValue("支出类别");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(10);
            cell.SetCellValue("年初预算");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(11);
            cell.SetCellValue("调整金额");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(12);
            cell.SetCellValue("调整后项目预算金额");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(13);
            cell.SetCellValue("调整说明");
            cell.CellStyle = cellHeadStyle;
            rowNumber++;

            double sum = 0, sum2 = 0, sum3 = 0;
            if(projectMsts != null && projectMsts.Count > 0)
            {
                var phids = projectMsts.Select(t => t.PhId).ToList();
                IList<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtls = new List<BudgetDtlBudgetDtlModel>();
                budgetDtlBudgetDtls = this.BudgetDtlBudgetDtlFacade.Find(t => phids.Contains(t.MstPhid)).Data;
                //附上名称
                RichHelpDac helpdac = new RichHelpDac();
                helpdac.CodeToName<BudgetDtlBudgetDtlModel>(budgetDtlBudgetDtls, "FSourceOfFunds", "FSourceOfFunds_EXName", "GHSourceOfFunds", "");
                helpdac.CodeToName<BudgetDtlBudgetDtlModel>(budgetDtlBudgetDtls, "FPaymentMethod", "FPaymentMethod_EXName", "GHPaymentMethod", "");
                helpdac.CodeToName<BudgetDtlBudgetDtlModel>(budgetDtlBudgetDtls, "FExpensesChannel", "FExpensesChannel_EXName", "GHExpensesChannel", "");
                helpdac.CodeToName<BudgetDtlBudgetDtlModel>(budgetDtlBudgetDtls, "FBudgetAccounts", "FBudgetAccounts_EXName", "GHBudgetAccounts", "");
                helpdac.CodeToName<BudgetDtlBudgetDtlModel>(budgetDtlBudgetDtls, "FQtZcgnfl", "FQtZcgnfl_EXName", "GHQtZcgnfl", "");
                //表格内容
                for (int i = 1; i <= projectMsts.Count; i++)
                {
                    if(budgetDtlBudgetDtls != null && budgetDtlBudgetDtls.Count > 0)
                    {
                        var dtlsByPhid = budgetDtlBudgetDtls.ToList().FindAll(t => t.MstPhid == projectMsts[i - 1].PhId);
                        if(dtlsByPhid != null && dtlsByPhid.Count > 0)
                        {
                            for (int k = 1; k <= dtlsByPhid.Count; k++)
                            {
                                row = sheet.CreateRow(rowNumber);
                                //每行
                                cell = row.CreateCell(0);
                                cell.SetCellValue(i);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(1);
                                cell.SetCellValue(projectMsts[i - 1].FProjCode);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(2);
                                cell.SetCellValue(projectMsts[i - 1].FProjName);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(3);
                                cell.SetCellValue(dtlsByPhid[k - 1].FName);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(4);
                                cell.SetCellValue(dtlsByPhid[k - 1].FBudgetAccounts_EXName);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(5);
                                cell.SetCellValue(dtlsByPhid[k - 1].FSourceOfFunds_EXName);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(6);
                                cell.SetCellValue(dtlsByPhid[k - 1].FQtZcgnfl_EXName);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(7);
                                cell.SetCellValue(dtlsByPhid[k - 1].FPaymentMethod_EXName);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(8);
                                cell.SetCellValue(dtlsByPhid[k - 1].FExpensesChannel_EXName);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(9);
                                cell.SetCellValue(projectMsts[i - 1].FExpenseCategory_EXName);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(10);
                                cell.SetCellValue((double)dtlsByPhid[k - 1].FAmount);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(11);
                                cell.SetCellValue((double)dtlsByPhid[k - 1].FAmountEdit);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(12);
                                cell.SetCellValue((double)dtlsByPhid[k - 1].FAmountAfterEdit);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(13);
                                cell.SetCellValue(dtlsByPhid[k - 1].FOtherInstructions);
                                cell.CellStyle = cellStyle2;
                                rowNumber++;
                                sum += (double)dtlsByPhid[k - 1].FAmount;
                                sum2 += (double)dtlsByPhid[k - 1].FAmountEdit;
                                sum3 += (double)dtlsByPhid[k - 1].FAmountAfterEdit;
                            }
                        }
                        
                    }
                    

                }
            }
            
            //合并单元格
            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 0, 9));
            //土地部分
            row = sheet.CreateRow(rowNumber);
            cell = row.CreateCell(0);
            cell.SetCellValue("项目金额合计：");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(10);
            cell.SetCellValue(sum);
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(11);
            cell.SetCellValue(sum2);
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(12);
            cell.SetCellValue(sum3);
            cell.CellStyle = cellHeadStyle;
            rowNumber++;

            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 0, 4));
            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 10, 13));
            row = sheet.CreateRow(rowNumber);
            cell = row.CreateCell(0);
            cell.SetCellValue("申报部门分管领导：");
            cell.CellStyle = cellTitleStyle2;
            cell = row.CreateCell(5);
            cell.SetCellValue("申报部门负责人：");
            cell.CellStyle = cellTitleStyle2;
            cell = row.CreateCell(10);
            cell.SetCellValue("经办人：");
            //cell.SetCellValue("制表人："+"                            "+"部门领导审核："+"                            "+"分管领导审核：");
            cell.CellStyle = cellTitleStyle2;

            //合并单元格
            //sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 2));
            //row = sheet.CreateRow(rowNumber);
            //cell = row.CreateCell(0);
            //cell.SetCellValue("工会主席:" + sysOrganize.Chairman);
            //cell.CellStyle = cellTitleStyle2;
            //cell = row.CreateCell(1);
            //cell.SetCellValue("财务负责人:" + sysOrganize.Treasurer);
            //cell.CellStyle = cellTitleStyle2;
            //cell = row.CreateCell(3);
            //cell.SetCellValue("复核:");
            //cell.CellStyle = cellTitleStyle2;
            //cell = row.CreateCell(4);
            //cell.SetCellValue("制表:" + sysUser.RealName);
            //cell.CellStyle = cellTitleStyle2;
            rowNumber++;

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            var buf = ms.ToArray();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\\DownLoadFiles\\BudgetMstXZZ";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            using (FileStream fs = File.OpenWrite(path + "\\" + filename))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
                fs.Close();
            }
            workbook = null;
            ms.Close();
            ms.Dispose();

            return JsonConvert.SerializeObject(new { path = "BudgetMstXZZ", filename = filename });
        }

        /// <summary>
        /// 年中调整汇总表的（按明细来显示）(年中新增与年中调整合在一起)
        /// </summary>
        /// <param name="projectMsts">项目集合</param>
        /// <param name="qtCoverUpForOrg">套打格式</param>
        /// <param name="organize">组织</param>
        /// <param name="user">人员</param>
        /// <returns></returns>
        public string ExportSummaryExcelTZ(IList<BudgetMstModel> projectMsts, QtCoverUpForOrgModel qtCoverUpForOrg, OrganizeModel organize, User2Model user)
        {

            string[] head = { "序号", "项目编码", "项目名称", "支出类别", "存续期限", "项目属性", "单据状态", "绩效评价", "项目金额", "开始日期", "结束日期", "申报日期", "申报人" };

            //行索引
            int rowNumber = 0;

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("工会年中调整列表汇总表");
            sheet.DefaultRowHeight = 18 * 20;
            sheet.DefaultColumnWidth = 12;
            sheet.SetColumnWidth(0, 4800);
            sheet.SetColumnWidth(1, 4800);
            sheet.SetColumnWidth(2, 4800);
            sheet.SetColumnWidth(3, 4800);
            sheet.SetColumnWidth(4, 4800);
            sheet.SetColumnWidth(5, 4800);
            sheet.SetColumnWidth(6, 4800);
            sheet.SetColumnWidth(7, 4800);
            sheet.SetColumnWidth(8, 4800);
            sheet.SetColumnWidth(9, 4800);
            sheet.SetColumnWidth(10, 4800);
            sheet.SetColumnWidth(11, 4800);
            sheet.SetColumnWidth(12, 4800);
            sheet.SetColumnWidth(13, 4800);

            //合并单元格
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 13));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 5));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 6, 11));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 12, 13));


            //标题单元格样式
            ICellStyle cellTitleStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 500, 14, false);
            ICellStyle cellTitleStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
            ICellStyle cellTitleStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Right, VerticalAlignment.Center, 400, 12, false);
            //表头单元格样式
            ICellStyle cellHeadStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
            //内容单元格样式
            ICellStyle cellStyle1 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
            ICellStyle cellStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
            ICellStyle cellStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
            //数字内容格式
            ICellStyle cellStyle4 = ExcelHelper.CreateStyle2(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);

            //标题
            IRow row = sheet.CreateRow(rowNumber);
            row.Height = 20 * 20;
            ICell cell = row.CreateCell(0);
            cell.SetCellValue(qtCoverUpForOrg.TempLateName + "工会部门年中调整项目汇总表");
            cell.CellStyle = cellTitleStyle;
            rowNumber++;
            row = sheet.CreateRow(rowNumber);
            cell = row.CreateCell(0);
            cell.SetCellValue("申报部门：（盖章）" + organize.OName);
            cell.CellStyle = cellTitleStyle2;
            cell = row.CreateCell(6);
            cell.SetCellValue("制表日期：" + DateTime.Now.ToString("yyyy年MM月dd日"));
            cell.CellStyle = cellTitleStyle2;
            cell = row.CreateCell(12);
            cell.SetCellValue("单位：元");
            cell.CellStyle = cellTitleStyle2;
            rowNumber++;

            //表头
            row = sheet.CreateRow(rowNumber);
            cell = row.CreateCell(0);
            cell.SetCellValue("序号");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(1);
            cell.SetCellValue("项目编码");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(2);
            cell.SetCellValue("项目名称");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(3);
            cell.SetCellValue("明细项目名称");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(4);
            cell.SetCellValue("会计科目");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(5);
            cell.SetCellValue("资金来源");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(6);
            cell.SetCellValue("功能科目");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(7);
            cell.SetCellValue("支付方式");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(8);
            cell.SetCellValue("支出渠道");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(9);
            cell.SetCellValue("支出类别");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(10);
            cell.SetCellValue("年初预算");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(11);
            cell.SetCellValue("调整金额");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(12);
            cell.SetCellValue("调整后项目预算金额");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(13);
            cell.SetCellValue("调整说明");
            cell.CellStyle = cellHeadStyle;
            rowNumber++;

            double sum = 0, sum2 = 0, sum3 = 0;
            if (projectMsts != null && projectMsts.Count > 0)
            {
                var phids = projectMsts.Select(t => t.PhId).ToList();
                IList<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtls = new List<BudgetDtlBudgetDtlModel>();
                budgetDtlBudgetDtls = this.BudgetDtlBudgetDtlFacade.Find(t => phids.Contains(t.MstPhid)).Data;
                //附上名称
                RichHelpDac helpdac = new RichHelpDac();
                helpdac.CodeToName<BudgetDtlBudgetDtlModel>(budgetDtlBudgetDtls, "FSourceOfFunds", "FSourceOfFunds_EXName", "GHSourceOfFunds", "");
                helpdac.CodeToName<BudgetDtlBudgetDtlModel>(budgetDtlBudgetDtls, "FPaymentMethod", "FPaymentMethod_EXName", "GHPaymentMethod", "");
                helpdac.CodeToName<BudgetDtlBudgetDtlModel>(budgetDtlBudgetDtls, "FExpensesChannel", "FExpensesChannel_EXName", "GHExpensesChannel", "");
                helpdac.CodeToName<BudgetDtlBudgetDtlModel>(budgetDtlBudgetDtls, "FBudgetAccounts", "FBudgetAccounts_EXName", "GHBudgetAccounts", "");
                helpdac.CodeToName<BudgetDtlBudgetDtlModel>(budgetDtlBudgetDtls, "FQtZcgnfl", "FQtZcgnfl_EXName", "GHQtZcgnfl", "");
                //表格内容
                for (int i = 1; i <= projectMsts.Count; i++)
                {
                    if (budgetDtlBudgetDtls != null && budgetDtlBudgetDtls.Count > 0)
                    {
                        var dtlsByPhid = budgetDtlBudgetDtls.ToList().FindAll(t => t.MstPhid == projectMsts[i - 1].PhId);
                        if (dtlsByPhid != null && dtlsByPhid.Count > 0)
                        {
                            for (int k = 1; k <= dtlsByPhid.Count; k++)
                            {
                                row = sheet.CreateRow(rowNumber);
                                //每行
                                cell = row.CreateCell(0);
                                cell.SetCellValue(i);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(1);
                                cell.SetCellValue(projectMsts[i - 1].FProjCode);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(2);
                                cell.SetCellValue(projectMsts[i - 1].FProjName);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(3);
                                cell.SetCellValue(dtlsByPhid[k - 1].FName);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(4);
                                cell.SetCellValue(dtlsByPhid[k - 1].FBudgetAccounts_EXName);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(5);
                                cell.SetCellValue(dtlsByPhid[k - 1].FSourceOfFunds_EXName);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(6);
                                cell.SetCellValue(dtlsByPhid[k - 1].FQtZcgnfl_EXName);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(7);
                                cell.SetCellValue(dtlsByPhid[k - 1].FPaymentMethod_EXName);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(8);
                                cell.SetCellValue(dtlsByPhid[k - 1].FExpensesChannel_EXName);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(9);
                                cell.SetCellValue(projectMsts[i - 1].FExpenseCategory_EXName);
                                cell.CellStyle = cellStyle2;
                                cell = row.CreateCell(10);
                                cell.SetCellValue((double)dtlsByPhid[k - 1].FAmount);
                                cell.CellStyle = cellStyle4;
                                cell = row.CreateCell(11);
                                cell.SetCellValue((double)dtlsByPhid[k - 1].FAmountEdit);
                                cell.CellStyle = cellStyle4;
                                cell = row.CreateCell(12);
                                cell.SetCellValue((double)dtlsByPhid[k - 1].FAmountAfterEdit);
                                cell.CellStyle = cellStyle4;
                                cell = row.CreateCell(13);
                                cell.SetCellValue(dtlsByPhid[k - 1].FOtherInstructions);
                                cell.CellStyle = cellStyle2;
                                rowNumber++;
                                sum += (double)dtlsByPhid[k - 1].FAmount;
                                sum2 += (double)dtlsByPhid[k - 1].FAmountEdit;
                                sum3 += (double)dtlsByPhid[k - 1].FAmountAfterEdit;
                            }
                        }

                    }


                }
            }

            //合并单元格
            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 0, 9));
            //土地部分
            row = sheet.CreateRow(rowNumber);
            cell = row.CreateCell(0);
            cell.SetCellValue("项目金额合计：");
            cell.CellStyle = cellHeadStyle;
            cell = row.CreateCell(10);
            cell.SetCellValue(sum);
            cell.CellStyle = cellStyle4;
            cell = row.CreateCell(11);
            cell.SetCellValue(sum2);
            cell.CellStyle = cellStyle4;
            cell = row.CreateCell(12);
            cell.SetCellValue(sum3);
            cell.CellStyle = cellStyle4;
            rowNumber++;

            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 0, 4));
            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 5, 9));
            sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 10, 13));
            row = sheet.CreateRow(rowNumber);
            cell = row.CreateCell(0);
            cell.SetCellValue("申报部门分管领导：");
            cell.CellStyle = cellTitleStyle2;
            cell = row.CreateCell(5);
            cell.SetCellValue("申报部门负责人：");
            cell.CellStyle = cellTitleStyle2;
            cell = row.CreateCell(10);
            cell.SetCellValue("经办人：");
            //cell.SetCellValue("制表人："+"                            "+"部门领导审核："+"                            "+"分管领导审核：");
            cell.CellStyle = cellTitleStyle2;

            //合并单元格
            //sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 1, 2));
            //row = sheet.CreateRow(rowNumber);
            //cell = row.CreateCell(0);
            //cell.SetCellValue("工会主席:" + sysOrganize.Chairman);
            //cell.CellStyle = cellTitleStyle2;
            //cell = row.CreateCell(1);
            //cell.SetCellValue("财务负责人:" + sysOrganize.Treasurer);
            //cell.CellStyle = cellTitleStyle2;
            //cell = row.CreateCell(3);
            //cell.SetCellValue("复核:");
            //cell.CellStyle = cellTitleStyle2;
            //cell = row.CreateCell(4);
            //cell.SetCellValue("制表:" + sysUser.RealName);
            //cell.CellStyle = cellTitleStyle2;
            rowNumber++;

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            var buf = ms.ToArray();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\\DownLoadFiles\\BudgetMstTZZ";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            using (FileStream fs = File.OpenWrite(path + "\\" + filename))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
                fs.Close();
            }
            workbook = null;
            ms.Close();
            ms.Dispose();

            return JsonConvert.SerializeObject(new { path = "BudgetMstTZZ", filename = filename });
        }

        //<span v-if="scope.row.FProjStatus ===1">预立项</span>
        //<span v-if="scope.row.FProjStatus ===2">项目立项</span>
        //<span v-if="scope.row.FProjStatus ===3">项目执行</span>
        //<span v-if="scope.row.FProjStatus ===4">项目调整</span>
        //<span v-if="scope.row.FProjStatus ===5">调整项目执行</span>
        //<span v-if="scope.row.FProjStatus ===6">项目终止</span>
        //<span v-if="scope.row.FProjStatus ===7">项目关闭</span>
        //<span v-if="scope.row.FProjStatus ===9">项目预执行</span>
        //<span v-if="scope.row.FProjStatus ===10">年中新增执行</span>
        //<span v-if="scope.row.FProjStatus ===11">年中项目执行</span>


        /// <summary>
        /// 通过项目属性获取项目属性名称
        /// </summary>
        /// <param name="fProjStatus">项目属性</param>
        /// <returns></returns>
        public static string GetProjStatusName(int fProjStatus)
        {
            string projStatusName = "";
            switch (fProjStatus)
            {
                case 1:
                    projStatusName = "预立项";
                    break;
                case 2:
                    projStatusName = "项目立项";
                    break;
                case 3:
                    projStatusName = "项目执行";
                    break;
                case 4:
                    projStatusName = "项目调整";
                    break;
                case 5:
                    projStatusName = "调整项目执行";
                    break;
                case 6:
                    projStatusName = "项目终止";
                    break;
                case 7:
                    projStatusName = "项目关闭";
                    break;
                case 9:
                    projStatusName = "项目预执行";
                    break;
                case 10:
                    projStatusName = "年中新增执行";
                    break;
                case 11:
                    projStatusName = "年中项目执行";
                    break;
                default:
                    projStatusName = "";
                    break;
            }
            return projStatusName;
        }

        /// <summary>
        /// 会议决议（0-通过，1-不通过）
        /// </summary>
        /// <param name="resolutionCode">会议决议编码</param>
        /// <returns></returns>
        public static string GetResolutionName(string resolutionCode)
        {
            if (!string.IsNullOrEmpty(resolutionCode))
            {
                if (resolutionCode.Equals("0"))
                {
                    return "通过";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 是否绩效评价
        /// </summary>
        /// <param name="ifPerformance"></param>
        /// <returns></returns>
        public static string GetIfPerformance(int ifPerformance)
        {
            if (ifPerformance == 1)
            {
                return "是";
            }
            else
            {
                return "否";
            }
        }
        /// <summary>
        /// 根据单据状态获取单据状态名
        /// </summary>
        /// <param name="approveCode">单据状态</param>
        /// <returns></returns>
        public static string GetApproveName(string approveCode)
        {
            string proApproveName = "";
            switch (approveCode)
            {
                case "1":
                    proApproveName = "待上报";
                    break;
                case "2":
                    proApproveName = "审批中";
                    break;
                case "3":
                    proApproveName = "审批通过";
                    break;
                case "4":
                    proApproveName = "已退回";
                    break;
                case "5":
                    proApproveName = "暂存";
                    break;              
                default:
                    proApproveName = "";
                    break;
            }
            return proApproveName;
        }

        /// <summary>
        /// 通过支出类别编码获取支出类别名称
        /// </summary>
        /// <param name="zclbCode">支出类别编码</param>
        /// <returns></returns>
        public static string GetProjectZclbName(string zclbCode)
        {
            if (string.IsNullOrEmpty(zclbCode))
            {
                return "";
            }
            else
            {
                if (zclbCode.Equals("1"))
                {
                    return "项目支出";
                }
                else
                {
                    return "基本支出";
                }
            }
        }

        /// <summary>
        /// 根据字典PHID取字表数据
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public List<ProjectDtlBudgetDtlModel> FindProjectDtlBudgetDtl(Dictionary<string, object> dictionary)
        {
            var Query = ProjectDtlBudgetDtlFacade.Find(dictionary);
            return Query.Data.ToList();
        }
        #endregion

        #region//民生银行相关

        /// <summary>
        /// 根据主表数据补充明细数据
        /// </summary>
        /// <param name="projectMsts">主表数据</param>
        /// <returns></returns>
        public List<ProjectAllDataModel> GetProjectAllDataModels(List<ProjectMstModel> projectMsts)
        {
            List<ProjectAllDataModel> projectAllDatas = new List<ProjectAllDataModel>();
            if (projectMsts != null && projectMsts.Count > 0)
            {
                List<long> mstPhids = new List<long>();
                mstPhids = projectMsts.Select(t => t.PhId).ToList();
                if (mstPhids != null && mstPhids.Count > 0)
                {
                    //主表对应附件集合
                    List<QtAttachmentModel> qtAttachments = QtAttachmentFacade.Find(t => t.BTable == "XM3_PROJECTMST" && mstPhids.Contains(t.RelPhid)).Data.ToList();
                    //主表对应明细所有数据
                    List<ProjectDtlBudgetDtlModel> projectDtlBudgetDtls = ProjectDtlBudgetDtlFacade.Find(t => mstPhids.Contains(t.MstPhid)).Data.ToList();
                    //List<ProjectDtlBudgetDtlModel> projectDtlBudgetDtls2 = ProjectDtlBudgetDtlFacade.Find(t => mstPhids.Contains(t.MstPhid)).Data.ToList();
                    List<ProjectDtlFundApplModel> projectDtlFundAppls = ProjectDtlFundApplFacade.Find(t => mstPhids.Contains(t.MstPhid)).Data.ToList();
                    List<ProjectDtlImplPlanModel> projectDtlImplPlans = ProjectDtlImplPlanFacade.Find(t => mstPhids.Contains(t.MstPhid)).Data.ToList();
                    List<ProjectDtlPerformTargetModel> projectDtlPerformTargets = ProjectDtlPerformTargetFacade.Find(t => mstPhids.Contains(t.MstPhid)).Data.ToList();
                    List<ProjectDtlPersonnelModel> projectDtlPersonnels = ProjectDtlPersonnelFacade.Find(t => mstPhids.Contains(t.MstPhid)).Data.ToList();
                    List<ProjectDtlPurchaseDtlModel> projectDtlPurchaseDtls = ProjectDtlPurchaseDtlFacade.Find(t => mstPhids.Contains(t.MstPhid)).Data.ToList();
                    List<ProjectDtlPurDtl4SOFModel> projectDtlPurDtl4SOFs = ProjectDtlPurDtl4SOFFacade.Find(t => mstPhids.Contains(t.MstPhid)).Data.ToList();
                    List<ProjectDtlTextContentModel> projectDtlTextContents = ProjectDtlTextContentFacade.Find(t => mstPhids.Contains(t.MstPhid)).Data.ToList();
                    List<ProjectDtlPersonNameModel> projectDtlPersonNames = ProjectDtlPersonNameFacade.Find(t => mstPhids.Contains(t.MstPhid)).Data.ToList();

                    //基础数据集合（为转名称做准备）
                    IList<QTSysSetModel> allSysSets = this.QTSysSetFacade.Find(t => t.DicType == "ZcfxName" && t.Orgcode == projectMsts[0].FDeclarationUnit).Data;

                    //科目集合（为转名称做准备）
                    IList<BudgetAccountsModel> allBudgetAccounts = this.BudgetAccountsFacade.Find(t => t.PhId != 0).Data;

                    //for循环逐个加入数据
                    foreach (var mst in projectMsts)
                    {
                        ProjectAllDataModel projectAllData = new ProjectAllDataModel();
                        projectAllData.ProjectMst = mst;
                        if(qtAttachments != null && qtAttachments.Count > 0)
                        {
                            projectAllData.ProjectAttachments = qtAttachments.FindAll(t => t.RelPhid == mst.PhId);
                        }
                        if(projectDtlBudgetDtls != null && projectDtlBudgetDtls.Count > 0)
                        {
                            foreach(var dtl in projectDtlBudgetDtls)
                            {
                                if(allBudgetAccounts != null && allBudgetAccounts.Count > 0)
                                {
                                    dtl.FBudgetAccounts_EXName = allBudgetAccounts.ToList().Find(t => t.KMDM == dtl.FBudgetAccounts) == null ? "" : allBudgetAccounts.ToList().Find(t => t.KMDM == dtl.FBudgetAccounts).KMMC;
                                }
                                if (allSysSets != null && allSysSets.Count > 0)
                                {
                                    dtl.FSubitemName = allSysSets.ToList().Find(t => t.TypeCode == dtl.FSubitemCode) == null ? "" : allSysSets.ToList().Find(t => t.TypeCode == dtl.FSubitemCode).TypeName;
                                }
                            }
                            projectAllData.ProjectDtlBudgetDtls = projectDtlBudgetDtls.FindAll(t => t.MstPhid == mst.PhId).OrderBy(t=>t.FDtlCode).ToList();
                            projectAllData.ProjectDtlBudgetDtls2 = projectAllData.ProjectDtlBudgetDtls;
                        }
                        if (projectDtlFundAppls != null && projectDtlFundAppls.Count > 0)
                        {
                            projectAllData.ProjectDtlFundAppls = projectDtlFundAppls.FindAll(t => t.MstPhid == mst.PhId);
                        }
                        if (projectDtlImplPlans != null && projectDtlImplPlans.Count > 0)
                        {
                            projectAllData.ProjectDtlImplPlans = projectDtlImplPlans.FindAll(t => t.MstPhid == mst.PhId);
                        }
                        if (projectDtlPerformTargets != null && projectDtlPerformTargets.Count > 0)
                        {
                            projectAllData.ProjectDtlPerformTargets = projectDtlPerformTargets.FindAll(t => t.MstPhid == mst.PhId);
                        }
                        if (projectDtlPersonnels != null && projectDtlPersonnels.Count > 0)
                        {
                            projectAllData.ProjectDtlPersonnels = projectDtlPersonnels.FindAll(t => t.MstPhid == mst.PhId);
                        }
                        if (projectDtlPurchaseDtls != null && projectDtlPurchaseDtls.Count > 0)
                        {
                            projectAllData.ProjectDtlPurchaseDtls = projectDtlPurchaseDtls.FindAll(t => t.MstPhid == mst.PhId);
                        }
                        if (projectDtlPurDtl4SOFs != null && projectDtlPurDtl4SOFs.Count > 0)
                        {
                            projectAllData.ProjectDtlPurDtl4SOFs = projectDtlPurDtl4SOFs.FindAll(t => t.MstPhid == mst.PhId);
                        }
                        if (projectDtlTextContents != null && projectDtlTextContents.Count > 0)
                        {
                            projectAllData.ProjectDtlTextContents = projectDtlTextContents.Find(t => t.MstPhid == mst.PhId);
                        }
                        if(projectDtlPersonNames != null && projectDtlPersonNames.Count > 0)
                        {
                            projectAllData.ProjectDtlPersonNames = projectDtlPersonNames.FindAll(t => t.MstPhid == mst.PhId);
                        }
                        projectAllDatas.Add(projectAllData);
                    }
                }
            }
            return projectAllDatas;
        }

        /// <summary>
        /// 根据主键集合生成相应的预算集合
        /// </summary>
        /// <param name="phids">主键集合</param>
        /// <returns></returns>
        public SavedResult<long> SaveMSHYBudgetMst(List<long> phids)
        {
            //存储保存结果
            SavedResult<long> savedResult = new SavedResult<long>();
            //先把这些主键集合对应的项目数据都查找出来
            List<ProjectMstModel> allProjects = new List<ProjectMstModel>();
            allProjects = this.ProjectMstFacade.Find(t => phids.Contains(t.PhId)).Data.ToList();
            //明细数据集合
            List<ProjectDtlBudgetDtlModel> allProjectDtlBudgets = new List<ProjectDtlBudgetDtlModel>();
            allProjectDtlBudgets = this.ProjectDtlBudgetDtlFacade.Find(t => phids.Contains(t.MstPhid)).Data.ToList();
            //人员分摊数据集合
            List<ProjectDtlPersonnelModel> allProjectDtlPersonnels = new List<ProjectDtlPersonnelModel>();
            allProjectDtlPersonnels = this.ProjectDtlPersonnelFacade.Find(t => phids.Contains(t.MstPhid)).Data.ToList();
            //追加人员名单集合
            List<ProjectDtlPersonNameModel> allProjectDtlPersonNames = new List<ProjectDtlPersonNameModel>();
            allProjectDtlPersonNames = this.ProjectDtlPersonNameFacade.Find(t => phids.Contains(t.MstPhid)).Data.ToList();

            if (phids != null && phids.Count > 0)
            {
                foreach (var phid in phids)
                {
                    ProjectMstModel projectMst = new ProjectMstModel();
                    if (allProjects != null && allProjects.Count > 0)
                    {
                        projectMst = allProjects.Find(t => t.PhId == phid);
                    }
                    if (projectMst != null && projectMst.PhId != 0)
                    {

                        //单条单据对应的明细
                        List<ProjectDtlBudgetDtlModel> projectDtlBudgetDtls = new List<ProjectDtlBudgetDtlModel>();
                        if (allProjectDtlBudgets != null && allProjectDtlBudgets.Count > 0)
                        {
                            projectDtlBudgetDtls = allProjectDtlBudgets.ToList().FindAll(t => t.MstPhid == phid);
                        }
                        //单条单据对应的人员分摊
                        List<ProjectDtlPersonnelModel> projectDtlPersonnels = new List<ProjectDtlPersonnelModel>();
                        if (allProjectDtlPersonnels != null && allProjectDtlPersonnels.Count > 0)
                        {
                            projectDtlPersonnels = allProjectDtlPersonnels.ToList().FindAll(t => t.MstPhid == phid);
                        }
                        //单条单据对应的追加人员名单
                        List<ProjectDtlPersonNameModel> projectDtlPersonNames = new List<ProjectDtlPersonNameModel>();
                        if(allProjectDtlPersonNames != null && allProjectDtlPersonNames.Count > 0)
                        {
                            projectDtlPersonNames = allProjectDtlPersonNames.ToList().FindAll(t => t.MstPhid == phid);
                        }

                        BudgetMstModel budgetmst = ModelChange<ProjectMstModel, BudgetMstModel>(projectMst);
                        decimal FBudgetAmount = 0;
                        //budgetmst.FDeclarer = projectMst.fde;
                        budgetmst.FDateofDeclaration = DateTime.Now;
                        //budgetmst.FMeetingTime = null;
                        //budgetmst.FMeetiingSummaryNo = "";
                        budgetmst.FProjStatus = 3;
                        budgetmst.FApproveStatus = "3";
                        budgetmst.XmMstPhid = projectMst.PhId;
                        // budgetmst.FApproveStatus ="3";  //项目生成预算审批状态不变
                        //查找预算单位下该部门所处预算进度

                        //vue改造 写死c0001
                        budgetmst.FType = "c";
                        budgetmst.FVerNo = "0001";

                        budgetmst.FMidYearChange = "0";
                        budgetmst.FBudgetAmount = budgetmst.FProjAmount;
                        budgetmst.PersistentState = PersistentState.Added;

                        //model.PersistentState = PersistentState.Added;
                        // budgetmst.Add(model);
                        //置空
                        budgetmst.FMeetingTime = null;
                        budgetmst.FMeetiingSummaryNo = "";

                        //项目明细转预算明细
                        var budgetdtlbudgetdtl = new List<BudgetDtlBudgetDtlModel>();
                        //var oldxm3BudgetDtl = new List<ProjectDtlBudgetDtlModel>();
                        if(projectDtlBudgetDtls != null && projectDtlBudgetDtls.Count > 0)
                        {
                            foreach (var item in projectDtlBudgetDtls)
                            {
                                var model = ModelChange<ProjectDtlBudgetDtlModel, BudgetDtlBudgetDtlModel>(item);
                                model.Xm3_DtlPhid = item.PhId;
                                model.FBudgetAmount = item.FAmount;
                                model.PersistentState = PersistentState.Added;
                                budgetdtlbudgetdtl.Add(model);
                                item.FBudgetAmount = item.FAmount;  //生成预算时回填项目里的预算金额
                                item.PersistentState = PersistentState.Modified;
                                FBudgetAmount += item.FAmount;
                            }
                        }
                        //项目人员分摊转预算人员分摊
                        List<BudgetDtlPersonnelModel> budgetDtlPersonnels = new List<BudgetDtlPersonnelModel>();
                        if(projectDtlPersonnels != null && projectDtlPersonnels.Count > 0)
                        {
                            foreach(var item in projectDtlPersonnels)
                            {
                                var model = ModelChange<ProjectDtlPersonnelModel, BudgetDtlPersonnelModel>(item);
                                model.XmPhId = item.PhId;
                                model.PersistentState = PersistentState.Added;
                                budgetDtlPersonnels.Add(model);
                            }
                        }

                        //项目人员分摊转预算人员分摊
                        List<BudgetDtlPersonNameModel> budgetDtlPersonNames = new List<BudgetDtlPersonNameModel>();
                        if (projectDtlPersonNames != null && projectDtlPersonNames.Count > 0)
                        {
                            foreach (var item in projectDtlPersonNames)
                            {
                                var model = ModelChange<ProjectDtlPersonNameModel, BudgetDtlPersonNameModel>(item);
                                model.XmPhId = item.PhId;
                                model.PersistentState = PersistentState.Added;
                                budgetDtlPersonNames.Add(model);
                            }
                        }

                        savedResult = BudgetMstFacade.SaveBudgetMst(budgetmst, null, null, null, budgetdtlbudgetdtl, null, null, null, budgetDtlPersonnels, budgetDtlPersonNames);

                        if (savedResult.Status == ResponseStatus.Success) //生成成功后改变项目状态
                        {
                            projectMst.FBudgetAmount = FBudgetAmount;
                            projectMst.FProjStatus = 3;
                            projectMst.FApproveStatus = "3";
                            projectMst.PersistentState = PersistentState.Modified;
                            this.ProjectMstFacade.SaveProjectMst(projectMst, null, null, null, null, null, projectDtlBudgetDtls, null);
                        }
                    }
                    else
                    {
                        throw new Exception("生成预算的单据不存在！");
                    }
                }

                //同步数据到老G6H
                List<string> phidList = new List<string>();
                foreach(var phid in phids)
                {
                    phidList.Add(phid.ToString());
                }
                ProjectMstFacade.AddData(phidList.ToArray());
            }
            return savedResult;
        }

        /// <summary>
        /// model 转换
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static T2 ModelChange<T1, T2>(T1 model)
        {
            if (model == null)
            {
                T2 obj1 = Activator.CreateInstance<T2>();
                return obj1;
            }
            Type t = model.GetType();   //获取传过来的实例类型
            var fullname = t.FullName; //获取类型名称
                                       //ProjectMstModel obj1 = new ProjectMstModel();

            // ProjectMstModel obj = Activator.CreateInstance<ProjectMstModel>();
            PropertyInfo[] PropertyList = t.GetProperties(); //获取实例的属性

            T2 obj = Activator.CreateInstance<T2>(); //创建T2映射的实例
            Type o = obj.GetType();
            PropertyInfo[] objList = o.GetProperties();
            foreach (PropertyInfo item in PropertyList)
            {
                //var name = item.Name;
                //var value = item.GetValue(model, null); //获取item在model中的值
                if (item.DeclaringType.FullName != fullname)  //；类型名称不同的跳过
                {
                    continue;
                }
                foreach (var ob in objList) //循环查找相同属性名并赋值
                {
                    if (ob.Name.ToLower() == item.Name.ToLower() && ob.PropertyType.Name == item.PropertyType.Name)  //名称相同且属性相同的赋值
                    {
                        ob.SetValue(obj, item.GetValue(model, null), null);
                        break;
                    }
                }
            }
            return obj; //返回转换后并赋值的model
        }

        /// <summary>
        /// 保存人员分摊数据
        /// </summary>
        /// <param name="projectDtlPersonnels">人员分摊数据集合</param>
        /// <param name="phid">主键id</param>
        /// <returns></returns>
        public SavedResult<long> SaveMSYHPersonnels(List<ProjectDtlPersonnelModel> projectDtlPersonnels, long phid)
        {
            //SavedResult<long> savedResult = new SavedResult<long>();
            //savedResult = this.ProjectDtlPersonnelFacade.Save<long>(projectDtlPersonnels, phid.ToString());
            return this.ProjectDtlPersonnelFacade.SaveMSYHPersonnels(projectDtlPersonnels, phid);
        }

        /// <summary>
        /// 保存项目内容信息
        /// </summary>
        /// <param name="projectDtlTextContent">项目内容信息对象</param>
        /// <returns></returns>
        public SavedResult<long> SaveMSYHTextContent(ProjectDtlTextContentModel projectDtlTextContent)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            //先判断此申报人该年度该组织下的内容信息是否存在
            var texts = this.ProjectDtlTextContentFacade.Find(t => t.FYear == projectDtlTextContent.FYear && t.FDeclarationUnit == projectDtlTextContent.FDeclarationUnit && t.FDeclarerId == projectDtlTextContent.FDeclarerId).Data;
            if(texts != null && texts.Count > 0)
            {
                projectDtlTextContent.PhId = texts[0].PhId;
                projectDtlTextContent.PersistentState = PersistentState.Modified;
            }
            else
            {
                projectDtlTextContent.PhId = (long)0;
                projectDtlTextContent.PersistentState = PersistentState.Added;
            }
            savedResult = this.ProjectDtlTextContentFacade.Save<long>(projectDtlTextContent);
            return savedResult;
        }

        /// <summary>
        /// 根据组织，年份与用户id获取对应的内容信息
        /// </summary>
        /// <param name="orgCode">组织编码</param>
        /// <param name="year">年份</param>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public ProjectDtlTextContentModel GetMSYHTextContent(string orgCode, string year, long userId)
        {
            ProjectDtlTextContentModel projectDtlTextContent = new ProjectDtlTextContentModel();
            var contents = this.ProjectDtlTextContentFacade.Find(t => t.FDeclarationUnit == orgCode && t.FYear == year && t.FDeclarerId == userId).Data;
            if(contents != null && contents.Count > 0)
            {
                projectDtlTextContent = contents[0];
            }
            return projectDtlTextContent;
        }

        /// <summary>
        /// 根据单据主键获取维护人员信息
        /// </summary>
        /// <param name="phid">单据主键</param>
        /// <returns></returns>
        public IList<ProjectDtlPersonNameModel> GetMSYHPersonNames(long phid)
        {
            IList<ProjectDtlPersonNameModel> projectDtlPersonNames = new List<ProjectDtlPersonNameModel>();
            //已存在的关联人员信息
            projectDtlPersonNames = this.ProjectDtlPersonNameFacade.Find(t => t.MstPhid == phid).Data;

            IList<ProjectDtlPersonNameModel> newProjectDtlPersonNames = new List<ProjectDtlPersonNameModel>();
            //主表信息
            var projectMsts = this.ProjectMstFacade.Find(t => t.PhId == phid).Data;
            if(projectMsts != null && projectMsts.Count > 0)
            {
                ProjectMstModel projectMst = new ProjectMstModel();
                projectMst = projectMsts[0];
                //获取组织信息与费用归属部门信息
                List<string> codes = new List<string>();
                codes.Add(projectMst.FDeclarationUnit);
                codes.Add(projectMst.FBudgetDept);
                var orgs = this.OrganizationFacade.Find(t => codes.Contains(t.OCode)).Data;
                if(orgs != null && orgs.Count > 0)
                {
                    //申报单位对应的组织信息
                    OrganizeModel organize1 = orgs.ToList().Find(t => t.OCode == projectMst.FDeclarationUnit);
                    //费用归属部门对应的组织信息
                    OrganizeModel organize2 = orgs.ToList().Find(t => t.OCode == projectMst.FBudgetDept);

                    if(organize1 != null && organize2 != null)
                    {
                        //组织对象用户信息
                        var users1 = this.UserOrgFacade.Find(t => t.OrgId == organize1.PhId).Data;
                        //费用归属部门对应的用户信息
                        var users2 = this.CorrespondenceSettingsFacade.Find(t => t.Dylx == "97" && t.Dydm == projectMst.FBudgetDept).Data;
                        int i = 0;
                        //必须要加入集合的数据
                        if(users2 != null && users2.Count > 0)
                        {
                            List<string> userCodes2 = users2.Select(t => t.Dwdm).Distinct().ToList();
                            //用户集合
                            var userList2 = this.UserFacade.Find(t => userCodes2.Contains(t.UserNo)).Data;
                            if(userList2 != null && userList2.Count > 0)
                            {
                                foreach (var user in userList2)
                                {
                                    ProjectDtlPersonNameModel projectDtlPerson = new ProjectDtlPersonNameModel();
                                    if(projectDtlPersonNames!= null && projectDtlPersonNames.Count > 0)
                                    {
                                        if(projectDtlPersonNames.ToList().Find(t=>t.FOrgId == organize1.PhId && t.FUserId == user.PhId) != null)
                                        {
                                            projectDtlPerson = projectDtlPersonNames.ToList().Find(t => t.FOrgId == organize1.PhId && t.FUserId == user.PhId);
                                            projectDtlPerson.FOrgName = organize1.OName;
                                            projectDtlPerson.FUserName = user.UserName;
                                        }
                                        else
                                        {
                                            projectDtlPerson.MstPhid = phid;
                                            projectDtlPerson.FOrgId = organize1.PhId;
                                            projectDtlPerson.FOrgCode = organize1.OCode;
                                            projectDtlPerson.FOrgName = organize1.OName;
                                            projectDtlPerson.FUserId = user.PhId;
                                            projectDtlPerson.FUserCode = user.UserNo;
                                            projectDtlPerson.FUserName = user.UserName;
                                        }
                                    }
                                    else
                                    {
                                        projectDtlPerson.MstPhid = phid;
                                        projectDtlPerson.FOrgId = organize1.PhId;
                                        projectDtlPerson.FOrgCode = organize1.OCode;
                                        projectDtlPerson.FOrgName = organize1.OName;
                                        projectDtlPerson.FUserId = user.PhId;
                                        projectDtlPerson.FUserCode = user.UserNo;
                                        projectDtlPerson.FUserName = user.UserName;
                                    }
                                    projectDtlPerson.SortCode = i;
                                    i++;
                                    projectDtlPerson.IsFix = 1;
                                    projectDtlPerson.IsRelation = 1;
                                    newProjectDtlPersonNames.Add(projectDtlPerson);
                                }
                            }                         
                        }

                        if (users1 != null && users1.Count > 0)
                        {
                            List<long> userCodes1 = users1.Select(t => t.UserId).Distinct().ToList();
                            //用户集合
                            var userList1 = this.UserFacade.Find(t => userCodes1.Contains(t.PhId)).Data;
                            if (userList1 != null && userList1.Count > 0)
                            {
                                foreach (var user in userList1)
                                {
                                    if (users2 != null && users2.Count > 0)
                                    {
                                        if(users2.ToList().Find(t=>t.Dwdm == user.UserNo) != null)
                                        {
                                            continue;
                                        }
                                    }
                                    ProjectDtlPersonNameModel projectDtlPerson = new ProjectDtlPersonNameModel();
                                    if (projectDtlPersonNames != null && projectDtlPersonNames.Count > 0)
                                    {
                                        if (projectDtlPersonNames.ToList().Find(t => t.FOrgId == organize1.PhId && t.FUserId == user.PhId) != null)
                                        {
                                            projectDtlPerson = projectDtlPersonNames.ToList().Find(t => t.FOrgId == organize1.PhId && t.FUserId == user.PhId);
                                            projectDtlPerson.IsFix = 0;
                                            projectDtlPerson.IsRelation = 1;
                                            projectDtlPerson.FOrgName = organize1.OName;
                                            projectDtlPerson.FUserName = user.UserName;
                                        }
                                        else
                                        {
                                            projectDtlPerson.MstPhid = phid;
                                            projectDtlPerson.FOrgId = organize1.PhId;
                                            projectDtlPerson.FOrgCode = organize1.OCode;
                                            projectDtlPerson.FOrgName = organize1.OName;
                                            projectDtlPerson.FUserId = user.PhId;
                                            projectDtlPerson.FUserCode = user.UserNo;
                                            projectDtlPerson.FUserName = user.UserName;
                                            projectDtlPerson.IsFix = 0;
                                            projectDtlPerson.IsRelation = 0;
                                        }
                                    }
                                    else
                                    {
                                        projectDtlPerson.MstPhid = phid;
                                        projectDtlPerson.FOrgId = organize1.PhId;
                                        projectDtlPerson.FOrgCode = organize1.OCode;
                                        projectDtlPerson.FOrgName = organize1.OName;
                                        projectDtlPerson.FUserId = user.PhId;
                                        projectDtlPerson.FUserCode = user.UserNo;
                                        projectDtlPerson.FUserName = user.UserName;
                                        projectDtlPerson.IsFix = 0;
                                        projectDtlPerson.IsRelation = 0;
                                    }
                                    projectDtlPerson.SortCode = i;
                                    i++;
                                    newProjectDtlPersonNames.Add(projectDtlPerson);
                                }
                            }
                        }
                        return newProjectDtlPersonNames;
                    }
                    else
                    {
                        throw new Exception("组织信息查询失败！");
                    }
                }
                else
                {
                    throw new Exception("组织信息查询失败！");
                }
            }
            else
            {
                throw new Exception("单据查询失败！");
            }
        }

        /// <summary>
        /// 保存维护人员集合
        /// </summary>
        /// <param name="projectDtlPersonNames">人员集合</param>
        /// <param name="phid">单据主键</param>
        /// <returns></returns>
        public SavedResult<long> SaveMSYHPersonNames(List<ProjectDtlPersonNameModel> projectDtlPersonNames, long phid)
        {
            //SavedResult<long> savedResult = new SavedResult<long>();
            //savedResult = this.ProjectDtlPersonNameFacade.Save<long>(projectDtlPersonNames, phid.ToString());
            return this.ProjectDtlPersonNameFacade.SaveMSYHPersonNames(projectDtlPersonNames, phid);
        }

        /// <summary>
        /// 根据单据主键集合获取相应的项目材料集合
        /// </summary>
        /// <param name="phids">单据主键集合</param>
        /// <returns></returns>
        public IList<ProjectDtlTextContentModel> GetProjectDtlTextContents(List<long> phids)
        {
            IList<ProjectDtlTextContentModel> projectDtlTextContents = new List<ProjectDtlTextContentModel>();
            projectDtlTextContents = this.ProjectDtlTextContentFacade.Find(t => phids.Contains(t.MstPhid)).Data;
            return projectDtlTextContents;
        }

        /// <summary>
        /// 根据选中的项目分发数据导出模板
        /// </summary>
        /// <returns></returns>
        public string ExportXMData(List<QtXmDistributeModel> data)
        {
            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            var rowNumber = 0;
            
            ISheet sheet = book.CreateSheet("支出预算");
            //合并第一行A-M(前两个数字是行号 后两个是列号)
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 12));
            IRow row = sheet.CreateRow(rowNumber);
            //row.Height = 20 * 20;
            ICell cell = row.CreateCell(0);
            cell.SetCellValue("支出预算数据导入模板");
            ICellStyle cellTitleStyle = ExcelHelper.CreateStyle(book, HorizontalAlignment.Center, VerticalAlignment.Center, 500, 12, false);
            cell.CellStyle = cellTitleStyle;
            rowNumber++;

            //合并第二行A-M(前两个数字是行号 后两个是列号)
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 12));
            row = sheet.CreateRow(rowNumber);
            //row.Height = 20 * 20;
            cell = row.CreateCell(0);
            cell.SetCellValue("说明：支出预算数据从第四行开始填写。实际使用时如有不需要填报的列，可以为空，但不能删除；同一项目编码的费用金额之和，应等于项目金额。");
            ICellStyle cellTitleStyle2 = ExcelHelper.CreateStyle3(book, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 11, false);
            //cell.CellStyle = cellTitleStyle;
            rowNumber++;

            //第三行表头
            row = sheet.CreateRow(rowNumber);
            ICellStyle cellTitleStyle3 = ExcelHelper.CreateStyle(book, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 11, false);
            cell = row.CreateCell(0);
            cell.SetCellValue("项目编码（必填）");
            cell.CellStyle = cellTitleStyle3;
            cell = row.CreateCell(1);
            cell.SetCellValue("项目名称（必填）");
            cell.CellStyle = cellTitleStyle3;
            cell = row.CreateCell(2);
            cell.SetCellValue("项目金额（必填）");
            cell.CellStyle = cellTitleStyle3;
            cell = row.CreateCell(3);
            cell.SetCellValue("业务条线（必填）");
            cell.CellStyle = cellTitleStyle3;
            cell = row.CreateCell(4);
            cell.SetCellValue("费用归属（必填）");
            cell.CellStyle = cellTitleStyle3;
            cell = row.CreateCell(5);
            cell.SetCellValue("费用说明（必填）");
            cell.CellStyle = cellTitleStyle3;
            cell = row.CreateCell(6);
            cell.SetCellValue("费用金额（必填）");
            cell.CellStyle = cellTitleStyle3;
            cell = row.CreateCell(7);
            cell.SetCellValue("科目名称（选填）");
            cell.CellStyle = cellTitleStyle3;
            cell = row.CreateCell(8);
            cell.SetCellValue("支出分项名称（选填）");
            cell.CellStyle = cellTitleStyle3;
            cell = row.CreateCell(9);
            cell.SetCellValue("是否申请补助（选填）");
            cell.CellStyle = cellTitleStyle3;
            cell = row.CreateCell(10);
            cell.SetCellValue("是否集中采购（选填）");
            cell.CellStyle = cellTitleStyle3;
            cell = row.CreateCell(11);
            cell.SetCellValue("是否必须签报列支（选填）");
            cell.CellStyle = cellTitleStyle3;
            cell = row.CreateCell(12);
            cell.SetCellValue("是否集体决议（选填）");
            cell.CellStyle = cellTitleStyle3;
            rowNumber++;
            if (data != null && data.Count > 0)
            {
                for (var i = 0; i < data.Count; i++)
                {
                    row = sheet.CreateRow(rowNumber);
                    cell = row.CreateCell(0);
                    cell.SetCellValue(data[i].FProjcode);
                    cell.CellStyle = cellTitleStyle3;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(data[i].FProjname);
                    cell.CellStyle = cellTitleStyle3;
                    rowNumber++;
                }
            }

            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            var buf = ms.ToArray();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\\DownLoadFiles\\ProjectZCYS";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            using (FileStream fs = File.OpenWrite(path + "\\" + filename))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
                fs.Close();
            }
            book = null;
            ms.Close();
            ms.Dispose();

            return JsonConvert.SerializeObject(new { path = "ProjectZCYS", filename = filename });


        }

        /// <summary>
        /// 模板导入用户数据
        /// </summary>
        /// <param name="uploadPath"></param>
        /// <returns></returns>
        public string PostImportXMData(string uploadPath,string extension)
        {
            //读取excel表格里面的数据,验证用户是否重复
            using (FileStream fs = File.OpenRead(uploadPath))
            {
                IWorkbook workbook = null;

                //2003版本(BaseDll下面的dll版本有问题,没办法创建.xlsx的Excel文件对象)
                if (".xls".Equals(extension))
                {
                    workbook = new HSSFWorkbook(fs);
                }
                if (workbook != null)
                {
                    //读取第一个sheet页
                    ISheet sheet = workbook.GetSheetAt(0);
                    if (sheet != null)
                    {
                        //获取总行数
                        int rowCount = sheet.LastRowNum - 2;
                        IRow firstRow = sheet.GetRow(0);
                        //获取总列数
                        int columnCount = firstRow.LastCellNum;

                        /*IRow row = sheet.GetRow(0);
                        ICell head = row.GetCell(0);
                        string headStr = head.StringCellValue.Trim();
                        if (!"操作员批量导入模板".Equals(headStr))
                        {
                            var data = new
                            {
                                Status = ResponseStatus.Error,
                                Msg = "导入模板不正确！"
                            };
                            return DataConverterHelper.SerializeObject(data);
                        }*/
                        Dictionary<string, object> users = new Dictionary<string, object>();
                        if (rowCount > 0)
                        {
                            IRow row;
                            ICell cell1;
                            //主表
                            string FProjCode;
                            string FProjName;
                            Decimal FProjAmount;
                            string FBusinessCode;
                            string FBudgetDept;//费用归属
                            byte FIsApply;//是否申请补助
                            byte FIsPurchase;//是否集中采购
                            byte FIsResolution;//是否集体决议
                            //明细表
                            string FName;//费用说明
                            Decimal FBudgetAmount;//费用金额
                            string FBudgetAccounts;
                            string FSubitemCode;//支出分项
                            List<ProjectMstModel> Msts = new List<ProjectMstModel>();
                            List<ProjectDtlBudgetDtlModel> Dtls = new List<ProjectDtlBudgetDtlModel>();
                            for (int i = 0; i < rowCount; i++)
                            {
                                row = sheet.GetRow(i + 3);
                                //如果row为空，继续下一次循环
                                if (row == null)
                                    continue;

                                cell1 = row.GetCell(0);
                                FProjCode = cell1.StringCellValue.Trim();
                                cell1 = row.GetCell(1);
                                FProjName = cell1.StringCellValue.Trim();
                                cell1 = row.GetCell(2);
                                FProjAmount = Decimal.Parse(cell1.StringCellValue.Trim());
                                cell1 = row.GetCell(3);
                                FBusinessCode = cell1.StringCellValue.Trim();
                                cell1 = row.GetCell(4);
                                FBudgetDept = cell1.StringCellValue.Trim();
                                cell1 = row.GetCell(5);
                                FName = cell1.StringCellValue.Trim();
                                cell1 = row.GetCell(6);
                                FBudgetAmount = Decimal.Parse(cell1.StringCellValue.Trim());
                                cell1 = row.GetCell(7);
                                FBudgetAccounts = cell1.StringCellValue.Trim();
                                cell1 = row.GetCell(8);
                                FSubitemCode = cell1.StringCellValue.Trim();
                                cell1 = row.GetCell(9);
                                FSubitemCode = cell1.StringCellValue.Trim();
                                cell1 = row.GetCell(10);
                                FIsApply = byte.Parse(cell1.StringCellValue.Trim());
                                cell1 = row.GetCell(11);
                                FIsPurchase = byte.Parse(cell1.StringCellValue.Trim());
                                cell1 = row.GetCell(11);
                                FIsPurchase = byte.Parse(cell1.StringCellValue.Trim());
                                cell1 = row.GetCell(12);
                                FIsResolution = byte.Parse(cell1.StringCellValue.Trim());

                                //默认项目代码相同的为一个项目
                                if (!string.IsNullOrEmpty(FProjCode))
                                {

                                }
                                else
                                {

                                }


                                //判断当前单元格内容是否为空
                                /*if (!String.IsNullOrEmpty(operatorCode))
                                {
                                    List<SysUserOrganizeRoleRecordModel> relations = SysUserService.FindRelationByOperatorCode(operatorCode);
                                    if (!users.ContainsKey(operatorCode))
                                    {
                                        users.Add(operatorCode, null);

                                        if (relations != null && relations.Count > 0)
                                        {
                                            var data = new
                                            {
                                                Status = ResponseStatus.Success,
                                                Msg = "第" + (i + 5) + "行操作员编码重复！",
                                                UploadPath = uploadPath,
                                                IsRepeat = 1
                                            };
                                            return DataConverterHelper.SerializeObject(data);
                                        }
                                    }
                                    else
                                    {
                                        var data = new
                                        {
                                            Status = ResponseStatus.Success,
                                            Msg = "第" + (i + 5) + "行操作员编码重复！",
                                            UploadPath = uploadPath,
                                            IsRepeat = 1
                                        };
                                        return DataConverterHelper.SerializeObject(data);
                                    }
                                }
                                else
                                {
                                    var data = new
                                    {
                                        Status = ResponseStatus.Success,
                                        Msg = "操作员编码不重复！",
                                        UploadPath = uploadPath,
                                        IsRepeat = 0
                                    };
                                    return DataConverterHelper.SerializeObject(data);
                                }*/
                            }
                        }
                        else
                        {
                            var data = new
                            {
                                Status = ResponseStatus.Error,
                                Msg = "导入模板内不包含任何数据！"
                            };
                            return DataConverterHelper.SerializeObject(data);
                        }

                    }
                }
            }

            return "";
        }

        #region//签报单相关
        //public XmReportMstModel GetMSYHProjectReport(long phid)
        //{
        //    XmReportMstModel xmReportMst = new XmReportMstModel();

        //}

        #endregion
        #endregion


    }
}

