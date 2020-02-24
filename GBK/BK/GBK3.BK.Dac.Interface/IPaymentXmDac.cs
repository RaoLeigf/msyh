#region Summary
/**************************************************************************************
    * 命名空间：			GBK3.BK.Dac.Interface
    * 类 名 称：			IPaymentXmDac
    * 文 件 名：			IPaymentXmDac.cs
    * 创建时间：			2019/5/23 
    * 作    者：			刘杭    
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
	/// PaymentXm数据访问层接口
	/// </summary>
    public partial interface IPaymentXmDac : IEntDacBase<PaymentXmModel>
    {
		#region IPaymentXmDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<PaymentXmModel> ExampleMethod<PaymentXmModel>(string param)

		#endregion
    }
}

