using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GData.YQHL.Model
{
    /// <summary>
    /// 银行返回信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BankReturnModel<T> : object where T : class
    {

        /// <summary>
        /// 交易代码
        /// </summary>
        public string transCode { get; set; }

        /// <summary>
        /// 银行代码
        /// </summary>
        public string bankCode { get; set; }
        
        /// <summary>
        /// 交易日期
        /// </summary>
        public string tranDate { get; set; }

        /// <summary>
        /// 交易时间 格式如HHmmssSSS，精确到毫秒
        /// </summary>
        public string tranTime { get; set; }


        /// <summary>
        /// 指令包序列号
        /// </summary>
        public string fSeqno { get; set; }

        /// <summary>
        /// 交易返回码
        /// </summary>
        public string retCode { get; set; }

        /// <summary>
        /// 交易返回描述
        /// </summary>
        public string retMsg { get; set; }

        /// <summary>
        /// 银行产生的银行批次号
        /// </summary>
        public string bankSerialNo { get; set; }        

        /// <summary>
        /// 序列化的明细对象
        /// </summary>
        public BankReturnDetailModel<T> detailData { get; set; }

        /// <summary>
        /// 下页标志
        /// </summary>
        public string nextTag { get; set; }
    }
}
