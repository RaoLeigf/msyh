#region Summary
/**************************************************************************************
    * 类 名 称：        CorrespondenceSettingsService
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        CorrespondenceSettingsService.cs
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
using SUP.Common.DataEntity;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Service;
using Enterprise3.Common.Base.Criterion;

using GQT3.QT.Service.Interface;
using GQT3.QT.Facade.Interface;
using GQT3.QT.Model.Domain;
using GQT3.QT.Model;
using GQT3.QT.Model.Enums;
using SUP.Common.Base;
using NG3;
using System.Data;
using GData3.Common.Utils;
using NG3.Data.Service;
using NG.KeepConn;
using NG3.Data;
using GQT3.QT.Model.Extra;

namespace GQT3.QT.Service
{
	/// <summary>
	/// CorrespondenceSettings服务组装处理类
	/// </summary>
    public partial class CorrespondenceSettingsService : EntServiceBase<CorrespondenceSettingsModel>, ICorrespondenceSettingsService
    {
		#region 类变量及属性
		/// <summary>
        /// CorrespondenceSettings业务外观处理对象
        /// </summary>
		ICorrespondenceSettingsFacade CorrespondenceSettingsFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as ICorrespondenceSettingsFacade;
            }
        }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IUserFacade UserFacade { get; set; }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
        private IOrganizationFacade OrganizationFacade { get; set; }

        /// <summary>
        /// UserOrgFacade业务外观处理对象
        /// </summary>
        private IUserOrgFacade UserOrgFacade { get; set; }

        private IQtBaseProjectFacade QtBaseProjectFacade { get; set; }

        private IQTMemoFacade QTMemoFacade { get; set; }

        private ICorrespondenceSettings2Facade CorrespondenceSettings2Facade { get; set; }
        #endregion

        #region 实现 ICorrespondenceSettingsService 业务添加的成员

        /// <summary>
        /// 取对应关系列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public PagedResult<User2Model> LoadWithPageYSBM(DataStoreParam dataStore) {
            
            Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
            new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("Status", "1"));
            //var result = UserFacade.LoadWithPage(dataStore.PageIndex, dataStore.PageSize, dicWhere2);
            var result = UserFacade.FacadeHelper.LoadWithPageInfinity("GQT.QT.GetAllUsers", dicWhere2);
            return result;
        }

        /// <summary>
        /// 取对应关系列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public PagedResult<OrganizeModel> LoadWithPageYSBM2(DataStoreParam dataStore,string userCode) {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            if (userCode != null && !"".Equals(userCode)) {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("UserNo", userCode));
            }
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("IfCorp", "N"))
                .Add(ORMRestrictions<string>.Eq("Dylx", "97"));
            var result = OrganizationFacade.FacadeHelper.LoadWithPageInfinity("GQT.QT.UserOrganizeList", dicWhere);
            return result;
        }

        /// <summary>
        /// 取对应关系列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public PagedResult<OrganizeModel> GetRelationYSBMLeftList(DataStoreParam dataStore, string userCode) {
            //Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            //if (userCode != null && !"".Equals(userCode)) {
            //    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dwdm", userCode));
            //}
            //new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "97"));
            //var result = OrganizationFacade.LoadWithPageInfinity("GQT.QT.GetRelationYSBMRightList", dicWhere);
            Dictionary<string, object> where1 = new Dictionary<string, object>();
            Dictionary<string, object> where2 = new Dictionary<string, object>();
            Dictionary<string, object> where3 = new Dictionary<string, object>();
            if (userCode != null && !"".Equals(userCode)) {
                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("UserNo", userCode));
                FindedResults<User2Model> finded = UserFacade.Find(dicWhere);
                User2Model model = finded.Data[0];
                new CreateCriteria(where1).Add(ORMRestrictions<Int64>.Eq("UserId", model.PhId));
            }
            new CreateCriteria(where1).Add(ORMRestrictions<EnumOrgInnerType>.Eq("InnerType", EnumOrgInnerType.InfoOrg));
            //查询操作员所对应的组织
            PagedResult<UserOrganize2Model> findedResults = UserOrgFacade.FacadeHelper.LoadWithPageInfinity("GQT.QT.GetOrganizeByUser", where1);
            List<UserOrganize2Model> organizeModels = findedResults.Results.ToList();
            List<Int64> ownerOrgPhid = new List<Int64>();
            for (int i = 0; i < organizeModels.Count; i++) {
                ownerOrgPhid.Add(organizeModels[i].OrgId);
            }
            //查询已经选入的部门
            new CreateCriteria(where2)
                .Add(ORMRestrictions<string>.Eq("Dylx", "97"))
                .Add(ORMRestrictions<string>.Eq("Dwdm", userCode));
            FindedResults<CorrespondenceSettingsModel> findedResult2 = base.Find(where2);
            List<CorrespondenceSettingsModel> correspondences = findedResult2.Data.ToList();
            List<string> existOrgCode = new List<string>();
            for (int i = 0; i < correspondences.Count; i++) {
                existOrgCode.Add(correspondences[i].Dydm);
            }
            new CreateCriteria(where3)
                .Add(ORMRestrictions<string>.Eq("IfCorp", "N"))
                .Add(ORMRestrictions<List<Int64>>.In("ParentOrgId", ownerOrgPhid));
                //.Add(ORMRestrictions<List<string>>.NotIn("OCode", existOrgCode));
            if (existOrgCode.Count > 0) {
                new CreateCriteria(where3).Add(ORMRestrictions<List<string>>.NotIn("OCode", existOrgCode));
            }
            var result = OrganizationFacade.FacadeHelper.LoadWithPageInfinity("GQT.QT.GetOrganize", where3);
            return result;
        }

        /// <summary>
        /// 取对应关系列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public PagedResult<OrganizeModel> GetRelationYSBMRightList(string userCode) {
            //Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            //if (userCode != null && !"".Equals(userCode)) {
            //    new CreateCriteria(dicWhere)
            //        .Add(ORMRestrictions<string>.Eq("UserNo", userCode))
            //        .Add(ORMRestrictions<string>.Eq("Dwdm", userCode));
            //}
            //new CreateCriteria(dicWhere)
            //    .Add(ORMRestrictions<string>.Eq("IfCorp", "N"))
            //    .Add(ORMRestrictions<string>.Eq("Dylx", "97"));
            //var result = OrganizationFacade.LoadWithPageInfinity("GQT.QT.GetRelationYSBMLeftList", dicWhere);
            Dictionary<string, object> where1 = new Dictionary<string, object>();
            Dictionary<string, object> where2 = new Dictionary<string, object>();
            new CreateCriteria(where1)
                .Add(ORMRestrictions<string>.Eq("Dylx", "97"))
                .Add(ORMRestrictions<string>.Eq("Dwdm", userCode));
            FindedResults<CorrespondenceSettingsModel> findedResult2 = base.Find(where1);
            List<CorrespondenceSettingsModel> correspondences = findedResult2.Data.ToList();
            List<string> existOrgCode = new List<string>();
            for (int i = 0; i < correspondences.Count; i++)
            {
                existOrgCode.Add(correspondences[i].Dydm);
            }
            if (existOrgCode.Count > 0)
            {
                new CreateCriteria(where2)
                    .Add(ORMRestrictions<string>.Eq("IfCorp", "N"))
                    .Add(ORMRestrictions<List<string>>.In("OCode", existOrgCode));
            }
            else {
                return new PagedResult<OrganizeModel>();
            }
            var result = OrganizationFacade.FacadeHelper.LoadWithPageInfinity("GQT.QT.GetOrganize", where2);
            return result;
        }

        /// <summary>
        /// 设置操作员对应预算部门关系
        /// </summary>
        /// <returns>返回Json串</returns>
        public List<CorrespondenceSettingsModel> UpdataRelationYSBM(List<OrganizeModel> models,string UserNo) {
            CommonResult<CorrespondenceSettingsModel> result = new CommonResult<CorrespondenceSettingsModel>();
            //Dictionary<string, object> dicWhere1 = new Dictionary<string, object>();

            //if (UserNo != null && !"".Equals(UserNo))
            //{
            //    new CreateCriteria(dicWhere1)
            //        .Add(ORMRestrictions<string>.Eq("UserNo", UserNo))
            //        .Add(ORMRestrictions<string>.Eq("Dwdm", UserNo));
            //}
            //new CreateCriteria(dicWhere1)
            //    .Add(ORMRestrictions<string>.Eq("IfCorp", "N"))
            //    .Add(ORMRestrictions<string>.Eq("Dylx", "97"));
            //var existResult = OrganizationFacade.LoadWithPageInfinity("GQT.QT.GetRelationYSBMLeftList", dicWhere1);
            Dictionary<string, object> where1 = new Dictionary<string, object>();
            Dictionary<string, object> where2 = new Dictionary<string, object>();
            var existResult = new PagedResult<OrganizeModel>();
            List<OrganizeModel> organizeModels = null;
            new CreateCriteria(where1)
                .Add(ORMRestrictions<string>.Eq("Dylx", "97"))
                .Add(ORMRestrictions<string>.Eq("Dwdm", UserNo));
            FindedResults<CorrespondenceSettingsModel> findedResult2 = base.Find(where1);
            List<CorrespondenceSettingsModel> correspondences = findedResult2.Data.ToList();
            List<string> existOrgCode = new List<string>();
            for (int i = 0; i < correspondences.Count; i++)
            {
                existOrgCode.Add(correspondences[i].Dydm);
            }
            if (existOrgCode.Count > 0)
            {
                new CreateCriteria(where2)
                    .Add(ORMRestrictions<string>.Eq("IfCorp", "N"))
                    .Add(ORMRestrictions<List<string>>.In("OCode", existOrgCode));
                existResult = OrganizationFacade.FacadeHelper.LoadWithPageInfinity("GQT.QT.GetOrganize", where2);
                //existResult = OrganizationFacade.LoadWithPage(1, 200, where2);
            }

            //var existResult = OrganizationFacade.LoadWithPageInfinity("GQT.QT.GetOrganize", where2);
            if (existResult.Results == null)
            {
                organizeModels = new List<OrganizeModel>();
            }
            else {
                organizeModels = existResult.Results.ToList();
            }

            for (int i = 0; i < organizeModels.Count; i++) {
                OrganizeModel organize = organizeModels[i];
                if (!models.Exists(t => t.PhId == organize.PhId))
                {
                    try {
                        Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
                        new CreateCriteria(dicWhere2)
                            .Add(ORMRestrictions<string>.Eq("Dwdm",UserNo))
                            .Add(ORMRestrictions<string>.Eq("Dydm", organize.OCode))
                            .Add(ORMRestrictions<string>.Eq("Dylx", "97"));
                        DeletedResult deletedResult = base.Delete(dicWhere2);
                    }
                    catch (Exception e) {
                        result.Status = ResponseStatus.Error;
                        result.Msg = "设置失败，请重新设置！";
                    }
                }
            }
            List<CorrespondenceSettingsModel> correspondencesList = new List<CorrespondenceSettingsModel>();
            for (int i = 0; i < models.Count; i++) {
                OrganizeModel organize = models[i];
                if (!organizeModels.Exists(t => t.PhId == organize.PhId)) {
                    try
                    {
                        CorrespondenceSettingsModel correspondence = new CorrespondenceSettingsModel();
                        correspondence.PersistentState = SUP.Common.Base.PersistentState.Added;
                        correspondence.Dwdm = UserNo;
                        correspondence.Dydm = organize.OCode;
                        correspondence.Dylx = "97";

                        Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
                        new CreateCriteria(dicWhere2).Add(ORMRestrictions<Int64>.Eq("PhId",organize.ParentOrgId));
                        FindedResults<OrganizeModel> findedResults = OrganizationFacade.Find(dicWhere2);
                        OrganizeModel findOrganize = findedResults.Data[0];
                        correspondence.DefStr1 = findOrganize.OCode;
                        //SavedResult<Int64> savedResult = base.Save<Int64>(correspondence);
                        correspondencesList.Add(correspondence);
                    }
                    catch (Exception e) {
                        result.Status = ResponseStatus.Error;
                        result.Msg = "设置失败，请重新设置！";
                    }
                }
            }
            return correspondencesList;
        }

        /// <summary>
        /// 取对应关系列表数据  操作员对应组织部门
        /// </summary>
        /// <returns>返回Json串</returns>
        public PagedResult<CorrespondenceSettingsModel> LoadWithPageUser_Org(DataStoreParam dataStore, string userCode)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            if (userCode != null && !"".Equals(userCode))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("UserNo", userCode));
            }
            new CreateCriteria(dicWhere)
                //.Add(ORMRestrictions<string>.Eq("IfCorp", "N"))
                .Add(ORMRestrictions<string>.Eq("Dylx", "08"));
            //var result = base.LoadWithPageByDynamicInfinity<User_OrgDefaultModel>("GQT.QT.User_DefaultOrgList", dicWhere);
            var result = base.ServiceHelper.LoadWithPageInfinity("GQT.QT.DYGX_UserList", dicWhere);
            return result;
        }

        /// <summary>
        /// 取所有org（不包括部门）
        /// </summary>
        /// <returns>返回Json串</returns>
        public PagedResult<OrganizeModel> LoadWithPageOrg(DataStoreParam dataStore)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("IfCorp", "Y"));
            var result = OrganizationFacade.FacadeHelper.LoadWithPageInfinity("GQT.QT.GetAllOrg", dicWhere);
            return result;
        }

        /// <summary>
        /// 取所有org（包括部门）
        /// </summary>
        /// <returns>返回Json串</returns>
        public PagedResult<OrganizeModel> LoadWithPageAllOrg(DataStoreParam dataStore)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            var result = OrganizationFacade.FacadeHelper.LoadWithPageInfinity("GQT.QT.GetAllOrg", dicWhere);
            return result;
        }

        /// <summary>
        /// 根据组织id取部门
        /// </summary>
        /// <returns>返回Json串</returns>
        public PagedResult<OrganizeModel> LoadWithPageBM(DataStoreParam dataStore, string OrgId)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("IfCorp", "N"))
                .Add(ORMRestrictions<string>.Eq("IsActive", "1"));
            if (OrgId != null && !"".Equals(OrgId))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("ParentOrgId", OrgId));
            }
            var result = OrganizationFacade.FacadeHelper.LoadWithPageInfinity("GQT.QT.GetAllOrg", dicWhere);
            return result;
        }


        /// <summary>
        /// 资金来源关系的改变
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <param name="OrgPhId"></param>
        /// <param name="mydelete"></param>
        /// <param name="myinsert"></param>
        /// <returns></returns>
        public CommonResult<CorrespondenceSettingsModel> UpdateZJLY(string OrgCode, string OrgPhId,List<CorrespondenceSettingsModel> mydelete, List<SourceOfFundsModel> myinsert)
        {
            CommonResult<CorrespondenceSettingsModel> result = new CommonResult<CorrespondenceSettingsModel>();
            if (mydelete != null && mydelete.Count>0)
            {
                for (int i = 0; i < mydelete.Count; i++)
                {
                    CorrespondenceSettingsModel delete = mydelete[i];
                    var Deleteid = delete.PhId;
                    try
                    {
                        base.Delete<System.Int64>(Deleteid);
                    }
                    catch (Exception e)
                    {
                        result.Status = ResponseStatus.Error;
                        result.Msg = "设置失败，请重新设置！";
                    }
                }
            }
            if (myinsert != null && myinsert.Count>0)
            {
                for (int i = 0; i < myinsert.Count; i++)
                {
                    SourceOfFundsModel insert = myinsert[i];
                    var Dwdm = OrgPhId;
                    var Dydm = insert.DM;
                    var Dylx = "96";
                    var DefStr1 = OrgCode;
                    try
                    {
                        CorrespondenceSettingsModel correspondence = new CorrespondenceSettingsModel();
                        correspondence.PersistentState = SUP.Common.Base.PersistentState.Added;
                        correspondence.Dwdm = Dwdm;
                        correspondence.Dydm = Dydm;
                        correspondence.Dylx = Dylx;
                        correspondence.DefStr1 = DefStr1;
                        SavedResult<Int64> savedResult2 = base.Save<Int64>(correspondence,"");
                    }
                    catch (Exception e)
                    {
                        result.Status = ResponseStatus.Error;
                        result.Msg = "设置失败，请重新设置！";
                    }

                }
            }
            return result;
        }

        /// <summary>
        /// 预算科目关系的改变
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <param name="OrgPhId"></param>
        /// <param name="OrgName"></param>
        /// <param name="mydelete"></param>
        /// <param name="myinsert"></param>
        /// <returns></returns>
        public CommonResult<CorrespondenceSettingsModel> UpdateYSKM(string OrgCode, string OrgPhId, string OrgName, List<CorrespondenceSettingsModel> mydelete, List<BudgetAccountsModel> myinsert)
        {
            CommonResult<CorrespondenceSettingsModel> result = new CommonResult<CorrespondenceSettingsModel>();
            if (mydelete != null && mydelete.Count>0)
            {
                for (int i = 0; i < mydelete.Count; i++)
                {
                    CorrespondenceSettingsModel delete = mydelete[i];
                    var Deleteid = delete.PhId;
                    Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<string>.Eq("FKmdm", delete.Dydm))
                        .Add(ORMRestrictions<Int64>.Eq("Fphid", long.Parse(OrgPhId)));
                    if (QtBaseProjectFacade.Find(dicWhere).Data.Count > 1)
                    {
                        result.Status = ResponseStatus.Error;
                        result.Msg = "科目代码为" + delete.Dydm + "的科目已经存在子科目，无法删除！";
                        return result;
                    }
                    try
                    {
                        base.Delete<System.Int64>(Deleteid);
                        QtBaseProjectFacade.Delete<System.Int64>(QtBaseProjectFacade.Find(dicWhere).Data[0].PhId);
                    }
                    catch (Exception e)
                    {
                        result.Status = ResponseStatus.Error;
                        result.Msg = "设置失败，请重新设置！";
                    }
                }
            }
            if (myinsert != null && myinsert.Count>0)
            {
                for (int i = 0; i < myinsert.Count; i++)
                {
                    BudgetAccountsModel insert = myinsert[i];
                    var Dwdm = OrgPhId;
                    var Dydm = insert.KMDM;
                    var Dylx = "02";
                    var DefStr1 = OrgCode;
                    try
                    {
                        CorrespondenceSettingsModel correspondence = new CorrespondenceSettingsModel();
                        correspondence.PersistentState = SUP.Common.Base.PersistentState.Added;
                        correspondence.Dwdm = Dwdm;
                        correspondence.Dydm = Dydm;
                        correspondence.Dylx = Dylx;
                        correspondence.DefStr1 = DefStr1;
                        SavedResult<Int64> savedResult2 = base.Save<Int64>(correspondence,"");
                        QtBaseProjectModel QTModel = new QtBaseProjectModel();
                        QTModel.Fphid = long.Parse(Dwdm);
                        QTModel.FKmdm = Dydm;
                        QTModel.FKMLB = insert.KMLB;
                        QTModel.Fkmmc = insert.KMMC;
                        QTModel.FDwdm = OrgCode;
                        QTModel.FDwmc = OrgName;
                        QTModel.PersistentState= SUP.Common.Base.PersistentState.Added;
                        QtBaseProjectFacade.Save<Int64>(QTModel);

                    }
                    catch (Exception e)
                    {
                        result.Status = ResponseStatus.Error;
                        result.Msg = "设置失败，请重新设置！";
                    }

                }
            }
            return result;
        }

        /// <summary>
        /// 预算库关系的改变
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <param name="OrgPhId"></param>
        /// <param name="mydelete"></param>
        /// <param name="myinsert"></param>
        /// <returns></returns>
        public CommonResult<CorrespondenceSettingsModel> UpdateYSK(string OrgCode, string OrgPhId, List<CorrespondenceSettingsModel> mydelete, List<ProjLibProjModel> myinsert)
        {
            CommonResult<CorrespondenceSettingsModel> result = new CommonResult<CorrespondenceSettingsModel>();
            if (mydelete != null && mydelete.Count>0)
            {
                for (int i = 0; i < mydelete.Count; i++)
                {
                    CorrespondenceSettingsModel delete = mydelete[i];
                    var Deleteid = delete.PhId;
                    try
                    {
                        base.Delete<System.Int64>(Deleteid);
                    }
                    catch (Exception e)
                    {
                        result.Status = ResponseStatus.Error;
                        result.Msg = "设置失败，请重新设置！";
                    }
                }
            }
            if (myinsert != null && myinsert.Count>0)
            {
                for (int i = 0; i < myinsert.Count; i++)
                {
                    ProjLibProjModel insert = myinsert[i];
                    var Dwdm = OrgPhId;
                    var Dydm = insert.DM;
                    var Dylx = "03";
                    var DefStr1 = OrgCode;
                    try
                    {
                        CorrespondenceSettingsModel correspondence = new CorrespondenceSettingsModel();
                        correspondence.PersistentState = SUP.Common.Base.PersistentState.Added;
                        correspondence.Dwdm = Dwdm;
                        correspondence.Dydm = Dydm;
                        correspondence.Dylx = Dylx;
                        correspondence.DefStr1 = DefStr1;
                        SavedResult<Int64> savedResult2 = base.Save<Int64>(correspondence,"");
                    }
                    catch (Exception e)
                    {
                        result.Status = ResponseStatus.Error;
                        result.Msg = "设置失败，请重新设置！";
                    }

                }
            }
            return result;
        }

        /// <summary>
        /// 操作员默认组织设置
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="username"></param>
        /// <param name="mydelete"></param>
        /// <param name="myinsert"></param>
        /// <returns></returns>
        public CommonResult<CorrespondenceSettingsModel> UpdateDefaultOrg(string usercode, string username, List<CorrespondenceSettingsModel> mydelete, List<OrganizeModel> myinsert)
        {
            CommonResult<CorrespondenceSettingsModel> result = new CommonResult<CorrespondenceSettingsModel>();
            if (mydelete != null && mydelete.Count > 0)
            {
                for (int i = 0; i < mydelete.Count; i++)
                {
                    CorrespondenceSettingsModel delete = mydelete[i];
                    var Deleteid = delete.PhId;
                    try
                    {
                        base.Delete<System.Int64>(Deleteid);
                    }
                    catch (Exception e)
                    {
                        result.Status = ResponseStatus.Error;
                        result.Msg = "设置失败，请重新设置！";
                    }
                }
            }
            if (myinsert != null && myinsert.Count > 0)
            {
                for (int i = 0; i < myinsert.Count; i++)
                {
                    OrganizeModel insert = myinsert[i];
                    var Dwdm = usercode;
                    var Dydm = insert.OCode;
                    var Dylx = "08";
                    var DefStr1 = username;
                    var DefStr2 = insert.PhId;
                    var DefStr3 = insert.OName;
                    var DefInt1string = insert.ForeignFn;
                    var DefInt1 = 0;
                    if (DefInt1string == "Y")
                    {
                        DefInt1 = 1;
                    }

                    try
                    {
                        CorrespondenceSettingsModel correspondence = new CorrespondenceSettingsModel();
                        correspondence.PersistentState = SUP.Common.Base.PersistentState.Added;
                        correspondence.Dwdm = Dwdm;
                        correspondence.Dydm = Dydm;
                        correspondence.Dylx = Dylx;
                        correspondence.DefStr1 = DefStr1;
                        correspondence.DefStr2 = DefStr2.ToString();
                        correspondence.DefStr3 = DefStr3;
                        correspondence.DefInt1 = DefInt1;
                        SavedResult<Int64> savedResult2 = base.Save<Int64>(correspondence,"");
                    }
                    catch (Exception e)
                    {
                        result.Status = ResponseStatus.Error;
                        result.Msg = "设置失败，请重新设置！";
                    }

                }
            }
            return result;
        }

        /// <summary>
        /// 取操作员能操作的org
        /// </summary>
        /// <returns>返回Json串</returns>
        public PagedResult<OrganizeModel> LoadWithPageOrgByUser(DataStoreParam dataStore,string userid)
        {
            var OrgPhIdlist = new List<Int64>();
            var UserOrgresult = new PagedResult<UserOrganize2Model>();
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("UserId", userid))
                .Add(ORMRestrictions<EnumOrgInnerType>.Eq("InnerType", EnumOrgInnerType.InfoOrg));

            UserOrgresult = UserOrgFacade.FacadeHelper.LoadWithPageInfinity("GQT.QT.GetOrganizeByUser", dicWhere);
            for(var i=0;i< UserOrgresult.Results.Count; i++)
            {
                UserOrganize2Model userorg = UserOrgresult.Results[i];
                OrgPhIdlist.Add(userorg.OrgId);
            }
            

            var Orgresult = new PagedResult<OrganizeModel>();
            Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
            if (OrgPhIdlist.Count > 0)
            {
                new CreateCriteria(dicWhere2)
                    .Add(ORMRestrictions<List<Int64>>.In("PhId", OrgPhIdlist))
                    .Add(ORMRestrictions<string>.Eq("IfCorp", "Y"))
                    .Add(ORMRestrictions<string>.Eq("IsActive", "1"));
            }
            else
            {
                return new PagedResult<OrganizeModel>();
            }
            Orgresult = OrganizationFacade.FacadeHelper.LoadWithPageInfinity("GQT.QT.GetAllOrg", dicWhere2);
            return Orgresult;
        }

        /// <summary>
        /// 删除单位对应预算科目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CommonResult DeleteQtbase(long id)
        {
            CommonResult result = new CommonResult();
            CorrespondenceSettingsModel correspondenceSettingsModel = base.Find(id).Data;
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("FKmdm", correspondenceSettingsModel.Dydm))
                .Add(ORMRestrictions<Int64>.Eq("Fphid", long.Parse(correspondenceSettingsModel.Dwdm)));
            if (QtBaseProjectFacade.Find(dicWhere).Data.Count > 1)
            {
                result.Status = ResponseStatus.Error;
                result.Msg = "科目代码为" + correspondenceSettingsModel.Dydm + "的科目已经存在子科目，无法删除！";
                return result;
            }
            try
            {
                base.Delete<System.Int64>(id);
                QtBaseProjectFacade.Delete<System.Int64>(QtBaseProjectFacade.Find(dicWhere).Data[0].PhId);
            }
            catch (Exception e)
            {
                result.Status = ResponseStatus.Error;
                result.Msg = "设置失败，请重新设置！";
            }
            return result;
        }

        /// <summary>
        /// 根据操作员和申报单位取预算部门
        /// </summary>
        /// <param name="Usercoode"></param>
        /// <param name="Unit"></param>
        /// <returns></returns>
        public IList<OrganizeModel> GetDept(string Usercoode, string Unit)
        {
            return OrganizationFacade.GetDept(Usercoode, Unit);
        }

        /// <summary>
        /// 组织树
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<OrganizeModel> GetOrgTree(long UserId)
        {
            return OrganizationFacade.GetOrgTree(UserId);
        }

        /// <summary>
        /// 得到登录信息
        /// </summary>
        /// <returns></returns>
        public string GetLogin()
        {
            OrganizeModel organize = OrganizationFacade.Find(AppInfoBase.OrgID).Data;
            //取年度
            var Year = "";
            var dicWhereYear = new Dictionary<string, object>();

            new CreateCriteria(dicWhereYear).Add(ORMRestrictions<string>.Eq("Dylx", "YEAR"))
                .Add(ORMRestrictions<string>.Eq("Dwdm", AppInfoBase.UserID.ToString()));
            var Model2 = CorrespondenceSettings2Facade.Find(dicWhereYear).Data;
            if (Model2.Count > 0)
            {
                Year = Model2[0].Dydm;
            }
            //判断登录的操作员密码是否为空
            var PsdIsEmpty = false;
            var User = UserFacade.Find(AppInfoBase.UserID).Data;//User2Model
            //if(User.)
            var selectsql= @"SELECT pwd FROM FG3_USER WHERE PHID = {0}";
            DataTable dataTable = DbHelper.GetDataTable(string.Format(selectsql, AppInfoBase.UserID));
            if (dataTable.Rows.Count > 0&& string.IsNullOrEmpty(dataTable.Rows[0][0].ToString()))
            {
                PsdIsEmpty = true;
            }
            //取权限
            List<string> suiteList = new List<string> { "GJS", "NYS", "NYK", "NJX", "NBF", "GYK" };//"GYS",
            Dictionary<string, string> MenuButton = new Dictionary<string, string>();
            for (var i = 0; i < suiteList.Count; i++)
            {
                string nodeid = "root";
                string suite = suiteList[i];
                bool rightFlag = true;
                bool lazyLoadFlag = false;
                string userType = NG3.AppInfoBase.UserType;
                SUP.Common.Base.ProductInfo prdInfo = new SUP.Common.Base.ProductInfo();
                DataTable menulist = QTMemoFacade.GetLoadMenu(prdInfo.ProductCode + prdInfo.Series, suite, false, userType, AppInfoBase.OrgID, AppInfoBase.UserID, nodeid, rightFlag, lazyLoadFlag, "");
                if (menulist.Rows.Count > 0)
                {
                    for (var j = 0; j < menulist.Rows.Count; j++)
                    {
                        string rightname = menulist.Rows[j]["url"].ToString();
                        if (!string.IsNullOrEmpty(rightname) && !rightname.Contains("/") && !MenuButton.Keys.Contains(rightname))
                        {
                            MenuButton.Add(rightname, "True");
                            var buttonlist = QTMemoFacade.GetFormRights(AppInfoBase.UserID, AppInfoBase.OrgID, NG3.AppInfoBase.UserType, rightname);
                            if (buttonlist.Count > 0)
                            {
                                foreach (string key in buttonlist.Keys)
                                {
                                    if (!MenuButton.Keys.Contains(key))
                                    {
                                        MenuButton.Add(key, buttonlist[key].ToString());
                                    }
                                }
                            }
                        }
                    }
                }

            }

            //取物料
            List<string> wlList = new List<string> { "GJS","NYS","NYK","NJX","NBF","GYK" };
            List<string> wlCodeList = new List<string> { "12130","12131", "12132", "12133", "12134", "12135" };
            Dictionary<string, bool> wlRight = new Dictionary<string, bool>();
            for (var j=0;j< wlCodeList.Count; j++)
            {
                string validErrMsg = "";
                var right=NG.KeepConn.NGModuleRight.Instance.HasRight(AppInfoBase.PubConnectString, AppInfoBase.UserConnectString, AppInfoBase.UCode, AppInfoBase.OCode, wlCodeList[j], ref validErrMsg);
                wlRight.Add(wlList[j], right);
            }
            var data = new
            {
                DbName = AppInfoBase.DbName,
                OrgId = AppInfoBase.OrgID,
                UserId = AppInfoBase.UserID,
                UserName = AppInfoBase.UserName,
                OrgName = AppInfoBase.OrgName,
                OrgCode = organize.OCode,
                Year= Year,
                MenuButton= MenuButton,
                PsdIsEmpty= PsdIsEmpty,
                wlRight= wlRight
            };
            return DataConverterHelper.SerializeObject(data);
        }

        /// <summary>
        /// 根据单位CODE取部门
        /// </summary>
        /// <param name="OrgID"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public IList<OrganizeModel> GetDeptByUnit(long OrgID, long UserId)
        {
            OrganizeModel organize = OrganizationFacade.Find(OrgID).Data;
            User2Model User = UserFacade.Find(UserId).Data;

            //取操作员对应预算部门
            Dictionary<string, object> dicdygx = new Dictionary<string, object>();
            new CreateCriteria(dicdygx)
                .Add(ORMRestrictions<string>.Eq("Dwdm", User.UserNo))
                .Add(ORMRestrictions<string>.Eq("Dylx", "97"))
                .Add(ORMRestrictions<string>.Eq("DefStr1", organize.OCode));
            var dygx = CorrespondenceSettingsFacade.Find(dicdygx).Data;
            List<string> OrgCodeList = new List<string>();
            foreach(CorrespondenceSettingsModel corr in dygx)
            {
                OrgCodeList.Add(corr.Dydm);
            }

            IList<OrganizeModel> OrgDepts = new List<OrganizeModel>();
            Dictionary<string, object> dicUnit = new Dictionary<string, object>();
            new CreateCriteria(dicUnit)
                .Add(ORMRestrictions<string>.Eq("OCode", organize.OCode));
            IList<OrganizeModel> OrgUnits = OrganizationFacade.Find(dicUnit).Data;
            if (OrgUnits.Count > 0)
            {
                OrganizeModel OrgUnit = OrgUnits[0];
                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<Int64>.Eq("ParentOrgId", OrgUnit.PhId))
                     .Add(ORMRestrictions<String>.Eq("IfCorp", "N"))
                     .Add(ORMRestrictions<String>.Eq("IsActive", "1"))
                     .Add(ORMRestrictions<List<string>>.In("OCode", OrgCodeList));
                OrgDepts = OrganizationFacade.Find(dicWhere,new string[] { "OCode ASC" }).Data;
            }

            return OrgDepts;
        }

        /// <summary>
        /// 完整组织树（没有权限，包括部门）
        /// </summary>
        /// <returns></returns>
        public List<OrganizeModel> GetALLOrgTree()
        {
            return OrganizationFacade.GetALLOrgTree();
        }


        /// <summary>
        /// 根据组织或者部门获取操作员
        /// </summary>
        /// <param name="Org"></param>
        /// <param name="queryStr"></param>
        /// <returns></returns>
        public FindedResults<CorrespondenceSettingsModel> getUserByOrg(string Org, string queryStr)
        {
            //SELECT* FROM Z_QTDYGX WHERE DYLX = '08' AND DEF_INT1 = 1 AND dydm = '101'
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic)
                   .Add(ORMRestrictions<string>.Eq("Dylx", "08"))
                   .Add(ORMRestrictions<Int32>.Eq("DefInt1", 1));


            if (!string.IsNullOrEmpty(Org))
            {
                if (Org.Contains("."))
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<String>.Eq("DefStr3", Org));
                }
                else
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<String>.Eq("Dydm", Org));
                }
            }
            if (!string.IsNullOrEmpty(queryStr))
            {
                Dictionary<string, object> where1 = new Dictionary<string, object>();
                Dictionary<string, object> where2 = new Dictionary<string, object>();
                new CreateCriteria(where1)
                           .Add(ORMRestrictions<String>.Like("Dwdm", queryStr));
                new CreateCriteria(where2)
                          .Add(ORMRestrictions<String>.Like("DefStr1", queryStr));
                new CreateCriteria(dic)
                    .Add(ORMRestrictions.Or(where1, where2));

            }
            /*var result = CorrespondenceSettingsFacade.FacadeHelper.LoadWithPage(pageIndex, pageSize, dic);//操作员编码：Dwdm;操作员姓名：DefStr1;部门代码：DefStr3;组织代码：Dydm
            if (result.Results.Count > 0)
            {
                foreach(CorrespondenceSettingsModel data in result.Results)
                {
                    if (!string.IsNullOrEmpty(data.Dydm))
                    {
                        data.DefStr4 = OrganizationFacade.FindMcByDm(data.Dydm);//DefStr4:组织名称
                    }
                    if (!string.IsNullOrEmpty(data.DefStr3))
                    {
                        data.DefStr5 = OrganizationFacade.FindMcByDm(data.DefStr3);//DefStr5:部门名称
                    }
                    var dicUser = new Dictionary<string, object>();//DefStr6 操作员phid
                    new CreateCriteria(dicUser)
                           .Add(ORMRestrictions<string>.Eq("UserNo", data.Dwdm));
                    IList<User2Model> users = UserFacade.Find(dicUser).Data;
                    if (users.Count > 0)
                    {
                        data.DefStr6 = users[0].PhId.ToString();
                    }
                }
            }*/
            var result = CorrespondenceSettingsFacade.Find(dic, new string[]{ "Dwdm Asc" });
            if (result.Data.Count > 0)
            {
                Dictionary<string, object> dicall = new Dictionary<string, object>();
                new CreateCriteria(dicall)
                          .Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                IList<OrganizeModel> OrgList = OrganizationFacade.Find(dicall).Data;//所有组织
                IList<User2Model> UserList = UserFacade.Find(dicall).Data;//所有操作员
                foreach (CorrespondenceSettingsModel data in result.Data)
                {
                    if (!string.IsNullOrEmpty(data.Dydm))
                    {
                        var org1 = OrgList.ToList().Find(t => t.OCode == data.Dydm);
                        if (org1 != null)
                        {
                            data.DefStr4 = org1.OName;
                        }
                        //data.DefStr4 = OrganizationFacade.FindMcByDm(data.Dydm);//DefStr4:组织名称
                    }
                    if (!string.IsNullOrEmpty(data.DefStr3))
                    {
                        var org2 = OrgList.ToList().Find(t => t.OCode == data.DefStr3);
                        if (org2!=null)
                        {
                            data.DefStr5 = org2.OName;
                        }
                        //data.DefStr5 = OrganizationFacade.FindMcByDm(data.DefStr3);//DefStr5:部门名称
                    }
                    if (!string.IsNullOrEmpty(data.Dwdm))
                    {
                        var user1 = UserList.ToList().Find(t => t.UserNo == data.Dwdm);
                        if (user1!=null)
                        {
                            data.DefStr6 = user1.PhId.ToString();//DefStr6 操作员phid
                        }
                    }
                }
            }
            return result;

        }

        /// <summary>
        /// 得到子级(包括部门)(不是树 是list)
        /// </summary>
        /// <returns></returns>
        public List<OrganizeModel> GetAllChildList(long OrgId)
        {
            return OrganizationFacade.GetAllChildList(OrgId);
        }

        /// <summary>
        /// 得到子级(包括部门)(树)
        /// </summary>
        /// <returns></returns>
        public List<OrganizeModel> GetAllChildTree(long OrgId)
        {
            if (OrgId != 0)
            {
                List<OrganizeModel> organizes = new List<OrganizeModel>();
                OrganizeModel organize = OrganizationFacade.Find(OrgId).Data;
                OrganizationFacade.GetAllChild(organize);
                organizes.Add(organize);
                return organizes;
            }
            else
            {
                return OrganizationFacade.GetALLOrgTree();
            }
        }

        /// <summary>
        /// 得到完整登录信息（组织用户）
        /// </summary>
        /// <returns></returns>
        public string GetLogininfo(long OrgId, long UserId)
        {
            if (OrgId == 0)
            {
                return DCHelper.ErrorMessage("组织id为空！");
            }

            if (UserId == 0)
            {
                return DCHelper.ErrorMessage("用户id为空！");
            }
            //民生银行权限全在GXM
            List<string> suiteList = new List<string> { "GXM" };//{  "GJS", "NYS", "NYK", "NJX", "NBF", "GYK" };//"GYS",
            Dictionary<string, string> MenuButton = new Dictionary<string, string>();
            OrganizeModel Org = OrganizationFacade.Find(OrgId).Data;
            User2Model User = UserFacade.Find(UserId).Data;
            try
            {
                for (var i = 0; i < suiteList.Count; i++)
                {
                    string nodeid = "root";
                    string suite = suiteList[i];
                    bool rightFlag = true;
                    bool lazyLoadFlag = false;
                    string userType = NG3.AppInfoBase.UserType;
                    SUP.Common.Base.ProductInfo prdInfo = new SUP.Common.Base.ProductInfo();
                    DataTable menulist = QTMemoFacade.GetLoadMenu(prdInfo.ProductCode + prdInfo.Series, suite, false, userType, OrgId, UserId, nodeid, rightFlag, lazyLoadFlag, "");
                    if (menulist.Rows.Count > 0)
                    {
                        for (var j = 0; j < menulist.Rows.Count; j++)
                        {
                            string rightname = menulist.Rows[j]["url"].ToString();
                            if (!string.IsNullOrEmpty(rightname) && !rightname.Contains("/") && !MenuButton.Keys.Contains(rightname))
                            {
                                MenuButton.Add(rightname, "True");
                                var buttonlist = QTMemoFacade.GetFormRights(UserId, OrgId, NG3.AppInfoBase.UserType, rightname);
                                if (buttonlist.Count > 0)
                                {
                                    foreach (string key in buttonlist.Keys)
                                    {
                                        if (!MenuButton.Keys.Contains(key))
                                        {
                                            MenuButton.Add(key, buttonlist[key].ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }

            //return "{\"totalRows\":" + menulist.Rows.Count + ",\"Record\":" + JsonConvert.SerializeObject(menulist) + "}";

            //取年度
            var Year = "";
            var dicWhereYear = new Dictionary<string, object>();

            new CreateCriteria(dicWhereYear).Add(ORMRestrictions<string>.Eq("Dylx", "YEAR"))
                .Add(ORMRestrictions<string>.Eq("Dwdm", AppInfoBase.UserID.ToString()));
            var Model2 = CorrespondenceSettings2Facade.Find(dicWhereYear).Data;
            if (Model2.Count > 0)
            {
                Year = Model2[0].Dydm;
            }

            //取物料
            //List<string> wlList = new List<string> { "GJS", "NYS", "NYK", "NJX", "NBF", "GYK" };
            //List<string> wlCodeList = new List<string> { "12130", "12131", "12132", "12133", "12134", "12135" };
            Dictionary<string, bool> wlRight = new Dictionary<string, bool>();
            /*try
            {
                for (var j = 0; j < wlCodeList.Count; j++)
                {
                    string validErrMsg = "";
                    var right = NG.KeepConn.NGModuleRight.Instance.HasRight(AppInfoBase.PubConnectString, AppInfoBase.UserConnectString, AppInfoBase.UCode, Org.OCode, wlCodeList[j], ref validErrMsg);
                    wlRight.Add(wlList[j], right);
                }
            }
            catch (Exception ex)
            {

            }*/

            var data = new
            {
                Org = Org,
                User = User,
                MenuButton = MenuButton,
                appinfo = new
                {
                    /*logid = "9999",
                    username = "省总管理员",
                    ocode = "101",
                    userID = "488181024000001",
                    orgID = "488181024000002",
                    dbServer = "MTAuMC4xMy4xNjg6MTUyMS9uM25q",
                    uCode = "0006"*/
                    logid = User.UserNo,
                    username = User.UserName,
                    ocode = Org.OCode,
                    userID = AppInfoBase.UserID,
                    orgID = AppInfoBase.OrgID,
                    dbServer = Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(AppInfoBase.DbServerName)),
                    uCode = AppInfoBase.UCode
                },
                Year = Year,
                wlRight = wlRight
            };
            return DataConverterHelper.SerializeObject(data);
        }

        /// <summary>
        /// 得到组织
        /// </summary>
        /// <returns></returns>
        public OrganizeModel GetOrg(string OrgCode)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                 .Add(ORMRestrictions<string>.Eq("OCode", OrgCode));
            IList<OrganizeModel> orgs = OrganizationFacade.Find(dicWhere).Data;
            if (orgs.Count > 0)
            {
                return orgs[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 取操作员对应预算部门
        /// </summary>
        /// <returns>返回Json串</returns>
        public List<OrganizeModel> FindYSBM(string userCode)
        {
            var result = new List<OrganizeModel>();
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            if (userCode != null && !"".Equals(userCode))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dwdm", userCode));
            }
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "97"));
            IList<CorrespondenceSettingsModel> dygx = CorrespondenceSettingsFacade.Find(dicWhere).Data;
            if (dygx.Count > 0)
            {
                List<string> Orgcodes = dygx.ToList().Select(x => x.Dydm).Distinct().ToList();
                var dicorg= new Dictionary<string, object>();
                new CreateCriteria(dicorg)
                    .Add(ORMRestrictions<string>.Eq("IfCorp", "N"))
                    .Add(ORMRestrictions<List<string>>.In("OCode", Orgcodes));
                result=OrganizationFacade.Find(dicorg,new string[] { "OCode" }).Data.ToList();
            }
            return result;
        }

        /// <summary>
        /// 得到包含自己及下级的组织树
        /// </summary>
        /// <param name="orgphid"></param>
        /// <returns></returns>
        public OrganizeModel GetOrg_tree(long orgphid)
        {
            OrganizeModel org = OrganizationFacade.Find(orgphid).Data;
            OrganizationFacade.GetChild(org, new List<long>());
            return org;
        }
        #endregion

        #region 物料有关
        /// <summary>
        /// 获取模块
        /// </summary>
        public List<QtModulesModel> GetModules()
        {
            NGModuleRight right = new NGModuleRight();
            string product = AppInfoBase.Product;
            string Connstr = AppInfoBase.UserConnectString;
            string PubStr = AppInfoBase.PubConnectString;
            //string product = AppSessionConfig.GetCurrentProduct() + AppSessionConfig.GetCurrentSeries();
            //string Connstr = AppSessionConfig.GetLoginConnstr();
            //string DBaseVender = AppSessionConfig.GetDbVender();    //得到数据库类型
            string sql = string.Empty;

            string sql2 = string.Empty;
            //为了解决套件号与模块号一样，对套件号做特殊处理

            if (Connstr.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)//DBaseVender == "Oracle"
            {
                sql = " select distinct '0' as chk,'('||suitno||')' as cno,suitname as text,'***' as parentcno,'0' as verifyflag,suitorder as orderno,product from ngproducts   "
                    + " where product={0} and moduleid not in {1} union "
                    + " select '0' as chk,moduleno as cno,modulename as text,'('||suitno||')' as parentcno,'1' as verifyflag,moduleorder as orderno,product from ngproducts "
                    + " where product={0} and moduleid not in {1} order by orderno    ";

                sql2 = string.Format(@"select distinct ngmodulerights.*,'('||ngusers.ucode||')'||ngusers.uname as uname from ngmodulerights 
                                    left join ngusers on ngmodulerights.ucode=ngusers.ucode ");
            }
            else
            {
                sql = " select distinct '0' as chk,'('+suitno+')' as cno,suitname as text,'***' as parentcno,'0' as verifyflag,suitorder as orderno,product from ngproducts  "
                    + " where product={0} and moduleid not in {1} union "
                    + " select '0' as chk,moduleno as cno,modulename as text,'('+suitno+')' as parentcno,'1' as verifyflag,moduleorder as orderno,product from ngproducts  "
                    + " where product={0} and moduleid not in {1} order by orderno    ";

                sql2 = string.Format(@"select distinct ngmodulerights.*,'('+ngusers.ucode+')'+ngusers.uname as uname from ngmodulerights 
                                    left join ngusers on ngmodulerights.ucode=ngusers.ucode ");
            }

            sql = string.Format(sql, DbConvert.ToSqlString(product), right.Get_NotRightControlModuleFilterString());
            DataTable data= DbHelper.GetDataTable(Connstr, sql);
            List<QtModulesModel> ModulesList= ModelTableHelper.ConvertTo<QtModulesModel>(data).ToList();
            List<QtModulesModel> result = ModulesList.FindAll(x => x.verifyflag == "0");


            DataTable dtRights = DbHelper.GetDataTable(PubStr, sql2);//获取所有的授权信息
            for (var i=0;i< result.Count; i++)
            {
                result[i].children = GetModulesChild(ModulesList, result[i].cno, dtRights);
                if (result[i].children!=null)
                {
                    result[i].leaf = false;
                }
                else
                {
                    result[i].leaf = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 取下级模块
        /// </summary>
        /// <param name="ModulesList"></param>
        /// <param name="cno"></param>
        /// <param name="dtRights"></param>
        /// <returns></returns>
        public List<QtModulesModel> GetModulesChild(List<QtModulesModel> ModulesList,string cno, DataTable dtRights)
        {
            List<QtModulesModel> result = ModulesList.FindAll(x => x.parentcno == cno);
            if (result.Count > 0)
            {
                for (var i = 0; i < result.Count; i++)
                {
                    string key = string.Empty;
                    key = result[i].product + "." + result[i].parentcno.Replace("(","").Replace(")","") + "." + result[i].cno;
                    if (NGCOM.Instance.ModuleRightsCount[key.ToLower()] != null)
                    {
                        result[i].TotalCount = Convert.ToInt32(NGCOM.Instance.ModuleRightsCount[key.ToLower()]);
                    }
                    if (dtRights != null)
                    {
                        string filter = "moduleno=" + DbConvert.ToSqlString(result[i].cno);
                        DataRow[] drs = dtRights.Select(filter);
                        result[i].UsedCount = drs.Length;
                    }
                    result[i].RemnantCount = result[i].TotalCount - result[i].UsedCount;
                    result[i].children = GetModulesChild(ModulesList, result[i].cno, dtRights);
                    if(result [i].cno== "SR")
                    {
                        result[i].TotalCount = 10;
                        result[i].UsedCount = 0;
                        result[i].RemnantCount = 10;
                    }
                    if (result[i].children != null)
                    {
                        result[i].leaf = false;
                    }
                    else
                    {
                        result[i].leaf = true;
                    }
                }
                return result;
            }
            else
            {
                return null;
            }
            
            
        }


        /// <summary>
        /// 获取当前帐套对应的登录组织(并判断是否有权限)
        /// </summary>
        /// <param name="moduleno"></param>
        /// <returns></returns>
        public List<QtOrgModel> GetLoginOrg(string moduleno)
        {
            string PubStr = AppInfoBase.PubConnectString;
            string UserStr = AppInfoBase.UserConnectString;
            //string PubStr = AppSessionConfig.GetPubDBConnStr();
            //string UserStr = new NGUserDac().GetNGUserConnString(PubStr, uCode);
            //string DBaseVender = AppSessionConfig.GetDbVender();    //得到数据库类型
            string sql = string.Empty;
            string sql2 = string.Empty;
            if (UserStr.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
            {
                sql = "select '0' as chk,ocode as cno,oname||'('||ocode||')' as text,'***' as parentcno,'1' as verifyflag from fg_orglist where (iflogin='1' and isactive='1') and ocode not in (select ocode from fg_orgtoattr where attrcode = '44') order by ocode ";
                if (string.IsNullOrEmpty(AppInfoBase.UCode) && string.IsNullOrEmpty(moduleno))
                {
                    sql2 = string.Format(@"select distinct ngmodulerights.*,'('||ngusers.ucode||')'||ngusers.uname as uname from ngmodulerights 
                                    left join ngusers on ngmodulerights.ucode=ngusers.ucode ");
                }
                else if (string.IsNullOrEmpty(AppInfoBase.UCode) && !string.IsNullOrEmpty(moduleno))
                {
                    sql2 = string.Format(@"select distinct ngmodulerights.*,'('||ngusers.ucode||')'||ngusers.uname as uname from ngmodulerights 
                                    left join ngusers on ngmodulerights.ucode=ngusers.ucode where ngmodulerights.moduleno={0}",
                        DbConvert.ToSqlString(moduleno));
                }
                else
                {
                    sql2 = string.Format(@"select distinct ngmodulerights.*,'('||ngusers.ucode||')'||ngusers.uname as uname from ngmodulerights 
                                    left join ngusers on ngmodulerights.ucode=ngusers.ucode where ngmodulerights.ucode={0} and ngmodulerights.moduleno={1}",
                        DbConvert.ToSqlString(AppInfoBase.UCode), DbConvert.ToSqlString(moduleno));
                }
            }
            else
            {
                sql = "select '0' as chk,ocode as cno,oname+'('+ocode+')' as text,'***' as parentcno,'1' as verifyflag from fg_orglist where (iflogin='1' and isactive='1') and ocode not in (select ocode from fg_orgtoattr where attrcode = '44') order by ocode ";
                if (string.IsNullOrEmpty(AppInfoBase.UCode) && string.IsNullOrEmpty(moduleno))
                {
                    sql2 = string.Format(@"select distinct ngmodulerights.*,'('+ngusers.ucode+')'+ngusers.uname as uname from ngmodulerights 
                                    left join ngusers on ngmodulerights.ucode=ngusers.ucode ");
                }
                else if (string.IsNullOrEmpty(AppInfoBase.UCode) && !string.IsNullOrEmpty(moduleno))
                {
                    sql2 = string.Format(@"select distinct ngmodulerights.*,'('+ngusers.ucode+')'+ngusers.uname as uname from ngmodulerights 
                                    left join ngusers on ngmodulerights.ucode=ngusers.ucode where ngmodulerights.moduleno={0}",
                        DbConvert.ToSqlString(moduleno));
                }
                else
                {
                    sql2 = string.Format(@"select distinct ngmodulerights.*,'('+ngusers.ucode+')'+ngusers.uname as uname from ngmodulerights 
                                    left join ngusers on ngmodulerights.ucode=ngusers.ucode where ngmodulerights.ucode={0} and ngmodulerights.moduleno={1}",
                        DbConvert.ToSqlString(AppInfoBase.UCode), DbConvert.ToSqlString(moduleno));
                }
            }
            DataTable Orgdata = DbHelper.GetDataTable(UserStr, sql);
            var OrgList= ModelTableHelper.ConvertTo<QtOrgModel>(Orgdata).ToList();
            DataTable Modulerightsdata = DbHelper.GetDataTable(PubStr, sql2);
            var ModulerightsList = ModelTableHelper.ConvertTo<QtModulerightsModel>(Modulerightsdata).ToList();
            var checkOrgList2 = new List<string>();
            if (ModulerightsList.Count > 0)
            {
                checkOrgList2 = ModulerightsList.Select(x => x.ocode).Distinct().ToList();
            }
            if (checkOrgList2.Count > 0)
            {
                foreach (var i in OrgList)
                {
                    i.leaf = true;
                    i.children = null;
                    if (checkOrgList2.Contains(i.cno))
                    {
                        i.@checked = true;
                    }
                    else
                    {
                        i.@checked = false;
                    }

                }
            }
            else
            {
                foreach (var i in OrgList)
                {

                    i.children = null;
                    i.@checked = false;
                    i.leaf = true;
                }
            }
            return OrgList;
        }

        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="moduleno"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public int SaveRights(string moduleno,List<QtModulerightsModel> list)
        {
            //var delete=DeleteByModule(moduleno);
            //var insert=Insert(list);
            //return delete + insert;

            string PubStr = AppInfoBase.PubConnectString;
            string UserStr = AppInfoBase.UserConnectString;
            string sqlorg = string.Format("select ocode,oname from fg_orglist");
            DataTable Orgdata = DbHelper.GetDataTable(UserStr, sqlorg);
            string sql=""; //= string.Format("delete from ngmodulerights where ucode={0} and moduleno={1} ; ",
            //DbConvert.ToSqlString(AppInfoBase.UCode), DbConvert.ToSqlString(moduleno));
            DeleteByModule(moduleno);
            if (list.Count > 0)
            {
                if (UserStr.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    sql = sql + " insert all ";
                }
                else
                {
                    sql = sql + " insert into ngmodulerights(ucode,moduleno,ocode,oname) values ";
                }
                foreach (var data in list)
                {
                    if (!string.IsNullOrEmpty(AppInfoBase.UCode) && !string.IsNullOrEmpty(data.moduleno) && !string.IsNullOrEmpty(data.ocode))
                    {
                        if (string.IsNullOrEmpty(data.oname))
                        {
                            string filter = "ocode=" + DbConvert.ToSqlString(data.ocode);
                            DataRow[] drs = Orgdata.Select(filter);
                            if (drs != null)
                            {
                                data.oname = drs[0]["oname"].ToString();
                            }
                            /*string sqloname = string.Format("select oname from fg_orglist where ocode='{0}'", data.ocode);
                            object obj = DbHelper.ExecuteScalar(UserStr, sqloname);
                            if (obj != null && obj != DBNull.Value)
                            {
                                data.oname = obj.ToString();
                            }*/
                        }
                        if (UserStr.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                        {
                            sql = sql + string.Format(" into ngmodulerights(ucode,moduleno,ocode,oname) values('{0}','{1}','{2}','{3}')  ",
                            AppInfoBase.UCode, data.moduleno, data.ocode, data.oname);
                        }
                        else
                        {
                            sql = sql + string.Format(" ('{0}','{1}','{2}','{3}') ,",
                            AppInfoBase.UCode, data.moduleno, data.ocode, data.oname);
                        }
                        
                    }
                }
                if (UserStr.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    sql = sql + " SELECT 1 FROM dual ";
                }
                else
                {
                    sql = sql.Substring(0, sql.Length - 1);
                }

                return DbHelper.ExecuteNonQuery(PubStr, sql);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 按模块号和帐套号删除权限
        /// </summary>
        /// <param name="moduleno"></param>
        /// <returns></returns>
        public static int DeleteByModule(string moduleno)
        {
            string PubStr = AppInfoBase.PubConnectString;
            string sql = string.Format("delete from ngmodulerights where ucode={0} and moduleno={1}",
            DbConvert.ToSqlString(AppInfoBase.UCode), DbConvert.ToSqlString(moduleno));

            return DbHelper.ExecuteNonQuery(PubStr, sql);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int Insert(List<QtModulerightsModel> list)
        {
            string PubStr= AppInfoBase.PubConnectString;
            string UserStr = AppInfoBase.UserConnectString;
            int result = 0;
            foreach (var data in list)
            {
                if (!string.IsNullOrEmpty(AppInfoBase.UCode) && !string.IsNullOrEmpty(data.moduleno) && !string.IsNullOrEmpty(data.ocode))
                {
                    if (string.IsNullOrEmpty(data.oname))
                    {
                        string sqloname = string.Format("select oname from fg_orglist where ocode='{0}'", data.ocode);
                        object obj = DbHelper.ExecuteScalar(UserStr, sqloname);
                        if (obj != null && obj != DBNull.Value)
                        {
                            data.oname = obj.ToString();
                        }
                    }
                    string sql = string.Format("insert into ngmodulerights(ucode,moduleno,ocode,oname) values('{0}','{1}','{2}','{3}')",
                        AppInfoBase.UCode, data.moduleno, data.ocode, data.oname);
                    result += DbHelper.ExecuteNonQuery(PubStr, sql);
                }
            }
            return result;
        }
        #endregion


        /// <summary>
        /// 得到组织拼接成的字符串
        /// </summary>
        /// <param name="orgphidList"></param>
        /// <returns></returns>
        public string GetOrgStr(List<long> orgphidList)
        {
            var org = OrganizationFacade.Find(x=> orgphidList.Contains(x.PhId), "OCode").Data.ToList().Select(x=>x.OName).ToList();
            var str = string.Join(",", org.ToArray());
            return str;
        }

        /// <summary>
        /// 得到组织列表
        /// </summary>
        /// <param name="orgphidList"></param>
        /// <returns></returns>
        public List<OrganizeModel> GetOrgInfo(List<long> orgIds)
        {
            var orgList = OrganizationFacade.Find(x => orgIds.Contains(x.PhId), "OCode").Data.ToList();
            
            return orgList;
        }

        /// <summary>
        /// 得到组织代码列表
        /// </summary>
        /// <param name="orgphidList"></param>
        /// <returns></returns>
        public List<OrganizeModel> GetOrgCodeList(List<long> orgphidList)
        {
            var org = OrganizationFacade.Find(x => orgphidList.Contains(x.PhId), "OCode").Data.ToList();
            return org;
        }
        /// <summary>
        /// 根据操作员组织权限列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public IList<OrganizeModel> GetAuthOrgList(long UserId)
        {
            return OrganizationFacade.GetAuthOrgList(UserId);
        }


        /// <summary>
        /// 根据操作员id取操作员
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public User2Model GetUserById(long UserId)
        {
            return UserFacade.Find(UserId).Data;
        }
        /// <summary>
        /// 根据操作员code取操作员
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        public User2Model GetUserByCode(string UserCode)
        {
            var Users = UserFacade.Find(x => x.UserNo == UserCode).Data;
            if(Users!=null && Users.Count > 0)
            {
                return Users[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 取组织列表
        /// </summary>
        /// <param name="Codes"></param>
        /// <returns></returns>
        public IList<OrganizeModel> GetOrgListByCode(List<string> Codes)
        {
            return OrganizationFacade.Find(x => Codes.Contains(x.OCode)).Data;
        }

        /// <summary>
        /// 完整组织列表
        /// </summary>
        /// <returns></returns>
        public IList<OrganizeModel> GetALLOrgList()
        {
            return OrganizationFacade.Find(x => x.PhId!=0).Data.ToList().OrderBy(x=>x.OCode).ToList();
        }
    }
}

