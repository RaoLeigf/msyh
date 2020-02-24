#region Summary
/**************************************************************************************
    * 类 名 称：        ExtendFunctionMstController
    * 命名空间：        NG3.Addin.Controller
    * 文 件 名：        ExtendFunctionMstController.cs
    * 创建时间：        2017/7/10 
    * 作    者：        韦忠吉    
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

using NG3.Addin.Service.Interface;
using NG3.Addin.Model.Domain;
using NG3.Addin.Model.Domain.BusinessModel;

namespace NG3.Addin.Controller
{
	/// <summary>
	/// ExtendFunctionMst控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ExtendFunctionMstController : AFCommonController
    {
        IExtendFunctionMstService ExtendFunctionMstService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public ExtendFunctionMstController()
	    {
	        ExtendFunctionMstService = base.GetObject<IExtendFunctionMstService>("NG3.Addin.Service.ExtendFunctionMst");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult ExtendFunctionMstList()
        {
			base.InitialMultiLanguage("ExtendFunctionMstList");
            return View("ExtendFunctionMstList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult ExtendFunctionMstEdit()
        {
			base.InitialMultiLanguage("ExtendFunctionMstEdit");
			ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型

            if (ViewBag.OType == "add")
            {
				//新增时
				//if (ExtendFunctionMstService.Has_BillNoRule("取业务类型") == true)
                //{
                //    var vBillNo = ExtendFunctionMstService.GetBillNo();//取用户编码,新增时界面上显示
                //    ViewBag.No = vBillNo.BillNoList[0];
                //}
            }

            return View("ExtendFunctionMstEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetExtendFunctionMstList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = ExtendFunctionMstService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<ExtendFunctionMstModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetExtendFunctionMstInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型

			var findedresult = ExtendFunctionMstService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetAddinUrlList()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            var url = ExtendFunctionMstService.GetUrl(id);
            return DataConverterHelper.EntityListToJson<ExtendFuncUrlBizModel>(url, url.Count);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
            string mstformData = System.Web.HttpContext.Current.Request.Form["mstformData"];

            var mstforminfo = DataConverterHelper.JsonToEntity<ExtendFunctionMstModel>(mstformData);

            //表达式数据
            string urlgriddata = System.Web.HttpContext.Current.Request.Form["urlgriddata"];
            var urlEntity = DataConverterHelper.JsonToEntity<ExtendFuncUrlBizModel>(urlgriddata);

                 

            string sqldata = System.Web.HttpContext.Current.Request.Form["sqlgriddata"];
            var sqlEntity = DataConverterHelper.JsonToEntity<AddinSqlModel>(sqldata);


            //程序集插件
            string assemblydata = System.Web.HttpContext.Current.Request.Form["assemblydata"];
            var assemblyEntity = DataConverterHelper.JsonToEntity<AddinAssemblyModel>(assemblydata);



            var savedresult = ExtendFunctionMstService.SaveExtendFunc(mstforminfo.AllRow, urlEntity.AllRow,sqlEntity.AllRow, assemblyEntity.AllRow);

            return DataConverterHelper.SerializeObject(savedresult);
        }

		/// <summary>
        /// 删除数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Delete()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            var deletedresult = ExtendFunctionMstService.DeleteExtendFunc(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }


        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetAddinSqlList()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            
            IList<AddinSqlModel> sqlModels = ExtendFunctionMstService.FindAddinSqlByMstPhid(id);

            return DataConverterHelper.EntityListToJson<AddinSqlModel>(sqlModels, sqlModels.Count);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetAddinAssemblyList()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            IList<AddinAssemblyModel> Models = ExtendFunctionMstService.FindAddinAssemblyByMstPhid(id);

            return DataConverterHelper.EntityListToJson<AddinAssemblyModel>(Models, Models.Count);

        }


    }
}

