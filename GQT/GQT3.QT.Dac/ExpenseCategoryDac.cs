#region Summary
/**************************************************************************************
    * 类 名 称：        ExpenseCategoryDac
    * 命名空间：        GQT3.QT.Dac
    * 文 件 名：        ExpenseCategoryDac.cs
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
using Enterprise3.NHORM.Dac;

using GQT3.QT.Model.Domain;
using GQT3.QT.Dac.Interface;

namespace GQT3.QT.Dac
{
	/// <summary>
	/// ExpenseCategory数据访问处理类
	/// </summary>
    public partial class ExpenseCategoryDac : EntDacBase<ExpenseCategoryModel>, IExpenseCategoryDac
    {
		#region 实现 IExpenseCategoryDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<ExpenseCategoryModel> ExampleMethod<ExpenseCategory>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

