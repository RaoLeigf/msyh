#region Summary
/**************************************************************************************
    * 类 名 称：        IPerformanceDtlTarImplFacade
    * 命名空间：        GJX3.JX.Facade.Interface
    * 文 件 名：        IPerformanceDtlTarImplFacade.cs
    * 创建时间：        2018/10/16 
    * 作    者：        夏华军    
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
	/// PerformanceDtlTarImpl业务组装层接口
	/// </summary>
    public partial interface IPerformanceDtlTarImplFacade : IEntFacadeBase<PerformanceDtlTarImplModel>
    {
		#region IPerformanceDtlTarImplFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<PerformanceDtlTarImplModel> ExampleMethod<PerformanceDtlTarImplModel>(string param)

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="performanceDtlTarImplEntity"></param>
		/// <param name="performanceDtlEvalEntities"></param>
		/// <param name="performanceDtlTextContEntities"></param>
		/// <param name="performanceDtlTarImplEntities"></param>
		/// <param name="performanceDtlBuDtlEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SavePerformanceDtlTarImpl(PerformanceDtlTarImplModel performanceDtlTarImplEntity, List<PerformanceDtlEvalModel> performanceDtlEvalEntities, List<PerformanceDtlTextContModel> performanceDtlTextContEntities, List<PerformanceDtlTarImplModel> performanceDtlTarImplEntities, List<PerformanceDtlBuDtlModel> performanceDtlBuDtlEntities);

		#endregion
    }
}
