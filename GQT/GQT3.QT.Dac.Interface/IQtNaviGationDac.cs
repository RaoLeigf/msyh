#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Dac.Interface
    * 类 名 称：			IQtNaviGationDac
    * 文 件 名：			IQtNaviGationDac.cs
    * 创建时间：			2019/11/14 
    * 作    者：			张宇    
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
	/// QtNaviGation数据访问层接口
	/// </summary>
    public partial interface IQtNaviGationDac : IEntDacBase<QtNaviGationModel>
    {
		#region IQtNaviGationDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<QtNaviGationModel> ExampleMethod<QtNaviGationModel>(string param)

		#endregion
    }
}

