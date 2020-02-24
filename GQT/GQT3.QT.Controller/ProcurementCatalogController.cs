#region Summary
/**************************************************************************************
    * 类 名 称：        ProcurementCatalogController
    * 命名空间：        GQT3.QT.Controller
    * 文 件 名：        ProcurementCatalogController.cs
    * 创建时间：        2018/10/16 
    * 作    者：        李长敏琛    
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
using System.Web;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Enterprise3.Common.Base.Criterion;

namespace GQT3.QT.Controller
{
	/// <summary>
	/// ProcurementCatalog控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ProcurementCatalogController : AFCommonController
    {
        IProcurementCatalogService ProcurementCatalogService { get; set; }
        IProjectMstService projectMstService { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcurementCatalogController()
	    {
	        ProcurementCatalogService = base.GetObject<IProcurementCatalogService>("GQT3.QT.Service.ProcurementCatalog");
            projectMstService = base.GetObject<IProjectMstService>("GXM3.XM.Service.ProjectMst");

        }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult ProcurementCatalogList()
        {
			ViewBag.Title = base.GetMenuLanguage("ProcurementCatalog");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "采购目录";
            }
            base.InitialMultiLanguage("ProcurementCatalog");
            ViewBag.IndividualInfo = this.GetIndividualUI("ProcurementCatalog");
            return View("ProcurementCatalogList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult ProcurementCatalogEdit()
        {
			var tabTitle = base.GetMenuLanguage("ProcurementCatalog");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "采购目录";
            }
            base.SetUserDefScriptUrl("ProcurementCatalog");
            base.InitialMultiLanguage("ProcurementCatalog");
            ViewBag.IndividualInfo = this.GetIndividualUI("ProcurementCatalog");

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

            return View("ProcurementCatalogEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetProcurementCatalogList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = ProcurementCatalogService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere, new string[] { "FCode Asc" });

            return DataConverterHelper.EntityListToJson<ProcurementCatalogModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public PagedResult<ProcurementCatalogModel> GetList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = ProcurementCatalogService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);
            return result;
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetProcurementCatalogInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = ProcurementCatalogService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string procurementcatalogformData = System.Web.HttpContext.Current.Request.Form["procurementcatalogformData"];

			var procurementcatalogforminfo = DataConverterHelper.JsonToEntity<ProcurementCatalogModel>(procurementcatalogformData);

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            PagedResult<ProcurementCatalogModel> result=this.GetList();
            
            try
			{
                foreach (ProcurementCatalogModel procurementCatalog in procurementcatalogforminfo.AllRow)
                {
                    //where条件已经包括新增以及修改，反证法
                    var pt = from pt1 in result.Results
                             where (pt1.FName == procurementCatalog.FName || pt1.FCode == procurementCatalog.FCode) && pt1.PhId != procurementCatalog.PhId
                             select pt1;
                    if (pt.Count() > 0)
                    {
                        throw new Exception("代码或名称重复");
                    }
                }
                savedresult = ProcurementCatalogService.Save<Int64>(procurementcatalogforminfo.AllRow,"");
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
            string codes =System.Web.HttpContext.Current.Request.Params["code"].ToString();
            string names = "";
            if (CanDelete(codes,out names))
            {
                var deletedresult = ProcurementCatalogService.Delete<System.Int64>(id);

                return DataConverterHelper.SerializeObject(deletedresult);
            }
            else
            {
                return "{\"result\":\"fail\",\"names\":\""+names+"\"}";
            }
        }


        //GXM.fac xml配置注入不了，所以目前暂时采用qt.controller调用xm.service,不采用qt.service调用xm.fac

        public bool CanDelete(string data,out string names) {
            string[] codes;
            if (data.EndsWith(",")) {
                data = data.Substring(0, data.Length - 1);
            }
            codes = data.Split(',');
            var result=this.projectMstService.FindProjectDtlPurchaseDtlByCatalogCode(codes);
            names = "";
            if (result.Data.Count > 0)
            {
                foreach (var ppd in result.Data) {
                    //666简化null检查
                    names += (ppd.FName ?? "未命名") + ",";
                }
                if (names.EndsWith(",")) {
                    names = names.Substring(0, names.Length - 1);
                }
                return false;
            }
            else {
                return true;
            }
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <returns></returns>
        public string ImportData(HttpPostedFileBase file, string clear)
        {
            if (file == null)
            {
                return DataConverterHelper.SerializeObject(new { success = false, message = "文件为空，请上传.xls格式的Excel文件！" });
            }
            //string ProductBZ = System.Web.HttpContext.Current.Request.Params["ProductBZ"];
            //long ProductPhid = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["ProductPhid"]);

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

            if (".xls".Equals(fileExtension) || ".xlsx".Equals(fileExtension))
            {
                file.SaveAs(filePath + "/" + savename);

                SavedResult<Int64> savedResult = ProcurementCatalogService.SaveImportData(fileExtension, filePath + "/" + savename, clear);
                if (savedResult == null)
                {
                    return DataConverterHelper.SerializeObject(new { success = false, message = "导入失败，请重新导入！" });
                }
            }
            else
            {
                return DataConverterHelper.SerializeObject(new { success = false, message = "文件格式错误，请上传.xls格式的Excel文件！" });
            }
            return DataConverterHelper.SerializeObject(new { success = true, message = "导入成功！" });
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <returns></returns>
        public void ExportData()
        {
            //string ProductBZ = System.Web.HttpContext.Current.Request.Params["ProductBZ"];
            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            ISheet sheet = book.CreateSheet("sheet1");
            IRow row1 = sheet.CreateRow(0);
            row1.CreateCell(0).SetCellValue("代码");
            row1.CreateCell(1).SetCellValue("名称");
            //row1.CreateCell(2).SetCellValue("备注");

            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<System.Int64>.NotEq("PhId", 0));
            IList<ProcurementCatalogModel> procurementCatalogs = ProcurementCatalogService.Find(dicWhere).Data;
            
            for (int i = 0; i < procurementCatalogs.Count; i++)
            {
                ProcurementCatalogModel model = procurementCatalogs[i];
                IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(model.FCode);
                row.CreateCell(1).SetCellValue(model.FName);
                //row.CreateCell(2).SetCellValue(model.FRemark);
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", DateTime.Now.ToString("yyyyMMddHHmmssfff")));
            Response.BinaryWrite(ms.ToArray());
            book = null;
            ms.Close();
            ms.Dispose();
        }

    }
}

