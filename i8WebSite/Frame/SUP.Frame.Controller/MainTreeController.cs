using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Web.Mvc;
using NG3.Aop.Transaction;
using SUP.Frame.Facade;
using NG3.Web.Controller;
using NG3;
using SUP.Common.Base;
using SUP.Frame.DataEntity;
using Newtonsoft.Json;
using System.Web.SessionState;

namespace SUP.Frame.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class MainTreeController : AFController
    {

        private IMainTreeFacade proxy;
        private IMenuConfigFacade MenuConfigProxy;
        private IIndividualSettingFacade IndividualSetting;
        public MainTreeController()
        {
            proxy = AopObjectProxy.GetObject<IMainTreeFacade>(new MainTreeFacade());
            MenuConfigProxy = AopObjectProxy.GetObject<IMenuConfigFacade>(new MenuConfigFacade());
            IndividualSetting = AopObjectProxy.GetObject<IIndividualSettingFacade>(new IndividualSettingFacade());
        }
        //系统功能树
        public ActionResult MainFrameView()
        {
            long userid = AppInfoBase.UserID;
            string usertype = AppInfoBase.UserType;
            //把用户自定义添加的管理对象，从数据库取出，以viewbag传到页面
            string ExistId = MenuConfigProxy.Load(userid);
            ViewBag.ExistId = ExistId;
            //把系统的全部管理对象，从配置文件取出，以viewbag传到页面
            string logid = NG3.AppInfoBase.LoginID;
            string xmlFile = AppDomain.CurrentDomain.BaseDirectory + @"/NG3Config/TabPageChoose.xml";
            //string xmlFile = "TabPageChoose.xml";
            DataSet ds = MenuConfigProxy.ConvertXMLToDataSet(xmlFile); //路径怎么破
            DataTable dt = ds.Tables[0];
            //i6s没有合同树套件
            string product = AppInfoBase.UP.Product;
            string series = AppInfoBase.UP.Series;

            //i8 15.1 去掉合同和文档
            DataRow[] dr = dt.Select("TabPageID = 'TabPageContractManage'");
            if (dr.Length > 0)
            {
                dt.Rows.Remove(dr[0]);
            }
            dr = dt.Select("TabPageID = 'tabPageWmDocTree'");
            if (dr.Length > 0)
            {
                dt.Rows.Remove(dr[0]);
            }
            dr = dt.Select("TabPageID = 'tabPageNavigation'");
            if (dr.Length > 0)
            {
                dt.Rows.Remove(dr[0]);
            }

            ViewBag.ProductId = product + series;
            int totalRecord = 0;
            string json = DataConverterHelper.ToJson(dt, totalRecord);
            ViewBag.DT = json;
            //把企业功能树是否启用、是否显示，系统功能树是否显示，从数据库读出，以viewbag传到页面
            //系统管理员固定显示系统功能树和我的功能树
            if (String.Compare(usertype, UserType.System, true) == 0)
            {
                ViewBag.LoadEnFuncTreeRight = "001";
            }
            else
            {
                json = MenuConfigProxy.LoadEnFuncTreeRight();
                ViewBag.LoadEnFuncTreeRight = json;
            }


            //把页面隐藏与否的状态从数据库读出，以viewbag传到页面
            json = MenuConfigProxy.GetDockControl(userid);
            ViewBag.isDockControl = json;

            //把页面换肤的主题状态从数据库读出，以viewbag传到页面
            json = MenuConfigProxy.GetUITheme(userid);
            ViewBag.UITheme = json;

            //把用户类型传到页面，控制是否显示管理对象栏
            ViewBag.UserType = AppInfoBase.UserType;
            return View("MainFrameView");  
        }
        /// <summary>
        /// 自定义我的功能树页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomMyFuncTree()
        {
            string json = MenuConfigProxy.LoadEnFuncTreeRight();
            ViewBag.LoadEnFuncTreeRight = json;
            ViewBag.UserType = AppInfoBase.UserType;
            return View("CustomMyFuncTree");
        }
        /// <summary>
        /// 个性化设置页面
        /// </summary>
        /// <returns></returns>
        public ActionResult IndividualConfig()
        {
            string json = MenuConfigProxy.LoadEnFuncTreeRight();
            ViewBag.LoadEnFuncTreeRight = json;
            
            string product = AppInfoBase.UP.Product;
            string series = AppInfoBase.UP.Series;
            long userid = AppInfoBase.UserID;
            //小铃铛设置的所有字段
            //string xmlFile = AppDomain.CurrentDomain.BaseDirectory + @"/i6SConfig/i6sAlertConfig.xml";
            string xmlFile = AppDomain.CurrentDomain.BaseDirectory + @"/"+ product + series + "Config/"+ product+ series + "AlertConfig.xml";
            //string xmlFile = "TabPageChoose.xml";
            DataSet ds = MenuConfigProxy.ConvertXMLToDataSet(xmlFile); //路径怎么破
            DataTable dt = ds.Tables[0];
            int totalRecord = 0;
            json = DataConverterHelper.ToJson(dt, totalRecord);
            ViewBag.BellItemsAll = json;

            //用户添加小铃铛设置的字段
            string BellItemsExist = IndividualSetting.LoadAlertItem();
            if (string.IsNullOrEmpty(BellItemsExist) || BellItemsExist.Substring(0, 1) == "[")
            {
                ViewBag.BellItemsExist = "";
            }
            else
            {
                ViewBag.BellItemsExist = BellItemsExist;
            }

            string AllowUser = IndividualSetting.LoadPictureSet();
            ViewBag.AllowUser = AllowUser;

            //把用户类型传到页面，控制要显示的tab页
            ViewBag.UserType = AppInfoBase.UserType;

            string uitheme = MenuConfigProxy.GetUITheme(userid);
            ViewBag.UITheme = uitheme;
            return View("IndividualConfig");
        }
        public ActionResult IndividualNavigation()
        {
            string text = System.Web.HttpContext.Current.Request.Params["text"];
            ViewBag.Text = text;
            string json = MenuConfigProxy.LoadEnFuncTreeRight();
            ViewBag.LoadEnFuncTreeRight = json;
            ViewBag.UserType = AppInfoBase.UserType;
            return View("IndividualNavigation");
        }

        public ActionResult Navigation()
        {
            string text = System.Web.HttpContext.Current.Request.Params["text"];
            ViewBag.Text = text;
            return View("Navigation");
        }

        public ActionResult AlertView()
        {
            string buskey = System.Web.HttpContext.Current.Request.Params["buskey"];
            //string xmlFile = "TabPageChoose.xml";
            //DataSet ds = MenuConfigProxy.ConvertXMLToDataSet(xmlFile); //路径怎么破
            //DataTable dt = ds.Tables[0];

            string cachedKey = "AlertConfig"; 
            DataTable dt;
            if (System.Web.HttpRuntime.Cache.Get(cachedKey) != null)
            {
                string dtJson = System.Web.HttpRuntime.Cache.Get(cachedKey).ToString();
                dt = (DataTable)JsonConvert.DeserializeObject(dtJson, typeof(DataTable));
            }
            else
            {
                string product = AppInfoBase.UP.Product;
                string series = AppInfoBase.UP.Series;
                string xmlFile = AppDomain.CurrentDomain.BaseDirectory + @"/" + product + series + "Config/" + product + series + "AlertConfig.xml";

                DataSet ds = MenuConfigProxy.ConvertXMLToDataSet(xmlFile); //路径怎么破
                dt = ds.Tables[0];

                string dtJson = JsonConvert.SerializeObject(dt);
                //缓存起来
                HttpRuntime.Cache.Add(cachedKey,
                                          dtJson,
                                          null,
                                          DateTime.Now.AddDays(1),
                                          System.TimeSpan.Zero,
                                          System.Web.Caching.CacheItemPriority.NotRemovable,
                                          null);
            }

            DataRow[] dr = dt.Select("buskey = '" + buskey + "'");
            ViewBag.text = "未读";
            if (dr.Length > 0)
            {
                ViewBag.text = dr[0]["Name"];
            }
            return View("AlertView");
        }
        public JsonResult LoadMenu()
        {
            string nodeid = System.Web.HttpContext.Current.Request.Params["node"];
            string suite = System.Web.HttpContext.Current.Request.Params["suite"];
            //string functiontree = System.Web.HttpContext.Current.Request.Params["functiontree"];//这个好像是没用了，等等删掉
            
            //是否控制权限的开关，flag,默认为true，控制权限
            string flagString = System.Web.HttpContext.Current.Request.Params["flag"];
            bool rightFlag = true;
            if (flagString == "false")
            {
                rightFlag = false;
            }

            //系统功能树是否懒加载的开关，lazyLoadFlag,默认为true
            string lazyLoadFlagString = System.Web.HttpContext.Current.Request.Params["lazyLoadFlag"];
            bool lazyLoadFlag = true;
            if (lazyLoadFlagString == "false")
            {
                lazyLoadFlag = false;
            }

            //按指定SQL语句构建系统功能树
            string treeFilter = System.Web.HttpContext.Current.Request.Params["treeFilter"];
            string userType = NG3.AppInfoBase.UserType;
            SUP.Common.Base.ProductInfo prdInfo = new SUP.Common.Base.ProductInfo();
            //IList<TreeJSONBase> list = proxy.LoadMenu(prdInfo.ProductCode+prdInfo.Series, suite, false, userType, AppInfoBase.OrgID, AppInfoBase.UserID, nodeid,functiontree,flag);
            IList<TreeJSONBase> list = proxy.LoadMenu(prdInfo.ProductCode + prdInfo.Series, suite, false, userType, AppInfoBase.OrgID, AppInfoBase.UserID, nodeid, rightFlag, lazyLoadFlag,treeFilter);

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetSuiteList()
        {
            string flagString = System.Web.HttpContext.Current.Request.Params["flag"];
            //是否控制权限的开关，flag,默认为true，控制权限
            bool rightFlag = true;
            if (flagString == "false")
            {
                rightFlag = false;
            }
            string treeFilter = System.Web.HttpContext.Current.Request.Params["treeFilter"];
            IList<SUP.Frame.DataEntity.SuiteInfoEntity> list = proxy.GetSuiteList(rightFlag,treeFilter);
            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Query()
        {
           
            string condition = System.Web.HttpContext.Current.Request.Params["condition"];
            string flagString = System.Web.HttpContext.Current.Request.Params["flag"];
            string treeFilter = System.Web.HttpContext.Current.Request.Params["treeFilter"];
            bool rightFlag = true;
            if (flagString == "false")
            {
                rightFlag = false;
            }
            string userType = NG3.AppInfoBase.UserType;
            SUP.Common.Base.ProductInfo prdInfo = new SUP.Common.Base.ProductInfo();
            IList<TreeJSONBase> list = proxy.Query(prdInfo.ProductCode + prdInfo.Series,  false, userType, AppInfoBase.OrgID, AppInfoBase.UserID,  condition, rightFlag,treeFilter);
            return this.Json(list, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 自定义我的功能树页面参数编辑里可添加参数
        /// </summary>
        /// <returns></returns>
        public string LoadEditParm()
        {
            List<MacroEntity> _macroList = new List<MacroEntity>(); 
            string logid = NG3.AppInfoBase.LoginID;
            MacroEntity macroEntity = null;
            macroEntity = new MacroEntity("loginID", "当前登陆的账号", NG3.AppInfoBase.LoginID);
            _macroList.Add(macroEntity);
            macroEntity = new MacroEntity("userName", "当前登陆的用户名", NG3.AppInfoBase.UserName);
            _macroList.Add(macroEntity);
            macroEntity = new MacroEntity("uCode", "当前帐套号", NG3.AppInfoBase.UCode);
            _macroList.Add(macroEntity);
            macroEntity = new MacroEntity("oCode", "当前组织代码", NG3.AppInfoBase.OCode);
            _macroList.Add(macroEntity);
            macroEntity = new MacroEntity("orgName", "当前组织名称", NG3.AppInfoBase.OrgName);
            _macroList.Add(macroEntity);            
            macroEntity = new MacroEntity("userHostName", "客户端主机名", Request.ServerVariables.Get("Remote_Host").ToString());
            _macroList.Add(macroEntity);
            macroEntity = new MacroEntity("loginId", "客户端ip地址", Request.ServerVariables.Get("Remote_Addr").ToString());
            _macroList.Add(macroEntity);
            string str = JsonConvert.SerializeObject(_macroList);
            return str;
        }
        /// <summary>
        /// 保存左侧树是否隐藏标记
        /// </summary>
        public bool SaveDockControll()
        {
            string isDockControlString = System.Web.HttpContext.Current.Request.Params["isDockControl"];
            int isDockControl = int.Parse(isDockControlString);
            long userid = AppInfoBase.UserID;
            return MenuConfigProxy.SaveDockControl(isDockControl, userid);
        }

        public bool SaveUITheme()
        {
            string UIThemeString = System.Web.HttpContext.Current.Request.Params["UITheme"];
            int UITheme = int.Parse(UIThemeString);
            long userid = AppInfoBase.UserID;
            return MenuConfigProxy.SaveUITheme(UITheme, userid);
        }

        //根据busphid取fg3-menu数据的接口
        public DataTable GetMenuByBusphid(long busphid)
        {
            return proxy.GetMenuByBusphid(busphid);
        }

        //取小铃铛弹窗配置的接口
        public void GetAlertConfig(out string BellItemsAll, out string BellItemsExist)
        {
            string product = AppInfoBase.UP.Product;
            string series = AppInfoBase.UP.Series;
            long userid = AppInfoBase.UserID;
            //小铃铛设置的所有字段
            //string xmlFile = AppDomain.CurrentDomain.BaseDirectory + @"/i6SConfig/i6sAlertConfig.xml";
            string xmlFile = AppDomain.CurrentDomain.BaseDirectory + @"/" + product + series + "Config/" + product + series + "AlertConfig.xml";
            //string xmlFile = "TabPageChoose.xml";
            DataSet ds = MenuConfigProxy.ConvertXMLToDataSet(xmlFile); //路径怎么破
            DataTable dt = ds.Tables[0];
            int totalRecord = 0;
            string json = DataConverterHelper.ToJson(dt, totalRecord);
            BellItemsAll = json;

            //用户添加小铃铛设置的字段
            BellItemsExist = IndividualSetting.LoadAlertItem();
            if (string.IsNullOrEmpty(BellItemsExist) || BellItemsExist.Substring(0, 1) == "[")
            {
                BellItemsExist = "";
            }
            return;
        }
    }
}
