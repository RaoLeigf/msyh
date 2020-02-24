#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service.Interface
    * 类 名 称：			IQtCoverUpForOrgService
    * 文 件 名：			IQtCoverUpForOrgService.cs
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
using Enterprise3.WebApi.GQT3.QT.Model.Response;
using GQT3.QT.Model.Domain;

namespace GQT3.QT.Service.Interface
{
	/// <summary>
	/// QtCoverUpForOrg服务组装层接口
	/// </summary>
    public partial interface IQtCoverUpForOrgService : IEntServiceBase<QtCoverUpForOrgModel>
    {
        #region IQtCoverUpForOrgService 业务添加的成员


        /// <summary>
        /// 根据所选的过程编码，返回所有组织的套打数据
        /// </summary>
        /// <param name="processCode">过程编码</param>
        /// <param name="processName">过程名称</param>
        /// <returns></returns>
        IList<QtCoverUpForOrgModel> GetQtCoverUpForOrgs(string processCode, string processName);


        /// <summary>
        /// 修改所有组织的套打数据
        /// </summary>
        /// <param name="allCoverUps">套打数据集合</param>
        /// <returns></returns>
        SavedResult<long> UpdateCoverUpList(List<AllCoverUpModel> allCoverUps);

        /// <summary>
        /// 根据过程组织获取对应的打印格式
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="processCode">过程编码</param>
        /// <returns></returns>
        IList<QtCoverUpForOrgModel> GetCoverUpByOrg(long orgId, string processCode);
        #endregion
    }
}
