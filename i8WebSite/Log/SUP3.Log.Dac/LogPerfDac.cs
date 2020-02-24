#region Summary
/**************************************************************************************
    * 类 名 称：        LogPerfDac
    * 命名空间：        SUP3.Log.Dac
    * 文 件 名：        LogPerfDac.cs
    * 创建时间：        2017/10/9 
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
	/// LogPerf数据访问处理类
	/// </summary>
    public partial class LogPerfDac : EntDacBase<LogPerfModel>, ILogPerfDac
    {
		#region 实现 ILogPerfDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public List<LogPerfModel> ExampleMethod<LogPerf>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

