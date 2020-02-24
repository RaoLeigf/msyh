#region Summary
/**************************************************************************************
    * 类 名 称：        ICorrespondenceSettings2Facade
    * 命名空间：        GQT3.QT.Facade.Interface
    * 文 件 名：        ICorrespondenceSettings2Facade.cs
    * 创建时间：        2018/9/6 
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
using System.Text;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GQT3.QT.Model.Domain;

namespace GQT3.QT.Facade.Interface
{
	/// <summary>
	/// CorrespondenceSettings2业务组装层接口
	/// </summary>
    public partial interface ICorrespondenceSettings2Facade : IEntFacadeBase<CorrespondenceSettings2Model>
    {
		#region ICorrespondenceSettings2Facade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<CorrespondenceSettings2Model> ExampleMethod<CorrespondenceSettings2Model>(string param)


		#endregion
    }
}
