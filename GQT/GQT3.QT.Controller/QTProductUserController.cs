#region Summary
/**************************************************************************************
    * 类 名 称：        QTProductUserController
    * 命名空间：        GQT3.QT.Controller
    * 文 件 名：        QTProductUserController.cs
    * 创建时间：        2018/12/12 
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
using System.Web;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace GQT3.QT.Controller
{
    /// <summary>
    /// QTProductUser控制处理类
    /// </summary>
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class QTProductUserController : AFCommonController
    {
        IQTProductUserService QTProductUserService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public QTProductUserController()
        {
            QTProductUserService = base.GetObject<IQTProductUserService>("GQT3.QT.Service.QTProductUser");
        }

        /// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QTProductUserList()
        {
            ViewBag.Title = base.GetMenuLanguage("QTProductUser");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "产品操作员";
            }
            base.InitialMultiLanguage("QTProductUser");
            ViewBag.IndividualInfo = this.GetIndividualUI("QTProductUser");
            return View("QTProductUserList");
        }

        /// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QTProductUserEdit()
        {
            var tabTitle = base.GetMenuLanguage("QTProductUser");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "产品操作员";
            }
            base.SetUserDefScriptUrl("QTProductUser");
            base.InitialMultiLanguage("QTProductUser");
            ViewBag.IndividualInfo = this.GetIndividualUI("QTProductUser");

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

            return View("QTProductUserEdit");
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTProductUserList()
        {
            /*string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary*/
            string ProductBZ = System.Web.HttpContext.Current.Request.Params["ProductBZ"];
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            if (ProductBZ != null && ProductBZ.Length > 0)
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("ProductBZ", ProductBZ));
            }
            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = QTProductUserService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<QTProductUserModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTProductUserInfo()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
            var findedresult = QTProductUserService.Find(id);
            return DataConverterHelper.ResponseResultToJson(findedresult);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
            string qtproductuserformData = System.Web.HttpContext.Current.Request.Form["qtproductuserformData"];

            var qtproductuserforminfo = DataConverterHelper.JsonToEntity<QTProductUserModel>(qtproductuserformData);

            SavedResult<Int64> savedresult = new SavedResult<Int64>();

            try
            {
                savedresult = QTProductUserService.Save2(qtproductuserforminfo.AllRow);
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

            var deletedresult = QTProductUserService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 获取产品用户通过产品标识和G6账号
        /// </summary>
        /// <returns></returns>
        public string getUserByProduct()
        {
            string ProductBZ = System.Web.HttpContext.Current.Request.Params["ProductBZ"];
            string UserNo = System.Web.HttpContext.Current.Request.Params["UserNo"];
            QTProductUserModel qTProductUser = QTProductUserService.getUserByProduct(ProductBZ, UserNo);
            return DataConverterHelper.ResponseResultToJson(qTProductUser);
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
            string ProductBZ = System.Web.HttpContext.Current.Request.Params["ProductBZ"];
            long ProductPhid = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["ProductPhid"]);

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

            if (".xls".Equals(fileExtension)|| ".xlsx".Equals(fileExtension))
            {
                file.SaveAs(filePath + "/" + savename);

                SavedResult<Int64> savedResult = QTProductUserService.SaveImportData(fileExtension,filePath + "/" + savename, clear, ProductBZ, ProductPhid);
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
            string ProductBZ = System.Web.HttpContext.Current.Request.Params["ProductBZ"];
            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            ISheet sheet = book.CreateSheet("sheet1");
            IRow row1 = sheet.CreateRow(0);
            row1.CreateCell(0).SetCellValue("账号");
            row1.CreateCell(1).SetCellValue("密码");
            row1.CreateCell(2).SetCellValue("姓名");

            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("ProductBZ", ProductBZ));
            IList<QTProductUserModel> qTProductUsers = QTProductUserService.ServiceHelper.LoadWithPageInfinity("GQT.QT.ALLProductUsers", dicWhere).Results;

            //List<BudgetAccountsModel> models = budgetAccounts.ToList();
            for (int i = 0; i < qTProductUsers.Count; i++)
            {
                QTProductUserModel model = qTProductUsers[i];
                IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(model.ProductUserCode);
                row.CreateCell(1).SetCellValue(model.ProductUserPwd);
                row.CreateCell(2).SetCellValue(model.ProductUserName);
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

