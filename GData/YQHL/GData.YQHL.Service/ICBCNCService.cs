using GData.YQHL.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GData.YQHL.Model;
using GData.YQHL.Service.ICNBCNC;
using GData.YQHL.Common;
using System.Reflection;
using System.Configuration;
using System.Resources;
using System.IO;
using System.Web;
using System.Collections;
using System.Threading;

namespace GData.YQHL.Service
{
    /// <summary>
    /// 工行NC版工厂类
    /// </summary>
    public class ICBCNCService : IBankServiceInterface
    {
        private static readonly XML_EASY _xmlTool = new XML_EASY();

        public string[] checkPayment(PaymentInfo payment)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获得对应银行账号的当前余额
        /// </summary>
        /// <param name="caller">调用者</param>
        /// <param name="bankCnt">银行账户信息</param>
        /// <param name="currency">币种</param>
        /// <returns></returns>
        public BalanceInfo getBalance(CallerInfo caller, BankAcnt bankCnt, string currency)
        {
            string error = string.Empty;
            var acct = bankCnt.acntNo.Trim();
            if (acct.Length < 19)
            {
                error = "账号长度错误!";
                throw new Exception(error);
            }

            //ICBCNCHelper ncHelper = ICBCNCHelper.GetInstance();
            string xmlString = ICBCNCHelper.GetXmlString("QACCBAL");

            InstallContext ic = new InstallContext();

            //初始化数据
            Type type = bankCnt.GetType();
            PropertyInfo[] ps = type.GetProperties();
            foreach (PropertyInfo i in ps)
            {
                object obj = i.GetValue(bankCnt, null);
                string name = i.Name;
                if (obj != null)
                {
                    ic.Set(i.Name, obj.ToString());
                }
                else
                {
                    ic.Set(name, " ");
                }
            }

            //如果不写币种，会使用账户的币种
            /*
            if (string.IsNullOrEmpty(currency)) {
                currency = "001"; //默认人名币                
            }
            */

            ic.Set("currency", currency);
            ic.Set("bankCode", bankCnt.swiftCode);
            ic.Set("cis", ConfigurationManager.AppSettings["cis"]);
            ic.Set("id", ConfigurationManager.AppSettings["id"]);

            //替换模板中的键为对应的值
            var newXmlStr = ReplaceHelper.ReplaceStringVar(xmlString, ic);


            ICBC_NetSafeClient iCBC_NetSafeClient = CreateOneNC();
            XML_EASY balString = new XML_EASY(newXmlStr);//new XML_EASY(ResourceSet.TransCode_QACCBAL_0010);
            if (acct.Length == 19)
            {
                balString.SetXMLNode("AccNo", acct);
            }
            //else if (acct.Length == 29 && acct[19] == '-')
            //{
            //    balString.SetXMLNode("AccNo", acct.Substring(0, 19));
            //    balString.SetXMLNode("AcctSeq", acct.Substring(20, 9));
            //}

            string seqNo, seqResult;
            iCBC_NetSafeClient.Run(balString.GetXML(), out seqNo, out seqResult);
            iCBC_NetSafeClient = null;

            balString.SetXML(seqResult);
            if (balString.LocateString("<error>") >= 0)
            {
                error = balString.GetXMLNode("error");
                throw new Exception(error);
            }
            string balMoney = balString.GetXMLNode("Balance");
            if (balMoney != null && balMoney.Length > 0)
            {
                BalanceInfo balInfo = new BalanceInfo();

                //取余额
                decimal balance = Convert.ToDecimal(balMoney) / 100.0m; //以币种的最小单位为单位,人民币的单位为分,转换为元要除以100

                //昨日余额
                decimal accBalance = this.getYuanMoney(balString.GetXMLNode("AccBalance"));
                
                //可用余额
                decimal usableBalance = this.getYuanMoney(balString.GetXMLNode("UsableBalance"));

                balInfo.bankAcnt = bankCnt; //账户信息

                balInfo.balance = balance;
                balInfo.hisBalance = accBalance;
                balInfo.availBalance = usableBalance;
                balInfo.acntType = balString.GetXMLNode("AcctProperty"); //账户属性 
                balInfo.currency = balString.GetXMLNode("CurrType"); //币种

                string retTime = balString.GetXMLNode("QueryTime");
                if (!string.IsNullOrEmpty(retTime))
                {
                    balInfo.balanceDate = this.convertToDateTime(retTime);
                }
                
                balInfo.fSeqno = seqNo; //balString.GetXMLNode("fSeqno"); //指令包序列号

                //error = "Name=" + balString.GetXMLNode("AccName") + "|Balance=" + accBalance.ToString() + "|银行应答时间=" + balString.GetXMLNode("QueryTime");
                return balInfo;
            }

            error = "银行RetMsg:" + balString.GetXMLNode("RetMsg");
            throw new Exception(error);
        }

        /// <summary>
        /// 获得人名币元为单位的值
        /// </summary>
        /// <param name="balMoney">以分为单位的人民币</param>
        /// <returns></returns>
        private decimal getYuanMoney(string balMoney) {
            decimal ret = 0;
            if (balMoney != null && balMoney.Length > 0)
            {
                ret = Convert.ToDecimal(balMoney) / 100.0m;
            }
            return ret;
        }

