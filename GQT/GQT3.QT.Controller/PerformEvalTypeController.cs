#region Summary
/**************************************************************************************
    * 类 名 称：        PerformEvalTypeController
    * 命名空间：        GQT3.QT.Controller
    * 文 件 名：        PerformEvalTypeController.cs
    * 创建时间：        2018/10/16 
    * 作    者：        李长敏琛    
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
using System.Reflection;
using GXM3.XM.Model.Domain;
using System.Linq;

namespace GQT3.QT.Controller
{
	/// <summary>
	/// PerformEvalType控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class PerformEvalTypeController : AFCommonController
    {
        IPerformEvalTypeService PerformEvalTypeService { get; set; }
        IProjectMstService projectMstService { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public PerformEvalTypeController()
	    {
            projectMstService = base.GetObject<IProjectMstService>("GXM3.XM.Service.ProjectMst");
            PerformEvalTypeService = base.GetObject<IPerformEvalTypeService>("GQT3.QT.Service.PerformEvalType");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult PerformEvalTypeList()
        {
			ViewBag.Title = base.GetMenuLanguage("PerformEvalType");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "绩效评价类型";
            }
            base.InitialMultiLanguage("PerformEvalType");
            ViewBag.IndividualInfo = this.GetIndividualUI("PerformEvalType");
            return View("PerformEvalTypeList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult PerformEvalTypeEdit()
        {
			var tabTitle = base.GetMenuLanguage("PerformEvalType");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "绩效评价类型";
            }
            base.SetUserDefScriptUrl("PerformEvalType");
            base.InitialMultiLanguage("PerformEvalType");
            ViewBag.IndividualInfo = this.GetIndividualUI("PerformEvalType");

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

            return View("PerformEvalTypeEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetPerformEvalTypeList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = PerformEvalTypeService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<PerformEvalTypeModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public PagedResult<PerformEvalTypeModel> GetList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = PerformEvalTypeService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);
            return result;

        }
        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetPerformEvalTypeInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = PerformEvalTypeService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string performevaltypeformData = System.Web.HttpContext.Current.Request.Form["performevaltypeformData"];

			var performevaltypeforminfo = DataConverterHelper.JsonToEntity<PerformEvalTypeModel>(performevaltypeformData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
            PagedResult<PerformEvalTypeModel> result = this.GetList();
            //this.GetList(out PagedResult<PerformEvalTypeModel> result);
            
            try
			{
                foreach (PerformEvalTypeModel performEvalType in performevaltypeforminfo.AllRow)
                {
                    //where条件已经包括新增以及修改，反证法
                    var pt = from pt1 in result.Results
                             where (pt1.FName == performEvalType.FName || pt1.FCode == performEvalType.FCode) && pt1.PhId != performEvalType.PhId
                             select pt1;
                    if (pt.Count() > 0)
                    {
                        throw new Exception("代码或名称重复");
                    }
                }
                savedresult = PerformEvalTypeService.Save<Int64>(performevaltypeforminfo.AllRow,"");
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
            if (CanDelete(codes, out names))
            {
                var deletedresult = PerformEvalTypeService.Delete<System.Int64>(id);

                return DataConverterHelper.SerializeObject(deletedresult);
            }
            else
            {
                return "{\"result\":\"fail\",\"names\":\"" + names + "\"}";
            }
        }


        //GXM.fac xml配置注入不了，所以目前暂时采用qt.controller调用xm.service,不采用qt.service调用xm.fac

        public bool CanDelete(string data, out string names)
        {
            string[] codes;
            if (data.EndsWith(","))
            {
                data = data.Substring(0, data.Length - 1);
            }
            codes = data.Split(',');
            var result = this.projectMstService.FindProjectMstByProperty(codes, "FPerformEvalType");
            names = "";
            if (result.Data.Count > 0)
            {
                foreach (var ppd in result.Data)
                {
                    //666简化null检查
                    names += (ppd.FProjName ?? "未命名") + ",";
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

