#region Summary
/**************************************************************************************
    * 类 名 称：        PerformanceMstController
    * 命名空间：        GJX3.JX.Controller
    * 文 件 名：        PerformanceMstController.cs
    * 创建时间：        2018/9/12 
    * 作    者：        吾丰明    
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

using GJX3.JX.Service.Interface;
using GJX3.JX.Model.Domain;
using GYS3.YS.Service.Interface;
using Enterprise3.Common.Base.Criterion;
using GYS3.YS.Model.Domain;
using GYS3.YS.Model.Enums;
using GQT3.QT.Service.Interface;

namespace GJX3.JX.Controller
{
	/// <summary>
	/// PerformanceMst控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class PerformanceMstController : AFCommonController
    {
        IPerformanceMstService PerformanceMstService { get; set; }
        IBudgetMstService BudgetMstService { get; set; }
        ICorrespondenceSettingsService CorrespondenceSettingsService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PerformanceMstController()
	    {
	        PerformanceMstService = base.GetObject<IPerformanceMstService>("GJX3.JX.Service.PerformanceMst");
            BudgetMstService = base.GetObject<IBudgetMstService>("GYS3.YS.Service.BudgetMst");
            CorrespondenceSettingsService = base.GetObject<ICorrespondenceSettingsService>("GQT3.QT.Service.CorrespondenceSettings");
        }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult PerformanceMstList()
        {
			ViewBag.Title = base.GetMenuLanguage("GHPerformanceMst");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "绩效评价";
            }
            base.InitialMultiLanguage("GHPerformanceMst");
            ViewBag.IndividualInfo = this.GetIndividualUI("GHPerformanceMst");
            return View("PerformanceMstList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult PerformanceMstEdit()
        {
			var tabTitle = base.GetMenuLanguage("GHPerformanceMst");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "绩效评价";
            }
            base.SetUserDefScriptUrl("GHPerformanceMst");
            base.InitialMultiLanguage("GHPerformanceMst");
            ViewBag.IndividualInfo = this.GetIndividualUI("GHPerformanceMst");

			ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型
            ViewBag.YSMstPhId = System.Web.HttpContext.Current.Request.Params["ysmstphid"];//预算项目主键

            ViewBag.FType= System.Web.HttpContext.Current.Request.Params["FType"];
            ViewBag.selfphid = System.Web.HttpContext.Current.Request.Params["selfphid"];//自评单据的主键
            if (!string.IsNullOrEmpty(ViewBag.FType))
            {
                if (ViewBag.FType == "1")
                {
                    tabTitle = "绩效自评";
                }
                else if(ViewBag.FType == "2")
                {
                    tabTitle = "绩效抽评";
                }
                
            }

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

            return View("PerformanceMstEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetPerformanceMstList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            string ysmstphid = System.Web.HttpContext.Current.Request.Params["ysmstphid"];  //预算项目主键
            string FType = System.Web.HttpContext.Current.Request.Params["FType"];
            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

           
            if (ysmstphid != null && !string.IsNullOrEmpty(ysmstphid))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<Int64>.Eq("YSMstPhId", Int64.Parse(ysmstphid)));
            }
            if (!string.IsNullOrEmpty(FType))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FType", FType));
            }
            
            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = PerformanceMstService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" });

            return DataConverterHelper.EntityListToJson<PerformanceMstModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 获取预算数据
        /// </summary>
        /// <returns></returns>
        public string GetBudgetMstList()
        {
            //string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            //Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            var workType = System.Web.HttpContext.Current.Request.Params["workType"]; //业务种类(年初,年中,特殊)// c - 年初,z - 年中
            var userId = System.Web.HttpContext.Current.Request.Params["userId"];
            var ShowAll = System.Web.HttpContext.Current.Request.Params["ShowAll"];
            var FBudgetDept= System.Web.HttpContext.Current.Request.Params["FBudgetDept"];
            var dicWhereDept = new Dictionary<string, object>();
            var dicWhere = new Dictionary<string, object>();
            if (userId != null && !string.IsNullOrEmpty(userId))
            {
                new CreateCriteria(dicWhereDept)
                .Add(ORMRestrictions<string>.Eq("Dwdm", userId)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
                var deptList = CorrespondenceSettingsService.Find(dicWhereDept);
                List<string> deptL = new List<string>();
                for (var i = 0; i < deptList.Data.Count; i++)
                {
                    deptL.Add(deptList.Data[i].Dydm);
                }
                new CreateCriteria(dicWhere).Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));
            }

            if (ShowAll == "1")
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions <List<string>>.In("FApproveStatus", new List<string>(){"2" ,"3" }));
            }
            else
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FApproveStatus", "3"));   //审核通过
            }
            if (!string.IsNullOrEmpty(FBudgetDept))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FBudgetDept", FBudgetDept));
            }

            new CreateCriteria(dicWhere)   
                .Add(ORMRestrictions<Enum>.Eq("FIfPerformanceAppraisal", EnumYesNo.Yes))           //是否绩效评价
                .Add(ORMRestrictions<int>.Eq("FLifeCycle", 0))                                     //版本标识
                .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));                            //单据调整判断 (0表示最新的数据)
         


            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = BudgetMstService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" });

            foreach(var data in result.Results)
            {
                if (string.IsNullOrEmpty(data.FAccount))
                {
                    data.FActualAmount = decimal.Round(0, 2); ;
                }
                else
                {
                    data.FActualAmount = decimal.Round(decimal.Parse(BudgetMstService.GetSJFSSbyXMCode(data.FAccount, data.FProjCode)), 2);
                }
                //data.FActualAmount = decimal.Round(decimal.Parse(BudgetMstService.GetSJFSSbyXMCode(data.FAccount, data.FProjCode)),2);
                data.FBalanceAmount = decimal.Round(data.FProjAmount - data.FActualAmount,2);
                data.FImplRate= decimal.Round(data.FActualAmount * 100 / data.FProjAmount, 2);
            }

            return DataConverterHelper.EntityListToJson<BudgetMstModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetPerformanceMstInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			switch (tabtype)
			{
				case "performancemst":
					var findedresultperformancemst = PerformanceMstService.Find2(id);
                    return DataConverterHelper.ResponseResultToJson(findedresultperformancemst);
				case "performancedtleval":
					var findedresultsperformancedtleval = PerformanceMstService.FindPerformanceDtlEvalByForeignKey(id);
					return DataConverterHelper.EntityListToJson(findedresultsperformancedtleval.Data, findedresultsperformancedtleval.Data.Count);
				case "performancedtltextcont":
					var findedresultsperformancedtltextcont = PerformanceMstService.FindPerformanceDtlTextContByForeignKey(id);
                    PerformanceDtlTextContModel textModel = new PerformanceDtlTextContModel();

                    if (findedresultsperformancedtltextcont.Data.Count > 0)
                        textModel = findedresultsperformancedtltextcont.Data[0];

                    return DataConverterHelper.ResponseResultToJson(textModel);
				case "performancedtlbudtl":
					var findedresultperformancedtlbudtl = PerformanceMstService.FindPerformanceDtlBuDtlByForeignKey(id);
					return DataConverterHelper.EntityListToJson(findedresultperformancedtlbudtl.Data, findedresultperformancedtlbudtl.Data.Count);
                case "performancedtltarimpl":
                    var findedresultperformancedtltarimpl = PerformanceMstService.FindPerformanceDtlTarImplByForeignKey(id);
                    return DataConverterHelper.EntityListToJson(findedresultperformancedtltarimpl.Data, findedresultperformancedtltarimpl.Data.Count);
                case "thirdattachment":
                    var findedresultthirdattachment = PerformanceMstService.FindThirdAttachmentByForeignKey(id);
                    return DataConverterHelper.EntityListToJson(findedresultthirdattachment.Data, findedresultthirdattachment.Data.Count);
                default:
					FindedResult findedresultother = new FindedResult();
					return DataConverterHelper.ResponseResultToJson(findedresultother);
			}
        }

        /// <summary>
        /// 根据主键  预算数据数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetBudgetMstInfo()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型

            switch (tabtype)
            {
                case "budgetmst":
                    var findedresultmst = BudgetMstService.Find(id);

                    FindedResult<PerformanceMstModel> resultmst = new FindedResult<PerformanceMstModel>();
                    PerformanceMstModel modelmst = new PerformanceMstModel();
                    if (findedresultmst.Data != null)
                    {
                        modelmst.YSMstPhId = findedresultmst.Data.PhId;
                        modelmst.FProjCode = findedresultmst.Data.FProjCode;
                        modelmst.FProjName = findedresultmst.Data.FProjName;
                        modelmst.FDeclarationUnit = findedresultmst.Data.FDeclarationUnit;
                        modelmst.FDeclarationUnit_EXName = findedresultmst.Data.FDeclarationUnit_EXName;
                        modelmst.FDeclarationDept = findedresultmst.Data.FDeclarationDept;
                        modelmst.FBudgetDept = findedresultmst.Data.FBudgetDept;
                        modelmst.FBudgetDept_EXName = findedresultmst.Data.FBudgetDept_EXName;
                        modelmst.FProjAttr = findedresultmst.Data.FProjAttr;
                        modelmst.FDuration = findedresultmst.Data.FDuration;
                        modelmst.FStartDate = findedresultmst.Data.FStartDate;
                        modelmst.FEndDate = findedresultmst.Data.FEndDate;
                        modelmst.FProjAmount = findedresultmst.Data.FBudgetAmount;
                        modelmst.FIfPerformanceAppraisal = (int)findedresultmst.Data.FIfPerformanceAppraisal;
                        modelmst.FIfKeyEvaluation = (int)findedresultmst.Data.FIfKeyEvaluation;
                        modelmst.FMeetingTime = findedresultmst.Data.FMeetingTime;
                        modelmst.FMeetiingSummaryNo = findedresultmst.Data.FMeetiingSummaryNo;
                        modelmst.FExpenseCategory = findedresultmst.Data.FExpenseCategory;
                        modelmst.FPerformType = findedresultmst.Data.FPerformType;
                    };
                    if (string.IsNullOrEmpty(findedresultmst.Data.FAccount))
                    {
                        modelmst.FActualAmount = decimal.Round(0, 2); ;
                    }
                    else
                    {
                        modelmst.FActualAmount = decimal.Round(decimal.Parse(BudgetMstService.GetSJFSSbyXMCode(findedresultmst.Data.FAccount, modelmst.FProjCode)), 2);
                    }
                    //modelmst.FActualAmount = decimal.Round(decimal.Parse(BudgetMstService.GetSJFSSbyXMCode(findedresultmst.Data.FAccount, modelmst.FProjCode)),2);
                    modelmst.FBalanceAmount = decimal.Round(modelmst.FProjAmount - modelmst.FActualAmount,2);
                    modelmst.FImplRate = decimal.Round(modelmst.FActualAmount * 100 / modelmst.FProjAmount, 2);

                    resultmst.Data = modelmst;
                    resultmst.Status = findedresultmst.Status;
                    resultmst.Msg= findedresultmst.Msg;


                    return DataConverterHelper.ResponseResultToJson(resultmst);
                case "budgetdtlbudgetdtl":
                    var mst = BudgetMstService.Find(id);
                    var findedresultbudgetdtlbudgetdtl = BudgetMstService.FindBudgetDtlBudgetDtlByForeignKey(id);
                    var listDtl = findedresultbudgetdtlbudgetdtl.Data;

                    //组装model
                    List<PerformanceDtlBuDtlModel> dtlList = new List<PerformanceDtlBuDtlModel>();

                    if (findedresultbudgetdtlbudgetdtl.Data.Count > 0)
                    {
                        foreach (var item in listDtl)
                        {
                            var index = dtlList.FindIndex(t => t.FDtlCode == item.FDtlCode);

                            if (index != -1)
                            {
                                dtlList[index].FBudgetAmount += item.FAmount;

                            }
                            else
                            {
                                decimal FActualAmount = 0;
                                if (string.IsNullOrEmpty(mst.Data.FAccount))
                                {
                                    FActualAmount = decimal.Round(0, 2); ;
                                }
                                else
                                {
                                    FActualAmount = decimal.Round(decimal.Parse(BudgetMstService.GetSJFSSbyMXCode(mst.Data.FAccount, item.FDtlCode)), 2);
                                }
                                //var FActualAmount = decimal.Round(decimal.Parse(BudgetMstService.GetSJFSSbyMXCode(mst.Data.FAccount, item.FDtlCode)),2);
                                //((parseFloat(upDate.FActualAmount) / parseFloat(upDate.FBudgetAmount)) * 100).toFixed(2)
                                var FImplRate = decimal.Round(FActualAmount * 100 / item.FBudgetAmount, 2);
                                PerformanceDtlBuDtlModel model = new PerformanceDtlBuDtlModel()
                                {
                                    DelPhid = item.PhId,
                                    FDtlCode = item.FDtlCode,
                                    FName = item.FName,
                                    FSourceOfFunds = item.FSourceOfFunds,
                                    FSourceOfFunds_EXName = item.FSourceOfFunds_EXName,
                                    FExpensesChannel_EXName = item.FExpensesChannel_EXName,
                                    FBudgetAmount = item.FBudgetAmount,
                                    FActualAmount = FActualAmount,
                                    FBalanceAmount = decimal.Round(item.FBudgetAmount- FActualAmount,2),
                                    FImplRate = FImplRate
                                };

                                dtlList.Add(model);
                            }
                        }
                    }

                    return DataConverterHelper.EntityListToJson((IList<PerformanceDtlBuDtlModel>)dtlList, dtlList.Count);
                case "performancedtltarimpl":
                    var results = BudgetMstService.FindBudgetDtlPerformTargetByForeignKey(id);
                    IList<PerformanceDtlTarImplModel> tarImplList = PerformanceMstService.ConvertData(results.Data);
                    return DataConverterHelper.EntityListToJson(tarImplList,tarImplList.Count);
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
			string performancemstformData = System.Web.HttpContext.Current.Request.Form["performancemstformData"];
			//string performancedtlevalgridData = System.Web.HttpContext.Current.Request.Form["performancedtlevalgridData"];
			string performancedtltextcontformData = System.Web.HttpContext.Current.Request.Form["performancedtltextcontformData"];
			string performancedtlbudtlgridData = System.Web.HttpContext.Current.Request.Form["performancedtlbudtlgridData"];
            string performancedtltarimplgridData = System.Web.HttpContext.Current.Request.Form["performancedtltarimplgridData"];
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Form["id"]);

            var performancemstforminfo = DataConverterHelper.JsonToEntity<PerformanceMstModel>(performancemstformData);
			//var performancedtlevalgridinfo = DataConverterHelper.JsonToEntity<PerformanceDtlEvalModel>(performancedtlevalgridData);
			var performancedtltextcontforminfo = DataConverterHelper.JsonToEntity<PerformanceDtlTextContModel>(performancedtltextcontformData);
			var performancedtlbudtlgridinfo = DataConverterHelper.JsonToEntity<PerformanceDtlBuDtlModel>(performancedtlbudtlgridData);
            var performancedtltarimplgridinfo = DataConverterHelper.JsonToEntity<PerformanceDtlTarImplModel>(performancedtltarimplgridData);
            var results = BudgetMstService.FindBudgetDtlPerformTargetByForeignKey(id);
            List<PerformanceDtlTarImplModel> performanceDtlTarImplModels = PerformanceMstService.ConvertSaveData(results.Data, performancedtltarimplgridinfo.AllRow);

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = PerformanceMstService.SavePerformanceMst(performancemstforminfo.AllRow[0],performancedtltextcontforminfo.AllRow,performancedtlbudtlgridinfo.AllRow, performancedtltarimplgridinfo.AllRow);
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

            var deletedresult = PerformanceMstService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 保存第三方评价数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string SaveThird()
        {
            string adddata = System.Web.HttpContext.Current.Request.Params["adddata"];
            string updatedata = System.Web.HttpContext.Current.Request.Params["updatedata"];
            string deletedata = System.Web.HttpContext.Current.Request.Params["deletedata"];
            var addinfo = JsonConvert.DeserializeObject<List<ThirdAttachmentModel>>(adddata);
            var updateinfo = JsonConvert.DeserializeObject<List<ThirdAttachmentModel>>(updatedata);
            var deleteinfo = JsonConvert.DeserializeObject<List<string>>(deletedata);

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            savedresult = PerformanceMstService.SaveThird(addinfo, updateinfo, deleteinfo);
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 上报
        /// </summary>
        /// <returns>返回Json串</returns>
        public string check()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            PerformanceMstModel model = PerformanceMstService.Find(id).Data;
            model.FAuditStatus = "2";
            model.PersistentState = PersistentState.Modified;
            var result = PerformanceMstService.Save<System.Int64>(model,"");

            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <returns>返回Json串</returns>
        public string valid()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            PerformanceMstModel model = PerformanceMstService.Find(id).Data;
            model.FAuditStatus = "4";
            model.PersistentState = PersistentState.Modified;
            var result = PerformanceMstService.Save<System.Int64>(model, "");

            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 根据预算phid判断是否可以引用
        /// </summary>
        /// <returns></returns>
        public string JudgeIfAllowRefence()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //预算表主键
            var ys = BudgetMstService.Find(id).Data;
            if (ys.FIfPerformanceAppraisal == EnumYesNo.Yes)
            {
                //若需要绩效评价判断是否存在已上报的自评单据
                var dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere).Add(ORMRestrictions<Int64>.Eq("YSMstPhId", id))
                    .Add(ORMRestrictions<string>.Eq("FAuditStatus", "2"));
                var result = PerformanceMstService.Find(dicWhere).Data;
                if (result.Count == 0)
                {
                    return "false";
                }
            }
            return "true";
        }
    }
}

