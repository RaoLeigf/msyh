#region Summary
/**************************************************************************************
    * 命名空间：			GGK3.GK.Dac
    * 类 名 称：			GKPaymentDtlDac
    * 文 件 名：			GKPaymentDtlDac.cs
    * 创建时间：			2019/5/20 
    * 作    者：			李明    
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
using Enterprise3.NHORM.Dac;

using GGK3.GK.Model.Domain;
using GGK3.GK.Dac.Interface;

namespace GGK3.GK.Dac
{
	/// <summary>
	/// GKPaymentDtl数据访问处理类
	/// </summary>
    public partial class GKPaymentDtlDac : EntDacBase<GKPaymentDtlModel>, IGKPaymentDtlDac
    {
		#region 实现 IGKPaymentDtlDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<GKPaymentDtlModel> ExampleMethod<GKPaymentDtl>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

