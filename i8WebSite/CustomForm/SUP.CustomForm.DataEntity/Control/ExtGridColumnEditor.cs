using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.Control
{
    public class ExtGridColumnEditor
    {
        private string _xtype = string.Empty;
        private List<string> _items = new List<string>();

        public string XType
        {
            get { return _xtype; }
            set { _xtype = value; }
        }

        private bool _allowBlank = true;

        public bool AllowBlank
        {
            get { return _allowBlank; }
            set { _allowBlank = value; }
        }

        public List<string> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        //通用帮助
        public string ValueField { get; set; }
        public string DisplayField { get; set; }
        public string ListFields { get; set; }
        public string ListHeadTexts { get; set; }
        public string Helpid { get; set; }
        public string CmpName { get; set; }
        public string OutFilter { get; set; }
        public bool MultiSelect { get; set; }

        //ngComboBox
        public string QueryMode { get; set; }
        public List<string> Data { get; set; }
    }
}
