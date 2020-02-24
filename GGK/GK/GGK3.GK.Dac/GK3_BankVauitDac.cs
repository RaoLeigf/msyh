#region Summary
/**************************************************************************************
    * 命名空间：			GGK3.GK.Dac
    * 类 名 称：			GK3_BankVauitDac
    * 文 件 名：			GK3_BankVauitDac.cs
    * 创建时间：			2019/11/18 
    * 作    者：			张宇    
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
	/// GK3_BankVauit数据访问处理类
	/// </summary>
    public partial class GK3_BankVauitDac : EntDacBase<GK3_BankVauitModel>, IGK3_BankVauitDac
    {
		#region 实现 IGK3_BankVauitDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<GK3_BankVauitModel> ExampleMethod<GK3_BankVauit>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

