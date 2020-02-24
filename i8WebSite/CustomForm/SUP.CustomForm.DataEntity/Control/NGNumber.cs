using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.Control
{
    public class NGNumber : ExtControlBase
    {
        private int _decimalPrecision = 2; //小数点位数
        private string _decimalSeparator = "."; //小数点符号
        private string _editMask = "###,###.000000";  //数值掩码

        public NGNumber()
        {
        }


        public int DecimalPrecision
        {
            get { return _decimalPrecision; }
            set { _decimalPrecision = value; }
        }


        public string DecimalSeparator
        {
            get { return _decimalSeparator; }
            set { _decimalSeparator = value; }
        }

        public string EditMask
        {
            get { return _editMask; }
            set { _editMask = value; }
        }
    }
}
