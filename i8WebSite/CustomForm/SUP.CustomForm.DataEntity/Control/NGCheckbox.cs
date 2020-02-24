using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.Control
{
    public class NGCheckbox : ExtControlBase
    {
        private string _inputValue;

        public NGCheckbox()
        { }

        public string InputValue
        {
            get { return _inputValue; }
            set { _inputValue = value; }
        }
    }
}
