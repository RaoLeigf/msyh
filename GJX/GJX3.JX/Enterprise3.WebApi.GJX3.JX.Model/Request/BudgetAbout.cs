using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Enterprise3.WebApi.GJX3.JX.Model.Request
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class BudgetAbout: BaseListModel
    {
        /// <summary>
        /// 预算部门编码
        /// </summary>
        [DataMember]
        public string FBudgetDept { get; set; }
    }
}