#region Summary
/**************************************************************************************
    * 命名空间：			GBK3.BK.Dac
    * 类 名 称：			PaymentXmDac
    * 文 件 名：			PaymentXmDac.cs
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
using System.Text;
using Enterprise3.NHORM.Dac;

using GBK3.BK.Model.Domain;
using GBK3.BK.Dac.Interface;

namespace GBK3.BK.Dac
{
	/// <summary>
	/// PaymentXm数据访问处理类
	/// </summary>
    public partial class PaymentXmDac : EntDacBase<PaymentXmModel>, IPaymentXmDac
    {
		#region 实现 IPaymentXmDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<PaymentXmModel> ExampleMethod<PaymentXm>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

