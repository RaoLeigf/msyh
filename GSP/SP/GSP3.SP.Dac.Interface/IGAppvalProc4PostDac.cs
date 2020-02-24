#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Dac.Interface
    * 类 名 称：			IGAppvalProc4PostDac
    * 文 件 名：			IGAppvalProc4PostDac.cs
    * 创建时间：			2019/5/21 
    * 作    者：			刘杭    
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

using GSP3.SP.Model.Domain;

namespace GSP3.SP.Dac.Interface
{
	/// <summary>
	/// GAppvalProc4Post数据访问层接口
	/// </summary>
    public partial interface IGAppvalProc4PostDac : IEntDacBase<GAppvalProc4PostModel>
    {
		#region IGAppvalProc4PostDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<GAppvalProc4PostModel> ExampleMethod<GAppvalProc4PostModel>(string param)

		#endregion
    }
}

