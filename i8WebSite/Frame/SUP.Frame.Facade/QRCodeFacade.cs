using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.Base;
using SUP.Frame.Rule;
using SUP.Frame.DataAccess;
using NG3;
using SUP.Frame.DataEntity;
using System.Data;

namespace SUP.Frame.Facade
{
    public class QRCodeFacade : IQRCodeFacade
    {
        private QRCodeDac dac = new QRCodeDac();
        [DBControl]
        public DataTable getUrlByCode(string code)
        {
            return dac.getUrlByCode(code);
        }
    }
}