        /// <summary>
        /// 把字符串格式的时间转换为日期时间格式
        /// </summary>
        /// <param name="timeStr"></param>
        /// <returns></returns>
        private DateTime convertToDateTime(string timeStr) {
            if (timeStr != null && timeStr.Length >= 14)
            {
                string year = timeStr.Substring(0, 4);
                string month = timeStr.Substring(4, 2);
                string day = timeStr.Substring(6, 2);
                string hour = timeStr.Substring(8, 2);
                string minute = timeStr.Substring(10, 2);
                string second = timeStr.Substring(12, 2);

                string s = string.Format("{0}-{1}-{2} {3}:{4}:{5}", year, month, day, hour, minute, second);
                DateTime dt = Convert.ToDateTime(s);

                return dt;
            }

            throw new Exception("日期字符串不符合要求: " + timeStr);
        }

        /// <summary>
        /// 获取当日交易明细
        /// </summary>
        /// <param name="caller">调用者</param>
        /// <param name="bankAcnt">银行账户</param>
        /// <param name="currency">目前用不到，可不输入</param>
        /// <returns></returns>
        public BankReturnModel<DetailInfo[]> getCurrentDetails(CallerInfo caller, BankAcnt bankAcnt, decimal minAmt, decimal maxAmt, string nextTag, string currency)
        {
            string error = string.Empty;
            string xmlString = ICBCNCHelper.GetXmlString("QPD");
            XML_EASY xmlQPD = new XML_EASY(xmlString);

            //设置头部信息
            xmlQPD.SetXMLNode("BankCode", "102"); //工行为 "102"
            xmlQPD.SetXMLNode("CIS", ConfigurationManager.AppSettings["cis"]);
            xmlQPD.SetXMLNode("ID", ConfigurationManager.AppSettings["id"]);

            xmlQPD.SetXMLNode("AccNo", bankAcnt.acntNo); //银行账号
            xmlQPD.SetXMLNode("MinAmt", (minAmt*100).ToString()); //发生额下限,人民币分为单位,把元转换为分
            xmlQPD.SetXMLNode("MaxAmt", (maxAmt*100).ToString()); //发生额上限,人民币分为单位,把元转换为分
            xmlQPD.SetXMLNode("NextTag", string.IsNullOrEmpty(nextTag)?"":nextTag); //下页标志

            ICBC_NetSafeClient iCBC_NetSafeClient = CreateOneNC();
            string seqNo, seqResult;
            iCBC_NetSafeClient.Run(xmlQPD.GetXML(), out seqNo, out seqResult);
            iCBC_NetSafeClient = null;

            xmlQPD.SetXML(seqResult);
            if (xmlQPD.LocateString("<error>") >= 0)
            {
                error = xmlQPD.GetXMLNode("error");
                throw new Exception(error);
            }

            string retCode = xmlQPD.GetXMLNode("RetCode");
            if (retCode != "0")
            {
                BankReturnModel<DetailInfo[]> retMo = new BankReturnModel<DetailInfo[]>();
                retMo.retCode = retCode;
                retMo.retMsg = xmlQPD.GetXMLNode("RetMsg");
                retMo.bankCode = "102";
                retMo.fSeqno = seqNo;
                retMo.transCode = "QPD";
                retMo.tranDate = xmlQPD.GetXMLNode("TranDate");
                retMo.tranTime = xmlQPD.GetXMLNode("TranTime");
                return retMo;
            }

            //取交易明细信息
            List<DetailInfo> states = null;
            DetailInfo pInfo = null;

            string rdString = xmlQPD.GetXMLNode("rd");
            int rdIndex = xmlQPD.Index();
            if (!string.IsNullOrEmpty(rdString))
            {
                states = new List<DetailInfo>();
                //循环获取交易明细
                do
                {
                    pInfo = getDetailInfofromRd(bankAcnt, rdString);
                    states.Add(pInfo);

                    rdString = xmlQPD.GetXMLNode("rd", rdIndex);
                    rdIndex = xmlQPD.Index();
                }
                while (!string.IsNullOrEmpty(rdString));

                BankReturnModel<DetailInfo[]> retMo = new BankReturnModel<DetailInfo[]>();
                retMo.retCode = retCode;
                retMo.retMsg = xmlQPD.GetXMLNode("RetMsg");
                retMo.bankCode = "102";
                retMo.fSeqno = seqNo;
                retMo.transCode = "QPD";
                retMo.tranDate = xmlQPD.GetXMLNode("TranDate");
                retMo.tranTime = xmlQPD.GetXMLNode("TranTime");
                retMo.nextTag = xmlQPD.GetXMLNode("NextTag"); //下页标志
                retMo.detailData = new BankReturnDetailModel<DetailInfo[]>();
                retMo.detailData.infoData = states.ToArray();

                return retMo;
            }

            return null;
        }

