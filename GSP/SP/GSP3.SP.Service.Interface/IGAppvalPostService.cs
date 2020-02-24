#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Service.Interface
    * 类 名 称：			IGAppvalPostService
    * 文 件 名：			IGAppvalPostService.cs
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

using GSP3.SP.Model.Domain;
using GSP3.SP.Model.Extra;

namespace GSP3.SP.Service.Interface
{
	/// <summary>
	/// GAppvalPost服务组装层接口
	/// </summary>
    public partial interface IGAppvalPostService : IEntServiceBase<GAppvalPostModel>
    {
		#region IGAppvalPostService 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="gAppvalPostEntity"></param>
		/// <param name="gAppvalPost4OperEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveGAppvalPost(GAppvalPostModel gAppvalPostEntity, List<GAppvalPost4OperModel> gAppvalPost4OperEntities);

        /// <summary>
        /// 通过外键值获取GAppvalPost4Oper明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<GAppvalPost4OperModel> FindGAppvalPost4OperByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 送审的时候，根据审批流程id，获取第一个审批岗位的审批人
        /// </summary>
        /// <param name="phid">审批流程id</param>
        /// <returns></returns>
        GAppvalPostModel GetFirstStepOperator(long phid);

        /// <summary>
        /// 根据审批流程id,当前岗位的id,查找下一岗位的审批人
        /// </summary>
        /// <param name="proc_phid">审批流程id</param>
        /// <param name="post_phid">审批岗位id</param>
        /// <param name="refbillPhid">关联单据主键</param>
        /// <param name="fBilltype">关联单据类型</param>
        /// <returns></returns>
        GAppvalPostModel GetNextStepOperator(long proc_phid, long post_phid, long refbillPhid, string fBilltype);

        /// <summary>
        /// 根据审批流程id获取审批岗位
        /// </summary>
        /// <param name="proc_phid">审批流程id</param>
        /// <returns></returns>
        List<GAppvalPostModel> GetApprovalPost(long proc_phid);

        /// <summary>
        /// 回退时,获取之前的所有岗位,包括发起人(岗位id为0)
        /// </summary>
        /// <param name="recordModel"></param>
        /// <returns></returns>
        List<GAppvalPostModel> GetBackApprovalPost(GAppvalRecordModel recordModel);

        /// <summary>
        /// 根据流程id获取整个流程的岗位人员
        /// </summary>
        /// <param name="refbillPhid">单据主键</param>
        /// <param name="procPhid">流程id</param>
        /// <returns></returns>
        List<GAppvalPostModel> GetBackApprovalPost2(long refbillPhid, long procPhid);

        /// <summary>
        /// 根据审批岗位获取审批人(包括岗位id为0的发起人)
        /// </summary>
        /// <param name="recordModel"></param>
        /// <returns></returns>
        List<GAppvalPost4OperModel> GetOperators(GAppvalRecordModel recordModel);


        /// <summary>
        /// 查看单个岗位的信息
        /// </summary>
        /// <param name="phid">岗位的phid</param>
        /// <returns></returns>
        GAppvalPostAndOpersModel GetAppvalPostOpers(long phid);

        /// <summary>
        /// 获取岗位列表
        /// </summary>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">页大小</param>
        /// <param name="orgId">组织id</param>
        /// <param name="uCode">用户编码</param>
        /// <param name="searchOrgid">搜索的组织</param>
        /// <param name="enableMark">是否启用</param>
        /// <param name="PostName">搜索字段</param>
        /// <returns></returns>
        List<GAppvalPostAndOpersModel> GetAppvalPostOpersList(int PageIndex, int PageSize, long orgId, string uCode, List<long> searchOrgid, string enableMark, string PostName);

        /// <summary>
        /// 根据组织获取岗位（2019.10.17改成岗位根据操作员对应组织权限取）
        /// </summary>
        /// <param name="userId">操作员id</param>
        /// <returns></returns>
        IList<GAppvalPostModel> GetAppvalPostList(long userId);

        /// <summary>
        /// 根据岗位主键集合删除岗位与岗位对应操作员信息
        /// </summary>
        /// <param name="phidList">岗位主键集合</param>
        /// <param name="uCode">用户账号</param>
        /// <returns></returns>
        DeletedResult DeletedPostOpers(List<long> phidList, string uCode);


        /// <summary>
        /// 新增岗位与操作员
        /// </summary>
        /// <param name="gAppvalPostAndOpers">岗位与操作员对象</param>
        /// <returns></returns>
        SavedResult<long> SavedSignle(GAppvalPostAndOpersModel gAppvalPostAndOpers);

        /// <summary>
        /// 修改岗位与操作员信息
        /// </summary>
        /// <param name="gAppvalPostAndOpers">岗位以及操作员对象</param>
        /// <returns></returns>
        SavedResult<long> UpdateSignle(GAppvalPostAndOpersModel gAppvalPostAndOpers);
        #endregion
    }
}
