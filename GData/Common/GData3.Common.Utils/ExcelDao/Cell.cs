using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GData3.Common.Utils
{
    public class Cell
    {
        public ICellStyle CellStyle { set; get;}
        
        public int RowIndex { set; get; }

        public int ColumnIndex { set; get; }

        public object Value { set; get; }
    }
}
