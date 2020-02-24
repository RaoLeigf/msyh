using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NG3.Data.Service;

namespace SUP.CustomForm.DataAccess
{
    public class ExpressionDac
    {
        /// <summary>
        /// 获得sql语句的结果;
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string GetSqlValue(string sql)
        {
            if (sql == string.Empty) return string.Empty;

            var value = DbHelper.GetString(sql);
            return value;
        }

        /// <summary>
        /// 取DataTable数据;
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            if (sql == string.Empty) return new DataTable();

            DataTable dt = DbHelper.GetDataTable(sql);
            return dt;
        }

        /// <summary>
        /// 汇总校验;
        /// </summary>
        /// <param name="leftSqlStr"></param>
        /// <param name="rightSqlStr"></param>
        /// <param name="opera"></param>
        /// <returns></returns>
        public bool ValidationCheck(string leftSqlStr,string rightSqlStr,string opera)
        {
            var leftValue = DbHelper.GetString(leftSqlStr);
            var rightValue = DbHelper.GetString(rightSqlStr);
            bool isValidate = false;
            try
            {
                /*double型的数据会导致精度问题;*/
                double dleftValue = string.IsNullOrEmpty(leftValue) ? 0 : Math.Round(Convert.ToDouble(leftValue), 2);
                double drightValue = string.IsNullOrEmpty(rightValue) ? 0 : Math.Round(Convert.ToDouble(rightValue), 2);

                switch (opera)
                {
                    case "=":
                        isValidate = dleftValue == drightValue ? true : false;
                        break;
                    case ">":
                    case ">=":
                        isValidate = dleftValue >= drightValue ? true : false;
                        break;
                    case "<":
                    case "<=":
                        isValidate = dleftValue <= drightValue ? true : false;
                        break;
                    case "!=":
                        isValidate = dleftValue != drightValue ? true : false;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return isValidate;
        }
    }
}
