#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Controller
    * 类 名 称：			QTProjectMstController
    * 文 件 名：			QTProjectMstController.cs
    * 创建时间：			2019/9/4 
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
using GQT3.QT.Model.Extra;
using Enterprise3.Common.Base.Criterion;

namespace GQT3.QT.Controller
{
	/// <summary>
	/// QTProjectMst控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class QTProjectMstController : AFCommonController
    {
        IQTProjectMstService QTProjectMstService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public QTProjectMstController()
	    {
	        QTProjectMstService = base.GetObject<IQTProjectMstService>("GQT3.QT.Service.QTProjectMst");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QTProjectMstList()
        {
			ViewBag.Title = base.GetMenuLanguage("QTProjectMst");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "快照";
            }
            base.InitialMultiLanguage("QTProjectMst");
            ViewBag.IndividualInfo = this.GetIndividualUI("QTProjectMst");
            return View("QTProjectMstList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QTProjectMstEdit()
        {
			var tabTitle = base.GetMenuLanguage("QTProjectMst");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "快照";
            }
            base.SetUserDefScriptUrl("QTProjectMst");
            base.InitialMultiLanguage("QTProjectMst");
            ViewBag.IndividualInfo = this.GetIndividualUI("QTProjectMst");

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

            return View("QTProjectMstEdit");
        }

        /// <summary>
        /// 指向申报部门项目汇总表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QTProjectMstHZ()
        {
            ViewBag.Title = base.GetMenuLanguage("QTProjectMst");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "申报部门项目汇总表";
            }
            base.InitialMultiLanguage("QTProjectMst");
            ViewBag.IndividualInfo = this.GetIndividualUI("QTProjectMst");
            return View("QTProjectMstHZ");
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTProjectMstList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = QTProjectMstService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<QTProjectMstModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTProjectMstInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			switch (tabtype)
			{
				case "qtprojectmst":
					var findedresultqtprojectmst = QTProjectMstService.Find(id);
					return DataConverterHelper.ResponseResultToJson(findedresultqtprojectmst);
				case "qtprojectdtlbudgetdtl":
					var findedresultqtprojectdtlbudgetdtl = QTProjectMstService.FindQTProjectDtlBudgetDtlByForeignKey(id);
					return DataConverterHelper.EntityListToJson(findedresultqtprojectdtlbudgetdtl.Data, findedresultqtprojectdtlbudgetdtl.Data.Count);
				case "qtprojectdtlfundappl":
					var findedresultqtprojectdtlfundappl = QTProjectMstService.FindQTProjectDtlFundApplByForeignKey(id);
					return DataConverterHelper.EntityListToJson(findedresultqtprojectdtlfundappl.Data, findedresultqtprojectdtlfundappl.Data.Count);
				case "qtprojectdtlimplplan":
					var findedresultqtprojectdtlimplplan = QTProjectMstService.FindQTProjectDtlImplPlanByForeignKey(id);
					return DataConverterHelper.EntityListToJson(findedresultqtprojectdtlimplplan.Data, findedresultqtprojectdtlimplplan.Data.Count);
				case "qtprojectdtlperformtarget":
					var findedresultqtprojectdtlperformtarget = QTProjectMstService.FindQTProjectDtlPerformTargetByForeignKey(id);
					return DataConverterHelper.EntityListToJson(findedresultqtprojectdtlperformtarget.Data, findedresultqtprojectdtlperformtarget.Data.Count);
				case "qtprojectdtlpurchasedtl":
					var findedresultqtprojectdtlpurchasedtl = QTProjectMstService.FindQTProjectDtlPurchaseDtlByForeignKey(id);
					return DataConverterHelper.EntityListToJson(findedresultqtprojectdtlpurchasedtl.Data, findedresultqtprojectdtlpurchasedtl.Data.Count);
				case "qtprojectdtlpurdtl4sof":
					var findedresultqtprojectdtlpurdtl4sof = QTProjectMstService.FindQTProjectDtlPurDtl4SOFByForeignKey(id);
					return DataConverterHelper.EntityListToJson(findedresultqtprojectdtlpurdtl4sof.Data, findedresultqtprojectdtlpurdtl4sof.Data.Count);
				case "qtprojectdtltextcontent":
					var findedresultqtprojectdtltextcontent = QTProjectMstService.FindQTProjectDtlTextContentByForeignKey(id);
					return DataConverterHelper.EntityListToJson(findedresultqtprojectdtltextcontent.Data, findedresultqtprojectdtltextcontent.Data.Count);
				default:
					FindedResult findedresultother = new FindedResult();
					return DataConverterHelper.ResponseResultToJson(findedresultother);
			}
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string qtprojectmstformData = System.Web.HttpContext.Current.Request.Form["qtprojectmstformData"];
			string qtprojectdtlbudgetdtlgridData = System.Web.HttpContext.Current.Request.Form["qtprojectdtlbudgetdtlgridData"];
			string qtprojectdtlfundapplgridData = System.Web.HttpContext.Current.Request.Form["qtprojectdtlfundapplgridData"];
			string qtprojectdtlimplplangridData = System.Web.HttpContext.Current.Request.Form["qtprojectdtlimplplangridData"];
			string qtprojectdtlperformtargetgridData = System.Web.HttpContext.Current.Request.Form["qtprojectdtlperformtargetgridData"];
			string qtprojectdtlpurchasedtlgridData = System.Web.HttpContext.Current.Request.Form["qtprojectdtlpurchasedtlgridData"];
			string qtprojectdtlpurdtl4sofgridData = System.Web.HttpContext.Current.Request.Form["qtprojectdtlpurdtl4sofgridData"];
			string qtprojectdtltextcontentgridData = System.Web.HttpContext.Current.Request.Form["qtprojectdtltextcontentgridData"];

			var qtprojectmstforminfo = DataConverterHelper.JsonToEntity<QTProjectMstModel>(qtprojectmstformData);
			var qtprojectdtlbudgetdtlgridinfo = DataConverterHelper.JsonToEntity<QTProjectDtlBudgetDtlModel>(qtprojectdtlbudgetdtlgridData);
			var qtprojectdtlfundapplgridinfo = DataConverterHelper.JsonToEntity<QTProjectDtlFundApplModel>(qtprojectdtlfundapplgridData);
			var qtprojectdtlimplplangridinfo = DataConverterHelper.JsonToEntity<QTProjectDtlImplPlanModel>(qtprojectdtlimplplangridData);
			var qtprojectdtlperformtargetgridinfo = DataConverterHelper.JsonToEntity<QTProjectDtlPerformTargetModel>(qtprojectdtlperformtargetgridData);
			var qtprojectdtlpurchasedtlgridinfo = DataConverterHelper.JsonToEntity<QTProjectDtlPurchaseDtlModel>(qtprojectdtlpurchasedtlgridData);
			var qtprojectdtlpurdtl4sofgridinfo = DataConverterHelper.JsonToEntity<QTProjectDtlPurDtl4SOFModel>(qtprojectdtlpurdtl4sofgridData);
			var qtprojectdtltextcontentgridinfo = DataConverterHelper.JsonToEntity<QTProjectDtlTextContentModel>(qtprojectdtltextcontentgridData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = QTProjectMstService.SaveQTProjectMst(qtprojectmstforminfo.AllRow[0],qtprojectdtlbudgetdtlgridinfo.AllRow,qtprojectdtlfundapplgridinfo.AllRow,qtprojectdtlimplplangridinfo.AllRow,qtprojectdtlperformtargetgridinfo.AllRow,qtprojectdtlpurchasedtlgridinfo.AllRow,qtprojectdtlpurdtl4sofgridinfo.AllRow,qtprojectdtltextcontentgridinfo.AllRow);
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

            var deletedresult = QTProjectMstService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string SaveDtlStage()
        {
            string Modified = System.Web.HttpContext.Current.Request.Params["Modified"];
            var ModifiedList = JsonConvert.DeserializeObject<List<QTProjectMstModel>>(Modified);
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            if (ModifiedList.Count > 0)
            {
                var saveDataList = new List<QTProjectMstModel>();
                var saveData = new QTProjectMstModel();
                foreach (var a in ModifiedList)
                {
                    saveData = QTProjectMstService.Find(a.PhId).Data;
                    saveData.FDtlstage = a.FDtlstage;
                    saveData.PersistentState = PersistentState.Modified;
                    saveDataList.Add(saveData);
                }
                savedresult = QTProjectMstService.Save<Int64>(saveDataList, "");
            }
            return DataConverterHelper.SerializeObject(savedresult); 
        }


        /// <summary>
        /// 申报部门项目汇总表
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTProjectMstHZ()
        {
            /*string FDeclarationDept = System.Web.HttpContext.Current.Request.Params["FDeclarationDept"];
            string FBudgetDept = System.Web.HttpContext.Current.Request.Params["FBudgetDept"];
            string FAccount = System.Web.HttpContext.Current.Request.Params["FAccount"];
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(FDeclarationDept))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FDeclarationDept", FDeclarationDept));
            }
            if (!string.IsNullOrEmpty(FBudgetDept))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FBudgetDept", FBudgetDept));
            }
            if (!string.IsNullOrEmpty(FAccount))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FAccount", FAccount));
            }*/
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary
            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = QTProjectMstService.GetQTProjectMstHZ(dicWhere, storeparam.PageIndex, storeparam.PageSize, out int TotalItems);

            return DataConverterHelper.EntityListToJson<QTProjectMstHZModel>(result, TotalItems);
        }

    }
}

