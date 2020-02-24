using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.Frame.DataEntity
{
    public class SuiteInfoEntity
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        
    }

    public class IndivadualSuiteInfoEntity
    {
        public virtual string Suite { get; set; }
        public virtual string Name { get; set; }

    }
}
