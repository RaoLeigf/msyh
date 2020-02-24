#region Summary
/**************************************************************************************
    * 命名空间：			GBK3.BK.Dac.Interface
    * 类 名 称：			IPaymentMstDac
    * 文 件 名：			IPaymentMstDac.cs
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
using System.Data;
using System.Linq;
using Enterprise3.NHORM.Interface.EntBase;

using GBK3.BK.Model.Domain;

namespace GBK3.BK.Dac.Interface
{
	/// <summary>
	/// PaymentMst数据访问层接口
	/// </summary>
    public partial interface IPaymentMstDac : IEntDacBase<PaymentMstModel>
    {
        #region IPaymentMstDac 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<PaymentMstModel> ExampleMethod<PaymentMstModel>(string param)


        /// <summary>
        /// 获取资金拨付主列表
        /// </summary>
        /// <param name="fPhid"></param>
        /// <returns></returns>
        DataTable GetPaymentList(long fPhid);
        #endregion
    }
}

