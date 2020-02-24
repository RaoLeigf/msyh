using GSP3.SP.Model.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace GSP3.SP.Model.Extra
{
    /// <summary>
    /// Examples实体定义类
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public partial class GAppvalPostAndOpersModel
    {
        /// <summary>
        /// 岗位信息
        /// </summary>
        [DataMember]
        public virtual GAppvalPostModel GAppvalPost
        {
            get;
            set;
        }

        /// <summary>
        /// 岗位对应人员信息(多个)
        /// </summary>
        [DataMember]
        public virtual IList<GAppvalPost4OperModel> GAppvalPost4Opers
        {
            get;
            set;
        }

        /// <summary>
        /// 用户账号
        /// </summary>
        [DataMember]
        public virtual string Ucode { get; set; }
    }
}