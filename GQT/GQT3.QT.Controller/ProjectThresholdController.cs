#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectThresholdController
    * 命名空间：        GQT3.QT.Controller
    * 文 件 名：        ProjectThresholdController.cs
    * 创建时间：        2018/10/17 
    * 作    者：        李长敏琛    
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
using System.Linq;
using Enterprise3.Common.Base.Criterion;

namespace GQT3.QT.Controller
{
	/// <summary>
	/// ProjectThreshold控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ProjectThresholdController : AFCommonController
    {
        IProjectThresholdService ProjectThresholdService { get; set; }
        ICorrespondenceSettingsService CorrespondenceSettingsService { get; set; }
        ICorrespondenceSettings2Service CorrespondenceSettings2Service { get; set; }
        IExpenseCategoryService ExpenseCategoryService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProjectThresholdController()
	    {
	        ProjectThresholdService = base.GetObject<IProjectThresholdService>("GQT3.QT.Service.ProjectThreshold");
            CorrespondenceSettingsService = base.GetObject<ICorrespondenceSettingsService>("GQT3.QT.Service.CorrespondenceSettings");
            ExpenseCategoryService = base.GetObject<IExpenseCategoryService>("GQT3.QT.Service.ExpenseCategory");
            //等GXM错误修复就可以用了
            CorrespondenceSettings2Service = base.GetObject<ICorrespondenceSettings2Service>("GQT3.QT.Service.CorrespondenceSettings2");
        }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult ProjectThresholdList()
        {
			ViewBag.Title = base.GetMenuLanguage("ProjectThreshold");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "绩效项目阈值";
            }
            base.InitialMultiLanguage("ProjectThreshold");
            ViewBag.IndividualInfo = this.GetIndividualUI("ProjectThreshold");
            return View("ProjectThresholdList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult ProjectThresholdEdit()
        {
			var tabTitle = base.GetMenuLanguage("ProjectThreshold");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "绩效项目阈值";
            }
            base.SetUserDefScriptUrl("ProjectThreshold");
            base.InitialMultiLanguage("ProjectThreshold");
            ViewBag.IndividualInfo = this.GetIndividualUI("ProjectThreshold");

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

            return View("ProjectThresholdEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetProjectThresholdList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = ProjectThresholdService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<ProjectThresholdModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回对象集合</returns>
        public IList<ProjectThresholdModel> GetPTMList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary
            new CreateCriteria(dicWhere).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = ProjectThresholdService.Find(dicWhere).Data;
            //ProjectThresholdService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return result;
        }




        /// <summary>
        /// 根据申报单位组装数据发送给前台
        /// </summary>
        /// <returns>返回Json串</returns>
        /**
         * 需要借用orange组件
         * 
         * 
         * */
        public string GetProjectThresholdListByUserPower()
        {
            DataStoreParam dataStoreParam = this.GetDataStoreParam();
            var result = CorrespondenceSettingsService.LoadWithPageOrg(dataStoreParam);
            IList<OrganizeModel> organizes=result.Results as List<OrganizeModel>;
            //Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic("");//查询条件转Dictionary
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
            IList <CorrespondenceSettings2Model> correspondenceSettings2s = CorrespondenceSettings2Service.Find(dicWhere).Data as List<CorrespondenceSettings2Model>;
            organizes = ProjectThresholdService.GetSBOrganizes(organizes, correspondenceSettings2s);
            IList<ProjectThresholdModel> projectThresholds= ProjectThresholdService.Find(dicWhere).Data as List<ProjectThresholdModel>;

            IList<VProjectThresholdModel> vprojectThresholds = new List<VProjectThresholdModel>();
            foreach (OrganizeModel organize in organizes) {
                var q1 = from dt1 in projectThresholds
                         where dt1.Orgid==organize.PhId
                         select dt1;
                VProjectThresholdModel pt1 = new VProjectThresholdModel();
                if (q1.Count() == 0) {                   
                    pt1.Orgcode = organize.OCode;
                    pt1.Orgid = organize.PhId;                    
                    pt1.FThreshold = "未设置";
                }
                if (q1.Count() == 1) {
                    var pm = q1.ToList()[0];
                    pt1.PhId= pm.PhId;
                    pt1.Orgid = pm.Orgid;
                    pt1.Orgcode = pm.Orgcode;
                    pt1.FThreshold = pm.FThreshold;
                    pt1.ProjTypeId = pm.ProjTypeId;
                    pt1.ProjTypeName = pm.ProjTypeName;
                }


                pt1.Orgname = organize.OName;
                vprojectThresholds.Add(pt1);
            }

            return DataConverterHelper.EntityListToJson<VProjectThresholdModel>(vprojectThresholds, (Int32)vprojectThresholds.Count());


            //return DataConverterHelper.EntityListToJson<ProjectThresholdModel>(result.Results, (Int32)result.TotalItems);
        }

        /**
         * 根据组织代码获取所有的支出类型
         * **/

        public IList<ExpenseCategoryModel> GetAllExpenseCategoryModel(String ocode) {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary
            new CreateCriteria(dicWhere).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
            DataStoreParam storeparam = this.GetDataStoreParam();
            //var result = ExpenseCategoryService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);
            var result = ExpenseCategoryService.Find(dicWhere).Data;

            IList<ExpenseCategoryModel> expenseCategoryModels = result;
            IList<CorrespondenceSettings2Model> cr2s = this.GetAllCR2(ocode);
            IList<String> ecmCodes = new List<String>();
            foreach (CorrespondenceSettings2Model cr2 in cr2s)
            {
                ecmCodes.Add(cr2.Dydm);
            }
            IList<ExpenseCategoryModel> expenseCategoryModelsByOcode = (from et1 in expenseCategoryModels
                                                                        where ecmCodes.Contains(et1.Dm)
                                                                        select et1).ToList<ExpenseCategoryModel>();
            return expenseCategoryModelsByOcode;
        }

        /**
         * 根据组织代码获取所有的对应关系
         * **/
        public IList<CorrespondenceSettings2Model> GetAllCR2(String ocode)
        {
            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "08"));
            if (ocode != null)
            {
                new CreateCriteria(dicWhere).
                        Add(ORMRestrictions<string>.Eq("DefStr1", ocode));
            }

            DataStoreParam storeparam = this.GetDataStoreParam();
            //var result = CorrespondenceSettings2Service.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);
            var result = CorrespondenceSettings2Service.ServiceHelper.LoadWithPageInfinity("GQT3.QT.DYGX2_All", dicWhere);
            return result.Results;
        }

