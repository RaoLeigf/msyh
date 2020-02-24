using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NG3.Metadata.PBDesignerParse.DataWindow
{
    sealed class DwInfo
    {
        private List<DwDbColumn> _dwDbColumns = new List<DwDbColumn>();
        private List<DwText> _dwTexts = new List<DwText>();
        private List<DwButton> _dwButtons = new List<DwButton>();
        private List<DwColumn> _dwColumns = new List<DwColumn>();
        private List<DwGroupBox> _dwGroupBoxs = new List<DwGroupBox>();
        private List<DwBitmap> _dwBitmaps = new List<DwBitmap>();

        private string _sql = string.Empty;

        private bool _isSort = false;
        private void Sort()
        {
            if (_isSort)
                return;

            _dwTexts.Sort((text, dwText) => text.XPos - dwText.XPos);
            _dwGroupBoxs.Sort((box, groupBox) => box.XPos - groupBox.XPos);
            _dwColumns.Sort((column, dwColumn) => column.XPos - dwColumn.XPos);
            _dwBitmaps.Sort((bitmap, dwBitmap) => bitmap.XPos - dwBitmap.XPos);
            _isSort = true;
        }

        public DwDbColumn GetDwDbColumnByName(string name)
        {
            return _dwDbColumns.Find(column => column.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public DwColumn GetDwColumnByName(string name)
        {
            return _dwColumns.Find(column => column.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public DwText GetDwTextByName(string name)
        {
            return _dwTexts.Find(column => (column.Name.Remove(column.Name.Length-2,2)).Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        //xkq：直接通过标签的名字找到DwText
        public DwText GetDwTextByName1(string name)
        {
            return _dwTexts.Find(column => column.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }


        public DwButton GetDwButtonByName(string name)
        {
            return _dwButtons.Find(column => column.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public DwGroupBox GetDwGroupBoxByName(string name)
        {
            return _dwGroupBoxs.Find(column => column.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public DwBitmap GetDwBitmapByName(string name)
        {
            return _dwBitmaps.Find(bitmap => bitmap.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public IList<DwDbColumn> DwDbColumns
        {
            get
            {
                Sort();
                return _dwDbColumns; 
            }
        }

        public IList<DwText> DwTexts
        {
            get { Sort(); return _dwTexts; }
        }

        public IList<DwButton> DwButtons
        {
            get { Sort(); return _dwButtons; }
        }

        public IList<DwColumn> DwColumns
        {
            get { Sort(); return _dwColumns; }
        }

        public IList<DwGroupBox> DwGroupBoxs
        {
            get { Sort(); return _dwGroupBoxs; }
        }

        public IList<DwBitmap> DwBitmaps
        {
            get { Sort(); return _dwBitmaps; }
        }

        /// <summary>
        /// 数据窗口对应的SQL语句
        /// </summary>
        public string Sql
        {
            get { return _sql; }
            set { _sql = value; }
        }
    }
}
