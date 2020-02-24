using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GQT3.QT.Model.Extra
{
    /// <summary>
    /// 取模块对应组织的权限
    /// </summary>
    public class QtModulerightsModel
    {
        /// <summary>
        /// 账套号
        /// </summary>
        [DataMember]
        public virtual string ucode
        {
            get; set;
        }
        /// <summary>
        /// 组织号
        /// </summary>
        [DataMember]
        public virtual string ocode
        {
            get; set;
        }
        /// <summary>
        /// 模块编码
        /// </summary>
        [DataMember]
        public virtual string moduleno
        {
            get; set;
        }
        /// <summary>
        /// 组织名称
        /// </summary>
        [DataMember]
        public virtual string oname
        {
            get; set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public virtual string uname
        {
            get; set;
        }

    }
}
