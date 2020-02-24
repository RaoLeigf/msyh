#region Summary
/**************************************************************************************
    * 类 名 称：        IProjectDtlFundApplDac
    * 命名空间：        GXM3.XM.Dac.Interface
    * 文 件 名：        IProjectDtlFundApplDac.cs
    * 创建时间：        2018/8/28 
    * 作    者：        李明    
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
	/// ProjectDtlFundAppl数据访问层接口
	/// </summary>
    public partial interface IProjectDtlFundApplDac : IEntDacBase<ProjectDtlFundApplModel>
    {
		#region IProjectDtlFundApplDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<ProjectDtlFundApplModel> ExampleMethod<ProjectDtlFundApplModel>(string param)

		#endregion
    }
}

