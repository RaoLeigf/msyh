#region Summary
/**************************************************************************************
    * 命名空间：			GBK3.BK.Facade.Interface
    * 类 名 称：			IPaymentDtlFacade
    * 文 件 名：			IPaymentDtlFacade.cs
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
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GBK3.BK.Model.Domain;

namespace GBK3.BK.Facade.Interface
{
	/// <summary>
	/// PaymentDtl业务组装层接口
	/// </summary>
    public partial interface IPaymentDtlFacade : IEntFacadeBase<PaymentDtlModel>
    {
        #region IPaymentDtlFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<PaymentDtlModel> ExampleMethod<PaymentDtlModel>(string param)


        /// <summary>
        /// 通过项目主键获取已使用金钱与已冻结金钱的汇总
        /// </summary>
        /// <param name="xmMstPhid">项目主键</param>
        /// <param name="phid">资金拨付项目主键</param>
        /// <returns></returns>
        Dictionary<string, object> GetSummary(string xmMstPhid, long phid);

        /// <summary>
        /// 通过项目主键list获取已使用金钱与已冻结金钱的汇总
        /// </summary>
        /// <param name="xmPhidList">项目主键list</param>
        /// <returns></returns>
        Dictionary<string, object> GetSummary2(List<long> xmPhidList);
        #endregion
    }
}
