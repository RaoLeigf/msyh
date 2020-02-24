using System;
using System.Web.Mvc;
using System.Web.SessionState;
using Enterprise3.Common.Model;
using NG3.Aop.Transaction;
using NG3.Web.Controller;
using SUP.Frame.Facade;
using System.Data;
using SUP.Common.Base;
using Enterprise3.Common.Base.Helpers;
using NG3.Data.Service;
using Enterprise3.WebApi.Client.Enums;
using Enterprise3.WebApi.Client;

namespace SUP.Frame.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class HrRightApplyController : AFController
    {

        private IHrRightApplyFacade proxy;

        public HrRightApplyController()
        {
            proxy = AopObjectProxy.GetObject<IHrRightApplyFacade>(new HrRightApplyFacade());
        }

        public ActionResult List()
        {
            return View("HrRightApplyList");
        }

        public ActionResult Edit()
        {
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型
            ViewBag.wfOType = System.Web.HttpContext.Current.Request.Params["wfotype"];
            
            //工作流处理
            if (!string.IsNullOrEmpty(ViewBag.wfOType) && ViewBag.OType == "view")
            {
                var wfPiid = System.Web.HttpContext.Current.Request.Params["wfpiid"];
                var wfTaskId = System.Web.HttpContext.Current.Request.Params["wftaskid"];
                ViewBag.Title = "员工权限申请-审核";

                try
                {
                    var client = new WebApiClient(ServiceURL, ApiReqParam, EnumDataFormat.Json);

                    var param = new ParameterCollection();
                    param.Add("piid", wfPiid);
                    param.Add("taskid", wfTaskId);
                    param.Add("wfotype", ViewBag.wfOType);

                    var res = client.Get<string>("api/workflow3/WorkFlowApi/GetFlowExecutionInfo", param);
                    ViewBag.WorkFlowInfo = res.Data;
                }
                catch (SystemException ex)
                {
                    throw ex;
                }

                ViewBag.Phid = System.Web.HttpContext.Current.Request.Params["id"];               
                DataTable dt = proxy.GetHrRightApplyDt(ViewBag.Phid);
                ViewBag.BillNo = dt.Rows[0]["billno"];
                ViewBag.BillName = dt.Rows[0]["billname"];
                ViewBag.FillPsnId = dt.Rows[0]["fillpsnid"];
                ViewBag.FillPsnName = dt.Rows[0]["fillpsnname"];
                ViewBag.FillDate = dt.Rows[0]["filldate"];
                ViewBag.Remark = dt.Rows[0]["remark"];
            }
            else
            {
                if (ViewBag.OType == "add")
                {
                    string hrid = proxy.GetUserHrid(NG3.AppInfoBase.UserID.ToString());
                    if (string.IsNullOrEmpty(hrid) || hrid == "0")
                    {
                        HttpContext.Response.Redirect("~/SUP/ErrorPage?msg=操作员没有对应员工,请设置！"); //重定向到错误页面
                    }
                    ViewBag.Hrid = hrid;

                    ResBillNoOrIdEntity en = SUP.Common.Rule.CommonUtil.GetBillNo("HrRightApply");
                    ViewBag.BillNo = en.BillNoList[0];
                    if (en.BillIdList != null)
                    {
                        ViewBag.Phid = en.BillIdList[0];
                    }
                }
                else if (ViewBag.OType == "edit" || ViewBag.OType == "view")
                {
                    ViewBag.Phid = System.Web.HttpContext.Current.Request.Params["phid"];
                    ViewBag.BillNo = System.Web.HttpContext.Current.Request.Params["billno"];
                    ViewBag.BillName = System.Web.HttpContext.Current.Request.Params["billname"];
                    ViewBag.FillPsnId = System.Web.HttpContext.Current.Request.Params["fillpsnid"];
                    ViewBag.FillPsnName = System.Web.HttpContext.Current.Request.Params["fillpsnname"];
                    ViewBag.FillDate = System.Web.HttpContext.Current.Request.Params["filldate"];
                    ViewBag.Remark = System.Web.HttpContext.Current.Request.Params["remark"];
                    ViewBag.Hrid = proxy.GetUserHrid(ViewBag.FillPsnId);
                }
            }

            return View("HrRightApplyEdit");
        }

        public string GetHrRightApply()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            DataTable dt = proxy.GetHrRightApply(clientJsonQuery);
            int totalRecord = dt.Rows.Count;
            return DataConverterHelper.ToJson(dt, totalRecord);
        }

        public string GetApplicantInfo(string hrids, string billno)
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(hrids))
            {
                dt = proxy.GetApplicantInfo(hrids);
            }
            else if(!string.IsNullOrEmpty(billno))
            {
                dt = proxy.GetEditApplicantInfo(billno);
            }

            int totalRecord = dt.Rows.Count;
            return DataConverterHelper.ToJson(dt, totalRecord);
        }

        public string GetOrgInfo(string userid, string billno)
        {
            DataTable dt = proxy.GetOrgInfo(userid, billno);
            int totalRecord = dt.Rows.Count;
            return DataConverterHelper.ToJson(dt, totalRecord);
        }

        public string GetRoleInfo(string fillpsnid, string userid, string orgid, string billno)
        {
            DataTable dt = proxy.GetRoleInfo(fillpsnid, userid, orgid, billno);
            int totalRecord = dt.Rows.Count;
            return DataConverterHelper.ToJson(dt, totalRecord);
        }

        public bool SaveHrRightApply(string phid, string billno, string billname, string remark, string applicantObj, string orgObj, string roleObj, string otype, string removeApplicant)
        {
            return proxy.SaveHrRightApply(phid, billno, billname, remark, applicantObj, orgObj, roleObj, otype, removeApplicant);
        }

        public bool DeleteHrRightApply(string billno)
        {
            return proxy.DeleteHrRightApply(billno);
        }

        public string GetHrRightApplyPrintInfo(string billno)
        {
            DataTable dt = proxy.GetHrRightApplyPrintInfo(billno);
            int totalRecord = dt.Rows.Count;
            return DataConverterHelper.ToJson(dt, totalRecord);
        }

        #region 工作流
        /// <summary>
        /// 服务地址
        /// </summary>
        private static string _serviceUrl = string.Empty;
        public string ServiceURL
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_serviceUrl))
                    return _serviceUrl;

                string infoStr = string.Empty;

                try
                {
                    infoStr = DbHelper.GetString("select info_str from fg_info where sysno = 'i6EcUrl'");

                    if (string.IsNullOrWhiteSpace(infoStr))
                        infoStr = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" +
                                  System.Web.HttpContext.Current.Request.Url.Port +
                                  (string.IsNullOrWhiteSpace(System.Web.HttpContext.Current.Request.ApplicationPath)
                                      ? "" : System.Web.HttpContext.Current.Request.ApplicationPath);
                }
                catch (Exception e)
                {
                    //此处应添加日志
                    throw new Exception("ServiceURL: " + e.Message);
                }

                if (string.IsNullOrWhiteSpace(infoStr))
                    throw new Exception("ServiceURL: url为空");

                _serviceUrl = infoStr;

                return _serviceUrl;
            }
        }

        /// <summary>
        /// 调用服务必要的请求参数对象
        /// </summary>
        public Enterprise3.WebApi.Client.Models.AppInfoBase ApiReqParam
        {
            get
            {
                return new Enterprise3.WebApi.Client.Models.AppInfoBase
                {
                    AppKey = ConfigHelper.GetString("AppKey", "D31B7F91-3068-4A49-91EE-F3E13AE5C48C"), //必须
                    AppSecret = ConfigHelper.GetString("AppSecret", "103CB639-840C-4E4F-8812-220ECE3C4E4D"), //必须
                    DbServerName = NG3.AppInfoBase.DbServerName,
                    OCode = NG3.AppInfoBase.OCode,
                    OrgName = NG3.AppInfoBase.OrgName,
                    SessionKey = NG3.AppInfoBase.SessionID,
                    TokenKey = string.Empty,
                    DbName = NG3.AppInfoBase.DbName,
                    UserKey = NG3.AppInfoBase.LoginID,
                    UserName = NG3.AppInfoBase.UserName,
                    UserId = NG3.AppInfoBase.UserID,
                    OrgId = NG3.AppInfoBase.OrgID,
                };
            }
        }
        #endregion

    }
}
