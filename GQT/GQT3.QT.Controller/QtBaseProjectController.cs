#region Summary
/**************************************************************************************
    * 类 名 称：        QtBaseProjectController
    * 命名空间：        GQT3.QT.Controller
    * 文 件 名：        QtBaseProjectController.cs
    * 创建时间：        2018/11/23 
    * 作    者：        刘杭    
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
using GYS3.YS.Service.Interface;
using GYS3.YS.Model.Domain;
using System.Linq;

namespace GQT3.QT.Controller
{
    /// <summary>
    /// QtBaseProject控制处理类
    /// </summary>
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class QtBaseProjectController : AFCommonController
    {
        IQtBaseProjectService QtBaseProjectService { get; set; }
        ICorrespondenceSettingsService CorrespondenceSettingsService { get; set; }
        IBudgetProcessCtrlService BudgetProcessCtrlService { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public QtBaseProjectController()
        {
            QtBaseProjectService = base.GetObject<IQtBaseProjectService>("GQT3.QT.Service.QtBaseProject");
            CorrespondenceSettingsService= base.GetObject<ICorrespondenceSettingsService>("GQT3.QT.Service.CorrespondenceSettings");
            BudgetProcessCtrlService = base.GetObject<IBudgetProcessCtrlService>("GYS3.YS.Service.BudgetProcessCtrl");
        }

        /// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QtBaseProjectList()
        {
            ViewBag.Title = base.GetMenuLanguage("QtBaseProject");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                ViewBag.Title = "单位对应基本项目";
            }
            base.InitialMultiLanguage("QtBaseProject");
            ViewBag.IndividualInfo = this.GetIndividualUI("QtBaseProject");
            return View("QtBaseProjectList");
        }

        /// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult QtBaseProjectEdit()
        {
            var tabTitle = base.GetMenuLanguage("QtBaseProject");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                tabTitle = "单位对应基本项目";
            }
            base.SetUserDefScriptUrl("QtBaseProject");
            base.InitialMultiLanguage("QtBaseProject");
            ViewBag.IndividualInfo = this.GetIndividualUI("QtBaseProject");

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

            return View("QtBaseProjectEdit");
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQtBaseProjectList()
        {
            //string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            //Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary
            long Fphid = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["Fphid"]);
            string FKMLB = System.Web.HttpContext.Current.Request.Params["FKMLB"];
            string FYear = System.Web.HttpContext.Current.Request.Params["FYear"];
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();

            new CreateCriteria(dicWhere).
                        Add(ORMRestrictions<System.Int64>.Eq("Fphid", Fphid))
                        .Add(ORMRestrictions<string>.Eq("FKMLB", FKMLB));
            if (!string.IsNullOrEmpty(FYear))//取科目及当前年度的子科目
            {
                Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
                Dictionary<string, object> dicWhere3 = new Dictionary<string, object>();
                new CreateCriteria(dicWhere2).
                        Add(ORMRestrictions<string>.Eq("FYear", FYear));
                new CreateCriteria(dicWhere3).
                        Add(ORMRestrictions<string>.Eq("FProjCode", ""));
                new CreateCriteria(dicWhere).Add(ORMRestrictions.Or(dicWhere2, dicWhere3));
            }

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = QtBaseProjectService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere, new string[] { "FKmdm", "NgInsertDt" });

            return DataConverterHelper.EntityListToJson<QtBaseProjectModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQtBaseProjectInfo()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
            var findedresult = QtBaseProjectService.Find(id);
            return DataConverterHelper.ResponseResultToJson(findedresult);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
            string qtbaseprojectformData = System.Web.HttpContext.Current.Request.Params["qtbaseprojectformData"];
            string subjectMstformData = System.Web.HttpContext.Current.Request.Params["subjectMstformData"];

            //var qtbaseprojectforminfo = DataConverterHelper.JsonToEntity<QtBaseProjectModel>(qtbaseprojectformData);
            QtBaseProjectModel qtBaseProjectModel = JsonConvert.DeserializeObject<QtBaseProjectModel>(qtbaseprojectformData);
            SubjectMstModel subjectMstModel = JsonConvert.DeserializeObject<SubjectMstModel>(subjectMstformData);
            
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            /*if (QtBaseProjectService.JudgeIfEnd(qtBaseProjectModel.FKmdm) == false)
            {
                savedresult.Status = ResponseStatus.Error;
                savedresult.Msg = "该科目为非末级科目！";
                return DataConverterHelper.SerializeObject(savedresult);
            }*/
            try
            {
                qtBaseProjectModel.PersistentState = PersistentState.Added;
                //savedresult = QtBaseProjectService.Save<Int64>(qtBaseProjectModel);


                //subjectMstModel.FYear = DateTime.Today.Year.ToString();//年度改为前台传
                subjectMstModel.FProjCode = qtBaseProjectModel.FProjCode;
                subjectMstModel.FBudgetDept = "浙江省总工会";
                subjectMstModel.FDateofDeclaration = DateTime.Today;
                subjectMstModel.FDeclarer = base.UserID.ToString();
                subjectMstModel.FApproveStatus = "1";
                subjectMstModel.FCarryOver = 1;
                subjectMstModel.PersistentState = PersistentState.Added;
                //savedresult = SubjectMstService.Save<Int64>(subjectMstModel);


                savedresult = QtBaseProjectService.Save2(qtBaseProjectModel, subjectMstModel);
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

            var deletedresult = QtBaseProjectService.Delete2(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public string Update()
        {
            string qtbaseprojectformData = System.Web.HttpContext.Current.Request.Params["qtbaseprojectformData"];
            QtBaseProjectModel qtBaseProjectModel = JsonConvert.DeserializeObject<QtBaseProjectModel>(qtbaseprojectformData);
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            savedresult = QtBaseProjectService.Update2(qtBaseProjectModel);
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 判断是否有明细
        /// </summary>
        /// <returns></returns>
        public string JudgeHaveDtl()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            CommonResult result = new CommonResult();
            if (QtBaseProjectService.JudgeHaveDtl(id) == true)
            {
                result.Status = ResponseStatus.Error;
                result.Msg = "该子科目已有明细内容，是否继续删除";
            }
            else
            {
                result.Status = ResponseStatus.Success;
            }
            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 删除有明细的数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string DeleteIfDtl()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            CommonResult deletedresult = new CommonResult();
            try
            {
                deletedresult = QtBaseProjectService.DeleteIfDtl(id);
            }
            catch (Exception ex)
            {
                deletedresult.Status = ResponseStatus.Error;
                deletedresult.Msg = ex.Message.ToString();
            }
            
            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 判断是否为末级科目
        /// </summary>
        /// <returns></returns>
        public string JudgeIfEnd()
        {
            string FKmdm = System.Web.HttpContext.Current.Request.Params["FKmdm"];
            CommonResult result = new CommonResult();
            if (QtBaseProjectService.JudgeIfEnd(FKmdm) == false)
            {
                result.Status = ResponseStatus.Error;
                result.Msg = "该科目为非末级科目！";
                
            }

            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetQtBaseProjectSelect()
        {
            //long Fphid = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["Fphid"]);
            string userCode= System.Web.HttpContext.Current.Request.Params["userCode"];
            string FKMLB = System.Web.HttpContext.Current.Request.Params["FKMLB"];
            string FType = System.Web.HttpContext.Current.Request.Params["FType"];
            string FYear = System.Web.HttpContext.Current.Request.Params["FYear"];
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            var DeptList = CorrespondenceSettingsService.GetRelationYSBMRightList(userCode).Results.ToList().Select(x => x.OCode).Distinct().ToList();
            new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<string>.Eq("FKMLB", FKMLB))
                        .Add(ORMRestrictions<string>.NotEq("FProjCode", ""))
                        .Add(ORMRestrictions<List<string>>.In("FFillDept", DeptList));
            if (!string.IsNullOrEmpty(FYear))
            {
                new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<string>.Eq("FYear", FYear));
            }
            DataStoreParam storeparam = this.GetDataStoreParam();
            //var result = QtBaseProjectService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere, new string[] { "FKmdm", "NgInsertDt" });
            var result = new PagedResult<QtBaseProjectModel>();
            if (FType=="c")
            {
                result = QtBaseProjectService.ServiceHelper.LoadWithPageInfinity("GQT.QT.TBJD.c", dicWhere);
            }
            else
            {
                result = QtBaseProjectService.ServiceHelper.LoadWithPageInfinity("GQT.QT.TBJD.z", dicWhere);
                foreach(QtBaseProjectModel a in result.Results)
                {
                    if (a.FType == "c")
                    {
                        a.FProjName = a.FProjName + "(年初新增)";
                    }
                    if (a.FType == "z")
                    {
                        a.FProjName = a.FProjName + "(年中新增)";
                    }
                    if (a.FType == "tz")
                    {
                        a.FProjName = a.FProjName + "(年中调整)";
                    }
                }
            }
            //var result = QtBaseProjectService.ServiceHelper.LoadWithPageInfinity("GQT.QT.TBJD", dicWhere);
            return DataConverterHelper.EntityListToJson<QtBaseProjectModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetTaskreferenceList()
        {
            long Fphid = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["Fphid"]);
            //string FKMLB = System.Web.HttpContext.Current.Request.Params["FKMLB"];
            string FYear = System.Web.HttpContext.Current.Request.Params["FYear"];
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();

            new CreateCriteria(dicWhere).
                        Add(ORMRestrictions<System.Int64>.Eq("Fphid", Fphid)).
                        Add(ORMRestrictions<string>.NotEq("FProjCode", ""));
            //.Add(ORMRestrictions<string>.Eq("FKMLB", FKMLB));
            if (!string.IsNullOrEmpty(FYear))//取科目及当前年度的子科目
            {
                new CreateCriteria(dicWhere).
                        Add(ORMRestrictions<string>.Eq("FYear", FYear));
            }

            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = QtBaseProjectService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere, new string[] { "FKmdm", "NgInsertDt" });

            return DataConverterHelper.EntityListToJson<QtBaseProjectModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string SaveTaskreference()
        {
            long PhId= Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["PhId"]);
            string FYear = System.Web.HttpContext.Current.Request.Params["FYear"];

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            try
            {
                QtBaseProjectModel qtBaseProjectModel = QtBaseProjectService.Find(PhId).Data;
                //进度控制判断
                var processStatus = BudgetProcessCtrlService.FindBudgetProcessCtrl(qtBaseProjectModel.FDwdm, qtBaseProjectModel.FFillDept, FYear);
                if (processStatus == "1")
                {
                    qtBaseProjectModel.FType = "c";
                }
                else if (processStatus == "3")
                {
                    qtBaseProjectModel.FType = "z";
                }
                else
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = "当前填报部门不能指派任务！";
                    return DataConverterHelper.SerializeObject(savedresult);
                }
                //重复的子科目项目判断
                var dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere).
                    Add(ORMRestrictions<string>.Eq("FKmdm", qtBaseProjectModel.FKmdm)).
                    Add(ORMRestrictions<string>.Eq("FDwdm", qtBaseProjectModel.FDwdm)).
                    Add(ORMRestrictions<string>.Eq("FProjName", qtBaseProjectModel.FProjName)).
                    Add(ORMRestrictions<string>.Eq("FYear",FYear));
                if (QtBaseProjectService.Find(dicWhere).Data.Count > 0)
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = "相同科目下不允许重复的子科目项目名称存在！";
                    return DataConverterHelper.SerializeObject(savedresult);
                }
                qtBaseProjectModel.PhId = 0;
                qtBaseProjectModel.FYear = FYear;
                qtBaseProjectModel.FProjCode = QtBaseProjectService.CreateOrGetMaxProjCode(FYear);
                qtBaseProjectModel.PersistentState = PersistentState.Added;
                savedresult = QtBaseProjectService.Save<Int64>(qtBaseProjectModel,"");

/*
                //subjectMstModel.FYear = DateTime.Today.Year.ToString();//年度改为前台传
                subjectMstModel.FProjCode = qtBaseProjectModel.FProjCode;
                subjectMstModel.FBudgetDept = "浙江省总工会";
                subjectMstModel.FDateofDeclaration = DateTime.Today;
                subjectMstModel.FDeclarer = base.UserID.ToString();
                subjectMstModel.FApproveStatus = "1";
                subjectMstModel.FCarryOver = 1;
                subjectMstModel.PersistentState = PersistentState.Added;
                //savedresult = SubjectMstService.Save<Int64>(subjectMstModel);


                savedresult = QtBaseProjectService.Save2(qtBaseProjectModel, subjectMstModel);*/
            }
            catch (Exception ex)
            {
                savedresult.Status = ResponseStatus.Error;
                savedresult.Msg = ex.Message.ToString();
            }
            return DataConverterHelper.SerializeObject(savedresult);
        }
    }
}

