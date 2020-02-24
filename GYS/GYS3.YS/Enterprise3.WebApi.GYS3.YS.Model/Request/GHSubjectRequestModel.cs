using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GYS3.YS.Model.Request
{
    public class GHSubjectRequestModel : BaseListModel
    {
        /// <summary>
		/// 主键
		/// </summary>
		[DataMember]
        public virtual System.Int64 PhId
        {
            get;
            set;
        }
        /// <summary>
        /// 用户userId
        /// </summary>
        [DataMember]
        public string UserId { get; set; }
        /// <summary>
        /// 0-全部 1-待上报 2-审批中 3-已审批 4-纳入预算 5-作废
        /// </summary>
        [DataMember]
        public string FApproveStatus { get; set; }
        /// <summary>
        /// FKmlb=1 基本支出申报 FKmlb=0 收入预算申报
        /// </summary>
        [DataMember] 
        public string Fkmlb { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string tabtype { get; set; }
    }
}
