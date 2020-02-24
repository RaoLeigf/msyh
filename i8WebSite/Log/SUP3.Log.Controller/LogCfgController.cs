#region Summary
/**************************************************************************************
    * 类 名 称：        LogCfgController
    * 命名空间：        SUP3.Log.Controller
    * 文 件 名：        LogCfgController.cs
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
using NG3.Log.Core;

namespace SUP3.Log.Controller
{
	/// <summary>
	/// LogCfg控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class LogCfgController : AFCommonController
    {
        ILogCfgService LogCfgService { get; set; }
        ILogOtherCfgService LogOtherCfgService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public LogCfgController()
	    {
            MultiDelegatingDbProvider.CurrentDbProviderName = LogDbProvider.LogDB;
            LogCfgService = base.GetObject<ILogCfgService>("SUP3.Log.Service.LogCfg");
            LogOtherCfgService = base.GetObject<ILogOtherCfgService>("SUP3.Log.Service.LogOtherCfg");
        }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult LogCfgList()
        {
			base.InitialMultiLanguage("LogCfgList");
            return View("LogCfgList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult LogCfgEdit()
        {
			base.InitialMultiLanguage("LogCfgEdit");
			ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型

            if (ViewBag.OType == "add")
            {
				//新增时
				//if (LogCfgService.Has_BillNoRule("取业务类型") == true)
                //{
                //    var vBillNo = LogCfgService.GetBillNo();//取用户编码,新增时界面上显示
                //    ViewBag.No = vBillNo.BillNoList[0];
                //}
            }

            return View("LogCfgEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetLogCfgList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = LogCfgService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<LogCfgModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 取其它配置的
        /// </summary>
        /// <returns></returns>
        public string GetLogOtherCfgList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = LogOtherCfgService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<LogOtherCfgModel>(result.Results, (Int32)result.TotalItems);
        }
        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetLogCfgInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型

			var findedresult = LogCfgService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {

            //sql数据
            string logcfgGrid = System.Web.HttpContext.Current.Request.Form["logcfgGrid"];

            var datas = JsonToModel.GetModifiedLogCfgModels(logcfgGrid);

            var savedresult = LogCfgService.SaveCfg(datas);

            //refresh 缓存
            if (savedresult.SaveRows > 0)
            {
                NG3LoggerManager.RefreshLogCfg();
            }

            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public string SaveOtherCfg()
        {
            //sql数据
            string logOtherCfgGrid = System.Web.HttpContext.Current.Request.Form["logothercfggrid"];

            var datas = JsonToModel.GetModifiedLogOtherCfgModel(logOtherCfgGrid);

            var savedresult = LogOtherCfgService.SaveOtherCfg(datas);
            //refresh 缓存
            if (savedresult.SaveRows > 0)
            {
                NG3LoggerManager.RefreshLogOtherCfg();
            }

            return DataConverterHelper.SerializeObject(savedresult);
        }

		/// <summary>
        /// 删除数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Delete()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            var deletedresult = LogCfgService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

    }
}

