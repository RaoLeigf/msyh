#region Summary
/**************************************************************************************
    * 类 名 称：        PaymentMethodController
    * 命名空间：        GQT3.QT.Controller
    * 文 件 名：        PaymentMethodController.cs
    * 创建时间：        2018/9/7 
    * 作    者：        夏华军    
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
using GXM3.XM.Model.Domain;
using GXM3.XM.Service.Interface;
using GYS3.YS.Model.Domain;
using GYS3.YS.Service.Interface;

namespace GQT3.QT.Controller
{
	/// <summary>
	/// PaymentMethod控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class PaymentMethodController : AFCommonController
    {
        IPaymentMethodService PaymentMethodService { get; set; }
        IProjectMstService ProjectMstService { get; set; }
        IBudgetMstService BudgetMstService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public PaymentMethodController()
	    {
	        PaymentMethodService = base.GetObject<IPaymentMethodService>("GQT3.QT.Service.PaymentMethod");
            ProjectMstService = base.GetObject<IProjectMstService>("GXM3.XM.Service.ProjectMst");
            BudgetMstService = base.GetObject<IBudgetMstService>("GYS3.YS.Service.BudgetMst");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult PaymentMethodList()
        {
			ViewBag.Title = base.GetMenuLanguage("PaymentMethod");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "支付方式";
            }
            base.InitialMultiLanguage("PaymentMethod");
            ViewBag.IndividualInfo = this.GetIndividualUI("PaymentMethod");
            return View("PaymentMethodList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult PaymentMethodEdit()
        {
			var tabTitle = base.GetMenuLanguage("PaymentMethod");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "支付方式";
            }
            base.SetUserDefScriptUrl("PaymentMethod");
            base.InitialMultiLanguage("PaymentMethod");
            ViewBag.IndividualInfo = this.GetIndividualUI("PaymentMethod");

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

            return View("PaymentMethodEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetPaymentMethodList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = PaymentMethodService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<PaymentMethodModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetPaymentMethodInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = PaymentMethodService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string paymentmethodformData = System.Web.HttpContext.Current.Request.Form["paymentmethodformData"];
            string otype = System.Web.HttpContext.Current.Request.Form["otype"];
			var paymentmethodforminfo = DataConverterHelper.JsonToEntity<PaymentMethodModel>(paymentmethodformData);

            List<PaymentMethodModel> paymentMethods = paymentmethodforminfo.AllRow;
            var checkresult = PaymentMethodService.ExecuteDataCheck(ref paymentMethods,otype);
            if (checkresult.Status == ResponseStatus.Error)
            {
                return DataConverterHelper.SerializeObject(checkresult);
            }

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = PaymentMethodService.Save<Int64>(paymentmethodforminfo.AllRow,"");
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

            FindedResults<PaymentMethodModel> paymentMethods = PaymentMethodService.Find(t => t.PhId == id);
            if (paymentMethods != null && paymentMethods.Data.Count > 0) {
                string dm = paymentMethods.Data[0].Dm;
                FindedResults<ProjectDtlBudgetDtlModel> findedResults1 = ProjectMstService.FindPaymentMethod(dm);
                if (findedResults1 != null && findedResults1.Status == ResponseStatus.Error) {
                    return DataConverterHelper.SerializeObject(findedResults1);
                }

                FindedResults<BudgetDtlBudgetDtlModel> findedResults2 = BudgetMstService.FindPaymentMethod(dm);
                if (findedResults2 != null && findedResults2.Status == ResponseStatus.Error) {
                    return DataConverterHelper.SerializeObject(findedResults2);
                }
            }

            var deletedresult = PaymentMethodService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

    }
}