        /**
        * 根据组织ID获取，该组织下选择了哪些项目类型（支出类别）设置了阈值，也是一个支出类型（项目类型）的集合
        * **/
        public String GetAlreadyExpenseCategoryModel(String Ocode)
        {
            IList<ExpenseCategoryModel> eMresult = new List<ExpenseCategoryModel>();
            IList<ExpenseCategoryModel> expenseCategoryModels = this.GetAllExpenseCategoryModel(Ocode);
            IList<ProjectThresholdModel> projectThresholdModels = this.GetPTMList();
            IList<ProjectThresholdModel> pt = (from pt1 in projectThresholdModels
                                            where pt1.Orgcode.Equals(Ocode)
                                            select pt1).ToList<ProjectThresholdModel>();
            if (pt.Count == 1) {
                IList<String> alcodes = (pt[0].ProjTypeId??"").Split(',').ToList<String>();

                eMresult = (from ex1 in expenseCategoryModels
                            where alcodes.Contains(ex1.Dm)
                            select ex1).ToList<ExpenseCategoryModel>();
            }


            return DataConverterHelper.EntityListToJson<ExpenseCategoryModel>(eMresult, (Int32)eMresult.Count);
        }

        /**
        * 根据组织ID获取，该组织下选择了哪些项目类型（支出类别）设置了阈值，也是一个支出类型（项目类型）的集合
        * **/
        public String GetUnSetExpenseCategoryModel(String Ocode)
        {
            IList<ExpenseCategoryModel> eMresult = new List<ExpenseCategoryModel>();
            IList<ExpenseCategoryModel> expenseCategoryModels = this.GetAllExpenseCategoryModel(Ocode);
            IList<ProjectThresholdModel> projectThresholdModels = this.GetPTMList();
            IList<ProjectThresholdModel> pt = (from pt1 in projectThresholdModels
                                               where pt1.Orgcode.Equals(Ocode)
                                               select pt1).ToList<ProjectThresholdModel>();
            if (pt.Count == 1)
            {
                IList<String> alcodes = (pt[0].ProjTypeId ?? "").Split(',').ToList<String>();
                eMresult = (from ex1 in expenseCategoryModels
                            where !(alcodes.Contains(ex1.Dm))
                            select ex1).ToList<ExpenseCategoryModel>();
            }
            else {
                eMresult = expenseCategoryModels;
            }
            



            return DataConverterHelper.EntityListToJson<ExpenseCategoryModel>(eMresult, (Int32)eMresult.Count);
        }



