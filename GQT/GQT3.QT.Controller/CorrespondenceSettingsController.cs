#region Summary
/**************************************************************************************
    * 类 名 称：        CorrespondenceSettingsController
    * 命名空间：        GQT3.QT.Controller
    * 文 件 名：        CorrespondenceSettingsController.cs
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
using System.Web.Mvc;
using Newtonsoft.Json;
using NG3.Web.Mvc;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Controller;

using GQT3.QT.Service.Interface;
using GQT3.QT.Model.Domain;
using GQT3.QT.Model;
using Enterprise3.Common.Base.Criterion;
using GData3.Common.Utils;
using GSP3.SP.Service.Interface;
using Enterprise3.WebApi.GSP3.SP.Model.Request;
using Enterprise3.WebApi.GSP3.SP.Model.Response;
using NG3.Data.Service;
using Spring.Data.Common;
using GGK3.GK.Service.Interface;
using System.Data;
using GQT3.QT.Model.Extra;

namespace GQT3.QT.Controller
{
    /// <summary>
    /// CorrespondenceSettings控制处理类
    /// </summary>
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class CorrespondenceSettingsController : AFCommonController
    {
        ICorrespondenceSettingsService CorrespondenceSettingsService { get; set; }
        ICorrespondenceSettings2Service CorrespondenceSettings2Service { get; set; }
        IQTSysSetService QTSysSetService { get; set; }
        IGAppvalRecordService GAppvalRecordService { get; set; }

        IGKPaymentMstService GKPaymentMstService { get; set; }
        ISysSettingService SysSettingService { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public CorrespondenceSettingsController()
        {
            CorrespondenceSettingsService = base.GetObject<ICorrespondenceSettingsService>("GQT3.QT.Service.CorrespondenceSettings");
            CorrespondenceSettings2Service = base.GetObject<ICorrespondenceSettings2Service>("GQT3.QT.Service.CorrespondenceSettings2");
            QTSysSetService = base.GetObject<IQTSysSetService>("GQT3.QT.Service.QTSysSet");
            GAppvalRecordService = base.GetObject<IGAppvalRecordService>("GSP3.SP.Service.GAppvalRecord");
            GKPaymentMstService = base.GetObject<IGKPaymentMstService>("GGK3.GK.Service.GKPaymentMst");
            SysSettingService = base.GetObject<ISysSettingService>("GQT3.QT.Service.SysSetting");
        }

        /// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult CorrespondenceSettingsList()
        {
            ViewBag.Title = base.GetMenuLanguage("CorrespondenceSettings");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "对应关系设置";
            }
            base.InitialMultiLanguage("CorrespondenceSettings");
            ViewBag.IndividualInfo = this.GetIndividualUI("CorrespondenceSettings");
            return View("CorrespondenceSettingsList");
        }

        /// <summary>
        /// 指向对应关系-资金来源页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult RelationZJLY()
        {
            ViewBag.Title = base.GetMenuLanguage("CorrespondenceSettings");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "对应关系设置-资金来源";
            }
            base.InitialMultiLanguage("CorrespondenceSettings");
            ViewBag.IndividualInfo = this.GetIndividualUI("CorrespondenceSettings");
            return View("RelationZJLY");
        }

        /// <summary>
        /// 指向对应关系-预算科目页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult RelationYSKM()
        {
            ViewBag.Title = base.GetMenuLanguage("CorrespondenceSettings");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "对应关系设置-预算科目";
            }
            base.InitialMultiLanguage("CorrespondenceSettings");
            ViewBag.IndividualInfo = this.GetIndividualUI("CorrespondenceSettings");
            return View("RelationYSKM");
        }

        /// <summary>
        /// 指向对应关系-预算科目页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult RelationYSK()
        {
            ViewBag.Title = base.GetMenuLanguage("CorrespondenceSettings");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "对应关系设置-预算库";
            }
            base.InitialMultiLanguage("CorrespondenceSettings");
            ViewBag.IndividualInfo = this.GetIndividualUI("CorrespondenceSettings");
            return View("RelationYSK");
        }

        /// <summary>
        /// 指向对应关系-操作员对应预算部门页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult RelationYSBM()
        {
            ViewBag.Title = base.GetMenuLanguage("CorrespondenceSettings");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "操作员对应预算部门设置";
            }
            base.InitialMultiLanguage("CorrespondenceSettings");
            ViewBag.IndividualInfo = this.GetIndividualUI("CorrespondenceSettings");
            return View("RelationYSBM");
        }

        /// <summary>
        /// 指向对应关系-操作员默认组织部门页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult RelationDefaultOrg()
        {
            ViewBag.Title = base.GetMenuLanguage("CorrespondenceSettings");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "操作员默认组织部门";
            }
            base.InitialMultiLanguage("CorrespondenceSettings");
            ViewBag.IndividualInfo = this.GetIndividualUI("CorrespondenceSettings");
            return View("RelationDefaultOrg");
        }


        /// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult CorrespondenceSettingsEdit()
        {
            var tabTitle = base.GetMenuLanguage("CorrespondenceSettings");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "对应关系设置";
            }
            base.SetUserDefScriptUrl("CorrespondenceSettings");
            base.InitialMultiLanguage("CorrespondenceSettings");
            ViewBag.IndividualInfo = this.GetIndividualUI("CorrespondenceSettings");

            ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型

            if (ViewBag.OType == "add")
            {
                ViewBag.Title = tabTitle + "-新增";
            }
            else if (ViewBag.OType == "edit")
            {
                ViewBag.Title = tabTitle + "-修改";
            }
            else if (ViewBag.OType == "view")
            {
                ViewBag.Title = tabTitle + "-查看";
            }

            return View("CorrespondenceSettingsEdit");
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetCorrespondenceSettingsList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            //var result = CorrespondenceSettingsService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);
            var result = CorrespondenceSettingsService.LoadWithPage(storeparam.PageIndex, 100, dicWhere);

            return DataConverterHelper.EntityListToJson<CorrespondenceSettingsModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetCorrespondenceSettingsInfo()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
            var findedresult = CorrespondenceSettingsService.Find(id);
            return DataConverterHelper.ResponseResultToJson(findedresult);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
            string correspondencesettingsformData = System.Web.HttpContext.Current.Request.Form["correspondencesettingsformData"];

            var correspondencesettingsforminfo = DataConverterHelper.JsonToEntity<CorrespondenceSettingsModel>(correspondencesettingsformData);

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            try
            {
                savedresult = CorrespondenceSettingsService.Save<Int64>(correspondencesettingsforminfo.AllRow, "");
            }
            catch (Exception ex)
            {
                savedresult.Status = ResponseStatus.Error;
                savedresult.Msg = ex.Message.ToString();
            }
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Delete()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            var deletedresult = CorrespondenceSettingsService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 后台传字符串保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save2(string data)
        {
            string correspondencesettingsformData = data;

            var correspondencesettingsforminfo = DataConverterHelper.JsonToEntity<CorrespondenceSettingsModel>(correspondencesettingsformData);

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            try
            {
                savedresult = CorrespondenceSettingsService.Save<Int64>(correspondencesettingsforminfo.AllRow, "");
            }
            catch (Exception ex)
            {
                savedresult.Status = ResponseStatus.Error;
                savedresult.Msg = ex.Message.ToString();
            }
            return DataConverterHelper.SerializeObject(savedresult);
        }


        /// <summary>
        /// 前台传主键删除数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string DeletebyId(long phid)
        {
            long id = phid;  //主表主键
            //long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            var deletedresult = CorrespondenceSettingsService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 取对应关系列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetCorrespondenceSettingsListbyRelation(string Dylx, string Dwdm, string DefStr1)
        {
            //string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            //Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary
            var dicWhere = new Dictionary<string, object>();
            if (Dylx != null)
            {
                new CreateCriteria(dicWhere).
                        Add(ORMRestrictions<string>.Eq("Dylx", Dylx));
            }
            if (Dwdm != null)
            {
                new CreateCriteria(dicWhere).
                        Add(ORMRestrictions<string>.Eq("Dwdm", Dwdm));
            }
            if (DefStr1 != null)
            {
                new CreateCriteria(dicWhere).
                        Add(ORMRestrictions<string>.Eq("DefStr1", DefStr1));
            }
            DataStoreParam storeparam = this.GetDataStoreParam();
            //var result = CorrespondenceSettingsService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);
            //var result = CorrespondenceSettingsService.LoadWithPage(storeparam.PageIndex, 100, dicWhere);
            var result = CorrespondenceSettingsService.ServiceHelper.LoadWithPageInfinity("GQT3.QT.DYGX_All", dicWhere);
            return DataConverterHelper.EntityListToJson<CorrespondenceSettingsModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 取对应关系列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetRelationYSBMList()
        {
            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = CorrespondenceSettingsService.LoadWithPageYSBM(storeparam);
            return DataConverterHelper.EntityListToJson<User2Model>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 取对应关系列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetRelationYSBMList2()
        {
            string userCode = System.Web.HttpContext.Current.Request.Params["userCode"];//查询条件
            DataStoreParam dataStoreParam = this.GetDataStoreParam();
            var result = CorrespondenceSettingsService.LoadWithPageYSBM2(dataStoreParam, userCode);
            return DataConverterHelper.EntityListToJson<OrganizeModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 取对应关系列表数据  操作员对应组织部门
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetRelationUser_OrgList()
        {
            string userCode = System.Web.HttpContext.Current.Request.Params["userCode"];//查询条件
            DataStoreParam dataStoreParam = this.GetDataStoreParam();
            var result = CorrespondenceSettingsService.LoadWithPageUser_Org(dataStoreParam, userCode);
            return DataConverterHelper.EntityListToJson<CorrespondenceSettingsModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 取对应关系列表数据  取所有org（不包括部门）
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetOrg()
        {
            DataStoreParam dataStoreParam = this.GetDataStoreParam();
            var result = CorrespondenceSettingsService.LoadWithPageOrg(dataStoreParam);
            return DataConverterHelper.EntityListToJson<OrganizeModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 取对应关系列表数据  取所有org（包括部门）
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetAllOrg()
        {
            DataStoreParam dataStoreParam = this.GetDataStoreParam();
            var result = CorrespondenceSettingsService.LoadWithPageAllOrg(dataStoreParam);
            return DataConverterHelper.EntityListToJson<OrganizeModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 取对应关系列表数据  根据组织id取部门
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetBM()
        {
            string OrgId = System.Web.HttpContext.Current.Request.Params["OrgId"];//查询条件
            DataStoreParam dataStoreParam = this.GetDataStoreParam();
            var result = CorrespondenceSettingsService.LoadWithPageBM(dataStoreParam, OrgId);
            return DataConverterHelper.EntityListToJson<OrganizeModel>(result.Results, (Int32)result.TotalItems);
        }


        /// <summary>
        /// 查找操作员默认组织跟部门
        /// </summary>
        /// <returns></returns>
        public string FindFDeclarationUnit()
        {
            string userID = System.Web.HttpContext.Current.Request.Params["userID"];  //操作员id

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "08"))
                .Add(ORMRestrictions<Int32>.Eq("DefInt1", 1)).Add(ORMRestrictions<string>.Eq("Dwdm", userID));
            var orgCode = "";
            var dept = "";
            var orgName = "";
            var deptName = "";
            var orgCodeList = CorrespondenceSettingsService.Find(dicWhere);
            if (orgCodeList.Data.Count > 0)
            {
                orgCode = orgCodeList.Data[0].Dydm;
                dept = orgCodeList.Data[0].DefStr3;
                if (!string.IsNullOrEmpty(orgCode))
                {
                    orgName = CorrespondenceSettingsService.GetOrg(orgCode).OName;
                }
                else
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = "该操作员未设置默认组织！";
                    return DataConverterHelper.SerializeObject(savedresult);
                }
                if (!string.IsNullOrEmpty(dept))
                {
                    deptName = CorrespondenceSettingsService.GetOrg(dept).OName;
                }
                else
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = "该操作员未设置默认部门！";
                    return DataConverterHelper.SerializeObject(savedresult);
                }
                var dicWhere1 = new Dictionary<string, object>();
                new CreateCriteria(dicWhere1).Add(ORMRestrictions<string>.Eq("Dylx", "SB"))
                .Add(ORMRestrictions<string>.Eq("Dwdm", orgCode));
                var orgSbList = CorrespondenceSettings2Service.Find(dicWhere1);
                if (orgSbList.Data.Count > 0) { }
                else
                {
                    orgCode = "";
                    dept = "";
                    orgName = "";
                    deptName = "";
                }

            }
            savedresult.Status = "success";
            savedresult.Msg = orgCode + "," + dept + "," + orgName + "," + deptName;
            return DataConverterHelper.SerializeObject(savedresult);
        }


        /// <summary>
        /// 查找默认组织跟部门用以单点登录
        /// </summary>
        /// <returns></returns>
        public string FindFDeclarationUnitToLoad()
        {
            string userID = System.Web.HttpContext.Current.Request.Params["userID"];  //操作员id

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "08"))
                .Add(ORMRestrictions<Int32>.Eq("DefInt1", 1)).Add(ORMRestrictions<string>.Eq("Dwdm", userID));
            var orgCode = "";
            var dept = "";
            var orgCodeList = CorrespondenceSettingsService.Find(dicWhere);
            if (orgCodeList.Data.Count > 0)
            {
                orgCode = orgCodeList.Data[0].Dydm;
                dept = orgCodeList.Data[0].DefStr3;
            }
            savedresult.Status = "success";
            savedresult.Msg = orgCode + "," + dept;
            return DataConverterHelper.SerializeObject(savedresult);
        }


        /// <summary>
        /// 取对应关系列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetRelationYSBMLeftList()
        {
            string userCode = System.Web.HttpContext.Current.Request.Params["userCode"];//查询条件
            DataStoreParam dataStoreParam = this.GetDataStoreParam();
            var result = CorrespondenceSettingsService.GetRelationYSBMLeftList(dataStoreParam, userCode);
            return DataConverterHelper.EntityListToJson<OrganizeModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 取对应关系列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetRelationYSBMRightList()
        {
            string userCode = System.Web.HttpContext.Current.Request.Params["userCode"];//查询条件
            //DataStoreParam dataStoreParam = this.GetDataStoreParam();
            var result = CorrespondenceSettingsService.GetRelationYSBMRightList(userCode);
            return DataConverterHelper.EntityListToJson<OrganizeModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 设置操作员对应预算部门关系
        /// </summary>
        /// <returns>返回Json串</returns>
        public string RelationYSBMSetting()
        {
            //string array = System.Web.HttpContext.Current.Request.Params["array"];
            string Models = System.Web.HttpContext.Current.Request.Params["Models"];
            string UserNo = System.Web.HttpContext.Current.Request.Params["UserNo"];
            var info = DataConverterHelper.JsonToEntity<OrganizeModel>(Models);
            var resultList = CorrespondenceSettingsService.UpdataRelationYSBM(info.AllRow, UserNo);
            var result = new SavedResult<Int64>();
            try
            {
                result = CorrespondenceSettingsService.Save<Int64>(resultList, "");
            }
            catch (Exception e)
            {
                result.Status = ResponseStatus.Error;
                result.Msg = "设置失败，请重新设置！";
            }
            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 资金来源关系的改变
        /// </summary>
        /// <returns></returns>
        public string UpdateZJLY()
        {

            string OrgCode = System.Web.HttpContext.Current.Request.Params["OrgCode"];
            string OrgPhId = System.Web.HttpContext.Current.Request.Params["OrgPhId"];

            string mydelete = System.Web.HttpContext.Current.Request.Params["mydelete"];
            var mydeleteinfo = DataConverterHelper.JsonToEntity<CorrespondenceSettingsModel>(mydelete);

            string myinsert = System.Web.HttpContext.Current.Request.Params["myinsert"];
            var myinsertinfo = DataConverterHelper.JsonToEntity<SourceOfFundsModel>(myinsert);


            var result = CorrespondenceSettingsService.UpdateZJLY(OrgCode, OrgPhId, mydeleteinfo.AllRow, myinsertinfo.AllRow);
            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 预算科目关系的改变
        /// </summary>
        /// <returns></returns>
        public string UpdateYSKM()
        {

            string OrgCode = System.Web.HttpContext.Current.Request.Params["OrgCode"];
            string OrgPhId = System.Web.HttpContext.Current.Request.Params["OrgPhId"];
            string OrgName = System.Web.HttpContext.Current.Request.Params["OrgName"];

            string mydelete = System.Web.HttpContext.Current.Request.Params["mydelete"];
            var mydeleteinfo = DataConverterHelper.JsonToEntity<CorrespondenceSettingsModel>(mydelete);

            string myinsert = System.Web.HttpContext.Current.Request.Params["myinsert"];
            var myinsertinfo = DataConverterHelper.JsonToEntity<BudgetAccountsModel>(myinsert);


            var result = CorrespondenceSettingsService.UpdateYSKM(OrgCode, OrgPhId, OrgName, mydeleteinfo.AllRow, myinsertinfo.AllRow);
            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 预算库关系的改变
        /// </summary>
        /// <returns></returns>
        public string UpdateYSK()
        {

            string OrgCode = System.Web.HttpContext.Current.Request.Params["OrgCode"];
            string OrgPhId = System.Web.HttpContext.Current.Request.Params["OrgPhId"];

            string mydelete = System.Web.HttpContext.Current.Request.Params["mydelete"];
            var mydeleteinfo = DataConverterHelper.JsonToEntity<CorrespondenceSettingsModel>(mydelete);

            string myinsert = System.Web.HttpContext.Current.Request.Params["myinsert"];
            var myinsertinfo = DataConverterHelper.JsonToEntity<ProjLibProjModel>(myinsert);


            var result = CorrespondenceSettingsService.UpdateYSK(OrgCode, OrgPhId, mydeleteinfo.AllRow, myinsertinfo.AllRow);
            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 操作员默认组织设置
        /// </summary>
        /// <returns></returns>
        public string UpdateDefaultOrg()
        {
            string usercode = System.Web.HttpContext.Current.Request.Params["usercode"];
            string username = System.Web.HttpContext.Current.Request.Params["username"];

            string mydelete = System.Web.HttpContext.Current.Request.Params["mydelete"];
            var mydeleteinfo = DataConverterHelper.JsonToEntity<CorrespondenceSettingsModel>(mydelete);

            string myinsert = System.Web.HttpContext.Current.Request.Params["myinsert"];
            var myinsertinfo = DataConverterHelper.JsonToEntity<OrganizeModel>(myinsert);


            var result = CorrespondenceSettingsService.UpdateDefaultOrg(usercode, username, mydeleteinfo.AllRow, myinsertinfo.AllRow);
            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 取操作员能操作的org
        /// </summary>
        /// <returns></returns>
        public string OrgByUser()
        {
            string userid = System.Web.HttpContext.Current.Request.Params["userid"];
            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = CorrespondenceSettingsService.LoadWithPageOrgByUser(storeparam, userid);
            //return DataConverterHelper.ResponseResultToJson(result);
            return DataConverterHelper.EntityListToJson<OrganizeModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string DeleteQtbase()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            var deletedresult = CorrespondenceSettingsService.DeleteQtbase(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }
        /// <summary>
        /// 得到登录信息
        /// </summary>
        /// <returns></returns>
        public string GetLogin()
        {
            return CorrespondenceSettingsService.GetLogin();
        }

        /// <summary>
        /// 获取所有审批种类对应的审批单据的总数量
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetRecordListNum()
        {
            long Uid = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["Uid"]);
            long Orgid = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["Orgid"]);
            string Year = System.Web.HttpContext.Current.Request.Params["Year"];
            if (Uid == 0)
            {
                return DCHelper.ErrorMessage("用户id为空！");
            }
            if (Orgid == 0)
            {
                return DCHelper.ErrorMessage("组织id为空！");
            }
            if (string.IsNullOrEmpty(Year))
            {
                return DCHelper.ErrorMessage("年度为空！");
            }

            //设置当前数据库连接信息            
            ConnectionInfoService.SetCallContextConnectString(NG3.AppInfoBase.UserConnectString);
            MultiDelegatingDbProvider.CurrentDbProviderName = NG3.AppInfoBase.DbName;

            BillRequestModel billRequest = new BillRequestModel();
            billRequest.Uid = Uid;
            billRequest.Orgid = Orgid;
            billRequest.Year = Year;
            var YNum = 0;//已审数量
            var NNum = 0;//待审数量
            try
            {
                //获取审批所有类型
                List<QTSysSetModel> procTypes = QTSysSetService.GetProcTypes();
                if (procTypes != null && procTypes.Count > 0)
                {
                    foreach (var sysSet in procTypes)
                    {
                        billRequest.BType = sysSet.Value;
                        billRequest.Splx_Phid = sysSet.PhId;
                        int total = 0;
                        List<AppvalRecordVo> recordVos = GAppvalRecordService.GetDoneRecordList(billRequest, out total);
                        int total2 = 0;
                        List<AppvalRecordVo> recordVos2 = GAppvalRecordService.GetUnDoRecordList(billRequest, out total2);

                        YNum += total;
                        NNum += total2;
                    }
                }
                var dic = new Dictionary<string, object>();
                new CreateCriteria(dic).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                new CreateCriteria(dic).Add(ORMRestrictions<List<Int32>>.In("FState", new List<int>() { 2, 0 }));
                new CreateCriteria(dic).Add(ORMRestrictions<Int64>.Eq("OrgPhid", Orgid));
                new CreateCriteria(dic).Add(ORMRestrictions<string>.Eq("FYear", Year));
                //var Query = GKPaymentMstService.GetPaymentFailure(dic);
                var Query = GKPaymentMstService.Find(dic).Data;
                return DataConverterHelper.SerializeObject(new
                {
                    Status = "success",
                    YNum = YNum,
                    NNum = NNum,
                    PaymentState = Query.Count//【待支付】信息提醒需求
                });
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 取操作员对应预算部门
        /// </summary>
        /// <returns>返回Json串</returns>
        public string FindYSBM()
        {
            string userCode = System.Web.HttpContext.Current.Request.Params["userCode"];
            var result = CorrespondenceSettingsService.FindYSBM(userCode);

            return DataConverterHelper.EntityListToJson<OrganizeModel>(result, (Int32)result.Count);
        }

        #region 物料有关
        /// <summary>
        /// 指向组织模块授权页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult ModuleRights()
        {
            ViewBag.Title = base.GetMenuLanguage("CorrespondenceSettings");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "组织模块授权";
            }
            base.InitialMultiLanguage("CorrespondenceSettings");
            ViewBag.IndividualInfo = this.GetIndividualUI("CorrespondenceSettings");
            return View("ModuleRights");
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetModules()
        {
            List<QtModulesModel> data = CorrespondenceSettingsService.GetModules();
            return DataConverterHelper.SerializeObject(data);
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetLoginOrg()
        {
            string moduleno = System.Web.HttpContext.Current.Request.Params["moduleno"];
            if (string.IsNullOrEmpty(moduleno))
            {
                return DataConverterHelper.SerializeObject(new List<QtOrgModel>());
            }
            else
            {
                List<QtOrgModel> data = CorrespondenceSettingsService.GetLoginOrg(moduleno);
                return DataConverterHelper.SerializeObject(data);
            }
        }

        /// <summary>
        /// 保存权限
        /// <returns></returns>
        public string SaveRights()
        {
            string moduleno = System.Web.HttpContext.Current.Request.Params["moduleno"];

            string data = System.Web.HttpContext.Current.Request.Params["data"];
            var list = JsonConvert.DeserializeObject<List<QtModulerightsModel>>(data);
            var result = new SavedResult<Int64>();
            try
            {
                CorrespondenceSettingsService.SaveRights(moduleno, list);
                result.Status = ResponseStatus.Success;
            }
            catch (Exception e)
            {
                result.Status= ResponseStatus.Error;
            }
            return DataConverterHelper.SerializeObject(result);
        }
        #endregion
    }
}

