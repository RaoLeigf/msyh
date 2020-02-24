using Enterprise3.Common.Base.Criterion;
using Enterprise3.WebApi.ApiControllerBase;
using Enterprise3.WebApi.GYS3.YS.Model;
using Enterprise3.WebApi.GYS3.YS.Model.Request;
using GYS3.YS.Model.Domain;
using GYS3.YS.Service.Interface;
using GQT3.QT.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SUP.Common.DataEntity;
using SUP.Common.Base;
using GYS3.YS.Model.Extend;
using GYS3.YS.Model.Enums;
using Enterprise3.Common.Model.Results;
using GXM3.XM.Service.Interface;
using GQT3.QT.Model.Domain;
using GYS3.YS.Model.Extra;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;
using Enterprise3.WebApi.ApiControllerBase.Models;
using System.IO;
using Enterprise3.WebApi.GYS3.YS.Model.Response;
using GSP3.SP.Service.Interface;
using log4net.Repository.Hierarchy;

namespace Enterprise3.WebApi.GYS3.YS.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [MethodExceptionFilter]
    public class BudgetMstApiController : ApiBase
    {
        IBudgetMstService BudgetMstService { get; set; }
        ICorrespondenceSettingsService CorrespondenceSettingsService { get; set; }
        ICorrespondenceSettings2Service CorrespondenceSettings2Service { get; set; }
        IProjectMstService ProjectMstService { get; set; }


        IQTSysSetService QTSysSetService { get; set; }

        IQtAttachmentService QtAttachmentService { get; set; }

        IYsAccountMstService YsAccountMstService { get; set; }

        IGAppvalProcService GAppvalProcService { get; set; }

        IQtTableCustomizeService QtTableCustomizeService { get; set; }

        IBudgetProcessCtrlService BudgetProcessCtrlService { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public BudgetMstApiController()
        {
            BudgetMstService = base.GetObject<IBudgetMstService>("GYS3.YS.Service.BudgetMst");
            CorrespondenceSettingsService = base.GetObject<ICorrespondenceSettingsService>("GQT3.QT.Service.CorrespondenceSettings");
            CorrespondenceSettings2Service = base.GetObject<ICorrespondenceSettings2Service>("GQT3.QT.Service.CorrespondenceSettings2");
            ProjectMstService = base.GetObject<IProjectMstService>("GXM3.XM.Service.ProjectMst");
            QTSysSetService = base.GetObject<IQTSysSetService>("GQT3.QT.Service.QTSysSet");
            QtAttachmentService = base.GetObject<IQtAttachmentService>("GQT3.QT.Service.QtAttachment");
            YsAccountMstService = base.GetObject<IYsAccountMstService>("GYS3.YS.Service.YsAccountMst");
            GAppvalProcService = base.GetObject<IGAppvalProcService>("GSP3.SP.Service.GAppvalProc");
            QtTableCustomizeService = base.GetObject<IQtTableCustomizeService>("GQT3.QT.Service.QtTableCustomize");
            BudgetProcessCtrlService = base.GetObject<IBudgetProcessCtrlService>("GYS3.YS.Service.BudgetProcessCtrl");
        }

        /// <summary>
        /// 取可选项目
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetBudgetMstList([FromUri]BudgetMstRequestModel param)
        {
            /*FYear: '2019',
               FDeclarationUnit: '101',
               FBudgetDept: '101.01'
              */

            if (string.IsNullOrEmpty(param.Year))
            {
                return DCHelper.ErrorMessage("年度为空！");
            }
            if (string.IsNullOrEmpty(param.UnitId))
            {
                return DCHelper.ErrorMessage("组织为空！");
            }
            if (string.IsNullOrEmpty(param.DeptId))
            {
                return DCHelper.ErrorMessage("部门为空！");
            }

            var result = BudgetMstService.GetDxbzList(param);
            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 通过主键找到对下补助明细
        /// </summary>
        /// <param name="id"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetBudgetMstDtlList([FromUri]long id, [FromUri]long orgid)
        {
            var result = BudgetMstService.GetDxbzDtl(id, orgid);
            return DataConverterHelper.SerializeObject(result);
        }

        #region 项目支出申报接口
        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetBudgetMstLists([FromUri]BudgetMstListsRequestModel param)
        {
            if (string.IsNullOrEmpty(param.UserId))
            {
                return DCHelper.ErrorMessage("用户编码不能为空！");
            }
            if (string.IsNullOrEmpty(param.Year))
            {
                return DCHelper.ErrorMessage("年份信息不能为空！");
            }
            try
            {
                Dictionary<string, object> dicWhere = new Dictionary<string, object>();//查询条件转Dictionary
                //new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FApproveStatus", param.FApproveStatus));

                var dicWhereDept = new Dictionary<string, object>();
                new CreateCriteria(dicWhereDept)
                    .Add(ORMRestrictions<string>.Eq("Dwdm", param.UserId)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
                var deptList = CorrespondenceSettingsService.Find(dicWhereDept);
                List<string> deptL = new List<string>();
                for (var i = 0; i < deptList.Data.Count; i++)
                {
                    deptL.Add(deptList.Data[i].Dydm);
                }
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FType", "c"))
                  .Add(ORMRestrictions<string>.Eq("FVerNo", "0001")).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));//.Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)) 要同时显示被年中调整的原单据

                //增加年份信息
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<string>.Eq("FYear", param.Year));

                //增加搜索条件
                if (!string.IsNullOrEmpty(param.FApproveStatus) && !"0".Equals(param.FApproveStatus))//审批状态
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<string>.Eq("FApproveStatus", param.FApproveStatus));
                }
                if (!string.IsNullOrEmpty(param.FProjCode) || !string.IsNullOrEmpty(param.FProjName))//项目编码
                {
                    Dictionary<string, object> dicWhere1 = new Dictionary<string, object>();
                    Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(param.FProjName))
                    {
                        new CreateCriteria(dicWhere1).Add(ORMRestrictions<string>.Like("FProjName", param.FProjName));
                    }
                    else
                    {
                        new CreateCriteria(dicWhere1).Add(ORMRestrictions<Int64>.Eq("PhId", 0));
                    }
                    new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FProjCode", param.FProjCode));
                    new CreateCriteria(dicWhere).Add(ORMRestrictions.Or(dicWhere1, dicWhere2));
                }

                if (!string.IsNullOrEmpty(param.FDeclarationDept))//申报部门
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FDeclarationDept", param.FDeclarationDept));
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
                if (param.FIfPerformanceAppraisal > 0)//绩效评价
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<EnumYesNo>.Eq("FIfPerformanceAppraisal", param.FIfPerformanceAppraisal));
                }
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

                var orderby = new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" };//对结果的排序
                if (!string.IsNullOrEmpty(param.OrderBy))
                {
                    orderby = new string[] { param.OrderBy, "NgInsertDt Desc", "NgUpdateDt Desc" };
                }
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FDeclarationUnit", param.FDeclarationUnit));

                var result = this.BudgetMstService.LoadWithPage(param.PageIndex, param.PageSize, dicWhere, orderby);

                return DCHelper.ModelListToJson<BudgetMstModel>(result.Results, (Int32)result.TotalItems);
                //return DataConverterHelper.EntityListToJson<BudgetMstModel>(result.Results, (Int32)result.TotalItems);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetBudgetMstInfo([FromUri]BudgetMstListsRequestModel param)
        {
            if (param.FBudgetPhId == 0)
            {
                return DCHelper.ErrorMessage("预算主键不能为空！");
            }
            try
            {
                BudgetMstAllDataModel budgetmstAllData = new BudgetMstAllDataModel();

                budgetmstAllData.BudgetMst = BudgetMstService.Find(param.FBudgetPhId).Data;
                budgetmstAllData.BudgetDtlImplPlans = BudgetMstService.FindBudgetDtlImplPlanByForeignKey(param.FBudgetPhId).Data.ToList();
                budgetmstAllData.BudgetDtlTextContents = BudgetMstService.FindBudgetDtlTextContentByForeignKey(param.FBudgetPhId).Data.ToList();
                budgetmstAllData.BudgetDtlFundAppls = BudgetMstService.FindBudgetDtlFundApplByForeignKey(param.FBudgetPhId).Data.ToList();

                budgetmstAllData.BudgetDtlBudgetDtls = BudgetMstService.FindBudgetDtlBudgetDtlByForeignKey(param.FBudgetPhId).Data.ToList();
                budgetmstAllData.BudgetDtlPurchaseDtls = BudgetMstService.FindBudgetDtlPurchaseDtlByForeignKey(param.FBudgetPhId).Data.ToList();
                budgetmstAllData.BudgetDtlPurDtl4SOFs = BudgetMstService.FindBudgetDtlPurDtl4SOFByForeignKey(param.FBudgetPhId).Data.ToList();
                budgetmstAllData.BudgetDtlPerformTargets = BudgetMstService.FindBudgetDtlPerformTargetByForeignKey(param.FBudgetPhId).Data.ToList();

                return DataConverterHelper.SerializeObject(budgetmstAllData);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpPost]
        public string PostDeleteBudgetMst([FromBody]BudgetMstListsRequestModel param)
        {

            if (param.FBudgetPhId == 0)
            {
                return DCHelper.ErrorMessage("预算主键不能为空！");
            }
            try
            {
                //删除时,对应关系里相关数据也删除
                var findedresultmst = BudgetMstService.Find(param.FBudgetPhId);

                var deletedresult = BudgetMstService.Delete<System.Int64>(param.FBudgetPhId);

                //删除年中调整数据时,恢复上一版本数据(如果是只做过一次调整,则去掉年初预算里的调整标志,删除相关年中调整数据)
                if ((findedresultmst.Data.FType == "c" && findedresultmst.Data.FVerNo == "0002") || findedresultmst.Data.FType == "z" && findedresultmst.Data.FVerNo == "0002")
                {
                    var dicWhere1 = new Dictionary<string, object>(); //年初新增
                    var XmMstPhid = findedresultmst.Data.XmMstPhid;
                    if (findedresultmst.Data.FLifeCycle == 0)
                    {
                        new CreateCriteria(dicWhere1).Add(ORMRestrictions<Int64>.Eq("XmMstPhid", XmMstPhid))
                          .Add(ORMRestrictions<string>.Eq("FMidYearChange", "1"));
                        var oldList = BudgetMstService.Find(dicWhere1).Data[0];
                        oldList.FMidYearChange = "0";
                        oldList.PersistentState = PersistentState.Modified;
                        BudgetMstService.Save<Int64>(oldList, "");

                    }
                    else
                    { //多次调整的,FLifeCycle找到最新版置为0
                        new CreateCriteria(dicWhere1).Add(ORMRestrictions<Int64>.Eq("XmMstPhid", XmMstPhid))
                         .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", findedresultmst.Data.FLifeCycle));
                        var oldList = BudgetMstService.Find(dicWhere1).Data[0];
                        oldList.FLifeCycle = 0;
                        oldList.PersistentState = PersistentState.Modified;
                        BudgetMstService.Save<Int64>(oldList, "");
                    }
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
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }


        #endregion

        #region 年中调整相关
        /// <summary>
        /// 获取年中调整列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetBudgetList([FromBody]BudgetMstListsRequestModel param)
        {
            if (string.IsNullOrEmpty(param.UserId))
            {
                return DCHelper.ErrorMessage("用户编码不能为空！");
            }
            try
            {
                Dictionary<string, object> dicWhere = new Dictionary<string, object>();//查询条件转Dictionary
                //new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FApproveStatus", param.FApproveStatus));

                var dicWhereDept = new Dictionary<string, object>();
                new CreateCriteria(dicWhereDept)
                    .Add(ORMRestrictions<string>.Eq("Dwdm", param.UserId)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
                var deptList = CorrespondenceSettingsService.Find(dicWhereDept);
                List<string> deptL = new List<string>();
                for (var i = 0; i < deptList.Data.Count; i++)
                {
                    deptL.Add(deptList.Data[i].Dydm);
                }

                //预算数据类型
                if (!string.IsNullOrEmpty(param.BudgetType) && !"0".Equals(param.BudgetType))
                {
                    if (param.BudgetType == "1")
                    {
                        new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FType", "c"))
                            .Add(ORMRestrictions<string>.Eq("FVerNo", "0001")).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));//.Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)) 要同时显示被年中调整的原单据
                    }
                    else if (param.BudgetType == "2")
                    {
                        new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FType", "z"))
                            .Add(ORMRestrictions<string>.Eq("FVerNo", "0001")).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));//.Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)) 要同时显示被年中调整的原单据

                    }
                    else if (param.BudgetType == "3")
                    {
                        new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FType", "z"))
                            .Add(ORMRestrictions<string>.Eq("FVerNo", "0002")).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));//.Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)) 要同时显示被年中调整的原单据

                    }
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
                if (param.FIfPerformanceAppraisal > 0)//绩效评价
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<EnumYesNo>.Eq("FIfPerformanceAppraisal", param.FIfPerformanceAppraisal));
                }
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

                var result = this.BudgetMstService.LoadWithPage(param.PageIndex, param.PageSize, dicWhere, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" });

                return DCHelper.ModelListToJson<BudgetMstModel>(result.Results, (Int32)result.TotalItems);
                //return DataConverterHelper.EntityListToJson<BudgetMstModel>(result.Results, (Int32)result.TotalItems);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 根据申报单位，申报部门，预算部门取预算单据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetYS_expense([FromUri]ExpenseMstModel expenseMst)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "GKBM"))
                .Add(ORMRestrictions<string>.Eq("Dydm", expenseMst.FBudgetDept));
            List<string> ProjcodeList = CorrespondenceSettings2Service.Find(dicWhere).Data.ToList().Select(x => x.Dwdm).Distinct().ToList();
            Dictionary<string, object> dicWhereys = new Dictionary<string, object>();
            Dictionary<string, object> dicWhereys1 = new Dictionary<string, object>();
            Dictionary<string, object> dicWhereys2 = new Dictionary<string, object>();
            Dictionary<string, object> dicWhereys3 = new Dictionary<string, object>();
            new CreateCriteria(dicWhereys1).Add(ORMRestrictions<List<string>>.In("FProjCode", ProjcodeList));
            new CreateCriteria(dicWhereys2)
                .Add(ORMRestrictions<string>.Eq("FBudgetDept", expenseMst.FBudgetDept));
            new CreateCriteria(dicWhereys3)
               .Add(ORMRestrictions<string>.Eq("FDeclarationDept", expenseMst.FBudgetDept));
            new CreateCriteria(dicWhereys).Add(ORMRestrictions.Or(dicWhereys1, dicWhereys2, dicWhereys3));

            new CreateCriteria(dicWhereys).Add(ORMRestrictions<string>.Eq("FDeclarationUnit", expenseMst.FDeclarationunit))
                .Add(ORMRestrictions<string>.Eq("FBudgetDept", expenseMst.FDeclarationDept))
                .Add(ORMRestrictions<string>.Eq("FYear", expenseMst.FYear))
                .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
                .Add(ORMRestrictions<string>.Eq("FApproveStatus", "3"))
                .Add(ORMRestrictions<String>.Eq("FMidYearChange", "0"));

            var BudgetMsts = BudgetMstService.Find(dicWhereys).Data;
            return DCHelper.ModelListToJson<BudgetMstModel>(BudgetMsts, (Int32)BudgetMsts.Count);
        }


        /// <summary>
        /// 获取年初申报列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetBeginYear([FromUri]BaseListModel param)
        {
            if (param.orgid == 0)
            {
                return DCHelper.ErrorMessage("组织id不能为空！");
            }
            if (string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织Code不能为空！");
            }
            if (string.IsNullOrEmpty(param.Year))
            {
                return DCHelper.ErrorMessage("年份信息不能为空！");
            }
            try
            {
                var result = this.BudgetMstService.GetYsAccounts(param.orgid.ToString(), param.orgCode, param.Year);
                YsAccountMstModel ysAccountMst = new YsAccountMstModel();
                IList<YsAccountMstModel> ysAccountMsts = new List<YsAccountMstModel>();
                ysAccountMsts = this.YsAccountMstService.Find(t => t.Orgid == param.orgid && t.Uyear == param.Year).Data;
                if (ysAccountMsts != null && ysAccountMsts.Count > 0)
                {
                    ysAccountMst = ysAccountMsts[0];
                }
                else
                {
                    ysAccountMst.Uyear = param.Year;
                    ysAccountMst.Orgcode = param.orgCode;
                    ysAccountMst.Orgid = param.orgid;
                }
                ysAccountMst.YsAccounts = result;
                var data = new
                {
                    Status = "success",
                    Msg = "年初申报查询成功！",
                    Data = result,
                    Data2 = ysAccountMst
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetYSBudgetMstList([FromUri]BudgetMstListsRequestModel param)
        {
            //string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            //Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            //var workType = System.Web.HttpContext.Current.Request.Params["workType"]; //业务种类(年初,年中,特殊)
            //var FApproveStatus = System.Web.HttpContext.Current.Request.Params["FApproveStatus"];
            ////增加根据操作员对应预算部门的过滤
            //var userId = System.Web.HttpContext.Current.Request.Params["userId"];
            //var showAll = System.Web.HttpContext.Current.Request.Params["showAll"];
            //if (showAll == "1")
            //{
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

            var dicWhereDept = new Dictionary<string, object>();
            new CreateCriteria(dicWhereDept)
                .Add(ORMRestrictions<string>.Eq("Dwdm", param.Ucode)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
            var deptList = CorrespondenceSettingsService.Find(dicWhereDept);
            List<string> deptL = new List<string>();
            for (var i = 0; i < deptList.Data.Count; i++)
            {
                deptL.Add(deptList.Data[i].Dydm);
            }
            new CreateCriteria(dicWhere).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", param.Year));
            //}
            //else
            //{
            //    //取默认申报部门
            //    var dicDefaultDept = new Dictionary<string, object>();
            //    new CreateCriteria(dicDefaultDept).Add(ORMRestrictions<string>.Eq("Dylx", "08"))
            //        .Add(ORMRestrictions<Int32>.Eq("DefInt1", 1)).Add(ORMRestrictions<string>.Eq("Dwdm", userId));
            //    var dygx1 = CorrespondenceSettingsService.Find(dicDefaultDept).Data;
            //    if (dygx1.Count > 0)
            //    {
            //        var DefaultDept = dygx1[0].DefStr3;
            //        Dictionary<string, object> dicWhere4 = new Dictionary<string, object>();
            //        new CreateCriteria(dicWhere4).Add(ORMRestrictions<string>.Eq("Dylx", "GKBM"))
            //            .Add(ORMRestrictions<string>.Eq("Dydm", DefaultDept));
            //        List<string> ProjcodeList = CorrespondenceSettings2Service.Find(dicWhere4).Data.ToList().Select(x => x.Dwdm).Distinct().ToList();

            //        Dictionary<string, object> dicWhereys1 = new Dictionary<string, object>();
            //        Dictionary<string, object> dicWhereys2 = new Dictionary<string, object>();
            //        Dictionary<string, object> dicWhereys3 = new Dictionary<string, object>();
            //        new CreateCriteria(dicWhereys1).Add(ORMRestrictions<List<string>>.In("FProjCode", ProjcodeList));
            //        new CreateCriteria(dicWhereys2).Add(ORMRestrictions<string>.Eq("FBudgetDept", DefaultDept));
            //        new CreateCriteria(dicWhereys3).Add(ORMRestrictions<string>.Eq("FDeclarationDept", DefaultDept));
            //        new CreateCriteria(dicWhere).Add(ORMRestrictions.Or(dicWhereys1, dicWhereys2, dicWhereys3));

            //    }
            //    else
            //    {
            //        return DataConverterHelper.EntityListToJson<BudgetMstModel>(null, 0);
            //    }
            //}

            new CreateCriteria(dicWhere).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0));

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
                new CreateCriteria(dicWhere).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
                    .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));
            }

            //增加搜索条件
            if (!string.IsNullOrEmpty(param.SearchValue))
            {
                Dictionary<string, object> dicName = new Dictionary<string, object>();
                Dictionary<string, object> dicCode = new Dictionary<string, object>();
                new CreateCriteria(dicName)
                        .Add(ORMRestrictions<string>.Like("FProjName", param.SearchValue));
                new CreateCriteria(dicCode)
                        .Add(ORMRestrictions<string>.Like("FProjCode", param.SearchValue));
                new CreateCriteria(dicWhere).Add(ORMRestrictions.Or(dicName, dicCode));
            }
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
            if (param.FIfPerformanceAppraisal > 0)//绩效评价
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<EnumYesNo>.Eq("FIfPerformanceAppraisal", param.FIfPerformanceAppraisal));
            }
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
            if(param.FProjStatus > 0)
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<Int32>.Eq("FProjStatus", param.FProjStatus));
            }

            var result = BudgetMstService.LoadWithPage(param.PageIndex, param.PageSize, dicWhere, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" });

            //取可选相同审批流是数据集合
            if (param.ProcPhid != 0)
            {
                var proList = this.BudgetMstService.Find(dicWhere, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" }).Data;
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
                                if (res.FApproveStatus == ((int)EnumApproveStatus.ToBeRepored).ToString() && procList.ToList().Find(t => t.OrgCode == res.FBudgetDept && t.PhId == param.ProcPhid) != null)
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

                result.Results = proList.Skip((param.PageIndex - 1) * param.PageSize).Take(param.PageSize).ToList();
                result.TotalItems = proList.Count;
            }

            //提高接口效率
            var Query = QtAttachmentService.Find(t => t.BTable == "YS3_BUDGETMST").Data;
            foreach (var item in result.Results)
            {
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
            //    new CreateCriteria(dics).Add(ORMRestrictions<string>.Eq("BTable", "YS3_BUDGETMST"));
            //    var Query = QtAttachmentService.Find(dics);
            //    var QueryList = QtAttachmentService.Find(dics).Data.ToList();
            //    item.list = QueryList;
            //    //var address = Query.Data.Select(m => m.BUrlpath).ToArray();
            //    //var name = Query.Data.Select(m => m.BName).ToArray();
            //    //var model = new UploadPackGYS { UploadFileaddress = address, UploadFilename = name };
            //    //item.Uploadmodel = model;
            //    item.UploadFileCount = Query.Data.Select(m => m.BUrlpath).Count();
            //}


            var dicSysset = new Dictionary<string, object>();
            new CreateCriteria(dicSysset).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
            var syssets = QTSysSetService.Find(dicSysset).Data.ToList();
            if(result.Results != null && result.Results.Count > 0)
            {
                foreach(var res in result.Results)
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
            return DCHelper.ModelListToJson<BudgetMstModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 取列表数据2
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetYSBudgetMstList2([FromUri]BudgetMstListsRequestModel param)
        {
            //string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            //Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            //var workType = System.Web.HttpContext.Current.Request.Params["workType"]; //业务种类(年初,年中,特殊)
            //var FApproveStatus = System.Web.HttpContext.Current.Request.Params["FApproveStatus"];
            ////增加根据操作员对应预算部门的过滤
            //var userId = System.Web.HttpContext.Current.Request.Params["userId"];
            //var showAll = System.Web.HttpContext.Current.Request.Params["showAll"];
            //if (showAll == "1")
            //{
            if (string.IsNullOrEmpty(param.Ucode))
            {
                return DCHelper.ErrorMessage("用户编码不能为空！");
            }
            if (string.IsNullOrEmpty(param.OrgCode))
            {
                return DCHelper.ErrorMessage("组织编码不能为空！");
            }
            if (string.IsNullOrEmpty(param.Year))
            {
                return DCHelper.ErrorMessage("年度信息不能为空");
            }
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("FDeclarationUnit", param.OrgCode));

            var dicWhereDept = new Dictionary<string, object>();
            new CreateCriteria(dicWhereDept)
                .Add(ORMRestrictions<string>.Eq("Dwdm", param.Ucode)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
            var deptList = CorrespondenceSettingsService.Find(dicWhereDept);
            List<string> deptL = new List<string>();
            for (var i = 0; i < deptList.Data.Count; i++)
            {
                deptL.Add(deptList.Data[i].Dydm);
            }
            new CreateCriteria(dicWhere).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", param.Year));

            //}
            //else
            //{
            //    //取默认申报部门
            //    var dicDefaultDept = new Dictionary<string, object>();
            //    new CreateCriteria(dicDefaultDept).Add(ORMRestrictions<string>.Eq("Dylx", "08"))
            //        .Add(ORMRestrictions<Int32>.Eq("DefInt1", 1)).Add(ORMRestrictions<string>.Eq("Dwdm", userId));
            //    var dygx1 = CorrespondenceSettingsService.Find(dicDefaultDept).Data;
            //    if (dygx1.Count > 0)
            //    {
            //        var DefaultDept = dygx1[0].DefStr3;
            //        Dictionary<string, object> dicWhere4 = new Dictionary<string, object>();
            //        new CreateCriteria(dicWhere4).Add(ORMRestrictions<string>.Eq("Dylx", "GKBM"))
            //            .Add(ORMRestrictions<string>.Eq("Dydm", DefaultDept));
            //        List<string> ProjcodeList = CorrespondenceSettings2Service.Find(dicWhere4).Data.ToList().Select(x => x.Dwdm).Distinct().ToList();

            //        Dictionary<string, object> dicWhereys1 = new Dictionary<string, object>();
            //        Dictionary<string, object> dicWhereys2 = new Dictionary<string, object>();
            //        Dictionary<string, object> dicWhereys3 = new Dictionary<string, object>();
            //        new CreateCriteria(dicWhereys1).Add(ORMRestrictions<List<string>>.In("FProjCode", ProjcodeList));
            //        new CreateCriteria(dicWhereys2).Add(ORMRestrictions<string>.Eq("FBudgetDept", DefaultDept));
            //        new CreateCriteria(dicWhereys3).Add(ORMRestrictions<string>.Eq("FDeclarationDept", DefaultDept));
            //        new CreateCriteria(dicWhere).Add(ORMRestrictions.Or(dicWhereys1, dicWhereys2, dicWhereys3));

            //    }
            //    else
            //    {
            //        return DataConverterHelper.EntityListToJson<BudgetMstModel>(null, 0);
            //    }
            //}

            new CreateCriteria(dicWhere).Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0));
            if (param.projectarea!=null)
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
            if (param.FIfPerformanceAppraisal > 0)//绩效评价
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<EnumYesNo>.Eq("FIfPerformanceAppraisal", param.FIfPerformanceAppraisal));
            }
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


            //提高接口效率
            var Query = QtAttachmentService.Find(t => t.BTable == "YS3_BUDGETMST").Data;
            foreach (var item in result.Results)
            {
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
            //    new CreateCriteria(dics).Add(ORMRestrictions<string>.Eq("BTable", "YS3_BUDGETMST"));
            //    var Query = QtAttachmentService.Find(dics);
            //    var QueryList = QtAttachmentService.Find(dics).Data.ToList();
            //    item.list = QueryList;
            //    //var address = Query.Data.Select(m => m.BUrlpath).ToArray();
            //    //var name = Query.Data.Select(m => m.BName).ToArray();
            //    //var model = new UploadPackGYS { UploadFileaddress = address, UploadFilename = name };
            //    //item.Uploadmodel = model;
            //    item.UploadFileCount = Query.Data.Select(m => m.BUrlpath).Count();
            //}


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
            return DCHelper.ModelListToJson<BudgetMstModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpPost]
        public string PostSaveProject([FromBody]BudgetAllDataRequest budgetAllData)
        {
            long id = 0;
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            var findedresult = new BudgetAllDataRequest();
            if(budgetAllData == null || budgetAllData.BudgetMst == null)
            {
                return DCHelper.ErrorMessage("参数传递不正确！");
            }
            try
            {

                //年中调整修改年初预算时,复制原来预算信息,f_VerNo值为”0002”，所有信息从”0001”版本拷贝过来(原”0001”的项目记录保存不变)
                if (budgetAllData.midYearEdit == "midYearEdit")
                {
                    //如果是第一次点年中调整或者点年中新增，主表phid=0
                    if (budgetAllData.BudgetMst.FType == "c")
                    {
                        if (budgetAllData.BudgetMst.FApproveStatus != "3")
                        {
                            return DCHelper.ErrorMessage("只有审批完成的数据才能进行年中调整！");
                        }
                    }
                    else if (budgetAllData.BudgetMst.FType == "z")
                    {
                        if (budgetAllData.BudgetMst.FVerNo == "0001")
                        {
                            return DCHelper.ErrorMessage("年中新增数据不能进行年中调整！");
                        }
                        else
                        {
                            if (budgetAllData.BudgetMst.FApproveStatus != "3")
                            {
                                return DCHelper.ErrorMessage("只有审批完成的数据才能进行年中调整！");
                            }
                        }
                    }
                    id = budgetAllData.BudgetMst.PhId;
                    //年中调整时,项目审批状态改为未审批,项目属性改为年中调整
                    budgetAllData.BudgetMst.FApproveStatus = "1";
                    budgetAllData.BudgetMst.FProjStatus = 4;
                    //budgetAllData.BudgetMst.FType = "c";
                    budgetAllData.BudgetMst.FVerNo = "0002";
                    //原先的数据
                    findedresult.BudgetMst = BudgetMstService.Find(id).Data;
                    findedresult.BudgetDtlBudgetDtls = BudgetMstService.FindBudgetDtlBudgetDtlByForeignKey(id).Data.ToList();
                    if (findedresult.BudgetMst == null)
                    {
                        return DCHelper.ErrorMessage("需要年中调整的数据不存在！");
                    }
                    else
                    {
                        if (findedresult.BudgetDtlBudgetDtls != null && findedresult.BudgetDtlBudgetDtls.Count >0)
                        {
                            if(budgetAllData.BudgetDtlBudgetDtls != null && budgetAllData.BudgetDtlBudgetDtls.Count > 0)
                            {
                                if(budgetAllData.BudgetDtlBudgetDtls.Count == findedresult.BudgetDtlBudgetDtls.Count)
                                {
                                    foreach(var dtls in findedresult.BudgetDtlBudgetDtls)
                                    {
                                        if(budgetAllData.BudgetDtlBudgetDtls.Find(t=>t.FDtlCode == dtls.FDtlCode) == null)
                                        {
                                            return DCHelper.ErrorMessage("年中调整时不能删除原先明细！");
                                        }
                                    }
                                    budgetAllData.BudgetMst.FAdjustType = "03";//标记是年中只调整了明细
                                }
                                else if (budgetAllData.BudgetDtlBudgetDtls.Count > findedresult.BudgetDtlBudgetDtls.Count)
                                {
                                    int flam = 0;
                                    foreach (var dtls in findedresult.BudgetDtlBudgetDtls)
                                    {
                                        var budgetDtl = budgetAllData.BudgetDtlBudgetDtls.Find(t => t.FDtlCode == dtls.FDtlCode);
                                        if (budgetDtl == null)
                                        {
                                            return DCHelper.ErrorMessage("年中调整时不能删除原先明细！");
                                        }
                                        else
                                        {
                                            if(budgetDtl.FAmountEdit != 0)
                                            {
                                                flam = 1;
                                            }
                                        }
                                    }
                                    if(flam > 0)
                                    {
                                        budgetAllData.BudgetMst.FAdjustType = "04";//标记是年中新增和修改了明细
                                    }
                                    else
                                    {
                                        budgetAllData.BudgetMst.FAdjustType = "02";//标记是年中只新增了明细
                                    }
                                }
                                else
                                {
                                    return DCHelper.ErrorMessage("年中调整时不能删除原先明细！");
                                }
                            }
                            else
                            {
                                return DCHelper.ErrorMessage("年中调整时不能删除原先明细！");
                            }
                        }
                        else
                        {
                            if(budgetAllData.BudgetDtlBudgetDtls != null && budgetAllData.BudgetDtlBudgetDtls.Count > 0)
                            {
                                budgetAllData.BudgetMst.FAdjustType = "02";//标记是年中只新增了明细
                            }
                            else
                            {
                                budgetAllData.BudgetMst.FAdjustType = "03";//标记是年中只调整了明细
                            }
                        }
                    }
                    ////年中调整时,如果是调整已年中调整过的单据,则FLifeCycle 加 1 ,其他不变
                    if (findedresult.BudgetMst != null && findedresult.BudgetMst.FVerNo == "0002")
                    {
                        //根据项目代码去预算表里查找相同代码的条数,得知相关版本号
                        var dicWhereLife = new Dictionary<string, object>();
                        new CreateCriteria(dicWhereLife).Add(ORMRestrictions<string>.Eq("FProjCode", findedresult.BudgetMst.FProjCode));
                        var FLifeCycle = BudgetMstService.Find(dicWhereLife);
                        findedresult.BudgetMst.FLifeCycle = FLifeCycle.Data.Count;
                    }
                    else
                    {
                        findedresult.BudgetMst.FLifeCycle = 1;//第一次年中调整
                    }
                    findedresult.BudgetMst.FMidYearChange = "1"; //年中调整后,原来年初调整的调整标志改为1
                    findedresult.BudgetMst.PersistentState = PersistentState.Added;
                    findedresult.BudgetDtlImplPlans = BudgetMstService.FindBudgetDtlImplPlanByForeignKey(id).Data.ToList();
                    findedresult.BudgetDtlTextContents = BudgetMstService.FindBudgetDtlTextContentByForeignKey(id).Data.ToList();
                    findedresult.BudgetDtlFundAppls = BudgetMstService.FindBudgetDtlFundApplByForeignKey(id).Data.ToList();
                    
                    findedresult.BudgetDtlPerformTargets = BudgetMstService.FindBudgetDtlPerformTargetByForeignKey(id).Data.ToList();
                    findedresult.BudgetDtlPurchaseDtls = BudgetMstService.FindBudgetDtlPurchaseDtlByForeignKey(id).Data.ToList();
                    findedresult.BudgetDtlPurDtl4SOFs = BudgetMstService.FindBudgetDtlPurDtl4SOFByForeignKey(id).Data.ToList();
                    findedresult.BudgetDtlFundAppls= BudgetMstService.FindBudgetDtlFundApplByForeignKey(id).Data.ToList();

                }
                //当不是新增时记录修改历史
                //if (!string.IsNullOrEmpty(mstforminfo.AllRow[0].PhId.ToString()) && mstforminfo.AllRow[0].PhId != 0)
                //{
                //    BudgetMstService.SaveModify(mstforminfo.AllRow[0], budgetdtlimplplangridinfo, budgetdtltextcontentgridinfo, budgetdtlfundapplgridinfo, budgetdtlbudgetdtlgridinfo, budgetdtlperformtargetgridinfo, projectdtlpurchasedtlforminfo, projectdtlpurdtl4sofgridinfo);//保存预算单据修改记录
                //}

                if (budgetAllData.BudgetMst.PhId == 0)
                {
                    if (string.IsNullOrEmpty(budgetAllData.BudgetMst.FProjCode))
                    {
                        if (string.IsNullOrEmpty(budgetAllData.BudgetMst.FYear))
                        {
                            return DCHelper.ErrorMessage("年中新增的项目年度不能为空！");
                        }
                        //获取最大项目库编码
                        //var projCode = ProjectMstService.CreateOrGetMaxProjCode(budgetAllData.BudgetMst.FYear);
                        //编码改成年度+部门编码+序号
                        var projCode = budgetAllData.BudgetMst.FYear + budgetAllData.BudgetMst.FBudgetDept + ProjectMstService.CreateOrGetMaxProjCode(budgetAllData.BudgetMst.FYear).Substring(8, 4);

                        budgetAllData.BudgetMst.FProjCode = projCode;                        
                    }
                    budgetAllData.BudgetMst.FAdjustType = "01";//标记是年中新增
                    budgetAllData.BudgetMst.PersistentState = PersistentState.Added;
                    budgetAllData.BudgetMst.FType = "z";
                    budgetAllData.BudgetMst.FVerNo = "0001";
                }
                else
                {
                    if ((budgetAllData.BudgetMst.FApproveStatus == "2" || budgetAllData.BudgetMst.FApproveStatus =="3") && budgetAllData.midYearEdit != "midYearEdit")
                    {
                        return DCHelper.ErrorMessage("只有待送审,暂存以及审批未通过的数据才能进行修改！");
                    }
                    budgetAllData.BudgetMst.PersistentState = PersistentState.Modified;
                }
                if (budgetAllData.BudgetDtlBudgetDtls != null && budgetAllData.BudgetDtlBudgetDtls.Count > 0)
                {
                    #region //生成项目明细编码: 项目明细编码=项目编码 + 6位流水号
                    string dtlCode = "";
                    string dtlName = "";
                    for (var i = 0; i < budgetAllData.BudgetDtlBudgetDtls.Count; i++)
                    {
                        dtlCode = budgetAllData.BudgetDtlBudgetDtls[i].FDtlCode;
                        dtlName = budgetAllData.BudgetDtlBudgetDtls[i].FName.Trim();
                        if (string.IsNullOrEmpty(dtlCode))
                        {
                            //多行存在明细项目相同的视为同一个明细项目，后台存一个代码；(相同项目名称的项目代码相同)
                            budgetAllData.BudgetDtlBudgetDtls[i].FDtlCode = budgetAllData.BudgetMst.FProjCode + string.Format("{0:D6}", i + 1);
                            //for (var j = 0; j < i; j++)
                            //{
                            //    if (dtlName == budgetAllData.BudgetDtlBudgetDtls[j].FName.Trim())
                            //    {
                            //        budgetAllData.BudgetDtlBudgetDtls[i].FDtlCode = budgetAllData.BudgetDtlBudgetDtls[j].FDtlCode;
                            //        break;
                            //    }
                            //}
                        }
                        dtlCode = budgetAllData.BudgetDtlBudgetDtls[i].FDtlCode; ;
                        if(budgetAllData.BudgetDtlPurchaseDtls != null && budgetAllData.BudgetDtlPurchaseDtls.Count > 0)
                        {
                            for (var j = 0; j < budgetAllData.BudgetDtlPurchaseDtls.Count; j++)
                            {
                                if (budgetAllData.BudgetDtlPurchaseDtls[j].FName == dtlName)
                                {
                                    budgetAllData.BudgetDtlPurchaseDtls[j].FDtlCode = dtlCode;
                                }
                            }
                        }
                        if (budgetAllData.BudgetDtlPurDtl4SOFs != null && budgetAllData.BudgetDtlPurDtl4SOFs.Count > 0)
                        {
                            for (var j = 0; j < budgetAllData.BudgetDtlPurDtl4SOFs.Count; j++)
                            {
                                if (budgetAllData.BudgetDtlPurDtl4SOFs[j].FName == dtlName)
                                {
                                    budgetAllData.BudgetDtlPurDtl4SOFs[j].FDtlCode = dtlCode;
                                }
                            }
                        }

                    }
                    #endregion
                    foreach (var BudgetDtlBudgetDtl in budgetAllData.BudgetDtlBudgetDtls)
                    {
                        BudgetDtlBudgetDtl.FAmountAfterEdit = BudgetDtlBudgetDtl.FAmount + BudgetDtlBudgetDtl.FAmountEdit;
                        if (BudgetDtlBudgetDtl.PhId == 0)
                        {
                            BudgetDtlBudgetDtl.PersistentState = PersistentState.Added;
                        }
                        else
                        {
                            if(BudgetDtlBudgetDtl.PersistentState!= PersistentState.Deleted)
                            {
                                BudgetDtlBudgetDtl.PersistentState = PersistentState.Modified;
                            }
                        }
                    }
                }                
                if (budgetAllData.BudgetDtlFundAppls != null && budgetAllData.BudgetDtlFundAppls.Count > 0)
                {
                    foreach (var BudgetDtlFundAppl in budgetAllData.BudgetDtlFundAppls)
                    {
                        if (BudgetDtlFundAppl.PhId == 0)
                        {
                            BudgetDtlFundAppl.PersistentState = PersistentState.Added;
                        }
                        else
                        {
                            if (BudgetDtlFundAppl.PersistentState != PersistentState.Deleted)
                            {
                                BudgetDtlFundAppl.PersistentState = PersistentState.Modified;
                            }
                        }
                    }
                }
                if (budgetAllData.BudgetDtlImplPlans != null && budgetAllData.BudgetDtlImplPlans.Count > 0)
                {
                    foreach (var BudgetDtlImplPlan in budgetAllData.BudgetDtlImplPlans)
                    {
                        if (BudgetDtlImplPlan.PhId == 0)
                        {
                            BudgetDtlImplPlan.PersistentState = PersistentState.Added;
                        }
                        else
                        {
                            if (BudgetDtlImplPlan.PersistentState != PersistentState.Deleted)
                            {
                                BudgetDtlImplPlan.PersistentState = PersistentState.Modified;
                            }
                        }
                    }
                }
                if (budgetAllData.BudgetDtlPerformTargets != null && budgetAllData.BudgetDtlPerformTargets.Count > 0)
                {
                    foreach (var PerformTarget in budgetAllData.BudgetDtlPerformTargets)
                    {
                        if (PerformTarget.PhId == 0)
                        {
                            PerformTarget.PersistentState = PersistentState.Added;
                        }
                        else
                        {
                            if (PerformTarget.PersistentState != PersistentState.Deleted)
                            {
                                PerformTarget.PersistentState = PersistentState.Modified;
                            }
                        }
                    }
                }
                if (budgetAllData.BudgetDtlTextContents != null && budgetAllData.BudgetDtlTextContents.Count>0)
                {
                    foreach (var TextContent in budgetAllData.BudgetDtlTextContents)
                    {
                        if (TextContent.PhId == 0)
                        {
                            TextContent.PersistentState = PersistentState.Added;
                        }
                        else
                        {
                            if (TextContent.PersistentState != PersistentState.Deleted)
                            {
                                TextContent.PersistentState = PersistentState.Modified;
                            }
                        }
                    }
                    //if (budgetAllData.BudgetDtlTextContents[0].PhId == 0)
                    //{
                    //    budgetAllData.BudgetDtlTextContents[0].PersistentState = PersistentState.Added;
                    //}
                    //else
                    //{
                    //    if (budgetAllData.BudgetDtlTextContents[0].PersistentState != PersistentState.Deleted)
                    //    {
                    //    budgetAllData.BudgetDtlTextContents[0].PersistentState = PersistentState.Modified;
                    //    }
                    //}
                }

                if (budgetAllData.BudgetDtlPurchaseDtls != null && budgetAllData.BudgetDtlPurDtl4SOFs != null)
                {
                    //当不是新增时,先删除原有采购和采购资金来源数据,重新保存
                    if (!string.IsNullOrEmpty(budgetAllData.BudgetMst.PhId.ToString()) && budgetAllData.BudgetMst.PhId != 0)
                    {
                        BudgetMstService.DeleteProjectDtlPurchase(budgetAllData.BudgetMst.PhId);
                        foreach (var PurchaseDtl in budgetAllData.BudgetDtlPurchaseDtls)
                        {
                            PurchaseDtl.PhId = 0;
                        }
                        foreach (var PurDtl4SOF in budgetAllData.BudgetDtlPurDtl4SOFs)
                        {
                            PurDtl4SOF.PhId = 0;
                        }
                    }
                    foreach (var PurchaseDtl in budgetAllData.BudgetDtlPurchaseDtls)
                    {
                        if (PurchaseDtl.PhId == 0)
                        {
                            PurchaseDtl.PersistentState = PersistentState.Added;
                        }
                        else
                        {
                            if (PurchaseDtl.PersistentState != PersistentState.Deleted)
                            {
                                PurchaseDtl.PersistentState = PersistentState.Modified;
                            }
                        }
                    }
                    foreach (var PurDtl4SOF in budgetAllData.BudgetDtlPurDtl4SOFs)
                    {
                        if (PurDtl4SOF.PhId == 0)
                        {
                            PurDtl4SOF.PersistentState = PersistentState.Added;
                        }
                        else
                        {
                            if (PurDtl4SOF.PersistentState != PersistentState.Deleted)
                            {
                                PurDtl4SOF.PersistentState = PersistentState.Modified;
                            }
                        }
                    }
                    savedresult = BudgetMstService.SaveBudgetMst(budgetAllData.BudgetMst, budgetAllData.BudgetDtlImplPlans, budgetAllData.BudgetDtlTextContents, budgetAllData.BudgetDtlFundAppls, budgetAllData.BudgetDtlBudgetDtls, budgetAllData.BudgetDtlPerformTargets, budgetAllData.BudgetDtlPurchaseDtls, budgetAllData.BudgetDtlPurDtl4SOFs, null);
                }
                else
                {
                    savedresult = BudgetMstService.SaveBudgetMst(budgetAllData.BudgetMst, budgetAllData.BudgetDtlImplPlans, budgetAllData.BudgetDtlTextContents, budgetAllData.BudgetDtlFundAppls, budgetAllData.BudgetDtlBudgetDtls, budgetAllData.BudgetDtlPerformTargets);
                }

                // savedresult = BudgetMstService.SaveBudgetMst(mstforminfo.AllRow[0], budgetdtlimplplangridinfo.AllRow, budgetdtltextcontentgridinfo.AllRow, budgetdtlfundapplgridinfo.AllRow, budgetdtlbudgetdtlgridinfo.AllRow, budgetdtlperformtargetgridinfo.AllRow);
                if (savedresult.Status == "success")
                {
                    var i = 0;
                    var FDtlCode = "";
                    var FSourceOfFunds = "";
                    decimal[] FAmount_NEw = new decimal[budgetAllData.BudgetDtlBudgetDtls.Count];
                    long[] Xm3_DtlPhid = new long[budgetAllData.BudgetDtlBudgetDtls.Count];
                    decimal FAmount;
                    foreach (var dtl in budgetAllData.BudgetDtlBudgetDtls)//判断是否引用自项目库
                    {
                        if (dtl.Xm3_DtlPhid > 0)
                        {

                            FDtlCode = dtl.FDtlCode;
                            FSourceOfFunds = dtl.FSourceOfFunds;
                            FAmount = dtl.FBudgetAmount;
                            foreach (var dtlAdd in budgetAllData.BudgetDtlBudgetDtls)
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
                    if (budgetAllData.midYearEdit == "midYearEdit" && id != 0)
                    {
                        var budgetdtlimplplan = new List<BudgetDtlImplPlanModel>();
                        foreach (var item in findedresult.BudgetDtlImplPlans)
                        {
                            item.PersistentState = PersistentState.Added;
                            budgetdtlimplplan.Add(item);
                        }
                        var budgetdtltextcontent = new List<BudgetDtlTextContentModel>();
                        foreach (var item in findedresult.BudgetDtlTextContents)
                        {
                            item.PersistentState = PersistentState.Added;
                            budgetdtltextcontent.Add(item);
                        }
                        var budgetdtlfundappl = new List<BudgetDtlFundApplModel>();
                        foreach (var item in findedresult.BudgetDtlFundAppls)
                        {
                            item.PersistentState = PersistentState.Added;
                            budgetdtlfundappl.Add(item);
                        }
                        var budgetdtlbudgetdtl = new List<BudgetDtlBudgetDtlModel>();
                        foreach (var item in findedresult.BudgetDtlBudgetDtls)
                        {
                            item.PersistentState = PersistentState.Added;
                            budgetdtlbudgetdtl.Add(item);
                        }

                        var BudgetDtlPerformTarget = new List<BudgetDtlPerformTargetModel>();
                        foreach (var item in findedresult.BudgetDtlPerformTargets)
                        {
                            item.PersistentState = PersistentState.Added;
                            item.MstPhId = 0;
                            BudgetDtlPerformTarget.Add(item);
                        }
                        var BudgetDtlPurchaseDtl = new List<BudgetDtlPurchaseDtlModel>();
                        foreach (var item in findedresult.BudgetDtlPurchaseDtls)
                        {
                            item.PersistentState = PersistentState.Added;
                            BudgetDtlPurchaseDtl.Add(item);
                        }
                        var BudgetDtlPurDtl4SOF = new List<BudgetDtlPurDtl4SOFModel>();
                        foreach (var item in findedresult.BudgetDtlPurDtl4SOFs)
                        {
                            item.PersistentState = PersistentState.Added;
                            BudgetDtlPurDtl4SOF.Add(item);
                        }
                        //savedresult = 
                        var savedresult2 = BudgetMstService.SaveBudgetMst(findedresult.BudgetMst, budgetdtlimplplan, budgetdtltextcontent, budgetdtlfundappl, budgetdtlbudgetdtl, BudgetDtlPerformTarget, BudgetDtlPurchaseDtl, BudgetDtlPurDtl4SOF, null);
                        savedresult.KeyCodes.Add(savedresult2.KeyCodes[0]);
                        //BudgetMstService.SaveBudgetMst(findedresultmst, budgetdtlimplplan, budgetdtltextcontent, budgetdtlfundappl, budgetdtlbudgetdtl, budgetdtlperformtargetgridinfo.AllRow);

                    }

                    //对应关系设置-预算库项目对应部门设置,对应关系存放在z_qtdygx中，dylx=’98’
                    var dwdm = budgetAllData.BudgetMst.FProjCode;
                    var dydm = budgetAllData.BudgetMst.FBudgetDept;
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
            var root = System.Web.Hosting.HostingEnvironment.MapPath("~/UpLoadFiles/BudgetMst/");
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

                        string b_urlpath = "/UpLoadFiles/BudgetMst/" + date + "/" + newFileName;

                        QtAttachmentModel attachmentModel = new QtAttachmentModel();
                        attachmentModel.BName = b_name;
                        attachmentModel.BSize = decimal.Round((decimal)stream.Length / 1024, 2);
                        attachmentModel.BTable = "YS3_BUDGETMST";
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
                    //projectPhid = long.Parse(value);
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
                return DCHelper.ErrorMessage("预算主表保存有误！");
            }
            try
            {
                SavedResult<long> savedResult = new SavedResult<long>();
                //原有的附件要删除
                IList<QtAttachmentModel> oldAttachments = new List<QtAttachmentModel>();
                oldAttachments = this.QtAttachmentService.Find(t => t.BTable == "YS3_BUDGETMST" && t.RelPhid == projectPhid).Data;
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
                        att.BTable = "YS3_BUDGETMST";
                        att.PersistentState = PersistentState.Added;
                    }
                    //savedResult = this.QtAttachmentService.Save<long>(attachmentModels, "");
                }
                if (oldattachmentModels != null && oldattachmentModels.Count > 0)
                {
                    foreach (var oldAtt in oldattachmentModels)
                    {
                        oldAtt.RelPhid = projectPhid;
                        oldAtt.BTable = "YS3_BUDGETMST";
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
        /// 获取年中调整单个详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetYSBudgetMst([FromUri]BudgetMstListsRequestModel param)
        {
            if(param == null || param.FBudgetPhId == 0)
            {
                return DCHelper.ErrorMessage("参数传递不正确！");
            }
            try
            {
                var dicSysset = new Dictionary<string, object>();
                new CreateCriteria(dicSysset).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                var syssets = QTSysSetService.Find(dicSysset).Data.ToList();

                BudgetAllDataModel projectAllData = new BudgetAllDataModel();
                projectAllData.BudgetMst = BudgetMstService.Find(param.FBudgetPhId).Data;

                //返回对象增加附件
                if (projectAllData.BudgetMst != null)
                {
                    projectAllData.BudgetAttachments = QtAttachmentService.Find(t => t.BTable == "YS3_BUDGETMST" && t.RelPhid == projectAllData.BudgetMst.PhId).Data.ToList();
                }

                var FDeclarationDept = CorrespondenceSettingsService.GetOrg(projectAllData.BudgetMst.FDeclarationDept);
                if (FDeclarationDept != null)
                {
                    //申报部门代码转名称
                    projectAllData.BudgetMst.FDeclarationDept_EXName = FDeclarationDept.OName;
                }
                var syssetProjectMst = syssets.FindAll(x => x.DicType == "ProjectProper" && x.Orgcode == projectAllData.BudgetMst.FDeclarationUnit && x.TypeCode == projectAllData.BudgetMst.FProjAttr);
                if (syssetProjectMst.Count > 0)
                {
                    //项目属性代码转名称
                    projectAllData.BudgetMst.FProjAttr_EXName = syssetProjectMst[0].TypeName;
                }
                var syssetProjectMst2 = syssets.FindAll(x => x.DicType == "TimeLimit" && x.Orgcode == projectAllData.BudgetMst.FDeclarationUnit && x.TypeCode == projectAllData.BudgetMst.FDuration);
                if (syssetProjectMst2.Count > 0)
                {
                    //存续期限代码转名称
                    projectAllData.BudgetMst.FDuration_EXName = syssetProjectMst2[0].TypeName;
                }

                var syssetProjectMst3 = syssets.FindAll(x => x.DicType == "ProjectLevel" && x.Orgcode == projectAllData.BudgetMst.FDeclarationUnit && x.TypeCode == projectAllData.BudgetMst.FLevel);
                if (syssetProjectMst3.Count > 0)
                {
                    //项目级别代码转名称
                    projectAllData.BudgetMst.FLevel_EXName = syssetProjectMst3[0].TypeName;
                }

                projectAllData.BudgetDtlImplPlans = BudgetMstService.FindBudgetDtlImplPlanByForeignKey(param.FBudgetPhId).Data.ToList();
                projectAllData.BudgetDtlTextContents = BudgetMstService.FindBudgetDtlTextContentByForeignKey(param.FBudgetPhId).Data.ToList();
                //if (findedresultprojectdtltextcontent != null)
                //{
                //    if (findedresultprojectdtltextcontent.Data.Count > 0)
                //    {
                //        ProjectDtlTextContentModel singleData = findedresultprojectdtltextcontent.Data[0];
                //        FindedResult<ProjectDtlTextContentModel> result = new FindedResult<ProjectDtlTextContentModel>(false, singleData);
                //        return DataConverterHelper.ResponseResultToJson(result);
                //    }
                //}
                projectAllData.BudgetDtlFundAppls = BudgetMstService.FindBudgetDtlFundApplByForeignKey(param.FBudgetPhId).Data.ToList();
                projectAllData.BudgetDtlBudgetDtls = BudgetMstService.FindBudgetDtlBudgetDtlByForeignKey(param.FBudgetPhId).Data.ToList();
                projectAllData.BudgetDtlBudgetDtls = projectAllData.BudgetDtlBudgetDtls.OrderBy(t => t.FDtlCode).ToList();
                foreach (var ProjectDtlBudgetDtl in projectAllData.BudgetDtlBudgetDtls)
                {
                    //支付方式代码转名称
                    var syssetProjectDtlBudgetDtl = syssets.FindAll(x => x.DicType == "PayMethodTwo" && x.Orgcode == projectAllData.BudgetMst.FDeclarationUnit && x.TypeCode == ProjectDtlBudgetDtl.FPaymentMethod);
                    if (syssetProjectDtlBudgetDtl.Count > 0)
                    {
                        ProjectDtlBudgetDtl.FPaymentMethod_EXName = syssetProjectDtlBudgetDtl[0].TypeName;
                    }

                }
                projectAllData.BudgetDtlPurchaseDtls = BudgetMstService.FindBudgetDtlPurchaseDtlByForeignKey(param.FBudgetPhId).Data.ToList();
                projectAllData.BudgetDtlPurDtl4SOFs = BudgetMstService.FindBudgetDtlPurDtl4SOFByForeignKey(param.FBudgetPhId).Data.ToList();
                projectAllData.BudgetDtlPerformTargets = BudgetMstService.FindBudgetDtlPerformTargetByForeignKey(param.FBudgetPhId).Data.ToList();
                OrganizeModel organize = this.BudgetMstService.GetOrganizeByCode(projectAllData.BudgetMst.FDeclarationUnit);
                if (organize != null)
                {
                    projectAllData.BudgetDtlPerformTargets = BudgetMstService.GetNewBudPerformTargets(projectAllData.BudgetDtlPerformTargets, projectAllData.BudgetMst.FPerformType, organize.PhId, organize.OCode);
                }
                //预算的绩效跟踪
                projectAllData.JxTrackings = BudgetMstService.FindJxTrackingByForeignKey(param.FBudgetPhId).Data.OrderByDescending(t=>t.FTime).ToList();
                return DataConverterHelper.SerializeObject(projectAllData);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 年中调整删除接口
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostDeleteYSBudgetMst([FromBody]BudgetMstListsRequestModel param)
        {
            if (param == null || param.FBudgetPhId == 0)
            {
                return DCHelper.ErrorMessage("参数传递不正确！");
            }
            try
            {
                //删除时,对应关系里相关数据也删除
                var findedresultmst = BudgetMstService.Find(param.FBudgetPhId);
                if(findedresultmst.Data == null)
                {
                    return DCHelper.ErrorMessage("删除的数据不存在！");
                }
                else
                {
                    if(findedresultmst.Data.FApproveStatus != "1" && findedresultmst.Data.FApproveStatus != "5")
                    {
                        return DCHelper.ErrorMessage("只能删除未送审以及暂存的数据！");
                    }
                }

                var deletedresult = BudgetMstService.Delete<System.Int64>(param.FBudgetPhId);

                //删除年中调整数据时,恢复上一版本数据(如果是只做过一次调整,则去掉年初预算里的调整标志,删除相关年中调整数据)
                if ((findedresultmst.Data.FType == "c" && findedresultmst.Data.FVerNo == "0002") || (findedresultmst.Data.FType == "z" && findedresultmst.Data.FVerNo == "0002"))
                {
                    var dicWhere1 = new Dictionary<string, object>(); //年初新增
                    var XmMstPhid = findedresultmst.Data.XmMstPhid;                  
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
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 预算界面的上报按钮
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostReportYSBudgetMst([FromBody]BudgetMstListsRequestModel param)
        {
            if (param == null || param.FBudgetPhId == 0 || param.BudgetDtlTextContent == null)
            {
                return DCHelper.ErrorMessage("参数传递不正确！");
            }
            try
            {
                SavedResult<long> savedResult = new SavedResult<long>();
                var findedresultmst = BudgetMstService.Find(param.FBudgetPhId);
                if (findedresultmst.Data == null)
                {
                    return DCHelper.ErrorMessage("上报的数据不存在！");
                }
                else
                {
                    //预算上报要进行进度控制的判定
                    string budList = findedresultmst.Data.FBudgetDept;
                    string yearList = findedresultmst.Data.FYear;
                    if (!string.IsNullOrEmpty(budList) && !string.IsNullOrEmpty(yearList))
                    {
                        var budProcess = this.BudgetProcessCtrlService.Find(t => t.FDeptCode == budList && t.FYear == yearList).Data;
                        if (budProcess != null && budProcess.Count > 0)
                        {
                            if (budProcess[0] != null && budProcess[0].FProcessStatus != "3")
                            {
                                throw new Exception("有单据的预算部门的进度已不在年中调整，因此无法上报！");
                            }
                        }
                    }
                    if (findedresultmst.Data.FApproveStatus != "1" && findedresultmst.Data.FApproveStatus != "3")
                    {
                        return DCHelper.ErrorMessage("只能上报未送审或者已审批通过的数据！");
                    }
                    if (findedresultmst.Data.FProjStatus == 3)//项目执行的单据上报只需要修改审批状态
                    {
                        findedresultmst.Data.FApproveStatus = "3";
                        if(findedresultmst.Data.FType.ToString().ToLower() == "z" && findedresultmst.Data.FVerNo == "0001")
                        {
                            findedresultmst.Data.FProjStatus = 10;//项目状态改成年中新增执行
                        }
                        else{
                            findedresultmst.Data.FProjStatus = 11;//项目状态改成年中项目执行
                        }
                    }
                    else if(findedresultmst.Data.FProjStatus == 4)//项目调整的数据需要修改审批状态与项目状态
                    {
                        findedresultmst.Data.FApproveStatus = "3";
                        findedresultmst.Data.FProjStatus = 5;//项目状态改成调整项目执行
                    }
                    else if(findedresultmst.Data.FProjStatus == 9)
                    {
                        if (findedresultmst.Data.FType.ToString().ToLower() == "z" && findedresultmst.Data.FVerNo == "0001")
                        {
                            findedresultmst.Data.FProjStatus = 10;//项目状态改成年中新增执行
                        }
                        else
                        {
                            findedresultmst.Data.FProjStatus = 5;//项目状态改成年中项目执行
                        }
                    }
                    findedresultmst.Data.FMeetiingSummaryNo = param.FMeetiingSummaryNo;
                    findedresultmst.Data.FMeetingTime = DateTime.Now;                   
                    findedresultmst.Data.PersistentState = PersistentState.Modified;
                    //savedResult = BudgetMstService.Save<Int64>(findedresultmst.Data, "");

                    var budgetDtlTextContent = this.BudgetMstService.FindBudgetDtlTextContentByForeignKey(findedresultmst.Data.PhId).Data;
                    if(budgetDtlTextContent != null && budgetDtlTextContent.Count > 0)
                    {
                        foreach(var bud in budgetDtlTextContent)
                        {
                            bud.FBz = param.BudgetDtlTextContent.FBz;
                            bud.FLeadingOpinions = param.BudgetDtlTextContent.FLeadingOpinions;
                            bud.FChairmanOpinions = param.BudgetDtlTextContent.FChairmanOpinions;
                            bud.FResolution = param.BudgetDtlTextContent.FResolution;
                            bud.PersistentState = PersistentState.Modified;
                        }
                        savedResult = BudgetMstService.SaveBudgetMst(findedresultmst.Data, null, budgetDtlTextContent.ToList(), null, null, null);
                    }
                    else
                    {
                        savedResult = BudgetMstService.SaveBudgetMst(findedresultmst.Data, null, null, null, null, null);
                    }
                    //同步数据到老G6H
                    this.BudgetMstService.AddDataInSaveBudgetMst(findedresultmst.Data, BudgetMstService.FindBudgetDtlBudgetDtlByForeignKey(findedresultmst.Data.PhId).Data.ToList());
                }
                return DataConverterHelper.SerializeObject(savedResult);
            }
            catch(Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        ///// <summary>
        ///// 预算界面的上报按钮(批量上报)
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public string PostReportYSBudgetMstList([FromBody]BudgetMstListsRequestModel param)
        //{
        //    if (param == null || param.FBudgetPhIdList == null || param.FBudgetPhIdList.Count <= 0)
        //    {
        //        return DCHelper.ErrorMessage("参数传递不正确！");
        //    }
        //    try
        //    {
        //        SavedResult<long> savedResult = new SavedResult<long>();
        //        var findedresultmst = BudgetMstService.Find(t=>param.FBudgetPhIdList.Contains(t.PhId));
        //        if (findedresultmst.Data == null)
        //        {
        //            return DCHelper.ErrorMessage("上报的数据不存在！");
        //        }
        //        else
        //        {
        //            if (findedresultmst.Data.FApproveStatus != "1" && findedresultmst.Data.FApproveStatus != "3")
        //            {
        //                return DCHelper.ErrorMessage("只能上报未送审或者已审批通过的数据！");
        //            }

        //            if (findedresultmst.Data.FProjStatus == 3)//项目执行的单据上报只需要修改审批状态
        //            {
        //                findedresultmst.Data.FApproveStatus = "3";
        //            }
        //            else if (findedresultmst.Data.FProjStatus == 4)//项目调整的数据需要修改审批状态与项目状态
        //            {
        //                findedresultmst.Data.FApproveStatus = "3";
        //                findedresultmst.Data.FProjStatus = 5;//项目状态改成调整项目执行
        //            }
        //            findedresultmst.Data.PersistentState = PersistentState.Modified;
        //            savedResult = BudgetMstService.Save<Int64>(findedresultmst.Data, "");
        //        }
        //        return DataConverterHelper.SerializeObject(savedResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        return DCHelper.ErrorMessage(ex.Message);
        //    }
        //}

        /// <summary>
        /// 预算上报时保存附件
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
            var root = System.Web.Hosting.HostingEnvironment.MapPath("~/UpLoadFiles/BudgetDtlTextContent/");
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

                        string b_urlpath = "/UpLoadFiles/BudgetDtlTextContent/" + date + "/" + newFileName;

                        QtAttachmentModel attachmentModel = new QtAttachmentModel();
                        attachmentModel.BName = b_name;
                        attachmentModel.BSize = decimal.Round((decimal)stream.Length / 1024, 2);
                        attachmentModel.BTable = "YS3_BUDGETDTL_TEXTCONTENT";
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
                    if (key == "OldAttachments")
                    {
                        var value2 = JsonConvert.DeserializeObject<List<QtAttachmentModel>>(value);
                        attachmentModels.AddRange(value2);
                    }

                    //string value = await content.ReadAsStringAsync();
                    //取项目主键
                    projectPhid = long.Parse(value);
                }
            }

            if (projectPhid <= 0)
            {
                return DCHelper.ErrorMessage("预算主表保存有误！");
            }
            try
            {
                SavedResult<long> savedResult = new SavedResult<long>();
                //原有的附件要删除
                IList<QtAttachmentModel> oldAttachments = new List<QtAttachmentModel>();
                oldAttachments = this.QtAttachmentService.Find(t => t.BTable == "YS3_BUDGETDTL_TEXTCONTENT" && t.RelPhid == projectPhid).Data;
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
                        att.BTable = "YS3_BUDGETDTL_TEXTCONTENT";
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
        /// 得到批量打印的数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string PostPrintData([FromBody]BaseListModel paramters)
        {
            if (paramters.fPhIdList == null || paramters.fPhIdList.Count() < 1)
            {
                return DCHelper.ErrorMessage("传递的单据集合有误！");
            }

            return DataConverterHelper.SerializeObject(BudgetMstService.PostPrintData(paramters.fPhIdList));
        }

        /// <summary>
        /// 上报时取项目预执行时保存的附件
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetAttach([FromUri]BudgetMstModel param)
        {
            var result= QtAttachmentService.Find(t => t.BTable == "XM3_PROJECTDTL_TEXTCONTENT" && t.RelPhid == param.XmMstPhid).Data.ToList();

            return DCHelper.ModelListToJson<QtAttachmentModel>(result, (Int32)result.Count);
        }

        #region//用款计划相关的
        /// <summary>
        /// 项目支出预算情况查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetXmZcYs([FromUri]ExpenseAllRequestModel param)
        {
            if(string.IsNullOrEmpty(param.UserCode))
            {
                return DCHelper.ErrorMessage("用户编码不能为空！");
            }
            try
            {
                var result = BudgetMstService.GetXmZcYs2(param.UserCode.ToString());
                var data = result.Results;
                if(data != null && data.Count > 0)
                {
                    //预算部门的筛选条件
                    if (!string.IsNullOrEmpty(param.FBudgetDept))
                    {
                        data = data.ToList().FindAll(t => t.FBudgetDept == param.FBudgetDept);
                    }
                    //搜索框搜索
                    if (!string.IsNullOrEmpty(param.Search))
                    {
                        data = data.ToList().FindAll(t => t.FProjName.Contains(param.Search));
                    }
                    //核定预算数
                    if (!string.IsNullOrEmpty(param.FBudgetAmountMax))
                    {
                        data = data.ToList().FindAll(t => t.FBudgetAmount <= decimal.Parse(param.FBudgetAmountMax));
                    }
                    if (!string.IsNullOrEmpty(param.FBudgetAmountMin))
                    {
                        data = data.ToList().FindAll(t => t.FBudgetAmount >= decimal.Parse(param.FBudgetAmountMin));
                    }
                    //已编报数
                    if (!string.IsNullOrEmpty(param.FProjAmountMax))
                    {
                        data = data.ToList().FindAll(t => t.FProjAmount <= decimal.Parse(param.FProjAmountMax));
                    }
                    if (!string.IsNullOrEmpty(param.FProjAmountMin))
                    {
                        data = data.ToList().FindAll(t => t.FProjAmount >= decimal.Parse(param.FProjAmountMin));
                    }
                    //if (param.FProjAmountMax > param.FProjAmountMin)
                    //{
                    //    data = data.ToList().FindAll(t => t.FProjAmount <= param.FProjAmountMax && t.FProjAmount >= param.FProjAmountMin);
                    //}
                    //剩余可编报数
                    if (!string.IsNullOrEmpty(param.FSurplusAmountMax))
                    {
                        data = data.ToList().FindAll(t => t.FSurplusAmount <= decimal.Parse(param.FSurplusAmountMax));
                    }
                    if (!string.IsNullOrEmpty(param.FSurplusAmountMin))
                    {
                        data = data.ToList().FindAll(t => t.FSurplusAmount >= decimal.Parse(param.FSurplusAmountMin));
                    }
                    //if (param.FSurplusAmountMax > param.FSurplusAmountMin)
                    //{
                    //    data = data.ToList().FindAll(t => t.FSurplusAmount <= param.FSurplusAmountMax && t.FSurplusAmount >= param.FSurplusAmountMin);
                    //}
                    //实际发生数
                    if (!string.IsNullOrEmpty(param.FHappenAmountMax))
                    {
                        data = data.ToList().FindAll(t => t.FHappenAmount <= decimal.Parse(param.FHappenAmountMax));
                    }
                    if (!string.IsNullOrEmpty(param.FHappenAmountMin))
                    {
                        data = data.ToList().FindAll(t => t.FHappenAmount >= decimal.Parse(param.FHappenAmountMin));
                    }
                    //if (param.FHappenAmountMax > param.FHappenAmountMin)
                    //{
                    //    data = data.ToList().FindAll(t => t.FHappenAmount <= param.FHappenAmountMax && t.FHappenAmount >= param.FHappenAmountMin);
                    //}
                }

                var data1 = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "数据获取成功",
                    Data = data.Skip((param.PageIndex - 1) * param.PageSize).Take(param.PageSize).ToList(),
                    Count = data.Count,
                };
                return DataConverterHelper.SerializeObject(data1);
                //return DataConverterHelper.EntityListToJson<BudgetMstModel>(data.Skip((param.PageIndex - 1) * param.PageSize).Take(param.PageSize).ToList(), (Int32)data.Count);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
        #endregion

        #region//预执行

        /// <summary>
        /// (年中调整)根据主键集合预执行（并同步数据到老G6H）
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostExecute([FromBody]BaseListModel paramters)
        {
            if (paramters.fPhIdList == null || paramters.fPhIdList.Count() < 1)
            {
                return DCHelper.ErrorMessage("传递的单据集合有误！");
            }
            try
            {
                IList<BudgetMstModel> MstList = new List<BudgetMstModel>();
                SavedResult<long> savedResult = new SavedResult<long>();
                //var SuccessNum = 0;
                //var FailNum = 0;
                MstList = BudgetMstService.Find(t => paramters.fPhIdList.Contains(t.PhId.ToString())).Data;
                if (MstList != null && MstList.Count > 0)
                {
                    foreach (var mst in MstList)
                    {
                        if ((mst.FProjStatus == 3 || mst.FProjStatus == 4))
                        {

                            mst.FProjStatus = 9;
                            mst.FApproveDate = DateTime.Now;
                            mst.FApprover = base.AppInfo.UserId;
                            mst.PersistentState = PersistentState.Modified;
                            //MstList.Add(mst);
                            //SuccessNum++;
                        }
                        else
                        {
                            return DCHelper.ErrorMessage("只有项目执行与项目调整的单据可以进行预执行！");
                            //FailNum++;
                        }
                    }
                    //if (SuccessNum > 0)
                    //{
                    //    try
                    //    {
                    //        this.BudgetMstService.Save<Int64>(MstList, "");
                    //    }
                    //    catch (Exception ex)
                    //    {

                    //    }
                    //}

                    //先同步数据到老G6H
                    this.BudgetMstService.AddData2(MstList.Select(t => t.PhId).ToList());

                    savedResult = this.BudgetMstService.Save<Int64>(MstList, "");
                }
                //var result = new
                //{
                //    SuccessNum = SuccessNum,
                //    FailNum = FailNum
                //};
                return DataConverterHelper.SerializeObject(savedResult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
            
        }
        #endregion


        #region//绩效跟踪相关
        /// <summary>
        /// 保存绩效跟踪集合
        /// </summary>
        /// <param name="budgetAllData"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostSaveJxTracks([FromBody]BudgetAllDataRequest budgetAllData)
        {
            try
            {
                SavedResult<long> savedResult = new SavedResult<long>();
                if(budgetAllData.JxTrackings != null && budgetAllData.JxTrackings.Count > 0)
                {
                    foreach(var track in budgetAllData.JxTrackings)
                    {
                        if(track.PhId == 0)
                        {
                            track.PersistentState = PersistentState.Added;
                            //track.FTime = DateTime.Now;
                        }
                        else
                        {
                            if(track.PersistentState != PersistentState.Deleted)
                            {
                                track.PersistentState = PersistentState.Modified;
                            }
                        }
                        
                    }
                    //每两百条保存一次
                    List<JxTrackingModel> newList = new List<JxTrackingModel>();
                    for(int i= 0 ; i < budgetAllData.JxTrackings.Count; i++)
                    {
                        if((i+1)% 200 == 0)
                        {
                            newList.Add(budgetAllData.JxTrackings[i]);
                            savedResult = this.BudgetMstService.SaveJxTracks(newList);
                            newList.Clear();
                        }
                        else
                        {
                            newList.Add(budgetAllData.JxTrackings[i]);
                        }
                    }
                    if(newList != null && newList.Count > 0)
                    {
                        savedResult = this.BudgetMstService.SaveJxTracks(newList);
                    }
                    //savedResult = this.BudgetMstService.SaveJxTracks(budgetAllData.JxTrackings);
                }
                return DataConverterHelper.SerializeObject(savedResult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
        #endregion

        #region//项目预算调整分析表
        /// <summary>
        /// 获取项目预算分析调整表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostBudgetAdjustAnalyseList([FromBody]BudgetAdjustModel param)
        {
            if (string.IsNullOrEmpty(param.Year))
            {
                return DCHelper.ErrorMessage("年份信息不能为空！");
            }
            if (string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织信息不能为空");
            }
            try
            {
                var result = this.BudgetMstService.GetBudgetAdjustAnalyseList(param.Year, param.orgCode);
                List<BudgetAdjustAnalyseModel> result2 = new List<BudgetAdjustAnalyseModel>();
                if(result != null && result.Count > 0)
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
                    if(result != null && result.Count > 0)
                    {
                        //先进行排序
                        result = result.OrderBy(t => t.FDeclarationUnit).ThenBy(t => t.FBudgetDept).ThenBy(t => t.FDeclarationDept).ThenBy(t => t.FProjCode).ThenBy(t => t.FDtlCode).ToList();
                        //再附上序号
                        int row = 1;
                        foreach(var res in result)
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
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        public string PostBudgetAdjustAnalyseListExecl([FromBody]BudgetAdjustModel param)
        {
            if (string.IsNullOrEmpty(param.Year))
            {
                return DCHelper.ErrorMessage("年份信息不能为空！");
            }
            if (string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织信息不能为空");
            }
            if (param.uid == 0)
            {
                return DCHelper.ErrorMessage("用户信息不能为空");
            }
            try
            {
                var result = this.BudgetMstService.GetBudgetAdjustAnalyseList(param.Year, param.orgCode);
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
                //组织信息
                OrganizeModel organize = this.BudgetMstService.GetOrganizeByCode(param.orgCode);
                if(organize == null)
                {
                    return DCHelper.ErrorMessage("组织不存在！");
                }
                //用户信息
                User2Model user = this.BudgetMstService.GetUser(param.uid);
                if(user == null)
                {
                    return DCHelper.ErrorMessage("用户不存在！");
                }
                //当前用户调整分析表自定义数据
                IList<QtTableCustomizeModel> qtTableCustomizes = new List<QtTableCustomizeModel>();

                qtTableCustomizes = this.QtTableCustomizeService.Find(t => t.UserId == param.uid && t.TableCode == "BudgetAdjustAnalyse").Data;

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
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
        #endregion


        [HttpPost]
        public string Postceshi([FromBody]BudgetAllDataRequest budgetAllData)
        {
            try
            {
                budgetAllData = new BudgetAllDataRequest();
                budgetAllData.BudgetMst = new BudgetMstModel();
                budgetAllData.BudgetMst.PersistentState = PersistentState.Added;
                var result = this.BudgetMstService.Save<long>(budgetAllData.BudgetMst, "");
                return DataConverterHelper.SerializeObject(result);
            }
            catch(Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        #region//民生银行
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
        /// 根据预算主表数据补充明细数据
        /// </summary>
        /// <param name="projectMst"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetMSYHBudgetMstList([FromUri]BudgetMstListsRequestModel projectMst)
        {
            if (string.IsNullOrEmpty(projectMst.OrgCode))
            {
                return DCHelper.ErrorMessage("单位编码不能为空！");
            }
            if (string.IsNullOrEmpty(projectMst.Ucode) || string.IsNullOrEmpty(projectMst.UserId))
            {
                return DCHelper.ErrorMessage("用户信息不能为空！");
            }
            if (string.IsNullOrEmpty(projectMst.Year))
            {
                return DCHelper.ErrorMessage("年度不能为空！");
            }
            try
            {
                //年份与单位编码筛选（排除掉已被删除的数据）
                Dictionary<string, object> dic = new Dictionary<string, object>();
                new CreateCriteria(dic)
                        .Add(ORMRestrictions<string>.Eq("FYear", projectMst.Year))
                        .Add(ORMRestrictions<string>.Eq("FDeclarationUnit", projectMst.OrgCode))
                        .Add(ORMRestrictions<byte>.Eq("FDeleteMark", (byte)0));
                //项目类型条件筛选
                if (projectMst.FProjStatus == 3)//项目执行
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
                        .Add(ORMRestrictions<Int32>.Eq("FProjStatus", 3));
                }
                else if (projectMst.FProjStatus == 4) //项目调整
                {
                    new CreateCriteria(dic)//.Add(ORMRestrictions<Int32>.Eq("FProjStatus", 2))
                       .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<Int32>.Eq("FProjStatus", 4));
                }
                else if (projectMst.FProjStatus == 5)//项目调整执行
                {
                    new CreateCriteria(dic)//.Add(ORMRestrictions<Int32>.Eq("FProjStatus", 2))
                       .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<Int32>.Eq("FProjStatus", 5));
                }
                else if (projectMst.FProjStatus == 12)
                {
                    new CreateCriteria(dic)//.Add(ORMRestrictions<Int32>.Eq("FProjStatus", 2))
                       .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<Int32>.Eq("FProjStatus", 12));
                }
                else if (projectMst.FProjStatus == 10)
                {
                    new CreateCriteria(dic)//.Add(ORMRestrictions<Int32>.Eq("FProjStatus", 2))
                       .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)).Add(ORMRestrictions<Int32>.Eq("FProjStatus", 10));
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
                if (projectMst.FProjAmountBegin > 0)//项目金额开始
                {
                    new CreateCriteria(dic).Add(ORMRestrictions<Decimal>.Ge("FProjAmount", projectMst.FProjAmountBegin));
                }
                if (projectMst.FProjAmountEnd > 0)//项目金额结束
                {
                    new CreateCriteria(dic).Add(ORMRestrictions<Decimal>.Le("FProjAmount", projectMst.FProjAmountEnd));
                }
                //if (!string.IsNullOrEmpty(projectMst.FProjAmountBegin))
                //{
                //    new CreateCriteria(dic)
                //        .Add(ORMRestrictions<Decimal>.Ge("FProjAmount", Decimal.Parse(projectMst.FProjAmountBegin)));
                //}
                //if (!string.IsNullOrEmpty(projectMst.FProjAmountEnd))
                //{
                //    new CreateCriteria(dic)
                //        .Add(ORMRestrictions<Decimal>.Le("FProjAmount", Decimal.Parse(projectMst.FProjAmountEnd)));
                //}
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
                if (!string.IsNullOrEmpty(projectMst.UserId))
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<long>.Eq("FDeclarerId", long.Parse(projectMst.UserId)));
                }

                //var result = this.ProjectMstService.LoadWithPage(projectMst.PageIndex, projectMst.PageSize, dic, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" });

                var result = this.BudgetMstService.Find(dic, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" }).Data;
                //取可选相同审批流是数据集合
                if (projectMst.ProcPhid != 0)
                {
                    var proList = this.BudgetMstService.Find(dic, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" }).Data;
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

                    var businesses = syssets.FindAll(x => x.DicType == "Business" && x.Orgcode == data.FDeclarationUnit && x.TypeCode == data.FBusinessCode);
                    if (businesses != null && businesses.Count > 0)
                    {
                        //业务条线转名称
                        data.FBusinessName = businesses[0].TypeName;
                    }
                }
                List<BudgetAllDataModel> projectAllDatas = new List<BudgetAllDataModel>();
                if (result != null && result.Count > 0)
                {
                    projectAllDatas = this.BudgetMstService.GetBudegtAllDataModels(result.ToList());
                }

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
        /// 保存预算调整与年中新增
        /// </summary>
        /// <param name="budgetAllData"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostSaveMSYHBudget([FromBody]BudgetAllDataRequest budgetAllData)
        {
            if (budgetAllData == null || budgetAllData.BudgetAllDataModels == null || budgetAllData.BudgetAllDataModels.Count <= 0)
            {
                return DCHelper.ErrorMessage("保存的参数不能为空！");
            }
            if (budgetAllData.uid == 0)
            {
                return DCHelper.ErrorMessage("用户信息不能为空！");
            }
            try
            {
                //获取当前所有数据库中所有已存在的单据编码
                List<string> allCodes = new List<string>();
                allCodes = this.ProjectMstService.Find(t => t.PhId != 0 && t.FDeleteMark == (byte)0).Data.Select(t => t.FProjCode).ToList();

                //保留要保存数据
                List<BudgetAllDataModel> budgetAlls = new List<BudgetAllDataModel>();

                //保存信息
                SavedResult<Int64> savedresult = new SavedResult<Int64>();

                foreach (var budgetData in budgetAllData.BudgetAllDataModels)
                {
                    BudgetAllDataModel budgetAll = new BudgetAllDataModel();

                    BudgetMstModel mstforminfo = new BudgetMstModel();
                    if (budgetData.BudgetMst != null)
                    {
                        mstforminfo = budgetData.BudgetMst;
                        //预算金额与项目金额一般一样
                        mstforminfo.FBudgetAmount = mstforminfo.FProjAmount;
                    }

                    List<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtls = new List<BudgetDtlBudgetDtlModel>();
                    if (budgetAllData.BudgetDtlBudgetDtls != null)
                    {
                        budgetDtlBudgetDtls = budgetAllData.BudgetDtlBudgetDtls;
                    }
                    BudgetDtlBudgetDtlModel budgetDtlBudgetDtl = new BudgetDtlBudgetDtlModel();
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
                            mstforminfo.FProjStatus = 12;
                            mstforminfo.FDeclarerId = budgetAllData.uid;
                        }
                        else
                        {
                            if (mstforminfo.PersistentState != PersistentState.Deleted)
                            {
                                mstforminfo.PersistentState = PersistentState.Modified;
                                mstforminfo.FProjStatus = 4;
                                mstforminfo.FDeclarerId = budgetAllData.uid;
                                if (!string.IsNullOrEmpty(mstforminfo.FApproveStatus) && mstforminfo.FApproveStatus != "1" && mstforminfo.FApproveStatus != "4")
                                {
                                    return DCHelper.ErrorMessage("审批以及审批成功的项目立项单据不能进行修改！");
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(mstforminfo.FApproveStatus) && mstforminfo.FApproveStatus != "1" && mstforminfo.FApproveStatus != "4")
                                {
                                    return DCHelper.ErrorMessage("审批以及审批成功的项目立项单据不能进行删除！");
                                }
                            }
                        }
                    }
                    //明细表数据整理
                    if (budgetDtlBudgetDtls != null && budgetDtlBudgetDtls.Count > 0)
                    {
                        List<BudgetDtlBudgetDtlModel> projectDtls1 = new List<BudgetDtlBudgetDtlModel>();
                        List<BudgetDtlBudgetDtlModel> projectDtls2 = new List<BudgetDtlBudgetDtlModel>();
                        List<BudgetDtlBudgetDtlModel> projectDtls3 = new List<BudgetDtlBudgetDtlModel>();
                        foreach (var pro in budgetDtlBudgetDtls)
                        {
                            pro.FAmountAfterEdit = pro.FAmount + pro.FAmountEdit;
                            pro.FBudgetAmount = pro.FAmountAfterEdit;
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
                        projectDtls1.AddRange(projectDtls2);
                        projectDtls1.AddRange(projectDtls3);
                        budgetDtlBudgetDtls = projectDtls1;
                    }

                    //申报进度
                    if (string.IsNullOrEmpty(mstforminfo.FType)) //项目保存时，如果没有进度状态，则增加
                    {

                        var processStatus = BudgetProcessCtrlService.FindBudgetProcessCtrl(mstforminfo.FDeclarationUnit, mstforminfo.FBudgetDept, mstforminfo.FYear);
                        //单位进度控制为1时，是年初申报，为3时，为年中调整
                        if (processStatus == "3")
                        {
                            if(mstforminfo.PhId == 0)
                            {
                                mstforminfo.FType = "z";
                                mstforminfo.FVerNo = "0001";
                            }
                            else
                            {
                                mstforminfo.FType = "c";
                                mstforminfo.FVerNo = "0001";
                            }
                            
                            //mstforminfo.FVerNo = "0001";
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
                        if (processStatus != "3")
                        {
                            return DCHelper.ErrorMessage("此组织的进度已不在年初，无法修改年初数据！");
                        }
                        else
                        {
                            if(mstforminfo.FType == "z" && mstforminfo.FVerNo == "0001")
                            {
                                return DCHelper.ErrorMessage("年中新增数据不能进行年中调整！");
                            }
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
                        mstforminfo.FDeclarationDept = ProjectMstService.GetDefaultDept(budgetAllData.uid);
                        //申报人id
                        mstforminfo.FDeclarerId = budgetAllData.uid;
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
                    if (budgetDtlBudgetDtls != null && budgetDtlBudgetDtls.Count > 0)
                    {
                        //项目总金额等于明细金额的总和
                        mstforminfo.FBudgetAmount = budgetDtlBudgetDtls.ToList().FindAll(t => t.PersistentState != PersistentState.Deleted).Sum(t => t.FAmountAfterEdit);
                        mstforminfo.FProjAmount = budgetDtlBudgetDtls.ToList().FindAll(t => t.PersistentState != PersistentState.Deleted).Sum(t => t.FAmountAfterEdit);

                        //暂存该项目下有效的明细编码
                        List<string> alldtlCodes = budgetDtlBudgetDtls.ToList().FindAll(t => t.PersistentState != PersistentState.Deleted && !string.IsNullOrEmpty(t.FDtlCode)).Select(t => t.FDtlCode).ToList();
                        for (var i = 0; i < budgetDtlBudgetDtls.Count; i++)
                        {
                            if (budgetDtlBudgetDtls[i].PersistentState == PersistentState.Deleted)
                            {
                                continue;
                            }
                            dtlCode = budgetDtlBudgetDtls[i].FDtlCode;
                            dtlName = budgetDtlBudgetDtls[i].FName;
                            if (string.IsNullOrEmpty(dtlCode))
                            {
                                if (alldtlCodes != null && alldtlCodes.Count > 0)
                                {
                                    budgetDtlBudgetDtls[i].FDtlCode = projCode + string.Format("{0:D4}", int.Parse(alldtlCodes.Max().Substring(alldtlCodes.Max().Length - 4, 4)) + 1);
                                }
                                else
                                {
                                    budgetDtlBudgetDtls[i].FDtlCode = projCode + string.Format("{0:D4}", 1);
                                }
                            }
                            dtlCode = budgetDtlBudgetDtls[i].FDtlCode;
                            alldtlCodes.Add(dtlCode);

                        }
                    }
                    #endregion
                    budgetAll.BudgetMst = mstforminfo;
                    budgetAll.BudgetDtlBudgetDtls = budgetDtlBudgetDtls;

                    budgetAlls.Add(budgetAll);

                }
                if (budgetAlls != null && budgetAlls.Count > 0)
                {
                    foreach (var pro in budgetAlls)
                    {
                        //保存数据
                        savedresult = BudgetMstService.SaveBudgetMst(pro.BudgetMst, null, null, null, pro.BudgetDtlBudgetDtls, null, null, null, null);
                    }
                }
                return DataConverterHelper.SerializeObject(savedresult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }

        }

        #endregion
    }
}
