#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Facade.Interface
    * 类 名 称：			IGAppvalProcFacade
    * 文 件 名：			IGAppvalProcFacade.cs
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

namespace GSP3.SP.Facade.Interface
{
	/// <summary>
	/// GAppvalProc业务组装层接口
	/// </summary>
    public partial interface IGAppvalProcFacade : IEntFacadeBase<GAppvalProcModel>
    {
        #region IGAppvalProcFacade 业务添加的成员

        /// <summary>
        /// 根据组织id，单据类型，审批类型获取所有的审批流程
        /// </summary>
        /// <param name="orgids">组织id</param>
        /// <param name="bType">单据类型</param>
        /// <param name="splx_phid">审批类型id</param>
        /// <returns></returns>
        List<GAppvalProcModel> GetAppvalProc(List<long> orgids, string bType, long splx_phid);

        /// <summary>
        /// 根据组织id，单据类型，审批类型,单据主键获取所有的符合条件的审批流程
        /// </summary>
        /// <param name="orgids">组织id</param>
        /// <param name="bType">单据类型</param>
        /// <param name="splx_phid">审批类型id</param>
        /// <param name="bPhIds">主键结合</param>
        /// <returns></returns>
        List<GAppvalProcModel> GetAppvalProcList(List<long> orgids, string bType, long splx_phid, List<long> bPhIds);

        /// <summary>
        /// 根据审批类型，单据类型，审批流程编码获取审批流程
        /// </summary>
        /// <param name="approvalTypeId">审批类型id</param>
        /// <param name="bType">单据类型</param>
        /// <param name="procCode">审批流程编码</param>
        /// <returns></returns>
        GAppvalProcModel GetAppvalProc(long approvalTypeId, string bType, string procCode);

        /// <summary>
        /// 判断审批流程是否被引用
        /// </summary>
        /// <param name="splx_phid">审批类型id</param>
        /// <returns></returns>
        bool ProcIsUsed(long splx_phid);

        /// <summary>
        /// 批量删除审批流程
        /// </summary>
        /// <param name="procModels"></param>
        /// <returns></returns>
        int DeleteAppvalProc(IList<GAppvalProcModel> procModels);

        /// <summary>
        /// 保存审批流程
        /// </summary>
        /// <param name="procModel"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveAppvalProc(GAppvalProcModel procModel);
        #endregion
    }
}
