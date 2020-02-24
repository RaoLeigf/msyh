using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.Core;
using NG3.Metadata.UI.PowserBuilder.Controls;

namespace NG3.Metadata.PBDesignerParse.DataWindow
{
    class DwButton:MetadataGod
    {
        private string _name = string.Empty;
        private bool _visible = false;
        private long _color = 0;
        private int _xPos = 0;
        private int _yPos = 0;
        private int _height = 0;
        private int _width = 0;
        private string _text = string.Empty;



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

        public long Color
        {
            get { return _color; }
            set { _color = value; }
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

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
    }
}
