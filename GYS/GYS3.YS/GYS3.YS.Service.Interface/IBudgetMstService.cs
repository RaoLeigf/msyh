#region Summary
/**************************************************************************************
    * 类 名 称：        IBudgetMstService
    * 命名空间：        GYS3.YS.Service.Interface
    * 文 件 名：        IBudgetMstService.cs
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
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;
using Enterprise3.WebApi.GYS3.YS.Model.Request;
using Enterprise3.WebApi.GYS3.YS.Model.Response;
using GQT3.QT.Model.Domain;
using GYS3.YS.Model.Domain;
using GYS3.YS.Model.Extra;
using SUP.Common.Base;

namespace GYS3.YS.Service.Interface
{
    /// <summary>
    /// BudgetMst服务组装层接口
    /// </summary>
    public partial interface IBudgetMstService : IEntServiceBase<BudgetMstModel>
    {
        #region IBudgetMstService 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<BudgetMstModel> ExampleMethod<BudgetMstModel>(string param)


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
        SavedResult<Int64> SaveBudgetMst(BudgetMstModel budgetMstEntity, List<BudgetDtlImplPlanModel> budgetDtlImplPlanEntities, List<BudgetDtlTextContentModel> budgetDtlTextContentEntities, List<BudgetDtlFundApplModel> budgetDtlFundApplEntities, List<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtlEntities, List<BudgetDtlPerformTargetModel> budgetDtlPerformTargetEntities, List<BudgetDtlPurchaseDtlModel> budgetDtlPurchaseDtlEntities, List<BudgetDtlPurDtl4SOFModel> budgetDtlPurDtl4SOFEntities, List<BudgetDtlPersonnelModel> budgetDtlPersonnels, List<BudgetDtlPersonNameModel> budgetDtlPersonNames = null);

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
        SavedResult<Int64> SaveBudgetMst(BudgetMstModel budgetMstEntity, List<BudgetDtlImplPlanModel> budgetDtlImplPlanEntities, List<BudgetDtlTextContentModel> budgetDtlTextContentEntities, List<BudgetDtlFundApplModel> budgetDtlFundApplEntities, List<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtlEntities, List<BudgetDtlPerformTargetModel> budgetDtlPerformTargetEntities);


        /// <summary>
        /// 通过外键值获取BudgetDtlImplPlan明细数据
        /// </summary> 
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<BudgetDtlImplPlanModel> FindBudgetDtlImplPlanByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取BudgetDtlTextContent明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<BudgetDtlTextContentModel> FindBudgetDtlTextContentByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取BudgetDtlFundAppl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<BudgetDtlFundApplModel> FindBudgetDtlFundApplByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取BudgetDtlBudgetDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<BudgetDtlBudgetDtlModel> FindBudgetDtlBudgetDtlByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取BudgetDtlPerformTarget明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<BudgetDtlPerformTargetModel> FindBudgetDtlPerformTargetByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取BudgetDtlDtlPurchaseDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<BudgetDtlPurchaseDtlModel> FindBudgetDtlPurchaseDtlByForeignKey<TValType>(TValType id);


        /// <summary>
        /// 通过外键值获取BudgetDtlPurDtl4SOF明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<BudgetDtlPurDtl4SOFModel> FindBudgetDtlPurDtl4SOFByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取JxTracking明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<JxTrackingModel> FindJxTrackingByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取BudgetDtlPersonnel明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<BudgetDtlPersonnelModel> FindBudgetDtlPersonnelByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取BudgetDtlPersonName明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<BudgetDtlPersonNameModel> FindBudgetDtlPersonNameByForeignKey<TValType>(TValType id);


        /// <summary>
        /// 通过科目代码获取BudgetDtlBudgetDtl明细数据
        /// </summary>
        /// <param name="kmdm">科目代码</param>
        /// <returns></returns>
        FindedResults<BudgetDtlBudgetDtlModel> FindBudgeAccount(string kmdm);

        /// <summary>
        /// 通过资金来源代码获取BudgetDtlBudgetDtl明细数据
        /// </summary>
        /// <param name="ZJDM">科目代码</param>
        /// <returns></returns>
        FindedResults<BudgetDtlBudgetDtlModel> FindBudgeAccountByZJDM(string ZJDM);

        /// <summary>
        /// 通过代码获取BudgetMstModel
        /// </summary>
        /// <param name="dm">代码</param>
        /// <returns></returns>
        FindedResults<BudgetMstModel> FindBudgetMst(string dm);

        /// <summary>
        /// 通过代码获取BudgetDtlBudgetDtlModel
        /// </summary>
        /// <param name="dm">代码</param>
        /// <returns></returns>
        FindedResults<BudgetDtlBudgetDtlModel> FindPaymentMethod(string dm);
        #endregion
        ///// <summary>
        ///// 预算科目删除时查找是否引用
        ///// </summary>
        ///// <param name="dicWhere"></param>
        ///// <returns></returns>
        ////FindedResults<BudgetDtlBudgetDtlModel> FindBudgetDtlBudgetDtlByCode(Dictionary<string, object> dicWhere);
        //long FindBudgetDtlBudgetDtlByCode(Dictionary<string, object> dicWhere);
        /// <summary>
        /// 获取已经完成的预算（相同的项目编码 取最新的一条）
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="dicWhere"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        PagedResult<BudgetMstModel> BudgetLoadWithPage(int pageIndex, int pageSize = 20, Dictionary<string, object> dicWhere = null, params string[] sorts);

        /// <summary>
        /// 删除集中采购明细和资金来源
        /// </summary>
        /// <param name="id"></param>
        void DeleteProjectDtlPurchase(Int64 id);

        /// <summary>
        /// 取金格控件标签值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="arcID"></param>
        /// <returns></returns>
        Dictionary<string, object> GetKingGridTagRelateData(string type, long arcID);

        /// <summary>
        /// 同步数据到老G6H数据库
        /// </summary>
        /// <returns></returns>
        string AddData();

        /// <summary>
        /// 根据预算单据主键集合同步数据到老G6H数据库
        /// </summary>
        /// <param name="phids">预算主键集合</param>
        /// <returns></returns>
        string AddData2(List<long> phids);

        /// <summary>
        /// 项目生成预算时同步数据到老G6H数据库
        /// </summary>
        /// <param name="budgetMstEntity"></param>
        /// <param name="budgetDtlBudgetDtlEntities"></param>
        /// <returns></returns>
        string AddDataInSaveBudgetMst(BudgetMstModel budgetMstEntity, List<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtlEntities);

        /// <summary>
        /// 查明细通过明细主键
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FindedResult<BudgetDtlBudgetDtlModel> FindDtlByPhid(long id);

        /// <summary>
        /// 允许预备费抵扣
        /// </summary>
        /// <returns></returns>
        string AddYBF(long id);

        /// <summary>
        /// 项目支出预算情况查询(已编报：FProjAmount，剩余可编报：FBillNO，账务实际：FDeclarationDept，年初预算：FBudgetAmount)
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedResult<BudgetMstModel> GetXmZcYs(string userID, int pageIndex, int pageSize);

        /// <summary>
        /// 项目支出预算情况查询(已编报：FProjAmount，剩余可编报：FSurplusAmount，账务实际：FHappenAmount，年初预算：FBudgetAmount)
        /// </summary>
        /// <param name="userID">用户主键</param>
        /// <returns></returns>
        PagedResult<BudgetMstModel> GetXmZcYs2(string userID);

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
        CommonResult SaveModify(BudgetMstModel AfterbudgetMst, EntityInfo<BudgetDtlImplPlanModel> budgetDtlImpls, EntityInfo<BudgetDtlTextContentModel> budgetDtlTexts, EntityInfo<BudgetDtlFundApplModel> budgetDtlFunds, EntityInfo<BudgetDtlBudgetDtlModel> budgetDtlBudgets, EntityInfo<BudgetDtlPerformTargetModel> budgetDtlPerforms, EntityInfo<BudgetDtlPurchaseDtlModel> budgetDtlPurchases, EntityInfo<BudgetDtlPurDtl4SOFModel> budgetDtlPurDtl4s);

        /// <summary>
        /// 取符合对下补助的项目
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        object GetDxbzList(BudgetMstRequestModel param);

        /// <summary>
        /// 根据单据主键取对下补助的明细
        /// </summary>
        /// <param name="id"></param>
        /// <param name="OrgId"></param>
        /// <returns></returns>
        object GetDxbzDtl(long id, long OrgId);

        /// <summary>
        /// 取符合对下补助的项目
        /// </summary>
        /// <param name="UnitId"></param>
        /// <param name="DeptId"></param>
        /// <param name="Year"></param>
        /// <returns></returns>
        List<BudgetMstModel> GetYsList(long UnitId, long DeptId, string Year);

        /// <summary>
        /// 回撤
        /// </summary>
        /// <param name="approveCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        PagedResult<BudgetMstModel> FindUnvalidPiid(string approveCode, string userId);

        /// <summary>
        /// 数据迁移到项目库
        /// </summary>
        /// <param name="userID"></param>
        string SaveSynOld(string userID);

        /// <summary>
        /// 获取已使用数
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="ProjCode"></param>
        /// <returns></returns>
        string GetUseAmount(string usercode, string ProjCode);

        /// <summary>
        /// 获取实际发生数
        /// </summary>
        /// <param name="FAccount"></param>
        /// <param name="XMCode"></param>
        /// <returns></returns>
        string GetSJFSSbyXMCode(string FAccount, string XMCode);

        /// <summary>
        /// 获取实际发生数
        /// </summary>
        /// <param name="FAccount"></param>
        /// <param name="DtlCode"></param>
        /// <returns></returns>
        string GetSJFSSbyMXCode(string FAccount, string DtlCode);
        // #endregion

        /// <summary>
        /// 项目预算调整分析表
        /// </summary>
        /// <param name="dicWhere"></param>
        /// <returns></returns>
        List<BudgetTZModel> GetBudgetTZList(string userID, Dictionary<string, object> dicWhere);

        /// <summary>
        /// 根据组织获取该组织的上年与本年决算
        /// </summary>
        /// <param name="orgId">组织ID</param>
        /// <param name="orgCode">组织Code</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        IList<YsAccountModel> GetYsAccounts(string orgId, string orgCode, string year);


        /// <summary>
        /// 根据组织获取该组织的上年与本年决算
        /// </summary>
        /// <param name="orgId">组织ID</param>
        /// <param name="orgCode">组织Code</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        IList<YsAccountModel> GetYsAccounts2(string orgId, string orgCode, string year);

        /// <summary>
        /// 返回一个有下级科目的科目
        /// </summary>
        /// <param name="ysAccounts">总的集合</param>
        /// <param name="subjectCode">科目编码</param>
        /// <param name="ys">得到的科目</param>
        /// <returns></returns>
        YsAccountModel GetCompleteYsAccount(IList<YsAccountModel> ysAccounts, string subjectCode, YsAccountModel ys);

        /// <summary>
        /// 修改预决算数据
        /// </summary>
        /// <param name="ysAccounts">列表集合</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        SavedResult<long> SaveAccountList(List<YsAccountModel> ysAccounts, string orgId, string orgCode, string year);

        /// <summary>
        /// 根据主键集合获取打印数据
        /// </summary>
        /// <param name="phids">主键集合</param>
        /// <returns></returns>
        List<object> PostPrintData(string[] phids);

        /// <summary>
        /// 保存绩效跟踪数据
        /// </summary>
        /// <param name="adddata"></param>
        /// <param name="updatedata"></param>
        /// <param name="deletedata"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveTracking(List<JxTrackingModel> adddata, List<JxTrackingModel> updatedata, List<string> deletedata);
        // #endregion

        #region//组织用户信息，用controller服务
        /// <summary>
        /// 根据用户主键获取用户信息
        /// </summary>
        /// <param name="phid"></param>
        /// <returns></returns>
        User2Model GetUser(long phid);

        /// <summary>
        /// 根据组织主键获取组织信息
        /// </summary>
        /// <param name="phid"></param>
        /// <returns></returns>
        OrganizeModel GetOrganize(long phid);

        /// <summary>
        /// 根据组织编码获取组织信息
        /// </summary>
        /// <param name="code">组织编码</param>
        /// <returns></returns>
        OrganizeModel GetOrganizeByCode(string code);

        /// <summary>
        /// 根据组织编码获取组织信息列表
        /// </summary>
        /// <param name="code">组织编码</param>
        /// <returns></returns>
        List<OrganizeModel> GetOrganizeByList(string code);
        #endregion

        /// <summary>
        /// 获取新的项目绩效集合
        /// </summary>
        /// <param name="budgetDtlPerformTargets">项目带的绩效集合</param>
        /// <param name="targetTypeCode">父级节点</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        List<BudgetDtlPerformTargetModel> GetNewBudPerformTargets(List<BudgetDtlPerformTargetModel> budgetDtlPerformTargets, string targetTypeCode, long orgId, string orgCode);


        /// <summary>
        /// 通过字典取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        List<BudgetDtlBudgetDtlModel> FindProjectDtlBudgetDtl(Dictionary<string, object> dictionary);

        /// <summary>
        /// 保存绩效跟踪集合
        /// </summary>
        /// <param name="jxTrackings">绩效跟踪集合</param>
        /// <returns></returns>
        SavedResult<long> SaveJxTracks(List<JxTrackingModel> jxTrackings);

        /// <summary>
        /// 根据年份与组织编码获取相应的项目预算调整分析表
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="orgCode">组织编码</param>
        /// <returns></returns>
        List<BudgetAdjustAnalyseModel> GetBudgetAdjustAnalyseList(string year, string orgCode);

        #region//民生银行
        /// <summary>
        /// 根据预算主表数据补充明细数据
        /// </summary>
        /// <param name="budgetMsts">主表数据集合</param>
        /// <returns></returns>
        List<BudgetAllDataModel> GetBudegtAllDataModels(List<BudgetMstModel> budgetMsts);
        #endregion
    }
}
