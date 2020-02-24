using GData.YQHL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GData.YQHL.Service.Interface
{
    public interface IBankServiceInterface
    {
        /// <summary>
        /// 取账户余额
        /// </summary>
        /// <param name="caller">调用者</param>
        /// <param name="bankCnt">要查询的账户</param>
        /// <param name="currency">币别</param>
        /// <returns></returns>
        BalanceInfo getBalance(CallerInfo caller, BankAcnt bankCnt, String currency);


        /// <summary>
        /// 取当日交易明细
        /// </summary>
        /// <param name="caller">调用者</param>
        /// <param name="bankAcnt">要查询的账户</param>
        /// <param name="minAmt">交易额下限</param>
        /// <param name="maxAmt">交易额上限</param>
        /// <param name="nextTag">查询下页标识: 当单页不能返回全部记录时，由客户指令通过“查询下页标识”指定页码进行分包返回；“查询下页标识”由银行在上次查询时返回给客户，提供客户下次查询时使用；如果“查询下页标识”返回为空，则标识没有后续明细记录；查询首页上送空；其他页需与银行返回包提供的一致</param>
        /// <param name="currency"></param>
        /// <returns></returns>
        BankReturnModel<DetailInfo[]> getCurrentDetails(CallerInfo caller, BankAcnt bankAcnt, decimal minAmt, decimal maxAmt, string nextTag, string currency);

        /// <summary>
        /// 取历史交易明细
        /// </summary>
        /// <param name="caller">调用者</param>
        /// <param name="bankAcnt">要查询的账户</param>
        /// <param name="currency">币别</param>
        /// <param name="beginTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <returns></returns>
        BankReturnModel<DetailInfo[]> getHistoryDetails(CallerInfo caller, BankAcnt bankAcnt, DateTime beginTime, DateTime endTime, decimal minAmt, decimal maxAmt,string nextTag, string currency);


        /// <summary>
        /// 检查付款单
        /// </summary>
        /// <param name="payment">付款单</param>
        /// <returns></returns>
        string[] checkPayment(PaymentInfo payment);

        /// <summary>
        /// 提交付款
        /// </summary>
        /// <param name="caller">调用者</param>
        /// <param name="payment">付款单</param>
        /// <returns></returns>
        BankReturnModel<PaymentInfo[]> submitPayment(CallerInfo caller, PaymentInfo[] payment);

        /// <summary>
        /// 查询付款状态
        /// </summary>
        /// <param name="caller">调用者</param>
        /// <param name="bankAcnt">要查询的账户</param>
        /// <param name="paymentSeqNo">要查询的付款单序列号</param>
        /// <returns></returns>
        BankReturnModel<PaymentInfo[]> getPaymentState(CallerInfo caller, BankVersionInfo bankVerInfo, String paymentSeqNo);

        /// <summary>
        /// 检查前置机https服务以及签名服务状态
        /// </summary>
        /// <param name="httpsState"></param>
        /// <param name="signState"></param>
        /// <returns></returns>
        string CheckNetSafeClient(out bool httpsState, out bool signState);
    }
}
