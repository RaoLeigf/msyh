#region Summary
/**************************************************************************************
    * 类 名 称：        ILogPerfFacade
    * 命名空间：        SUP3.Log.Facade.Interface
    * 文 件 名：        ILogPerfFacade.cs
    * 创建时间：        2017/10/9 
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
using System.Text;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using SUP3.Log.Model.Domain;

namespace SUP3.Log.Facade.Interface
{
	/// <summary>
	/// LogPerf业务组装层接口
	/// </summary>
    public partial interface ILogPerfFacade : IEntFacadeBase<LogPerfModel>
    {
		#region ILogPerfFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<LogPerfModel> ExampleMethod<LogPerfModel>(string param)


		#endregion
    }
}
