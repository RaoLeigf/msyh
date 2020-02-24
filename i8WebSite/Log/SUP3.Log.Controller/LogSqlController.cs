#region Summary
/**************************************************************************************
    * 类 名 称：        LogSqlController
    * 命名空间：        SUP3.Log.Controller
    * 文 件 名：        LogSqlController.cs
    * 创建时间：        2017/10/9 
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
using System.Diagnostics;
using NG3.Log.Core;

namespace SUP3.Log.Controller
{
	/// <summary>
	/// LogSql控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class LogSqlController : AFCommonController
    {
        ILogSqlService LogSqlService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public LogSqlController()
	    {
            NG3LoggerManager.Info(typeof(LogSqlController), "LogController", "invoke time:" + Convert.ToString(DateTime.Now));
            MultiDelegatingDbProvider.CurrentDbProviderName = LogDbProvider.LogDB;
            Stopwatch watch = Stopwatch.StartNew();
            LogSqlService = base.GetObject<ILogSqlService>("SUP3.Log.Service.LogSql");
            NG3LoggerManager.Info(typeof(LogSqlController), "LogController", "elapsed time:" + watch.ElapsedMilliseconds);
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult LogSqlList()
        {
			base.InitialMultiLanguage("LogSqlList");
            return View("LogSqlList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult LogSqlEdit()
        {
			base.InitialMultiLanguage("LogSqlEdit");
			ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型

            if (ViewBag.OType == "add")
            {
				//新增时
				//if (LogSqlService.Has_BillNoRule("取业务类型") == true)
                //{
                //    var vBillNo = LogSqlService.GetBillNo();//取用户编码,新增时界面上显示
                //    ViewBag.No = vBillNo.BillNoList[0];
                //}
            }

            return View("LogSqlEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetLogSqlList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            string sortbyparm = System.Web.HttpContext.Current.Request.Params["sortby"];
            if (string.IsNullOrEmpty(sortbyparm)) sortbyparm = "2";
            string[] sortby = null;
            if(sortbyparm == "0")
            {
                sortby = new string[] { "Duration Asc"};
            }
            else if (sortbyparm=="1")
            {
                sortby = new string[] { "Duration Desc" };
            }
            else if(sortbyparm=="2")
            {
                sortby = new string[] { "Phid Asc" };
            }
            else if(sortbyparm=="3")
            {
                sortby = new string[] { "Phid Desc" };
            }
                
            var result = LogSqlService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere,sortby);

            return DataConverterHelper.EntityListToJson<LogSqlModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetLogSqlInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型

			var findedresult = LogSqlService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
            string mstformData = System.Web.HttpContext.Current.Request.Form["mstformData"];

            var mstforminfo = DataConverterHelper.JsonToEntity<LogSqlModel>(mstformData);

            var savedresult = LogSqlService.Save<Int64>(mstforminfo.AllRow);

            return DataConverterHelper.SerializeObject(savedresult);
        }

		/// <summary>
        /// 删除数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Delete()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            var deletedresult = LogSqlService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

    }
}

