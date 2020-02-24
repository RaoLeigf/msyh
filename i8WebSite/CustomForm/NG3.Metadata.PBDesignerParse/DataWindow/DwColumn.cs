using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.Core;

namespace NG3.Metadata.PBDesignerParse.DataWindow
{
    class DwColumn : MetadataGod
    {
        private string _name = string.Empty;
        private bool _visible = false;
        private bool _mustinput = false;
        private bool _protect = false;
        private string _protectexp = string.Empty;
        private long _color = 0;
        private long _labeltextcolor = 0;
        private int _xPos = 0;
        private int _yPos = 0;
        private int _height = 0;
        private int _width = 0;
        private int _maxLength = 0;
        private int _tabSequence = 0;       
        private bool _singletext = false;
        private bool _textarea = false;
        private int _colSpan = 1;
        private bool _multiSelect = false;        
        private string _defaultvalue = string.Empty;
        private string _tag = string.Empty;
        private string _format = string.Empty;
        private string _editmask = string.Empty;
        private DwControlType _controlType = DwControlType.Other;
        private long _backgroundColor = 0;//列的背景颜色

        public string DefaultValue
        {
            get { return _defaultvalue; }
            set { _defaultvalue = value; }
        }

        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public string EditMask
        {
            get { return _editmask; }
            set { _editmask = value; }
        }

        public bool SingleText
        {
            get { return _singletext; }
            set { _singletext = value; }
        }

        public bool TextArea
        {
            get { return _textarea; }
            set { _textarea = value; }
        }

        public int ColSpan
        {
            get { return _colSpan; }
            set { _colSpan = value; }
        }

        public bool MultiSelect
        {
            get { return _multiSelect; }
            set { _multiSelect = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public bool Mustinput
        {
            get { return _mustinput; }
            set { _mustinput = value; }
        }

        public bool Protect
        {
            get { return _protect; }
            set { _protect = value; }
        }

        public string ProtectExp
        {
            get { return _protectexp; }
            set { _protectexp = value; }
        }

        public long Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public long LabelTextColor
        {
            get { return _labeltextcolor; }
            set { _labeltextcolor = value; }
        }

        public int XPos
        {
            get { return _xPos; }
            set { _xPos = value; }
        }

        public int YPos
        {
            get { return _yPos; }
            set { _yPos = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int MaxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; }
        }

        public int TabSequence
        {
            get { return _tabSequence; }
            set { _tabSequence = value; }
        }

        public string Format
        {
            get { return _format; }
            set { _format = value; }
        }

        public DwControlType ControlType
        {
            get { return _controlType; }
            set { _controlType = value; }
        }

        public long BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }
    }
}
