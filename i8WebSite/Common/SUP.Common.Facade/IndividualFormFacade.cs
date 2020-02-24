using NG3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUP.Common.DataAccess;
using SUP.Common.Rule;
using System.Data;

namespace SUP.Common.Facade
{
    public class IndividualFormFacade : IIndividualFormFacade
    {
        private IndividualFormDac dac = new IndividualFormDac();
        private IndividualFormRule rule = new IndividualFormRule();

        public IndividualFormFacade()
        {

        }

        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public int Save(DataTable dt)
        {
            return rule.Save(dt);
        }

        [DBControl]
        public DataTable GetIndividualFormList(string bustype)
        {
            return dac.GetIndividualFormList(bustype);
        }

    }
}
