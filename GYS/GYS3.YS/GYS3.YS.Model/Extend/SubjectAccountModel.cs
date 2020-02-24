using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace GYS3.YS.Model.Extend
{
    /// <summary>
    /// 关于年初申报的实体定义类
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public partial class SubjectAccountModel
    {
        /// <summary>
        /// 科目代码
        /// </summary>
        [DataMember]
        public virtual string SubjectCode
        {
            get;
            set;
        }

        /// <summary>
        /// 科目名称
        /// </summary>
        [DataMember]
        public virtual string SubjectName
        {
            get;
            set;
        }

        /// <summary>
        /// 科目金额
        /// </summary>
        [DataMember]
        public virtual decimal SubjectAccount
        {
            get;
            set;
        }

        /// <summary>
        /// 科目借方金额
        /// </summary>
        [DataMember]
        public virtual decimal SubjectJ
        {
            get;
            set;
        }
        /// <summary>
        /// 科目贷方金额
        /// </summary>
        [DataMember]
        public virtual decimal SubjectD
        {
            get;
            set;
        }
    }
}