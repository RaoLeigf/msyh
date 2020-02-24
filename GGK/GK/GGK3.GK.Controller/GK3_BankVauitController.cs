#region Summary
/**************************************************************************************
    * 命名空间：			GGK3.GK.Controller
    * 类 名 称：			GK3_BankVauitController
    * 文 件 名：			GK3_BankVauitController.cs
    * 创建时间：			2019/11/18 
    * 作    者：			张宇    
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
	/// GK3_BankVauit控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class GK3_BankVauitController : AFCommonController
    {
        IGK3_BankVauitService GK3_BankVauitService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public GK3_BankVauitController()
	    {
	        GK3_BankVauitService = base.GetObject<IGK3_BankVauitService>("GGK3.GK.Service.GK3_BankVauit");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult GK3_BankVauitList()
        {
			ViewBag.Title = base.GetMenuLanguage("gk3_bankvauit");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "网银银行";
            }
            base.InitialMultiLanguage("gk3_bankvauit");
            ViewBag.IndividualInfo = this.GetIndividualUI("gk3_bankvauit");
            return View("GK3_BankVauitList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult GK3_BankVauitEdit()
        {
			var tabTitle = base.GetMenuLanguage("gk3_bankvauit");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "网银银行";
            }
            base.SetUserDefScriptUrl("gk3_bankvauit");
            base.InitialMultiLanguage("gk3_bankvauit");
            ViewBag.IndividualInfo = this.GetIndividualUI("gk3_bankvauit");

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

            return View("GK3_BankVauitEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetGK3_BankVauitList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = GK3_BankVauitService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<GK3_BankVauitModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetGK3_BankVauitInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = GK3_BankVauitService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string gk3_bankvauitformData = System.Web.HttpContext.Current.Request.Form["gk3_bankvauitformData"];

			var gk3_bankvauitforminfo = DataConverterHelper.JsonToEntity<GK3_BankVauitModel>(gk3_bankvauitformData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = GK3_BankVauitService.Save<Int64>(gk3_bankvauitforminfo.AllRow,"");
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

            var deletedresult = GK3_BankVauitService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

    }
}

