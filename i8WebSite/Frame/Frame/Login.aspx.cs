using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NG3;
using Newtonsoft.Json.Linq;

namespace NG3.SUP.Frame
{
    public partial class Login : NG3.Common.Web.UI.NG2Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Abandon();
        }        
    }
}