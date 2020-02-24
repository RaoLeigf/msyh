using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SUP.Frame.DataAccess;

namespace SUP.Frame.Rule
{
    public class QRCodeSetRule
    {
        private QRCodeSetDac dac = new QRCodeSetDac();
        public DataTable GetList(string query, int pageSize, int pageIndex, ref int totalRecord)
        {
            return dac.GetList(query, pageSize, pageIndex, ref totalRecord);
        }

        public DataTable GetMaster(string id)
        {
            return dac.GetMaster(id);
        }

        public DataTable GetProduct(string id)
        {
            return dac.GetProduct(id);
        }

        public DataTable GetDetailField(string id)
        {
            return dac.GetDetailField(id);
        }

        public string getPhidByCode(string code)
        {
            return dac.getPhidByCode(code);
        }
    }
}
