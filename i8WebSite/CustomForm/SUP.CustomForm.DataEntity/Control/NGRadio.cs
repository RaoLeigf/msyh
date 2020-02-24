using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.Control
{
    public class NGRadio : ExtControlBase
    {
        private string _inputValue;
        private List<string> _items = new List<string>();

        public string InputValue
        {
            get { return _inputValue; }
            set { _inputValue = value; }
        }

        public List<string> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public NGRadio()
        {
        }
    }
}
