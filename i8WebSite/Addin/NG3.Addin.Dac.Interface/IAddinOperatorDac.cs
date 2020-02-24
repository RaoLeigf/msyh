#region Summary
/**************************************************************************************
    * 类 名 称：        IAddinOperatorDac
    * 命名空间：        NG3.Addin.Dac.Interface
    * 文 件 名：        IAddinOperatorDac.cs
    * 创建时间：        2017/8/3 
    * 作    者：        韦忠吉    
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

using NG3.Addin.Model.Domain;

namespace NG3.Addin.Dac.Interface
{
	/// <summary>
	/// AddinOperator数据访问层接口
	/// </summary>
    public partial interface IAddinOperatorDac : IEntDacBase<AddinOperatorModel>
    {
		#region IAddinOperatorDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<AddinOperatorModel> ExampleMethod<AddinOperatorModel>(string param)

		#endregion
    }
}

