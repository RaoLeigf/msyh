#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service.Interface
    * 类 名 称：			IQTSysSetService
    * 文 件 名：			IQTSysSetService.cs
    * 创建时间：			2019/6/3 
    * 作    者：			刘杭    
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
	/// QTSysSet服务组装层接口
	/// </summary>
    public partial interface IQTSysSetService : IEntServiceBase<QTSysSetModel>
    {
        #region IQTSysSetService 业务添加的成员

        /// <summary>
        /// 保存审批类型
        /// </summary>
        /// <param name="sysSetModel"></param>
        /// <returns></returns>
        SavedResult<Int64> PostAddProcType(QTSysSetModel sysSetModel);

        /// <summary>
        /// 获取审批类型
        /// </summary>
        /// <returns></returns>
        List<QTSysSetModel> GetProcTypes();

        /// <summary>
        /// 修改审批类型
        /// </summary>
        /// <param name="sysSetModel"></param>
        /// <returns></returns>
        SavedResult<Int64> PostUpdateProcType(QTSysSetModel sysSetModel);

        /// <summary>
        /// 返回用户是否启用了Usb加密狗支付，如果启用了则返回对应信息
        /// </summary>
        /// <param name="user_phid">用户phid</param>
        /// <param name="usbKey">启用后，返回UsbKey</param>
        /// <param name="start_dt">启用后，返回启用时间</param>
        /// <param name="endDt">启用后，返回失效时间</param>
        /// <returns></returns>
        bool GetPayUsbKeyIsActive(long user_phid, out string usbKey, out DateTime start_dt, out DateTime endDt);

        /// <summary>
        /// 根据id获取组织
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        OrganizeModel GetOrg(long orgid);

        /// <summary>
        /// 获取所有的组织信息
        /// </summary>
        /// <returns></returns>
        List<OrganizeModel> GetAllOrgs();

        /// <summary>
        /// 根据id获取用户
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        User2Model GetUser(long userid);

        /// <summary>
        /// 根据组织以及类型获取不同的基础数据集合
        /// </summary>
        /// <param name="orgCode">组织编码</param>
        /// <param name="dicType">基础数据类别</param>
        /// <returns></returns>
        IList<QTSysSetModel> GetSetListByOrgType(string orgCode, string dicType);
        #endregion
    }
}
