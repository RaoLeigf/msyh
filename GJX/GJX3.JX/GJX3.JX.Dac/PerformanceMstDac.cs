#region Summary
/**************************************************************************************
    * 类 名 称：        PerformanceMstDac
    * 命名空间：        GJX3.JX.Dac
    * 文 件 名：        PerformanceMstDac.cs
    * 创建时间：        2018/9/12 
    * 作    者：        吾丰明    
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

using GJX3.JX.Model.Domain;
using GJX3.JX.Dac.Interface;

namespace GJX3.JX.Dac
{
	/// <summary>
	/// PerformanceMst数据访问处理类
	/// </summary>
    public partial class PerformanceMstDac : EntDacBase<PerformanceMstModel>, IPerformanceMstDac
    {
		#region 实现 IPerformanceMstDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<PerformanceMstModel> ExampleMethod<PerformanceMst>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

