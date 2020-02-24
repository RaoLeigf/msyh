#region Summary
/**************************************************************************************
    * 类 名 称：        SourceOfFundsController
    * 命名空间：        GQT3.QT.Controller
    * 文 件 名：        SourceOfFundsController.cs
    * 创建时间：        2018/8/28 
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
using GYS3.YS.Model.Domain;
using GYS3.YS.Service.Interface;
using GXM3.XM.Model.Domain;
using GXM3.XM.Service.Interface;
using Enterprise3.Common.Base.Criterion;

namespace GQT3.QT.Controller
{
	/// <summary>
	/// SourceOfFunds控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class SourceOfFundsController : AFCommonController
    {
        ISourceOfFundsService SourceOfFundsService { get; set; }
        IBudgetMstService BudgetMstService { get; set; }
        IProjectMstService ProjectMstService { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public SourceOfFundsController()
	    {
	        SourceOfFundsService = base.GetObject<ISourceOfFundsService>("GQT3.QT.Service.SourceOfFunds");
            BudgetMstService = base.GetObject<IBudgetMstService>("GYS3.YS.Service.BudgetMst");
            ProjectMstService = base.GetObject<IProjectMstService>("GXM3.XM.Service.ProjectMst");
        }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult SourceOfFundsList()
        {
			ViewBag.Title = base.GetMenuLanguage("SourceOfFunds");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "资金来源";
            }
            base.InitialMultiLanguage("SourceOfFunds");
            ViewBag.IndividualInfo = this.GetIndividualUI("SourceOfFunds");
            return View("SourceOfFundsList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult SourceOfFundsEdit()
        {
			var tabTitle = base.GetMenuLanguage("SourceOfFunds");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "资金来源";
            }
            base.SetUserDefScriptUrl("SourceOfFunds");
            base.InitialMultiLanguage("SourceOfFunds");
            ViewBag.IndividualInfo = this.GetIndividualUI("SourceOfFunds");

			ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型

			if (ViewBag.OType == "add")
            {
			    //新增时
				//if (SourceOfFundsService.Has_BillNoRule("单据规则类型") == true)
                //{
                //    var vBillNo = SourceOfFundsService.GetBillNo();//取用户编码,新增时界面上显示
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

            return View("SourceOfFundsEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetSourceOfFundsList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = SourceOfFundsService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<SourceOfFundsModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 根据组织取对应关系列表基础数据详细(得到的PhId为对应关系的主键)
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetSourceOfFundsListDYGXdtl()
        {
            string Dwdm = System.Web.HttpContext.Current.Request.Params["Dwdm"];//查询条件
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "96"));
            if (Dwdm != null)
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dwdm", Dwdm));
            }

            var result = SourceOfFundsService.ServiceHelper.LoadWithPageInfinity("GQT.QT.DYGX-ZJLY", dicWhere);

            return DataConverterHelper.EntityListToJson<SourceOfFundsModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 根据组织取对应关系列表基础数据详细(得到的PhId为对应关系的主键)（没有对应关系的数据）
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetSourceOfFundsListNoDYGXdtl()
        {
            string Dwdm = System.Web.HttpContext.Current.Request.Params["Dwdm"];//查询条件
            List<string> list = new List<string>();
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "96"));
            if (Dwdm != null)
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dwdm", Dwdm));
            }

            var result = SourceOfFundsService.ServiceHelper.LoadWithPageInfinity("GQT.QT.DYGX-ZJLY", dicWhere);
            for (var i = 0; i < result.TotalItems; i++)
            {
                list.Add(result.Results[i].DM);
            }
            if (list.Count > 0)
            {
                new CreateCriteria(dicWhere2).Add(ORMRestrictions<List<string>>.NotIn("DM", list));
            }
            var result2 = SourceOfFundsService.ServiceHelper.LoadWithPageInfinity("GQT.QT.ZJLYALL", dicWhere2, false, new string[] { "DM Asc" });
            return DataConverterHelper.EntityListToJson<SourceOfFundsModel>(result2.Results, (Int32)result2.TotalItems);
        }


        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetSourceOfFundsInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型

			var findedresult = SourceOfFundsService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
            string mstformData = System.Web.HttpContext.Current.Request.Form["mstformData"];

            var mstforminfo = DataConverterHelper.JsonToEntity<SourceOfFundsModel>(mstformData);

            List<SourceOfFundsModel> sourceOfFunds = mstforminfo.AllRow;
            for(int i=0;i< sourceOfFunds.Count; i++)
            {
                sourceOfFunds[i].DM=sourceOfFunds[i].DM.Replace(" ","");
                sourceOfFunds[i].MC=sourceOfFunds[i].MC.Trim();
                sourceOfFunds[i].BZ=sourceOfFunds[i].BZ.Trim();
            }

            var savedresult = SourceOfFundsService.Save<Int64>(sourceOfFunds,"");

            return DataConverterHelper.SerializeObject(savedresult);
        }

		/// <summary>
        /// 删除数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Delete()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            FindedResults<SourceOfFundsModel> sourceOfFunds=SourceOfFundsService.Find(t=>t.PhId==id,"");
            string DM = sourceOfFunds.Data[0].DM;
            FindedResults<BudgetDtlBudgetDtlModel> findedResults = BudgetMstService.FindBudgeAccountByZJDM(DM);
            FindedResults<ProjectDtlBudgetDtlModel> findedResults2 = ProjectMstService.FindProjectDtlBudgetDtlMstByZJDM(DM);
            if (findedResults.Data.Count > 0 || findedResults2.Data.Count > 0)
            {
                findedResults.Status = "failure";
                findedResults.Msg = "当前资金来源已被引用，无法删除！";
                return DataConverterHelper.SerializeObject(findedResults);
            }

            var deletedresult = SourceOfFundsService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 根据code判断是否是末级
        /// </summary>
        /// <returns>返回Json串</returns>
        public string IfLastStage(string Code)
        {
            var findResult = SourceOfFundsService.IfLastStage(Code);
            return DataConverterHelper.SerializeObject(findResult);
        }

    }
}

