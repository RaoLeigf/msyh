using GQT3.QT.Model.Domain;
using GXM3.XM.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GXM3.XM.Model.Request
{
    /// <summary>
    /// 保存签报单数据
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class XmReportSaveModel
    {
        /// <summary>
        /// 签报单主表对象
        /// </summary>
        [DataMember]
        public XmReportMstModel XmReportMst
        {
            get;
            set;
        }
        /// <summary>
        /// 签报单明细对象
        /// </summary>
        [DataMember]
        public List<XmReportDtlModel> XmReportDtls
        {
            get;
            set;
        }

        /// <summary>
        /// 签报单额度返还对象
        /// </summary>
        [DataMember]
        public List<XmReportReturnModel> XmReportReturns
        {
            get;
            set;
        }

        /// <summary>
        /// 附件
        /// </summary>
        [DataMember]
        public virtual List<QtAttachmentModel> Attachments
        {
            get;
            set;
        }
    }
}
