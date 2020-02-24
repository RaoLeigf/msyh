using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GData.YQHL.Model
{
    /// <summary>
    /// 银行版本信息
    /// </summary>
    public class BankVersionInfo
    {
        /// <summary>
        /// 版本名称
        /// </summary>
        [DataMember]
        public string versionName { get; set; }

        /// <summary>
        /// 版本名称简写
        /// </summary>
        [DataMember]
        public string shortName { get; set; }

        /// <summary>
        /// 开发者
        /// </summary>
        [DataMember]
        public string vendor { get; set; }

    }
}
