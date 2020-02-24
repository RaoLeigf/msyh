using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NG3;
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
using System.Web.SessionState;

namespace SUP.Frame.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class BillCodeRuleController : AFController
    {
        private IBillCodeRuleFacade facade;

        public BillCodeRuleController()
        {
            facade = AopObjectProxy.GetObject<IBillCodeRuleFacade>(new BillCodeRuleFacade());
        }

        public ActionResult BillCodeRule()
        {
            return View("BillCodeRule");
        }

        public ActionResult BillCodeRuleEdit()
        {
            ViewBag.C_bcode = System.Web.HttpContext.Current.Request.Params["c_bcode"];
             string c_code= System.Web.HttpContext.Current.Request.Params["ruletype"];
            ViewBag.RuleType = facade.GetTypeName(c_code);
            ViewBag.BusName = System.Web.HttpContext.Current.Request.Params["busname"];
            ViewBag.CodeLimit = Int32.Parse(System.Web.HttpContext.Current.Request.Params["codelimit"]);
            //string str = System.Web.HttpContext.Current.Request.Params["delimiter"];
            //string delimiter = string.Empty;
            //switch (str)
            //{
            //    case "无":delimiter = "无";
            //        break;
            //    case "shortbar":delimiter = "-";
            //        break;
            //    case "point":delimiter = ".";
            //        break;
            //    case "underline":delimiter = "_";
            //        break;
            //    case "slash":delimiter = "/";
            //        break;
            //}
            ViewBag.Delimiter = System.Web.HttpContext.Current.Request.Params["delimiter"];
            ViewBag.CodeMode = System.Web.HttpContext.Current.Request.Params["codemode"];
            ViewBag.C_btype = System.Web.HttpContext.Current.Request.Params["c_btype"];
            ViewBag.BusPhid = System.Web.HttpContext.Current.Request.Params["busphid"];
            string loginid = AppInfoBase.LoginID;
            long orgid = AppInfoBase.OrgID;
            DataTable dt = facade.GetInfo(loginid, orgid);
            ViewBag.LoginID = loginid;
            ViewBag.Deptno = dt.Rows[0]["deptno"];
            ViewBag.Ocode = dt.Rows[0]["ocode"];
            ViewBag.CodeValue = dt.Rows[0]["codevalue"];
            return View("BillCodeRuleEdit");
        }

        /// <summary>
        /// 获取编码规则页面列表
        /// </summary>
        /// <returns></returns>
        public string GetBillCodeRuleList()
        {
            try
            {
                string c_btype = System.Web.HttpContext.Current.Request.Params["c_btype"];
                if (string.IsNullOrEmpty(c_btype))
                {
                    c_btype = "-1";
                }
                return JsonConvert.SerializeObject(facade.GetBillCodeRuleList(c_btype));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取编码规则设置页面列表
        /// </summary>
        /// <returns></returns>
        public string GetBillCodeRuleDetailList()
        {
            try
            {
                string c_bcode = System.Web.HttpContext.Current.Request.Params["c_bcode"];
                DataTable dt = facade.GetBillCodeRuleDetailList(c_bcode);
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["codeitem"].ToString() == "systeminfo")
                    {
                        switch (dr["codeitemcontent"].ToString())
                        {
                            case "operatorno":
                                dr["codeitemcontent"] = "当前操作员编码";
                                break;
                            case "deptno":
                                dr["codeitemcontent"] = "当前操作员所在的部门编码";
                                break;
                            case "ocode":
                                dr["codeitemcontent"] = "当前组织号";
                                break;
                            case "codevalue":
                                dr["codeitemcontent"] = "当前组织简码";
                                break;
                        }
                    }
                }
                    return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取编码项为单据信息时获取帮助信息
        /// </summary>
        /// <returns></returns>
        public string GetBillInfoHelp()
        {
            try
            {
                string containerid = System.Web.HttpContext.Current.Request.Params["c_btype"];
                return JsonConvert.SerializeObject(facade.GetBillInfoHelp(containerid));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取toolkit注册的业务类型
        /// </summary>
        /// <returns></returns>
        public string GetBillType()
        {
            try
            {
                string busphid = System.Web.HttpContext.Current.Request.Params["busphid"];
                return JsonConvert.SerializeObject(facade.GetBillType(busphid));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存编码规则
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            try
            {
                string data = System.Web.HttpContext.Current.Request.Params["griddata"];
                string c_btype = System.Web.HttpContext.Current.Request.Params["treetype"];
                string deletedata = System.Web.HttpContext.Current.Request.Params["deletedata"];
                DataTable dt = DataConverterHelper.ToDataTable(data, "select * from c_pfc_billnorule_m");
                DataTable deldt = new DataTable();
                deldt.Columns.Add("key",Type.GetType("System.String"));
                if (!string.IsNullOrEmpty(deletedata))
                {
                    var del = JArray.Parse(JsonConvert.DeserializeObject(deletedata).ToString());
                    foreach (var ss in del)
                    {
                        DataRow deldr = deldt.NewRow();
                        deldr["key"] = ss["row"]["key"];
                        deldt.Rows.Add(deldr);
                    }
                }
                return facade.Save(dt, c_btype, deldt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存编码规则明细
        /// </summary>
        /// <returns></returns>
        public int SaveDetails()
        {
            try
            {
                string loginid = AppInfoBase.LoginID;
                long orgid = AppInfoBase.OrgID;
                string data = System.Web.HttpContext.Current.Request.Params["griddata"];
                string c_bcode = System.Web.HttpContext.Current.Request.Params["c_bcode"];
                DataTable dt = DataConverterHelper.ToDataTable(data, "select * from c_pfc_billnorule_d");
                for (int i = 0; i< dt.Rows.Count; i++)
                {
                    dt.Rows[i]["c_bcode"] = c_bcode;
                    if (dt.Rows[i]["codeitem"].ToString() == "systeminfo")
                    {
                        switch (dt.Rows[i]["codeitemcontent"].ToString())
                        {
                            case "当前操作员编码":
                                dt.Rows[i]["codeitemcontent"] = "operatorno";
                                break;
                            case "当前操作员所在的部门编码":
                                dt.Rows[i]["codeitemcontent"] = "deptno";
                                break;
                            case "当前组织号":
                                dt.Rows[i]["codeitemcontent"] = "ocode";
                                break;
                            case "当前组织简码":
                                dt.Rows[i]["codeitemcontent"] = "codevalue";
                                break;
                        }
                    }
                }
                return facade.SaveDetails(dt, loginid, orgid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 保存对应组织的方案
        /// </summary>
        /// <returns></returns>
        public object SaveRuleDistribution()
        {
            try
            {
                string data = System.Web.HttpContext.Current.Request.Params["data"];
                string billRule_m_code = System.Web.HttpContext.Current.Request.Params["billRule_m_code"];
                string[] strArray = JsonConvert.DeserializeObject<string[]>(data);
                if (strArray.Length > 0)
                {
                    strArray = data.Split(',');
                }
                #region 构建新增dt
                DataTable dt = new DataTable();
                dt.Columns.Add("phid", Type.GetType("System.Int64"));
                dt.Columns.Add("billrule_m_code", Type.GetType("System.String"));
                dt.Columns.Add("org_code", Type.GetType("System.String"));
                #endregion
                #region 构建过滤dt
                DataTable checkdt = new DataTable();
                checkdt.Columns.Add("code", Type.GetType("System.String"));
                #endregion
                return JsonConvert.SerializeObject(facade.SaveRuleDistribution(dt, checkdt, strArray, billRule_m_code));
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
