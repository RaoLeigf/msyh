#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Controller
    * 类 名 称：			QtVenueController
    * 文 件 名：			QtVenueController.cs
    * 创建时间：			2019/11/27 
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
using Enterprise3.Common.Base.Criterion;

namespace GQT3.QT.Controller
{
	/// <summary>
	/// QtVenue控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class QtVenueController : AFCommonController
    {
        IQtVenueService QtVenueService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public QtVenueController()
	    {
	        QtVenueService = base.GetObject<IQtVenueService>("GQT3.QT.Service.QtVenue");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QtVenueList()
        {
			ViewBag.Title = base.GetMenuLanguage("QtVenue");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "活动地点";
            }
            base.InitialMultiLanguage("QtVenue");
            ViewBag.IndividualInfo = this.GetIndividualUI("QtVenue");
            return View("QtVenueList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QtVenueEdit()
        {
			var tabTitle = base.GetMenuLanguage("QtVenue");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "活动地点";
            }
            base.SetUserDefScriptUrl("QtVenue");
            base.InitialMultiLanguage("QtVenue");
            ViewBag.IndividualInfo = this.GetIndividualUI("QtVenue");

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

            return View("QtVenueEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQtVenueList()
        {
            string Orgcode = System.Web.HttpContext.Current.Request.Params["Orgcode"];
            //string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            //Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();

            //DataStoreParam storeparam = this.GetDataStoreParam();
            //var result = QtVenueService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);
            if (!string.IsNullOrEmpty(Orgcode))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Orgcode", Orgcode));
                var result = QtVenueService.Find(dicWhere, new string[] { "Dm" }).Data;
                return DataConverterHelper.EntityListToJson<QtVenueModel>(result, (Int32)result.Count);
            }
            else
            {
                return DataConverterHelper.EntityListToJson<QtVenueModel>(null, 0);
            }

            
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQtVenueInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = QtVenueService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string qtvenueformData = System.Web.HttpContext.Current.Request.Form["qtvenueformData"];

			var qtvenueforminfo = DataConverterHelper.JsonToEntity<QtVenueModel>(qtvenueformData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = QtVenueService.Save<Int64>(qtvenueforminfo.AllRow,"");
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

            var deletedresult = QtVenueService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public string Save2()
        {
            string updatedata = System.Web.HttpContext.Current.Request.Params["updatedata"];
            string deletedata = System.Web.HttpContext.Current.Request.Params["deletedata"];

            var updateinfo = JsonConvert.DeserializeObject<List<QtVenueModel>>(updatedata);
            var deleteinfo = JsonConvert.DeserializeObject<List<string>>(deletedata);
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            try
            {
                savedresult = QtVenueService.Save2(updateinfo, deleteinfo);
            }
            catch (Exception ex)
            {
                savedresult.Status = ResponseStatus.Error;
                savedresult.Msg = ex.Message.ToString();
            }
            return DataConverterHelper.SerializeObject(savedresult);
        }
    }
}

