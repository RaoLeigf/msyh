#region Summary
/**************************************************************************************
    * 类 名 称：        CorrespondenceSettings2Controller
    * 命名空间：        GQT3.QT.Controller
    * 文 件 名：        CorrespondenceSettings2Controller.cs
    * 创建时间：        2018/9/6 
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
using System.Linq;
using GYS3.YS.Service.Interface;

namespace GQT3.QT.Controller
{
	/// <summary>
	/// CorrespondenceSettings2控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class CorrespondenceSettings2Controller : AFCommonController
    {
        ICorrespondenceSettings2Service CorrespondenceSettings2Service { get; set; }
        IOrgRelatitem2Service OrgRelatitem2Service { get; set; }
        IBudgetMstService BudgetMstService { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public CorrespondenceSettings2Controller()
	    {
	        CorrespondenceSettings2Service = base.GetObject<ICorrespondenceSettings2Service>("GQT3.QT.Service.CorrespondenceSettings2");
            OrgRelatitem2Service = base.GetObject<IOrgRelatitem2Service>("GQT3.QT.Service.OrgRelatitem2");
            BudgetMstService = base.GetObject<IBudgetMstService>("GYS3.YS.Service.BudgetMst");
        }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult CorrespondenceSettings2List()
        {
			ViewBag.Title = base.GetMenuLanguage("CorrespondenceSettings2");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "对应关系设置2";
            }
            base.InitialMultiLanguage("CorrespondenceSettings2");
            ViewBag.IndividualInfo = this.GetIndividualUI("CorrespondenceSettings2");
            return View("CorrespondenceSettings2List");
        }

        /// <summary>
        /// 指向对应关系-支出类别页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult RelationZCLB()
        {
            ViewBag.Title = base.GetMenuLanguage("CorrespondenceSettings2");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "对应关系-项目类型";
            }
            base.InitialMultiLanguage("CorrespondenceSettings2");
            ViewBag.IndividualInfo = this.GetIndividualUI("CorrespondenceSettings2");
            return View("RelationZCLB");
        }

        /// <summary>
        /// 指向单位是否为申报单位设置页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult IsOrgSB()
        {
            ViewBag.Title = base.GetMenuLanguage("CorrespondenceSettings2");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "单位是否为申报单位设置";
            }
            base.InitialMultiLanguage("CorrespondenceSettings2");
            ViewBag.IndividualInfo = this.GetIndividualUI("CorrespondenceSettings2");
            return View("IsOrgSB");
        }

        /// <summary>
        /// 指向对应关系-支出类别页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult RelationSBDW()
        {
            ViewBag.Title = base.GetMenuLanguage("CorrespondenceSettings2");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "对应关系-项目类型";
            }
            base.InitialMultiLanguage("CorrespondenceSettings2");
            ViewBag.IndividualInfo = this.GetIndividualUI("CorrespondenceSettings2");
            return View("RelationSBDW");
        }

        /// <summary>
        /// 指向项目类型对应项目预算明细显示格式设置页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult YSDtljiage()
        {
            ViewBag.Title = base.GetMenuLanguage("CorrespondenceSettings2");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "项目类型对应项目预算明细显示格式设置";
            }
            base.InitialMultiLanguage("CorrespondenceSettings2");
            ViewBag.IndividualInfo = this.GetIndividualUI("CorrespondenceSettings2");
            return View("YSDtljiage");
        }

        /// <summary>
        /// 预算接口组织对应数据库设置
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult DbConnByOrg()
        {
            ViewBag.Title = base.GetMenuLanguage("CorrespondenceSettings2");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "预算接口组织对应数据库设置";
            }
            base.InitialMultiLanguage("CorrespondenceSettings2");
            ViewBag.IndividualInfo = this.GetIndividualUI("CorrespondenceSettings2");
            return View("DbConnByOrg");
        }


        /// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult CorrespondenceSettings2Edit()
        {
			var tabTitle = base.GetMenuLanguage("CorrespondenceSettings2");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "对应关系设置2";
            }
            base.SetUserDefScriptUrl("CorrespondenceSettings2");
            base.InitialMultiLanguage("CorrespondenceSettings2");
            ViewBag.IndividualInfo = this.GetIndividualUI("CorrespondenceSettings2");

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

            return View("CorrespondenceSettings2Edit");
        }

        /// <summary>
        /// 归口项目对应部门设置
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult RelationGKXM()
        {
            ViewBag.Title = base.GetMenuLanguage("CorrespondenceSettings2");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "归口项目对应部门设置";
            }
            base.InitialMultiLanguage("CorrespondenceSettings2");
            ViewBag.IndividualInfo = this.GetIndividualUI("CorrespondenceSettings2");
            return View("RelationGKXM");
        }

        /// <summary>
        /// 首页显示设置
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult IndexSet()
        {
            ViewBag.Title = base.GetMenuLanguage("CorrespondenceSettings2");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "首页显示设置";
            }
            base.InitialMultiLanguage("CorrespondenceSettings2");
            ViewBag.IndividualInfo = this.GetIndividualUI("CorrespondenceSettings2");
            return View("IndexSet");
        }

        /// <summary>
        /// 绩效得分对应绩效结论设置
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult RelationJXJL()
        {
            ViewBag.Title = base.GetMenuLanguage("CorrespondenceSettings2");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "绩效得分对应绩效结论设置";
            }
            base.InitialMultiLanguage("CorrespondenceSettings2");
            ViewBag.IndividualInfo = this.GetIndividualUI("CorrespondenceSettings2");
            return View("RelationJXJL");
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetCorrespondenceSettings2List()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = CorrespondenceSettings2Service.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<CorrespondenceSettings2Model>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetCorrespondenceSettings2Info()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = CorrespondenceSettings2Service.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string correspondencesettings2formData = System.Web.HttpContext.Current.Request.Form["correspondencesettings2formData"];

			var correspondencesettings2forminfo = DataConverterHelper.JsonToEntity<CorrespondenceSettings2Model>(correspondencesettings2formData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = CorrespondenceSettings2Service.Save<Int64>(correspondencesettings2forminfo.AllRow,"");
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

            var deletedresult = CorrespondenceSettings2Service.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 后台传字符串保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save2(string data)
        {
            string correspondencesettings2formData = data;

            var correspondencesettings2forminfo = DataConverterHelper.JsonToEntity<CorrespondenceSettings2Model>(correspondencesettings2formData);

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            try
            {
                savedresult = CorrespondenceSettings2Service.Save<Int64>(correspondencesettings2forminfo.AllRow,"");
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

            var deletedresult = CorrespondenceSettings2Service.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 取对应关系列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetCorrespondenceSettings2ListbyRelation(string Dylx, string Dwdm, string DefStr1)
        {
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
            //var result = CorrespondenceSettings2Service.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);
            var result = CorrespondenceSettings2Service.ServiceHelper.LoadWithPageInfinity("GQT3.QT.DYGX2_All", dicWhere);

            return DataConverterHelper.EntityListToJson<CorrespondenceSettings2Model>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 是否为申报单位的设置
        /// </summary>
        /// <returns>返回Json串</returns>
        public string UpdateIfSBOrg()
        {
            string Models = System.Web.HttpContext.Current.Request.Params["Models"];
            var info = DataConverterHelper.JsonToEntity<OrganizeModel>(Models);

            string InsertData = System.Web.HttpContext.Current.Request.Params["InsertData"];
            var Insertinfo = DataConverterHelper.JsonToEntity<OrganizeModel>(InsertData);

            string DeleteData = System.Web.HttpContext.Current.Request.Params["DeleteData"];
            var Deleteinfo = DataConverterHelper.JsonToEntity<CorrespondenceSettings2Model>(DeleteData);


            var result = CorrespondenceSettings2Service.UpdateIfSBOrg(info.AllRow, Deleteinfo.AllRow, Insertinfo.AllRow);
            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 支出类别关系的改变
        /// </summary>
        /// <returns></returns>
        public string UpdateZCLB()
        {

            string OrgCode = System.Web.HttpContext.Current.Request.Params["OrgCode"];
            string OrgPhId = System.Web.HttpContext.Current.Request.Params["OrgPhId"];

            string mydelete = System.Web.HttpContext.Current.Request.Params["mydelete"];
            var mydeleteinfo = DataConverterHelper.JsonToEntity<CorrespondenceSettings2Model>(mydelete);

            string myinsert = System.Web.HttpContext.Current.Request.Params["myinsert"];
            var myinsertinfo = DataConverterHelper.JsonToEntity<ExpenseCategoryModel>(myinsert);


            var result = CorrespondenceSettings2Service.UpdateZCLB(OrgCode, OrgPhId, mydeleteinfo.AllRow, myinsertinfo.AllRow);
            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 判断是否是末级组织（获取所有的父类信息）
        /// </summary>
        /// <returns></returns>
        public string JudgeIsend()
        {
            //string ParentOrg = System.Web.HttpContext.Current.Request.Params["ParentOrg"];
            //var result = CorrespondenceSettings2Service.LoadWithPageIsend();
            var dicWhere = new Dictionary<string, object>();
            var result = OrgRelatitem2Service.ServiceHelper.LoadWithPageInfinity("GQT.QT.GetAllorgrelatitem", dicWhere);
            return DataConverterHelper.EntityListToJson<OrgRelatitem2Model>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 保存项目类型对应项目预算明细显示格式设置
        /// </summary>
        /// <returns></returns>
        public string UpdateYSDtlGS()
        {
            string data = System.Web.HttpContext.Current.Request.Params["data"];
            var result = CorrespondenceSettings2Service.UpdateYSDtlGS(data);

            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 取数据对接配置文件
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetConnDbConfig()
        {
            var dicWhere = new Dictionary<string, object>();
            var result= CorrespondenceSettings2Service.ServiceHelper.LoadWithPageInfinity("GQT.QT.GetConnDBConfig", dicWhere);
            return DataConverterHelper.EntityListToJson<CorrespondenceSettings2Model>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 保存数据对接配置
        /// </summary>
        /// <returns></returns>
        public string SaveDbConfig()
        {
            string Models = System.Web.HttpContext.Current.Request.Params["Models"];
            var datas = JsonConvert.DeserializeObject<List<CorrespondenceSettings2Model>>(Models);
            //var datas = DataConverterHelper.JsonToEntity<CorrespondenceSettings2Model>(Models).AllRow;
            for(var i = 0; i < datas.Count; i++)
            {
                if (datas[i].PhId == 0)
                {
                    datas[i].PersistentState = PersistentState.Added;
                }
                else
                {
                    CorrespondenceSettings2Model correspondenceSettings2 = datas[i];
                    datas[i] = CorrespondenceSettings2Service.Find(datas[i].PhId).Data;
                    datas[i].DefStr2 = correspondenceSettings2.DefStr2;
                    datas[i].DefStr3 = correspondenceSettings2.DefStr3;
                    datas[i].DefStr4 = correspondenceSettings2.DefStr4;
                    datas[i].PersistentState = PersistentState.Modified;
                }
            }
            SavedResult<Int64> savedresult= CorrespondenceSettings2Service.Save<Int64>(datas,"");
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 根据按钮主键取对应关系(得到的PhId为对应关系的主键)(按钮权限)
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetIfButton()
        {
            string Dwdm = System.Web.HttpContext.Current.Request.Params["Dwdm"];//查询条件
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "button"))
                .Add(ORMRestrictions<string>.Eq("Dwdm", Dwdm));
            var result = CorrespondenceSettings2Service.ServiceHelper.LoadWithPageInfinity("GQT3.QT.DYGX2_All", dicWhere);

            return DataConverterHelper.EntityListToJson<CorrespondenceSettings2Model>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 获取没有设置权限的组织
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetIfNoButton()
        {
            string Dwdm = System.Web.HttpContext.Current.Request.Params["Dwdm"];//查询条件
            var result = CorrespondenceSettings2Service.GetOrgListNoDYGXdtl(Dwdm);

            return result;
        }

        /// <summary>
        /// 页面功能控制
        /// </summary>
        /// <returns></returns>
        public string UpdateControlSet()
        {

            string Setcode = System.Web.HttpContext.Current.Request.Params["Setcode"];
            string SetPhId = System.Web.HttpContext.Current.Request.Params["SetPhId"];

            string mydelete = System.Web.HttpContext.Current.Request.Params["mydelete"];
            var mydeleteinfo = DataConverterHelper.JsonToEntity<CorrespondenceSettings2Model>(mydelete);

            string myinsert = System.Web.HttpContext.Current.Request.Params["myinsert"];
            var myinsertinfo = DataConverterHelper.JsonToEntity<OrganizeModel>(myinsert);


            var result = CorrespondenceSettings2Service.UpdateControlSet(Setcode, SetPhId, mydeleteinfo.AllRow, myinsertinfo.AllRow);
            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 根据项目代码获取对应部门
        /// </summary>
        /// <returns></returns>
        public string GetBMListDYGXdtl()
        {
            string Dwdm = System.Web.HttpContext.Current.Request.Params["Dwdm"];
            var result = CorrespondenceSettings2Service.GetBMListDYGXdtl(Dwdm);
            return DataConverterHelper.EntityListToJson<OrganizeModel>(result, (Int32)result.Count);
        }

        /// <summary>
        /// 根据项目代码获取对应部门（没有对应关系的）
        /// </summary>
        /// <returns></returns>
        public string GetBMListNoDYGXdtl()
        {
            string Dwdm = System.Web.HttpContext.Current.Request.Params["Dwdm"];
            string OrgId = System.Web.HttpContext.Current.Request.Params["OrgId"];
            var result = CorrespondenceSettings2Service.GetBMListNoDYGXdtl(Dwdm, OrgId);
            return DataConverterHelper.EntityListToJson<OrganizeModel>(result, (Int32)result.Count);
        }

        /// <summary>
        /// 归口项目对应部门设置关系的改变
        /// </summary>
        /// <returns></returns>
        public string UpdateGKXM()
        {
            string mydelete = System.Web.HttpContext.Current.Request.Params["mydelete"];
            List<long> mydeleteinfo = JsonConvert.DeserializeObject<List<long>>(mydelete);
            string myinsert = System.Web.HttpContext.Current.Request.Params["myinsert"];
            List<CorrespondenceSettings2Model> myinsertinfo = JsonConvert.DeserializeObject<List<CorrespondenceSettings2Model>>(myinsert);


            var result = CorrespondenceSettings2Service.UpdateGKXM(mydeleteinfo, myinsertinfo);
            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 根据申报部门获取设置过对应关系的项目+项目的预算部门为该申报部门的项目
        /// </summary>
        /// <returns></returns>
        public string GetXMbySBDept()
        {
            string Dept = System.Web.HttpContext.Current.Request.Params["Dept"];
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "GKBM"))
                .Add(ORMRestrictions<string>.Eq("Dydm", Dept));
            List<string> ProjcodeList= CorrespondenceSettings2Service.Find(dicWhere).Data.ToList().Select(x => x.Dwdm).Distinct().ToList();
            Dictionary<string, object> dicWhereys = new Dictionary<string, object>();
            Dictionary<string, object> dicWhereys1 = new Dictionary<string, object>();
            Dictionary<string, object> dicWhereys2 = new Dictionary<string, object>();
            Dictionary<string, object> dicWhereys3 = new Dictionary<string, object>();
            new CreateCriteria(dicWhereys1).Add(ORMRestrictions<List<string>>.In("FProjCode", ProjcodeList));
            new CreateCriteria(dicWhereys2).Add(ORMRestrictions<string>.Eq("FYear", DateTime.Now.Year.ToString()))
                .Add(ORMRestrictions<string>.Eq("FBudgetDept", Dept))
                .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
                .Add(ORMRestrictions<System.String>.Eq("FApproveStatus", "3"))
                .Add(ORMRestrictions<String>.Eq("FMidYearChange", "0"));
            new CreateCriteria(dicWhereys3).Add(ORMRestrictions<string>.Eq("FYear", DateTime.Now.Year.ToString()))
               .Add(ORMRestrictions<string>.Eq("FDeclarationDept", Dept))
               .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
               .Add(ORMRestrictions<System.String>.Eq("FApproveStatus", "3"))
               .Add(ORMRestrictions<String>.Eq("FMidYearChange", "0"));
            new CreateCriteria(dicWhereys).Add(ORMRestrictions.Or(dicWhereys1, dicWhereys2, dicWhereys3));
            string[] Array= BudgetMstService.Find(dicWhereys).Data.ToList().Select(x => x.FProjCode).Distinct().ToArray();
            var result = JsonConvert.SerializeObject(Array);
            return result;
        }
        /// <summary>
        /// 根据操作员代码获取对应首页显示设置
        /// </summary>
        /// <returns></returns>
        public string GetIndexSet()
        {
            string UserCode = System.Web.HttpContext.Current.Request.Params["UserCode"];
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "IndexSet"))
                .Add(ORMRestrictions<string>.Eq("Dwdm", UserCode));
            var result = CorrespondenceSettings2Service.Find(dicWhere).Data;
            if (result.Count > 0)
            {
                
            }
            else
            {
                CorrespondenceSettings2Model model = new CorrespondenceSettings2Model();
                model.DefStr1 = "0";
                model.DefStr2 = "0";
                model.DefStr3 = "0";
                model.DefStr4 = "0";
                result.Add(model);
            }
            return DataConverterHelper.EntityListToJson<CorrespondenceSettings2Model>(result, (Int32)result.Count);
            //return DataConverterHelper.ResponseResultToJson(result[0]);

        }

        /// <summary>
        /// 根据操作员代码获取对应首页显示设置
        /// </summary>
        /// <returns></returns>
        public string GetIndexSet2()
        {
            string UserCode = System.Web.HttpContext.Current.Request.Params["UserCode"];
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "IndexSet"))
                .Add(ORMRestrictions<string>.Eq("Dwdm", UserCode));
            var result = CorrespondenceSettings2Service.Find(dicWhere).Data;
            if (result.Count > 0)
            {

            }
            else
            {
                CorrespondenceSettings2Model model = new CorrespondenceSettings2Model();
                model.DefStr1 = "0";
                model.DefStr2 = "0";
                model.DefStr3 = "0";
                model.DefStr4 = "0";
                result.Add(model);
            }
            return JsonConvert.SerializeObject(result[0]);
            //return DataConverterHelper.ResponseResultToJson(result[0]);

        }

        /// <summary>
        /// 保存首页显示设置
        /// </summary>
        /// <returns></returns>
        public string SaveIndexSet()
        {
            string UserCode = System.Web.HttpContext.Current.Request.Params["UserCode"];
            string data = System.Web.HttpContext.Current.Request.Params["data"];
            var datainfo = JsonConvert.DeserializeObject<CorrespondenceSettings2Model>(data);
            if (datainfo.PhId == 0)
            {
                datainfo.Dwdm = UserCode;
                datainfo.Dylx = "IndexSet";
                datainfo.PersistentState = PersistentState.Added;
                var result = CorrespondenceSettings2Service.Save<Int64>(datainfo, "");
                return DataConverterHelper.SerializeObject(result);
            }
            else
            {
                var Model2 = CorrespondenceSettings2Service.Find(datainfo.PhId).Data;
                Model2.DefStr1 = datainfo.DefStr1;
                Model2.DefStr2 = datainfo.DefStr2;
                Model2.DefStr3 = datainfo.DefStr3;
                Model2.DefStr4 = datainfo.DefStr4;
                Model2.PersistentState = PersistentState.Modified;
                var result = CorrespondenceSettings2Service.Save<Int64>(Model2, "");
                return DataConverterHelper.SerializeObject(result);
            }
            
        }

        /// <summary>
        /// 保存用户设置的当前年度
        /// </summary>
        /// <returns></returns>
        public string SaveUserYear()
        {
            string UserCode = System.Web.HttpContext.Current.Request.Params["UserCode"];
            string FYear = System.Web.HttpContext.Current.Request.Params["FYear"];


            var dicWhere = new Dictionary<string, object>();

            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "YEAR"))
                .Add(ORMRestrictions<string>.Eq("Dwdm", UserCode));
            var Model2 = CorrespondenceSettings2Service.Find(dicWhere).Data;

            if(Model2.Count > 0)
            {
                Model2[0].Dydm = FYear;
                Model2[0].PersistentState = PersistentState.Modified;
                var result = CorrespondenceSettings2Service.Save<Int64>(Model2[0], "");
                return DataConverterHelper.SerializeObject(result);
            }
            else
            {
                CorrespondenceSettings2Model model = new CorrespondenceSettings2Model();
                model.Dydm = FYear;
                model.Dylx = "YEAR";
                model.Dwdm = UserCode;
                model.PersistentState = PersistentState.Added;
                var result = CorrespondenceSettings2Service.Save<Int64>(model, "");
                return DataConverterHelper.SerializeObject(result);
            }

        }


        /// <summary>
        /// 查找用户设置的年度
        /// </summary>
        /// <returns></returns>
        public string FindUserYear()
        {
            string UserCode = System.Web.HttpContext.Current.Request.Params["UserCode"];


            var dicWhere = new Dictionary<string, object>();

            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "YEAR"))
                .Add(ORMRestrictions<string>.Eq("Dwdm", UserCode));
            var Model2 = CorrespondenceSettings2Service.Find(dicWhere).Data;

            SavedResult<Int64> savedresult = new SavedResult<Int64>();

            savedresult.Status = ResponseStatus.Success;
            savedresult.Msg = "";
            if (Model2.Count > 0)
            {
                savedresult.Msg = Model2[0].Dydm;
            }

            return DataConverterHelper.SerializeObject(savedresult);

        }

        /// <summary>
        /// 根据操作员取申报单位
        /// </summary>
        /// <returns></returns>
        public string GetSBUnit()
        {
            long uid = long.Parse(System.Web.HttpContext.Current.Request.Params["uid"]);
            IList<OrganizeModel> organizes = CorrespondenceSettings2Service.GetSBUnit(uid);
            return DataConverterHelper.EntityListToJson<OrganizeModel>(organizes, (Int32)organizes.Count);
        }

        /// <summary>
        /// 根据组织取绩效得分对应绩效结论设置列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetJXJLset()
        {
            string Dwdm = System.Web.HttpContext.Current.Request.Params["Dwdm"];
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();//查询条件转Dictionary
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dwdm", Dwdm))
                .Add(ORMRestrictions<string>.Eq("Dylx", "JXJL"));
            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = CorrespondenceSettings2Service.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere,new string[] { "DefStr1" });

            return DataConverterHelper.EntityListToJson<CorrespondenceSettings2Model>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 保存绩效得分对应绩效结论设置列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string SaveJXJLset()
        {
            string updatedata = System.Web.HttpContext.Current.Request.Params["updatedata"];
            string deletedata = System.Web.HttpContext.Current.Request.Params["deletedata"];
            var updateinfo = JsonConvert.DeserializeObject<List<CorrespondenceSettings2Model>>(updatedata);
            var deleteinfo = JsonConvert.DeserializeObject<List<string>>(deletedata);

            SavedResult<Int64> savedresult = new SavedResult<Int64>();

            savedresult = CorrespondenceSettings2Service.SaveJXJLset( updateinfo, deleteinfo);
            return DataConverterHelper.SerializeObject(savedresult);
        }
    }
}

