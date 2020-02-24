#region Summary
/**************************************************************************************
    * 类 名 称：        IPerformEvalTargetService
    * 命名空间：        GQT3.QT.Service.Interface
    * 文 件 名：        IPerformEvalTargetService.cs
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
	/// PerformEvalTarget服务组装层接口
	/// </summary>
    public partial interface IPerformEvalTargetService : IEntServiceBase<PerformEvalTargetModel>
    {
        #region IPerformEvalTargetService 业务添加的成员

        /// <summary>
        /// 指标类别转名称
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        PagedResult<PerformEvalTargetModel> CodeToName(PagedResult<PerformEvalTargetModel> result);

        FindedResults<PerformEvalTargetModel> FindPerformEvalTargetByAnyCode<TValType>(TValType values, string Pname);

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <returns></returns>
        FindedResults<PerformEvalTargetModel> ExecuteDataCheck(ref List<PerformEvalTargetModel> performEvalTarget, string otype);


        /// <summary>
        /// 根据指标类型获取指标集合
        /// </summary>
        /// <param name="targetTypeCode">指标类型编码</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        IList<PerformEvalTargetModel> GetPerformEvalTargetList(string targetTypeCode, string orgId, string orgCode);

        /// <summary>
        /// 根据指标类型获取指标集合(指标类型有多层)
        /// </summary>
        /// <param name="targetTypeCode">指标类型编码</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        IList<PerformEvalTargetModel> GetPerformEvalTargetList2(string targetTypeCode, string orgId, string orgCode);
        /// <summary>
        /// 修改指标明细集合
        /// </summary>
        /// <param name="performEvalTargets">集合</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        SavedResult<long> UpdateTargets(List<PerformEvalTargetModel> performEvalTargets, string orgId, string orgCode);

        /// <summary>
        /// 保存指标类型数
        /// </summary>
        /// <param name="targetTypeModel">指标类型对象</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <param name="perCode">父级code</param>
        /// <param name="performEvalTargets">该组织明细集合</param>
        /// <returns></returns>
        SavedResult<long> UpdateTargetType(PerformEvalTargetTypeModel targetTypeModel, string orgId, string orgCode, string perCode, IList<PerformEvalTargetModel> performEvalTargets);
        #endregion
    }
}
