using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Frame.DataAccess;
using Enterprise3.NHORM.Rule;

namespace SUP.Frame.Rule
{
    public class UserRule
    {
        private UserDac dac = new UserDac();


        public bool AddUser(string logid, string username, string pwd)
        {
            BillNoCommon bill = new BillNoCommon();
            long id = bill.GetBillId("fg_orgpop", "id");
            return dac.AddUser(logid, username, pwd, id);
        }
    }
}
