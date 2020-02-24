#region Summary
/**************************************************************************************
    * 类 名 称：        LogOtherCfgController
    * 命名空间：        SUP3.Log.Controller
    * 文 件 名：        LogOtherCfgController.cs
    * 创建时间：        2017/10/16 
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

namespace SUP3.Log.Controller
{
	/// <summary>
	/// LogOtherCfg控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class LogOtherCfgController : AFCommonController
    {
        ILogOtherCfgService LogOtherCfgService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public LogOtherCfgController()
	    {
	        LogOtherCfgService = base.GetObject<ILogOtherCfgService>("SUP3.Log.Service.LogOtherCfg");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult LogOtherCfgList()
        {
			base.InitialMultiLanguage("LogOtherCfgList");
            return View("LogOtherCfgList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult LogOtherCfgEdit()
        {
			base.InitialMultiLanguage("LogOtherCfgEdit");
			ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型

            if (ViewBag.OType == "add")
            {
				//新增时
				//if (LogOtherCfgService.Has_BillNoRule("取业务类型") == true)
                //{
                //    var vBillNo = LogOtherCfgService.GetBillNo();//取用户编码,新增时界面上显示
                //    ViewBag.No = vBillNo.BillNoList[0];
                //}
            }

            return View("LogOtherCfgEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
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
        public string GetLogOtherCfgInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型

			var findedresult = LogOtherCfgService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
            string mstformData = System.Web.HttpContext.Current.Request.Form["mstformData"];

            var mstforminfo = DataConverterHelper.JsonToEntity<LogOtherCfgModel>(mstformData);

            var savedresult = LogOtherCfgService.Save<Int64>(mstforminfo.AllRow);

            return DataConverterHelper.SerializeObject(savedresult);
        }

		/// <summary>
        /// 删除数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Delete()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            var deletedresult = LogOtherCfgService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

    }
}

