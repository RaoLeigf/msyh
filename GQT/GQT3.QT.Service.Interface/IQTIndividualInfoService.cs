#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service.Interface
    * 类 名 称：			IQTIndividualInfoService
    * 文 件 名：			IQTIndividualInfoService.cs
    * 创建时间：			2019/5/14 
    * 作    者：			董泉伟    
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
	/// QTIndividualInfo服务组装层接口
	/// </summary>
    public partial interface IQTIndividualInfoService : IEntServiceBase<QTIndividualInfoModel>
    {
        #region IQTIndividualInfoService 业务添加的成员

        /// <summary>
        /// 保存自定义表单跟金额关联设置
        /// </summary>
        /// <param name="templePhid"></param>
        /// <param name="phid"></param>
        /// <param name="bustype"></param>
        /// <returns></returns>
        int SaveTemple(long templePhid, string bustype, long phid);
        /// <summary>
        /// 删除自定义表单跟金额关联设置
        /// </summary>
        /// <param name="phid"></param>
        ///  <param name="bustype"></param>
        /// <returns></returns>
        int DeleteTemple(string bustype, long phid);

        /// <summary>
        /// 根据组织代码串得到组织
        /// </summary>
        /// <param name="OrgStr"></param>
        /// <returns></returns>
        IList<OrganizeModel> GetUseOrg(string OrgStr);

        /// <summary>
        /// 根据组织代码串得到组织(非)
        /// </summary>
        /// <param name="OrgStr"></param>
        /// <returns></returns>
        IList<OrganizeModel> GetNoUseOrg(string OrgStr);
        #endregion
    }
}
