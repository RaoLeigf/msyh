#region Summary
/**************************************************************************************
    * 类 名 称：        IExpenseMstFacade
    * 命名空间：        GYS3.YS.Facade.Interface
    * 文 件 名：        IExpenseMstFacade.cs
    * 创建时间：        2019/1/24 
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
using GSP3.SP.Model.Domain;
using GYS3.YS.Model.Domain;
using GYS3.YS.Model.Extra;

namespace GYS3.YS.Facade.Interface
{
	/// <summary>
	/// ExpenseMst业务组装层接口
	/// </summary>
    public partial interface IExpenseMstFacade : IEntFacadeBase<ExpenseMstModel>
    {
        #region IExpenseMstFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<ExpenseMstModel> ExampleMethod<ExpenseMstModel>(string param)

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="expenseMstEntity"></param>
        /// <param name="expenseDtlEntities"></param>
        /// <param name="NCmoney">年初预算金额</param>
        /// <param name="SumFSurplusamount">数据库支出和返回的计算值</param>
        /// <param name="beforeSum">本单据初始预计支出金额</param>
        /// <param name="beforeFReturnamount">本单据初始预计返还金额</param>
        /// <param name="Ifreturn">是否额度返还</param>
        /// <returns></returns>
        SavedResult<Int64> SaveExpenseMst(ExpenseMstModel expenseMstEntity, List<ExpenseDtlModel> expenseDtlEntities, string NCmoney, string SumFSurplusamount, string beforeSum, string beforeFReturnamount, string Ifreturn);


        PagedResult<ExpenseMstModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, params string[] sorts);

        /// <summary>
        /// 部门代码转
        /// </summary>
        /// <param name="nameSqlName"></param>
        /// <param name="dicWhere"></param>
        /// <param name="isUseInfoRight"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        PagedResult<ExpenseMstModel> LoadWithPageForDept(string nameSqlName = "", Dictionary<string, object> dicWhere = null, bool isUseInfoRight = false, params string[] sorts);

        /*/// <summary>
        /// 获取实际发生数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="expenseMsts"></param>
        /// <returns></returns>
        PagedResult<ExpenseMstModel> GetSJFSS(string userID, PagedResult<ExpenseMstModel> expenseMsts);*/

        /// <summary>
        /// 通过项目代码和操作员获取财务实际发生数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        string GetSJFSSbyCode(string userID, string code);

        /// <summary>
        /// 修改主表审批状态
        /// </summary>
        /// <param name="recordModel">传参对象</param>
        /// <param name="fApproval">审批状态</param>
        /// <returns></returns>
        SavedResult<long> UpdateExpense(GAppvalRecordModel recordModel, string fApproval);

        /// <summary>
        /// 根据用款计划的主键获取相关数据集合
        /// </summary>
        /// <param name="phid">主键</param>
        /// <returns></returns>
        ExpenseAllModel GetExpenseAllModel(long phid);
        #endregion
    }
}
