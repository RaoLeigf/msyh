using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GData.YQHL.Model
{
    /// <summary>
    /// 是否， 1 是, 0 否
    /// </summary>
    public enum PersistentState
    {
        UnChanged = 0,
        Added = 1,
        Modified = 2,
        Deleted = 3
    }
}
