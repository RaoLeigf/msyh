using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Mvc;
using NG3.Aop.Transaction;
using SUP.Frame.Facade;
using NG3.Web.Controller;
using NG3;
using SUP.Common.Base;
using System.Data;
using System.Web.SessionState;

namespace SUP.Frame.Controller 
{
    public class ReportListController : AFController
    {
        private IReportListFacade proxy;
        public ReportListController()
        {
            proxy = AopObjectProxy.GetObject<IReportListFacade>(new ReportListFacade());
        }
        public JsonResult LoadReportList()
        {
            long userid = AppInfoBase.UserID;//作为参数传进来
            long orgidInt = AppInfoBase.OrgID;

            string page=System.Web.HttpContext.Current.Request.Params["page"];
            //string useridString = System.Web.HttpContext.Current.Request.Params["userid"];

            string useridString = userid.ToString();
            //int orgidInt = int.Parse(orgid);
            

            IList <TreeJSONBase> list = proxy.LoadReportList(useridString, orgidInt,page);
            if (list == null)
            {
                return null;
            }
            else
            {
                return this.Json(list, JsonRequestBehavior.AllowGet);
            }

        }
    }
}
