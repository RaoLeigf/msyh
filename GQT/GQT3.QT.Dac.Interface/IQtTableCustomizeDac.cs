#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Dac.Interface
    * 类 名 称：			IQtTableCustomizeDac
    * 文 件 名：			IQtTableCustomizeDac.cs
    * 创建时间：			2019/11/26 
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
	/// QtTableCustomize数据访问层接口
	/// </summary>
    public partial interface IQtTableCustomizeDac : IEntDacBase<QtTableCustomizeModel>
    {
		#region IQtTableCustomizeDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<QtTableCustomizeModel> ExampleMethod<QtTableCustomizeModel>(string param)

		#endregion
    }
}

