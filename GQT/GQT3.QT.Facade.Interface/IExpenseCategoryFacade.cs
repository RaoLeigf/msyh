#region Summary
/**************************************************************************************
    * 类 名 称：        IExpenseCategoryFacade
    * 命名空间：        GQT3.QT.Facade.Interface
    * 文 件 名：        IExpenseCategoryFacade.cs
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

namespace GQT3.QT.Facade.Interface
{
	/// <summary>
	/// ExpenseCategory业务组装层接口
	/// </summary>
    public partial interface IExpenseCategoryFacade : IEntFacadeBase<ExpenseCategoryModel>
    {
        #region IExpenseCategoryFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<ExpenseCategoryModel> ExampleMethod<ExpenseCategoryModel>(string param)

        PagedResult<ExpenseCategoryModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts);

        /// <summary>
        /// 通过代码找名称
        /// </summary>
        /// <param name="Dm"></param>
        /// <returns></returns>
        string FindMcByDm(string Dm);
        #endregion
    }
}
