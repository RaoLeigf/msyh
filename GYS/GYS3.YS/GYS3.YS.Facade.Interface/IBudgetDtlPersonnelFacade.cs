#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Facade.Interface
    * 类 名 称：			IBudgetDtlPersonnelFacade
    * 文 件 名：			IBudgetDtlPersonnelFacade.cs
    * 创建时间：			2020/1/2 
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
using System.Text;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GYS3.YS.Model.Domain;

namespace GYS3.YS.Facade.Interface
{
	/// <summary>
	/// BudgetDtlPersonnel业务组装层接口
	/// </summary>
    public partial interface IBudgetDtlPersonnelFacade : IEntFacadeBase<BudgetDtlPersonnelModel>
    {
		#region IBudgetDtlPersonnelFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<BudgetDtlPersonnelModel> ExampleMethod<BudgetDtlPersonnelModel>(string param)

		#endregion
    }
}
