#region Summary
/**************************************************************************************
    * 类 名 称：        ILogOtherCfgService
    * 命名空间：        SUP3.Log.Service.Interface
    * 文 件 名：        ILogOtherCfgService.cs
    * 创建时间：        2017/10/16 
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

namespace SUP3.Log.Service.Interface
{
	/// <summary>
	/// LogOtherCfg服务组装层接口
	/// </summary>
    public partial interface ILogOtherCfgService : IEntServiceBase<LogOtherCfgModel>
    {
        #region ILogOtherCfgService 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<LogOtherCfgModel> ExampleMethod<LogOtherCfgModel>(string param)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        SavedResult<long> SaveOtherCfg(IList<LogOtherCfgModel> data);
        #endregion
    }
}
