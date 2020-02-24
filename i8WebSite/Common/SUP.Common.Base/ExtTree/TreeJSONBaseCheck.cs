using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.Common.Base
{
    //ext树的数据格式
    [Serializable]
    public class TreeJSONBaseCheck : TreeJSONBase
    {
        public virtual bool @checked { get; set; }
    } 
}
