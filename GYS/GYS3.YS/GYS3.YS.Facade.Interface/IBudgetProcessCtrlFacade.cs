#region Summary
/**************************************************************************************
    * 类 名 称：        IBudgetProcessCtrlFacade
    * 命名空间：        GYS3.YS.Facade.Interface
    * 文 件 名：        IBudgetProcessCtrlFacade.cs
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

using GYS3.YS.Model.Domain;
using GQT3.QT.Model.Domain;

namespace GYS3.YS.Facade.Interface
{
	/// <summary>
	/// BudgetProcessCtrl业务组装层接口
	/// </summary>
    public partial interface IBudgetProcessCtrlFacade : IEntFacadeBase<BudgetProcessCtrlModel>
    {
        #region IBudgetProcessCtrlFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<BudgetProcessCtrlModel> ExampleMethod<BudgetProcessCtrlModel>(string param)


        /// <summary>
        /// 保存进度控制数据
        /// </summary>
        /// <returns></returns>
        SavedResult<Int64> UpdateBudgetProcess(List<OrganizeModel> allOrganizeList, List<BudgetProcessCtrlModel> existOrganizeList,OrganizeModel organizeModel,string ocode);
		#endregion
    }
}
