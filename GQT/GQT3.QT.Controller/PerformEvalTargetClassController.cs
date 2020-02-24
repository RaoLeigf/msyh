#region Summary
/**************************************************************************************
    * 类 名 称：        PerformEvalTargetClassController
    * 命名空间：        GQT3.QT.Controller
    * 文 件 名：        PerformEvalTargetClassController.cs
    * 创建时间：        2018/10/16 
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
using GXM3.XM.Service.Interface;
using System.Linq;

namespace GQT3.QT.Controller
{
	/// <summary>
	/// PerformEvalTargetClass控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class PerformEvalTargetClassController : AFCommonController
    {
        IPerformEvalTargetClassService PerformEvalTargetClassService { get; set; }
        IPerformEvalTargetService performEvalTargetService { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public PerformEvalTargetClassController()
	    {
	        PerformEvalTargetClassService = base.GetObject<IPerformEvalTargetClassService>("GQT3.QT.Service.PerformEvalTargetClass");
            performEvalTargetService = base.GetObject<IPerformEvalTargetService>("GQT3.QT.Service.PerformEvalTarget");
        }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult PerformEvalTargetClassList()
        {
			ViewBag.Title = base.GetMenuLanguage("PerformEvalTargetClass");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "绩效项目评价指标类别";
            }
            base.InitialMultiLanguage("PerformEvalTargetClass");
            ViewBag.IndividualInfo = this.GetIndividualUI("PerformEvalTargetClass");
            return View("PerformEvalTargetClassList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult PerformEvalTargetClassEdit()
        {
			var tabTitle = base.GetMenuLanguage("PerformEvalTargetClass");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "绩效项目评价指标类别";
            }
            base.SetUserDefScriptUrl("PerformEvalTargetClass");
            base.InitialMultiLanguage("PerformEvalTargetClass");
            ViewBag.IndividualInfo = this.GetIndividualUI("PerformEvalTargetClass");

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

            return View("PerformEvalTargetClassEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetPerformEvalTargetClassList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = PerformEvalTargetClassService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<PerformEvalTargetClassModel>(result.Results, (Int32)result.TotalItems);
        }

        public PagedResult<PerformEvalTargetClassModel> GetList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = PerformEvalTargetClassService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);
            return result;
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetPerformEvalTargetClassInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = PerformEvalTargetClassService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string performevaltargetclassformData = System.Web.HttpContext.Current.Request.Form["performevaltargetclassformData"];

			var performevaltargetclassforminfo = DataConverterHelper.JsonToEntity<PerformEvalTargetClassModel>(performevaltargetclassformData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();

            PagedResult<PerformEvalTargetClassModel> result=this.GetList();
            
            try
			{
                foreach (PerformEvalTargetClassModel performEvalTargetClass in performevaltargetclassforminfo.AllRow)
                {
                    //where条件已经包括新增以及修改，反证法
                    var pt = from pt1 in result.Results
                             where (pt1.FName == performEvalTargetClass.FName || pt1.FCode == performEvalTargetClass.FCode) && pt1.PhId != performEvalTargetClass.PhId
                             select pt1;
                    if (pt.Count() > 0)
                    {
                       
                        throw new Exception("代码或名称重复");
                    }
                }
                savedresult = PerformEvalTargetClassService.Save<Int64>(performevaltargetclassforminfo.AllRow,"");
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
            string codes = System.Web.HttpContext.Current.Request.Params["code"].ToString();
            string names = "";
            if (CanDelete(codes, out  names))
            {
                var deletedresult = PerformEvalTargetClassService.Delete<System.Int64>(id);

                return DataConverterHelper.SerializeObject(deletedresult);
            }
            else
            {
                return "{\"result\":\"fail\",\"names\":\"" + names + "\"}";
            }

            //var deletedresult = PerformEvalTargetClassService.Delete<System.Int64>(id);

            //return DataConverterHelper.SerializeObject(deletedresult);
        }

        public bool CanDelete(string data, out string names)
        {
            string[] codes;
            if (data.EndsWith(","))
            {
                data = data.Substring(0, data.Length - 1);
            }
            codes = data.Split(',');
            var result = this.performEvalTargetService.FindPerformEvalTargetByAnyCode(codes, "FTargetClassCode");
            names = "";
            if (result.Data.Count > 0)
            {
                foreach (var ppd in result.Data)
                {
                    //666简化null检查
                    names += (ppd.FTargetName ?? "未命名") + ",";
                }
                if (names.EndsWith(","))
                {
                    names = names.Substring(0, names.Length - 1);
                }
                return false;
            }
            else
            {
                return true;
            }
        }


    }
}

