#region Summary
/**************************************************************************************
    * 类 名 称：        IProcurementTypeService
    * 命名空间：        GQT3.QT.Service.Interface
    * 文 件 名：        IProcurementTypeService.cs
    * 创建时间：        2018/10/15 
    * 作    者：        刘杭    
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

namespace GQT3.QT.Service.Interface
{
	/// <summary>
	/// ProcurementType服务组装层接口
	/// </summary>
    public partial interface IProcurementTypeService : IEntServiceBase<ProcurementTypeModel>
    {
        #region IProcurementTypeService 业务添加的成员

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <returns></returns>
        FindedResults<ProcurementTypeModel> ExecuteDataCheck(ref List<ProcurementTypeModel> procurementTypes, string otype);


        /// <summary>
        /// 根据组织获取所有采购类型集合
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        IList<ProcurementTypeModel> GetProcurementTypes(string orgId, string orgCode);

        /// <summary>
        /// 保存采购类型集合
        /// </summary>
        /// <param name="procurementTypes">集合对象</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        SavedResult<long> UpdateProcurementTypes(IList<ProcurementTypeModel> procurementTypes, string orgId, string orgCode);
        #endregion
    }
}
