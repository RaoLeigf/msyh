#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Facade.Interface
    * 类 名 称：			IGAppvalProcCondsFacade
    * 文 件 名：			IGAppvalProcCondsFacade.cs
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
using System.Text;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GSP3.SP.Model.Domain;

namespace GSP3.SP.Facade.Interface
{
	/// <summary>
	/// GAppvalProcConds业务组装层接口
	/// </summary>
    public partial interface IGAppvalProcCondsFacade : IEntFacadeBase<GAppvalProcCondsModel>
    {
		#region IGAppvalProcCondsFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<GAppvalProcCondsModel> ExampleMethod<GAppvalProcCondsModel>(string param)

		#endregion
    }
}
