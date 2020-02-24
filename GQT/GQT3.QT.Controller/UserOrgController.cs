#region Summary
/**************************************************************************************
    * 类 名 称：        UserOrgController
    * 命名空间：        GQT3.QT.Controller
    * 文 件 名：        UserOrgController.cs
    * 创建时间：        2018/9/19 
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

namespace GQT3.QT.Controller
{
	/// <summary>
	/// UserOrg控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class UserOrgController : AFCommonController
    {
        IUserOrgService UserOrgService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public UserOrgController()
	    {
	        UserOrgService = base.GetObject<IUserOrgService>("GQT3.QT.Service.UserOrg");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult UserOrgList()
        {
			ViewBag.Title = base.GetMenuLanguage("CorrespondenceSettings2");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "对应关系设置2";
            }
            base.InitialMultiLanguage("CorrespondenceSettings2");
            ViewBag.IndividualInfo = this.GetIndividualUI("CorrespondenceSettings2");
            return View("UserOrgList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult UserOrgEdit()
        {
			var tabTitle = base.GetMenuLanguage("CorrespondenceSettings2");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "对应关系设置2";
            }
            base.SetUserDefScriptUrl("CorrespondenceSettings2");
            base.InitialMultiLanguage("CorrespondenceSettings2");
            ViewBag.IndividualInfo = this.GetIndividualUI("CorrespondenceSettings2");

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

            return View("UserOrgEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetUserOrgList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = UserOrgService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<UserOrganize2Model>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetUserOrgInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = UserOrgService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string userorgformData = System.Web.HttpContext.Current.Request.Form["userorgformData"];

			var userorgforminfo = DataConverterHelper.JsonToEntity<UserOrganize2Model>(userorgformData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = UserOrgService.Save<Int64>(userorgforminfo.AllRow,"");
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

            var deletedresult = UserOrgService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetList() {
            //GQT.QT.GetList
            long userId = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["userId"]);
            PagedResult<RWReportModel> result = UserOrgService.GetList(userId);
            return DataConverterHelper.EntityListToJson<RWReportModel>(result.Results, (Int32)result.TotalItems);
        }
    }
}

