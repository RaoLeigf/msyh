using Newtonsoft.Json;
using NG3.Aop.Transaction;
using NG3.Web.Controller;
using SUP.Common.Base;
using SUP.Frame.Facade;
using SUP.Frame.Facade.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MDP.BusObj.Facade;
using MDP.BusObj.Model.SysMenu;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Net;

namespace SUP.Frame.Controller
{
    public class OptionSettingController : AFController
    {

        private const string UPAppInfoNameInSession = "NGWebAppInfo";

        private IOptionSettingFacade proxy;

        public OptionSettingController()
        {
            proxy = AopObjectProxy.GetObject<IOptionSettingFacade>(new OptionSettingFacade());
        }

        public ActionResult OptionSetting()
        {
            string logid = NG3.AppInfoBase.LoginID;
            ViewBag.isEnable = proxy.GetInitSetting();
            return View("OptionSetting");
        }

        /// <summary>
        /// 绑定选项分类树
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOptionTree()
        {
            string moduleid = System.Web.HttpContext.Current.Request.Params["moduleid"];
            //IList<TreeJSONBase> orgList = proxy.LoadOrgTree();
            IList<TreeJSONBase> list = proxy.LoadOptionTree(moduleid);
            if (list == null)
            {
                return null;
            }
            else
            {
                return this.Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 绑定选项分类树
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOrgTree()
        {
            string detailPhid = System.Web.HttpContext.Current.Request.Params["detailPhid"];
            IList<TreeJSONBase> list = proxy.LoadOrgTree(detailPhid);
            if (list == null)
            {
                return null;
            }
            else
            {
                return this.Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存初始化设置数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetargumentTree()
        {
            JsonResult argJson = this.GetOptionTree();
            return argJson;
        }

        
        /// <summary>
        /// 绑定选项分类树
        /// </summary>
        /// <returns></returns>
        public int SaveInitSetting()
        {
            return proxy.SaveInitSetting();
        }

        /// <summary>
        /// 获取选项明细列表数据
        /// </summary>
        public string GetGridList()
        {
            //string url = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/i8/SUP/ShortcutMenu/GetShortcutKey";

            //WebClient wc = new WebClient();
            //Byte[] pageData = wc.DownloadData(url);

            //string shortcut = Encoding.UTF8.GetString(pageData);

            string json = string.Empty;
            string phid = System.Web.HttpContext.Current.Request.Params["phid"];
            string limit = System.Web.HttpContext.Current.Request.Params["limit"];
            string page = System.Web.HttpContext.Current.Request.Params["page"];
            string type = System.Web.HttpContext.Current.Request.Params["type"];//判断是初始化还是选项设置界面

            int pageSize = 20;
            int.TryParse(limit, out pageSize);
            int pageIndex = 0;
            int.TryParse(page, out pageIndex);

            int totalRecord = 0;

            DataTable dt = proxy.GetGridList(Convert.ToInt64(phid), pageSize, pageIndex, ref totalRecord, type);
            json = DataConverterHelper.ToJson(dt, totalRecord);
            return json;
        }

        /// <summary>
        /// 获取纳税组织列表
        /// </summary>
        public string GetTaxOrgGrid()
        {
            string json = string.Empty;
            int totalRecord = 0;

            string detailPhid = System.Web.HttpContext.Current.Request.Params["detailPhid"];
            DataTable dt = proxy.GetTaxOrgGrid(detailPhid);
            json = DataConverterHelper.ToJson(dt, totalRecord);
            return json;
        }

        public string GetOptionValue()
        {
            string phid = System.Web.HttpContext.Current.Request.Params["phid"];
            DataTable dt = proxy.GetOptionValue(Convert.ToInt64(phid));
            int totalRecord = dt.Rows.Count;
            string json = DataConverterHelper.ToJson(dt, totalRecord);
            return json;
        }

        /// <summary>
        /// 根据分组和选项代码获取选项列表
        /// </summary>
        /// <returns></returns>
        public string GetOptionDetail(string option_group, string option_code)
        {
            //string option_group = System.Web.HttpContext.Current.Request.Params["option_group"];
            //string option_code = System.Web.HttpContext.Current.Request.Params["option_code"];
            string detailJson = proxy.GetOptionDetail(option_group, option_code);
            return detailJson;
        }

        /// <summary>
        /// 根据分组和选项代码，组织列表获取组织和参数键值对
        /// </summary>
        /// <returns></returns>
        public string GetArgumentDic(string option_group, string option_code, string[] keys)
        {
            //keys = new string[3] { "1","2","3"};
            Dictionary<string, string> arguDic = proxy.GetArgumentDic(option_group, option_code, keys);
            return DataConverterHelper.SerializeObject(arguDic);
        }

        /// <summary>
        /// 根据分组、选项代码和单个组织标记获取参数值
        /// </summary>
        /// <returns></returns>
        public string GetSingleArgument(string option_group, string option_code, string key)
        {
            //string option_group = System.Web.HttpContext.Current.Request.Params["option_group"];
            //string option_code = System.Web.HttpContext.Current.Request.Params["option_code"];
            //string key = System.Web.HttpContext.Current.Request.Params["key"];
            string argValue = proxy.GetSingleArgument(option_group, option_code, key);
            return argValue;
        }



        /// <summary>
        /// 根据分组、选项代码和单个业务类型标记获取选项值
        /// </summary>
        /// <returns></returns>
        public string GetValueByKey()
        {
            string option_group = System.Web.HttpContext.Current.Request.Params["option_group"];
            string option_code = System.Web.HttpContext.Current.Request.Params["option_code"];
            string key = System.Web.HttpContext.Current.Request.Params["key"];
            if (key == null)
                key = "";
            string option_value = proxy.GetValueByKey(option_group, option_code, key);
            return option_value;
        }

        /// <summary>
        /// 保存明细表信息
        /// </summary>
        /// <returns></returns>
        public int SaveDetailData()
        {
            string logid = NG3.AppInfoBase.LoginID;
            string detailData = System.Web.HttpContext.Current.Request.Params["gridData"];



            DataTable dt = DataConverterHelper.ToDataTable(detailData, "select * from fg3_option_detail");
            int iret = proxy.SaveDetailData(dt, logid);
            return iret;
        }

        public int SaveTaxOrg()
        {
            string detailData = System.Web.HttpContext.Current.Request.Params["gridData"];
            string detailPhid = System.Web.HttpContext.Current.Request.Params["detailPhid"];
            string option_type = System.Web.HttpContext.Current.Request.Params["option_type"];

            DataTable dt = DataConverterHelper.ToDataTable(detailData, "select * from fg3_argument_setting");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["detail_phid"] = Convert.ToInt64(detailPhid);
                    dt.Rows[i]["type"] = option_type;
                }
            }
            int iret = proxy.SaveTaxOrg(dt);
            return iret;
        }


        public string GetFunTree()
        {
            string detailPhid = System.Web.HttpContext.Current.Request.Params["detailPhid"];
            var product = NG3.AppInfoBase.ProductFullName;//Pub.Request("product");
            var nodeid = "root";
            var tablename = "";
            var enTree = false;
            string suite = string.Empty;
            nodeid = string.IsNullOrWhiteSpace(nodeid) ? "root" : nodeid;
            //if (nodeid != "root")
            //{
            //    var temp = 0L;
            //    if (!long.TryParse(nodeid, out temp))
            //    {
            //        suite = nodeid;
            //        nodeid = "root";
            //    }
            //}
            MDP.BusObj.Facade.SysMenuFacade busFacade = new SysMenuFacade();
            IList<TreeJSONBase> json = busFacade.GetMenuHelprTree(product, tablename, enTree, false,nodeid, suite,true);

            DataTable dt = proxy.GetArgumentByPhid(detailPhid);
            foreach (var item in json)
            {
                setChild((SysMenuTreeBase)item, detailPhid, dt);
            }


            return DataConverterHelper.SerializeObject(json);
        }



        private void setChild(SysMenuTreeBase node, string detailPhid, DataTable dt)
        {
            if (node.leaf)
            {
                if (!string.IsNullOrEmpty(node.bustype))
                {
                    DataRow dr = dt.Select("id='" + node.bustype + "'").FirstOrDefault();
                    if (dr != null && dr["argument"] != DBNull.Value)
                    {
                        node.exparams = dr["argument"] != DBNull.Value ? dr["argument"].ToString() : "";
                    }
                    if (dr != null && dr["phid"] != DBNull.Value)
                    {
                        node.customsort = dr["phid"] != DBNull.Value ? dr["phid"].ToString() : "";
                    }
                    if (dr != null && dr["name"] != DBNull.Value)
                    {
                        node.cls = dr["name"] != DBNull.Value ? dr["name"].ToString() : "";
                    }
                }
            }
            else
            {
                if (node.children != null)
                {
                    foreach (var item in node.children)
                    {
                        setChild((SysMenuTreeBase)item, detailPhid, dt);
                    }
                }
            }
        }

        //public string GetOrgTreeData()
        //{
        //    string detailPhid = System.Web.HttpContext.Current.Request.Params["detailPhid"];
        //    string parentphid = System.Web.HttpContext.Current.Request.Params["parentphid"];
        //    DataTable dt = proxy.GetArgumentByPhid(detailPhid);
        //    Int64 parentorgid = 0;
        //    Int64.TryParse(parentphid, out parentorgid);
        //    //OrgRelatService orgService = new OrgRelatService();

        //    string url = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/i8/SUP/ShortcutMenu/GetShortcutKey";

        //    WebClient wc = new WebClient();
        //    Byte[] pageData = wc.DownloadData(url);

        //    string shortcut = Encoding.UTF8.GetString(pageData);


        //    DMC3.Org.Facade.OrgRelatFacade orgFacade = new DMC3.Org.Facade.OrgRelatFacade();

        //    //IList<TreeJSONBase> orgJson = orgFacade.GetUserLoginOrg(NG3.AppInfoBase.UserID, parentorgid);

        //    //IApplicationContext ctx = ContextRegistry.GetContext();
        //    //return ctx.GetObject<T>(objId);



        //    //foreach (var item in orgJson) {
        //    //    setOrgChild((OrgRelatitemTreeNodeModel)item,detailPhid,dt);
        //    //}

        //    return DataConverterHelper.SerializeObject(orgJson);
        //}

        //private void setOrgChild(OrgRelatitemTreeNodeModel node, string detailPhid, DataTable dt)
        //{
        //    if (node.leaf)
        //    {
        //        if (!string.IsNullOrEmpty(node.OrgId))
        //        {
        //            DataRow dr = dt.Select("id='" + node.OrgId + "'").FirstOrDefault();
        //            if (dr != null && dr["argument"] != DBNull.Value)
        //            {
        //                node.exparams = dr["argument"] != DBNull.Value ? dr["argument"].ToString() : "";
        //            }
        //            if (dr != null && dr["phid"] != DBNull.Value)
        //            {
        //                node.customsort = dr["phid"] != DBNull.Value ? dr["phid"].ToString() : "";
        //            }
        //            if (dr != null && dr["name"] != DBNull.Value)
        //            {
        //                node.cls = dr["name"] != DBNull.Value ? dr["name"].ToString() : "";
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (node.children != null)
        //        {
        //            foreach (var item in node.children)
        //            {
        //                setOrgChild((OrgRelatitemTreeNodeModel)item, detailPhid, dt);
        //            }
        //        }
        //    }
        //}

        public int SaveFunData()
        {
            string gridData = System.Web.HttpContext.Current.Request.Params["gridData"];
            string detailPhid = System.Web.HttpContext.Current.Request.Params["detailPhid"];
            string option_type = System.Web.HttpContext.Current.Request.Params["option_type"];

            //Array arrlist = JsonConvert.DeserializeObject(gridData);
            JArray ja = JArray.Parse(gridData);
            DataTable dt = new DataTable();
            dt.Columns.Add("phid", typeof(Int64));
            dt.Columns.Add("detail_phid", typeof(Int64));
            dt.Columns.Add("type", typeof(string));
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("argument", typeof(string));
            dt.Columns.Add("user_mod_flg", typeof(Int16));
            dt.Columns.Add("sysflg", typeof(Int16));
            if (ja.Count > 0)
            {
                for (int i = 0; i < ja.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    if (ja[i]["phid"] != null && ja[i]["phid"].ToString() != "")
                    {
                        dr["phid"] = Convert.ToInt64(ja[i]["phid"].ToString());
                    }
                    if (ja[i]["id"] != null)
                    {
                        dr["id"] = ja[i]["id"].ToString();
                    }
                    if (ja[i]["name"] != null)
                    {
                        dr["name"] = ja[i]["name"].ToString();
                    }
                    if (ja[i]["argument"] != null)
                    {
                        dr["argument"] = ja[i]["argument"].ToString();
                    }
                    dr["detail_phid"] = detailPhid;
                    dr["type"] = option_type;
                    dr["user_mod_flg"] = 0;
                    dr["sysflg"] = 1;
                    dt.Rows.Add(dr);
                }
            }

            //DataTable dt = DataConverterHelper.ToDataTable(gridData, "select * from fg3_argument_setting");
            //DataTable newDt = dt;
            int iret = proxy.SaveFunData(dt);
            return iret;
        }


        public int SaveOrgData()
        {
            string gridData = System.Web.HttpContext.Current.Request.Params["gridData"];
            string detailPhid = System.Web.HttpContext.Current.Request.Params["detailPhid"];
            string option_type = System.Web.HttpContext.Current.Request.Params["option_type"];

            //Array arrlist = JsonConvert.DeserializeObject(gridData);
            JArray ja = JArray.Parse(gridData);
            DataTable dt = new DataTable();
            dt.Columns.Add("phid", typeof(Int64));
            dt.Columns.Add("detail_phid", typeof(Int64));
            dt.Columns.Add("type", typeof(string));
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("argument", typeof(string));
            dt.Columns.Add("user_mod_flg", typeof(Int16));
            dt.Columns.Add("sysflg", typeof(Int16));
            if (ja.Count > 0)
            {
                for (int i = 0; i < ja.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    if (ja[i]["phid"] != null && ja[i]["phid"].ToString() != "")
                    {
                        dr["phid"] = Convert.ToInt64(ja[i]["phid"].ToString());
                    }
                    if (ja[i]["id"] != null)
                    {
                        dr["id"] = ja[i]["id"].ToString();
                    }
                    if (ja[i]["name"] != null)
                    {
                        dr["name"] = ja[i]["name"].ToString();
                    }
                    if (ja[i]["argument"] != null)
                    {
                        dr["argument"] = ja[i]["argument"].ToString();
                    }
                    dr["detail_phid"] = detailPhid;
                    dr["type"] = option_type;
                    dr["user_mod_flg"] = 0;
                    dr["sysflg"] = 1;
                    dt.Rows.Add(dr);
                }
            }

            //DataTable dt = DataConverterHelper.ToDataTable(gridData, "select * from fg3_argument_setting");
            //DataTable newDt = dt;
            int iret = proxy.SaveFunData(dt);
            return iret;
        }
        //public DataTable jsonToDatatable(string json) {

        //    DataTable dt = new DataTable();

        //}

        class OptionSettingTreeJSONBase : SysMenuTreeBase
        {

            public virtual string newphid { get; set; }
            /// <summary>
            /// 选项值
            /// </summary>
            public virtual string argument { get; set; }
            /// <summary>
            /// 选项值名称
            /// </summary>
            public virtual string option_value_name { get; set; }
        }
    }
}