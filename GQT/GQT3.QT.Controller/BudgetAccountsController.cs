#region Summary
/**************************************************************************************
    * 类 名 称：        BudgetAccountsController
    * 命名空间：        GQT3.QT.Controller
    * 文 件 名：        BudgetAccountsController.cs
    * 创建时间：        2018/8/29 
    * 作    者：        夏华军    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.IO;
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
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Enterprise3.Common.Base.Criterion;

namespace GQT3.QT.Controller
{
	/// <summary>
	/// BudgetAccounts控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class BudgetAccountsController : AFCommonController
    {
        IBudgetAccountsService BudgetAccountsService { get; set; }
        IBudgetMstService BudgetMstService { get; set; }
        
		/// <summary>
        /// 构造函数
        /// </summary>
	    public BudgetAccountsController()
	    {
	        BudgetAccountsService = base.GetObject<IBudgetAccountsService>("GQT3.QT.Service.BudgetAccounts");
            BudgetMstService = base.GetObject<IBudgetMstService>("GYS3.YS.Service.BudgetMst");
        }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult BudgetAccountsList()
        {
			ViewBag.Title = base.GetMenuLanguage("BudgetAccounts");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "预算科目";
            }
            base.InitialMultiLanguage("BudgetAccounts");
            ViewBag.IndividualInfo = this.GetIndividualUI("BudgetAccounts");
            return View("BudgetAccountsList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult BudgetAccountsEdit()
        {
			var tabTitle = base.GetMenuLanguage("BudgetAccounts");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "预算科目";
            }
            base.SetUserDefScriptUrl("BudgetAccounts");
            base.InitialMultiLanguage("BudgetAccounts");
            ViewBag.IndividualInfo = this.GetIndividualUI("BudgetAccounts");

			ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型

			if (ViewBag.OType == "add")
            {
			    //新增时
				//if (BudgetAccountsService.Has_BillNoRule("单据规则类型") == true)
                //{
                //    var vBillNo = BudgetAccountsService.GetBillNo();//取用户编码,新增时界面上显示
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

            return View("BudgetAccountsEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetBudgetAccountsList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = BudgetAccountsService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<BudgetAccountsModel>(result.Results, (Int32)result.TotalItems);
        }


        /// <summary>
        /// 取all列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetBudgetAccountsListAll()
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();

            var result = BudgetAccountsService.ServiceHelper.LoadWithPageInfinity("GQT.QT.GetBudgetAccountAll", dicWhere);

            return DataConverterHelper.EntityListToJson<BudgetAccountsModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 根据组织取对应关系列表基础数据详细(得到的PhId为对应关系的主键)
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetBudgetAccountsListDYGXdtl()
        {
            string Dwdm = System.Web.HttpContext.Current.Request.Params["Dwdm"];//查询条件
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "02"));
            if (Dwdm != null)
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dwdm", Dwdm));
            }

            var result = BudgetAccountsService.ServiceHelper.LoadWithPageInfinity("GQT.QT.DYGX-YSKM", dicWhere);

            return DataConverterHelper.EntityListToJson<BudgetAccountsModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 根据组织取对应关系列表基础数据详细(得到的PhId为对应关系的主键)（没有对应关系的数据）
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetBudgetAccountsListNoDYGXdtl()
        {
            string Dwdm = System.Web.HttpContext.Current.Request.Params["Dwdm"];//查询条件
            List<string> list = new List<string>();
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "02"));
            if (Dwdm != null)
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dwdm", Dwdm));
            }

            var result = BudgetAccountsService.ServiceHelper.LoadWithPageInfinity("GQT.QT.DYGX-YSKM", dicWhere);
            for (var i = 0; i < result.TotalItems; i++)
            {
                list.Add(result.Results[i].KMDM);
            }
            if (list.Count > 0)
            {
                new CreateCriteria(dicWhere2).Add(ORMRestrictions<List<string>>.NotIn("KMDM", list));
            }
            var result2 = BudgetAccountsService.ServiceHelper.LoadWithPageInfinity("GQT.QT.YSKMAll", dicWhere2);
            return DataConverterHelper.EntityListToJson<BudgetAccountsModel>(result2.Results, (Int32)result2.TotalItems);
        }


        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetBudgetAccountsInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型

			var findedresult = BudgetAccountsService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
            string mstformData = System.Web.HttpContext.Current.Request.Form["mstformData"];
            string otype = System.Web.HttpContext.Current.Request.Form["otype"];
            var mstforminfo = DataConverterHelper.JsonToEntity<BudgetAccountsModel>(mstformData);

            List<BudgetAccountsModel> budgetAccounts = mstforminfo.AllRow;
            var checkresult = BudgetAccountsService.ExecuteDataCheck(ref budgetAccounts,otype);
            if (checkresult.Status == ResponseStatus.Error)
            {
                return DataConverterHelper.SerializeObject(checkresult);
            }
            
            var savedresult = BudgetAccountsService.Save<Int64>(budgetAccounts,"");

            return DataConverterHelper.SerializeObject(savedresult);
        }

		/// <summary>
        /// 删除数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Delete()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            FindedResults<BudgetAccountsModel> budgetAccounts = BudgetAccountsService.Find(t => t.PhId == id, "");
            if (budgetAccounts != null && budgetAccounts.Data.Count > 0)
            {
                string kmdm = budgetAccounts.Data[0].KMDM;
                FindedResults<BudgetDtlBudgetDtlModel> findedResults = BudgetMstService.FindBudgeAccount(kmdm);
                if (findedResults != null && findedResults.Data.Count > 0) {
                    findedResults.Status = ResponseStatus.Error;
                    findedResults.Msg = "当前科目已被引用，无法删除！";
                    return DataConverterHelper.SerializeObject(findedResults);
                }
            }


            var deletedresult = BudgetAccountsService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }


        /// <summary>
        /// 导入数据
        /// </summary>
        /// <returns></returns>
        public string ImportData(HttpPostedFileBase file,string clear) {

            if (file == null)
            {
                return DataConverterHelper.SerializeObject(new { success = false, message = "文件为空，请上传.xls格式的Excel文件！" });
            }

            //上传文件
            string filePath = Server.MapPath("~/UpLoadFiles/GQT");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string filename = Path.GetFileName(file.FileName);
            string fileExtension = Path.GetExtension(filename);
            string savename = Guid.NewGuid().ToString() + fileExtension;
            
            //var result = Json(new { success = true, message = "上传成功"});
            //return JsonConvert.SerializeObject(result.Data);
            
            if (".xls".Equals(fileExtension))
            {
                file.SaveAs(filePath + "/" + savename);

                SavedResult<Int64> savedResult = BudgetAccountsService.ImportData(filePath + "/" + savename,clear);
                if (savedResult == null) {
                    return DataConverterHelper.SerializeObject(new { success = false, message = "导入失败，请重新导入！" });
                }
                //if ("0".Equals(clear))
                //{

                //}
                //using (FileStream fs = new FileStream(filePath + "/" + savename, FileMode.Open, FileAccess.Read))
                //{
                //    HSSFWorkbook workbook = new HSSFWorkbook(fs);
                //    ISheet sheet = workbook.GetSheetAt(0);
                //    int rowCount = sheet.LastRowNum;
                //    for (int i = 1; i <= rowCount; i++)
                //    {
                //        BudgetAccountsModel budgetAccounts = new BudgetAccountsModel();
                //        IRow row = sheet.GetRow(i);
                //        ICell cell1 = row.GetCell(0);
                //        ICell cell2 = row.GetCell(1);
                //        ICell cell3 = row.GetCell(2);
                //        cell1.SetCellType(CellType.String);
                //        cell2.SetCellType(CellType.String);
                //        cell3.SetCellType(CellType.String);
                //        string kmdm = cell1.StringCellValue;
                //        string kmmc = cell2.StringCellValue;
                //        string kmlb = (cell3.StringCellValue == "收入" ? "0" : "1");
                //        budgetAccounts.KMDM = kmdm;
                //        budgetAccounts.KMMC = kmmc;
                //        budgetAccounts.KMLB = kmlb;
                //        BudgetAccountsService.ImportData(budgetAccounts);
                //    }
                //}
            }
            else {
                return DataConverterHelper.SerializeObject(new { success = false, message = "文件格式错误，请上传.xls格式的Excel文件！" });
            }
            return DataConverterHelper.SerializeObject(new { success = true, message = "导入成功！" });
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <returns></returns>
        public void ExportData() {

            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            ISheet sheet = book.CreateSheet("sheet1");
            IRow row1 = sheet.CreateRow(0);
            row1.CreateCell(0).SetCellValue("科目代码");
            row1.CreateCell(1).SetCellValue("科目名称");
            row1.CreateCell(2).SetCellValue("科目类别");

            IList<BudgetAccountsModel> budgetAccounts = BudgetAccountsService.ExportData();
            //List<BudgetAccountsModel> models = budgetAccounts.ToList();
            for (int i = 0; i < budgetAccounts.Count; i++)
            {
                BudgetAccountsModel model = budgetAccounts[i];
                IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(model.KMDM);
                row.CreateCell(1).SetCellValue(model.KMMC);
                row.CreateCell(2).SetCellValue(model.KMLB == "0" ? "收入" : "支出");
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", DateTime.Now.ToString("yyyyMMddHHmmssfff")));
            Response.BinaryWrite(ms.ToArray());
            book = null;
            ms.Close();
            ms.Dispose();
        }

        /// <summary>
        /// 根据code判断是否是末级
        /// </summary>
        /// <returns>返回Json串</returns>
        public string IfLastStage(string budgetAccountsCode)
        {
            var findResult = BudgetAccountsService.IfLastStage(budgetAccountsCode);
            return DataConverterHelper.SerializeObject(findResult);
        }
    }
}

