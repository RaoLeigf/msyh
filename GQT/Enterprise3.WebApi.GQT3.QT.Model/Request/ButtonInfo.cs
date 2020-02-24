using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GQT3.QT.Model.Request
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class ButtonInfo : BaseListModel
    {
        /// <summary>
        /// 菜单标识
        /// </summary>
        [DataMember]
        public string rightname { get; set; }
    }
}
