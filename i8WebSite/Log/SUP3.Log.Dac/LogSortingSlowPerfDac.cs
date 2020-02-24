#region Summary
/**************************************************************************************
    * 类 名 称：        LogSortingSlowPerfDac
    * 命名空间：        SUP3.Log.Dac
    * 文 件 名：        LogSortingSlowPerfDac.cs
    * 创建时间：        2017/11/4 
    * 作    者：        洪鹏    
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

using SUP3.Log.Model.Domain;
using SUP3.Log.Dac.Interface;

namespace SUP3.Log.Dac
{
	/// <summary>
	/// LogSortingSlowPerf数据访问处理类
	/// </summary>
    public partial class LogSortingSlowPerfDac : EntDacBase<LogSortingSlowPerfModel>, ILogSortingSlowPerfDac
    {
		#region 实现 ILogSortingSlowPerfDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public List<LogSortingSlowPerfModel> ExampleMethod<LogSortingSlowPerf>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

