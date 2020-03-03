using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GXM3.XM.Model.Request
{
    // <summary>
    /// 保存立项数据
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public  class ImportXMDataRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [DataMember]
        public long UserId { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        [DataMember]
        public int FYear { get; set; }

        /// <summary>
        /// 机构Id
        /// </summary>
        [DataMember]
        public long OrgId { get; set; }

        /// <summary>
        /// 机构编码
        /// </summary>
        [DataMember]
        public string OrgCode { get; set; }
    }
}
