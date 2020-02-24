using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.Core;

namespace NG3.Metadata.PBDesignerParse.DataWindow
{
    class DwDbColumn : MetadataGod
    {
        private string _name = string.Empty;
        private string _dbName = string.Empty;
        private string _dataType = string.Empty;
        private string _values = string.Empty;
        //private string _summarytype = string.Empty;

        //public string SummaryType
        //{
        //    get { return _summarytype; }
        //    set { _summarytype = value; }
        //}

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string DbName
        {
            get { return _dbName; }
            set { _dbName = value; }
        }

        public string DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        public string Values
        {
            get { return _values; }
            set { _values = value; }
        }


    }
}
