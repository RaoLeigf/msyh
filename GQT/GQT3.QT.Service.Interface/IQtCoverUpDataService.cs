#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service.Interface
    * 类 名 称：			IQtCoverUpDataService
    * 文 件 名：			IQtCoverUpDataService.cs
    * 创建时间：			2019/10/29 
    * 作    者：			王冠冠    
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

using GQT3.QT.Model.Domain;

namespace GQT3.QT.Service.Interface
{
	/// <summary>
	/// QtCoverUpData服务组装层接口
	/// </summary>
    public partial interface IQtCoverUpDataService : IEntServiceBase<QtCoverUpDataModel>
    {
        #region IQtCoverUpDataService 业务添加的成员

        /// <summary>
        /// 获取内置的启用的内置套打格式数据
        /// </summary>
        /// <returns></returns>
        IList<QtCoverUpDataModel> GetQtCoverUpDataList();
        #endregion
    }
}
