#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Controller
    * 类 名 称：			QTSysSetController
    * 文 件 名：			QTSysSetController.cs
    * 创建时间：			2019/6/3 
    * 作    者：			刘杭    
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

using GQT3.QT.Service.Interface;
using GQT3.QT.Model.Domain;

namespace GQT3.QT.Controller
{
	/// <summary>
	/// QTSysSet控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class QTSysSetController : AFCommonController
    {
        IQTSysSetService QTSysSetService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public QTSysSetController()
	    {
	        QTSysSetService = base.GetObject<IQTSysSetService>("GQT3.QT.Service.QTSysSet");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QTSysSetList()
        {
			ViewBag.Title = base.GetMenuLanguage("QTSysSet");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "系统设置";
            }
            base.InitialMultiLanguage("QTSysSet");
            ViewBag.IndividualInfo = this.GetIndividualUI("QTSysSet");
            return View("QTSysSetList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QTSysSetEdit()
        {
			var tabTitle = base.GetMenuLanguage("QTSysSet");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "系统设置";
            }
            base.SetUserDefScriptUrl("QTSysSet");
            base.InitialMultiLanguage("QTSysSet");
            ViewBag.IndividualInfo = this.GetIndividualUI("QTSysSet");

			ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型

			if (ViewBag.OType == "add")
            {
			    ViewBag.Title = tabTitle + "-新增";
            }
            else if (ViewBag.OType == "edit")
            {
                ViewBag.Title = tabTitle + "-修改";
            }
            else if (ViewBag.OType == "view")
            {
                ViewBag.Title = tabTitle + "-查看";
            }

            return View("QTSysSetEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTSysSetList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = QTSysSetService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<QTSysSetModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTSysSetInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = QTSysSetService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string qtsyssetformData = System.Web.HttpContext.Current.Request.Form["qtsyssetformData"];

			var qtsyssetforminfo = DataConverterHelper.JsonToEntity<QTSysSetModel>(qtsyssetformData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = QTSysSetService.Save<Int64>(qtsyssetforminfo.AllRow,"");
			}
			catch (Exception ex)
			{
				savedresult.Status = ResponseStatus.Error;
				savedresult.Msg = ex.Message.ToString();
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

            var deletedresult = QTSysSetService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 判断加密锁信息
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetUsbKey()
        {
            string uid = System.Web.HttpContext.Current.Request.Params["uid"];  
            bool isActive = false, isValid = false;//是否超时
            string lockKey = string.Empty;
            DateTime start_dt, end_dt;
            Dictionary<string, object> result = new Dictionary<string, object>();
            isActive = QTSysSetService.GetPayUsbKeyIsActive(long.Parse(uid), out lockKey, out start_dt, out end_dt);
            if (isActive == true)
            {
                if (end_dt < DateTime.Now)
                {
                    isValid = true;
                }
                
            }
            result.Add("isActive", isActive);
            result.Add("lockKey", lockKey);
            result.Add("isValid", isValid);

            return DataConverterHelper.SerializeObject(result);
        }

    }
}

