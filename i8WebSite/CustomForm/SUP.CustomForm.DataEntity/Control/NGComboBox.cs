using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.Control
{
    public class NGComboBox : NGHelpBase
    {
        private string _queryMode;
        private List<string> _data = new List<string>();
        
        
        public NGComboBox()
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
