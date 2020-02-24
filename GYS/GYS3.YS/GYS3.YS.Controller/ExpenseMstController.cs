#region Summary
/**************************************************************************************
    * 类 名 称：			ExpenseMstController
    * 命名空间：			GYS3.YS.Controller
    * 文 件 名：			ExpenseMstController.cs
    * 创建时间：			2019/1/24 
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

using GYS3.YS.Service.Interface;
using GYS3.YS.Model.Domain;

using GQT3.QT.Service.Interface;
using GQT3.QT.Model.Domain;
using Enterprise3.Common.Base.Helpers;
using System.Reflection;
using Enterprise3.Common.Base.Criterion;

namespace GYS3.YS.Controller
{
	/// <summary>
	/// ExpenseMst控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ExpenseMstController : AFCommonController
    {
        IExpenseMstService ExpenseMstService { get; set; }
        ICorrespondenceSettingsService CorrespondenceSettingsService { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ExpenseMstController()
	    {
	        ExpenseMstService = base.GetObject<IExpenseMstService>("GYS3.YS.Service.ExpenseMst");
            CorrespondenceSettingsService = base.GetObject<ICorrespondenceSettingsService>("GQT3.QT.Service.CorrespondenceSettings");
        }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult ExpenseMstList()
        {
			ViewBag.Title = base.GetMenuLanguage("GHExpenseMst");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "项目支出预算审批表";//用款计划
            }
            base.InitialMultiLanguage("GHExpenseMst");
            ViewBag.IndividualInfo = this.GetIndividualUI("GHExpenseMst");
            return View("ExpenseMstList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult ExpenseMstEdit()
        {
            var tabTitle = base.GetMenuLanguage("GHExpenseMst");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "项目支出预算审批表";
            }
            base.SetUserDefScriptUrl("GHExpenseMst");
            base.InitialMultiLanguage("GHExpenseMst");
            ViewBag.IndividualInfo = this.GetIndividualUI("GHExpenseMst");

            ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型
            ViewBag.IfReturn = System.Web.HttpContext.Current.Request.Params["Ifreturn"];//是否额度返还
            ViewBag.AmountHX = System.Web.HttpContext.Current.Request.Params["AmountHX"];//是否额度核销

            if (ViewBag.OType == "add")
            {
                ViewBag.Title = tabTitle + "-新增";
            }
            else if (ViewBag.OType == "edit")
            {
                if (ViewBag.IfReturn == "1")
                {
                    ViewBag.Title = tabTitle + "-额度返还";
                }
                else
                {
                    ViewBag.Title = tabTitle + "-修改";
                }
            }
            else if (ViewBag.OType == "view")
            {
                ViewBag.Title = tabTitle + "-查看";
            }

            return View("ExpenseMstEdit");
        }
        /// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult ZcYs()
        {
            ViewBag.Title = base.GetMenuLanguage("GHExpenseMst");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "项目支出预算情况查询";
            }
            base.InitialMultiLanguage("GHExpenseMst");
            ViewBag.IndividualInfo = this.GetIndividualUI("GHExpenseMst");
            return View("ZcYs");
        }
        

        /// <summary>
        /// 指向用款计划审批列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult WorkFlowTaskList_toExpense()
        {
            
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "项目支出预算情况审批";
            }
           
            return View("WorkFlowTaskList_toExpense");
        }


        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetExpenseMstList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            var userId = System.Web.HttpContext.Current.Request.Params["userId"];
            var dicWhereDept = new Dictionary<string, object>();
            new CreateCriteria(dicWhereDept)
                .Add(ORMRestrictions<string>.Eq("Dwdm", userId)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
            var deptList = CorrespondenceSettingsService.Find(dicWhereDept);
            List<string> deptL = new List<string>();
            for (var i = 0; i < deptList.Data.Count; i++)
            {
                deptL.Add(deptList.Data[i].Dydm);
            }
            new CreateCriteria(dicWhere)
                   .Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = ExpenseMstService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere,new string[] { "FPerformevaltype Desc" });

            return DataConverterHelper.EntityListToJson<ExpenseMstModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetExpenseMstInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			switch (tabtype)
			{
				case "expensemst":
					var findedresultexpensemst = ExpenseMstService.Find(id);
					return DataConverterHelper.ResponseResultToJson(findedresultexpensemst);
				case "expensedtl":
					var findedresultexpensedtl = ExpenseMstService.FindExpenseDtlByForeignKey(id);
					return DataConverterHelper.EntityListToJson(findedresultexpensedtl.Data, findedresultexpensedtl.Data.Count);
                case "expensehx":
                    var findedresultexpensehx = ExpenseMstService.FindExpenseHxByForeignKey(id);
                    return DataConverterHelper.EntityListToJson(findedresultexpensehx.Data, findedresultexpensehx.Data.Count);
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
            string expensemstformData = System.Web.HttpContext.Current.Request.Form["expensemstformData"];
            string expensedtlgridData = System.Web.HttpContext.Current.Request.Form["expensedtlgridData"];
            string NCmoney = System.Web.HttpContext.Current.Request.Params["NCmoney"];//年初预算金额
            string beforeSum = System.Web.HttpContext.Current.Request.Params["beforeSum"];//本单据初始预计支出金额
            string beforeFReturnamount = System.Web.HttpContext.Current.Request.Params["beforeFReturnamount"];//本单据初始预计返还金额
            string Ifreturn = System.Web.HttpContext.Current.Request.Params["Ifreturn"];//是否额度返还

            var expensemstforminfo = DataConverterHelper.JsonToEntity<ExpenseMstModel>(expensemstformData);
            var expensedtlgridinfo = DataConverterHelper.JsonToEntity<ExpenseDtlModel>(expensedtlgridData);

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            try
            {
                savedresult = ExpenseMstService.SaveExpenseMst(expensemstforminfo.AllRow[0], expensedtlgridinfo.AllRow, NCmoney, beforeSum, beforeFReturnamount, Ifreturn);
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

            var deletedresult = ExpenseMstService.Delete2(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 删除额度返还单据（额度逆返还）
        /// </summary>
        /// <returns>返回Json串</returns>
        public string DeleteReturn()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            var deletedresult = ExpenseMstService.DeleteReturn(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }


        /// <summary>
        /// 取消送审
        /// </summary>
        /// <returns></returns>
        public string UnChecked()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            var Findresult = ExpenseMstService.Find<System.Int64>(id);
            SavedResult<Int64> savedresult = new SavedResult<Int64>();


            Findresult.Data.FApprovestatus = "1";
            Findresult.Data.FApprover = 0;
            Findresult.Data.FApprovedate = new Nullable<DateTime>();
            Findresult.Data.PersistentState = PersistentState.Modified;
            savedresult = ExpenseMstService.Save<Int64>(Findresult.Data,"");

            return DataConverterHelper.SerializeObject(savedresult);
        }


        /// <summary>
        /// 根据项目代码取预计支出金额的和
        /// </summary>
        /// <returns></returns>
        public string SumFSurplusamount()
        {
            string FProjCode = System.Web.HttpContext.Current.Request.Params["FProjCode"];  //项目代码

            var result = ExpenseMstService.SumFSurplusamount(FProjCode);

            return result;
        }

        /// <summary>
        /// 根据主表phid取明细剩余金额
        /// </summary>
        /// <returns></returns>
        public string RestOfAmount()
        {
            long id= Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);
            string code = System.Web.HttpContext.Current.Request.Params["code"];
            var result = ExpenseMstService.RestOfAmount(id, code);
            return result;
        }

        /*/// <summary>
        /// 项目支出预算情况查询
        /// </summary>
        /// <returns></returns>
        public string GetXmZcYs()
        {
            string userID = System.Web.HttpContext.Current.Request.Params["userID"];
            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = ExpenseMstService.GetXmZcYs(userID, storeparam.PageIndex, storeparam.PageSize);
            return DataConverterHelper.EntityListToJson<ExpenseMstModel>(result.Results, (Int32)result.TotalItems);
        }*/

        /// <summary>
        /// 通过项目代码和操作员获取财务实际发生数
        /// </summary>
        /// <returns></returns>
        public string GetSJFSSbyCode()
        {
            string userID = System.Web.HttpContext.Current.Request.Params["userID"];
            string code = System.Web.HttpContext.Current.Request.Params["code"];
            var result = ExpenseMstService.GetSJFSSbyCode(userID, code);
            if (result == "" || result == null)
            {
                result = "0.00";
            }
            return result;
        }

        /// <summary>
        /// 保存额度核销数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string SaveHX()
        {
            string adddata = System.Web.HttpContext.Current.Request.Params["adddata"];
            string updatedata = System.Web.HttpContext.Current.Request.Params["updatedata"];
            string deletedata = System.Web.HttpContext.Current.Request.Params["deletedata"];
            var addinfo = JsonConvert.DeserializeObject<List<ExpenseHxModel>>(adddata);
            var updateinfo = JsonConvert.DeserializeObject<List<ExpenseHxModel>>(updatedata);
            var deleteinfo = JsonConvert.DeserializeObject<List<string>>(deletedata);

            CommonResult savedresult = new CommonResult();
            savedresult = ExpenseMstService.SaveHX(addinfo, updateinfo, deleteinfo);
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 额度核销执行完毕确认
        /// </summary>
        /// <returns></returns>
        public string SaveHXgo()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);
            Decimal FPlayamount = Convert.ToDecimal(System.Web.HttpContext.Current.Request.Params["FPlayamount"]);
            Decimal FReturnamount = Convert.ToDecimal(System.Web.HttpContext.Current.Request.Params["FReturnamount"]);
            string dtldata = System.Web.HttpContext.Current.Request.Params["dtldata"];

            var dtls = JsonConvert.DeserializeObject<List<ExpenseDtlModel>>(dtldata);

            CommonResult savedresult = new CommonResult();
            savedresult=ExpenseMstService.SaveHXgo(id, FPlayamount, FReturnamount, dtls);
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 额度核销撤销
        /// </summary>
        /// <returns></returns>
        public string SaveHXreturn()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);
            Decimal FPlayamount = Convert.ToDecimal(System.Web.HttpContext.Current.Request.Params["FPlayamount"]);
            CommonResult savedresult = new CommonResult();
            savedresult = ExpenseMstService.SaveHXreturn(id, FPlayamount);
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 根据项目代码获取额度核销总金额
        /// </summary>
        /// <returns></returns>
        public string GetHXsumByCode()
        {
            string code = System.Web.HttpContext.Current.Request.Params["code"];
            return ExpenseMstService.GetHXsumByCode(code).ToString();
        }
    }
}

