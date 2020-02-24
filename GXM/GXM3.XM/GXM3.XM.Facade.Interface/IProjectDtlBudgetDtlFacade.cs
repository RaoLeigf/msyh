#region Summary
/**************************************************************************************
    * 类 名 称：        IProjectDtlBudgetDtlFacade
    * 命名空间：        GXM3.XM.Facade.Interface
    * 文 件 名：        IProjectDtlBudgetDtlFacade.cs
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
using System.Text;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GXM3.XM.Model.Domain;

namespace GXM3.XM.Facade.Interface
{
	/// <summary>
	/// ProjectDtlBudgetDtl业务组装层接口
	/// </summary>
    public partial interface IProjectDtlBudgetDtlFacade : IEntFacadeBase<ProjectDtlBudgetDtlModel>
    {
        #region IProjectDtlBudgetDtlFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<ProjectDtlBudgetDtlModel> ExampleMethod<ProjectDtlBudgetDtlModel>(string param)
        /// <summary>
        /// 预算根据明细表主键回填预算金额
        /// </summary>
        /// <param name="phid"></param>
        /// <param name="FAmount"></param>
        void UpdateDtlFBudgetAmount(long[] phid, decimal[] FAmount);
        /// <summary>
        /// 生成预算时回填明细
        /// </summary>
        /// <param name="oldxm3BudgetDtl"></param>
        void UpdateBudgetDtlList(List<ProjectDtlBudgetDtlModel> oldxm3BudgetDtl);
        #endregion
    }
}
