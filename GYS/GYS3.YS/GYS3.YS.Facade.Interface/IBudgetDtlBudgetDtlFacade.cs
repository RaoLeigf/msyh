#region Summary
/**************************************************************************************
    * 类 名 称：        IBudgetDtlBudgetDtlFacade
    * 命名空间：        GYS3.YS.Facade.Interface
    * 文 件 名：        IBudgetDtlBudgetDtlFacade.cs
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

using GYS3.YS.Model.Domain;

namespace GYS3.YS.Facade.Interface
{
	/// <summary>
	/// BudgetDtlBudgetDtl业务组装层接口
	/// </summary>
    public partial interface IBudgetDtlBudgetDtlFacade : IEntFacadeBase<BudgetDtlBudgetDtlModel>
    {
        ///// <summary>
        ///// 预算科目删除时查找是否引用
        ///// </summary>
        ///// <param name="dicWhere"></param>
        ///// <returns></returns>
        //FindedResults<BudgetDtlBudgetDtlModel> FindByCode(Dictionary<string, object> dicWhere);

        #region IBudgetDtlBudgetDtlFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<BudgetDtlBudgetDtlModel> ExampleMethod<BudgetDtlBudgetDtlModel>(string param)


        #endregion
    }
}
