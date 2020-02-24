using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Mvc;
using System.Web.SessionState;
using Newtonsoft.Json;
using NG3;
using NG3.Aop.Transaction;
using NG3.Bill.Base;
using NG3.Web.Controller;
using SUP.Common.Base;
using SUP.Frame.Facade;
using SUP.Frame.DataEntity;

namespace SUP.Frame.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class CustomFloatMenuController : AFController
    {
        private ICustomFloatMenuFacade proxy;

        SUP.Common.Base.ProductInfo productInfo = new SUP.Common.Base.ProductInfo();

        public CustomFloatMenuController()
        {
            proxy = AopObjectProxy.GetObject<ICustomFloatMenuFacade>(new CustomFloatMenuFacade());
        }

        //[PageAuthorize(RightKey = 9527)]
        public ActionResult Index()
        {
            if (new OptionSetting().GetInitSettingValue("SUP", "CustomFloatMenu") == "0")
            {
                HttpContext.Response.Redirect("~/SUP/ErrorPage?msg=未启用灵动菜单条！可在基础数据-企业级基础数据-系统选项设置-选项设置页面，公共选项设置-公共选项-是否启用灵动菜单开启。"); //重定向到错误页面
            }

            ViewBag.Code = System.Web.HttpContext.Current.Request.Params["code"];//默认打开业务点

            return View("CustomFloatMenu");
        }

        public string GetSuiteNoName()
        {
            DataSet ds = ReadProductInfo();
            DataTable dt = ds.Tables["SuitInfo"];
            DataTable dt1 = dt.Clone();
            for(int i=0;i<dt1.Columns.Count;i++)
            {
                if(dt1.Columns[i].ColumnName != "Code" && dt1.Columns[i].ColumnName != "Name")
                {
                    dt.Columns.Remove(dt1.Columns[i].ColumnName);
                }
            }

            int totalRecord = 0;
            string json = JsonConvert.SerializeObject(dt);

            json = "{totalRows: " + totalRecord + ", items: " + json + "}";

            return json;
        }

        public DataSet ReadProductInfo()
        {
            string rootpath = System.AppDomain.CurrentDomain.BaseDirectory;

            string file = Path.Combine(rootpath, "product.xml"); //i6Culture.GetPrdXmlFilePath(rootpath);

            DataSet ds = new DataSet();
           
            try
            {
                ds.ReadXml(file);                   
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public JsonResult GetMenuTreeWithCheck()
        {
            DataTable dt = new DataTable();
            string suite = Pub.Request("suite");

            if (string.IsNullOrEmpty(suite))
            {                
                return null;//套件不能为空
            }

            string product = productInfo.ProductCode + productInfo.Series;
            
            dt = proxy.LoadMenuData(product, suite, "");

            string filter = "(pid is null or pid='')";
            IList<TreeJSONBase> list = new CustomFloatMenuTreeBuilder().GetExtTreeList(dt, "pid", "id", filter, TreeDataLevelType.TopLevel);
            return this.Json(list, JsonRequestBehavior.AllowGet);            
        }

        public JsonResult GetMenuTree()
        {
            DataTable dt = new DataTable();
            string suite = Pub.Request("suite");

            if (string.IsNullOrEmpty(suite))
            {                
                return null;//套件不能为空
            }

            string product = productInfo.ProductCode + productInfo.Series;

            dt = proxy.LoadMenuData(product, suite, "");

            string filter = "(pid is null or pid='')";
            IList<TreeJSONBase> list = new CustomFloatMenuSelectedTreeBuilder().GetExtTreeList(dt, "pid", "id", filter, TreeDataLevelType.TopLevel);
            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        public string GetFloatMenuTree()
        {
            string code = Pub.Request("code");
            DataTable dt = proxy.GetFloatMenuTree(code);
            string json = JsonConvert.SerializeObject(dt);
            json = "{items: " + json + "}";
            return json;
        }

        public string GetFloatMenuOutBusName(string code)
        {
            string busName = proxy.GetBusNameByCode(code);
            DataTable dt = proxy.GetFloatMenuOut(code);
            string json = JsonConvert.SerializeObject(dt);
            json = "{\"busName\": \"" + busName + "\", \"floatMenuOut\": " + json + "}";
            return json;
        }

        public string GetFloatMenuIn(string code)
        {
            DataTable dt = proxy.GetFloatMenuIn(code);
            return JsonConvert.SerializeObject(dt);
        }

        public bool SaveFloatMenu(string code,string floatMenus)
        {
            List<FloatMenuEntity> floatMenuList = JsonConvert.DeserializeObject<List<FloatMenuEntity>>(floatMenus);
            return proxy.SaveFloatMenu(code, floatMenuList);
        }

        public string GetFloatMenuByCode(string code)
        {
            DataTable dt = new DataTable();
            if (new OptionSetting().GetInitSettingValue("SUP", "CustomFloatMenu") != "0")
            { 
                dt = proxy.GetFloatMenuByCode(code);
            }
            string json = JsonConvert.SerializeObject(dt);
            json = "{items: " + json + "}";
            return json;
        }

        public string GetReportList()
        {
            return proxy.LoadReportList();
        }

        public string GetSearchReportList(string search)
        {
            return proxy.LoadSearchReportList(search);
        }

        public string GetSheet()
        {
            string phid = Pub.Request("phid");
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(phid))
            {
                dt = proxy.GetSheet(phid);
            }

            return "{data: " + JsonConvert.SerializeObject(dt) + "}";
        }

        public string GetDsc()
        {
            string phid = Pub.Request("phid");
            string sheetid = Pub.Request("sheetid");
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(phid) || !string.IsNullOrEmpty(sheetid))
            {
                dt = proxy.GetDsc(phid, sheetid);
            }

            return "{data: " + JsonConvert.SerializeObject(dt) + "}";
        }

        public string GetPara()
        {
            string phid = Pub.Request("phid");
            string sheetid = Pub.Request("sheetid");
            string ds_no = Pub.Request("ds_no");
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(phid) || !string.IsNullOrEmpty(sheetid) || !string.IsNullOrEmpty(ds_no))
            {
                dt = proxy.GetPara(phid, sheetid, ds_no);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string dsc_para = dt.Rows[i]["dsc_para"].ToString();
                    string displayname = dt.Rows[i]["displayname"].ToString();
                    if (!string.IsNullOrEmpty(displayname) && !displayname.Equals(dsc_para))
                    {
                        dt.Rows[i]["displayname"] = dt.Rows[i]["dsc_para"] + "(" + dt.Rows[i]["displayname"] + ")";
                    }
                    else
                    {
                        dt.Rows[i]["displayname"] = dsc_para;
                    }
                }
            }

            return "{data: " + JsonConvert.SerializeObject(dt) + "}";
        }

        class CustomFloatMenuTreeJSONBase : TreeJSONBase
        {
            public virtual string pid { get; set; }

            public virtual bool @checked { get; set; }

            public virtual string code { get; set; }
        }

        public class CustomFloatMenuTreeBuilder : ExtJsTreeBuilderBase
        {
            public override TreeJSONBase BuildTreeNode(DataRow dr)
            {
                CustomFloatMenuTreeJSONBase node = new CustomFloatMenuTreeJSONBase();
                node.pid = dr["pid"].ToString();
                node.expanded = string.IsNullOrEmpty(node.pid);
                node.id = dr["id"].ToString();
                node.text = dr["name"].ToString();
                node.@checked = false;
                node.code = dr["code"].ToString();
                return node;
            }
        }

        class CustomFloatMenuSelectedTreeJSONBase : TreeJSONBase
        {
            public virtual string pid { get; set; }

            public virtual string code { get; set; }
        }

        public class CustomFloatMenuSelectedTreeBuilder : ExtJsTreeBuilderBase
        {
            public override TreeJSONBase BuildTreeNode(DataRow dr)
            {
                CustomFloatMenuSelectedTreeJSONBase node = new CustomFloatMenuSelectedTreeJSONBase();
                node.pid = dr["pid"].ToString();
                node.expanded = string.IsNullOrEmpty(node.pid);
                node.id = dr["id"].ToString();
                node.text = dr["name"].ToString();
                node.code = dr["code"].ToString();
                return node;
            }
        }
    }
}