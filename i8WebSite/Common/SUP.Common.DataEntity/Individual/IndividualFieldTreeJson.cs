using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.Base;

namespace SUP.Common.DataEntity.Individual
{
    public class IndividualFieldTreeJson : TreeJSONBase
    {
        //控件配置信息
        public virtual ExtControlInfoBase control { get; set; }

        public virtual string from { get; set; }

        //容器类型
        public virtual string container_uitype { get; set; }

        //单据列表的列信息
        public virtual ExtGridColumnInfoBase listColumnInfo { get; set; }
    }


}
