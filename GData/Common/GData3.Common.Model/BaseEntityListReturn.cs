using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GData3.Common.Model
{
    public class BaseEntityListReturn<T>
    {
        public int totalRows { set; get; }
        public List<T> Record { set; get; }
        public int index { set; get; }
        public int size { set; get; }
    }
}
