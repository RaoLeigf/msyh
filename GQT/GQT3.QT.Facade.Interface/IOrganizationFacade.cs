#region Summary
/**************************************************************************************
    * 类 名 称：        IOrganizationFacade
    * 命名空间：        GQT3.QT.Facade.Interface
    * 文 件 名：        IOrganizationFacade.cs
    * 创建时间：        2018/9/13 
    * 作    者：        夏华军    
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

namespace GQT3.QT.Facade.Interface
{
	/// <summary>
	/// Organization业务组装层接口
	/// </summary>
    public partial interface IOrganizationFacade : IEntFacadeBase<OrganizeModel>
    {
		#region IOrganizationFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<OrganizationModel> ExampleMethod<OrganizationModel>(string param)

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="organizationEntity"></param>
		/// <param name="userEntities"></param>
		/// <param name="organizationEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveOrganization(OrganizeModel organizationEntity, List<User2Model> userEntities, List<OrganizeModel> organizationEntities);

        /// <summary>
        /// 通过代码找名称
        /// </summary>
        /// <param name="Dm"></param>
        /// <returns></returns>
        string FindMcByDm(string Dm);

        /// <summary>
        /// 根据操作员取申报单位
        /// </summary>
        /// <param name="USERID"></param>
        /// <returns></returns>
        IList<OrganizeModel> GetSBUnit(long USERID);

        /// <summary>
        /// 根据操作员和申报单位取预算部门
        /// </summary>
        /// <param name="Usercoode"></param>
        /// <param name="Unit"></param>
        /// <returns></returns>
        IList<OrganizeModel> GetDept(string Usercoode, string Unit);

        /// <summary>
        /// 根据操作员取组织树（是否需要权限 WeChatId判断）
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        List<OrganizeModel> GetOrgTree(long UserId);

        /// <summary>
        /// 得到子级
        /// </summary>
        /// <param name="orgTree"></param>
        /// <param name="OrgPhids"></param>
        /// <returns></returns>
        List<OrganizeModel> GetChild(OrganizeModel orgTree, List<Int64> OrgPhids);

        /// <summary>
        /// 取组织树（包括部门）
        /// </summary>
        /// <returns></returns>
        List<OrganizeModel> GetALLOrgTree();

        /// <summary>
        /// 得到子级(包括部门)(不是树 是list)
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="OrgList"></param>
        /// <returns></returns>
        List<OrganizeModel> GetAllChildList(long orgId, List<OrganizeModel> OrgList = null);


        /// <summary>
        /// 得到子级(包括部门)(树)
        /// </summary>
        /// <param name="orgTree"></param>
        /// <returns></returns>
        List<OrganizeModel> GetAllChild(OrganizeModel orgTree);
        #endregion
    }
}
