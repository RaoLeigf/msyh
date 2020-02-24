#region Summary
/**************************************************************************************
    * 类 名 称：        ILogSortingSlowSqlFacade
    * 命名空间：        SUP3.Log.Facade.Interface
    * 文 件 名：        ILogSortingSlowSqlFacade.cs
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
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using SUP3.Log.Model.Domain;

namespace SUP3.Log.Facade.Interface
{
	/// <summary>
	/// LogSortingSlowSql业务组装层接口
	/// </summary>
    public partial interface ILogSortingSlowSqlFacade : IEntFacadeBase<LogSortingSlowSqlModel>
    {
		#region ILogSortingSlowSqlFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<LogSortingSlowSqlModel> ExampleMethod<LogSortingSlowSqlModel>(string param)


		#endregion
    }
}
