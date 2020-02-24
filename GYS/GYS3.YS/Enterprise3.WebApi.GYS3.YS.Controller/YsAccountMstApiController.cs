using Enterprise3.Common.Model.Results;
using Enterprise3.WebApi.ApiControllerBase;
using Enterprise3.WebApi.GYS3.YS.Model;
using GQT3.QT.Model.Domain;
using GYS3.YS.Model.Domain;
using GYS3.YS.Service.Interface;
using NPOI.XWPF.UserModel;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Web.Http;

namespace Enterprise3.WebApi.GYS3.YS.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [MethodExceptionFilter]
    public class YsAccountMstApiController : ApiBase
    {
        IYsAccountMstService YsAccountMstService { get; set; }

        IBudgetMstService BudgetMstService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public YsAccountMstApiController()
        {
            YsAccountMstService = base.GetObject<IYsAccountMstService>("GYS3.YS.Service.YsAccountMst");
            BudgetMstService = base.GetObject<IBudgetMstService>("GYS3.YS.Service.BudgetMst");
        }

        /// <summary>
        /// 保存以及上报年初预算
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string PostSaveAccountList([FromBody]BaseInfoModel<YsAccountMstModel> param)
        {
            if(param.uid == 0)
            {
                return DCHelper.ErrorMessage("用户id不能为空！");
            }
            if(param.orgid == 0)
            {
                return DCHelper.ErrorMessage("组织id不能为空！");
            }
            if(param.infoData == null)
            {
                return DCHelper.ErrorMessage("参数传递有误！");
            }
            try
            {
                if(param.infoData.YsAccounts != null && param.infoData.YsAccounts.Count > 0)
                {
                    IList<YsAccountModel> ysAccounts = new List<YsAccountModel>();
                    foreach (var acc in param.infoData.YsAccounts)
                    {
                        ysAccounts.Add(acc);
                        GetYsAccountsChilds(acc, ysAccounts);
                    }
                    if(ysAccounts != null && ysAccounts.Count > 0)
                    {
                        //后端重新算一遍数据（防止前端传入错误数据）
                        foreach(var ysAcc in ysAccounts)
                        {
                            if (ysAcc.SUBJECTCODE == "4BNHJSR")
                            {
                                ysAcc.FINALACCOUNTSTOTAL = ysAccounts.ToList().FindAll(t => (t.SUBJECTCODE == null ? "" : t.SUBJECTCODE).StartsWith("4") && (t.SUBJECTCODE == null ? "" : t.SUBJECTCODE).Length == 3) == null ? 0 : ysAccounts.ToList().FindAll(t => (t.SUBJECTCODE == null ? "" : t.SUBJECTCODE).StartsWith("4") && (t.SUBJECTCODE == null ? "" : t.SUBJECTCODE).Length == 3).Sum(t => t.FINALACCOUNTSTOTAL);
                                ysAcc.BUDGETTOTAL = ysAccounts.ToList().FindAll(t => (t.SUBJECTCODE == null ? "" : t.SUBJECTCODE).StartsWith("4") && (t.SUBJECTCODE == null ? "" : t.SUBJECTCODE).Length == 3) == null ? 0 : ysAccounts.ToList().FindAll(t => (t.SUBJECTCODE == null ? "" : t.SUBJECTCODE).StartsWith("4") && (t.SUBJECTCODE == null ? "" : t.SUBJECTCODE).Length == 3).Sum(t => t.BUDGETTOTAL);
                                ysAcc.ADJUSTMENT = ysAccounts.ToList().FindAll(t => (t.SUBJECTCODE == null ? "" : t.SUBJECTCODE).StartsWith("4") && (t.SUBJECTCODE == null ? "" : t.SUBJECTCODE).Length == 3) == null ? 0 : ysAccounts.ToList().FindAll(t => (t.SUBJECTCODE == null ? "" : t.SUBJECTCODE).StartsWith("4") && (t.SUBJECTCODE == null ? "" : t.SUBJECTCODE).Length == 3).Sum(t => t.ADJUSTMENT);
                                ysAcc.APPROVEDBUDGETTOTAL = ysAccounts.ToList().FindAll(t => (t.SUBJECTCODE == null ? "" : t.SUBJECTCODE).StartsWith("4") && (t.SUBJECTCODE == null ? "" : t.SUBJECTCODE).Length == 3) == null ? 0 : ysAccounts.ToList().FindAll(t => (t.SUBJECTCODE == null ? "" : t.SUBJECTCODE).StartsWith("4") && (t.SUBJECTCODE == null ? "" : t.SUBJECTCODE).Length == 3).Sum(t => t.APPROVEDBUDGETTOTAL);
                                ysAcc.ThisaccountsTotal = ysAccounts.ToList().FindAll(t => (t.SUBJECTCODE == null ? "" : t.SUBJECTCODE).StartsWith("4") && (t.SUBJECTCODE == null ? "" : t.SUBJECTCODE).Length == 3) == null ? 0 : ysAccounts.ToList().FindAll(t => (t.SUBJECTCODE == null ? "" : t.SUBJECTCODE).StartsWith("4") && (t.SUBJECTCODE == null ? "" : t.SUBJECTCODE).Length == 3).Sum(t => t.ThisaccountsTotal);
                            }
                            if (ysAcc.SUBJECTCODE == "5QM1")
                            {
                                ysAcc.FINALACCOUNTSTOTAL = (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").FINALACCOUNTSTOTAL) - (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").FINALACCOUNTSTOTAL);
                                ysAcc.BUDGETTOTAL = (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").BUDGETTOTAL) - (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").BUDGETTOTAL);
                                ysAcc.ADJUSTMENT = (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").ADJUSTMENT) - (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").ADJUSTMENT);
                                ysAcc.APPROVEDBUDGETTOTAL = (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").APPROVEDBUDGETTOTAL) - (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").APPROVEDBUDGETTOTAL);
                                ysAcc.ThisaccountsTotal = (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").ThisaccountsTotal) - (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").ThisaccountsTotal);
                            }
                            if (ysAcc.SUBJECTCODE == "5QM6")
                            {
                                ysAcc.FINALACCOUNTSTOTAL = (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").FINALACCOUNTSTOTAL) + (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").FINALACCOUNTSTOTAL)
                                        + (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").FINALACCOUNTSTOTAL) - (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").FINALACCOUNTSTOTAL)
                                        - (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").FINALACCOUNTSTOTAL);
                                ysAcc.BUDGETTOTAL = (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").BUDGETTOTAL) + (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").BUDGETTOTAL)
                                        + (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").BUDGETTOTAL) - (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").BUDGETTOTAL)
                                        - (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").BUDGETTOTAL);
                                ysAcc.ADJUSTMENT = (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").ADJUSTMENT) + (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").ADJUSTMENT)
                                        + (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").ADJUSTMENT) - (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").ADJUSTMENT)
                                        - (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").ADJUSTMENT);
                                ysAcc.APPROVEDBUDGETTOTAL = (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").APPROVEDBUDGETTOTAL) + (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").APPROVEDBUDGETTOTAL)
                                        + (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").APPROVEDBUDGETTOTAL) - (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").APPROVEDBUDGETTOTAL)
                                        - (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").APPROVEDBUDGETTOTAL);
                                ysAcc.ThisaccountsTotal = (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").ThisaccountsTotal) + (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").ThisaccountsTotal)
                                        + (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").ThisaccountsTotal) - (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").ThisaccountsTotal)
                                        - (ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").ThisaccountsTotal);
                            }

                            if (ysAcc.BUDGETTOTAL == 0)
                            {
                                ysAcc.BudgetComplete = 100;
                            }
                            else
                            {
                                ysAcc.BudgetComplete = ysAcc.APPROVEDBUDGETTOTAL / ysAcc.BUDGETTOTAL * 100;
                            }
                            if (ysAcc.APPROVEDBUDGETTOTAL == 0)
                            {
                                ysAcc.COMPLETE = 100;
                            }
                            else
                            {
                                ysAcc.COMPLETE = ysAcc.ThisaccountsTotal / ysAcc.APPROVEDBUDGETTOTAL * 100;
                            }
                        }
                    }
                    param.infoData.YsAccounts = ysAccounts;
                }
                //if ("1".Equals(param.value))
                //{
                //    if(param.infoData.VerifyStart == 1)
                //    {
                //        return DCHelper.ErrorMessage("年初申报数据已上报，不能修改！");
                //    }
                //}
                //else if ("2".Equals(param.value))
                //{
                //    if (param.infoData.VerifyMiddle == 1)
                //    {
                //        return DCHelper.ErrorMessage("年中调整数据已上报，不能修改！");
                //    }
                //}
                //else if ("3".Equals(param.value))
                //{
                //    if (param.infoData.VerifyEnd == 1)
                //    {
                //        return DCHelper.ErrorMessage("年末决算数据已上报，不能修改！");
                //    }
                //}
                SavedResult<long> savedResult = new SavedResult<long>();
                savedResult = this.YsAccountMstService.SaveYsAccount(param.infoData, param.orgid, param.orgCode, param.Year, param.uid, param.value);
                return DataConverterHelper.SerializeObject(savedResult);
            }
            catch(Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        public IList<YsAccountModel> GetYsAccountsChilds(YsAccountModel ysAccount, IList<YsAccountModel> ysAccounts)
        {
            if (ysAccount.Childrens != null && ysAccount.Childrens.Count > 0)
            {                
                foreach(var child in ysAccount.Childrens)
                {
                    if (!string.IsNullOrEmpty(child.SUBJECTCODE))
                    {
                        ysAccounts.Add(child);
                        GetYsAccountsChilds(child, ysAccounts);
                    }
                }
            }
            return ysAccounts;
        }

        /// <summary>
        /// 修改预决算说明书
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostUpdateAccountMst([FromBody]BaseInfoModel<YsAccountMstModel> param)
        {
            if(param.infoData == null)
            {
                return DCHelper.ErrorMessage("参数传递不能修改！");
            }
            try
            {
                SavedResult<Int64> savedResult = this.YsAccountMstService.UpdateAccountMst(param.infoData);
                if (savedResult != null && savedResult.SaveRows > 0)
                {
                    return DCHelper.SuccessMessage("保存成功！");
                }
                else
                {
                    return DCHelper.ErrorMessage("保存失败！");
                }
            }
            catch(Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 根据组织与年份获取各个上报组织的数量
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetYsAccountNum([FromUri]BaseListModel param)
        {
            if (string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织编码不能为空！");
            }
            if (string.IsNullOrEmpty(param.Year))
            {
                return DCHelper.ErrorMessage("年份参数不能为空！");
            }
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic = this.YsAccountMstService.GetYsAccountNum(param.orgCode, param.Year);
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "获取成功！",
                    Data = dic
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch(Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 根据组织获取该组织以及其下级的所有汇总信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetAllReportData([FromUri]BaseListModel param)
        {
            if (param.orgid == 0)
            {
                return DCHelper.ErrorMessage("组织id不能为空！");
            }
            if (string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织编码不能为空！");
            }
            if (string.IsNullOrEmpty(param.Year))
            {
                return DCHelper.ErrorMessage("年份参数不能为空！");
            }
            try
            {
                YsAccountMstModel ysAccountMst = new YsAccountMstModel();
                ysAccountMst = this.YsAccountMstService.GetAllYsAccountList(param.orgid, param.orgCode, param.Year, param.ChooseOwn, param.value);
                if(ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
                {
                    IList<YsAccountModel> ysAccounts = new List<YsAccountModel>();
                    foreach (var acc in ysAccountMst.YsAccounts)
                    {
                        if (acc.SUBJECTCODE.Length == 3)
                        {
                            YsAccountModel ys = new YsAccountModel();
                            ysAccounts.Add(this.BudgetMstService.GetCompleteYsAccount(ysAccountMst.YsAccounts, acc.SUBJECTCODE, ys));
                        }
                        else
                        {
                            if (acc.SUBJECTCODE == "4BNHJSR" || acc.SUBJECTCODE == "5BNHJZC" || acc.SUBJECTCODE.StartsWith("5QM"))
                            {
                                ysAccounts.Add(acc);
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    ysAccountMst.YsAccounts = ysAccounts;
                }
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "获取成功！",
                    Data = ysAccountMst
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch(Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }


        /// <summary>
        /// 根据组织导出该组织以及其下级的所有汇总信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetAllReportExcel([FromUri]BaseListModel param)
        {
            if (param.orgid == 0)
            {
                return DCHelper.ErrorMessage("组织id不能为空！");
            }
            if(param.uid == 0)
            {
                return DCHelper.ErrorMessage("用户id不能为空！");
            }
            if (string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织编码不能为空！");
            }
            if (string.IsNullOrEmpty(param.Year))
            {
                return DCHelper.ErrorMessage("年份参数不能为空！");
            }
            try
            {
                string result = "";
                YsAccountMstModel ysAccountMst = new YsAccountMstModel();
                ysAccountMst = this.YsAccountMstService.GetAllYsAccountList(param.orgid, param.orgCode, param.Year, param.ChooseOwn, param.value);
                if (ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
                {
                    //获取组织和用户信息
                    User2Model userModel = new User2Model();
                    OrganizeModel organizeModel = new OrganizeModel();
                    userModel = this.BudgetMstService.GetUser(param.uid);
                    organizeModel = this.BudgetMstService.GetOrganize(param.orgid);
                  

                    if (param.value == "1")
                    {
                        result = this.YsAccountMstService.GetBeginExcel(ysAccountMst.YsAccounts.ToArray(), null, userModel, organizeModel);
                    }
                    else if (param.value == "2")
                    {
                        result = this.YsAccountMstService.GetMiddleExcel(ysAccountMst.YsAccounts.ToArray(), null, userModel, organizeModel);
                    }
                    else if (param.value == "3")
                    {
                        result = this.YsAccountMstService.GetEndExcel(ysAccountMst.YsAccounts.ToArray(), null, userModel, organizeModel);
                    }
                    
                }
                return result;
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }


        /// <summary>
        /// 获取上报未上报组织集合
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetOrganizeVerifyList([FromUri]BaseListModel param)
        {
            if (string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织编码不能为空！");
            }
            if (string.IsNullOrEmpty(param.Year))
            {
                return DCHelper.ErrorMessage("年份参数不能为空！");
            }
            try
            {
                IList<OrganizeModel> organizes = new List<OrganizeModel>();
                IList<OrganizeModel> newOrganizes = new List<OrganizeModel>();
                //先获取打上是否审批标志的组织集合
                organizes = this.YsAccountMstService.GetOrganizeVerifyList(param.orgCode, param.Year);
                if(organizes != null && organizes.Count > 0)
                {
                    if ("1".Equals(param.value))
                    {
                        newOrganizes = organizes.ToList().FindAll(t => t.VerifyStart == param.Verify);
                    }
                    else if ("2".Equals(param.value))
                    {
                        newOrganizes = organizes.ToList().FindAll(t => t.VerifyMiddle == param.Verify);
                    }
                    else if ("3".Equals(param.value))
                    {
                        newOrganizes = organizes.ToList().FindAll(t => t.VerifyEnd == param.Verify);
                    }
                    if (!string.IsNullOrEmpty(param.Search))
                    {
                        newOrganizes = newOrganizes.ToList().FindAll(t => t.OName.Contains(param.Search));
                    }
                }
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "获取成功！",
                    Data = newOrganizes
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch(Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        #region//年中调整，年末决算报表上报

        /// <summary>
        /// 得到年初上报的数据（单个组织）
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
                YsAccountMstModel ysAccountMst = new YsAccountMstModel();
                ysAccountMst = this.YsAccountMstService.GetBegineAccounts(param.orgid, param.orgCode, param.Year);
                if (ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
                {
                    IList<YsAccountModel> ysAccounts = new List<YsAccountModel>();
                    foreach (var acc in ysAccountMst.YsAccounts)
                    {
                        if (acc.SUBJECTCODE.Length == 3)
                        {
                            YsAccountModel ys = new YsAccountModel();
                            ysAccounts.Add(this.BudgetMstService.GetCompleteYsAccount(ysAccountMst.YsAccounts, acc.SUBJECTCODE, ys));
                        }
                        else
                        {
                            if (acc.SUBJECTCODE == "4BNHJSR" || acc.SUBJECTCODE == "5BNHJZC" || acc.SUBJECTCODE.StartsWith("5QM"))
                            {
                                ysAccounts.Add(acc);
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    ysAccountMst.YsAccounts = ysAccounts;
                }
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "获取成功！",
                    Data = ysAccountMst
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }


        /// <summary>
        /// 得到年中上报的数据（单个组织）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetMiddleYear([FromUri]BaseListModel param)
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
                YsAccountMstModel ysAccountMst = new YsAccountMstModel();
                ysAccountMst = this.YsAccountMstService.GetMiddleAccounts(param.orgid, param.orgCode, param.Year);
                if (ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
                {
                    IList<YsAccountModel> ysAccounts = new List<YsAccountModel>();
                    foreach (var acc in ysAccountMst.YsAccounts)
                    {
                        if (acc.SUBJECTCODE.Length == 3)
                        {
                            YsAccountModel ys = new YsAccountModel();
                            ysAccounts.Add(this.BudgetMstService.GetCompleteYsAccount(ysAccountMst.YsAccounts, acc.SUBJECTCODE, ys));
                        }
                        else
                        {
                            if (acc.SUBJECTCODE == "4BNHJSR" || acc.SUBJECTCODE == "5BNHJZC" || acc.SUBJECTCODE.StartsWith("5QM"))
                            {
                                ysAccounts.Add(acc);
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    ysAccountMst.YsAccounts = ysAccounts;
                }
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "获取成功！",
                    Data = ysAccountMst
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 得到年末上报的数据（单个组织）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetEndYear([FromUri]BaseListModel param)
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
                YsAccountMstModel ysAccountMst = new YsAccountMstModel();
                ysAccountMst = this.YsAccountMstService.GetEndAccounts(param.orgid, param.orgCode, param.Year);
                if (ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
                {
                    IList<YsAccountModel> ysAccounts = new List<YsAccountModel>();
                    foreach (var acc in ysAccountMst.YsAccounts)
                    {
                        if (acc.SUBJECTCODE.Length == 3)
                        {
                            YsAccountModel ys = new YsAccountModel();
                            ysAccounts.Add(this.BudgetMstService.GetCompleteYsAccount(ysAccountMst.YsAccounts, acc.SUBJECTCODE, ys));
                        }
                        else
                        {
                            if (acc.SUBJECTCODE == "4BNHJSR" || acc.SUBJECTCODE == "5BNHJZC" || acc.SUBJECTCODE.StartsWith("5QM"))
                            {
                                ysAccounts.Add(acc);
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    ysAccountMst.YsAccounts = ysAccounts;
                }
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "获取成功！",
                    Data = ysAccountMst
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
        #endregion

        #region//预算导出相关

        /// <summary>
        /// 导出预算说明书的封面
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetYearStartCover([FromUri]BaseListModel parameter)
        {

            if (parameter == null)
            {
                return DCHelper.ErrorMessage("请求参数为空！");
            }

            if (parameter.orgid == 0)
            {
                return DCHelper.ErrorMessage("组织id为空！");
            }

            if (parameter.uid == 0)
            {
                return DCHelper.ErrorMessage("用户id为空！");
            }

            if (String.IsNullOrEmpty(parameter.Year))
            {
                return DCHelper.ErrorMessage("年度为空！");
            }
            //value来判断是年初，年中，年末预算说明说
            if (String.IsNullOrEmpty(parameter.value))
            {
                return DCHelper.ErrorMessage("判断年初,年中,年末的value条件为空！");
            }

            //获取组织和用户信息
            User2Model userModel = new User2Model();
            OrganizeModel organizeModel = new OrganizeModel();

            try
            {
                userModel = this.BudgetMstService.GetUser(parameter.uid);
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage("获取用户信息失败！");
            }

            try
            {
                organizeModel = this.BudgetMstService.GetOrganize(parameter.orgid);
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage("获取组织信息失败！");
            }
            FileStream fs = null;
            FileStream output = null;
            try
            {
                string title = "";
                string content1 = "";
                string content2 = "";
                if ("1".Equals(parameter.value))
                {
                    title = "预算表";
                    content1 = "预算说明书";
                    content2 = "经费收支预算表";
                }
                else if ("2".Equals(parameter.value))
                {
                    title = "预算表(年中调整)";
                    content1 = "预算说明书(年中调整)";
                    content2 = "经费收支预算表(年中调整)";
                }
                else if ("3".Equals(parameter.value))
                {
                    title = "决算表";
                    content1 = "决算说明书";
                    content2 = "经费决算预算表";
                }

                string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".docx";
                string filePath = HostingEnvironment.MapPath("~/DownLoadFiles/SubjectBudget");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                File.Copy(HostingEnvironment.MapPath("~" + "/DownLoadFiles/template/预算封面.docx"), filePath + "/" + filename, true);

                fs = new FileStream(filePath + "/" + filename, FileMode.Open, FileAccess.Read);

                XWPFDocument document = new XWPFDocument(fs);
                foreach (var para in document.Paragraphs)
                {
                    string oldContext = para.ParagraphText;
                    if (String.IsNullOrEmpty(oldContext))
                    {
                        continue;
                    }
                    string context = para.ParagraphText;
                    if (context.Contains("{$date}"))
                    {
                        context = context.Replace("{$date}", DateTime.Now.ToString("yyyy/MM/dd"));
                    }
                    if (context.Contains("{$chairman}"))
                    {
                        context = context.Replace("{$chairman}", userModel.UserName);
                    }
                    if (context.Contains("{$treasurer}"))
                    {
                        context = context.Replace("{$treasurer}", userModel.UserName);
                    }
                    if (context.Contains("{$maker}"))
                    {
                        context = context.Replace("{$maker}", userModel.UserName);
                    }
                    if (context.Contains("{$checker}"))
                    {
                        context = context.Replace("{$checker}", "");
                    }
                    if (context.Contains("{$year}"))
                    {
                        context = context.Replace("{$year}", DateTime.Now.Year.ToString());
                    }
                    if (context.Contains("{$month}"))
                    {
                        context = context.Replace("{$month}", DateTime.Now.Month.ToString());
                    }
                    if (context.Contains("{$day}"))
                    {
                        context = context.Replace("{$day}", DateTime.Now.Day.ToString());
                    }
                    if (context.Contains("{$title}"))
                    {
                        context = context.Replace("{$title}", title);
                    }
                    if (context.Contains("{$content1}"))
                    {
                        context = context.Replace("{$content1}", content1);
                    }
                    if (context.Contains("{$content2}"))
                    {
                        context = context.Replace("{$content2}", content2);
                    }

                    if (oldContext != context)
                        para.ReplaceText(oldContext, context);

                }
                output = new FileStream(filePath + "/" + filename, FileMode.Create);
                document.Write(output);
                fs.Close();
                fs.Dispose();
                output.Close();
                output.Dispose();

                return DataConverterHelper.SerializeObject(new { path = filePath, filename = filename });
            }
            catch (Exception ex)
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
                if (output != null)
                {
                    output.Close();
                    output.Dispose();
                }
                return DCHelper.ErrorMessage("导出年初申报的封面失败！" + ex.Message);
            }

        }

        /// <summary>
        /// 导出预算说明书
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetDescriptionDocx([FromUri]BaseListModel parameter)
        {
            if (parameter == null)
            {
                return DCHelper.ErrorMessage("请求参数为空！");
            }

            if (parameter.orgid == 0)
            {
                return DCHelper.ErrorMessage("组织id为空！");
            }

            if (parameter.uid == 0)
            {
                return DCHelper.ErrorMessage("用户id为空！");
            }

            if (String.IsNullOrEmpty(parameter.Year))
            {
                return DCHelper.ErrorMessage("年度为空！");
            }
            //value来判断是年初，年中，年末预算说明说
            if (String.IsNullOrEmpty(parameter.value))
            {
                return DCHelper.ErrorMessage("判断年初,年中,年末的value条件为空！");
            }
            //查询预算主表对象，获取预算说明书
            string description = "";
            try
            {
                IList<YsAccountMstModel> mstModels = YsAccountMstService.Find(t => t.Orgid == parameter.orgid && t.Uyear == parameter.Year).Data;
                if (mstModels != null && mstModels.Count > 0)
                {
                    if ("1".Equals(parameter.value))
                    {
                        description = mstModels[0].DescriptionStart;
                    }
                    else if ("2".Equals(parameter.value))
                    {
                        description = mstModels[0].DescriptionMiddle;
                    }
                    else if ("3".Equals(parameter.value))
                    {
                        description = mstModels[0].DescriptionEnd;
                    }
                }
                else
                {
                    description = "";
                }
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage("预算主表对象失败！");
            }

            FileStream fs = null;
            FileStream output = null;
            try
            {
                string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".docx";
                string filePath = HostingEnvironment.MapPath("~/DownLoadFiles/SubjectBudget");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                File.Copy(HostingEnvironment.MapPath("~" + "/DownLoadFiles/template/预算说明书.docx"), filePath + "/" + filename, true);

                fs = new FileStream(filePath + "/" + filename, FileMode.Open, FileAccess.Read);

                XWPFDocument document = new XWPFDocument(fs);
                foreach (var para in document.Paragraphs)
                {
                    string oldContext = para.ParagraphText;
                    if (String.IsNullOrEmpty(oldContext))
                    {
                        continue;
                    }
                    string context = para.ParagraphText;
                    if (context.Contains("{$date}"))
                    {
                        context = context.Replace("{$date}", DateTime.Now.ToString("yyyy/MM/dd"));
                    }
                    if (context.Contains("{$content}"))
                    {
                        context = context.Replace("{$content}", description);
                    }

                    if (oldContext != context)
                        para.ReplaceText(oldContext, context);

                }
                output = new FileStream(filePath + "/" + filename, FileMode.Create);
                document.Write(output);
                fs.Close();
                fs.Dispose();
                output.Close();
                output.Dispose();

                return DataConverterHelper.SerializeObject(new { path = filePath, filename = filename });
            }
            catch (Exception e)
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
                if (output != null)
                {
                    output.Close();
                    output.Dispose();
                }
                return DCHelper.ErrorMessage("导出预算说明书失败！" + e.Message);
            }
        }


        /// <summary>
        /// 导出年初预算报表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostExportBeginYear([FromBody]BaseInfoModel<YsAccountMstModel> param)
        {
            if (param.uid == 0)
            {
                return DCHelper.ErrorMessage("用户id不能为空！");
            }
            if (param.orgid == 0)
            {
                return DCHelper.ErrorMessage("组织id不能为空！");
            }
            if (param.infoData == null)
            {
                return DCHelper.ErrorMessage("参数传递有误！");
            }
            try
            {
                if (param.infoData.YsAccounts != null && param.infoData.YsAccounts.Count > 0)
                {
                    IList<YsAccountModel> ysAccounts = new List<YsAccountModel>();
                    foreach (var acc in param.infoData.YsAccounts)
                    {
                        ysAccounts.Add(acc);
                        GetYsAccountsChilds(acc, ysAccounts);
                    }
                    param.infoData.YsAccounts = ysAccounts;
                }
                else
                {
                    return DCHelper.ErrorMessage("导出参数传递有误！");
                }
                //获取组织和用户信息
                User2Model userModel = new User2Model();
                OrganizeModel organizeModel = new OrganizeModel();
                userModel = this.BudgetMstService.GetUser(param.uid);
                organizeModel = this.BudgetMstService.GetOrganize(param.orgid);

                //string result = "";
                // ysAccountMst = new YsAccountMstModel();
                //ysAccountMst = this.YsAccountMstService.GetMiddleAccounts(488181024000002,"101", "2019");
                string result = this.YsAccountMstService.GetBeginExcel(param.infoData.YsAccounts.ToArray(), null, userModel, organizeModel);
                return result;
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 导出年中调整报表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostExportMiddleYear([FromBody]BaseInfoModel<YsAccountMstModel> param)
        {
            if (param.uid == 0)
            {
                return DCHelper.ErrorMessage("用户id不能为空！");
            }
            if (param.orgid == 0)
            {
                return DCHelper.ErrorMessage("组织id不能为空！");
            }
            if (param.infoData == null)
            {
                return DCHelper.ErrorMessage("参数传递有误！");
            }
            try
            {
                if (param.infoData.YsAccounts != null && param.infoData.YsAccounts.Count > 0)
                {
                    IList<YsAccountModel> ysAccounts = new List<YsAccountModel>();
                    foreach (var acc in param.infoData.YsAccounts)
                    {
                        ysAccounts.Add(acc);
                        GetYsAccountsChilds(acc, ysAccounts);
                    }
                    param.infoData.YsAccounts = ysAccounts;
                }
                else
                {
                    return DCHelper.ErrorMessage("导出参数传递有误！");
                }
                //获取组织和用户信息
                User2Model userModel = new User2Model();
                OrganizeModel organizeModel = new OrganizeModel();
                userModel = this.BudgetMstService.GetUser(param.uid);
                organizeModel = this.BudgetMstService.GetOrganize(param.orgid);

                //string result = "";
                // ysAccountMst = new YsAccountMstModel();
                //ysAccountMst = this.YsAccountMstService.GetMiddleAccounts(488181024000002,"101", "2019");
                string result = this.YsAccountMstService.GetMiddleExcel(param.infoData.YsAccounts.ToArray(), null, userModel, organizeModel);
                return result;
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 导出年末决算报表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostExportEndYear([FromBody]BaseInfoModel<YsAccountMstModel> param)
        {
            if (param.uid == 0)
            {
                return DCHelper.ErrorMessage("用户id不能为空！");
            }
            if (param.orgid == 0)
            {
                return DCHelper.ErrorMessage("组织id不能为空！");
            }
            if (param.infoData == null)
            {
                return DCHelper.ErrorMessage("参数传递有误！");
            }
            try
            {
                if (param.infoData.YsAccounts != null && param.infoData.YsAccounts.Count > 0)
                {
                    IList<YsAccountModel> ysAccounts = new List<YsAccountModel>();
                    foreach (var acc in param.infoData.YsAccounts)
                    {
                        ysAccounts.Add(acc);
                        GetYsAccountsChilds(acc, ysAccounts);
                    }
                    param.infoData.YsAccounts = ysAccounts;
                }
                else
                {
                    return DCHelper.ErrorMessage("导出参数传递有误！");
                }
                //获取组织和用户信息
                User2Model userModel = new User2Model();
                OrganizeModel organizeModel = new OrganizeModel();
                userModel = this.BudgetMstService.GetUser(param.uid);
                organizeModel = this.BudgetMstService.GetOrganize(param.orgid);

                //string result = "";
                // ysAccountMst = new YsAccountMstModel();
                //ysAccountMst = this.YsAccountMstService.GetMiddleAccounts(488181024000002,"101", "2019");
                string result = this.YsAccountMstService.GetEndExcel(param.infoData.YsAccounts.ToArray(), null, userModel, organizeModel);
                return result;
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage(e.Message);
            }
        }
        #endregion
    }
}