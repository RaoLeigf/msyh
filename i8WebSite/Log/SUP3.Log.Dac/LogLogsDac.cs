#region Summary
/**************************************************************************************
    * 类 名 称：        LogLogsDac
    * 命名空间：        SUP3.Log.Dac
    * 文 件 名：        LogLogsDac.cs
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
	/// LogLogs数据访问处理类
	/// </summary>
    public partial class LogLogsDac : EntDacBase<LogLogsModel>, ILogLogsDac
    {
		#region 实现 ILogLogsDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public List<LogLogsModel> ExampleMethod<LogLogs>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

