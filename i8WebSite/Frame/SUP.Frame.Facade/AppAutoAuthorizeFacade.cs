using NG3;
using SUP.Frame.DataAccess;
using SUP.Frame.Facade.Interface;
using SUP.Frame.Rule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Frame.Facade
{
    public class AppAutoAuthorizeFacade: IAppAutoAuthorizeFacade
    {
        private AppAutoAuthorizeDac dac = new AppAutoAuthorizeDac();
        private AppAutoAuthorizeRule rule = new AppAutoAuthorizeRule();

        [DBControl]
        public DataTable GetAppAutoAuthorizeList(string rolename)
        {
            return dac.GetAppAutoAuthorizeList(rolename);
        }

        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public int Save(DataTable dt)
        {
            return dac.Save(dt);
        }
    }
}
