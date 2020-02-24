using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Right.Facade;
using NG3.Aop.Transaction;
using NG3;
using System.Web.Mvc;
using System.Web.SessionState;

namespace SUP.Right.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class PageRightController : NG3.Web.Controller.AFController
    {
        private PageRightFacad rightFacad;

        public PageRightController()
        {
            rightFacad = AopObjectProxy.GetObject<PageRightFacad>(new PageRightFacad());
        }
        /// <summary>
        /// 获取界面上没用权限的按钮
        /// </summary>
        /// <param name="UCode">账套</param>
        /// <param name="organizeId">组织</param>
        /// <param name="userId">账户</param>
        /// <param name="userType"></param>
        /// <param name="pageName">界面</param>
        /// <param name="FuncName">功能按钮</param>
        /// <returns></returns>
        public string GetNonFormRightsItems(string UCode, Int64 orgID, Int64 userID, string userType, string pageName, string FuncName,string connStr)
        {
            return rightFacad.GetNonFormRightsItems(UCode, orgID, userID, userType, pageName, FuncName,connStr);
        }

        public string GetNoRightsButtons(string pageName,string FuncName)
        {
            string orgid = System.Web.HttpContext.Current.Request.Params["orgid"];
            Int64 org = (string.IsNullOrWhiteSpace(orgid)) ? AppInfoBase.OrgID : Convert.ToInt64(orgid);
            string str = rightFacad.GetNonFormRightsItems(AppInfoBase.UCode, org, AppInfoBase.UserID, AppInfoBase.UserType, pageName, FuncName,AppInfoBase.UserConnectString);
            return str;
        }
    }
}
