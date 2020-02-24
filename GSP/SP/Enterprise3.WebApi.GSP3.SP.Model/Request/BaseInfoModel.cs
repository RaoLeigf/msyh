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
    [Serializable]
    [DataContract(Namespace = "")]
    public class BaseInfoModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [DataMember]
        public long Uid { get; set; }

        /// <summary>
        /// 组织ID
        /// </summary>
        [DataMember]
        public long Orgid { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        [DataMember]
        public string Year { get; set; }
    }
}
