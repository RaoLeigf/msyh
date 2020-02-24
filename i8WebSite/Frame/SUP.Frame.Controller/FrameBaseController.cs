using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using NG3;
//using NG3.SUP.Frame;
using System.Data;
using NG3.Data.Builder;
using NG3.Web.Mvc;
using NG3.Web.Controller;
using NG3.Base;
//using NG3.Common;

namespace SUP.Frame.Controller
{
    /// <summary>
    /// 框架基础服务
    /// </summary>
    public class FrameBaseController : AFController
    {
        public FrameBaseController()
        {
 
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        [UrlMethod, UrlSecurity(UrlSecurity.Everyone), ActionAuthorize(Level.NonCheck)]
        public void GetGroup()
        {
            Ajax.WriteRaw(AppInfo.UserGroup);
        }
        /// <summary>
        /// 获取主框架状态栏信息
        /// </summary>
        [UrlMethod, UrlSecurity(UrlSecurity.LoginUser)]
        public void GetStatusBarContent()
        {
            Ajax.WriteRaw("当前登录用户:[" + AppInfo.LoginID + "]" + AppInfo.UserName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="btype"></param>
        [UrlMethod, UrlSecurity(UrlSecurity.LoginUser)]
        public void ProductAbout()
        {
            NG3.SUP.NSServer.PruductValidate pv = new NG3.SUP.NSServer.PruductValidate(AppInfo.UserConnectString);
            int cnum = 0;
            int dnum = 0;
            bool isdemoend = pv.IsDemoEnd("PAS",ref cnum, ref dnum);
            Ajax.WriteRaw(isdemoend.ToString()+"cnum:"+cnum+" dnum:"+dnum);
        }
    }
}