        public BankReturnModel<DetailInfo[]> getHistoryDetails(CallerInfo caller, BankAcnt bankAcnt, DateTime beginTime, DateTime endTime, decimal minAmt, decimal maxAmt, string nextTag, string currency)
        {
            string error = string.Empty;
            string xmlString = ICBCNCHelper.GetXmlString("QHISD");
            XML_EASY xmlQPD = new XML_EASY(xmlString);

            //设置头部信息
            xmlQPD.SetXMLNode("BankCode", "102"); //工行为 "102"
            xmlQPD.SetXMLNode("CIS", ConfigurationManager.AppSettings["cis"]);
            xmlQPD.SetXMLNode("ID", ConfigurationManager.AppSettings["id"]);

            xmlQPD.SetXMLNode("AccNo", bankAcnt.acntNo); //银行账号
            xmlQPD.SetXMLNode("MinAmt", (minAmt * 100).ToString()); //发生额下限,人民币分为单位,把元转换为分
            xmlQPD.SetXMLNode("MaxAmt", (maxAmt * 100).ToString()); //发生额上限,人民币分为单位,把元转换为分
            xmlQPD.SetXMLNode("NextTag", string.IsNullOrEmpty(nextTag) ? "" : nextTag); //下页标志
            xmlQPD.SetXMLNode("BeginDate", beginTime.ToString("yyyyMMdd"));
            xmlQPD.SetXMLNode("EndDate", endTime.ToString("yyyyMMdd"));

            ICBC_NetSafeClient iCBC_NetSafeClient = CreateOneNC();
            string seqNo, seqResult;
            iCBC_NetSafeClient.Run(xmlQPD.GetXML(), out seqNo, out seqResult);
            iCBC_NetSafeClient = null;

            xmlQPD.SetXML(seqResult);
            if (xmlQPD.LocateString("<error>") >= 0)
            {
                error = xmlQPD.GetXMLNode("error");
                throw new Exception(error);
            }

            string retCode = xmlQPD.GetXMLNode("RetCode");
            if (retCode != "0")
            {
                //error = "error=" + xmlQPD.GetXMLNode("RetMsg");
                //throw new Exception(error);

                BankReturnModel<DetailInfo[]> retMo = new BankReturnModel<DetailInfo[]>();
                retMo.retCode = retCode;
                retMo.retMsg = xmlQPD.GetXMLNode("RetMsg");
                retMo.bankCode = "102";
                retMo.fSeqno = seqNo;
                retMo.transCode = "QHISD";
                retMo.tranDate = xmlQPD.GetXMLNode("TranDate");
                retMo.tranTime = xmlQPD.GetXMLNode("TranTime");
                return retMo;
            }

            //取交易明细信息
            List<DetailInfo> states = null;
            DetailInfo pInfo = null;

            string rdString = xmlQPD.GetXMLNode("rd");
            int rdIndex = xmlQPD.Index();
            if (!string.IsNullOrEmpty(rdString))
            {
                states = new List<DetailInfo>();
                //循环获取交易明细
                do
                {
                    pInfo = getDetailInfofromRd(bankAcnt, rdString);
                    states.Add(pInfo);

                    rdString = xmlQPD.GetXMLNode("rd", rdIndex);
                    rdIndex = xmlQPD.Index();
                }
                while (!string.IsNullOrEmpty(rdString));

                BankReturnModel<DetailInfo[]> retMo = new BankReturnModel<DetailInfo[]>();
                retMo.retCode = retCode;
                retMo.retMsg = xmlQPD.GetXMLNode("RetMsg");
                retMo.bankCode = "102";
                retMo.fSeqno = seqNo;
                retMo.transCode = "QPAYENT";
                retMo.tranDate = xmlQPD.GetXMLNode("TranDate");
                retMo.tranTime = xmlQPD.GetXMLNode("TranTime");
                retMo.nextTag = xmlQPD.GetXMLNode("NextTag"); //下页标志
                retMo.detailData = new BankReturnDetailModel<DetailInfo[]>();
                retMo.detailData.infoData = states.ToArray();

                return retMo;
            }

            return null;
        }

