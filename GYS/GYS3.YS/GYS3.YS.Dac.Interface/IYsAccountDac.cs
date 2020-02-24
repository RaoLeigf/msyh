#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Dac.Interface
    * 类 名 称：			IYsAccountDac
    * 文 件 名：			IYsAccountDac.cs
    * 创建时间：			2019/9/23 
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

using GYS3.YS.Model.Domain;

namespace GYS3.YS.Dac.Interface
{
	/// <summary>
	/// YsAccount数据访问层接口
	/// </summary>
    public partial interface IYsAccountDac : IEntDacBase<YsAccountModel>
    {
		#region IYsAccountDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<YsAccountModel> ExampleMethod<YsAccountModel>(string param)

		#endregion
    }
}

