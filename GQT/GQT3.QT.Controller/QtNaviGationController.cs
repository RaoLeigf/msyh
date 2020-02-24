#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Controller
    * 类 名 称：			QtNaviGationController
    * 文 件 名：			QtNaviGationController.cs
    * 创建时间：			2019/11/14 
    * 作    者：			张宇    
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

namespace GQT3.QT.Controller
{
    /// <summary>
    /// QtNaviGation控制处理类
    /// </summary>
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class QtNaviGationController : AFCommonController
    {
        IQtNaviGationService QtNaviGationService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public QtNaviGationController()
        {
            QtNaviGationService = base.GetObject<IQtNaviGationService>("GQT3.QT.Service.QtNaviGation");
        }

        /// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QtNaviGationList()
        {
            ViewBag.Title = base.GetMenuLanguage("QtNaviGation");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "预算项目导航";
            }
            base.InitialMultiLanguage("QtNaviGation");
            ViewBag.IndividualInfo = this.GetIndividualUI("QtNaviGation");
            return View("QtNaviGationList");
        }

        /// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QtNaviGationEdit()
        {
            var tabTitle = base.GetMenuLanguage("QtNaviGation");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "预算项目导航";
            }
            base.SetUserDefScriptUrl("QtNaviGation");
            base.InitialMultiLanguage("QtNaviGation");
            ViewBag.IndividualInfo = this.GetIndividualUI("QtNaviGation");

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

            return View("QtNaviGationEdit");
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQtNaviGationList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = QtNaviGationService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<QtNaviGationModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQtNaviGationInfo()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
            var findedresult = QtNaviGationService.Find(id);
            return DataConverterHelper.ResponseResultToJson(findedresult);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
            string qtnavigationformData = System.Web.HttpContext.Current.Request.Form["qtnavigationformData"];

            var qtnavigationforminfo = DataConverterHelper.JsonToEntity<QtNaviGationModel>(qtnavigationformData);

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            try
            {
                savedresult = QtNaviGationService.Save<Int64>(qtnavigationforminfo.AllRow, "");
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

            var deletedresult = QtNaviGationService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }
    }
}

