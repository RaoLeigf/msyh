#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Facade.Interface
    * 类 名 称：			IGAppvalRecordFacade
    * 文 件 名：			IGAppvalRecordFacade.cs
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
using GSP3.SP.Model.Domain;

namespace GSP3.SP.Facade.Interface
{
	/// <summary>
	/// GAppvalRecord业务组装层接口
	/// </summary>
    public partial interface IGAppvalRecordFacade : IEntFacadeBase<GAppvalRecordModel>
    {
        #region IGAppvalRecordFacade 业务添加的成员

        /// <summary>
        /// 获取当前用户的待审批记录
        /// </summary>
        /// <param name="billRequest"></param>
        /// <param name="total">总条数</param>
        /// <returns></returns>
        List<AppvalRecordVo> GetUnDoRecords(BillRequestModel billRequest, out int total);

        /// <summary>
        /// 分页获取已审批单据数据
        /// </summary>
        /// <param name="billRequest"></param>
        /// <param name="total">总条数</param>
        /// <returns></returns>
        List<AppvalRecordVo> GetDoneRecordList(BillRequestModel billRequest, out int total);

        /// <summary>
        /// 根据关联单据id,单据类型查找审批记录
        /// </summary>
        /// <param name="phid">单据id</param>
        /// <param name="billType">单据类型</param>
        /// <returns></returns>
        List<GAppvalRecordModel> FindByRelId(long phid,string billType);

        /// <summary>
        /// 更新审批记录
        /// </summary>
        /// <param name="recordModel"></param>
        /// <param name="presentPost">当前审批岗位</param>
        /// <param name="models">当前审批岗位的审批记录</param>
        /// <returns></returns>
        SavedResult<Int64> UpdateApprovalRecord(GAppvalRecordModel recordModel, GAppvalPostModel presentPost, List<GAppvalRecordModel> models);

        /// <summary>
        /// 把数据保存到审批记录表中
        /// </summary>
        /// <param name="gAppvalRecord">审批发起记录</param>
        /// <returns></returns>
        SavedResult<long> AddAppvalRecord(GAppvalRecordModel gAppvalRecord);

        /// <summary>
        /// 生成支付单
        /// </summary>
        /// <param name="bill_phid">资金拨付单id</param>
        /// <returns></returns>
        SavedResult<Int64> PostAddPayMent(long bill_phid);

        /// <summary>
        /// 批量生成支付单
        /// </summary>
        /// <param name="bill_phids">资金拨付单id集合</param>
        /// <returns></returns>
        SavedResult<Int64> PostAddPayMents(List<long> bill_phids);


        /// <summary>
        /// 审批流查看
        /// </summary>
        /// <param name="phid">单据主键</param>
        /// <param name="proc_phid">流程主键</param>
        /// <param name="billType">单据类型</param>
        /// <returns></returns>
        List<GAppvalRecordModel> GetAppvalRecordList(long phid, long proc_phid, string billType);



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

        /// <summary>
        /// 获取当前用户的待审批记录
        /// </summary>
        /// <param name="billRequest"></param>
        /// <param name="total">总条数</param>
        /// <returns></returns>
        List<AppvalRecordVo> GetUnDoRecords2(BillRequestModel billRequest, out int total);

        /// <summary>
        /// 获取当前用户的待审批记录(不分页)
        /// </summary>
        /// <param name="billRequest"></param>
        /// <returns></returns>
        List<AppvalRecordVo> GetUnDoRecords3(BillRequestModel billRequest);

        /// <summary>
        /// 分页获取已审批单据数据
        /// </summary>
        /// <param name="billRequest"></param>
        /// <param name="total">总条数</param>
        /// <returns></returns>
        List<AppvalRecordVo> GetDoneRecordList2(BillRequestModel billRequest, out int total);
        #endregion
    }
}
