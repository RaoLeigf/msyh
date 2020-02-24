using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace GQT3.QT.Model.Extra
{
    /// <summary>
    /// 关于申报部门的对象
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public partial class DeftDataModel
    {
        /// <summary>
        /// 部门编码
        /// </summary>
        [DataMember]
        public virtual string DeftCode
        {
            get;
            set;
        }

        /// <summary>
        /// 部门名称
        /// </summary>
        [DataMember]
        public virtual string DeftName
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 基础数据返回
    /// </summary>
    public partial class BasicDataModel
    {
        /// <summary>
        /// 数据id
        /// </summary>
        [DataMember]
        public virtual long BasicId
        {
            get;
            set;
        }

        /// <summary>
        /// 数据编码
        /// </summary>
        [DataMember]
        public virtual string BasicCode
        {
            get;
            set;
        }

        /// <summary>
        /// 数据名称
        /// </summary>
        [DataMember]
        public virtual string BasicName
        {
            get;
            set;
        }
    }
}