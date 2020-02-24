using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GSP3.SP.Model.Extra
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class Organize
    {
        [DataMember]
        public long OrgId { get; set; }

        [DataMember]
        public string OrgCode { get; set; }

        [DataMember]
        public string OrgName { get; set; }
    }
}
