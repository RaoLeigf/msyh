#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Service.Interface
    * 类 名 称：			IGAppvalRecordService
    * 文 件 名：			IGAppvalRecordService.cs
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
using Enterprise3.WebApi.GSP3.SP.Model.Request;
using Enterprise3.WebApi.GSP3.SP.Model.Response;
using GQT3.QT.Model.Domain;
using GSP3.SP.Model.Domain;

namespace GSP3.SP.Service.Interface
{
	/// <summary>
	/// GAppvalRecord服务组装层接口
	/// </summary>
    public partial interface IGAppvalRecordService : IEntServiceBase<GAppvalRecordModel>
    {
        #region IGAppvalRecordService 业务添加的成员

        /// <summary>
        /// 分页获取待我审批单据数据
        /// </summary>
        /// <param name="billRequest"></param>
        /// <param name="total">总条数</param>
        /// <returns></returns>
        List<AppvalRecordVo> GetUnDoRecordList(BillRequestModel billRequest, out int total);

        /// <summary>
        /// 分页获取已审批单据数据
        /// </summary>
        /// <param name="billRequest"></param>
        /// <param name="total">总条数</param>
        /// <returns></returns>
        List<AppvalRecordVo> GetDoneRecordList(BillRequestModel billRequest, out int total);

        /// <summary>
        /// 审批流查看
        /// </summary>
        /// <param name="phid">单据phid</param>
        /// <param name="proc_phid">审批流程id</param>
        /// <param name="billType">单据类型</param>
        /// <returns></returns>
        List<GAppvalRecordModel> GetAppvalRecord(long phid,long proc_phid,string billType);

        /// <summary>
        /// 审批
        /// </summary>
        /// <param name="recordModel"></param>
        void PostApprovalRecord(GAppvalRecordModel recordModel);

        /// <summary>
        /// 审批(带附件)
        /// </summary>
        /// <param name="recordModel">审批记录对象</param>
        /// <param name="qtAttachments">附件集合</param>
        void PostApprovalRecordList(GAppvalRecordModel recordModel, List<QtAttachmentModel> qtAttachments);

        /// <summary>
        /// 把数据保存到审批记录表中
        /// </summary>
        /// <param name="gAppval">审批发起记录</param>
        /// <returns></returns>
        SavedResult<long> AddAppvalRecord(GAppvalRecordModel gAppval);

        /// <summary>
        /// 生成支付单
        /// </summary>
        /// <param name="recordModel"></param>
        SavedResult<Int64> PostAddPayMent(GAppvalRecordModel recordModel);

        /// <summary>
        /// 批量生成支付单
        /// </summary>
        /// <param name="recordModel"></param>
        SavedResult<Int64> PostAddPayMents(GAppvalRecordModel recordModel);


        /// <summary>
        /// 审批流查看
        /// </summary>
        /// <param name="phid">单据主键</param>
        /// <param name="billType">单据类型</param>
        /// <returns></returns>
        List<GAppvalRecordModel> GetAppvalRecordList(long phid, string billType);


        /// <summary>
        /// 根据流程获取所有岗位以及操作员名字
        /// </summary>
        /// <param name="procPhid">流程主键</param>
        /// <returns></returns>
        List<GAppvalRecordModel> GetAllPostsAndOpersByProc(long procPhid);

        /// <summary>
        /// 根据单据号与单据类型取消单据送审
        /// </summary>
        /// <param name="gAppval"></param>
        /// <returns></returns>
        SavedResult<long> PostCancelAppvalRecord(GAppvalRecordModel gAppval);
        #endregion
    }
}
