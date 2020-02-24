#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Controller
    * 类 名 称：			QTIndividualInfoController
    * 文 件 名：			QTIndividualInfoController.cs
    * 创建时间：			2019/5/14 
    * 作    者：			董泉伟    
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
using System.Linq;

namespace GQT3.QT.Controller
{
	/// <summary>
	/// QTIndividualInfo控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class QTIndividualInfoController : AFCommonController
    {
        IQTIndividualInfoService QTIndividualInfoService { get; set; }

		/// <summary>
        /// 构造函数
        /// </summary>
	    public QTIndividualInfoController()
	    {
	        QTIndividualInfoService = base.GetObject<IQTIndividualInfoService>("GQT3.QT.Service.QTIndividualInfo");
	    }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QTIndividualInfoList()
        {
			ViewBag.Title = base.GetMenuLanguage("QTIndividualInfo");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "表单自定义跟金额对应表";
            }
            base.InitialMultiLanguage("QTIndividualInfo");
            ViewBag.IndividualInfo = this.GetIndividualUI("QTIndividualInfo");
            return View("QTIndividualInfoList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QTIndividualInfoEdit()
        {
			var tabTitle = base.GetMenuLanguage("QTIndividualInfo");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "表单自定义跟金额对应表";
            }
            base.SetUserDefScriptUrl("QTIndividualInfo");
            base.InitialMultiLanguage("QTIndividualInfo");
            ViewBag.IndividualInfo = this.GetIndividualUI("QTIndividualInfo");

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

            return View("QTIndividualInfoEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTIndividualInfoList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary
            string IndividualinfoBustype = System.Web.HttpContext.Current.Request.Params["IndividualinfoBustype"];
            string OrgCode = System.Web.HttpContext.Current.Request.Params["OrgCode"];
            if (!string.IsNullOrEmpty(IndividualinfoBustype))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("IndividualinfoBustype", IndividualinfoBustype));
            }
            

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = QTIndividualInfoService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere, new string[] { "IndividualinfoBustype" , "DEFINT1 ASC", "NgInsertDt Desc", "NgUpdateDt Desc" });
            if (result.Results.Count > 0&&!string.IsNullOrEmpty(OrgCode))
            {
                result.Results = result.Results.ToList().FindAll(x => !string.IsNullOrEmpty(x.DEFSTR9) && x.DEFSTR9.Split(',').ToList().Contains(OrgCode));
            }
            return DataConverterHelper.EntityListToJson<QTIndividualInfoModel>(result.Results, (Int32)result.Results.Count);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTIndividualInfoInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = QTIndividualInfoService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string qtindividualinfoformData = System.Web.HttpContext.Current.Request.Form["qtindividualinfoformData"];

