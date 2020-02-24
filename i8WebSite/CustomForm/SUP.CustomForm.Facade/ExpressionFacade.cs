using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NG3;
using SUP.CustomForm.DataAccess;
using SUP.CustomForm.Facade.Interface;

namespace SUP.CustomForm.Facade
{
    public class ExpressionFacade:IExpressionFacade
    {
        private ExpressionDac dac = new ExpressionDac();

        [DBControl]
        public string GetSqlValue(string sql)
        {
            return dac.GetSqlValue(sql);
        }

        [DBControl]
        public DataTable GetDataTable(string sql)
        {
            return dac.GetDataTable(sql);
        }

        [DBControl]
        public bool ValidationCheck(string leftSqlStr, string rightSqlStr, string opera)
        {
            return dac.ValidationCheck(leftSqlStr, rightSqlStr, opera);
        }
    }
}
