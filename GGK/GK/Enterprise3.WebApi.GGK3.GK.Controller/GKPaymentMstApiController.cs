#region Summary
/**************************************************************************************
    * 命名空间：			GBK3.BK.Controller
    * 类 名 称：			PaymentMstController
    * 文 件 名：			PaymentMstController.cs
    * 创建时间：			2019/5/15 
    * 作    者：			吾丰明    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using Enterprise3.Common.Model.Results;

using GGK3.GK.Service.Interface;
using Enterprise3.WebApi.ApiControllerBase;
using System.Web.Http;
using GData3.Common.Utils;
using GData3.Common.Model;
using GGK3.GK.Model.Domain;
using GData3.Common.Model.Enums;
using GGK3.GK.Model.Enums;
using System.Threading.Tasks;
using System.Web;
using System.Text;
using Newtonsoft.Json;
using Enterprise3.WebApi.ApiControllerBase.Models;
using Spring.Data.Common;
using System.Linq;
using GData.YQHL.Model;
using EnumPaymentState = GGK3.GK.Model.Enums.EnumPaymentState;
using Enterprise3.Common.Base.Criterion;
using GQT3.QT.Service.Interface;
using GQT3.QT.Model.Domain;
using GData.YQHL.Service;
using Enterprise3.WebApi.GSP3.SP.Model.Request;

namespace Enterprise3.WebApi.GGK3.GK.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [MethodExceptionFilter]
    public class GKPaymentMstApiController : ApiBase
    {
        IGKPaymentMstService GKPaymentMstService { get; set; }
        IQTSysSetService QTSysSetService { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public GKPaymentMstApiController()
        {
            GKPaymentMstService = base.GetObject<IGKPaymentMstService>("GGK3.GK.Service.GKPaymentMst");
            QTSysSetService = base.GetObject<IQTSysSetService>("GQT3.QT.Service.QTSysSet");

        }

        /// <summary>
        /// 获取支付单列表
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetPaymentList([FromUri]Parameter parameters)
        {
            if (parameters == null || string.IsNullOrEmpty(parameters.uid))
            {
                return DCHelper.ErrorMessage("用户id为空！");
            }
            if (string.IsNullOrEmpty(parameters.orgid))
            {
                return DCHelper.ErrorMessage("组织编码为空！");
            }
            if (string.IsNullOrEmpty(parameters.Ryear))
            {
                return DCHelper.ErrorMessage("年度为空！");
            }
            if (parameters.pageindex < 0 || parameters.pagesize == 0)
            {
                return DCHelper.ErrorMessage("分页参数不正确！");
            }

            try
            {
                Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(parameters.queryfilter ?? "");

                //二次处理dic
                DCHelper.ConvertDic<GKPaymentMstModel>(dicWhere);

                //var result = GKPaymentMstService.Find(dicWhere);  //this.GKPaymentMstService.FindList(parameters.uid, parameters.orgid, parameters.pageindex, parameters.pagesize, dicWhere, parameters.sort);
                var result = GKPaymentMstService.LoadWithPage(parameters.pageindex, parameters.pagesize, dicWhere, new string[] { "NgInsertDt asc" });
                IList<GKPaymentMstModel> list = result.Results;



                return DCHelper.ModelListToJson<GKPaymentMstModel>(list, result.TotalItems, parameters.pageindex, parameters.pagesize);
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage(e.Message);
            }
        }


        /// <summary>
        /// 获取资金拨付支付单列表
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetPaymentList4Zjbf([FromUri]Parameter parameters)
        {
            if (parameters == null || string.IsNullOrEmpty(parameters.uid))
            {
                return DCHelper.ErrorMessage("用户id为空！");
            }
            if (string.IsNullOrEmpty(parameters.orgid))
            {
                return DCHelper.ErrorMessage("组织编码为空！");
            }
            if (string.IsNullOrEmpty(parameters.Ryear))
            {
                return DCHelper.ErrorMessage("年度为空！");
            }
            if (parameters.pageindex < 0 || parameters.pagesize == 0)
            {
                return DCHelper.ErrorMessage("分页参数不正确！");
            }

            string[] sorts = parameters.sort;
            if (sorts == null || sorts.Length < 1)
            {
                sorts = new string[] { "NgInsertDt desc" };
            }

            try
            {
                Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(parameters.queryfilter ?? "");

                //二次处理dic
                DCHelper.ConvertDic<GKPaymentMstModel>(dicWhere);

                var result = GKPaymentMstService.GetPaymentList4Zjbf(parameters.pageindex, parameters.pagesize, dicWhere, sorts);
                IList<GKPayment4ZjbfModel> list = result.Results;

                return DCHelper.ModelListToJson<GKPayment4ZjbfModel>(list, result.TotalItems, parameters.pageindex, parameters.pagesize);
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        // <summary>
        /// 获取资金拨付支付单列表(审批人是登录用户的单据置顶)
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetPaymentList4Zjbf2([FromUri]Parameter parameters)
        {
            if (parameters == null || string.IsNullOrEmpty(parameters.uid))
            {
                return DCHelper.ErrorMessage("用户id为空！");
            }
            if (string.IsNullOrEmpty(parameters.orgid))
            {
                return DCHelper.ErrorMessage("组织编码为空！");
            }
            if (string.IsNullOrEmpty(parameters.Ryear))
            {
                return DCHelper.ErrorMessage("年度为空！");
            }
            if (parameters.pageindex < 0 || parameters.pagesize == 0)
            {
                return DCHelper.ErrorMessage("分页参数不正确！");
            }

            string[] sorts = parameters.sort;
            if (sorts == null || sorts.Length < 1)
            {
                sorts = new string[] { "NgInsertDt desc" };
            }

            try
            {
                Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(parameters.queryfilter ?? "");

                //二次处理dic
                DCHelper.ConvertDic<GKPaymentMstModel>(dicWhere);

                BillRequestModel billRequest = new BillRequestModel();
                billRequest.Year = parameters.Ryear;
                billRequest.Uid = long.Parse(parameters.uid);
                billRequest.BType = "002";
                billRequest.Orgid = long.Parse(parameters.orgid);

                var result = GKPaymentMstService.GetPaymentList4Zjbf2(billRequest, parameters.pageindex, parameters.pagesize, dicWhere, sorts);
                IList<GKPayment4ZjbfModel> list = new List<GKPayment4ZjbfModel>();
                if (result != null && result.Results != null)
                {
                    list = result.Results;
                }
                

                return DCHelper.ModelListToJson<GKPayment4ZjbfModel>(list, result.TotalItems, parameters.pageindex, parameters.pagesize);
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 根据序列Json串，创建对象
        /// </summary>
        [HttpPost]
        public string PostAdd([FromBody]InfoBaseModel<GKPaymentModel> parameters)
        {
            try
            {
                //增加后台数据检查：比如年度、会计期、凭证日期、组织id、组织号等


                if (string.IsNullOrWhiteSpace(parameters.infoData.Mst.OrgCode))
                {
                    return DCHelper.ErrorMessage("缺少组织号!");
                }

                if (parameters.infoData.Mst.OrgPhid <= 0)
                {
                    return DCHelper.ErrorMessage("组织Id有误!");
                }

                if (string.IsNullOrWhiteSpace(parameters.infoData.Mst.FBilltype))
                {
                    return DCHelper.ErrorMessage("缺少单据类型!");
                }


                SavedResult<Int64> savedResult = this.GKPaymentMstService.SaveGKPayment(parameters.infoData);

                return DataConverterHelper.SerializeObject(savedResult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }


        /// <summary>
        /// 获取资金拨付支付单信息
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetPayment4Zjbf([FromUri]BaseSingleModel parameter)
        {
            if (string.IsNullOrEmpty(parameter.id))
            {
                return DCHelper.ErrorMessage("单据主键Phid为空！");
            }
            if (parameter == null || string.IsNullOrEmpty(parameter.uid))
            {
                return DCHelper.ErrorMessage("用户id为空！");
            }
            if (string.IsNullOrEmpty(parameter.orgid))
            {
                return DCHelper.ErrorMessage("组织编码为空！");
            }
            if (string.IsNullOrEmpty(parameter.Ryear))
            {
                return DCHelper.ErrorMessage("年度为空！");
            }


            try
            {
                var result = GKPaymentMstService.GetPayment4Zjbf(long.Parse(parameter.id));
                if (result == null)
                {
                    return DCHelper.ErrorMessage("无法获取单据，单据不存在或已被删除！");
                }
                return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 根据序列Json串，创建对象
        /// </summary>
        [HttpPost]
        public string PostUpdate([FromBody]InfoBaseModel<GKPaymentModel> parameters)
        {
            try
            {
                if (parameters == null || string.IsNullOrEmpty(parameters.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }

                if (string.IsNullOrWhiteSpace(parameters.infoData.Mst.OrgCode))
                {
                    return DCHelper.ErrorMessage("缺少组织号!");
                }

                if (parameters.infoData.Mst.OrgPhid <= 0)
                {
                    return DCHelper.ErrorMessage("组织Id有误!");
                }

                if (string.IsNullOrWhiteSpace(parameters.infoData.Mst.FBilltype))
                {
                    return DCHelper.ErrorMessage("缺少单据类型!");
                }
                var flam2 = parameters.infoData.Mst.FDelete;
                if (flam2 == (byte)EnumYesNo.Yes)
                {
                    return DCHelper.ErrorMessage("作废的单据不允许进行修改！");
                }

                SavedResult<Int64> savedResult = this.GKPaymentMstService.SaveGKPayment(parameters.infoData);

                return DataConverterHelper.SerializeObject(savedResult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 删除支付单据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostDelete([FromBody]BaseSingleModel parameter)
        {
            if (string.IsNullOrEmpty(parameter.id))
            {
                return DCHelper.ErrorMessage("单据主键Phid为空！");
            }
            if (parameter == null || string.IsNullOrEmpty(parameter.uid))
            {
                return DCHelper.ErrorMessage("用户id为空！");
            }
            if (string.IsNullOrEmpty(parameter.orgid))
            {
                return DCHelper.ErrorMessage("组织编码为空！");
            }
            if (string.IsNullOrEmpty(parameter.Ryear))
            {
                return DCHelper.ErrorMessage("年度为空！");
            }


            try
            {
                var result = GKPaymentMstService.Find<long>(long.Parse(parameter.id));
                if (!result.IsError && result.Data != null)
                {
                    //判断要删除的支付单的状态
                    if (result.Data.FApproval == (int)EnumBillApproval.Approved)
                    {
                        return DCHelper.ErrorMessage("单据已审批，不能删除！");
                    }

                    if (result.Data.FApproval == (int)EnumBillApproval.InApproval)
                    {
                        return DCHelper.ErrorMessage("单据在审批中，不能删除！");
                    }

                    if (result.Data.FApproval == (int)EnumBillApproval.Approved)
                    {
                        return DCHelper.ErrorMessage("单据已审批通过，不能删除！");
                    }

                    if (result.Data.FState == (int)EnumPaymentState.AbnormalPayment)
                    {
                        return DCHelper.ErrorMessage("单据已审批通过，不能删除！");
                    }

                    var result2 = GKPaymentMstService.Delete(long.Parse(parameter.id));
                    return DataConverterHelper.SerializeObject(result2);
                }
                else
                {
                    result.Msg = "单据不存在或已被其他人删除！";
                }

                return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 单笔支付单提交支付
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostSubmitPayment([FromBody]BaseSingleModel parameter)
        {
            if (parameter == null || string.IsNullOrEmpty(parameter.uid))
            {
                return DCHelper.ErrorMessage("用户id为空！");
            }

            if (string.IsNullOrEmpty(parameter.id))
            {
                return DCHelper.ErrorMessage("单据主键Phid为空！");
            }

            //if (string.IsNullOrEmpty(parameter.orgid))
            //{
            //    return DCHelper.ErrorMessage("组织编码为空！");
            //}

            //if (string.IsNullOrEmpty(parameter.Ryear))
            //{
            //    return DCHelper.ErrorMessage("年度为空！");
            //}

            /*
            ICBCNCService icbcService = new ICBCNCService();
            bool httpsState, signState;
            string msg = icbcService.CheckNetSafeClient(out httpsState, out signState);
            if (httpsState == false || signState == false)
            {
                return DCHelper.ErrorMessage(msg + "  请联系系统管理员排查问题");
            }
            */

            try
            {
                string errMsg = "";
                var result = GKPaymentMstService.SubmitPayment(long.Parse(parameter.id), out errMsg, long.Parse(parameter.uid));
                if (result == null && !string.IsNullOrEmpty(errMsg))
                {
                    return DCHelper.ErrorMessage(errMsg);
                }

                return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 刷新支付单支付状态
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostRefreshPaymentState([FromBody]BaseSingleModel parameter)
        {
            if (parameter == null || string.IsNullOrEmpty(parameter.uid))
            {
                return DCHelper.ErrorMessage("用户id为空！");
            }

            if (string.IsNullOrEmpty(parameter.id))
            {
                return DCHelper.ErrorMessage("单据主键Phid为空！");
            }

            /*
            ICBCNCService icbcService = new ICBCNCService();
            bool httpsState, signState;
            string msg = icbcService.CheckNetSafeClient(out httpsState, out signState);
            if (httpsState == false || signState == false)
            {
                return DCHelper.ErrorMessage(msg + "  请联系系统管理员排查问题");
            }
            */

            try
            {
                string errMsg = "";
                var result = GKPaymentMstService.RefreshPaymentState(long.Parse(parameter.id), out errMsg);
                if (result == null && !string.IsNullOrEmpty(errMsg))
                {
                    return DCHelper.ErrorMessage(errMsg);
                }

                return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 批量处理提交支付单据
        /// </summary>
        /// <param name="parameters">支付phid数组</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PostSubmitPayments([FromBody]InfoBaseModel<long[]> parameters)
        {

            if (parameters == null || string.IsNullOrEmpty(parameters.uid))
            {
                return DCHelper.ErrorMessage("用户id为空！");
            }

            if (parameters.infoData == null || parameters.infoData.Length < 0)
            {
                return DCHelper.ErrorMessage("单据主键Phid为空！");
            }

            /*
            ICBCNCService icbcService = new ICBCNCService();
            bool httpsState, signState;
            string msg = icbcService.CheckNetSafeClient(out httpsState, out signState);
            if (httpsState == false || signState == false) {
                return DCHelper.ErrorMessage(msg + "  请联系系统管理员排查问题");
            }
            */

            string errMsg = string.Empty;
            foreach (long id in parameters.infoData)
            {
                GKPaymentMstService.SubmitPayment(id, out errMsg, long.Parse(parameters.uid));
            }

            return DCHelper.SuccessMessage("合并支付提交成功，后台处理中!");
        }

        /// <summary>
        /// 批量刷新支付单据支付状态
        /// </summary>
        /// <param name="parameters">支付phid数组</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PostRefreshPaymentsState([FromBody]InfoBaseModel<long[]> parameters)
        {
            if (parameters.infoData == null || parameters.infoData.Length < 0)
            {
                return DCHelper.ErrorMessage("单据主键Phid为空！");
            }

            /*
            ICBCNCService icbcService = new ICBCNCService();
            bool httpsState, signState;
            string msg = icbcService.CheckNetSafeClient(out httpsState, out signState);
            if (httpsState == false || signState == false)
            {
                return DCHelper.ErrorMessage(msg + "  请联系系统管理员排查问题");
            }
            */

            string errMsg = string.Empty;
            foreach (long id in parameters.infoData)
            {
                GKPaymentMstService.RefreshPaymentState(id, out errMsg);
            }

            return DCHelper.SuccessMessage("批量刷新支付状态提交成功，后台处理中!");
        }

        [HttpGet]
        public string GetBankServiceState()
        {
            ICBCNCService icbcService = new ICBCNCService();
            bool httpsState, signState;
            string msg = icbcService.CheckNetSafeClient(out httpsState, out signState);
            if (httpsState == false || signState == false)
            {
                return DCHelper.ErrorMessage(msg + "  请联系系统管理员排查问题");
            }

            return DCHelper.SuccessMessage("银行服务状态OK");
        }

        /// <summary>
        /// 批量刷新所有支付单据支付状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PostRefreshAllPaymentsState()
        {
            //获取支付中且FSeqno不为空的数据
            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<byte>.Eq("FState", (byte)EnumPaymentState.DuringPayment));
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Gt("FSeqno", " "));

            var result = GKPaymentMstService.Find(dicWhere, new string[] { "NgInsertDt asc" }); //LoadWithPage(parameters.pageindex, parameters.pagesize, dicWhere, new string[] { "NgInsertDt asc" });
            if (result.IsError == false && result.Data != null)
            {
                IList<GKPaymentMstModel> list = result.Data;

                string errMsg = string.Empty;
                foreach (var mst in list)
                {
                    GKPaymentMstService.RefreshPaymentState(mst.PhId, out errMsg);
                }
            }

            return DCHelper.SuccessMessage("批量刷新支付状态提交成功，后台处理中!");
        }

        /// <summary>
        /// 批量刷新支付单据支付状态
        /// </summary>
        /// <param name="parameters">支付phid数组</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PostUpdatePaymentsState([FromBody]InfoBaseModel<long[]> parameters)
        {
            if (parameters.infoData == null || parameters.infoData.Length < 0)
            {
                return DCHelper.ErrorMessage("单据主键Phid为空！");
            }

            if (string.IsNullOrEmpty(parameters.value))
            {
                return DCHelper.ErrorMessage("单据状态为空！");
            }

            if (int.Parse(parameters.value) != (int)EnumPaymentState.Paid)
            {
                return DCHelper.ErrorMessage("单据状态仅能更新为支付成功！");
            }

            if (string.IsNullOrEmpty(parameters.uid))
            {
                return DCHelper.ErrorMessage("用户id为空！");
            }

            string errMsg = string.Empty;
            byte payState = byte.Parse(parameters.value);
            long uid = long.Parse(parameters.uid);

            //判断支付方式为网银的需要更新
            /*
            Dictionary<string, object> PayMethoddic = new Dictionary<string, object>();
            new CreateCriteria(PayMethoddic)
                .Add(ORMRestrictions<string>.Eq("DicType", "PayMethod"))
                .Add(ORMRestrictions<Byte>.Eq("Issystem", 1))
                .Add(ORMRestrictions<String>.Eq("TypeName", "网银"));
            IList<QTSysSetModel> PayMethods = QTSysSetService.Find(PayMethoddic).Data;
            if (PayMethods.Count > 0)
            {
                foreach (long id in parameters.infoData)
                {
                    GKPaymentMstModel gKPaymentMst = GKPaymentMstService.Find(id).Data;
                    if (gKPaymentMst.FPaymethod == PayMethods[0].PhId)
                    {
                        this.GKPaymentMstService.UpdatePaymentState(id, payState, uid);
                    }
                }
            }
            */
            //直接更新状态
            foreach (long id in parameters.infoData)
            {
                this.GKPaymentMstService.UpdatePaymentState(id, payState, uid);
            }

            return DCHelper.SuccessMessage("批量更新支付状态提交成功，后台处理中!");
        }

        /// <summary>
        /// 修改作废状态
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostCancetGkPaymentList([FromBody]BaseSingleModel paramters)
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
                var result = this.GKPaymentMstService.PostCancetGkPaymentList(fCodes);
                return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
        /// <summary>
        /// 网银模板导出
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetExportTemplate([FromUri] BaseSingleModel area)
        {
            var Result = GKPaymentMstService.GetPaymentArea(area.ids, area.type);
            return Result;
        }

        
    }
}