        /// <summary>
        /// 获得支付指令序列号对应的支付状态信息
        /// </summary>
        /// <param name="caller">调用者</param>
        /// <param name="bankVerInfo">银行版本</param>
        /// <param name="paymentSeqNo">支付指令序列号(在调用支付指令后系统返回的序列号(fSeqNo))</param>
        /// <returns></returns>
        public BankReturnModel<PaymentInfo[]> getPaymentState(CallerInfo caller, BankVersionInfo bankVerInfo, String paymentSeqNo)
        {
            string error = string.Empty;
            string xmlString = ICBCNCHelper.GetXmlString("QPAYENT");
            XML_EASY xmlPayState = new XML_EASY(xmlString);

            //设置头部信息
            xmlPayState.SetXMLNode("BankCode", "102"); //工行为 "102"
            xmlPayState.SetXMLNode("CIS", ConfigurationManager.AppSettings["cis"]);
            xmlPayState.SetXMLNode("ID", ConfigurationManager.AppSettings["id"]);

            xmlPayState.SetXMLNode("QryfSeqno", paymentSeqNo); //使用fSeqno来查询
            xmlPayState.SetXMLNode("QrySerialNo", ""); //值为空，则使用fSeqno来查询

            ICBC_NetSafeClient iCBC_NetSafeClient = CreateOneNC();
            string seqNo, seqResult;
            iCBC_NetSafeClient.Run(xmlPayState.GetXML(), out seqNo, out seqResult);
            iCBC_NetSafeClient = null;

            xmlPayState.SetXML(seqResult);
            if (xmlPayState.LocateString("<error>") >= 0)
            {
                error = xmlPayState.GetXMLNode("error");
                throw new Exception(error);
            }

            string retCode = xmlPayState.GetXMLNode("RetCode");
            if ( retCode!= "0")
            {
                //error = "error=" + xmlPayState.GetXMLNode("RetMsg");
                //throw new Exception(error);

                BankReturnModel<PaymentInfo[]> retMo = new BankReturnModel<PaymentInfo[]>();
                retMo.retCode = retCode;
                retMo.retMsg = xmlPayState.GetXMLNode("RetMsg");
                retMo.bankCode = "102";
                retMo.fSeqno = seqNo;
                retMo.transCode = "QPAYENT";
                retMo.tranDate = xmlPayState.GetXMLNode("TranDate");
                retMo.tranTime = xmlPayState.GetXMLNode("TranTime");
                return retMo;
            }

            //取支付明细信息
            List<PaymentInfo> states = null;
            PaymentInfo pInfo = null;            

            string rdString = xmlPayState.GetXMLNode("rd");
            int rdIndex = xmlPayState.Index();
            //string result = xmlPayState.GetXMLNode("Result", rdIndex);
            if (!string.IsNullOrEmpty(rdString))
            {
                states = new List<PaymentInfo>();
                //循环获取明细
                do
                {
                    pInfo = getPaymentInfofromRd(bankVerInfo, rdString);
                    if (pInfo.submitDate == DateTime.MinValue) {
                        pInfo.submitDate = this.convertToDateTime(xmlPayState.GetXMLNode("TranDate") + xmlPayState.GetXMLNode("TranTime"));
                    }
                    pInfo.paymentSeqNo = paymentSeqNo;
                    states.Add(pInfo);

                    rdString = xmlPayState.GetXMLNode("rd", rdIndex);
                    rdIndex = xmlPayState.Index();
                }
                while (!string.IsNullOrEmpty(rdString));

                BankReturnModel < PaymentInfo[] > retMo = new BankReturnModel<PaymentInfo[]>();
                retMo.retCode = retCode;
                retMo.retMsg = xmlPayState.GetXMLNode("RetMsg");
                retMo.bankCode = "102";
                retMo.fSeqno = seqNo;
                retMo.transCode = "QPAYENT";
                retMo.tranDate = xmlPayState.GetXMLNode("TranDate");
                retMo.tranTime = xmlPayState.GetXMLNode("TranTime");                
                retMo.detailData = new BankReturnDetailModel<PaymentInfo[]>();
                retMo.detailData.infoData = states.ToArray();

                return retMo;
            }

            return null;
        }

        /// <summary>
        /// 根据指令状态返回支付状态
        /// </summary>
        /// <param name="resultCode"></param>
        /// <returns></returns>
        static EnumPaymentState getPaymentStateByResultCode(string resultCode) {
            switch (resultCode) {
                case "7":
                    return EnumPaymentState.Paid;
                case "6":
                case "8":
                    return EnumPaymentState.AbnormalPayment;
                default:
                    return EnumPaymentState.DuringPayment;
            }
        }
        

        /// <summary>
        /// 取得指令状态对应的消息描述
        /// </summary>
        /// <param name="resultCode"></param>
        /// <returns></returns>
        static string getMsgByResultCode(string resultCode) {
            string msg = "";
            switch (resultCode) {
                case "0":
                    msg = "提交成功，等待银行处理";
                    break;
                case "1":
                    msg = "授权成功，等待银行处理";
                    break;
                case "2":
                    msg = "等待授权";
                    break;
                case "3":
                    msg = "等待二次授权";
                    break;
                case "4":
                    msg = "等待银行答复";
                    break;
                case "5":
                    msg = "主机返回待处理";
                    break;
                case "6":
                    msg = "被银行拒绝";
                    break;
                case "7":
                    msg = "处理成功";
                    break;
                case "8":
                    msg = "指令被拒绝授权";
                    break;
                case "9":
                    msg = "银行正在处理";
                    break;
                case "10":
                    msg = "预约指令";
                    break;
                case "11":
                    msg = "预约取消";
                    break;
                case "86":
                    msg = "等待电话核实";
                    break;
                case "95":
                    msg = "待核查";
                    break;
                case "98":
                    msg = "区域中心通讯可疑";
                    break;
            }

            return msg;
        }

