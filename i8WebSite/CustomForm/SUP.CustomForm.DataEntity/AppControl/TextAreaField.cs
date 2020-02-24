using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.AppControl
{
    public class TextAreaField:TextField
    {

        private int maxRow;

        public int MaxRow
        {
            get { return maxRow; }
            set { maxRow = value; }
        }


        public TextAreaField()
        {
 
        }


    }
}
