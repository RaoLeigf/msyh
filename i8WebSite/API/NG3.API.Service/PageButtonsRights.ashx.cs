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
    public class PageButtonsRights :System.Web.UI.Page, IHttpHandler
    {
        //http://10.0.17.12/i6p/PageButtonsRights.ashx?ucode=0001&ocode=001&loginid=9999&pagename=MeetingListPage&funcname=
        public void ProcessRequest(HttpContext context)
        {
            //string _userCode = string.Empty;
            //if (String.IsNullOrEmpty(UCode))
            //    _userCode = DataBaseXML.DefaultAccount;
            //else
            //    _userCode = UCode;

            object obj = null;
            obj = context.Request.QueryString["ucode"];
            string ucode = obj==null?"":obj.ToString();
            obj = context.Request.QueryString["ocode"];
            string ocode = obj == null ? "" : obj.ToString();
            obj = context.Request.QueryString["loginid"];
            string loginid = obj == null ? "" : obj.ToString();
            obj = context.Request.QueryString["pagename"];
            string pagename = obj == null ? "" : obj.ToString();
            obj = context.Request.QueryString["funcname"];
            string funcname = obj == null ? "" : obj.ToString();

            string UserConnectString = string.Empty;
            Hashtable ht = new Hashtable();

            string ret = string.Empty;

            AccountSet accountSet = new AccountSet();
            i6.Biz.DMC.CPCM_Interface cpcm_Interface = new i6.Biz.DMC.CPCM_Interface(accountSet.GetUserConnectString(ucode));

            ht = cpcm_Interface.GetFormRights(ucode, ocode, loginid, string.Empty, pagename, funcname);

            

            foreach (DictionaryEntry de in ht)
            {
                ret = ret + de.Key.ToString() + ":" + de.Value + ","; 
            }
            ret= "{" + ret + "}";
            context.Response.ContentType = "text/plain";
            context.Response.Write(ret);
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