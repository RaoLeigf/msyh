#region Summary
/**************************************************************************************
    * 类 名 称：        MethodAroundMstController
    * 命名空间：        NG3.Addin.Controller
    * 文 件 名：        MethodAroundMstController.cs
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
using System.Reflection;
using NG3.Addin.Core.Cfg;

namespace NG3.Addin.Controller
{
	/// <summary>
	/// MethodAroundMst控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class MethodAroundMstController : AFCommonController
    {
        IMethodAroundMstService MethodAroundMstService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public MethodAroundMstController()
	    {
	        MethodAroundMstService = base.GetObject<IMethodAroundMstService>("NG3.Addin.Service.MethodAroundMst");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult MethodAroundMstList()
        {
			base.InitialMultiLanguage("MethodAroundMstList");
            return View("MethodAroundMstList");
        }

        /// <summary>
        /// 扫描当前系统可以提供服务的类
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string InitService()
        {
            var services = InterceptedServiceScanner.GetInterceptedService();
            return DataConverterHelper.EntityListToJson<InterceptedServiceBizModel>(services, services.Count);
        }

        /// <summary>
        /// 发布配置
        /// 
        /// </summary>
        /// <returns></returns>
        public string DeployConfigure()
        {
            var id = System.Web.HttpContext.Current.Request.Params["id"];//主键
            long phid = Convert.ToInt64(id);
            bool b = MethodAroundMstService.DeployConfigure(phid);
            if (b) return "true";

            return "false";
        }

        /// <summary>
        /// 取消发布
        /// </summary>
        /// <returns></returns>
        public string UndeployConfigure()
        {
            var id = System.Web.HttpContext.Current.Request.Params["id"];//主键
            long phid = Convert.ToInt64(id);
            bool b = MethodAroundMstService.UndeployConfigure(phid);
            if (b) return "true";

            return "false";

        }
        /// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult MethodAroundMstEdit()
        {
			base.InitialMultiLanguage("MethodAroundMstEdit");
			ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型

            if (ViewBag.OType == "add")
            {
				//新增时
				//if (MethodAroundMstService.Has_BillNoRule("取业务类型") == true)
                //{
                //    var vBillNo = MethodAroundMstService.GetBillNo();//取用户编码,新增时界面上显示
                //    ViewBag.No = vBillNo.BillNoList[0];
                //}
            }

            return View("MethodAroundMstEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetMethodAroundMstList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            //var result = MethodAroundMstService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);
            var result = MethodAroundMstService.GetMethodAroundMstList(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<MethodAroundMstModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetMethodAroundMstInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型


            //var findedresult = MethodAroundMstService.Find<Int64>(id);

            var findedresult = MethodAroundMstService.GetMethodAroundMst(id);

            return DataConverterHelper.ResponseResultToJson(findedresult);
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetAddinSqlList()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
                                                                                             // string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型


            IList<AddinSqlModel> sqlModels  = MethodAroundMstService.FindAddinSqlByMstPhid(id);

            return DataConverterHelper.EntityListToJson<AddinSqlModel>(sqlModels, sqlModels.Count);
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetAddinExpressionVarList()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            IList<AddinExpressionVarModel> Models = MethodAroundMstService.FindAddinExpressionVarByMstPhid(id);

            return DataConverterHelper.EntityListToJson<AddinExpressionVarModel>(Models, Models.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetAddinAssemblyList()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            IList<AddinAssemblyModel> Models = MethodAroundMstService.FindAddinAssemblyByMstPhid(id);

            return DataConverterHelper.EntityListToJson<AddinAssemblyModel>(Models, Models.Count);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetAddinExpressionList()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            IList<AddinExpressionModel> Models = MethodAroundMstService.FindAddinExpressionByMstPhid(id);

            return DataConverterHelper.EntityListToJson<AddinExpressionModel>(Models, Models.Count);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
            string mstformData = System.Web.HttpContext.Current.Request.Form["mstformData"];

            var mstforminfo = DataConverterHelper.JsonToEntity<MethodAroundMstModel>(mstformData);

            //sql数据
            string sqldata = System.Web.HttpContext.Current.Request.Form["sqlgriddata"];
            var sqlEntity = DataConverterHelper.JsonToEntity<AddinSqlModel>(sqldata);

            //表达式数据
            string expvardata = System.Web.HttpContext.Current.Request.Form["expvardata"];
            var expVarEntity = DataConverterHelper.JsonToEntity<AddinExpressionVarModel>(expvardata);

            //表达式
            string expression = System.Web.HttpContext.Current.Request.Form["expression"];
            var expressionEntity = DataConverterHelper.JsonToEntity<AddinExpressionModel>(expression);

            //var expressionEntity = new AddinExpressionModel();
            //expressionEntity.ExpText = expression;
            //List<AddinExpressionModel> expressionEntitys = new List<AddinExpressionModel>();
            //expressionEntitys.Add(expressionEntity);


            //程序集插件
            string assemblydata = System.Web.HttpContext.Current.Request.Form["assemblydata"];
            var assemblyEntity = DataConverterHelper.JsonToEntity<AddinAssemblyModel>(assemblydata);

            var savedresult = MethodAroundMstService.SaveMethodAround(mstforminfo.AllRow, sqlEntity.AllRow, expVarEntity.AllRow, expressionEntity.AllRow, assemblyEntity.AllRow);

            return DataConverterHelper.SerializeObject(savedresult);
        }

		/// <summary>
        /// 删除数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Delete()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            var deletedresult = MethodAroundMstService.DeleteMethodAround(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetFuncList()
        {
            var result = MethodAroundMstService.GetSupportFunctions();
            return DataConverterHelper.EntityListToJson<SupportFunctionBizModel>(result, result.Count);
        }
    }
}

