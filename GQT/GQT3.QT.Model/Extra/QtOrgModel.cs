using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GQT3.QT.Model.Extra
{
    /// <summary>
    /// 组织（包含是否选中）
    /// </summary>
    public class QtOrgModel
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public virtual string chk
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public virtual bool leaf
        {
            get; set;
        }

        /// <summary>
        /// 模块代码
        /// </summary>
        [DataMember]
        public virtual string cno
        {
            get; set;
        }

        /// <summary>
        /// 模块名称
        /// </summary>
        [DataMember]
        public virtual string text//cname
        {
            get; set;
        }

        /// <summary>
        /// 模块父级代码
        /// </summary>
        [DataMember]
        public virtual string parentcno
        {
            get; set;
        }
        /// <summary>
        /// 级数
        /// </summary>
        [DataMember]
        public virtual string verifyflag
        {
            get; set;
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        [DataMember(Name = "checked")]
        public bool @checked//Ischecked
        {
            get; set;
        }
        /// <summary>
        /// 子
        /// </summary>
        [DataMember]
        public virtual List<QtOrgModel> children
        {
            get; set;
        }

    }
}
