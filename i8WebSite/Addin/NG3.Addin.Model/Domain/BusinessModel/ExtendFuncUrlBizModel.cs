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
    public class ExtendFuncUrlBizModel
    {
        /// <summary>
		/// Phid
		/// </summary>
		[DataMember]
        public virtual System.Int64 Phid
        {
            get;
            set;
        }

        /// <summary>
		/// 重新定位的URL
		/// </summary>
		[DataMember]
        public virtual System.String Url
        {
            get;
            set;
        }
    }
}
