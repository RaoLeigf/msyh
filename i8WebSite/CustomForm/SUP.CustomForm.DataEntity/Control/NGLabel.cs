using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.Control
{
   public class NGLabel : ExtControlBase
    {
        private string text;

        public NGLabel()
        {

        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
    }
}
