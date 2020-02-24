#region Summary
/**************************************************************************************
    * 命名空间：			GXM3.XM.Controller
    * 类 名 称：			XmReportMstController
    * 文 件 名：			XmReportMstController.cs
    * 创建时间：			2020/1/17 
    * 作    者：			王冠冠    
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

using GXM3.XM.Service.Interface;
using GXM3.XM.Model.Domain;

namespace GXM3.XM.Controller
{
	/// <summary>
	/// XmReportMst控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class XmReportMstController : AFCommonController
    {
        IXmReportMstService XmReportMstService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public XmReportMstController()
	    {
	        XmReportMstService = base.GetObject<IXmReportMstService>("GXM3.XM.Service.XmReportMst");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult XmReportMstList()
        {
			ViewBag.Title = base.GetMenuLanguage("XmReportMst");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "签报单";
            }
            base.InitialMultiLanguage("XmReportMst");
            ViewBag.IndividualInfo = this.GetIndividualUI("XmReportMst");
            return View("XmReportMstList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult XmReportMstEdit()
        {
			var tabTitle = base.GetMenuLanguage("XmReportMst");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "签报单";
            }
            base.SetUserDefScriptUrl("XmReportMst");
            base.InitialMultiLanguage("XmReportMst");
            ViewBag.IndividualInfo = this.GetIndividualUI("XmReportMst");

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

            return View("XmReportMstEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetXmReportMstList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = XmReportMstService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<XmReportMstModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetXmReportMstInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			switch (tabtype)
			{
				case "xmreportmst":
					var findedresultxmreportmst = XmReportMstService.Find(id);
					return DataConverterHelper.ResponseResultToJson(findedresultxmreportmst);
				case "xmreportdtl":
					var findedresultxmreportdtl = XmReportMstService.FindXmReportDtlByForeignKey(id);
					return DataConverterHelper.EntityListToJson(findedresultxmreportdtl.Data, findedresultxmreportdtl.Data.Count);
				default:
					FindedResult findedresultother = new FindedResult();
					return DataConverterHelper.ResponseResultToJson(findedresultother);
			}
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string xmreportmstformData = System.Web.HttpContext.Current.Request.Form["xmreportmstformData"];
			string xmreportdtlgridData = System.Web.HttpContext.Current.Request.Form["xmreportdtlgridData"];

			var xmreportmstforminfo = DataConverterHelper.JsonToEntity<XmReportMstModel>(xmreportmstformData);
			var xmreportdtlgridinfo = DataConverterHelper.JsonToEntity<XmReportDtlModel>(xmreportdtlgridData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = XmReportMstService.SaveXmReportMst(xmreportmstforminfo.AllRow[0],xmreportdtlgridinfo.AllRow);
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

            var deletedresult = XmReportMstService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

    }
}

