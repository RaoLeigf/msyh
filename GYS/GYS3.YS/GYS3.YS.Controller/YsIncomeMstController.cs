#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Controller
    * 类 名 称：			YsIncomeMstController
    * 文 件 名：			YsIncomeMstController.cs
    * 创建时间：			2020/1/6 
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

using GYS3.YS.Service.Interface;
using GYS3.YS.Model.Domain;

namespace GYS3.YS.Controller
{
	/// <summary>
	/// YsIncomeMst控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class YsIncomeMstController : AFCommonController
    {
        IYsIncomeMstService YsIncomeMstService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public YsIncomeMstController()
	    {
	        YsIncomeMstService = base.GetObject<IYsIncomeMstService>("GYS3.YS.Service.YsIncomeMst");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult YsIncomeMstList()
        {
			ViewBag.Title = base.GetMenuLanguage("YsIncomeMst");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "收入预算主表";
            }
            base.InitialMultiLanguage("YsIncomeMst");
            ViewBag.IndividualInfo = this.GetIndividualUI("YsIncomeMst");
            return View("YsIncomeMstList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult YsIncomeMstEdit()
        {
			var tabTitle = base.GetMenuLanguage("YsIncomeMst");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "收入预算主表";
            }
            base.SetUserDefScriptUrl("YsIncomeMst");
            base.InitialMultiLanguage("YsIncomeMst");
            ViewBag.IndividualInfo = this.GetIndividualUI("YsIncomeMst");

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

            return View("YsIncomeMstEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetYsIncomeMstList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = YsIncomeMstService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<YsIncomeMstModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetYsIncomeMstInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			switch (tabtype)
			{
				case "ysincomemst":
					var findedresultysincomemst = YsIncomeMstService.Find(id);
					return DataConverterHelper.ResponseResultToJson(findedresultysincomemst);
				case "ysincomedtl":
					var findedresultysincomedtl = YsIncomeMstService.FindYsIncomeDtlByForeignKey(id);
					return DataConverterHelper.EntityListToJson(findedresultysincomedtl.Data, findedresultysincomedtl.Data.Count);
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
			string ysincomemstformData = System.Web.HttpContext.Current.Request.Form["ysincomemstformData"];
			string ysincomedtlgridData = System.Web.HttpContext.Current.Request.Form["ysincomedtlgridData"];

			var ysincomemstforminfo = DataConverterHelper.JsonToEntity<YsIncomeMstModel>(ysincomemstformData);
			var ysincomedtlgridinfo = DataConverterHelper.JsonToEntity<YsIncomeDtlModel>(ysincomedtlgridData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = YsIncomeMstService.SaveYsIncomeMst(ysincomemstforminfo.AllRow[0],ysincomedtlgridinfo.AllRow);
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

            var deletedresult = YsIncomeMstService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

    }
}

