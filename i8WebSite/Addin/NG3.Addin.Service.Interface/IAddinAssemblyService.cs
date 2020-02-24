#region Summary
/**************************************************************************************
    * 类 名 称：        IAddinAssemblyService
    * 命名空间：        NG3.Addin.Service.Interface
    * 文 件 名：        IAddinAssemblyService.cs
    * 创建时间：        2017/7/10 
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
using System.Text;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using NG3.Addin.Model.Domain;

namespace NG3.Addin.Service.Interface
{
	/// <summary>
	/// AddinAssembly服务组装层接口
	/// </summary>
    public partial interface IAddinAssemblyService : IEntServiceBase<AddinAssemblyModel>
    {
		#region IAddinAssemblyService 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<AddinAssemblyModel> ExampleMethod<AddinAssemblyModel>(string param)


		#endregion
    }
}
