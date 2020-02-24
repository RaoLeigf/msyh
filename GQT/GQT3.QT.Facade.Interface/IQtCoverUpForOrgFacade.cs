#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Facade.Interface
    * 类 名 称：			IQtCoverUpForOrgFacade
    * 文 件 名：			IQtCoverUpForOrgFacade.cs
    * 创建时间：			2019/10/29 
    * 作    者：			王冠冠    
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
	/// QtCoverUpForOrg业务组装层接口
	/// </summary>
    public partial interface IQtCoverUpForOrgFacade : IEntFacadeBase<QtCoverUpForOrgModel>
    {
		#region IQtCoverUpForOrgFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<QtCoverUpForOrgModel> ExampleMethod<QtCoverUpForOrgModel>(string param)

		#endregion
    }
}
