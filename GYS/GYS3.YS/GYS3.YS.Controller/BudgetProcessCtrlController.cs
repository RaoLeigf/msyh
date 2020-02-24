#region Summary
/**************************************************************************************
    * 类 名 称：        BudgetProcessCtrlController
    * 命名空间：        GYS3.YS.Controller
    * 文 件 名：        BudgetProcessCtrlController.cs
    * 创建时间：        2018/9/10 
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

using GYS3.YS.Service.Interface;
using GYS3.YS.Model.Domain;

namespace GYS3.YS.Controller
{
	/// <summary>
	/// BudgetProcessCtrl控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class BudgetProcessCtrlController : AFCommonController
    {
        IBudgetProcessCtrlService BudgetProcessCtrlService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public BudgetProcessCtrlController()
	    {
	        BudgetProcessCtrlService = base.GetObject<IBudgetProcessCtrlService>("GYS3.YS.Service.BudgetProcessCtrl");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult BudgetProcessCtrlList()
        {
			ViewBag.Title = base.GetMenuLanguage("GHBudgetProgressctrl");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "进度控制";
            }
            base.InitialMultiLanguage("GHBudgetProgressctrl");
            ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetProgressctrl");
            return View("BudgetProcessCtrlList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult BudgetProcessCtrlEdit()
        {
			var tabTitle = base.GetMenuLanguage("GHBudgetProgressctrl");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "进度控制";
            }
            base.SetUserDefScriptUrl("GHBudgetProgressctrl");
            base.InitialMultiLanguage("GHBudgetProgressctrl");
            ViewBag.IndividualInfo = this.GetIndividualUI("GHBudgetProgressctrl");

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

            return View("BudgetProcessCtrlEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetBudgetProcessCtrlList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = BudgetProcessCtrlService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<BudgetProcessCtrlModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetBudgetProcessCtrlDistinctList()
        {
            DataStoreParam storeparam = this.GetDataStoreParam();
            string query = System.Web.HttpContext.Current.Request.Params["FOcode"];//查询条件
            string userId = System.Web.HttpContext.Current.Request.Params["userId"];
            var result = BudgetProcessCtrlService.GetBudgetProcessCtrlDistinctList(storeparam,query,userId);
            return DataConverterHelper.EntityListToJson<BudgetProcessCtrlModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetBudgetProcessCtrlPorcessList() {
            DataStoreParam storeparam = this.GetDataStoreParam();
            string focode = System.Web.HttpContext.Current.Request.Params["FOcode"];//查询条件 
            string Fyear = System.Web.HttpContext.Current.Request.Params["FYear"];//年度
            List<BudgetProcessCtrlModel> list = null;
            BudgetProcessCtrlService.GetBudgetProcessCtrlPorcessList(storeparam,focode, Fyear, out list);
            if (list.Count > 0) {
                BudgetProcessCtrlService.Save<Int64>(list, "");
            }
            var result = BudgetProcessCtrlService.GetBudgetProcessCtrlPorcessList2(storeparam, focode, Fyear);
            return DataConverterHelper.EntityListToJson<BudgetProcessCtrlModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetBudgetProcessCtrlInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = BudgetProcessCtrlService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string budgetprocessctrlformData = System.Web.HttpContext.Current.Request.Form["jsonArray"];
            string symbol = System.Web.HttpContext.Current.Request.Form["symbol"];
            //List<BudgetProcessCtrlModel> list = JsonConvert.DeserializeObject<List<BudgetProcessCtrlModel>>(budgetprocessctrlformData);

            //IList<BudgetProcessCtrlModel> list2 = BudgetProcessCtrlService.FindBudgetProcessCtrlModelByList(list);

            //for (int i = 0; i < list.Count; i++) {
            //    list[i].PersistentState = PersistentState.Modified;
            //}
            var budgetprocessctrlforminfo = DataConverterHelper.JsonToEntity<BudgetProcessCtrlModel>(budgetprocessctrlformData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				//savedresult = BudgetProcessCtrlService.Save<Int64>(budgetprocessctrlforminfo.AllRow);
                savedresult = BudgetProcessCtrlService.SaveProcessSetting(budgetprocessctrlforminfo.AllRow, symbol);
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

            var deletedresult = BudgetProcessCtrlService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

    }
}

