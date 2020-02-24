using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.AppContainer
{
    public class AppToolbar
    {
        private List<string> _LButtons = new List<string>();
        private List<string> _RButtons = new List<string>();
               

        public AppToolbar()
        {
 
        }

        public List<string> LButtons
        {
            get { return _LButtons; }
            set { _LButtons = value; }
        }

        public List<string> RButtons
        {
            get { return _RButtons; }
            set { _RButtons = value; }
        }

    }
}
