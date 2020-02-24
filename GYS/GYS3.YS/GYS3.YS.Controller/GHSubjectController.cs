#region Summary
/**************************************************************************************
    * 类 名 称：        GHSubjectController
    * 命名空间：        GYS3.YS.Controller
    * 文 件 名：        GHSubjectController.cs
    * 创建时间：        2018/11/26 
    * 作    者：        董泉伟    
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

using GYS3.YS.Service.Interface;
using GYS3.YS.Model.Domain;
using GQT3.QT.Service.Interface;
using GQT3.QT.Model.Domain;

using Enterprise3.Common.Base.Criterion;
using System.Linq;

namespace GYS3.YS.Controller
{
    /// <summary>
    /// GHSubject控制处理类
    /// </summary>
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class GHSubjectController : AFCommonController
    {
        IGHSubjectService GHSubjectService { get; set; }
        ICorrespondenceSettingsService CorrespondenceSettingsService { get; set; }
        IQtBaseProjectService QtBaseProjectService { get; set; }
        IQtSysCodeSeqService SysCodeSeqService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public GHSubjectController()
        {
            GHSubjectService = base.GetObject<IGHSubjectService>("GYS3.YS.Service.GHSubject");
            CorrespondenceSettingsService = base.GetObject<ICorrespondenceSettingsService>("GQT3.QT.Service.CorrespondenceSettings");
            SysCodeSeqService = base.GetObject<IQtSysCodeSeqService>("GQT3.QT.Service.QtSysCodeSeq");
            QtBaseProjectService = base.GetObject<IQtBaseProjectService>("GQT3.QT.Service.QtBaseProject");
        }

        /// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult GHSubjectList()
        {
            string Fkmlb = System.Web.HttpContext.Current.Request.Params["FKmlb"];
            ViewBag.Title = base.GetMenuLanguage("GHSubjectMst");//根据业务类型对应的langkey取多语言
            ViewBag.Iftz = System.Web.HttpContext.Current.Request.Params["Iftz"];//是否调整 是为1
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                if(Fkmlb == "1")
                {
                    ViewBag.Title = "基本支出";
                }
                else if(Fkmlb == "0")
                {
                    ViewBag.Title = "基本收入";
                }
                if (ViewBag.Iftz == "1")
                {
                    ViewBag.Title = ViewBag.Title + "调整";
                }

            }
            ViewBag.Fkmlb = Fkmlb;
            base.InitialMultiLanguage("GHSubjectMst");
            ViewBag.IndividualInfo = this.GetIndividualUI("GHSubjectMst");
            return View("GHSubjectList");
        }

        /// <summary>
        /// 指向编辑页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult GHSubjectEdit()
        {
            string Fkmlb = System.Web.HttpContext.Current.Request.Params["FKmlb"];
            var deleteBefore = System.Web.HttpContext.Current.Request.Params["deleteBefore"];//1 同步删除原单据
            var deleteAndAdd = System.Web.HttpContext.Current.Request.Params["deleteAndAdd"];//1 同步原单据内容至新单据(作废原单据)
            var tabTitle = base.GetMenuLanguage("GHSubjectMst");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(tabTitle))
            {
                if (Fkmlb == "1")
                {
                    tabTitle = "基本支出";
                }
                else if (Fkmlb == "0")
                {
                    tabTitle = "基本收入";
                }
               
            }
            base.SetUserDefScriptUrl("GHSubjectMst");
            base.InitialMultiLanguage("GHSubjectMst");
            ViewBag.IndividualInfo = this.GetIndividualUI("GHSubjectMst");

            ViewBag.Fkmlb = Fkmlb;
            ViewBag.deleteBefore = deleteBefore;
            ViewBag.deleteAndAdd = deleteAndAdd;
            ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型
            ViewBag.FType = System.Web.HttpContext.Current.Request.Params["FType"];//c年初申报 z年中新增
            if (ViewBag.OType == "add")
            {
                if (ViewBag.FType == "tz")
                {
                    ViewBag.Title = tabTitle + "-年中调整";
                }
                else if(ViewBag.FType=="c")
                {
                    ViewBag.Title = tabTitle + "-年初新增";
                }
                else
                {
                    ViewBag.Title = tabTitle + "-年中新增";
                }
                //ViewBag.Title = tabTitle + "-新增";
            }
            else if (ViewBag.OType == "edit")
            {
                ViewBag.Title = tabTitle + "-修改";
            }
            else if (ViewBag.OType == "view")
            {
                ViewBag.Title = tabTitle + "-查看";
            }

            return View("GHSubjectEdit");
        }

        /// <summary>
        /// 指向基本收/支申报查询页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult GHSubjectSelect()
        {
            //string Fkmlb = System.Web.HttpContext.Current.Request.Params["FKmlb"];
            ViewBag.Title = base.GetMenuLanguage("GHSubjectMst");//根据业务类型对应的langkey取多语言
            ViewBag.FType = System.Web.HttpContext.Current.Request.Params["FType"];
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                if (ViewBag.FType == "z")
                {
                    ViewBag.Title = "基本收/支年中调整";
                }
                else
                {
                    ViewBag.Title = "基本收/支申报查询";
                }
                
            }
            //ViewBag.Fkmlb = Fkmlb;
            base.InitialMultiLanguage("GHSubjectMst");
            ViewBag.IndividualInfo = this.GetIndividualUI("GHSubjectMst");
            return View("GHSubjectSelect");
        }

        /// <summary>
        /// 指向列表页面
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult GHSubjectBMHZ()
        {
            string Fkmlb = System.Web.HttpContext.Current.Request.Params["FKmlb"];
            string SaveAllDept = System.Web.HttpContext.Current.Request.Params["SaveAllDept"];
            ViewBag.Title = base.GetMenuLanguage("GHSubjectMst");//根据业务类型对应的langkey取多语言
            if (string.IsNullOrWhiteSpace(ViewBag.Title))
            {
                if (Fkmlb == "1")
                {
                    ViewBag.Title = "基本支出-各部门汇总";
                }
                else if (Fkmlb == "0")
                {
                    ViewBag.Title = "收入预算-各部门汇总";
                }

            }
            ViewBag.Fkmlb = Fkmlb;
            ViewBag.SaveAllDept = SaveAllDept;
            base.InitialMultiLanguage("GHSubjectMst");
            ViewBag.IndividualInfo = this.GetIndividualUI("GHSubjectMst");
            return View("GHSubjectBMHZ");
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetGHSubjectList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary
            string Fkmlb = System.Web.HttpContext.Current.Request.Params["FKmlb"];
            string SaveAllDept = System.Web.HttpContext.Current.Request.Params["SaveAllDept"];
            string Iftz = System.Web.HttpContext.Current.Request.Params["Iftz"];//是否调整
            
            string FYear = System.Web.HttpContext.Current.Request.Params["FYear"];
            if (!string.IsNullOrEmpty(FYear))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", FYear));
            }
            if (!string.IsNullOrEmpty(Fkmlb))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FProjAttr", Fkmlb));
            }

            
            if (SaveAllDept == "1")
            {
                if (Fkmlb == "1")
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FProjName", "各部门基本支出汇总数据"));
                }
                else
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FProjName", "各部门收入预算汇总数据"));

                }
            }
            else
            {
                //增加根据操作员对应预算部门的过滤
                var userId = System.Web.HttpContext.Current.Request.Params["userId"];

                var dicWhereDept = new Dictionary<string, object>();
                new CreateCriteria(dicWhereDept)
                    .Add(ORMRestrictions<string>.Eq("Dwdm", userId)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
                var deptList = CorrespondenceSettingsService.Find(dicWhereDept);
                List<string> deptL = new List<string>();
                for (var i = 0; i < deptList.Data.Count; i++)
                {
                    deptL.Add(deptList.Data[i].Dydm);
                }
                new CreateCriteria(dicWhere).Add(ORMRestrictions<IList<String>>.In("FDeclarationDept", deptL));
                if (Iftz == "1")
                {
                    Dictionary<string, object> dicWhere1 = new Dictionary<string, object>();
                    Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
                    new CreateCriteria(dicWhere1)
                        .Add(ORMRestrictions<List<String>>.NotIn("FApproveStatus", new List<string>() { "5","6", "7" }))
                        .Add(ORMRestrictions<String>.NotEq("FType", "c"));
                    if (Fkmlb == "1")
                    {
                        new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FProjName", "各部门基本支出汇总数据"));
                    }
                    else
                    {
                        new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FProjName", "各部门收入预算汇总数据"));
                    }
                    new CreateCriteria(dicWhere).Add(ORMRestrictions.Or(dicWhere1, dicWhere2));
                }
                else
                {
                    Dictionary<string, object> dicWhere1 = new Dictionary<string, object>();
                    Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
                    new CreateCriteria(dicWhere1)
                        .Add(ORMRestrictions<string>.Eq("FType", "c"));
                    if (Fkmlb == "1")
                    {
                        new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FProjName", "各部门基本支出汇总数据"));
                    }
                    else
                    {
                        new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FProjName", "各部门收入预算汇总数据"));
                    }
                    new CreateCriteria(dicWhere).Add(ORMRestrictions.Or(dicWhere1, dicWhere2));
                }
            }
            DataStoreParam storeparam = this.GetDataStoreParam();
            var result = GHSubjectService.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" });

            return DataConverterHelper.EntityListToJson<GHSubjectModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetGHSubjectInfo()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键
            string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
            switch (tabtype)
            {
                case "ghsubject":
                    var findedresultghsubject = GHSubjectService.Find(id);
                    return DataConverterHelper.ResponseResultToJson(findedresultghsubject);
                case "subjectmst":
                    var findedresultsubjectmst = GHSubjectService.FindSubjectMstByForeignKey(id);
                    foreach(var item in findedresultsubjectmst.Data)
                    {
                        if( !string.IsNullOrEmpty(item.FProjCode) || !string.IsNullOrEmpty(item.FProjName))
                        {
                            item.FSubjectCode = "";
                            item.FSubjectName = "";
                        }
                    }
                    return DataConverterHelper.EntityListToJson(findedresultsubjectmst.Data, findedresultsubjectmst.Data.Count);
                case "subjectmstbudgetdtl":
                    var findedresultsubjectmstbudgetdtl = GHSubjectService.FindSubjectMstBudgetDtlByForeignKey(id);
                    return DataConverterHelper.EntityListToJson(findedresultsubjectmstbudgetdtl.Data, findedresultsubjectmstbudgetdtl.Data.Count);
                default:
                    FindedResult findedresultother = new FindedResult();
                    return DataConverterHelper.ResponseResultToJson(findedresultother);
            }
        }

        /// <summary>
        /// 获取子科目明细数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetGHSubjectmstbudgetdtlInfo()
        {
            var findedresultsubjectmstbudgetdtl = GHSubjectService.FindSubjectMstBudgetDtl();
            return DataConverterHelper.EntityListToJson(findedresultsubjectmstbudgetdtl.Data, findedresultsubjectmstbudgetdtl.Data.Count);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public string Save()
        {
            string ghsubjectformData = System.Web.HttpContext.Current.Request.Form["ghsubjectformData"];
            string subjectmstgridData = System.Web.HttpContext.Current.Request.Form["subjectmstgridData"];
            string subjectmstbudgetdtlgridData = System.Web.HttpContext.Current.Request.Form["subjectmstbudgetdtlgridData"];
            string deleteBefore = System.Web.HttpContext.Current.Request.Form["deleteBefore"];
            string deleteAndAdd = System.Web.HttpContext.Current.Request.Form["deleteAndAdd"];

            var ghsubjectforminfo = DataConverterHelper.JsonToEntity<GHSubjectModel>(ghsubjectformData);
            var subjectmstgridinfo = DataConverterHelper.JsonToEntity<SubjectMstModel>(subjectmstgridData);
            var subjectmstbudgetdtlgridinfo = DataConverterHelper.JsonToEntity<SubjectMstBudgetDtlModel>(subjectmstbudgetdtlgridData);


            for(var i= 0; i < subjectmstgridinfo.AllRow.Count; i ++)
            {
                var item = subjectmstgridinfo.AllRow[i];

                item.FAmountAfterEdit = item.FProjAmount + item.FAmountEdit;
                
                if (i>0 && ( !string.IsNullOrEmpty(item.FProjCode) || !string.IsNullOrEmpty(item.FProjName)) && string.IsNullOrEmpty(item.FSubjectCode))
                {
                    Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FProjCode", item.FProjCode));
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", item.FYear));

                    var baseSubject = QtBaseProjectService.Find(dicWhere);
                    if(baseSubject.Data.Count > 0)
                    {
                        item.FSubjectCode = baseSubject.Data[0].FKmdm;
                        item.FSubjectName = baseSubject.Data[0].Fkmmc;
                    }
                    
                }
            }
            for(var i= 0; i < subjectmstbudgetdtlgridinfo.AllRow.Count; i++)
            {
                var item = subjectmstbudgetdtlgridinfo.AllRow[i];
                
                item.FAmountAfterEdit = item.FAmount + item.FAmountEdit;
                
            }

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            try
            {
                savedresult = GHSubjectService.SaveGHSubject(ghsubjectforminfo.AllRow[0], subjectmstgridinfo.AllRow, subjectmstbudgetdtlgridinfo.AllRow);
            }
            catch (Exception ex)
            {
                savedresult.Status = ResponseStatus.Error;
                savedresult.Msg = ex.Message.ToString();
            }

            if(savedresult.Status == ResponseStatus.Success)
            {
                //查找该填报部门下已拥有未上报的填报单据
                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FDeclarationDept", ghsubjectforminfo.AllRow[0].FDeclarationDept))
                        .Add(ORMRestrictions<string>.Eq("FYear", ghsubjectforminfo.AllRow[0].FYear))
                        .Add(ORMRestrictions<string>.Eq("FProjAttr", ghsubjectforminfo.AllRow[0].FProjAttr))
                        
                         .Add(ORMRestrictions<Int64>.NotEq("PhId", savedresult.KeyCodes[0]));
                if (ghsubjectforminfo.AllRow[0].FType != "tz") { 
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FApproveStatus", "1"))//未作废数据
                         .Add(ORMRestrictions<string>.Eq("FType", ghsubjectforminfo.AllRow[0].FType));
                }
                else
                {
                    //new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.NotEq("FApproveStatus", "5"));
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FApproveStatus", "3"))
                        .Add(ORMRestrictions<List<string>>.In("FType", new List<string>() {"c","tz" }));
                }
                if (ghsubjectforminfo.AllRow[0].FProjAttr == "0")
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.NotEq("FProjName", "各部门收入预算汇总数据"));
                }
                else if (ghsubjectforminfo.AllRow[0].FProjAttr == "1")
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.NotEq("FProjName", "各部门基本支出汇总数据"));//现在列表只显示各部门自己单独数据
                }

                var findSubject = GHSubjectService.Find(dicWhere);
                //同步原单据内容至新单据后 原单据状态改为作废
                if (deleteAndAdd == "1")//&& ghsubjectforminfo.AllRow[0].FType!="tz"
                {
                   foreach(var item in findSubject.Data)
                    {
                        if (ghsubjectforminfo.AllRow[0].FType == "tz" && item.FType=="c")
                        {
                            item.FApproveStatus = "6";
                        }
                        else
                        {
                            item.FApproveStatus = "5";
                        }
                        item.PersistentState = PersistentState.Modified;
                    }
                    GHSubjectService.Save<Int64>(findSubject.Data, "");
                }
                //同步删除原申报单据
                if (deleteBefore == "1")// && ghsubjectforminfo.AllRow[0].FType != "tz"
                {
                    foreach (var item in findSubject.Data)
                    {
                        GHSubjectService.Delete<Int64>(item.PhId);
                    }
                    
                }
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
            var data = GHSubjectService.Find(id).Data;
            if (data.FType == "tz"&& data.FApproveStatus!="5")//删除年中调整 未作废单据 恢复上一个年中调整单据
            {
                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FDeclarationDept", data.FDeclarationDept))
                        .Add(ORMRestrictions<string>.Eq("FYear", data.FYear))
                        .Add(ORMRestrictions<string>.Eq("FProjAttr", data.FProjAttr))
                        .Add(ORMRestrictions<string>.Eq("FApproveStatus", "5"))
                         .Add(ORMRestrictions<string>.Eq("FType", data.FType));
                IList<GHSubjectModel> gHSubjects = GHSubjectService.Find(dicWhere, new string[] { "NgUpdateDt Desc" }).Data;
                if (gHSubjects.Count > 0)
                {
                    gHSubjects[0].FApproveStatus = "3";
                    gHSubjects[0].PersistentState = PersistentState.Modified;
                    GHSubjectService.Save<long>(gHSubjects[0], "");
                }
                else
                {
                    //没有年中调整单据时将年初新增状态为调整中的变成已审批
                    Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
                    new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FDeclarationDept", data.FDeclarationDept))
                            .Add(ORMRestrictions<string>.Eq("FYear", data.FYear))
                            .Add(ORMRestrictions<string>.Eq("FProjAttr", data.FProjAttr))
                            .Add(ORMRestrictions<string>.Eq("FApproveStatus", "6"))
                             .Add(ORMRestrictions<string>.Eq("FType", "c"));
                    IList<GHSubjectModel> gHSubjects2 = GHSubjectService.Find(dicWhere).Data;
                    if (gHSubjects2.Count > 0)
                    {
                        foreach(var a in gHSubjects2)
                        {
                            a.FApproveStatus = "3";
                            a.PersistentState = PersistentState.Modified;
                        }
                        GHSubjectService.Save<long>(gHSubjects2, "");
                    }
                }
            }
            else
            {
                
            }
            var deletedresult = GHSubjectService.Delete<System.Int64>(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 取消送审
        /// </summary>
        /// <returns></returns>
        public string UnChecked()
        {
            long id = Convert.ToInt64(System.Web.HttpContext.Current.Request.Params["id"]);  //主表主键

            var Findresult = GHSubjectService.Find<System.Int64>(id);
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
           
            
            Findresult.Data.FApproveStatus = "1";
            Findresult.Data.FApprover = 0;
            Findresult.Data.FApproveDate = new Nullable<DateTime>();
            Findresult.Data.PersistentState = PersistentState.Modified;
            savedresult = GHSubjectService.Save<Int64>(Findresult.Data, "");

            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 查找该填报部门下的科目
        /// </summary>
        /// <returns></returns>
        public string FindSubjectData()
        {
            var FDwdm = System.Web.HttpContext.Current.Request.Params["FDwdm"];
            var FKmlb = System.Web.HttpContext.Current.Request.Params["FKmlb"];
            var Dept = System.Web.HttpContext.Current.Request.Params["Dept"];
            var deleteBefore = System.Web.HttpContext.Current.Request.Params["deleteBefore"];
            var deleteAndAdd = System.Web.HttpContext.Current.Request.Params["deleteAndAdd"];
            var FType= System.Web.HttpContext.Current.Request.Params["FType"];

            var FYear = System.Web.HttpContext.Current.Request.Params["FYear"];

            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            //if (!string.IsNullOrEmpty(FYear))
            //{
            //    new CreateCriteria(dicWhere)
            //    .Add(ORMRestrictions<string>.Eq("FYear", FYear));
            //}
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

            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("FDwdm", FDwdm)).Add(ORMRestrictions<string>.Eq("FKMLB", FKmlb));
            
            var FindSubjectData = QtBaseProjectService.Find(dicWhere, new string[] { "FKmdm", "NgInsertDt" });
            //var FindSubjectData = QtBaseProjectService.FindSubjectData(FDwdm, FKmlb);

            var OldSubject = new List<SubjectMstModel>();
            if (deleteAndAdd == "0")
            {
                 OldSubject = FindOldSubject(Dept, FKmlb, FYear);
            }
            List<string> fprojcodeList = new List<string>();
            if (FType == "tz")
            {
                Dictionary<string, object> dicWheretz = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(FYear))
                {
                    new CreateCriteria(dicWheretz)
                    .Add(ORMRestrictions<string>.Eq("FYear", FYear));
                }
                new CreateCriteria(dicWheretz)
                    .Add(ORMRestrictions<string>.Eq("FDeclarationDept", Dept))
                    .Add(ORMRestrictions<string>.Eq("FApproveStatus", "3"))
                    .Add(ORMRestrictions<List<string>>.In("FType", new List<string>() { "c", "tz" }));
                IList<GHSubjectModel> subjectModels = GHSubjectService.Find(dicWheretz).Data;
                if (subjectModels.Count > 0)
                {
                    for (var q = 0; q < subjectModels.Count; q++)
                    {
                        FindedResults<SubjectMstModel> mstModels = GHSubjectService.FindSubjectMstByForeignKey(subjectModels[q].PhId);
                        if (mstModels.Data.Count > 0)
                        {
                            fprojcodeList.AddRange(mstModels.Data.ToList().FindAll(x => !string.IsNullOrEmpty(x.FProjCode)).Select(x => x.FProjCode).Distinct().ToList());
                        }
                    }
                }
            }

            IList<QtBaseProjectModel> SubjectMst = new List<QtBaseProjectModel>();
            var count = FindSubjectData.Data.Count;
            //先去除其他填报部门的子科目
            for (var i =0; i < count; i++)
            {
                //去除填报部门不为空，又不是选择的填报部门的数据
                if(!string.IsNullOrEmpty( FindSubjectData.Data[i].FFillDept) && FindSubjectData.Data[i].FFillDept != Dept)
                {
                    FindSubjectData.Data.RemoveAt(i);
                    i--;
                    count--;
                    continue;
                }
                if(FindSubjectData.Data[i].FFillDept == Dept)
                {
                    //0时表示不同步原单据内容至新单据，则原单据有数据时则不添加到新单据
                    if (deleteAndAdd == "0")
                    {
                        if(OldSubject.FindIndex(s => s.FProjCode.Equals(FindSubjectData.Data[i].FProjCode)) >= 0)
                        {
                            FindSubjectData.Data.RemoveAt(i);
                            i--;
                            count--;
                            continue;
                        }
                    }

                    if (FType == "tz")
                    {
                        
                        if (FindSubjectData.Data[i].FType == "c"&& fprojcodeList.Contains(FindSubjectData.Data[i].FProjCode))
                        {
                            SubjectMst.Add(FindSubjectData.Data[i]);
                        }
                    }
                    else 
                    {
                        if (FindSubjectData.Data[i].FType == FType)
                        {
                            SubjectMst.Add(FindSubjectData.Data[i]);
                        }
                        else
                        {
                            FindSubjectData.Data.RemoveAt(i);
                            i--;
                            count--;
                            continue;
                        }
                    }
                }
            }

            var count1 = FindSubjectData.Data.Count;
            var chileSubject = 0;
            for (var i = 0; i < count1;i++)
            {
                //if (string.IsNullOrEmpty(FindSubjectData.Data[i].FFillDept))
                if (string.IsNullOrEmpty(FindSubjectData.Data[i].FFillDept) || (FindSubjectData.Data[i].FFillDept== Dept))
                {
                    chileSubject = 0;
                    for (var j = 0; j < SubjectMst.Count;j++)
                    {
                        if(SubjectMst[j].FKmdm.IndexOf(FindSubjectData.Data[i].FKmdm,0) == 0)
                        {
                            chileSubject = 1;
                            break;
                        }
                    }
                    if(chileSubject == 0)
                    {
                        FindSubjectData.Data.RemoveAt(i);
                        i--;
                        count1--;
                    }
                }
            }

            //var count1 = FindSubjectData.Results.Count;
            //var chileSubject =0;
            //for(var i = 0; i <count1; i++)
            //{
            //    chileSubject = 0;
            //    //填报部门为空时，查找下面有没有拥有子项目的子科目，没有的则删除
            //    if (string.IsNullOrEmpty(FindSubjectData.Results[i].FFillDept))
            //    {
            //        for(var j = i+ 1;j < count1;j++)
            //        {
            //            if(!string.IsNullOrEmpty(FindSubjectData.Results[j].FFillDept) && FindSubjectData.Results[j].FKmdm.IndexOf(FindSubjectData.Results[i].FKmdm,0) > 0)
            //            {
            //                chileSubject = 1;
            //            }
            //        }
            //        if(chileSubject == 0)
            //        {
            //            FindSubjectData.Results.RemoveAt(i);
            //            i--;
            //            count1--;
            //        }
            //    }
            //}

            return DataConverterHelper.EntityListToJson(FindSubjectData.Data, FindSubjectData.Data.Count);
        }


        /// <summary>
        /// 查找该填报部门下的子科目项目明细项目
        /// </summary>
        /// <returns></returns>
        public string FindSubjectDtlData()
        {
            var FDwdm = System.Web.HttpContext.Current.Request.Params["FDwdm"];
            var FKmlb = System.Web.HttpContext.Current.Request.Params["FKmlb"];
            var Dept = System.Web.HttpContext.Current.Request.Params["Dept"];
            var FType = System.Web.HttpContext.Current.Request.Params["FType"];
            var FYear = System.Web.HttpContext.Current.Request.Params["FYear"];

            //查找该填报部门下已拥有未上报的填报单据
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FDeclarationDept", Dept))
                    .Add(ORMRestrictions<string>.Eq("FProjAttr", FKmlb));
            //.Add(ORMRestrictions<string>.Eq("FApproveStatus", "1"));//未作废数据
            if (!string.IsNullOrEmpty(FYear))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", FYear));
            }
            if (FType == "tz")
            {
                /*Dictionary<string, object> dicWhere1 = new Dictionary<string, object>();
                Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
                //年中调整取年初已审批的单据
                new CreateCriteria(dicWhere1).Add(ORMRestrictions<string>.Eq("FApproveStatus", "3"))
                    .Add(ORMRestrictions<string>.Eq("FType", "c"));
                new CreateCriteria(dicWhere2)
                    .Add(ORMRestrictions<string>.Eq("FType", "tz"));
                new CreateCriteria(dicWhere).Add(ORMRestrictions.Or(dicWhere1, dicWhere2));*/
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FApproveStatus", "3"))
                    .Add(ORMRestrictions<List<string>>.In("FType", new List<string>() { "c","tz" }));
            }
            else
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FApproveStatus", "1"))//未作废数据
                    .Add(ORMRestrictions<string>.Eq("FType", FType));
            }
            if (FKmlb == "0")
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.NotEq("FProjName", "各部门收入预算汇总数据"));
            }
            else if (FKmlb == "1")
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.NotEq("FProjName", "各部门基本支出汇总数据"));//现在列表只显示各部门自己单独数据
            }

            IList<SubjectMstBudgetDtlModel> FindSubjectDtlData = new List<SubjectMstBudgetDtlModel>();
            var findSubject = GHSubjectService.Find(dicWhere);
            for(var i = 0;i < findSubject.Data.Count; i++)
            {
                var id = findSubject.Data[i].PhId;
                var dtlData = GHSubjectService.FindSubjectMstBudgetDtlByForeignKey(id);
                foreach(var item in dtlData.Data)
                {
                    item.PhId = 0;
                    item.Mstphid = 0;
                    FindSubjectDtlData.Add(item);
                }
            }


            return DataConverterHelper.EntityListToJson(FindSubjectDtlData, FindSubjectDtlData.Count);
        }
     

        /// <summary>
        /// 按部门汇总
        /// </summary>
        /// <returns></returns>
        public string SaveAllBudget()
        {
            var FDwdm = System.Web.HttpContext.Current.Request.Params["FDwdm"];
            var FKmlb = System.Web.HttpContext.Current.Request.Params["FKmlb"];
            var Dept = System.Web.HttpContext.Current.Request.Params["Dept"];
            var saveAll = System.Web.HttpContext.Current.Request.Params["ALL"];
            var FYear= System.Web.HttpContext.Current.Request.Params["FYear"];

            Dictionary<string, object> dicWhere1 = new Dictionary<string, object>();

            //如再次执行各部门汇总操作，则覆盖这条新任务单据的内容并更新
            if (FKmlb == "1")
            {
                new CreateCriteria(dicWhere1).Add(ORMRestrictions<string>.Eq("FProjName", "各部门基本支出汇总数据"))
                    .Add(ORMRestrictions<string>.Eq("FYear", FYear));
                var findData = GHSubjectService.Find(dicWhere1);
                if(findData.Data.Count > 0)
                {
                    GHSubjectService.Delete<System.Int64>(findData.Data[0].PhId);
                }
            }
            else
            {
                new CreateCriteria(dicWhere1).Add(ORMRestrictions<string>.Eq("FProjName", "各部门收入预算汇总数据"))
                    .Add(ORMRestrictions<string>.Eq("FYear", FYear));
                var findData = GHSubjectService.Find(dicWhere1);
                if (findData.Data.Count > 0)
                {
                    GHSubjectService.Delete<System.Int64>(findData.Data[0].PhId);
                }
                
            }


            var FindBaseProjectData = QtBaseProjectService.FindSubjectData(FDwdm, FKmlb,"", FYear);

            var ghsubjectforminfo = new GHSubjectModel();
            
            var subjectmstgridData = new List<SubjectMstModel>();
            var subjectmstbudgetdtlgrid = new SubjectMstBudgetDtlModel();
            var subjectmstbudgetdtlgridinfo = new List<SubjectMstBudgetDtlModel>();




            List<QtBaseProjectModel> BaseProjectData = new List<QtBaseProjectModel>();
            var count = FindBaseProjectData.TotalItems;
            decimal AmountBudget = 0;
            decimal EditAmountBudget = 0;
            decimal AfterEditAmountBudget = 0;
            //先去除其他填报部门的子科目
            for (var i = 0; i < count; i++)
            {
                var subjectmstgrid = new SubjectMstModel();
                //填报部门不为空，则为子项目数据
                if (!string.IsNullOrEmpty(FindBaseProjectData.Results[i].FFillDept) && !string.IsNullOrEmpty(FindBaseProjectData.Results[i].FProjCode))
                {

                    Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FProjCode", FindBaseProjectData.Results[i].FProjCode))
                        .Add(ORMRestrictions<string>.Eq("FBudgetset", "0"))
                         .Add(ORMRestrictions<string>.Eq("FYear", FYear));

                    var baseData = GHSubjectService.FindSubjectMstBudgetDtl(dicWhere);
                    
                    AmountBudget = 0;
                    EditAmountBudget = 0;
                    AfterEditAmountBudget = 0;
                    if (baseData.Data.Count > 0)
                    {

                        foreach (var item in baseData.Data)
                        {
                            ////明细科目所在单据没审批完成的，则不进行添加
                            //var findedresultghsubject = GHSubjectService.Find(item.PhId);
                            //if(findedresultghsubject.Data.FApproveStatus == "1" || findedresultghsubject.Data.FApproveStatus == "2")
                            //{
                            //    continue;
                            //}
                            //单据不是当前年度的
                            var findedresultghsubject = GHSubjectService.Find(item.Mstphid);
                            
                            if(saveAll == "0") //是否汇总未审批数据 0 是，1 否
                            {
                                //汇总未审批数据时不汇总调整前单据  不取作废，调整前，调整中
                                if (findedresultghsubject.Data.FApproveStatus == "5" || findedresultghsubject.Data.FApproveStatus == "6" || findedresultghsubject.Data.FApproveStatus == "7")
                                {
                                    continue;
                                }
                                if (findedresultghsubject.Data == null || findedresultghsubject.Data.FYear != FYear || findedresultghsubject.Data.FApproveStatus == "1" || findedresultghsubject.Data.FApproveStatus == "5")
                                {
                                    continue;
                                }
                            }
                            else if(saveAll == "1")
                            {
                                //不汇总未审批数据时 审批完和调整中单据
                                if (findedresultghsubject.Data.FApproveStatus != "3" && findedresultghsubject.Data.FApproveStatus != "6" )
                                {
                                    continue;
                                }
                                if (findedresultghsubject.Data == null || findedresultghsubject.Data.FYear != FYear || findedresultghsubject.Data.FApproveStatus != "3" )
                                {
                                    continue;
                                }
                            }
                            

                            AmountBudget += item.FAmount;
                            EditAmountBudget += item.FAmountEdit;
                            AfterEditAmountBudget += item.FAmountAfterEdit;
                            item.FBudgetset = "1";
                            item.PersistentState = PersistentState.Added;
                            subjectmstbudgetdtlgridinfo.Add(item);
                        }
                        subjectmstgrid.FProjAmount = AmountBudget;
                        subjectmstgrid.FAmountEdit = EditAmountBudget;
                        subjectmstgrid.FAmountAfterEdit = AfterEditAmountBudget;
                    }
                   
                    // BaseProjectData.Add(FindBaseProjectData.Results[i]);
                }

                subjectmstgrid.FKMLB = FindBaseProjectData.Results[i].FKMLB;
                subjectmstgrid.FSubjectCode = FindBaseProjectData.Results[i].FKmdm;
                subjectmstgrid.FSubjectName = FindBaseProjectData.Results[i].Fkmmc;
                subjectmstgrid.FProjName = FindBaseProjectData.Results[i].FProjName;
                subjectmstgrid.FFillDept = FindBaseProjectData.Results[i].FFillDept;
                subjectmstgrid.FProjCode = FindBaseProjectData.Results[i].FProjCode;
                subjectmstgrid.PersistentState = PersistentState.Added;
                subjectmstgridData.Add(subjectmstgrid);
            }
            //同级,父级汇总
            for(var i = 0; i < subjectmstgridData.Count; i++)
            {
                if(string.IsNullOrEmpty(subjectmstgridData[i].FProjCode)) //如果没项目代码，则为科目行
                {
                    var kmdm = subjectmstgridData[i].FSubjectCode;
                    decimal admountSum = 0;
                    decimal admountSum2 = 0;
                    decimal admountSum3 = 0;
                    for (var j = 0;j < subjectmstgridData.Count; j ++)
                    {
                        if(subjectmstgridData[j].FSubjectCode.IndexOf(kmdm,0) == 0)  //科目包含原有科目，则为同级会下级，则汇总
                        {
                            admountSum += subjectmstgridData[j].FProjAmount;
                            admountSum2 += subjectmstgridData[j].FAmountEdit;
                            admountSum3 += subjectmstgridData[j].FAmountAfterEdit;
                        }
                    }
                    subjectmstgridData[i].FProjAmount = admountSum;
                    subjectmstgridData[i].FAmountEdit = admountSum2;
                    subjectmstgridData[i].FAmountAfterEdit = admountSum3;
                }
            }
            


            ghsubjectforminfo.FDeclarationUnit = FDwdm;
            ghsubjectforminfo.FDeclarationDept = Dept;
            ghsubjectforminfo.FDateofDeclaration = DateTime.Today;
            ghsubjectforminfo.FDeclarer = base.UserName;
            ghsubjectforminfo.FBudgetDept = "省总";
            ghsubjectforminfo.FApproveStatus = "1";
            ghsubjectforminfo.FYear = FYear;
            ghsubjectforminfo.FProjAttr = FKmlb;
            if (FKmlb == "1")
            {
                ghsubjectforminfo.FProjName = "各部门基本支出汇总数据";
            }
            else
            {
                ghsubjectforminfo.FProjName = "各部门收入预算汇总数据";
            }
            
            ghsubjectforminfo.PersistentState = PersistentState.Added;


            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            try
            {
                savedresult = GHSubjectService.SaveGHSubject(ghsubjectforminfo, subjectmstgridData, subjectmstbudgetdtlgridinfo);
            }
            catch (Exception ex)
            {
                savedresult.Status = ResponseStatus.Error;
                savedresult.Msg = ex.Message.ToString();
            }
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 查找部门是否全部审批
        /// </summary>
        /// <returns></returns>
        public string FindAllApprove()
        {
            var FDwdm = System.Web.HttpContext.Current.Request.Params["FDwdm"];
            var FKmlb = System.Web.HttpContext.Current.Request.Params["FKmlb"];
            var FYear= System.Web.HttpContext.Current.Request.Params["FYear"];
            var FindBaseProjectData = QtBaseProjectService.FindSubjectData(FDwdm, FKmlb,"", FYear);
           
            string[] dept = new string[FindBaseProjectData.TotalItems];
            var s = 0;
            for(var i = 0; i < FindBaseProjectData.TotalItems; i++)
            {
                if(!string.IsNullOrEmpty(FindBaseProjectData.Results[i].FFillDept))
                {
                    var findDept = 0;
                   for(var j =0; j < dept.Length; j++)
                    {
                        if(FindBaseProjectData.Results[i].FFillDept == dept[j])
                        {
                            findDept = 1;
                            break;
                        }
                    }
                   if(findDept == 0)
                    {
                        dept[s] = FindBaseProjectData.Results[i].FFillDept; //先找出所有的填报部门
                        s++;
                    }
                }
            }
            var allApprove = 0;
            for(var i = 0; i < s ; i++)
            {
                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                if(FKmlb == "1")
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FDeclarationDept", dept[i]))
                    .Add(ORMRestrictions<string>.Eq("FYear", FYear))
                    .Add(ORMRestrictions<string>.Eq("FProjAttr", FKmlb))
                    .Add(ORMRestrictions<string>.NotEq("FProjName", "各部门基本支出汇总数据"));
                }
                else
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FDeclarationDept", dept[i]))
                    .Add(ORMRestrictions<string>.Eq("FYear", FYear))
                    .Add(ORMRestrictions<string>.Eq("FProjAttr", FKmlb))
                    .Add(ORMRestrictions<string>.NotEq("FProjName", "各部门收入预算汇总数据"));
                }

                var findSubject = GHSubjectService.Find(dicWhere);

                var approveCount = 0;
                for (var j = 0; j < findSubject.Data.Count; j++)
                {
                    approveCount = 0;
                    if (findSubject.Data[j].FApproveStatus == "1" || findSubject.Data[j].FApproveStatus == "2")
                    {
                        approveCount++;
                    }

                }
                //未找到数据，或有找到数据，并且未审批数据跟找到数据条数一样，则全部都没审批完，则提示
                //if(findSubject.Data.Count == 0 || approveCount == findSubject.Data.Count )
                if (approveCount>0)
                {
                    allApprove = 1;
                    break;
                }

                //if(findSubject.Data.Count > 0)
                //{
                //    if(findSubject.Data[0].FApproveStatus == "1" || findSubject.Data[0].FApproveStatus == "2")
                //    {
                //        allApprove = 1;
                //        break;
                //    }
                //}

            }

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            savedresult.Status = ResponseStatus.Success;
            savedresult.Msg = allApprove;

            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 查找有没有新增的子科目
        /// </summary>
        /// <returns></returns>
        public string FindNewAddSubject()
        {

            var FDwdm = System.Web.HttpContext.Current.Request.Params["FDwdm"];
            var FKmlb = System.Web.HttpContext.Current.Request.Params["FKmlb"];
            var Dept = System.Web.HttpContext.Current.Request.Params["Dept"];
            var FType= System.Web.HttpContext.Current.Request.Params["FType"];
            var FYear = System.Web.HttpContext.Current.Request.Params["FYear"];
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            savedresult.Status = ResponseStatus.Success;
            savedresult.Msg = "0";

            var OldSubject = FindOldSubject(Dept, FKmlb, FYear);
            var QtBaseSubject = FindQtBaseSubject( FDwdm,  FKmlb,  Dept, FType, FYear);

            for(var i = 0;i< QtBaseSubject.Count;i++)
            {
                var projectCode = QtBaseSubject[i].FProjCode;
                var index = OldSubject.FindIndex(s => s.FProjCode.Equals(projectCode));
                if(index == -1)  //如果该部门要填报子科目项目在已填报数据中未找到，则返回1，表示有新增子科目项目
                {
                    savedresult.Msg = "1";
                    break;
                }
            }
            

            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 查找该填报部门下已填报的子科目项目
        /// </summary>
        /// <param name="Dept"></param>
        /// <param name="FKmlb"></param>
        /// <param name="FYear"></param>
        /// <returns></returns>
        public List<SubjectMstModel> FindOldSubject(string Dept,string FKmlb,string FYear)
        {
            List<SubjectMstModel> oldSubject = new List<SubjectMstModel>();
            //查找该填报部门下已拥有的填报单据
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FDeclarationDept", Dept))
                    .Add(ORMRestrictions<string>.Eq("FYear", FYear))
                    .Add(ORMRestrictions<string>.Eq("FProjAttr", FKmlb))
                     .Add(ORMRestrictions<string>.NotEq("FApproveStatus", "5"));//未作废数据

            if (FKmlb == "0")
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.NotEq("FProjName", "各部门收入预算汇总数据"));
            }
            else if (FKmlb == "1")
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.NotEq("FProjName", "各部门基本支出汇总数据"));//现在列表只显示各部门自己单独数据
            }
            var findSubject = GHSubjectService.Find(dicWhere);
            for (var i = 0; i < findSubject.Data.Count; i++)
            {
                var id = findSubject.Data[i].PhId;
                var findedresultsubjectmst = GHSubjectService.FindSubjectMstByForeignKey(id);
                foreach (var item in findedresultsubjectmst.Data)
                {
                    //项目编码不为空则是子科目项目
                    if (!string.IsNullOrEmpty(item.FProjCode) || !string.IsNullOrEmpty(item.FProjName))
                    {
                        oldSubject.Add(item);
                    }
                }
            }

            return oldSubject;

        }

        /// <summary>
        /// 该部门要填报的子科目项目
        /// </summary>
        /// <param name="FDwdm"></param>
        /// <param name="FKmlb"></param>
        /// <param name="Dept"></param>
        /// <param name="FType"></param>
        /// <param name="FYear"></param>
        /// <returns></returns>
        public List<QtBaseProjectModel> FindQtBaseSubject(string FDwdm,string FKmlb,string Dept,string FType,string FYear)
        {
            List<QtBaseProjectModel> oldQtBaseSubject = new List<QtBaseProjectModel>();
            var FindBaseProjectData = QtBaseProjectService.FindSubjectData(FDwdm, FKmlb, FType, FYear);
            var count = FindBaseProjectData.TotalItems;
            for (var i = 0; i < count; i++)
            {
                //选取该填报部门要填报的子科目项目
                if (FindBaseProjectData.Results[i].FFillDept == Dept && !string.IsNullOrEmpty(FindBaseProjectData.Results[i].FProjCode))
                {
                    oldQtBaseSubject.Add(FindBaseProjectData.Results[i]);
                }
               
            }
            return oldQtBaseSubject;

        }

        /// <summary>
        /// 判断汇总数据是否审批
        /// </summary>
        /// <returns></returns>
        public string IfSP()
        {
            var FKmlb = System.Web.HttpContext.Current.Request.Params["FKmlb"];
            var FYear = System.Web.HttpContext.Current.Request.Params["FYear"];
            CommonResult result = new CommonResult();
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", FYear));
            if (FKmlb=="1")
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FProjName", "各部门基本支出汇总数据"));
            }
            else
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FProjName", "各部门收入预算汇总数据"));
            }
            var findData = GHSubjectService.Find(dicWhere);
            if (findData.Data.Count > 0)
            {
                if (findData.Data[0].FApproveStatus != "1")
                {
                    result.Status = ResponseStatus.Error;
                    result.Msg = "汇总数据已审批";
                }
            }
            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 同步数据到老G6H数据库
        /// </summary>
        /// <returns></returns>
        public string insertData()
        {
            var result = GHSubjectService.AddData();
            if (result == "")
            {
                return "同步成功";
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 纳入预算同步数据到老G6H数据库
        /// </summary>
        /// <returns></returns>
        public string AddDataSP()
        {
            var result = GHSubjectService.AddDataSP();
            if (result == "")
            {
                return "同步成功";
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 判断是否存在未审批完的调整单据
        /// </summary>
        /// <returns></returns>
        public string JudgeTZIfApproval()
        {
            var dept = System.Web.HttpContext.Current.Request.Params["dept"];
            var FKmlb = System.Web.HttpContext.Current.Request.Params["FKmlb"];
            var FYear = System.Web.HttpContext.Current.Request.Params["FYear"];
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(FYear))
            {
                new CreateCriteria(dicWhere)
                   .Add(ORMRestrictions<string>.Eq("FYear", FYear));
            }
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("FDeclarationDept", dept))
                .Add(ORMRestrictions<string>.Eq("FProjAttr", FKmlb))
                .Add(ORMRestrictions<List<string>>.In("FApproveStatus", new List<string>() { "1","2"}))
                .Add(ORMRestrictions<string>.Eq("FType", "tz"));
            var data = GHSubjectService.Find(dicWhere).Data;
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            savedresult.Status = ResponseStatus.Success;
            if (data.Count > 0)
            {
                savedresult.Msg = "1";
            }
            else
            {
                savedresult.Msg = "0";
            }
            
            return DataConverterHelper.SerializeObject(savedresult);
        }

    }
}

