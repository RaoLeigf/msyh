#region Summary
/**************************************************************************************
    * 类 名 称：        IProcurementCatalogService
    * 命名空间：        GQT3.QT.Service.Interface
    * 文 件 名：        IProcurementCatalogService.cs
    * 创建时间：        2018/10/16 
    * 作    者：        李长敏琛    
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

using GQT3.QT.Model.Domain;
using GXM3.XM.Model.Domain;

namespace GQT3.QT.Service.Interface
{
	/// <summary>
	/// ProcurementCatalog服务组装层接口
	/// </summary>
    public partial interface IProcurementCatalogService : IEntServiceBase<ProcurementCatalogModel>
    {
        #region IProcurementCatalogService 业务添加的成员

        //IList<ProjectDtlPurchaseDtlModel> getQuoteByProjectDtlPurchaseDtl(string ProcurementCatalogPhId);

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <returns></returns>
        SavedResult<Int64> SaveImportData(string fileExtension, string filePath, string clear);

        /// <summary>
        /// 根据组织获取所有采购目录集合
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        IList<ProcurementCatalogModel> GetProcurementCatalogs(string orgId, string orgCode);

        /// <summary>
        /// 保存采购目录集合
        /// </summary>
        /// <param name="procurementCatalogs">集合对象</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        SavedResult<long> UpdateProcurementCatalogs(IList<ProcurementCatalogModel> procurementCatalogs, string orgId, string orgCode);
        #endregion
    }
}
