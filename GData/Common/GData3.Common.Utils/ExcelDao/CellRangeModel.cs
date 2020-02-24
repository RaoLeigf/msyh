using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GData3.Common.Utils
{
    public class CellRangeModel
    {
        public int FirstRow { set; get; }
        public int LastRow { set; get; }
        public int FirstCol { set; get; }
        public int LastCol { set; get; }
    }
}
