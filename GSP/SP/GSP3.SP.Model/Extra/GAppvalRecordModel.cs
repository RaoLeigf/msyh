using Enterprise3.Common.Model.NHORM;
using GQT3.QT.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GSP3.SP.Model.Domain
{
    /// <summary>
    /// 审批流记录表
    /// </summary>
    public partial class GAppvalRecordModel : EntityBase<GAppvalRecordModel>
    {
        /// <summary>
        /// 操作员名称
        /// </summary>
        [DataMember]
        public virtual string OperaName { get; set; }

        /// <summary>
        /// 审批岗位名称
        /// </summary>
        [DataMember]
        public virtual string PostName { get; set; }

        /// <summary>
        /// 下一岗位审批人
        /// </summary>
        [DataMember]
        public virtual List<long> NextOperators { get; set; }

        /// <summary>
        /// 审批不通过，回退到哪个岗位
        /// </summary>
        [DataMember]
        public virtual long BackPostPhid { get; set; }

        /// <summary>
        /// 多个单据送审
        /// </summary>
        [DataMember]
        public virtual List<long> RefbillPhidList { get; set; }

        /// <summary>
        /// 用来判断送审人
        /// </summary>
        [DataMember]
        public virtual int JudgeRefer { get; set; }

        /// <summary>
        /// 附件集合
        /// </summary> 
        [DataMember]
        public virtual List<QtAttachmentModel> QtAttachments { get; set; }

        /// <summary>
        /// 支付标志（0-未支付，1-支付完成，2-支付异常）
        /// </summary>
        [DataMember]
        public virtual int IsPay { get; set; }

        /// <summary>
        /// 会签标志（0-不是，1-是）
        /// </summary>
        [DataMember]
        public virtual int IsMode { get; set; }

        /// <summary>
        /// 流程岗位顺序号
        /// </summary>
        [DataMember]
        public virtual int SortNum { get; set; }

        /// <summary>
        /// 流程岗位顺序号对应个数
        /// </summary>
        [DataMember]
        public virtual int SameNum { get; set; }

        /// <summary>
        /// 是否跨审批流程退回（0-不跨，1-跨审批流程）
        /// </summary>
        [DataMember]
        public virtual int IsSpanBack { get; set; }

        /// <summary>
        /// 跨审批流回退对应的流程id
        /// </summary>
        [DataMember]
        public virtual long BackProcPhid { get; set; }


        /// <summary>
        /// 所勾选的所有审批记录的主键集合
        /// </summary>
        [DataMember]
        public virtual List<long> RecordPhids { get; set; }

        /// <summary>
        /// 民生银行用来判断送审点击的按钮（0-点击的送审， 1-点击的生成草案）
        /// </summary>
        [DataMember]
        public virtual int ClickType { get; set; }
    }
}
