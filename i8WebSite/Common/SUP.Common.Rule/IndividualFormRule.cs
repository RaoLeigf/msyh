using SUP.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Common.Rule
{
    public class IndividualFormRule
    {
        private IndividualFormDac dac = new IndividualFormDac();

        public int Save(DataTable dt)
        {
            Int64 begin = CommonUtil.GetMaxId("fg_individualform");
            foreach (DataRow dr in dt.Rows)
            {
                if (dr.RowState == DataRowState.Deleted) continue;

                if (dr.RowState == DataRowState.Added)
                {
                    begin++;
                    dr["phid"] = begin; //CommonUtil.GetPhId("fg_individualform");  
                    //dr["phid"] = CommonUtil.GetPhId("fg_individualform");                      
                }
            }

            return dac.Save(dt);
        }
              
    }
}
