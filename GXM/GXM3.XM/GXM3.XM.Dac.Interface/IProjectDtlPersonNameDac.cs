#region Summary
/**************************************************************************************
    * 命名空间：			GXM3.XM.Dac.Interface
    * 类 名 称：			IProjectDtlPersonNameDac
    * 文 件 名：			IProjectDtlPersonNameDac.cs
    * 创建时间：			2020/1/14 
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

using GXM3.XM.Model.Domain;

namespace GXM3.XM.Dac.Interface
{
	/// <summary>
	/// ProjectDtlPersonName数据访问层接口
	/// </summary>
    public partial interface IProjectDtlPersonNameDac : IEntDacBase<ProjectDtlPersonNameModel>
    {
		#region IProjectDtlPersonNameDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<ProjectDtlPersonNameModel> ExampleMethod<ProjectDtlPersonNameModel>(string param)

		#endregion
    }
}

