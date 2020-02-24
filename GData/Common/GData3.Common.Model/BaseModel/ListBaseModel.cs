using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GData3.Common.Model
{
    /// <summary>
    ///请求列表基础参数
    /// </summary>
    public class ListBaseModel : BaseModel
    {
        /// <summary>
        ///queryfilter的格式：{"属性1*str*判断规则{B}*是否{C}":"值","Mc*str*eq*1":"x"}，直接调用I8的接口
        /// </summary>
        [Description("查询串，格式如下：{\"属性1 * str * 判断规则{B}* 是否 { C }\":\"值\",\"Mc* str* eq*1\":\"x\"}，直接调用I8的接口")]
        public string queryfilter { get; set; }

        /// <summary>
        ///排序格式："属性名 升序/降序"
        /// </summary>
        [Description("排序格式：\"属性名 升序 / 降序\"")]
        public string[] sort { get; set; }

        /// <summary>
        ///分页大小
        /// </summary>
        [Description("分页大小")]
        public int pagesize { set; get; }

        /// <summary>
        ///分页序号
        /// </summary>
        [Description("分页序号")]
        public int pageindex { set; get; }
    }
}
