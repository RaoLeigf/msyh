#region Summary
/**************************************************************************************
    * 类 名 称：        IExpenseDtlDac
    * 命名空间：        GYS3.YS.Dac.Interface
    * 文 件 名：        IExpenseDtlDac.cs
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
using Enterprise3.NHORM.Interface.EntBase;

using GYS3.YS.Model.Domain;

namespace GYS3.YS.Dac.Interface
{
	/// <summary>
	/// ExpenseDtl数据访问层接口
	/// </summary>
    public partial interface IExpenseDtlDac : IEntDacBase<ExpenseDtlModel>
    {
		#region IExpenseDtlDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<ExpenseDtlModel> ExampleMethod<ExpenseDtlModel>(string param)

		#endregion
    }
}

