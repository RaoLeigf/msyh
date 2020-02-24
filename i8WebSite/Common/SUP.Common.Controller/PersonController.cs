using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Web.Controller;
using SUP.Common.Facade;
using NG3.Aop.Transaction;
using System.Web.Mvc;
using NG3.Web.Mvc;
using System.Data;
using SUP.Common.Base;
using SUP.Common.DataEntity;

namespace SUP.Common.Controller
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class PersonController : AFController
    {
        private IPersonFacade proxy;
        public PersonController()
        {
            proxy = AopObjectProxy.GetObject<IPersonFacade>(new PersonFacade());
        }

        #region 获取人员列表数据
        /// <summary>
        /// 获取员工列表数据
        /// </summary>
        /// <returns></returns>
        public string GetEmpList()
        {
            string limit = System.Web.HttpContext.Current.Request.Params["limit"];
            string page = System.Web.HttpContext.Current.Request.Params["page"];
            string sqlFilter = System.Web.HttpContext.Current.Request.Params["sqlfilter"];  //组件定义时传入的默认查询串
            string searchTxt = System.Web.HttpContext.Current.Request.Params["searchtxt"];  //输入的查询串
            string empstatus = System.Web.HttpContext.Current.Request.Params["empstatus"];  //员工状态
            string emptype = System.Web.HttpContext.Current.Request.Params["emptype"];      //员工类型
            string partMark = System.Web.HttpContext.Current.Request.Params["partmark"];    //显示兼职
            string proyMark = System.Web.HttpContext.Current.Request.Params["proymark"];    //显示代理

            string oCode = System.Web.HttpContext.Current.Request.Params["ocode"];          //当前节点
            string leaf = System.Web.HttpContext.Current.Request.Params["leaf"];            //是否叶子节点
            string relatIndex = System.Web.HttpContext.Current.Request.Params["relatindex"];//节点关系索引

            string and = " and ", assigntypeFilter = "assigntype is null or assigntype='0'";
            if (string.IsNullOrEmpty(sqlFilter))
            {
                and = "";
                sqlFilter = "";
            }
            if (!string.IsNullOrEmpty(searchTxt))  //查询串
            {
                sqlFilter += and + searchTxt;
                and = " and ";
            }
            if (!string.IsNullOrEmpty(empstatus))
            {
                sqlFilter += and + string.Format("hr_epm_main.empstatus='{0}'", empstatus); //员工状态
                and = " and ";
            }
            if (!string.IsNullOrEmpty(emptype))
            {
                sqlFilter += and + string.Format("hr_epm_main.emptype='{0}'", emptype); //员工类型
                and = " and ";
            }
            if (!string.IsNullOrEmpty(partMark)) //显示兼职
            {
                assigntypeFilter += " or assigntype='1'";
            }
            if (!string.IsNullOrEmpty(proyMark)) //显示代理
            {
                assigntypeFilter += " or assigntype='2'";
            }

            sqlFilter += and + "(" + assigntypeFilter + ")";
            int totalRecord = 0, pageSize = 20, pageIndex = 0;
            int.TryParse(limit, out pageSize);
            int.TryParse(page, out pageIndex);

            DataTable tmpDT = proxy.GetEmpList(sqlFilter, pageSize, pageIndex, ref totalRecord, oCode, leaf == "true", relatIndex);
            string str = DataConverterHelper.ToJson(tmpDT, totalRecord);
            return str;
        }

        /// <summary>
        /// 通过不同分组获取人员数据
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        private string GetJsonList(string group)
        {
            System.Web.HttpRequest request = System.Web.HttpContext.Current.Request;

            string lgSign = request.Params["lgsign"];        //显示禁用

            string sqlFilter = request.Params["searchtxt"];  //输入的查询串
            string oCode = request.Params["ocode"];          //当前节点
            string leaf = request.Params["leaf"];            //是否叶子节点

            string limit = request.Params["limit"]; //每页显示数
            string page = request.Params["page"];  //当前索引

            string defaultFilter = "";

            switch (group)
            {
                case "ugroup":
                case "actor":
                    {
                        if (string.IsNullOrEmpty(lgSign) || lgSign == "0") //默认不显示禁用用户
                        {
                            defaultFilter = "secuser.lg_sign='1'";
                        }
                    }
                    break;
                default:
                    break;
            }

            int totalRecord = 0, pageSize = 20, pageIndex = 0;
            int.TryParse(limit, out pageSize);
            int.TryParse(page, out pageIndex);
            DataTable tmpDT = proxy.GetDataTable(defaultFilter, sqlFilter, pageSize, pageIndex, ref totalRecord, oCode, leaf == "true", group);
            string str = DataConverterHelper.ToJson(tmpDT, totalRecord);
            return str;
        }

        /// <summary>
        /// 获取人员列表数据
        /// </summary>
        /// <returns></returns>
        public string GetPersonList()
        {
            System.Web.HttpRequest request = System.Web.HttpContext.Current.Request;
            string getType = request.Params["gettype"];
            string group = request.Params["group"]; //用户组(ugroup)、角色(actor)、自定义联系人(selfgroup)、在线人员(online)、外部人员(outer)

            string returnValue = "{totalRows:0,Record:[]}";
            if (string.IsNullOrEmpty(getType)) { return returnValue; }
            switch (getType)
            {
                case "emplist":
                    returnValue = GetEmpList();
                    break;
                case "userlist":
                case "onlinelist":
                case "selfgrouplist":
                case "outerlist":
                    returnValue = GetJsonList(group); ;
                    break;
                default:
                    break;
            }
            return returnValue;
        }
        #endregion

        #region 加载树
        /// <summary>
        /// 获取树
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadTree()
        {
            string ctype = System.Web.HttpContext.Current.Request.Params["ctype"];
            string nodeid = System.Web.HttpContext.Current.Request.Params["node"];
            switch (ctype)
            {
                case "hr":
                    return LoadHrTree();
                case "actor":
                    return LoadActorTree();
                case "ugroup":
                    return LoadUGroupTree();
                case "selfgroup":
                    return LoadSelfGroupTree();
                case "outer":
                    return LoadOuterTree();
            }
            return null;
        }

        /// <summary>
        /// 加载人力资源树
        /// </summary>
        /// <returns></returns>
        private JsonResult LoadHrTree()
        {
            string nodeid = System.Web.HttpContext.Current.Request.Params["node"];
            IList<TreeJSONBase> list = proxy.LoadHrTree(nodeid);
            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 加载角色树
        /// </summary>
        /// <returns></returns>
        private JsonResult LoadActorTree()
        {
            string nodeid = System.Web.HttpContext.Current.Request.Params["node"];
            IList<TreeJSONBase> list = proxy.LoadActorTree(nodeid);
            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 加载用户组树
        /// </summary>
        /// <returns></returns>
        private JsonResult LoadUGroupTree()
        {
            string nodeid = System.Web.HttpContext.Current.Request.Params["node"];
            IList<TreeJSONBase> list = proxy.LoadUGroupTree(nodeid);
            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 加载自定义联系人组树
        /// </summary>
        /// <returns></returns>
        private JsonResult LoadSelfGroupTree()
        {
            IList<TreeJSONBase> list = proxy.LoadSelfGroupTree(NG3.AppInfoBase.LoginID);
            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 加载外部人员分组树
        /// </summary>
        /// <returns></returns>
        private JsonResult LoadOuterTree()
        {
            string product = System.Web.HttpContext.Current.Request.Params["product"];
            IList<TreeJSONBase> list = proxy.LoadOuterTree(string.IsNullOrEmpty(product) ? "i6p" : product);
            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 获取员工状态、类型
        /// <summary>
        /// 获取员工状态
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string GetEmpStatus()
        {
            DataTable tmpDT = proxy.GetDT("hr_base_enum a left join  hr_epm_status_property b on a.ccode=b.ccode", "a.ccode,a.cname", "a.ctype='empstatus' and b.isuse='1'", "a.ccode");
            string str = DataConverterHelper.ToJson(tmpDT, tmpDT.Rows.Count);
            return str;
        }

        /// <summary>
        /// 获取员工类型
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string GetEmpType()
        {
            DataTable tmpDT = proxy.GetDT("pr_emptype", "emptype,typename", "", "emptype");
            string str = DataConverterHelper.ToJson(tmpDT, tmpDT.Rows.Count);
            return str;
        }
        #endregion

        #region 树节点记忆功能处理
        /// <summary>
        /// 获取树节点的记忆状态
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult GetTreeMemoryInfo()
        {
            string type = System.Web.HttpContext.Current.Request.Params["type"];
            string busstype = System.Web.HttpContext.Current.Request.Params["busstype"];
            TreeMemoryEntity treeMemoryEntity = proxy.GetTreeMemory(GetTreeMemoryType(type), string.IsNullOrEmpty(busstype) ? "all" : busstype);
            return this.Json(treeMemoryEntity, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 更新树节点的记忆状态
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public void UpdataTreeMemory()
        {
            string type = System.Web.HttpContext.Current.Request.Params["type"];
            string busstype = System.Web.HttpContext.Current.Request.Params["busstype"];
            string FoucedNodeValue = System.Web.HttpContext.Current.Request.Params["foucednodevalue"];
            string IsMemo = System.Web.HttpContext.Current.Request.Params["ismemo"];
            TreeMemoryEntity treeMemoryEntity = new TreeMemoryEntity(NG3.AppInfoBase.LoginID, NG3.AppInfoBase.OCode, GetTreeMemoryType(type), string.IsNullOrEmpty(busstype) ? "all" : busstype);
            treeMemoryEntity.FoucedNodeValue = FoucedNodeValue;
            treeMemoryEntity.IsMemo = IsMemo == "true";
            proxy.UpdataTreeMemory(treeMemoryEntity);
        }

        /// <summary>
        /// 获取树的枚举类型值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private TreeMemoryType GetTreeMemoryType(string type)
        {
            switch (type)
            {
                case "empsingle":
                    return TreeMemoryType.EmpSingleHelp;
                case "empmulti":
                    return TreeMemoryType.EmpMultiHelp;
                case "opsingle":
                    return TreeMemoryType.OpSingleHelp;
                case "opmulti":
                    return TreeMemoryType.OpMultiHelp;
                case "person_hr":
                    return TreeMemoryType.HrTree;
                case "person_actor":
                    return TreeMemoryType.ActorTree;
                case "person_ugroup":
                    return TreeMemoryType.UGroupTree;
                case "person_selfgroup":
                    return TreeMemoryType.SelfGroupTree;
                case "person_online":
                    return TreeMemoryType.OnLineTree;
                case "person_outer":
                    return TreeMemoryType.OuterTree;
                default:
                    return TreeMemoryType.Other;
            }
        }
        #endregion
    }
}
