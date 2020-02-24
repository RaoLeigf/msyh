#region Summary
/**************************************************************************************
    * 命名空间：			GBK3.BK.Service
    * 类 名 称：			PaymentMstService
    * 文 件 名：			PaymentMstService.cs
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
using SUP.Common.DataEntity;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Service;

using GBK3.BK.Service.Interface;
using GBK3.BK.Facade.Interface;
using GBK3.BK.Model.Domain;
using GBK3.BK.Model.Extend;
using Enterprise3.Common.Base.Criterion;
using SUP.Common.Base;
using GSP3.SP.Model.Domain;

namespace GBK3.BK.Service
{
	/// <summary>
	/// PaymentMst服务组装处理类
	/// </summary>
    public partial class PaymentMstService : EntServiceBase<PaymentMstModel>, IPaymentMstService
    {
		#region 类变量及属性
		/// <summary>
        /// PaymentMst业务外观处理对象
        /// </summary>
		IPaymentMstFacade PaymentMstFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IPaymentMstFacade;
            }
        }

		/// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IPaymentDtlFacade PaymentDtlFacade { get; set; }

        #endregion

        #region 实现 IPaymentMstService 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="paymentMstEntity"></param>
        /// <param name="paymentDtlEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SavePaymentMst(PaymentMstModel paymentMstEntity, List<PaymentDtlModel> paymentDtlEntities)
        {
			return PaymentMstFacade.SavePaymentMst(paymentMstEntity, paymentDtlEntities);
        }

        /// <summary>
        /// 通过外键值获取PaymentDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<PaymentDtlModel> FindPaymentDtlByForeignKey<TValType>(TValType id)
        {
            return PaymentDtlFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 获取资金拨付主列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="payment">对象</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public IList<PaymentMstModel> GetPaymentMstList(int pageIndex, PaymentMstModel payment, int pageSize)
        {
            var result = this.PaymentMstFacade.GetPaymentMstList(pageIndex, payment, pageSize);
            return result;
        }

        /// <summary>
        /// 点击申请单显示详情
        /// </summary>
        /// <param name="fCode"></param>
        /// <returns></returns>
        public PaymentMstAndXmModel GetPaymentMst(string fCode)
        {
            //throw new Exception("");
            return this.PaymentMstFacade.GetPaymentMst(fCode);
        }

        /// <summary>
        /// 根据多个单据号删除多条单据以及单据明细
        /// </summary>
        /// <param name="fCodes">多个单据号</param>
        /// <returns></returns>
        public int DeleteSignle(List<long> fCodes)
        {
            return this.PaymentMstFacade.DeleteSignle(fCodes);
        }


        /// <summary>
        /// 新增申请单
        /// </summary>
        /// <param name="paymentMstAndXm">申请单对象</param>
        /// <returns></returns>
        public SavedResult<long> AddSignle(PaymentMstAndXmModel paymentMstAndXm)
        {
            return this.PaymentMstFacade.AddSignle(paymentMstAndXm);
        }

        /// <summary>
        /// 修改申请单
        /// </summary>
        /// <param name="paymentMstAndXm">新的申请单</param>
        /// <returns></returns>
        public SavedResult<long> UpdateSignle(PaymentMstAndXmModel paymentMstAndXm)
        {
            return this.PaymentMstFacade.UpdateSignle(paymentMstAndXm);
        }


        /// <summary>
        /// 根据主表主键查找明细表数据
        /// </summary>
        /// <param name="mstPhid">主表主键</param>
        /// <returns></returns>
        public IList<PaymentDtlModel> GetPaymentDtlsByMstPhid(long mstPhid)
        {
            return this.PaymentMstFacade.GetPaymentDtlsByMstPhid(mstPhid);
        }


        /// <summary>
        /// 通过项目主键获取已使用金钱与已冻结金钱的汇总
        /// </summary>
        /// <param name="xmMstPhid">项目主键</param>
        /// <param name="phid">资金拨付项目主键</param>
        /// <returns></returns>
        public Dictionary<string, object> GetSummary(string xmMstPhid, long phid =0)
        {
            return this.PaymentDtlFacade.GetSummary(xmMstPhid, phid);
        }

        /// <summary>
        /// 通过项目主键list获取所有已使用金钱与已冻结金钱的汇总
        /// </summary>
        /// <param name="xmPhidList"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetSummary2(List<long> xmPhidList)
        {
            return this.PaymentDtlFacade.GetSummary2(xmPhidList);
        }

        /// <summary>
        /// 根据单据主键与支付状态修改单据
        /// </summary>
        /// <param name="phid">单据主键</param>
        /// <param name="isPay">支付状态</param>
        public void UpdatePaymentPay(long phid, int isPay)
        {
            this.PaymentMstFacade.UpdatePaymentPay(phid, isPay);
        }

        /// <summary>
        /// 修改后的获取审批单据列表的接口
        /// </summary>
        /// <param name="payment">参数结合</param>
        /// <returns></returns>
        public IList<PaymentMstModel> GetPaymentList(PaymentMstModel payment)
        {

            return this.PaymentMstFacade.GetPaymentList(payment);
        }

        /// <summary>
        /// 批量作废单据
        /// </summary>
        /// <param name="phids">单据集合</param>
        /// <returns></returns>
        public SavedResult<long> PostCancetPaymentList(List<long> phids)
        {
            return this.PaymentMstFacade.PostCancetPaymentList(phids);
        }
        #endregion
    }
}

