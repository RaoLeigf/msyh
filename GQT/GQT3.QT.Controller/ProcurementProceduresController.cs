#region Summary
/**************************************************************************************
    * 类 名 称：        ProcurementProceduresController
    * 命名空间：        GQT3.QT.Controller
    * 文 件 名：        ProcurementProceduresController.cs
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
using System.Linq;

namespace GQT3.QT.Controller
{
	/// <summary>
	/// ProcurementProcedures控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ProcurementProceduresController : AFCommonController
    {
        IProcurementProceduresService ProcurementProceduresService { get; set; }
        IProjectMstService projectMstService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcurementProceduresController()
	    {
	        ProcurementProceduresService = base.GetObject<IProcurementProceduresService>("GQT3.QT.Service.ProcurementProcedures");
            projectMstService = base.GetObject<IProjectMstService>("GXM3.XM.Service.ProjectMst");
        }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult ProcurementProceduresList()
        {
			ViewBag.Title = base.GetMenuLanguage("ProcurementProcedures");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "采购程序";
            }
            base.InitialMultiLanguage("ProcurementProcedures");
            ViewBag.IndividualInfo = this.GetIndividualUI("ProcurementProcedures");
            return View("ProcurementProceduresList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult ProcurementProceduresEdit()
        {
			var tabTitle = base.GetMenuLanguage("ProcurementProcedures");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "采购程序";
            }
            base.SetUserDefScriptUrl("ProcurementProcedures");
            base.InitialMultiLanguage("ProcurementProcedures");
            ViewBag.IndividualInfo = this.GetIndividualUI("ProcurementProcedures");

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

            return View("ProcurementProceduresEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetProcurementProceduresList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = ProcurementProceduresService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere, new string[] { "FCode Asc" });

            return DataConverterHelper.EntityListToJson<ProcurementProceduresModel>(result.Results, (Int32)result.TotalItems);
        }

        public PagedResult<ProcurementProceduresModel> GetList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = ProcurementProceduresService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);
            return result;
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetProcurementProceduresInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = ProcurementProceduresService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string procurementproceduresformData = System.Web.HttpContext.Current.Request.Form["procurementproceduresformData"];

			var procurementproceduresforminfo = DataConverterHelper.JsonToEntity<ProcurementProceduresModel>(procurementproceduresformData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
            PagedResult<ProcurementProceduresModel> result=this.GetList();
            
            try
			{
                foreach (ProcurementProceduresModel procurementProcedures in procurementproceduresforminfo.AllRow)
                {
                    //where条件已经包括新增以及修改，反证法
                    var pt = from pt1 in result.Results
                             where (pt1.FName == procurementProcedures.FName || pt1.FCode == procurementProcedures.FCode) && pt1.PhId != procurementProcedures.PhId
                             select pt1;
                    if (pt.Count() > 0)
                    {
                        throw new Exception("代码或名称重复");
                    }
                }
                savedresult = ProcurementProceduresService.Save<Int64>(procurementproceduresforminfo.AllRow,"");
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

            //var deletedresult = ProcurementProceduresService.Delete<System.Int64>(id);



            //return DataConverterHelper.SerializeObject(deletedresult);
            string codes = System.Web.HttpContext.Current.Request.Params["code"].ToString();
            string names = "";
            if (CanDelete(codes, out names))
            {
                var deletedresult = ProcurementProceduresService.Delete<System.Int64>(id);

                return DataConverterHelper.SerializeObject(deletedresult);
            }
            else
            {
                return "{\"result\":\"fail\",\"names\":\"" + names + "\"}";
            }
        }

        public bool CanDelete(string data, out string names)
        {
            string[] codes;
            if (data.EndsWith(","))
            {
                data = data.Substring(0, data.Length - 1);
            }
            codes = data.Split(',');
            var result = this.projectMstService.FindProjectDtlPurchaseDtlByAnyCode(codes, "FProcedureCode");
            names = "";
            if (result.Data.Count > 0)
            {
                foreach (var ppd in result.Data)
                {
                    //666简化null检查
                    names += (ppd.FName ?? "未命名") + ",";
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

