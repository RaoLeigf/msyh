using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GData3.Common.Model
{
    /// <summary>
    /// 请求列表的参数封装，继承自ListBaseModel
    /// </summary>
    public class Parameter : ListBaseModel
    {
        /// <summary>
        /// 筛选值
        /// </summary>
        [Description("筛选值")]
        public string value { get; set; }

        /// <summary>
        /// 表达式集合,格式：参数名*运算符*参数值：例DM*eq*0001
        /// 例：["OrgName*eq*123123123", "EnterpriseCode*eq*123123123","OrgName*eq*政云数据"]
        /// </summary>
        [Description("表达式集合,格式：参数名*运算符*参数值, 例：[\"OrgName * eq * 123123123\", \"EnterpriseCode * eq * 123123123\",\"OrgName * eq * 政云数据\"]")]
        public IList<string> parames { get; set; }

        /// <summary>
        ///逻辑符集合，格式：表达式+%AND%/%OR%+表达式，一个逻辑式中至多出现一个%AND%/%OR%,
        ///例：["OrgName*eq*123123123%AND%EnterpriseCode*eq*123123123","OrgName*eq*123123123EnterpriseCode*eq*123123123%OR%OrgName*eq*政云数据"]
        /// </summary>
        [Description("逻辑符集合，格式：表达式+%AND%/%OR%+表达式，一个逻辑式中至多出现一个%AND%/%OR%, 例：[\"OrgName * eq * 123123123 % AND % EnterpriseCode * eq * 123123123\",\"OrgName * eq * 123123123EnterpriseCode * eq * 123123123 % OR % OrgName * eq * 政云数据\"]")]
        public IList<string> logics { get; set; }



    }
}