        /// <summary>
        /// 从rd节点获取支付信息
        /// </summary>
        /// <param name="bankVerInfo"></param>
        /// <param name="rdString"></param>
        /// <returns></returns>
        private PaymentInfo getPaymentInfofromRd(BankVersionInfo bankVerInfo, string rdString)
        {
            //获得支付信息
            PaymentInfo pInfo = new PaymentInfo();
            XML_EASY xmlPay = new XML_EASY(rdString);
            pInfo.response = rdString;

            pInfo.iSeqno = xmlPay.GetXMLNode("iSeqno");  //支付序号
            pInfo.result = xmlPay.GetXMLNode("Result");            
            pInfo.payState = (int)getPaymentStateByResultCode(pInfo.result);
            pInfo.resultMsg = getMsgByResultCode(pInfo.result);

            if (pInfo.result != "7")
            {
                string iRetCode = xmlPay.GetXMLNode("iRetCode");
                string iRetMsg = xmlPay.GetXMLNode("iRetMsg");
                if (!string.IsNullOrEmpty(iRetCode) || !string.IsNullOrEmpty(iRetMsg))
                {
                    if (iRetCode != "0")
                    {
                        pInfo.resultMsg += ":" + iRetCode + " " + iRetMsg;
                    }
                    else {
                        pInfo.resultMsg += ":" + xmlPay.GetXMLNode("instrRetCode") + " " + xmlPay.GetXMLNode("instrRetMsg");
                    }
                }
            }

            pInfo.amount = this.getYuanMoney(xmlPay.GetXMLNode("PayAmt"));
            pInfo.postScript = xmlPay.GetXMLNode("PostScript");

            pInfo.currency = xmlPay.GetXMLNode("CurrType");
            pInfo.explantion = xmlPay.GetXMLNode("Summary");
            pInfo.usage = xmlPay.GetXMLNode("UseCN");
            pInfo.sameCity = (int)(xmlPay.GetXMLNode("IsSameCity") == "1" ? enumYesNo.Yes : enumYesNo.No);
            pInfo.sameBank = (int)(xmlPay.GetXMLNode("SysIOFlg") == "1" ? enumYesNo.Yes : enumYesNo.No);
            pInfo.isUrgent = (int)(xmlPay.GetXMLNode("PayType") == "1" ? enumYesNo.Yes : enumYesNo.No);
            string retTime = xmlPay.GetXMLNode("BankRetTime");
            if (!string.IsNullOrEmpty(retTime)) {
                pInfo.submitDate = this.convertToDateTime(retTime);
            }
            
            pInfo.bankAcnt = new BankAcnt();
            pInfo.bankAcnt.acntName = xmlPay.GetXMLNode("PayAccNameCN");
            pInfo.bankAcnt.acntName = xmlPay.GetXMLNode("PayAccNo");
            pInfo.bankAcnt.bankVersionInfo = bankVerInfo;

            pInfo.oppoBankAcnt = new BankAcnt();
            pInfo.oppoBankAcnt.acntName = xmlPay.GetXMLNode("RecAccNameCN");
            pInfo.oppoBankAcnt.acntNo = xmlPay.GetXMLNode("RecAccNo"); 
            
            return pInfo;
        }


        /// <summary>
        /// 从rd节点获取交易明细信息
        /// </summary>
        /// <param name="bankVerInfo"></param>
        /// <param name="rdString"></param>
        /// <returns></returns>
        private DetailInfo getDetailInfofromRd(BankAcnt bankAcnt, string rdString)
        {
            //获得交易明细信息
            DetailInfo DInfo = new DetailInfo();
            XML_EASY xml = new XML_EASY(rdString);

            DInfo.bankAcnt = bankAcnt; //本方账号
            //DInfo.bussinessName = 
            
            decimal amount = this.getYuanMoney(xml.GetXMLNode("Amount"));            
            DInfo.bussinessRefNo = xml.GetXMLNode("Ref");
            if (xml.GetXMLNode("Drcrf") == "1")
            {
                //借方
                DInfo.crebitAmount = amount;
            }
            else {
                //贷方
                DInfo.debitAmount = amount;
            }

            DInfo.currency = xml.GetXMLNode("CurrType"); //币种
            DInfo.explantion = xml.GetXMLNode("Summary"); //摘要
            DInfo.rawTransTime = xml.GetXMLNode("TimeStamp"); //时间戳
            DInfo.usage = xml.GetXMLNode("UseCN"); //用途
            DInfo.vouhNo = xml.GetXMLNode("VouhNo"); //凭证号
            DInfo.cVouhType = xml.GetXMLNode("CvouhType"); //凭证种类
            DInfo.postScript = xml.GetXMLNode("PostScript"); //附言
            DInfo.addInfo = xml.GetXMLNode("AddInfo"); //附件信息

            //对方账号
            DInfo.oppoBankAcnt = new BankAcnt();
            DInfo.oppoBankAcnt.acntName = xml.GetXMLNode("RecipName"); //对方户明
            DInfo.oppoBankAcnt.acntNo = xml.GetXMLNode("RecipAccNo"); //对方账号
            DInfo.oppoBankAcnt.bankInfo = new BankInfo();
            DInfo.oppoBankAcnt.bankInfo.bankCode = xml.GetXMLNode("RecipBkNo"); //对方行号
            
            return DInfo;
        }

