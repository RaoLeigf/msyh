#region Summary
/**************************************************************************************
    * 类 名 称：        IPerformanceMstFacade
    * 命名空间：        GJX3.JX.Facade.Interface
    * 文 件 名：        IPerformanceMstFacade.cs
    * 创建时间：        2018/9/12 
    * 作    者：        吾丰明    
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

using GJX3.JX.Model.Domain;

namespace GJX3.JX.Facade.Interface
{
	/// <summary>
	/// PerformanceMst业务组装层接口
	/// </summary>
    public partial interface IPerformanceMstFacade : IEntFacadeBase<PerformanceMstModel>
    {
        #region IPerformanceMstFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<PerformanceMstModel> ExampleMethod<PerformanceMstModel>(string param)

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="performanceMstEntity"></param>
        /// <param name="performanceDtlTextContEntities"></param>
        /// <param name="performanceDtlBuDtlEntities"></param>
        /// <param name="performanceDtlTarImplEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SavePerformanceMst(PerformanceMstModel performanceMstEntity, List<PerformanceDtlTextContModel> performanceDtlTextContEntities, List<PerformanceDtlBuDtlModel> performanceDtlBuDtlEntities, List<PerformanceDtlTarImplModel> performanceDtlTarImplEntities);

        PagedResult<PerformanceMstModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, params string[] sorts);

        /// <summary>
        /// 获取新的项目绩效集合
        /// </summary>
        /// <param name="projectDtlPerformTargets">项目带的绩效集合</param>
        /// <param name="targetTypeCode">父级节点</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        List<PerformanceDtlTarImplModel> GetNewProPerformTargets(List<PerformanceDtlTarImplModel> projectDtlPerformTargets, string targetTypeCode, long orgId, string orgCode);

        #endregion
    }
}
