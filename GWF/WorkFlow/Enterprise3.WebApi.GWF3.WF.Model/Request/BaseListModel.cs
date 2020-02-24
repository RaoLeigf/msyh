using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GWF3.WF.Model.Request
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class BaseListModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [DataMember]
        public string userid { get; set; }

        /// <summary>
        /// 组织ID
        /// </summary>
        [DataMember]
        public string orgid { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        [DataMember]
        public int pagesize { set; get; }
        /// <summary>
        /// 当前页数
        /// </summary>
        [DataMember]
        public int pageindex { set; get; }

        /// <summary>
        ///搜索条件
        /// </summary>
        [DataMember]
        public string filter { get; set; }
        /// <summary>
        ///
        /// </summary>
        [DataMember]
        public string sqlstr { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DataMember]
        public string sortstr { get; set; }

    }
}
