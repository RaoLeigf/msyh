using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.Rule
{
    public class SqlTypeConverter
    {
        public static string ConvertToExtControl(System.Data.SqlDbType sqltype)
        {
            string datatype = string.Empty;
            switch (sqltype)
            {
                case SqlDbType.Date:
                case SqlDbType.DateTime:
                case SqlDbType.DateTime2:
                case SqlDbType.DateTimeOffset:
                    datatype = "datetime";
                    break;
                case SqlDbType.Float:
                case SqlDbType.Decimal:
                    datatype = "float";
                    break;
                case SqlDbType.Int:
                case SqlDbType.SmallInt:
                    datatype = "int";
                    break;
                default:
                    datatype = "string";
                    break;
            }

            return datatype;
        }
    }
}
