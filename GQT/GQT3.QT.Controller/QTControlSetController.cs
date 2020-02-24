#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Controller
    * 类 名 称：			QTControlSetController
    * 文 件 名：			QTControlSetController.cs
    * 创建时间：			2019/4/3 
    * 作    者：			刘杭    
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

namespace GQT3.QT.Controller
{
	/// <summary>
	/// QTControlSet控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class QTControlSetController : AFCommonController
    {
        IQTControlSetService QTControlSetService { get; set; }
        ICorrespondenceSettings2Service CorrespondenceSettings2Service { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public QTControlSetController()
	    {
	        QTControlSetService = base.GetObject<IQTControlSetService>("GQT3.QT.Service.QTControlSet");
            CorrespondenceSettings2Service = base.GetObject<ICorrespondenceSettings2Service>("GQT3.QT.Service.CorrespondenceSettings2");
        }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QTControlSetList()
        {
			ViewBag.Title = base.GetMenuLanguage("QTControlSet");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "页面功能控制设置表";
            }
            base.InitialMultiLanguage("QTControlSet");
            ViewBag.IndividualInfo = this.GetIndividualUI("QTControlSet");
            return View("QTControlSetList");
        }

		/// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QTControlSetEdit()
        {
			var tabTitle = base.GetMenuLanguage("QTControlSet");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "页面功能控制设置表";
            }
            base.SetUserDefScriptUrl("QTControlSet");
            base.InitialMultiLanguage("QTControlSet");
            ViewBag.IndividualInfo = this.GetIndividualUI("QTControlSet");

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

            return View("QTControlSetEdit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTControlSetList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
			Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = QTControlSetService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);

            return DataConverterHelper.EntityListToJson<QTControlSetModel>(result.Results, (Int32)result.TotalItems);
        }

		/// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTControlSetInfo()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = QTControlSetService.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string qtcontrolsetformData = System.Web.HttpContext.Current.Request.Form["qtcontrolsetformData"];

			var qtcontrolsetforminfo = DataConverterHelper.JsonToEntity<QTControlSetModel>(qtcontrolsetformData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = QTControlSetService.Save<Int64>(qtcontrolsetforminfo.AllRow,"");
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

            var deletedresult = QTControlSetService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string UpdateOrg()
        {
            string SetPhId = System.Web.HttpContext.Current.Request.Params["SetPhId"];

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            try
            {
                savedresult = QTControlSetService.UpdateOrg(SetPhId);
            }
            catch (Exception ex)
            {
                savedresult.Status = ResponseStatus.Error;
                savedresult.Msg = ex.Message.ToString();
            }
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 更改是否控制保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string SaveIfControl()
        {
            var ModifieData = System.Web.HttpContext.Current.Request.Params["ModifieData"];
            List<QTControlSetModel> qTControlSets = JsonConvert.DeserializeObject<List<QTControlSetModel>>(ModifieData);
            List<QTControlSetModel> updateData=new List<QTControlSetModel>();
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            for (var i=0;i< qTControlSets.Count; i++)
            {
                QTControlSetModel qTControlSet = QTControlSetService.Find(qTControlSets[i].PhId).Data;
                qTControlSet.ControlOrNot = qTControlSets[i].ControlOrNot;
                qTControlSet.BEGINFDATE = qTControlSets[i].BEGINFDATE;
                qTControlSet.PersistentState = PersistentState.Modified;
                updateData.Add(qTControlSet);
            }
            
            try
            {
                savedresult=QTControlSetService.Save<Int64>(updateData, "");
            }
            catch(Exception ex)
            {
                savedresult.Status = ResponseStatus.Error;
                savedresult.Msg = ex.Message.ToString();
            }
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 根据BZ获取数据（只取控制的）
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTControlByBZ()
        {
            string BZ = System.Web.HttpContext.Current.Request.Params["BZ"];
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            Dictionary<string, object> dicWhereSet = new Dictionary<string, object>();
            new CreateCriteria(dicWhereSet)
               .Add(ORMRestrictions<string>.Eq("BZ", BZ))
               .Add(ORMRestrictions<string>.Eq("ControlOrNot", "1"));
            IList<QTControlSetModel> SetResult = QTControlSetService.Find(dicWhereSet).Data;


            if (SetResult.Count > 0)
            {
                savedresult.Msg = "true";
            }
            else
            {
                savedresult.Msg = "false";
            }

            savedresult.Status = ResponseStatus.Success;

            return DataConverterHelper.SerializeObject(savedresult);
            
        }

        /// <summary>
        /// 根据BZ获取数据（只取控制的）
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetAllQTControlByBZ()
        {
            string BZ = System.Web.HttpContext.Current.Request.Params["BZ"];
            Dictionary<string, object> dicWhereSet = new Dictionary<string, object>();
            new CreateCriteria(dicWhereSet)
               .Add(ORMRestrictions<string>.Eq("BZ", BZ));
            var SetResult = QTControlSetService.Find(dicWhereSet);

            return DataConverterHelper.SerializeObject(SetResult);

        }

        /// <summary>
        /// 根据BZ和组织phid获取数据（只取控制的）
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQTControlByBZAndOrg()
        {
            string BZ = System.Web.HttpContext.Current.Request.Params["BZ"];
            string OrgCode = System.Web.HttpContext.Current.Request.Params["OrgCode"];//查询条件
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            Dictionary<string, object> dicWhereSet = new Dictionary<string, object>();
            new CreateCriteria(dicWhereSet)
               .Add(ORMRestrictions<string>.Eq("BZ", BZ))
               .Add(ORMRestrictions<string>.Eq("ControlOrNot", "1"));
            IList<QTControlSetModel> SetResult = QTControlSetService.Find(dicWhereSet).Data;


            if (SetResult.Count > 0)
            {
                Dictionary<string, object> dicWheredygx = new Dictionary<string, object>();
                new CreateCriteria(dicWheredygx).Add(ORMRestrictions<string>.Eq("Dylx", "button"))
                    .Add(ORMRestrictions<string>.Eq("Dydm", OrgCode))
                    .Add(ORMRestrictions<string>.Eq("DefStr1", BZ));
                var result = CorrespondenceSettings2Service.Find(dicWheredygx).Data;
                if (result.Count > 0)
                {
                    savedresult.Msg = "true";
                }
                else
                {
                    savedresult.Msg = "false";
                }
            }
            else
            {
                savedresult.Msg = "false";
            }

            savedresult.Status = ResponseStatus.Success;

            return DataConverterHelper.SerializeObject(savedresult);

        }
    }
}

