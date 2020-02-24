using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GSP3.SP.Model.Request
{
    /// <summary>
    /// 
    /// </summary>
    public class BillRequestModel:BaseListModel
    {
        /// <summary>
        /// 单据名称
        /// </summary>
        [DataMember]
        public string BName { get; set; }

        /// <summary>
        /// 单据编号
        /// </summary>
        [DataMember]
        public string BNum { get; set; }

        /// <summary>
        /// 单据类型("001":资金拨付单,"002":支付单)
        /// </summary>
        [DataMember]
        public string BType { get; set; }

        /// <summary>
        /// 审批流程类型Id
        /// </summary>
        [DataMember]
        public long ApprovalType { get; set; }

        /// <summary>
        /// 申报起始日期
        /// </summary>
        [DataMember]
        public DateTime BStartDate { get; set; }

        /// <summary>
        /// 申报截止日期
        /// </summary>
        [DataMember]
        public DateTime BEndTime { get; set; }

        /// <summary>
        /// 停留时长
        /// </summary>
        [DataMember]
        public double StopHour { get; set; }

        /// <summary>
        /// 停留时长的判断条件(1:等于,2:大于,3:小于)
        /// </summary>
        [DataMember]
        public int Operator { get; set; }

        /// <summary>
        /// 审批类型id
        /// </summary>
        [DataMember]
        public long Splx_Phid { get; set; }

        /// <summary>
        /// 单据主键
        /// </summary>
        [DataMember]
        public string BPhIds { get; set; }

        /// <summary>
        /// 单据主键
        /// </summary>
        [DataMember]
        public List<long> PhIds { get; set; }

        /// <summary>
        /// 审批流程的主键
        /// </summary>
        [DataMember]
        public long ProcPhid { get; set; }

        /// <summary>
        /// 审批岗位的主键
        /// </summary>
        [DataMember]
        public long PostPhid { get; set; }

        /// <summary>
        /// 是否是初次加载（1-是（获取所有组织的数据），0-不是）
        /// </summary>
        [DataMember]
        public Int32 IsFirst { get; set; }
    }
}
