using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.Facade.Interface
{
    public interface IExpressionFacade
    {
        string GetSqlValue(string sql);

        DataTable GetDataTable(string sql);

        bool ValidationCheck(string leftSqlStr, string rightSqlStr, string opera);
    }
}
