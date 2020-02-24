#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Facade.Interface
    * 类 名 称：			IYsAccountFacade
    * 文 件 名：			IYsAccountFacade.cs
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

using GYS3.YS.Model.Domain;

namespace GYS3.YS.Facade.Interface
{
	/// <summary>
	/// YsAccount业务组装层接口
	/// </summary>
    public partial interface IYsAccountFacade : IEntFacadeBase<YsAccountModel>
    {
        #region IYsAccountFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<YsAccountModel> ExampleMethod<YsAccountModel>(string param)


        /// <summary>
        /// 根据组织获取该组织的上年与本年决算
        /// </summary>
        /// <param name="orgId">组织ID</param>
        /// <param name="orgCode">组织Code</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        IList<YsAccountModel> GetYsAccounts(string orgId, string orgCode, string year);


        /// <summary>
        /// 根据今年账套的数据库连接串，获取上年账套的数据库连接串
        /// </summary>
        /// <param name="oldConn">今年账套的数据库连接串</param>
        /// <returns></returns>
        string GetNewConn(string oldConn);

        /// <summary>
        /// 修改预决算数据
        /// </summary>
        /// <param name="ysAccounts">列表集合</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        SavedResult<long> SaveAccountList(List<YsAccountModel> ysAccounts, string orgId, string orgCode, string year);

        /// <summary>
        /// 得到年初上报的数据（单个组织）
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        YsAccountMstModel GetBegineAccounts(long orgId, string orgCode, string year);

        /// <summary>
        /// 得到年中上报的数据（单个组织）
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        YsAccountMstModel GetMiddleAccounts(long orgId, string orgCode, string year);

        /// <summary>
        /// 得到年末上报的数据（单个组织）
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        YsAccountMstModel GetEndAccounts(long orgId, string orgCode, string year);
        #endregion
    }
}
