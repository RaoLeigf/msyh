#region Summary
/**************************************************************************************
    * 类 名 称：        IPerformanceDtlEvalFacade
    * 命名空间：        GJX3.JX.Facade.Interface
    * 文 件 名：        IPerformanceDtlEvalFacade.cs
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
	/// PerformanceDtlEval业务组装层接口
	/// </summary>
    public partial interface IPerformanceDtlEvalFacade : IEntFacadeBase<PerformanceDtlEvalModel>
    {
		#region IPerformanceDtlEvalFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<PerformanceDtlEvalModel> ExampleMethod<PerformanceDtlEvalModel>(string param)

		#endregion
    }
}
