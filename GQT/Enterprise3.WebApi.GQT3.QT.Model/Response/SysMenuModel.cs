using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GQT3.QT.Model.Response
{
    /// <summary>
    /// fg3_menu 表的内容
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class SysMenuModel
    {
        /// <summary>
        ///  主键
        /// </summary>
        [DataMember]
        public string code { get; set; }

        /// <summary>
        /// 菜单编码
        /// </summary>
        [DataMember]
        public string id { get; set; }
        /// <summary>
        /// 父级编码
        /// </summary>
        [DataMember]
        public string pid { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        [DataMember]
        public string name { get; set; }
        /// <summary>
        /// 路由
        /// </summary>
        [DataMember]
        public string url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string managername { get; set; }
        /// <summary>
        /// 页面按钮权限标识
        /// </summary>
        [DataMember]
        public string rightname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string functionname { get; set; }
        /// <summary>
        /// 模块标识
        /// </summary>
        [DataMember]
        public string suite { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string apptype { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Byte functionnode_flag { get; set; }
        /// <summary>
        /// 模块代码
        /// </summary>
        [DataMember]
        public string moduleno { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public long rightkey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Byte ebankflg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Byte norightcontrol { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [DataMember]
        public int seq { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Byte adminvisiable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string expanded { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public long busphid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string langkey { get; set; }
    }
}
