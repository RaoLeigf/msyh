using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.Control
{
   public class NGButton : ExtControlBase
    {
        private string text;

        public NGButton()
        {

        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
    }
}