        public ActionResult SaveOrUpdate(String data) {
            DataStoreParam dataStoreParam = this.GetDataStoreParam();
            var result = CorrespondenceSettingsService.LoadWithPageOrg(dataStoreParam);
            //Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic("");
            IList<OrganizeModel> organizes = result.Results as List<OrganizeModel>;

            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
            IList<CorrespondenceSettings2Model> correspondenceSettings2s = CorrespondenceSettings2Service.Find(dicWhere).Data as List<CorrespondenceSettings2Model>;

            //IList<CorrespondenceSettings2Model> correspondenceSettings2s = CorrespondenceSettings2Service.LoadWithPage(dataStoreParam.PageIndex, dataStoreParam.PageSize, dicWhere).Results as List<CorrespondenceSettings2Model>;
            organizes = ProjectThresholdService.GetSBOrganizes(organizes, correspondenceSettings2s);
            //IList<ProjectThresholdModel> projectThresholds = (ProjectThresholdService.LoadWithPage(dataStoreParam.PageIndex, dataStoreParam.PageSize, dicWhere)).Results as List<ProjectThresholdModel>;
            IList<ProjectThresholdModel> projectThresholds = ProjectThresholdService.Find(dicWhere).Data as List<ProjectThresholdModel>;
            if (this.ProjectThresholdService.SaveOrUpdate(data, organizes, projectThresholds, dataStoreParam))
            {
                return Content("{\"result\":\"success\"}");
            }
            else {
                return Content("{\"result\":\"fail\"}");
            }
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetProjectThresholdInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = ProjectThresholdService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }


        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetProjectThresholdInfoToOrgcode()
        {
            string orgcode = System.Web.HttpContext.Current.Request.Params["orgcode"];  //组织号
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("Orgcode", orgcode));


