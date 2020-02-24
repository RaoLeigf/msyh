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
    public class MenuInfoModel: BaseListModel
    {
        /// <summary>
        /// 是否控制权限的开关
        /// </summary>
        [DataMember]
        public string flag { get; set; }
        /// <summary>
        /// 指定SQL语句构建系统功能树
        /// </summary>
        [DataMember]
        public string treeFilter { get; set; }
        /// <summary>
        /// 模块标识:GXM
        /// </summary>
        [DataMember]
        public string suite { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string node { get; set; }
        /// <summary>
        /// 系统功能树是否懒加载的开关
        /// </summary>
        [DataMember]
        public string lazyLoadFlag { get; set; }
    }
}
