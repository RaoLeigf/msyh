#region Summary
/**************************************************************************************
    * 类 名 称：        IPerformEvalTypeService
    * 命名空间：        GQT3.QT.Service.Interface
    * 文 件 名：        IPerformEvalTypeService.cs
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

namespace GQT3.QT.Service.Interface
{
	/// <summary>
	/// PerformEvalType服务组装层接口
	/// </summary>
    public partial interface IPerformEvalTypeService : IEntServiceBase<PerformEvalTypeModel>
    {
        #region IPerformEvalTypeService 业务添加的成员
        /// <summary>
        /// 获取绩效评价类型
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        IList<PerformEvalTypeModel> GetPerformEvalTypes(string orgId, string orgCode);


        /// <summary>
        /// 保存绩效评价类型集合
        /// </summary>
        /// <param name="performEvalTypes">集合对象</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        SavedResult<long> UpdatePerformEvalTypes(IList<PerformEvalTypeModel> performEvalTypes, string orgId, string orgCode);
        #endregion
    }
}
