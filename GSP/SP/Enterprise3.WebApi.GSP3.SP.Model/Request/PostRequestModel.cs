using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Enterprise3.WebApi.GSP3.SP.Model.Request
{
    /// <summary>
    /// 岗位传参
    /// </summary>
    [Serializable]
    public class PostRequestModel : BaseListModel
    {
        /// <summary>
        /// 审批类型id
        /// </summary>
        [DataMember]
        public List<long> PostPhidList { get; set; }

        /// <summary>
        /// 岗位搜索字段
        /// </summary>
        [DataMember]
        public string PostName { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [DataMember]
        public string EnableMark { get; set; }

        /// <summary>
        /// 搜索组织id
        /// </summary>
        [DataMember]
        public List<long> SearchOrgid { get; set; }

    }
}