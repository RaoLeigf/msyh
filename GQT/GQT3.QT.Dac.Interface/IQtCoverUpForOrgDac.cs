#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Dac.Interface
    * 类 名 称：			IQtCoverUpForOrgDac
    * 文 件 名：			IQtCoverUpForOrgDac.cs
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
using Enterprise3.NHORM.Interface.EntBase;

using GQT3.QT.Model.Domain;

namespace GQT3.QT.Dac.Interface
{
	/// <summary>
	/// QtCoverUpForOrg数据访问层接口
	/// </summary>
    public partial interface IQtCoverUpForOrgDac : IEntDacBase<QtCoverUpForOrgModel>
    {
		#region IQtCoverUpForOrgDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<QtCoverUpForOrgModel> ExampleMethod<QtCoverUpForOrgModel>(string param)

		#endregion
    }
}

