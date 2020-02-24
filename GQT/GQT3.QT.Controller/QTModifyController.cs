#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Controller
    * 类 名 称：			QTModifyController
    * 文 件 名：			QTModifyController.cs
    * 创建时间：			2019/5/20 
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
using Enterprise3.Common.Base.Criterion;

namespace GQT3.QT.Controller
{
	/// <summary>
	/// QTModify控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class QTModifyController : AFCommonController
    {
        IQTModifyService QTModifyService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public QTModifyController()
	    {
	        QTModifyService = base.GetObject<IQTModifyService>("GQT3.QT.Service.QTModify");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QTModifyList()
        {
			ViewBag.Title = base.GetMenuLanguage("QTModify");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "单据修改历史";
            }
            ViewBag.Unit = System.Web.HttpContext.Current.Request.Params["Unit"];//申报组织
            ViewBag.Dept = System.Web.HttpContext.Current.Request.Params["Dept"];//预算部门
            ViewBag.year = System.Web.HttpContext.Current.Request.Params["year"];//年度
            ViewBag.projcode = System.Web.HttpContext.Current.Request.Params["projcode"];//单据代码

            base.InitialMultiLanguage("QTModify");
            ViewBag.IndividualInfo = this.GetIndividualUI("QTModify");
            return View("QTModifyList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QTModifyEdit()
        {
			var tabTitle = base.GetMenuLanguage("QTModify");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "预算单据修改历史";
            }
            base.SetUserDefScriptUrl("QTModify");
            base.InitialMultiLanguage("QTModify");
            ViewBag.IndividualInfo = this.GetIndividualUI("QTModify");

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

            return View("QTModifyEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTModifyList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            string FProjCode = System.Web.HttpContext.Current.Request.Params["FProjCode"];
            
            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            if (!string.IsNullOrEmpty(FProjCode))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FProjCode", FProjCode));
            }
            else
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<Int64>.Eq("PhId", 0));
            }
            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = QTModifyService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere, new string[] { "NgInsertDt Desc" });

            return DataConverterHelper.EntityListToJson<QTModifyModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTModifyInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = QTModifyService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string qtmodifyformData = System.Web.HttpContext.Current.Request.Form["qtmodifyformData"];

			var qtmodifyforminfo = DataConverterHelper.JsonToEntity<QTModifyModel>(qtmodifyformData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = QTModifyService.Save<Int64>(qtmodifyforminfo.AllRow,"");
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

            var deletedresult = QTModifyService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

    }
}

