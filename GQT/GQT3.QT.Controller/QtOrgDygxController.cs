#region Summary
/**************************************************************************************
    * 类 名 称：			QtOrgDygxController
    * 命名空间：			GQT3.QT.Controller
    * 文 件 名：			QtOrgDygxController.cs
    * 创建时间：			2019/2/14 
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
using System.Web;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Enterprise3.Common.Base.Criterion;

namespace GQT3.QT.Controller
{
	/// <summary>
	/// QtOrgDygx控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class QtOrgDygxController : AFCommonController
    {
        IQtOrgDygxService QtOrgDygxService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public QtOrgDygxController()
	    {
	        QtOrgDygxService = base.GetObject<IQtOrgDygxService>("GQT3.QT.Service.QtOrgDygx");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QtOrgDygxList()
        {
			ViewBag.Title = base.GetMenuLanguage("QtOrgDygx");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "项目库对应G6H设置-组织";
            }
            base.InitialMultiLanguage("QtOrgDygx");
            ViewBag.IndividualInfo = this.GetIndividualUI("QtOrgDygx");
            return View("QtOrgDygxList");
        }

        /// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QtOrgDygxList2()
        {
            ViewBag.Title = base.GetMenuLanguage("QtOrgDygx");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "项目库对应G6H设置-部门";
            }
            base.InitialMultiLanguage("QtOrgDygx");
            ViewBag.IndividualInfo = this.GetIndividualUI("QtOrgDygx");
            return View("QtOrgDygxList2");
        }

        /// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QtOrgDygxEdit()
        {
			var tabTitle = base.GetMenuLanguage("QtOrgDygx");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "项目库对应G6H设置";
            }
            base.SetUserDefScriptUrl("QtOrgDygx");
            base.InitialMultiLanguage("QtOrgDygx");
            ViewBag.IndividualInfo = this.GetIndividualUI("QtOrgDygx");

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

            return View("QtOrgDygxEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQtOrgDygxList()
        {
            long ParentOrgId = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["ParentOrgId"]);  //主表主键

            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary
            
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<System.Int64>.Eq("ParentOrgId", ParentOrgId));
           

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = QtOrgDygxService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<QtOrgDygxModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQtOrgDygxInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = QtOrgDygxService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string qtorgdygxformData = System.Web.HttpContext.Current.Request.Form["qtorgdygxformData"];

			var qtorgdygxforminfo = DataConverterHelper.JsonToEntity<QtOrgDygxModel>(qtorgdygxformData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = QtOrgDygxService.Save<Int64>(qtorgdygxforminfo.AllRow, "");
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

            var deletedresult = QtOrgDygxService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public string Save2()
        {
            string adddata = System.Web.HttpContext.Current.Request.Params["adddata"];
            string updatedata = System.Web.HttpContext.Current.Request.Params["updatedata"];
            string deletedata = System.Web.HttpContext.Current.Request.Params["deletedata"];
            var addinfo = JsonConvert.DeserializeObject<List<QtOrgDygxModel>>(adddata);
            var updateinfo = JsonConvert.DeserializeObject<List<QtOrgDygxModel>>(updatedata);
            var deleteinfo = JsonConvert.DeserializeObject<List<string>>(deletedata);

            CommonResult savedresult = new CommonResult();
            savedresult = QtOrgDygxService.Save2(addinfo, updateinfo, deleteinfo);
            return DataConverterHelper.SerializeObject(savedresult);

        }


        /// <summary>
        /// 导入数据
        /// </summary>
        /// <returns></returns>
        public string ImportData(HttpPostedFileBase file)
        {
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

            if (".xls".Equals(fileExtension) || ".xlsx".Equals(fileExtension))
            {
                file.SaveAs(filePath + "/" + savename);

                SavedResult<Int64> savedResult = QtOrgDygxService.ImportData(fileExtension, filePath + "/" + savename);
                if (savedResult.Status == ResponseStatus.Error)
                {
                    return DataConverterHelper.SerializeObject(new { success = false, message = savedResult.Msg });
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

            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            ISheet sheet = book.CreateSheet("sheet1");
            IRow row1 = sheet.CreateRow(0);
            row1.CreateCell(0).SetCellValue("项目库组织代码");
            row1.CreateCell(1).SetCellValue("老G6H组织代码");

            Dictionary<string, object> dicwhere = new Dictionary<string, object>();
            new CreateCriteria(dicwhere)
                .Add(ORMRestrictions<System.Int64>.NotEq("PhId", 0));
            IList<QtOrgDygxModel> qtOrgDygxes = QtOrgDygxService.Find(dicwhere).Data;//数据库的所有数据
            
            for (int i = 0; i < qtOrgDygxes.Count; i++)
            {
                QtOrgDygxModel model = qtOrgDygxes[i];
                IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(model.Xmorg);
                row.CreateCell(1).SetCellValue(model.Oldorg);
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

