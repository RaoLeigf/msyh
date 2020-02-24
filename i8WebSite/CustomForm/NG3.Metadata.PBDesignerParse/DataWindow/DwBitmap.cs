using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NG3.Metadata.PBDesignerParse.DataWindow
{
    class DwBitmap
    {
        private string _name = string.Empty;
        private bool _visible = false;
        private string _tag = string.Empty;
        private string _filename = string.Empty;
        private int _xPos = 0;
        private int _yPos = 0;
        private int _height = 0;
        private int _width = 0;

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

        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public string FileName
        {
            get { return _filename; }
            set { _filename = value; }
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

    }
}
