#region Summary
/**************************************************************************************
    * 类 名 称：        PerformEvalTargetController
    * 命名空间：        GQT3.QT.Controller
    * 文 件 名：        PerformEvalTargetController.cs
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
using Enterprise3.Common.Base.Criterion;
using GXM3.XM.Service.Interface;
using GXM3.XM.Model.Domain;

namespace GQT3.QT.Controller
{
	/// <summary>
	/// PerformEvalTarget控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class PerformEvalTargetController : AFCommonController
    {
        IPerformEvalTargetService PerformEvalTargetService { get; set; }
        IProjectMstService ProjectMstService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PerformEvalTargetController()
	    {
	        PerformEvalTargetService = base.GetObject<IPerformEvalTargetService>("GQT3.QT.Service.PerformEvalTarget");
            ProjectMstService = base.GetObject<IProjectMstService>("GXM3.XM.Service.ProjectMst");
        }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult PerformEvalTargetList()
        {
			ViewBag.Title = base.GetMenuLanguage("PerformEvalTarget");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "绩效项目评价指标";
            }
            base.InitialMultiLanguage("PerformEvalTarget");
            ViewBag.IndividualInfo = this.GetIndividualUI("PerformEvalTarget");
            return View("PerformEvalTargetList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult PerformEvalTargetEdit()
        {
			var tabTitle = base.GetMenuLanguage("PerformEvalTarget");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "绩效项目评价指标";
            }
            base.SetUserDefScriptUrl("PerformEvalTarget");
            base.InitialMultiLanguage("PerformEvalTarget");
            ViewBag.IndividualInfo = this.GetIndividualUI("PerformEvalTarget");

			ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型

            ViewBag.FTargetTypeCode = System.Web.HttpContext.Current.Request.Params["FTargetTypeCode"];//指标类别代码
            ViewBag.FTargetTypeName = System.Web.HttpContext.Current.Request.Params["FTargetTypeName"];//指标类别名称

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

            return View("PerformEvalTargetEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetPerformEvalTargetList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = PerformEvalTargetService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<PerformEvalTargetModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetPerformEvalTargetInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = PerformEvalTargetService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string performevaltargetformData = System.Web.HttpContext.Current.Request.Form["performevaltargetformData"];
            string otype = System.Web.HttpContext.Current.Request.Form["otype"];

            var performevaltargetforminfo = DataConverterHelper.JsonToEntity<PerformEvalTargetModel>(performevaltargetformData);
            List<PerformEvalTargetModel> performEvalTarget = performevaltargetforminfo.AllRow;
            var checkresult = PerformEvalTargetService.ExecuteDataCheck(ref performEvalTarget, otype);
            if (checkresult != null && checkresult.Status == ResponseStatus.Error)
            {
                return DataConverterHelper.SerializeObject(checkresult);
            }
            var savedresult = PerformEvalTargetService.Save<Int64>(performevaltargetforminfo.AllRow,"");
            /*SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = PerformEvalTargetService.Save<Int64>(performevaltargetforminfo.AllRow);
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

            FindedResults<PerformEvalTargetModel> performEvalTarget = PerformEvalTargetService.Find(t => t.PhId == id, "");
            string FTargetCode= performEvalTarget.Data[0].FTargetCode;
            FindedResults<ProjectDtlPerformTargetModel> findedResults = ProjectMstService.FindProjectDtlPerformTargetByFTargetCode(FTargetCode);
            if (findedResults != null&&findedResults.Data.Count > 0)
            {
                findedResults.Status = "failure";
                findedResults.Msg = "当前指标已被引用，无法删除！";
                return DataConverterHelper.SerializeObject(findedResults);
            }

            var deletedresult = PerformEvalTargetService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 取列表数据根据绩效项目评价指标类型
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetPerformEvalTargetListByClassCode()
        {
            string TypeCode = System.Web.HttpContext.Current.Request.Params["TypeCode"];//查询条件
            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).
                        Add(ORMRestrictions<string>.Eq("FTargetTypeCode", TypeCode));

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = PerformEvalTargetService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere, new string[] { "FTargetCode" });

            result = PerformEvalTargetService.CodeToName(result);

            return DataConverterHelper.EntityListToJson<PerformEvalTargetModel>(result.Results, (Int32)result.TotalItems);
        }
    }
}

