using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using GData.YQHL.Service.Interface;
using GData.YQHL.Model;
using GData.YQHL.Service;
using GData.YQHL.Common;
using System.Web;

namespace GData.YQHL.WebApi.Controller
{
    /// <summary>
    /// 银行控制类
    /// </summary>
    public class BankServiceController: ApiController
    {

        static readonly FactoryICBCNC icbcncBankFactory = new FactoryICBCNC();
        static readonly FactoryICBCCMP icbccmpBankFactory = new FactoryICBCCMP();

        public BankServiceController() {
            
        }

        /// <summary>
        /// 查询余额
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostGetBalance([FromBody] BankParamModel<BankAcnt> param) {
            string retStr = string.Empty;

            if (string.IsNullOrWhiteSpace(param.caller.callerIP)){
                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrWhiteSpace(ip))
                {
                    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; 
                }
                param.caller.callerIP = ip;
            }

            IBankServiceInterface bankService = null;
            switch (param.infoData.bankVersionInfo.shortName) {
                case "ICBCNC":
                    bankService = icbcncBankFactory.CreateBankService();
                    break;
                case "ICBCCMP":
                    bankService = icbccmpBankFactory.CreateBankService();
                    break;
            }

            if (bankService != null)
            {
                BalanceInfo baInfo = bankService.getBalance(param.caller, param.infoData, param.currency);
                bankService = null;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Data", baInfo);
                DCHelper.ConvertDic2Success(dic);
                return DCHelper.Message(dic);
            }

            return retStr;
        }

        /// <summary>
        /// 查询当日交易明细
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostGetCurrentDetails([FromBody] BankParamModel<BankAcnt> param)
        {
            string retStr = string.Empty;

            if (string.IsNullOrWhiteSpace(param.caller.callerIP))
            {
                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrWhiteSpace(ip))
                {
                    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                param.caller.callerIP = ip;
            }

            IBankServiceInterface bankService = null;
            switch (param.infoData.bankVersionInfo.shortName)
            {
                case "ICBCNC":
                    bankService = icbcncBankFactory.CreateBankService();
                    break;
                case "ICBCCMP":
                    bankService = icbccmpBankFactory.CreateBankService();
                    break;
            }

            if (bankService != null)
            {
                BankReturnModel<DetailInfo[]> retMo = bankService.getCurrentDetails(param.caller, param.infoData, param.minAmt, param.maxAmt, param.nextTag, "");
                bankService = null;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Data", retMo);
                DCHelper.ConvertDic2Success(dic);
                return DCHelper.Message(dic);
            }

            return retStr;
        }

        /// <summary>
        /// 查询历史交易明细
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostGetHistoryDetails([FromBody] BankParamModel<BankAcnt> param)
        {
            string retStr = string.Empty;

            if (string.IsNullOrWhiteSpace(param.caller.callerIP))
            {
                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrWhiteSpace(ip))
                {
                    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                param.caller.callerIP = ip;
            }

            IBankServiceInterface bankService = null;
            switch (param.infoData.bankVersionInfo.shortName)
            {
                case "ICBCNC":
                    bankService = icbcncBankFactory.CreateBankService();
                    break;
                case "ICBCCMP":
                    bankService = icbccmpBankFactory.CreateBankService();
                    break;
            }

            if (bankService != null)
            {
                BankReturnModel<DetailInfo[]> retMo = bankService.getHistoryDetails(param.caller, param.infoData, param.beginDate, param.endDate, param.minAmt, param.maxAmt, param.nextTag, "");
                bankService = null;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Data", retMo);
                DCHelper.ConvertDic2Success(dic);
                return DCHelper.Message(dic);
            }

            return retStr;
        }

        /// <summary>
        /// 提交支付
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostSubmitPayment([FromBody] BankParamModel<PaymentInfo[]> param)
        {
            string retStr = string.Empty;

            if (string.IsNullOrWhiteSpace(param.caller.callerIP))
            {
                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrWhiteSpace(ip))
                {
                    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                param.caller.callerIP = ip;
            }

            IBankServiceInterface bankService = null;
            switch (param.infoData[0].bankAcnt.bankVersionInfo.shortName)
            {
                case "ICBCNC":
                    bankService = icbcncBankFactory.CreateBankService();
                    break;
                case "ICBCCMP":
                    bankService = icbccmpBankFactory.CreateBankService();
                    break;
            }

            if (bankService != null)
            {
                BankReturnModel<PaymentInfo[]> retMo = bankService.submitPayment(param.caller, param.infoData);
                bankService = null;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Data", retMo);
                DCHelper.ConvertDic2Success(dic);
                return DCHelper.Message(dic);
            }

            return DCHelper.ErrorMessage("支付失败!");
        }


        /// <summary>
        /// 查询余额
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostGetPaymentState([FromBody] BankParamModel<BankVersionInfo> param)
        {
            string retStr = string.Empty;

            if (string.IsNullOrWhiteSpace(param.caller.callerIP))
            {
                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrWhiteSpace(ip))
                {
                    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                param.caller.callerIP = ip;
            }

            IBankServiceInterface bankService = null;
            switch (param.infoData.shortName)
            {
                case "ICBCNC":
                    bankService = icbcncBankFactory.CreateBankService();
                    break;
                case "ICBCCMP":
                    bankService = icbccmpBankFactory.CreateBankService();
                    break;
            }

            if (bankService != null)
            {
                BankReturnModel<PaymentInfo[]> retMo = bankService.getPaymentState(param.caller, param.infoData, param.fSeqNo );
                bankService = null;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Data", retMo);
                DCHelper.ConvertDic2Success(dic);
                return DCHelper.Message(dic);
            }

            return retStr;
        }
    }
}
