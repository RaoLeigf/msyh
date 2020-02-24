using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.AppControl
{
    public class CheckBoxField : BaseField
    {

        private bool _checked;//勾选
               


        public CheckBoxField()
        { 

        }


        public bool Checked
        {
            get { return _checked; }
            set { _checked = value; }
        }

    }
}