        /// <summary>
        /// 提交支付指令
        /// </summary>
        /// <param name="caller">调用者</param>
        /// <param name="payments">支付单据数组</param>
        /// <returns></returns>
        public BankReturnModel<PaymentInfo[]> submitPayment(CallerInfo caller, PaymentInfo[] payments)
        {
            if (payments.Length < 1) {
                throw new Exception("没有支付信息！");
            }
            
            string xmlString = ICBCNCHelper.GetXmlString("PAYENT");
            XML_EASY xmlPay = new XML_EASY(xmlString);
            string xmlRd = xmlPay.GetXMLNode("rd");
            
            var newXmlStr = string.Empty;
            decimal amount = 0;
            int num = 0; //支付笔数或序号
            StringBuilder sb = new StringBuilder();
            XML_EASY xmlTmp = new XML_EASY();
            string s = string.Empty;
            int iseq = 0;
            payments = payments.OrderBy(t => t.iSeqno).ToArray();  //iSeqno 在银行端支付时按rd的顺序，不按iSeqno来，需要先按iSeqno排序后，在生成rd
            foreach (PaymentInfo pay in payments) {
                num++;
                iseq = string.IsNullOrEmpty(pay.iSeqno)? num : int.Parse(pay.iSeqno);
                sb.Append("<rd>");
                sb.Append(this.dealwithPaymentRd(xmlRd, pay, iseq));
                sb.Append("</rd>");

                if (pay.amount != 0)
                {
                    amount += pay.amount;
                }
            }
            xmlPay.RepXMLNode("rd", sb.ToString());

            //设置头部信息
            xmlPay.SetXMLNode("BankCode", string.IsNullOrEmpty(payments[0].bankAcnt.swiftCode)?"102": payments[0].bankAcnt.swiftCode); //工行为 "102"
            xmlPay.SetXMLNode("CIS", ConfigurationManager.AppSettings["cis"]);
            xmlPay.SetXMLNode("ID", ConfigurationManager.AppSettings["id"]);

            //设置支付信息
            xmlPay.SetXMLNode("TotalNum", num.ToString("f0"));
            xmlPay.SetXMLNode("TotalAmt", amount.ToString("f0"));

            newXmlStr = xmlPay.GetXML();
            string result = this.ICBC_YQHL(newXmlStr);
            xmlPay.SetXML(result);
            if (xmlPay.LocateString("<error>") >= 0)
            {
                result = xmlPay.GetXMLNode("error");
                throw new Exception(result);
            }

            if (!string.IsNullOrEmpty(result))
            {
                XML_EASY xml_easy1 = new XML_EASY(result);
                BankReturnModel<PaymentInfo[]> retMo = new BankReturnModel<PaymentInfo[]>();

                retMo.bankCode = xml_easy1.GetXMLNode("BankCode");
                retMo.transCode = xml_easy1.GetXMLNode("TransCode");
                retMo.tranDate = xml_easy1.GetXMLNode("TranDate");
                retMo.tranTime = xml_easy1.GetXMLNode("TranTime");
                retMo.fSeqno = xml_easy1.GetXMLNode("fSeqno");
                retMo.retCode = xml_easy1.GetXMLNode("RetCode");
                retMo.retMsg = xml_easy1.GetXMLNode("RetMsg");
                retMo.bankSerialNo = xml_easy1.GetXMLNode("SerialNo");


                string resultCode = xml_easy1.GetXMLNode("Result");
                if (string.IsNullOrEmpty(resultCode)) {
                    //文件级返回

                    ////再次发送指令查询状态
                    //if (retMo.retCode == "0")
                    //{
                    //    Thread.Sleep(20 * 1000); //延时后再查询
                    //    return this.getPaymentState(caller, payments[0].bankAcnt.bankVersionInfo, retMo.fSeqno);
                    //}

                    //直接返回结果
                    retMo.detailData = new BankReturnDetailModel<PaymentInfo[]>();
                    retMo.detailData.resultCode = resultCode;
                    retMo.detailData.iRetCode = xml_easy1.GetXMLNode("iRetCode");
                    retMo.detailData.iRetMsg = xml_easy1.GetXMLNode("iRetMsg");
                    retMo.detailData.businessRefNo = xml_easy1.GetXMLNode("Ref");
                    retMo.detailData.infoData = null; //payment;
                    return retMo;
                }
                else
                {
                    //单笔明细支付，指令级返回
                    //retMo.detailData = new BankReturnDetailModel<PaymentInfo[]>();
                    //retMo.detailData.resultCode = resultCode;
                    //retMo.detailData.iRetCode = xml_easy1.GetXMLNode("iRetCode");
                    //retMo.detailData.iRetMsg = xml_easy1.GetXMLNode("iRetMsg");
                    //retMo.detailData.businessRefNo = xml_easy1.GetXMLNode("Ref");
                    //retMo.detailData.infoData = null; //payment;

                    string rdString = xml_easy1.GetXMLNode("rd");
                    int rdIndex = xml_easy1.Index();
                    //string result = xmlPayState.GetXMLNode("Result", rdIndex);
                    if (!string.IsNullOrEmpty(rdString))
                    {
                        var states = new List<PaymentInfo>();
                        PaymentInfo pInfo = null;
                        //循环获取明细
                        do
                        {
                            pInfo = getPaymentInfofromRd(payments[0].bankAcnt.bankVersionInfo, rdString);
                            pInfo.paymentSeqNo = xml_easy1.GetXMLNode("fSeqno");
                            if (pInfo.submitDate == DateTime.MinValue)
                            {
                                pInfo.submitDate = this.convertToDateTime(xml_easy1.GetXMLNode("TranDate") + xml_easy1.GetXMLNode("TranTime"));
                            }
                            states.Add(pInfo);

                            rdString = xml_easy1.GetXMLNode("rd", rdIndex);
                            rdIndex = xml_easy1.Index();
                        }
                        while (!string.IsNullOrEmpty(rdString));

                        retMo.detailData = new BankReturnDetailModel<PaymentInfo[]>();
                        retMo.detailData.infoData = states.ToArray();

                        return retMo;
                    }

                }
                return retMo;
            }

            return null;
        }

