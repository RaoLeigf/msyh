#region Summary
/**************************************************************************************
    * 类 名 称：        IBudgetAccountsService
    * 命名空间：        GQT3.QT.Service.Interface
    * 文 件 名：        IBudgetAccountsService.cs
    * 创建时间：        2018/8/29 
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
using System.Web;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GQT3.QT.Model.Domain;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace GQT3.QT.Service.Interface
{
	/// <summary>
	/// BudgetAccounts服务组装层接口
	/// </summary>
    public partial interface IBudgetAccountsService : IEntServiceBase<BudgetAccountsModel>
    {
        #region IBudgetAccountsService 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<BudgetAccountsModel> ExampleMethod<BudgetAccountsModel>(string param)

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <returns></returns>
        IList<BudgetAccountsModel> ExportData();

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <returns></returns>
        SavedResult<Int64> ImportData(string filePath,string clear);

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <returns></returns>
        FindedResults<BudgetAccountsModel> ExecuteDataCheck(ref List<BudgetAccountsModel> budgetAccounts,string otype);

        /// <summary>
        /// 根据code判断是否是末级
        /// </summary>
        /// <returns>返回Json串</returns>
        FindedResults<BudgetAccountsModel> IfLastStage(string code);
        #endregion
    }
}
