using GYS3.YS.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GYS3.YS.Model.Request
{
    /// <summary>
    /// 
    /// </summary>
    public class BudgetProcessCtrlRequestModel : BaseListModel
    {
        /// <summary>
        /// 用户userId
        /// </summary>
        [DataMember]
        public string UserId { get; set; }
        /// <summary>
        /// 组织编码
        /// </summary>
        [DataMember]
        public string FOcode { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        [DataMember]
        public string FYear { get; set; }
    }

    public class BudgetProcessSaveRequestModel
    {
        /// <summary>
        /// 组织编码
        /// </summary>
        [DataMember]
        public string FOcode { get; set; }
        /// <summary>
        /// 申报种类
        /// </summary>
        public string FProcessStatus { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        [DataMember]
        public DateTime StartDt { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        [DataMember]
        public DateTime EndDt { get; set; }

    }
    public class BudgetProcessSaveRequestListModel
    {
        public List<BudgetProcessCtrlModel> infodata { get; set; }
    }
}
