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

namespace SUP.Frame.Controller
{
    public class SchemeAllocationController : AFController
    {
        private ISchemeAllocationFacade proxy;

        /// <summary>
        /// 方案分配
        /// </summary>
        /// <returns></returns>
        public SchemeAllocationController()
        {
            proxy = AopObjectProxy.GetObject<ISchemeAllocationFacade>(new SchemeAllocationFacade());
        }
        public ActionResult SchemeAllocation()
        {
            return View("SchemeAllocation");
        }

        /// <summary>
        /// 获取方案分配grid列表
        /// </summary>
        /// <returns></returns>
        public string GetUserSchemeAllocation()
        {
            string code = System.Web.HttpContext.Current.Request.Params["code"];
            string name = System.Web.HttpContext.Current.Request.Params["name"];

            string result = string.Empty;
            DataTable dt = new DataTable();
            dt = proxy.GetUserSchemeAllocation();
            if (!string.IsNullOrEmpty(code))
            {
                DataRow[] drs = dt.Select("userno like '%" + code + "%'");
                if (drs.Length > 0)
                    dt = drs.CopyToDataTable();
            }
            if (!string.IsNullOrEmpty(name))
            {
                DataRow[] drs = dt.Select("username like '%" + name + "%'");
                if (drs.Length > 0)
                    dt = drs.CopyToDataTable();
            }

            int totalRecord = dt != null ? dt.Rows.Count : 0;
            result = DataConverterHelper.ToJson(dt, totalRecord);
            return result;
        }

        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        public string GetRoleList()
        {
            string result = string.Empty;
            DataTable dt = new DataTable();
            dt = proxy.GetRoleList();
            int totalRecord = dt != null ? dt.Rows.Count : 0;
            result = DataConverterHelper.ToJson(dt, totalRecord);
            return result;
        }

        /// <summary>
        /// 操作员列表
        /// </summary>
        /// <returns></returns>
        public string GetUserList()
        {
            string result = string.Empty;
            DataTable dt = new DataTable();
            dt = proxy.GetUserList();
            int totalRecord = dt != null ? dt.Rows.Count : 0;
            result = DataConverterHelper.ToJson(dt, totalRecord);
            return result;
        }

        /// <summary>
        /// 保存方案分配
        /// </summary>
        /// <returns></returns>
        public string SaveUserSchemeAllocation()
        {
            string oriphid = System.Web.HttpContext.Current.Request.Params["oriphid"];
            string oriuserid = System.Web.HttpContext.Current.Request.Params["oriuserid"];
            string oriusertype = System.Web.HttpContext.Current.Request.Params["oriusertype"];
            string userid = System.Web.HttpContext.Current.Request.Params["userid"];
            string usertype = System.Web.HttpContext.Current.Request.Params["usertype"];

            string result = string.Empty;
            result = proxy.SaveUserSchemeAllocation(oriuserid, oriusertype, userid, usertype);
            return result;
        }

        /// <summary>
        /// 删除方案分配
        /// </summary>
        /// <returns></returns>
        public string DeleteUserSchemeAllocation()
        {
            string phid = System.Web.HttpContext.Current.Request.Params["phid"];

            string result = string.Empty;
            result = proxy.DeleteUserSchemeAllocation(phid);
            return result;
        }
    }
}
