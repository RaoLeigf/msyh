using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.AppControl
{
    public class TextField : BaseField
    {

        private bool _readonly;//只读
        private int _maxLength;//最大长度
              
        
        public TextField() { 
        
        }

        public bool ReadOnly
        {
            get { return _readonly; }
            set { _readonly = value; }
        }

        public int MaxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; }
        }
    }
}
