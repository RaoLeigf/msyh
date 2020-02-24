#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Controller
    * 类 名 称：			QtAattachmentController
    * 文 件 名：			QtAattachmentController.cs
    * 创建时间：			2019/6/15 
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
	/// QtAattachment控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class QtAattachmentController : AFCommonController
    {
        IQtAattachmentService QtAattachmentService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public QtAattachmentController()
	    {
	        QtAattachmentService = base.GetObject<IQtAattachmentService>("GQT3.QT.Service.QtAattachment");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QtAattachmentList()
        {
			ViewBag.Title = base.GetMenuLanguage("QtAattachment");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "资金拨付附件表";
            }
            base.InitialMultiLanguage("QtAattachment");
            ViewBag.IndividualInfo = this.GetIndividualUI("QtAattachment");
            return View("QtAattachmentList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QtAattachmentEdit()
        {
			var tabTitle = base.GetMenuLanguage("QtAattachment");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "资金拨付附件表";
            }
            base.SetUserDefScriptUrl("QtAattachment");
            base.InitialMultiLanguage("QtAattachment");
            ViewBag.IndividualInfo = this.GetIndividualUI("QtAattachment");

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

            return View("QtAattachmentEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQtAattachmentList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = QtAattachmentService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<QtAattachmentModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQtAattachmentInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = QtAattachmentService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string qtaattachmentformData = System.Web.HttpContext.Current.Request.Form["qtaattachmentformData"];

			var qtaattachmentforminfo = DataConverterHelper.JsonToEntity<QtAattachmentModel>(qtaattachmentformData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = QtAattachmentService.Save<Int64>(qtaattachmentforminfo.AllRow,"");
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

            var deletedresult = QtAattachmentService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

    }
}

