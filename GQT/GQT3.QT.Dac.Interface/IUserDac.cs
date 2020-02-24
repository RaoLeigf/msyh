#region Summary
/**************************************************************************************
    * 类 名 称：        IUserDac
    * 命名空间：        GQT3.QT.Dac.Interface
    * 文 件 名：        IUserDac.cs
    * 创建时间：        2018/9/13 
    * 作    者：        夏华军    
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
	/// User数据访问层接口
	/// </summary>
    public partial interface IUserDac : IEntDacBase<User2Model>
    {
		#region IUserDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<UserModel> ExampleMethod<UserModel>(string param)

		#endregion
    }
}

