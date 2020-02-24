#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Facade.Interface
    * 类 名 称：			IQTControlSetFacade
    * 文 件 名：			IQTControlSetFacade.cs
    * 创建时间：			2019/4/3 
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
using System.Text;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GQT3.QT.Model.Domain;

namespace GQT3.QT.Facade.Interface
{
	/// <summary>
	/// QTControlSet业务组装层接口
	/// </summary>
    public partial interface IQTControlSetFacade : IEntFacadeBase<QTControlSetModel>
    {
		#region IQTControlSetFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<QTControlSetModel> ExampleMethod<QTControlSetModel>(string param)

		#endregion
    }
}
