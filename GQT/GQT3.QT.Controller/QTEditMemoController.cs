#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Controller
    * 类 名 称：			QTEditMemoController
    * 文 件 名：			QTEditMemoController.cs
    * 创建时间：			2019/5/29 
    * 作    者：			董泉伟    
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
using System.Net;

using Enterprise3.Common.Base.Criterion;

namespace GQT3.QT.Controller
{
	/// <summary>
	/// QTEditMemo控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class QTEditMemoController : AFCommonController
    {
        IQTEditMemoService QTEditMemoService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public QTEditMemoController()
	    {
	        QTEditMemoService = base.GetObject<IQTEditMemoService>("GQT3.QT.Service.QTEditMemo");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QTEditMemoList()
        {
			ViewBag.Title = base.GetMenuLanguage("QTEditMemo");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "批注记录";
            }
            base.InitialMultiLanguage("QTEditMemo");
            ViewBag.IndividualInfo = this.GetIndividualUI("QTEditMemo");
            return View("QTEditMemoList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QTEditMemoEdit()
        {
			var tabTitle = base.GetMenuLanguage("QTEditMemo");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "批注记录";
            }
            base.SetUserDefScriptUrl("QTEditMemo");
            base.InitialMultiLanguage("QTEditMemo");
            ViewBag.IndividualInfo = this.GetIndividualUI("QTEditMemo");

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

            return View("QTEditMemoEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTEditMemoList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary
            var  Memophid = Convert.ToInt64( System.Web.HttpContext.Current.Request.Params["Memophid"]);

            new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<Int64>.Eq("Memophid", Memophid));

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = QTEditMemoService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" });

            return DataConverterHelper.EntityListToJson<QTEditMemoModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTEditMemoInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = QTEditMemoService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string qteditmemoformData = System.Web.HttpContext.Current.Request.Form["qteditmemoformData"];

			var qteditmemoforminfo = DataConverterHelper.JsonToEntity<QTEditMemoModel>(qteditmemoformData);
            //获取IP
            string IP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    IP = _IPAddress.ToString();
                }
            }
            for (var i = 0; i < qteditmemoforminfo.AllRow.Count; i++)
            {
                qteditmemoforminfo.AllRow[i].MemoTime = DateTime.Today;
                qteditmemoforminfo.AllRow[i].UserCode = base.LoginID;
                qteditmemoforminfo.AllRow[i].UserName = base.UserName;
                qteditmemoforminfo.AllRow[i].IP = IP;
            }

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = QTEditMemoService.Save<Int64>(qteditmemoforminfo.AllRow,"");
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

            var deletedresult = QTEditMemoService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }


        /// <summary>
        /// 保存记录
        /// </summary>
        /// <returns></returns>
        public string SaveData()
        {
            string qtindividualinfoData = System.Web.HttpContext.Current.Request.Form["data"];
            var qtindividualinfoforminfo = DataConverterHelper.JsonToEntity<QTEditMemoModel>(qtindividualinfoData);
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            try
            {
                savedresult = QTEditMemoService.Save<Int64>(qtindividualinfoforminfo.AllRow, "");
               
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

