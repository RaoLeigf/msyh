#region Summary
/**************************************************************************************
    * 类 名 称：        ExamplesController
    * 命名空间：        GXM3.XM.Controller
    * 文 件 名：        ExamplesController.cs
    * 创建时间：        2018/8/17 9:44:51 
    * 作    者：        丰立新    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using NG3.Web.Mvc;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using Enterprise3.NHORM.Controller;

using GXM3.XM.Model;
using GXM3.XM.Service.Interface;

namespace GXM3.XM.Controller
{
    /// <summary>
    /// Examples控制处理类
    /// </summary>
    public class ExamplesController : AFCommonController
    {
        IExamplesService ExamplesService { get; set; }

        /// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult ExamplesList()
        {
            return View("ExamplesList");
        }

        /// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult ExamplesEdit()
        {
            ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型

            if (ViewBag.OType == "add")
            {
                //新增时
                //if (CommonBiz.Has_BillNoRule("取业务类型") == true)
                //{
                //    var vBillNo = ExamplesFacade.GetBillNo();//取用户编码,新增时界面上显示
                //    ViewBag.No = vBillNo.BillNoList[0];
                //}
            }

            return View("ExamplesEdit");
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetExamplesList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = ExamplesService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<ExamplesModel>(result.Results.ToList(), (Int32)result.TotalItems);
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetExamplesInfo()
        {
            Int64 id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型

            var findedresult = ExamplesService.Find(id);
            return DataConverterHelper.ResponseResultToJson(findedresult);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
            string busid = System.Web.HttpContext.Current.Request.Form["busid"];
            string mstformData = System.Web.HttpContext.Current.Request.Form["mstformData"];

            var mstforminfo = DataConverterHelper.JsonToEntity<ExamplesModel>(mstformData);

            var savedresult = ExamplesService.Save(mstforminfo.AllRow);

            return JsonConvert.SerializeObject(savedresult);
        }

    }
}

