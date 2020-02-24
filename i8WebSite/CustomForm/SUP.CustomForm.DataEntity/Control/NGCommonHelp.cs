using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.Control
{
    public class NGCommonHelp : NGHelpBase
    {
        private string _queryMode;
        private List<string> _data = new List<string>();

        public NGCommonHelp()
        {
        }

        public List<string> Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public string QueryMode
        {
            get { return _queryMode; }
            set { _queryMode = value; }
        }
    }
}
