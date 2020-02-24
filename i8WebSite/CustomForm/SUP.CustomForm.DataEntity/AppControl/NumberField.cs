using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.AppControl
{
    public class NumberField : TextField
    {

        private int minValue;      
        private int maxValue;
                        
        public NumberField()
        {
 
        }

        public int MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }

        public int MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }

    }
}
