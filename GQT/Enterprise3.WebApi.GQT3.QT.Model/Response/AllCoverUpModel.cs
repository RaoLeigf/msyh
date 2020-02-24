using GQT3.QT.Model.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Enterprise3.WebApi.GQT3.QT.Model.Response
{
    /// <summary>
    /// 套打相关的返回数据
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class AllCoverUpModel
    {
        /// <summary>
        ///  过程编码
        /// </summary>
        [DataMember]
        public string ProcessCode
        {
            get;
            set;
        }

        /// <summary>
        ///  过程名称
        /// </summary>
        [DataMember]
        public string ProcessName
        {
            get;
            set;
        }

        /// <summary>
        /// 对应过程下的所有组织的套打格式信息
        /// </summary>
        [DataMember]
        public IList<QtCoverUpForOrgModel> QtCoverUpForOrgs
        {
            get;
            set;
        }
    }
}