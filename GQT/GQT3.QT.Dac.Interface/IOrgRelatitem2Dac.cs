#region Summary
/**************************************************************************************
    * 类 名 称：        IOrgRelatitemDac
    * 命名空间：        GQT3.QT.Dac.Interface
    * 文 件 名：        IOrgRelatitemDac.cs
    * 创建时间：        2018/9/20 
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
	/// OrgRelatitem数据访问层接口
	/// </summary>
    public partial interface IOrgRelatitem2Dac : IEntDacBase<OrgRelatitem2Model>
    {
		#region IOrgRelatitem2Dac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<OrgRelatitem2Model> ExampleMethod<OrgRelatitem2Model>(string param)

		#endregion
    }
}

