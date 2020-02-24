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
    /// /仿造I8返回格式，重新返回通用json格式 配置Model
    /// </summary>
    public class ProjectMstListModel
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string totalRows { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<ProjectMstModel> Record { get; set; }
    }
}
