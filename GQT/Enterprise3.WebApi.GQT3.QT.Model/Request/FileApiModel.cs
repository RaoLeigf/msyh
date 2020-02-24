using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Enterprise3.WebApi.GQT3.QT.Model.Request
{
    /// <summary>
    /// 附件请求参数
    /// </summary>
    [Serializable]
    public class FileApiModel: BaseListModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Int64 PhId { get; set; }

        /// <summary>
        /// 附件的对应表名
        /// </summary>
        public string BTable { get; set; }

        /// <summary>
        /// 附件存储地址
        /// </summary>
        public string BUrlPath { get; set; }
    }
}