#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Dac.Interface
    * 类 名 称：			IGAppvalProcCondsDac
    * 文 件 名：			IGAppvalProcCondsDac.cs
    * 创建时间：			2019/5/20 
    * 作    者：			李明    
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
	/// GAppvalProcConds数据访问层接口
	/// </summary>
    public partial interface IGAppvalProcCondsDac : IEntDacBase<GAppvalProcCondsModel>
    {
		#region IGAppvalProcCondsDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<GAppvalProcCondsModel> ExampleMethod<GAppvalProcCondsModel>(string param)

		#endregion
    }
}