        /// <summary>
        /// 生成rd中的支付信息
        /// </summary>
        /// <param name="xmlRd">rd字符串</param>
        /// <param name="payment">支付信息</param>
        /// <param name="seq">支付顺序</param>
        /// <returns></returns>
        private string dealwithPaymentRd(string xmlRd, PaymentInfo payment, int seq) {
            string ret = string.Empty;

            string error = string.Empty;
            var acct = payment.bankAcnt.acntNo.Trim(); //工行账户，支付方
            if (acct.Length < 19)
            {
                error = string.Format("账号长度错误!", payment.bankAcnt.acntNo);
                throw new Exception(error);
            }

            InstallContext ic = new InstallContext();

            //初始化数据    
            string name = string.Empty;
            object obj = null;

            //本方账户
            Type type = payment.bankAcnt.GetType();
            PropertyInfo[] ps = type.GetProperties();
            foreach (PropertyInfo i in ps)
            {
                obj = i.GetValue(payment.bankAcnt, null);
                name = "bankAcnt_" + i.Name;
                if (obj != null)
                {
                    ic.Set(name, obj.ToString());
                }
                else
                {
                    ic.Set(name, " ");
                }
            }

            //对方账户
            type = payment.oppoBankAcnt.GetType();
            ps = type.GetProperties();
            foreach (PropertyInfo i in ps)
            {
                obj = i.GetValue(payment.oppoBankAcnt, null);
                name = "oppoBankAcnt_" + i.Name;
                if (obj != null)
                {
                    ic.Set(name, obj.ToString());
                }
                else
                {
                    ic.Set(name, " ");
                }
            }

            //付款单
            type = payment.GetType();
            ps = type.GetProperties();
            
            foreach (PropertyInfo i in ps)
            {
                obj = i.GetValue(payment, null);
                name = i.Name;
                if (obj != null)
                {
                    if ( string.IsNullOrWhiteSpace( obj.ToString().Trim() ) )
                    {
                        ic.Set(name, " ");
                    }
                    else
                    {
                        ic.Set(name, obj.ToString());
                    }
                }
                else
                {
                    ic.Set(name, " ");
                }
            }

            //是否加绩
            if (payment.isUrgent == (int)enumYesNo.Yes)
            {
                ic.Set("isUrgent", "1"); //加急
            }
            else
            {
                ic.Set("isUrgent", "2"); //普通
            }

            //是否同城
            if (payment.sameCity == (int)enumYesNo.Yes)
            {
                ic.Set("sameCity", "1"); //同城
            }
            else
            {
                ic.Set("sameCity", "2"); //异地
            }

            //是否跨行
            if (payment.sameBank == (int)enumYesNo.Yes)
            {
                ic.Set("sameBank", "1"); //系统内，工行

                //把跨行数据置空
                ic.Set("oppoBankAcnt_city", " ");
                ic.Set("oppoBankAcnt_bankInfo_bankCode", " ");
                ic.Set("oppoBankAcnt_bankInfo_bankName", " ");
            }
            else
            {
                ic.Set("sameBank", "2"); //跨行

                //跨行数据
                ic.Set("oppoBankAcnt_city", payment.oppoBankAcnt.city);
                ic.Set("oppoBankAcnt_bankInfo_bankCode", payment.oppoBankAcnt.bankInfo.bankCode);
                ic.Set("oppoBankAcnt_bankInfo_bankName", payment.oppoBankAcnt.bankInfo.bankName);
            }

            //如果不写币种，则默认人民币
            string currency = payment.currency;
            if (string.IsNullOrEmpty(currency))
            {
                currency = "001"; //默认人名币                
            }

            payment.amount = payment.amount * 100; //工行按分来计算，元转分要乘以100

            ic.Set("currency", currency);
            ic.Set("bankCode", payment.bankAcnt.swiftCode); //工行为 "102"
            ic.Set("cis", ConfigurationManager.AppSettings["cis"]);
            ic.Set("id", ConfigurationManager.AppSettings["id"]);

            ic.Set("iSeqno", seq.ToString()); //支付顺序号

            //替换模板中的键为对应的值
            var newXmlStr = ReplaceHelper.ReplaceStringVar(xmlRd, ic);
            _xmlTool.SetXML(newXmlStr);
            _xmlTool.SetXMLNode("PayAmt", payment.amount.ToString("f0")); //必须是整数
            newXmlStr = _xmlTool.GetXML();

            return newXmlStr;
        }

