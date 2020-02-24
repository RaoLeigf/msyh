#region Summary
/**************************************************************************************
    * 类 名 称：        LogSortingSlowPerfController
    * 命名空间：        SUP3.Log.Controller
    * 文 件 名：        LogSortingSlowPerfController.cs
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
	/// LogSortingSlowPerf控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class LogSortingSlowPerfController : AFCommonController
    {
        ILogSortingSlowPerfService LogSortingSlowPerfService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public LogSortingSlowPerfController()
	    {
            MultiDelegatingDbProvider.CurrentDbProviderName = LogDbProvider.LogDB;
            LogSortingSlowPerfService = base.GetObject<ILogSortingSlowPerfService>("SUP3.Log.Service.LogSortingSlowPerf");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult LogSortingSlowPerfList()
        {
			base.InitialMultiLanguage("LogSortingSlowPerfList");
            return View("LogSortingSlowPerfList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult LogSortingSlowPerfEdit()
        {
			base.InitialMultiLanguage("LogSortingSlowPerfEdit");
			ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型

            if (ViewBag.OType == "add")
            {
				//新增时
				//if (LogSortingSlowPerfService.Has_BillNoRule("取业务类型") == true)
                //{
                //    var vBillNo = LogSortingSlowPerfService.GetBillNo();//取用户编码,新增时界面上显示
                //    ViewBag.No = vBillNo.BillNoList[0];
                //}
            }

            return View("LogSortingSlowPerfEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetLogSortingSlowPerfList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = LogSortingSlowPerfService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere, new string[] { "Duration desc" });

            return DataConverterHelper.EntityListToJson<LogSortingSlowPerfModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetLogSortingSlowPerfInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型

			var findedresult = LogSortingSlowPerfService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
            string mstformData = System.Web.HttpContext.Current.Request.Form["mstformData"];

            var mstforminfo = DataConverterHelper.JsonToEntity<LogSortingSlowPerfModel>(mstformData);

            var savedresult = LogSortingSlowPerfService.Save<Int64>(mstforminfo.AllRow);

            return DataConverterHelper.SerializeObject(savedresult);
        }

		/// <summary>
        /// 删除数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Delete()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            var deletedresult = LogSortingSlowPerfService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

    }
}

