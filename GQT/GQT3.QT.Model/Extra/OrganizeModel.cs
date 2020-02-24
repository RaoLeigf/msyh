using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GQT3.QT.Model.Domain
{
    public partial class OrganizeModel
    {
        /// <summary>
        /// 子组织
        /// </summary>
        [JsonProperty]
        public virtual List<OrganizeModel> children
        {
            get; set;
        }

        /// <summary>
        /// 年初上报标记
        /// </summary>
        [DataMember]
        public virtual System.Int32 VerifyStart
        {
            get;
            set;
        }

        /// <summary>
        /// 年初上报时间
        /// </summary>
        [DataMember]
        public virtual System.DateTime? VerifyStartTime
        {
            get;
            set;
        }

        /// <summary>
        /// 年中上报标记
        /// </summary>
        [DataMember]
        public virtual System.Int32 VerifyMiddle
        {
            get;
            set;
        }

        /// <summary>
        /// 年中上报时间
        /// </summary>
        [DataMember]
        public virtual System.DateTime? VerifyMiddleTime
        {
            get;
            set;
        }

        /// <summary>
        /// 年末上报标记
        /// </summary>
        [DataMember]
        public virtual System.Int32 VerifyEnd
        {
            get;
            set;
        }

        /// <summary>
        /// 年末上报时间
        /// </summary>
        [DataMember]
        public virtual System.DateTime? VerifyEndTime
        {
            get;
            set;
        }

    }
}
