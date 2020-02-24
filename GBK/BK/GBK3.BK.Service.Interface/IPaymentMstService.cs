#region Summary
/**************************************************************************************
    * 命名空间：			GBK3.BK.Service.Interface
    * 类 名 称：			IPaymentMstService
    * 文 件 名：			IPaymentMstService.cs
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
using GSP3.SP.Model.Domain;

namespace GBK3.BK.Service.Interface
{
	/// <summary>
	/// PaymentMst服务组装层接口
	/// </summary>
    public partial interface IPaymentMstService : IEntServiceBase<PaymentMstModel>
    {
		#region IPaymentMstService 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="paymentMstEntity"></param>
		/// <param name="paymentDtlEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SavePaymentMst(PaymentMstModel paymentMstEntity, List<PaymentDtlModel> paymentDtlEntities);

        /// <summary>
        /// 通过外键值获取PaymentDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<PaymentDtlModel> FindPaymentDtlByForeignKey<TValType>(TValType id);


        /// <summary>
        /// 获取资金拨付主列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="payment">对象</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        IList<PaymentMstModel> GetPaymentMstList(int pageIndex, PaymentMstModel payment, int pageSize);

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
        /// 根据主表主键查找明细表数据
        /// </summary>
        /// <param name="mstPhid">主表主键</param>
        /// <returns></returns>
        IList<PaymentDtlModel> GetPaymentDtlsByMstPhid(long mstPhid);

        /// <summary>
        /// 通过项目主键获取已使用金钱与已冻结金钱的汇总
        /// </summary>
        /// <param name="xmMstPhid">项目主键</param>
        /// <param name="phid">资金拨付项目主键</param>
        /// <returns></returns>
        Dictionary<string, object> GetSummary(string xmMstPhid, long phid =0);

        /// <summary>
        /// 通过项目主键list获取所有已使用金钱与已冻结金钱的汇总
        /// </summary>
        /// <param name="xmPhidList"></param>
        /// <returns></returns>
        Dictionary<string, object> GetSummary2(List<long> xmPhidList);

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
        #endregion
    }
}
