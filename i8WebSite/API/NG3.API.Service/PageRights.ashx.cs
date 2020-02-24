using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using i6.UIProcess.MainFrame;

namespace NG3.API.Service
{
    /// <summary>
    /// Summary description for Rights
    /// </summary>
    public class PageRights :System.Web.UI.Page, IHttpHandler
    {
        //http://10.0.17.12/i6p/PageRights.ashx?ucode=0001&ocode=001&loginid=9999&pagename=MeetingListPage&funcname=
        public void ProcessRequest(HttpContext context)
        {
            object obj = null;
            obj = context.Request.QueryString["ucode"];
            string ucode = obj == null ? "" : obj.ToString();
            obj = context.Request.QueryString["ocode"];
            string ocode = obj == null ? "" : obj.ToString();
            obj = context.Request.QueryString["loginid"]; 
            string loginid = obj == null ? "" : obj.ToString();
            obj = context.Request.QueryString["pagename"];
            string pagename = obj == null ? "" : obj.ToString();
            obj = context.Request.QueryString["funcname"];
            string funcname = obj == null ? "" : obj.ToString();

            string UserConnectString = string.Empty;
          
            string ret = string.Empty;

            AccountSet accountSet = new AccountSet();
            i6.Biz.DMC.CPCM_Interface cpcm_Interface = new i6.Biz.DMC.CPCM_Interface(accountSet.GetUserConnectString(ucode));

            bool rst = cpcm_Interface.IsHaveRight(ocode, loginid, pagename, funcname);
           
            context.Response.ContentType = "text/plain";
            context.Response.Write(rst.ToString());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}