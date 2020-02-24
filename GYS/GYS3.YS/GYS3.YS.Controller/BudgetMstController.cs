#region Summary
/**************************************************************************************
    * 类 名 称：        BudgetMstController
    * 命名空间：        GYS3.YS.Controller
    * 文 件 名：        BudgetMstController.cs
    * 创建时间：        2018/8/30 
    * 作    者：        董泉伟    
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

using Enterprise3.Common.Base.Criterion;

using GYS3.YS.Service.Interface;
using GYS3.YS.Model.Domain;
using GXM3.XM.Service.Interface;
using GQT3.QT.Service.Interface;
using GQT3.QT.Model.Domain;
using GXM3.XM.Model.Domain;

using System.Reflection;
using System.Linq;
using GYS3.YS.Model.Enums;
using GJX3.JX.Service.Interface;
using GYS3.YS.Model.Domain.GenerateCodes;
using GYS3.YS.Model.Extra;
using Enterprise3.WebApi.GYS3.YS.Model.Request;
using Enterprise3.WebApi.GYS3.YS.Model.Response;

namespace GYS3.YS.Controller
{
    /// <summary>
    /// BudgetMst控制处理类
    /// </summary>
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class BudgetMstController : AFCommonController
    {
        IBudgetMstService BudgetMstService { get; set; }
        IProjectMstService ProjectMstService { get; set; }

        ICorrespondenceSettingsService CorrespondenceSettingsService { get; set; }

        IBudgetProcessCtrlService BudgetProcessCtrlService { get; set; }
        IQTIndividualInfoService QTIndividualInfoService { get; set; }

        ICorrespondenceSettings2Service CorrespondenceSettings2Service { get; set; }
        IQTProjectMstService QTProjectMstService { get; set; }
        IQTEditMemoService QTEditMemoService { get; set; }
        IPerformanceMstService PerformanceMstService { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public BudgetMstController()
        {
            BudgetMstService = base.GetObject<IBudgetMstService>("GYS3.YS.Service.BudgetMst");
            ProjectMstService = base.GetObject<IProjectMstService>("GXM3.XM.Service.ProjectMst");
            BudgetProcessCtrlService = base.GetObject<IBudgetProcessCtrlService>("GYS3.YS.Service.BudgetProcessCtrl");
            CorrespondenceSettingsService = base.GetObject<ICorrespondenceSettingsService>("GQT3.QT.Service.CorrespondenceSettings");
            QTIndividualInfoService = base.GetObject<IQTIndividualInfoService>("GQT3.QT.Service.QTIndividualInfo");
            CorrespondenceSettings2Service = base.GetObject<ICorrespondenceSettings2Service>("GQT3.QT.Service.CorrespondenceSettings2");
            QTProjectMstService = base.GetObject<IQTProjectMstService>("GQT3.QT.Service.QTProjectMst");
            QTEditMemoService = base.GetObject<IQTEditMemoService>("GQT3.QT.Service.QTEditMemo");
            PerformanceMstService = base.GetObject<IPerformanceMstService>("GJX3.JX.Service.PerformanceMst");
        }

        /// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult BudgetMstList()
        {
            ViewBag.Title = base.GetMenuLanguage("GHBudgetMst");//根据业务类型对应的langkey取多语言
            var workType = System.Web.HttpContext.Current.Request.Params["workType"];//业务类型
            ViewBag.FApproveStatus = System.Web.HttpContext.Current.Request.Params["FApproveStatus"]; //是否待上报页面
            // c - 年初,z - 年中,x - 专项

            ViewBag.workType = workType;
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                if (workType == "z") //年中
                {
                    ViewBag.Title = "年中预算调整";
                }
                else if (workType == "x") //专项
                {
                    ViewBag.Title = "特殊预算专项";
                }
                else if (workType == "c")
                {
                    ViewBag.Title = "年初预算申报";
                }
            }
            base.InitialMultiLanguage("GHProjectBegin");
            ViewBag.IndividualInfo = this.GetIndividualUI("GHProjectBegin");
            //base.InitialMultiLanguage("GHBudgetMst");
            //ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetMst");


            return View("BudgetMstList");
        }

        /// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult BudgetMstEdit()
        {
            var tabTitle = base.GetMenuLanguage("GHBudgetMst");//根据业务类型对应的langkey取多语言

            var workType = System.Web.HttpContext.Current.Request.Params["workType"];//业务类型
            var midYearEdit = System.Web.HttpContext.Current.Request.Params["midYearEdit"]; // 年中调整
            var IndividualinfoId = System.Web.HttpContext.Current.Request.Params["IndividualinfoId"];//自定义界面ID
            ViewBag.memoRight = System.Web.HttpContext.Current.Request.Params["memoRight"];//是不是批注
            ViewBag.changeIndividualinfoId = System.Web.HttpContext.Current.Request.Params["changeIndividualinfoId"];//否是自动切换模板

            ViewBag.midYearEdit = midYearEdit;
            ViewBag.workType = workType;
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                if (workType == "z") //年中
                {
                    tabTitle = "年中预算调整";
                }
                else if (workType == "x") //专项
                {
                    tabTitle = "特殊预算专项";
                }
                else if (workType == "c")
                {
                    tabTitle = "年初预算申报";
                }

            }



            if (string.IsNullOrEmpty(IndividualinfoId))
            {
                ViewBag.IndividualInfo = this.GetIndividualUI("GHProjectBegin");
            }
            else
            {
                //var IndividualInfoData = QTIndividualInfoService.Find(Convert.ToInt64(IndividualinfoId)).Data;
                ViewBag.IndividualinfoId = IndividualinfoId;

                ViewBag.IndividualInfo = this.GetIndividualUI("GHProjectBegin", Convert.ToInt64(IndividualinfoId));
            }

            base.SetUserDefScriptUrl("GHProjectBegin");
            base.InitialMultiLanguage("GHProjectBegin");
            //ViewBag.IndividualInfo = this.GetIndividualUI("GHProjectBegin");
            //base.SetUserDefScriptUrl("GHBudgetMst");
            //base.InitialMultiLanguage("GHBudgetMst");
            //ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetMst");

            ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型


            if (ViewBag.OType == "add")
            {
                //新增时
                //if (BudgetMstService.Has_BillNoRule("单据规则类型") == true)
                //{
                //    var vBillNo = BudgetMstService.GetBillNo();//取用户编码,新增时界面上显示
                //    ViewBag.No = vBillNo.BillNoList[0];
                //}
                ViewBag.Title = tabTitle + "-新增";
            }
            else if (ViewBag.OType == "edit")
            {

            }
            else if (ViewBag.OType == "view")
            {
                ViewBag.Title = tabTitle + "-查看";
            }
            else if (ViewBag.memoRight == "memoRight")
            {
                ViewBag.Title = tabTitle + "-批注";
            }

            return View("BudgetMstEdit");
        }

        /// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult YsTable()
        {
            ViewBag.Title = "项目预算调整分析表";


            return View("YsTable");
        }


        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetBudgetMstList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            if (dicWhere.ContainsKey("PZ*str*eq*1"))
            {
                Dictionary<string, object> dicEditMemo = new Dictionary<string, object>();
                new CreateCriteria(dicEditMemo).Add(ORMRestrictions<long>.NotEq("Memophid", 0));
                var Memophids = QTEditMemoService.Find(dicEditMemo).Data.ToList().Select(x => x.Memophid).Distinct().ToList();
                if (dicWhere["PZ*str*eq*1"].ToString() == "1")
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<List<long>>.In("PhId", Memophids));
                }
                else if (dicWhere["PZ*str*eq*1"].ToString() == "2")
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<List<long>>.NotIn("PhId", Memophids));
                }
                dicWhere.Remove("PZ*str*eq*1");
            }
            //支出类型为基本支出时
            if (dicWhere.ContainsKey("FZcType*str*eq*1") && dicWhere["FZcType*str*eq*1"].ToString() == "4")
            {
                dicWhere.Remove("FZcType*str*eq*1");
                new CreateCriteria(dicWhere).Add(ORMRestrictions<List<string>>.In("FZcType", new List<string>() { "2", "3" }));
            }
            var workType = System.Web.HttpContext.Current.Request.Params["workType"]; //业务种类(年初,年中,特殊)
            var FApproveStatus = System.Web.HttpContext.Current.Request.Params["FApproveStatus"];

            //增加年度过滤
            var FYear = System.Web.HttpContext.Current.Request.Params["FYear"];

            //增加根据操作员对应预算部门的过滤
            var userId = System.Web.HttpContext.Current.Request.Params["userId"];
            var showAll = System.Web.HttpContext.Current.Request.Params["showAll"];
            var ShowTZ = System.Web.HttpContext.Current.Request.Params["ShowTZ"];

            var FDeclarationUnit = System.Web.HttpContext.Current.Request.Params["FDeclarationUnit"];
            if (showAll == "1")
            {

                var dicWhereDept = new Dictionary<string, object>();
                new CreateCriteria(dicWhereDept)
                    .Add(ORMRestrictions<string>.Eq("Dwdm", userId)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
                var deptList = CorrespondenceSettingsService.Find(dicWhereDept);
                List<string> deptL = new List<string>();
                for (var i = 0; i < deptList.Data.Count; i++)
                {
                    deptL.Add(deptList.Data[i].Dydm);
                }
                new CreateCriteria(dicWhere).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
            }
            else
            {
                //取默认申报部门
                var dicDefaultDept = new Dictionary<string, object>();
                new CreateCriteria(dicDefaultDept).Add(ORMRestrictions<string>.Eq("Dylx", "08"))
                    .Add(ORMRestrictions<Int32>.Eq("DefInt1", 1)).Add(ORMRestrictions<string>.Eq("Dwdm", userId));
                var dygx1 = CorrespondenceSettingsService.Find(dicDefaultDept).Data;
                if (dygx1.Count > 0)
                {
                    var DefaultDept = dygx1[0].DefStr3;
                    Dictionary<string, object> dicWhere4 = new Dictionary<string, object>();
                    new CreateCriteria(dicWhere4).Add(ORMRestrictions<string>.Eq("Dylx", "GKBM"))
                        .Add(ORMRestrictions<string>.Eq("Dydm", DefaultDept));
                    List<string> ProjcodeList = CorrespondenceSettings2Service.Find(dicWhere4).Data.ToList().Select(x => x.Dwdm).Distinct().ToList();

                    Dictionary<string, object> dicWhereys1 = new Dictionary<string, object>();
                    Dictionary<string, object> dicWhereys2 = new Dictionary<string, object>();
                    Dictionary<string, object> dicWhereys3 = new Dictionary<string, object>();
                    new CreateCriteria(dicWhereys1).Add(ORMRestrictions<List<string>>.In("FProjCode", ProjcodeList));
                    new CreateCriteria(dicWhereys2).Add(ORMRestrictions<string>.Eq("FBudgetDept", DefaultDept));
                    new CreateCriteria(dicWhereys3).Add(ORMRestrictions<string>.Eq("FDeclarationDept", DefaultDept));
                    new CreateCriteria(dicWhere).Add(ORMRestrictions.Or(dicWhereys1, dicWhereys2, dicWhereys3));

                }
                else
                {
                    return DataConverterHelper.EntityListToJson<BudgetMstModel>(null, 0);
                }
            }



            // c - 年初,z - 年中,x - 专项
            if (workType == "z") //年中
            {
                //年中时可以调整年初预算单据
                var dicWhere1 = new Dictionary<string, object>(); //年中调整新增
                var dicWhere2 = new Dictionary<string, object>(); //年初预算新增后调整
                var dicWhere3 = new Dictionary<string, object>(); //年初新增
                new CreateCriteria(dicWhere1).Add(ORMRestrictions<string>.Eq("FType", "z"))
                    .Add(ORMRestrictions<string>.Eq("FVerNo", "0002")).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
                    .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));
                new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FType", "c"))
                    .Add(ORMRestrictions<string>.Eq("FVerNo", "0002")).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
                    .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0")); //年初没调整过的单据
                new CreateCriteria(dicWhere3).Add(ORMRestrictions<string>.Eq("FType", "c"))
                    .Add(ORMRestrictions<string>.Eq("FVerNo", "0001")).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
                    .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));

                new CreateCriteria(dicWhere).Add(ORMRestrictions.Or(dicWhere1, dicWhere2, dicWhere3));
                //new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FType", "z"));


            }
            else if (workType == "x") //专项
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FType", "x"))
                    .Add(ORMRestrictions<string>.Eq("FVerNo", "0001")).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0));
            }
            else if (workType == "c")//年初
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FType", "c"))
                    .Add(ORMRestrictions<string>.Eq("FVerNo", "0001"));//.Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)) 要同时显示被年中调整的原单据
            }
            else
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
                    .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));
            }


            if (FApproveStatus == "1" && (clientJsonQuery == null || clientJsonQuery.IndexOf("FApproveStatus") == -1))
            {

                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FApproveStatus", "1"));

            }

            //增加年度过滤条件
            //if (clientJsonQuery.IndexOf("FYear") == -1)
            //{
            //    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", FYear));
            //}
            //if (!dicWhere.ContainsKey("FYear*str*eq*1") || !string.IsNullOrEmpty(dicWhere["FYear*str*eq*1"].ToString()))
            //{
            //    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", FYear));
            //}
            if (!string.IsNullOrEmpty(FYear))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FGoYear", FYear));
            }
            if ((clientJsonQuery == null || clientJsonQuery.IndexOf("FDeclarationUnit") == -1) && !string.IsNullOrEmpty(FDeclarationUnit))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FDeclarationUnit", FDeclarationUnit));
            }

            DataStoreParam storeparam = this.GetDataStoreParam();

            if (ShowTZ == "1")
            {
                var result = BudgetMstService.Find(dicWhere).Data.ToList();
                for (var a = 0; a < result.Count; a++)
                {
                    result[a].FBillType = "ys";
                }
                var ProjcodeList = result.Select(x => x.FProjCode).Distinct().ToList();
                dicWhere.Remove("FMidYearChange*str*eq");
                new CreateCriteria(dicWhere).Add(ORMRestrictions<List<string>>.NotIn("FProjCode", ProjcodeList));
                var result2 = ProjectMstService.Find(dicWhere).Data.ToList();
                if (result2.Count > 0)
                {
                    for (var i = 0; i < result2.Count; i++)
                    {
                        var model = ModelChange<ProjectMstModel, BudgetMstModel>(result2[i]);
                        model.FBillType = "xm";
                        result.Add(model);
                    }
                }
                result.Sort((BudgetMstModel a, BudgetMstModel b) => a.FProjCode.CompareTo(b.FProjCode));
                var TotalItems = result.Count;
                //分页
                result = result.Skip(storeparam.PageIndex * storeparam.PageSize).Take(storeparam.PageSize).ToList();
                return DataConverterHelper.EntityListToJson<BudgetMstModel>(result, (Int32)TotalItems);
            }
            else
            {
                var result = BudgetMstService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" });

                return DataConverterHelper.EntityListToJson<BudgetMstModel>(result.Results, (Int32)result.TotalItems);
            }
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetBudgetMstInfo()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型

            switch (tabtype)
            {
                case "budgetmst":
                    var findedresultmst = BudgetMstService.Find(id);
                    return DataConverterHelper.ResponseResultToJson(findedresultmst);
                case "budgetdtlimplplan":
                    var findedresultbudgetdtlimplplan = BudgetMstService.FindBudgetDtlImplPlanByForeignKey(id);
                    return DataConverterHelper.EntityListToJson(findedresultbudgetdtlimplplan.Data, findedresultbudgetdtlimplplan.Data.Count);
                case "budgetdtltextcontent":
                    var findedresultbudgetdtltextcontent = BudgetMstService.FindBudgetDtlTextContentByForeignKey(id);

                    if (findedresultbudgetdtltextcontent != null)
                    {
                        if (findedresultbudgetdtltextcontent.Data.Count > 0)
                        {
                            BudgetDtlTextContentModel singleData = findedresultbudgetdtltextcontent.Data[0];
                            FindedResult<BudgetDtlTextContentModel> result = new FindedResult<BudgetDtlTextContentModel>(false, singleData);
                            return DataConverterHelper.ResponseResultToJson(result);
                        }
                    }

                    return DataConverterHelper.EntityListToJson(findedresultbudgetdtltextcontent.Data, findedresultbudgetdtltextcontent.Data.Count);
                case "budgetdtlfundappl":
                    var findedresultbudgetdtlfundappl = BudgetMstService.FindBudgetDtlFundApplByForeignKey(id);
                    return DataConverterHelper.EntityListToJson(findedresultbudgetdtlfundappl.Data, findedresultbudgetdtlfundappl.Data.Count);
                case "budgetdtlbudgetdtl":
                    var findedresultbudgetdtlbudgetdtl = BudgetMstService.FindBudgetDtlBudgetDtlByForeignKey(id);
                    return DataConverterHelper.EntityListToJson(findedresultbudgetdtlbudgetdtl.Data, findedresultbudgetdtlbudgetdtl.Data.Count);

                case "projectdtlpurchasedtl":
                    var findedresultsprojectdtlpurchasedtl = BudgetMstService.FindBudgetDtlPurchaseDtlByForeignKey(id);
                    return DataConverterHelper.EntityListToJson(findedresultsprojectdtlpurchasedtl.Data, findedresultsprojectdtlpurchasedtl.Data.Count);
                // return DataConverterHelper.ResponseResultToJson(findedresultsprojectdtlpurchasedtl.Data[0]);
                case "projectdtlpurdtl4sof":
                    var findedresultprojectdtlpurdtl4sof = BudgetMstService.FindBudgetDtlPurDtl4SOFByForeignKey(id);
                    return DataConverterHelper.EntityListToJson(findedresultprojectdtlpurdtl4sof.Data, findedresultprojectdtlpurdtl4sof.Data.Count);
                case "budgetdtlperformtarget":
                    var findedresultbudgetdtlperformtarget = BudgetMstService.FindBudgetDtlPerformTargetByForeignKey(id);
                    return DataConverterHelper.EntityListToJson(findedresultbudgetdtlperformtarget.Data, findedresultbudgetdtlperformtarget.Data.Count);
                case "jxtracking":
                    var findedresultjxtracking = BudgetMstService.FindJxTrackingByForeignKey(id);
                    return DataConverterHelper.EntityListToJson(findedresultjxtracking.Data, findedresultjxtracking.Data.Count);
                default:
                    FindedResult findedresultother = new FindedResult();
                    return DataConverterHelper.ResponseResultToJson(findedresultother);
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
            string mstformData = System.Web.HttpContext.Current.Request.Form["mstformData"];
            string budgetdtlimplplangridData = System.Web.HttpContext.Current.Request.Form["budgetdtlimplplangridData"];
            string budgetdtltextcontentgridData = System.Web.HttpContext.Current.Request.Form["budgetdtltextcontentgridData"];
            string budgetdtlfundapplgridData = System.Web.HttpContext.Current.Request.Form["budgetdtlfundapplgridData"];
            string budgetdtlbudgetdtlgridData = System.Web.HttpContext.Current.Request.Form["budgetdtlbudgetdtlgridData"];
            string budgetdtlperformtargetgridData = System.Web.HttpContext.Current.Request.Form["budgetdtlperformtargetgridData"];
            string projectdtlpurchasedtlformData = System.Web.HttpContext.Current.Request.Form["projectdtlpurchasedtlformData"];
            string projectdtlpurdtl4sofgridData = System.Web.HttpContext.Current.Request.Form["projectdtlpurdtl4sofgridData"];

            string midYearEdit = System.Web.HttpContext.Current.Request.Form["midYearEdit"];

            var mstforminfo = DataConverterHelper.JsonToEntity<BudgetMstModel>(mstformData);
            var budgetdtlimplplangridinfo = DataConverterHelper.JsonToEntity<BudgetDtlImplPlanModel>(budgetdtlimplplangridData);
            var budgetdtltextcontentgridinfo = DataConverterHelper.JsonToEntity<BudgetDtlTextContentModel>(budgetdtltextcontentgridData);
            var budgetdtlfundapplgridinfo = DataConverterHelper.JsonToEntity<BudgetDtlFundApplModel>(budgetdtlfundapplgridData);
            var budgetdtlbudgetdtlgridinfo = DataConverterHelper.JsonToEntity<BudgetDtlBudgetDtlModel>(budgetdtlbudgetdtlgridData);
            var budgetdtlperformtargetgridinfo = DataConverterHelper.JsonToEntity<BudgetDtlPerformTargetModel>(budgetdtlperformtargetgridData);

            EntityInfo<BudgetDtlPurchaseDtlModel> projectdtlpurchasedtlforminfo = null;
            if (projectdtlpurchasedtlformData != null)
            {
                projectdtlpurchasedtlforminfo = DataConverterHelper.JsonToEntity<BudgetDtlPurchaseDtlModel>(projectdtlpurchasedtlformData);
            }

            EntityInfo<BudgetDtlPurDtl4SOFModel> projectdtlpurdtl4sofgridinfo = null;
            if (projectdtlpurdtl4sofgridData != null)
            {
                projectdtlpurdtl4sofgridinfo = DataConverterHelper.JsonToEntity<BudgetDtlPurDtl4SOFModel>(projectdtlpurdtl4sofgridData);
            }

            var findedresultmst = new BudgetMstModel();
            var findedresultbudgetdtlimplplan = new FindedResults<BudgetDtlImplPlanModel>();
            var findedresultbudgetdtltextcontent = new FindedResults<BudgetDtlTextContentModel>();
            var findedresultbudgetdtlfundappl = new FindedResults<BudgetDtlFundApplModel>();
            var findedresultbudgetdtlbudgetdtl = new FindedResults<BudgetDtlBudgetDtlModel>();
            var findedresultbudgetdtlperformtarget = new FindedResults<BudgetDtlPerformTargetModel>();
            var findedresultbudgetdtlPurchaseDtl = new FindedResults<BudgetDtlPurchaseDtlModel>();
            var findedresultbudgetdtlPurDtl4SOF = new FindedResults<BudgetDtlPurDtl4SOFModel>();


            long id = 0;
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            try
            {

                //年中调整修改年初预算时,复制原来预算信息,f_VerNo值为”0002”，所有信息从”0001”版本拷贝过来(原”0001”的项目记录保存不变)
                if (midYearEdit == "midYearEdit")
                {
                    id = mstforminfo.AllRow[0].PhId;
                    //年中调整时,项目审批状态改为未审批,项目属性改为年中调整
                    mstforminfo.AllRow[0].FApproveStatus = "1";
                    mstforminfo.AllRow[0].FProjStatus = 4;


                    findedresultmst = BudgetMstService.Find(id).Data;
                    ////年中调整时,如果是调整已年中调整过的单据,则FLifeCycle 加 1 ,其他不变
                    if (findedresultmst.FVerNo == "0002")
                    {
                        //根据项目代码去预算表里查找相同代码的条数,得知相关版本号
                        var dicWhereLife = new Dictionary<string, object>();
                        new CreateCriteria(dicWhereLife).Add(ORMRestrictions<string>.Eq("FProjCode", findedresultmst.FProjCode));
                        var FLifeCycle = BudgetMstService.Find(dicWhereLife);
                        findedresultmst.FLifeCycle = FLifeCycle.Data.Count;
                    }
                    else
                    {
                        findedresultmst.FLifeCycle = 1;//第一次年中调整
                    }
                    findedresultmst.FMidYearChange = "1"; //年中调整后,原来年初调整的调整标志改为1
                    findedresultmst.PersistentState = PersistentState.Added;
                    findedresultbudgetdtlimplplan = BudgetMstService.FindBudgetDtlImplPlanByForeignKey(id);
                    findedresultbudgetdtltextcontent = BudgetMstService.FindBudgetDtlTextContentByForeignKey(id);
                    findedresultbudgetdtlfundappl = BudgetMstService.FindBudgetDtlFundApplByForeignKey(id);
                    findedresultbudgetdtlbudgetdtl = BudgetMstService.FindBudgetDtlBudgetDtlByForeignKey(id);
                    findedresultbudgetdtlperformtarget = BudgetMstService.FindBudgetDtlPerformTargetByForeignKey(id);
                    findedresultbudgetdtlPurchaseDtl = BudgetMstService.FindBudgetDtlPurchaseDtlByForeignKey(id);
                    findedresultbudgetdtlPurDtl4SOF = BudgetMstService.FindBudgetDtlPurDtl4SOFByForeignKey(id);

                }
                //当不是新增时记录修改历史
                if (!string.IsNullOrEmpty(mstforminfo.AllRow[0].PhId.ToString()) && mstforminfo.AllRow[0].PhId != 0)
                {
                    BudgetMstService.SaveModify(mstforminfo.AllRow[0], budgetdtlimplplangridinfo, budgetdtltextcontentgridinfo, budgetdtlfundapplgridinfo, budgetdtlbudgetdtlgridinfo, budgetdtlperformtargetgridinfo, projectdtlpurchasedtlforminfo, projectdtlpurdtl4sofgridinfo);//保存预算单据修改记录
                }
                if (projectdtlpurchasedtlforminfo != null && projectdtlpurdtl4sofgridinfo != null)
                {
                    //当不是新增时,先删除原有采购和采购资金来源数据,重新保存
                    if (!string.IsNullOrEmpty(mstforminfo.AllRow[0].PhId.ToString()) && mstforminfo.AllRow[0].PhId != 0)
                    {
                        BudgetMstService.DeleteProjectDtlPurchase(mstforminfo.AllRow[0].PhId);
                    }
                    savedresult = BudgetMstService.SaveBudgetMst(mstforminfo.AllRow[0], budgetdtlimplplangridinfo.AllRow, budgetdtltextcontentgridinfo.AllRow, budgetdtlfundapplgridinfo.AllRow, budgetdtlbudgetdtlgridinfo.AllRow, budgetdtlperformtargetgridinfo.AllRow, projectdtlpurchasedtlforminfo.AllRow, projectdtlpurdtl4sofgridinfo.AllRow, null);
                }
                else
                {
                    savedresult = BudgetMstService.SaveBudgetMst(mstforminfo.AllRow[0], budgetdtlimplplangridinfo.AllRow, budgetdtltextcontentgridinfo.AllRow, budgetdtlfundapplgridinfo.AllRow, budgetdtlbudgetdtlgridinfo.AllRow, budgetdtlperformtargetgridinfo.AllRow);
                }

                // savedresult = BudgetMstService.SaveBudgetMst(mstforminfo.AllRow[0], budgetdtlimplplangridinfo.AllRow, budgetdtltextcontentgridinfo.AllRow, budgetdtlfundapplgridinfo.AllRow, budgetdtlbudgetdtlgridinfo.AllRow, budgetdtlperformtargetgridinfo.AllRow);
                if (savedresult.Status == "success")
                {
                    var i = 0;
                    var FDtlCode = "";
                    var FSourceOfFunds = "";
                    decimal[] FAmount_NEw = new decimal[budgetdtlbudgetdtlgridinfo.AllRow.Count];
                    long[] Xm3_DtlPhid = new long[budgetdtlbudgetdtlgridinfo.AllRow.Count];
                    decimal FAmount;
                    foreach (var dtl in budgetdtlbudgetdtlgridinfo.AllRow)//判断是否引用自项目库
                    {
                        if (dtl.Xm3_DtlPhid > 0)
                        {

                            FDtlCode = dtl.FDtlCode;
                            FSourceOfFunds = dtl.FSourceOfFunds;
                            FAmount = dtl.FBudgetAmount;
                            foreach (var dtlAdd in budgetdtlbudgetdtlgridinfo.AllRow)
                            {
                                if (dtlAdd.Xm3_DtlPhid == 0 && dtlAdd.FDtlCode == FDtlCode && dtlAdd.FSourceOfFunds == FSourceOfFunds)
                                {
                                    FAmount += dtlAdd.FBudgetAmount;
                                }
                            }
                            dtl.FBudgetAmount = FAmount;
                            FAmount_NEw[i] = FAmount;
                            Xm3_DtlPhid[i] = dtl.Xm3_DtlPhid;
                            i++;
                        }
                    }
                    if (Xm3_DtlPhid[0] > 0)
                    {
                        ProjectMstService.UpdateDtlFBudgetAmount(Xm3_DtlPhid, FAmount_NEw);
                    }

                    //年中调整修改年初预算时,复制原来预算信息,f_VerNo值为”0002”，所有信息从”0001”版本拷贝过来(原”0001”的项目记录保存不变)
                    if (midYearEdit == "midYearEdit" && id != 0)
                    {
                        var budgetdtlimplplan = new List<BudgetDtlImplPlanModel>();
                        foreach (var item in findedresultbudgetdtlimplplan.Data)
                        {
                            item.PersistentState = PersistentState.Added;
                            budgetdtlimplplan.Add(item);
                        }
                        var budgetdtltextcontent = new List<BudgetDtlTextContentModel>();
                        foreach (var item in findedresultbudgetdtltextcontent.Data)
                        {
                            item.PersistentState = PersistentState.Added;
                            budgetdtltextcontent.Add(item);
                        }
                        var budgetdtlfundappl = new List<BudgetDtlFundApplModel>();
                        foreach (var item in findedresultbudgetdtlfundappl.Data)
                        {
                            item.PersistentState = PersistentState.Added;
                            budgetdtlfundappl.Add(item);
                        }
                        var budgetdtlbudgetdtl = new List<BudgetDtlBudgetDtlModel>();
                        foreach (var item in findedresultbudgetdtlbudgetdtl.Data)
                        {
                            item.PersistentState = PersistentState.Added;
                            budgetdtlbudgetdtl.Add(item);
                        }

                        var BudgetDtlPerformTarget = new List<BudgetDtlPerformTargetModel>();
                        foreach (var item in findedresultbudgetdtlperformtarget.Data)
                        {
                            item.PersistentState = PersistentState.Added;
                            item.MstPhId = 0;
                            BudgetDtlPerformTarget.Add(item);
                        }
                        var BudgetDtlPurchaseDtl = new List<BudgetDtlPurchaseDtlModel>();
                        foreach (var item in findedresultbudgetdtlPurchaseDtl.Data)
                        {
                            item.PersistentState = PersistentState.Added;
                            BudgetDtlPurchaseDtl.Add(item);
                        }
                        var BudgetDtlPurDtl4SOF = new List<BudgetDtlPurDtl4SOFModel>();
                        foreach (var item in findedresultbudgetdtlPurDtl4SOF.Data)
                        {
                            item.PersistentState = PersistentState.Added;
                            BudgetDtlPurDtl4SOF.Add(item);
                        }
                        //savedresult = 
                        var savedresult2 = BudgetMstService.SaveBudgetMst(findedresultmst, budgetdtlimplplan, budgetdtltextcontent, budgetdtlfundappl, budgetdtlbudgetdtl, BudgetDtlPerformTarget, BudgetDtlPurchaseDtl, BudgetDtlPurDtl4SOF, null);
                        savedresult.KeyCodes.Add(savedresult2.KeyCodes[0]);
                        //BudgetMstService.SaveBudgetMst(findedresultmst, budgetdtlimplplan, budgetdtltextcontent, budgetdtlfundappl, budgetdtlbudgetdtl, budgetdtlperformtargetgridinfo.AllRow);

                    }

                    //对应关系设置-预算库项目对应部门设置,对应关系存放在z_qtdygx中，dylx=’98’
                    var dwdm = mstforminfo.AllRow[0].FProjCode;
                    var dydm = mstforminfo.AllRow[0].FBudgetDept;
                    var dicWhere = new Dictionary<string, object>(); //年初新增
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dwdm", dwdm))
                        .Add(ORMRestrictions<string>.Eq("Dydm", dydm)).Add(ORMRestrictions<string>.Eq("Dylx", "98"));
                    var find = CorrespondenceSettingsService.Find(dicWhere);
                    if (find.Data.Count > 0) { }
                    else
                    {
                        CorrespondenceSettingsModel dygxModel = new CorrespondenceSettingsModel();
                        dygxModel.Dwdm = dwdm;
                        dygxModel.Dydm = dydm;
                        dygxModel.Dylx = "98";
                        dygxModel.DefInt1 = 0;
                        CorrespondenceSettingsService.Save<Int64>(dygxModel, "");
                    }

                }

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



            //删除时,对应关系里相关数据也删除
            var findedresultmst = BudgetMstService.Find(id);

            var deletedresult = BudgetMstService.Delete<System.Int64>(id);

            //删除年中调整数据时,恢复上一版本数据(如果是只做过一次调整,则去掉年初预算里的调整标志,删除相关年中调整数据)
            if ((findedresultmst.Data.FType == "c" && findedresultmst.Data.FVerNo == "0002") || findedresultmst.Data.FType == "z" && findedresultmst.Data.FVerNo == "0002")
            {
                var dicWhere1 = new Dictionary<string, object>(); //年初新增
                var XmMstPhid = findedresultmst.Data.XmMstPhid;
                /*if (findedresultmst.Data.FLifeCycle == 0)
                {
                    new CreateCriteria(dicWhere1).Add(ORMRestrictions<Int64>.Eq("XmMstPhid", XmMstPhid))
                      .Add(ORMRestrictions<string>.Eq("FMidYearChange", "1"));
                    var oldList = BudgetMstService.Find(dicWhere1).Data[0];
                    oldList.FMidYearChange = "0";
                    oldList.PersistentState = PersistentState.Modified;
                    BudgetMstService.Save<Int64>(oldList,"");

                }
                else
                { //多次调整的,FLifeCycle找到最新版置为0
                    new CreateCriteria(dicWhere1).Add(ORMRestrictions<Int64>.Eq("XmMstPhid", XmMstPhid))
                     .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", findedresultmst.Data.FLifeCycle));
                    var oldList = BudgetMstService.Find(dicWhere1).Data[0];
                    oldList.FLifeCycle = 0;
                    oldList.PersistentState = PersistentState.Modified;
                    BudgetMstService.Save<Int64>(oldList,"");
                }*/
                new CreateCriteria(dicWhere1).Add(ORMRestrictions<Int64>.Eq("XmMstPhid", XmMstPhid));
                var oldList = BudgetMstService.Find(dicWhere1, new string[] { "FLifeCycle Desc" }).Data;
                oldList[0].FLifeCycle = 0;
                oldList[0].FMidYearChange = "0";
                oldList[0].PersistentState = PersistentState.Modified;
                BudgetMstService.Save<Int64>(oldList[0], "");
            }
            else //不是年中调整数据则删除对应关系
            {
                var dwdm = findedresultmst.Data.FProjCode;
                var dydm = findedresultmst.Data.FBudgetDept;
                var dicWhere = new Dictionary<string, object>(); //
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dwdm", dwdm))
                    .Add(ORMRestrictions<string>.Eq("Dydm", dydm)).Add(ORMRestrictions<string>.Eq("Dylx", "98"));
                var find = CorrespondenceSettingsService.Find(dicWhere);
                if (find.Data.Count > 0)
                {
                    long dygxId = find.Data[0].PhId;
                    CorrespondenceSettingsService.Delete<System.Int64>(dygxId);
                }
            }





            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 更改项目状态,项目状态更改成“单位备选”时,删除当前预算，并把对应项目的状态改为“单位备选”
        /// </summary>
        /// <returns></returns>
        public string DeleteAndChangeXmk()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            long XmMstPhid = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["XmMstPhid"]);  //项目表主键

            ProjectMstService.UpdateFProjStatus(XmMstPhid);

            var deletedresult = BudgetMstService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }
        /// <summary>
        /// 根据预算单位和预算部门查找部门所在预算进度
        /// </summary>
        /// <returns></returns>
        public string FindBudgetProcessCtrl()
        {
            string oCode = System.Web.HttpContext.Current.Request.Params["oCode"];  //预算单位
            string deptCode = System.Web.HttpContext.Current.Request.Params["deptCode"]; //预算部门
            string FYear = System.Web.HttpContext.Current.Request.Params["FYear"]; //年度

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            var processStatus = BudgetProcessCtrlService.FindBudgetProcessCtrl(oCode,deptCode, FYear);
            if (processStatus == "")
            {
                List<BudgetProcessCtrlModel> list = null;
                BudgetProcessCtrlService.GetBudgetProcessCtrlPorcessList(null, oCode, FYear, out list);
                if (list.Count > 0)
                {
                    BudgetProcessCtrlService.Save<Int64>(list, "");
                }
                processStatus = "1";
            }
            savedresult.Status = "success";
            savedresult.Msg = processStatus;
            return DataConverterHelper.SerializeObject(savedresult);
        }

        ///// <summary>
        ///// 取列表数据
        ///// </summary>
        ///// <returns>返回Json串</returns>
        //public string GetYskmList()
        //{
        //    string code = System.Web.HttpContext.Current.Request.Params["code"];//查询条件
        //var dicWhere = new Dictionary<string, object>();
        //    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FBudgetAccounts", code));


        //    var result = BudgetMstService.Find( dicWhere);

        //    return "";
        //}

        /// <summary>
        /// model 转换
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static T2 ModelChange<T1, T2>(T1 model)
        {
            Type t = model.GetType();
            var fullname = t.FullName;
            //ProjectMstModel obj1 = new ProjectMstModel();
            T2 obj = Activator.CreateInstance<T2>();
            // ProjectMstModel obj = Activator.CreateInstance<ProjectMstModel>();
            PropertyInfo[] PropertyList = t.GetProperties();
            Type o = obj.GetType();
            PropertyInfo[] objList = o.GetProperties();
            foreach (PropertyInfo item in PropertyList)
            {
                var name = item.Name;
                var value = item.GetValue(model, null);
                if (item.DeclaringType.FullName != fullname)
                {
                    continue;
                }
                foreach (var ob in objList)
                {
                    if (ob.Name.ToLower() == item.Name.ToLower())
                    {
                        ob.SetValue(obj, value, null);
                        break;
                    }
                }
            }
            return obj;
        }


        /// <summary>
        /// model 转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="newModel"></param>
        /// <returns></returns>
        public static object ModelChange<T>(T model, object newModel)
        {
            Type t = model.GetType();
            Type o = newModel.GetType();
            var fullName = t.FullName;
            PropertyInfo[] objList = o.GetProperties();
            PropertyInfo[] PropertyList = t.GetProperties();

            // ProjectMstModel obj = Activator.CreateInstance<ProjectMstModel>();

            foreach (PropertyInfo item in PropertyList)
            {
                // var name = item.Name;
                var value = item.GetValue(model, null);
                if (item.DeclaringType.FullName != fullName)
                {
                    continue;
                }
                foreach (var ob in objList)
                {
                    if (ob.Name.ToLower() == item.Name.ToLower())
                    {
                        ob.SetValue(newModel, value, null);
                        break;
                    }
                }

            }
            return newModel;
        }

        /// <summary>
        /// 金格控件取数
        /// </summary>
        /// <returns></returns>
        public string GetKingGridData()
        {
            string oCode = System.Web.HttpContext.Current.Request.Params["FDeclarationUnit"];  //预算单位
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic = BudgetMstService.GetKingGridTagRelateData(oCode, 0);
            return JsonConvert.SerializeObject(dic);
        }


        /// <summary>
        /// 根据项目代码获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetBudgetMstByFProjCode()
        {
            string FProjCode = System.Web.HttpContext.Current.Request.Params["FProjCode"];  //项目代码
            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FProjCode", FProjCode))
                .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0));
            BudgetMstModel findedresultmst = BudgetMstService.Find(dicWhere).Data[0];

            return DataConverterHelper.EntityToJson<BudgetMstModel>(findedresultmst);
        }

        /// <summary>
        /// 同步数据到老G6H数据库
        /// </summary>
        /// <returns></returns>
        public string insertData()
        {
            var result = BudgetMstService.AddData();
            if (result == "")
            {
                return "同步成功";
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 查明细通过明细主键
        /// </summary>
        /// <returns></returns>
        public string FindDtlByPhid()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            var findedresultmst = BudgetMstService.FindDtlByPhid(id);
            return DataConverterHelper.ResponseResultToJson(findedresultmst);
        }

        /// <summary>
        /// 允许预备费抵扣
        /// </summary>
        /// <returns></returns>
        public string AddYBF()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            return BudgetMstService.AddYBF(id);
        }

        /// <summary>
        /// 项目支出预算情况查询
        /// </summary>
        /// <returns></returns>
        public string GetXmZcYs()
        {
            string userID = System.Web.HttpContext.Current.Request.Params["userID"];
            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = BudgetMstService.GetXmZcYs(userID, storeparam.PageIndex, storeparam.PageSize);
            return DataConverterHelper.EntityListToJson<BudgetMstModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 取消审批
        /// </summary>
        /// <returns></returns>
        public string FindUnvalidPiid()
        {
            var approveCode = System.Web.HttpContext.Current.Request.Params["approveCode"]; ;
            var userId = System.Web.HttpContext.Current.Request.Params["userId"];
            var piid = BudgetMstService.FindUnvalidPiid(approveCode, userId);
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            savedresult.Status = ResponseStatus.Success;
            if (piid.Results.Count > 0)
            {
                savedresult.Msg = piid.Results[0].FProjName;
            }
            else
            {
                savedresult.Msg = "";
            }

            return DataConverterHelper.SerializeObject(savedresult);
        }


        /// <summary>
        /// 判断当前选则的模本金额跟实际录入金额的大小比较
        /// </summary>
        /// <returns></returns>
        public string FindIndividualInfo()
        {
            var busType = System.Web.HttpContext.Current.Request.Params["busType"];
            var IndividualInfoId = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["IndividualInfoId"]);
            var projAmount = Convert.ToDecimal(System.Web.HttpContext.Current.Request.Params["projAmount"]);

            var OrgCode = System.Web.HttpContext.Current.Request.Params["OrgCode"];
            var new_id = ProjectMstService.FindIndividualInfo(busType, IndividualInfoId, projAmount, OrgCode);

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            savedresult.Status = ResponseStatus.Success;
            savedresult.Msg = new_id;

            return DataConverterHelper.SerializeObject(savedresult);
        }
        /*
        /// <summary>
        /// 获取老g6h预算数据主表
        /// </summary>
        /// <returns></returns>
        public string GetOldMstList()
        {
            string userID = System.Web.HttpContext.Current.Request.Params["userID"];
            IList<BudgetMstModel> budgetMsts = BudgetMstService.GetOldMstList(userID);
            return DataConverterHelper.EntityListToJson<BudgetMstModel>(budgetMsts, budgetMsts.Count);
        }

        /// <summary>
        /// 获取老g6h预算数据明细表(FQtZcgnfl存的是主单据代码FProjCode)
        /// </summary>
        /// <returns></returns>
        public string GetOldDtlList()
        {
            string userID = System.Web.HttpContext.Current.Request.Params["userID"];
            IList<BudgetDtlBudgetDtlModel> budgetDtls = BudgetMstService.GetOldDtlList(userID);
            return DataConverterHelper.EntityListToJson<BudgetDtlBudgetDtlModel>(budgetDtls, budgetDtls.Count);
        }

        /// <summary>
        /// 获取老g6h预算数据text表(FLTPerformGoal存的是主单据代码FProjCode)
        /// </summary>
        /// <returns></returns>
        public string GetOldTextList()
        {
            string userID = System.Web.HttpContext.Current.Request.Params["userID"];
            IList<BudgetDtlTextContentModel> budgetTexts = BudgetMstService.GetOldTextList(userID);
            return DataConverterHelper.EntityListToJson<BudgetDtlTextContentModel>(budgetTexts, budgetTexts.Count);
        }*/

        /// <summary>
        /// 数据迁移到项目库
        /// </summary>
        public string SynOld()
        {
            string userID = System.Web.HttpContext.Current.Request.Params["userID"];
            var result = BudgetMstService.SaveSynOld(userID);
            return result;
        }

        /// <summary>
        /// 获取已使用数
        /// </summary>
        /// <returns></returns>
        public string GetUseAmount()
        {
            string usercode = System.Web.HttpContext.Current.Request.Params["usercode"];
            string ProjCode = System.Web.HttpContext.Current.Request.Params["ProjCode"];
            return BudgetMstService.GetUseAmount(usercode, ProjCode);
        }

        /// <summary>
        /// 项目预算调整分析表
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetBudgetTZList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary
            var userID = System.Web.HttpContext.Current.Request.Params["userID"];

            var workType = System.Web.HttpContext.Current.Request.Params["workType"]; //业务种类(年初,年中,特殊)
            workType = "z";

            //增加根据操作员对应预算部门的过滤
            var userId = System.Web.HttpContext.Current.Request.Params["userId"];
            var dicWhereDept = new Dictionary<string, object>();
            new CreateCriteria(dicWhereDept)
                .Add(ORMRestrictions<string>.Eq("Dwdm", userId)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
            var deptList = CorrespondenceSettingsService.Find(dicWhereDept);
            List<string> deptL = new List<string>();
            for (var i = 0; i < deptList.Data.Count; i++)
            {
                deptL.Add(deptList.Data[i].Dydm);
            }



            // c - 年初,z - 年中,x - 专项
            if (workType == "z") //年中
            {
                //年中时可以调整年初预算单据
                var dicWhere1 = new Dictionary<string, object>(); //年中调整新增
                var dicWhere2 = new Dictionary<string, object>(); //年初预算新增后调整
                var dicWhere3 = new Dictionary<string, object>(); //年初新增
                new CreateCriteria(dicWhere1).Add(ORMRestrictions<string>.Eq("FType", "z"))
                    .Add(ORMRestrictions<string>.Eq("FVerNo", "0002")).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
                    .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0")).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
                new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FType", "c"))
                    .Add(ORMRestrictions<string>.Eq("FVerNo", "0002")).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
                    .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0")).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL)); //年初没调整过的单据
                new CreateCriteria(dicWhere3).Add(ORMRestrictions<string>.Eq("FType", "c"))
                    .Add(ORMRestrictions<string>.Eq("FVerNo", "0001")).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
                    .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0")).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));

                new CreateCriteria(dicWhere).Add(ORMRestrictions.Or(dicWhere1, dicWhere2, dicWhere3));
                //new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FType", "z"));


            }
            else if (workType == "x") //专项
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FType", "x"))
                    .Add(ORMRestrictions<string>.Eq("FVerNo", "0001")).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
            }
            else if (workType == "c")//年初
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FType", "c"))
                    .Add(ORMRestrictions<string>.Eq("FVerNo", "0001")).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
            }
            else
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
                    .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0")).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
            }





            var result = BudgetMstService.GetBudgetTZList(userID, dicWhere);

            return DataConverterHelper.EntityListToJson<BudgetTZModel>(result, (Int32)result.Count);
        }


        /// <summary>
        /// 获取所有归口项目
        /// </summary>
        /// <returns></returns>
        public string GetGKXM()
        {
            string Dept = System.Web.HttpContext.Current.Request.Params["Dept"];
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", DateTime.Now.Year.ToString()))
                //.Add(ORMRestrictions<string>.Eq("FDeclarationUnit", Unit.OCode))
                //.Add(ORMRestrictions<string>.Eq("FBudgetDept", Dept))
                .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
                .Add(ORMRestrictions<System.String>.Eq("FApproveStatus", "3"))
                .Add(ORMRestrictions<String>.Eq("FMidYearChange", "0"));
            if (!string.IsNullOrEmpty(Dept))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FBudgetDept", Dept));
            }
            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = BudgetMstService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere, new string[] { "FProjCode Asc" });
            return DataConverterHelper.EntityListToJson<BudgetMstModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 预算生成快照
        /// </summary>
        /// <returns></returns>
        public string SaveSnapshot()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            string FDtlstage = System.Web.HttpContext.Current.Request.Params["FDtlstage"];
            var findedresultmst = BudgetMstService.Find(id);
            var findedresultprojectdtlbudgetdtl = BudgetMstService.FindBudgetDtlBudgetDtlByForeignKey(id);
            var findedresultprojectdtlfundappl = BudgetMstService.FindBudgetDtlFundApplByForeignKey(id);
            var findedresultprojectdtlimplplan = BudgetMstService.FindBudgetDtlImplPlanByForeignKey(id);
            var findedresultprojectdtltextcontent = BudgetMstService.FindBudgetDtlTextContentByForeignKey(id);
            var findedresultprojectdtlpurchasedtl = BudgetMstService.FindBudgetDtlPurchaseDtlByForeignKey(id);
            var findedresultprojectdtlpurdtl4sof = BudgetMstService.FindBudgetDtlPurDtl4SOFByForeignKey(id);
            var findedresultprojectdtlPerformTarget = BudgetMstService.FindBudgetDtlPerformTargetByForeignKey(id);

            var mst = ModelChange<BudgetMstModel, QTProjectMstModel>(findedresultmst.Data);
            //查找预算单位下该部门所处预算进度
            mst.FProcessstatus = BudgetProcessCtrlService.FindBudgetProcessCtrl(mst.FDeclarationUnit, mst.FBudgetDept, mst.FYear);
            mst.FTemporarydate = DateTime.Now;
            mst.FDtlstage = FDtlstage;
            mst.PersistentState = PersistentState.Added;

            var budgetdtls = new List<QTProjectDtlBudgetDtlModel>();
            foreach (var item in findedresultprojectdtlbudgetdtl.Data)
            {
                var model = ModelChange<BudgetDtlBudgetDtlModel, QTProjectDtlBudgetDtlModel>(item);
                model.PersistentState = PersistentState.Added;
                budgetdtls.Add(model);
            }
            var fundappls = new List<QTProjectDtlFundApplModel>();
            foreach (var item in findedresultprojectdtlfundappl.Data)
            {
                var model = ModelChange<BudgetDtlFundApplModel, QTProjectDtlFundApplModel>(item);
                model.PersistentState = PersistentState.Added;
                fundappls.Add(model);
            }
            var implplans = new List<QTProjectDtlImplPlanModel>();
            foreach (var item in findedresultprojectdtlimplplan.Data)
            {
                var model = ModelChange<BudgetDtlImplPlanModel, QTProjectDtlImplPlanModel>(item);
                model.PersistentState = PersistentState.Added;
                implplans.Add(model);
            }

            var PerformTargets = new List<QTProjectDtlPerformTargetModel>();
            foreach (var item in findedresultprojectdtlPerformTarget.Data)
            {
                var model = ModelChange<BudgetDtlPerformTargetModel, QTProjectDtlPerformTargetModel>(item);
                model.PersistentState = PersistentState.Added;
                PerformTargets.Add(model);
            }

            var purchasedtls = new List<QTProjectDtlPurchaseDtlModel>();
            foreach (var item in findedresultprojectdtlpurchasedtl.Data)
            {
                var model = ModelChange<BudgetDtlPurchaseDtlModel, QTProjectDtlPurchaseDtlModel>(item);
                model.PersistentState = PersistentState.Added;
                purchasedtls.Add(model);
            }

            var purdtl4sofs = new List<QTProjectDtlPurDtl4SOFModel>();
            foreach (var item in findedresultprojectdtlpurdtl4sof.Data)
            {
                var model = ModelChange<BudgetDtlPurDtl4SOFModel, QTProjectDtlPurDtl4SOFModel>(item);
                model.PersistentState = PersistentState.Added;
                purdtl4sofs.Add(model);
            }

            var textcontent = new List<QTProjectDtlTextContentModel>();
            foreach (var item in findedresultprojectdtltextcontent.Data)
            {
                var model = ModelChange<BudgetDtlTextContentModel, QTProjectDtlTextContentModel>(item);
                model.PersistentState = PersistentState.Added;
                textcontent.Add(model);
            }
            SavedResult<Int64> savedresult = new SavedResult<Int64>();

            try
            {
                savedresult = QTProjectMstService.SaveQTProjectMst(mst, budgetdtls, fundappls, implplans, PerformTargets, purchasedtls, purdtl4sofs, textcontent);
            }
            catch (Exception ex)
            {
                savedresult.Status = ResponseStatus.Error;
                savedresult.Msg = ex.Message.ToString();
            }

            return DataConverterHelper.SerializeObject(savedresult);

        }
        /// <summary>
        /// 保存绩效跟踪数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string SaveTracking()
        {
            string adddata = System.Web.HttpContext.Current.Request.Params["adddata"];
            string updatedata = System.Web.HttpContext.Current.Request.Params["updatedata"];
            string deletedata = System.Web.HttpContext.Current.Request.Params["deletedata"];
            var addinfo = JsonConvert.DeserializeObject<List<JxTrackingModel>>(adddata);
            var updateinfo = JsonConvert.DeserializeObject<List<JxTrackingModel>>(updatedata);
            var deleteinfo = JsonConvert.DeserializeObject<List<string>>(deletedata);

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            savedresult = BudgetMstService.SaveTracking(addinfo, updateinfo, deleteinfo);
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 根据年度取可引用的单据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetReferenceXM()
        {
            var FGoYear = System.Web.HttpContext.Current.Request.Params["FGoYear"];
            var userId = System.Web.HttpContext.Current.Request.Params["userId"];
            var FDeclarationUnit = System.Web.HttpContext.Current.Request.Params["FDeclarationUnit"];
            var FBudgetDept = System.Web.HttpContext.Current.Request.Params["FBudgetDept"];
            var FDeclarationDept = System.Web.HttpContext.Current.Request.Params["FDeclarationDept"];

            var dicWhere = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(FDeclarationUnit))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FDeclarationUnit", FDeclarationUnit));
            }
            if (!string.IsNullOrEmpty(FDeclarationDept))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FDeclarationDept", FDeclarationDept));
            }
            if (!string.IsNullOrEmpty(FBudgetDept))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FBudgetDept", FBudgetDept));
            }
            else
            {
                var dicWhereDept = new Dictionary<string, object>();
                new CreateCriteria(dicWhereDept)
                    .Add(ORMRestrictions<string>.Eq("Dwdm", userId)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
                var deptL = CorrespondenceSettingsService.Find(dicWhereDept).Data.ToList().Select(x => x.Dydm).Distinct().ToList();
                new CreateCriteria(dicWhere).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
            }
            new CreateCriteria(dicWhere).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
                .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"))
                 .Add(ORMRestrictions<string>.Eq("FGoYear", FGoYear));

            //年中时可以调整年初预算单据
            var dicWhere1 = new Dictionary<string, object>(); //年中调整新增
            var dicWhere2 = new Dictionary<string, object>(); //年初预算新增后调整
            var dicWhere3 = new Dictionary<string, object>(); //年初新增
            new CreateCriteria(dicWhere1).Add(ORMRestrictions<string>.Eq("FType", "z"))
                .Add(ORMRestrictions<string>.Eq("FVerNo", "0002"));
            new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FType", "c"))
                .Add(ORMRestrictions<string>.Eq("FVerNo", "0002")); //年初没调整过的单据
            new CreateCriteria(dicWhere3).Add(ORMRestrictions<string>.Eq("FType", "c"))
                .Add(ORMRestrictions<string>.Eq("FVerNo", "0001"));
            new CreateCriteria(dicWhere).Add(ORMRestrictions.Or(dicWhere1, dicWhere2, dicWhere3));

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = BudgetMstService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere, new string[] { "FProjCode" });

            /*for (var x=0;x< result.Results.Count; x++)
            {
                if(result.Results[x].FIfPerformanceAppraisal== EnumYesNo.Yes)
                {
                    var dicWherejx = new Dictionary<string, object>();
                    new CreateCriteria(dicWherejx).Add(ORMRestrictions<Int64>.Eq("YSMstPhId", result.Results[x].PhId))
                        .Add(ORMRestrictions<string>.Eq("FAuditStatus", "2"));
                    var jx=PerformanceMstService.Find(dicWherejx).Data;
                    if (jx.Count > 0)
                    {
                        result.Results[x].JXResult = jx[0].FEvaluationResult;
                    }
                }
            }*/
            return DataConverterHelper.EntityListToJson<BudgetMstModel>(result.Results, (Int32)result.TotalItems);

        }

        /// <summary>
        ///广东调整预算表-ext
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>

        public string GetBudgetAdjustAnalyseList(BudgetAdjustModel param)
        {
            if (string.IsNullOrEmpty(param.Year))
            {
                return ("年份信息不能为空！");
            }
            if (string.IsNullOrEmpty(param.orgCode))
            {
                return ("组织信息不能为空");
            }
            try
            {
                var result = BudgetMstService.GetBudgetAdjustAnalyseList(param.Year, param.orgCode);
                List<BudgetAdjustAnalyseModel> result2 = new List<BudgetAdjustAnalyseModel>();
                if (result != null && result.Count > 0)
                {
                    //筛选申报单位
                    if (!string.IsNullOrEmpty(param.FDeclarationUnit))
                    {
                        result = result.FindAll(t => t.FDeclarationUnit == param.FDeclarationUnit);
                    }
                    //筛选申报部门
                    if (!string.IsNullOrEmpty(param.FDeclarationDept))
                    {
                        result = result.FindAll(t => t.FDeclarationDept == param.FDeclarationDept);
                    }
                    //筛选预算部门
                    if (!string.IsNullOrEmpty(param.FBudgetDept))
                    {
                        result = result.FindAll(t => t.FBudgetDept == param.FBudgetDept);
                    }
                    //先将结果，再附上序号
                    if (result != null && result.Count > 0)
                    {
                        //先进行排序
                        result = result.OrderBy(t => t.FDeclarationUnit).ThenBy(t => t.FBudgetDept).ThenBy(t => t.FDeclarationDept).ThenBy(t => t.FProjCode).ThenBy(t => t.FDtlCode).ToList();
                        //再附上序号
                        int row = 1;
                        foreach (var res in result)
                        {
                            if (!string.IsNullOrEmpty(res.FDtlCode) && res.FDtlCode.Equals("zzzheji"))
                            {
                                continue;
                            }
                            else
                            {
                                res.OrderNumber = row;
                                row++;
                            }
                        }
                        //按项目显示列表，只取合计行即可
                        result2 = result.ToList().FindAll(t => t.FDtlCode == "zzzheji").OrderBy(t => t.FDeclarationUnit).ThenBy(t => t.FBudgetDept).ThenBy(t => t.FDeclarationDept).ThenBy(t => t.FProjCode).ToList();

                    }
                }
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "数据获取成功！",
                    Data = result,
                    Data2 = result2
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return (ex.ToString());
            }
        }
    }
}

