using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Report.Func.Core.Dac
{
    public static class ObjectExtension
    {
        public static bool IsDBNullOrEmpty(this object value)
        {
            return (value == null) ||
                   (value == DBNull.Value) ||
                   (string.IsNullOrEmpty(value.ToString()));
        }

        public static int TryGetInt(this object value)
        {
            if (value == null || value == DBNull.Value)
                return 0;
            else
                return Convert.ToInt32(value);
        }
        public static string TryGetString(this object value)
        {
            if (value == null || value == DBNull.Value)
                return string.Empty;
            else
                return value.ToString();
        }
    }
}
