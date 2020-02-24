#region Summary
/**************************************************************************************
    * 命名空间：			GBK3.BK.Dac
    * 类 名 称：			PaymentDtlDac
    * 文 件 名：			PaymentDtlDac.cs
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
using System.Text;
using Enterprise3.NHORM.Dac;

using GBK3.BK.Model.Domain;
using GBK3.BK.Dac.Interface;

namespace GBK3.BK.Dac
{
	/// <summary>
	/// PaymentDtl数据访问处理类
	/// </summary>
    public partial class PaymentDtlDac : EntDacBase<PaymentDtlModel>, IPaymentDtlDac
    {
		#region 实现 IPaymentDtlDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<PaymentDtlModel> ExampleMethod<PaymentDtl>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

