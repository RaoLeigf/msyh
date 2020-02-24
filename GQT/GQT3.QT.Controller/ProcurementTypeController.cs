#region Summary
/**************************************************************************************
    * 类 名 称：        ProcurementTypeController
    * 命名空间：        GQT3.QT.Controller
    * 文 件 名：        ProcurementTypeController.cs
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
using GXM3.XM.Service.Interface;
using GXM3.XM.Model.Domain;

namespace GQT3.QT.Controller
{
	/// <summary>
	/// ProcurementType控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ProcurementTypeController : AFCommonController
    {
        IProcurementTypeService ProcurementTypeService { get; set; }
        IProjectMstService ProjectMstService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcurementTypeController()
	    {
	        ProcurementTypeService = base.GetObject<IProcurementTypeService>("GQT3.QT.Service.ProcurementType");
            ProjectMstService = base.GetObject<IProjectMstService>("GXM3.XM.Service.ProjectMst");
        }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult ProcurementTypeList()
        {
			ViewBag.Title = base.GetMenuLanguage("ProcurementType");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "采购类型";
            }
            base.InitialMultiLanguage("ProcurementType");
            ViewBag.IndividualInfo = this.GetIndividualUI("ProcurementType");
            return View("ProcurementTypeList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult ProcurementTypeEdit()
        {
			var tabTitle = base.GetMenuLanguage("ProcurementType");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "采购类型";
            }
            base.SetUserDefScriptUrl("ProcurementType");
            base.InitialMultiLanguage("ProcurementType");
            ViewBag.IndividualInfo = this.GetIndividualUI("ProcurementType");

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

            return View("ProcurementTypeEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetProcurementTypeList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = ProcurementTypeService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere, new string[] { "FCode Asc" });

            return DataConverterHelper.EntityListToJson<ProcurementTypeModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetProcurementTypeInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = ProcurementTypeService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string procurementtypeformData = System.Web.HttpContext.Current.Request.Form["procurementtypeformData"];
            string otype = System.Web.HttpContext.Current.Request.Form["otype"];

            var procurementtypeforminfo = DataConverterHelper.JsonToEntity<ProcurementTypeModel>(procurementtypeformData);
            List<ProcurementTypeModel> procurementTypes = procurementtypeforminfo.AllRow;
            var checkresult = ProcurementTypeService.ExecuteDataCheck(ref procurementTypes, otype);
            if (checkresult != null && checkresult.Status == ResponseStatus.Error)
            {
                return DataConverterHelper.SerializeObject(checkresult);
            }

            var savedresult = ProcurementTypeService.Save<Int64>(procurementtypeforminfo.AllRow,"");

            /*SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = ProcurementTypeService.Save<Int64>(procurementtypeforminfo.AllRow);
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

            FindedResults<ProcurementTypeModel> procurementType = ProcurementTypeService.Find(t => t.PhId == id, "");
            string FTypeCode = procurementType.Data[0].FCode;
            FindedResults<ProjectDtlPurchaseDtlModel> findedResults = ProjectMstService.FindProjectDtlPurchaseDtlByFTypeCode(FTypeCode);
            if (findedResults.Data.Count > 0)
            {
                findedResults.Status = "failure";
                findedResults.Msg = "当前采购类型已被引用，无法删除！";
                return DataConverterHelper.SerializeObject(findedResults);
            }

            var deletedresult = ProcurementTypeService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

    }
}

