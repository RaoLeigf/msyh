#region Summary
/**************************************************************************************
    * 命名空间：			GGK3.GK.Service
    * 类 名 称：			GKPaymentMstService
    * 文 件 名：			GKPaymentMstService.cs
    * 创建时间：			2019/5/20 
    * 作    者：			李明    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using SUP.Common.DataEntity;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Service;

using GGK3.GK.Service.Interface;
using GGK3.GK.Facade.Interface;
using GGK3.GK.Model.Domain;
using GGK3.GK.Model.Enums;
using GData3.Common.Model.Enums;
using GData.YQHL.Model;
using GData.YQHL.Service;
using GData3.Common.Utils;
using System.Text;
using Newtonsoft.Json;
using Enterprise3.WebApi.ApiControllerBase.Models;
using Enterprise3.Common.Model;
using Enterprise3.Common.Base.Criterion;
using GQT3.QT.Facade.Interface;
using GQT3.QT.Model.Domain;
using EnumPaymentState = GGK3.GK.Model.Enums.EnumPaymentState;
using GSP3.SP.Facade.Interface;
using Enterprise3.WebApi.GSP3.SP.Model.Request;
using Enterprise3.WebApi.GSP3.SP.Model.Response;
using GBK3.BK.Facade.Interface;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.IO;

namespace GGK3.GK.Service
{
    /// <summary>
    /// GKPaymentMst服务组装处理类
    /// </summary>
    public partial class GKPaymentMstService : EntServiceBase<GKPaymentMstModel>, IGKPaymentMstService
    {
        #region 类变量及属性
        /// <summary>
        /// GKPaymentMst业务外观处理对象
        /// </summary>
        IGKPaymentMstFacade GKPaymentMstFacade
        {
            get
            {
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IGKPaymentMstFacade;
            }
        }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
        private IGKPaymentDtlFacade GKPaymentDtlFacade { get; set; }

        private IQTSysSetFacade QTSysSetFacade { get; set; }

        private IGAppvalRecordFacade GAppvalRecordFacade { get; set; }

        private IOrganizationFacade OrganizationFacade { get; set; }

        private IPaymentMstFacade PaymentMstFacade { get; set; }
        #endregion

        #region 实现 IGKPaymentMstService 业务添加的成员


        public FindedResults<GKPaymentDtl4ZjbfModel> Find(Dictionary<string, object> dicWhere, params string[] sorts)
        {
            FindedResults<GKPaymentDtl4ZjbfModel> ret = null;
            var result = this.GKPaymentDtlFacade.Find(dicWhere, sorts);

            ret = CommonUtils.TransReflection<FindedResults<GKPaymentDtlModel>, FindedResults<GKPaymentDtl4ZjbfModel>>(result);

            return ret;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="gKPaymentMstEntity"></param>
		/// <param name="gKPaymentDtlEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveGKPaymentMst(GKPaymentMstModel gKPaymentMstEntity, List<GKPaymentDtlModel> gKPaymentDtlEntities)
        {
            return GKPaymentMstFacade.SaveGKPaymentMst(gKPaymentMstEntity, gKPaymentDtlEntities);
        }

        /// <summary>
        /// 通过外键值获取GKPaymentDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<GKPaymentDtlModel> FindGKPaymentDtlByForeignKey<TValType>(TValType id)
        {
            return GKPaymentDtlFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 保存付款单
        /// </summary>
        /// <param name="paymentEntity"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveGKPayment(GKPaymentModel paymentEntity)
        {
            return GKPaymentMstFacade.SaveGKPaymentMst(paymentEntity.Mst, paymentEntity.Dtls);
        }

        /// <summary>
        /// 获取资金拨付支付单信息
        /// </summary>
        /// <param name="phid">支付单主键</param>
        /// <returns></returns>
        public GKPayment4ZjbfModel GetPayment4Zjbf(Int64 phid)
        {
            return GKPaymentMstFacade.GetPayment4Zjbf(phid);
        }

        /// <summary>
        /// 单笔提交支付,如果返回null且errorMsg不为空，则表示出错了
        /// </summary>
        /// <param name="phid"></param>
        /// <param name="errorMsg">错误消息提示信息</param>
        /// <param name="submitterID"></param>
        /// <returns></returns>
        public GKPaymentModel SubmitPayment(Int64 phid, out string errorMsg, Int64 submitterID)
        {
            errorMsg = "";

            var result = base.EntFacade.Find<long>(phid);
            if (result != null && result.Data != null)
            {
                var bill = result.Data;

                //判断支付单的状态
                if (bill.FApproval == (int)EnumBillApproval.InApproval)
                {
                    errorMsg = "单据在审批中，不能支付！";
                    return null;
                }

                if (bill.FApproval == (int)EnumBillApproval.PendingSend)
                {
                    errorMsg = "单据待送审，不能支付！";
                    return null;
                }

                if (bill.FApproval == (int)EnumBillApproval.NotPass)
                {
                    errorMsg = "单据未通过审批，不能支付！";
                    return null;
                }

                if (bill.FApproval == (int)EnumBillApproval.Approved)
                {
                    if (bill.FState == (int)Model.Enums.EnumPaymentState.Paid)
                    {
                        errorMsg = "单据已支付成功，不能重复支付！";
                        //return new GKPaymentModel
                        //{
                        //    Mst = bill,
                        //    Dtls = null
                        //};
                        return null;
                    }

                    if (bill.FState == (int)Model.Enums.EnumPaymentState.DuringPayment)
                    {
                        errorMsg = "单据支付中，请不要重复支付！";
                        return null;
                    }

                    if (bill.FState == (int)Model.Enums.EnumPaymentState.AbnormalPayment)
                    {
                        errorMsg = "单据支付异常，请做异常处理操作！";
                        return null;
                    }
                }
                Dictionary<string, object> PayMethoddic = new Dictionary<string, object>();
                new CreateCriteria(PayMethoddic)
                    .Add(ORMRestrictions<string>.Eq("DicType", "PayMethod"))
                    .Add(ORMRestrictions<Byte>.Eq("Issystem", 1))
                    .Add(ORMRestrictions<String>.Eq("TypeName", "网银"));
                IList<QTSysSetModel> PayMethods = QTSysSetFacade.Find(PayMethoddic).Data;
                if (PayMethods.Count <= 0 || PayMethods[0].PhId != bill.FPaymethod)
                {
                    /*
                    errorMsg = "单据支付方式异常，请检查支付方式！";
                    return null;
                    */
                    //非网银支付，直接修改状态
                    var ret = this.GKPaymentMstFacade.UpdatePaymentState(phid, (int)EnumPaymentState.Paid, submitterID);
                    if (ret.IsError == false)
                    {
                        var dtls = this.GKPaymentDtlFacade.FindByForeignKey(phid);
                        return new GKPaymentModel
                        {
                            Mst = bill,
                            Dtls = dtls.Data.ToList()
                        };
                    }
                    else
                    {
                        errorMsg = "单据支付方式异常，请检查支付方式！";
                        return null;
                    }
                }


                #region 更改支付状态为支付中
                var dtlResult = this.GKPaymentDtlFacade.FindByForeignKey(phid);
                if (!dtlResult.IsError && dtlResult.Data != null)
                {
                    var billDtls = dtlResult.Data;
                    foreach (var dtl in billDtls)
                    {
                        dtl.FState = (int)Model.Enums.EnumPaymentState.DuringPayment;
                        dtl.PersistentState = SUP.Common.Base.PersistentState.Modified;
                    }

                    bill.FState = (int)Model.Enums.EnumPaymentState.DuringPayment;
                    bill.FSubmitterId = submitterID; //提交人
                    bill.FSubmitdate = DateTime.Now; //提交时间
                    bill.PersistentState = SUP.Common.Base.PersistentState.Modified;

                    //保存
                    SavedResult<Int64> savedResult = this.EntFacade.Save<long>(bill);
                    if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
                    {
                        if (billDtls.Count > 0)
                        {
                            this.GKPaymentDtlFacade.Save<long>(billDtls);
                        }
                        this.GKPaymentMstFacade.UpdateZjbfPaymentPayState(bill.PhId, bill.FState); //更新资金拨付单状态
                    }
                }
                #endregion

                #region 银行支付，支付后再次更新支付状态
                result = base.EntFacade.Find<long>(phid);
                bill = result.Data;
                dtlResult = this.GKPaymentDtlFacade.FindByForeignKey(phid);
                if (!dtlResult.IsError && dtlResult.Data != null)
                {
                    var billDtls = dtlResult.Data;
                    //默认使用工行NC客户端
                    string bankCode = "102", bankName = "工商银行";
                    var bankInfo = new BankInfo { bankCode = bankCode, bankName = bankName, shortName = "ICBC", bankKeys = "工行", description = "中国工商银行" };
                    var defaultBankVersion = new BankVersionInfo { versionName = "工商银行NC版", shortName = "ICBCNC", vendor = "杭州政云数据技术有限公司" };
                    var now = DateTime.Now;
                    var today = now.ToString("yyyy-MM-dd");

                    PaymentInfo paymentInfo = null;
                    List<PaymentInfo> payList = new List<PaymentInfo>();
                    foreach (var dtl in billDtls)
                    {
                        if (string.IsNullOrEmpty(dtl.FPayBankcode))
                        {
                            errorMsg = "付款方银行账号异常，请修改！";
                            return null;
                        }
                        else
                        {
                            if (dtl.FPayBankcode.Length < 3)
                            {
                                errorMsg = "付款方银行账号异常，请修改！";
                                return null;
                            }
                        }

                        if (string.IsNullOrEmpty(dtl.FRecBankcode))
                        {
                            errorMsg = "收款方银行账号异常，请修改！";
                            return null;
                        }
                        else
                        {
                            if (dtl.FRecBankcode.Length < 3)
                            {
                                errorMsg = "收款方银行账号异常，请修改！";
                                return null;
                            }
                        }

                        paymentInfo = new PaymentInfo
                        {
                            amount = dtl.FAmount,
                            currency = "001",
                            bookingDate = now,
                            submitDate = now,
                            sameCity = dtl.FPayCityname == dtl.FRecCityname ? 1 : 2,
                            sameBank = dtl.FPayBankcode.Substring(0, 3) == dtl.FRecBankcode.Substring(0, 3) ? 1 : 2, //判断
                            isUrgent = 1,  //默认加急
                            explantion = dtl.FExplation,
                            usage = dtl.FUsage,
                            submitterID = "",
                            businessRefNo = " ",
                            iSeqno = dtl.FIseqno
                        };
                        paymentInfo.bankAcnt = new BankAcnt
                        {
                            //bankVersionInfo = bankVersion,
                            acntNo = dtl.FPayAcnt,
                            acntName = dtl.FPayAcntname,
                            bankName = dtl.FPayBankname,
                            city = dtl.FPayCityname,
                            bankInfo = new BankInfo { bankCode = dtl.FPayBankcode, bankName = dtl.FPayBankname },
                            bankVersionInfo = defaultBankVersion
                        };

                        paymentInfo.oppoBankAcnt = new BankAcnt
                        {
                            acntNo = dtl.FRecAcnt,
                            acntName = dtl.FRecAcntname,
                            bankName = dtl.FRecBankname,
                            city = dtl.FRecCityname,
                            bankInfo = new BankInfo { bankCode = dtl.FRecBankcode, bankName = dtl.FRecBankname }
                        };

                        payList.Add(paymentInfo);
                    }
                    paymentInfo = null;


                    #region 使用webapi方式调用
                    //提交银行api支付
                    //var data = new
                    //{
                    //    currency = "001", //工行： 001 表示人民币
                    //    beginDate = today,
                    //    endDate = today,
                    //    caller = new CallerInfo
                    //    {
                    //        caller = "web",
                    //        version = "0.1beta",
                    //        callerIP = "",
                    //        callerOS = "",
                    //        callerTime = DateTime.Now
                    //    },
                    //    infoData = payList
                    //};
                    //string json = JsonConvert.SerializeObject(data);

                    //调用银行支付接口

                    //WebApiClient client = new WebApiClient(BankApiURL, ApiReqParam, EnumDataFormat.Json);
                    //var res = client.Post("api/BIF/BankService/PostSubmitPayment", json);
                    //if (res.IsError)
                    //{
                    //    errorMsg = res.ErrMsg;
                    //    return null;
                    //}
                    #endregion

                    ICBCNCService icbcService = new ICBCNCService();
                    CallerInfo caller = new CallerInfo
                    {
                        caller = "web",
                        version = "0.1beta",
                        callerIP = "",
                        callerOS = "",
                        callerTime = DateTime.Now
                    };

                    try
                    {
                        #region 提交银行支付
                        var subResult = icbcService.submitPayment(caller, payList.ToArray());

                        if (subResult.retCode == "0")
                        {
                            if (subResult.detailData.infoData == null)
                            {
                                //文件级返回，当做支付中  
                                foreach (var dtl in billDtls)
                                {
                                    dtl.FResult = "0";
                                    dtl.FSeqno = subResult.fSeqno;
                                    dtl.FBkSn = subResult.bankSerialNo;
                                    dtl.FState = (int)Model.Enums.EnumPaymentState.DuringPayment;
                                    dtl.PersistentState = SUP.Common.Base.PersistentState.Modified;
                                }

                                bill.FSeqno = subResult.fSeqno;
                                bill.FState = (int)Model.Enums.EnumPaymentState.DuringPayment;
                                bill.FSubmitterId = submitterID; //提交人
                                bill.FSubmitdate = DateTime.Now; //提交时间
                                bill.PersistentState = SUP.Common.Base.PersistentState.Modified;

                                //保存
                                SavedResult<Int64> savedResult = this.EntFacade.Save<long>(bill);
                                if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
                                {
                                    if (billDtls.Count > 0)
                                    {
                                        this.GKPaymentDtlFacade.Save<long>(billDtls);
                                    }

                                    this.GKPaymentMstFacade.UpdateZjbfPaymentPayState(bill.PhId, bill.FState); //更新资金拨付单状态
                                }

                                return new GKPaymentModel
                                {
                                    Mst = bill,
                                    Dtls = billDtls.ToList()
                                };
                            }
                            else
                            {
                                //指令级返回
                                if (subResult.detailData.infoData.Length > 0)
                                {
                                    var paymentInfos = subResult.detailData.infoData;
                                    IEnumerable<PaymentInfo> find = null;
                                    int success = 0, failed = 0, during = 0;
                                    foreach (var dtl in billDtls)
                                    {
                                        find = paymentInfos.Where(x => x.amount == dtl.FAmount && x.oppoBankAcnt.acntNo == dtl.FRecAcnt);
                                        if (find.Count() > 0)
                                        {
                                            dtl.FResult = find.First().result;  //支付状态码
                                            dtl.FState = (byte)find.First().payState;
                                            dtl.FSeqno = find.First().paymentSeqNo;
                                            dtl.FBkSn = subResult.bankSerialNo;
                                            dtl.FResultmsg = find.First().resultMsg;

                                            switch (dtl.FState)
                                            {
                                                case (int)Model.Enums.EnumPaymentState.Paid: //支付成功
                                                    success++;
                                                    break;
                                                case (int)Model.Enums.EnumPaymentState.AbnormalPayment: //支付异常
                                                    failed++;
                                                    break;
                                                default:    //支付中
                                                    during++;
                                                    break;
                                            }

                                            //支付时间
                                            if (find.Single().submitDate == DateTime.MinValue)
                                            {
                                                dtl.FSubmitdate = null;
                                            }
                                            else
                                            {
                                                dtl.FSubmitdate = (Nullable<DateTime>)find.First().submitDate;
                                            }

                                            dtl.PersistentState = SUP.Common.Base.PersistentState.Modified;
                                        }
                                    }

                                    if (success == billDtls.Count)
                                    {
                                        bill.FState = (int)Model.Enums.EnumPaymentState.Paid;
                                    }

                                    if (during == billDtls.Count)
                                    {
                                        bill.FState = (int)Model.Enums.EnumPaymentState.DuringPayment;
                                    }

                                    //如果全部支付失败，则单据状态为异常； 
                                    //如果支付成功的 +支付失败的和与单据支付明细数一致，则表示单据有确定状态了，状态为异常
                                    if (failed == billDtls.Count || (success > 0 && failed > 0 && (success + failed) == billDtls.Count))
                                    {
                                        bill.FState = (int)Model.Enums.EnumPaymentState.AbnormalPayment;
                                    }

                                    bill.FSubmitterId = submitterID; //提交人
                                    bill.FSubmitdate = DateTime.Now; //提交时间
                                    bill.PersistentState = SUP.Common.Base.PersistentState.Modified;

                                    //保存单据状态                                
                                    SavedResult<Int64> savedResult = this.EntFacade.Save<long>(bill);
                                    if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
                                    {
                                        if (billDtls.Count > 0)
                                        {
                                            this.GKPaymentDtlFacade.Save<long>(billDtls);
                                        }

                                        this.GKPaymentMstFacade.UpdatePaymentState(phid, bill.FState, submitterID); //更新支付状态

                                        //this.GKPaymentMstFacade.UpdateZjbfPaymentPayState(bill.PhId, bill.FState); //更新资金拨付单状态
                                        this.PaymentMstFacade.UpdatePayState(bill, billDtls);//更新资金拨付单状态2
                                    }

                                    return new GKPaymentModel
                                    {
                                        Mst = bill,
                                        Dtls = billDtls.ToList()
                                    };
                                }
                            }
                        }
                        else
                        {
                            errorMsg = subResult.retMsg;
                            //支付异常
                            foreach (var dtl in billDtls)
                            {
                                dtl.FResult = "-1";
                                dtl.FResultmsg = errorMsg;

                                dtl.FState = (int)Model.Enums.EnumPaymentState.AbnormalPayment;
                                dtl.PersistentState = SUP.Common.Base.PersistentState.Modified;
                            }

                            bill.FSubmitterId = submitterID; //提交人
                            bill.FSubmitdate = DateTime.Now; //提交时间
                            bill.FState = (int)Model.Enums.EnumPaymentState.AbnormalPayment;
                            bill.PersistentState = SUP.Common.Base.PersistentState.Modified;

                            //保存
                            SavedResult<Int64> savedResult = this.EntFacade.Save<long>(bill);
                            if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
                            {
                                if (billDtls.Count > 0)
                                {
                                    this.GKPaymentDtlFacade.Save<long>(billDtls);
                                }
                                this.PaymentMstFacade.UpdatePayState(bill, billDtls);//更新资金拨付单状态
                            }

                            return null;
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        //支付异常
                        foreach (var dtl in billDtls)
                        {
                            dtl.FResult = "-1";
                            dtl.FResultmsg = ex.Message;

                            dtl.FState = (int)Model.Enums.EnumPaymentState.AbnormalPayment;
                            dtl.PersistentState = SUP.Common.Base.PersistentState.Modified;
                        }

                        bill.FSubmitterId = submitterID; //提交人
                        bill.FSubmitdate = DateTime.Now; //提交时间
                        bill.FState = (int)Model.Enums.EnumPaymentState.AbnormalPayment;
                        bill.PersistentState = SUP.Common.Base.PersistentState.Modified;

                        //保存
                        SavedResult<Int64> savedResult = this.EntFacade.Save<long>(bill);
                        if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
                        {
                            if (billDtls.Count > 0)
                            {
                                this.GKPaymentDtlFacade.Save<long>(billDtls);
                            }
                            //资金拨付单也要支付异常
                            //this.GKPaymentMstFacade.UpdateZjbfPaymentPayState(bill.PhId, bill.FState); //更新资金拨付单状态
                            this.PaymentMstFacade.UpdatePayState(bill, billDtls);//更新资金拨付单状态2
                        }

                        errorMsg = ex.Message;
                        return null;
                    }
                }
                #endregion

                errorMsg = "";
                return null;
            }
            else
            {
                errorMsg = "单据不存在或已被删除，无法支付！";
                return null;
            }
        }

        /// <summary>
        /// 刷新单笔支付单支付状态,如果返回null且errorMsg不为空，则表示出错了
        /// </summary>
        /// <param name="phid"></param>
        /// <param name="errorMsg">错误消息提示信息</param>
        /// <returns></returns>
        public GKPaymentModel RefreshPaymentState(Int64 phid, out string errorMsg)
        {
            errorMsg = "";

            var result = base.EntFacade.Find<long>(phid);
            if (result != null && result.Data != null)
            {

                var bill = result.Data;

                //判断支付单的状态
                if (bill.FApproval == (int)EnumBillApproval.InApproval)
                {
                    errorMsg = "单据在审批中！";
                    return null;
                }

                if (bill.FApproval == (int)EnumBillApproval.PendingSend)
                {
                    errorMsg = "单据待送审！";
                    return null;
                }

                if (bill.FApproval == (int)EnumBillApproval.NotPass)
                {
                    errorMsg = "单据未通过审批！";
                    return null;
                }

                if (bill.FApproval == (int)EnumBillApproval.Approved)
                {
                    if (bill.FState == (int)Model.Enums.EnumPaymentState.Paid)
                    {
                        errorMsg = "单据已支付成功！";
                        return new GKPaymentModel
                        {
                            Mst = bill,
                            Dtls = null
                        };
                    }

                    if (bill.FState == (int)EnumPaymentState.AbnormalPayment)
                    {
                        errorMsg = "单据支付异常，请做异常处理操作！";
                        return null;
                    }

                    if (string.IsNullOrEmpty(bill.FSeqno))
                    {
                        errorMsg = "单据未支付过，无需查询状态！";
                        return null;
                    }
                }


                //准备数据
                //默认使用工行NC客户端
                string bankCode = "102", bankName = "工商银行";
                var bankInfo = new BankInfo { bankCode = bankCode, bankName = bankName, shortName = "ICBC", bankKeys = "工行", description = "中国工商银行" };
                var defaultBankVersion = new BankVersionInfo { versionName = "工商银行NC版", shortName = "ICBCNC", vendor = "杭州政云数据技术有限公司" };

                ICBCNCService icbcService = new ICBCNCService();
                CallerInfo caller = new CallerInfo
                {
                    caller = "web",
                    version = "0.1beta",
                    callerIP = "",
                    callerOS = "",
                    callerTime = DateTime.Now
                };

                var subResult = icbcService.getPaymentState(caller, defaultBankVersion, bill.FSeqno);

                if (subResult.retCode == "0")
                {
                    if (subResult.detailData.infoData == null)
                    {
                        errorMsg = "无法获取支付明细状态!";
                        return null;
                    }
                    else
                    {
                        //指令级返回
                        if (subResult.detailData.infoData.Length > 0)
                        {
                            var dtlResult = this.GKPaymentDtlFacade.FindByForeignKey(phid);
                            if (!dtlResult.IsError && dtlResult.Data != null)
                            {
                                var billDtls = dtlResult.Data;
                                var paymentInfos = subResult.detailData.infoData;

                                Dictionary<long, long> oldMstIds = new Dictionary<long, long>();
                                Dictionary<long, long> oldDtlIds = new Dictionary<long, long>();
                                Dictionary<long, byte> dtlStates = new Dictionary<long, byte>();

                                IEnumerable<PaymentInfo> find = null;
                                int success = 0, failed = 0, during = 0;
                                foreach (var dtl in billDtls)
                                {
                                    find = paymentInfos.Where(x => x.amount == dtl.FAmount && x.oppoBankAcnt.acntNo == dtl.FRecAcnt && x.iSeqno == dtl.FIseqno);
                                    if (find.Count() > 0)
                                    {
                                        dtl.FResult = find.First().result;  //指令状态码
                                        dtl.FResultmsg = find.First().resultMsg; //指令状态描述
                                        dtl.FState = (byte)find.First().payState;
                                        dtl.FSeqno = find.First().paymentSeqNo;
                                        //dtl.FBkSn = 

                                        switch (dtl.FState)
                                        {
                                            case (int)Model.Enums.EnumPaymentState.Paid: //支付成功
                                                success++;
                                                break;
                                            case (int)Model.Enums.EnumPaymentState.AbnormalPayment: //支付异常
                                                failed++;
                                                break;
                                            default:    //支付中
                                                during++;
                                                break;
                                        }

                                        //支付时间
                                        if (find.Single().submitDate == DateTime.MinValue)
                                        {
                                            dtl.FSubmitdate = null;
                                        }
                                        else
                                        {
                                            dtl.FSubmitdate = (Nullable<DateTime>)find.Single().submitDate;
                                        }

                                        dtl.PersistentState = SUP.Common.Base.PersistentState.Modified;

                                        #region 获得原支付单的条件，后面用来同步原支付单状态
                                        if (dtl.OldMstPhid > 0)
                                        {
                                            if (!oldMstIds.ContainsKey(dtl.OldMstPhid))
                                            {
                                                oldMstIds.Add(dtl.OldMstPhid, dtl.OldMstPhid);
                                            }
                                        }

                                        if (dtl.OldDtlPhid > 0)
                                        {
                                            if (!oldDtlIds.ContainsKey(dtl.OldDtlPhid))
                                            {
                                                oldDtlIds.Add(dtl.OldDtlPhid, dtl.OldDtlPhid);
                                                dtlStates.Add(dtl.OldDtlPhid, dtl.FState);
                                            }
                                        }
                                        #endregion

                                    }
                                }

                                if (success == billDtls.Count)
                                {
                                    bill.FState = (int)Model.Enums.EnumPaymentState.Paid;
                                }

                                if (during == billDtls.Count)
                                {
                                    bill.FState = (int)Model.Enums.EnumPaymentState.DuringPayment;
                                }

                                //如果全部支付失败，则单据状态为异常； 
                                //如果支付成功的 +支付失败的和与单据支付明细数一致，则表示单据有确定状态了，状态为异常
                                if (failed == billDtls.Count || (success > 0 && failed > 0 && (success + failed) == billDtls.Count))
                                {
                                    bill.FState = (int)Model.Enums.EnumPaymentState.AbnormalPayment;
                                }

                                bill.PersistentState = SUP.Common.Base.PersistentState.Modified;

                                //保存单据状态
                                SavedResult<Int64> savedResult = this.EntFacade.Save<long>(bill);
                                if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
                                {
                                    if (billDtls.Count > 0)
                                    {
                                        this.GKPaymentDtlFacade.Save<long>(billDtls);

                                        #region 更新老支付单的明细数据的支付状态
                                        if (oldDtlIds.Count > 0)
                                        {
                                            foreach (var id in oldDtlIds)
                                            {
                                                this.GKPaymentMstFacade.UpdateSingleDtlPayState(id.Key, dtlStates[id.Key]); //更新明细表支付状态
                                            }
                                        }
                                        #endregion

                                        #region 更新老支付单的主表数据的支付状态
                                        if (oldMstIds.Count > 0)
                                        {
                                            this.GKPaymentMstFacade.UpdateMstPayState(oldMstIds.Keys.ToList(), bill.FState); //批量更新主表支付状态
                                        }
                                        #endregion

                                    }
                                    this.PaymentMstFacade.UpdatePayState(bill, billDtls);
                                }

                                return new GKPaymentModel
                                {
                                    Mst = bill,
                                    Dtls = billDtls.ToList()
                                };
                            }
                        }
                    }
                }
                else
                {
                    errorMsg = subResult.retMsg;
                }

                return null;
            }
            else
            {
                errorMsg = "单据不存在或已被删除，无法查询支付状态！";
                return null;
            }

        }

        /// <summary>
        /// 获取资金拨付支付单列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="dicWhere"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public PagedResult<GKPayment4ZjbfModel> GetPaymentList4Zjbf(int pageIndex, int pageSize = 20, Dictionary<string, object> dicWhere = null, params string[] sorts)
        {
            PagedResult<GKPayment4ZjbfModel> data = GKPaymentMstFacade.GetPaymentList4Zjbf(pageIndex, pageSize, dicWhere, sorts);
            var paymethods = this.QTSysSetFacade.Find(t => t.DicType == "PayMethod").Data;
            foreach (var a in data.Results)
            {
                if (a.Mst.FPaymethod != 0)
                {

                    //var Paymethod = QTSysSetFacade.Find(a.Mst.FPaymethod).Data;
                    if(paymethods != null && paymethods.Count > 0)
                    {
                        var Paymethod = paymethods.ToList().Find(t => t.PhId == a.Mst.FPaymethod);
                        if(Paymethod != null)
                        {
                            a.Mst.FPaymethodCode = Paymethod.TypeCode;
                            a.Mst.FPaymethodName = Paymethod.TypeName;
                        }
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// 获取资金拨付支付单列表
        /// </summary>
        /// <param name="billRequest"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="dicWhere"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public PagedResult<GKPayment4ZjbfModel> GetPaymentList4Zjbf2(BillRequestModel billRequest, int pageIndex, int pageSize = 20, Dictionary<string, object> dicWhere = null, params string[] sorts)
        {
            OrganizeModel Org = OrganizationFacade.Find(billRequest.Orgid).Data;
            billRequest.OrgCode = Org.OCode;
            Dictionary<string, object> dicset = new Dictionary<string, object>();
            new CreateCriteria(dicset)
                        .Add(ORMRestrictions<string>.Eq("DicType", "splx"))
                        .Add(ORMRestrictions<string>.Eq("Value", "002"));
            IList<QTSysSetModel> qTSysSets = QTSysSetFacade.Find(dicset).Data;
            if (qTSysSets.Count > 0)
            {
                billRequest.Splx_Phid = qTSysSets[0].PhId;
            }
            List<AppvalRecordVo> appval = GAppvalRecordFacade.GetUnDoRecords3(billRequest);
            List<string> PayCodes = appval.Select(x => x.PayNum).ToList();

            var mstResult2 = new List<GKPaymentMstModel>();
            var mstResult = GKPaymentMstFacade.Find(dicWhere, sorts).Data.ToList();
            var TotalItems = 0;
            if (mstResult != null && mstResult.Count > 0)
            {
                TotalItems = mstResult.Count;
                foreach (var mst in mstResult)
                {
                    if (PayCodes.Contains(mst.FCode))
                    {
                        mst.AppvalPhid = appval.Find(x => x.PayNum == mst.FCode).PhId;
                        mst.ProcPhid = appval.Find(x => x.PayNum == mst.FCode).ProcPhid;
                        mst.PostPhid = appval.Find(x => x.PayNum == mst.FCode).PostPhid;
                        mst.OperaPhid = appval.Find(x => x.PayNum == mst.FCode).OperaPhid;
                        mst.IsApprovalNow = true;
                        mstResult2.Add(mst);
                    }
                    else
                    {
                        mst.IsApprovalNow = false;
                    }
                }
            }
            if(mstResult2 != null && mstResult2.Count > 0)
            {
                for (var i = 0; i < mstResult2.Count; i++)
                {
                    if(mstResult != null && mstResult.Count > 0)
                    {
                        mstResult.Remove(mstResult2[i]);
                    }                   
                }
            }
            if (mstResult != null && mstResult.Count > 0)
            {
                mstResult2.AddRange(mstResult);
            }
            //分页
            mstResult2 = mstResult2.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            var data = GKPaymentMstFacade.GetPaymentList4Zjbf2(mstResult2, TotalItems, pageIndex, pageSize);

            var paymethods = this.QTSysSetFacade.Find(t => t.DicType == "PayMethod").Data;
            if(data != null && data.Results != null && data.Results.Count > 0)
            {
                foreach (var a in data.Results)
                {
                    if (a.Mst.FPaymethod != 0)
                    {

                        //var Paymethod = QTSysSetFacade.Find(a.Mst.FPaymethod).Data;
                        if (paymethods != null && paymethods.Count > 0)
                        {
                            var Paymethod = paymethods.ToList().Find(t => t.PhId == a.Mst.FPaymethod);
                            if (Paymethod != null)
                            {
                                a.Mst.FPaymethodCode = Paymethod.TypeCode;
                                a.Mst.FPaymethodName = Paymethod.TypeName;
                            }
                        }
                    }
                }
            }

            //foreach (var a in data.Results)
            //{
            //    if (a.Mst.FPaymethod != 0)
            //    {
            //        var Paymethod = QTSysSetFacade.Find(a.Mst.FPaymethod).Data;
            //        a.Mst.FPaymethodCode = Paymethod.TypeCode;
            //        a.Mst.FPaymethodName = Paymethod.TypeName;
            //    }
            //}

            return data;
        }

        /// <summary>
        /// 更新单据支付状态
        /// </summary>
        /// <param name="phid"></param>
        /// <param name="payState"></param>
        /// <param name="submitterID"></param>
        /// <returns></returns>
        public SavedResult<long> UpdatePaymentState(Int64 phid, byte payState, Int64 submitterID)
        {
            return GKPaymentMstFacade.UpdatePaymentState(phid, payState, submitterID);
        }

        /// <summary>
        /// 根据单据号集合作废单据
        /// </summary>
        /// <param name="phids">单据集合</param>
        /// <returns></returns>
        public SavedResult<long> PostCancetGkPaymentList(List<long> phids)
        {
            return this.GKPaymentMstFacade.PostCancetGkPaymentList(phids);
        }


        /// <summary>
        /// 通过项目属性获取项目属性名称
        /// </summary>
        /// <param name="fProjStatus">项目属性</param>
        /// <returns></returns>
        public string GetProjStatusName(int fProjStatus)
        {
            string projStatusName = "";
            switch (fProjStatus)
            {
                case 1:
                    projStatusName = "预立项";
                    break;
                case 2:
                    projStatusName = "项目立项";
                    break;
                case 3:
                    projStatusName = "项目执行";
                    break;
                case 4:
                    projStatusName = "项目调整";
                    break;
                case 5:
                    projStatusName = "调整项目执行";
                    break;
                case 6:
                    projStatusName = "项目终止";
                    break;
                case 7:
                    projStatusName = "项目关闭";
                    break;
                case 9:
                    projStatusName = "项目预执行";
                    break;
                case 10:
                    projStatusName = "年中新增执行";
                    break;
                case 11:
                    projStatusName = "年中项目执行";
                    break;
                default:
                    projStatusName = "";
                    break;
            }
            return projStatusName;
        }

        /// <summary>
        /// 导出网银模板
        /// </summary>
        /// <param name="list"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public string ExportTemplate(List<GKPaymentDtlModel> list, int Type)
        {
            if (Type == 1)
            {
                #region 工商模板

                string[] head = { "金额", "付款账号开户行名称", "付款账号", "付款账号名称", "收款账号开户行", "收款账号省份", "收款账号地市", "收款账号地区码", "收款账号", "收款账号名称", "汇款用途", "备注信息", "收款账户短信通知手机号码", "自定义序号" };

                //行索引
                int rowNumber = 0;

                HSSFWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("网银模板");
                sheet.DefaultRowHeight = 18 * 20;
                sheet.DefaultColumnWidth = 12;
                sheet.SetColumnWidth(0, 4800);
                sheet.SetColumnWidth(1, 4800);
                sheet.SetColumnWidth(2, 4800);
                sheet.SetColumnWidth(3, 4800);
                sheet.SetColumnWidth(4, 4800);
                sheet.SetColumnWidth(5, 4800);
                sheet.SetColumnWidth(6, 4800);
                sheet.SetColumnWidth(7, 4800);
                sheet.SetColumnWidth(8, 4800);
                sheet.SetColumnWidth(9, 4800);
                sheet.SetColumnWidth(10, 4800);
                sheet.SetColumnWidth(11, 4800);
                sheet.SetColumnWidth(12, 4800);
                sheet.SetColumnWidth(13, 4800);
                //合并单元格
                //sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 14));
                //sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 5));
                //  sheet.AddMergedRegion(new CellRangeAddress(1, 1, 6, 11));
                //       sheet.AddMergedRegion(new CellRangeAddress(1, 1, 12, 14));


                //标题单元格样式
                ICellStyle cellTitleStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 500, 14, false);
                ICellStyle cellTitleStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                ICellStyle cellTitleStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Right, VerticalAlignment.Center, 400, 12, false);
                //表头单元格样式
                ICellStyle cellHeadStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
                //内容单元格样式
                ICellStyle cellStyle1 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                ICellStyle cellStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
                ICellStyle cellStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                //表头
                IRow row = sheet.CreateRow(rowNumber);
                ICell cell = row.CreateCell(0);
                cell.SetCellValue("付款金额");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(1);
                cell.SetCellValue("付款账号开户行名称");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(2);
                cell.SetCellValue("付款账号");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(3);
                cell.SetCellValue("付款账号名称");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(4);
                cell.SetCellValue("收款账号开户行");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(5);
                cell.SetCellValue("收款账号省份");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(6);
                cell.SetCellValue("收款账号地市");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(7);
                cell.SetCellValue("收款账号地区码");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(8);
                cell.SetCellValue("收款账号");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(9);
                cell.SetCellValue("收款账号名称");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(10);
                cell.SetCellValue("汇款用途");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(11);
                cell.SetCellValue("备注信息");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(12);
                cell.SetCellValue("汇款方式");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(13);
                cell.SetCellValue("收款账户短信通知手机号码");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(14);
                cell.SetCellValue("自定义序号");
                cell.CellStyle = cellHeadStyle;
                rowNumber++;

                double sum = 0;
                //表格内容
                for (int i = 1; i <= list.Count; i++)
                {
                    row = sheet.CreateRow(rowNumber);
                    //每行
                    cell = row.CreateCell(0);
                    cell.SetCellValue((double)list[i - 1].FAmount);//金额
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(list[i - 1].FPayBankname);//付款账号开户行名称
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(2);
                    cell.SetCellValue(list[i - 1].FPayAcnt);//付款账号
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(3);
                    cell.SetCellValue(list[i - 1].FPayAcntname);//付款账号名称
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(4);
                    cell.SetCellValue(list[i - 1].FRecBankname);//收款账号开户行
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(5);
                    cell.SetCellValue("");//收款账号省份
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(6);
                    cell.SetCellValue("");//收款账号地市
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(7);
                    cell.SetCellValue("");//收款账号地区码
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(8);
                    cell.SetCellValue(list[i - 1].FRecAcnt);//收款账号
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(9);
                    cell.SetCellValue(list[i - 1].FRecAcntname);//收款账号名称
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(10);
                    cell.SetCellValue(list[i - 1].FUsage);//汇款用途
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(11);
                    cell.SetCellValue(list[i - 1].FDescribe);//备注信息
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(12);
                    cell.SetCellValue("普通");//汇款方式
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(13);
                    cell.SetCellValue("");//收款账户短信通知手机号码
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(14);
                    cell.SetCellValue(i);//自定义序号
                    cell.CellStyle = cellStyle2;
                    rowNumber++;
                }
                MemoryStream ms = new MemoryStream();
                workbook.Write(ms);
                var buf = ms.ToArray();
                string path = AppDomain.CurrentDomain.BaseDirectory + @"\\DownLoadFiles\\YProjectMst";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
                using (FileStream fs = File.OpenWrite(path + "\\" + filename))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                    fs.Close();
                }
                workbook = null;
                ms.Close();
                ms.Dispose();

                return JsonConvert.SerializeObject(new { path = "YProjectMst", filename = filename });
                #endregion
            }
            else if (Type == 2)
            {
                #region 建行模板

                string[] head = { "*序号", "*付款方客户账号", "*付款方账户名称", @"*收款方行别代码
（01-本行 02-国内他行）", "*收款方客户账号", "*收款方账户名称", "收款方开户行名称", "收款方联行号", "客户方流水号", "*金额", "*用途", "备注" };

                //行索引
                int rowNumber = 0;

                HSSFWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("网银模板");
                sheet.DefaultRowHeight = 18 * 20;
                sheet.DefaultColumnWidth = 12;
                sheet.SetColumnWidth(0, 4800);
                sheet.SetColumnWidth(1, 6800);
                sheet.SetColumnWidth(2, 6800);
                sheet.SetColumnWidth(3, 4800);
                sheet.SetColumnWidth(4, 6800);
                sheet.SetColumnWidth(5, 6800);
                sheet.SetColumnWidth(6, 6800);
                sheet.SetColumnWidth(7, 6800);
                sheet.SetColumnWidth(8, 6800);
                sheet.SetColumnWidth(9, 3800);
                sheet.SetColumnWidth(10, 4800);
                sheet.SetColumnWidth(11, 4800);
                //合并单元格
                //sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 14));
                //sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 5));
                //  sheet.AddMergedRegion(new CellRangeAddress(1, 1, 6, 11));
                //       sheet.AddMergedRegion(new CellRangeAddress(1, 1, 12, 14));


                //标题单元格样式
                ICellStyle cellTitleStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 500, 14, false);
                ICellStyle cellTitleStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                ICellStyle cellTitleStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Right, VerticalAlignment.Center, 400, 12, false);
                //表头单元格样式
                ICellStyle cellHeadStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 14, false);
                //内容单元格样式
                ICellStyle cellStyle1 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                ICellStyle cellStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
                ICellStyle cellStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                //表头
                IRow row = sheet.CreateRow(rowNumber);
                row.Height = 50 * 20;
                ICell cell = row.CreateCell(0);
                cellHeadStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.PaleBlue.Index;
                cellHeadStyle.FillPattern = FillPattern.SolidForeground;
                cell.CellStyle = cellHeadStyle;

                cell.SetCellValue("*序号");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(1);
                cell.SetCellValue("*付款方客户账号");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(2);
                cell.SetCellValue("*付款方账户名称");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(3);
                cell.SetCellValue(@"*收款方行别代码
（01 - 本行 02 - 国内他行）");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(4);
                cell.SetCellValue("*收款方客户账号");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(5);
                cell.SetCellValue("*收款方账户名称");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(6);
                cell.SetCellValue("收款方开户行名称");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(7);
                cell.SetCellValue("收款方联行号");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(8);
                cell.SetCellValue("客户方流水号");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(9);
                cell.SetCellValue("*金额");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(10);
                cell.SetCellValue("*用途");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(11);
                cell.SetCellValue("备注");
                cell.CellStyle = cellHeadStyle;
                rowNumber++;
                //表格内容
                for (int i = 1; i <= list.Count; i++)
                {
                    row = sheet.CreateRow(rowNumber++);
                    //每行
                    cell = row.CreateCell(0);
                    cell.SetCellValue(i);//序号
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(1);
                    cell.SetCellValue(list[i - 1].FPayAcnt);//*付款方客户账号
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(2);
                    cell.SetCellValue(list[i - 1].FPayAcntname);//*付款方账户名称
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(3);
                    cell.SetCellValue("");//*收款方行别代码
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(4);
                    cell.SetCellValue(list[i - 1].FRecAcnt);//*收款方客户账号
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(5);
                    cell.SetCellValue(list[i - 1].FRecAcntname);//*收款方账户名称
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(6);
                    cell.SetCellValue(list[i - 1].FRecBankname);//收款方开户行名称
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(7);
                    cell.SetCellValue(list[i - 1].FRecBankcode);//收款方联行号
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(8);
                    cell.SetCellValue(list[i - 1].FBkSn);//客户方流水号
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(9);
                    cell.SetCellValue((Double)list[i - 1].FAmount);//*金额
                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(10);
                    cell.SetCellValue(list[i - 1].FUsage);//用途
                    cell.CellStyle = cellStyle2;

                    cell.CellStyle = cellStyle2;
                    cell = row.CreateCell(11);
                    cell.SetCellValue(list[i - 1].FDescribe);//备注
                    cell.CellStyle = cellStyle2;
                }
                MemoryStream ms = new MemoryStream();
                workbook.Write(ms);
                var buf = ms.ToArray();
                string path = AppDomain.CurrentDomain.BaseDirectory + @"\\DownLoadFiles\\YProjectMst";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
                using (FileStream fs = File.OpenWrite(path + "\\" + filename))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                    fs.Close();
                }
                workbook = null;
                ms.Close();
                ms.Dispose();

                return JsonConvert.SerializeObject(new { path = "YProjectMst", filename = filename });
                #endregion
            }
            else
            {
                return DCHelper.ErrorMessage("数据导出失败");
            }
        }
        /// <summary>
        /// 根据数组获取模板数据
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetPaymentArea(long[] ids, int type)
        {
            List<GKPaymentMstModel> list = new List<GKPaymentMstModel>();
            if (ids != null && type != 0)
            {
                GKPayment4ZjbfModel gkresult = new GKPayment4ZjbfModel();
                foreach (var item in ids)
                {
                    var dic = new Dictionary<string, object>();
                    new CreateCriteria(dic).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                    //new CreateCriteria(dic).Add(ORMRestrictions<Byte>.Eq("FState", 3));
                    new CreateCriteria(dic).Add(ORMRestrictions<Int64>.Eq("PhId", item));
                    var Query = GKPaymentMstFacade.Find(dic).Data.FirstOrDefault();
                    list.Add(Query);

                }
                //var a = GKPaymentMstFacade.FacadeHelper.Find(ids).Data;//直接是根据数组集合进行查询的
                if (list.Count > 0)
                {

                    var dic = new Dictionary<string, object>();
                    new CreateCriteria(dic).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                    //new CreateCriteria(dic).Add(ORMRestrictions<Byte>.Eq("FState", 3));
                    new CreateCriteria(dic).Add(ORMRestrictions<List<Int64>>.In("MstPhid", list.Select(m => m.PhId).ToList()));
                    var Query = GKPaymentDtlFacade.Find(dic).Data.ToList();
                    var result = ExportTemplate(Query, type);//调取工商模板 
                    return result;
                }
                else
                {
                    return DCHelper.ErrorMessage("参数列表获取失败");
                }
            }
            else
            {
                return DCHelper.ErrorMessage("参数传递失败");
            }
        }

        /// <summary>
        /// 获取支付失败和未支付的单据
        /// </summary>
        /// <param name="dicWhere"></param>
        /// <returns></returns>
        public List<GKPaymentMstModel> GetPaymentFailure(Dictionary<string, object> dicWhere)
        {
            FindedResults<GKPaymentMstModel> findedResult = new FindedResults<GKPaymentMstModel>();
            var dic = new Dictionary<string, object>();
            new CreateCriteria(dic).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
            new CreateCriteria(dic).Add(ORMRestrictions<Dictionary<string, object>>.Eq("FState", dicWhere));
            var Query = GKPaymentMstFacade.Find(dic).Data.ToList();
            return Query;
        }

        #endregion
    }
}

