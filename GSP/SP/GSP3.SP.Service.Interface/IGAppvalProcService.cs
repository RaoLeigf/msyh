#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Service.Interface
    * 类 名 称：			IGAppvalProcService
    * 文 件 名：			IGAppvalProcService.cs
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

namespace GSP3.SP.Service.Interface
{
	/// <summary>
	/// GAppvalProc服务组装层接口
	/// </summary>
    public partial interface IGAppvalProcService : IEntServiceBase<GAppvalProcModel>
    {
        #region IGAppvalProcService 业务添加的成员

        /// <summary>
        /// 查找审批流程
        /// </summary>
        /// <param name="phid">审批流程phid</param>
        /// <returns></returns>
        GAppvalProcModel FindSingle(long phid);

        /// <summary>
        /// 根据组织id，单据类型，审批类型获取所有的审批流程
        /// </summary>
        /// <param name="orgid">组织id</param>
        /// <param name="bType">单据类型</param>
        /// <param name="splx_phid">审批类型id</param>
        /// <returns></returns>
        List<GAppvalProcModel> GetAppvalProc(long orgid,string bType,long splx_phid);

        /// <summary>
        /// 根据组织id，单据类型，审批类型,单据主键获取所有的符合条件的审批流程
        /// </summary>
        /// <param name="orgid">组织id</param>
        /// <param name="bType">单据类型</param>
        /// <param name="splx_phid">审批类型id</param>
        /// <param name="bPhIds">主键结合</param>
        /// <returns></returns>
        List<GAppvalProcModel> GetAppvalProcList(long orgid, string bType, long splx_phid, List<long> bPhIds);

        /// <summary>
        /// 判断审批流程是否被引用
        /// </summary>
        /// <param name="proc_phid">审批类型id</param>
        /// <returns></returns>
        bool ProcIsUsed(long proc_phid);

        /// <summary>
        /// 删除审批类型
        /// </summary>
        /// <param name="proc_phid">审批类型id</param>
        /// <returns></returns>
        DeletedResult PostDeleteProcType(long proc_phid);

        /// <summary>
        /// 批量删除审批类型
        /// </summary>
        /// <param name="ids">审批类型id集合</param>
        /// <returns></returns>
        DeletedResult PostDeleteProcTypes(List<long> ids);

        /// <summary>
        /// 新增审批流程
        /// </summary>
        /// <param name="procModel"></param>
        /// <returns></returns>
        SavedResult<Int64> PostAddProc(GAppvalProcModel procModel);

        /// <summary>
        /// 新增审批流程
        /// </summary>
        /// <param name="procModels"></param>
        /// <returns></returns>
        SavedResult<Int64> PostAddProcs(List<GAppvalProcModel> procModels);

        /// <summary>
        /// 分页获取审批流程数据
        /// </summary>
        /// <param name="orgid">组织id</param>
        /// <param name="approvalTypeId">审批类型id</param>
        /// <param name="bType">单据类型</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="queryStr">流程编码或流程名称的查询条件</param>
        /// <param name="count">总记录条数</param>
        /// <returns></returns>
        List<GAppvalProcModel> GetProcList(long orgid,long approvalTypeId,string bType,int pageIndex,int pageSize,string queryStr,out int count);

        /// <summary>
        /// 获取审批流程明细
        /// </summary>
        /// <param name="approvalTypeId">审批类型id</param>
        /// <param name="bType">单据类型</param>
        /// <param name="proc_code">审批流程编码</param>
        /// <param name="orgids">组织id集合</param>
        /// <returns></returns>
        GAppvalProcModel GetProcDetail(long approvalTypeId, string bType,string proc_code,List<long> orgids);

        /// <summary>
        /// 修改审批流程
        /// </summary>
        /// <param name="approvalTypeId">审批类型id</param>
        /// <param name="bType">单据类型</param>
        /// <param name="proc_code">审批流程编码</param>
        /// <param name="orgids">组织id集合</param>
        /// <param name="procModels">新增集合</param>
        /// <param name="uCode">用户账号</param>
        /// <returns></returns>
        SavedResult<Int64> PostUpdateProc(long approvalTypeId, string bType, string proc_code, List<long> orgids, List<GAppvalProcModel> procModels, string uCode);

        /// <summary>
        /// 删除审批流程
        /// </summary>
        /// <param name="approvalTypeId">审批类型id</param>
        /// <param name="bType">单据类型</param>
        /// <param name="proc_code">审批流程编码</param>
        /// <param name="orgids">组织id集合</param>
        /// <param name="uCode">用户账号</param>
        void PostDeleteProc(long approvalTypeId, string bType, string proc_code, List<long> orgids, string uCode);

        /// <summary>
        /// 批量删除审批流程
        /// </summary>
        /// <param name="procModels"></param>
        void PostDeleteProc(List<GAppvalProcModel> procModels);

        /// <summary>
        /// 更新启用组织
        /// </summary>
        /// <param name="procModel"></param>
        /// <returns></returns>
        int PostUpdateProcOrganize(GAppvalProcModel procModel);
        #endregion
    }
}
