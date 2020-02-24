#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Dac
    * 类 名 称：			GAppvalProcDac
    * 文 件 名：			GAppvalProcDac.cs
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

using GSP3.SP.Model.Domain;
using GSP3.SP.Dac.Interface;

namespace GSP3.SP.Dac
{
	/// <summary>
	/// GAppvalProc数据访问处理类
	/// </summary>
    public partial class GAppvalProcDac : EntDacBase<GAppvalProcModel>, IGAppvalProcDac
    {
		#region 实现 IGAppvalProcDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<GAppvalProcModel> ExampleMethod<GAppvalProc>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

