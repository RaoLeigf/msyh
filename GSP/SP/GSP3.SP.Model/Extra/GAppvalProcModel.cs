using Enterprise3.Common.Model.NHORM;
using GSP3.SP.Model.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GSP3.SP.Model.Domain
{
    public partial class GAppvalProcModel: EntityBase<GAppvalProcModel>
    {

        /// <summary>
        /// 组织名称
        /// </summary>
        [DataMember]
        public virtual string OrgName { get; set; }

        /// <summary>
        /// 审批流程和审批岗位的对应关系
        /// </summary>
        [DataMember]
        public virtual List<GAppvalProc4PostModel> Proc4PostModels { get; set; }

        /// <summary>
        /// 审批岗位
        /// </summary>
        [DataMember]
        public virtual List<GAppvalPostModel> PostModels { get; set; }

        /// <summary>
        /// 流程启用条件
        /// </summary>
        [DataMember]
        public virtual List<GAppvalProcCondsModel> CondsModels { get; set; }

        /// <summary>
        /// 当前审批流程启用的组织
        /// </summary>
        [DataMember]
        public virtual List<Organize> Organizes { get; set; }

        /// <summary>
        /// 新增组织集合
        /// </summary>
        [DataMember]
        public virtual List<Organize> NewOrganizes { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        [DataMember]
        public virtual string Ucode { get; set; }
    }
}
