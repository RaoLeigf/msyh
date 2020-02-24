#region Summary
/**************************************************************************************
    * 类 名 称：        IExpenseCategoryDac
    * 命名空间：        GQT3.QT.Dac.Interface
    * 文 件 名：        IExpenseCategoryDac.cs
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
using Enterprise3.NHORM.Interface.EntBase;

using GQT3.QT.Model.Domain;

namespace GQT3.QT.Dac.Interface
{
	/// <summary>
	/// ExpenseCategory数据访问层接口
	/// </summary>
    public partial interface IExpenseCategoryDac : IEntDacBase<ExpenseCategoryModel>
    {
		#region IExpenseCategoryDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<ExpenseCategoryModel> ExampleMethod<ExpenseCategoryModel>(string param)

		#endregion
    }
}

