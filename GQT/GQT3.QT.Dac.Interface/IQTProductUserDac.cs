#region Summary
/**************************************************************************************
    * 类 名 称：        IQTProductUserDac
    * 命名空间：        GQT3.QT.Dac.Interface
    * 文 件 名：        IQTProductUserDac.cs
    * 创建时间：        2018/12/12 
    * 作    者：        刘杭    
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
	/// QTProductUser数据访问层接口
	/// </summary>
    public partial interface IQTProductUserDac : IEntDacBase<QTProductUserModel>
    {
		#region IQTProductUserDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<QTProductUserModel> ExampleMethod<QTProductUserModel>(string param)

		#endregion
    }
}

