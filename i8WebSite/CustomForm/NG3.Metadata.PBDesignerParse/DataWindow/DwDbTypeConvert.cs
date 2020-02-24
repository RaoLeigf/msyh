using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Controls;

namespace NG3.Metadata.PBDesignerParse.DataWindow
{
    class DwDbTypeConvert
    {
        public static SqlDbType ToSqlDbType(string typeStr)
        {
            try
            {
                SqlDbType sqlDbType = SqlDbType.Text;
                string str = typeStr.Trim();
                Debug.Assert(str.Length > 0);

                if (str.StartsWith("char", StringComparison.OrdinalIgnoreCase))
                {
                    sqlDbType = SqlDbType.NVarChar;
                }
                else if (str.StartsWith("datetime", StringComparison.OrdinalIgnoreCase))
                {
                    sqlDbType = SqlDbType.DateTime;
                }
                else if (str.StartsWith("long", StringComparison.OrdinalIgnoreCase))
                {
                    sqlDbType = SqlDbType.Int;
                }
                else if (str.StartsWith("number", StringComparison.OrdinalIgnoreCase) || str.StartsWith("decimal", StringComparison.OrdinalIgnoreCase))
                {
                    sqlDbType = SqlDbType.Decimal;
                }

                return sqlDbType;
            }
            catch (Exception)
            {
                
                throw;
            }

        }

        public static IList<PbPairValueInfo> ToPbPairValueInfo(string valuesStr)
        {
            try
            {
                IList<PbPairValueInfo> pbPairValueInfos = new List<PbPairValueInfo>();
                Debug.Assert(!string.IsNullOrEmpty(valuesStr));
                string str = valuesStr.Trim();
                string[] array = str.Split(new string[]{"/"},StringSplitOptions.RemoveEmptyEntries);
                Debug.Assert(array.Length == 2);

                foreach (string s in array)
                {
                    string temp = s;
                    if (!temp.Contains("\t"))
                    {
                        string[] childArray = temp.Split(' ');
                        Debug.Assert(childArray.Length == 2);
                        pbPairValueInfos.Add(new PbPairValueInfo(childArray[0], childArray[1]));
                    }
                    else
                    {
                        string[] childArray = temp.Split(new string[]{"\t"},StringSplitOptions.RemoveEmptyEntries);
                        if (childArray.Length == 1)
                        {
                            pbPairValueInfos.Add(new PbPairValueInfo(string.Empty, childArray[0]));
                        }
                        else if (childArray.Length == 2)
                        {
                            pbPairValueInfos.Add(new PbPairValueInfo(childArray[0], childArray[1]));
                        }

                    }
                }
                return pbPairValueInfos;

            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
