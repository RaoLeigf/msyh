#region Summary
/**************************************************************************************
    * 类 名 称：        ExpenseCategoryController
    * 命名空间：        GQT3.QT.Controller
    * 文 件 名：        ExpenseCategoryController.cs
    * 创建时间：        2018/8/30 
    * 作    者：        夏华军    
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
using GXM3.XM.Model.Domain;
using GXM3.XM.Service.Interface;
using GYS3.YS.Model.Domain;
using GYS3.YS.Service.Interface;
using Enterprise3.Common.Base.Criterion;

namespace GQT3.QT.Controller
{
	/// <summary>
	/// ExpenseCategory控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ExpenseCategoryController : AFCommonController
    {
        IExpenseCategoryService ExpenseCategoryService { get; set; }
        IProjectMstService ProjectMstService { get; set; }
        IBudgetMstService BudgetMstService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public ExpenseCategoryController()
	    {
	        ExpenseCategoryService = base.GetObject<IExpenseCategoryService>("GQT3.QT.Service.ExpenseCategory");
            ProjectMstService = base.GetObject<IProjectMstService>("GXM3.XM.Service.ProjectMst");
            BudgetMstService = base.GetObject<IBudgetMstService>("GYS3.YS.Service.BudgetMst");
        }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult ExpenseCategoryList()
        {
			ViewBag.Title = base.GetMenuLanguage("ExpenseCategory");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "项目类型";
            }
            base.InitialMultiLanguage("ExpenseCategory");
            ViewBag.IndividualInfo = this.GetIndividualUI("ExpenseCategory");
            return View("ExpenseCategoryList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult ExpenseCategoryEdit()
        {
			var tabTitle = base.GetMenuLanguage("ExpenseCategory");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "支出类别";
            }
            base.SetUserDefScriptUrl("ExpenseCategory");
            base.InitialMultiLanguage("ExpenseCategory");
            ViewBag.IndividualInfo = this.GetIndividualUI("ExpenseCategory");

			ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型

			if (ViewBag.OType == "add")
            {
			    //新增时
				//if (ExpenseCategoryService.Has_BillNoRule("单据规则类型") == true)
                //{
                //    var vBillNo = ExpenseCategoryService.GetBillNo();//取用户编码,新增时界面上显示
                //    ViewBag.No = vBillNo.BillNoList[0];
                //}
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

            return View("ExpenseCategoryEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetExpenseCategoryList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = ExpenseCategoryService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<ExpenseCategoryModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 根据组织取对应关系列表基础数据详细(得到的PhId为对应关系的主键)
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetExpenseCategoryListDYGXdtl()
        {
            string Dwdm = System.Web.HttpContext.Current.Request.Params["Dwdm"];//查询条件
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "08"));
            if (Dwdm != null)
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dwdm", Dwdm));
            }

            var result = ExpenseCategoryService.ServiceHelper.LoadWithPageInfinity("GQT.QT.DYGX2-ZCLB", dicWhere);

            return DataConverterHelper.EntityListToJson<ExpenseCategoryModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 根据组织取对应关系列表基础数据详细(得到的PhId为对应关系的主键)（没有对应关系的数据）
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetExpenseCategoryListNoDYGXdtl()
        {
            string Dwdm = System.Web.HttpContext.Current.Request.Params["Dwdm"];//查询条件
            List<string> list = new List<string>();
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "08"));
            if (Dwdm != null)
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dwdm", Dwdm));
            }

            var result = ExpenseCategoryService.ServiceHelper.LoadWithPageInfinity("GQT.QT.DYGX2-ZCLB", dicWhere);
            for (var i = 0; i < result.TotalItems; i++)
            {
                list.Add(result.Results[i].Dm);
            }
            if (list.Count > 0)
            {
                new CreateCriteria(dicWhere2).Add(ORMRestrictions<List<string>>.NotIn("Dm", list));
            }
            var result2 = ExpenseCategoryService.ServiceHelper.LoadWithPageInfinity("GQT.QT.ZCLBALL", dicWhere2, false, new string[] { "Dm Asc" });
            return DataConverterHelper.EntityListToJson<ExpenseCategoryModel>(result2.Results, (Int32)result2.TotalItems);
        }


        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetExpenseCategoryInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型

			var findedresult = ExpenseCategoryService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
            /*string mstformData = System.Web.HttpContext.Current.Request.Form["mstformData"];
            string otype = System.Web.HttpContext.Current.Request.Form["otype"];
            var mstforminfo = DataConverterHelper.JsonToEntity<ExpenseCategoryModel>(mstformData);

            List<ExpenseCategoryModel> expenseCategories = mstforminfo.AllRow;
            var checkresult = ExpenseCategoryService.ExecuteDataCheck(ref expenseCategories,otype);
            if (checkresult != null && checkresult.Status == ResponseStatus.Error)
            {
                return DataConverterHelper.SerializeObject(checkresult);
            }

            var savedresult = ExpenseCategoryService.Save<Int64>(mstforminfo.AllRow);*/
            string Data2 = System.Web.HttpContext.Current.Request.Params["Data2"];
            string edittype = System.Web.HttpContext.Current.Request.Params["edittype"];

            ExpenseCategoryModel expenseCategory = JsonConvert.DeserializeObject<ExpenseCategoryModel>(Data2);


            var savedresult = ExpenseCategoryService.Save2(expenseCategory, edittype);

            return DataConverterHelper.SerializeObject(savedresult);
        }

		/// <summary>
        /// 删除数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Delete()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            FindedResults<ExpenseCategoryModel> expenseCategorys = ExpenseCategoryService.Find(t => t.PhId == id);
            if (expenseCategorys != null && expenseCategorys.Data.Count > 0)
            {
                string dm = expenseCategorys.Data[0].Dm;
                FindedResults<ProjectMstModel> findedResults1 = ProjectMstService.FindProjectMst(dm);
                FindedResults<BudgetMstModel> findedResults2 = BudgetMstService.FindBudgetMst(dm);
                if (findedResults1 != null && findedResults1.Data.Count > 0)
                {
                    findedResults1.Status = ResponseStatus.Error;
                    findedResults1.Msg = "当前类别已被引用，无法删除！";
                    return DataConverterHelper.SerializeObject(findedResults1);
                }
                if (findedResults2 != null && findedResults2.Data.Count > 0)
                {
                    findedResults2.Status = ResponseStatus.Error;
                    findedResults2.Msg = "当前类别已被引用，无法删除！";
                    return DataConverterHelper.SerializeObject(findedResults2);
                }
            }

            var deletedresult = ExpenseCategoryService.Delete<System.Int64>(id);
            
            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 根据支出类别(项目类型)的code判断是否是末级
        /// </summary>
        /// <returns>返回Json串</returns>
        public string IfLastStage(string expenseCategoryCode) {
            var findResult = ExpenseCategoryService.IfLastStage(expenseCategoryCode);
            return DataConverterHelper.SerializeObject(findResult);
        }
    }
}

