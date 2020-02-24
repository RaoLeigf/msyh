using Enterprise3.NHORM.Controller;
using NG3.Addin.Model.Domain;
using NG3.Addin.Model.Domain.BusinessModel;
using NG3.Addin.Service.Interface;
using NG3.Web.Mvc;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NG3.Addin.Controller
{
    /// <summary>
    /// 新增加功能controller
    /// </summary>
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class BasicSettingController  : AFCommonController
    {

        IBasicSettingService BasicSettingService { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
	    public BasicSettingController()
        {
             BasicSettingService = base.GetObject<IBasicSettingService>("NG3.Addin.Service.BasicSetting");
        }



        /// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult BasicSettingList()
        {
            base.InitialMultiLanguage("BasicSettingList");
            return View("BasicSettingList");
        }


        /// <summary>
        /// 取得操作员列表
        /// </summary>
        /// <returns></returns>
        public string GetAddinOperatorList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = BasicSettingService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<AddinOperatorModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public string SaveAddinOperator()
        {
            //sql数据
            string operdata = System.Web.HttpContext.Current.Request.Form["operdata"];
            var operModel= DataConverterHelper.JsonToEntity<AddinOperatorModel>(operdata);
            var savedresult = BasicSettingService.SaveAddinOperator(operModel.AllRow);
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public string DeleteAddinOperator()
        {
            //sql数据
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            var deletedresult = BasicSettingService.DeleteAddinOperator(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }


        /// <summary>
        /// 取系统变量
        /// </summary>
        /// <returns></returns>
        public string GetSystemVariables()
        {
            var result = BasicSettingService.GetSystemVariables();
            return  DataConverterHelper.EntityListToJson<SupportVariableBizModel>(result, result.Count);
        }

        /// <summary>
        /// 取业务变量
        /// </summary>
        /// <returns></returns>
        public string GetBizVariables()
        {
            var result = BasicSettingService.GetBizVariables();
            return DataConverterHelper.EntityListToJson<SupportVariableBizModel>(result, result.Count);
        }


        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <returns></returns>
        public string GetServiceRequestParameters()
        {
            var result = BasicSettingService.GetServiceRequestParameters();
            return DataConverterHelper.EntityListToJson<ServiceUIParamBizModel>(result, result.Count);
        }
    }
}
