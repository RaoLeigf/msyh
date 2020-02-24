using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Web.Mvc;
using NG3.Aop.Transaction;
using SUP.Frame.Facade;
using NG3.Web.Controller;
using NG3;
using SUP.Common.Base;
using System.Web.SessionState;
using WM3.KM.Service.Interface;
using Newtonsoft.Json.Linq;

namespace SUP.Frame.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class NavigationCenterController : AFController
    {
        private INavigationCenterFacade proxy;
        private IMenuConfigFacade MenuConfigProxy;
        private IWiKiCategoryService WiKiCategory;
        private IWikiMainService WikiMain;
        public NavigationCenterController()
        {
            proxy = AopObjectProxy.GetObject<INavigationCenterFacade>(new NavigationCenterFacade());
            MenuConfigProxy = AopObjectProxy.GetObject<IMenuConfigFacade>(new MenuConfigFacade());
            WiKiCategory = new Enterprise3.NHORM.Base.SpringNHBase.SpringObject().GetObject<IWiKiCategoryService>("WM3.KM.Service.WiKiCategory");
            WikiMain = new Enterprise3.NHORM.Base.SpringNHBase.SpringObject().GetObject<IWikiMainService>("WM3.KM.Service.WikiMain");
        }
        public ActionResult NavigationCenter()
        {
            return View("NavigationCenter");
        }

        public ActionResult IndividualNavigationTree()
        {
            return View("IndividualNavigationTree");
        }

        public ActionResult IndividualNavigationChart()
        {
            string text = System.Web.HttpContext.Current.Request.Params["text"];
            string currentNodePhid = System.Web.HttpContext.Current.Request.Params["currentNodePhid"];
            string myid = System.Web.HttpContext.Current.Request.Params["myid"];
            string json = MenuConfigProxy.LoadEnFuncTreeRight();
            ViewBag.LoadEnFuncTreeRight = json;
            ViewBag.Text = text;
            ViewBag.currentNodePhid = currentNodePhid;
            ViewBag.UserType = AppInfoBase.UserType;
            if(myid!=""&& myid != null)
            {
                ViewBag.Myid = myid;
            }
            else
            {
                ViewBag.Myid = "";
            }
            return View("IndividualNavigationChart");
        }

        public JsonResult LoadTree()
        {
            string nodeid = System.Web.HttpContext.Current.Request.Params["node"];
            string type = System.Web.HttpContext.Current.Request.Params["type"];
            long userid = AppInfoBase.UserID;//作为参数传进来
            //string useridString = System.Web.HttpContext.Current.Request.Params["userid"];
            //int userid = int.Parse(useridString);

            IList<TreeJSONBase> list = proxy.LoadTree(type);
            if (list == null)
            {
                return null;
            }
            else
            {
                return this.Json(list, JsonRequestBehavior.AllowGet);
            }

        }
        public string SaveTree()
        {
            string NavigationTree = System.Web.HttpContext.Current.Request.Form["NavigationTree"];
            long userid = AppInfoBase.UserID;
            DataTable NavigationTreeTable = DataConverterHelper.ToDataTable(NavigationTree, "select * from fg3_process_tree");
            int iret;
            try
            {
                iret = proxy.SaveTree(NavigationTreeTable);
                if (iret > 0)
                {
                    return "{status : \"ok\"}";
                }
                else
                {
                    return "{status : \"error\",messsage:\"保存失败\"}";
                }
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                return "{status : \"error\",messsage:\"" + ex.Message + "\"}";
            }


        }

        public string LoadChart(string phid)
        {
            return proxy.LoadChart(phid);
        }

        public string SaveChart(string svgConfig, string phid)
        {
            return proxy.SaveChart(svgConfig, phid);
        }

        public JsonResult LoadWiKiCategory()
        {
            IList<TreeJSONBase> list = WiKiCategory.GetWiKiCategoryForProcess();
            if (list == null)
            {
                return null;
            }
            else
            {
                return this.Json(list, JsonRequestBehavior.AllowGet);
            }

        }

        public string FindWikiByProcess(long phid)
        {
            IList<WM3.KM.Model.Domain.WikiMainModel> list = WikiMain.FindWikiByProcess(phid);
            if (list == null || list.Count < 1)
            {
                return null;
            }
            StringBuilder strbuilder = new StringBuilder();
            StringBuilder strbuilderOrg = new StringBuilder();
            strbuilder.Append(string.Format("<ul id='FindWikiByProcess'>"));
            strbuilderOrg.Append(string.Format("<ul id='FindWikiByProcess'>"));
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].IsOrgShare != "1" || list[i].CurOrgId == AppInfoBase.OrgID)
                {
                    strbuilder.Append(string.Format("<li id={0} name={1}><a target ='_self' id={0}> {1} </a></li>", list[i].PhId, list[i].Cname));
                }
                if (list[i].IsOrgShare == "1" && list[i].CurOrgId == AppInfoBase.OrgID)
                {
                    strbuilderOrg.Append(string.Format("<li id={0} name={1}><a target ='_self' id={0}> {1} </a></li>", list[i].PhId, list[i].Cname));
                }
            }
            //for (int i = 0; i < 100 && list.Count>0; i++)
            //{
            //    strbuilder.Append(string.Format("<li url={0} name={1}><a> {1} </a></li>", list[0].Creator, list[0].Cname));
            //} 
            strbuilder.Append(string.Format("</ul>"));
            string allWiki = strbuilder.ToString();

            strbuilderOrg.Append(string.Format("</ul>"));
            string orgWiki = strbuilderOrg.ToString();

            JObject jo = new JObject();
            jo.Add("allWiki", allWiki);
            jo.Add("orgWiki", orgWiki);
            return jo.ToString();
        }

        public string FindProcessByWiki(long phid)
        {
            IList<long> phidList =  WikiMain.FindProcessByWiki(phid);
            DataTable dt = proxy.FindProcessByWiki(phidList);

            if (dt==null || dt.Rows.Count<1)
            {
                return null;
            }
            StringBuilder strbuilder = new StringBuilder();
            strbuilder.Append(string.Format("<ul id='FindProcessByWiki'>"));
            foreach (DataRow dr in dt.Rows)
            {
                strbuilder.Append(string.Format("<li phid={0} name={1}><a target ='_self' phid={0}> {1} </a></li>", dr["phid"].ToString(), dr["name"].ToString()));
            }
            //for (int i = 0; i < 100 && list.Count>0; i++)
            //{
            //    strbuilder.Append(string.Format("<li url={0} name={1}><a> {1} </a></li>", list[0].Creator, list[0].Cname));
            //}
            strbuilder.Append(string.Format("</ul>"));
            return strbuilder.ToString();
        }

        public string FindWikiByCategoty(Int64 phid)
        {
            IList<WM3.KM.Model.Domain.WikiMainModel> list = WikiMain.FindWikiByCategoty(phid);
            if (list == null || list.Count < 1)
            {
                return null;
            }
            StringBuilder strbuilder = new StringBuilder();
            strbuilder.Append(string.Format("<ul id='FindWikiByCategoty'>"));
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].IsOrgShare != "1" || list[i].CurOrgId == AppInfoBase.OrgID)
                {
                    strbuilder.Append(string.Format("<li id={0} name={1}><a target ='_self' id={0}> {1} </a></li>", list[i].PhId, list[i].Cname));
                }
            }
            //for (int i = 0; i < 100 && list.Count > 0; i++)
            //{
            //    strbuilder.Append(string.Format("<li url={0} name={1}><a> {1} </a></li>", list[0].Creator, list[0].Cname));
            //}
            strbuilder.Append(string.Format("</ul>"));
            return strbuilder.ToString();
        }
    }
}
