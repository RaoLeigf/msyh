#region Summary
/**************************************************************************************
    * 类 名 称：        ICorrespondenceSettingsService
    * 命名空间：        GQT3.QT.Service.Interface
    * 文 件 名：        ICorrespondenceSettingsService.cs
    * 创建时间：        2018/9/3 
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
using SUP.Common.DataEntity;

using GQT3.QT.Model.Domain;
using GQT3.QT.Model;
using SUP.Common.DataEntity;
using System.Data;
using GQT3.QT.Model.Extra;

namespace GQT3.QT.Service.Interface
{
	/// <summary>
	/// CorrespondenceSettings服务组装层接口
	/// </summary>
    public partial interface ICorrespondenceSettingsService : IEntServiceBase<CorrespondenceSettingsModel>
    {
        #region ICorrespondenceSettingsService 业务添加的成员

        /// <summary>
        /// 取对应关系列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        PagedResult<User2Model> LoadWithPageYSBM(DataStoreParam dataStore);

        /// <summary>
        /// 取对应关系列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        PagedResult<OrganizeModel> LoadWithPageYSBM2(DataStoreParam dataStore,string userCode);

        /// <summary>
        /// 取对应关系列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        PagedResult<OrganizeModel> GetRelationYSBMLeftList(DataStoreParam dataStore, string userCode);
        
        /// <summary>
        /// 取对应关系列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        PagedResult<OrganizeModel> GetRelationYSBMRightList(string userCode);

        /// <summary>
        /// 设置操作员对应预算部门关系
        /// </summary>
        /// <returns>返回Json串</returns>
        List<CorrespondenceSettingsModel> UpdataRelationYSBM(List<OrganizeModel> models,string UserNo);

        /// <summary>
        /// 取对应关系列表数据  操作员对应组织部门
        /// </summary>
        /// <returns>返回Json串</returns>
        PagedResult<CorrespondenceSettingsModel> LoadWithPageUser_Org(DataStoreParam dataStore, string userCode);

        /// <summary>
        /// 取所有org（不包括部门）
        /// </summary>
        /// <returns>返回Json串</returns>
        PagedResult<OrganizeModel> LoadWithPageOrg(DataStoreParam dataStore);

        /// <summary>
        /// 取所有org（包括部门）
        /// </summary>
        /// <returns>返回Json串</returns>
        PagedResult<OrganizeModel> LoadWithPageAllOrg(DataStoreParam dataStore);

        /// <summary>
        /// 根据组织id取部门
        /// </summary>
        /// <returns>返回Json串</returns>
        PagedResult<OrganizeModel> LoadWithPageBM(DataStoreParam dataStore, string OrgId);

        /// <summary>
        /// 资金来源关系的改变
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <param name="OrgPhId"></param>
        /// <param name="mydelete"></param>
        /// <param name="myinsert"></param>
        /// <returns></returns>
        CommonResult<CorrespondenceSettingsModel> UpdateZJLY(string OrgCode, string OrgPhId,List<CorrespondenceSettingsModel> mydelete, List<SourceOfFundsModel> myinsert);

        /// <summary>
        /// 预算科目关系的改变
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <param name="OrgPhId"></param>
        /// <param name="OrgName"></param>
        /// <param name="mydelete"></param>
        /// <param name="myinsert"></param>
        /// <returns></returns>
        CommonResult<CorrespondenceSettingsModel> UpdateYSKM(string OrgCode, string OrgPhId, string OrgName, List<CorrespondenceSettingsModel> mydelete, List<BudgetAccountsModel> myinsert);

        /// <summary>
        /// 预算库关系的改变
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <param name="OrgPhId"></param>
        /// <param name="mydelete"></param>
        /// <param name="myinsert"></param>
        /// <returns></returns>
        CommonResult<CorrespondenceSettingsModel> UpdateYSK(string OrgCode, string OrgPhId, List<CorrespondenceSettingsModel> mydelete, List<ProjLibProjModel> myinsert);

        /// <summary>
        /// 操作员默认组织设置
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="username"></param>
        /// <param name="mydelete"></param>
        /// <param name="myinsert"></param>
        /// <returns></returns>
        CommonResult<CorrespondenceSettingsModel> UpdateDefaultOrg(string usercode, string username, List<CorrespondenceSettingsModel> mydelete, List<OrganizeModel> myinsert);

        /// <summary>
        /// 取操作员能操作的org
        /// </summary>
        /// <returns>返回Json串</returns>
        PagedResult<OrganizeModel> LoadWithPageOrgByUser(DataStoreParam dataStore, string userid);

        /// <summary>
        /// 删除单位对应预算科目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CommonResult DeleteQtbase(long id);

        /// <summary>
        /// 根据操作员和申报单位取预算部门
        /// </summary>
        /// <param name="Usercoode"></param>
        /// <param name="Unit"></param>
        /// <returns></returns>
        IList<OrganizeModel> GetDept(string Usercoode, string Unit);

        /// <summary>
        /// 组织树
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        List<OrganizeModel> GetOrgTree(long UserId);

        /// <summary>
        /// 得到登录信息
        /// </summary>
        /// <returns></returns>
        string GetLogin();

        /// <summary>
        /// 根据单位CODE取部门
        /// </summary>
        /// <param name="OrgID"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        IList<OrganizeModel> GetDeptByUnit(long OrgID, long UserId);

        /// <summary>
        /// 完整组织树（没有权限，包括部门）
        /// </summary>
        /// <returns></returns>
        List<OrganizeModel> GetALLOrgTree();

        /// <summary>
        /// 根据组织或者部门获取操作员
        /// </summary>
        /// <param name="Org"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryStr"></param>
        /// <returns></returns>
        FindedResults<CorrespondenceSettingsModel> getUserByOrg(string Org, string queryStr);

        /// <summary>
        /// 得到子级(包括部门)(不是树 是list)
        /// </summary>
        /// <returns></returns>
        List<OrganizeModel> GetAllChildList(long OrgId);

        /// <summary>
        /// 得到子级(包括部门)(树)
        /// </summary>
        /// <returns></returns>
        List<OrganizeModel> GetAllChildTree(long OrgId);

        /// <summary>
        /// 得到完整登录信息（组织用户）
        /// </summary>
        /// <returns></returns>
        string GetLogininfo(long OrgId, long UserId);

        /// <summary>
        /// 得到组织
        /// </summary>
        /// <returns></returns>
        OrganizeModel GetOrg(string OrgCode);

        /// <summary>
        /// 取操作员对应预算部门
        /// </summary>
        /// <returns>返回Json串</returns>
        List<OrganizeModel> FindYSBM(string userCode);

        /// <summary>
        /// 得到包含自己及下级的组织树
        /// </summary>
        /// <param name="orgphid"></param>
        /// <returns></returns>
        OrganizeModel GetOrg_tree(long orgphid);
        #endregion


        #region 物料有关
        /// <summary>
        /// 获取模块
        /// </summary>
        List<QtModulesModel> GetModules();

        /// <summary>
        /// 获取当前帐套对应的登录组织(并判断是否有权限)
        /// </summary>
        /// <param name="moduleno"></param>
        /// <returns></returns>
        List<QtOrgModel> GetLoginOrg(string moduleno);

        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="moduleno"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        int SaveRights(string moduleno, List<QtModulerightsModel> list);
        #endregion

        /// <summary>
        /// 得到组织拼接成的字符串
        /// </summary>
        /// <param name="orgphidList"></param>
        /// <returns></returns>
        string GetOrgStr(List<long> orgphidList);

        /// <summary>
        /// 得到组织列表
        /// </summary>
        /// <param name="orgphidList"></param>
        /// <returns></returns>
        List<OrganizeModel> GetOrgInfo(List<long> orgIds);

        /// <summary>
        /// 得到组织代码列表
        /// </summary>
        /// <param name="orgphidList"></param>
        /// <returns></returns>
        List<OrganizeModel> GetOrgCodeList(List<long> orgphidList);

        /// <summary>
        /// 根据操作员组织权限列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        IList<OrganizeModel> GetAuthOrgList(long UserId);
    }
}
