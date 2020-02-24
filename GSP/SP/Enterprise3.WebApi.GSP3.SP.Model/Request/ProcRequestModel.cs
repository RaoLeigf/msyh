using GSP3.SP.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GSP3.SP.Model.Request
{
    /// <summary>
    /// 审批流程请求参数相关的model
    /// </summary>
    [Serializable]
    public class ProcRequestModel : BaseListModel
    {
        /// <summary>
        /// 审批类型id
        /// </summary>
        [DataMember]
        public long ApprovalTypeId { get; set; }

        /// <summary>
        /// 审批类型id集合
        /// </summary>
        [DataMember]
        public List<long> ApprovalTypeIds { get; set; }

        /// <summary>
        /// 审批类型名称
        /// </summary>
        [DataMember]
        public string ApprovalTypeName { get; set; }

        /// <summary>
        /// 审批类型编码
        /// </summary>
        [DataMember]
        public string ApprovalTypeCode { get; set; }

        /// <summary>
        /// 单据类型("001":资金拨付单,"002":支付单)
        /// </summary>
        [DataMember]
        public string BillType { get; set; }

        /// <summary>
        /// 审批流程id
        /// </summary>
        [DataMember]
        public long ProcId { get; set; }

        /// <summary>
        /// 审批流程编码
        /// </summary>
        [DataMember]
        public string ProcCode { get; set; }

        /// <summary>
        /// 审批流程
        /// </summary>
        [DataMember]
        public List<GAppvalProcModel> ProcModels { get; set; }
    }
}
