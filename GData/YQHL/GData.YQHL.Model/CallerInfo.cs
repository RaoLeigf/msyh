using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GData.YQHL.Model
{
    /// <summary>
    /// 调用者信息
    /// </summary>    
    public class CallerInfo
    {
        /// <summary>
        /// 调用平台的业务系统名称
        /// </summary>
        [DataMember]
        public string caller { get; set; }

        /// <summary>
        /// 调用平台的业务系统版本
        /// </summary>
        [DataMember]
        public string version { get; set; }

        /// <summary>
        /// 调用者IP
        /// </summary>
        [DataMember]
        public string callerIP { get; set; }

        /// <summary>
        /// 调用者操作系统
        /// </summary>
        [DataMember]
        public string callerOS { get; set; }

        /// <summary>
        /// 调用时间
        /// </summary>
        [DataMember]
        public DateTime callerTime { get; set; }

    }
}
