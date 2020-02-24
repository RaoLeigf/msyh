#region Summary
/**************************************************************************************
    * 类 名 称：        QTProductController
    * 命名空间：        GQT3.QT.Controller
    * 文 件 名：        QTProductController.cs
    * 创建时间：        2018/12/12 
    * 作    者：        刘杭    
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
	/// QTProduct控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class QTProductController : AFCommonController
    {
        IQTProductService QTProductService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public QTProductController()
	    {
	        QTProductService = base.GetObject<IQTProductService>("GQT3.QT.Service.QTProduct");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QTProductList()
        {
			ViewBag.Title = base.GetMenuLanguage("QTProduct");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "产品标识表";
            }
            base.InitialMultiLanguage("QTProduct");
            ViewBag.IndividualInfo = this.GetIndividualUI("QTProduct");
            return View("QTProductList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QTProductEdit()
        {
			var tabTitle = base.GetMenuLanguage("QTProduct");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "产品标识表";
            }
            base.SetUserDefScriptUrl("QTProduct");
            base.InitialMultiLanguage("QTProduct");
            ViewBag.IndividualInfo = this.GetIndividualUI("QTProduct");

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

            return View("QTProductEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTProductList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = QTProductService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<QTProductModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTProductInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = QTProductService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string qtproductformData = System.Web.HttpContext.Current.Request.Form["qtproductformData"];

			var qtproductforminfo = DataConverterHelper.JsonToEntity<QTProductModel>(qtproductformData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = QTProductService.Save<Int64>(qtproductforminfo.AllRow, "");
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

            var deletedresult = QTProductService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }


        /// <summary>
        /// 根据标志获取产品详细
        /// </summary>
        /// <returns></returns>
        public string GetQTProductByBZ()
        {
            string ProductBZ = System.Web.HttpContext.Current.Request.Params["ProductBZ"];
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic).Add(ORMRestrictions<string>.Eq("ProductBZ", ProductBZ));
            var findedresult = QTProductService.Find(dic).Data[0];
            return DataConverterHelper.ResponseResultToJson(findedresult);
        }

    }
}

