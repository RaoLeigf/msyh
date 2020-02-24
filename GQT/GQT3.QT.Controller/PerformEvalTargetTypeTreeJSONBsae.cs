using SUP.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GQT3.QT.Controller
{
    class PerformEvalTargetTypeTreeJSONBsae : TreeJSONBase
    {
        /// <summary>
        /// 父级节点代码
        /// </summary>
        public virtual string parentCode { get; set; }

        /// <summary>
        /// 本级节点代码
        /// </summary>
        public virtual string curentCode { get; set; }

        /// <summary>
        /// 是否末级项目： "1" 是, null or "2" 否
        /// </summary>
        //public virtual string isProject { get; set; }
    }
}
