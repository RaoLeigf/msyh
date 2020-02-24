using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.Control
{
    public class ExtGridColumn
    {
        private string _header = string.Empty;
         
        private int _flex = 1;
        private bool _sortable = true;
        private bool _mustInput = false; //必输列
        private string _datatype = string.Empty;
        private string _dataIndex = string.Empty;
        private int _width = 80;
        //private string _summarytype = string.Empty;
        private bool _protect = false;
        private string _defaultvalue = string.Empty;
        private string _format = string.Empty;
        private string _editMask = string.Empty;
        private ExtGridColumnEditor _editor = new ExtGridColumnEditor();
        private string _rgbcolor = string.Empty;//列的字体颜色
        private string _backgroundColor = string.Empty;//列的背景颜色

        public ExtGridColumn()
        {
 
        }

        public ExtGridColumn(string XType, string fieldname,string header)
        {
            this._dataIndex = fieldname;
            this._header = header;
            this._editor.XType = XType;
        }

        public string DefaultValue
        {
            get { return _defaultvalue; }
            set { _defaultvalue = value; }
        }

        public string Format
        {
            get { return _format; }
            set { _format = value; }
        }

        public string EditMask
        {
            get { return _editMask; }
            set { _editMask = value; }
        }

        public bool Protect
        {
            get { return _protect; }
            set { _protect = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }

        //public string SummaryType
        //{
        //    get { return _summarytype; }
        //    set { _summarytype = value; }
        //}

        public int Flex
        {
            get { return _flex; }
            set { _flex = value; }
        }
       
        public bool Sortable
        {
            get { return _sortable; }
            set { _sortable = value; }
        }
       
        public bool MustInput
        {
            get { return _mustInput; }
            set { _mustInput = value; }
        }
        
        public string Datatype
        {
            get { return _datatype; }
            set { _datatype = value; }
        }

        public string DataIndex
        {
            get { return _dataIndex; }
            set { _dataIndex = value; }
        }

        public ExtGridColumnEditor editor
        {
            get { return _editor; }
            set { _editor = value; }
        }

        public string RgbColor
        {
            get { return _rgbcolor; }
            set { _rgbcolor = value; }
        }

        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }
    }
}
