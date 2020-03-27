#region Summary
/**************************************************************************************
    * 类 名 称：        IProjectMstService
    * 命名空间：        GXM3.XM.Service.Interface
    * 文 件 名：        IProjectMstService.cs
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
using System.Reflection;
using System.Text;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;
using GQT3.QT.Model.Domain;
using GXM3.XM.Model;
using GXM3.XM.Model.Domain;
using GXM3.XM.Model.Extra;
using GYS3.YS.Model.Domain;
using SUP.Common.Base;

namespace GXM3.XM.Service.Interface
{
    /// <summary>
    /// ProjectMst服务组装层接口
    /// </summary>
    public partial interface IProjectMstService : IEntServiceBase<ProjectMstModel>
    {
        #region IProjectMstService 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<ProjectMstModel> ExampleMethod<ProjectMstModel>(string param)

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="projectMstEntity"></param>
        /// <param name="projectDtlImplPlanEntities"></param>
        /// <param name="projectDtlTextContentEntities"></param>
        /// <param name="projectDtlFundApplEntities"></param>
        /// <param name="projectDtlBudgetDtlEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveProjectMst(ProjectMstModel projectMstEntity, List<ProjectDtlImplPlanModel> projectDtlImplPlanEntities, List<ProjectDtlTextContentModel> projectDtlTextContentEntities, List<ProjectDtlFundApplModel> projectDtlFundApplEntities, List<ProjectDtlBudgetDtlModel> projectDtlBudgetDtlEntities);

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
        SavedResult<Int64> SaveProjectMst(ProjectMstModel projectMstEntity, List<ProjectDtlTextContentModel> projectDtlTextContentEntities, List<ProjectDtlPurchaseDtlModel> projectDtlPurchaseDtlEntities, List<ProjectDtlPurDtl4SOFModel> projectDtlPurDtl4SOFEntities, List<ProjectDtlPerformTargetModel> projectDtlPerformTargetEntities, List<ProjectDtlFundApplModel> projectDtlFundApplEntities, List<ProjectDtlBudgetDtlModel> projectDtlBudgetDtlEntities, List<ProjectDtlImplPlanModel> projectDtlImplPlanEntities);


        /// <summary>
        /// 通过外键值获取ProjectDtlImplPlan明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<ProjectDtlImplPlanModel> FindProjectDtlImplPlanByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取ProjectDtlTextContent明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<ProjectDtlTextContentModel> FindProjectDtlTextContentByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取ProjectDtlFundAppl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<ProjectDtlFundApplModel> FindProjectDtlFundApplByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取ProjectDtlBudgetDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<ProjectDtlBudgetDtlModel> FindProjectDtlBudgetDtlByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取ProjectDtlPersonnel明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<ProjectDtlPersonnelModel> FindProjectDtlPersonnelByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取ProjectDtlPersonName明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<ProjectDtlPersonNameModel> FindProjectDtlPersonNameByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 返回对应Phid的明细记录
        /// </summary>
        /// <typeparam name="Int64"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        FindedResult<ProjectDtlBudgetDtlModel> FindProjectDtlBudgetDtlByPhID<Int64>(Int64 id);

        /// <summary>
        /// 获取首页页面的 条数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="ng3_logid">当前登录人</param>
        /// <param name="usercode">账号</param>
        /// <returns></returns>
        ProjectCountModel GetDataCount(int pageIndex, int pageSize, long ng3_logid, string usercode, string sessionFYear);

        /// <summary>
        /// 通过代码获取ProjectMstModel
        /// </summary>
        /// <param name="dm">代码</param>
        /// <returns></returns>
        FindedResults<ProjectMstModel> FindProjectMst(string dm);

        FindedResults<ProjectMstModel> FindProjectMstByProperty<T>(T Types, String propertyName);

        /// <summary>
        /// ProjectDtlBudgetDtlModel
        /// </summary>
        /// <param name="dm">代码</param>
        /// <returns></returns>
        FindedResults<ProjectDtlBudgetDtlModel> FindPaymentMethod(string dm);

        /// <summary>
        /// 通过资金来源代码获取ProjectDtlBudgetDtl明细数据
        /// </summary>
        /// <param name="ZJDM">资金来源代码</param>
        /// <returns></returns>
        FindedResults<ProjectDtlBudgetDtlModel> FindProjectDtlBudgetDtlMstByZJDM(string ZJDM);


        /// <summary>
        /// 更改项目状态,项目状态更改成“单位备选”时,删除当前预算，并把对应项目的状态改为“单位备选”
        /// </summary>
        /// <param name="phid"></param>
        void UpdateFProjStatus(long phid);

        /// <summary>
        /// 预算根据明细表主键回填预算金额
        /// </summary>
        /// <param name="phid"></param>
        /// <param name="FAmount"></param>
        void UpdateDtlFBudgetAmount(long[] phid, decimal[] FAmount);

        /// <summary>
        /// 获取最大项目库编码
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        string CreateOrGetMaxProjCode(string year);
        /// <summary>
        /// 生成预算时回填明细
        /// </summary>
        /// <param name="oldxm3BudgetDtl"></param>
        void UpdateBudgetDtlList(List<ProjectDtlBudgetDtlModel> oldxm3BudgetDtl);

        /// <summary>
        /// 通过外键值获取ProjectDtlPerformTarget明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<ProjectDtlPerformTargetModel> FindProjectDtlPerformTargetByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取ProjectDtlPurchaseDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<ProjectDtlPurchaseDtlModel> FindProjectDtlPurchaseDtlByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过采购目录代码获取ProjectDtlPurchaseDtl明细数据
        /// </summary>
        /// <param name="code">采购目录代码</param>
        /// <returns></returns>
        FindedResults<ProjectDtlPurchaseDtlModel> FindProjectDtlPurchaseDtlByCatalogCode<TValType>(TValType code);

        /// <summary>
        /// 筛选ProjectDtlPurchaseDtl明细数据通用方法
        /// </summary>
        /// <param name="code">属性代码</param>
        /// /// <param name="Pname">属性名称</param>
        /// <returns></returns>
        FindedResults<ProjectDtlPurchaseDtlModel> FindProjectDtlPurchaseDtlByAnyCode<TValType>(TValType code, string Pname);

        /// <summary>
        /// 通过外键值获取ProjectDtlPurDtl4SOF明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<ProjectDtlPurDtl4SOFModel> FindProjectDtlPurDtl4SOFByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过指标代码获取ProjectDtlPerformTarget明细数据
        /// </summary>
        /// <param name="FTargetCode">指标代码</param>
        /// <returns></returns>
        FindedResults<ProjectDtlPerformTargetModel> FindProjectDtlPerformTargetByFTargetCode(string FTargetCode);

        /// <summary>
        /// 通过采购类型代码获取ProjectDtlPurchaseDtl明细数据
        /// </summary>
        /// <param name="FTypeCode">指标代码</param>
        /// <returns></returns>
        FindedResults<ProjectDtlPurchaseDtlModel> FindProjectDtlPurchaseDtlByFTypeCode(string FTypeCode);

        /// <summary>
        /// 删除集中采购明细和资金来源
        /// </summary>
        /// <param name="id"></param>
        void DeleteProjectDtlPurchase(Int64 id);

        /// <summary>
        /// 回撤
        /// </summary>
        /// <param name="approveCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        PagedResult<ProjectMstModel> FindUnvalidPiid(string approveCode, string userId);

        /// <summary>
        /// 项目同步数据到老G6H
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        string AddData(string[] ids);

        /// <summary>
        /// 查找是否有设置功能控制
        /// </summary>
        /// <param name="BZ">功能标识</param>
        /// <param name="DWDM">组织</param>
        /// <returns></returns>
        List<CorrespondenceSettings2Model> FindQTControlSet(string BZ, string DWDM);

        /// <summary>
        /// 操作员默认部门
        /// </summary>
        /// <param name="userPhid"></param>
        /// <returns></returns>
        string GetDefaultDept(long userPhid);

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
        CommonResult SaveModify(ProjectMstModel AfterProjectMst, EntityInfo<ProjectDtlImplPlanModel> projectDtlImpls, EntityInfo<ProjectDtlTextContentModel> projectDtlTexts, EntityInfo<ProjectDtlFundApplModel> projectDtlFunds, EntityInfo<ProjectDtlBudgetDtlModel> projectDtlBudgets, EntityInfo<ProjectDtlPerformTargetModel> projectDtlPerforms, EntityInfo<ProjectDtlPurchaseDtlModel> projectDtlPurchases, EntityInfo<ProjectDtlPurDtl4SOFModel> projectDtlPurDtl4s);

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
        CommonResult SaveModify2(ProjectMstModel AfterProjectMst, ProjectDtlImplPlanModel projectDtlImpls, ProjectDtlTextContentModel projectDtlTexts, ProjectDtlFundApplModel projectDtlFunds, ProjectDtlBudgetDtlModel projectDtlBudgets, ProjectDtlPerformTargetModel projectDtlPerforms, ProjectDtlPurchaseDtlModel projectDtlPurchases, ProjectDtlPurDtl4SOFModel projectDtlPurDtl4s);

        /// <summary>
        /// 判断当前选则的模本金额跟实际录入金额的大小比较
        /// </summary>
        /// <param name="busType"></param>
        /// <param name="IndividualInfoId"></param>
        /// <param name="projAmount"></param>
        /// <param name="OrgCode"></param>
        /// <returns></returns>
        string FindIndividualInfo(string busType, long IndividualInfoId, decimal projAmount, string OrgCode);

        /// <summary>
        /// 根据项目id获取符合条件的表单
        /// </summary>
        /// <param name="busType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        string FindIndividualInfoById(string busType, long id);


        /// <summary>
        /// 通过字典取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        List<ProjectDtlBudgetDtlModel> FindProjectDtlBudgetDtl(Dictionary<string, object> dictionary);

        #endregion

        #region
        /// <summary>
        /// 根据主键集合修改作废状态
        /// </summary>
        /// <param name="phids">主键集合</param>
        /// <returns></returns>
        SavedResult<long> PostCancetProjectList(List<long> phids);

        /// <summary>
        /// 根据主键集合获取打印数据
        /// </summary>
        /// <param name="phids">主键集合</param>
        /// <returns></returns>
        List<object> PostPrintData(string[] phids);

        /// <summary>
        /// 获取新的项目绩效集合
        /// </summary>
        /// <param name="projectDtlPerformTargets">项目带的绩效集合</param>
        /// <param name="targetTypeCode">父级节点</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        List<ProjectDtlPerformTargetModel> GetNewProPerformTargets(List<ProjectDtlPerformTargetModel> projectDtlPerformTargets, string targetTypeCode, long orgId, string orgCode);
        #endregion

        #region

        /// <summary>
        /// 预立项汇总打印(省总)
        /// </summary>
        /// <param name="projectMsts">项目集合</param>
        /// <param name="qtCoverUpForOrg">套打格式</param>
        /// <param name="organize">组织</param>
        /// <param name="user">人员</param>
        /// <returns></returns>
        string ExportSummaryExcelSZ1(IList<ProjectMstModel> projectMsts, QtCoverUpForOrgModel qtCoverUpForOrg, OrganizeModel organize, User2Model user);

        /// <summary>
        /// 预立项汇总打印
        /// </summary>
        /// <param name="projectMsts">项目集合</param>
        /// <param name="qtCoverUpForOrg">套打格式</param>
        /// <param name="organize">组织</param>
        /// <param name="user">人员</param>
        /// <returns></returns>
        string ExportSummaryExcel1(IList<ProjectMstModel> projectMsts, QtCoverUpForOrgModel qtCoverUpForOrg, OrganizeModel organize, User2Model user);

        /// <summary>
        /// 立项汇总打印(省总)
        /// </summary>
        /// <param name="projectMsts">项目集合</param>
        /// <param name="qtCoverUpForOrg">套打格式</param>
        /// <param name="organize">组织</param>
        /// <param name="user">人员</param>
        /// <returns></returns>
        string ExportSummaryExcelSZ2(IList<ProjectMstModel> projectMsts, QtCoverUpForOrgModel qtCoverUpForOrg, OrganizeModel organize, User2Model user);

        /// <summary>
        /// 立项汇总打印(明细区域的)
        /// </summary>
        /// <param name="projectMsts">项目集合</param>
        /// <param name="qtCoverUpForOrg">套打格式</param>
        /// <param name="organize">组织</param>
        /// <param name="user">人员</param>
        /// <returns></returns>
        string ExportSummaryExcel2(IList<ProjectMstModel> projectMsts, QtCoverUpForOrgModel qtCoverUpForOrg, OrganizeModel organize, User2Model user);

        /// <summary>
        /// 预立项申报表打印
        /// </summary>
        /// <param name="projectMsts">项目集合</param>
        /// <param name="qtCoverUpForOrg">套打格式</param>
        /// <param name="organize">组织</param>
        /// <param name="user">人员</param>
        /// <returns></returns>
        string ExportDeclareExcel1(IList<ProjectMstModel> projectMsts, QtCoverUpForOrgModel qtCoverUpForOrg, OrganizeModel organize, User2Model user);

        /// <summary>
        /// 立项申报表打印
        /// </summary>
        /// <param name="projectMsts">项目集合</param>
        /// <param name="qtCoverUpForOrg">套打格式</param>
        /// <param name="organize">组织</param>
        /// <param name="user">人员</param>
        /// <returns></returns>
        string ExportDeclareExcel2(IList<ProjectMstModel> projectMsts, QtCoverUpForOrgModel qtCoverUpForOrg, OrganizeModel organize, User2Model user);

        /// <summary>
        /// 导出年中调整申报表(年中新增与年中调整放在同一个execl)
        /// </summary>
        /// <param name="budgetMsts">年中新增集合</param>
        /// <param name="qtCoverUpForOrg">套打格式</param>
        /// <param name="organize">组织</param>
        /// <param name="user">人员</param>
        /// <returns></returns>
        string ExportDeclareExcelTZ2(IList<BudgetMstModel> budgetMsts, QtCoverUpForOrgModel qtCoverUpForOrg, OrganizeModel organize, User2Model user);

        /// <summary>
        /// 年中调整汇总表的（按明细来显示）(年中新增与年中调整合在一起)
        /// </summary>
        /// <param name="projectMsts">项目集合</param>
        /// <param name="qtCoverUpForOrg">套打格式</param>
        /// <param name="organize">组织</param>
        /// <param name="user">人员</param>
        /// <returns></returns>
        string ExportSummaryExcelTZ(IList<BudgetMstModel> projectMsts, QtCoverUpForOrgModel qtCoverUpForOrg, OrganizeModel organize, User2Model user);
        #endregion

        #region//民生银行

        /// <summary>
        /// 根据主表数据补充明细数据
        /// </summary>
        /// <param name="projectMsts">主表数据</param>
        /// <returns></returns>
        List<ProjectAllDataModel> GetProjectAllDataModels(List<ProjectMstModel> projectMsts);

        /// <summary>
        /// 根据主键集合生成相应的预算集合
        /// </summary>
        /// <param name="phids">主键集合</param>
        /// <returns></returns>
        SavedResult<long> SaveMSHYBudgetMst(List<long> phids);

        /// <summary>
        /// 保存人员分摊数据
        /// </summary>
        /// <param name="projectDtlPersonnels">人员分摊数据集合</param>
        /// <param name="phid">主键id</param>
        /// <returns></returns>
        SavedResult<long> SaveMSYHPersonnels(List<ProjectDtlPersonnelModel> projectDtlPersonnels, long phid);

        /// <summary>
        /// 保存项目内容信息
        /// </summary>
        /// <param name="projectDtlTextContent">项目内容信息对象</param>
        /// <returns></returns>
        SavedResult<long> SaveMSYHTextContent(ProjectDtlTextContentModel projectDtlTextContent);

        /// <summary>
        /// 根据组织，年份与用户id获取对应的内容信息
        /// </summary>
        /// <param name="orgCode">组织编码</param>
        /// <param name="year">年份</param>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        ProjectDtlTextContentModel GetMSYHTextContent(string orgCode, string year, long userId);

        /// <summary>
        /// 根据单据主键获取维护人员信息
        /// </summary>
        /// <param name="phid">单据主键</param>
        /// <returns></returns>
        IList<ProjectDtlPersonNameModel> GetMSYHPersonNames(long phid);

        /// <summary>
        /// 保存维护人员集合
        /// </summary>
        /// <param name="projectDtlPersonNames">人员集合</param>
        /// <param name="phid">单据主键</param>
        /// <returns></returns>
        SavedResult<long> SaveMSYHPersonNames(List<ProjectDtlPersonNameModel> projectDtlPersonNames, long phid);

        /// <summary>
        /// 根据单据主键集合获取相应的项目材料集合
        /// </summary>
        /// <param name="phids">单据主键集合</param>
        /// <returns></returns>
        IList<ProjectDtlTextContentModel> GetProjectDtlTextContents(List<long> phids);

        /// <summary>
        /// 根据选中的项目分发数据导出模板
        /// </summary>
        /// <returns></returns>
        string ExportXMData(List<QtXmDistributeModel> data);

        /// <summary>
        /// 通过项目分项
        /// </summary>
        /// <param name="subitemcode"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        bool GetProjectMstDtlByOrgAndSubitem(string subitemcode, string orgCode);
        #endregion

        /// <summary>
        /// 通过外键集合值获取ProjectDtlBudgetDtl明细数据
        /// </summary>
        /// <param name="ids">外键值</param>
        /// <returns></returns>
        FindedResults<ProjectDtlBudgetDtlModel> FindProjectDtlBudgetDtlsByForeignKeys(List<long> ids);
    }
}
