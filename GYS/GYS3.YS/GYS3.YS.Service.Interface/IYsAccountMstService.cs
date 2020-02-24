#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Service.Interface
    * 类 名 称：			IYsAccountMstService
    * 文 件 名：			IYsAccountMstService.cs
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

namespace GYS3.YS.Service.Interface
{
	/// <summary>
	/// YsAccountMst服务组装层接口
	/// </summary>
    public partial interface IYsAccountMstService : IEntServiceBase<YsAccountMstModel>
    {
		#region IYsAccountMstService 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="ysAccountMstEntity"></param>
		/// <param name="ysAccountEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveYsAccountMst(YsAccountMstModel ysAccountMstEntity, List<YsAccountModel> ysAccountEntities);

        /// <summary>
        /// 通过外键值获取YsAccount明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<YsAccountModel> FindYsAccountByForeignKey<TValType>(TValType id);


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
        /// 修改预决算说明书
        /// </summary>
        /// <param name="ysAccountMst">主表对象</param>
        /// <returns></returns>
        SavedResult<long> UpdateAccountMst(YsAccountMstModel ysAccountMst);

        /// <summary>
        /// 根据组织获取各个上报组织的数量
        /// </summary>
        /// <param name="orgCode">组织code</param>
        /// <param name="year">对应年份</param>
        /// <returns></returns>
        Dictionary<string, object> GetYsAccountNum(string orgCode, string year);

        /// <summary>
        /// 根据组织获取该组织以及其下级的所有汇总信息
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <param name="year">年份</param>
        /// <param name="chooseOwn">是否包含本级</param>
        /// <param name="verify">用来判断年初年中年末</param>
        /// <returns></returns>
        YsAccountMstModel GetAllYsAccountList(long orgId, string orgCode, string year, int chooseOwn, string verify);

        /// <summary>
        /// 给所选组织的本级与下级组织打上审批记号
        /// </summary>
        /// <param name="orgCode">组织编码</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        IList<OrganizeModel> GetOrganizeVerifyList(string orgCode, string year);


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

        /// <summary>
        /// 导出年初预算报表
        /// </summary>
        /// <param name="datas">数据集合</param>
        /// <param name="title">标题</param>
        /// <param name="userModel">用户对象</param>
        /// <param name="organizeModel">组织对象</param>
        /// <returns></returns>
        String GetBeginExcel(YsAccountModel[] datas, String[] title, User2Model userModel, OrganizeModel organizeModel);

        /// <summary>
        /// 导出年中预算报表
        /// </summary>
        /// <param name="datas">数据集合</param>
        /// <param name="title">标题</param>
        /// <param name="userModel">用户对象</param>
        /// <param name="organizeModel">组织对象</param>
        /// <returns></returns>
        String GetMiddleExcel(YsAccountModel[] datas, String[] title, User2Model userModel, OrganizeModel organizeModel);

        /// <summary>
        /// 导出年末决算报表
        /// </summary>
        /// <param name="datas">数据集合</param>
        /// <param name="title">标题</param>
        /// <param name="userModel">用户对象</param>
        /// <param name="organizeModel">组织对象</param>
        /// <returns></returns>
        String GetEndExcel(YsAccountModel[] datas, String[] title, User2Model userModel, OrganizeModel organizeModel);
        #endregion
    }
}
