
using GGK3.GK.Model.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GData3.Common.Model
{
    /// <summary>
    ///请求单个对象基类
    /// </summary>
    public class BaseSingleModel : BaseModel
    {
        /// <summary>
        ///对象主键
        /// </summary>
        [Description("对象主键")]
        public string id { set; get; }

        public string value { get; set; }

        /// <summary>
        //当前年份
        /// </summary>
        [Description("主键集合")]
        public List<string> fPhIdList { get; set; }

        /// <summary>
        /// 主键集合
        /// </summary>
        public long[] ids { get; set; }
        /// <summary>
        /// 模板类型
        /// </summary>
        public int type { get; set; }
        public List<GK3_BankVauitModel> gklist { get; set; }
    }
}
