using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Enterprise3.WebApi.GBK3.BK.Model.Request
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class PaymentModel: BaseListModel
    {
        /// <summary>
        /// 年度
        /// </summary>
        [DataMember]
        public string FYear
        {
            get;
            set;
        }
        /// <summary>
        /// 申请单位主键
        /// </summary>
        [DataMember]
        public Int64 FOrgphid
        {
            get;
            set;
        }
        /// <summary>
        /// 申报部门主键
        /// </summary>
        [DataMember]
        public Int64 FDepphid
        {
            get;
            set;
        }
        /// <summary>
        /// 开始筛选日期
        /// </summary>
        [DataMember]
        public DateTime? StartDate
        {
            get;
            set;
        }

        /// <summary>
        /// 结束筛选日期
        /// </summary>
        [DataMember]
        public DateTime? EndDate
        {
            get;
            set;
        }

        /// <summary>
        /// 筛选金额上线
        /// </summary>
        [DataMember]
        public string MaxAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 筛选金额下线
        /// </summary>
        [DataMember]
        public string MinAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 申请单名称
        /// </summary>
        [DataMember]
        public string FName
        {
            get;
            set;
        }

        /// <summary>
        /// 0- 否 1-待支付 9-支付完成
        /// </summary>
        [DataMember]
        public string PayBz
        {
            get;
            set;
        }

        /// <summary>
        /// 0- 未审批 1-待审批 2- 未通过 9-审批通过
        /// </summary>
        [DataMember]
        public string ApprovalBz
        {
            get;
            set;
        }

        /// <summary>
        /// 0- 否 1-待支付 9-支付完成(多个筛选条件)
        /// </summary>
        [DataMember]
        public List<byte> PayBzs
        {
            get;
            set;
        }

        /// <summary>
        /// 0- 未审批 1-待审批 2- 未通过 9-审批通过(多个筛选条件)
        /// </summary>
        [DataMember]
        public List<byte> ApprovalBzs
        {
            get;
            set;
        }

        /// <summary>
        /// 用来筛选的审批流
        /// </summary>
        [DataMember]
        public long ProcPhid
        {
            get;
            set;
        }
    }
}