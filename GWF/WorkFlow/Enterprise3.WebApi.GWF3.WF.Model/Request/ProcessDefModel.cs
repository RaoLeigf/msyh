using Enterprise3.WebApi.GWF3.WF.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GWF3.WF.Model.Request
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class ProcessDefModel: BaseInfoModel
    {
        /// <summary>
        /// 单据类型
        /// </summary>
        [DataMember]
        public string billtype { set; get; }
        /// <summary>
        /// 单据编码
        /// </summary>
        [DataMember]
        public string billcode { set; get; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string pdid { set; get; }
    }
}
