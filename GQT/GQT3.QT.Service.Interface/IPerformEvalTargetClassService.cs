#region Summary
/**************************************************************************************
    * 类 名 称：        IPerformEvalTargetClassService
    * 命名空间：        GQT3.QT.Service.Interface
    * 文 件 名：        IPerformEvalTargetClassService.cs
    * 创建时间：        2018/10/16 
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
	/// PerformEvalTargetClass服务组装层接口
	/// </summary>
    public partial interface IPerformEvalTargetClassService : IEntServiceBase<PerformEvalTargetClassModel>
    {
        #region IPerformEvalTargetClassService 业务添加的成员

        /// <summary>
        /// 获取评价指标类别
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        IList<PerformEvalTargetClassModel> GetPerformEvalTargetClasses(string orgId, string orgCode);

        /// <summary>
        /// 保存绩效评价指标类别集合
        /// </summary>
        /// <param name="performEvalTargetClasses">集合对象</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        SavedResult<long> UpdatePerformEvalTargetClasses(IList<PerformEvalTargetClassModel> performEvalTargetClasses, string orgId, string orgCode);
        #endregion
    }
}
