#region Summary
/**************************************************************************************
    * 类 名 称：        OrganizationFacade
    * 命名空间：        GQT3.QT.Facade
    * 文 件 名：        OrganizationFacade.cs
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
using SUP.Common.DataEntity;
using SUP.Common.DataAccess;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Facade;

using GQT3.QT.Facade.Interface;
using GQT3.QT.Rule.Interface;
using GQT3.QT.Model.Domain;
using Enterprise3.Common.Base.Criterion;

namespace GQT3.QT.Facade
{
    /// <summary>
    /// Organization业务组装处理类
    /// </summary>
    public partial class OrganizationFacade : EntFacadeBase<OrganizeModel>, IOrganizationFacade
    {
		#region 类变量及属性
		/// <summary>
        /// Organization业务逻辑处理对象
        /// </summary>
		IOrganizationRule OrganizationRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IOrganizationRule;
            }
        }
        /// <summary>
        /// User业务逻辑处理对象
        /// </summary>
        /// IUserRule UserRule { get; set; }
        /// <summary>
        /// Organization业务逻辑处理对象
        /// </summary>
        ///IOrganizationRule OrganizationRule { get; set; }


        IUserOrgRule UserOrgRule { get; set; }
        ICorrespondenceSettings2Rule CorrespondenceSettings2Rule { get; set; }
        ICorrespondenceSettingsRule CorrespondenceSettingsRule { get; set; }
        IOrgRelatitem2Rule OrgRelatitem2Rule { get; set; }

        #endregion

        #region 重载方法
        /// <summary>
        /// 通过外关联的单主键值，获取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns>实体</returns>
        public override FindedResults<OrganizeModel> FindByForeignKey<TValType>(TValType id, params string[] sorts)
        {
            FindedResults<OrganizeModel> findedResults = base.FindByForeignKey(id, sorts);

            #region 明细Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<OrganizationModel>(findedResults.Data, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<OrganizationModel>(findedResults.Data, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return findedResults;
        }

        /// <summary>
        /// 通过id，删除数据
        /// </summary>
        /// <param name="id">单主键id值</param>
        public override DeletedResult Delete<TValType>(TValType id)
        {
			//UserRule.DeleteByForeignKey(id);
			//OrganizationRule.DeleteByForeignKey(id);
			return base.Delete(id);
        }

		/// <summary>
        /// 通过ids，删除数据
        /// </summary>
        /// <param name="ids">单主键id集合</param>
        public override DeletedResult Delete<TValType>(IList<TValType> ids)
        {
			//UserRule.DeleteByForeignKey(ids);
			//OrganizationRule.DeleteByForeignKey(ids);
			return base.Delete(ids);
        }
        #endregion

		#region 实现 IOrganizationFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<OrganizationModel> ExampleMethod<OrganizationModel>(string param)
        //{
        //    //编写代码
        //}

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="organizationEntity"></param>
		/// <param name="userEntities"></param>
		/// <param name="organizationEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveOrganization(OrganizeModel organizationEntity, List<User2Model> userEntities, List<OrganizeModel> organizationEntities)
        {
            SavedResult<Int64> savedResult = base.Save<Int64>(organizationEntity);
            if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
            {
				if (userEntities.Count > 0)
				{
					//UserRule.Save(userEntities, savedResult.KeyCodes[0]);
				}
				if (organizationEntities.Count > 0)
				{
					//OrganizationRule.Save(organizationEntities, savedResult.KeyCodes[0]);
				}
            }

			return savedResult;
        }

        /// <summary>
        /// 通过代码找名称
        /// </summary>
        /// <param name="Dm"></param>
        /// <returns></returns>
        public string FindMcByDm(string Dm)
        {
            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                   .Add(ORMRestrictions<string>.Eq("OCode", Dm));
            IList<OrganizeModel> organizes = OrganizationRule.Find(dicWhere);
            if (organizes.Count > 0)
            {
                return organizes[0].OName;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 根据操作员取申报单位
        /// </summary>
        /// <param name="USERID"></param>
        /// <returns></returns>
        public IList<OrganizeModel> GetSBUnit(long USERID)
        {
            //SELECT DISTINCT fg3_userorg.ORGID FROM fg3_userorg WHERE USERID=488181024000001
            List<Int64> PHIDs1 = new List<Int64>();
            List<Int64> PHIDs2 = new List<Int64>();
            Dictionary<string, object> dic_userorg = new Dictionary<string, object>();
            new CreateCriteria(dic_userorg)
                .Add(ORMRestrictions<Int64>.Eq("UserId", USERID));
            IList<UserOrganize2Model> userOrganize2s = UserOrgRule.Find(dic_userorg);
            if (userOrganize2s.Count > 0)
            {
                for (var i = 0; i < userOrganize2s.Count; i++)
                {
                    if (!PHIDs1.Contains(userOrganize2s[i].OrgId))
                    {
                        PHIDs1.Add(userOrganize2s[i].OrgId);
                    }
                }
            }

            Dictionary<string, object> dic_Corr2 = new Dictionary<string, object>();
            new CreateCriteria(dic_Corr2)
                .Add(ORMRestrictions<String>.Eq("Dylx", "SB"));
            IList<CorrespondenceSettings2Model> correspondenceSettings2 = CorrespondenceSettings2Rule.Find(dic_Corr2);
            if (correspondenceSettings2.Count > 0)
            {
                for (var i = 0; i < correspondenceSettings2.Count; i++)
                {
                    if (!PHIDs2.Contains(long.Parse(correspondenceSettings2[i].DefStr2)))
                    {
                        PHIDs2.Add(long.Parse(correspondenceSettings2[i].DefStr2));
                    }
                }
            }
            List<Int64> phid3 = PHIDs1.Intersect(PHIDs2).ToList();
            Dictionary<string, object> dic_org = new Dictionary<string, object>();
            new CreateCriteria(dic_org)
               .Add(ORMRestrictions<String>.Eq("IfCorp", "Y"))
               .Add(ORMRestrictions<String>.Eq("IsActive", "1"))
               .Add(ORMRestrictions<List<Int64>>.In("PhId", phid3));
            IList<OrganizeModel> organizes = OrganizationRule.Find(dic_org, new string[] { "OCode" });
            return organizes;
        }

        /// <summary>
        /// 根据操作员和申报单位取预算部门
        /// </summary>
        /// <param name="Usercoode"></param>
        /// <param name="Unit"></param>
        /// <returns></returns>
        public IList<OrganizeModel> GetDept(string Usercoode, string Unit)
        {
            Dictionary<string, object> dic_Corr = new Dictionary<string, object>();
            new CreateCriteria(dic_Corr)
                .Add(ORMRestrictions<String>.Eq("Dwdm", Usercoode))
                .Add(ORMRestrictions<String>.Eq("Dylx", "97"))
                .Add(ORMRestrictions<String>.LLike("Dydm", Unit));
            IList<CorrespondenceSettingsModel> correspondenceSettings = CorrespondenceSettingsRule.Find(dic_Corr);

            List<string> ocodeList = new List<string>();
            if (correspondenceSettings.Count > 0)
            {
                for (var i = 0; i < correspondenceSettings.Count; i++)
                {
                    if (!ocodeList.Contains(correspondenceSettings[i].Dydm))
                    {
                        ocodeList.Add(correspondenceSettings[i].Dydm);
                    }
                }
            }
            Dictionary<string, object> dic_org = new Dictionary<string, object>();
            new CreateCriteria(dic_org)
               .Add(ORMRestrictions<String>.Eq("IfCorp", "N"))
               .Add(ORMRestrictions<String>.Eq("IsActive", "1"))
               .Add(ORMRestrictions<List<String>>.In("OCode", ocodeList));
            IList<OrganizeModel> organizes = OrganizationRule.Find(dic_org);
            return organizes;
        }

        /// <summary>
        /// 根据操作员取组织树（是否需要权限 WeChatId判断）
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<OrganizeModel> GetOrgTree(long UserId)
        {
            List<OrganizeModel> trees = new List<OrganizeModel>();

            List<Int64> PHIDs1 = new List<Int64>();//存有权限的组织phid 

            Dictionary<string, object> dic_userorg = new Dictionary<string, object>();
            new CreateCriteria(dic_userorg)
                .Add(ORMRestrictions<Int64>.Eq("UserId", UserId));
            //IList<UserOrganize2Model> userOrganize2s = UserOrgRule.Find(dic_userorg);
            List<long> PHIDs2 = UserOrgRule.Find(dic_userorg).ToList().Select(x => x.OrgId).Distinct().ToList();

            Dictionary<string, object> dic_sb = new Dictionary<string, object>();
            new CreateCriteria(dic_sb).
                        Add(ORMRestrictions<string>.Eq("Dylx", "SB"));
            List<Int64> PHIDs3 = CorrespondenceSettings2Rule.Find(dic_sb).ToList().Select(x => long.Parse(x.DefStr2)).Distinct().ToList();

            /*if (PHIDs1.Count > 0)
            {
                for (var i = 0; i < PHIDs1.Count; i++)
                {
                    if (!PHIDs2.Contains(PHIDs1[i]))
                    {
                        PHIDs1.Remove(PHIDs1[i]);
                    }
                }
            }*/
            PHIDs1 = PHIDs3.Intersect(PHIDs2).ToList();



            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic)
               .Add(ORMRestrictions<String>.Eq("RelatId", "lg"))
               .Add(ORMRestrictions<Int64>.Eq("ParentOrgId", 0));
            IList<OrgRelatitem2Model> orgRelatitems = OrgRelatitem2Rule.Find(dic,new string[] { "RelId Asc" });
            if (orgRelatitems.Count > 0)
            {
                for (var i = 0; i < orgRelatitems.Count; i++)
                {
                    OrganizeModel tree = OrganizationRule.Find(orgRelatitems[i].OrgId);
                    if (tree.IfCorp == "Y" && tree.IsActive == "1" )//筛选组织
                    {
                        
                        if (!PHIDs1.Contains(tree.PhId))
                        {
                            tree.WeChatId = "false";//用于判断是否没有权限
                        }
                        GetChild(tree, PHIDs1);
                        
                        trees.Add(tree);
                    }
                }
            }
            return trees;

        }

        /// <summary>
        /// 得到子级
        /// </summary>
        /// <param name="orgTree"></param>
        /// <param name="OrgPhids"></param>
        /// <returns></returns>
        public List<OrganizeModel> GetChild(OrganizeModel orgTree, List<Int64> OrgPhids)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic)
               .Add(ORMRestrictions<String>.Eq("RelatId", "lg"))
               .Add(ORMRestrictions<Int64>.Eq("ParentOrgId", orgTree.PhId));
            IList<OrgRelatitem2Model> orgRelatitems = OrgRelatitem2Rule.Find(dic, new string[] { "RelId Asc" });
            if (orgRelatitems.Count > 0)
            {
                for(var i=0;i< orgRelatitems.Count; i++)
                {
                    OrganizeModel Orgdata = OrganizationRule.Find(orgRelatitems[i].OrgId);
                    if (Orgdata.IfCorp == "Y" && Orgdata.IsActive == "1")//筛选组织
                    {
                        
                        if (!OrgPhids.Contains(Orgdata.PhId))
                        {
                            Orgdata.WeChatId = "false";//用于判断是否没有权限
                        }
                        Orgdata.children = GetChild(Orgdata, OrgPhids);
                        
                       
                        if (orgTree.children == null)
                        {
                            orgTree.children = new List<OrganizeModel>();
                        }
                        orgTree.children.Add(Orgdata);
                    }
                }
            }
            return orgTree.children;
        }

        /// <summary>
        /// 取组织树（包括部门）
        /// </summary>
        /// <returns></returns>
        public List<OrganizeModel> GetALLOrgTree()
        {
            List<OrganizeModel> trees = new List<OrganizeModel>();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic)
               .Add(ORMRestrictions<String>.Eq("RelatId", "lg"))
               .Add(ORMRestrictions<Int64>.Eq("ParentOrgId", 0));
            IList<OrgRelatitem2Model> orgRelatitems = OrgRelatitem2Rule.Find(dic);
            if (orgRelatitems.Count > 0)
            {
                for (var i = 0; i < orgRelatitems.Count; i++)
                {
                    OrganizeModel tree = OrganizationRule.Find(orgRelatitems[i].OrgId);
                    
                    GetAllChild(tree);

                    trees.Add(tree);
                    
                }
            }
            return trees;
        }

        /// <summary>
        /// 得到子级(包括部门)(树)
        /// </summary>
        /// <param name="orgTree"></param>
        /// <returns></returns>
        public List<OrganizeModel> GetAllChild(OrganizeModel orgTree)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic)
               .Add(ORMRestrictions<String>.Eq("RelatId", "lg"))
               .Add(ORMRestrictions<Int64>.Eq("ParentOrgId", orgTree.PhId));
            IList<OrgRelatitem2Model> orgRelatitems = OrgRelatitem2Rule.Find(dic);
            if (orgRelatitems.Count > 0)
            {
                for (var i = 0; i < orgRelatitems.Count; i++)
                {
                    OrganizeModel Orgdata = OrganizationRule.Find(orgRelatitems[i].OrgId);
                    if (Orgdata.IfCorp == "Y" && Orgdata.IsActive == "1")//筛选组织
                    {
                        Orgdata.children = GetAllChild(Orgdata);
                        
                        if (orgTree.children == null)
                        {
                            orgTree.children = new List<OrganizeModel>();
                        }
                        orgTree.children.Add(Orgdata);
                    }
                }
            }

            Dictionary<string, object> dicDept = new Dictionary<string, object>();
            new CreateCriteria(dicDept)
               .Add(ORMRestrictions<Int64>.Eq("ParentOrgId", orgTree.PhId))
               .Add(ORMRestrictions<String>.Eq("IfCorp", "N"))
               .Add(ORMRestrictions<String>.Eq("IsActive", "1"));
            List<OrganizeModel> Depts = OrganizationRule.Find(dicDept).ToList();
            if (Depts.Count > 0)
            {
                if (orgTree.children == null)
                {
                    orgTree.children = new List<OrganizeModel>();
                }
                for(var i=0;i< Depts.Count; i++)
                {
                    Depts[i].OName = orgTree.OName + "-" + Depts[i].OName;//部门名称变为 组织-部门
                    orgTree.children.Add(Depts[i]);
                }
            }
            return orgTree.children;
        }


        /// <summary>
        /// 得到子级(包括部门)(不是树 是list)
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="OrgList"></param>
        /// <returns></returns>
        public List<OrganizeModel> GetAllChildList(long orgId,List<OrganizeModel> OrgList = null)
        {
            if (OrgList == null)
            {
                OrgList = new List<OrganizeModel>();
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic)
               .Add(ORMRestrictions<String>.Eq("RelatId", "lg"))
               .Add(ORMRestrictions<Int64>.Eq("ParentOrgId", orgId));
            IList<OrgRelatitem2Model> orgRelatitems = OrgRelatitem2Rule.Find(dic);
            if (orgRelatitems.Count > 0)
            {
                for (var i = 0; i < orgRelatitems.Count; i++)
                {
                    OrganizeModel Orgdata = OrganizationRule.Find(orgRelatitems[i].OrgId);
                    if (Orgdata.IfCorp == "Y" && Orgdata.IsActive == "1")//筛选组织
                    {
                        OrgList.Add(Orgdata);
                        GetAllChildList(Orgdata.PhId, OrgList);
                    }
                }
            }
            Dictionary<string, object> dicDept = new Dictionary<string, object>();
            new CreateCriteria(dicDept)
               .Add(ORMRestrictions<Int64>.Eq("ParentOrgId", orgId))
               .Add(ORMRestrictions<String>.Eq("IfCorp", "N"))
               .Add(ORMRestrictions<String>.Eq("IsActive", "1"));
            List<OrganizeModel> Depts = OrganizationRule.Find(dicDept).ToList();
            if (Depts.Count > 0)
            {
                for (var i = 0; i < Depts.Count; i++)
                {
                    OrgList.Add(Depts[i]);
                }
            }
            return OrgList;
        }
        
        #endregion
    }
}

