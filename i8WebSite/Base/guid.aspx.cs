using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NG3.SUP.Base
{
    public partial class guid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Literal1.Text = Guid.NewGuid().ToString();
        }
    }
}