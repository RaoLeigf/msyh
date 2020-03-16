using Enterprise3.Common.Model.Results;
using Enterprise3.WebApi.ApiControllerBase;
using Enterprise3.WebApi.GYS3.YS.Model;
using Enterprise3.WebApi.GYS3.YS.Model.Request;
using GQT3.QT.Model.Domain;
using GQT3.QT.Service.Interface;
using GYS3.YS.Model.Domain;
using GYS3.YS.Service.Interface;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace Enterprise3.WebApi.GYS3.YS.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [MethodExceptionFilter]
    public class YsIncomeMstApiController: ApiBase
    {
        IYsIncomeMstService YsIncomeMstService { get; set; }

        ICorrespondenceSettings2Service CorrespondenceSettings2Service { get; set; }

        ICorrespondenceSettingsService CorrespondenceSettingsService { get; set; }

        IBudgetAccountsService BudgetAccountsService { get; set; }

        IBudgetProcessCtrlService BudgetProcessCtrlService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public YsIncomeMstApiController()
        {
            YsIncomeMstService = base.GetObject<IYsIncomeMstService>("GYS3.YS.Service.YsIncomeMst");
            CorrespondenceSettings2Service = base.GetObject<ICorrespondenceSettings2Service>("GQT3.QT.Service.CorrespondenceSettings2");
            CorrespondenceSettingsService = base.GetObject<ICorrespondenceSettingsService>("GQT3.QT.Service.CorrespondenceSettings");
            BudgetAccountsService = base.GetObject<IBudgetAccountsService>("GQT3.QT.Service.BudgetAccounts");
            BudgetProcessCtrlService = base.GetObject<IBudgetProcessCtrlService>("GYS3.YS.Service.BudgetProcessCtrl");
        }

        /// <summary>
        /// 获取单个组织的收入预算信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetYsIncome([FromUri]BaseListModel param)
        {
            if (param.orgid == 0 || string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织信息不能为空！");
            }
            if (string.IsNullOrEmpty(param.Year))
            {
                return DCHelper.ErrorMessage("年度信息不能为空！");
            }
            try
            {
                //存主信息
                YsIncomeMstModel ysIncomeMst = new YsIncomeMstModel();
                //预算部门集合（收入归属）
                IList<OrganizeModel> organizes = new List<OrganizeModel>();
                organizes = this.YsIncomeMstService.GetAllOrganize();
                //存明细信息
                IList<YsIncomeDtlModel> ysIncomeDtls = new List<YsIncomeDtlModel>();
                var ysIncomeMsts = this.YsIncomeMstService.Find(t => t.FOrgID == param.orgid && t.FYear == param.Year).Data;
                if(ysIncomeMsts != null && ysIncomeMsts.Count > 0)
                {
                    ysIncomeMst = ysIncomeMsts[0];
                    ysIncomeDtls = this.YsIncomeMstService.FindYsIncomeDtlByForeignKey(ysIncomeMsts[0].PhId).Data;
                }
                else
                {
                    ysIncomeMst.FOrgID = param.orgid;
                    ysIncomeMst.FOrgcode = param.orgCode;
                    ysIncomeMst.FYear = param.Year;
                    //预算科目的基本信息
                    IList<BudgetAccountsModel> budgetAccounts = new List<BudgetAccountsModel>();
                    budgetAccounts = this.BudgetAccountsService.Find(t => t.PhId > 0).Data;

                    if(budgetAccounts == null || budgetAccounts.Count <= 0)
                    {
                        return DCHelper.ErrorMessage("预算科目基础配置信息为空！");
                    }
                    //获取该组织的所对应的所有预算科目
                    IList<CorrespondenceSettingsModel> correspondenceSettingss = new List<CorrespondenceSettingsModel>();
                    correspondenceSettingss = this.CorrespondenceSettingsService.Find(t => t.Dylx == "02" && t.Dwdm == param.orgid.ToString()).Data;
                    if(correspondenceSettingss != null && correspondenceSettingss.Count > 0)
                    {

                        foreach(var corr in correspondenceSettingss)
                        {
                            YsIncomeDtlModel ysIncomeDtl = new YsIncomeDtlModel();
                            ysIncomeDtl.FSubjectCode = corr.Dydm;
                            ysIncomeDtl.FSubjectname = budgetAccounts.ToList().Find(t => t.KMDM == corr.Dydm) == null ? "" : budgetAccounts.ToList().Find(t => t.KMDM == corr.Dydm).KMMC;
                            ysIncomeDtl.FProcessStatus = "";//进度控制暂定
                            ysIncomeDtls.Add(ysIncomeDtl);
                        }
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("此单位对应的预算科目信息为空！");
                    }
                }
                if(ysIncomeDtls != null && ysIncomeDtls.Count > 0)
                {
                    foreach (var dtl in ysIncomeDtls)
                    {
                        if(ysIncomeDtls.ToList().Find(t=>t.FSubjectCode.StartsWith(dtl.FSubjectCode) && t.FSubjectCode != dtl.FSubjectCode) == null)
                        {
                            dtl.IsLast = 1;
                        }
                        if(organizes != null && organizes.Count > 0)
                        {
                            dtl.FBudgetName = organizes.ToList().Find(t => t.OCode == dtl.FBudgetcode) == null ? "" : organizes.ToList().Find(t => t.OCode == dtl.FBudgetcode).OName;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(param.Search) && ysIncomeDtls != null && ysIncomeDtls.Count > 0)
                {
                    ysIncomeDtls = ysIncomeDtls.ToList().FindAll(t => (t.FSubjectCode.Contains(param.Search) || t.FSubjectname.Contains(param.Search)));
                }
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "获取成功！",
                    Mst = ysIncomeMst,
                    Dtls = ysIncomeDtls.OrderBy(t=>t.FSubjectCode).ToList()
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 获取单个组织的收入预算信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetYsIncome2([FromUri]BaseListModel param)
        {
            if (param.FPhId == 0)
            {
                return DCHelper.ErrorMessage("主键信息不能为空！");
            }
            try
            {
                //存主信息
                YsIncomeMstModel ysIncomeMst = new YsIncomeMstModel();
                //预算部门集合（收入归属）
                IList<OrganizeModel> organizes = new List<OrganizeModel>();
                organizes = this.YsIncomeMstService.GetAllOrganize();
                //存明细信息
                IList<YsIncomeDtlModel> ysIncomeDtls = new List<YsIncomeDtlModel>();
                var ysIncomeMsts = this.YsIncomeMstService.Find(t => t.PhId == param.FPhId).Data;
                if (ysIncomeMsts != null && ysIncomeMsts.Count > 0)
                {
                    ysIncomeMst = ysIncomeMsts[0];
                    ysIncomeDtls = this.YsIncomeMstService.FindYsIncomeDtlByForeignKey(ysIncomeMsts[0].PhId).Data;
                }
                else
                {
                    return DCHelper.ErrorMessage("预算收入数据查找失败！");
                }
                if (ysIncomeDtls != null && ysIncomeDtls.Count > 0)
                {
                    foreach (var dtl in ysIncomeDtls)
                    {
                        if (ysIncomeDtls.ToList().Find(t => t.FSubjectCode.StartsWith(dtl.FSubjectCode) && t.FSubjectCode != dtl.FSubjectCode) == null)
                        {
                            dtl.IsLast = 1;
                        }
                        if (organizes != null && organizes.Count > 0)
                        {
                            dtl.FBudgetName = organizes.ToList().Find(t => t.OCode == dtl.FBudgetcode) == null ? "" : organizes.ToList().Find(t => t.OCode == dtl.FBudgetcode).OName;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(param.Search) && ysIncomeDtls != null && ysIncomeDtls.Count > 0)
                {
                    ysIncomeDtls = ysIncomeDtls.ToList().FindAll(t => (t.FSubjectCode.Contains(param.Search) || t.FSubjectname.Contains(param.Search)));
                }
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "获取成功！",
                    Mst = ysIncomeMst,
                    Dtls = ysIncomeDtls.OrderBy(t => t.FSubjectCode).ToList()
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 保存单个组织的收入预算
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostSave([FromBody]AllYsIncomeRequestModel param)
        {
            if(param.YsIncomeMst == null || param.YsIncomeDtls == null || param.YsIncomeDtls.Count <= 0)
            {
                return DCHelper.ErrorMessage("传递的收入预算信息不能为空！");
            }
            if(param.orgid == 0 || param.YsIncomeMst.FOrgID == 0)
            {
                return DCHelper.ErrorMessage("组织信息不能为空！");
            }
            if(string.IsNullOrEmpty(param.Year) || string.IsNullOrEmpty(param.YsIncomeMst.FYear))
            {
                return DCHelper.ErrorMessage("年度信息不能为空！");
            }
            try
            {
                if ((param.YsIncomeMst.FApproval != 0 && param.YsIncomeMst.FApproval != 2)
                    || param.YsIncomeMst.FIsbudget != 0)
                {
                    return DCHelper.ErrorMessage("只有待送审,未生成预算的收入预算可以进行修改！");
                }
                IList<YsIncomeDtlModel> ysIncomeDtls = new List<YsIncomeDtlModel>();
                //先进行数据调整
                if (param.YsIncomeMst.PhId == 0)
                {
                    param.YsIncomeMst.FDeclareTime = DateTime.Now;
                    param.YsIncomeMst.PersistentState = PersistentState.Added;
                    foreach (var dtl in param.YsIncomeDtls)
                    {
                        if (dtl.PhId == 0)
                        {
                            if(dtl.PersistentState == PersistentState.Deleted)
                            {
                                continue;
                            }
                            dtl.PersistentState = PersistentState.Added;
                        }
                        else
                        {
                            if (dtl.PersistentState != PersistentState.Deleted)
                            {
                                dtl.PersistentState = PersistentState.Modified;
                            }
                        }
                        ysIncomeDtls.Add(dtl);
                    }
                }
                else
                {
                    param.YsIncomeMst.FDeclareTime = DateTime.Now;
                    //没打上删除标记的都是修改
                    if (param.YsIncomeMst.PersistentState != PersistentState.Deleted)
                    {
                        param.YsIncomeMst.PersistentState = PersistentState.Modified;
                        foreach (var dtl in param.YsIncomeDtls)
                        {
                            if (dtl.PhId == 0)
                            {
                                if (dtl.PersistentState == PersistentState.Deleted)
                                {
                                    continue;
                                }
                                dtl.PersistentState = PersistentState.Added;
                            }
                            else
                            {
                                if (dtl.PersistentState != PersistentState.Deleted)
                                {
                                    dtl.PersistentState = PersistentState.Modified;
                                }
                            }
                            ysIncomeDtls.Add(dtl);
                        }
                    }
                    else
                    {
                        foreach (var dtl in param.YsIncomeDtls)
                        {
                            if(dtl.PhId == 0)
                            {
                                continue;
                            }
                            dtl.PersistentState = PersistentState.Deleted;
                            ysIncomeDtls.Add(dtl);
                        }
                    }
                }
                param.YsIncomeMst.FDeclareAmount = ysIncomeDtls.ToList().FindAll(t => t.PersistentState != PersistentState.Deleted).Sum(t => t.FBudgetamount);
                SavedResult<long> savedResult = new SavedResult<long>();
                savedResult = this.YsIncomeMstService.SaveYsIncome(param.YsIncomeMst, ysIncomeDtls);
                return DataConverterHelper.SerializeObject(savedResult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 删除该组织当前年度的收入预算
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostDelete([FromBody]BaseListModel param)
        {
            if (param.FPhId == 0)
            {
                return DCHelper.ErrorMessage("要删除的预算信息不能为空！");
            }
            try
            {
                var delete = this.YsIncomeMstService.SaveDelete(param.FPhId);
                return DataConverterHelper.SerializeObject(delete);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }


        /// <summary>
        /// 收入预算生成预算按钮
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostBudget([FromBody]BaseListModel param)
        {
            //if (param.FPhIds == null || param.FPhIds.Count <= 0)
            //{
            //    return DCHelper.ErrorMessage("生成收入预算集合不能为空！");
            //}
            if (param.FPhId == 0)
            {
                return DCHelper.ErrorMessage("生成预算信息不能为空！");
            }
            if (param.uid == 0)
            {
                return DCHelper.ErrorMessage("生成预算人员信息不能为空！");
            }
            try
            {
                var result = this.YsIncomeMstService.SaveBudget(param.FPhId, param.uid);
                return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        [HttpPost]
        public string Postceshi([FromBody]AllYsIncomeRequestModel budgetAllData)
        {
            try
            {
                budgetAllData = new AllYsIncomeRequestModel();
                budgetAllData.YsIncomeMst = new YsIncomeMstModel();
                budgetAllData.YsIncomeMst.PersistentState = PersistentState.Added;
                var result = this.YsIncomeMstService.Save<long>(budgetAllData.YsIncomeMst, "");
                return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
    }
}