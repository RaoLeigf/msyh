using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NG3;
using System.Diagnostics;
using System.Net;

using NG3.Data.Builder;


namespace NG3.SUP.Frame
{
    public partial class TestWS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Unnamed2_Click(object sender, EventArgs e)
        {
            string ss = string.Empty;
            Stopwatch s = new Stopwatch();
            s.Start();
            var str = NG3.Common.ESB.InvokeWebService("~/Test/WebService1.asmx", "sid").Value.ToString() + "_" + NG3.Common.ESB.InvokeWebService("~/Test/WebService1.asmx", "sid").Value.ToString();
            s.Stop();
            Label1.Text = "ESB: " + HttpContext.Current.Session.SessionID + " " + str + " " + s.ElapsedMilliseconds.ToString();
            //s.Reset();
            //s.Start();
            //var str2 = abcd.sid() + "_" + abcd.sid();
            //s.Stop();
            //Label1.Text += "<br>原生: " + HttpContext.Current.Session.SessionID + " " + str2 + " " + s.ElapsedMilliseconds.ToString(); 
        }

        protected void Unnamed3_Click(object sender, EventArgs e)
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            var str = NG3.Common.ESB.InvokeWebService("http://ng2/Test/WebService1.asmx", "Add", 1, 3).ToString();
            s.Stop();
            Label1.Text = str + " " + s.ElapsedMilliseconds.ToString();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var dt = DB.SQL("select * from secuser").GetDataTable(10);
            var meta = dt.ToMeta("logid");

        }
    }
}