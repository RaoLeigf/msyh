#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Facade.Interface
    * 类 名 称：			IYsAccountMstFacade
    * 文 件 名：			IYsAccountMstFacade.cs
    * 创建时间：			2019/9/23 
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
using GYS3.YS.Model.Domain;

namespace GYS3.YS.Facade.Interface
{
	/// <summary>
	/// YsAccountMst业务组装层接口
	/// </summary>
    public partial interface IYsAccountMstFacade : IEntFacadeBase<YsAccountMstModel>
    {
		#region IYsAccountMstFacade 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="ysAccountMstEntity"></param>
		/// <param name="ysAccountEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveYsAccountMst(YsAccountMstModel ysAccountMstEntity, List<YsAccountModel> ysAccountEntities);


        /// <summary>
        /// 保存预决算报表
        /// </summary>
        /// <param name="ysAccountMst">对象</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <param name="year">年份</param>
        /// <param name="uid">用户id</param>
        /// <param name="verify">用来判断年初，年中，年末（1、年初，2、年中，3、年末）</param>
        /// <returns></returns>
        SavedResult<long> SaveYsAccount(YsAccountMstModel ysAccountMst, long orgId, string orgCode, string year, long uid, string verify);

        /// <summary>
        /// 根据组织获取各个上报组织的数量
        /// </summary>
        /// <param name="orgCode">组织code</param>
        /// <param name="year">对应年份</param>
        /// <returns></returns>
        Dictionary<string, object> GetYsAccountNum(string orgCode, string year);

        /// <summary>
        /// 根据组织和年份获取该组织以及下级组织已上报所有明细数据
        /// </summary>
        /// <param name="orgCode">组织编码</param>
        /// <param name="year">年份</param>
        /// <param name="chooseOwn">是否包含本级组织（0-包含，1-不包含）</param>
        /// <param name="verify">用来筛选年初年中年末</param>
        /// <returns></returns>
        List<YsAccountModel> GetAllYsAccountList(string orgCode, string year, int chooseOwn, string verify);

        /// <summary>
        /// 根据组织获取该组织预算科目对应的初始预决算集合
        /// </summary>
        /// <param name="orgCode">组织Code</param>
        /// <param name="orgId">组织id</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        List<YsAccountModel> GetYsAccountsBySubject(string orgCode, long orgId, string year);

        /// <summary>
        /// 给所选组织的本级与下级组织打上审批记号
        /// </summary>
        /// <param name="orgCode">组织编码</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        IList<OrganizeModel> GetOrganizeVerifyList(string orgCode, string year);
        #endregion
    }
}
