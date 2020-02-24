#region Summary
/**************************************************************************************
    * 命名空间：			GBK3.BK.Controller
    * 类 名 称：			PaymentMstController
    * 文 件 名：			PaymentMstController.cs
    * 创建时间：			2019/5/15 
    * 作    者：			吾丰明    
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

using GBK3.BK.Service.Interface;
using GBK3.BK.Model.Domain;

namespace GBK3.BK.Controller
{
	/// <summary>
	/// PaymentMst控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class PaymentMstController : AFCommonController
    {
        IPaymentMstService PaymentMstService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public PaymentMstController()
	    {
	        PaymentMstService = base.GetObject<IPaymentMstService>("GBK3.BK.Service.PaymentMst");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult PaymentMstList()
        {
			ViewBag.Title = base.GetMenuLanguage("PaymentMst");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "资金拨付";
            }
            base.InitialMultiLanguage("PaymentMst");
            ViewBag.IndividualInfo = this.GetIndividualUI("PaymentMst");
            return View("PaymentMstList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult PaymentMstEdit()
        {
			var tabTitle = base.GetMenuLanguage("PaymentMst");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "资金拨付";
            }
            base.SetUserDefScriptUrl("PaymentMst");
            base.InitialMultiLanguage("PaymentMst");
            ViewBag.IndividualInfo = this.GetIndividualUI("PaymentMst");

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

            return View("PaymentMstEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetPaymentMstList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = PaymentMstService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<PaymentMstModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetPaymentMstInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			switch (tabtype)
			{
				case "paymentmst":
					var findedresultpaymentmst = PaymentMstService.Find(id);
					return DataConverterHelper.ResponseResultToJson(findedresultpaymentmst);
				case "paymentdtl":
					var findedresultpaymentdtl = PaymentMstService.FindPaymentDtlByForeignKey(id);
					return DataConverterHelper.EntityListToJson(findedresultpaymentdtl.Data, findedresultpaymentdtl.Data.Count);
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
			string paymentmstformData = System.Web.HttpContext.Current.Request.Form["paymentmstformData"];
			string paymentdtlgridData = System.Web.HttpContext.Current.Request.Form["paymentdtlgridData"];

			var paymentmstforminfo = DataConverterHelper.JsonToEntity<PaymentMstModel>(paymentmstformData);
			var paymentdtlgridinfo = DataConverterHelper.JsonToEntity<PaymentDtlModel>(paymentdtlgridData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = PaymentMstService.SavePaymentMst(paymentmstforminfo.AllRow[0],paymentdtlgridinfo.AllRow);
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

            var deletedresult = PaymentMstService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

    }
}

