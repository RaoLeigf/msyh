using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.AppControl
{
    public class HelpBase : BaseField
    {
        private string _helpid;
        
        public HelpBase()
        {
 
        }

        public string HelpID
        {
            get { return _helpid; }
            set { _helpid = value; }
        }
    }
}