			var qtindividualinfoforminfo = DataConverterHelper.JsonToEntity<QTIndividualInfoModel>(qtindividualinfoformData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = QTIndividualInfoService.Save<Int64>(qtindividualinfoforminfo.AllRow,"");
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

            var Data = QTIndividualInfoService.Find(id);
            if (!string.IsNullOrEmpty(Data.Data.YLXPhid))
            {
                QTIndividualInfoService.SaveTemple(0, "GHBudgetYLX", id);
            }
            if (!string.IsNullOrEmpty(Data.Data.XMLXPhid))
            {
                QTIndividualInfoService.SaveTemple(0, "GHBudgetXMLX", id);
            }
            if (!string.IsNullOrEmpty(Data.Data.NZTXPhid))
            {
                QTIndividualInfoService.SaveTemple(0, "GHProjectBegin", id);
            }

            var deletedresult = QTIndividualInfoService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 保存记录
        /// </summary>
        /// <returns></returns>
        public string SaveData()
        {
            string qtindividualinfoData = System.Web.HttpContext.Current.Request.Form["data"];
            var qtindividualinfoforminfo = DataConverterHelper.JsonToEntity<QTIndividualInfoModel>(qtindividualinfoData);
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            try
            {
                savedresult = QTIndividualInfoService.Save<Int64>(qtindividualinfoforminfo.AllRow, "");
                if(savedresult.Status == ResponseStatus.Success && savedresult.KeyCodes.Count > 0)
                {
                    for(var i  = 0; i < savedresult.KeyCodes.Count; i++)
                    {
                        var Data = QTIndividualInfoService.Find(savedresult.KeyCodes[i]);
                        if(!string.IsNullOrEmpty(Data.Data.YLXPhid))
                        {
                            QTIndividualInfoService.SaveTemple(Convert.ToInt64(Data.Data.YLXPhid), "GHBudgetYLX", savedresult.KeyCodes[i]);
                        }
                        if (!string.IsNullOrEmpty(Data.Data.XMLXPhid))
                        {
                            QTIndividualInfoService.SaveTemple(Convert.ToInt64(Data.Data.XMLXPhid), "GHBudgetXMLX", savedresult.KeyCodes[i]);
                        }
                        if (!string.IsNullOrEmpty(Data.Data.NZTXPhid))
                        {
                            QTIndividualInfoService.SaveTemple(Convert.ToInt64(Data.Data.NZTXPhid), "GHProjectBegin", savedresult.KeyCodes[i]);
                        }
                    }
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
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        //public string saveTemple()
        //{
        //    string qtindividualinfoTempleData = System.Web.HttpContext.Current.Request.Form["data"];

        //    var qtindividualinfoforminfo = DataConverterHelper.JsonToEntity<QTIndividualInfoTempleModel>(qtindividualinfoTempleData);

        //    if (qtindividualinfoforminfo.ModifyRow.Count > 0)
        //    {
        //        var newList = new List<QTIndividualInfoModel>();

        //        for (var i = 0; i < qtindividualinfoforminfo.ModifyRow.Count; i ++)
        //        {
        //            var newmodel = new QTIndividualInfoModel();
        //            newmodel.IndividualinfoBustype = qtindividualinfoforminfo.ModifyRow[i].bustype;
        //            newmodel.IndividualinfoName = qtindividualinfoforminfo.ModifyRow[i].name;
        //            newmodel.IndividualinfoPhid = qtindividualinfoforminfo.ModifyRow[i].PhId;
        //            newmodel.IndividualinfoBustypeName = BustypeName(qtindividualinfoforminfo.ModifyRow[i].bustype);
        //        }
        //    }
        //    if (qtindividualinfoforminfo.NewRow.Count > 0)
        //    {
        //        for (var i = 0; i < qtindividualinfoforminfo.NewRow.Count; i++)
        //        {

        //        }
        //    }

        //    if (qtindividualinfoforminfo.DeleteRow.Count > 0)
        //    {
        //        for (var i = 0; i < qtindividualinfoforminfo.DeleteRow.Count; i++)
        //        {
        //            var dicWhere = new Dictionary<string, object>();
        //            new CreateCriteria(dicWhere)
        //                .Add(ORMRestrictions<Int64>.Eq("IndividualinfoPhid", qtindividualinfoforminfo.DeleteRow[i].PhId)); //闭区间
        //            var result = QTIndividualInfoService.Find(dicWhere);
        //            if(result.Data.Count > 0)
        //            {
        //                //根据模板类型和对应设置的主键删除关联
        //                var deleteResult = QTIndividualInfoService.Delete<System.Int64>(result.Data[0].PhId);
        //                var ret = QTIndividualInfoService.DeleteTemple(qtindividualinfoforminfo.DeleteRow[i].bustype, result.Data[0].PhId);
        //            }

        //        }

        //    }

        //    SavedResult<Int64> savedresult = new SavedResult<Int64>();
        //    //try
        //    //{
        //    //    savedresult = QTIndividualInfoService.Save<Int64>(qtindividualinfoforminfo.AllRow, "");
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    savedresult.Status = ResponseStatus.Error;
        //    //    savedresult.Msg = ex.Message.ToString();
        //    //}
        //    return DataConverterHelper.SerializeObject(savedresult);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bustype"></param>
        /// <returns></returns>
        public string BustypeName(string bustype)
        {
            var bustypeName = "";
            switch (bustype)
            {
                case "GHProjectBegin":
                    bustypeName = "年中调整";
                    break;
                case "GHBudgetYLX":
                    bustypeName = "预立项";
                    break;
                case "GHBudgetXMLX":
                    bustypeName = "项目立项";
                    break;
                case "GHExpenseMst":
                    bustypeName = "项目支出预算审批";
                    break;
                case "GHPerformanceMst":
                    bustypeName = "绩效评价";
                    break;
                default:
                    break;

            }
            return bustypeName;

        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTIndividualInfoOrg()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            var data = QTIndividualInfoService.Find(id).Data;
            var Org = QTIndividualInfoService.GetUseOrg(data.DEFSTR9);

            return DataConverterHelper.EntityListToJson<OrganizeModel>(Org, (Int32)Org.Count);
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTIndividualInfoNoOrg()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            var data = QTIndividualInfoService.Find(id).Data;
            var Org = QTIndividualInfoService.GetNoUseOrg(data.DEFSTR9);

            return DataConverterHelper.EntityListToJson<OrganizeModel>(Org, (Int32)Org.Count);
        }

    }
}

