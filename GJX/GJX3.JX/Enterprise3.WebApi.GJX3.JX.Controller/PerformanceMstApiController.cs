using Enterprise3.Common.Base.Criterion;
using Enterprise3.Common.Model.Results;
using Enterprise3.WebApi.ApiControllerBase;
using Enterprise3.WebApi.ApiControllerBase.Models;
using Enterprise3.WebApi.GJX3.JX.Model.Request;
using GData3.Common.Utils;
using GData3.Common.Utils.Filters;
using GJX3.JX.Model.Domain;
using GJX3.JX.Model.Enums;
using GJX3.JX.Service.Interface;
using GQT3.QT.Model.Domain;
using GQT3.QT.Service.Interface;
using GYS3.YS.Model.Domain;
using GYS3.YS.Model.Extra;
using GYS3.YS.Service.Interface;
using Newtonsoft.Json;
using SUP.Common.Base;
using SUP.Common.DataAccess;
using SUP.Common.DataEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Enterprise3.WebApi.GJX3.JX.Controller
{
    /// <summary>
    /// PerformanceMstApi控制处理类
    /// </summary>
    [MethodExceptionFilter, SyncUserDbFilter]
    public class PerformanceMstApiController : ApiBase
    {
        IPerformanceMstService PerformanceMstService { get; set; }

        IBudgetMstService BudgetMstService { get; set; }
        ICorrespondenceSettingsService CorrespondenceSettingsService { get; set; }

        IPerformEvalTargetTypeService PerformEvalTargetTypeService { get; set; }

        IQTSysSetService QTSysSetService { get; set; }

        IQtAttachmentService QtAttachmentService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PerformanceMstApiController()
        {
            PerformanceMstService = base.GetObject<IPerformanceMstService>("GJX3.JX.Service.PerformanceMst");
            BudgetMstService = base.GetObject<IBudgetMstService>("GYS3.YS.Service.BudgetMst");
            CorrespondenceSettingsService = base.GetObject<ICorrespondenceSettingsService>("GQT3.QT.Service.CorrespondenceSettings");
            PerformEvalTargetTypeService = base.GetObject<IPerformEvalTargetTypeService>("GQT3.QT.Service.PerformEvalTargetType");
            QtAttachmentService = base.GetObject<IQtAttachmentService>("GQT3.QT.Service.QtAttachment");
            QTSysSetService = base.GetObject<IQTSysSetService>("GQT3.QT.Service.QTSysSet");
        }

        /// <summary>
        /// 获取预算可以自评或者抽评的数据（及页面右上角的列表）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetBudgetMstList([FromUri]BudgetAbout param)
        {
            if (string.IsNullOrEmpty(param.Ucode))
            {
                return DCHelper.ErrorMessage("用户编码不能为空！");
            }
            //if (string.IsNullOrEmpty(param.FBudgetDept))
            //{
            //    return DCHelper.ErrorMessage("预算部门编码不能为空！");
            //}
            if (string.IsNullOrEmpty(param.Year))
            {
                return DCHelper.ErrorMessage("年度信息不能为空！");
            }
            try
            {
                var dicWhereDept = new Dictionary<string, object>();
                var dicWhere = new Dictionary<string, object>();
                //用户对应的拥有权限的预算部门
                if (!string.IsNullOrEmpty(param.Ucode))
                {
                    new CreateCriteria(dicWhereDept)
                    .Add(ORMRestrictions<string>.Eq("Dwdm", param.Ucode)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
                    var deptList = CorrespondenceSettingsService.Find(dicWhereDept);
                    List<string> deptL = new List<string>();
                    for (var i = 0; i < deptList.Data.Count; i++)
                    {
                        deptL.Add(deptList.Data[i].Dydm);
                    }
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
                }
                //新增年度条件
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", param.Year));   
                //审批通过的单据才能自评
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FApproveStatus", "3"));   //审核通过
                if (!string.IsNullOrEmpty(param.FBudgetDept))
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FBudgetDept", param.FBudgetDept));
                }

                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<Enum>.Eq("FIfPerformanceAppraisal", GYS3.YS.Model.Enums.EnumYesNo.Yes))           //是否绩效评价
                    .Add(ORMRestrictions<int>.Eq("FLifeCycle", 0))                                     //版本标识
                    .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));                            //单据调整判断 (0表示最新的数据)

                var result = BudgetMstService.LoadWithPage(param.PageIndex, param.PageSize, dicWhere, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" });
                foreach (var data in result.Results)
                {
                    if (string.IsNullOrEmpty(data.FAccount))
                    {
                        data.FActualAmount = decimal.Round(0, 2); ;
                    }
                    else
                    {
                        data.FActualAmount = decimal.Round(decimal.Parse(BudgetMstService.GetSJFSSbyXMCode(data.FAccount, data.FProjCode)), 2);
                    }                  
                    data.FBalanceAmount = decimal.Round(data.FProjAmount - data.FActualAmount, 2);
                    data.FImplRate = decimal.Round(data.FActualAmount * 100 / data.FProjAmount, 2);
                }

                var data1 = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "获取成功！",
                    Data = result.Results,
                    Count = (Int32)result.TotalItems,
                };
                return DataConverterHelper.SerializeObject(data1);
                //return DataConverterHelper.EntityListToJson<BudgetMstModel>(result.Results, (Int32)result.TotalItems);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 获取由单个预算数据组装成的绩效数据（加入一些必要的数据）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetBudgetMst([FromUri]BaseListModel param)
        {
            if (param.fBudgetPhid == 0)
            {
                return DCHelper.ErrorMessage("预算主键不能为空！");
            }
            try
            {
                //var dicSysset = new Dictionary<string, object>();
                //new CreateCriteria(dicSysset).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                //var syssets = QTSysSetService.Find(dicSysset).Data.ToList();

                PerformanceAllData performanceAll = new PerformanceAllData();
                BudgetMstModel budgetMst = new BudgetMstModel();
                budgetMst = BudgetMstService.Find(param.fBudgetPhid).Data;
                if(budgetMst != null)
                {
                    PerformanceMstModel performanceMst = new PerformanceMstModel();
                    //自评主对象
                    performanceMst.YSMstPhId = budgetMst.PhId;
                    performanceMst.FProjCode = budgetMst.FProjCode;
                    performanceMst.FProjName = budgetMst.FProjName;
                    performanceMst.FDeclarationUnit = budgetMst.FDeclarationUnit;
                    performanceMst.FDeclarationUnit_EXName = budgetMst.FDeclarationUnit_EXName;
                    performanceMst.FDeclarationDept = budgetMst.FDeclarationDept;
                    performanceMst.FBudgetDept = budgetMst.FBudgetDept;
                    performanceMst.FBudgetDept_EXName = budgetMst.FBudgetDept_EXName;
                    performanceMst.FProjAttr = budgetMst.FProjAttr;
                    performanceMst.FDuration = budgetMst.FDuration;
                    performanceMst.FStartDate = budgetMst.FStartDate;
                    performanceMst.FEndDate = budgetMst.FEndDate;
                    performanceMst.FProjAmount = budgetMst.FBudgetAmount;
                    performanceMst.FIfPerformanceAppraisal = (int)budgetMst.FIfPerformanceAppraisal;
                    performanceMst.FIfKeyEvaluation = (int)budgetMst.FIfKeyEvaluation;
                    performanceMst.FMeetingTime = budgetMst.FMeetingTime;
                    performanceMst.FMeetiingSummaryNo = budgetMst.FMeetiingSummaryNo;
                    performanceMst.FExpenseCategory = budgetMst.FExpenseCategory;
                    performanceMst.FPerformType = budgetMst.FPerformType;
                    if (string.IsNullOrEmpty(budgetMst.FAccount))
                    {
                        performanceMst.FActualAmount = decimal.Round(0, 2); ;
                    }
                    else
                    {
                        performanceMst.FActualAmount = decimal.Round(decimal.Parse(BudgetMstService.GetSJFSSbyXMCode(budgetMst.FAccount, performanceMst.FProjCode)), 2);
                    }
                    //performanceMst.FActualAmount = decimal.Round(decimal.Parse(BudgetMstService.GetSJFSSbyXMCode(budgetMst.FAccount, performanceMst.FProjCode)), 2);
                    performanceMst.FBalanceAmount = decimal.Round(performanceMst.FProjAmount - performanceMst.FActualAmount, 2);
                    performanceMst.FImplRate = decimal.Round(performanceMst.FActualAmount * 100 / (performanceMst.FProjAmount==0?1: performanceMst.FProjAmount), 2);

                    performanceAll.PerformanceMst = performanceMst;

                    //明细对象集合
                    var findedresultbudgetdtlbudgetdtl = BudgetMstService.FindBudgetDtlBudgetDtlByForeignKey(param.fBudgetPhid);
                    var listDtl = findedresultbudgetdtlbudgetdtl.Data;

                    //组装model
                    List<PerformanceDtlBuDtlModel> dtlList = new List<PerformanceDtlBuDtlModel>();

                    if (listDtl != null && listDtl.Count > 0)
                    {
                        foreach (var item in listDtl)
                        {
                            var index = dtlList.FindIndex(t => t.FDtlCode == item.FDtlCode);

                            if (index != -1)
                            {
                                dtlList[index].FBudgetAmount += item.FAmount;

                            }
                            else
                            {
                                decimal FActualAmount = 0;
                                if (string.IsNullOrEmpty(budgetMst.FAccount))
                                {
                                    FActualAmount = decimal.Round(0, 2); ;
                                }
                                else
                                {
                                    FActualAmount = decimal.Round(decimal.Parse(BudgetMstService.GetSJFSSbyMXCode(budgetMst.FAccount, item.FDtlCode)), 2);
                                }
                                //var FActualAmount = decimal.Round(decimal.Parse(BudgetMstService.GetSJFSSbyMXCode(budgetMst.FAccount, item.FDtlCode)), 2);
                                //((parseFloat(upDate.FActualAmount) / parseFloat(upDate.FBudgetAmount)) * 100).toFixed(2)
                                var FImplRate = decimal.Round(FActualAmount * 100 / (item.FBudgetAmount==0?1: item.FBudgetAmount), 2);
                                PerformanceDtlBuDtlModel model = new PerformanceDtlBuDtlModel()
                                {
                                    DelPhid = item.PhId,
                                    FDtlCode = item.FDtlCode,
                                    FName = item.FName,
                                    FSourceOfFunds = item.FSourceOfFunds,
                                    FSourceOfFunds_EXName = item.FSourceOfFunds_EXName,
                                    FExpensesChannel_EXName = item.FExpensesChannel_EXName,
                                    FBudgetAmount = item.FBudgetAmount,
                                    FActualAmount = FActualAmount,
                                    FBalanceAmount = decimal.Round(item.FBudgetAmount - FActualAmount, 2),
                                    FImplRate = FImplRate
                                };

                                dtlList.Add(model);
                            }
                        }
                    }

                    performanceAll.PerformanceDtlBuDtls = dtlList;
                    //绩效明细
                    var results = BudgetMstService.FindBudgetDtlPerformTargetByForeignKey(param.fBudgetPhid);
                    IList<PerformanceDtlTarImplModel> tarImplList = PerformanceMstService.ConvertData2(results.Data, performanceMst);

                    performanceAll.PerformanceDtlTarImpls = tarImplList;
                }

                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "获取成功！",
                    Data = performanceAll
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }


        /// <summary>
        /// 根据预算主键获取已保存的所有自评与抽评数据(包括审批未审批，上报已上报的数据)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetPerformanceList([FromUri]BaseListModel param)
        {
            if(param.fBudgetPhid == 0)
            {
                return DCHelper.ErrorMessage("预算主键不能为空！");
            }
            try
            {
                //同一预算所有自评与抽评数据集合
                IList<PerformanceMstModel> AllPerfors = this.PerformanceMstService.Find(t => t.YSMstPhId == param.fBudgetPhid).Data;
                //自评数据
                IList<PerformanceMstModel> ZPerfors = new List<PerformanceMstModel>();
                //抽评数据
                IList<PerformanceMstModel> CPerfors = new List<PerformanceMstModel>();
                //第三方数据
                IList<PerformanceMstModel> SPerfors = new List<PerformanceMstModel>();
                if (AllPerfors != null && AllPerfors.Count > 0)
                {
                    RichHelpDac helpdac = new RichHelpDac();
                    //helpdac.CodeToName<PerformanceMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
                    //helpdac.CodeToName<PerformanceMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
                    //helpdac.CodeToName<PerformanceMstModel>(pageResult.Results, "FProjName", "FProjName_EXName", "xm3_xmlist", "");
                    helpdac.CodeToName<PerformanceMstModel>(AllPerfors, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist", "");
                    helpdac.CodeToName<PerformanceMstModel>(AllPerfors, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
                    helpdac.CodeToName<PerformanceMstModel>(AllPerfors, "FDeclarationDept", "FDeclarationDept_EXName", "ys_orglist", "");
                    helpdac.CodeToName<PerformanceMstModel>(AllPerfors, "FEvaluationDept", "FEvaluationDept_EXName", "ys_orglist", "");
                    helpdac.CodeToName<PerformanceMstModel>(AllPerfors, "FInformant", "FInformantName", "fg3_user", "");
                    //helpdac.CodeToName<PerformanceMstModel>(AllPerfors, "FPerformType", "FPerformType_EXName", "GHPerformEvalTargetTypeTree", "");
                    helpdac.CodeToName<PerformanceMstModel>(AllPerfors, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory", "");
                    //根据申报组织获取绩效基础数据，为后续数据准备
                    var allTypes = this.PerformEvalTargetTypeService.Find(t => t.Orgcode == AllPerfors[0].FDeclarationUnit).Data;
                    if(allTypes != null && allTypes.Count > 0)
                    {
                        foreach(var perfor in AllPerfors)
                        {
                            var typeModel1 = allTypes.ToList().FindAll(t => t.FCode == perfor.FPerformType);
                            if (typeModel1 != null && typeModel1.Count > 0)
                            {
                                perfor.FPerformType_EXName = typeModel1[0].FName;
                            }
                        }
                    }


                    //Ftype(1-自评，2-抽评, 3-第三方)
                    ZPerfors = AllPerfors.ToList().FindAll(t => t.FType == EnumPerType.Self.ToString()).OrderByDescending(t=>t.NgInsertDt).ToList();
                    CPerfors = AllPerfors.ToList().FindAll(t => t.FType == EnumPerType.Review.ToString()).OrderByDescending(t => t.NgInsertDt).ToList();
                    SPerfors = AllPerfors.ToList().FindAll(t => t.FType == EnumPerType.Third.ToString()).OrderByDescending(t => t.NgInsertDt).ToList();
                }
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "获取成功！",
                    AllPerfors = AllPerfors,
                    ZPerfors = ZPerfors,
                    CPerfors = CPerfors,
                    SPerfors = SPerfors,
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 根据绩效主键获取单个绩效的数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetPerformanceMst([FromUri]BaseListModel param)
        {
            if(param.performancePhid == 0)
            {
                return DCHelper.ErrorMessage("绩效主键不能为空！");
            }
            try
            {
                var result = this.PerformanceMstService.GetPerformanceMst(param.performancePhid);

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
        /// 自评，抽评保存
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostSave([FromBody]PerformanceAllData param)
        {
            if(param.PerformanceMst == null || param.PerformanceDtlTarImpls == null || param.PerformanceDtlBuDtls == null || param.PerformanceDtlTextConts == null)
            {
                return DCHelper.ErrorMessage("传递数据不能为空！");
            }
            //if(param.fBudgetPhid == 0 && param.PerformanceDtlTarImpls != null && param.PerformanceDtlTarImpls.Count > 0)
            //{
            //    return DCHelper.ErrorMessage("预算主键不能为空！");
            //}
            try
            {
                //List<PerformanceDtlTarImplModel> performanceDtlTarImplModels = new List<PerformanceDtlTarImplModel>();
                //if (param.fBudgetPhid > 0)
                //{
                //    var results = BudgetMstService.FindBudgetDtlPerformTargetByForeignKey(param.fBudgetPhid);
                //    performanceDtlTarImplModels = PerformanceMstService.ConvertSaveData(results.Data, param.PerformanceDtlTarImpls);
                //}
                SavedResult<Int64> savedresult = new SavedResult<Int64>();
                if(param.PerformanceMst.PhId == 0)
                {
                    //主键为0，则是新增，要判断原单据审批类型是否是1或3
                    param.PerformanceMst.PersistentState = PersistentState.Added;
                }
                else
                {
                    param.PerformanceMst.PersistentState = PersistentState.Modified;
                    //若是修改，则要判断原单据审批类型是否是1或3
                    var perfors = this.PerformanceMstService.Find(t => t.PhId == param.PerformanceMst.PhId).Data;
                    if(perfors != null && perfors.Count == 1)
                    {
                        if(perfors[0].FAuditStatus == EnumPerStatus.Check.ToString() || perfors[0].FAuditStatus == EnumPerStatus.Valid.ToString() || perfors[0].FAuditStatus == EnumPerStatus.Third.ToString())
                        {
                            return DCHelper.ErrorMessage("原单据已审核或已上报，无法进行修改！");
                        }
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("原单据不存在！");
                    }
                }
                if ((param.PerformanceMst.FAuditStatus != EnumPerStatus.NoCheck.ToString() && param.PerformanceMst.FType == EnumPerType.Self.ToString()) || (param.PerformanceMst.FAuditStatus != EnumPerStatus.NoValid.ToString() && param.PerformanceMst.FType == EnumPerType.Review.ToString()) || (param.PerformanceMst.FAuditStatus != EnumPerStatus.NoThird.ToString() && param.PerformanceMst.FType == EnumPerType.Third.ToString()))
                {
                    return DCHelper.ErrorMessage("只能新增与修改审批状态是未审批与未上报的单据，而要与单据状态相对应！");
                }
                if(param.PerformanceDtlTextConts != null && param.PerformanceDtlTextConts.Count > 0)
                {
                    foreach (var cont in param.PerformanceDtlTextConts)
                    {
                        if(cont.PhId == 0)
                        {
                            cont.PersistentState = PersistentState.Added;
                        }
                        else
                        {
                            if(cont.PersistentState != PersistentState.Deleted)
                            {
                                cont.PersistentState = PersistentState.Modified;
                            }
                        }
                    }
                }
                if(param.PerformanceDtlBuDtls != null && param.PerformanceDtlBuDtls.Count > 0)
                {
                    foreach(var dtl in param.PerformanceDtlBuDtls)
                    {
                        if(dtl.PhId == 0)
                        {
                            dtl.PersistentState = PersistentState.Added;
                        }
                        else
                        {
                            if(dtl.PersistentState != PersistentState.Deleted)
                            {
                                dtl.PersistentState = PersistentState.Modified;
                            }
                        }
                    }
                }
                if(param.PerformanceDtlTarImpls != null && param.PerformanceDtlTarImpls.Count > 0)
                {
                    foreach(var imp in param.PerformanceDtlTarImpls)
                    {
                        if(imp.PhId == 0)
                        {
                            imp.PersistentState = PersistentState.Added;
                        }
                        else
                        {
                            if(imp.PersistentState != PersistentState.Deleted)
                            {
                                imp.PersistentState = PersistentState.Modified;
                            }
                        }
                    }
                }
                savedresult = PerformanceMstService.SavePerformanceMst(param.PerformanceMst, param.PerformanceDtlTextConts.ToList(), param.PerformanceDtlBuDtls.ToList(), param.PerformanceDtlTarImpls.ToList());
                return DataConverterHelper.SerializeObject(savedresult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 自评，抽评，第三方评价加入附件信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PostSave2()
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
            var root = System.Web.Hosting.HostingEnvironment.MapPath("~/UpLoadFiles/PerformanceMst/"); 
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

                        string b_urlpath = "/UpLoadFiles/PerformanceMst/" + date + "/" + newFileName;

                        QtAttachmentModel attachmentModel = new QtAttachmentModel();
                        attachmentModel.BName = b_name;
                        attachmentModel.BSize = decimal.Round((decimal)stream.Length / 1024, 2);
                        attachmentModel.BTable = "JX3_PERFORMANCEMST";
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
                oldAttachments = this.QtAttachmentService.Find(t => t.BTable == "JX3_PERFORMANCEMST" && t.RelPhid == projectPhid).Data;
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
                        att.BTable = "JX3_PERFORMANCEMST";
                        att.PersistentState = PersistentState.Added;
                    }
                }
                if (oldattachmentModels != null && oldattachmentModels.Count > 0)
                {
                    foreach (var oldAtt in oldattachmentModels)
                    {
                        oldAtt.RelPhid = projectPhid;
                        oldAtt.BTable = "JX3_PERFORMANCEMST";
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
        /// 删除未上报自评与未审批抽评的绩效单据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostDelete([FromBody]BaseListModel param)
        {
            if(param.performancePhid == 0)
            {
                return DCHelper.ErrorMessage("绩效主键不能为空！");
            }
            try
            {
                var perforMst = this.PerformanceMstService.Find(t => t.PhId == param.performancePhid).Data;
                if(perforMst != null && perforMst.Count == 1)
                {
                    if(perforMst[0].FAuditStatus != EnumPerStatus.NoCheck.ToString() && perforMst[0].FAuditStatus != EnumPerStatus.NoValid.ToString() && perforMst[0].FAuditStatus != EnumPerStatus.NoThird.ToString())
                    {
                        return DCHelper.ErrorMessage("只有未上报和未审批的绩效单据可以删除！");
                    }
                }
                else
                {
                    return DCHelper.ErrorMessage("绩效信息查询失败！");
                }
                var deletedresult = PerformanceMstService.Delete<long>(param.performancePhid);

                return DataConverterHelper.SerializeObject(deletedresult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 上报未上报的绩效自评
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostCheck([FromBody]BaseListModel param)
        {
            if (param.performancePhid == 0)
            {
                return DCHelper.ErrorMessage("绩效主键不能为空！");
            }
            if(param.UserId == 0)
            {
                return DCHelper.ErrorMessage("上报人信息不能为空！");
            }
            try
            {
                var perforMst = this.PerformanceMstService.Find(t => t.PhId == param.performancePhid).Data;
                if (perforMst != null && perforMst.Count == 1)
                {
                    if (perforMst[0].FAuditStatus != EnumPerStatus.NoCheck.ToString()|| perforMst[0].FType != EnumPerType.Self.ToString())
                    {
                        return DCHelper.ErrorMessage("只有未上报的绩效自评单据可以上报！");
                    }
                    var oldPerfors = this.PerformanceMstService.Find(t => t.YSMstPhId == perforMst[0].YSMstPhId && t.FType == EnumPerType.Self.ToString() && t.FAuditStatus == EnumPerStatus.Check.ToString()).Data;
                    if(oldPerfors != null && oldPerfors.Count > 0)
                    {
                        return DCHelper.ErrorMessage("已存在已上报的自评绩效单据，不能进行上报！");
                    }
                    perforMst[0].FType = EnumPerType.Self.ToString();
                    perforMst[0].FAuditStatus = EnumPerStatus.Check.ToString();
                    perforMst[0].PersistentState = PersistentState.Modified;
                    perforMst[0].FAuditor = param.UserId;
                    perforMst[0].FAuditDate = DateTime.Now;
                    var result = PerformanceMstService.Save<long>(perforMst[0], "");

                    return DataConverterHelper.SerializeObject(result);
                }
                else
                {
                    return DCHelper.ErrorMessage("绩效信息查询失败！");
                }
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 审批未审批的抽评绩效
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostValid([FromBody]BaseListModel param)
        {
            if (param.performancePhid == 0)
            {
                return DCHelper.ErrorMessage("绩效主键不能为空！");
            }
            if (param.UserId == 0)
            {
                return DCHelper.ErrorMessage("上报人信息不能为空！");
            }
            try
            {
                var perforMst = this.PerformanceMstService.Find(t => t.PhId == param.performancePhid).Data;
                if (perforMst != null && perforMst.Count == 1)
                {
                    if (perforMst[0].FAuditStatus != EnumPerStatus.NoValid.ToString() || perforMst[0].FType != EnumPerType.Review.ToString())
                    {
                        return DCHelper.ErrorMessage("只有未审批的绩效抽评可以审批！");
                    }
                    var oldPerfors = this.PerformanceMstService.Find(t => t.YSMstPhId == perforMst[0].YSMstPhId && t.FType == EnumPerType.Review.ToString() && t.FAuditStatus == EnumPerStatus.Valid.ToString()).Data;
                    if (oldPerfors != null && oldPerfors.Count > 0)
                    {
                        return DCHelper.ErrorMessage("已存在已审核的抽评绩效单据，不能进行审核！");
                    }
                    perforMst[0].FType = EnumPerType.Review.ToString();
                    perforMst[0].FAuditStatus = EnumPerStatus.Valid.ToString();
                    perforMst[0].PersistentState = PersistentState.Modified;
                    perforMst[0].FAuditor = param.UserId;
                    perforMst[0].FAuditDate = DateTime.Now;
                    var result = PerformanceMstService.Save<long>(perforMst[0], "");

                    return DataConverterHelper.SerializeObject(result);
                }
                else
                {
                    return DCHelper.ErrorMessage("绩效信息查询失败！");
                }
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 保存绩效抽评的第三方评价
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostSaveThird([FromBody]PerformanceAllData param)
        {
            if(param.performancePhid == 0)
            {
                return DCHelper.ErrorMessage("第三方评价的绩效主键不能为空！");
            }
            try
            {
                var perforMsts = this.PerformanceMstService.Find(t => t.PhId == param.performancePhid).Data;
                if(perforMsts != null && perforMsts.Count == 1)
                {
                    if(perforMsts[0].FType == EnumPerType.Review.ToString() && (perforMsts[0].FAuditStatus == EnumPerStatus.NoValid.ToString() || perforMsts[0].FAuditStatus == EnumPerStatus.Valid.ToString()))
                    {
                        SavedResult<long> savedResult = new SavedResult<long>();
                        savedResult = this.PerformanceMstService.SaveThird(perforMsts[0], param.ThirdAttachmentModels);
                        return DataConverterHelper.SerializeObject(savedResult);
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("只有绩效抽评单据可以进行第三方评价！");
                    }
                }
                else
                {
                    return DCHelper.ErrorMessage("绩效信息查询失败！");
                }
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }


        /// <summary>
        /// 另一个保存第三方评价附件的接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PostSaveThird2()
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
            var root = System.Web.Hosting.HostingEnvironment.MapPath("~/UpLoadFiles/PerformanceMst/");
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

                        string b_urlpath = "/UpLoadFiles/PerformanceMst/" + date + "/" + newFileName;

                        QtAttachmentModel attachmentModel = new QtAttachmentModel();
                        attachmentModel.BName = b_name;
                        attachmentModel.BSize = decimal.Round((decimal)stream.Length / 1024, 2);
                        attachmentModel.BTable = "JX3_THIRDATTACHMENT";
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
                oldAttachments = this.QtAttachmentService.Find(t => t.BTable == "JX3_THIRDATTACHMENT" && t.RelPhid == projectPhid).Data;
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
                        att.BTable = "JX3_THIRDATTACHMENT";
                        att.PersistentState = PersistentState.Added;
                    }
                }
                if (oldattachmentModels != null && oldattachmentModels.Count > 0)
                {
                    foreach (var oldAtt in oldattachmentModels)
                    {
                        oldAtt.RelPhid = projectPhid;
                        oldAtt.BTable = "JX3_THIRDATTACHMENT";
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
        /// 点击自评按钮时进行判断，只有未上报的才能新增自评
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostJudgeSelf([FromBody]BaseListModel param)
        {
            if(param.fBudgetPhid == 0)
            {
                return DCHelper.ErrorMessage("预算主键不能为空！");
            }
            try
            {
                var selfs = this.PerformanceMstService.Find(t => t.YSMstPhId == param.fBudgetPhid && t.FType == EnumPerType.Self.ToString() && t.FAuditStatus == EnumPerStatus.Check.ToString()).Data;
                if(selfs != null && selfs.Count > 0)
                {
                    return DCHelper.ErrorMessage("已存在已上报的自评数据，不能再新增自评！");
                }
                else
                {
                    return DCHelper.SuccessMessage("请新增自评！");
                }
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 点击抽评按钮时进行判断，只有未审核的且上报过自评的才能新增抽评
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostJudgeReview([FromBody]BaseListModel param)
        {
            if (param.fBudgetPhid == 0)
            {
                return DCHelper.ErrorMessage("预算主键不能为空！");
            }
            try
            {
                var selfs = this.PerformanceMstService.Find(t => t.YSMstPhId == param.fBudgetPhid && t.FType == EnumPerType.Self.ToString() && t.FAuditStatus == EnumPerStatus.Check.ToString()).Data;
                if(selfs != null && selfs.Count > 0)
                {
                    var reviews = this.PerformanceMstService.Find(t => t.YSMstPhId == param.fBudgetPhid && t.FType == EnumPerType.Review.ToString() && t.FAuditStatus == EnumPerStatus.Valid.ToString()).Data;
                    if (reviews != null && reviews.Count > 0)
                    {
                        return DCHelper.ErrorMessage("已存在已审核的抽评数据，不能再新增抽评！");
                    }
                    else
                    {
                        var result = this.PerformanceMstService.GetPerformanceMst(selfs[0].PhId);
                        if(result != null)
                        {
                            if(result.PerformanceMst != null)
                            {
                                result.PerformanceMst.PhId = 0;
                                result.PerformanceMst.FType = EnumPerType.Review.ToString();
                                result.PerformanceMst.FAuditStatus = EnumPerStatus.NoValid.ToString();
                            }
                            if(result.PerformanceDtlBuDtls != null && result.PerformanceDtlBuDtls.Count > 0)
                            {
                                foreach(var dtl in result.PerformanceDtlBuDtls)
                                {
                                    dtl.PhId = 0;
                                    dtl.MstPhid = 0;
                                }
                            }
                            if (result.PerformanceDtlTarImpls != null && result.PerformanceDtlTarImpls.Count > 0)
                            {
                                foreach (var imp in result.PerformanceDtlTarImpls)
                                {
                                    imp.PhId = 0;
                                    imp.MstPhid = 0;
                                }
                            }
                            if (result.PerformanceDtlTextConts != null && result.PerformanceDtlTextConts.Count > 0)
                            {
                                foreach (var cont in result.PerformanceDtlTextConts)
                                {
                                    cont.PhId = 0;
                                    cont.MstPhid = 0;
                                }
                            }
                        }

                        var data = new
                        {
                            Status = ResponseStatus.Success,
                            Msg = "获取成功！",
                            Data = result
                        };
                        return DataConverterHelper.SerializeObject(data);
                    }
                }
                else
                {
                    return DCHelper.ErrorMessage("只有自评已上报的单据可以进行新增抽评！");
                }
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 点击第三方评价按钮时进行判断，只有未第三方评价的且审核过抽评的才能新增第三方评价
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostThirdJudgeReview([FromBody]BaseListModel param)
        {
            if (param.fBudgetPhid == 0)
            {
                return DCHelper.ErrorMessage("预算主键不能为空！");
            }
            try
            {
                var selfs = this.PerformanceMstService.Find(t => t.YSMstPhId == param.fBudgetPhid && t.FType == EnumPerType.Review.ToString() && t.FAuditStatus == EnumPerStatus.Valid.ToString()).Data;
                if (selfs != null && selfs.Count > 0)
                {
                    var reviews = this.PerformanceMstService.Find(t => t.YSMstPhId == param.fBudgetPhid && t.FType == EnumPerType.Third.ToString() && t.FAuditStatus == EnumPerStatus.Third.ToString()).Data;
                    if (reviews != null && reviews.Count > 0)
                    {
                        return DCHelper.ErrorMessage("已存在已审核的第三方数据，不能再新增第三方评价！");
                    }
                    else
                    {
                        var result = this.PerformanceMstService.GetPerformanceMst(selfs[0].PhId);
                        if (result != null)
                        {
                            if (result.PerformanceMst != null)
                            {
                                result.PerformanceMst.PhId = 0;
                                result.PerformanceMst.FType = EnumPerType.Third.ToString();
                                result.PerformanceMst.FAuditStatus = EnumPerStatus.NoThird.ToString();
                            }
                            if (result.PerformanceDtlBuDtls != null && result.PerformanceDtlBuDtls.Count > 0)
                            {
                                foreach (var dtl in result.PerformanceDtlBuDtls)
                                {
                                    dtl.PhId = 0;
                                    dtl.MstPhid = 0;
                                }
                            }
                            if (result.PerformanceDtlTarImpls != null && result.PerformanceDtlTarImpls.Count > 0)
                            {
                                foreach (var imp in result.PerformanceDtlTarImpls)
                                {
                                    imp.PhId = 0;
                                    imp.MstPhid = 0;
                                }
                            }
                            if (result.PerformanceDtlTextConts != null && result.PerformanceDtlTextConts.Count > 0)
                            {
                                foreach (var cont in result.PerformanceDtlTextConts)
                                {
                                    cont.PhId = 0;
                                    cont.MstPhid = 0;
                                }
                            }
                        }

                        var data = new
                        {
                            Status = ResponseStatus.Success,
                            Msg = "获取成功！",
                            Data = result
                        };
                        return DataConverterHelper.SerializeObject(data);
                    }
                }
                else
                {
                    return DCHelper.ErrorMessage("只有抽评已审核的单据可以进行第三方评价！");
                }
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 第三方评价审核
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostThird([FromBody]BaseListModel param)
        {
            if (param.performancePhid == 0)
            {
                return DCHelper.ErrorMessage("绩效主键不能为空！");
            }
            if (param.UserId == 0)
            {
                return DCHelper.ErrorMessage("上报人信息不能为空！");
            }
            try
            {
                var perforMst = this.PerformanceMstService.Find(t => t.PhId == param.performancePhid).Data;
                if (perforMst != null && perforMst.Count == 1)
                {
                    if (perforMst[0].FAuditStatus != EnumPerStatus.NoThird.ToString() || perforMst[0].FType != EnumPerType.Third.ToString())
                    {
                        return DCHelper.ErrorMessage("只有未审批的第三方评价可以审批！");
                    }
                    var oldPerfors = this.PerformanceMstService.Find(t => t.YSMstPhId == perforMst[0].YSMstPhId && t.FType == EnumPerType.Third.ToString() && t.FAuditStatus == EnumPerStatus.Third.ToString()).Data;
                    if (oldPerfors != null && oldPerfors.Count > 0)
                    {
                        return DCHelper.ErrorMessage("已存在已审核的第三方评价绩效单据，不能进行审核！");
                    }
                    perforMst[0].FType = EnumPerType.Third.ToString();
                    perforMst[0].FAuditStatus = EnumPerStatus.Third.ToString();
                    perforMst[0].PersistentState = PersistentState.Modified;
                    perforMst[0].FAuditor = param.UserId;
                    perforMst[0].FAuditDate = DateTime.Now;
                    var result = PerformanceMstService.Save<long>(perforMst[0], "");

                    return DataConverterHelper.SerializeObject(result);
                }
                else
                {
                    return DCHelper.ErrorMessage("绩效信息查询失败！");
                }
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
    }
}