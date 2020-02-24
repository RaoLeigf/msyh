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
    public class ExtendFuncBizModel
    {
        /// <summary>
        /// phid
        /// </summary>
        [DataMember]

        public ExtendFunctionMstModel MstModel { set; get; }

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
       
    }
}