        /// <summary>
        /// 创建网银客户端
        /// </summary>
        /// <returns></returns>
        private ICBC_NetSafeClient CreateOneNC()
        {
            ICBC_NetSafeClient client1 = new ICBC_NetSafeClient();
            client1.SetHttpsIP(ConfigurationManager.AppSettings["nc_ip"]);
            client1.SetSignIP(ConfigurationManager.AppSettings["nc_ip"]);
            client1.SetHttpsPort(Convert.ToInt32(ConfigurationManager.AppSettings["nc_hp"]));
            client1.SetSignPort(Convert.ToInt32(ConfigurationManager.AppSettings["nc_sp"]));
            client1.SetInterfaceVersion("0.0.0.1");
            client1.SetGroupID(ConfigurationManager.AppSettings["cis"]);
            client1.SetCertID(ConfigurationManager.AppSettings["id"]);

            //应该从配置文件里面获取，这里不设置
            string path = Path.Combine(HttpRuntime.AppDomainAppPath, @"ICBC_Log\" + DateTime.Now.ToString("yyyyMMdd"));
            bool flag1 = ConfigurationManager.AppSettings["log"].Trim().ToLower() == "on";
            if (flag1 && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (flag1 && Directory.Exists(path))
            {
                client1.SetLogFilePath(path);
                client1.SetLogSwitch(true);
            }

            client1.SetHttpsTimeOut(30);
            client1.SetSignTimeOut(10);
            return client1;
        }

        /// <summary>
        /// 封装发送xml请求方法
        /// </summary>
        /// <param name="xml_str"></param>
        /// <param name="ver"></param>
        /// <returns></returns>
        private string ICBC_YQHL(string xml_str, string ver ="0.0.0.1")
        {

            XML_EASY xml_easy1 = new XML_EASY(xml_str);
            string xMLNode = xml_easy1.GetXMLNode("CIS", 0);
            string str3 = xml_easy1.GetXMLNode("ID", 0);
            if (((xMLNode != ConfigurationManager.AppSettings["cis"]) || (str3 != ConfigurationManager.AppSettings["id"])) || (ver.Length == 0))
            {
                return "ReqID=|ReqResult=XML中CIS、ID与设定不符，或版本号信息有误！";
            }
            ICBC_NetSafeClient client = this.CreateOneNC();
            if (string.IsNullOrWhiteSpace(ver)) {
                ver = "0.0.0.1";
            }
            client.SetInterfaceVersion(ver);

            //string path = Path.Combine(HttpRuntime.AppDomainAppPath, @"ICBC_Log\" + DateTime.Now.ToString("yyyyMMdd"));
            //bool flag1 = ConfigurationManager.AppSettings["log"].Trim().ToLower() == "on";
            //if (flag1 && !Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}
            //if (flag1 && Directory.Exists(path))
            //{
            //    client.SetLogFilePath(path);
            //    client.SetLogSwitch(true);
            //}

            string seqNo, result;
            client.Run(xml_str, out seqNo, out result);
            client = null;

            //return ("ReqID=" + seqNo + "|ReqResult=" + Convert.ToBase64String(Encoding.GetEncoding("GBK").GetBytes(result)));
            return result;
        }


        /// <summary>
        /// 检查前置机https服务以及签名服务状态
        /// </summary>
        /// <param name="httpsState"></param>
        /// <param name="signState"></param>
        /// <returns></returns>
        public string CheckNetSafeClient(out bool httpsState, out bool signState)
        {
            httpsState = false;
            signState = false;
            string msg = string.Empty;

            #region 检查https服务
            string error = string.Empty;
            BankAcnt bankCnt = new BankAcnt {
                acntNo = "1202022719927388888",
                acntName = "菌邢票董租氮蒸幻憨野该痼赴挥傻"
            };            

            var acct = bankCnt.acntNo.Trim();
            if (acct.Length < 19)
            {
                error = "账号长度错误!";
                throw new Exception(error);
            }
                
            string xmlString = ICBCNCHelper.GetXmlString("QACCBAL");

            InstallContext ic = new InstallContext();

            //初始化数据
            Type type = bankCnt.GetType();
            PropertyInfo[] ps = type.GetProperties();
            foreach (PropertyInfo i in ps)
            {
                object obj = i.GetValue(bankCnt, null);
                string name = i.Name;
                if (obj != null)
                {
                    ic.Set(i.Name, obj.ToString());
                }
                else
                {
                    ic.Set(name, " ");
                }
            }
            
            ic.Set("currency", "001");
            ic.Set("bankCode", "102");
            ic.Set("cis", ConfigurationManager.AppSettings["cis"]);
            ic.Set("id", ConfigurationManager.AppSettings["id"]);

            //替换模板中的键为对应的值
            var newXmlStr = ReplaceHelper.ReplaceStringVar(xmlString, ic);
            
            ICBC_NetSafeClient iCBC_NetSafeClient = CreateOneNC();
            XML_EASY balString = new XML_EASY(newXmlStr);
            if (acct.Length == 19)
            {
                balString.SetXMLNode("AccNo", acct);
            }

            string seqNo, seqResult;
            iCBC_NetSafeClient.Run(balString.GetXML(), out seqNo, out seqResult);
            

            balString.SetXML(seqResult);
            if (balString.LocateString("<error>") >= 0)
            {
                error = balString.GetXMLNode("error");
                httpsState = false;
                msg = "前置机Https服务有问题:" + error;
            }
            else
            {
                httpsState = true;
                msg = "前置机Https服务OK";
            }
            #endregion

            #region 检查签名服务
            string signstr = iCBC_NetSafeClient.GetSignStr("www.gtdata.com");
            if (!string.IsNullOrEmpty(signstr) && signstr.IndexOf("<sign>") > 0)
            {
                signState = true;
                msg += "  前置机签名服务OK.";
            }
            else {
                signState = false;
                msg += "  前置机签名服务有问题.";
            }
            #endregion

            iCBC_NetSafeClient = null;

            return msg;
        }
    }
}
