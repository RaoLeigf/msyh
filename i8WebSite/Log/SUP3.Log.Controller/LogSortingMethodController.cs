#region Summary
/**************************************************************************************
    * 类 名 称：        LogSortingMethodController
    * 命名空间：        SUP3.Log.Controller
    * 文 件 名：        LogSortingMethodController.cs
    * 创建时间：        2017/11/4 
    * 作    者：        洪鹏    
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

using SUP3.Log.Service.Interface;
using SUP3.Log.Model.Domain;
using Spring.Data.Common;

namespace SUP3.Log.Controller
{
	/// <summary>
	/// LogSortingMethod控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class LogSortingMethodController : AFCommonController
    {
        ILogSortingMethodService LogSortingMethodService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public LogSortingMethodController()
	    {
            MultiDelegatingDbProvider.CurrentDbProviderName = LogDbProvider.LogDB;
            LogSortingMethodService = base.GetObject<ILogSortingMethodService>("SUP3.Log.Service.LogSortingMethod");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult LogSortingMethodList()
        {
			base.InitialMultiLanguage("LogSortingMethodList");
            return View("LogSortingMethodList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult LogSortingMethodEdit()
        {
			base.InitialMultiLanguage("LogSortingMethodEdit");
			ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型

            if (ViewBag.OType == "add")
            {
				//新增时
				//if (LogSortingMethodService.Has_BillNoRule("取业务类型") == true)
                //{
                //    var vBillNo = LogSortingMethodService.GetBillNo();//取用户编码,新增时界面上显示
                //    ViewBag.No = vBillNo.BillNoList[0];
                //}
            }

            return View("LogSortingMethodEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetLogSortingMethodList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = LogSortingMethodService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere,new string[] { "InvokeCount desc" });

            return DataConverterHelper.EntityListToJson<LogSortingMethodModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetLogSortingMethodInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型

			var findedresult = LogSortingMethodService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
            string mstformData = System.Web.HttpContext.Current.Request.Form["mstformData"];

            var mstforminfo = DataConverterHelper.JsonToEntity<LogSortingMethodModel>(mstformData);

            var savedresult = LogSortingMethodService.Save<Int64>(mstforminfo.AllRow);

            return DataConverterHelper.SerializeObject(savedresult);
        }

		/// <summary>
        /// 删除数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Delete()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            var deletedresult = LogSortingMethodService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

    }
}

