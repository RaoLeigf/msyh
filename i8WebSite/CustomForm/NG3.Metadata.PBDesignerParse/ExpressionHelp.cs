using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Events;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;

namespace NG3.Metadata.PBDesignerParse
{
    /// <summary>
    /// PB表达式解析帮助类
    /// </summary>
    internal sealed class ExpressionHelp
    {

        private static PbExpressionImp ParseExpression(string expressionStr)
        {
            PbExpressionImp pbExpressionImp = new PbExpressionImp();
            string str = expressionStr.Trim();
            int index = str.IndexOf("=", StringComparison.OrdinalIgnoreCase);
            str = str.Remove(0, index+1);
            index = str.IndexOf(";", StringComparison.OrdinalIgnoreCase);
            string rule = str.Substring(0, index);
            pbExpressionImp.ExpressionType = (PbExpressionType) Convert.ToInt32(rule);
            str = str.Remove(0, index + 1);
            str = str.Replace("@", "=");
            str = str.Replace("\r\n", "");
            pbExpressionImp.Expression = str;
            return pbExpressionImp;
        }

        /// <summary>
        /// 解析内置表达式
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static PbBuildInImp ParseBuildInImp(string inputStr)
        {
            string[] strArray = inputStr.Split(';');
            Debug.Assert(strArray.Length == 2);
            PbBuildInImp pbBuildInImp = new PbBuildInImp();
            pbBuildInImp.EventImpType = (PbEventImpType) Convert.ToInt32(strArray[0]);
            pbBuildInImp.Param = strArray[1];
            return pbBuildInImp;
        }

        /// <summary>
        /// 解析表达式
        /// </summary>
        /// <param name="expressionList"></param>
        /// <returns></returns>
        public static PbEvent<PbExpressionImp> ParseExpressionList(List<string> expressionList)
        {
            try
            {
                PbEvent<PbExpressionImp> pbEvent = new PbEvent<PbExpressionImp>();
                expressionList.Sort((str1,str2) =>
                {
                    string tempStr1 = str1.Split('=')[0].Trim().Remove(0, 4);
                    int value1 = Convert.ToInt32(tempStr1);
                    string tempStr2 = str2.Split('=')[0].Trim().Remove(0, 4);
                    int value2 = Convert.ToInt32(tempStr2);

                    return value1 - value2;

                });

                foreach (string str in expressionList)
                {
                    pbEvent.PbImp.Add(ParseExpression(str));
                }

                return pbEvent;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
