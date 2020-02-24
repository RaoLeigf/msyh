using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.Frame.DataEntity
{
    public class RequestRecordEntity
    {
        public string Guid { get; set; }
        public string Moduleno { get; set; }
        public string Url { get; set; }
        public long UserId { get; set; }
        /// <summary>
        /// 狗号
        /// </summary>
        public string SN { get; set; }
        public int Frequency { get; set; }
    }
}
