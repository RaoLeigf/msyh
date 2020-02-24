#region Summary
/**************************************************************************************
    * 命名空间：			GGK3.GK.Facade.Interface
    * 类 名 称：			IGKPaymentMstFacade
    * 文 件 名：			IGKPaymentMstFacade.cs
    * 创建时间：			2019/5/20 
    * 作    者：			李明    
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

using GGK3.GK.Model.Domain;

namespace GGK3.GK.Facade.Interface
{
	/// <summary>
	/// GKPaymentMst业务组装层接口
	/// </summary>
    public partial interface IGKPaymentMstFacade : IEntFacadeBase<GKPaymentMstModel>
    {
		#region IGKPaymentMstFacade 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="gKPaymentMstEntity"></param>
		/// <param name="gKPaymentDtlEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveGKPaymentMst(GKPaymentMstModel gKPaymentMstEntity, List<GKPaymentDtlModel> gKPaymentDtlEntities);

        /// <summary>
        /// 获取资金拨付支付单信息
        /// </summary>
        /// <param name="phid">支付单主键</param>
        /// <returns></returns>
        GKPayment4ZjbfModel GetPayment4Zjbf(Int64 phid);

        /// <summary>
        /// 获取资金拨付支付单列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="dicWhere"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        PagedResult<GKPayment4ZjbfModel> GetPaymentList4Zjbf(int pageIndex, int pageSize = 20, Dictionary<string, object> dicWhere = null, params string[] sorts);

        /// <summary>
        /// 获取资金拨付支付单列表
        /// </summary>
        /// <param name="mstList"></param>
        /// <param name="TotalItems"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedResult<GKPayment4ZjbfModel> GetPaymentList4Zjbf2(List<GKPaymentMstModel> mstList, int TotalItems, int pageIndex, int pageSize = 20);
        
        /// <summary>
        /// 更新单据支付状态
        /// </summary>
        /// <param name="phid"></param>
        /// <param name="payState"></param>
        /// <param name="submitterID"></param>
        /// <returns></returns>
        SavedResult<long> UpdatePaymentState(Int64 phid, byte payState, Int64 submitterID);


        /// <summary>
        /// 批量更新明细表支付状态
        /// </summary>
        /// <param name="phIds"></param>
        /// <param name="payState"></param>
        /// <returns></returns>
        SavedResult<long> UpdateDtlPayState(List<long> phIds, byte payState);

        /// <summary>
        /// 批量更新主表支付状态
        /// </summary>
        /// <param name="phIds"></param>
        /// <param name="payState"></param>
        /// <returns></returns>
        SavedResult<long> UpdateMstPayState(List<long> phIds, byte payState);

        /// <summary>
        /// 更新单行明细表支付状态
        /// </summary>
        /// <param name="phId"></param>
        /// <param name="payState"></param>
        /// <returns></returns>
        SavedResult<long> UpdateSingleDtlPayState(long phId, byte payState);

        /// <summary>
        /// 更新支付单的审批状态
        /// </summary>
        /// <param name="relbill_phid">关联单据id</param>
        /// <param name="fApproval">审批状态</param>
        /// <returns></returns>
        SavedResult<long> UpdatePaymentApprovalState(long relbill_phid, byte fApproval);

        /// <summary>
        /// 通过支付单phid来更新资金拨付单状态
        /// </summary>
        /// <param name="phid"></param>
        /// <param name="payState"></param>
        void UpdateZjbfPaymentPayState(long phid, int payState);

        /// <summary>
        /// 根据单据号集合作废单据
        /// </summary>
        /// <param name="phids">单据集合</param>
        /// <returns></returns>
        SavedResult<long> PostCancetGkPaymentList(List<long> phids);
        #endregion
    }
}
