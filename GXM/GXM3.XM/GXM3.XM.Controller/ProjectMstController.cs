#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectMstController
    * 命名空间：        GXM3.XM.Controller
    * 文 件 名：        ProjectMstController.cs
    * 创建时间：        2018/8/28 
    * 作    者：        李明    
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

using GXM3.XM.Service.Interface;
using GXM3.XM.Model.Domain;

using NG3.WorkFlow.Rule;
using GQT3.QT.Service.Interface;
using GQT3.QT.Model.Domain;
using Enterprise3.Common.Base.Criterion;
using System.Data;
using GYS3.YS.Model.Domain;
using GYS3.YS.Service.Interface;

using System.Reflection;
using Enterprise3.Common.Base.Helpers;
using System.Linq;
using GYS3.YS.Model.Enums;
using GJX3.JX.Service.Interface;

namespace GXM3.XM.Controller
{
    /// <summary>
    /// ProjectMst控制处理类
    /// </summary>
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ProjectMstController : AFCommonController
    {
        IProjectMstService ProjectMstService { get; set; }
        IProjLibProjService ProjLibService { get; set; }
        IQtSysCodeSeqService SysCodeSeqService { get; set; }
        IGHProjDtlShareService ProjDtlShareService { get; set; }
        IBudgetMstService BudgetMstService { get; set; }
        ICorrespondenceSettingsService CorrespondenceSettingsService { get; set; }
        IBudgetProcessCtrlService BudgetProcessCtrlService { get; set; }
        IExpenseMstService ExpenseMstService { get; set; }
        IGHSubjectService GHSubjectService { get; set; }
        IQTIndividualInfoService QTIndividualInfoService { get; set; }
        IQTProjectMstService QTProjectMstService { get; set; }
        IQTEditMemoService QTEditMemoService { get; set; }
        IPerformanceMstService PerformanceMstService { get; set; }
        IQtAccountService QtAccountService { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProjectMstController()
        {
            ProjectMstService = base.GetObject<IProjectMstService>("GXM3.XM.Service.ProjectMst");
            SysCodeSeqService = base.GetObject<IQtSysCodeSeqService>("GQT3.QT.Service.QtSysCodeSeq");
            ProjLibService = base.GetObject<IProjLibProjService>("GQT3.QT.Service.ProjLibProj");
            ProjDtlShareService = base.GetObject<IGHProjDtlShareService>("GQT3.QT.Service.GHProjDtlShare");
            BudgetMstService = base.GetObject<IBudgetMstService>("GYS3.YS.Service.BudgetMst");
            CorrespondenceSettingsService = base.GetObject<ICorrespondenceSettingsService>("GQT3.QT.Service.CorrespondenceSettings");
            BudgetProcessCtrlService = base.GetObject<IBudgetProcessCtrlService>("GYS3.YS.Service.BudgetProcessCtrl");
            ExpenseMstService = base.GetObject<IExpenseMstService>("GYS3.YS.Service.ExpenseMst");
            GHSubjectService = base.GetObject<IGHSubjectService>("GYS3.YS.Service.GHSubject");
            QTIndividualInfoService = base.GetObject<IQTIndividualInfoService>("GQT3.QT.Service.QTIndividualInfo");
            QTProjectMstService = base.GetObject<IQTProjectMstService>("GQT3.QT.Service.QTProjectMst");
            QTEditMemoService = base.GetObject<IQTEditMemoService>("GQT3.QT.Service.QTEditMemo");
            PerformanceMstService = base.GetObject<IPerformanceMstService>("GJX3.JX.Service.PerformanceMst");
            QtAccountService = base.GetObject<IQtAccountService>("GQT3.QT.Service.QtAccount");

        }

        /// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult ProjectMstList()
        {
            ViewBag.Title = base.GetMenuLanguage("GHProjectMst");//根据业务类型对应的langkey取多语言
            ViewBag.ProjStatus = System.Web.HttpContext.Current.Request.Params["ProjStatus"];//页面进来的立项情况(1,预立项;2,立项)
            ViewBag.FApproveStatus = System.Web.HttpContext.Current.Request.Params["FApproveStatus"]; //是否待上报页面
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "项目立项";
                if (ViewBag.ProjStatus == "1")
                {
                    ViewBag.Title = "项目预立项";
                    base.InitialMultiLanguage("GHBudgetYLX");
                    ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetYLX");
                }
                else if (ViewBag.ProjStatus == "2")
                {
                    ViewBag.Title = "项目立项";
                    base.InitialMultiLanguage("GHBudgetXMLX");
                    ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetXMLX");
                }


            }
            //base.InitialMultiLanguage("GHProjectMst");
            //ViewBag.IndividualInfo = this.GetIndividualUI("GHProjectMst");

            return View("ProjectMstList");
        }

        /// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult ProjectMstEdit()
        {
            var tabTitle = base.GetMenuLanguage("GHProjectMst");//根据业务类型对应的langkey取多语言
            var ViewProjStatus = System.Web.HttpContext.Current.Request.Params["ProjStatus"];//页面进来的立项情况(1,预立项;2,立项)
            var IndividualinfoId = System.Web.HttpContext.Current.Request.Params["IndividualinfoId"];//自定义界面对应金额ID
            ViewBag.midEdit = System.Web.HttpContext.Current.Request.Params["midEdit"]; //是不是调整
            ViewBag.memoRight = System.Web.HttpContext.Current.Request.Params["memoRight"];//是不是批注
            ViewBag.IndividualInfo = "";
            ViewBag.changeIndividualinfoId = System.Web.HttpContext.Current.Request.Params["changeIndividualinfoId"];//否是自动切换模板
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            ViewBag.XMreference = System.Web.HttpContext.Current.Request.Params["XMreference"]; //判断是否为已有项目第一次修改
            if (id != 0)
            {
                var findedresultmst = ProjectMstService.Find(id);
                if (string.IsNullOrEmpty(ViewProjStatus))
                {
                    ViewProjStatus = findedresultmst.Data.FProjStatus.ToString();

                }
                if (string.IsNullOrEmpty(IndividualinfoId))
                {
                    IndividualinfoId = findedresultmst.Data.FIndividualinfophid;
                }
                //如果还是空
                if (string.IsNullOrEmpty(IndividualinfoId))
                {
                    Dictionary<string, object> dic222 = new Dictionary<string, object>();
                    string OrgCode = NG3.AppInfoBase.OCode;
                    if (findedresultmst.Data.FProjStatus.ToString() == "1")
                    {
                        new CreateCriteria(dic222).Add(ORMRestrictions<string>.Eq("IndividualinfoBustype", "GHBudgetYLX"));
                        var IndividualInfoData = QTIndividualInfoService.Find(dic222).Data;
                        IndividualInfoData = IndividualInfoData.ToList().FindAll(x => !string.IsNullOrEmpty(x.DEFSTR9) && x.DEFSTR9.Split(',').ToList().Contains(OrgCode));
                        if (IndividualInfoData.Count > 0)
                        {
                            IndividualinfoId = IndividualInfoData[0].PhId.ToString();
                            ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetYLX",IndividualInfoData[0].PhId);
                        }
                        else
                        {
                            ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetYLX");
                        }

                    }
                    else
                    {
                        new CreateCriteria(dic222).Add(ORMRestrictions<string>.Eq("IndividualinfoBustype", "GHBudgetXMLX"));
                        var IndividualInfoData = QTIndividualInfoService.Find(dic222).Data;
                        IndividualInfoData = IndividualInfoData.ToList().FindAll(x => !string.IsNullOrEmpty(x.DEFSTR9) && x.DEFSTR9.Split(',').ToList().Contains(OrgCode));
                        if (IndividualInfoData.Count > 0)
                        {
                            IndividualinfoId = IndividualInfoData[0].PhId.ToString();
                            ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetXMLX", IndividualInfoData[0].PhId);
                        }
                        else
                        {
                            ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetXMLX");
                        }
                    }

                }
                else
                {
                    //var IndividualInfoData = QTIndividualInfoService.Find(Convert.ToInt64(IndividualinfoId)).Data;
                    if (findedresultmst.Data.FProjStatus.ToString() == "1")
                    {
                        ViewBag.IndividualInfo= this.GetIndividualUI("GHBudgetYLX", Convert.ToInt64(IndividualinfoId));
                    }
                    else
                    {
                        ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetXMLX", Convert.ToInt64(IndividualinfoId));
                    }
                    
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(IndividualinfoId))
                {
                    var IndividualInfoData = QTIndividualInfoService.Find(Convert.ToInt64(IndividualinfoId)).Data;
                    if (ViewProjStatus == "1")
                    {
                        //ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetYLX", Convert.ToInt64(IndividualInfoData.YLXPhid));
                        ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetYLX", IndividualInfoData.PhId);
                    }
                    else
                    {
                        //ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetXMLX", Convert.ToInt64(IndividualInfoData.XMLXPhid));
                        ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetXMLX", IndividualInfoData.PhId);
                    }
                }
                else
                {
                    if (ViewProjStatus == "1")
                    {
                        ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetYLX");
                    }
                    else
                    {
                        ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetXMLX");
                    }
                }
            }

            ViewBag.ProjStatus = ViewProjStatus;

            //string IndividualinfoValue = "";
            //if (!string.IsNullOrEmpty(IndividualinfoId))
            //{
            //    var IndividualInfoData = QTIndividualInfoService.Find(Convert.ToInt64(IndividualinfoId));
            //    if (ViewBag.ProjStatus == "1")
            //    {
            //        IndividualinfoValue = IndividualInfoData.Data.YLXPhid;
            //    }else if(ViewBag.ProjStatus == "2")
            //    {
            //        IndividualinfoValue = IndividualInfoData.Data.XMLXPhid;
            //    }

            //}

            ViewBag.IndividualinfoId = IndividualinfoId;


            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "项目立项";
                if (ViewBag.ProjStatus == "1")
                {
                    tabTitle = "项目预立项";
                    /*if (string.IsNullOrEmpty(IndividualinfoId)) {
                        ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetYLX");
                    }
                    else
                    {
                        ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetYLX", Convert.ToInt64(IndividualinfoId));
                    }*/

                    base.SetUserDefScriptUrl("GHBudgetYLX");
                    base.InitialMultiLanguage("GHBudgetYLX");
                }
                else if (ViewBag.ProjStatus == "2")
                {
                    tabTitle = "项目立项";
                    /*if (string.IsNullOrEmpty(IndividualinfoId))
                    {
                        ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetXMLX");
                    }
                    else
                    {
                        ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetXMLX", Convert.ToInt64(IndividualinfoId));
                    }*/

                    base.SetUserDefScriptUrl("GHBudgetXMLX");
                    base.InitialMultiLanguage("GHBudgetXMLX");
                }
            }
            //base.SetUserDefScriptUrl("GHProjectMst");
            //base.InitialMultiLanguage("GHProjectMst");
            //ViewBag.IndividualInfo = this.GetIndividualUI("GHProjectMst");

            ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型


            if (ViewBag.OType == "add")
            {
                //新增时
                //if (ProjectMstService.Has_BillNoRule("单据规则类型") == true)
                //{
                //    var vBillNo = ProjectMstService.GetBillNo();//取用户编码,新增时界面上显示
                //    ViewBag.No = vBillNo.BillNoList[0];
                //}
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
            else if (ViewBag.memoRight == "memoRight")
            {
                ViewBag.Title = tabTitle + "-批注";
            }

            return View("ProjectMstEdit");
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetProjectMstList()
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
                new CreateCriteria(dicWhere).Add(ORMRestrictions<List<string>>.In("FZcType", new List<string>() {"2","3" }));
            }
            var FApproveStatus = System.Web.HttpContext.Current.Request.Params["FApproveStatus"];

            var ProjStatus = System.Web.HttpContext.Current.Request.Params["ProjStatus"];
            //增加根据操作员对应预算部门的过滤
            var userId = System.Web.HttpContext.Current.Request.Params["userId"];

            //增加年度过滤
            var FYear = System.Web.HttpContext.Current.Request.Params["FYear"];

            var showAll = System.Web.HttpContext.Current.Request.Params["showAll"];
            var FDeclarationUnit= System.Web.HttpContext.Current.Request.Params["FDeclarationUnit"];
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
                    Dictionary<string, object> dicWhereys2 = new Dictionary<string, object>();
                    Dictionary<string, object> dicWhereys3 = new Dictionary<string, object>();
                    new CreateCriteria(dicWhereys2).Add(ORMRestrictions<string>.Eq("FBudgetDept", DefaultDept));
                    new CreateCriteria(dicWhereys3).Add(ORMRestrictions<string>.Eq("FDeclarationDept", DefaultDept));
                    new CreateCriteria(dicWhere).Add(ORMRestrictions.Or(dicWhereys2, dicWhereys3));

                }
                else
                {
                    return DataConverterHelper.EntityListToJson<BudgetMstModel>(null, 0);
                }
            }

            if (ProjStatus == "1")  //预立项
            {
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
                    .Add(ORMRestrictions<Int32>.Eq("FProjStatus", 1));
            }
            else if (ProjStatus == "2") //立项
            {
                new CreateCriteria(dicWhere)//.Add(ORMRestrictions<Int32>.Eq("FProjStatus", 2))
                   .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<IList<int>>.NotIn("FProjStatus", new List<int> { 1 }));
            }
            else
            {
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0));
            }


            if (FApproveStatus == "1" && (clientJsonQuery == null|| clientJsonQuery.IndexOf("FApproveStatus") == -1))
            {

                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FApproveStatus", "1"))
                    .Add(ORMRestrictions<Int32>.Eq("FProjStatus", 1));

            }
            //增加年度过滤条件
            //if (clientJsonQuery.IndexOf("FYear") == -1)
            //{
            //    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", FYear));
            //}
            /*if (!dicWhere.ContainsKey("FYear*str*eq*1")|| !string.IsNullOrEmpty(dicWhere["FYear*str*eq*1"].ToString()))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", FYear));
            }*/

            if (!string.IsNullOrEmpty(FYear))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FGoYear", FYear));
            }
            if ((clientJsonQuery ==null|| clientJsonQuery.IndexOf("FDeclarationUnit") == -1)&&!string.IsNullOrEmpty(FDeclarationUnit))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FDeclarationUnit", FDeclarationUnit));
            }

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = ProjectMstService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" });

            return DataConverterHelper.EntityListToJson<ProjectMstModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetProjectMstInfo()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型

            switch (tabtype)
            {
                case "projectmst":
                    var findedresultmst = ProjectMstService.Find(id);
                    return DataConverterHelper.ResponseResultToJson(findedresultmst);
                case "projectdtlimplplan":
                    var findedresultprojectdtlimplplan = ProjectMstService.FindProjectDtlImplPlanByForeignKey(id);
                    return DataConverterHelper.EntityListToJson(findedresultprojectdtlimplplan.Data, findedresultprojectdtlimplplan.Data.Count);
                case "projectdtltextcontent":
                    var findedresultprojectdtltextcontent = ProjectMstService.FindProjectDtlTextContentByForeignKey(id);
                    if (findedresultprojectdtltextcontent != null)
                    {
                        if (findedresultprojectdtltextcontent.Data.Count > 0)
                        {
                            ProjectDtlTextContentModel singleData = findedresultprojectdtltextcontent.Data[0];
                            FindedResult<ProjectDtlTextContentModel> result = new FindedResult<ProjectDtlTextContentModel>(false, singleData);
                            return DataConverterHelper.ResponseResultToJson(result);
                        }
                    }
                    return DataConverterHelper.EntityListToJson(findedresultprojectdtltextcontent.Data, findedresultprojectdtltextcontent.Data.Count);
                case "projectdtlfundappl":
                    var findedresultprojectdtlfundappl = ProjectMstService.FindProjectDtlFundApplByForeignKey(id);
                    return DataConverterHelper.EntityListToJson(findedresultprojectdtlfundappl.Data, findedresultprojectdtlfundappl.Data.Count);
                case "projectdtlbudgetdtl":
                    var findedresultprojectdtlbudgetdtl = ProjectMstService.FindProjectDtlBudgetDtlByForeignKey(id);
                    return DataConverterHelper.EntityListToJson(findedresultprojectdtlbudgetdtl.Data, findedresultprojectdtlbudgetdtl.Data.Count);
                case "projectdtlpurchasedtl":
                    var findedresultsprojectdtlpurchasedtl = ProjectMstService.FindProjectDtlPurchaseDtlByForeignKey(id);
                    return DataConverterHelper.EntityListToJson(findedresultsprojectdtlpurchasedtl.Data, findedresultsprojectdtlpurchasedtl.Data.Count);
                // return DataConverterHelper.ResponseResultToJson(findedresultsprojectdtlpurchasedtl.Data[0]);
                case "projectdtlpurdtl4sof":
                    var findedresultprojectdtlpurdtl4sof = ProjectMstService.FindProjectDtlPurDtl4SOFByForeignKey(id);
                    return DataConverterHelper.EntityListToJson(findedresultprojectdtlpurdtl4sof.Data, findedresultprojectdtlpurdtl4sof.Data.Count);
                case "projectdtlperformtarget":
                    var findedresultprojectdtlperformtarget = ProjectMstService.FindProjectDtlPerformTargetByForeignKey(id);
                    return DataConverterHelper.EntityListToJson(findedresultprojectdtlperformtarget.Data, findedresultprojectdtlperformtarget.Data.Count);
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
            string projectdtlimplplangridData = System.Web.HttpContext.Current.Request.Form["projectdtlimplplangridData"];
            string projectdtltextcontentgridData = System.Web.HttpContext.Current.Request.Form["projectdtltextcontentgridData"];
            string projectdtlfundapplgridData = System.Web.HttpContext.Current.Request.Form["projectdtlfundapplgridData"];
            string projectdtlbudgetdtlgridData = System.Web.HttpContext.Current.Request.Form["projectdtlbudgetdtlgridData"];
            string projectdtlperformtargetgridData = System.Web.HttpContext.Current.Request.Form["projectdtlperformtargetgridData"];
            string projectdtlpurchasedtlformData = System.Web.HttpContext.Current.Request.Form["projectdtlpurchasedtlformData"];
            string projectdtlpurdtl4sofgridData = System.Web.HttpContext.Current.Request.Form["projectdtlpurdtl4sofgridData"];

            string midEdit = System.Web.HttpContext.Current.Request.Form["midEdit"];

            var mstforminfo = DataConverterHelper.JsonToEntity<ProjectMstModel>(mstformData);
            var projectdtlimplplangridinfo = DataConverterHelper.JsonToEntity<ProjectDtlImplPlanModel>(projectdtlimplplangridData);
            var projectdtltextcontentgridinfo = DataConverterHelper.JsonToEntity<ProjectDtlTextContentModel>(projectdtltextcontentgridData);
            var projectdtlfundapplgridinfo = DataConverterHelper.JsonToEntity<ProjectDtlFundApplModel>(projectdtlfundapplgridData);
            var projectdtlbudgetdtlgridinfo = DataConverterHelper.JsonToEntity<ProjectDtlBudgetDtlModel>(projectdtlbudgetdtlgridData);
            var projectdtlperformtargetgridinfo = DataConverterHelper.JsonToEntity<ProjectDtlPerformTargetModel>(projectdtlperformtargetgridData);


            EntityInfo<ProjectDtlPurchaseDtlModel> projectdtlpurchasedtlforminfo = null;
            if (projectdtlpurchasedtlformData != null)
            {
                 projectdtlpurchasedtlforminfo = DataConverterHelper.JsonToEntity<ProjectDtlPurchaseDtlModel>(projectdtlpurchasedtlformData);
            }

            EntityInfo<ProjectDtlPurDtl4SOFModel> projectdtlpurdtl4sofgridinfo = null;
            if (projectdtlpurdtl4sofgridData != null)
            {
                 projectdtlpurdtl4sofgridinfo = DataConverterHelper.JsonToEntity<ProjectDtlPurDtl4SOFModel>(projectdtlpurdtl4sofgridData);
            }

            var findedresultmst = new ProjectMstModel();
            var findedresultbudgetdtlimplplan = new FindedResults<ProjectDtlImplPlanModel>();
            var findedresultbudgetdtltextcontent = new FindedResults<ProjectDtlTextContentModel>();
            var findedresultbudgetdtlfundappl = new FindedResults<ProjectDtlFundApplModel>();
            var findedresultbudgetdtlbudgetdtl = new FindedResults<ProjectDtlBudgetDtlModel>();

            var findedresultPurchasedtl = new FindedResults<ProjectDtlPurchaseDtlModel>();
            var findedresultPurDtl4SOF = new FindedResults<ProjectDtlPurDtl4SOFModel>();
            var findedresultPerformTarget = new FindedResults<ProjectDtlPerformTargetModel>();
            long id = 0;

            if(mstforminfo.AllRow[0].FType == "") //项目保存时，如果没有进度状态，则增加
            {

                var processStatus = BudgetProcessCtrlService.FindBudgetProcessCtrl(mstforminfo.AllRow[0].FDeclarationUnit, mstforminfo.AllRow[0].FBudgetDept,mstforminfo.AllRow[0].FYear);
                //单位进度控制为1时，是年初申报，为3时，为年中调整
                if (processStatus == "1")
                {
                    mstforminfo.AllRow[0].FType = "c";
                    mstforminfo.AllRow[0].FVerNo = "0001";
                }
                else if (processStatus == "3")
                {
                    
                    mstforminfo.AllRow[0].FType = "z";
                    //budgetmst.FType = "z";
                    mstforminfo.AllRow[0].FVerNo = "0002";
                }
            }

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            try
            {
                bool isNew = false;
                var projCode = mstforminfo.AllRow[0].FProjCode;
                var year = mstforminfo.AllRow[0].FYear;

                bool bCreateProjCode = false; //项目编码是是否为本次生成的： true 是, false 否

                //新增的项目
                if (mstforminfo.NewRow.Count == 1)
                {
                    mstforminfo.AllRow[0].FDeclarationDept = ProjectMstService.GetDefaultDept(base.UserID);
                    isNew = true;
                    #region 生成项目编码       
                    if (string.IsNullOrEmpty(mstforminfo.AllRow[0].FProjCode))
                    {
                        bCreateProjCode = true; //项目代码为本次生成

                        //获取最大项目库编码
                        projCode = ProjectMstService.CreateOrGetMaxProjCode(year);
                        
                        mstforminfo.AllRow[0].FProjCode = projCode;
                    }
                    else {
                        bCreateProjCode = false; //项目代码为引用
                    }
                    #endregion
                    //当没有录入账套信息时取默认账套
                    if (string.IsNullOrEmpty(mstforminfo.AllRow[0].FAccount))
                    {
                        var Accounts=QtAccountService.Find(x => x.IsDefault == 1).Data;
                        if (Accounts.Count > 0)
                        {
                            mstforminfo.AllRow[0].FAccount = Accounts[0].Dm;
                        }
                    }
                }

                //当不是延续项目时 可能执行年度没有值
                if (string.IsNullOrEmpty(mstforminfo.AllRow[0].FGoYear))
                {
                    mstforminfo.AllRow[0].FGoYear = mstforminfo.AllRow[0].FYear;
                }
                #region 生成项目明细编码: 项目明细编码=项目编码 + 6位流水号
                string dtlCode = "";
                string dtlName = "";
                for (var i = 0; i < projectdtlbudgetdtlgridinfo.NewRow.Count; i++)
                {
                    dtlCode = projectdtlbudgetdtlgridinfo.NewRow[i].FDtlCode;
                    dtlName = projectdtlbudgetdtlgridinfo.NewRow[i].FName.Trim();
                    if (string.IsNullOrEmpty(dtlCode))
                    {
                        //多行存在明细项目相同的视为同一个明细项目，后台存一个代码；(相同项目名称的项目代码相同)
                        projectdtlbudgetdtlgridinfo.NewRow[i].FDtlCode = projCode + string.Format("{0:D6}", i + 1);
                        for (var j = 0; j < i; j++)
                        {
                            if(dtlName == projectdtlbudgetdtlgridinfo.NewRow[j].FName.Trim())
                            {
                                projectdtlbudgetdtlgridinfo.NewRow[i].FDtlCode = projectdtlbudgetdtlgridinfo.NewRow[j].FDtlCode;
                                break;
                            }
                        }

                    }

                    dtlCode = projectdtlbudgetdtlgridinfo.NewRow[i].FDtlCode; ;

                    for (var j = 0; j < projectdtlpurchasedtlforminfo.AllRow.Count;j++)
                    {
                        if(projectdtlpurchasedtlforminfo.NewRow[j].FName == dtlName)
                        {
                            projectdtlpurchasedtlforminfo.NewRow[j].FDtlCode = dtlCode;
                        }
                    }

                    for (var j = 0; j < projectdtlpurdtl4sofgridinfo.AllRow.Count; j++)
                    {
                        if (projectdtlpurdtl4sofgridinfo.NewRow[j].FName == dtlName)
                        {
                            projectdtlpurdtl4sofgridinfo.NewRow[j].FDtlCode = dtlCode;
                        }
                    }


                }
                #endregion
                
                #region 缓存删除的明细信息
                List<GHProjDtlShareModel> projShareDeleteList = new List<GHProjDtlShareModel>();
                GHProjDtlShareModel pshareM = null;
                foreach (var budgetDtlModel in projectdtlbudgetdtlgridinfo.DeleteRow)
                {
                    var rec = ProjectMstService.FindProjectDtlBudgetDtlByPhID(budgetDtlModel.PhId);
                    if (rec.Data != null)
                    {
                        pshareM = new GHProjDtlShareModel
                        {
                            DM = projCode,
                            XMDM = rec.Data.FDtlCode,
                            PersistentState = PersistentState.Deleted
                        };
                        projShareDeleteList.Add(pshareM);
                    }
                }
                #endregion


                //预立项调整时,保存原有单据信息,版本号加 1 
                if (midEdit == "midEdit")
                {
                    id = mstforminfo.AllRow[0].PhId;
                    //年中调整时,项目审批状态改为未审批,项目属性改为预立项
                    mstforminfo.AllRow[0].FApproveStatus = "1";
                    mstforminfo.AllRow[0].FProjStatus = 1;


                    findedresultmst = ProjectMstService.Find(id).Data;

                    //根据项目代码去项目表里查找相同代码的条数,得知相关版本号
                    var dicWhereLife = new Dictionary<string, object>();
                    new CreateCriteria(dicWhereLife).Add(ORMRestrictions<string>.Eq("FProjCode", findedresultmst.FProjCode))  ;
                    var FLifeCycle = ProjectMstService.Find(dicWhereLife);

                    findedresultmst.FLifeCycle = FLifeCycle.Data.Count;
                    
                    findedresultmst.PersistentState = PersistentState.Added;
                    findedresultbudgetdtlimplplan = ProjectMstService.FindProjectDtlImplPlanByForeignKey(id);
                    findedresultbudgetdtltextcontent = ProjectMstService.FindProjectDtlTextContentByForeignKey(id);
                    findedresultbudgetdtlfundappl = ProjectMstService.FindProjectDtlFundApplByForeignKey(id);
                    findedresultbudgetdtlbudgetdtl = ProjectMstService.FindProjectDtlBudgetDtlByForeignKey(id);

                    findedresultPurchasedtl = ProjectMstService.FindProjectDtlPurchaseDtlByForeignKey(id);
                    findedresultPurDtl4SOF = ProjectMstService.FindProjectDtlPurDtl4SOFByForeignKey(id);
                    findedresultPerformTarget = ProjectMstService.FindProjectDtlPerformTargetByForeignKey(id);
                }

                //当不是新增时记录修改历史
                if (!string.IsNullOrEmpty(mstforminfo.AllRow[0].PhId.ToString()) && mstforminfo.AllRow[0].PhId != 0)
                {
                    ProjectMstService.SaveModify(mstforminfo.AllRow[0], projectdtlimplplangridinfo, projectdtltextcontentgridinfo, projectdtlfundapplgridinfo, projectdtlbudgetdtlgridinfo, projectdtlperformtargetgridinfo, projectdtlpurchasedtlforminfo, projectdtlpurdtl4sofgridinfo);//保存预算单据修改记录
                }

                //保存
                //savedresult = ProjectMstService.SaveProjectMst(mstforminfo.AllRow[0], projectdtlimplplangridinfo.AllRow, projectdtltextcontentgridinfo.AllRow, projectdtlfundapplgridinfo.AllRow, projectdtlbudgetdtlgridinfo.AllRow);
                if (projectdtlpurchasedtlforminfo != null && projectdtlpurdtl4sofgridinfo != null)
                {
                    //当不是新增时,先删除原有采购和采购资金来源数据,重新保存
                    if (!string.IsNullOrEmpty(mstforminfo.AllRow[0].PhId.ToString()) && mstforminfo.AllRow[0].PhId != 0)
                    {
                        ProjectMstService.DeleteProjectDtlPurchase(mstforminfo.AllRow[0].PhId);
                    }
                    savedresult = ProjectMstService.SaveProjectMst(mstforminfo.AllRow[0], projectdtltextcontentgridinfo.AllRow, projectdtlpurchasedtlforminfo.AllRow, projectdtlpurdtl4sofgridinfo.AllRow, projectdtlperformtargetgridinfo.AllRow, projectdtlfundapplgridinfo.AllRow, projectdtlbudgetdtlgridinfo.AllRow, projectdtlimplplangridinfo.AllRow);
                }
                else
                {
                    savedresult = ProjectMstService.SaveProjectMst(mstforminfo.AllRow[0], projectdtltextcontentgridinfo.AllRow, null, null, projectdtlperformtargetgridinfo.AllRow, projectdtlfundapplgridinfo.AllRow, projectdtlbudgetdtlgridinfo.AllRow, projectdtlimplplangridinfo.AllRow);
                }


                if (isNew)
                {
                    if (savedresult.Status == ResponseStatus.Success && savedresult.KeyCodes.Count > 0)
                    {
                        //保存项目到Z_QTGKXM
                        if (bCreateProjCode)
                        {
                            ProjLibProjModel pModel = new ProjLibProjModel
                            {
                                DM = projCode,
                                MC = mstforminfo.AllRow[0].FProjName,
                                DEFSTR1 = "1",
                                DEFSTR3 = mstforminfo.AllRow[0].FDeclarationUnit,
                                DEFSTR4= mstforminfo.AllRow[0].FBudgetDept,
                                PersistentState = PersistentState.Added
                            };
                            SavedResult<Int64> sr = ProjLibService.Save<Int64>(pModel,"");
                        }
                        else {
                            var find = ProjLibService.Find(t => t.DM == projCode);
                            if (find.Data.Count>0) {
                                ProjLibProjModel pModel = find.Data[0];
                                pModel.DEFSTR1 = "1";
                                pModel.DEFSTR3 = mstforminfo.AllRow[0].FDeclarationUnit;
                                pModel.DEFSTR4 = mstforminfo.AllRow[0].FBudgetDept;
                                pModel.PersistentState = PersistentState.Modified;

                                SavedResult<Int64> sr = ProjLibService.Save<Int64>(pModel,"");
                            }
                        }
                    }
                }

                #region 更新z_qtgkxm
                if (mstforminfo.ModifyRow.Count > 0) {
                    var find = ProjLibService.Find(t => t.DM == projCode);
                    if (find.Data.Count > 0)
                    {
                        ProjLibProjModel pModel = find.Data[0];
                        pModel.MC = mstforminfo.ModifyRow[0].FProjName;
                        pModel.DEFSTR1 = "1";
                        pModel.DEFSTR3 = mstforminfo.AllRow[0].FDeclarationUnit;
                        pModel.DEFSTR4 = mstforminfo.AllRow[0].FBudgetDept;
                        pModel.PersistentState = PersistentState.Modified;
                        SavedResult<Int64> sr = ProjLibService.Save<Int64>(pModel,"");
                    }
                }
                #endregion

                /*#region 保存项目明细数据到JJ_FXGL中
                //新增的
                List<GHProjDtlShareModel> shareModels = new List<GHProjDtlShareModel>();
                GHProjDtlShareModel shareModel = null;
                foreach (var budgetDtlModel in projectdtlbudgetdtlgridinfo.NewRow)
                {
                    shareModel = new GHProjDtlShareModel
                    {
                        MK = "jj",
                        DM = projCode,
                        XMDM = budgetDtlModel.FDtlCode,
                        MC = budgetDtlModel.FName,
                        PersistentState = PersistentState.Added
                    };
                    shareModels.Add(shareModel);
                }
                var res = ProjDtlShareService.Save<Int64>(shareModels,"");

                //修改的 
                shareModels.Clear();
                foreach (var budgetDtlModel in projectdtlbudgetdtlgridinfo.ModifyRow)
                {
                    var findRes = ProjDtlShareService.Find(t => t.DM == projCode && t.XMDM == budgetDtlModel.FDtlCode);
                    if (findRes.Data.Count > 0) {
                        shareModel = findRes.Data[0];
                        shareModel.MC = budgetDtlModel.FName;
                        shareModel.PersistentState = PersistentState.Modified;
                        shareModels.Add(shareModel);
                    }
                }
                res = ProjDtlShareService.Save<Int64>(shareModels,"");

                //删除的
                if (projShareDeleteList.Count > 0)
                {
                    //res = ProjDtlShareService.Save<Int64>(projShareDeleteList);
                    foreach (var m in projShareDeleteList) {
                        //删除项目库对应项目
                        var dicWhere = new Dictionary<string, object>();
                        new CreateCriteria(dicWhere)
                            .Add(ORMRestrictions<string>.Eq("DM", m.DM))
                            .Add(ORMRestrictions<string>.Eq("XMDM", m.XMDM)); //闭区间
                        var result = ProjDtlShareService.Delete(dicWhere);
                    }
                }
                #endregion*/

                //年中调整时,保存原来的单据
                if (midEdit == "midEdit" && id != 0 && savedresult.Status == ResponseStatus.Success)
                {
                    var budgetdtlimplplan = new List<ProjectDtlImplPlanModel>();
                    foreach (var item in findedresultbudgetdtlimplplan.Data)
                    {
                        item.PersistentState = PersistentState.Added;
                        budgetdtlimplplan.Add(item);
                    }
                    var budgetdtltextcontent = new List<ProjectDtlTextContentModel>();
                    foreach (var item in findedresultbudgetdtltextcontent.Data)
                    {
                        item.PersistentState = PersistentState.Added;
                        budgetdtltextcontent.Add(item);
                    }
                    var budgetdtlfundappl = new List<ProjectDtlFundApplModel>();
                    foreach (var item in findedresultbudgetdtlfundappl.Data)
                    {
                        item.PersistentState = PersistentState.Added;
                        budgetdtlfundappl.Add(item);
                    }
                    var budgetdtlbudgetdtl = new List<ProjectDtlBudgetDtlModel>();
                    foreach (var item in findedresultbudgetdtlbudgetdtl.Data)
                    {
                        item.PersistentState = PersistentState.Added;
                        budgetdtlbudgetdtl.Add(item);
                    }

                    var Purchasedtl = new List<ProjectDtlPurchaseDtlModel>();
                    foreach (var item in findedresultPurchasedtl.Data)
                    {
                        item.PersistentState = PersistentState.Added;
                        Purchasedtl.Add(item);
                    }

                    var PurDtl4SOF = new List<ProjectDtlPurDtl4SOFModel>();
                    foreach (var item in findedresultPurDtl4SOF.Data)
                    {
                        item.PersistentState = PersistentState.Added;
                        PurDtl4SOF.Add(item);
                    }

                    var performtargetgridinfo = new List<ProjectDtlPerformTargetModel>();
                    foreach (var item in findedresultPerformTarget.Data)
                    {
                        item.PersistentState = PersistentState.Added;
                        performtargetgridinfo.Add(item);
                    }

                  

                    ProjectMstService.SaveProjectMst(findedresultmst, budgetdtltextcontent, Purchasedtl, PurDtl4SOF, performtargetgridinfo, budgetdtlfundappl, budgetdtlbudgetdtl,  budgetdtlimplplan);
                  

                    //ProjectMstService.SaveProjectMst(findedresultmst, budgetdtlimplplan, budgetdtltextcontent, budgetdtlfundappl, budgetdtlbudgetdtl);
                    //var findmstforminfo = DataConverterHelper.JsonToEntity<BudgetMstModel>(mstformData);
                    //var udgetdtlimplplan = DataConverterHelper.EntityListToJson(findedresultbudgetdtlimplplan, findedresultbudgetdtlimplplan.Count);
                    //var findbudgetdtlimplplangridinfo = DataConverterHelper.JsonToEntity<BudgetDtlImplPlanModel>(udgetdtlimplplan);
                    //var budgetdtltext = DataConverterHelper.EntityListToJson(findedresultbudgetdtltextcontent, findedresultbudgetdtltextcontent.Count);
                    //var findbudgetdtltextcontentgridinfo = DataConverterHelper.JsonToEntity<BudgetDtlTextContentModel>(budgetdtltext);
                    //var budgetdtlfundappl = DataConverterHelper.EntityListToJson(findedresultbudgetdtlfundappl, findedresultbudgetdtlfundappl.Count);
                    //var findbudgetdtlfundapplgridinfo = DataConverterHelper.JsonToEntity<BudgetDtlFundApplModel>(budgetdtlfundappl);
                    //var budgetdtlbudgetdtl = DataConverterHelper.EntityListToJson(findedresultbudgetdtlbudgetdtl, findedresultbudgetdtlbudgetdtl.Count);
                    //var findbudgetdtlbudgetdtlgridinfo = DataConverterHelper.JsonToEntity<BudgetDtlBudgetDtlModel>(budgetdtlbudgetdtl);
                    //BudgetMstService.SaveBudgetMst(findedresultmst, findbudgetdtlimplplangridinfo.AllRow, findbudgetdtltextcontentgridinfo.AllRow, findbudgetdtlfundapplgridinfo.AllRow, findbudgetdtlbudgetdtlgridinfo.AllRow);
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
            string projCode = System.Web.HttpContext.Current.Request.Params["projcode"];    //项目代码
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]); //主表主键

            var findedresultmst = ProjectMstService.Find(id).Data;
            //根据项目代码去项目表里查找相同代码的条数,得知相关版本号
            var dicWhereLife = new Dictionary<string, object>();
            new CreateCriteria(dicWhereLife).Add(ORMRestrictions<string>.Eq("FProjCode", findedresultmst.FProjCode));
            //var FLifeCycle = ProjectMstService.Find(dicWhereLife);

            var deletedresult = ProjectMstService.Delete<System.Int64>(id);
            //相同项目代码只有一条时,表示没做过调整,则直接删除;多于一条时,则找版本号最大(即最新)那条单据,版本号改为0
            /*if (FLifeCycle.Data.Count == 1)
            {
                if (deletedresult.Status == ResponseStatus.Success)
                {
                    //删除项目库对应项目
                    var dicWhere = new Dictionary<string, object>();
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<string>.Eq("DM", projCode)); //闭区间
                    var result = ProjLibService.Delete(dicWhere);

                    var result2 = ProjDtlShareService.Delete(dicWhere); //删除对应的项目明细共享(JJ_FXGL)
                }
            }
            else
            {
                var dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<string>.Eq("FProjCode", projCode)).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", FLifeCycle.Data.Count - 1));
                var oldXmList = ProjectMstService.Find(dicWhere).Data[0];
                oldXmList.FLifeCycle = 0;
                oldXmList.PersistentState = PersistentState.Modified;
                ProjectMstService.Save<Int64>(oldXmList, "");
            }*/
            
            if (deletedresult.Status == ResponseStatus.Success)
            {
                new CreateCriteria(dicWhereLife).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0));
                var data = ProjectMstService.Find(dicWhereLife).Data;
                if (data.Count <= 1)
                {
                    //删除项目库对应项目
                    var dicWhere = new Dictionary<string, object>();
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<string>.Eq("DM", projCode)); //闭区间
                    var result = ProjLibService.Delete(dicWhere);

                    var result2 = ProjDtlShareService.Delete(dicWhere); //删除对应的项目明细共享(JJ_FXGL)
                }
            }





            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 判断是否启用工作流
        /// </summary>
        /// <param name="bizid"></param>
        /// <param name="ocode"></param>
        /// <returns></returns>
        public bool GetBizApproveFlag(string bizid, string ocode) {
            return WorkFlowHelper.GetBizApproveFlag(bizid, ocode);
        }


        /// <summary>
        /// 获取项目树数据
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="OrgCode"></param>
        /// <param name="DeptCode"></param>
        /// <returns></returns>
        public IList<TreeJSONBase> GetProjTreeData(string nodeid,string OrgCode,string DeptCode)
        {

            FindedResults<ProjLibProjModel> results = null;
            if (string.IsNullOrEmpty(nodeid) || nodeid == "root")
            {
                results = ProjLibService.Find(t => (t.DEFSTR2 == null || t.DEFSTR2 == "") && t.DEFSTR3== OrgCode && t.DEFSTR4 == DeptCode);
            }
            else {
                results = ProjLibService.Find(t => t.DEFSTR2 == nodeid && t.DEFSTR3 == OrgCode && t.DEFSTR4 == DeptCode);
            }

            if (results.Data.Count > 0) {
                DataTable dt = new DataTable();
                dt.Columns.Add("PhId", typeof(System.String));
                dt.Columns.Add("DM", typeof(System.String));
                dt.Columns.Add("MC", typeof(System.String));
                dt.Columns.Add("DEFSTR1", typeof(System.String));
                dt.Columns.Add("DEFSTR2", typeof(System.String));

                DataRow dr = null;
                foreach (var m in results.Data) {
                    dr = dt.NewRow();
                    dr["PhId"] = m.PhId;
                    dr["DM"] = m.DM;
                    dr["MC"] = m.MC;
                    dr["DEFSTR1"] = m.DEFSTR1;
                    dr["DEFSTR2"] = m.DEFSTR2;
                    dt.Rows.Add(dr);
                }

                string filter = string.Empty;
                return new ProjTreeBuilder().GetExtTreeList(dt, "DEFSTR2", "DM", filter, TreeDataLevelType.LazyLevel);
            }

            return null;
        }

        /// <summary>
        /// 返回项目树Json
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadProjTree() {
            string nodeid = System.Web.HttpContext.Current.Request.Params["node"];
            string OrgCode = System.Web.HttpContext.Current.Request.Params["OrgCode"];
            string DeptCode = System.Web.HttpContext.Current.Request.Params["DeptCode"];
            IList<TreeJSONBase> list = this.GetProjTreeData(nodeid, OrgCode, DeptCode);

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 项目生成预算
        /// </summary>
        /// <returns></returns>
        public string SaveBudgetMst()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            var findedresultmst = ProjectMstService.Find(id);
            var findedresultprojectdtlimplplan = ProjectMstService.FindProjectDtlImplPlanByForeignKey(id);
            var findedresultprojectdtltextcontent = ProjectMstService.FindProjectDtlTextContentByForeignKey(id);
            var findedresultprojectdtlfundappl = ProjectMstService.FindProjectDtlFundApplByForeignKey(id);
            var findedresultprojectdtlbudgetdtl = ProjectMstService.FindProjectDtlBudgetDtlByForeignKey(id);
            var findedresultprojectdtlpurchasedtl = ProjectMstService.FindProjectDtlPurchaseDtlByForeignKey(id);
            var findedresultprojectdtlpurdtl4sof = ProjectMstService.FindProjectDtlPurDtl4SOFByForeignKey(id);
            var findedresultprojectdtlPerformTarget = ProjectMstService.FindProjectDtlPerformTargetByForeignKey(id);

            var budgetmst = ModelChange<ProjectMstModel, BudgetMstModel>(findedresultmst.Data);
            decimal FBudgetAmount = 0;
            //budgetmst.FDeclarer = "";
            //budgetmst.FDateofDeclaration = DateTime.Now;
            //budgetmst.FMeetingTime = null;
            //budgetmst.FMeetiingSummaryNo = "";
            budgetmst.FProjStatus = 3;
            budgetmst.XmMstPhid = findedresultmst.Data.PhId;
            // budgetmst.FApproveStatus ="3";  //项目生成预算审批状态不变
            //查找预算单位下该部门所处预算进度
            var processStatus = BudgetProcessCtrlService.FindBudgetProcessCtrl(budgetmst.FDeclarationUnit, budgetmst.FBudgetDept,budgetmst.FYear);
            if(processStatus == "1")
            {
                budgetmst.FType = "c";
                budgetmst.FVerNo = "0001";
            }
            else if (processStatus == "3")
            {
                //年中调整时,如果该单据已存在于预算中,则为年中调整,数据类型(FType)跟原来一样
                var projCode1 = budgetmst.FProjCode;
                var dicWhereLife1 = new Dictionary<string, object>();
                new CreateCriteria(dicWhereLife1).Add(ORMRestrictions<string>.Eq("FProjCode", projCode1))
                    .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));
                var Ftype1 = BudgetMstService.Find(dicWhereLife1);
                if(Ftype1.Data.Count > 0)
                {
                    budgetmst.FType = Ftype1.Data[0].FType;
                }
                else
                {
                    budgetmst.FType = "z";
                }

                //budgetmst.FType = "z";
                budgetmst.FVerNo = "0002";
            }

            budgetmst.FMidYearChange = "0";
            budgetmst.FBudgetAmount = budgetmst.FProjAmount;
            budgetmst.PersistentState = PersistentState.Added;

            //model.PersistentState = PersistentState.Added;
            // budgetmst.Add(model);

            var budgetdtlimplplan = new List<BudgetDtlImplPlanModel>();
            foreach (var item in findedresultprojectdtlimplplan.Data)
            {
                var model = ModelChange<ProjectDtlImplPlanModel, BudgetDtlImplPlanModel>(item);
                model.PersistentState = PersistentState.Added;
                budgetdtlimplplan.Add(model);
            }

            var budgetdtltextcontent = new List<BudgetDtlTextContentModel>();
            foreach (var item in findedresultprojectdtltextcontent.Data)
            {
                var model = ModelChange<ProjectDtlTextContentModel, BudgetDtlTextContentModel>(item);
                model.PersistentState = PersistentState.Added;
                budgetdtltextcontent.Add(model);
            }

            var budgetdtlfundappl = new List<BudgetDtlFundApplModel>();
            foreach (var item in findedresultprojectdtlfundappl.Data)
            {
                var model = ModelChange<ProjectDtlFundApplModel, BudgetDtlFundApplModel>(item);
                model.PersistentState = PersistentState.Added;
                budgetdtlfundappl.Add(model);
            }

            var budgetdtlpurchasedtl = new List<BudgetDtlPurchaseDtlModel>();
            foreach (var item in findedresultprojectdtlpurchasedtl.Data)
            {
                var model = ModelChange<ProjectDtlPurchaseDtlModel, BudgetDtlPurchaseDtlModel>(item);
                model.PersistentState = PersistentState.Added;
                budgetdtlpurchasedtl.Add(model);
            }

            var budgetdtlpurdtl4sof = new List<BudgetDtlPurDtl4SOFModel>();
            foreach (var item in findedresultprojectdtlpurdtl4sof.Data)
            {
                var model = ModelChange<ProjectDtlPurDtl4SOFModel, BudgetDtlPurDtl4SOFModel>(item);
                model.PersistentState = PersistentState.Added;
                budgetdtlpurdtl4sof.Add(model);
            }

            var budgetdtlbudgetdtl = new List<BudgetDtlBudgetDtlModel>();
            var oldxm3BudgetDtl = new List<ProjectDtlBudgetDtlModel>();
            foreach (var item in findedresultprojectdtlbudgetdtl.Data)
            {
                var model = ModelChange<ProjectDtlBudgetDtlModel, BudgetDtlBudgetDtlModel>(item);
                model.Xm3_DtlPhid = item.PhId;
                model.FBudgetAmount = item.FAmount;
                model.PersistentState = PersistentState.Added;
                budgetdtlbudgetdtl.Add(model);
                item.FBudgetAmount = item.FAmount;  //生成预算时回填项目里的预算金额
                item.PersistentState = PersistentState.Modified;
                FBudgetAmount += item.FAmount;
                oldxm3BudgetDtl.Add(item);
            }
            SavedResult<Int64> savedresult = new SavedResult<Int64>();


            var budgetdtlPerformTarget = new List<BudgetDtlPerformTargetModel>(); //暂时申明，未做数据处理 by LiMing 2018.10.18
            foreach (var item in findedresultprojectdtlPerformTarget.Data)
            {
                var model = ModelChange<ProjectDtlPerformTargetModel, BudgetDtlPerformTargetModel>(item);
                model.MstPhId = 0;
                model.PersistentState = PersistentState.Added;
                budgetdtlPerformTarget.Add(model);
            }

            try
            {
                //生成预算时查找有没有当前项目ID,版本为0的预算,有 则保存原来的数据(FLifeCycle + 1),新增一条
                var dicWhere1 = new Dictionary<string, object>();
                new CreateCriteria(dicWhere1)
                    .Add(ORMRestrictions<string>.Eq("FProjCode", findedresultmst.Data.FProjCode)).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
                    .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0")); //闭区间
                var findBudget =  BudgetMstService.Find(dicWhere1);
                long oldBudgetPhid = 0;
                if(findBudget.Data.Count > 0)
                {
                    oldBudgetPhid = findBudget.Data[0].PhId;
                }

                //向老g6h同步数据
                if (findedresultmst.Data.FSaveToOldG6h != 1)
                {
                    string TBresult = BudgetMstService.AddDataInSaveBudgetMst(budgetmst, budgetdtlbudgetdtl);
                    if (TBresult!= "")
                    {
                        //savedresult.Status = ResponseStatus.Error;
                        savedresult.Msg = TBresult;
                        //return DataConverterHelper.SerializeObject(savedresult);
                    }
                    else
                    {
                        findedresultmst.Data.FSaveToOldG6h = 1;
                        findedresultmst.Data.PersistentState = PersistentState.Modified;
                        //ProjectMstService.Save<Int64>(findedresultmst.Data,"");
                        budgetmst.FSaveToOldG6h = 1;
                    }
                }

                //savedresult = BudgetMstService.SaveBudgetMst(budgetmst, budgetdtlimplplan, budgetdtltextcontent, budgetdtlfundappl, budgetdtlbudgetdtl);
                savedresult = BudgetMstService.SaveBudgetMst(budgetmst, budgetdtlimplplan, budgetdtltextcontent, budgetdtlfundappl, budgetdtlbudgetdtl, budgetdtlPerformTarget, budgetdtlpurchasedtl, budgetdtlpurdtl4sof, null);

                //生成预算时,如果原来已经有相关项目的预算了,则原来的预算版本好加1(即从列表区隐藏掉),重新生成预算
                if (findBudget.Data.Count > 0 && savedresult.Status == ResponseStatus.Success)
                {

                    //根据项目代码去预算表里查找相同代码的条数,得知相关版本号
                    var dicWhereLife = new Dictionary<string, object>();
                    new CreateCriteria(dicWhereLife).Add(ORMRestrictions<string>.Eq("FProjCode", findBudget.Data[0].FProjCode));
                    var FLifeCycle = BudgetMstService.Find(dicWhereLife);

                    findBudget.Data[0].FLifeCycle = FLifeCycle.Data.Count;
                    findBudget.Data[0].PersistentState = PersistentState.Modified;
                    BudgetMstService.Save<Int64>(findBudget.Data[0],"");//

                }

            }
            catch (Exception ex)
            {
                savedresult.Status = ResponseStatus.Error;
                savedresult.Msg = ex.Message.ToString();
            }
            if (savedresult.Status == ResponseStatus.Success) //生成成功后改变项目状态
            {
                findedresultmst.Data.FBudgetAmount = FBudgetAmount;
                findedresultmst.Data.FProjStatus = 3;
                findedresultmst.Data.PersistentState = PersistentState.Modified;
                ProjectMstService.Save<Int64>(findedresultmst.Data,"");
                ProjectMstService.UpdateBudgetDtlList(oldxm3BudgetDtl);
                //对应关系设置-预算库项目对应部门设置,对应关系存放在z_qtdygx中，dylx=’98’
                var dwdm = budgetmst.FProjCode;
                var dydm = budgetmst.FBudgetDept;
                var dicWhere = new Dictionary<string, object>(); 
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
                    dygxModel.PersistentState = PersistentState.Added;
                    CorrespondenceSettingsService.Save<Int64>(dygxModel,"");
                }
                
            }

            return DataConverterHelper.SerializeObject(savedresult);

        }

        /// <summary>
        /// model 转换
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static T2 ModelChange<T1, T2>(T1 model)
        {
            if (model == null)
            {
                T2 obj1 = Activator.CreateInstance<T2>();
                return obj1;
            }
            Type t = model.GetType();   //获取传过来的实例类型
            var fullname = t.FullName; //获取类型名称
            //ProjectMstModel obj1 = new ProjectMstModel();
            
            // ProjectMstModel obj = Activator.CreateInstance<ProjectMstModel>();
            PropertyInfo[] PropertyList = t.GetProperties(); //获取实例的属性

            T2 obj = Activator.CreateInstance<T2>(); //创建T2映射的实例
            Type o = obj.GetType();
            PropertyInfo[] objList = o.GetProperties();
            foreach (PropertyInfo item in PropertyList)
            {
                //var name = item.Name;
                //var value = item.GetValue(model, null); //获取item在model中的值
                if (item.DeclaringType.FullName != fullname)  //；类型名称不同的跳过
                {
                    continue;
                }
                foreach (var ob in objList) //循环查找相同属性名并赋值
                {
                    if (ob.Name.ToLower() == item.Name.ToLower() && ob.PropertyType.Name == item.PropertyType.Name)  //名称相同且属性相同的赋值
                    {
                        ob.SetValue(obj, item.GetValue(model, null), null);
                        break;
                    }
                }
            }
            return obj; //返回转换后并赋值的model
        }

        /// <summary>
        /// 取消审批
        /// </summary>
        /// <returns></returns>
        public string FindUnvalidPiid()
        {
            var approveCode = System.Web.HttpContext.Current.Request.Params["approveCode"]; ;
            var userId = System.Web.HttpContext.Current.Request.Params["userId"];
            var  piid = ProjectMstService.FindUnvalidPiid(approveCode,  userId);
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            savedresult.Status = ResponseStatus.Success;
            if(piid.Results.Count > 0)
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
        /// 取消审批
        /// </summary>
        /// <returns></returns>
        public string FindUnvalid()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            var findedresultmst = ProjectMstService.Find(id).Data;
            findedresultmst.FApproveStatus = "2";
            findedresultmst.FDateofDeclaration = DateTime.Now;
            findedresultmst.PersistentState = PersistentState.Modified;
            ProjectMstService.Save<Int64>(findedresultmst,"");
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            savedresult.Status = ResponseStatus.Success;
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 项目同步数据到老G6H
        /// </summary>
        /// <returns></returns>
        public string AddData()
        {
            var id = System.Web.HttpContext.Current.Request.Params["id"];  //主表主键
            //List<long> ids = JsonConvert.DeserializeObject<List<long>>(id);
            var ids = id.Split(',');
            string result = ProjectMstService.AddData(ids);
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
        /// 取没同步到老G6H里，是项目立项已审批的数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetProjectMstListToSaveOldG6h()
        {
            var dicWhere = new Dictionary<string, object>();

            var FApproveStatus = System.Web.HttpContext.Current.Request.Params["FApproveStatus"];
            //增加年度过滤
            var FYear = System.Web.HttpContext.Current.Request.Params["FYear"];

            var ProjStatus = System.Web.HttpContext.Current.Request.Params["ProjStatus"];
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
            ////在“初报完成”和“调整完成”阶段，不允许用户进行【同步数据】操作
            //根据操作员部门列表查找对应部门进度状态
            var budgetProcessList = BudgetProcessCtrlService.FindBudgetProcessCtrlByList(deptL, FYear);
            List<string> deptLWithPro = new List<string>();
            for (var i = 0; i< budgetProcessList.Data.Count;i ++)
            {
                //取进度状态在年初或年中调整的部门
                if(budgetProcessList.Data[i].FProcessStatus == "1" || budgetProcessList.Data[i].FProcessStatus == "3")
                {
                    deptLWithPro.Add(budgetProcessList.Data[i].FDeptCode); 
                }
            }

            new CreateCriteria(dicWhere).Add(ORMRestrictions<Int32>.Eq("FProjStatus", 2)).Add(ORMRestrictions<string>.Eq("FApproveStatus", "3"))
                .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptLWithPro))
                .Add(ORMRestrictions<Int32>.Eq("FSaveToOldG6h", 0)).Add(ORMRestrictions<string>.Eq("FGoYear", FYear));


            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = ProjectMstService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere, new string[] { "FProjCode Asc" });

            return DataConverterHelper.EntityListToJson<ProjectMstModel>(result.Results, (Int32)result.TotalItems);
        }
   
        /// <summary>
        /// 根据code判断该单据是否存在
        /// </summary>
        /// <returns></returns>
        public string JudgeIf()
        {
            var code = System.Web.HttpContext.Current.Request.Params["code"];
            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("FProjCode", code));
            IList<ProjectMstModel> projects = ProjectMstService.Find(dicWhere).Data;
            if (projects.Count > 0)
            {
                return "1";//已经录过预算数据
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 项目分级修改
        /// </summary>
        /// <returns></returns>
        public string UpdateMC()
        {
            string DM = System.Web.HttpContext.Current.Request.Params["DM"];
            string MC = System.Web.HttpContext.Current.Request.Params["MC"];
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            var dicWhere1 = new Dictionary<string, object>();
            new CreateCriteria(dicWhere1)
                .Add(ORMRestrictions<string>.Eq("DM", DM));
            IList<ProjLibProjModel> projLibProjModels = ProjLibService.Find(dicWhere1).Data;
            if (projLibProjModels.Count > 0)
            {
                projLibProjModels[0].MC = MC;
                projLibProjModels[0].PersistentState = PersistentState.Modified;
                savedresult=ProjLibService.Save<Int64>(projLibProjModels[0],"");
            }
            var dicWhere2 = new Dictionary<string, object>();
            new CreateCriteria(dicWhere2)
                .Add(ORMRestrictions<string>.Eq("FProjCode", DM));
            IList<ProjectMstModel> projects = ProjectMstService.Find(dicWhere2).Data;
            if (projects.Count > 0)
            {
                projects[0].FProjName = MC;
                projects[0].PersistentState = PersistentState.Modified;
                savedresult=ProjectMstService.Save<Int64>(projects[0], "");
            }
            return DataConverterHelper.SerializeObject(savedresult);
        }


        /// <summary>
        /// 获取安装位置，去判断单点登录获取路径
        /// </summary>
        /// <returns></returns>
        public string GetSetupPath()
        {
            string SetupPath = ConfigHelper.GetString("SetupPath");
            return SetupPath;
            
        }


        /// <summary>
        /// 项目转预算判断必录项(预算科目,支出渠道如果不点击修改,不能判断有没有录,故再判断一下)
        /// </summary>
        /// <returns></returns>
        public string GetMustInput()
        {
            //long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            var ids = System.Web.HttpContext.Current.Request.Params["id"];
            var idList = ids.Split(',');
            var newIds = "";
            var allInput = "";
            for (var i = 0; i < idList.Length; i++)
            {
                allInput = "";
                long id = Convert.ToInt64(idList[i]);
                var findedresultprojectdtlbudgetdtl = ProjectMstService.FindProjectDtlBudgetDtlByForeignKey(id);
                for (var j = 0; j < findedresultprojectdtlbudgetdtl.Data.Count; j++)
                {
                    if (string.IsNullOrEmpty(findedresultprojectdtlbudgetdtl.Data[j].FBudgetAccounts) || string.IsNullOrEmpty(findedresultprojectdtlbudgetdtl.Data[j].FExpensesChannel))
                    {
                        allInput = "not";
                        break;
                    }

                }
                if (allInput == "")
                {
                    newIds += idList[i] + ',';
                }

            }
            if (newIds.Length > 0)
            {
                newIds = newIds.Substring(0, newIds.Length - 1);
            }
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            savedresult.Status = ResponseStatus.Success;
            savedresult.Msg = newIds;
          

            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 查找是否有设置功能控制
        /// </summary>
        /// <returns></returns>
        public string FindQTControlSet()
        {
            var BZ = System.Web.HttpContext.Current.Request.Params["BZ"];
            var DWDM = System.Web.HttpContext.Current.Request.Params["DWDM"];
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            List<CorrespondenceSettings2Model> correspondenceSettings2Models = ProjectMstService.FindQTControlSet(BZ, DWDM);
            
            if (correspondenceSettings2Models.Count > 0)
            {
                savedresult.Msg = "true";
            }
            else
            {
                savedresult.Msg = "false";
            }

            savedresult.Status = ResponseStatus.Success;
            


            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 
        /// </summary>
        public string SaveNextApprove()
        {
            var activeName = System.Web.HttpContext.Current.Request.Params["activeName"];
            var id = System.Web.HttpContext.Current.Request.Params["ID"];
            var biztypeName = System.Web.HttpContext.Current.Request.Params["biztypeName"];
            
            switch (biztypeName)
            {
                case "GHProject"://项目
                    var FindProjectList = ProjectMstService.Find(id).Data;
                    FindProjectList.FNextApprove = activeName;
                    FindProjectList.PersistentState = PersistentState.Modified;
                    ProjectMstService.Save<Int64>(FindProjectList, "");
                    break;
                case "GHBudget":   //预算
                    var FindBudgetList = BudgetMstService.Find(id).Data;
                    FindBudgetList.FNextApprove = activeName;
                    FindBudgetList.PersistentState = PersistentState.Modified;
                    BudgetMstService.Save<Int64>(FindBudgetList, "");
                    break;
                case "GHSubject":   //基本支出
                    var FindSubjectList = GHSubjectService.Find(id).Data;
                    FindSubjectList.FNextApprove = activeName;
                    FindSubjectList.PersistentState = PersistentState.Modified;
                    GHSubjectService.Save<Int64>(FindSubjectList, "");
                    break;
                case "GHExpense":   //用款计划
                    var FindExpenseList = ExpenseMstService.Find(id).Data;
                    FindExpenseList.FNextApprove = activeName;
                    FindExpenseList.PersistentState = PersistentState.Modified;
                    ExpenseMstService.Save<Int64>(FindExpenseList, "");
                    break;
                default:
                    break;
            }

       

            return "";
        }

        /// <summary>
        /// 审批流驳回
        /// </summary>
        /// <returns></returns>
        public string SaveRollBack()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);
            var biztypeName = System.Web.HttpContext.Current.Request.Params["biztypeName"];

            SavedResult<Int64> savedresult = new SavedResult<Int64>();

            switch (biztypeName)
            {
                case "GHProject"://项目
                    var FindProjectList = ProjectMstService.Find(id).Data;
                    FindProjectList.FApproveStatus = "4";//已退回
                    FindProjectList.PersistentState = PersistentState.Modified;
                    savedresult = ProjectMstService.Save<Int64>(FindProjectList, "");
                    break;
                case "GHBudget":   //预算
                    var FindBudgetList = BudgetMstService.Find(id).Data;
                    FindBudgetList.FApproveStatus = "4";//已退回
                    FindBudgetList.PersistentState = PersistentState.Modified;
                    savedresult = BudgetMstService.Save<Int64>(FindBudgetList, "");
                    break;
                case "GHSubject":   //基本支出
                    var FindSubjectList = GHSubjectService.Find(id).Data;
                    FindSubjectList.FApproveStatus = "6";//已退回
                    FindSubjectList.PersistentState = PersistentState.Modified;
                    savedresult = GHSubjectService.Save<Int64>(FindSubjectList, "");
                    break;
                case "GHExpense":   //用款计划
                    var FindExpenseList = ExpenseMstService.Find(id).Data;
                    FindExpenseList.FApprovestatus = "5";//已退回
                    FindExpenseList.PersistentState = PersistentState.Modified;
                    savedresult = ExpenseMstService.Save<Int64>(FindExpenseList, "");
                    break;
                default:
                    break;
            }
            return DataConverterHelper.SerializeObject(savedresult);

        }


        /// <summary>
        /// 取可批量生成预算的单据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Getbatchbudget()
        {
            

            var FApproveStatus = System.Web.HttpContext.Current.Request.Params["FApproveStatus"];

            var ProjStatus = System.Web.HttpContext.Current.Request.Params["ProjStatus"];
            
            var userId = System.Web.HttpContext.Current.Request.Params["userId"];
            //增加根据操作员对应预算部门的过滤
            var dicWhereDept = new Dictionary<string, object>();
            new CreateCriteria(dicWhereDept)
                .Add(ORMRestrictions<string>.Eq("Dwdm", userId)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
            var deptList = CorrespondenceSettingsService.Find(dicWhereDept);
            List<string> deptL = new List<string>();
            for (var i = 0; i < deptList.Data.Count; i++)
            {
                deptL.Add(deptList.Data[i].Dydm);
            }
            ////在“初报完成”和“调整完成”阶段，不允许用户进行【生成预算】操作

            var dicWhereDept2 = new Dictionary<string, object>();
            new CreateCriteria(dicWhereDept2)
                .Add(ORMRestrictions<List<string>>.In("FProcessStatus", new List<string> { "1", "3" }));
            IList<BudgetProcessCtrlModel> budgetProcessList = BudgetProcessCtrlService.Find(dicWhereDept2).Data;
            List<string> deptL2 = new List<string>();
            for (var j=0;j< budgetProcessList.Count; j++)
            {
                if (deptL.Contains(budgetProcessList[j].FDeptCode))
                {
                    deptL2.Add(budgetProcessList[j].FDeptCode);
                }
            }

            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<Int32>.NotEq("FProjStatus", 3)).Add(ORMRestrictions<string>.Eq("FApproveStatus", "3"))
                .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL2));


            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = ProjectMstService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere, new string[] { "FProjCode Asc" });

            return DataConverterHelper.EntityListToJson<ProjectMstModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 判断当前选则的模本金额跟实际录入金额的大小比较
        /// </summary>
        /// <returns></returns>
        public  string FindIndividualInfo()
        {
            var busType = System.Web.HttpContext.Current.Request.Params["busType"];
            var IndividualInfoId = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["IndividualInfoId"]);//传的是单据主键
            var projAmount = Convert.ToDecimal(System.Web.HttpContext.Current.Request.Params["projAmount"]);
            var OrgCode = System.Web.HttpContext.Current.Request.Params["OrgCode"];

            var new_id = ProjectMstService.FindIndividualInfo(busType, IndividualInfoId, projAmount, OrgCode);

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            savedresult.Status = ResponseStatus.Success;
            savedresult.Msg = new_id;


            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 项目生成快照
        /// </summary>
        /// <returns></returns>
        public string SaveSnapshot()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            string FDtlstage = System.Web.HttpContext.Current.Request.Params["FDtlstage"]; 
            var findedresultmst = ProjectMstService.Find(id);
            var findedresultprojectdtlbudgetdtl = ProjectMstService.FindProjectDtlBudgetDtlByForeignKey(id);
            var findedresultprojectdtlfundappl = ProjectMstService.FindProjectDtlFundApplByForeignKey(id);
            var findedresultprojectdtlimplplan = ProjectMstService.FindProjectDtlImplPlanByForeignKey(id);
            var findedresultprojectdtltextcontent = ProjectMstService.FindProjectDtlTextContentByForeignKey(id);
            var findedresultprojectdtlpurchasedtl = ProjectMstService.FindProjectDtlPurchaseDtlByForeignKey(id);
            var findedresultprojectdtlpurdtl4sof = ProjectMstService.FindProjectDtlPurDtl4SOFByForeignKey(id);
            var findedresultprojectdtlPerformTarget = ProjectMstService.FindProjectDtlPerformTargetByForeignKey(id);

            var mst = ModelChange<ProjectMstModel, QTProjectMstModel>(findedresultmst.Data);
            //查找预算单位下该部门所处预算进度
            mst.FProcessstatus = BudgetProcessCtrlService.FindBudgetProcessCtrl(mst.FDeclarationUnit, mst.FBudgetDept, mst.FYear);
            mst.FTemporarydate = DateTime.Now;
            mst.FDtlstage = FDtlstage;
            mst.PersistentState = PersistentState.Added;

            var budgetdtls = new List<QTProjectDtlBudgetDtlModel>();
            foreach (var item in findedresultprojectdtlbudgetdtl.Data)
            {
                var model = ModelChange<ProjectDtlBudgetDtlModel, QTProjectDtlBudgetDtlModel>(item);
                model.PersistentState = PersistentState.Added;
                budgetdtls.Add(model);
            }
            var fundappls = new List<QTProjectDtlFundApplModel>();
            foreach (var item in findedresultprojectdtlfundappl.Data)
            {
                var model = ModelChange<ProjectDtlFundApplModel, QTProjectDtlFundApplModel>(item);
                model.PersistentState = PersistentState.Added;
                fundappls.Add(model);
            }
            var implplans = new List<QTProjectDtlImplPlanModel>();
            foreach (var item in findedresultprojectdtlimplplan.Data)
            {
                var model = ModelChange<ProjectDtlImplPlanModel, QTProjectDtlImplPlanModel>(item);
                model.PersistentState = PersistentState.Added;
                implplans.Add(model);
            }

            var PerformTargets = new List<QTProjectDtlPerformTargetModel>(); 
            foreach (var item in findedresultprojectdtlPerformTarget.Data)
            {
                var model = ModelChange<ProjectDtlPerformTargetModel, QTProjectDtlPerformTargetModel>(item);
                model.PersistentState = PersistentState.Added;
                PerformTargets.Add(model);
            }

            var purchasedtls = new List<QTProjectDtlPurchaseDtlModel>();
            foreach (var item in findedresultprojectdtlpurchasedtl.Data)
            {
                var model = ModelChange<ProjectDtlPurchaseDtlModel, QTProjectDtlPurchaseDtlModel>(item);
                model.PersistentState = PersistentState.Added;
                purchasedtls.Add(model);
            }

            var purdtl4sofs = new List<QTProjectDtlPurDtl4SOFModel>();
            foreach (var item in findedresultprojectdtlpurdtl4sof.Data)
            {
                var model = ModelChange<ProjectDtlPurDtl4SOFModel, QTProjectDtlPurDtl4SOFModel>(item);
                model.PersistentState = PersistentState.Added;
                purdtl4sofs.Add(model);
            }

            var textcontent = new List<QTProjectDtlTextContentModel>();
            foreach (var item in findedresultprojectdtltextcontent.Data)
            {
                var model = ModelChange<ProjectDtlTextContentModel, QTProjectDtlTextContentModel>(item);
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
        /// 引用上年项目
        /// </summary>
        /// <returns></returns>
        public string SaveXMreference()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            var FGoYear = System.Web.HttpContext.Current.Request.Params["FGoYear"];
            var Msg = "";
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            var mst = BudgetMstService.Find(id).Data;
            if (mst.FIfPerformanceAppraisal == EnumYesNo.Yes)
            {
                //若需要绩效评价判断是否存在已上报的自评单据
                var dicWherejx = new Dictionary<string, object>();
                new CreateCriteria(dicWherejx).Add(ORMRestrictions<Int64>.Eq("YSMstPhId", id))
                    .Add(ORMRestrictions<string>.Eq("FAuditStatus", "2"));
                var result = PerformanceMstService.Find(dicWherejx).Data;
                if (result.Count == 0)
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = "该项目上年度未进行项目自评，请先完成自评！";

                    return DataConverterHelper.SerializeObject(savedresult);
                }
                else
                {
                    if (result[0].FEvaluationResult == "差")
                    {
                        savedresult.Status = ResponseStatus.Error;
                        savedresult.Msg = "因该项目上年度绩效自评结果为差，故本年不予进行项目编报！具体请与财务部联系！";

                        return DataConverterHelper.SerializeObject(savedresult);
                    }
                    if (result[0].FEvaluationResult == "一般")
                    {
                        //savedresult.Status = ResponseStatus.Error;
                        Msg = "该项目上年度绩效评价结果为‘一般’，若本年度评价结果仍为‘一般’时，将有取消该项目预算资金的风险！";

                        //return DataConverterHelper.SerializeObject(savedresult);
                    }
                }
            }

            var dicWherexm = new Dictionary<string, object>();
            new CreateCriteria(dicWherexm).Add(ORMRestrictions<string>.Eq("FProjCode", mst.FProjCode))
                .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
                .Add(ORMRestrictions<string>.Eq("FReference", "1"));
            var xmlist = ProjectMstService.Find(dicWherexm).Data;
            if (xmlist.Count > 0)
            {
                savedresult.Status = ResponseStatus.Error;
                savedresult.Msg = "当前项目已经存在，请勿重复编报预算！";

                return DataConverterHelper.SerializeObject(savedresult);
            }

            var projectdtlbudgetdtl = BudgetMstService.FindBudgetDtlBudgetDtlByForeignKey(id).Data;
            var projectdtlfundappl = BudgetMstService.FindBudgetDtlFundApplByForeignKey(id).Data;
            var projectdtlimplplan = BudgetMstService.FindBudgetDtlImplPlanByForeignKey(id).Data;
            var projectdtltextcontent = BudgetMstService.FindBudgetDtlTextContentByForeignKey(id).Data;
            var projectdtlpurchasedtl = BudgetMstService.FindBudgetDtlPurchaseDtlByForeignKey(id).Data;
            var projectdtlpurdtl4sof = BudgetMstService.FindBudgetDtlPurDtl4SOFByForeignKey(id).Data;
            var projectdtlPerformTarget = BudgetMstService.FindBudgetDtlPerformTargetByForeignKey(id).Data;

            var mst2 = ModelChange<BudgetMstModel, ProjectMstModel>(mst);
            mst2.FGoYear = FGoYear;
            mst2.FProjStatus = 1;
            mst2.FApproveStatus = "1";
            //项目库引用项目金额不要代入
            mst2.FProjAmount = 0;
            mst2.FBudgetAmount = 0;
            mst2.FReference = "1";//项目库引用标志
            mst2.FStartDate = null;
            mst2.FEndDate = null;
            mst2.FDateofDeclaration = DateTime.Now;
            mst2.FProjAttr = "1";
            //mst2.FDuration = "3";
            mst2.FYear = FGoYear;
            mst2.PersistentState = PersistentState.Added;

            var budgetdtls = new List<ProjectDtlBudgetDtlModel>();
            if (projectdtlbudgetdtl.Count > 0)
            {
                foreach (var item in projectdtlbudgetdtl)
                {
                    var model = ModelChange<BudgetDtlBudgetDtlModel, ProjectDtlBudgetDtlModel>(item);
                    model.FLastYearAmount = model.FAmountAfterEdit;
                    model.FAmount = 0;
                    model.FBudgetAmount = 0;
                    model.FAmountEdit = 0;
                    model.FAmountAfterEdit = 0;
                    model.PersistentState = PersistentState.Added;
                    budgetdtls.Add(model);
                }
            }
            var fundappls = new List<ProjectDtlFundApplModel>();
            if (projectdtlfundappl.Count > 0)
            {
                foreach (var item in projectdtlfundappl)
                {
                    var model = ModelChange<BudgetDtlFundApplModel, ProjectDtlFundApplModel>(item);
                    model.FAmount = 0;
                    model.PersistentState = PersistentState.Added;
                    fundappls.Add(model);
                }
            }
            var implplans = new List<ProjectDtlImplPlanModel>();
            if (projectdtlimplplan.Count > 0)
            {
                foreach (var item in projectdtlimplplan)
                {
                    var model = ModelChange<BudgetDtlImplPlanModel, ProjectDtlImplPlanModel>(item);
                    model.PersistentState = PersistentState.Added;
                    implplans.Add(model);
                }
            }
            var PerformTargets = new List<ProjectDtlPerformTargetModel>();
            if (projectdtlPerformTarget.Count > 0)
            {
                foreach (var item in projectdtlPerformTarget)
                {
                    var model = ModelChange<BudgetDtlPerformTargetModel, ProjectDtlPerformTargetModel>(item);
                    model.PersistentState = PersistentState.Added;
                    PerformTargets.Add(model);
                }
            }
            var purchasedtls = new List<ProjectDtlPurchaseDtlModel>();
            if (projectdtlpurchasedtl.Count > 0)
            {
                foreach (var item in projectdtlpurchasedtl)
                {
                    var model = ModelChange<BudgetDtlPurchaseDtlModel, ProjectDtlPurchaseDtlModel>(item);
                    model.PersistentState = PersistentState.Added;
                    purchasedtls.Add(model);
                }
            }
            var purdtl4sofs = new List<ProjectDtlPurDtl4SOFModel>();
            if (projectdtlpurdtl4sof.Count > 0)
            {
                foreach (var item in projectdtlpurdtl4sof)
                {
                    var model = ModelChange<BudgetDtlPurDtl4SOFModel, ProjectDtlPurDtl4SOFModel>(item);
                    model.PersistentState = PersistentState.Added;
                    purdtl4sofs.Add(model);
                }
            }
            var textcontent = new List<ProjectDtlTextContentModel>();
            if (projectdtltextcontent.Count > 0)
            {
                foreach (var item in projectdtltextcontent)
                {
                    var model = ModelChange<BudgetDtlTextContentModel, ProjectDtlTextContentModel>(item);
                    model.PersistentState = PersistentState.Added;
                    textcontent.Add(model);
                }
            }
            if (purchasedtls.Count>0 && purdtl4sofs.Count>0)
            {
                savedresult = ProjectMstService.SaveProjectMst(mst2, textcontent, purchasedtls, purdtl4sofs, PerformTargets, fundappls, budgetdtls, implplans);
            }
            else
            {
                savedresult = ProjectMstService.SaveProjectMst(mst2, textcontent, null, null, PerformTargets, fundappls, budgetdtls, implplans);
            }
            if (savedresult.Status == ResponseStatus.Success&&!string.IsNullOrEmpty(Msg))
            {
                savedresult.Msg = Msg;
            }

            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 取相应的模板
        /// </summary>
        /// <returns></returns>
        public string FindIndividualInfoById()
        {
            var busType = System.Web.HttpContext.Current.Request.Params["busType"];
            var id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);


            var new_id = ProjectMstService.FindIndividualInfoById(busType,id);

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            savedresult.Status = ResponseStatus.Success;
            savedresult.Msg = new_id;


            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 根据操作员默认部门判断上年是否存在单据为进行自评上报
        /// </summary>
        /// <returns></returns>
        public string JudegIfJx()
        {
            var FGoYear = System.Web.HttpContext.Current.Request.Params["FGoYear"];
            var userId = System.Web.HttpContext.Current.Request.Params["userId"];

            var dept = System.Web.HttpContext.Current.Request.Params["dept"];

            var dicWhere = new Dictionary<string, object>();
           
            if (!string.IsNullOrEmpty(dept))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FDeclarationDept", dept));
            }
            
            var dicWhereDept = new Dictionary<string, object>();
            new CreateCriteria(dicWhereDept)
                .Add(ORMRestrictions<string>.Eq("Dwdm", userId)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
            var deptL = CorrespondenceSettingsService.Find(dicWhereDept).Data.ToList().Select(x => x.Dydm).Distinct().ToList();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));

            
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
            new CreateCriteria(dicWhere).Add(ORMRestrictions<EnumYesNo>.Eq("FIfPerformanceAppraisal", EnumYesNo.Yes));
            
            var result = BudgetMstService.Find(dicWhere).Data;
            var ysPhidList = result.ToList().Select(x => x.PhId).ToList();

            var dicWherejx = new Dictionary<string, object>();
            new CreateCriteria(dicWherejx).Add(ORMRestrictions<List<Int64>>.In("YSMstPhId", ysPhidList));
            var jxList = PerformanceMstService.Find(dicWherejx).Data.ToList().FindAll(x=>x.FAuditStatus=="2"|| x.FAuditStatus == "3"|| x.FAuditStatus == "4").Select(x=>x.YSMstPhId).Distinct().ToList();
            if(ysPhidList.Count> jxList.Count)
            {
                return "false";
            }
            else
            {
                return "true";
            }
        }
    }
}

