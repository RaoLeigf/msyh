using Enterprise3.Common.Base.Criterion;
using Enterprise3.Common.Model.Results;
using Enterprise3.WebApi.ApiControllerBase;
using Enterprise3.WebApi.GQT3.QT.Model;
using Enterprise3.WebApi.GQT3.QT.Model.Request;
using GQT3.QT.Model.Domain;
using GQT3.QT.Service.Interface;
using Newtonsoft.Json;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Enterprise3.WebApi.GQT3.QT.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [MethodExceptionFilter]
    public class SysSettingApiController : ApiBase
    {
        IQTMemoService QTMemoService { get; set; }

        IExpenseCategoryService ExpenseCategoryService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SysSettingApiController()
        {
            QTMemoService = base.GetObject<IQTMemoService>("GQT3.QT.Service.QTMemo");
            ExpenseCategoryService = base.GetObject<IExpenseCategoryService>("GQT3.QT.Service.ExpenseCategory");
        }

        /// <summary>
        /// 页面里的按钮权限
        /// </summary>
        /// <param name="parament"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetMenuList([FromUri]ButtonInfo parament)
        {
            ResultModel result = new ResultModel();

            if (string.IsNullOrWhiteSpace(parament.orgid))
            {
                return DCHelper.ErrorMessage("组织id为空！");
            }

            if (string.IsNullOrWhiteSpace(parament.uid))
            {
                return DCHelper.ErrorMessage("用户id为空！");
            }

            try
            {

                string userType = NG3.AppInfoBase.UserType;

                var buttonlist = QTMemoService.GetFormRights(long.Parse(parament.uid), long.Parse(parament.orgid), userType, parament.rightname);
                result.Status = "success";
                result.Data = buttonlist;

                return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
            
        }
        /// <summary>
        /// 根据当前userid和orgid，来获取模块菜单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetLoadMenu([FromUri]MenuInfoModel param)
        {

            ResultModel result = new ResultModel();

            string nodeid = param.node;
            string suite = param.suite;

            //是否控制权限的开关，flag,默认为true，控制权限
            string flagString = param.flag;
            bool rightFlag = true;
            if (flagString == "false")
            {
                rightFlag = false;
            }


            //系统功能树是否懒加载的开关，lazyLoadFlag,默认为true
            string lazyLoadFlagString = param.lazyLoadFlag;
            bool lazyLoadFlag = false;
            if (lazyLoadFlagString == "true")
            {
                lazyLoadFlag = true;
            }

            //按指定SQL语句构建系统功能树
            string treeFilter = param.treeFilter;

            if (string.IsNullOrWhiteSpace(param.orgid))
            {
                return DCHelper.ErrorMessage("组织id为空！");
            }

            if (string.IsNullOrWhiteSpace(param.uid))
            {
                return DCHelper.ErrorMessage("用户id为空！");
            }

            try
            {
                string userType = NG3.AppInfoBase.UserType;
                SUP.Common.Base.ProductInfo prdInfo = new SUP.Common.Base.ProductInfo();

                DataTable menulist = QTMemoService.GetLoadMenu(prdInfo.ProductCode + prdInfo.Series, suite, false, userType, long.Parse(param.orgid), long.Parse(param.uid), nodeid, rightFlag, lazyLoadFlag, treeFilter);

                return "{\"totalRows\":" + menulist.Rows.Count + ",\"Record\":" + JsonConvert.SerializeObject(menulist) + "}";
                //return DCHelper.ModelListToJson(list, list.Count);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 获取支出集合
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetExpenseCategoryList([FromUri]BaseListModel baseList)
        {
            try
            {
                //Dictionary<string, object> dic = new Dictionary<string, object>();
                //new CreateCriteria(dic).Add(ORMRestrictions<long>.NotEq("PhId", (long)0));
                //var result = this.ExpenseCategoryService.Find(dic);
                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "08"));
                if (!string.IsNullOrEmpty(baseList.orgid))
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dwdm", baseList.orgid));
                }
                var result = ExpenseCategoryService.ServiceHelper.LoadWithPageInfinity("GQT.QT.DYGX2-ZCLB", dicWhere).Results;
                return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
    }
}
