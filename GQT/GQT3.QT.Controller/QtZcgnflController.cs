#region Summary
/**************************************************************************************
    * 类 名 称：			QtZcgnflController
    * 命名空间：			GQT3.QT.Controller
    * 文 件 名：			QtZcgnflController.cs
    * 创建时间：			2019/1/23 
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
	/// QtZcgnfl控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class QtZcgnflController : AFCommonController
    {
        IQtZcgnflService QtZcgnflService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public QtZcgnflController()
	    {
	        QtZcgnflService = base.GetObject<IQtZcgnflService>("GQT3.QT.Service.QtZcgnfl");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QtZcgnflList()
        {
			ViewBag.Title = base.GetMenuLanguage("ExpenseAccounts");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "支出功能分类科目";
            }
            base.InitialMultiLanguage("ExpenseAccounts");
            ViewBag.IndividualInfo = this.GetIndividualUI("ExpenseAccounts");
            return View("QtZcgnflList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QtZcgnflEdit()
        {
			var tabTitle = base.GetMenuLanguage("ExpenseAccounts");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "支出功能分类科目";
            }
            base.SetUserDefScriptUrl("ExpenseAccounts");
            base.InitialMultiLanguage("ExpenseAccounts");
            ViewBag.IndividualInfo = this.GetIndividualUI("ExpenseAccounts");

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

            return View("QtZcgnflEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQtZcgnflList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = QtZcgnflService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere, new string[] { "KMDM Asc" });

            return DataConverterHelper.EntityListToJson<QtZcgnflModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQtZcgnflInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = QtZcgnflService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string qtzcgnflformData = System.Web.HttpContext.Current.Request.Form["qtzcgnflformData"];

			var qtzcgnflforminfo = DataConverterHelper.JsonToEntity<QtZcgnflModel>(qtzcgnflformData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = QtZcgnflService.Save<Int64>(qtzcgnflforminfo.AllRow,"");
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

            var deletedresult = QtZcgnflService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 根据code判断是否是末级
        /// </summary>
        /// <returns>返回Json串</returns>
        public string IfLastStage(string QtZcgnflCode)
        {
            var findResult = QtZcgnflService.IfLastStage(QtZcgnflCode);
            return DataConverterHelper.SerializeObject(findResult);
        }

    }
}

