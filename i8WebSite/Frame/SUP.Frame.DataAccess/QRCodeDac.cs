using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NG3.Data.Service;
using NG3;
using SUP.Frame.DataAccess;

namespace SUP.Frame.DataAccess
{
    public class QRCodeDac
    {       
        public DataTable getUrlByCode(string code)
        {
            string sql = String.Format(@"SELECT controllerurl,viewurl FROM qrcode_rule 
                            WHERE code = {0}",
                            code);
            DataTable dt = DbHelper.GetDataTable(sql);
            return dt;
        }
    }
}
