using System;
using Enterprise3.NHORM.Controller;
using NG3.Data.Service;
using SUP.Common.Base;

namespace SUP.CustomForm.Controller
{
    /// <summary>
    /// 自定义单据单点登录
    /// </summary>
    public class CustomAFController : AFCommonController  //AFController
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CustomAFController() : base(string.Empty)  //调老丰基类的带参空构造函数，以免报错
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="requestContext">请求上下文</param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            var isSso = System.Web.HttpContext.Current.Request.Params["issso"] == null ? "" : System.Web.HttpContext.Current.Request.Params["issso"].ToString();

            if (isSso == "1")
            {
                var logId = System.Web.HttpContext.Current.Request.Params["logno"] == null ? "" : System.Web.HttpContext.Current.Request.Params["logno"].ToString();
                var logName = System.Web.HttpContext.Current.Request.Params["logname"] == null ? "" : System.Web.HttpContext.Current.Request.Params["logname"].ToString();
                var oCode = System.Web.HttpContext.Current.Request.Params["ocode"] == null ? "" : System.Web.HttpContext.Current.Request.Params["ocode"].ToString();
                var uCode = System.Web.HttpContext.Current.Request.Params["ucode"] == null ? "" : System.Web.HttpContext.Current.Request.Params["ucode"].ToString();

                //comment by ljy 2018.01.17 特变要求预览能跟正式发布单据一样操作
                //ViewBag.IsSso = "true";
                ViewBag.IsSso = "false";

                try
                {
                    //设置NG3的i6WebInfoBase对象
                    var dbbuilder = new DBConnectionStringBuilder();
                    var dbName = string.Empty;

                    //没有传帐套号过来
                    if (string.IsNullOrWhiteSpace(uCode))
                    {
                        dbName = dbbuilder.DefaultDB;  //如NG0001
                        uCode = dbName.Substring(2);   //如0001
                    }
                    else
                    {
                        dbName = "NG" + uCode;
                    }

                    var result = string.Empty;
                    var pubConn = dbbuilder.GetMainConnStringElement(0, out result, false);  //获取主数据库连接串 NGSoft
                    var userConn = dbbuilder.GetAccConnstringElement(0, dbName, pubConn, out result);  //获取默认数据库连接串 NG0001

                    var i6AppInfo = new I6WebAppInfo()
                    {
                        UserType = UserType.OrgUser,
                        PubConnectString = pubConn,
                        UserConnectString = userConn,
                        LoginID = logId,
                        UserName = logName,
                        OCode = oCode,
                        UCode = uCode,
                        DbName = dbName,
                        UserID = Convert.ToInt64(DbHelper.GetString(userConn, string.Format("select phid from fg3_user where userno='{0}'", logId))),
                        OrgID = Convert.ToInt64(DbHelper.GetString(userConn, string.Format("select phid from fg_orglist where ocode='{0}'", oCode)))
                    };

                    System.Web.HttpContext.Current.Session["NGWebAppInfo"] = i6AppInfo;
                    ConnectionInfoService.SetSessionConnectString(i6AppInfo.UserConnectString);                    
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message); //应该加入日志，设置i6WebAppInfo异常。                    
                }                
            }

            base.Initialize(requestContext);

            //调老丰AFCommonController的InitialAF()，里面含WorkFlowHandling()
            string dbnameStr = NG3.AppInfoBase.DbName;
            if (string.IsNullOrWhiteSpace(dbnameStr))
            {
                dbnameStr = base.NGPreCompileHandling();
            }
            base.InitialAF(dbnameStr);
        }
    }
}
