using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.Control
{
    public class NGHelpBase : ExtControlBase
    {
        private bool _multiselect = false;
        private string _valueField;
        private string _displayField;
        private string _helpid;
       
        private string _listFields;
        private string _listHeadTexts;

        private string _outFilter;

        public NGHelpBase()
        {
 
        }

        public bool MultiSelect
        {
            get { return _multiselect; }
            set { _multiselect = value; }
        }

        public string ValueField
        {
            get { return _valueField; }
            set { _valueField = value; }
        }

        public string DisplayField
        {
            get { return _displayField; }
            set { _displayField = value; }
        }

        public string Helpid
        {
            get { return _helpid; }
            set { _helpid = value; }
        }

        public string ListFields
        {
            get { return _listFields; }
            set { _listFields = value; }
        }

        public string ListHeadTexts
        {
            get { return _listHeadTexts; }
            set { _listHeadTexts = value; }
        }

        public string OutFilter
        {
            get { return _outFilter; }
            set { _outFilter = value; }
        }
    }
}
