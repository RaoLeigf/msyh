using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GQT3.QT.Model.Extra
{
    /// <summary>
    /// 模块
    /// </summary>
    public class QtModulesModel
    {
        /// <summary>
        /// 子模块
        /// </summary>
        [DataMember]
        public virtual List<QtModulesModel> children
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
        /// 排序
        /// </summary>
        [DataMember]
        public virtual Int32 orderno
        {
            get; set;
        }
        /// <summary>
        /// 产品
        /// </summary>
        [DataMember]
        public virtual string product
        {
            get; set;
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public virtual string chk
        {
            get; set;
        }
        /// <summary>
        /// 授权总数
        /// </summary>
        [DataMember]
        public virtual decimal TotalCount
        {
            get; set;
        }
        /// <summary>
        /// 已授权数
        /// </summary>
        [DataMember]
        public virtual decimal UsedCount
        {
            get; set;
        }
        /// <summary>
        /// 剩余数
        /// </summary>
        [DataMember]
        public virtual decimal RemnantCount
        {
            get; set;
        }
    }
}
