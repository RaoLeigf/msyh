using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GXM3.XM.Model.Request
{
    // <summary>
    /// 保存立项数据
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class ProjectMstExcelModel
    {
       public List<PraojectMstBase> model { get; set; }
    }

    public class PraojectMstBase
    {
        /// <summary>
        /// 项目编码
        /// </summary>
        [DataMember]
        public virtual string ProjectCode { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [DataMember]
        public virtual string ProjectName { get; set; }

        /// <summary>
        /// 业务条线
        /// </summary>
        [DataMember]
        public virtual string FBusinessName { get; set; }

    }

}
