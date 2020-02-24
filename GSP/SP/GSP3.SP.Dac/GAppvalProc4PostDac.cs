#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Dac
    * 类 名 称：			GAppvalProc4PostDac
    * 文 件 名：			GAppvalProc4PostDac.cs
    * 创建时间：			2019/5/21 
    * 作    者：			刘杭    
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

using GSP3.SP.Model.Domain;
using GSP3.SP.Dac.Interface;

namespace GSP3.SP.Dac
{
	/// <summary>
	/// GAppvalProc4Post数据访问处理类
	/// </summary>
    public partial class GAppvalProc4PostDac : EntDacBase<GAppvalProc4PostModel>, IGAppvalProc4PostDac
    {
		#region 实现 IGAppvalProc4PostDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<GAppvalProc4PostModel> ExampleMethod<GAppvalProc4Post>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

