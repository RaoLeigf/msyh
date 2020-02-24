using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.AppControl
{
    public class RadioField:BaseField
    {

        private bool _checked;//勾选
        private List<string> _items = new List<string>();

        public RadioField()
        { 
        }

        public List<string> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public bool Checked
        {
            get { return _checked; }
            set { _checked = value; }
        }

    }
}
