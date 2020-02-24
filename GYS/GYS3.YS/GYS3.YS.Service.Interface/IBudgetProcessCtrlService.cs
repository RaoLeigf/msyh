#region Summary
/**************************************************************************************
    * 类 名 称：        IBudgetProcessCtrlService
    * 命名空间：        GYS3.YS.Service.Interface
    * 文 件 名：        IBudgetProcessCtrlService.cs
    * 创建时间：        2018/9/10 
    * 作    者：        夏华军    
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
using SUP.Common.DataEntity;

using GYS3.YS.Model.Domain;

namespace GYS3.YS.Service.Interface
{
	/// <summary>
	/// BudgetProcessCtrl服务组装层接口
	/// </summary>
    public partial interface IBudgetProcessCtrlService : IEntServiceBase<BudgetProcessCtrlModel>
    {
        #region IBudgetProcessCtrlService 业务添加的成员
        
        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回</returns>
        PagedResult<BudgetProcessCtrlModel> GetBudgetProcessCtrlDistinctList(DataStoreParam storeParam, string query,string userId);

        /// <summary>
        /// 返回未添加到进度控制表中的部门数据
        /// </summary>
        /// <returns>返回</returns>
        void GetBudgetProcessCtrlPorcessList(DataStoreParam storeParam, string focode, string FYear, out List<BudgetProcessCtrlModel> list);

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回</returns>
        PagedResult<BudgetProcessCtrlModel> GetBudgetProcessCtrlPorcessList2(DataStoreParam storeParam, string focode, string FYear);

        /// <summary>
        /// 不分页取列表数据
        /// </summary>
        /// <returns>返回</returns>
        List<BudgetProcessCtrlModel> GetBudgetProcessCtrlPorcessList3(string focode);

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>List</returns>
        IList<BudgetProcessCtrlModel> FindBudgetProcessCtrlModelByList(List<BudgetProcessCtrlModel> list);
        /// <summary>
        /// 根据预算单位和预算部门查找部门所在预算进度
        /// </summary>
        /// <param name="oCode"></param>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        string FindBudgetProcessCtrl(string oCode, string deptCode, string FYear);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        SavedResult<Int64> SaveProcessSetting(List<BudgetProcessCtrlModel> models,string symbol);

        /// <summary>
        /// 根据预算部门查找部门所在预算进度
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        FindedResults<BudgetProcessCtrlModel> FindBudgetProcessCtrlByList(List<string> deptCode, string FYear);
        #endregion
    }
}
