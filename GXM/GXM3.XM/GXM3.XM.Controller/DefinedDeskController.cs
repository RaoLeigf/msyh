#region Summary
/**************************************************************************************
    * 类 名 称：        DefinedDeskControlle
    * 命名空间：        GXM3.XM.Controller
    * 文 件 名：        DefinedDeskControlle.cs
    * 创建时间：        2018/8/29 
    * 作    者：        吾丰明    
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
using GXM3.XM.Service.Interface;
using GYS3.YS.Service.Interface;
using GXM3.XM.Model;
using Newtonsoft.Json.Converters;
using Enterprise3.Common.Base.Criterion;

namespace GXM3.XM.Controller
{
    /// <summary>
    /// 自定义桌面
    /// </summary>
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class DefinedDeskController : AFCommonController
    {
        IProjectMstService ProjectMstService { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public DefinedDeskController()
        {
            ProjectMstService = base.GetObject<IProjectMstService>("GXM3.XM.Service.ProjectMst");
        }

        /// <summary>
        /// 打开桌面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult Index()
        {
            ViewBag.Title = "自定义桌面";
            return View("Index");
        }
        
        /// <summary>
        /// 指向下载pb页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult Index_DownLoadPB()
        {

            ViewBag.Title = "下载";
            return View("Index_DownLoadPB");
        }

        /// <summary>
        /// 指向下载pb页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult LoginCenter()
        {

            ViewBag.Title = "登录中心";
            return View("LoginCenter");
        }

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        [HttpPost]
        public string ProjectDataCount()
        {
            string ysmstphid = System.Web.HttpContext.Current.Request.Params["ng3_logid"];  //当前人员
            string usercode = System.Web.HttpContext.Current.Request.Params["ng3_usercode"];  //当前人员//根据年度过滤
            var sessionFYear = System.Web.HttpContext.Current.Request.Params["sessionFYear"];

            var dicWhere = new Dictionary<string, object>();
            long ng3_logid = 0;
            long.TryParse(ysmstphid, out ng3_logid);

            var model = ProjectMstService.GetDataCount(0, 1, ng3_logid, usercode, sessionFYear);
            //var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            //return JsonConvert.SerializeObject(result, timeConverter);

            var list = new List<ProjectCountModel>();
            list.Add(model);

            return DataConverterHelper.EntityToJson<ProjectCountModel>(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        [HttpPost]
        public string GetNotice()
        {
            IList<Notice> noticeList = new List<Notice>
            {
                new Notice { id = 1, title = "1.关于推荐申报全国模范职工之家、全国模范", datetime="2018-09-05"},
                new Notice { id = 2, title = "2.关于推荐申报全国模范职工之家、全国模范", datetime="2018-09-05"},
                new Notice { id = 3, title = "3.关于推荐申报全国模范职工之家、全国模范", datetime="2018-09-05"},
                new Notice { id = 4, title = "4.关于推荐申报全国模范职工之家、全国模范", datetime="2018-09-05" },
                new Notice { id = 5, title = "5.关于推荐申报全国模范职工之家、全国模范", datetime="2018-09-05" },
                new Notice { id = 6, title = "6.关于推荐申报全国模范职工之家、全国模范", datetime="2018-09-05" },
                new Notice { id = 7, title = "7.关于推荐申报全国模范职工之家、全国模范", datetime="2018-09-05"}
            };

            return DataConverterHelper.EntityListToJson<Notice>(noticeList, (Int32)noticeList.Count);
        }

    }
}
