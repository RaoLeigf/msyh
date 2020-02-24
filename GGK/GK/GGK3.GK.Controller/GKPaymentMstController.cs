#region Summary
/**************************************************************************************
    * 命名空间：			GGK3.GK.Controller
    * 类 名 称：			GKPaymentMstController
    * 文 件 名：			GKPaymentMstController.cs
    * 创建时间：			2019/5/20 
    * 作    者：			李明    
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

using GGK3.GK.Service.Interface;
using GGK3.GK.Model.Domain;

namespace GGK3.GK.Controller
{
	/// <summary>
	/// GKPaymentMst控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class GKPaymentMstController : AFCommonController
    {
        IGKPaymentMstService GKPaymentMstService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public GKPaymentMstController()
	    {
	        GKPaymentMstService = base.GetObject<IGKPaymentMstService>("GGK3.GK.Service.GKPaymentMst");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult GKPaymentMstList()
        {
			ViewBag.Title = base.GetMenuLanguage("GKPayment");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "支付单";
            }
            base.InitialMultiLanguage("GKPayment");
            ViewBag.IndividualInfo = this.GetIndividualUI("GKPayment");
            return View("GKPaymentMstList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult GKPaymentMstEdit()
        {
			var tabTitle = base.GetMenuLanguage("GKPayment");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "支付单";
            }
            base.SetUserDefScriptUrl("GKPayment");
            base.InitialMultiLanguage("GKPayment");
            ViewBag.IndividualInfo = this.GetIndividualUI("GKPayment");

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

            return View("GKPaymentMstEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetGKPaymentMstList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = GKPaymentMstService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<GKPaymentMstModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetGKPaymentMstInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			switch (tabtype)
			{
				case "gkpaymentmst":
					var findedresultgkpaymentmst = GKPaymentMstService.Find(id);
					return DataConverterHelper.ResponseResultToJson(findedresultgkpaymentmst);
				case "gkpaymentdtl":
					var findedresultgkpaymentdtl = GKPaymentMstService.FindGKPaymentDtlByForeignKey(id);
					return DataConverterHelper.EntityListToJson(findedresultgkpaymentdtl.Data, findedresultgkpaymentdtl.Data.Count);
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
			string gkpaymentmstformData = System.Web.HttpContext.Current.Request.Form["gkpaymentmstformData"];
			string gkpaymentdtlgridData = System.Web.HttpContext.Current.Request.Form["gkpaymentdtlgridData"];

			var gkpaymentmstforminfo = DataConverterHelper.JsonToEntity<GKPaymentMstModel>(gkpaymentmstformData);
			var gkpaymentdtlgridinfo = DataConverterHelper.JsonToEntity<GKPaymentDtlModel>(gkpaymentdtlgridData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = GKPaymentMstService.SaveGKPaymentMst(gkpaymentmstforminfo.AllRow[0],gkpaymentdtlgridinfo.AllRow);
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

            var deletedresult = GKPaymentMstService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

    }
}

