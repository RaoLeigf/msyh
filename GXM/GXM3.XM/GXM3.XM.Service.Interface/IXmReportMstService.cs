#region Summary
/**************************************************************************************
    * 命名空间：			GXM3.XM.Service.Interface
    * 类 名 称：			IXmReportMstService
    * 文 件 名：			IXmReportMstService.cs
    * 创建时间：			2020/1/17 
    * 作    者：			王冠冠    
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

using GXM3.XM.Model.Domain;

namespace GXM3.XM.Service.Interface
{
	/// <summary>
	/// XmReportMst服务组装层接口
	/// </summary>
    public partial interface IXmReportMstService : IEntServiceBase<XmReportMstModel>
    {
		#region IXmReportMstService 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="xmReportMstEntity"></param>
		/// <param name="xmReportDtlEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveXmReportMst(XmReportMstModel xmReportMstEntity, List<XmReportDtlModel> xmReportDtlEntities);

        /// <summary>
        /// 通过外键值获取XmReportDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<XmReportDtlModel> FindXmReportDtlByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取XmReportReturn明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<XmReportReturnModel> FindXmReportReturnByForeignKey<TValType>(TValType id);
        /// <summary>
        /// 通过单据主键获取签报单信息
        /// </summary>
        /// <param name="phid">单据主键</param>
        /// <returns></returns>
        XmReportMstModel GetMSYHProjectReport(long phid);

        /// <summary>
        /// 保存签报单额度返还明细
        /// </summary>
        /// <param name="XmReportReturns"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveReturn(List<XmReportReturnModel> XmReportReturns);

        /// <summary>
        /// 额度分配后金额的保存
        /// </summary>
        /// <param name="Msts"></param>
        /// <param name="XmReportDtls"></param>
        /// <returns></returns>
        string SaveReturnAmount(List<XmReportMstModel> Msts, List<XmReportDtlModel> XmReportDtls);

        /// <summary>
        /// 通过外键集合获取XmReportDtl明细数据
        /// </summary>
        /// <param name="ids">外键值</param>
        /// <returns></returns>
        FindedResults<XmReportDtlModel> FindXmReportDtlsByForeignKeys(List<long> ids);
        #endregion
    }
}
