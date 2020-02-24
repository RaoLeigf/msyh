#region Summary
/**************************************************************************************
    * 类 名 称：        IExpenseCategoryService
    * 命名空间：        GQT3.QT.Service.Interface
    * 文 件 名：        IExpenseCategoryService.cs
    * 创建时间：        2018/8/30 
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

using GQT3.QT.Model.Domain;

namespace GQT3.QT.Service.Interface
{
	/// <summary>
	/// ExpenseCategory服务组装层接口
	/// </summary>
    public partial interface IExpenseCategoryService : IEntServiceBase<ExpenseCategoryModel>
    {
        #region IExpenseCategoryService 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<ExpenseCategoryModel> ExampleMethod<ExpenseCategoryModel>(string param)

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <returns></returns>
        FindedResults<ExpenseCategoryModel> ExecuteDataCheck(ref List<ExpenseCategoryModel> expenseCategories,string otype);

        /// <summary>
        /// 根据支出类别(项目类型)的code判断是否是末级
        /// </summary>
        /// <returns>返回Json串</returns>
        FindedResults<ExpenseCategoryModel> IfLastStage(string code);
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="expenseCategory"></param>
        /// <param name="edittype"></param>
        /// <returns></returns>
        SavedResult<Int64> Save2(ExpenseCategoryModel expenseCategory, string edittype);
        #endregion
    }
}