            var findedresult = ProjectThresholdService.Find(dicWhere);
            return DataConverterHelper.ResponseResultToJson(findedresult);
        }


        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string projectthresholdformData = System.Web.HttpContext.Current.Request.Form["projectthresholdformData"];

			var projectthresholdforminfo = DataConverterHelper.JsonToEntity<ProjectThresholdModel>(projectthresholdformData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = ProjectThresholdService.Save<Int64>(projectthresholdforminfo.AllRow,"");
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

            var deletedresult = ProjectThresholdService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 根据组织取数
        /// </summary>
        /// <returns></returns>
        public string GetProjectThresholdListByOrg()
        {
            string Orgcode = System.Web.HttpContext.Current.Request.Params["Orgcode"];
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(Orgcode))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Orgcode", Orgcode));
            }
            else
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
            }
            IList<ProjectThresholdModel> projectThresholds = ProjectThresholdService.Find(dicWhere).Data;
            return DataConverterHelper.EntityListToJson<ProjectThresholdModel>(projectThresholds, (Int32)projectThresholds.Count());
        }

        /// <summary>
        /// 左边的可选项目类型
        /// </summary>
        /// <returns></returns>
        public String GetLeftExpenseCategoryModel()
        {
            string Orgcode = System.Web.HttpContext.Current.Request.Params["Orgcode"];
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Orgcode", Orgcode));
            List<ExpenseCategoryModel> ALLData = this.GetAllExpenseCategoryModel(Orgcode).ToList();
            if (ALLData.Count > 0)
            {
                List<String> useCodeList = new List<string>();
                IList<ProjectThresholdModel> projectThresholdModels = ProjectThresholdService.Find(dicWhere).Data;
                if (projectThresholdModels.Count > 0)
                {
                    foreach (var projectThresholdModel in projectThresholdModels)
                    {
                        useCodeList = useCodeList.Union((projectThresholdModel.ProjTypeId ?? "").Split(',').ToList()).ToList();
                    }
                }
                ALLData = ALLData.Where(x => !useCodeList.Contains(x.Dm)).ToList();
                ALLData.Sort((ExpenseCategoryModel a, ExpenseCategoryModel b) => a.Dm.CompareTo(b.Dm));
            }
            return DataConverterHelper.EntityListToJson<ExpenseCategoryModel>(ALLData, (Int32)ALLData.Count);
        }

        /// <summary>
        /// 右边的已选项目类型
        /// </summary>
        /// <returns></returns>
        public String GetRightExpenseCategoryModel()
        {
            string Orgcode = System.Web.HttpContext.Current.Request.Params["Orgcode"];
            string PhId = System.Web.HttpContext.Current.Request.Params["PhId"];
            List<ExpenseCategoryModel> expenseCategoryModels = new List<ExpenseCategoryModel>();
            if (!string.IsNullOrEmpty(PhId))
            {
                ProjectThresholdModel projectThreshold = ProjectThresholdService.Find(long.Parse(PhId)).Data;
                expenseCategoryModels = this.GetAllExpenseCategoryModel(Orgcode).ToList();
                IList<String> useCodeList = (projectThreshold.ProjTypeId ?? "").Split(',').ToList<String>();
                expenseCategoryModels= expenseCategoryModels.Where(x => useCodeList.Contains(x.Dm)).ToList();
                expenseCategoryModels.Sort((ExpenseCategoryModel a, ExpenseCategoryModel b) => a.Dm.CompareTo(b.Dm));
            }

            return DataConverterHelper.EntityListToJson<ExpenseCategoryModel>(expenseCategoryModels, (Int32)expenseCategoryModels.Count);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public string Save2()
        {
            string Orgcode = System.Web.HttpContext.Current.Request.Params["Orgcode"];
            string data = System.Web.HttpContext.Current.Request.Params["data"];

            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Orgcode", Orgcode));
            IList<ProjectThresholdModel> projectThresholdModels = ProjectThresholdService.Find(dicWhere).Data;
            List<long> phidList = projectThresholdModels.ToList().Select(x => x.PhId).ToList();
            List<ProjectThresholdModel> SaveData = new List<ProjectThresholdModel>();
            if (data.EndsWith("|"))
            {
                data = data.Substring(0, data.Length - 1);
            }
            String[] Items = data.Split('|');
            foreach (String item in Items)
            {
                //取到item，即序列化对象
                String[] attrs = item.Split(':');
                if (string.IsNullOrEmpty(attrs[0]) || attrs[0]=="0")
                {
                    var p1 = new ProjectThresholdModel();
                    p1.Orgcode = attrs[1];
                    p1.FThreshold = attrs[2];
                    p1.ProjTypeId = attrs[3];
                    p1.ProjTypeName = attrs[4];
                    p1.Orgid = long.Parse(attrs[5]);
                    p1.PersistentState = PersistentState.Added;
                    SaveData.Add(p1);
                }
                else
                {
                    if (phidList.Contains(long.Parse(attrs[0])))
                    {
                        phidList.Remove(long.Parse(attrs[0]));
                    }
                    var p1 = projectThresholdModels.ToList().Find(x => x.PhId == long.Parse(attrs[0]));
                    p1.FThreshold = attrs[2];
                    p1.ProjTypeId = attrs[3];
                    p1.ProjTypeName = attrs[4];
                    p1.PersistentState = PersistentState.Modified;
                    SaveData.Add(p1);
                }
            }
            if (phidList.Count > 0)
            {
                foreach(var phid in phidList)
                {
                    var p1 = projectThresholdModels.ToList().Find(x => x.PhId == phid);
                    p1.PersistentState = PersistentState.Deleted;
                    SaveData.Add(p1);
                }
            }

            SavedResult<Int64> savedresult = ProjectThresholdService.Save<Int64>(SaveData, "");
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 根据组织和项目类型取阈值
        /// </summary>
        /// <returns></returns>
        public string GetProjectThresholdByOrgAndZCLB()
        {
            string Orgcode = System.Web.HttpContext.Current.Request.Params["Orgcode"];
            string FExpenseCategoryCode = System.Web.HttpContext.Current.Request.Params["FExpenseCategoryCode"];
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Orgcode", Orgcode));
            IList<ProjectThresholdModel> projectThresholds = ProjectThresholdService.Find(dicWhere).Data;
            if (projectThresholds.Count > 0)
            {
                foreach (var projectThreshold in projectThresholds)
                {
                    IList<String> useCodeList = (projectThreshold.ProjTypeId ?? "").Split(',').ToList<String>();
                    if (useCodeList.Contains(FExpenseCategoryCode))
                    {
                        return DataConverterHelper.ResponseResultToJson(projectThreshold);
                    }
                }
            }
            return DataConverterHelper.ResponseResultToJson(new ProjectThresholdModel());
        }
    }
}

