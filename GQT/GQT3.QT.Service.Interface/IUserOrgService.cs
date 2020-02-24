#region Summary
/**************************************************************************************
    * 类 名 称：        IUserOrgService
    * 命名空间：        GQT3.QT.Service.Interface
    * 文 件 名：        IUserOrgService.cs
    * 创建时间：        2018/9/19 
    * 作    者：        刘杭    
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
	/// UserOrg服务组装层接口
	/// </summary>
    public partial interface IUserOrgService : IEntServiceBase<UserOrganize2Model>
    {
        #region IUserOrgService 业务添加的成员

        /// <summary>
        /// 报表
        /// </summary>
        PagedResult<RWReportModel> GetList(long userId);
        #endregion
    }
}
