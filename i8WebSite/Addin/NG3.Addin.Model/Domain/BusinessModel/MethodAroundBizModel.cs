using NG3.Addin.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Model.Domain.BusinessModel
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(Namespace = "")]
    public class MethodAroundBizModel
    {
        /// <summary>
        /// phid
        /// </summary>
        [DataMember]

        public MethodAroundMstModel MstModel { set; get; }
     
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public IList<AddinSqlModel> SqlModels { set; get; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public IList<AddinAssemblyModel> AssemblyModels { set; get; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public IList<AddinExpressionModel> ExpModels { set; get; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public IList<AddinExpressionVarModel> ExpVarModels { set; get; }


  

 
    }
}
