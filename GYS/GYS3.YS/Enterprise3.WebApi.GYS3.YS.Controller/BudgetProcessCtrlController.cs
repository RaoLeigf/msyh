using Enterprise3.Common.Model.Results;
using Enterprise3.WebApi.ApiControllerBase;
using Enterprise3.WebApi.GYS3.YS.Model.Request;
using GData3.Common.Model;
using GXM3.XM.Model.Domain;
using GYS3.YS.Model.Domain;
using GYS3.YS.Service.Interface;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Enterprise3.WebApi.GYS3.YS.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [MethodExceptionFilter]
    public class BudgetProcessCtrlController : ApiBase
    {
        IBudgetProcessCtrlService BudgetProcessCtrlService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BudgetProcessCtrlController()
        {
            BudgetProcessCtrlService = base.GetObject<IBudgetProcessCtrlService>("GYS3.YS.Service.BudgetProcessCtrl");
        }

        /// <summary>
        /// 取左侧工会列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetBudgetProcessCtrlDistinctList([FromUri]BudgetProcessCtrlRequestModel param)
        {
            if (string.IsNullOrEmpty(param.UserId))
            {
                return DCHelper.ErrorMessage("用户编码不能为空！");
            }
            try
            {
                //string query = System.Web.HttpContext.Current.Request.Params["FOcode"];//查询条件
                string query = "";//查询条件
                SUP.Common.DataEntity.DataStoreParam storeparam = new SUP.Common.DataEntity.DataStoreParam();
                var result = BudgetProcessCtrlService.GetBudgetProcessCtrlDistinctList(storeparam, query, param.UserId);

                return DCHelper.ModelListToJson<BudgetProcessCtrlModel>(result.Results, (Int32)result.TotalItems);
                //return DataConverterHelper.EntityListToJson<BudgetProcessCtrlModel>(result.Results, (Int32)result.TotalItems);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 取根据公会Code获取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetBudgetProcessCtrlPorcessList([FromUri]BudgetProcessCtrlRequestModel param)
        {
            if (string.IsNullOrEmpty(param.FOcode))
            {
                return DCHelper.ErrorMessage("组织编码不能为空！");
            }
            try
            {
                SUP.Common.DataEntity.DataStoreParam storeparam = new SUP.Common.DataEntity.DataStoreParam();//查询条件 
                storeparam.PageIndex = 0; storeparam.PageSize = 100;
                if (param.PageIndex > 0) storeparam.PageIndex = param.PageIndex;
                if (param.PageSize > 0) storeparam.PageSize = param.PageSize;

                List<BudgetProcessCtrlModel> list = null;
                BudgetProcessCtrlService.GetBudgetProcessCtrlPorcessList(storeparam, param.FOcode,param.FYear, out list);
                if (list.Count > 0)
                {
                    BudgetProcessCtrlService.Save<Int64>(list, "");
                }
                var result = BudgetProcessCtrlService.GetBudgetProcessCtrlPorcessList2(storeparam, param.FOcode, param.FYear);

                var changeList = result.Results.ToList();
                if (changeList != null && changeList.Count > 0)//批量更新
                {
                    foreach (var item in changeList)
                    {
                        if (item.FProcessStatus == "1"&&DateTime.Now > item.EndDt)
                        {
                            item.FProcessStatus = "2";
                        }
                        if (item.FProcessStatus == "3" && DateTime.Now > item.EndDt2)
                        {
                            item.FProcessStatus = "4";
                        }
                        item.PersistentState = PersistentState.Modified;
                    }
                    BudgetProcessCtrlService.Save<Int64>(changeList, "");
                }
                //再取一遍 不然版本号有问题
                result = BudgetProcessCtrlService.GetBudgetProcessCtrlPorcessList2(storeparam, param.FOcode, param.FYear);
                //return DataConverterHelper.EntityListToJson<BudgetProcessCtrlModel>(result.Results, (Int32)result.TotalItems);
                return DCHelper.ModelListToJson<BudgetProcessCtrlModel>(result.Results, (Int32)result.TotalItems);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }


        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpPost]
        public string PostSaveBudgetProcessCtrl([FromBody]BudgetProcessSaveRequestListModel param)
        {
            //string budgetprocessctrlformData = System.Web.HttpContext.Current.Request.Form["jsonArray"];
            //string symbol = System.Web.HttpContext.Current.Request.Form["symbol"];

            //List<BudgetProcessCtrlModel> list = JsonConvert.DeserializeObject<List<BudgetProcessCtrlModel>>(budgetprocessctrlformData);

            //IList<BudgetProcessCtrlModel> list2 = BudgetProcessCtrlService.FindBudgetProcessCtrlModelByList(list);

            //for (int i = 0; i < list.Count; i++) {
            //    list[i].PersistentState = PersistentState.Modified;
            //}
            //var budgetprocessctrlforminfo = DataConverterHelper.JsonToEntity<BudgetProcessCtrlModel>(budgetprocessctrlformData);

            for (int i = 0; i < param.infodata.Count; i++)
            {
                param.infodata[i].PersistentState = PersistentState.Modified;
            }

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            try
            {
                //savedresult = BudgetProcessCtrlService.Save<Int64>(budgetprocessctrlforminfo.AllRow);
                savedresult = BudgetProcessCtrlService.SaveProcessSetting(param.infodata, "1");
            }
            catch (Exception ex)
            {
                savedresult.Status = ResponseStatus.Error;
                savedresult.Msg = ex.Message.ToString();
            }
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 批量保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpPost]
        public string PostSaveBudgetProcessTime([FromBody]BudgetProcessSaveRequestModel param)
        {
            if (string.IsNullOrEmpty(param.FOcode))
            {
                return DCHelper.ErrorMessage("组织编码不能为空！");
            }
            SavedResult<Int64> savedresult = new SavedResult<Int64>();

            try
            {
                SUP.Common.DataEntity.DataStoreParam storeparam = new SUP.Common.DataEntity.DataStoreParam();//查询条件 


                List<BudgetProcessCtrlModel> list = BudgetProcessCtrlService.GetBudgetProcessCtrlPorcessList3(param.FOcode);

                if (list != null && list.Count > 0)
                {
                    //批量更新各阶段的起始时间和截止时间
                    if (param.FProcessStatus == "2" || param.FProcessStatus == "4")
                    {
                        foreach (var item in list)
                        {
                            item.FProcessStatus = param.FProcessStatus;
                            item.PersistentState = PersistentState.Modified;
                        }
                    }
                    if (param.FProcessStatus == "1")
                    {
                        foreach (var item in list)
                        {
                            item.FProcessStatus = param.FProcessStatus;
                            item.StartDt = param.StartDt;
                            item.EndDt = param.EndDt;
                            item.PersistentState = PersistentState.Modified;
                        }
                    }
                    if (param.FProcessStatus == "3")
                    {
                        foreach (var item in list)
                        {
                            item.FProcessStatus = param.FProcessStatus;
                            item.StartDt2 = param.StartDt;
                            item.EndDt2 = param.EndDt;
                            item.PersistentState = PersistentState.Modified;
                        }
                    }
                    savedresult = BudgetProcessCtrlService.Save<Int64>(list, "");
                }
            }
            catch (Exception ex)
            {
                savedresult.Status = ResponseStatus.Error;
                savedresult.Msg = ex.Message.ToString();
            }
            return DataConverterHelper.SerializeObject(savedresult);
        }
        //BudgetProcessCtrlService.FindBudgetProcessCtrl(mstforminfo.FDeclarationUnit, mstforminfo.FBudgetDept,mstforminfo.FYear);
        /// <summary>
        /// 得到进度
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetBudgetProcessCtrl([FromUri]ProjectMstModel param)
        {
            if (string.IsNullOrEmpty(param.FDeclarationUnit))
            {
                return DCHelper.ErrorMessage("申报单位为空！");
            }
            if (string.IsNullOrEmpty(param.FBudgetDept))
            {
                return DCHelper.ErrorMessage("预算部门为空！");
            }
            if (string.IsNullOrEmpty(param.FYear))
            {
                return DCHelper.ErrorMessage("年度为空！");
            }

            var processStatus = BudgetProcessCtrlService.FindBudgetProcessCtrl(param.FDeclarationUnit, param.FBudgetDept, param.FYear);
            return DataConverterHelper.SerializeObject(processStatus);
        }
    }
}
