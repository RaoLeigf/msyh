using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.AppControl
{
    public class Button : BaseField
    {
        private string text;

        public Button()
        { 
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

    }
}
