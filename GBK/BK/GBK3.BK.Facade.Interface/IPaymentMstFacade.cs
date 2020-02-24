#region Summary
/**************************************************************************************
    * 命名空间：			GBK3.BK.Facade.Interface
    * 类 名 称：			IPaymentMstFacade
    * 文 件 名：			IPaymentMstFacade.cs
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
using GBK3.BK.Model.Extend;
using GGK3.GK.Model.Domain;
using GSP3.SP.Model.Domain;

namespace GBK3.BK.Facade.Interface
{
	/// <summary>
	/// PaymentMst业务组装层接口
	/// </summary>
    public partial interface IPaymentMstFacade : IEntFacadeBase<PaymentMstModel>
    {
		#region IPaymentMstFacade 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="paymentMstEntity"></param>
		/// <param name="paymentDtlEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SavePaymentMst(PaymentMstModel paymentMstEntity, List<PaymentDtlModel> paymentDtlEntities);


        /// <summary>
        /// 获取资金拨付主列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="payment">对象</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        IList<PaymentMstModel> GetPaymentMstList(int pageIndex, PaymentMstModel payment, int pageSize = 20);

        /// <summary>
        /// 点击申请单显示详情
        /// </summary>
        /// <param name="fCode"></param>
        /// <returns></returns>
        PaymentMstAndXmModel GetPaymentMst(string fCode);

        /// <summary>
        /// 根据多个单据号删除多条单据以及单据明细
        /// </summary>
        /// <param name="fCodes">多个单据号</param>
        /// <returns></returns>
        int DeleteSignle(List<long> fCodes);

        /// <summary>
        /// 新增申请单
        /// </summary>
        /// <param name="paymentMstAndXm">申请单对象</param>
        /// <returns></returns>
        SavedResult<long> AddSignle(PaymentMstAndXmModel paymentMstAndXm);

        /// <summary>
        /// 修改申请单
        /// </summary>
        /// <param name="paymentMstAndXm">新的申请单</param>
        /// <returns></returns>
        SavedResult<long> UpdateSignle(PaymentMstAndXmModel paymentMstAndXm);

        /// <summary>
        /// 更新资金拨付单的审批状态
        /// </summary>
        /// <param name="phid">单据id</param>
        /// <param name="fApproval">审批状态</param>
        /// <returns></returns>
        SavedResult<long> UpdatePayment(long phid,byte fApproval);

        /// <summary>
        /// 根据主表主键查找明细表数据
        /// </summary>
        /// <param name="mstPhid">主表主键</param>
        /// <returns></returns>
        IList<PaymentDtlModel> GetPaymentDtlsByMstPhid(long mstPhid);

        /// <summary>
        /// 根据单据主键与支付状态修改单据
        /// </summary>
        /// <param name="phid">单据主键</param>
        /// <param name="isPay">支付状态</param>
        void UpdatePaymentPay(long phid, int isPay);

        /// <summary>
        /// 修改后的获取审批单据列表的接口
        /// </summary>
        /// <param name="payment">参数结合</param>
        /// <returns></returns>
        IList<PaymentMstModel> GetPaymentList(PaymentMstModel payment);

        /// <summary>
        /// 批量作废单据
        /// </summary>
        /// <param name="phids">单据集合</param>
        /// <returns></returns>
        SavedResult<long> PostCancetPaymentList(List<long> phids);

        /// <summary>
        /// 修改业务单据的支付状态
        /// </summary>
        /// <param name="GkPayment">支付主表对象</param>
        /// <param name="gKPaymentDtls">支付子表对象</param>
        /// <returns></returns>
        SavedResult<long> UpdatePayState(GKPaymentMstModel GkPayment, IList<GKPaymentDtlModel> gKPaymentDtls);
        #endregion
    }
}
