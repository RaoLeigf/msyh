#region Summary
/**************************************************************************************
    * 类 名 称：        IBudgetAccountsDac
    * 命名空间：        GQT3.QT.Dac.Interface
    * 文 件 名：        IBudgetAccountsDac.cs
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
using Enterprise3.NHORM.Interface.EntBase;

using GQT3.QT.Model.Domain;

namespace GQT3.QT.Dac.Interface
{
	/// <summary>
	/// BudgetAccounts数据访问层接口
	/// </summary>
    public partial interface IBudgetAccountsDac : IEntDacBase<BudgetAccountsModel>
    {
        #region IBudgetAccountsDac 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<BudgetAccountsModel> ExampleMethod<BudgetAccountsModel>(string param)

        
        #endregion
    }
}

