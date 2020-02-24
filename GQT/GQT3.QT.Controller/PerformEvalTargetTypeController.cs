#region Summary
/**************************************************************************************
    * 类 名 称：        PerformEvalTargetTypeController
    * 命名空间：        GQT3.QT.Controller
    * 文 件 名：        PerformEvalTargetTypeController.cs
    * 创建时间：        2018/10/15 
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
using System.Data;

namespace GQT3.QT.Controller
{
	/// <summary>
	/// PerformEvalTargetType控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class PerformEvalTargetTypeController : AFCommonController
    {
        IPerformEvalTargetTypeService PerformEvalTargetTypeService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public PerformEvalTargetTypeController()
	    {
	        PerformEvalTargetTypeService = base.GetObject<IPerformEvalTargetTypeService>("GQT3.QT.Service.PerformEvalTargetType");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult PerformEvalTargetTypeList()
        {
			ViewBag.Title = base.GetMenuLanguage("PerformEvalTargetType");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "绩效项目评价指标类型";
            }
            base.InitialMultiLanguage("PerformEvalTargetType");
            ViewBag.IndividualInfo = this.GetIndividualUI("PerformEvalTargetType");
            return View("PerformEvalTargetTypeList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult PerformEvalTargetTypeEdit()
        {
			var tabTitle = base.GetMenuLanguage("PerformEvalTargetType");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "绩效项目评价指标类型";
            }
            base.SetUserDefScriptUrl("PerformEvalTargetType");
            base.InitialMultiLanguage("PerformEvalTargetType");
            ViewBag.IndividualInfo = this.GetIndividualUI("PerformEvalTargetType");

			ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型
            ViewBag.FParentCode = System.Web.HttpContext.Current.Request.Params["FParentCode"];//上级代码
            ViewBag.FParentName = System.Web.HttpContext.Current.Request.Params["FParentName"];//上级名称

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

            return View("PerformEvalTargetTypeEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetPerformEvalTargetTypeList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = PerformEvalTargetTypeService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<PerformEvalTargetTypeModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetPerformEvalTargetTypeInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = PerformEvalTargetTypeService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string performevaltargettypeformData = System.Web.HttpContext.Current.Request.Form["performevaltargettypeformData"];
            string otype = System.Web.HttpContext.Current.Request.Form["otype"];

            var performevaltargettypeforminfo = DataConverterHelper.JsonToEntity<PerformEvalTargetTypeModel>(performevaltargettypeformData);

            List<PerformEvalTargetTypeModel> performEvalTargetType = performevaltargettypeforminfo.AllRow;
            var checkresult = PerformEvalTargetTypeService.ExecuteDataCheck(ref performEvalTargetType, otype);
            if (checkresult != null && checkresult.Status == ResponseStatus.Error)
            {
                return DataConverterHelper.SerializeObject(checkresult);
            }

            var savedresult = PerformEvalTargetTypeService.Save<Int64>(performevaltargettypeforminfo.AllRow,"");

            /*SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = PerformEvalTargetTypeService.Save<Int64>(performevaltargettypeforminfo.AllRow);
			}
			catch (Exception ex)
			{
				savedresult.Status = ResponseStatus.Error;
				savedresult.Msg = ex.Message.ToString();
			}*/
            return DataConverterHelper.SerializeObject(savedresult);
        }

		/// <summary>
        /// 删除数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Delete()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            var deletedresult = PerformEvalTargetTypeService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 返回树Json
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadTree()
        {
            string nodeid = System.Web.HttpContext.Current.Request.Params["node"];
            IList<TreeJSONBase> list = this.GetTreeData(nodeid);

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取项目树数据
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        public IList<TreeJSONBase> GetTreeData(string nodeid)
        {

            FindedResults<PerformEvalTargetTypeModel> results = null;
            if (string.IsNullOrEmpty(nodeid) || nodeid == "root")
            {
                results = PerformEvalTargetTypeService.Find(t => t.FParentCode == null || t.FParentCode == "");
            }
            else
            {
                results = PerformEvalTargetTypeService.Find(t => t.FParentCode == nodeid);
            }

            if (results.Data.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("PhId", typeof(System.String));
                dt.Columns.Add("FCode", typeof(System.String));
                dt.Columns.Add("FName", typeof(System.String));
                dt.Columns.Add("FParentCode", typeof(System.String));

                DataRow dr = null;
                foreach (var m in results.Data)
                {
                    dr = dt.NewRow();
                    dr["PhId"] = m.PhId;
                    dr["FCode"] = m.FCode;
                    dr["FName"] = m.FName;
                    dr["FParentCode"] = m.FParentCode;
                    dt.Rows.Add(dr);
                }

                string filter = string.Empty;
                return new PerformEvalTargetTypeTreeBuilder().GetExtTreeList(dt, "FParentCode", "FCode", filter, TreeDataLevelType.LazyLevel);
            }

            return null;
        }
    }
}

