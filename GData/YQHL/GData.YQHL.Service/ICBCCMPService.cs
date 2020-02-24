using GData.YQHL.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GData.YQHL.Model;

namespace GData.YQHL.Service
{
    /// <summary>
    /// 工行NC版工厂类
    /// </summary>
    public class ICBCCMPService : IBankServiceInterface
    {
        public string[] checkPayment(PaymentInfo payment)
        {
            throw new NotImplementedException();
        }

        public BalanceInfo getBalance(CallerInfo caller, BankAcnt bankCnt, string currency)
        {
            throw new NotImplementedException();
        }

        public BankReturnModel<DetailInfo[]> getCurrentDetails(CallerInfo caller, BankAcnt bankAcnt, decimal minAmt, decimal maxAmt, string nextTag, string currency)
        {
            throw new NotImplementedException();
        }

        public BankReturnModel<DetailInfo[]> getHistoryDetails(CallerInfo caller, BankAcnt bankAcnt, DateTime beginTime, DateTime endTime, decimal minAmt, decimal maxAmt, string nextTag, string currency)
        {
            throw new NotImplementedException();
        }

        public BankReturnModel<PaymentInfo[]> getPaymentState(CallerInfo caller, BankVersionInfo bankVerInfo, String paymentSeqNo)
        {
            throw new NotImplementedException();
        }

        public BankReturnModel<PaymentInfo[]> submitPayment(CallerInfo caller, PaymentInfo[] payment)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 检查前置机https服务以及签名服务状态
        /// </summary>
        /// <param name="httpsState"></param>
        /// <param name="signState"></param>
        /// <returns></returns>
        public string CheckNetSafeClient(out bool httpsState, out bool signState) {
            throw new NotImplementedException();
        }

    }
}
