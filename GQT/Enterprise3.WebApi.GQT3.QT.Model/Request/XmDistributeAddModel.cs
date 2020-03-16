using GQT3.QT.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GQT3.QT.Model.Request
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class XmDistributeAddModel
    {
        /// <summary>
        /// 
        /// </summary>
        public List<QtXmDistributeModel> data
        {
            get; set;
        }
        /// <summary>
        /// 年度
        /// </summary>
        [DataMember]
        public string Year
        {
            get; set;
        }
        /// <summary>
		/// 组织id
		/// </summary>
		[DataMember]
        public virtual System.Int64 Orgid
        {
            get;
            set;
        }

        /// <summary>
        /// 组织代码
        /// </summary>
        [DataMember]
        public virtual System.String Orgcode
        {
            get;
            set;
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        [DataMember]
        public virtual bool IfUse
        {
            get;
            set;
        }
        /// <summary>
        /// 用户id
        /// </summary>
        [DataMember]
        public System.Int64 userid
        {
            get; set;
        }
    }
}
