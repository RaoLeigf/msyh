#region Summary
/**************************************************************************************
    * 类 名 称：        ILogLogsDac
    * 命名空间：        SUP3.Log.Dac.Interface
    * 文 件 名：        ILogLogsDac.cs
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
using Enterprise3.NHORM.Interface.EntBase;

using SUP3.Log.Model.Domain;

namespace SUP3.Log.Dac.Interface
{
	/// <summary>
	/// LogLogs数据访问层接口
	/// </summary>
    public partial interface ILogLogsDac : IEntDacBase<LogLogsModel>
    {
		#region ILogLogsDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<LogLogsModel> ExampleMethod<LogLogsModel>(string param)

		#endregion
    }
}

