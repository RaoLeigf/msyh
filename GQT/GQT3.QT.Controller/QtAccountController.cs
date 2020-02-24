#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Controller
    * 类 名 称：			QtAccountController
    * 文 件 名：			QtAccountController.cs
    * 创建时间：			2019/9/18 
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
	/// QtAccount控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class QtAccountController : AFCommonController
    {
        IQtAccountService QtAccountService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public QtAccountController()
	    {
	        QtAccountService = base.GetObject<IQtAccountService>("GQT3.QT.Service.QtAccount");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QtAccountList()
        {
			ViewBag.Title = base.GetMenuLanguage("QtAccount");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "账套";
            }
            base.InitialMultiLanguage("QtAccount");
            ViewBag.IndividualInfo = this.GetIndividualUI("QtAccount");
            return View("QtAccountList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QtAccountEdit()
        {
			var tabTitle = base.GetMenuLanguage("QtAccount");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "账套";
            }
            base.SetUserDefScriptUrl("QtAccount");
            base.InitialMultiLanguage("QtAccount");
            ViewBag.IndividualInfo = this.GetIndividualUI("QtAccount");

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

            return View("QtAccountEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQtAccountList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = QtAccountService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere,new string[] { "Dm" });

            return DataConverterHelper.EntityListToJson<QtAccountModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQtAccountInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = QtAccountService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string qtaccountformData = System.Web.HttpContext.Current.Request.Form["qtaccountformData"];

			var qtaccountforminfo = DataConverterHelper.JsonToEntity<QtAccountModel>(qtaccountformData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = QtAccountService.Save<Int64>(qtaccountforminfo.AllRow,"");
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

            var deletedresult = QtAccountService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save2()
        {
            string updatedata = System.Web.HttpContext.Current.Request.Params["updatedata"];

            var updateinfo = JsonConvert.DeserializeObject<List<QtAccountModel>>(updatedata);

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            try
            {
                if (updateinfo.Count > 0)
                {
                    var data = new List<QtAccountModel>();
                    for (var i =0;i< updateinfo.Count; i++)
                    {
                        QtAccountModel a = QtAccountService.Find(updateinfo[i].PhId).Data;
                        a.IsDefault = updateinfo[i].IsDefault;
                        a.PersistentState = PersistentState.Modified;
                        data.Add(a);
                    }
                    savedresult = QtAccountService.Save<Int64>(data, "");
                }
                
            }
            catch (Exception ex)
            {
                savedresult.Status = ResponseStatus.Error;
                savedresult.Msg = ex.Message.ToString();
            }
            return DataConverterHelper.SerializeObject(savedresult);
        }


    }
}

