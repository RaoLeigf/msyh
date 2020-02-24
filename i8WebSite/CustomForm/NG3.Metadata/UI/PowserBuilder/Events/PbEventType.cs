using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NG3.Metadata.UI.PowserBuilder.Events
{
    /// <summary>
    /// PB事件类型
    /// </summary>
    public enum PbEventType
    {
        Other,
        Click,
        DoubleClick,
        Update,
        EditAddInit,
        BillDelCheck,
        BillDelUpdate,
        BillSaveUpdate,
        BillApprovalUpdate,
        BillUnApprovalUpdate,
    }
}
