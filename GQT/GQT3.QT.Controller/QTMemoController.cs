#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Controller
    * 类 名 称：			QTMemoController
    * 文 件 名：			QTMemoController.cs
    * 创建时间：			2019/5/15 
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
	/// QTMemo控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class QTMemoController : AFCommonController
    {
        IQTMemoService QTMemoService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public QTMemoController()
	    {
	        QTMemoService = base.GetObject<IQTMemoService>("GQT3.QT.Service.QTMemo");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QTMemoList()
        {
			ViewBag.Title = base.GetMenuLanguage("QTMemo");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "备忘录";
            }
            base.InitialMultiLanguage("QTMemo");
            ViewBag.IndividualInfo = this.GetIndividualUI("QTMemo");
            return View("QTMemoList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QTMemoEdit()
        {
			var tabTitle = base.GetMenuLanguage("QTMemo");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "备忘录";
            }
            base.SetUserDefScriptUrl("QTMemo");
            base.InitialMultiLanguage("QTMemo");
            ViewBag.IndividualInfo = this.GetIndividualUI("QTMemo");

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

            return View("QTMemoEdit");
        }

        /// <summary>
        /// 指向备忘录编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult BwlEdit()
        {
            var tabTitle = base.GetMenuLanguage("QTMemo");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "备忘录";
            }
            base.SetUserDefScriptUrl("QTMemo");
            base.InitialMultiLanguage("QTMemo");
            ViewBag.IndividualInfo = this.GetIndividualUI("QTMemo");

            ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//模板表的主键
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型
            ViewBag.No = System.Web.HttpContext.Current.Request.Params["no"];//备忘录表的主键

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

            return View("BwlEdit");
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTMemoList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            long userId = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["userId"]);

            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<System.Int64>.Eq("Creator", userId));
            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = QTMemoService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<QTMemoModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTMemoInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = QTMemoService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string qtmemoformData = System.Web.HttpContext.Current.Request.Form["qtmemoformData"];

			var qtmemoforminfo = DataConverterHelper.JsonToEntity<QTMemoModel>(qtmemoformData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
                if(qtmemoforminfo.AllRow[0].PersistentState== PersistentState.Added)
                {
                    qtmemoforminfo.AllRow[0].MenoStatus = "0";
                }
				savedresult = QTMemoService.Save<Int64>(qtmemoforminfo.AllRow,"");
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

            var deletedresult = QTMemoService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 保存金格控件的word数据phid
        /// </summary>
        /// <returns>返回Json串</returns>
        public string SaveWord()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);
            long WordPhid = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["WordPhid"]);
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            try
            {
                QTMemoModel qTMemo = QTMemoService.Find(id).Data;
                qTMemo.WordPhid = WordPhid;
                qTMemo.PersistentState = PersistentState.Modified;
                savedresult = QTMemoService.Save<Int64>(qTMemo, "");
            }
            catch (Exception ex)
            {
                savedresult.Status = ResponseStatus.Error;
                savedresult.Msg = ex.Message.ToString();
            }
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save2()
        {
            string adddata = System.Web.HttpContext.Current.Request.Params["adddata"];
            string updatedata = System.Web.HttpContext.Current.Request.Params["updatedata"];
            string deletedata = System.Web.HttpContext.Current.Request.Params["deletedata"];
            var addinfo = JsonConvert.DeserializeObject<List<QTMemoModel>>(adddata);
            var updateinfo = JsonConvert.DeserializeObject<List<QTMemoModel>>(updatedata);
            var deleteinfo = JsonConvert.DeserializeObject<List<string>>(deletedata);

            CommonResult savedresult = new CommonResult();
            savedresult = QTMemoService.Save2(addinfo, updateinfo, deleteinfo);
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 根据提醒方式判断是否存在备忘录需要提醒
        /// </summary>
        /// <returns></returns>
        public string Remind()
        {
            long Creator= Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["userid"]);
            string MenoRemind = System.Web.HttpContext.Current.Request.Params["MenoRemind"];
            Dictionary<string, object> dicwhere = new Dictionary<string, object>();
            new CreateCriteria(dicwhere)
                .Add(ORMRestrictions<System.Int64>.Eq("Creator", Creator))
                .Add(ORMRestrictions<string>.Eq("MenoStatus", "0"))
                .Add(ORMRestrictions<string>.Like("MenoRemind", MenoRemind));
            IList<QTMemoModel> data = QTMemoService.Find(dicwhere).Data;
            if (data.Count > 0)
            {
                return "true";
            }
            else{
                return "false";
            }

        }
    }
}

