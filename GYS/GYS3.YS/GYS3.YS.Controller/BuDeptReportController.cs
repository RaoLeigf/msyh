#region Summary
/**************************************************************************************
    * 类 名 称：        GHProjDtlShareController
    * 命名空间：        GQT3.QT.Controller
    * 文 件 名：        GHProjDtlShareController.cs
    * 创建时间：        2018/9/11 
    * 作    者：        李明    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using NG3.Web.Mvc;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Controller;

using GYS3.YS.Service.Interface;
using GYS3.YS.Model.Extend;

namespace GYS3.YS.Controller
{
	/// <summary>
	/// GHProjDtlShare控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class BuDeptReportController : AFCommonController
    {
        IBuDeptReportService BuDeptReportService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public BuDeptReportController()
	    {
            BuDeptReportService = base.GetObject<IBuDeptReportService>("GYS3.YS.Service.BuDeptReport");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult BuDeptReportList()
        {
			ViewBag.Title = base.GetMenuLanguage("GHBudgetDepartment");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "部门预算分解表";
            }
            base.InitialMultiLanguage("GHBudgetDepartment");
            ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetDepartment");
            return View("BuDeptReportList");
        }
        	
		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetBuDeptReportList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            //var result = BuDeptReportService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);
            var result = BuDeptReportService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, "GHBuDeptReport", dicWhere, false);
            
            return DataConverterHelper.EntityListToJson<BuDeptReportModel>(result.Results, (Int32)result.TotalItems);
        }


        /// <summary>
        /// 指向工会经费收支预算表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult BuIncomeExpendReportList()
        {
            ViewBag.Title = base.GetMenuLanguage("GHBudgetIncomeExpend");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "工会经费收支预算表";
            }
            base.InitialMultiLanguage("GHBudgetIncomeExpend");
            ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetIncomeExpend");
            return View("BuIncomeExpendReportList");
        }

        /// <summary>
        /// 取工会经费收支预算表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetBuIncomeExpendReportList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();            
            var result = BuDeptReportService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, "GHBudgetIncomeExpend", dicWhere, false);

            return DataConverterHelper.EntityListToJson<BuDeptReportModel>(result.Results, (Int32)result.TotalItems);
        }


        /// <summary>
        /// 指向预算执行情况表
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult BuImplementReportList()
        {
            ViewBag.Title = base.GetMenuLanguage("GHBudgetImplement");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "预算执行情况表";
            }
            base.InitialMultiLanguage("GHBudgetImplement");
            ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetImplement");
            return View("BuImplementReportList");
        }

        /// <summary>
        /// 取预算执行情况表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetBuImplementReportList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = BuDeptReportService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, "GHBudgetImplement", dicWhere, false);

            return DataConverterHelper.EntityListToJson<BuDeptReportModel>(result.Results, (Int32)result.TotalItems);
        }
    }
}

