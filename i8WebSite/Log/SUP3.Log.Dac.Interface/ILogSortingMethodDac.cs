#region Summary
/**************************************************************************************
    * 类 名 称：        ILogSortingMethodDac
    * 命名空间：        SUP3.Log.Dac.Interface
    * 文 件 名：        ILogSortingMethodDac.cs
    * 创建时间：        2017/11/4 
    * 作    者：        洪鹏    
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

using SUP3.Log.Model.Domain;

namespace SUP3.Log.Dac.Interface
{
	/// <summary>
	/// LogSortingMethod数据访问层接口
	/// </summary>
    public partial interface ILogSortingMethodDac : IEntDacBase<LogSortingMethodModel>
    {
		#region ILogSortingMethodDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<LogSortingMethodModel> ExampleMethod<LogSortingMethodModel>(string param)

		#endregion
    }
}

