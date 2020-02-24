#region Summary
/**************************************************************************************
    * 类 名 称：        IPerformanceMstDac
    * 命名空间：        GJX3.JX.Dac.Interface
    * 文 件 名：        IPerformanceMstDac.cs
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
using Enterprise3.NHORM.Interface.EntBase;

using GJX3.JX.Model.Domain;

namespace GJX3.JX.Dac.Interface
{
	/// <summary>
	/// PerformanceMst数据访问层接口
	/// </summary>
    public partial interface IPerformanceMstDac : IEntDacBase<PerformanceMstModel>
    {
		#region IPerformanceMstDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<PerformanceMstModel> ExampleMethod<PerformanceMstModel>(string param)

		#endregion
    }
}

