using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.Common.DataEntity
{
    [Serializable]
    public class HelpValueNameEntity
    {

        public virtual string HelpID { get; set; }

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string OutJsonQuery { get; set; }

        public virtual string SelectMode { get; set; }//单选或者多选        

        public virtual string HelpType { get; set; }//帮助类型ngCommonHelp、ngRichHelp

        public virtual bool IsORMapping { get; set; }//是否是orm模式
    }
}
