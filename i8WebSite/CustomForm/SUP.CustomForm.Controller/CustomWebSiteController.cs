using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Mvc;
using NG3;
using NG3.Data.Service;
using SUP.Common.Base;

namespace SUP.Controllers
{
    //初始化类
    public class CustomController : Controller
    {
        //初始化数据库
        private void InitDb()
        {
            //数据库连接方式取值方法一：固定链接串
            //string connectionString=@"ConnectType=SqlClient;Server=10.0.16.168\upty;Database=NG0054;User ID=sa;Password=123456";
            //string connectionString = "ConnectType=SqlClient;Server=10.0.18.21;Database=NG0008;User ID=sa;Password=";
            //string connectionString = "ConnectType=SqlClient;Server=10.0.17.118;Database=NG0004;User ID=sa;Password=psoft";

            //二：从web.config取值
            //string connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

            //三：从config目录下文件DataBases.xml中取值
            DBConnectionStringBuilder dbbuilder = new DBConnectionStringBuilder();
            //var userConn = dbbuilder.GetDefaultConnString(); //主帐套连接串

            var result = string.Empty;
            var dbname = dbbuilder.DefaultDB;
            var pubConn = dbbuilder.GetMainConnStringElement(0, out result, false);  //获取主数据库连接串 NGSoft
            var userConn = dbbuilder.GetAccConnstringElement(0, dbname, pubConn, out result);  //获取默认数据库连接串 NG0001

            ConnectionInfoService.SetSessionConnectString(userConn);

            I6WebAppInfo appInfo = new I6WebAppInfo();
            appInfo.PubConnectString = pubConn;
            appInfo.UserConnectString = userConn;
            appInfo.LoginID = "xyp";
            appInfo.UserName = "xypname";
            appInfo.OCode = "001";
            appInfo.UCode = dbname.Substring(2);
            appInfo.DbName = dbname;
            //appInfo.UserID = 2;
            //appInfo.OrgID = 1;

            string uid = DbHelper.GetString(userConn, string.Format("select phid from fg3_user where userno='{0}'", appInfo.LoginID));
            string oid = DbHelper.GetString(userConn, string.Format("select phid from fg_orglist where ocode='{0}'", appInfo.OCode));

            if (!string.IsNullOrWhiteSpace(uid))
            {
                appInfo.UserID = Convert.ToInt64(uid);
            }
            else
            {
                appInfo.UserID = 1;
            }

            if (!string.IsNullOrWhiteSpace(oid))
            {
                appInfo.OrgID = Convert.ToInt64(oid);
            }
            else
            {
                appInfo.OrgID = 1;
            }

            System.Web.HttpContext.Current.Session["NGWebAppInfo"] = appInfo;
        }

        //返回首页
        public ActionResult Index()
        {
            InitDb();

            ViewBag.Root = System.Web.HttpContext.Current.Request.ApplicationPath;
            if (ViewBag.Root == "/")
            {
                ViewBag.Root = "";
            }
            ViewBag.Path = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;

            ViewBag.Logid = AppInfoBase.LoginID;
            ViewBag.OCode = AppInfoBase.OCode;
            ViewBag.userID = AppInfoBase.UserID;
            ViewBag.orgID = AppInfoBase.OrgID;

            return View("Index");
        }

        public ActionResult Generate()
        {
            return View("Generate");
        }

        public ActionResult Error()
        {
            ViewData["error"] = System.Web.HttpContext.Current.Request.Params["error"];
            return View("ErrorView");
        }

        //add by ljy 180604，用于测试app表单，以后删除此函数
        public ActionResult Aform0000000083()
        {
            ViewBag.Title = "Aform0000000083";

            return View("Aform0000000083");
        }
    }

    //错误信息类
    public class ErrorController : Controller
    {
        public ActionResult Error()
        {
            ViewData["error"] = System.Web.HttpContext.Current.Request.Params["error"];
            return View("ErrorView");
        }
    }

