using Enterprise3.Common.Model.NHORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GSP3.SP.Model.Domain
{
    public partial class GAppvalPost4OperModel : EntityBase<GAppvalPost4OperModel>
    {
        /// <summary>
        /// 操作员名称
        /// </summary>
        [DataMember]
        public virtual string OperatorName { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [DataMember]
        public virtual string DepName { get; set; }

        /// <summary>
        /// 部门代码
        /// </summary>
        [DataMember]
        public virtual string DepCode { get; set; }

        /// <summary>
        /// 组织名称
        /// </summary>
        [DataMember]
        public virtual string OrgName { get; set; }

        /// <summary>
        /// 组织名称
        /// </summary>
        [DataMember]
        public virtual string OrgCode { get; set; }
    }
}
