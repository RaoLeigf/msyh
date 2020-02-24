#region Summary
/**************************************************************************************
    * 命名空间：			GBK3.BK.Dac.Interface
    * 类 名 称：			IPaymentDtlDac
    * 文 件 名：			IPaymentDtlDac.cs
    * 创建时间：			2019/5/15 
    * 作    者：			吾丰明    
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

using GBK3.BK.Model.Domain;

namespace GBK3.BK.Dac.Interface
{
	/// <summary>
	/// PaymentDtl数据访问层接口
	/// </summary>
    public partial interface IPaymentDtlDac : IEntDacBase<PaymentDtlModel>
    {
		#region IPaymentDtlDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<PaymentDtlModel> ExampleMethod<PaymentDtlModel>(string param)

		#endregion
    }
}

