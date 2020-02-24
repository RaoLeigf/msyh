#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Controller
    * 类 名 称：			QtYJKController
    * 文 件 名：			QtYJKController.cs
    * 创建时间：			2019/4/15 
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
	/// QtYJK控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class QtYJKController : AFCommonController
    {
        IQtYJKService QtYJKService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public QtYJKController()
	    {
	        QtYJKService = base.GetObject<IQtYJKService>("GQT3.QT.Service.QtYJK");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QtYJKList()
        {
			ViewBag.Title = base.GetMenuLanguage("QtYJK");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "意见库";
            }
            base.InitialMultiLanguage("QtYJK");
            ViewBag.IndividualInfo = this.GetIndividualUI("QtYJK");
            return View("QtYJKList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QtYJKEdit()
        {
			var tabTitle = base.GetMenuLanguage("QtYJK");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "意见库";
            }
            base.SetUserDefScriptUrl("QtYJK");
            base.InitialMultiLanguage("QtYJK");
            ViewBag.IndividualInfo = this.GetIndividualUI("QtYJK");

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

            return View("QtYJKEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQtYJKList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = QtYJKService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere,new string[]{ "Usenum Desc" });

            return DataConverterHelper.EntityListToJson<QtYJKModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQtYJKInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = QtYJKService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string qtyjkformData = System.Web.HttpContext.Current.Request.Form["qtyjkformData"];

			var qtyjkforminfo = DataConverterHelper.JsonToEntity<QtYJKModel>(qtyjkformData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = QtYJKService.Save<Int64>(qtyjkforminfo.AllRow,"");
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

            var deletedresult = QtYJKService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 更新意见库
        /// </summary>
        /// <returns></returns>
        public string Update1()
        {
            string DeleteYJ = System.Web.HttpContext.Current.Request.Params["DeleteYJ"];
            string ChangeYJ = System.Web.HttpContext.Current.Request.Params["ChangeYJ"];
            string InsertYJ = System.Web.HttpContext.Current.Request.Params["InsertYJ"];
            
            var DeleteYJPhids= JsonConvert.DeserializeObject<List<long>>(DeleteYJ);
            var Changedatas = JsonConvert.DeserializeObject<List<QtYJKModel>>(ChangeYJ);
            var Insertdatas = JsonConvert.DeserializeObject<List<QtYJKModel>>(InsertYJ);
            SavedResult<Int64> savedresult = QtYJKService.Update1(DeleteYJPhids, Changedatas, Insertdatas);
            return DataConverterHelper.SerializeObject(savedresult);

        }

        /// <summary>
        /// 更新意见次数
        /// </summary>
        /// <returns></returns>
        public string UpdateNum()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            QtYJKModel model = QtYJKService.Find(id).Data;
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            model.Usenum = model.Usenum + 1;
            model.PersistentState = PersistentState.Modified;
            try
            {
                savedresult = QtYJKService.Save<Int64>(model, "");
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

