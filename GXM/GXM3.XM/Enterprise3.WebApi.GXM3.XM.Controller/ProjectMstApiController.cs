using Enterprise3.Common.Base.Criterion;
using Enterprise3.Common.Model.Results;
using Enterprise3.WebApi.ApiControllerBase;
using Enterprise3.WebApi.ApiControllerBase.Models;
using Enterprise3.WebApi.GSP3.SP.Model.Request;
using Enterprise3.WebApi.GSP3.SP.Model.Response;
using Enterprise3.WebApi.GXM3.XM.Model.Common;
using Enterprise3.WebApi.GXM3.XM.Model.Request;
using Enterprise3.WebApi.GYS3.YS.Model.Request;
using GData3.Common.Utils.Filters;
using GQT3.QT.Model.Domain;
using GQT3.QT.Service;
using GQT3.QT.Service.Interface;
using GSP3.SP.Service.Interface;
using GXM3.XM.Model.Domain;
using GXM3.XM.Model.Enums;
using GXM3.XM.Model.Extra;
using GXM3.XM.Service.Interface;
using GYS3.YS.Model.Domain;
using GYS3.YS.Service.Interface;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using SUP.Common.Base;
using SUP.Common.DataAccess;
using SUP.Common.DataEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Enterprise3.WebApi.GXM3.XM.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [MethodExceptionFilter, SyncUserDbFilter]
    public class ProjectMstApiController : ApiBase
    {
        IProjectMstService ProjectMstService;


        ICorrespondenceSettingsService CorrespondenceSettingsService { get; set; }

        IBudgetProcessCtrlService BudgetProcessCtrlService { get; set; }

        IProjLibProjService ProjLibService { get; set; }

        IGHProjDtlShareService ProjDtlShareService { get; set; }

        IQTSysSetService QTSysSetService { get; set; }

        IBudgetMstService BudgetMstService { get; set; }

        IQtAttachmentService QtAttachmentService { get; set; }

        IQtCoverUpDataService QtCoverUpDataService { get; set; }

        IQtCoverUpForOrgService QtCoverUpForOrgService { get; set; }

        IPerformEvalTargetTypeService PerformEvalTargetTypeService { get; set; }
        IGAppvalRecordService GAppvalRecordService { get; set; }

        IGAppvalProcService GAppvalProcService { get; set; }

        IQtXmDistributeService QtXmDistributeService { get; set; }

        IBudgetAccountsService BudgetAccountsService { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProjectMstApiController()
        {
            ProjectMstService = base.GetObject<IProjectMstService>("GXM3.XM.Service.ProjectMst");
            CorrespondenceSettingsService = base.GetObject<ICorrespondenceSettingsService>("GQT3.QT.Service.CorrespondenceSettings");
            BudgetProcessCtrlService = base.GetObject<IBudgetProcessCtrlService>("GYS3.YS.Service.BudgetProcessCtrl");
            ProjLibService = base.GetObject<IProjLibProjService>("GQT3.QT.Service.ProjLibProj");
            ProjDtlShareService = base.GetObject<IGHProjDtlShareService>("GQT3.QT.Service.GHProjDtlShare");
            QTSysSetService = base.GetObject<IQTSysSetService>("GQT3.QT.Service.QTSysSet");
            BudgetMstService = base.GetObject<IBudgetMstService>("GYS3.YS.Service.BudgetMst");
            QtAttachmentService = base.GetObject<IQtAttachmentService>("GQT3.QT.Service.QtAttachment");
            QtCoverUpDataService = base.GetObject<IQtCoverUpDataService>("GQT3.QT.Service.QtCoverUpData");
            QtCoverUpForOrgService = base.GetObject<IQtCoverUpForOrgService>("GQT3.QT.Service.QtCoverUpForOrg");

            PerformEvalTargetTypeService = base.GetObject<IPerformEvalTargetTypeService>("GQT3.QT.Service.PerformEvalTargetType");
            GAppvalRecordService = base.GetObject<IGAppvalRecordService>("GSP3.SP.Service.GAppvalRecord");
            GAppvalProcService = base.GetObject<IGAppvalProcService>("GSP3.SP.Service.GAppvalProc");
            QtXmDistributeService = base.GetObject<IQtXmDistributeService>("GQT3.QT.Service.QtXmDistribute");
            BudgetAccountsService = base.GetObject<IBudgetAccountsService>("GQT3.QT.Service.BudgetAccounts");
        }

        /// <summary>
        /// 获取项目主界面列表
        /// </summary>
        /// <param name="projectMst"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetProjectMstList([FromUri]ProjectMstRequest projectMst)
        {
            if (string.IsNullOrEmpty(projectMst.OrgCode))
            {
                return DCHelper.ErrorMessage("单位编码不能为空！");
            }
            if (string.IsNullOrEmpty(projectMst.Ucode))
            {
                return DCHelper.ErrorMessage("用户编码不能为空！");
            }
            if (string.IsNullOrEmpty(projectMst.Year))
            {
                return DCHelper.ErrorMessage("年度不能为空！");
            }
            try
            {
                var dicWhereDept = new Dictionary<string, object>();
                new CreateCriteria(dicWhereDept)
                    .Add(ORMRestrictions<string>.Eq("Dwdm", projectMst.Ucode)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
                var deptList = CorrespondenceSettingsService.Find(dicWhereDept);
                List<string> deptL = new List<string>();
                for (var i = 0; i < deptList.Data.Count; i++)
                {
                    deptL.Add(deptList.Data[i].Dydm);
                }
                //年份与单位编码筛选
                Dictionary<string, object> dic = new Dictionary<string, object>();
                new CreateCriteria(dic)
                        .Add(ORMRestrictions<string>.Eq("FYear", projectMst.Year))
                        .Add(ORMRestrictions<string>.Eq("FDeclarationUnit", projectMst.OrgCode));
                //项目类型条件筛选
                if (projectMst.ProjStatus == "1")
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL))
                        .Add(ORMRestrictions<List<Int32>>.In("FProjStatus", new List<Int32> { 1, 8 }));
                }
                else if (projectMst.ProjStatus == "2") //立项
                {
                    new CreateCriteria(dic)//.Add(ORMRestrictions<Int32>.Eq("FProjStatus", 2))
                       .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<Int32>.Eq("FProjStatus", 2))
                       .Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
                }
                else if (projectMst.ProjStatus == "3")//项目执行
                {
                    new CreateCriteria(dic)//.Add(ORMRestrictions<Int32>.Eq("FProjStatus", 2))
                       .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<Int32>.Eq("FProjStatus", 3))
                       .Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
                }
                else if (projectMst.ProjStatus == "0")
                {
                    new CreateCriteria(dic)//.Add(ORMRestrictions<Int32>.Eq("FProjStatus", 2))
                       .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<IList<int>>.NotIn("FProjStatus", new List<int> { 1, 8 }))
                       .Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
                }
                else
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
                }
                //增加搜索条件
                if (!string.IsNullOrEmpty(projectMst.SearchValue))
                {
                    Dictionary<string, object> dicName = new Dictionary<string, object>();
                    Dictionary<string, object> dicCode = new Dictionary<string, object>();
                    new CreateCriteria(dicName)
                            .Add(ORMRestrictions<string>.Like("FProjName", projectMst.SearchValue));
                    new CreateCriteria(dicCode)
                            .Add(ORMRestrictions<string>.Like("FProjCode", projectMst.SearchValue));
                    new CreateCriteria(dic).Add(ORMRestrictions.Or(dicName, dicCode));
                }
                if (!string.IsNullOrEmpty(projectMst.FExpenseCategory) && !"0".Equals(projectMst.FExpenseCategory))
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<string>.Eq("FExpenseCategory", projectMst.FExpenseCategory));
                }
                if (!string.IsNullOrEmpty(projectMst.FApproveStatus) && !"0".Equals(projectMst.FApproveStatus))
                {
                    if (projectMst.FApproveStatus == "1")
                    {
                        new CreateCriteria(dic)
                           .Add(ORMRestrictions<List<string>>.In("FApproveStatus", new List<string> { "1", "5" }));//暂存的也要出来
                    }
                    else
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<string>.Eq("FApproveStatus", projectMst.FApproveStatus));
                    }
                }
                if (!string.IsNullOrEmpty(projectMst.FBudgetDept))//预算部门
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<string>.Eq("FBudgetDept", projectMst.FBudgetDept));
                }
                if (!string.IsNullOrEmpty(projectMst.FDeclarationDept))//申报部门
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<string>.Eq("FDeclarationDept", projectMst.FDeclarationDept));
                }
                if (!string.IsNullOrEmpty(projectMst.FProjCode))//项目编码
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<string>.Eq("FProjCode", projectMst.FProjCode));
                }
                if (!string.IsNullOrEmpty(projectMst.FProjName))//项目名称
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<string>.Eq("FProjName", projectMst.FProjName));
                }
                if (!string.IsNullOrEmpty(projectMst.FProjAmountBegin))
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<Decimal>.Ge("FProjAmount", Decimal.Parse(projectMst.FProjAmountBegin)));
                }
                if (!string.IsNullOrEmpty(projectMst.FProjAmountEnd))
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<Decimal>.Le("FProjAmount", Decimal.Parse(projectMst.FProjAmountEnd)));
                }
                if (projectMst.FStartDate != null)
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<System.DateTime?>.Ge("FDateofDeclaration", projectMst.FStartDate));
                }
                if (projectMst.FEndDate != null)
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<System.DateTime?>.Le("FDateofDeclaration", projectMst.FEndDate));
                }
                if (projectMst.FIfPerformanceAppraisal != 0 && !string.IsNullOrEmpty(projectMst.FIfPerformanceAppraisal.ToString()))
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<EnumYesNo>.Eq("FIfPerformanceAppraisal", projectMst.FIfPerformanceAppraisal));
                }
                var result = this.ProjectMstService.LoadWithPage(projectMst.PageIndex, projectMst.PageSize, dic, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" });

                //取可选相同审批流是数据集合
                if (projectMst.ProcPhid != 0)
                {
                    var proList = this.ProjectMstService.Find(dic, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" }).Data;
                    if (proList != null && proList.Count > 0)
                    {
                        List<string> orgList = proList.ToList().Select(t => t.FBudgetDept).Distinct().ToList();
                        if (orgList != null && orgList.Count > 0)
                        {
                            var procList = this.GAppvalProcService.Find(t => orgList.Contains(t.OrgCode)).Data;
                            if (procList != null && procList.Count > 0)
                            {
                                //可以选取相同审批流的打上标记
                                foreach (var res in proList)
                                {
                                    if (res.FApproveStatus == ((int)EnumApproveStatus.ToBeRepored).ToString() && procList.ToList().Find(t => t.OrgCode == res.FBudgetDept && t.PhId == projectMst.ProcPhid) != null)
                                    {
                                        res.BatchPracBz = 1;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                proList = proList.ToList().FindAll(t => t.BatchPracBz == 1);
                            }
                        }
                    }

                    result.Results = proList.Skip((projectMst.PageIndex - 1) * projectMst.PageSize).Take(projectMst.PageSize).ToList();
                    result.TotalItems = proList.Count;
                }

                //提高接口效率
                var Query = QtAttachmentService.Find(t => t.BTable == "XM3_PROJECTMST").Data;
                foreach (var item in result.Results)
                {
                    //var dics = new Dictionary<string, object>();
                    //new CreateCriteria(dics).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                    //new CreateCriteria(dics).Add(ORMRestrictions<Int64>.Eq("RelPhid", item.PhId));
                    //new CreateCriteria(dics).Add(ORMRestrictions<string>.Eq("BTable", "XM3_PROJECTMST"));
                    //var Query = QtAttachmentService.Find(dics);
                    if (Query != null && Query.Count > 0)
                    {
                        var uploadlist = Query.ToList().FindAll(t => t.RelPhid == item.PhId);
                        item.list = uploadlist;
                        item.UploadFileCount = uploadlist.Count;
                    }
                }

                //foreach (var item in result.Results)
                //{
                //    var dics = new Dictionary<string, object>();
                //    new CreateCriteria(dics).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                //    new CreateCriteria(dics).Add(ORMRestrictions<Int64>.Eq("RelPhid", item.PhId));
                //    new CreateCriteria(dics).Add(ORMRestrictions<string>.Eq("BTable", "XM3_PROJECTMST"));
                //    var Query = QtAttachmentService.Find(dics);
                //    var uploadlist = Query.Data.ToList();
                //    //var address = Query.Data.Select(m => m.BUrlpath).ToArray();
                //    //var name = Query.Data.Select(m => m.BName).ToArray();
                //    //var model = new UploadPackGXM { UploadFileaddress = address, UploadFilename = name };
                //    item.list = uploadlist;
                //    item.UploadFileCount = Query.Data.Select(m => m.BUrlpath).Count();
                //}
                var dicSysset = new Dictionary<string, object>();
                new CreateCriteria(dicSysset).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                var syssets = QTSysSetService.Find(dicSysset).Data.ToList();
                foreach (var data in result.Results)
                {
                    var syssetProjectMst = syssets.FindAll(x => x.DicType == "ProjectProper" && x.Orgcode == data.FDeclarationUnit && x.TypeCode == data.FProjAttr);
                    if (syssetProjectMst.Count > 0)
                    {
                        //项目属性代码转名称
                        data.FProjAttr_EXName = syssetProjectMst[0].TypeName;
                    }
                    var syssetProjectMst2 = syssets.FindAll(x => x.DicType == "TimeLimit" && x.Orgcode == data.FDeclarationUnit && x.TypeCode == data.FDuration);
                    if (syssetProjectMst2.Count > 0)
                    {
                        //存续期限代码转名称
                        data.FDuration_EXName = syssetProjectMst2[0].TypeName;
                    }

                    var syssetProjectMst3 = syssets.FindAll(x => x.DicType == "ProjectLevel" && x.Orgcode == data.FDeclarationUnit && x.TypeCode == data.FLevel);
                    if (syssetProjectMst3.Count > 0)
                    {
                        //项目级别代码转名称
                        data.FLevel_EXName = syssetProjectMst3[0].TypeName;
                    }

                    var dtls = ProjectMstService.FindProjectDtlBudgetDtlByForeignKey(data.PhId).Data;
                    var FIfYsxz = 0;
                    if (dtls.Count > 0)
                    {
                        foreach (var dtl in dtls)
                        {
                            if (string.IsNullOrEmpty(dtl.FBudgetAccounts) || string.IsNullOrEmpty(dtl.FExpensesChannel))
                            {
                                FIfYsxz++;
                            }
                        }
                    }
                    data.FIfYsxz = FIfYsxz;
                }
                return DCHelper.ModelListToJson<ProjectMstModel>(result.Results, (Int32)result.TotalItems);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 获取项目主界面列表2
        /// </summary>
        /// <param name="projectMst"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetProjectMstList2([FromUri]ProjectMstRequest projectMst)
        {
            if (string.IsNullOrEmpty(projectMst.OrgCode))
            {
                return DCHelper.ErrorMessage("单位编码不能为空！");
            }
            if (string.IsNullOrEmpty(projectMst.Ucode))
            {
                return DCHelper.ErrorMessage("用户编码不能为空！");
            }
            if (string.IsNullOrEmpty(projectMst.Year))
            {
                return DCHelper.ErrorMessage("年度不能为空！");
            }
            try
            {
                var dicWhereDept = new Dictionary<string, object>();
                new CreateCriteria(dicWhereDept)
                    .Add(ORMRestrictions<string>.Eq("Dwdm", projectMst.Ucode)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
                var deptList = CorrespondenceSettingsService.Find(dicWhereDept);
                List<string> deptL = new List<string>();
                for (var i = 0; i < deptList.Data.Count; i++)
                {
                    deptL.Add(deptList.Data[i].Dydm);
                }
                //年份与单位编码筛选
                Dictionary<string, object> dic = new Dictionary<string, object>();
                new CreateCriteria(dic)
                        .Add(ORMRestrictions<string>.Eq("FYear", projectMst.Year))
                        .Add(ORMRestrictions<string>.Eq("FDeclarationUnit", projectMst.OrgCode));
                //项目类型条件筛选
                //if (projectMst.ProjStatus == "1")
                //{
                //    new CreateCriteria(dic)
                //        .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL))
                //        .Add(ORMRestrictions<List<Int32>>.In("FProjStatus", new List<Int32> { 1, 8 }));
                //}
                //else if (projectMst.ProjStatus == "2") //立项
                //{
                //    new CreateCriteria(dic)//.Add(ORMRestrictions<Int32>.Eq("FProjStatus", 2))
                //       .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<IList<int>>.NotIn("FProjStatus", new List<int> { 1, 8 }))
                //       .Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
                //}
                //else
                //{
                //    new CreateCriteria(dic)
                //        .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
                //}

                new CreateCriteria(dic)
                    .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
                if (projectMst.projectarea != null)
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<List<Int32>>.In("FProjStatus", projectMst.projectarea));
                }
                if (projectMst.fapprovearea != null)
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<List<string>>.In("FApproveStatus", projectMst.fapprovearea));
                }
                //增加搜索条件
                if (!string.IsNullOrEmpty(projectMst.SearchValue))
                {
                    Dictionary<string, object> dicName = new Dictionary<string, object>();
                    Dictionary<string, object> dicCode = new Dictionary<string, object>();
                    new CreateCriteria(dicName)
                            .Add(ORMRestrictions<string>.Like("FProjName", projectMst.SearchValue));
                    new CreateCriteria(dicCode)
                            .Add(ORMRestrictions<string>.Like("FProjCode", projectMst.SearchValue));
                    new CreateCriteria(dic).Add(ORMRestrictions.Or(dicName, dicCode));
                }
                if (!string.IsNullOrEmpty(projectMst.FExpenseCategory) && !"0".Equals(projectMst.FExpenseCategory))
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<string>.Eq("FExpenseCategory", projectMst.FExpenseCategory));
                }
                //if (!string.IsNullOrEmpty(projectMst.FApproveStatus) && !"0".Equals(projectMst.FApproveStatus))
                //{
                //    if (projectMst.FApproveStatus == "1")
                //    {
                //        new CreateCriteria(dic)
                //           .Add(ORMRestrictions<List<string>>.In("FApproveStatus", new List<string> { "1", "5" }));//暂存的也要出来
                //    }
                //    else
                //    {
                //        new CreateCriteria(dic)
                //            .Add(ORMRestrictions<string>.Eq("FApproveStatus", projectMst.FApproveStatus));
                //    }
                //}
                if (!string.IsNullOrEmpty(projectMst.FBudgetDept))//预算部门
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<string>.Eq("FBudgetDept", projectMst.FBudgetDept));
                }
                if (!string.IsNullOrEmpty(projectMst.FDeclarationDept))//申报部门
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<string>.Eq("FDeclarationDept", projectMst.FDeclarationDept));
                }
                if (!string.IsNullOrEmpty(projectMst.FProjCode))//项目编码
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<string>.Eq("FProjCode", projectMst.FProjCode));
                }
                if (!string.IsNullOrEmpty(projectMst.FProjName))//项目名称
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<string>.Eq("FProjName", projectMst.FProjName));
                }
                if (!string.IsNullOrEmpty(projectMst.FProjAmountBegin))
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<Decimal>.Ge("FProjAmount", Decimal.Parse(projectMst.FProjAmountBegin)));
                }
                if (!string.IsNullOrEmpty(projectMst.FProjAmountEnd))
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<Decimal>.Le("FProjAmount", Decimal.Parse(projectMst.FProjAmountEnd)));
                }
                if (projectMst.FStartDate != null)
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<System.DateTime?>.Ge("FDateofDeclaration", projectMst.FStartDate));
                }
                if (projectMst.FEndDate != null)
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<System.DateTime?>.Le("FDateofDeclaration", projectMst.FEndDate));
                }
                if (projectMst.FIfPerformanceAppraisal != 0 && !string.IsNullOrEmpty(projectMst.FIfPerformanceAppraisal.ToString()))
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<EnumYesNo>.Eq("FIfPerformanceAppraisal", projectMst.FIfPerformanceAppraisal));
                }
                var result = this.ProjectMstService.LoadWithPage(projectMst.PageIndex, projectMst.PageSize, dic, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" });

                //提高接口效率
                var Query = QtAttachmentService.Find(t => t.BTable == "XM3_PROJECTMST").Data;
                foreach (var item in result.Results)
                {
                    //var dics = new Dictionary<string, object>();
                    //new CreateCriteria(dics).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                    //new CreateCriteria(dics).Add(ORMRestrictions<Int64>.Eq("RelPhid", item.PhId));
                    //new CreateCriteria(dics).Add(ORMRestrictions<string>.Eq("BTable", "XM3_PROJECTMST"));
                    //var Query = QtAttachmentService.Find(dics);
                    if (Query != null && Query.Count > 0)
                    {
                        var uploadlist = Query.ToList().FindAll(t => t.RelPhid == item.PhId);
                        item.list = uploadlist;
                        item.UploadFileCount = uploadlist.Count;
                    }
                }

                //foreach (var item in result.Results)
                //{
                //    var dics = new Dictionary<string, object>();
                //    new CreateCriteria(dics).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                //    new CreateCriteria(dics).Add(ORMRestrictions<Int64>.Eq("RelPhid", item.PhId));
                //    new CreateCriteria(dics).Add(ORMRestrictions<string>.Eq("BTable", "XM3_PROJECTMST"));
                //    var Query = QtAttachmentService.Find(dics);
                //    var uploadlist = Query.Data.ToList();
                //    //var address = Query.Data.Select(m => m.BUrlpath).ToArray();
                //    //var name = Query.Data.Select(m => m.BName).ToArray();
                //    //var model = new UploadPackGXM { UploadFileaddress = address, UploadFilename = name };
                //    item.list = uploadlist;
                //    item.UploadFileCount = Query.Data.Select(m => m.BUrlpath).Count();
                //}
                var dicSysset = new Dictionary<string, object>();
                new CreateCriteria(dicSysset).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                var syssets = QTSysSetService.Find(dicSysset).Data.ToList();
                foreach (var data in result.Results)
                {
                    var syssetProjectMst = syssets.FindAll(x => x.DicType == "ProjectProper" && x.Orgcode == data.FDeclarationUnit && x.TypeCode == data.FProjAttr);
                    if (syssetProjectMst.Count > 0)
                    {
                        //项目属性代码转名称
                        data.FProjAttr_EXName = syssetProjectMst[0].TypeName;
                    }
                    var syssetProjectMst2 = syssets.FindAll(x => x.DicType == "TimeLimit" && x.Orgcode == data.FDeclarationUnit && x.TypeCode == data.FDuration);
                    if (syssetProjectMst2.Count > 0)
                    {
                        //存续期限代码转名称
                        data.FDuration_EXName = syssetProjectMst2[0].TypeName;
                    }

                    var syssetProjectMst3 = syssets.FindAll(x => x.DicType == "ProjectLevel" && x.Orgcode == data.FDeclarationUnit && x.TypeCode == data.FLevel);
                    if (syssetProjectMst3.Count > 0)
                    {
                        //项目级别代码转名称
                        data.FLevel_EXName = syssetProjectMst3[0].TypeName;
                    }

                    var dtls = ProjectMstService.FindProjectDtlBudgetDtlByForeignKey(data.PhId).Data;
                    var FIfYsxz = 0;
                    if (dtls.Count > 0)
                    {
                        foreach (var dtl in dtls)
                        {
                            if (string.IsNullOrEmpty(dtl.FBudgetAccounts) || string.IsNullOrEmpty(dtl.FExpensesChannel))
                            {
                                FIfYsxz++;
                            }
                        }
                    }
                    data.FIfYsxz = FIfYsxz;
                }
                return DCHelper.ModelListToJson<ProjectMstModel>(result.Results, (Int32)result.TotalItems);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 获取单个预算项目详情
        /// </summary>
        /// <param name="projectMst"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetProjectMst([FromUri]ProjectMstRequest projectMst)
        {
            if (projectMst.FProjPhId == 0)
            {
                return DCHelper.ErrorMessage("项目主键不能为空！");
            }

            try
            {
                var dicSysset = new Dictionary<string, object>();
                new CreateCriteria(dicSysset).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                var syssets = QTSysSetService.Find(dicSysset).Data.ToList();

                ProjectAllDataModel projectAllData = new ProjectAllDataModel();
                projectAllData.ProjectMst = ProjectMstService.Find(projectMst.FProjPhId).Data;

                //返回对象增加附件
                if (projectAllData.ProjectMst != null)
                {
                    projectAllData.ProjectAttachments = QtAttachmentService.Find(t => t.BTable == "XM3_PROJECTMST" && t.RelPhid == projectAllData.ProjectMst.PhId).Data.ToList();
                }

                var FDeclarationDept = CorrespondenceSettingsService.GetOrg(projectAllData.ProjectMst.FDeclarationDept);
                if (FDeclarationDept != null)
                {
                    //申报部门代码转名称
                    projectAllData.ProjectMst.FDeclarationDept_EXName = FDeclarationDept.OName;
                }
                var syssetProjectMst = syssets.FindAll(x => x.DicType == "ProjectProper" && x.Orgcode == projectAllData.ProjectMst.FDeclarationUnit && x.TypeCode == projectAllData.ProjectMst.FProjAttr);
                if (syssetProjectMst.Count > 0)
                {
                    //项目属性代码转名称
                    projectAllData.ProjectMst.FProjAttr_EXName = syssetProjectMst[0].TypeName;
                }
                var syssetProjectMst2 = syssets.FindAll(x => x.DicType == "TimeLimit" && x.Orgcode == projectAllData.ProjectMst.FDeclarationUnit && x.TypeCode == projectAllData.ProjectMst.FDuration);
                if (syssetProjectMst2.Count > 0)
                {
                    //存续期限代码转名称
                    projectAllData.ProjectMst.FDuration_EXName = syssetProjectMst2[0].TypeName;
                }

                var syssetProjectMst3 = syssets.FindAll(x => x.DicType == "ProjectLevel" && x.Orgcode == projectAllData.ProjectMst.FDeclarationUnit && x.TypeCode == projectAllData.ProjectMst.FLevel);
                if (syssetProjectMst3.Count > 0)
                {
                    //项目级别代码转名称
                    projectAllData.ProjectMst.FLevel_EXName = syssetProjectMst3[0].TypeName;
                }

                projectAllData.ProjectDtlImplPlans = ProjectMstService.FindProjectDtlImplPlanByForeignKey(projectMst.FProjPhId).Data.ToList();
                projectAllData.ProjectDtlTextContents = ProjectMstService.FindProjectDtlTextContentByForeignKey(projectMst.FProjPhId).Data[0];
                //if (findedresultprojectdtltextcontent != null)
                //{
                //    if (findedresultprojectdtltextcontent.Data.Count > 0)
                //    {
                //        ProjectDtlTextContentModel singleData = findedresultprojectdtltextcontent.Data[0];
                //        FindedResult<ProjectDtlTextContentModel> result = new FindedResult<ProjectDtlTextContentModel>(false, singleData);
                //        return DataConverterHelper.ResponseResultToJson(result);
                //    }
                //}
                projectAllData.ProjectDtlFundAppls = ProjectMstService.FindProjectDtlFundApplByForeignKey(projectMst.FProjPhId).Data.ToList();
                projectAllData.ProjectDtlBudgetDtls = ProjectMstService.FindProjectDtlBudgetDtlByForeignKey(projectMst.FProjPhId).Data.ToList();
                projectAllData.ProjectDtlBudgetDtls.Sort((ProjectDtlBudgetDtlModel dtl1, ProjectDtlBudgetDtlModel dtl2) => dtl1.FDtlCode.CompareTo(dtl2.FDtlCode));
                foreach (var ProjectDtlBudgetDtl in projectAllData.ProjectDtlBudgetDtls)
                {
                    //支付方式代码转名称
                    var syssetProjectDtlBudgetDtl = syssets.FindAll(x => x.DicType == "PayMethodTwo" && x.Orgcode == projectAllData.ProjectMst.FDeclarationUnit && x.TypeCode == ProjectDtlBudgetDtl.FPaymentMethod);
                    if (syssetProjectDtlBudgetDtl.Count > 0)
                    {
                        ProjectDtlBudgetDtl.FPaymentMethod_EXName = syssetProjectDtlBudgetDtl[0].TypeName;
                    }

                }
                projectAllData.ProjectDtlPurchaseDtls = ProjectMstService.FindProjectDtlPurchaseDtlByForeignKey(projectMst.FProjPhId).Data.ToList();
                projectAllData.ProjectDtlPurDtl4SOFs = ProjectMstService.FindProjectDtlPurDtl4SOFByForeignKey(projectMst.FProjPhId).Data.ToList();
                projectAllData.ProjectDtlPerformTargets = ProjectMstService.FindProjectDtlPerformTargetByForeignKey(projectMst.FProjPhId).Data.ToList();
                OrganizeModel organize = this.BudgetMstService.GetOrganizeByCode(projectAllData.ProjectMst.FDeclarationUnit);
                if (organize != null)
                {
                    projectAllData.ProjectDtlPerformTargets = ProjectMstService.GetNewProPerformTargets(projectAllData.ProjectDtlPerformTargets, projectAllData.ProjectMst.FPerformType, organize.PhId, organize.OCode);
                }
                return DataConverterHelper.SerializeObject(projectAllData);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 保存立项数据
        /// </summary>
        /// <param name="projectAllData"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostSaveProject([FromBody]ProjectAllDataRequest projectAllData)
        {

            //string mstformData = System.Web.HttpContext.Current.Request.Form["mstformData"];
            //string projectdtlimplplangridData = System.Web.HttpContext.Current.Request.Form["projectdtlimplplangridData"];
            //string projectdtltextcontentgridData = System.Web.HttpContext.Current.Request.Form["projectdtltextcontentgridData"];
            //string projectdtlfundapplgridData = System.Web.HttpContext.Current.Request.Form["projectdtlfundapplgridData"];
            //string projectdtlbudgetdtlgridData = System.Web.HttpContext.Current.Request.Form["projectdtlbudgetdtlgridData"];
            //string projectdtlperformtargetgridData = System.Web.HttpContext.Current.Request.Form["projectdtlperformtargetgridData"];
            //string projectdtlpurchasedtlformData = System.Web.HttpContext.Current.Request.Form["projectdtlpurchasedtlformData"];
            //string projectdtlpurdtl4sofgridData = System.Web.HttpContext.Current.Request.Form["projectdtlpurdtl4sofgridData"];

            string midEdit = projectAllData.MidEdit;

            ProjectMstModel mstforminfo = new ProjectMstModel();
            if (projectAllData.ProjectMst != null)
            {
                mstforminfo = projectAllData.ProjectMst;
            }

            List<ProjectDtlBudgetDtlModel> projectdtlbudgetdtlgridinfo = new List<ProjectDtlBudgetDtlModel>();
            if (projectAllData.ProjectDtlBudgetDtls != null)
            {
                projectdtlbudgetdtlgridinfo = projectAllData.ProjectDtlBudgetDtls;
            }

            List<ProjectDtlFundApplModel> projectdtlfundapplgridinfo = new List<ProjectDtlFundApplModel>();
            if (projectAllData.ProjectDtlFundAppls != null)
            {
                projectdtlfundapplgridinfo = projectAllData.ProjectDtlFundAppls;
            }
            List<ProjectDtlImplPlanModel> projectdtlimplplangridinfo = new List<ProjectDtlImplPlanModel>();
            if (projectAllData.ProjectDtlImplPlans != null)
            {
                projectdtlimplplangridinfo = projectAllData.ProjectDtlImplPlans;
            }
            List<ProjectDtlPerformTargetModel> projectdtlperformtargetgridinfo = new List<ProjectDtlPerformTargetModel>();
            if (projectAllData.ProjectDtlPerformTargets != null)
            {
                projectdtlperformtargetgridinfo = projectAllData.ProjectDtlPerformTargets;
            }
            ProjectDtlTextContentModel projectdtltextcontentgridinfo = new ProjectDtlTextContentModel();
            if (projectAllData.ProjectDtlTextContents != null)
            {
                projectdtltextcontentgridinfo = projectAllData.ProjectDtlTextContents;
            }
            //var mstforminfo = projectAllData.ProjectMst;
            //var projectdtlimplplangridinfo = projectAllData.ProjectDtlImplPlans;
            //var projectdtltextcontentgridinfo = projectAllData.ProjectDtlTextContents;
            //var projectdtlfundapplgridinfo = projectAllData.ProjectDtlFundAppls;
            //var projectdtlbudgetdtlgridinfo = projectAllData.ProjectDtlBudgetDtls;
            //var projectdtlperformtargetgridinfo = projectAllData.ProjectDtlPerformTargets;


            List<ProjectDtlPurchaseDtlModel> projectdtlpurchasedtlforminfo = new List<ProjectDtlPurchaseDtlModel>();
            if (projectAllData.ProjectDtlPurchaseDtls != null)
            {
                projectdtlpurchasedtlforminfo = projectAllData.ProjectDtlPurchaseDtls;
            }

            List<ProjectDtlPurDtl4SOFModel> projectdtlpurdtl4sofgridinfo = new List<ProjectDtlPurDtl4SOFModel>();
            if (projectAllData.ProjectDtlPurDtl4SOFs != null)
            {
                projectdtlpurdtl4sofgridinfo = projectAllData.ProjectDtlPurDtl4SOFs;
            }

            ProjectDtlBudgetDtlModel projectDtlBudgetDtl = new ProjectDtlBudgetDtlModel();
            ProjectDtlFundApplModel projectDtlFundAppl = new ProjectDtlFundApplModel();
            ProjectDtlImplPlanModel projectDtlImplPlan = new ProjectDtlImplPlanModel();
            ProjectDtlPerformTargetModel projectDtlPerformTarget = new ProjectDtlPerformTargetModel();
            ProjectDtlPurchaseDtlModel projectDtlPurchaseDtl = new ProjectDtlPurchaseDtlModel();
            ProjectDtlPurDtl4SOFModel projectDtlPurDtl4SOF = new ProjectDtlPurDtl4SOFModel();
            ProjectDtlTextContentModel projectDtlTextContent = new ProjectDtlTextContentModel();

            if (mstforminfo != null)
            {
                if (mstforminfo.PhId == 0)
                {
                    mstforminfo.PersistentState = PersistentState.Added;
                }
                else
                {
                    if (mstforminfo.PersistentState != PersistentState.Deleted)
                    {
                        mstforminfo.PersistentState = PersistentState.Modified;
                    }
                }
            }
            if (projectdtlbudgetdtlgridinfo != null && projectdtlbudgetdtlgridinfo.Count > 0)
            {
                List<ProjectDtlBudgetDtlModel> projectDtls1 = new List<ProjectDtlBudgetDtlModel>();
                List<ProjectDtlBudgetDtlModel> projectDtls2 = new List<ProjectDtlBudgetDtlModel>();
                List<ProjectDtlBudgetDtlModel> projectDtls3 = new List<ProjectDtlBudgetDtlModel>();
                foreach (var pro in projectdtlbudgetdtlgridinfo)
                {
                    pro.FAmountAfterEdit = pro.FAmount + pro.FAmountEdit;
                    if (pro.PhId == 0)
                    {
                        pro.PersistentState = PersistentState.Added;
                        projectDtls1.Add(pro);
                    }
                    else
                    {
                        if (pro.PersistentState == PersistentState.Deleted)
                        {
                            projectDtls2.Add(pro);
                        }
                        else
                        {
                            pro.PersistentState = PersistentState.Modified;
                            projectDtls3.Add(pro);
                        }
                    }
                }
                projectDtlBudgetDtl.ProjectDtlBudgetDtlsAdd = projectDtls1;
                projectDtlBudgetDtl.ProjectDtlBudgetDtlsDel = projectDtls2;
                projectDtlBudgetDtl.ProjectDtlBudgetDtlsMid = projectDtls3;
            }
            if (projectdtlfundapplgridinfo != null && projectdtlfundapplgridinfo.Count > 0)
            {
                List<ProjectDtlFundApplModel> projectDtlFunds1 = new List<ProjectDtlFundApplModel>();
                List<ProjectDtlFundApplModel> projectDtlFunds2 = new List<ProjectDtlFundApplModel>();
                List<ProjectDtlFundApplModel> projectDtlFunds3 = new List<ProjectDtlFundApplModel>();
                foreach (var pro in projectdtlfundapplgridinfo)
                {
                    if (pro.PhId == 0)
                    {
                        pro.PersistentState = PersistentState.Added;
                        projectDtlFunds1.Add(pro);
                    }
                    else
                    {
                        if (pro.PersistentState == PersistentState.Deleted)
                        {
                            projectDtlFunds2.Add(pro);
                        }
                        else
                        {
                            pro.PersistentState = PersistentState.Modified;
                            projectDtlFunds3.Add(pro);
                        }
                    }
                }
                projectDtlFundAppl.ProjectDtlFundApplsAdd = projectDtlFunds1;
                projectDtlFundAppl.ProjectDtlFundApplsDel = projectDtlFunds2;
                projectDtlFundAppl.ProjectDtlFundApplsMid = projectDtlFunds3;
            }
            if (projectdtlimplplangridinfo != null && projectdtlimplplangridinfo.Count > 0)
            {
                List<ProjectDtlImplPlanModel> projectDtlImpls1 = new List<ProjectDtlImplPlanModel>();
                List<ProjectDtlImplPlanModel> projectDtlImpls2 = new List<ProjectDtlImplPlanModel>();
                List<ProjectDtlImplPlanModel> projectDtlImpls3 = new List<ProjectDtlImplPlanModel>();
                foreach (var pro in projectdtlimplplangridinfo)
                {
                    if (pro.PhId == 0)
                    {
                        pro.PersistentState = PersistentState.Added;
                        projectDtlImpls1.Add(pro);
                    }
                    else
                    {
                        if (pro.PersistentState == PersistentState.Deleted)
                        {
                            projectDtlImpls2.Add(pro);
                        }
                        else
                        {
                            pro.PersistentState = PersistentState.Modified;
                            projectDtlImpls3.Add(pro);
                        }
                    }
                }
                projectDtlImplPlan.ProjectDtlImplPlansAdd = projectDtlImpls1;
                projectDtlImplPlan.ProjectDtlImplPlansDel = projectDtlImpls2;
                projectDtlImplPlan.ProjectDtlImplPlansMid = projectDtlImpls3;
            }
            if (projectdtlperformtargetgridinfo != null && projectdtlperformtargetgridinfo.Count > 0)
            {
                List<ProjectDtlPerformTargetModel> projectDtlPerforms1 = new List<ProjectDtlPerformTargetModel>();
                List<ProjectDtlPerformTargetModel> projectDtlPerforms2 = new List<ProjectDtlPerformTargetModel>();
                List<ProjectDtlPerformTargetModel> projectDtlPerforms3 = new List<ProjectDtlPerformTargetModel>();
                foreach (var pro in projectdtlperformtargetgridinfo)
                {
                    if (pro.PhId == 0)
                    {
                        pro.PersistentState = PersistentState.Added;
                        projectDtlPerforms1.Add(pro);
                    }
                    else
                    {
                        if (pro.PersistentState == PersistentState.Deleted)
                        {
                            projectDtlPerforms2.Add(pro);
                        }
                        else
                        {
                            pro.PersistentState = PersistentState.Modified;
                            projectDtlPerforms3.Add(pro);
                        }
                    }
                }
                projectDtlPerformTarget.ProjectDtlPerformTargetsAdd = projectDtlPerforms1;
                projectDtlPerformTarget.ProjectDtlPerformTargetsDel = projectDtlPerforms2;
                projectDtlPerformTarget.ProjectDtlPerformTargetsMid = projectDtlPerforms3;
            }
            if (projectdtlpurchasedtlforminfo != null && projectdtlpurchasedtlforminfo.Count > 0)
            {
                List<ProjectDtlPurchaseDtlModel> projectDtlPurchases1 = new List<ProjectDtlPurchaseDtlModel>();
                List<ProjectDtlPurchaseDtlModel> projectDtlPurchases2 = new List<ProjectDtlPurchaseDtlModel>();
                List<ProjectDtlPurchaseDtlModel> projectDtlPurchases3 = new List<ProjectDtlPurchaseDtlModel>();
                foreach (var pro in projectdtlpurchasedtlforminfo)
                {
                    if (pro.PhId == 0)
                    {
                        pro.PersistentState = PersistentState.Added;
                        projectDtlPurchases1.Add(pro);
                    }
                    else
                    {
                        if (pro.PersistentState == PersistentState.Deleted)
                        {
                            projectDtlPurchases2.Add(pro);
                        }
                        else
                        {
                            pro.PersistentState = PersistentState.Modified;
                            projectDtlPurchases3.Add(pro);
                        }
                    }
                }
                projectDtlPurchaseDtl.ProjectDtlPurchaseDtlsAdd = projectDtlPurchases1;
                projectDtlPurchaseDtl.ProjectDtlPurchaseDtlsDel = projectDtlPurchases2;
                projectDtlPurchaseDtl.ProjectDtlPurchaseDtlsMid = projectDtlPurchases3;
            }
            if (projectdtlpurdtl4sofgridinfo != null && projectdtlpurdtl4sofgridinfo.Count > 0)
            {
                List<ProjectDtlPurDtl4SOFModel> projectDtlPurDtl4s1 = new List<ProjectDtlPurDtl4SOFModel>();
                List<ProjectDtlPurDtl4SOFModel> projectDtlPurDtl4s2 = new List<ProjectDtlPurDtl4SOFModel>();
                List<ProjectDtlPurDtl4SOFModel> projectDtlPurDtl4s3 = new List<ProjectDtlPurDtl4SOFModel>();
                foreach (var pro in projectdtlpurdtl4sofgridinfo)
                {
                    if (pro.PhId == 0)
                    {
                        pro.PersistentState = PersistentState.Added;
                        projectDtlPurDtl4s1.Add(pro);
                    }
                    else
                    {
                        if (pro.PersistentState == PersistentState.Deleted)
                        {
                            projectDtlPurDtl4s2.Add(pro);
                        }
                        else
                        {
                            pro.PersistentState = PersistentState.Modified;
                            projectDtlPurDtl4s3.Add(pro);
                        }
                    }
                }
                projectDtlPurDtl4SOF.ProjectDtlPurDtl4SOFsAdd = projectDtlPurDtl4s1;
                projectDtlPurDtl4SOF.ProjectDtlPurDtl4SOFsDel = projectDtlPurDtl4s2;
                projectDtlPurDtl4SOF.ProjectDtlPurDtl4SOFsMid = projectDtlPurDtl4s3;
            }
            if (projectdtltextcontentgridinfo != null)
            {
                if (projectdtltextcontentgridinfo.PhId == 0)
                {
                    projectdtltextcontentgridinfo.PersistentState = PersistentState.Added;
                }
                else
                {
                    if (projectdtltextcontentgridinfo.PersistentState != PersistentState.Deleted)
                    {
                        projectdtltextcontentgridinfo.PersistentState = PersistentState.Modified;
                    }
                }
            }

            List<ProjectDtlTextContentModel> projectdtltextcontentgridinfos = new List<ProjectDtlTextContentModel>();
            projectdtltextcontentgridinfos.Add(projectdtltextcontentgridinfo);
            projectDtlTextContent = projectdtltextcontentgridinfo;
            var findedresultmst = new ProjectMstModel();
            var findedresultbudgetdtlimplplan = new FindedResults<ProjectDtlImplPlanModel>();
            var findedresultbudgetdtltextcontent = new FindedResults<ProjectDtlTextContentModel>();
            var findedresultbudgetdtlfundappl = new FindedResults<ProjectDtlFundApplModel>();
            var findedresultbudgetdtlbudgetdtl = new FindedResults<ProjectDtlBudgetDtlModel>();

            var findedresultPurchasedtl = new FindedResults<ProjectDtlPurchaseDtlModel>();
            var findedresultPurDtl4SOF = new FindedResults<ProjectDtlPurDtl4SOFModel>();
            var findedresultPerformTarget = new FindedResults<ProjectDtlPerformTargetModel>();
            long id = 0;

            if (mstforminfo.FType == "") //项目保存时，如果没有进度状态，则增加
            {

                var processStatus = BudgetProcessCtrlService.FindBudgetProcessCtrl(mstforminfo.FDeclarationUnit, mstforminfo.FBudgetDept, mstforminfo.FYear);
                //单位进度控制为1时，是年初申报，为3时，为年中调整
                if (processStatus == "1")
                {
                    mstforminfo.FType = "c";
                    mstforminfo.FVerNo = "0001";
                }
                else if (processStatus == "3")
                {

                    mstforminfo.FType = "z";
                    //budgetmst.FType = "z";
                    mstforminfo.FVerNo = "0002";
                }
            }

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            //用来存取主表的保存信息
            SavedResult<long> savedResultMst = new SavedResult<long>();
            try
            {
                bool isNew = false;
                var projCode = mstforminfo.FProjCode;
                var year = mstforminfo.FYear;

                bool bCreateProjCode = false; //项目编码是是否为本次生成的： true 是, false 否

                //新增的项目
                if (mstforminfo.PhId == 0)
                {
                    mstforminfo.FDeclarationDept = ProjectMstService.GetDefaultDept(projectAllData.UserId);
                    isNew = true;
                    #region 生成项目编码       
                    if (string.IsNullOrEmpty(mstforminfo.FProjCode))
                    {
                        bCreateProjCode = true; //项目代码为本次生成

                        //获取最大项目库编码
                        //projCode = ProjectMstService.CreateOrGetMaxProjCode(year);
                        //编码改成年度+部门编码+序号
                        projCode = year + mstforminfo.FBudgetDept + ProjectMstService.CreateOrGetMaxProjCode(year).Substring(8, 4);

                        mstforminfo.FProjCode = projCode;
                    }
                    else
                    {
                        bCreateProjCode = false; //项目代码为引用
                    }
                    #endregion                    
                }

                #region 生成项目明细编码: 项目明细编码=项目编码 + 6位流水号
                string dtlCode = "";
                string dtlName = "";
                if (projectdtlbudgetdtlgridinfo != null && projectdtlbudgetdtlgridinfo.Count > 0)
                {
                    for (var i = 0; i < projectdtlbudgetdtlgridinfo.Count; i++)
                    {
                        dtlCode = projectdtlbudgetdtlgridinfo[i].FDtlCode;
                        dtlName = projectdtlbudgetdtlgridinfo[i].FName;
                        if (!string.IsNullOrEmpty(dtlName))
                        {
                            dtlName = dtlName.Trim();
                        }
                        if (string.IsNullOrEmpty(dtlCode))
                        {
                            //多行存在明细项目相同的视为同一个明细项目，后台存一个代码；(相同项目名称的项目代码相同)
                            projectdtlbudgetdtlgridinfo[i].FDtlCode = projCode + string.Format("{0:D6}", i + 1);
                            //for (var j = 0; j < i; j++)
                            //{
                            //    if (dtlName == projectdtlbudgetdtlgridinfo[j].FName.Trim())
                            //    {
                            //        projectdtlbudgetdtlgridinfo[i].FDtlCode = projectdtlbudgetdtlgridinfo[j].FDtlCode;
                            //        break;
                            //    }
                            //}

                        }

                        dtlCode = projectdtlbudgetdtlgridinfo[i].FDtlCode; ;

                        for (var j = 0; j < projectdtlpurchasedtlforminfo.Count; j++)
                        {
                            if (projectdtlpurchasedtlforminfo[j].FName == dtlName)
                            {
                                projectdtlpurchasedtlforminfo[j].FDtlCode = dtlCode;
                            }
                        }

                        for (var j = 0; j < projectdtlpurdtl4sofgridinfo.Count; j++)
                        {
                            if (projectdtlpurdtl4sofgridinfo[j].FName == dtlName)
                            {
                                projectdtlpurdtl4sofgridinfo[j].FDtlCode = dtlCode;
                            }
                        }


                    }
                }

                #endregion

                #region 缓存删除的明细信息
                List<GHProjDtlShareModel> projShareDeleteList = new List<GHProjDtlShareModel>();
                GHProjDtlShareModel pshareM = null;
                foreach (var budgetDtlModel in projectDtlBudgetDtl.ProjectDtlBudgetDtlsDel)
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
                    id = mstforminfo.PhId;
                    //年中调整时,项目审批状态改为未审批,项目属性改为预立项
                    mstforminfo.FApproveStatus = "1";
                    mstforminfo.FProjStatus = 1;


                    findedresultmst = ProjectMstService.Find(id).Data;

                    //根据项目代码去项目表里查找相同代码的条数,得知相关版本号
                    var dicWhereLife = new Dictionary<string, object>();
                    new CreateCriteria(dicWhereLife).Add(ORMRestrictions<string>.Eq("FProjCode", findedresultmst.FProjCode));
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
                //if (!string.IsNullOrEmpty(mstforminfo.PhId.ToString()) && mstforminfo.PhId != 0)
                //{
                //    ProjectMstService.SaveModify2(mstforminfo, projectDtlImplPlan, projectdtltextcontentgridinfo, projectDtlFundAppl, projectDtlBudgetDtl, projectDtlPerformTarget, projectDtlPurchaseDtl, projectDtlPurDtl4SOF);//保存预算单据修改记录
                //}

                //保存
                //savedresult = ProjectMstService.SaveProjectMst(mstforminfo.AllRow[0], projectdtlimplplangridinfo.AllRow, projectdtltextcontentgridinfo.AllRow, projectdtlfundapplgridinfo.AllRow, projectdtlbudgetdtlgridinfo.AllRow);
                if (projectdtlpurchasedtlforminfo.Count > 0 && projectdtlpurdtl4sofgridinfo.Count > 0)
                {
                    //当不是新增时,先删除原有采购和采购资金来源数据,重新保存
                    if (!string.IsNullOrEmpty(mstforminfo.PhId.ToString()) && mstforminfo.PhId != 0)
                    {
                        ProjectMstService.DeleteProjectDtlPurchase(mstforminfo.PhId);
                        foreach (var a in projectdtlpurchasedtlforminfo)
                        {
                            a.PhId = 0;
                            a.PersistentState = PersistentState.Added;
                        }
                        foreach (var b in projectdtlpurdtl4sofgridinfo)
                        {
                            b.PhId = 0;
                            b.PersistentState = PersistentState.Added;
                        }
                    }
                    savedresult = ProjectMstService.SaveProjectMst(mstforminfo, projectdtltextcontentgridinfos, projectdtlpurchasedtlforminfo, projectdtlpurdtl4sofgridinfo, projectdtlperformtargetgridinfo, projectdtlfundapplgridinfo, projectdtlbudgetdtlgridinfo, projectdtlimplplangridinfo);
                }
                else
                {
                    savedresult = ProjectMstService.SaveProjectMst(mstforminfo, projectdtltextcontentgridinfos, null, null, projectdtlperformtargetgridinfo, projectdtlfundapplgridinfo, projectdtlbudgetdtlgridinfo, projectdtlimplplangridinfo);
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
                                MC = mstforminfo.FProjName,
                                DEFSTR1 = "1",
                                DEFSTR3 = mstforminfo.FDeclarationUnit,
                                PersistentState = PersistentState.Added
                            };
                            SavedResult<Int64> sr = ProjLibService.Save<Int64>(pModel, "");
                        }
                        else
                        {
                            var find = ProjLibService.Find(t => t.DM == projCode);
                            if (find.Data.Count > 0)
                            {
                                ProjLibProjModel pModel = find.Data[0];
                                pModel.DEFSTR1 = "1";
                                pModel.PersistentState = PersistentState.Modified;

                                SavedResult<Int64> sr = ProjLibService.Save<Int64>(pModel, "");
                            }
                        }
                    }
                }
                /*
                #region 更新z_qtgkxm
                if (mstforminfo.PersistentState == PersistentState.Modified)
                {
                    var find = ProjLibService.Find(t => t.DM == projCode);
                    if (find.Data.Count > 0)
                    {
                        ProjLibProjModel pModel = find.Data[0];
                        pModel.MC = mstforminfo.FProjName;
                        pModel.DEFSTR1 = "1";
                        pModel.PersistentState = PersistentState.Modified;
                        SavedResult<Int64> sr = ProjLibService.Save<Int64>(pModel, "");
                    }
                }
                #endregion

                #region 保存项目明细数据到JJ_FXGL中
                //新增的
                List<GHProjDtlShareModel> shareModels = new List<GHProjDtlShareModel>();
                GHProjDtlShareModel shareModel = null;
                foreach (var budgetDtlModel in projectDtlBudgetDtl.ProjectDtlBudgetDtlsAdd)
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
                var res = ProjDtlShareService.Save<Int64>(shareModels, "");

                //修改的 
                shareModels.Clear();
                foreach (var budgetDtlModel in projectDtlBudgetDtl.ProjectDtlBudgetDtlsMid)
                {
                    var findRes = ProjDtlShareService.Find(t => t.DM == projCode && t.XMDM == budgetDtlModel.FDtlCode);
                    if (findRes.Data.Count > 0)
                    {
                        shareModel = findRes.Data[0];
                        shareModel.MC = budgetDtlModel.FName;
                        shareModel.PersistentState = PersistentState.Modified;
                        shareModels.Add(shareModel);
                    }
                }
                res = ProjDtlShareService.Save<Int64>(shareModels, "");

                //删除的
                if (projShareDeleteList.Count > 0)
                {
                    //res = ProjDtlShareService.Save<Int64>(projShareDeleteList);
                    foreach (var m in projShareDeleteList)
                    {
                        //删除项目库对应项目
                        var dicWhere = new Dictionary<string, object>();
                        new CreateCriteria(dicWhere)
                            .Add(ORMRestrictions<string>.Eq("DM", m.DM))
                            .Add(ORMRestrictions<string>.Eq("XMDM", m.XMDM)); //闭区间
                        var result = ProjDtlShareService.Delete(dicWhere);
                    }
                }
                #endregion
                */
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
                    ProjectMstService.SaveProjectMst(findedresultmst, budgetdtltextcontent, Purchasedtl, PurDtl4SOF, performtargetgridinfo, budgetdtlfundappl, budgetdtlbudgetdtl, budgetdtlimplplan);
                }
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 保存立项数据（含附件）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PostSaveProject2()
        {
            List<QtAttachmentModel> attachmentModels = new List<QtAttachmentModel>();
            List<QtAttachmentModel> oldattachmentModels = new List<QtAttachmentModel>();
            //具体数据对象
            long projectPhid = 0;
            //判断form表单类型是否正确
            if (!Request.Content.IsMimeMultipartContent())
            {
                var data1 = new
                {
                    Status = ResponseStatus.Error,
                    Msg = "请求数据不是multipart/form-data类型",
                    Data = ""
                };
                return DataConverterHelper.SerializeObject(data1);
            }
            //I6WebAppInfo i6AppInfo = (I6WebAppInfo)HttpContext.Current.Session["NGWebAppInfo"] ?? null;
            //获取AppInfo值 头部信息记录
            var base64EncodedBytes = Convert.FromBase64String(HttpContext.Current.Request.Headers.GetValues("AppInfo").First());
            var jsonText = Encoding.UTF8.GetString(base64EncodedBytes);
            var AppInfo = JsonConvert.DeserializeObject<AppInfoBase>(jsonText);


            //如果路径不存在,创建路径
            var root = System.Web.Hosting.HostingEnvironment.MapPath("~/UpLoadFiles/ProjectMst/");
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string filePath = Path.Combine(root, date);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var multipartMemoryStreamProvider = await Request.Content.ReadAsMultipartAsync();
            var contentsList = multipartMemoryStreamProvider.Contents;

            foreach (var content in contentsList)
            {
                //通过判断fileName是否为空,是否为文件
                if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                {
                    //处理文件名字符串
                    string fileName = content.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                    using (Stream stream = await content.ReadAsStreamAsync())
                    {
                        //文件如果大于100MB  提示不允许
                        if (stream.Length > 104857600)
                        {
                            return DCHelper.ErrorMessage("上传的文件不能大于100MB！");
                        }
                        byte[] bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, bytes.Length);
                        stream.Seek(0, SeekOrigin.Begin);

                        //获取对应文件后缀名
                        string extension = Path.GetExtension(fileName);
                        //获取文件名
                        string b_name = Path.GetFileName(fileName);

                        //修改文件名
                        string newFileName = Guid.NewGuid().ToString("N") + extension;
                        string uploadPath = Path.Combine(filePath, newFileName);

                        //保存文件
                        MemoryStream ms = new MemoryStream(bytes);
                        FileStream fs = new FileStream(uploadPath, FileMode.Create);
                        ms.WriteTo(fs);
                        ms.Close();
                        fs.Close();

                        string b_urlpath = "/UpLoadFiles/ProjectMst/" + date + "/" + newFileName;

                        QtAttachmentModel attachmentModel = new QtAttachmentModel();
                        attachmentModel.BName = b_name;
                        attachmentModel.BSize = decimal.Round((decimal)stream.Length / 1024, 2);
                        attachmentModel.BTable = "XM3_PROJECTMST";
                        attachmentModel.BType = extension;
                        attachmentModel.BUrlpath = b_urlpath;
                        attachmentModel.PersistentState = PersistentState.Added;
                        attachmentModels.Add(attachmentModel);
                    }
                }
                else
                {
                    //获取键值对值,并通过反射获取对象中的属性
                    string key = content.Headers.ContentDisposition.Name.Replace("\"", string.Empty);
                    string value = await content.ReadAsStringAsync();
                    //取项目主键
                    if (key == "PhId")
                    {
                        projectPhid = long.Parse(value);
                    }
                    else if (key == "OldAttachments")
                    {
                        var value2 = JsonConvert.DeserializeObject<List<QtAttachmentModel>>(value);
                        oldattachmentModels = value2;
                    }
                }
            }
            //string mstformData = System.Web.HttpContext.Current.Request.Form["mstformData"];
            //string projectdtlimplplangridData = System.Web.HttpContext.Current.Request.Form["projectdtlimplplangridData"];
            //string projectdtltextcontentgridData = System.Web.HttpContext.Current.Request.Form["projectdtltextcontentgridData"];
            //string projectdtlfundapplgridData = System.Web.HttpContext.Current.Request.Form["projectdtlfundapplgridData"];
            //string projectdtlbudgetdtlgridData = System.Web.HttpContext.Current.Request.Form["projectdtlbudgetdtlgridData"];
            //string projectdtlperformtargetgridData = System.Web.HttpContext.Current.Request.Form["projectdtlperformtargetgridData"];
            //string projectdtlpurchasedtlformData = System.Web.HttpContext.Current.Request.Form["projectdtlpurchasedtlformData"];
            //string projectdtlpurdtl4sofgridData = System.Web.HttpContext.Current.Request.Form["projectdtlpurdtl4sofgridData"];

            if (projectPhid <= 0)
            {
                return DCHelper.ErrorMessage("项目主表保存有误！");
            }
            try
            {
                SavedResult<long> savedResult = new SavedResult<long>();
                //原有的附件要删除
                IList<QtAttachmentModel> oldAttachments = new List<QtAttachmentModel>();
                oldAttachments = this.QtAttachmentService.Find(t => t.BTable == "XM3_PROJECTMST" && t.RelPhid == projectPhid).Data;
                if (oldAttachments != null && oldAttachments.Count > 0)
                {
                    foreach (var oldAtt in oldAttachments)
                    {
                        oldAtt.PersistentState = PersistentState.Deleted;
                    }
                    this.QtAttachmentService.Save<long>(oldAttachments, "");
                }
                if (attachmentModels != null && attachmentModels.Count > 0)
                {
                    foreach (var att in attachmentModels)
                    {
                        att.RelPhid = projectPhid;
                        att.BTable = "XM3_PROJECTMST";
                        att.PersistentState = PersistentState.Added;
                    }
                }
                if (oldattachmentModels != null && oldattachmentModels.Count > 0)
                {
                    foreach (var oldAtt in oldattachmentModels)
                    {
                        oldAtt.RelPhid = projectPhid;
                        oldAtt.BTable = "XM3_PROJECTMST";
                        oldAtt.PersistentState = PersistentState.Added;
                        attachmentModels.Add(oldAtt);
                    }
                }
                if (attachmentModels != null && attachmentModels.Count > 0)
                {
                    savedResult = this.QtAttachmentService.Save<long>(attachmentModels, "");
                }
                return DataConverterHelper.SerializeObject(savedResult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 项目确认执行时保存附件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PostSaveAttach()
        {
            List<QtAttachmentModel> attachmentModels = new List<QtAttachmentModel>();
            //具体数据对象
            long projectPhid = 0;
            //判断form表单类型是否正确
            if (!Request.Content.IsMimeMultipartContent())
            {
                var data1 = new
                {
                    Status = ResponseStatus.Error,
                    Msg = "请求数据不是multipart/form-data类型",
                    Data = ""
                };
                return DataConverterHelper.SerializeObject(data1);
            }
            //I6WebAppInfo i6AppInfo = (I6WebAppInfo)HttpContext.Current.Session["NGWebAppInfo"] ?? null;
            //获取AppInfo值 头部信息记录
            var base64EncodedBytes = Convert.FromBase64String(HttpContext.Current.Request.Headers.GetValues("AppInfo").First());
            var jsonText = Encoding.UTF8.GetString(base64EncodedBytes);
            var AppInfo = JsonConvert.DeserializeObject<AppInfoBase>(jsonText);


            //如果路径不存在,创建路径
            var root = System.Web.Hosting.HostingEnvironment.MapPath("~/UpLoadFiles/ProjectDtlTextContent/");
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string filePath = Path.Combine(root, date);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var multipartMemoryStreamProvider = await Request.Content.ReadAsMultipartAsync();
            var contentsList = multipartMemoryStreamProvider.Contents;

            foreach (var content in contentsList)
            {
                //通过判断fileName是否为空,是否为文件
                if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                {
                    //处理文件名字符串
                    string fileName = content.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                    using (Stream stream = await content.ReadAsStreamAsync())
                    {
                        //文件如果大于100MB  提示不允许
                        if (stream.Length > 104857600)
                        {
                            return DCHelper.ErrorMessage("上传的文件不能大于100MB！");
                        }
                        byte[] bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, bytes.Length);
                        stream.Seek(0, SeekOrigin.Begin);

                        //获取对应文件后缀名
                        string extension = Path.GetExtension(fileName);
                        //获取文件名
                        string b_name = Path.GetFileName(fileName);

                        //修改文件名
                        string newFileName = Guid.NewGuid().ToString("N") + extension;
                        string uploadPath = Path.Combine(filePath, newFileName);

                        //保存文件
                        MemoryStream ms = new MemoryStream(bytes);
                        FileStream fs = new FileStream(uploadPath, FileMode.Create);
                        ms.WriteTo(fs);
                        ms.Close();
                        fs.Close();

                        string b_urlpath = "/UpLoadFiles/ProjectDtlTextContent/" + date + "/" + newFileName;

                        QtAttachmentModel attachmentModel = new QtAttachmentModel();
                        attachmentModel.BName = b_name;
                        attachmentModel.BSize = decimal.Round((decimal)stream.Length / 1024, 2);
                        attachmentModel.BTable = "XM3_PROJECTDTL_TEXTCONTENT";
                        attachmentModel.BType = extension;
                        attachmentModel.BUrlpath = b_urlpath;
                        attachmentModel.PersistentState = PersistentState.Added;
                        attachmentModels.Add(attachmentModel);
                    }
                }
                else
                {
                    //获取键值对值,并通过反射获取对象中的属性
                    string key = content.Headers.ContentDisposition.Name.Replace("\"", string.Empty);
                    string value = await content.ReadAsStringAsync();
                    //取项目主键
                    projectPhid = long.Parse(value);
                }
            }

            if (projectPhid <= 0)
            {
                return DCHelper.ErrorMessage("项目主表保存有误！");
            }
            try
            {
                SavedResult<long> savedResult = new SavedResult<long>();
                //原有的附件要删除
                IList<QtAttachmentModel> oldAttachments = new List<QtAttachmentModel>();
                oldAttachments = this.QtAttachmentService.Find(t => t.BTable == "XM3_PROJECTDTL_TEXTCONTENT" && t.RelPhid == projectPhid).Data;
                if (oldAttachments != null && oldAttachments.Count > 0)
                {
                    foreach (var oldAtt in oldAttachments)
                    {
                        oldAtt.PersistentState = PersistentState.Deleted;
                    }
                    savedResult = this.QtAttachmentService.Save<long>(oldAttachments, "");
                }
                if (attachmentModels != null && attachmentModels.Count > 0)
                {
                    foreach (var att in attachmentModels)
                    {
                        att.RelPhid = projectPhid;
                        att.BTable = "XM3_PROJECTDTL_TEXTCONTENT";
                        att.PersistentState = PersistentState.Added;
                    }
                    savedResult = this.QtAttachmentService.Save<long>(attachmentModels, "");
                }
                return DataConverterHelper.SerializeObject(savedResult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }


        /// <summary>
        /// 删除预算项目
        /// </summary>
        /// <param name="mstRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostDeleteProject([FromBody]ProjectMstRequest mstRequest)
        {
            if (string.IsNullOrEmpty(mstRequest.FProjCode))
            {
                return DCHelper.ErrorMessage("项目编码不能为空！");
            }
            if (mstRequest.FProjPhId == 0)
            {
                return DCHelper.ErrorMessage("项目主键不能为空！");
            }
            try
            {
                string projCode = mstRequest.FProjCode;    //项目代码
                long id = mstRequest.FProjPhId; //主表主键

                var findedresultmst = ProjectMstService.Find(id).Data;
                //做能否删除的判断
                if (findedresultmst.FApproveStatus != "1" && findedresultmst.FApproveStatus != "5")
                {
                    return DCHelper.ErrorMessage("只允许删除未上报或暂存的项目！");
                }
                //根据项目代码去项目表里查找相同代码的条数,得知相关版本号
                var dicWhereLife = new Dictionary<string, object>();
                new CreateCriteria(dicWhereLife).Add(ORMRestrictions<string>.Eq("FProjCode", findedresultmst.FProjCode));
                var FLifeCycle = ProjectMstService.Find(dicWhereLife);

                var deletedresult = ProjectMstService.Delete<System.Int64>(id);
                //相同项目代码只有一条时,表示没做过调整,则直接删除;多于一条时,则找版本号最大(即最新)那条单据,版本号改为0
                if (FLifeCycle.Data.Count == 1)
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
                }
                return DataConverterHelper.SerializeObject(deletedresult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 批量删除项目
        /// </summary>
        /// <param name="mstRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostDeteleProjects([FromBody]ProjectMstRequest mstRequest)
        {
            if (mstRequest.FProjCodeList == null || mstRequest.FProjCodeList.Count < 1)
            {
                return DCHelper.ErrorMessage("项目编码集合不能为空！");
            }
            if (mstRequest.FProjPhIdList == null || mstRequest.FProjPhIdList.Count < 1)
            {
                return DCHelper.ErrorMessage("项目主键集合不能为空！");
            }
            if (mstRequest.FProjCodeList.Count != mstRequest.FProjPhIdList.Count)
            {
                return DCHelper.ErrorMessage("项目主键集合与项目编码集合不对应！");
            }
            try
            {
                DeletedResult deletedresult = new DeletedResult();
                for (int i = 0; i < mstRequest.FProjCodeList.Count; i++)
                {
                    string projCode = mstRequest.FProjCodeList[i];    //项目代码
                    long id = mstRequest.FProjPhIdList[i]; //主表主键

                    var findedresultmst = ProjectMstService.Find(id).Data;

                    if (findedresultmst.FApproveStatus != "1" && findedresultmst.FApproveStatus != "5")
                    {
                        return DCHelper.ErrorMessage("只允许删除未上报或暂存的项目！");
                    }
                    //根据项目代码去项目表里查找相同代码的条数,得知相关版本号
                    var dicWhereLife = new Dictionary<string, object>();
                    new CreateCriteria(dicWhereLife).Add(ORMRestrictions<string>.Eq("FProjCode", findedresultmst.FProjCode));
                    var FLifeCycle = ProjectMstService.Find(dicWhereLife);

                    deletedresult = ProjectMstService.Delete<System.Int64>(id);
                    //相同项目代码只有一条时,表示没做过调整,则直接删除;多于一条时,则找版本号最大(即最新)那条单据,版本号改为0
                    if (FLifeCycle.Data.Count == 1)
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
                    }
                }

                return DataConverterHelper.SerializeObject(deletedresult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }


        /// <summary>
        /// 取消审批
        /// </summary>
        /// <param name="mstRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostNoAppval([FromBody]ProjectMstRequest mstRequest)
        {
            if (mstRequest.FProjPhId == 0)
            {
                return DCHelper.ErrorMessage("项目主键不能为空！");
            }
            try
            {
                long id = mstRequest.FProjPhId;  //主表主键
                var findedresultmst = ProjectMstService.Find(id).Data;
                findedresultmst.FApproveStatus = "2";
                findedresultmst.FDateofDeclaration = DateTime.Now;
                findedresultmst.PersistentState = PersistentState.Modified;
                ProjectMstService.Save<Int64>(findedresultmst, "");
                SavedResult<Int64> savedresult = new SavedResult<Int64>();
                savedresult.Status = ResponseStatus.Success;
                return DataConverterHelper.SerializeObject(savedresult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 项目生成预算
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetSaveBudgetMst([FromUri] long id)
        {
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
            budgetmst.FApproveStatus = "3";
            budgetmst.XmMstPhid = findedresultmst.Data.PhId;
            // budgetmst.FApproveStatus ="3";  //项目生成预算审批状态不变
            //查找预算单位下该部门所处预算进度
            /*var processStatus = BudgetProcessCtrlService.FindBudgetProcessCtrl(budgetmst.FDeclarationUnit, budgetmst.FBudgetDept, budgetmst.FYear);
            if (processStatus == "1")
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
                if (Ftype1.Data.Count > 0)
                {
                    budgetmst.FType = Ftype1.Data[0].FType;
                }
                else
                {
                    budgetmst.FType = "z";
                }

                //budgetmst.FType = "z";
                budgetmst.FVerNo = "0001";
            }*/
            //vue改造 写死c0001
            budgetmst.FType = "c";
            budgetmst.FVerNo = "0001";

            budgetmst.FMidYearChange = "0";
            budgetmst.FBudgetAmount = budgetmst.FProjAmount;
            budgetmst.PersistentState = PersistentState.Added;

            //model.PersistentState = PersistentState.Added;
            // budgetmst.Add(model);
            //置空
            budgetmst.FMeetingTime = null;
            budgetmst.FMeetiingSummaryNo = "";

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
                //置空
                model.FLeadingOpinions = "";
                model.FChairmanOpinions = "";
                model.FBz = "";
                model.FDeptOpinions = "";
                model.FDeptOpinions2 = "";
                model.FResolution = "";

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
                var findBudget = BudgetMstService.Find(dicWhere1);
                long oldBudgetPhid = 0;
                if (findBudget.Data.Count > 0)
                {
                    oldBudgetPhid = findBudget.Data[0].PhId;
                }

                //向老g6h同步数据
                /*if (findedresultmst.Data.FSaveToOldG6h != 1)
                {
                    string TBresult = BudgetMstService.AddDataInSaveBudgetMst(budgetmst, budgetdtlbudgetdtl);
                    if (TBresult != "")
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
                }*/

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
                    BudgetMstService.Save<Int64>(findBudget.Data[0], "");//

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
                findedresultmst.Data.FApproveStatus = "3";
                findedresultmst.Data.PersistentState = PersistentState.Modified;
                ProjectMstService.Save<Int64>(findedresultmst.Data, "");
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
                    CorrespondenceSettingsService.Save<Int64>(dygxModel, "");
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
        /// 根据主键集合修改作废状态
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostCancetProjectList([FromBody]Model.Request.BaseListModel paramters)
        {
            try
            {
                if (paramters.fPhIdList == null || paramters.fPhIdList.Count() < 1)
                {
                    return DCHelper.ErrorMessage("传递的单据集合有误！");
                }
                List<long> fCodes = new List<long>();
                for (int i = 0; i < paramters.fPhIdList.Count(); i++)
                {
                    fCodes.Add(long.Parse(paramters.fPhIdList[i]));
                }
                var result = this.ProjectMstService.PostCancetProjectList(fCodes);
                return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 根据主键集合上报（不启用工作流）
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostValid([FromBody]Model.Request.BaseListModel paramters)
        {
            if (paramters.fPhIdList == null || paramters.fPhIdList.Count() < 1)
            {
                return DCHelper.ErrorMessage("传递的单据集合有误！");
            }
            //立项与预立项的单据上报要进行进度控制的判断
            List<long> phids = new List<long>();
            foreach (var phid in paramters.fPhIdList)
            {
                phids.Add(long.Parse(phid));
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Clear();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<List<long>>.In("PhId", phids))
                .Add(ORMRestrictions<int>.Eq("FLifeCycle", 0));
            var paymentList = this.ProjectMstService.Find(dic).Data;
            if (paymentList != null && paymentList.Count > 0)
            {
                List<string> budList = paymentList.Select(t => t.FBudgetDept).Distinct().ToList();
                List<string> yearList = paymentList.Select(t => t.FYear).Distinct().ToList();
                if (budList != null && budList.Count > 0 && yearList != null && yearList.Count > 0)
                {
                    var budProcess = this.BudgetProcessCtrlService.Find(t => budList.Contains(t.FDeptCode) && yearList.Contains(t.FYear)).Data;
                    if (budProcess != null && budProcess.Count > 0)
                    {
                        foreach (var payM in paymentList)
                        {
                            var process = budProcess.ToList().Find(t => t.FDeptCode == payM.FBudgetDept && t.FYear == payM.FYear);
                            if (process != null && process.FProcessStatus != "1")
                            {
                                throw new Exception("有单据的预算部门的进度已不在年初申报，因此无法上报！");
                            }
                        }
                    }
                }
            }
            var MstList = new List<ProjectMstModel>();
            var SuccessNum = 0;
            var FailNum = 0;
            foreach (var fPhId in paramters.fPhIdList)
            {
                var mst = ProjectMstService.Find(long.Parse(fPhId)).Data;
                if (mst.FProjStatus == 1 && mst.FApproveStatus == "1")
                {
                    mst.FApproveStatus = "2";
                    mst.FApproveDate = DateTime.Now;
                    mst.FApprover = base.AppInfo.UserId;
                    mst.PersistentState = PersistentState.Modified;
                    MstList.Add(mst);
                    SuccessNum++;
                }
                else
                {
                    FailNum++;
                }
            }
            if (SuccessNum > 0)
            {
                try
                {
                    ProjectMstService.Save<Int64>(MstList, "");
                }
                catch (Exception ex)
                {
                    return DCHelper.ErrorMessage(ex.Message);
                }
            }
            var result = new
            {
                SuccessNum = SuccessNum,
                FailNum = FailNum
            };
            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 根据主键集合转立项（不启用工作流）
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostVerify([FromBody]Model.Request.BaseListModel paramters)
        {
            if (paramters.fPhIdList == null || paramters.fPhIdList.Count() < 1)
            {
                return DCHelper.ErrorMessage("传递的单据集合有误！");
            }
            //立项与预立项的单据转立项要进行进度控制的判断
            List<long> phids = new List<long>();
            foreach (var phid in paramters.fPhIdList)
            {
                phids.Add(long.Parse(phid));
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Clear();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<List<long>>.In("PhId", phids))
                .Add(ORMRestrictions<int>.Eq("FLifeCycle", 0));
            var paymentList = this.ProjectMstService.Find(dic).Data;
            if (paymentList != null && paymentList.Count > 0)
            {
                List<string> budList = paymentList.Select(t => t.FBudgetDept).Distinct().ToList();
                List<string> yearList = paymentList.Select(t => t.FYear).Distinct().ToList();
                if (budList != null && budList.Count > 0 && yearList != null && yearList.Count > 0)
                {
                    var budProcess = this.BudgetProcessCtrlService.Find(t => budList.Contains(t.FDeptCode) && yearList.Contains(t.FYear)).Data;
                    if (budProcess != null && budProcess.Count > 0)
                    {
                        foreach (var payM in paymentList)
                        {
                            var process = budProcess.ToList().Find(t => t.FDeptCode == payM.FBudgetDept && t.FYear == payM.FYear);
                            if (process != null && process.FProcessStatus != "1")
                            {
                                throw new Exception("有单据的预算部门的进度已不在年初申报，因此无法上报！");
                            }
                        }
                    }
                }
            }
            var MstList = new List<ProjectMstModel>();
            var SuccessNum = 0;
            var FailNum = 0;
            foreach (var fPhId in paramters.fPhIdList)
            {
                var mst = ProjectMstService.Find(long.Parse(fPhId)).Data;
                if (mst.FProjStatus == 1 && mst.FApproveStatus == "2")
                {
                    mst.FProjStatus = 2;
                    mst.FApproveStatus = "1";
                    mst.FApproveDate = DateTime.Now;
                    mst.FApprover = base.AppInfo.UserId;
                    mst.PersistentState = PersistentState.Modified;
                    MstList.Add(mst);
                    SuccessNum++;
                }
                else
                {
                    FailNum++;
                }
            }
            if (SuccessNum > 0)
            {
                try
                {
                    ProjectMstService.Save<Int64>(MstList, "");
                }
                catch (Exception ex)
                {
                    return DCHelper.ErrorMessage(ex.Message);
                }
            }
            var result = new
            {
                SuccessNum = SuccessNum,
                FailNum = FailNum
            };
            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 根据主键集合驳回（不启用工作流）
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostUnValid([FromBody]Model.Request.BaseListModel paramters)
        {
            if (paramters.fPhIdList == null || paramters.fPhIdList.Count() < 1)
            {
                return DCHelper.ErrorMessage("传递的单据集合有误！");
            }
            //立项与预立项的单据驳回要进行进度控制的判断
            List<long> phids = new List<long>();
            foreach (var phid in paramters.fPhIdList)
            {
                phids.Add(long.Parse(phid));
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Clear();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<List<long>>.In("PhId", phids))
                .Add(ORMRestrictions<int>.Eq("FLifeCycle", 0));
            var paymentList = this.ProjectMstService.Find(dic).Data;
            if (paymentList != null && paymentList.Count > 0)
            {
                List<string> budList = paymentList.Select(t => t.FBudgetDept).Distinct().ToList();
                List<string> yearList = paymentList.Select(t => t.FYear).Distinct().ToList();
                if (budList != null && budList.Count > 0 && yearList != null && yearList.Count > 0)
                {
                    var budProcess = this.BudgetProcessCtrlService.Find(t => budList.Contains(t.FDeptCode) && yearList.Contains(t.FYear)).Data;
                    if (budProcess != null && budProcess.Count > 0)
                    {
                        foreach (var payM in paymentList)
                        {
                            var process = budProcess.ToList().Find(t => t.FDeptCode == payM.FBudgetDept && t.FYear == payM.FYear);
                            if (process != null && process.FProcessStatus != "1")
                            {
                                throw new Exception("有单据的预算部门的进度已不在年初申报，因此无法上报！");
                            }
                        }
                    }
                }
            }
            var MstList = new List<ProjectMstModel>();
            var SuccessNum = 0;
            var FailNum = 0;
            foreach (var fPhId in paramters.fPhIdList)
            {
                var mst = ProjectMstService.Find(long.Parse(fPhId)).Data;
                if (mst.FProjStatus == 2 && mst.FApproveStatus == "1")////待执行数据 驳回为预立项,待审批数据
                {
                    mst.FProjStatus = 1;
                    mst.FApproveStatus = "2";
                    mst.FApproveDate = DateTime.Now;
                    mst.FApprover = base.AppInfo.UserId;
                    mst.PersistentState = PersistentState.Modified;
                    MstList.Add(mst);
                    SuccessNum++;
                }
                else if (mst.FProjStatus == 1 && mst.FApproveStatus == "2")
                {
                    mst.FApproveStatus = "1";
                    mst.FApproveDate = DateTime.Now;
                    mst.FApprover = base.AppInfo.UserId;
                    mst.PersistentState = PersistentState.Modified;
                    MstList.Add(mst);
                    SuccessNum++;
                }
                else
                {
                    FailNum++;
                }
            }
            if (SuccessNum > 0)
            {
                try
                {
                    ProjectMstService.Save<Int64>(MstList, "");
                }
                catch (Exception ex)
                {
                    return DCHelper.ErrorMessage(ex.Message);
                }
            }
            var result = new
            {
                SuccessNum = SuccessNum,
                FailNum = FailNum
            };
            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 项目同步数据到老G6H
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string PostAddData([FromBody]Model.Request.BaseListModel paramters)
        {
            if (paramters.fPhIdList == null || paramters.fPhIdList.Count() < 1)
            {
                return DCHelper.ErrorMessage("传递的单据集合有误！");
            }
            string result = ProjectMstService.AddData(paramters.fPhIdList);
            if (result == "")
            {
                return DCHelper.SuccessMessage("同步成功");
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 得到批量打印的数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string PostPrintData([FromBody]Model.Request.BaseListModel paramters)
        {
            if (paramters.fPhIdList == null || paramters.fPhIdList.Count() < 1)
            {
                return DCHelper.ErrorMessage("传递的单据集合有误！");
            }

            return DataConverterHelper.SerializeObject(ProjectMstService.PostPrintData(paramters.fPhIdList));
        }

        /// <summary>
        /// 取没同步到老G6H里，是项目立项已审批的数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetProjectMstListToSaveOldG6h([FromUri]Model.Request.BaseListModel paramters)
        {
            var dicWhere = new Dictionary<string, object>();

            var dicWhereDept = new Dictionary<string, object>();
            new CreateCriteria(dicWhereDept)
                .Add(ORMRestrictions<string>.Eq("Dwdm", paramters.Ucode)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
            var deptList = CorrespondenceSettingsService.Find(dicWhereDept);
            List<string> deptL = new List<string>();
            for (var i = 0; i < deptList.Data.Count; i++)
            {
                deptL.Add(deptList.Data[i].Dydm);
            }
            ////在“初报完成”和“调整完成”阶段，不允许用户进行【同步数据】操作
            //根据操作员部门列表查找对应部门进度状态
            var budgetProcessList = BudgetProcessCtrlService.FindBudgetProcessCtrlByList(deptL, paramters.Year);
            List<string> deptLWithPro = new List<string>();
            for (var i = 0; i < budgetProcessList.Data.Count; i++)
            {
                //取进度状态在年初或年中调整的部门
                if (budgetProcessList.Data[i].FProcessStatus == "1" || budgetProcessList.Data[i].FProcessStatus == "3")
                {
                    deptLWithPro.Add(budgetProcessList.Data[i].FDeptCode);
                }
            }

            new CreateCriteria(dicWhere).Add(ORMRestrictions<Int32>.Eq("FProjStatus", 9))
                .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptLWithPro))
                .Add(ORMRestrictions<Int32>.Eq("FSaveToOldG6h", 0)).Add(ORMRestrictions<string>.Eq("FYear", paramters.Year));


            var result = ProjectMstService.Find(dicWhere, new string[] { "FProjCode Asc" }).Data;

            return DCHelper.ModelListToJson<ProjectMstModel>(result, (Int32)result.Count);
        }

        /// <summary>
        /// 根据主键集合预执行
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostExecute([FromBody]Model.Request.BaseListModel paramters)
        {
            if (paramters.fPhIdList == null || paramters.fPhIdList.Count() < 1)
            {
                return DCHelper.ErrorMessage("传递的单据集合有误！");
            }
            var MstList = new List<ProjectMstModel>();
            var SuccessNum = 0;
            var FailNum = 0;
            foreach (var fPhId in paramters.fPhIdList)
            {
                var mst = ProjectMstService.Find(long.Parse(fPhId)).Data;
                if (mst.FProjStatus == 2)
                {
                    mst.FProjStatus = 9;
                    mst.FApproveDate = DateTime.Now;
                    mst.FApprover = base.AppInfo.UserId;
                    mst.PersistentState = PersistentState.Modified;
                    MstList.Add(mst);
                    SuccessNum++;
                }
                else
                {
                    FailNum++;
                }
            }
            if (SuccessNum > 0)
            {
                try
                {
                    ProjectMstService.Save<Int64>(MstList, "");
                }
                catch (Exception ex)
                {
                    return DCHelper.ErrorMessage(ex.Message);
                }
            }
            var result = new
            {
                SuccessNum = SuccessNum,
                FailNum = FailNum
            };
            return DataConverterHelper.SerializeObject(result);
        }

        #region//导出汇总与申请表

        /// <summary>
        /// 根据项目主键集合导出汇总以及申请表
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostProjectExcel([FromBody]Model.Request.BaseListModel paramters)
        {
            if (paramters.OrgId == 0)
            {
                return DCHelper.ErrorMessage("组织主键不能为空！");
            }
            if (string.IsNullOrEmpty(paramters.OrgCode))
            {
                return DCHelper.ErrorMessage("组织编码不能为空！");
            }
            if (paramters.UserId == 0)
            {
                return DCHelper.ErrorMessage("用户主键不能为空！");
            }
            List<long> phids = new List<long>();
            if (paramters.fPhIdList != null && paramters.fPhIdList.Count() > 0)
            {
                for (int i = 0; i < paramters.fPhIdList.Count(); i++)
                {
                    phids.Add(long.Parse(paramters.fPhIdList[i]));
                }
            }
            else
            {
                return DCHelper.ErrorMessage("项目主键集合不能为空！");
            }

            if (string.IsNullOrEmpty(paramters.execlType))
            {
                return DCHelper.ErrorMessage("导出的类型不能为空！");
            }
            try
            {
                //获取组织和用户信息
                User2Model userModel = new User2Model();
                OrganizeModel organizeModel = new OrganizeModel();
                userModel = this.BudgetMstService.GetUser(paramters.UserId);
                if (userModel == null)
                {
                    return DCHelper.ErrorMessage("用户信息查询失败！");
                }
                organizeModel = this.BudgetMstService.GetOrganize(paramters.OrgId);
                if (organizeModel == null)
                {
                    return DCHelper.ErrorMessage("组织信息查询失败！");
                }
                //内置的套打信息
                IList<QtCoverUpDataModel> allCoverUpDatas = new List<QtCoverUpDataModel>();
                allCoverUpDatas = this.QtCoverUpDataService.GetQtCoverUpDataList();
                //根据组织获取组织自己选择的套打格式
                IList<QtCoverUpForOrgModel> coversForOrg = new List<QtCoverUpForOrgModel>();
                coversForOrg = this.QtCoverUpForOrgService.Find(t => t.EnabledMark == (byte)0 && t.OrgId == paramters.OrgId).Data;
                if (coversForOrg != null && coversForOrg.Count > 0)
                {
                    foreach (var cover in coversForOrg)
                    {
                        cover.TypeNumber = allCoverUpDatas.ToList().Find(t => t.PhId == cover.TempLateId) == null ? 0 : allCoverUpDatas.ToList().Find(t => t.PhId == cover.TempLateId).TypeNumber;
                    }
                    string result = "";
                    QtCoverUpForOrgModel qtCoverUpForOrg = new QtCoverUpForOrgModel();
                    if (paramters.execlType.Equals("01"))//预立项汇总表
                    {
                        qtCoverUpForOrg = coversForOrg.ToList().Find(t => t.TypeNumber == 1 && t.ProcessCode == "001");
                        if (qtCoverUpForOrg != null)
                        {
                            var syssets = QTSysSetService.Find(t => t.PhId != 0).Data.ToList();
                            IList<ProjectMstModel> projectMsts = new List<ProjectMstModel>();
                            projectMsts = this.ProjectMstService.Find(t => phids.Contains(t.PhId)).Data.OrderByDescending(t => t.NgInsertDt).ThenByDescending(t => t.NgUpdateDt).ToList();

                            //IList<PerformEvalTargetTypeModel> performEvalTargetTypes = new List<PerformEvalTargetTypeModel>();
                            //performEvalTargetTypes = this.PerformEvalTargetTypeService.Find(t => t.Orgid == paramters.OrgId).Data;
                            if (projectMsts != null && projectMsts.Count > 0)
                            {
                                foreach (var pro in projectMsts)
                                {
                                    RichHelpDac helpdac = new RichHelpDac();
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FApprover", "FApprover_EXName", "fg3_user");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FPerformType", "FPerformType_EXName", "GHPerformEvalTargetTypeTree");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FPerformEvalType", "FPerformEvalType_EXName", "GHPerformEvalType");

                                    if (string.IsNullOrEmpty(pro.FDeclarationDept))
                                    {
                                        //申报部门代码转名称
                                        pro.FDeclarationDept_EXName = organizeModel.OName;
                                    }
                                    var syssetProjectMst = syssets.FindAll(x => x.DicType == "ProjectProper" && x.Orgcode == pro.FDeclarationUnit && x.TypeCode == pro.FProjAttr);
                                    if (syssetProjectMst != null && syssetProjectMst.Count > 0)
                                    {
                                        //项目属性代码转名称
                                        pro.FProjAttr_EXName = syssetProjectMst[0].TypeName;
                                    }
                                    var syssetProjectMst2 = syssets.FindAll(x => x.DicType == "TimeLimit" && x.Orgcode == pro.FDeclarationUnit && x.TypeCode == pro.FDuration);
                                    if (syssetProjectMst2 != null && syssetProjectMst2.Count > 0)
                                    {
                                        //存续期限代码转名称
                                        pro.FDuration_EXName = syssetProjectMst2[0].TypeName;
                                    }
                                    var syssetProjectMst3 = syssets.FindAll(x => x.DicType == "ProjectLevel" && x.Orgcode == pro.FDeclarationUnit && x.TypeCode == pro.FLevel);
                                    if (syssetProjectMst3 != null && syssetProjectMst3.Count > 0)
                                    {
                                        //项目级别代码转名称
                                        pro.FLevel_EXName = syssetProjectMst3[0].TypeName;
                                    }
                                    //if(performEvalTargetTypes != null && performEvalTargetTypes.Count > 0 && !string.IsNullOrEmpty(pro.FPerformType))
                                    //{
                                    //绩效评价名称
                                    //    pro.FPerformType_EXName = performEvalTargetTypes.ToList().Find(t => t.FCode == pro.FPerformType) == null ? "": performEvalTargetTypes.ToList().Find(t => t.FCode == pro.FPerformType).FName; 
                                    //}
                                    if (pro.FApprover != 0)
                                    {
                                        //审批人姓名
                                        pro.FApprover_EXName = this.BudgetMstService.GetUser(paramters.UserId).UserName;
                                    }
                                }
                                if (qtCoverUpForOrg.TempLateCode == "001")
                                {
                                    return this.ProjectMstService.ExportSummaryExcelSZ1(projectMsts, qtCoverUpForOrg, organizeModel, userModel);
                                }
                                else
                                {
                                    return this.ProjectMstService.ExportSummaryExcel1(projectMsts, qtCoverUpForOrg, organizeModel, userModel);
                                }
                            }
                            else
                            {
                                return DCHelper.ErrorMessage("项目集合查询失败！");
                            }
                        }
                        else
                        {
                            return DCHelper.ErrorMessage("该组织还未分配汇总表套打格式的打印导出模板，请联系系统管理员！");
                        }
                    }
                    else if (paramters.execlType.Equals("02"))//预立项申请表
                    {
                        qtCoverUpForOrg = coversForOrg.ToList().Find(t => t.TypeNumber == 2 && t.ProcessCode == "001");
                        if (qtCoverUpForOrg != null)
                        {
                            var syssets = QTSysSetService.Find(t => t.PhId != 0).Data.ToList();
                            IList<ProjectMstModel> projectMsts = new List<ProjectMstModel>();
                            projectMsts = this.ProjectMstService.Find(t => phids.Contains(t.PhId)).Data.OrderByDescending(t => t.NgInsertDt).ThenByDescending(t => t.NgUpdateDt).ToList();

                            //IList<PerformEvalTargetTypeModel> performEvalTargetTypes = new List<PerformEvalTargetTypeModel>();
                            //performEvalTargetTypes = this.PerformEvalTargetTypeService.Find(t => t.Orgid == paramters.OrgId).Data;
                            if (projectMsts != null && projectMsts.Count > 0)
                            {
                                foreach (var pro in projectMsts)
                                {
                                    RichHelpDac helpdac = new RichHelpDac();
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FApprover", "FApprover_EXName", "fg3_user");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FPerformType", "FPerformType_EXName", "GHPerformEvalTargetTypeTree");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FPerformEvalType", "FPerformEvalType_EXName", "GHPerformEvalType");
                                    if (string.IsNullOrEmpty(pro.FDeclarationDept))
                                    {
                                        //申报部门代码转名称
                                        pro.FDeclarationDept_EXName = organizeModel.OName;
                                    }
                                    var syssetProjectMst = syssets.FindAll(x => x.DicType == "ProjectProper" && x.Orgcode == pro.FDeclarationUnit && x.TypeCode == pro.FProjAttr);
                                    if (syssetProjectMst != null && syssetProjectMst.Count > 0)
                                    {
                                        //项目属性代码转名称
                                        pro.FProjAttr_EXName = syssetProjectMst[0].TypeName;
                                    }
                                    var syssetProjectMst2 = syssets.FindAll(x => x.DicType == "TimeLimit" && x.Orgcode == pro.FDeclarationUnit && x.TypeCode == pro.FDuration);
                                    if (syssetProjectMst2 != null && syssetProjectMst2.Count > 0)
                                    {
                                        //存续期限代码转名称
                                        pro.FDuration_EXName = syssetProjectMst2[0].TypeName;
                                    }
                                    var syssetProjectMst3 = syssets.FindAll(x => x.DicType == "ProjectLevel" && x.Orgcode == pro.FDeclarationUnit && x.TypeCode == pro.FLevel);
                                    if (syssetProjectMst3 != null && syssetProjectMst3.Count > 0)
                                    {
                                        //项目级别代码转名称
                                        pro.FLevel_EXName = syssetProjectMst3[0].TypeName;
                                    }
                                    //if (performEvalTargetTypes != null && performEvalTargetTypes.Count > 0 && !string.IsNullOrEmpty(pro.FPerformType))
                                    //{
                                    //    //绩效评价名称
                                    //    pro.FPerformType_EXName = performEvalTargetTypes.ToList().Find(t => t.FCode == pro.FPerformType) == null ? "" : performEvalTargetTypes.ToList().Find(t => t.FCode == pro.FPerformType).FName;
                                    //}
                                    if (pro.FApprover != 0)
                                    {
                                        //审批人名称
                                        pro.FApprover_EXName = this.BudgetMstService.GetUser(paramters.UserId).UserName;
                                    }
                                }

                                return this.ProjectMstService.ExportDeclareExcel1(projectMsts, qtCoverUpForOrg, organizeModel, userModel);
                            }
                            else
                            {
                                return DCHelper.ErrorMessage("项目集合查询失败！");
                            }
                        }
                        else
                        {
                            return DCHelper.ErrorMessage("该组织还未分配申请表套打格式的打印导出模板，请联系系统管理员！");
                        }
                    }
                    else if (paramters.execlType.Equals("03"))//立项汇总表
                    {
                        qtCoverUpForOrg = coversForOrg.ToList().Find(t => t.TypeNumber == 1 && t.ProcessCode == "002");
                        if (qtCoverUpForOrg != null)
                        {
                            var syssets = QTSysSetService.Find(t => t.PhId != 0).Data.ToList();
                            IList<ProjectMstModel> projectMsts = new List<ProjectMstModel>();
                            projectMsts = this.ProjectMstService.Find(t => phids.Contains(t.PhId)).Data.OrderByDescending(t => t.NgInsertDt).ThenByDescending(t => t.NgUpdateDt).ToList();

                            //IList<PerformEvalTargetTypeModel> performEvalTargetTypes = new List<PerformEvalTargetTypeModel>();
                            //performEvalTargetTypes = this.PerformEvalTargetTypeService.Find(t => t.Orgid == paramters.OrgId).Data;
                            if (projectMsts != null && projectMsts.Count > 0)
                            {
                                foreach (var pro in projectMsts)
                                {
                                    RichHelpDac helpdac = new RichHelpDac();
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FApprover", "FApprover_EXName", "fg3_user");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FPerformType", "FPerformType_EXName", "GHPerformEvalTargetTypeTree");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FPerformEvalType", "FPerformEvalType_EXName", "GHPerformEvalType");
                                    if (string.IsNullOrEmpty(pro.FDeclarationDept))
                                    {
                                        //申报部门代码转名称
                                        pro.FDeclarationDept_EXName = organizeModel.OName;
                                    }
                                    var syssetProjectMst = syssets.FindAll(x => x.DicType == "ProjectProper" && x.Orgcode == pro.FDeclarationUnit && x.TypeCode == pro.FProjAttr);
                                    if (syssetProjectMst != null && syssetProjectMst.Count > 0)
                                    {
                                        //项目属性代码转名称
                                        pro.FProjAttr_EXName = syssetProjectMst[0].TypeName;
                                    }
                                    var syssetProjectMst2 = syssets.FindAll(x => x.DicType == "TimeLimit" && x.Orgcode == pro.FDeclarationUnit && x.TypeCode == pro.FDuration);
                                    if (syssetProjectMst2 != null && syssetProjectMst2.Count > 0)
                                    {
                                        //存续期限代码转名称
                                        pro.FDuration_EXName = syssetProjectMst2[0].TypeName;
                                    }
                                    var syssetProjectMst3 = syssets.FindAll(x => x.DicType == "ProjectLevel" && x.Orgcode == pro.FDeclarationUnit && x.TypeCode == pro.FLevel);
                                    if (syssetProjectMst3 != null && syssetProjectMst3.Count > 0)
                                    {
                                        //项目级别代码转名称
                                        pro.FLevel_EXName = syssetProjectMst3[0].TypeName;
                                    }
                                    //if(performEvalTargetTypes != null && performEvalTargetTypes.Count > 0 && !string.IsNullOrEmpty(pro.FPerformType))
                                    //{
                                    //绩效评价名称
                                    //    pro.FPerformType_EXName = performEvalTargetTypes.ToList().Find(t => t.FCode == pro.FPerformType) == null ? "": performEvalTargetTypes.ToList().Find(t => t.FCode == pro.FPerformType).FName; 
                                    //}
                                    if (pro.FApprover != 0)
                                    {
                                        //审批人姓名
                                        pro.FApprover_EXName = this.BudgetMstService.GetUser(paramters.UserId).UserName;
                                    }
                                }
                                if (qtCoverUpForOrg.TempLateCode == "001")
                                {
                                    return this.ProjectMstService.ExportSummaryExcelSZ2(projectMsts, qtCoverUpForOrg, organizeModel, userModel);
                                }
                                else
                                {
                                    return this.ProjectMstService.ExportSummaryExcel2(projectMsts, qtCoverUpForOrg, organizeModel, userModel);
                                }
                            }
                            else
                            {
                                return DCHelper.ErrorMessage("项目集合查询失败！");
                            }
                        }
                        else
                        {
                            return DCHelper.ErrorMessage("该组织还未分配汇总表套打格式的打印导出模板，请联系系统管理员！");
                        }
                    }
                    else if (paramters.execlType.Equals("04"))//立项申请表
                    {
                        qtCoverUpForOrg = coversForOrg.ToList().Find(t => t.TypeNumber == 1 && t.ProcessCode == "002");
                        if (qtCoverUpForOrg != null)
                        {
                            var syssets = QTSysSetService.Find(t => t.PhId != 0).Data.ToList();
                            IList<ProjectMstModel> projectMsts = new List<ProjectMstModel>();
                            projectMsts = this.ProjectMstService.Find(t => phids.Contains(t.PhId)).Data.OrderByDescending(t => t.NgInsertDt).ThenByDescending(t => t.NgUpdateDt).ToList();

                            //IList<PerformEvalTargetTypeModel> performEvalTargetTypes = new List<PerformEvalTargetTypeModel>();
                            //performEvalTargetTypes = this.PerformEvalTargetTypeService.Find(t => t.Orgid == paramters.OrgId).Data;
                            if (projectMsts != null && projectMsts.Count > 0)
                            {
                                foreach (var pro in projectMsts)
                                {
                                    RichHelpDac helpdac = new RichHelpDac();
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FApprover", "FApprover_EXName", "fg3_user");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FPerformType", "FPerformType_EXName", "GHPerformEvalTargetTypeTree");
                                    helpdac.CodeToName<ProjectMstModel>(pro, "FPerformEvalType", "FPerformEvalType_EXName", "GHPerformEvalType");
                                    if (string.IsNullOrEmpty(pro.FDeclarationDept))
                                    {
                                        //申报部门代码转名称
                                        pro.FDeclarationDept_EXName = organizeModel.OName;
                                    }
                                    var syssetProjectMst = syssets.FindAll(x => x.DicType == "ProjectProper" && x.Orgcode == pro.FDeclarationUnit && x.TypeCode == pro.FProjAttr);
                                    if (syssetProjectMst != null && syssetProjectMst.Count > 0)
                                    {
                                        //项目属性代码转名称
                                        pro.FProjAttr_EXName = syssetProjectMst[0].TypeName;
                                    }
                                    var syssetProjectMst2 = syssets.FindAll(x => x.DicType == "TimeLimit" && x.Orgcode == pro.FDeclarationUnit && x.TypeCode == pro.FDuration);
                                    if (syssetProjectMst2 != null && syssetProjectMst2.Count > 0)
                                    {
                                        //存续期限代码转名称
                                        pro.FDuration_EXName = syssetProjectMst2[0].TypeName;
                                    }
                                    var syssetProjectMst3 = syssets.FindAll(x => x.DicType == "ProjectLevel" && x.Orgcode == pro.FDeclarationUnit && x.TypeCode == pro.FLevel);
                                    if (syssetProjectMst3 != null && syssetProjectMst3.Count > 0)
                                    {
                                        //项目级别代码转名称
                                        pro.FLevel_EXName = syssetProjectMst3[0].TypeName;
                                    }
                                    //if(performEvalTargetTypes != null && performEvalTargetTypes.Count > 0 && !string.IsNullOrEmpty(pro.FPerformType))
                                    //{
                                    //绩效评价名称
                                    //    pro.FPerformType_EXName = performEvalTargetTypes.ToList().Find(t => t.FCode == pro.FPerformType) == null ? "": performEvalTargetTypes.ToList().Find(t => t.FCode == pro.FPerformType).FName; 
                                    //}
                                    if (pro.FApprover != 0)
                                    {
                                        //审批人姓名
                                        pro.FApprover_EXName = this.BudgetMstService.GetUser(paramters.UserId).UserName;
                                    }
                                }
                                return this.ProjectMstService.ExportDeclareExcel2(projectMsts, qtCoverUpForOrg, organizeModel, userModel);
                            }
                            else
                            {
                                return DCHelper.ErrorMessage("项目集合查询失败！");
                            }
                        }
                        else
                        {
                            return DCHelper.ErrorMessage("该组织还未分配汇总表套打格式的打印导出模板，请联系系统管理员！");
                        }
                    }
                    else if (paramters.execlType.Equals("05"))
                    {
                        //年中相关汇总表(包括年中新增与年中调整)
                        qtCoverUpForOrg = coversForOrg.ToList().Find(t => t.TypeNumber == 1 && (t.ProcessCode == "003" || t.ProcessCode == "004"));
                        if (qtCoverUpForOrg != null)
                        {
                            var syssets = QTSysSetService.Find(t => t.PhId != 0).Data.ToList();
                            IList<BudgetMstModel> projectMsts = new List<BudgetMstModel>();
                            projectMsts = this.BudgetMstService.Find(t => phids.Contains(t.PhId)).Data.OrderByDescending(t => t.NgInsertDt).ThenByDescending(t => t.NgUpdateDt).ToList();

                            //IList<PerformEvalTargetTypeModel> performEvalTargetTypes = new List<PerformEvalTargetTypeModel>();
                            //performEvalTargetTypes = this.PerformEvalTargetTypeService.Find(t => t.Orgid == paramters.OrgId).Data;
                            if (projectMsts != null && projectMsts.Count > 0)
                            {
                                foreach (var pro in projectMsts)
                                {
                                    RichHelpDac helpdac = new RichHelpDac();
                                    helpdac.CodeToName<BudgetMstModel>(pro, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist");
                                    helpdac.CodeToName<BudgetMstModel>(pro, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode");
                                    helpdac.CodeToName<BudgetMstModel>(pro, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist");
                                    helpdac.CodeToName<BudgetMstModel>(pro, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory");
                                    helpdac.CodeToName<BudgetMstModel>(pro, "FApprover", "FApprover_EXName", "fg3_user");
                                    helpdac.CodeToName<BudgetMstModel>(pro, "FPerformType", "FPerformType_EXName", "GHPerformEvalTargetTypeTree");
                                    helpdac.CodeToName<BudgetMstModel>(pro, "FPerformEvalType", "FPerformEvalType_EXName", "GHPerformEvalType");

                                    if (string.IsNullOrEmpty(pro.FDeclarationDept))
                                    {
                                        //申报部门代码转名称
                                        pro.FDeclarationDept_EXName = organizeModel.OName;
                                    }
                                    var syssetProjectMst = syssets.FindAll(x => x.DicType == "ProjectProper" && x.Orgcode == pro.FDeclarationUnit && x.TypeCode == pro.FProjAttr);
                                    if (syssetProjectMst != null && syssetProjectMst.Count > 0)
                                    {
                                        //项目属性代码转名称
                                        pro.FProjAttr_EXName = syssetProjectMst[0].TypeName;
                                    }
                                    var syssetProjectMst2 = syssets.FindAll(x => x.DicType == "TimeLimit" && x.Orgcode == pro.FDeclarationUnit && x.TypeCode == pro.FDuration);
                                    if (syssetProjectMst2 != null && syssetProjectMst2.Count > 0)
                                    {
                                        //存续期限代码转名称
                                        pro.FDuration_EXName = syssetProjectMst2[0].TypeName;
                                    }
                                    var syssetProjectMst3 = syssets.FindAll(x => x.DicType == "ProjectLevel" && x.Orgcode == pro.FDeclarationUnit && x.TypeCode == pro.FLevel);
                                    if (syssetProjectMst3 != null && syssetProjectMst3.Count > 0)
                                    {
                                        //项目级别代码转名称
                                        pro.FLevel_EXName = syssetProjectMst3[0].TypeName;
                                    }
                                    //if(performEvalTargetTypes != null && performEvalTargetTypes.Count > 0 && !string.IsNullOrEmpty(pro.FPerformType))
                                    //{
                                    //绩效评价名称
                                    //    pro.FPerformType_EXName = performEvalTargetTypes.ToList().Find(t => t.FCode == pro.FPerformType) == null ? "": performEvalTargetTypes.ToList().Find(t => t.FCode == pro.FPerformType).FName; 
                                    //}
                                    if (pro.FApprover != 0)
                                    {
                                        //审批人姓名
                                        pro.FApprover_EXName = this.BudgetMstService.GetUser(paramters.UserId).UserName;
                                    }
                                }
                                return this.ProjectMstService.ExportSummaryExcelTZ(projectMsts, qtCoverUpForOrg, organizeModel, userModel);
                            }
                            else
                            {
                                return DCHelper.ErrorMessage("项目集合查询失败！");
                            }
                        }
                        else
                        {
                            return DCHelper.ErrorMessage("该组织还未分配汇总表套打格式的打印导出模板，请联系系统管理员！");
                        }
                    }
                    else if (paramters.execlType.Equals("06"))
                    {
                        //IList<BudgetMstModel> budgetMsts = new List<BudgetMstModel>();
                        //budgetMsts = this.BudgetMstService.Find(t => phids.Contains(t.PhId)).Data;
                        //if (budgetMsts != null && budgetMsts.Count > 0)
                        //{
                        //    var xBudgets = budgetMsts.ToList().FindAll(t => t.FType == "z" && t.FVerNo == "0001");
                        //    if (xBudgets != null && xBudgets.Count > 0)
                        //    {

                        //    }
                        //    var tBudgets = budgetMsts.ToList().FindAll(t => (t.FType != "z" || t.FVerNo != "0001"));
                        //}

                        //年中相关申请表
                        qtCoverUpForOrg = coversForOrg.ToList().Find(t => t.TypeNumber == 2 && (t.ProcessCode == "003" || t.ProcessCode == "004"));
                        if (qtCoverUpForOrg != null)
                        {
                            var syssets = QTSysSetService.Find(t => t.PhId != 0).Data.ToList();
                            IList<BudgetMstModel> projectMsts = new List<BudgetMstModel>();
                            projectMsts = this.BudgetMstService.Find(t => phids.Contains(t.PhId)).Data.OrderBy(t => t.NgInsertDt).ThenBy(t => t.NgUpdateDt).ToList();
                            //IList<PerformEvalTargetTypeModel> performEvalTargetTypes = new List<PerformEvalTargetTypeModel>();
                            //performEvalTargetTypes = this.PerformEvalTargetTypeService.Find(t => t.Orgid == paramters.OrgId).Data;
                            if (projectMsts != null && projectMsts.Count > 0)
                            {
                                foreach (var pro in projectMsts)
                                {
                                    RichHelpDac helpdac = new RichHelpDac();
                                    helpdac.CodeToName<BudgetMstModel>(pro, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist");
                                    helpdac.CodeToName<BudgetMstModel>(pro, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode");
                                    helpdac.CodeToName<BudgetMstModel>(pro, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist");
                                    helpdac.CodeToName<BudgetMstModel>(pro, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory");
                                    helpdac.CodeToName<BudgetMstModel>(pro, "FApprover", "FApprover_EXName", "fg3_user");
                                    helpdac.CodeToName<BudgetMstModel>(pro, "FPerformType", "FPerformType_EXName", "GHPerformEvalTargetTypeTree");
                                    helpdac.CodeToName<BudgetMstModel>(pro, "FPerformEvalType", "FPerformEvalType_EXName", "GHPerformEvalType");
                                    if (string.IsNullOrEmpty(pro.FDeclarationDept))
                                    {
                                        //申报部门代码转名称
                                        pro.FDeclarationDept_EXName = organizeModel.OName;
                                    }
                                    var syssetProjectMst = syssets.FindAll(x => x.DicType == "ProjectProper" && x.Orgcode == pro.FDeclarationUnit && x.TypeCode == pro.FProjAttr);
                                    if (syssetProjectMst != null && syssetProjectMst.Count > 0)
                                    {
                                        //项目属性代码转名称
                                        pro.FProjAttr_EXName = syssetProjectMst[0].TypeName;
                                    }
                                    var syssetProjectMst2 = syssets.FindAll(x => x.DicType == "TimeLimit" && x.Orgcode == pro.FDeclarationUnit && x.TypeCode == pro.FDuration);
                                    if (syssetProjectMst2 != null && syssetProjectMst2.Count > 0)
                                    {
                                        //存续期限代码转名称
                                        pro.FDuration_EXName = syssetProjectMst2[0].TypeName;
                                    }
                                    var syssetProjectMst3 = syssets.FindAll(x => x.DicType == "ProjectLevel" && x.Orgcode == pro.FDeclarationUnit && x.TypeCode == pro.FLevel);
                                    if (syssetProjectMst3 != null && syssetProjectMst3.Count > 0)
                                    {
                                        //项目级别代码转名称
                                        pro.FLevel_EXName = syssetProjectMst3[0].TypeName;
                                    }
                                    //if (performEvalTargetTypes != null && performEvalTargetTypes.Count > 0 && !string.IsNullOrEmpty(pro.FPerformType))
                                    //{
                                    //    //绩效评价名称
                                    //    pro.FPerformType_EXName = performEvalTargetTypes.ToList().Find(t => t.FCode == pro.FPerformType) == null ? "" : performEvalTargetTypes.ToList().Find(t => t.FCode == pro.FPerformType).FName;
                                    //}
                                    if (pro.FApprover != 0)
                                    {
                                        //审批人名称
                                        pro.FApprover_EXName = this.BudgetMstService.GetUser(paramters.UserId).UserName;
                                    }
                                }

                                return this.ProjectMstService.ExportDeclareExcelTZ2(projectMsts, qtCoverUpForOrg, organizeModel, userModel);
                            }
                            else
                            {
                                return DCHelper.ErrorMessage("项目集合查询失败！");
                            }
                        }
                        else
                        {
                            return DCHelper.ErrorMessage("该组织还未分配申请表套打格式的打印导出模板，请联系系统管理员！");
                        }
                    }
                    return result;
                }
                else
                {
                    return DCHelper.ErrorMessage("该组织还未分配任何套打格式的打印导出模板，请联系系统管理员！");
                }
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 项目看板根据code 获取项目看板数据
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Type"></param>
        /// <param name="Year"></param>
        /// <param name="Ucode"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetcollectList([FromUri] string Code, int Type, string Year, string Ucode)
        {
            var workType = "z";
            if (!string.IsNullOrEmpty(Code) && Type != 0 && !string.IsNullOrEmpty(Year) && !string.IsNullOrEmpty(Ucode))
            {
                var dicWhereDept = new Dictionary<string, object>();
                new CreateCriteria(dicWhereDept)
                    .Add(ORMRestrictions<string>.Eq("Dwdm", Ucode)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
                var deptList = CorrespondenceSettingsService.Find(dicWhereDept);
                List<string> deptL = new List<string>();
                for (var i = 0; i < deptList.Data.Count; i++)
                {
                    deptL.Add(deptList.Data[i].Dydm);
                }
                var QueryOrg = BudgetMstService.GetOrganizeByList(Code).ToList();
                var orgCodes = QueryOrg.Select(p => p.OCode).ToList();
                if (Type == 1)
                {
                    var dic = new Dictionary<string, object>();
                    var diction = new Dictionary<string, object>();
                    List<string> list = new List<string>() { "5", "10", "11" };
                    //根据Code 查询所有下级组织  ??这个方法应不应该是 like'101%'
                    //获取查询所有下级组织的项目
                    new CreateCriteria(dic).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                    new CreateCriteria(dic).Add(ORMRestrictions<string>.Eq("FYear", Year));//审批通过状态 
                    new CreateCriteria(dic).Add(ORMRestrictions<string>.Eq("FApproveStatus", "3"));
                    new CreateCriteria(dic).Add(ORMRestrictions<List<int>>.NotIn("FProjStatus", new List<int>() { 1, 8 }));//项目执行状态
                    new CreateCriteria(dic).Add(ORMRestrictions<List<string>>.In("FDeclarationUnit", orgCodes));
                    new CreateCriteria(dic).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
                    var QueryXm3 = ProjectMstService.Find(dic).Data.ToList();//年初表的基本信息
                    var phIds = QueryXm3.Select(p => p.PhId).ToList();//Phid集合
                    new CreateCriteria(diction).Add(ORMRestrictions<List<Int64>>.In("MstPhid", phIds));
                    var Querybudgetdtl = ProjectMstService.FindProjectDtlBudgetDtl(diction);
                    var AmountCount = Querybudgetdtl.Sum(p => p.FAmount) / 10000;//总金额
                                                                                 //所有组织项目总数
                    var maxCount = QueryXm3.Count;
                    var data = QueryXm3.GroupBy(p => p.FDeclarationUnit).Select(p => new
                    {
                        Oname = QueryOrg.Where(p2 => p2.OCode == p.Key).First()?.OName ?? string.Empty,//单位名
                        Count = p.Count(),  //单位项目数
                        Per = Math.Round(p.Count() / (float)maxCount * 100, 2), //百分比
                        Amount = Math.Round(Querybudgetdtl.Where(p2 => p.Select(p3 => p3.PhId).ToList().Exists(p3 => p3 == p2.MstPhid)).Sum(p2 => p2.FAmount) / 10000, 2).ToString(),//单位金额
                                                                                                                                                                                     // AmountCount = Math.Round(AmountCount, 2),//总金额
                        Amountpercentage = Math.Round((Querybudgetdtl.Where(p2 => p.Select(p3 => p3.PhId).ToList().Exists(p3 => p3 == p2.MstPhid)).Sum(p2 => p2.FAmount) / 10000) / (AmountCount * 100), 2)//金额占比
                    });
                    var result = new
                    {
                        data,
                        AmountCount = Math.Round(AmountCount, 2)
                    };


                    return DataConverterHelper.SerializeObject(result);

                }
                else if (Type == 2)
                {
                    var dic = new Dictionary<string, object>();
                    var diction = new Dictionary<string, object>();
                    if (workType == "z") //年中
                    {
                        //年中时可以调整年初预算单据
                        var dicWhere1 = new Dictionary<string, object>(); //年中调整新增
                        var dicWhere2 = new Dictionary<string, object>(); //年初预算新增后调整
                        var dicWhere3 = new Dictionary<string, object>(); //年初新增
                        var dicWhere4 = new Dictionary<string, object>(); //年中新增
                        new CreateCriteria(dicWhere1).Add(ORMRestrictions<string>.Eq("FType", "z"))
                            .Add(ORMRestrictions<string>.Eq("FVerNo", "0002"))
                            .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));
                        new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FType", "c"))
                            .Add(ORMRestrictions<string>.Eq("FVerNo", "0002"))
                            .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0")); //年初没调整过的单据
                        new CreateCriteria(dicWhere3).Add(ORMRestrictions<string>.Eq("FType", "c"))
                            .Add(ORMRestrictions<string>.Eq("FVerNo", "0001"))
                            .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));
                        new CreateCriteria(dicWhere4).Add(ORMRestrictions<string>.Eq("FType", "z"))
                            .Add(ORMRestrictions<string>.Eq("FVerNo", "0001"))
                            .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));
                        new CreateCriteria(dic).Add(ORMRestrictions.Or(dicWhere1, dicWhere2, dicWhere3, dicWhere4));
                        //new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FType", "z"));


                    }
                    List<string> list = new List<string>() { "5", "10", "11" };
                    new CreateCriteria(dic).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                    new CreateCriteria(dic).Add(ORMRestrictions<string>.Eq("FYear", Year));
                    new CreateCriteria(dic).Add(ORMRestrictions<string>.Eq("FApproveStatus", "3"));
                    new CreateCriteria(dic).Add(ORMRestrictions<List<string>>.In("FProjStatus", list));
                    new CreateCriteria(dic).Add(ORMRestrictions<List<string>>.In("FDeclarationUnit", orgCodes));
                    new CreateCriteria(dic).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
                    new CreateCriteria(dic).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0));
                    var QueryYs3 = BudgetMstService.Find(dic).Data.ToList();//年初表的基本信息
                    var phIds = QueryYs3.Select(p => p.PhId).ToList();//Phid集合
                    new CreateCriteria(diction).Add(ORMRestrictions<List<Int64>>.In("MstPhid", phIds));
                    var Querybudgetdtl = BudgetMstService.FindProjectDtlBudgetDtl(diction);
                    var AmountCount = Querybudgetdtl.Sum(p => p.FAmount) / 10000;//总金额
                                                                                 //所有组织项目总数
                    var maxCount = QueryYs3.Count;
                    var data = QueryYs3.GroupBy(p => p.FDeclarationUnit).Select(p => new
                    {
                        Oname = QueryOrg.Where(p2 => p2.OCode == p.Key).First()?.OName ?? string.Empty,//单位名
                        Count = p.Count(),  //单位项目数
                        Per = Math.Round(p.Count() / (float)maxCount * 100, 2), //百分比
                        Amount = Math.Round(Querybudgetdtl.Where(p2 => p.Select(p3 => p3.PhId).ToList().Exists(p3 => p3 == p2.MstPhid)).Sum(p2 => p2.FAmount) / 10000, 2).ToString(),//单位金额
                                                                                                                                                                                     //  AmountCount = Math.Round(AmountCount, 2),//总金额
                        Amountpercentage = Math.Round((Querybudgetdtl.Where(p2 => p.Select(p3 => p3.PhId).ToList().Exists(p3 => p3 == p2.MstPhid)).Sum(p2 => p2.FAmount) / 10000) / AmountCount * 100, 2)//金额占比
                    });
                    var result = new
                    {
                        data,
                        AmountCount = Math.Round(AmountCount, 2)
                    };
                    return DataConverterHelper.SerializeObject(result);
                }
                else
                {
                    return DCHelper.ErrorMessage("参数类型传递失败");
                }
            }
            else
            {
                return DCHelper.ErrorMessage("参数传递失败");
            }
        }

        /// <summary>
        /// 不启用审批流情况下 对用户进行消息提醒需求
        /// </summary>
        /// <param name="Ucode"></param>
        /// <param name="Year"></param>
        /// <param name="OrgCode"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetApprovalProcess([FromUri] string Ucode, string Year, string OrgCode)
        {
            //Ucode = "9999";
            //Year = "2019";
            //OrgCode = "101";
            if (!string.IsNullOrEmpty(Ucode) && !string.IsNullOrEmpty(Year) && !string.IsNullOrEmpty(OrgCode))
            {
                #region 定义参数规范

                var dicWhereDept = new Dictionary<string, object>();
                new CreateCriteria(dicWhereDept)
                    .Add(ORMRestrictions<string>.Eq("Dwdm", Ucode)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
                var deptList = CorrespondenceSettingsService.Find(dicWhereDept);
                List<string> deptL = new List<string>();
                for (var i = 0; i < deptList.Data.Count; i++)
                {
                    deptL.Add(deptList.Data[i].Dydm);
                }
                #endregion

                #region 预立项项目
                //年份与单位编码筛选预立项
                ProjectMstRequest projectMst1 = new ProjectMstRequest();
                projectMst1.Ucode = Ucode;
                projectMst1.OrgCode = OrgCode;
                projectMst1.Year = Year;
                projectMst1.fapprovearea = new List<string>() { "1" };
                projectMst1.projectarea = new List<int>() { 1 };
                var Result1 = JsonConvert.DeserializeObject<ProjectMstListModel>(GetProjectMstList2(projectMst1)).totalRows;
                ProjectMstRequest projectMst2 = new ProjectMstRequest();
                projectMst2.Ucode = Ucode;
                projectMst2.OrgCode = OrgCode;
                projectMst2.Year = Year;
                projectMst2.fapprovearea = new List<string>() { "2" };
                projectMst2.projectarea = new List<int>() { 1 };
                var Result2 = JsonConvert.DeserializeObject<ProjectMstListModel>(GetProjectMstList2(projectMst2)).totalRows;
                #endregion

                #region 立项项目
                //项目类型条件筛选立项
                ProjectMstRequest projectMst = new ProjectMstRequest();
                projectMst.OrgCode = OrgCode;
                projectMst.Year = Year;
                projectMst.Ucode = Ucode;
                projectMst.fapprovearea = new List<string>() { "1", "2" };
                projectMst.projectarea = new List<int>() { 2, 3 };
                var Result3 = 0;
                if (string.IsNullOrEmpty(projectMst.OrgCode))
                {
                    return DCHelper.ErrorMessage("单位编码不能为空！");
                }
                if (string.IsNullOrEmpty(projectMst.Ucode))
                {
                    return DCHelper.ErrorMessage("用户编码不能为空！");
                }
                if (string.IsNullOrEmpty(projectMst.Year))
                {
                    return DCHelper.ErrorMessage("年度不能为空！");
                }
                try
                {
                    //年份与单位编码筛选
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    new CreateCriteria(dic)
                            .Add(ORMRestrictions<string>.Eq("FYear", projectMst.Year))
                            .Add(ORMRestrictions<string>.Eq("FDeclarationUnit", projectMst.OrgCode));
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
                    if (projectMst.projectarea != null)
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<List<Int32>>.In("FProjStatus", projectMst.projectarea));
                    }
                    if (projectMst.fapprovearea != null)
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<List<string>>.In("FApproveStatus", projectMst.fapprovearea));
                    }
                    //增加搜索条件
                    if (!string.IsNullOrEmpty(projectMst.SearchValue))
                    {
                        Dictionary<string, object> dicName = new Dictionary<string, object>();
                        Dictionary<string, object> dicCode = new Dictionary<string, object>();
                        new CreateCriteria(dicName)
                                .Add(ORMRestrictions<string>.Like("FProjName", projectMst.SearchValue));
                        new CreateCriteria(dicCode)
                                .Add(ORMRestrictions<string>.Like("FProjCode", projectMst.SearchValue));
                        new CreateCriteria(dic).Add(ORMRestrictions.Or(dicName, dicCode));
                    }
                    if (!string.IsNullOrEmpty(projectMst.FExpenseCategory) && !"0".Equals(projectMst.FExpenseCategory))
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<string>.Eq("FExpenseCategory", projectMst.FExpenseCategory));
                    }
                    if (!string.IsNullOrEmpty(projectMst.FBudgetDept))//预算部门
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<string>.Eq("FBudgetDept", projectMst.FBudgetDept));
                    }
                    if (!string.IsNullOrEmpty(projectMst.FDeclarationDept))//申报部门
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<string>.Eq("FDeclarationDept", projectMst.FDeclarationDept));
                    }
                    if (!string.IsNullOrEmpty(projectMst.FProjCode))//项目编码
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<string>.Eq("FProjCode", projectMst.FProjCode));
                    }
                    if (!string.IsNullOrEmpty(projectMst.FProjName))//项目名称
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<string>.Eq("FProjName", projectMst.FProjName));
                    }
                    if (!string.IsNullOrEmpty(projectMst.FProjAmountBegin))
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<Decimal>.Ge("FProjAmount", Decimal.Parse(projectMst.FProjAmountBegin)));
                    }
                    if (!string.IsNullOrEmpty(projectMst.FProjAmountEnd))
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<Decimal>.Le("FProjAmount", Decimal.Parse(projectMst.FProjAmountEnd)));
                    }
                    if (projectMst.FStartDate != null)
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<System.DateTime?>.Ge("FDateofDeclaration", projectMst.FStartDate));
                    }
                    if (projectMst.FEndDate != null)
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<System.DateTime?>.Le("FDateofDeclaration", projectMst.FEndDate));
                    }
                    if (projectMst.FIfPerformanceAppraisal != 0 && !string.IsNullOrEmpty(projectMst.FIfPerformanceAppraisal.ToString()))
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<EnumYesNo>.Eq("FIfPerformanceAppraisal", projectMst.FIfPerformanceAppraisal));
                    }
                    var results = this.ProjectMstService.LoadWithPage(projectMst.PageIndex, projectMst.PageSize, dic, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" });

                    //提高接口效率
                    var Query = QtAttachmentService.Find(t => t.BTable == "XM3_PROJECTMST").Data;
                    foreach (var item in results.Results)
                    {
                        if (Query != null && Query.Count > 0)
                        {
                            var uploadlist = Query.ToList().FindAll(t => t.RelPhid == item.PhId);
                            item.list = uploadlist;
                            item.UploadFileCount = uploadlist.Count;
                        }
                    }
                    var dicSyssets = new Dictionary<string, object>();
                    new CreateCriteria(dicSyssets).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                    var syssetss = QTSysSetService.Find(dicSyssets).Data.ToList();
                    foreach (var data in results.Results)
                    {
                        var syssetProjectMst = syssetss.FindAll(x => x.DicType == "ProjectProper" && x.Orgcode == data.FDeclarationUnit && x.TypeCode == data.FProjAttr);
                        if (syssetProjectMst.Count > 0)
                        {
                            //项目属性代码转名称
                            data.FProjAttr_EXName = syssetProjectMst[0].TypeName;
                        }
                        var syssetProjectMst2 = syssetss.FindAll(x => x.DicType == "TimeLimit" && x.Orgcode == data.FDeclarationUnit && x.TypeCode == data.FDuration);
                        if (syssetProjectMst2.Count > 0)
                        {
                            //存续期限代码转名称
                            data.FDuration_EXName = syssetProjectMst2[0].TypeName;
                        }

                        var syssetProjectMst3 = syssetss.FindAll(x => x.DicType == "ProjectLevel" && x.Orgcode == data.FDeclarationUnit && x.TypeCode == data.FLevel);
                        if (syssetProjectMst3.Count > 0)
                        {
                            //项目级别代码转名称
                            data.FLevel_EXName = syssetProjectMst3[0].TypeName;
                        }

                        var dtls = ProjectMstService.FindProjectDtlBudgetDtlByForeignKey(data.PhId).Data;
                        var FIfYsxz = 0;
                        if (dtls.Count > 0)
                        {
                            foreach (var dtl in dtls)
                            {
                                if (string.IsNullOrEmpty(dtl.FBudgetAccounts) || string.IsNullOrEmpty(dtl.FExpensesChannel))
                                {
                                    FIfYsxz++;
                                }
                            }
                        }
                        data.FIfYsxz = FIfYsxz;
                    }
                    Result3 = Convert.ToInt32(results.TotalItems);
                }
                catch (Exception ex)
                {
                    return DCHelper.ErrorMessage(ex.Message);
                }
                #endregion

                #region 年终调整
                //年份与单位编码筛选年终调整
                //Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                //new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FDeclarationUnit", OrgCode));
                //new CreateCriteria(dicWhere).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
                //new CreateCriteria(dicWhere).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0));
                //new CreateCriteria(dicWhere).Add(ORMRestrictions<List<Int32>>.NotIn("FProjStatus", new List<Int32> { 10, 11 }))
                //   .Add(ORMRestrictions<List<string>>.In("FApproveStatus", new List<string> { "1" }));
                //var niz = BudgetMstService.Find(dicWhere);
                var Result4 = 0;
                BudgetMstListsRequestModel param = new BudgetMstListsRequestModel();
                param.Ucode = Ucode;
                param.OrgCode = OrgCode;
                param.Year = Year;
                param.projectarea = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                if (string.IsNullOrEmpty(param.Ucode))
                {
                    return DCHelper.ErrorMessage("用户编码不能为空！");
                }
                if (string.IsNullOrEmpty(param.OrgCode))
                {
                    return DCHelper.ErrorMessage("组织编码不能为空！");
                }
                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<string>.Eq("FDeclarationUnit", param.OrgCode));
                new CreateCriteria(dicWhere).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
                new CreateCriteria(dicWhere).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0));
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", Year));
                if (param.projectarea != null)
                {
                    new CreateCriteria(dicWhere)
                         .Add(ORMRestrictions<List<Int32>>.In("FProjStatus", param.projectarea));
                }
                if (param.fapprovearea != null)
                {
                    new CreateCriteria(dicWhere)
                          .Add(ORMRestrictions<List<string>>.NotIn("FApproveStatus", param.fapprovearea));
                }
                // c - 年初,z - 年中,x - 专项
                if (param.workType == "z") //年中
                {
                    //年中时可以调整年初预算单据
                    var dicWhere1 = new Dictionary<string, object>(); //年中调整新增
                    var dicWhere2 = new Dictionary<string, object>(); //年初预算新增后调整
                    var dicWhere3 = new Dictionary<string, object>(); //年初新增
                    var dicWhere4 = new Dictionary<string, object>(); //年中新增
                    new CreateCriteria(dicWhere1).Add(ORMRestrictions<string>.Eq("FType", "z"))
                        .Add(ORMRestrictions<string>.Eq("FVerNo", "0002"))
                        .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));
                    new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FType", "c"))
                        .Add(ORMRestrictions<string>.Eq("FVerNo", "0002"))
                        .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0")); //年初没调整过的单据
                    new CreateCriteria(dicWhere3).Add(ORMRestrictions<string>.Eq("FType", "c"))
                        .Add(ORMRestrictions<string>.Eq("FVerNo", "0001"))
                        .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));
                    new CreateCriteria(dicWhere4).Add(ORMRestrictions<string>.Eq("FType", "z"))
                        .Add(ORMRestrictions<string>.Eq("FVerNo", "0001"))
                        .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));
                    new CreateCriteria(dicWhere).Add(ORMRestrictions.Or(dicWhere1, dicWhere2, dicWhere3, dicWhere4));
                    //new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FType", "z"));


                }
                else if (param.workType == "c")//年初
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FType", "c"))
                        .Add(ORMRestrictions<string>.Eq("FVerNo", "0001"));//.Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)) 要同时显示被年中调整的原单据
                }
                else
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));
                }

                //增加搜索条件
                if (!string.IsNullOrEmpty(param.FApproveStatus) && !"0".Equals(param.FApproveStatus))//审批状态
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<string>.Eq("FApproveStatus", param.FApproveStatus));
                }
                if (!string.IsNullOrEmpty(param.FProjCode))//项目编码
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FProjCode", param.FProjCode));
                }
                if (!string.IsNullOrEmpty(param.FProjName))//项目名称
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Like("FProjName", param.FProjName));
                }
                if (!string.IsNullOrEmpty(param.FDeclarationDept))//申报部门
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FDeclarationDept", param.FDeclarationDept));
                }
                if (!string.IsNullOrEmpty(param.FBudgetDept))//预算部门
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FBudgetDept", param.FDeclarationDept));
                }
                if (param.FProjAmountBegin > 0)//项目金额开始
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<Decimal>.Ge("FProjAmount", param.FProjAmountBegin));
                }
                if (param.FProjAmountEnd > 0)//项目金额结束
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<Decimal>.Le("FProjAmount", param.FProjAmountEnd));
                }
                if (!string.IsNullOrEmpty(param.FProjAttr))//项目属性
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FProjAttr", param.FProjAttr));
                }
                if (!string.IsNullOrEmpty(param.FDuration))//续存期限
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FDuration", param.FDuration));
                }
                if (!string.IsNullOrEmpty(param.FExpenseCategory) && !"0".Equals(param.FExpenseCategory))//支出类别
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FExpenseCategory", param.FExpenseCategory));
                }
                //if (param.FIfPerformanceAppraisal > 0)//绩效评价
                //{
                //    new CreateCriteria(dicWhere).Add(ORMRestrictions<EnumYesNo>.Eq("FIfPerformanceAppraisal", param.FIfPerformanceAppraisal));
                //}
                if (param.FStartDate != null)//起止日期
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<DateTime?>.Ge("FStartDate", param.FStartDate));
                }
                if (param.FEndDate != null)//结束日期
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<DateTime?>.Le("FEndDate", param.FEndDate));
                }
                if (param.FDateofDeclaration != null)//申报日期
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<DateTime?>.Ge("FDateofDeclaration", param.FDateofDeclaration));
                }

                var result = BudgetMstService.LoadWithPage(param.PageIndex, param.PageSize, dicWhere, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" });
                foreach (var item in result.Results)
                {
                    var dics = new Dictionary<string, object>();
                    new CreateCriteria(dics).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                    new CreateCriteria(dics).Add(ORMRestrictions<Int64>.Eq("RelPhid", item.PhId));
                    new CreateCriteria(dics).Add(ORMRestrictions<string>.Eq("BTable", "YS3_BUDGETMST"));
                    var Query = QtAttachmentService.Find(dics);
                    var QueryList = QtAttachmentService.Find(dics).Data.ToList();
                    item.list = QueryList;
                    //var address = Query.Data.Select(m => m.BUrlpath).ToArray();
                    //var name = Query.Data.Select(m => m.BName).ToArray();
                    //var model = new UploadPackGYS { UploadFileaddress = address, UploadFilename = name };
                    //item.Uploadmodel = model;
                    item.UploadFileCount = Query.Data.Select(m => m.BUrlpath).Count();
                }


                var dicSysset = new Dictionary<string, object>();
                new CreateCriteria(dicSysset).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                var syssets = QTSysSetService.Find(dicSysset).Data.ToList();
                if (result.Results != null && result.Results.Count > 0)
                {
                    foreach (var res in result.Results)
                    {
                        var syssetProjectMst = syssets.FindAll(x => x.DicType == "ProjectProper" && x.Orgcode == res.FDeclarationUnit && x.TypeCode == res.FProjAttr);
                        if (syssetProjectMst.Count > 0)
                        {
                            //项目属性代码转名称
                            res.FProjAttr_EXName = syssetProjectMst[0].TypeName;
                        }
                        var syssetProjectMst2 = syssets.FindAll(x => x.DicType == "TimeLimit" && x.Orgcode == res.FDeclarationUnit && x.TypeCode == res.FDuration);
                        if (syssetProjectMst2.Count > 0)
                        {
                            //存续期限代码转名称
                            res.FDuration_EXName = syssetProjectMst2[0].TypeName;
                        }

                        var syssetProjectMst3 = syssets.FindAll(x => x.DicType == "ProjectLevel" && x.Orgcode == res.FDeclarationUnit && x.TypeCode == res.FLevel);
                        if (syssetProjectMst3.Count > 0)
                        {
                            //项目级别代码转名称
                            res.FLevel_EXName = syssetProjectMst3[0].TypeName;
                        }
                    }
                }
                Result4 = Convert.ToInt32(result.TotalItems);


                #endregion

                if (string.IsNullOrEmpty(Result1))
                {
                    Result1 = "0";
                }
                if (string.IsNullOrEmpty(Result2))
                {
                    Result2 = "0";
                }
                if (Result3 == 0)
                {
                    Result3 = 0;
                }
                if (Result4 == 0)
                {
                    Result4 = 0;
                }
                try
                {
                    var data = new
                    {
                        Status = ResponseStatus.Success,
                        Msg = "获取成功",
                        ylix1 = Result1,
                        ylix2 = Result2,
                        ylix = Result3,
                        nizt = Result4,
                    };
                    return DataConverterHelper.SerializeObject(data);

                }
                catch (Exception ex)
                {
                    var data = new
                    {
                        Status = ResponseStatus.Error,
                        Msg = ex.ToString()
                    };
                    return DataConverterHelper.SerializeObject(data);

                }
            }
            else
            {
                return DCHelper.ErrorMessage("参数信息不完整！");
            }

        }

        /// <summary>
        ///启用审批流情况下 对用户进行消息提醒需求
        /// </summary>
        /// <param name="Ucode"></param>
        /// <param name="Year"></param>
        /// <param name="OrgCode"></param>
        /// <param name="uid"></param>
        /// <param name="Orgid"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetExaminedProcess([FromUri] string Ucode, string Year, string OrgCode, long uid, long Orgid)
        {
            //Ucode = "9999";
            //Year = "2019";
            //OrgCode = "101";
            var result = 0;
            if (!string.IsNullOrEmpty(Ucode) && !string.IsNullOrEmpty(Year) && !string.IsNullOrEmpty(OrgCode))
            {
                #region 定义参数规范

                var dicWhereDept = new Dictionary<string, object>();
                new CreateCriteria(dicWhereDept)
                    .Add(ORMRestrictions<string>.Eq("Dwdm", Ucode)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
                var deptList = CorrespondenceSettingsService.Find(dicWhereDept);
                List<string> deptL = new List<string>();
                for (var i = 0; i < deptList.Data.Count; i++)
                {
                    deptL.Add(deptList.Data[i].Dydm);
                }
                #endregion

                #region 预立项项目
                //年份与单位编码筛选预立项

                //Dictionary<string, object> dic = new Dictionary<string, object>();
                //new CreateCriteria(dic)
                //        .Add(ORMRestrictions<string>.Eq("FYear", Year))
                //        .Add(ORMRestrictions<string>.Eq("FDeclarationUnit", OrgCode));
                //new CreateCriteria(dic)
                //        .Add(ORMRestrictions<List<string>>.In("FApproveStatus", new List<string> { "1", "5" }))
                //        .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
                //        .Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL))
                //        .Add(ORMRestrictions<List<Int32>>.In("FProjStatus", new List<Int32> { 1 }));
                //var ylix1 = ProjectMstService.Find(dic);

                //年份与单位编码筛选预立项
                ProjectMstRequest projectMst1 = new ProjectMstRequest();
                projectMst1.Ucode = Ucode;
                projectMst1.OrgCode = OrgCode;
                projectMst1.Year = Year;
                projectMst1.fapprovearea = new List<string>() { "1", "5" };
                projectMst1.projectarea = new List<int>() { 1 };
                var Result1 = JsonConvert.DeserializeObject<ProjectMstListModel>(GetProjectMstList2(projectMst1)).totalRows;

                try
                {
                    BillRequestModel billRequest = new BillRequestModel();
                    billRequest.Uid = uid;
                    billRequest.Orgid = Orgid;
                    billRequest.Year = Year;
                    if (billRequest == null || billRequest.Uid == 0)
                    {
                        return DCHelper.ErrorMessage("用户id为空！");
                    }
                    /*if (string.IsNullOrEmpty(billRequest.OrgCode))
                    {
                        return DCHelper.ErrorMessage("组织编码为空！");
                    }*/
                    if (billRequest.Orgid == 0)
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    if (string.IsNullOrEmpty(billRequest.Year))
                    {
                        return DCHelper.ErrorMessage("年度为空！");
                    }

                    //获取审批所有类型
                    List<QTSysSetModel> procTypes = QTSysSetService.GetProcTypes().ToList().FindAll(X => X.Value != "001" && X.Value != "002" && X.Value != "003");
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

                            sysSet.YNum = total;
                            sysSet.NNum = total2;

                        }
                    }
                    result = procTypes.Sum(m => m.NNum);
                }
                catch (Exception e)
                {
                    return DCHelper.ErrorMessage(e.Message);
                }

                #endregion

                #region 立项项目
                //项目类型条件筛选立项
                //Dictionary<string, object> lixc = new Dictionary<string, object>();
                //new CreateCriteria(lixc)
                //        .Add(ORMRestrictions<string>.Eq("FYear", Year))
                //        .Add(ORMRestrictions<string>.Eq("FDeclarationUnit", OrgCode));
                ////项目类型条件筛选立项
                //new CreateCriteria(lixc)
                //   .Add(ORMRestrictions<List<string>>.In("FApproveStatus", new List<string> { "3" }))
                //   .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
                //   .Add(ORMRestrictions<IList<int>>.In("FProjStatus", new List<int> { 2 }))
                //   .Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
                //var lix = ProjectMstService.Find(lixc);

                ProjectMstRequest projectMst = new ProjectMstRequest();
                projectMst.OrgCode = OrgCode;
                projectMst.Year = Year;
                projectMst.Ucode = Ucode;
                projectMst.fapprovearea = new List<string>() { "3" };
                projectMst.projectarea = new List<int>() { 2 };
                var Result3 = 0;
                if (string.IsNullOrEmpty(projectMst.OrgCode))
                {
                    return DCHelper.ErrorMessage("单位编码不能为空！");
                }
                if (string.IsNullOrEmpty(projectMst.Ucode))
                {
                    return DCHelper.ErrorMessage("用户编码不能为空！");
                }
                if (string.IsNullOrEmpty(projectMst.Year))
                {
                    return DCHelper.ErrorMessage("年度不能为空！");
                }
                try
                {
                    //年份与单位编码筛选
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    new CreateCriteria(dic)
                            .Add(ORMRestrictions<string>.Eq("FYear", projectMst.Year))
                            .Add(ORMRestrictions<string>.Eq("FDeclarationUnit", projectMst.OrgCode));
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
                    if (projectMst.projectarea != null)
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<List<Int32>>.In("FProjStatus", projectMst.projectarea));
                    }
                    if (projectMst.fapprovearea != null)
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<List<string>>.In("FApproveStatus", projectMst.fapprovearea));
                    }
                    //增加搜索条件
                    if (!string.IsNullOrEmpty(projectMst.SearchValue))
                    {
                        Dictionary<string, object> dicName = new Dictionary<string, object>();
                        Dictionary<string, object> dicCode = new Dictionary<string, object>();
                        new CreateCriteria(dicName)
                                .Add(ORMRestrictions<string>.Like("FProjName", projectMst.SearchValue));
                        new CreateCriteria(dicCode)
                                .Add(ORMRestrictions<string>.Like("FProjCode", projectMst.SearchValue));
                        new CreateCriteria(dic).Add(ORMRestrictions.Or(dicName, dicCode));
                    }
                    if (!string.IsNullOrEmpty(projectMst.FExpenseCategory) && !"0".Equals(projectMst.FExpenseCategory))
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<string>.Eq("FExpenseCategory", projectMst.FExpenseCategory));
                    }
                    if (!string.IsNullOrEmpty(projectMst.FBudgetDept))//预算部门
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<string>.Eq("FBudgetDept", projectMst.FBudgetDept));
                    }
                    if (!string.IsNullOrEmpty(projectMst.FDeclarationDept))//申报部门
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<string>.Eq("FDeclarationDept", projectMst.FDeclarationDept));
                    }
                    if (!string.IsNullOrEmpty(projectMst.FProjCode))//项目编码
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<string>.Eq("FProjCode", projectMst.FProjCode));
                    }
                    if (!string.IsNullOrEmpty(projectMst.FProjName))//项目名称
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<string>.Eq("FProjName", projectMst.FProjName));
                    }
                    if (!string.IsNullOrEmpty(projectMst.FProjAmountBegin))
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<Decimal>.Ge("FProjAmount", Decimal.Parse(projectMst.FProjAmountBegin)));
                    }
                    if (!string.IsNullOrEmpty(projectMst.FProjAmountEnd))
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<Decimal>.Le("FProjAmount", Decimal.Parse(projectMst.FProjAmountEnd)));
                    }
                    if (projectMst.FStartDate != null)
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<System.DateTime?>.Ge("FDateofDeclaration", projectMst.FStartDate));
                    }
                    if (projectMst.FEndDate != null)
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<System.DateTime?>.Le("FDateofDeclaration", projectMst.FEndDate));
                    }
                    if (projectMst.FIfPerformanceAppraisal != 0 && !string.IsNullOrEmpty(projectMst.FIfPerformanceAppraisal.ToString()))
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<EnumYesNo>.Eq("FIfPerformanceAppraisal", projectMst.FIfPerformanceAppraisal));
                    }
                    var resultss = this.ProjectMstService.LoadWithPage(projectMst.PageIndex, projectMst.PageSize, dic, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" });

                    //提高接口效率
                    var Query = QtAttachmentService.Find(t => t.BTable == "XM3_PROJECTMST").Data;
                    foreach (var item in resultss.Results)
                    {
                        if (Query != null && Query.Count > 0)
                        {
                            var uploadlist = Query.ToList().FindAll(t => t.RelPhid == item.PhId);
                            item.list = uploadlist;
                            item.UploadFileCount = uploadlist.Count;
                        }
                    }
                    var dicSyssets = new Dictionary<string, object>();
                    new CreateCriteria(dicSyssets).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                    var syssetss = QTSysSetService.Find(dicSyssets).Data.ToList();
                    foreach (var data in resultss.Results)
                    {
                        var syssetProjectMst = syssetss.FindAll(x => x.DicType == "ProjectProper" && x.Orgcode == data.FDeclarationUnit && x.TypeCode == data.FProjAttr);
                        if (syssetProjectMst.Count > 0)
                        {
                            //项目属性代码转名称
                            data.FProjAttr_EXName = syssetProjectMst[0].TypeName;
                        }
                        var syssetProjectMst2 = syssetss.FindAll(x => x.DicType == "TimeLimit" && x.Orgcode == data.FDeclarationUnit && x.TypeCode == data.FDuration);
                        if (syssetProjectMst2.Count > 0)
                        {
                            //存续期限代码转名称
                            data.FDuration_EXName = syssetProjectMst2[0].TypeName;
                        }

                        var syssetProjectMst3 = syssetss.FindAll(x => x.DicType == "ProjectLevel" && x.Orgcode == data.FDeclarationUnit && x.TypeCode == data.FLevel);
                        if (syssetProjectMst3.Count > 0)
                        {
                            //项目级别代码转名称
                            data.FLevel_EXName = syssetProjectMst3[0].TypeName;
                        }

                        var dtls = ProjectMstService.FindProjectDtlBudgetDtlByForeignKey(data.PhId).Data;
                        var FIfYsxz = 0;
                        if (dtls.Count > 0)
                        {
                            foreach (var dtl in dtls)
                            {
                                if (string.IsNullOrEmpty(dtl.FBudgetAccounts) || string.IsNullOrEmpty(dtl.FExpensesChannel))
                                {
                                    FIfYsxz++;
                                }
                            }
                        }
                        data.FIfYsxz = FIfYsxz;
                    }
                    Result3 = Convert.ToInt32(resultss.TotalItems);
                }
                catch (Exception ex)
                {
                    return DCHelper.ErrorMessage(ex.Message);
                }

                #endregion

                #region 年终调整
                //年份与单位编码筛选年终调整
                //Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                //new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FApproveStatus", "3"));
                //new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FDeclarationUnit", OrgCode));
                //new CreateCriteria(dicWhere).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
                //new CreateCriteria(dicWhere).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0));
                //new CreateCriteria(dicWhere).Add(ORMRestrictions<List<Int32>>.NotIn("FProjStatus", new List<Int32> { 4, 9, 3 }));
                //var niz = BudgetMstService.Find(dicWhere);


                var Result4 = 0;
                BudgetMstListsRequestModel param = new BudgetMstListsRequestModel();
                param.Ucode = Ucode;
                param.OrgCode = OrgCode;
                param.Year = Year;
                param.fapprovearea = new List<string>() { "3" };
                param.projectarea = new List<int>() { 4, 9, 3 };
                if (string.IsNullOrEmpty(param.Ucode))
                {
                    return DCHelper.ErrorMessage("用户编码不能为空！");
                }
                if (string.IsNullOrEmpty(param.OrgCode))
                {
                    return DCHelper.ErrorMessage("组织编码不能为空！");
                }
                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<string>.Eq("FDeclarationUnit", param.OrgCode));
                new CreateCriteria(dicWhere).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
                new CreateCriteria(dicWhere).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0));
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", Year));
                if (param.projectarea != null)
                {
                    new CreateCriteria(dicWhere)
                         .Add(ORMRestrictions<List<Int32>>.In("FProjStatus", param.projectarea));
                }
                if (param.fapprovearea != null)
                {
                    new CreateCriteria(dicWhere)
                          .Add(ORMRestrictions<List<string>>.In("FApproveStatus", param.fapprovearea));
                }
                // c - 年初,z - 年中,x - 专项
                if (param.workType == "z") //年中
                {
                    //年中时可以调整年初预算单据
                    var dicWhere1 = new Dictionary<string, object>(); //年中调整新增
                    var dicWhere2 = new Dictionary<string, object>(); //年初预算新增后调整
                    var dicWhere3 = new Dictionary<string, object>(); //年初新增
                    var dicWhere4 = new Dictionary<string, object>(); //年中新增
                    new CreateCriteria(dicWhere1).Add(ORMRestrictions<string>.Eq("FType", "z"))
                        .Add(ORMRestrictions<string>.Eq("FVerNo", "0002"))
                        .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));
                    new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FType", "c"))
                        .Add(ORMRestrictions<string>.Eq("FVerNo", "0002"))
                        .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0")); //年初没调整过的单据
                    new CreateCriteria(dicWhere3).Add(ORMRestrictions<string>.Eq("FType", "c"))
                        .Add(ORMRestrictions<string>.Eq("FVerNo", "0001"))
                        .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));
                    new CreateCriteria(dicWhere4).Add(ORMRestrictions<string>.Eq("FType", "z"))
                        .Add(ORMRestrictions<string>.Eq("FVerNo", "0001"))
                        .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));
                    new CreateCriteria(dicWhere).Add(ORMRestrictions.Or(dicWhere1, dicWhere2, dicWhere3, dicWhere4));
                    //new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FType", "z"));


                }
                else if (param.workType == "c")//年初
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FType", "c"))
                        .Add(ORMRestrictions<string>.Eq("FVerNo", "0001"));//.Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)) 要同时显示被年中调整的原单据
                }
                else
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));
                }

                //增加搜索条件
                if (!string.IsNullOrEmpty(param.FApproveStatus) && !"0".Equals(param.FApproveStatus))//审批状态
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<string>.Eq("FApproveStatus", param.FApproveStatus));
                }
                if (!string.IsNullOrEmpty(param.FProjCode))//项目编码
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FProjCode", param.FProjCode));
                }
                if (!string.IsNullOrEmpty(param.FProjName))//项目名称
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Like("FProjName", param.FProjName));
                }
                if (!string.IsNullOrEmpty(param.FDeclarationDept))//申报部门
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FDeclarationDept", param.FDeclarationDept));
                }
                if (!string.IsNullOrEmpty(param.FBudgetDept))//预算部门
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FBudgetDept", param.FDeclarationDept));
                }
                if (param.FProjAmountBegin > 0)//项目金额开始
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<Decimal>.Ge("FProjAmount", param.FProjAmountBegin));
                }
                if (param.FProjAmountEnd > 0)//项目金额结束
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<Decimal>.Le("FProjAmount", param.FProjAmountEnd));
                }
                if (!string.IsNullOrEmpty(param.FProjAttr))//项目属性
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FProjAttr", param.FProjAttr));
                }
                if (!string.IsNullOrEmpty(param.FDuration))//续存期限
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FDuration", param.FDuration));
                }
                if (!string.IsNullOrEmpty(param.FExpenseCategory) && !"0".Equals(param.FExpenseCategory))//支出类别
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FExpenseCategory", param.FExpenseCategory));
                }
                //if (param.FIfPerformanceAppraisal > 0)//绩效评价
                //{
                //    new CreateCriteria(dicWhere).Add(ORMRestrictions<EnumYesNo>.Eq("FIfPerformanceAppraisal", param.FIfPerformanceAppraisal));
                //}
                if (param.FStartDate != null)//起止日期
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<DateTime?>.Ge("FStartDate", param.FStartDate));
                }
                if (param.FEndDate != null)//结束日期
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<DateTime?>.Le("FEndDate", param.FEndDate));
                }
                if (param.FDateofDeclaration != null)//申报日期
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<DateTime?>.Ge("FDateofDeclaration", param.FDateofDeclaration));
                }

                var results = BudgetMstService.LoadWithPage(param.PageIndex, param.PageSize, dicWhere, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" });
                foreach (var item in results.Results)
                {
                    var dics = new Dictionary<string, object>();
                    new CreateCriteria(dics).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                    new CreateCriteria(dics).Add(ORMRestrictions<Int64>.Eq("RelPhid", item.PhId));
                    new CreateCriteria(dics).Add(ORMRestrictions<string>.Eq("BTable", "YS3_BUDGETMST"));
                    var Query = QtAttachmentService.Find(dics);
                    var QueryList = QtAttachmentService.Find(dics).Data.ToList();
                    item.list = QueryList;
                    //var address = Query.Data.Select(m => m.BUrlpath).ToArray();
                    //var name = Query.Data.Select(m => m.BName).ToArray();
                    //var model = new UploadPackGYS { UploadFileaddress = address, UploadFilename = name };
                    //item.Uploadmodel = model;
                    item.UploadFileCount = Query.Data.Select(m => m.BUrlpath).Count();
                }


                var dicSysset = new Dictionary<string, object>();
                new CreateCriteria(dicSysset).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                var syssets = QTSysSetService.Find(dicSysset).Data.ToList();
                if (results.Results != null && results.Results.Count > 0)
                {
                    foreach (var res in results.Results)
                    {
                        var syssetProjectMst = syssets.FindAll(x => x.DicType == "ProjectProper" && x.Orgcode == res.FDeclarationUnit && x.TypeCode == res.FProjAttr);
                        if (syssetProjectMst.Count > 0)
                        {
                            //项目属性代码转名称
                            res.FProjAttr_EXName = syssetProjectMst[0].TypeName;
                        }
                        var syssetProjectMst2 = syssets.FindAll(x => x.DicType == "TimeLimit" && x.Orgcode == res.FDeclarationUnit && x.TypeCode == res.FDuration);
                        if (syssetProjectMst2.Count > 0)
                        {
                            //存续期限代码转名称
                            res.FDuration_EXName = syssetProjectMst2[0].TypeName;
                        }

                        var syssetProjectMst3 = syssets.FindAll(x => x.DicType == "ProjectLevel" && x.Orgcode == res.FDeclarationUnit && x.TypeCode == res.FLevel);
                        if (syssetProjectMst3.Count > 0)
                        {
                            //项目级别代码转名称
                            res.FLevel_EXName = syssetProjectMst3[0].TypeName;
                        }
                    }
                }
                Result4 = Convert.ToInt32(results.TotalItems);
                #endregion
                if (string.IsNullOrEmpty(Result1))
                {
                    Result1 = "0";
                }
                if (result == 0)
                {
                    result = 0;
                }
                if (Result3 == 0)
                {
                    Result3 = 0;
                }
                if (Result4 == 0)
                {
                    Result4 = 0;
                }


                try
                {
                    var data = new
                    {
                        Status = ResponseStatus.Success,
                        Msg = "获取成功",
                        ylix1 = Result1,
                        spzx = result,
                        ylix = Result3,
                        nizt = Result4,
                    };
                    return DataConverterHelper.SerializeObject(data);

                }
                catch (Exception ex)
                {
                    var data = new
                    {
                        Status = ResponseStatus.Error,
                        Msg = ex.ToString()
                    };
                    return DataConverterHelper.SerializeObject(data);

                }

            }
            else
            {
                return DCHelper.ErrorMessage("参数信息不完整！");
            }

        }

        #region//民生银行专版

        //<span v-if="scope.row.FProjStatus ===1">项目立项</span>
        //<span v-if="scope.row.FProjStatus ===2">项目草案</span>
        //<span v-if="scope.row.FProjStatus ===3">项目执行</span>
        //<span v-if="scope.row.FProjStatus ===4">项目调整</span>
        //<span v-if="scope.row.FProjStatus ===5">调整项目执行</span>
        //<span v-if="scope.row.FProjStatus ===6">项目终止</span>
        //<span v-if="scope.row.FProjStatus ===7">项目关闭</span>
        //<span v-if="scope.row.FProjStatus ===9">项目预执行</span>
        //<span v-if="scope.row.FProjStatus ===10">年中新增执行</span>
        //<span v-if="scope.row.FProjStatus ===11">年中项目执行</span>
        //<span v-if="scope.row.FProjStatus ===12">年中新增项目</span>

        /// <summary>
        /// 获取民生银行支出预算列表集合
        /// </summary>
        /// <param name="projectMst"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetMSYHProjectMstList([FromUri]ProjectMstRequest projectMst)
        {
            if (string.IsNullOrEmpty(projectMst.OrgCode))
            {
                return DCHelper.ErrorMessage("单位编码不能为空！");
            }
            if (string.IsNullOrEmpty(projectMst.Ucode) || projectMst.UserId == 0)
            {
                return DCHelper.ErrorMessage("用户信息不能为空！");
            }
            if (string.IsNullOrEmpty(projectMst.Year))
            {
                return DCHelper.ErrorMessage("年度不能为空！");
            }
            try
            {
                //var dicWhereDept = new Dictionary<string, object>();
                //new CreateCriteria(dicWhereDept)
                //    .Add(ORMRestrictions<string>.Eq("Dwdm", projectMst.Ucode)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
                //var deptList = CorrespondenceSettingsService.Find(dicWhereDept);
                //List<string> deptL = new List<string>();
                //for (var i = 0; i < deptList.Data.Count; i++)
                //{
                //    deptL.Add(deptList.Data[i].Dydm);
                //}
                //年份与单位编码筛选（排除掉已被删除的数据）
                Dictionary<string, object> dic = new Dictionary<string, object>();
                new CreateCriteria(dic)
                        .Add(ORMRestrictions<string>.Eq("FYear", projectMst.Year))
                        .Add(ORMRestrictions<string>.Eq("FDeclarationUnit", projectMst.OrgCode))
                        .Add(ORMRestrictions<byte>.Eq("FDeleteMark", (byte)0));
                //项目类型条件筛选
                if (projectMst.ProjStatus == "1")//项目立项
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
                        .Add(ORMRestrictions<List<Int32>>.In("FProjStatus", new List<Int32> { 1 }));
                }
                else if (projectMst.ProjStatus == "2") //项目草案
                {
                    new CreateCriteria(dic)//.Add(ORMRestrictions<Int32>.Eq("FProjStatus", 2))
                       .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<Int32>.Eq("FProjStatus", 2));
                }
                else if (projectMst.ProjStatus == "3")//项目执行
                {
                    new CreateCriteria(dic)//.Add(ORMRestrictions<Int32>.Eq("FProjStatus", 2))
                       .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<Int32>.Eq("FProjStatus", 3));
                }
                else if (projectMst.ProjStatus == "0")
                {
                    new CreateCriteria(dic)//.Add(ORMRestrictions<Int32>.Eq("FProjStatus", 2))
                       .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<IList<int>>.NotIn("FProjStatus", new List<int> { 1 }));
                }
                else
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0));
                }
                //增加搜索条件
                if (!string.IsNullOrEmpty(projectMst.SearchValue))
                {
                    Dictionary<string, object> dicName = new Dictionary<string, object>();
                    Dictionary<string, object> dicCode = new Dictionary<string, object>();
                    new CreateCriteria(dicName)
                            .Add(ORMRestrictions<string>.Like("FProjName", projectMst.SearchValue));
                    new CreateCriteria(dicCode)
                            .Add(ORMRestrictions<string>.Like("FProjCode", projectMst.SearchValue));
                    new CreateCriteria(dic).Add(ORMRestrictions.Or(dicName, dicCode));
                }
                if (!string.IsNullOrEmpty(projectMst.FExpenseCategory) && !"0".Equals(projectMst.FExpenseCategory))
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<string>.Eq("FExpenseCategory", projectMst.FExpenseCategory));
                }
                if (!string.IsNullOrEmpty(projectMst.FApproveStatus) && !"0".Equals(projectMst.FApproveStatus))
                {
                    if (projectMst.FApproveStatus == "1")
                    {
                        new CreateCriteria(dic)
                           .Add(ORMRestrictions<List<string>>.In("FApproveStatus", new List<string> { "1", "5" }));//暂存的也要出来
                    }
                    else
                    {
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<string>.Eq("FApproveStatus", projectMst.FApproveStatus));
                    }
                }
                if (!string.IsNullOrEmpty(projectMst.FBudgetDept))//预算部门
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<string>.Eq("FBudgetDept", projectMst.FBudgetDept));
                }
                if (!string.IsNullOrEmpty(projectMst.FDeclarationDept))//申报部门
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<string>.Eq("FDeclarationDept", projectMst.FDeclarationDept));
                }
                if (!string.IsNullOrEmpty(projectMst.FProjCode))//项目编码
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<string>.Eq("FProjCode", projectMst.FProjCode));
                }
                if (!string.IsNullOrEmpty(projectMst.FProjName))//项目名称
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<string>.Eq("FProjName", projectMst.FProjName));
                }
                if (!string.IsNullOrEmpty(projectMst.FProjAmountBegin))
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<Decimal>.Ge("FProjAmount", Decimal.Parse(projectMst.FProjAmountBegin)));
                }
                if (!string.IsNullOrEmpty(projectMst.FProjAmountEnd))
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<Decimal>.Le("FProjAmount", Decimal.Parse(projectMst.FProjAmountEnd)));
                }
                if (projectMst.FStartDate != null)
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<System.DateTime?>.Ge("FDateofDeclaration", projectMst.FStartDate));
                }
                if (projectMst.FEndDate != null)
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<System.DateTime?>.Le("FDateofDeclaration", projectMst.FEndDate));
                }
                if (projectMst.FIfPerformanceAppraisal != 0 && !string.IsNullOrEmpty(projectMst.FIfPerformanceAppraisal.ToString()))
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<EnumYesNo>.Eq("FIfPerformanceAppraisal", projectMst.FIfPerformanceAppraisal));
                }
                //名生银行的需要按用户筛选数据且不分页
                if (projectMst.UserId != 0)
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<long>.Eq("FDeclarerId", projectMst.UserId));
                }

                //var result = this.ProjectMstService.LoadWithPage(projectMst.PageIndex, projectMst.PageSize, dic, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" });

                var result = this.ProjectMstService.Find(dic, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" }).Data;
                //取可选相同审批流是数据集合
                if (projectMst.ProcPhid != 0)
                {
                    var proList = this.ProjectMstService.Find(dic, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" }).Data;
                    if (proList != null && proList.Count > 0)
                    {
                        List<string> orgList = proList.ToList().Select(t => t.FBudgetDept).Distinct().ToList();
                        if (orgList != null && orgList.Count > 0)
                        {
                            var procList = this.GAppvalProcService.Find(t => orgList.Contains(t.OrgCode)).Data;
                            if (procList != null && procList.Count > 0)
                            {
                                //可以选取相同审批流的打上标记
                                foreach (var res in proList)
                                {
                                    if (res.FApproveStatus == ((int)EnumApproveStatus.ToBeRepored).ToString() && procList.ToList().Find(t => t.OrgCode == res.FBudgetDept && t.PhId == projectMst.ProcPhid) != null)
                                    {
                                        res.BatchPracBz = 1;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                proList = proList.ToList().FindAll(t => t.BatchPracBz == 1);
                            }
                        }
                    }

                    result = proList;
                    //result.TotalItems = proList.Count;
                }

                //提高接口效率
                var Query = QtAttachmentService.Find(t => t.BTable == "XM3_PROJECTMST").Data;
                foreach (var item in result)
                {
                    if (Query != null && Query.Count > 0)
                    {
                        var uploadlist = Query.ToList().FindAll(t => t.RelPhid == item.PhId);
                        item.list = uploadlist;
                        item.UploadFileCount = uploadlist.Count;
                    }
                }

                //先把该组织下的基础数据都找出来
                var dicSysset = new Dictionary<string, object>();
                new CreateCriteria(dicSysset).Add(ORMRestrictions<Int64>.NotEq("PhId", 0))
                    .Add(ORMRestrictions<string>.Eq("Orgcode", projectMst.OrgCode));
                var syssets = QTSysSetService.Find(dicSysset).Data.ToList();
                var projectCodes = result.Select(p => p.FProjCode.Remove(p.FProjCode.Length - 6,6)).Distinct().ToList();
                var qtxms = QtXmDistributeService.Find(p => projectCodes.Contains(p.FProjcode)).Data;
                foreach (var data in result)
                {
                    var syssetProjectMst = syssets.FindAll(x => x.DicType == "ProjectProper" && x.Orgcode == data.FDeclarationUnit && x.TypeCode == data.FProjAttr);
                    if (syssetProjectMst.Count > 0)
                    {
                        //项目属性代码转名称
                        data.FProjAttr_EXName = syssetProjectMst[0].TypeName;
                    }
                    var syssetProjectMst2 = syssets.FindAll(x => x.DicType == "TimeLimit" && x.Orgcode == data.FDeclarationUnit && x.TypeCode == data.FDuration);
                    if (syssetProjectMst2.Count > 0)
                    {
                        //存续期限代码转名称
                        data.FDuration_EXName = syssetProjectMst2[0].TypeName;
                    }

                    var syssetProjectMst3 = syssets.FindAll(x => x.DicType == "ProjectLevel" && x.Orgcode == data.FDeclarationUnit && x.TypeCode == data.FLevel);
                    if (syssetProjectMst3.Count > 0)
                    {
                        //项目级别代码转名称
                        data.FLevel_EXName = syssetProjectMst3[0].TypeName;
                    }

                    

                    var qtxm = qtxms.Where(p => data.FProjCode.StartsWith(p.FProjcode)).FirstOrDefault();
                    if(qtxm != null)
                    {
                        data.FProjName = qtxm.FProjname;
                        data.FBusinessCode = qtxm.FBusiness;
                    }

                    var businesses = syssets.FindAll(x => x.DicType == "Business" && x.Orgcode == data.FDeclarationUnit && x.TypeCode == data.FBusinessCode);
                    if (businesses != null && businesses.Count > 0)
                    {
                        //业务条线转名称
                        data.FBusinessName = businesses[0].TypeName;
                    }
                    //var dtls = ProjectMstService.FindProjectDtlBudgetDtlByForeignKey(data.PhId).Data;
                    //var FIfYsxz = 0;
                    //if (dtls.Count > 0)
                    //{
                    //    foreach (var dtl in dtls)
                    //    {
                    //        if (string.IsNullOrEmpty(dtl.FBudgetAccounts) || string.IsNullOrEmpty(dtl.FExpensesChannel))
                    //        {
                    //            FIfYsxz++;
                    //        }
                    //    }
                    //}
                    //data.FIfYsxz = FIfYsxz;
                }
                List<ProjectAllDataModel> projectAllDatas = new List<ProjectAllDataModel>();
                if (result != null && result.Count > 0)
                {
                    projectAllDatas = this.ProjectMstService.GetProjectAllDataModels(result.ToList());
                }
                //return DCHelper.ModelListToJson<ProjectMstModel>(result, (Int32)result.Count);

                var data1 = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "获取成功！",
                    Data = projectAllDatas,
                    Count = projectAllDatas.Count
                };
                return DataConverterHelper.SerializeObject(data1);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 保存立项数据
        /// </summary>
        /// <param name="projectAllData"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostSaveMSYHProject([FromBody]ProjectAllDataRequest projectAllData)
        {
            if (projectAllData == null || projectAllData.ProjectAllDataModels == null || projectAllData.ProjectAllDataModels.Count <= 0)
            {
                return DCHelper.ErrorMessage("保存的参数不能为空！");
            }
            if (projectAllData.UserId == 0)
            {
                return DCHelper.ErrorMessage("用户信息不能为空！");
            }
            try
            {
                //获取当前所有数据库中所有已存在的单据编码
                List<string> allCodes = new List<string>();
                allCodes = this.ProjectMstService.Find(t => t.PhId != 0 && t.FDeleteMark == (byte)0).Data.Select(t => t.FProjCode).ToList();

                //保留要保存数据
                List<ProjectAllDataModel> projectAlls = new List<ProjectAllDataModel>();

                //保存信息
                SavedResult<Int64> savedresult = new SavedResult<Int64>();

                foreach (var projectData in projectAllData.ProjectAllDataModels)
                {
                    ProjectAllDataModel projectAll = new ProjectAllDataModel();

                    ProjectMstModel mstforminfo = new ProjectMstModel();
                    if (projectData.ProjectMst != null)
                    {
                        mstforminfo = projectData.ProjectMst;
                        //预算金额与项目金额一般一样
                        mstforminfo.FBudgetAmount = mstforminfo.FProjAmount;
                    }

                    List<ProjectDtlBudgetDtlModel> projectdtlbudgetdtlgridinfo = new List<ProjectDtlBudgetDtlModel>();
                    if (projectData.ProjectDtlBudgetDtls != null)
                    {
                        projectdtlbudgetdtlgridinfo = projectData.ProjectDtlBudgetDtls;
                    }
                    ProjectDtlBudgetDtlModel projectDtlBudgetDtl = new ProjectDtlBudgetDtlModel();
                    //主表数据整理
                    if (mstforminfo != null)
                    {
                        if (mstforminfo.PhId == 0)
                        {
                            if (mstforminfo.PersistentState == PersistentState.Deleted)
                            {
                                continue;
                            }
                            mstforminfo.PersistentState = PersistentState.Added;
                        }
                        else
                        {
                            if (mstforminfo.PersistentState != PersistentState.Deleted)
                            {
                                mstforminfo.PersistentState = PersistentState.Modified;
                                if (mstforminfo.FProjStatus == 1)
                                {
                                    //if (!string.IsNullOrEmpty(mstforminfo.FApproveStatus) && mstforminfo.FApproveStatus != "1" && mstforminfo.FApproveStatus != "4")
                                    //{
                                    //    return DCHelper.ErrorMessage("审批以及审批成功的项目立项单据不能进行修改！");
                                    //}
                                }
                                else if (mstforminfo.FProjStatus == 2)
                                {
                                    if (!string.IsNullOrEmpty(mstforminfo.FApproveStatus) && mstforminfo.FApproveStatus == "2")
                                    {
                                        return DCHelper.ErrorMessage("审批中的草案单据不能进行修改！");
                                    }
                                    if (!string.IsNullOrEmpty(mstforminfo.FApproveStatus) && mstforminfo.FApproveStatus == "3")
                                    {
                                        //审批通过的项目草案再次修改  状态要变成项目立项待送审
                                        mstforminfo.FProjStatus = 1;
                                        mstforminfo.FApproveStatus = "1";
                                    }
                                }
                                else
                                {
                                    return DCHelper.ErrorMessage("项目状态传递有误！");
                                }
                            }
                            else
                            {
                                if (mstforminfo.FProjStatus == 1)
                                {
                                    if (!string.IsNullOrEmpty(mstforminfo.FApproveStatus) && mstforminfo.FApproveStatus != "1" && mstforminfo.FApproveStatus != "4")
                                    {
                                        return DCHelper.ErrorMessage("审批以及审批成功的项目立项单据不能进行删除！");
                                    }
                                }
                                else if (mstforminfo.FProjStatus == 2)
                                {
                                    if (!string.IsNullOrEmpty(mstforminfo.FApproveStatus) && mstforminfo.FApproveStatus != "2" && mstforminfo.FApproveStatus != "4")
                                    {
                                        return DCHelper.ErrorMessage("审批中的草案单据不能进行删除！");
                                    }
                                }
                                else
                                {
                                    return DCHelper.ErrorMessage("项目状态传递有误！");
                                }
                            }
                        }
                    }
                    //明细表数据整理
                    if (projectdtlbudgetdtlgridinfo != null && projectdtlbudgetdtlgridinfo.Count > 0)
                    {
                        List<ProjectDtlBudgetDtlModel> projectDtls1 = new List<ProjectDtlBudgetDtlModel>();
                        List<ProjectDtlBudgetDtlModel> projectDtls2 = new List<ProjectDtlBudgetDtlModel>();
                        List<ProjectDtlBudgetDtlModel> projectDtls3 = new List<ProjectDtlBudgetDtlModel>();
                        foreach (var pro in projectdtlbudgetdtlgridinfo)
                        {
                            pro.FAmountAfterEdit = pro.FAmount + pro.FAmountEdit;
                            pro.FBudgetAmount = pro.FAmount;
                            if (pro.PhId == 0)
                            {
                                if (pro.PersistentState == PersistentState.Deleted)
                                {
                                    continue;
                                }
                                pro.PersistentState = PersistentState.Added;
                                projectDtls1.Add(pro);
                            }
                            else
                            {
                                if (pro.PersistentState == PersistentState.Deleted)
                                {
                                    projectDtls2.Add(pro);
                                }
                                else
                                {
                                    pro.PersistentState = PersistentState.Modified;
                                    projectDtls3.Add(pro);
                                }
                            }
                        }
                        projectDtlBudgetDtl.ProjectDtlBudgetDtlsAdd = projectDtls1;
                        projectDtlBudgetDtl.ProjectDtlBudgetDtlsDel = projectDtls2;
                        projectDtlBudgetDtl.ProjectDtlBudgetDtlsMid = projectDtls3;

                        projectDtls1.AddRange(projectDtls2);
                        projectDtls1.AddRange(projectDtls3);
                        projectdtlbudgetdtlgridinfo = projectDtls1;
                    }

                    //申报进度
                    if (string.IsNullOrEmpty(mstforminfo.FType)) //项目保存时，如果没有进度状态，则增加
                    {
                        var processStatus = BudgetProcessCtrlService.FindBudgetProcessCtrl(mstforminfo.FDeclarationUnit, mstforminfo.FBudgetDept, mstforminfo.FYear);
                        //单位进度控制为1时，是年初申报，为3时，为年中调整
                        if (processStatus == "1")
                        {
                            mstforminfo.FType = "c";
                            mstforminfo.FVerNo = "0001";
                        }
                        else
                        {
                            return DCHelper.ErrorMessage("此组织的进度已不在年初，无法修改年初数据！");
                        }
                        //else if (processStatus == "3")
                        //{

                        //    mstforminfo.FType = "z";
                        //    //budgetmst.FType = "z";
                        //    mstforminfo.FVerNo = "0002";
                        //}
                    }
                    else
                    {
                        var processStatus = BudgetProcessCtrlService.FindBudgetProcessCtrl(mstforminfo.FDeclarationUnit, mstforminfo.FBudgetDept, mstforminfo.FYear);
                        if (processStatus != "1")
                        {
                            return DCHelper.ErrorMessage("此组织的进度已不在年初，无法修改年初数据！");
                        }
                    }

                    //民生银行的数据应该都是由项目分发而来，所以项目编码是不可能为空的
                    if (string.IsNullOrEmpty(mstforminfo.FProjCode))
                    {
                        return DCHelper.ErrorMessage("分发的单据编码不能为空！");
                    }


                    var projCode = mstforminfo.FProjCode;
                    var year = mstforminfo.FYear;

                    //新增的项目
                    if (mstforminfo.PhId == 0)
                    {
                        //申报部门
                        mstforminfo.FDeclarationDept = ProjectMstService.GetDefaultDept(projectAllData.UserId);
                        //申报人id
                        mstforminfo.FDeclarerId = projectAllData.UserId;
                        #region 生成项目编码       
                        string maxCode = "";
                        //获取最大项目库编码
                        if (allCodes != null && allCodes.Count > 0)
                        {
                            maxCode = allCodes.ToList().FindAll(t => t.StartsWith(projCode)) == null ? "" : allCodes.ToList().FindAll(t => t.StartsWith(projCode)).Max();
                        }
                        //分发的编码再加6位流水线号
                        if (string.IsNullOrEmpty(maxCode))
                        {
                            projCode = mstforminfo.FProjCode + "000001";
                        }
                        else
                        {
                            projCode = mstforminfo.FProjCode + string.Format("{0:D6}", int.Parse(maxCode.Substring(maxCode.Length - 6, 6)) + 1);
                        }
                        allCodes.Add(projCode);
                        mstforminfo.FProjCode = projCode;
                        #endregion
                    }
                    else if(mstforminfo.PersistentState == PersistentState.Modified)
                    {
                        #region 生成项目编码       
                        string maxCode = "";
                        //获取最大项目库编码
                        if (allCodes != null && allCodes.Count > 0)
                        {
                            maxCode = allCodes.ToList().FindAll(t => t.StartsWith(projCode)) == null ? "" : allCodes.ToList().FindAll(t => t.StartsWith(projCode)).Max();
                        }
                        //分发的编码再加6位流水线号
                        if (string.IsNullOrEmpty(maxCode))
                        {
                            projCode = mstforminfo.FProjCode + "000001";
                        }
                        else
                        {
                            projCode = mstforminfo.FProjCode + string.Format("{0:D6}", int.Parse(maxCode.Substring(maxCode.Length - 6, 6)) + 1);
                        }
                        allCodes.Add(projCode);
                        mstforminfo.FProjCode = projCode;
                        #endregion
                    }



                    #region 生成项目明细编码: 项目明细编码=项目编码 + 4位流水号
                    string dtlCode = "";
                    string dtlName = "";
                    if (projectdtlbudgetdtlgridinfo != null && projectdtlbudgetdtlgridinfo.Count > 0)
                    {
                        //项目总金额等于明细金额的总和
                        mstforminfo.FBudgetAmount = projectdtlbudgetdtlgridinfo.ToList().FindAll(t => t.PersistentState != PersistentState.Deleted).Sum(t => t.FAmount);
                        mstforminfo.FProjAmount = projectdtlbudgetdtlgridinfo.ToList().FindAll(t => t.PersistentState != PersistentState.Deleted).Sum(t => t.FAmount);

                        //暂存该项目下有效的明细编码
                        List<string> alldtlCodes = projectdtlbudgetdtlgridinfo.ToList().FindAll(t => t.PersistentState != PersistentState.Deleted && !string.IsNullOrEmpty(t.FDtlCode)).Select(t => t.FDtlCode).ToList();
                        for (var i = 0; i < projectdtlbudgetdtlgridinfo.Count; i++)
                        {
                            if (projectdtlbudgetdtlgridinfo[i].PersistentState == PersistentState.Deleted)
                            {
                                continue;
                            }
                            dtlCode = projectdtlbudgetdtlgridinfo[i].FDtlCode;
                            dtlName = projectdtlbudgetdtlgridinfo[i].FName;
                            if (string.IsNullOrEmpty(dtlCode))
                            {
                                if (alldtlCodes != null && alldtlCodes.Count > 0)
                                {
                                    projectdtlbudgetdtlgridinfo[i].FDtlCode = projCode + string.Format("{0:D4}", int.Parse(alldtlCodes.Max().Substring(alldtlCodes.Max().Length - 4, 4)) + 1);
                                }
                                else
                                {
                                    projectdtlbudgetdtlgridinfo[i].FDtlCode = projCode + string.Format("{0:D4}", 1);
                                }
                            }
                            dtlCode = projectdtlbudgetdtlgridinfo[i].FDtlCode;
                            alldtlCodes.Add(dtlCode);

                        }
                    }
                    #endregion
                    projectAll.ProjectMst = mstforminfo;
                    projectAll.ProjectDtlBudgetDtls = projectdtlbudgetdtlgridinfo;

                    projectAlls.Add(projectAll);

                }
                if (projectAlls != null && projectAlls.Count > 0)
                {
                    foreach (var pro in projectAlls)
                    {
                        var proCode = pro.ProjectMst.FProjCode.Remove(pro.ProjectMst.FProjCode.Length - 6, 6);
                        var qtxm = QtXmDistributeService.Find(p => p.FProjcode == proCode && p.Orgcode == pro.ProjectMst.FDeclarationUnit)?.Data?.FirstOrDefault();
                        var oldCode = string.Empty;
                        if (pro.ProjectMst.PersistentState == PersistentState.Modified)
                        {
                            var project = ProjectMstService.Find(p => p.PhId == pro.ProjectMst.PhId).Data.FirstOrDefault();
                            if(project != null)
                            {
                                oldCode = project.FProjCode.Remove(project.FProjCode.Length - 6, 6);
                            }
                            
                        }
                        //保存数据
                        savedresult = ProjectMstService.SaveProjectMst(pro.ProjectMst, null, null, null, null, null, pro.ProjectDtlBudgetDtls, null);
                        //更新项目项目的引用状态

                        
                        if (pro.ProjectMst.PersistentState == PersistentState.Added)
                        {
                            //var qtxm = QtXmDistributeService.Find(p => p.FProjcode == proCode && p.Orgcode == pro.ProjectMst.FDeclarationUnit)?.Data?.First();
                            if(qtxm != null && qtxm.Isactive != 1)
                            {
                                qtxm.Isactive = 1;
                                qtxm.PersistentState = PersistentState.Modified;
                                QtXmDistributeService.Save<Int64>(qtxm, "");
                            }
                        }
                        else if(pro.ProjectMst.PersistentState == PersistentState.Deleted)
                        {
                            //var qtxm = QtXmDistributeService.Find(p => p.FProjcode == proCode && p.Orgcode == pro.ProjectMst.FDeclarationUnit)?.Data?.First();
                            var projects = ProjectMstService.Find(p => p.FProjCode.StartsWith(proCode) && p.FDeclarationUnit == pro.ProjectMst.FDeclarationUnit).Data;

                            if((projects == null || projects.Count == 0) && qtxm != null && qtxm.Isactive != 0)
                            {
                                qtxm.Isactive = 0;
                                qtxm.PersistentState = PersistentState.Modified;
                                QtXmDistributeService.Save<Int64>(qtxm, "");
                            }
                        }
                        else if(pro.ProjectMst.PersistentState == PersistentState.Modified)
                        {
                            //var qtxm = QtXmDistributeService.Find(p => p.FProjcode == proCode && p.Orgcode == pro.ProjectMst.FDeclarationUnit)?.Data?.First();
                            

                            if(oldCode != pro.ProjectMst.FProjCode)
                            {
                                if (qtxm != null)
                                {
                                    qtxm.Isactive = 1;
                                    qtxm.PersistentState = PersistentState.Modified;
                                    QtXmDistributeService.Save<Int64>(qtxm, "");
                                }

                                var projects = ProjectMstService.Find(p => p.FProjCode.StartsWith(oldCode) && p.FDeclarationUnit == pro.ProjectMst.FDeclarationUnit).Data;
                                qtxm = QtXmDistributeService.Find(p => p.FProjcode == oldCode && p.Orgcode == pro.ProjectMst.FDeclarationUnit)?.Data?.FirstOrDefault();
                                if ((projects == null || projects.Count == 0) && qtxm != null)
                                {
                                    qtxm.Isactive = 0;
                                    qtxm.PersistentState = PersistentState.Modified;
                                    QtXmDistributeService.Save<Int64>(qtxm, "");
                                }
                            }
                        }
                    }
                }
                return DataConverterHelper.SerializeObject(savedresult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }

        }

        /// <summary>
        /// 保存立项数据（含附件）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PostSaveMSYHProject2()
        {
            List<QtAttachmentModel> attachmentModels = new List<QtAttachmentModel>();
            List<QtAttachmentModel> oldattachmentModels = new List<QtAttachmentModel>();
            //具体数据对象
            long projectPhid = 0;
            //判断form表单类型是否正确
            if (!Request.Content.IsMimeMultipartContent())
            {
                var data1 = new
                {
                    Status = ResponseStatus.Error,
                    Msg = "请求数据不是multipart/form-data类型",
                    Data = ""
                };
                return DataConverterHelper.SerializeObject(data1);
            }
            //I6WebAppInfo i6AppInfo = (I6WebAppInfo)HttpContext.Current.Session["NGWebAppInfo"] ?? null;
            //获取AppInfo值 头部信息记录
            var base64EncodedBytes = Convert.FromBase64String(HttpContext.Current.Request.Headers.GetValues("AppInfo").First());
            var jsonText = Encoding.UTF8.GetString(base64EncodedBytes);
            var AppInfo = JsonConvert.DeserializeObject<AppInfoBase>(jsonText);


            //如果路径不存在,创建路径
            var root = System.Web.Hosting.HostingEnvironment.MapPath("~/UpLoadFiles/ProjectMst/");
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string filePath = Path.Combine(root, date);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var multipartMemoryStreamProvider = await Request.Content.ReadAsMultipartAsync();
            var contentsList = multipartMemoryStreamProvider.Contents;

            foreach (var content in contentsList)
            {
                //通过判断fileName是否为空,是否为文件
                if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                {
                    //处理文件名字符串
                    string fileName = content.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                    using (Stream stream = await content.ReadAsStreamAsync())
                    {
                        //文件如果大于100MB  提示不允许
                        if (stream.Length > 104857600)
                        {
                            return DCHelper.ErrorMessage("上传的文件不能大于100MB！");
                        }
                        byte[] bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, bytes.Length);
                        stream.Seek(0, SeekOrigin.Begin);

                        //获取对应文件后缀名
                        string extension = Path.GetExtension(fileName);
                        //获取文件名
                        string b_name = Path.GetFileName(fileName);

                        //修改文件名
                        string newFileName = Guid.NewGuid().ToString("N") + extension;
                        string uploadPath = Path.Combine(filePath, newFileName);

                        //保存文件
                        MemoryStream ms = new MemoryStream(bytes);
                        FileStream fs = new FileStream(uploadPath, FileMode.Create);
                        ms.WriteTo(fs);
                        ms.Close();
                        fs.Close();

                        string b_urlpath = "/UpLoadFiles/ProjectMst/" + date + "/" + newFileName;

                        QtAttachmentModel attachmentModel = new QtAttachmentModel();
                        attachmentModel.BName = b_name;
                        attachmentModel.BSize = decimal.Round((decimal)stream.Length / 1024, 2);
                        attachmentModel.BTable = "XM3_PROJECTMST";
                        attachmentModel.BType = extension;
                        attachmentModel.BUrlpath = b_urlpath;
                        attachmentModel.PersistentState = PersistentState.Added;
                        attachmentModels.Add(attachmentModel);
                    }
                }
                else
                {
                    //获取键值对值,并通过反射获取对象中的属性
                    string key = content.Headers.ContentDisposition.Name.Replace("\"", string.Empty);
                    string value = await content.ReadAsStringAsync();
                    //取项目主键
                    if (key == "PhId")
                    {
                        projectPhid = long.Parse(value);
                    }
                    else if (key == "OldAttachments")
                    {
                        var value2 = JsonConvert.DeserializeObject<List<QtAttachmentModel>>(value);
                        oldattachmentModels = value2;
                    }
                }
            }

            if (projectPhid <= 0)
            {
                return DCHelper.ErrorMessage("项目主表保存有误！");
            }
            try
            {
                SavedResult<long> savedResult = new SavedResult<long>();
                //原有的附件要删除
                IList<QtAttachmentModel> oldAttachments = new List<QtAttachmentModel>();
                oldAttachments = this.QtAttachmentService.Find(t => t.BTable == "XM3_PROJECTMST" && t.RelPhid == projectPhid).Data;
                if (oldAttachments != null && oldAttachments.Count > 0)
                {
                    foreach (var oldAtt in oldAttachments)
                    {
                        oldAtt.PersistentState = PersistentState.Deleted;
                    }
                    this.QtAttachmentService.Save<long>(oldAttachments, "");
                }
                if (attachmentModels != null && attachmentModels.Count > 0)
                {
                    foreach (var att in attachmentModels)
                    {
                        att.RelPhid = projectPhid;
                        att.BTable = "XM3_PROJECTMST";
                        att.PersistentState = PersistentState.Added;
                    }
                }
                if (oldattachmentModels != null && oldattachmentModels.Count > 0)
                {
                    foreach (var oldAtt in oldattachmentModels)
                    {
                        oldAtt.RelPhid = projectPhid;
                        oldAtt.BTable = "XM3_PROJECTMST";
                        oldAtt.PersistentState = PersistentState.Added;
                        attachmentModels.Add(oldAtt);
                    }
                }
                if (attachmentModels != null && attachmentModels.Count > 0)
                {
                    savedResult = this.QtAttachmentService.Save<long>(attachmentModels, "");
                }
                return DataConverterHelper.SerializeObject(savedResult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 获取单个预算项目详情
        /// </summary>
        /// <param name="projectMst"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetMSYHProjectMst([FromUri]ProjectMstRequest projectMst)
        {
            if (projectMst.FProjPhId == 0)
            {
                return DCHelper.ErrorMessage("项目主键不能为空！");
            }

            try
            {
                var dicSysset = new Dictionary<string, object>();
                new CreateCriteria(dicSysset).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                var syssets = QTSysSetService.Find(dicSysset).Data.ToList();

                ProjectAllDataModel projectAllData = new ProjectAllDataModel();
                projectAllData.ProjectMst = ProjectMstService.Find(projectMst.FProjPhId).Data;

                //返回对象增加附件
                if (projectAllData.ProjectMst != null)
                {
                    projectAllData.ProjectAttachments = QtAttachmentService.Find(t => t.BTable == "XM3_PROJECTMST" && t.RelPhid == projectAllData.ProjectMst.PhId).Data.ToList();
                }

                var FDeclarationDept = CorrespondenceSettingsService.GetOrg(projectAllData.ProjectMst.FDeclarationDept);
                if (FDeclarationDept != null)
                {
                    //申报部门代码转名称
                    projectAllData.ProjectMst.FDeclarationDept_EXName = FDeclarationDept.OName;
                }
                var syssetProjectMst = syssets.FindAll(x => x.DicType == "ProjectProper" && x.Orgcode == projectAllData.ProjectMst.FDeclarationUnit && x.TypeCode == projectAllData.ProjectMst.FProjAttr);
                if (syssetProjectMst.Count > 0)
                {
                    //项目属性代码转名称
                    projectAllData.ProjectMst.FProjAttr_EXName = syssetProjectMst[0].TypeName;
                }
                var syssetProjectMst2 = syssets.FindAll(x => x.DicType == "TimeLimit" && x.Orgcode == projectAllData.ProjectMst.FDeclarationUnit && x.TypeCode == projectAllData.ProjectMst.FDuration);
                if (syssetProjectMst2.Count > 0)
                {
                    //存续期限代码转名称
                    projectAllData.ProjectMst.FDuration_EXName = syssetProjectMst2[0].TypeName;
                }

                var syssetProjectMst3 = syssets.FindAll(x => x.DicType == "ProjectLevel" && x.Orgcode == projectAllData.ProjectMst.FDeclarationUnit && x.TypeCode == projectAllData.ProjectMst.FLevel);
                if (syssetProjectMst3.Count > 0)
                {
                    //项目级别代码转名称
                    projectAllData.ProjectMst.FLevel_EXName = syssetProjectMst3[0].TypeName;
                }

                projectAllData.ProjectDtlImplPlans = ProjectMstService.FindProjectDtlImplPlanByForeignKey(projectMst.FProjPhId).Data.ToList();
                projectAllData.ProjectDtlTextContents = ProjectMstService.FindProjectDtlTextContentByForeignKey(projectMst.FProjPhId).Data[0];
                projectAllData.ProjectDtlFundAppls = ProjectMstService.FindProjectDtlFundApplByForeignKey(projectMst.FProjPhId).Data.ToList();
                projectAllData.ProjectDtlBudgetDtls = ProjectMstService.FindProjectDtlBudgetDtlByForeignKey(projectMst.FProjPhId).Data.ToList();
                projectAllData.ProjectDtlBudgetDtls.Sort((ProjectDtlBudgetDtlModel dtl1, ProjectDtlBudgetDtlModel dtl2) => dtl1.FDtlCode.CompareTo(dtl2.FDtlCode));
                foreach (var ProjectDtlBudgetDtl in projectAllData.ProjectDtlBudgetDtls)
                {
                    //支付方式代码转名称
                    var syssetProjectDtlBudgetDtl = syssets.FindAll(x => x.DicType == "PayMethodTwo" && x.Orgcode == projectAllData.ProjectMst.FDeclarationUnit && x.TypeCode == ProjectDtlBudgetDtl.FPaymentMethod);
                    if (syssetProjectDtlBudgetDtl.Count > 0)
                    {
                        ProjectDtlBudgetDtl.FPaymentMethod_EXName = syssetProjectDtlBudgetDtl[0].TypeName;
                    }

                }
                projectAllData.ProjectDtlPurchaseDtls = ProjectMstService.FindProjectDtlPurchaseDtlByForeignKey(projectMst.FProjPhId).Data.ToList();
                projectAllData.ProjectDtlPurDtl4SOFs = ProjectMstService.FindProjectDtlPurDtl4SOFByForeignKey(projectMst.FProjPhId).Data.ToList();
                projectAllData.ProjectDtlPerformTargets = ProjectMstService.FindProjectDtlPerformTargetByForeignKey(projectMst.FProjPhId).Data.ToList();
                OrganizeModel organize = this.BudgetMstService.GetOrganizeByCode(projectAllData.ProjectMst.FDeclarationUnit);
                if (organize != null)
                {
                    projectAllData.ProjectDtlPerformTargets = ProjectMstService.GetNewProPerformTargets(projectAllData.ProjectDtlPerformTargets, projectAllData.ProjectMst.FPerformType, organize.PhId, organize.OCode);
                }
                return DataConverterHelper.SerializeObject(projectAllData);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 根据主键集合修改作废状态(修改删除标志)
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostMSYHCancetProjectList([FromBody]Model.Request.BaseListModel paramters)
        {
            try
            {
                if (paramters.fPhIdList == null || paramters.fPhIdList.Count() < 1)
                {
                    return DCHelper.ErrorMessage("传递的单据集合有误！");
                }
                List<long> fCodes = new List<long>();
                for (int i = 0; i < paramters.fPhIdList.Count(); i++)
                {
                    fCodes.Add(long.Parse(paramters.fPhIdList[i]));
                }
                var result = this.ProjectMstService.PostCancetProjectList(fCodes);
                return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 项目立项数据转项目立项草案
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostMSYHVerify([FromBody]Model.Request.BaseListModel paramters)
        {
            if (paramters.fPhIdList == null || paramters.fPhIdList.Count() < 1)
            {
                return DCHelper.ErrorMessage("传递的单据集合有误！");
            }
            if (paramters.UserId == 0)
            {
                return DCHelper.ErrorMessage("用户信息不能为空！");
            }
            try
            {

            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
            //立项与预立项的单据转立项要进行进度控制的判断
            List<long> phids = new List<long>();
            foreach (var phid in paramters.fPhIdList)
            {
                phids.Add(long.Parse(phid));
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Clear();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<List<long>>.In("PhId", phids))
                .Add(ORMRestrictions<int>.Eq("FLifeCycle", 0));
            var paymentList = this.ProjectMstService.Find(dic).Data;
            if (paymentList != null && paymentList.Count > 0)
            {
                List<string> budList = paymentList.Select(t => t.FBudgetDept).Distinct().ToList();
                List<string> yearList = paymentList.Select(t => t.FYear).Distinct().ToList();
                if (budList != null && budList.Count > 0 && yearList != null && yearList.Count > 0)
                {
                    var budProcess = this.BudgetProcessCtrlService.Find(t => budList.Contains(t.FDeptCode) && yearList.Contains(t.FYear)).Data;
                    if (budProcess != null && budProcess.Count > 0)
                    {
                        foreach (var payM in paymentList)
                        {
                            var process = budProcess.ToList().Find(t => t.FDeptCode == payM.FBudgetDept && t.FYear == payM.FYear);
                            if (process != null && process.FProcessStatus != "1")
                            {
                                throw new Exception("有单据的预算部门的进度已不在年初申报，因此无法上报！");
                            }
                        }
                    }
                }
            }
            IList<ProjectMstModel> MstList = new List<ProjectMstModel>();

            MstList = ProjectMstService.Find(t => phids.Contains(t.PhId)).Data;

            foreach (var mst in MstList)
            {
                if (mst.FProjStatus == 1 && mst.FApproveStatus == "2")
                {
                    mst.FProjStatus = 2;
                    mst.FApproveStatus = "1";
                    mst.FApproveDate = DateTime.Now;
                    mst.FApprover = base.AppInfo.UserId;
                    mst.PersistentState = PersistentState.Modified;
                    MstList.Add(mst);

                }
                else
                {

                }
            }

            var SuccessNum = 0;
            var FailNum = 0;
            foreach (var fPhId in paramters.fPhIdList)
            {
                var mst = ProjectMstService.Find(long.Parse(fPhId)).Data;
                if (mst.FProjStatus == 1 && mst.FApproveStatus == "2")
                {
                    mst.FProjStatus = 2;
                    mst.FApproveStatus = "1";
                    mst.FApproveDate = DateTime.Now;
                    mst.FApprover = base.AppInfo.UserId;
                    mst.PersistentState = PersistentState.Modified;
                    MstList.Add(mst);
                    SuccessNum++;
                }
                else
                {
                    FailNum++;
                }
            }
            if (SuccessNum > 0)
            {
                try
                {
                    ProjectMstService.Save<Int64>(MstList, "");
                }
                catch (Exception ex)
                {
                    return DCHelper.ErrorMessage(ex.Message);
                }
            }
            var result = new
            {
                SuccessNum = SuccessNum,
                FailNum = FailNum
            };
            return DataConverterHelper.SerializeObject(result);
        }


        /// <summary>
        /// 项目同步数据到老G6H（点击生成预算获取生成草案都要触发）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string PostMSYHAddData([FromBody]Model.Request.BaseListModel paramters)
        {
            if (paramters.fPhIdList == null || paramters.fPhIdList.Count() < 1)
            {
                return DCHelper.ErrorMessage("传递的单据集合有误！");
            }
            string result = ProjectMstService.AddData(paramters.fPhIdList);
            if (result == "")
            {
                return DCHelper.SuccessMessage("同步成功");
            }
            else
            {
                return result;
            }
        }


        /// <summary>
        /// 根据项目主键集合批量生成预算
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string PostSaveMSHYBudgetMst([FromUri]Model.Request.BaseListModel param)
        {
            if (param.FPhids == null || param.FPhids.Count <= 0)
            {
                return DCHelper.ErrorMessage("单据主键不能为空！");
            }
            try
            {
                //先把这些主键集合对应的项目数据都查找出来（先进行数据验证）
                List<ProjectMstModel> allProjects = new List<ProjectMstModel>();
                allProjects = this.ProjectMstService.Find(t => param.FPhids.Contains(t.PhId)).Data.ToList();
                //单据的会议材料集合
                IList<ProjectDtlTextContentModel> allTextContents = new List<ProjectDtlTextContentModel>();
                allTextContents = this.ProjectMstService.GetProjectDtlTextContents(param.FPhids);
                if (allProjects != null && allProjects.Count > 0)
                {
                    foreach (var pro in allProjects)
                    {
                        if (pro.FApproveStatus == "3" && pro.FProjStatus != 3)
                        {
                            if (allTextContents != null && allTextContents.Count > 0)
                            {
                                if (allTextContents.ToList().Find(t => t.MstPhid == pro.PhId) != null)
                                {
                                    if (allTextContents.ToList().Find(t => t.MstPhid == pro.PhId).FResolution != "0")
                                    {
                                        return DCHelper.ErrorMessage("选中的单据会议决议未通过，不能生成预算！");
                                    }
                                }
                                //else
                                //{
                                //    return DCHelper.ErrorMessage("选中的单据暂无项目材料信息，不能生成预算！");
                                //}
                            }
                            //else
                            //{
                            //    return DCHelper.ErrorMessage("选中的单据暂无项目材料信息，不能生成预算！");
                            //}
                            continue;
                        }
                        else
                        {
                            return DCHelper.ErrorMessage("只有审批通过的项目立项与项目草案单据可以生成预算！");
                        }
                    }
                    SavedResult<long> savedResult = new SavedResult<long>();
                    savedResult = this.ProjectMstService.SaveMSHYBudgetMst(param.FPhids);
                    return DataConverterHelper.SerializeObject(savedResult);
                }
                else
                {
                    return DCHelper.ErrorMessage("查询的单据不存在！");
                }
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }

        }

        /// <summary>
        /// 保存人员分摊信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string PostMSYHPersonnels([FromBody]ProjectAllDataRequest param)
        {
            if (param == null || param.ProjectDtlPersonnels == null || param.ProjectDtlPersonnels.Count <= 0)
            {
                return DCHelper.ErrorMessage("传递的参数不能为空！");
            }
            if (param.FPhid == 0)
            {
                return DCHelper.ErrorMessage("对应的主表主键信息不能为空！");
            }
            try
            {
                //先进行数据验证（人员分摊的总金额应该跟项目总额相同）
                foreach (var per in param.ProjectDtlPersonnels)
                {
                    if (per.PhId == 0)
                    {
                        per.PersistentState = PersistentState.Added;
                    }
                    else
                    {
                        if (per.PersistentState != PersistentState.Deleted)
                        {
                            per.PersistentState = PersistentState.Modified;
                        }
                    }
                }
                //项目信息
                var pros = this.ProjectMstService.Find(t => t.PhId == param.FPhid).Data;
                if (pros != null && pros.Count > 0)
                {
                    if (pros[0].FIsShare != 1 || pros[0].FProjStatus == 3)
                    {
                        return DCHelper.ErrorMessage("此单据无权进行额度分摊！");
                    }
                    SavedResult<long> savedResult = new SavedResult<long>();
                    if (pros[0].FProjAmount == param.ProjectDtlPersonnels.ToList().FindAll(t => t.PersistentState != PersistentState.Deleted).Sum(t => t.FAmount))
                    {
                        savedResult = this.ProjectMstService.SaveMSYHPersonnels(param.ProjectDtlPersonnels, param.FPhid);
                        return DataConverterHelper.SerializeObject(savedResult);
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("人员分摊总额应与项目总额相等！");
                    }
                }
                else
                {
                    return DCHelper.ErrorMessage("项目信息查找失败！");
                }
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 保存项目内容信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string PostMSYHTextContent([FromBody]ProjectAllDataRequest param)
        {
            if (param == null || param.ProjectDtlTextContents == null)
            {
                return DCHelper.ErrorMessage("传递的数据不能为空！");
            }
            if (string.IsNullOrEmpty(param.Year))
            {
                return DCHelper.ErrorMessage("年份信息不能为空！");
            }
            if (string.IsNullOrEmpty(param.OrgCode))
            {
                return DCHelper.ErrorMessage("组织信息不能为空！");
            }
            if (param.UserId == 0)
            {
                return DCHelper.ErrorMessage("申报人信息不能为空！");
            }
            try
            {
                //数据调整
                ProjectDtlTextContentModel projectDtlTextContent = new ProjectDtlTextContentModel();
                projectDtlTextContent = param.ProjectDtlTextContents;
                projectDtlTextContent.FYear = param.Year;
                projectDtlTextContent.FDeclarationUnit = param.OrgCode;
                projectDtlTextContent.FDeclarerId = param.UserId;
                SavedResult<long> savedResult = new SavedResult<long>();
                savedResult = this.ProjectMstService.SaveMSYHTextContent(projectDtlTextContent);
                return DataConverterHelper.SerializeObject(savedResult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 获取民生银行首页显示数量
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetMSYHProjectDataCount([FromUri]Model.Request.BaseListModel paramters)
        {
            // 待上报预算
            Dictionary<string, object> dicWhere1 = new Dictionary<string, object>();
            new CreateCriteria(dicWhere1).Add(ORMRestrictions<string>.Eq("FApproveStatus", "3"))
                .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
                .Add(ORMRestrictions<Int64>.Eq("FDeclarerId", paramters.UserId))
                .Add(ORMRestrictions<string>.Eq("FDeclarationUnit", paramters.OrgCode));
            var ProjectCount = ProjectMstService.Find(dicWhere1).Data.Count;

            //待上报调整预算
            var BudgetCount = BudgetMstService.Find(dicWhere1).Data.Count;

            //待审批任务
            BillRequestModel billRequest = new BillRequestModel();
            billRequest.Uid = paramters.UserId;
            billRequest.Orgid = paramters.OrgId;
            billRequest.Year = paramters.Year;
            //var YNum = 0;//已审数量
            var Approval = 0;//待审数量
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
                        //int total = 0;
                        //List<AppvalRecordVo> recordVos = GAppvalRecordService.GetDoneRecordList(billRequest, out total);
                        int total2 = 0;
                        List<AppvalRecordVo> recordVos2 = GAppvalRecordService.GetUnDoRecordList(billRequest, out total2);

                        //YNum += total;
                        Approval += total2;
                    }
                }

            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage(e.Message);
            }
            var data = new
            {
                Status = ResponseStatus.Success,
                Msg = "获取成功",
                ProjectCount = ProjectCount,
                BudgetCount = BudgetCount,
                Approval = Approval
            };
            return DataConverterHelper.SerializeObject(data);
        }


        /// <summary>
        /// 根据组织，年份与用户id获取对应的内容信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string GetMSYHTextContent([FromUri]Model.Request.BaseListModel param)
        {
            if (string.IsNullOrEmpty(param.OrgCode))
            {
                return DCHelper.ErrorMessage("组织编码不能为空！");
            }
            if (string.IsNullOrEmpty(param.Year))
            {
                return DCHelper.ErrorMessage("年度信息不能为空！");
            }
            if (param.UserId == 0)
            {
                return DCHelper.ErrorMessage("用户信息不能为空！");
            }
            try
            {
                var result = this.ProjectMstService.GetMSYHTextContent(param.OrgCode, param.Year, param.UserId);
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "获取成功！",
                    Data = result
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 获取单个组织的关联人员维护信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetMSYHPersonName([FromUri]Model.Request.BaseListModel param)
        {
            if (param.FPhid == 0)
            {
                return DCHelper.ErrorMessage("单据主键不能为空！");
            }
            try
            {
                var result = this.ProjectMstService.GetMSYHPersonNames(param.FPhid);
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "获取成功！",
                    Data = result
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 保存维护人员集合
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string PostMSYHPersonName([FromBody]ProjectAllDataRequest param)
        {
            if (param.ProjectDtlPersonNames == null || param.ProjectDtlPersonNames.Count <= 0)
            {
                return DCHelper.ErrorMessage("维护人员信息名单不能为空！");
            }
            if (param.FPhid == 0)
            {
                return DCHelper.ErrorMessage("单据主键不能为空！");
            }
            try
            {
                List<ProjectDtlPersonNameModel> projectDtlPersonNames = new List<ProjectDtlPersonNameModel>();
                SavedResult<long> savedResult = new SavedResult<long>();
                foreach (var pro in param.ProjectDtlPersonNames)
                {
                    pro.MstPhid = param.FPhid;
                    if (pro.IsFix == 1)
                    {
                        pro.IsRelation = 1;
                    }
                    if (pro.IsRelation == 1)
                    {
                        if (pro.PhId == 0)
                        {
                            pro.PersistentState = PersistentState.Added;
                        }
                        else
                        {
                            pro.PersistentState = PersistentState.Modified;
                        }
                        projectDtlPersonNames.Add(pro);
                    }
                    else
                    {
                        if (pro.PhId != 0)
                        {
                            pro.PersistentState = PersistentState.Deleted;
                            projectDtlPersonNames.Add(pro);
                        }
                    }
                }
                savedResult = this.ProjectMstService.SaveMSYHPersonNames(projectDtlPersonNames, param.FPhid);
                return DataConverterHelper.SerializeObject(savedResult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 根据选中的项目分发数据导出模板
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string PostExportXMData([FromBody]Model.Request.BaseListModel paramters)
        {
            if (paramters.fPhIdList != null && paramters.fPhIdList.Count() > 0)
            {

            }
            else
            {
                return DCHelper.ErrorMessage("项目分发主键集合不能为空！");
            }
            var data = QtXmDistributeService.Find(x => paramters.fPhIdList.Contains(x.PhId.ToString()), "FProjcode").Data.ToList();
            if (data != null && data.Count > 0)
            {
                return ProjectMstService.ExportXMData(data);
            }
            else
            {
                return DCHelper.ErrorMessage("找不到对应项目分发的数据！");
            }

        }

        ///// <summary>
        ///// 模板导入用户数据
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<string> PostImportXMData()
        //{
        //    try
        //    {
        //        //判断form表单类型是否正确
        //        if (!Request.Content.IsMimeMultipartContent())
        //        {
        //            var data = new
        //            {
        //                Status = ResponseStatus.Error,
        //                Msg = "请求数据不是multipart/form-data类型"
        //            };
        //            return DataConverterHelper.SerializeObject(data);
        //        }

        //        //I6WebAppInfo i6AppInfo = (I6WebAppInfo)HttpContext.Current.Session["NGWebAppInfo"] ?? null;
        //        //获取AppInfo值 头部信息记录
        //        var base64EncodedBytes = Convert.FromBase64String(HttpContext.Current.Request.Headers.GetValues("AppInfo").First());
        //        var jsonText = Encoding.UTF8.GetString(base64EncodedBytes);
        //        var AppInfo = JsonConvert.DeserializeObject<AppInfoBase>(jsonText);

        //        //如果路径不存在,创建路径
        //        var root = System.Web.Hosting.HostingEnvironment.MapPath("~/UpLoadFiles/XM/");
        //        string filePath = root;
        //        if (!Directory.Exists(filePath))
        //        {
        //            Directory.CreateDirectory(filePath);
        //        }

        //        var multipartMemoryStreamProvider = await Request.Content.ReadAsMultipartAsync();
        //        var contentsList = multipartMemoryStreamProvider.Contents;

        //        string uploadPath = "";
        //        string extension = "";
        //        //保存导入的模板文件
        //        foreach (var content in contentsList)
        //        {
        //            //通过判断fileName是否为空,是否为文件
        //            if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
        //            {
        //                //处理文件名字符串
        //                string fileName = content.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
        //                using (Stream stream = await content.ReadAsStreamAsync())
        //                {
        //                    byte[] bytes = new byte[stream.Length];
        //                    stream.Read(bytes, 0, bytes.Length);
        //                    stream.Seek(0, SeekOrigin.Begin);

        //                    //获取对应文件后缀名
        //                    extension = Path.GetExtension(fileName);
        //                    //获取文件名
        //                    string b_name = Path.GetFileName(fileName);

        //                    //修改文件名
        //                    string newFileName = Guid.NewGuid().ToString("N") + extension;
        //                    uploadPath = Path.Combine(filePath, newFileName);

        //                    //保存文件
        //                    MemoryStream ms = new MemoryStream(bytes);
        //                    FileStream fs = new FileStream(uploadPath, FileMode.Create);
        //                    ms.WriteTo(fs);
        //                    ms.Close();
        //                    fs.Close();
        //                }

        //            }
        //        }


        //        var data1 = new
        //        {
        //            Status = ResponseStatus.Success,
        //            Msg = "操作员编码不重复！",
        //            UploadPath = uploadPath,
        //            IsRepeat = 0
        //        };
        //        return DataConverterHelper.SerializeObject(data1);

        //    }
        //    catch (Exception e)
        //    {
        //        return DCHelper.ErrorMessage("模板导入失败！");
        //    }

        //}

        /// <summary>
        /// 模板导入用户数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string PostImportXMData()
        {
            ImportXMDataRequest request = new ImportXMDataRequest();
            var form = HttpContext.Current.Request.Form;
            for (int i = 0;i < form.AllKeys.Count(); i++)
            {
                switch (form.AllKeys[i])
                {
                    case "UserId": 
                        request.UserId = long.Parse(HttpContext.Current.Request.Form[i]); break;
                    case "FYear":
                        request.FYear = int.Parse(HttpContext.Current.Request.Form[i]); break;
                    case "OrgId":
                        request.OrgId = long.Parse(HttpContext.Current.Request.Form[i]); break;
                    case "OrgCode":
                        request.OrgCode = HttpContext.Current.Request.Form[i]; break;
                    default:break;
                }
            }

            var now = DateTime.Now;
            try
            {
                if (request.UserId == 0)
                {
                    return DataConverterHelper.SerializeObject(new
                    {
                        Status = ResponseStatus.Error,
                        Msg = "用户信息为空"
                    });
                }

                if (request.OrgId == 0)
                {
                    return DataConverterHelper.SerializeObject(new
                    {
                        Status = ResponseStatus.Error,
                        Msg = "机构信息为空"
                    });
                }

                if (request.FYear == 0)
                {
                    return DataConverterHelper.SerializeObject(new
                    {
                        Status = ResponseStatus.Error,
                        Msg = "年份不正确"
                    });
                }


                var files = HttpContext.Current.Request.Files;
                if (files == null || files.Count == 0)
                {
                    return DataConverterHelper.SerializeObject(new
                    {
                        Status = ResponseStatus.Error,
                        Msg = "上传文件为空"
                    });
                }

                var file = files[0];
                if (file.ContentType != "application/vnd.ms-excel"
                && file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                && file.ContentType != "application/octet-stream")
                {
                    return DataConverterHelper.SerializeObject(new
                    {
                        Status = ResponseStatus.Error,
                        Msg = "文件格式不正确"
                    });
                }
                ISheet sheet;

                if (file.ContentType == "application/vnd.ms-excel"){
                    XSSFWorkbook wk = new XSSFWorkbook(file.InputStream);
                    sheet = wk.GetSheetAt(0);
                }
                else
                {
                    HSSFWorkbook wk = new HSSFWorkbook(file.InputStream);
                    sheet = wk.GetSheetAt(0);
                }
                /*
                 * 第一行为标题行，第二行为注意事项，第三个为小标题，第四行开始为正式数据（ 科目名称（选填）	支出分项名称（选填））
                 * 1.导入数据需要在日志上记录
                 * 2.项目编码对应的项目名称和业务条线的正确
                 * 3.下拉项的中文名称是否能找到对应的编码
                 * 4.单文件上传，不需要多文件上传
                 */

                var projectMstUploads = new List<ProjectMstUpload>();
                //循环读取数据
                for (int i = 3; i <= sheet.LastRowNum; i++)
                {
                    try
                    {
                        var row = sheet.GetRow(i);

                        var fProjCode = row.GetCell(0)?.ToString()?.Trim() ?? string.Empty;//项目编码
                        var fProjName = row.GetCell(1)?.ToString()?.Trim() ?? string.Empty;//项目名称
                        long.TryParse(row.GetCell(6)?.ToString()?.Trim() ?? "0", out long fProPrice);//项目金额
                        var fBusinessName = row.GetCell(3)?.ToString()?.Trim() ?? string.Empty;//业务条线
                        var fBudgetDept = row.GetCell(4)?.ToString()?.Trim() ?? string.Empty;//费用归属
                        var fName = row.GetCell(5)?.ToString()?.Trim() ?? string.Empty;//费用说明
                        long.TryParse(row.GetCell(6)?.ToString()?.Trim() ?? "0", out long fAmount);//费用金额
                        //var fBudgetAccounts = row.GetCell(7)?.ToString()?.Trim() ?? string.Empty;//科目名称
                        //var fSubitemName = row.GetCell(8)?.ToString()?.Trim() ?? string.Empty;//支出分项名称
                        var fIsApply = row.GetCell(7)?.ToString()?.Trim() ?? string.Empty;//是否申请补助
                        var fIsPurchase = row.GetCell(8)?.ToString()?.Trim() ?? string.Empty;//是否集中采购
                        var fIsReport = row.GetCell(9)?.ToString()?.Trim() ?? string.Empty;//是否必须签报列支
                        var fIsResolution = row.GetCell(10)?.ToString()?.Trim() ?? string.Empty;//是否集体决议
                        //var fIsShare = row.GetCell(13)?.ToString()?.Trim() ?? string.Empty;//是否个人额度分摊

                        var projectMstUpload = new ProjectMstUpload
                        {
                            LineNum = i + 1,
                            FProjCode = fProjCode,
                            FProjName = fProjName,
                            FProPrice = fProPrice,
                            FBusinessName = fBusinessName,
                            FBudgetDept = fBudgetDept,
                            FName = fName,
                            FAmount = fAmount,
                            //FBudgetAccounts = fBudgetAccounts,
                            //FSubitemName = fBusinessName,
                            FIsApply = fIsApply,
                            FIsPurchase = fIsPurchase,
                            FIsReport = fIsReport,
                            FIsResolution = fIsResolution,
                            //FIsShare = fIsShare
                        };
                        #region 数据校验
                        //数据填写不完整
                        if (string.IsNullOrEmpty(fProjCode)
                            || string.IsNullOrEmpty(fProjName)
                            || string.IsNullOrEmpty(fBusinessName)
                            || string.IsNullOrEmpty(fBudgetDept)
                            || string.IsNullOrEmpty(fName)
                            || string.IsNullOrEmpty(fIsApply)
                            || string.IsNullOrEmpty(fIsPurchase)
                            || string.IsNullOrEmpty(fIsReport)
                            || string.IsNullOrEmpty(fIsResolution)
                            )
                        {
                            throw new Exception($"第{i + 1}行数据上传失败。(必填数据没有填写完整)");
                        }

                        //验证金额是否是数字
                        try
                        {
                            long.Parse(row.GetCell(6)?.ToString()?.Trim());
                        }
                        catch
                        {
                            throw new Exception($"第{i + 1}行数据上传失败。(【费用金额】不是数字类型)");
                        }

                        //四个选择项需要输入 是/否
                        if (fIsApply != "是" && fIsApply != "否")
                        {
                            throw new Exception($"第{i + 1}行数据上传失败。(【是否申请补助】只能输入 是/否)");
                        }
                        if (fIsPurchase != "是" && fIsPurchase != "否")
                        {
                            throw new Exception($"第{i + 1}行数据上传失败。(【是否集中采购】只能输入 是/否)");
                        }
                        if (fIsReport != "是" && fIsReport != "否")
                        {
                            throw new Exception($"第{i + 1}行数据上传失败。(【是否必须签报列支】只能输入 是/否)");
                        }
                        if (fIsResolution != "是" && fIsResolution != "否")
                        {
                            throw new Exception($"第{i + 1}行数据上传失败。(【是否集体决议】只能输入 是/否)");
                        }


                        //验证项目编码是否正确
                        var qtXmDistribute = QtXmDistributeService.Find(p => p.FProjcode == fProjCode && p.Orgid
                         == request.OrgId && p.IfUse == 1).Data.ToList();
                        if (qtXmDistribute == null || qtXmDistribute.Count == 0)
                        {
                            throw new Exception($"第{i + 1}行数据上传失败。(【项目编码】不正确)");
                        }

                        if (qtXmDistribute.First().FProjname != fProjName)
                        {
                            throw new Exception($"第{i + 1}行数据上传失败。(【项目名称】不正确)");
                        }

                        //验证业务条线是否正确
                        var business = QTSysSetService.Find(p => p.Orgid == request.OrgId
                            && p.TypeCode == qtXmDistribute.First().FBusiness
                            && p.DicType == "Business");

                        if (business.Data == null || business.Data.Count == 0 || business.Data.First().TypeName != fBusinessName)
                        {
                            throw new Exception($"第{i + 1}行数据上传失败。(【业务条线】不正确)");
                        }

                        projectMstUpload.FBusinessCode = business.Data.First().TypeCode;

                        //验证费用归属
                        var depts = CorrespondenceSettingsService.GetDeptByUnit(request.OrgId, request.UserId);
                        if (depts == null
                            || depts.Count == 0
                            || !depts.ToList().Exists(p => p.OName == fBudgetDept))
                        {
                            throw new Exception($"第{i + 1}行数据上传失败。(【费用归属】不正确)");
                        }
                        projectMstUpload.FBudgetDeptCode = depts.Where(p => p.OName == fBudgetDept).First().OCode;

                        ////科目名称（选填）fBudgetAccounts	
                        //if (fBudgetAccounts != string.Empty)
                        //{
                        //    var budgetAccounts = BudgetAccountsService.Find(p => p.KMMC == fBudgetAccounts);

                        //    if (budgetAccounts.Data == null || budgetAccounts.Data.Count == 0)
                        //    {
                        //        throw new Exception($"第{i + 1}行数据上传失败。(【科目名称】不正确)");
                        //    }

                        //    projectMstUpload.FBudgetAccountsCode = budgetAccounts.Data.First().KMDM;
                        //}

                        ////支出分项名称（选填）fSubitemName
                        //if (fSubitemName != string.Empty)
                        //{
                        //    var fSubitemBusiness = QTSysSetService.Find(p => p.Orgid == request.OrgId
                        //    && p.TypeName == fSubitemName
                        //    && p.DicType == "ZcfxName");

                        //    if (fSubitemBusiness.Data == null || fSubitemBusiness.Data.Count == 0)
                        //    {
                        //        throw new Exception($"第{i + 1}行数据上传失败。(【支出分项名称】不正确)");
                        //    }

                        //    projectMstUpload.FSubitemCode = fSubitemBusiness.Data.First().TypeCode;
                        //}

                        #endregion

                        projectMstUploads.Add(projectMstUpload);
                    }
                    catch (Exception ex)
                    {

                        //写入日志
                        Logger.Error($"时间：{now}  用户：{request.UserId}  异常：{ex.Message}");
                    }
                }

                //没有成功的数据
                if (projectMstUploads.Count == 0)
                {
                    return DCHelper.ErrorMessage("模板导入失败！");
                }

                var allList = new List<ProjectAllDataModel>();

                var allCodes = ProjectMstService.Find(p => p.PhId != 0 && p.FDeleteMark == 0)?.Data?.Select(p => p.FProjCode)?.ToList() ?? new List<string>();

                //导入正确的数据
                var projectMstUploadsGroup = projectMstUploads.GroupBy(p => new { p.FProjCode, p.FBudgetDeptCode });
                foreach (var projectMstUploadGroup in projectMstUploadsGroup)
                {
                    var projectAllData = new ProjectAllDataModel();
                    //主表信息所有数据都有
                    var baseData = projectMstUploadGroup.First();

                    var projectMst = new ProjectMstModel
                    {
                        FYear = request.FYear.ToString(),
                        FDeclarationUnit = request.OrgCode,
                        FProjCode = baseData.FProjCode,
                        FProjName = baseData.FProjName,
                        FProjAmount = projectMstUploadGroup.Sum(p => p.FAmount),
                        FBudgetAmount = projectMstUploadGroup.Sum(p => p.FAmount),
                        FBusinessName = baseData.FBusinessName,
                        FBudgetDept = baseData.FBudgetDeptCode,
                        FProjStatus = 1,
                        FApproveStatus = "1",
                        FIsApply = baseData.FIsApply == "是" ? (byte)1 : (byte)0,
                        FIsPurchase = baseData.FIsPurchase == "是" ? (byte)1 : (byte)0,
                        FIsReport = baseData.FIsReport == "是" ? (byte)1 : (byte)0,
                        FIsResolution = baseData.FIsResolution == "是" ? (byte)1 : (byte)0,
                        FBusinessCode = baseData.FBusinessCode,
                        PersistentState = PersistentState.Added 
                    };

                    var projectdtlList = projectMstUploadGroup.Select(p => new ProjectDtlBudgetDtlModel
                    {
                        FName = p.FName,
                        FAmount = p.FAmount,
                        FBudgetAccounts = p.FBudgetAccountsCode,
                        FSubitemCode = p.FSubitemCode,
                        FAmountAfterEdit = p.FAmount,
                        PersistentState = PersistentState.Added
                    });

                    //没有进度状态
                    var processStatus = BudgetProcessCtrlService.FindBudgetProcessCtrl(projectMst.FDeclarationUnit, projectMst.FBudgetDept, projectMst.FYear);
                    if (processStatus == "1")
                    {
                        projectMst.FType = "c";
                        projectMst.FVerNo = "0001";
                    }
                    else
                    {
                        projectMstUploadGroup.ToList().ForEach(p =>
                        {
                            //写入日志
                            Logger.Error($"时间：{now}  用户：{request.UserId}  异常：第{p.LineNum}行数据上传失败。(此组织的进度已不在年初，无法修改年初数据！)");
                        });
                        continue;
                    }

                    //申报部门
                    projectMst.FDeclarationDept = ProjectMstService.GetDefaultDept(request.UserId);
                    //申报人
                    projectMst.FDeclarerId = request.UserId;

                    string maxCode = "";
                    if (allCodes != null && allCodes.Count > 0)
                    {
                        maxCode = allCodes.ToList().FindAll(p => p.StartsWith(projectMst.FProjCode)) == null
                            ? "" : allCodes.ToList().FindAll(p => p.StartsWith(projectMst.FProjCode)).Max();
                    }

                    var projCode = string.Empty;
                    //分发的编码再加6位流水线号
                    if (string.IsNullOrEmpty(maxCode))
                    {
                        projCode = projectMst.FProjCode + "000001";

                    }
                    else
                    {
                        projCode = projectMst.FProjCode + string.Format("{0:D6}", int.Parse(maxCode.Substring(maxCode.Length - 6, 6)) + 1);
                        allCodes.Add(projCode);
                    }


                    allCodes.Add(projCode);
                    projectMst.FProjCode = projCode;

                    var dtlCode = "";
                    var dtlName = "";
                    if (projectdtlList.Count() > 0)
                    {
                        projectMst.FBudgetAmount = projectdtlList.Sum(p => p.FAmount);
                        projectMst.FProjAmount = projectdtlList.Sum(p => p.FAmount);

                        var alldtlCodes = projectdtlList.Where(p => !string.IsNullOrEmpty(p.FDtlCode)).Select(p => p.FDtlCode).ToList();

                        foreach (var projectdtl in projectdtlList)
                        {
                            dtlCode = projectdtl.FDtlCode;
                            dtlName = projectdtl.FName;
                            if (string.IsNullOrEmpty(dtlCode))
                            {
                                if (alldtlCodes != null && alldtlCodes.Count > 0)
                                {
                                    projectdtl.FDtlCode = projCode + string.Format("{0:D4}", int.Parse(alldtlCodes.Max().Substring(alldtlCodes.Max().Length - 4, 4)) + 1);
                                }
                                else
                                {
                                    projectdtl.FDtlCode = projCode + string.Format("{0:D4}", 1);
                                }
                            }
                            dtlCode = projectdtl.FDtlCode;
                            alldtlCodes.Add(dtlCode);
                        }
                        //projectAllData.ProjectMst = projectMst;
                        //projectAllData.ProjectDtlBudgetDtls = projectdtlList.ToList();
                        //allList.Add(projectAllData);
                        ProjectMstService.SaveProjectMst(projectMst, null, null, null, null, null, projectdtlList.ToList(), null);
                        var proCode = projectMst.FProjCode.Remove(projectMst.FProjCode.Length - 6, 6);
                        var qtxm = QtXmDistributeService.Find(p => p.FProjcode == proCode && p.Orgcode == projectMst.FDeclarationUnit)?.Data?.FirstOrDefault();
                        if (qtxm != null && qtxm.Isactive != 1)
                        {
                            qtxm.Isactive = 1;
                            qtxm.PersistentState = PersistentState.Modified;
                            QtXmDistributeService.Save<Int64>(qtxm, "");
                        }
                    }


                }
                //return "模板导入成功";
                return DataConverterHelper.SerializeObject(new
                {
                    Staus = ResponseStatus.Success,
                    Msg = "模板导入成功"
                });

            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage("模板导入失败！");
            }

        }


        /// <summary>
        /// 下载模板
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostModelExcel([FromBody]ProjectMstExcelModel model)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + @"\\DownLoadFiles\\ProjectMst";
            FileStream modelFs = File.OpenRead(path + "\\model.xlsx");

            var book = new XSSFWorkbook(modelFs);
            //测试新建一个sheet
            var sheet = book.GetSheet("支出预算");
            var list = model.model;
            for (int i = 0; i < list.Count; i++)
            {

                var row = sheet.GetRow(i + 3);

                row.GetCell(0).SetCellValue(list[i].ProjectCode ?? string.Empty);//项目编码
                row.GetCell(1).SetCellValue(list[i].ProjectName ?? string.Empty);//项目名称
                row.GetCell(3).SetCellValue(list[i].FBusinessName ?? string.Empty);//业务条线
            }



            string filename = "model_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            using (FileStream fs = File.OpenWrite(path + "\\" + filename))
            {
                book.Write(fs);//向打开的这个xls文件中写入并保存。  
                fs.Flush();
                fs.Close();
            }
            return JsonConvert.SerializeObject(new { path = "ProjectMst", filename = filename });

        }
        #region//签报单相关接口

        /// <summary>
        /// 获取该组织该人员可以进行签报的项目数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetMSYHProjectForQB([FromUri]Model.Request.BaseListModel param)
        {
            if (param.OrgId == 0 || string.IsNullOrEmpty(param.OrgCode))
            {
                return DCHelper.ErrorMessage("组织信息不能为空！");
            }
            if (param.UserId == 0)
            {
                return DCHelper.ErrorMessage("用户信息不能为空！");
            }
            if (string.IsNullOrEmpty(param.Year))
            {
                return DCHelper.ErrorMessage("年份信息不能为空！");
            }
            try
            {
                IList<ProjectMstModel> projectMsts = new List<ProjectMstModel>();
                //筛选出该组织该人员可以进行签报的项目数据
                projectMsts = this.ProjectMstService.Find(t => t.FDeclarationUnit == param.OrgCode && t.FDeclarerId == param.UserId && t.FApproveStatus == "3" && t.FYear == param.Year).Data;
                //取业务条线 用于代码转名称
                var syssets = QTSysSetService.Find(x => x.DicType == "Business" && x.Orgid == param.OrgId).Data.ToList();
                if (syssets != null)
                {
                    foreach (var mst in projectMsts)
                    {
                        if (!string.IsNullOrEmpty(mst.FBusinessCode))
                        {
                            if (syssets.Find(x => x.TypeCode == mst.FBusinessCode) != null)
                            {
                                mst.FBusinessName = syssets.Find(x => x.TypeCode == mst.FBusinessCode).TypeName;
                            }
                        }
                    }
                }
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "获取成功！",
                    Data = projectMsts
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 获取该组织该人员进行签报时能取的费用说明
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetCostitemForQB([FromUri]Model.Request.BaseListModel param)
        {
            if (param.OrgId == 0)
            {
                return DCHelper.ErrorMessage("组织信息不能为空！");
            }
            if (param.FPhid == 0)//项目单据的主键 不是签报单据的主键
            {
                return DCHelper.ErrorMessage("请先选择项目！");
            }
            var syssets = QTSysSetService.Find(x => x.DicType == "Costitem" && x.Orgid == param.OrgId).Data.ToList();
            var dtls = ProjectMstService.FindProjectDtlBudgetDtlByForeignKey(param.FPhid).Data.ToList();
            if (dtls != null && dtls.Count > 0)
            {
                for (var i = 0; i < dtls.Count; i++)
                {
                    var set = new QTSysSetModel();
                    set.TypeName = dtls[i].FName;
                    syssets.Add(set);
                }
            }
            var data = new
            {
                Status = ResponseStatus.Success,
                Msg = "获取成功！",
                Costitem = syssets,//费用说明
            };
            return DataConverterHelper.SerializeObject(data);
        }

       
        /// <summary>
        /// 获取单个支出预算项目详情
        /// </summary>
        /// <param name="projectMst"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetProjectInfo([FromUri]ProjectMstRequest projectMst)
        {
            if (projectMst.FProjPhId == 0)
            {
                return DCHelper.ErrorMessage("项目主键不能为空！");
            }

            try
            {
                ProjectAllDataModel projectAllData = new ProjectAllDataModel();
                projectAllData.ProjectMst = ProjectMstService.Find(projectMst.FProjPhId).Data;

                //返回对象增加附件
                if (projectAllData.ProjectMst != null)
                {
                    projectAllData.ProjectAttachments = QtAttachmentService.Find(t => t.BTable == "XM3_PROJECTMST" && t.RelPhid == projectAllData.ProjectMst.PhId).Data.ToList();
                }
                projectAllData.ProjectDtlBudgetDtls = ProjectMstService.FindProjectDtlBudgetDtlByForeignKey(projectMst.FProjPhId).Data.ToList();
                projectAllData.ProjectDtlBudgetDtls.Sort((ProjectDtlBudgetDtlModel dtl1, ProjectDtlBudgetDtlModel dtl2) => dtl1.FDtlCode.CompareTo(dtl2.FDtlCode));

                return DataConverterHelper.SerializeObject(projectAllData);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
        #endregion
        #endregion
    }
}