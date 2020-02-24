using Enterprise3.Common.Model.NHORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GSP3.SP.Model.Domain
{
    /// <summary>
    /// 审批岗位表
    /// </summary>
    public partial class GAppvalPostModel : EntityBase<GAppvalPostModel>
    {
        /// <summary>
        /// 岗位顺序
        /// </summary>
        [DataMember]
        public virtual int Seq { get; set; }

        /// <summary>
        /// 会签模式
        /// </summary>
        [DataMember]
        public virtual byte FMode { get; set; }

        /// <summary>
        /// 岗位对应的操作员
        /// </summary>
        [DataMember]
        public virtual List<GAppvalPost4OperModel> Operators { get; set; }

        /// <summary>
        /// 用来区分是否跨审批流（0-不跨， 1-跨审批流）
        /// </summary>
        [DataMember]
        public virtual int IsSpanBack { get; set; }

        /// <summary>
        /// 跨审批流回退对应的流程id
        /// </summary>
        [DataMember]
        public virtual long BackProcPhid { get; set; }
    }
}
