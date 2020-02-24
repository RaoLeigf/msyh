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
    public class FlowInfoModel: BaseInfoModel
    {
        /// <summary>
        /// 类型  我发起的流程-1,已办任务-2
        /// </summary>
        [DataMember]
        public string flowType { set; get; }
    }
}
