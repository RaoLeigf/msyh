#region Summary
/**************************************************************************************
    * 类 名 称：        VCorrespondenceSetting2Controller
    * 命名空间：        GQT3.QT.Controller
    * 文 件 名：        VCorrespondenceSetting2Controller.cs
    * 创建时间：        2018/9/13 
    * 作    者：        李长敏琛    
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
using System.Collections;
using GQT3.QT.Dac;
using System.Linq;

namespace GQT3.QT.Controller
{
	/// <summary>
	/// VCorrespondenceSetting2控制处理类
	/// </summary>
	[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class VCorrespondenceSetting2Controller : AFCommonController
    {
        IVCorrespondenceSetting2Service VCorrespondenceSetting2Service { get; set; }
        ICorrespondenceSettings2Service CorrespondenceSettings2Service { get; set; }
        ICorrespondenceSettingsService CorrespondenceSettingsService { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public VCorrespondenceSetting2Controller()
	    {
	        VCorrespondenceSetting2Service = base.GetObject<IVCorrespondenceSetting2Service>("GQT3.QT.Service.VCorrespondenceSetting2");
            CorrespondenceSettings2Service = base.GetObject<ICorrespondenceSettings2Service>("GQT3.QT.Service.CorrespondenceSettings2");
            CorrespondenceSettingsService = base.GetObject<ICorrespondenceSettingsService>("GQT3.QT.Service.CorrespondenceSettings");
        }

		/// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult VCorrespondenceSetting2List()
        {
			ViewBag.Title = base.GetMenuLanguage("VCorrespondenceSettings2");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "对应关系设置2(视图)";
            }
            base.InitialMultiLanguage("VCorrespondenceSettings2");
            ViewBag.IndividualInfo = this.GetIndividualUI("VCorrespondenceSettings2");
            return View("VCorrespondenceSetting2List");
        }


        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult RelationZCQD()
        {
            //this.VCorrespondenceSetting2Service.GetOrange();
            return View();
        }
        /// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult VCorrespondenceSetting2Edit(String dwdm)
        {
			var tabTitle = base.GetMenuLanguage("VCorrespondenceSettings2");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "对应关系设置2(视图)";
            }
            base.SetUserDefScriptUrl("VCorrespondenceSettings2");
            base.InitialMultiLanguage("VCorrespondenceSettings2");
            ViewBag.IndividualInfo = this.GetIndividualUI("VCorrespondenceSettings2");

			ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型

			if (ViewBag.OType == "add")
            {
			    ViewBag.Title = tabTitle + "-新增";
            }
            else if (ViewBag.OType == "edit")
            {
                ViewBag.Title = tabTitle + "-修改";
                ViewBag.Dwdm = dwdm;
                return View("RelationZCQDSet");
            }
            else if (ViewBag.OType == "view")
            {
                ViewBag.Title = tabTitle + "-查看";
            }

            return View("VCorrespondenceSetting2Edit");
        }

		/// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetVCorrespondenceSetting2List(String dwdm)
        {
            //         string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            //Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary
            //         CreateCriteria createCriteria = new CreateCriteria(dicWhere);
            //         createCriteria.Add(ORMRestrictions<string>.Eq("Dylx", "ZC"));
            //         createCriteria.Add(ORMRestrictions<string>.NotEq("Dydm","NULL"));
            //         if (dwdm != null)
            //         {
            //             createCriteria.Add(ORMRestrictions<string>.Eq("Dwdm", dwdm));
            //         }

            //         DataStoreParam storeparam = this.GetDataStoreParam();
            //         //var result = VCorrespondenceSetting2Service.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere);
            //         var cr2s = this.CorrespondenceSettings2Service.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere).Results as List<CorrespondenceSettings2Model>;
            //         var ors = this.GetOrg();
            //         IList<VCorrespondenceSetting2Model> vcr2s = new List<VCorrespondenceSetting2Model>();
            //         foreach (CorrespondenceSettings2Model cr2 in cr2s) {
            //             VCorrespondenceSetting2Model vc2 = new VCorrespondenceSetting2Model();             
            //             vc2.PhId = cr2.PhId;
            //             vc2.DWDM = cr2.Dwdm;              
            //             vc2.DYDM = cr2.Dydm;
            //             vc2.DYLX = cr2.Dylx;

            //             var or = from or1 in ors
            //                      where or1.OCode.Equals(cr2.Dydm)
            //                      select or1;
            //             if (or.Count() == 1)
            //             {
            //                 vc2.Dymc = or.ToList()[0].OName;
            //             }
            //             else {
            //                 vc2.Dymc = "未设置";
            //             }
            //             or = from or1 in ors
            //                  where or1.OCode.Equals(cr2.Dwdm)
            //                  select or1;
            //             if (or.Count() == 1)
            //             {
            //                 vc2.Dwmc = or.ToList()[0].OName;
            //                 vc2.Dymc = "未设置";
            //             }

            //             vcr2s.Add(vc2);
            //         }
            //var result = VCorrespondenceSetting2Service.LoadWithPageInfinity("GQT3.QT.Org_ZCQD", dicWhere);

            var vcr2s = this.GetVc2mList(dwdm);
            return DataConverterHelper.EntityListToJson<VCorrespondenceSetting2Model>(vcr2s, (Int32)vcr2s.Count());
        }

        public IList<VCorrespondenceSetting2Model> GetVc2mList(String dwdm)
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];
            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary
            CreateCriteria createCriteria = new CreateCriteria(dicWhere);
            createCriteria.Add(ORMRestrictions<string>.Eq("Dylx", "ZC"));
            createCriteria.Add(ORMRestrictions<string>.NotEq("Dydm", "NULL"));
            if (dwdm != null)
            {
                createCriteria.Add(ORMRestrictions<string>.Eq("Dwdm", dwdm));
            }

            DataStoreParam storeparam = this.GetDataStoreParam();
            var cr2s = this.CorrespondenceSettings2Service.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere).Results as List<CorrespondenceSettings2Model>;
            var ors = this.GetOrg();
            IList<VCorrespondenceSetting2Model> vcr2s = new List<VCorrespondenceSetting2Model>();
            foreach (CorrespondenceSettings2Model cr2 in cr2s)
            {
                VCorrespondenceSetting2Model vc2 = new VCorrespondenceSetting2Model();
                vc2.PhId = cr2.PhId;
                vc2.DWDM = cr2.Dwdm;
                vc2.DYDM = cr2.Dydm;
                vc2.DYLX = cr2.Dylx;

                var or = from or1 in ors
                         where or1.OCode.Equals(cr2.Dydm)
                         select or1;
                if (or.Count() == 1)
                {
                    vc2.Dymc = or.ToList()[0].OName;
                }
                else
                {
                    vc2.Dymc = "未设置";
                }
                or = from or1 in ors
                     where or1.OCode.Equals(cr2.Dwdm)
                     select or1;
                if (or.Count() == 1)
                {
                    vc2.Dwmc = or.ToList()[0].OName;
                }
                else {
                    vc2.Dymc = "未设置";
                }

                vcr2s.Add(vc2);
            }
            return vcr2s;
        }


        public List<OrganizeModel> GetOrg()
        {
            DataStoreParam dataStoreParam = this.GetDataStoreParam();
            var result = CorrespondenceSettingsService.LoadWithPageOrg(dataStoreParam);
            return result.Results as List<OrganizeModel>;
        }

        public string GetSpendUnRealdyList(String dwdm)
        {
            //DeletedResult dr = new VCorrespondenceSetting2Facade().Delete(-1);
            //DacHelper<OrganizeModel> dacHelper = new DacHelper<OrganizeModel>();
            //OrganizeModel t = dacHelper.GetById();
            //IList<OrganizeModel> ms=dacHelper.GetSession().QueryOver<OrganizeModel>().List();
            //可用序列化结合泛型优化，待做
            List<OrganizeModel> organizes = this.GetOrg();
            //List<OrganizeModel> organizes_result = this.GetOrg();
            //Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(null);
            //CreateCriteria createCriteria = new CreateCriteria(dicWhere);
            //createCriteria.Add(ORMRestrictions<string>.Eq("DYLX", "ZC"));
            //if (dwdm != null)
            //{
            //    createCriteria.Add(ORMRestrictions<string>.Eq("DWDM", dwdm));
            //}
            //IList<VCorrespondenceSetting2Model> temmp = VCorrespondenceSetting2Service.LoadWithPage(0, Int32.MaxValue, dicWhere).Results;

            IList<VCorrespondenceSetting2Model> temmp = this.GetVc2mList(dwdm);
            foreach (VCorrespondenceSetting2Model vc2m in temmp) {
                int i = 0;
                while (i<organizes.Count)
                {
                    if (organizes[i].OCode.Equals(vc2m.DYDM)|| organizes[i].OCode.Equals(dwdm))
                    {
                        organizes.Remove(organizes[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            return DataConverterHelper.EntityListToJson<OrganizeModel>(organizes, (Int32)organizes.Count);
        }
        public string UpdateR(String dwphid,String dwdm,String selected,String unselected)
        {
            int dr = -1;
            int sr = 1;
            //第一步，获得该单位已保存的所有支出渠道
            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(null);
            CreateCriteria createCriteria = new CreateCriteria(dicWhere);
            createCriteria.Add(ORMRestrictions<string>.Eq("Dylx", "ZC"));
            if (dwdm != null)
            {
                createCriteria.Add(ORMRestrictions<string>.Eq("Dwdm", dwdm));
            }
            List<CorrespondenceSettings2Model> correspondenceSettings2s = CorrespondenceSettings2Service.LoadWithPage(0, Int32.MaxValue, dicWhere).Results as List<CorrespondenceSettings2Model>;
            ArrayList dbSave = new ArrayList();
            String[] selects = selected.Split(',');
            String[] unseleteds = unselected.Split(',');
            foreach (CorrespondenceSettings2Model c2m in correspondenceSettings2s)
            {
                foreach (String us in unseleteds)
                {
                    if (c2m.Dydm.Equals(us))
                    {
                        CorrespondenceSettings2Service.Delete(c2m.PhId);
                        dr--;
                        break;
                    }
                    else {
                        dbSave.Add(c2m.Dydm);
                    }
                }
            }
            foreach (String s in selects)
            {
                if (!dbSave.Contains(s)&& s!=null&&!s.Equals("")&&!s.Equals(" "))
                {
                    CorrespondenceSettings2Model c2m = new CorrespondenceSettings2Model();
                    c2m.Dylx = "ZC";
                    c2m.Dydm = s;
                    c2m.Dwdm = dwdm;
                    c2m.DefStr2 = dwphid;
                    c2m.PersistentState = PersistentState.Added;
                    //List<CorrespondenceSettings2Model> tc2 = new List<CorrespondenceSettings2Model>();
                    //tc2.Add(c2m);
                    String resulet= DataConverterHelper.SerializeObject(CorrespondenceSettings2Service.Save<Int64>(c2m,""));
                    //c2m.PhId = 55466;
                    //tc2.Clear();
                    //tc2.Add(c2m);
                    //resulet = DataConverterHelper.SerializeObject(CorrespondenceSettings2Service.Save<Int64>(tc2));
                    sr++;
                }
            }

            return (dr*sr).ToString();
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetVCorrespondenceSetting2Info()
        {
			long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
			string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
			var findedresult = VCorrespondenceSetting2Service.Find(id);
			return DataConverterHelper.ResponseResultToJson(findedresult);
        }

		/// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
			string vcorrespondencesetting2formData = System.Web.HttpContext.Current.Request.Form["vcorrespondencesetting2formData"];

			var vcorrespondencesetting2forminfo = DataConverterHelper.JsonToEntity<VCorrespondenceSetting2Model>(vcorrespondencesetting2formData);

			SavedResult<Int64> savedresult = new SavedResult<Int64>();
			try
			{
				savedresult = VCorrespondenceSetting2Service.Save<Int64>(vcorrespondencesetting2forminfo.AllRow,"");
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
			int id = Convert.ToInt32(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            var deletedresult = VCorrespondenceSetting2Service.Delete<System.Int32>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

    }
}

