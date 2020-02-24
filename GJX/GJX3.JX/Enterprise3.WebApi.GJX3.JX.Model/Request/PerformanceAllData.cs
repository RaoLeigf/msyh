using GJX3.JX.Model.Domain;
using GQT3.QT.Model.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Enterprise3.WebApi.GJX3.JX.Model.Request
{
    /// <summary>
    /// 绩效全数据
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class PerformanceAllData : BaseListModel
    {
        /// <summary>
        /// 绩效主表
        /// </summary>
        [DataMember]
        public virtual PerformanceMstModel PerformanceMst
        {
            get;
            set;
        }

        /// <summary>
        /// 绩效子表
        /// </summary>
        [DataMember]
        public virtual IList<PerformanceDtlBuDtlModel> PerformanceDtlBuDtls
        {
            get;
            set;
        }

        /// <summary>
        /// 绩效对应绩效
        /// </summary>
        [DataMember]
        public virtual IList<PerformanceDtlTarImplModel> PerformanceDtlTarImpls
        {
            get;
            set;
        }

        /// <summary>
        /// 绩效内容
        /// </summary>
        [DataMember]
        public virtual IList<PerformanceDtlTextContModel> PerformanceDtlTextConts
        {
            get;
            set;
        }

        /// <summary>
        /// 第三方评价
        /// </summary>
        [DataMember]
        public virtual IList<ThirdAttachmentModel> ThirdAttachmentModels
        {
            get;
            set;
        }

        /// <summary>
        /// 相关附件集合
        /// </summary>
        [DataMember]
        public virtual IList<QtAttachmentModel> QtAttachments
        {
            get;
            set;
        }
       
    }
}