    //树菜单类
    public class CustomTreeController : Controller
    {
        public JsonResult LoadTree()
        {
            string app = System.Web.HttpContext.Current.Request.ApplicationPath;

            if (app == "/")
            {
                app = string.Empty;
            }

            string configPath = this.Request.PhysicalApplicationPath + "\\NG3Config\\MainNavigation.xml";

            DataSet ds = new DataSet();
            ds.ReadXml(configPath);

            IList<TreeJSON> nodeList = new List<TreeJSON>();

            IList<TreeJSON> list = new List<TreeJSON>();

            TreeJSON node = null;
            TreeJSON childNode = null;

            for (int i = 0; i < ds.Tables["TreeNodeGroup"].Rows.Count; i++)
            {
                node = new TreeJSON();
                node.id = "Node_" + i.ToString().PadLeft(2, '0');
                node.cls = "folder";
                node.text = ds.Tables["TreeNodeGroup"].Rows[i]["Text"].ToString();
                node.expanded = false;
                node.children = new List<TreeJSON>();
                int twoIndex = 0;

                for (int j = 0; j < ds.Tables["TreeNode"].Rows.Count; j++)
                {
                    if (ds.Tables["TreeNode"].Rows[j]["TreeNodeGroup_id"].ToString() ==
                        ds.Tables["TreeNodeGroup"].Rows[i]["TreeNodeGroup_id"].ToString())
                    {
                        childNode = new TreeJSON();
                        childNode.id = "Node_" + i.ToString() + "_" + (twoIndex + j).ToString();
                        childNode.cls = "file";
                        childNode.text = ds.Tables["TreeNode"].Rows[j]["Text"].ToString();
                        childNode.tabTitle = ds.Tables["TreeNode"].Rows[j]["TabTitle"].ToString();
                        childNode.leaf = true;
                        //childNode.url = app + ds.Tables["TreeNode"].Rows[j]["NavigateUrl"].ToString();
                        childNode.url = ds.Tables["TreeNode"].Rows[j]["NavigateUrl"].ToString();
                        //childNode.hrefTarget = ds.Tables["TreeNode"].Rows[j]["NavigateUrl"].ToString();
                        node.children.Add(childNode);
                    }
                }

                nodeList.Add(node);
            }

            JsonResult jsons = this.Json(nodeList, JsonRequestBehavior.AllowGet);
            return jsons;

        }
    }

    [Serializable]
    public class TreeJSON
    {
        //ext树的数据格式
        public virtual string id { get; set; }

        public virtual string text { get; set; }

        public virtual string url { get; set; }

        public virtual string cls { get; set; }

        public virtual bool expanded { get; set; }

        public virtual IList<TreeJSON> children { get; set; }

        public virtual bool leaf { get; set; }

        public virtual string hrefTarget { get; set; }

        public virtual string tabTitle { get; set; }
    }


    //表单生成类
    public class CustomFormController : Controller
    {
        /// <summary>
        /// 生成自定义表单
        /// </summary>
        /// <returns></returns>
        public string BuildCustomForm()
        {
            var fileid = System.Web.HttpContext.Current.Request.Params["fileid"];
            var type = System.Web.HttpContext.Current.Request.Params["type"];

            if (string.IsNullOrEmpty(type))
            {
                type = "web";
            }
            
            //var buildPara = new SUP.CustomForm.DataEntity.BuildParameter();
            //buildPara.Id = fileid;
            //buildPara.Type = type;
            //buildPara.AssemblyPath = AppDomain.CurrentDomain.BaseDirectory + "bin\\";
            //buildPara.CsFilePath = AppDomain.CurrentDomain.BaseDirectory + "CustomFormTemp\\";
            //buildPara.Host = "10.0.18.21"; //原来是：10.0.13.60

            var buildPara = new SUP.CustomForm.DataEntity.BuildParameter();
            buildPara.Id = fileid;
            buildPara.Type = type;
            var status = new SUP.CustomForm.Builder.Build().BuildCustomForm(buildPara);

            return "{status:\"" + status + "\"}";
        }

        /// <summary>
        /// 删除自定义表单生成的类库和文件
        /// </summary>
        /// <returns></returns>
        public string DeleteCustomForm()
        {
            var fileid = System.Web.HttpContext.Current.Request.Params["fileid"];
            var fileList = GenDeleteFiles(fileid);
            var path = AppDomain.CurrentDomain.BaseDirectory + "bin\\";
            foreach (var value in fileList)
            {
                if (System.IO.File.Exists(path + value))
                {
                    System.IO.File.Delete(path + value);
                }
            }

            var dirPath = AppDomain.CurrentDomain.BaseDirectory + "CustomFormTemp\\pform" + fileid;
            Directory.Delete(dirPath, true);

            return "{status:\"ok\"}";
        }

        private static IEnumerable<string> GenDeleteFiles(string id)
        {
            var fileList = new List<string>();
            var items = new string[] { "Controller", "DataAccess", "Facade" };
            var last = new string[] { "dll", "pdb" };
            for (int i = 0; i < items.Length; i++)
            {
                for (int j = 0; j < last.Length; j++)
                {
                    var file = string.Format("SUP.CustomForm.pform{0}.{1}.{2}", id, items[i], last[j]);
                    fileList.Add(file);
                }
            }

            return fileList;
        }
    }
}
