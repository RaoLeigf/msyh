using Enterprise3.WebApi.GWF3.WF.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GWF3.WF.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class FlowInstanModel: BaseListModel
    {
        /// <summary>
        /// 类型  我发起的流程-1,已办任务-2
        /// </summary>
        [DataMember]
        public int myflowtype { set; get; }
    }
}
