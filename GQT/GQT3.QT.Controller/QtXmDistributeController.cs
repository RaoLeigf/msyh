#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Controller
    * 类 名 称：			QtXmDistributeController
    * 文 件 名：			QtXmDistributeController.cs
    * 创建时间：			2020/1/6 
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
	/// QtXmDistribute控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class QtXmDistributeController : AFCommonController
    {
        IQtXmDistributeService QtXmDistributeService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public QtXmDistributeController()
	    {
	        QtXmDistributeService = base.GetObject<IQtXmDistributeService>("GQT3.QT.Service.QtXmDistribute");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QtXmDistributeList()
        {
			ViewBag.Title = base.GetMenuLanguage("QtXmDistribute");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "项目分发表";
            }
            base.InitialMultiLanguage("QtXmDistribute");
            ViewBag.IndividualInfo = this.GetIndividualUI("QtXmDistribute");
            return View("QtXmDistributeList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QtXmDistributeEdit()
        {
			var tabTitle = base.GetMenuLanguage("QtXmDistribute");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "项目分发表";
            }
            base.SetUserDefScriptUrl("QtXmDistribute");
            base.InitialMultiLanguage("QtXmDistribute");
            ViewBag.IndividualInfo = this.GetIndividualUI("QtXmDistribute");

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

            return View("QtXmDistributeEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQtXmDistributeList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = QtXmDistributeService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<QtXmDistributeModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQtXmDistributeInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = QtXmDistributeService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string qtxmdistributeformData = System.Web.HttpContext.Current.Request.Form["qtxmdistributeformData"];

			var qtxmdistributeforminfo = DataConverterHelper.JsonToEntity<QtXmDistributeModel>(qtxmdistributeformData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = QtXmDistributeService.Save<Int64>(qtxmdistributeforminfo.AllRow,"");
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

            var deletedresult = QtXmDistributeService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

    }
}

