#region Summary
/**************************************************************************************
    * 类 名 称：        IBudgetMstFacade
    * 命名空间：        GYS3.YS.Facade.Interface
    * 文 件 名：        IBudgetMstFacade.cs
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
using Enterprise3.WebApi.GYS3.YS.Model.Response;
using GSP3.SP.Model.Domain;
using GXM3.XM.Model.Domain;
using GYS3.YS.Model.Domain;

namespace GYS3.YS.Facade.Interface
{
	/// <summary>
	/// BudgetMst业务组装层接口
	/// </summary>
    public partial interface IBudgetMstFacade : IEntFacadeBase<BudgetMstModel>
    {
        #region IBudgetMstFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<BudgetMstModel> ExampleMethod<BudgetMstModel>(string param)

        /// <summary>
        /// 获取不分页集合
        /// </summary>
        /// <param name="nameSqlName">命名SQL名称</param>
        /// <param name="dic">查询条件字典（建议使用Enterprise3.Common.Base.CreateCriteria进行创建）</param>
        /// <param name="sorts">排序({属性名 desc,属性名,属性名 asc})</param>
        /// <returns>集合</returns>
        PagedResult<BudgetMstModel> LoadWithPageInfinity(string nameSqlName = "", Dictionary<string, object> dic = null, params string[] sorts);

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
        SavedResult<Int64> SaveBudgetMst(BudgetMstModel budgetMstEntity, List<BudgetDtlImplPlanModel> budgetDtlImplPlanEntities, List<BudgetDtlTextContentModel> budgetDtlTextContentEntities, List<BudgetDtlFundApplModel> budgetDtlFundApplEntities, List<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtlEntities, List<BudgetDtlPerformTargetModel> budgetDtlPerformTargetEntities, List<BudgetDtlPurchaseDtlModel> budgetDtlPurchaseDtlEntities, List<BudgetDtlPurDtl4SOFModel> budgetDtlPurDtl4SOFEntities, List<BudgetDtlPersonnelModel> budgetDtlPersonnels, List<BudgetDtlPersonNameModel> budgetDtlPersonNames);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="budgetMstEntity"></param>
        /// <param name="budgetDtlImplPlanEntities"></param>
        /// <param name="budgetDtlTextContentEntities"></param>
        /// <param name="budgetDtlFundApplEntities"></param>
        /// <param name="budgetDtlPerformTargetEntities"></param>
        /// <param name="budgetDtlBudgetDtlEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveBudgetMst(BudgetMstModel budgetMstEntity, List<BudgetDtlImplPlanModel> budgetDtlImplPlanEntities, List<BudgetDtlTextContentModel> budgetDtlTextContentEntities, List<BudgetDtlFundApplModel> budgetDtlFundApplEntities, List<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtlEntities, List<BudgetDtlPerformTargetModel> budgetDtlPerformTargetEntities);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="nameSqlName"></param>
        /// <param name="dic"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        PagedResult<BudgetMstModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, params string[] sorts);
        /// <summary>
        /// 金格控件标签取数
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
        /// <returns></returns>
        string AddDataInSaveBudgetMst(BudgetMstModel budgetMstEntity, List<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtlEntities);

        /// <summary>
        /// 允许预备费抵扣
        /// </summary>
        /// <returns></returns>
        string AddYBF(long id);

        /// <summary>
        /// 获取实际发生数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="BudgetMsts"></param>
        /// <returns></returns>
        PagedResult<BudgetMstModel> GetSJFSS(string userID, PagedResult<BudgetMstModel> BudgetMsts);

        /// <summary>
        /// 获取项目支出预算情况
        /// </summary>
        /// <param name="userID">用户id</param>
        /// <param name="BudgetMsts">预算对象集合</param>
        /// <returns></returns>
        PagedResult<BudgetMstModel> GetSJFSS2(string userID, PagedResult<BudgetMstModel> BudgetMsts);

        /// <summary>
        /// 获取老g6h预算数据主表
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        IList<BudgetMstModel> GetOldMstList(string userConn);

        /// <summary>
        /// 获取老g6h预算数据明细表(FQtZcgnfl存的是主单据代码FProjCode)
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        IList<BudgetDtlBudgetDtlModel> GetOldDtlList(string userConn);

        /// <summary>
        /// 获取老g6h预算数据text表(FLTPerformGoal存的是主单据代码FProjCode)
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        IList<BudgetDtlTextContentModel> GetOldTextList(string userConn);

        /// <summary>
        /// 获取老g6h预算数据主表XM
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        IList<ProjectMstModel> GetOldMstListXM(string userConn);

        /// <summary>
        /// 获取老g6h预算数据明细表(FQtZcgnfl存的是主单据代码FProjCode)XM
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        IList<ProjectDtlBudgetDtlModel> GetOldDtlListXM(string userConn);

        /// <summary>
        /// 获取老g6h预算数据text表(FLTPerformGoal存的是主单据代码FProjCode)XM
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        IList<ProjectDtlTextContentModel> GetOldTextListXM(string userConn);

        /// <summary>
        /// 项目预算调整分析表
        /// </summary>
        /// <param name="dicWhere"></param>
        /// <returns></returns>
        List<BudgetTZModel> GetBudgetTZList(string userID, Dictionary<string, object> dicWhere);
        #endregion


        #region//预算工作流相关
        /// <summary>
        /// 修改项目审批状态
        /// </summary>
        /// <param name="recordModel">审批对象</param>
        /// <param name="fApproval">审批字段</param>
        /// <returns></returns>
        SavedResult<long> UpdateBudget(GAppvalRecordModel recordModel, string fApproval);
        #endregion

        /// <summary>
        /// 根据年份与组织编码获取相应的项目预算调整分析表
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="orgCode">组织编码</param>
        /// <returns></returns>
        List<BudgetAdjustAnalyseModel> GetBudgetAdjustAnalyseList(string year, string orgCode);
    }
}
