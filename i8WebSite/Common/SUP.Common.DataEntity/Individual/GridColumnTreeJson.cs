using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.Base;

namespace SUP.Common.DataEntity.Individual
{
    public class GridColumnTreeJson : TreeJSONBase
    {
        //控件配置信息
        public virtual ExtGridColumnInfoBase control { get; set; }

        public virtual string from { get; set; }

        //容器类型
        public virtual string container_uitype { get; set; }
    }
}
