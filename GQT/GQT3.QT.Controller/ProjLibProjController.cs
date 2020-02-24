#region Summary
/**************************************************************************************
    * 类 名 称：        ProjLibProjController
    * 命名空间：        GQT3.QT.Controller
    * 文 件 名：        ProjLibProjController.cs
    * 创建时间：        2018/9/10 
    * 作    者：        李明    
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
using Enterprise3.Common.Base.Criterion;
using System.Linq;

namespace GQT3.QT.Controller
{
	/// <summary>
	/// ProjLibProj控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ProjLibProjController : AFCommonController
    {
        IProjLibProjService ProjLibProjService { get; set; }
        IProjectMstService ProjectMstService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public ProjLibProjController()
	    {
	        ProjLibProjService = base.GetObject<IProjLibProjService>("GQT3.QT.Service.ProjLibProj");
            ProjectMstService = base.GetObject<IProjectMstService>("GXM3.XM.Service.ProjectMst");
        }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult ProjLibProjList()
        {
			ViewBag.Title = base.GetMenuLanguage("ProjLibProj");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "项目库项目";
            }
            base.InitialMultiLanguage("ProjLibProj");
            ViewBag.IndividualInfo = this.GetIndividualUI("ProjLibProj");
            return View("ProjLibProjList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult ProjLibProjEdit()
        {
			var tabTitle = base.GetMenuLanguage("ProjLibProj");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "项目库项目";
            }
            base.SetUserDefScriptUrl("ProjLibProj");
            base.InitialMultiLanguage("ProjLibProj");
            ViewBag.IndividualInfo = this.GetIndividualUI("ProjLibProj");

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

            return View("ProjLibProjEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetProjLibProjList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = ProjLibProjService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<ProjLibProjModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetProjLibProjInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = ProjLibProjService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string projlibprojformData = System.Web.HttpContext.Current.Request.Form["projlibprojformData"];
            string year = System.Web.HttpContext.Current.Request.Params["year"];

            var projlibprojforminfo = DataConverterHelper.JsonToEntity<ProjLibProjModel>(projlibprojformData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
            try
            {
                if (projlibprojforminfo.NewRow.Count > 0)
                {
                    string dm = projlibprojforminfo.NewRow[0].DM;
                    if (string.IsNullOrEmpty(dm))
                    {
                        dm = ProjectMstService.CreateOrGetMaxProjCode(year);
                        projlibprojforminfo.NewRow[0].DM = dm;
                    }
                }

                savedresult = ProjLibProjService.Save<Int64>(projlibprojforminfo.AllRow,"");

                if (projlibprojforminfo.NewRow.Count > 0 && savedresult.KeyCodes.Count>0 ) {                    
                    savedresult.Data =  JsonConvert.DeserializeObject( "{" + string.Format(@"'DM':'{0}', 'MC':'{1}', 'PhId':{2}", projlibprojforminfo.NewRow[0].DM, projlibprojforminfo.NewRow[0].MC, savedresult.KeyCodes[0]) + "}");
                }
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

            var deletedresult = ProjLibProjService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 同级增加获取代码
        /// </summary>
        /// <returns></returns>
        public string getCode()
        {
            string parentCode = System.Web.HttpContext.Current.Request.Params["parentCode"];
            string year= System.Web.HttpContext.Current.Request.Params["year"];
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic).Add(ORMRestrictions<string>.Eq("DEFSTR2", parentCode));
            if (parentCode == "") {
                new CreateCriteria(dic).Add(ORMRestrictions<string>.LLike("DM", year));
            }
            List<ProjLibProjModel> ProjLibProjList = ProjLibProjService.Find(dic,new string[] { "DM Desc"}).Data.ToList().FindAll(x=>!x.DM.Contains("."));
            if (ProjLibProjList.Count > 0)
            {
                var Top = ProjLibProjList[0].DM.Substring(0, ProjLibProjList[0].DM.Length - 4);
                var End = ProjLibProjList[0].DM.Substring(ProjLibProjList[0].DM.Length - 4);
                var End2 = "0000" + (Convert.ToInt64(End) + 1).ToString();
                return Top+ End2.Substring(End2.Length-4);
            }
            else
            {
                if (parentCode == "")
                {
                    return ProjectMstService.CreateOrGetMaxProjCode(year);
                }
                else
                {
                    return parentCode + "0001";
                }
            }

        }

        /// <summary>
        /// 判断是否允许修改
        /// </summary>
        /// <returns></returns>
        public string JudgeCode()
        {
            string code = System.Web.HttpContext.Current.Request.Params["code"];
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic).Add(ORMRestrictions<string>.LLike("DM", code));
            IList<ProjLibProjModel> ProjLibProjList = ProjLibProjService.Find(dic).Data;
            if (ProjLibProjList.Count > 0)
            {
                var dmList = ProjLibProjList.ToList().Select(x => x.DM).ToList();
                Dictionary<string, object> dic2 = new Dictionary<string, object>();
                new CreateCriteria(dic2).Add(ORMRestrictions<List<string>>.In("FProjCode", dmList))
                    .Add(ORMRestrictions<List<string>>.In("FApproveStatus", new List<string>() { "2","3"}))
                    .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0)); ;
                var mst=ProjectMstService.Find(dic2).Data;
                if (mst.Count > 0)
                {
                    return "false";
                }
                else
                {
                    return "true";
                }
            }
            else
            {
                return "true";
            }
        }
    }
}

