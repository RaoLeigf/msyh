using Enterprise3.Common.Base.Criterion;
using Enterprise3.WebApi.ApiControllerBase;
using Enterprise3.WebApi.GYS3.YS.Model.Request;
using GQT3.QT.Service.Interface;
using GYS3.YS.Model.Domain;
using GYS3.YS.Service.Interface;
using SUP.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Enterprise3.WebApi.GYS3.YS.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [MethodExceptionFilter]
    public class GHSubjectController : ApiBase
    {
        IGHSubjectService GHSubjectService { get; set; }
        ICorrespondenceSettingsService CorrespondenceSettingsService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public GHSubjectController()
        {
            GHSubjectService = base.GetObject<IGHSubjectService>("GYS3.YS.Service.GHSubject");
            CorrespondenceSettingsService = base.GetObject<ICorrespondenceSettingsService>("GQT3.QT.Service.CorrespondenceSettings");
        }

        #region (1-基本支出/2-收入预算)申报接口

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <param name="param"> FKmlb=1 基本支出申报 FKmlb=0 收入预算申报</param>
        /// <returns></returns>
        [HttpGet]
        public string GetGHSubjectList([FromUri]GHSubjectRequestModel param)
        {
            if (string.IsNullOrEmpty(param.UserId))
            {
                return DCHelper.ErrorMessage("用户编码不能为空！");
            }
            if (string.IsNullOrEmpty(param.Fkmlb))
            {
                return DCHelper.ErrorMessage("申报种类不能为空！");
            }
            try
            {
                Dictionary<string, object> dicWhere = new Dictionary<string, object>();//查询条件转Dictionary
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FProjAttr", param.Fkmlb));
                
                //增加搜索条件
                if (!string.IsNullOrEmpty(param.FApproveStatus) && !"0".Equals(param.FApproveStatus))
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<string>.Eq("FApproveStatus", param.FApproveStatus));
                }
                if (!string.IsNullOrEmpty(param.Year))
                {
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", param.Year));
                }

                //增加根据操作员对应预算部门的过滤
                var dicWhereDept = new Dictionary<string, object>();
                new CreateCriteria(dicWhereDept)
                    .Add(ORMRestrictions<string>.Eq("Dwdm", param.UserId)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
                var deptList = CorrespondenceSettingsService.Find(dicWhereDept);
                List<string> deptL = new List<string>();
                for (var i = 0; i < deptList.Data.Count; i++)
                {
                    deptL.Add(deptList.Data[i].Dydm);
                }
                new CreateCriteria(dicWhere).Add(ORMRestrictions<IList<String>>.In("FDeclarationDept", deptL))
                .Add(ORMRestrictions<string>.Eq("FYear", DateTime.Today.Year.ToString()));

                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FType", "c"));

                var result = GHSubjectService.LoadWithPage(param.PageIndex, param.PageSize, dicWhere, new string[] { "NgInsertDt Desc", "NgUpdateDt Desc" });
                //return DataConverterHelper.EntityListToJson<GHSubjectModel>(result.Results, (Int32)result.TotalItems);
                return DCHelper.ModelListToJson<GHSubjectModel>(result.Results, (Int32)result.TotalItems);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetGHSubjectInfo([FromUri]GHSubjectRequestModel param)
        {
            if (param.PhId == 0)
            {
                return DCHelper.ErrorMessage("主键不能为空！");
            }
            try
            {
               //主表主键
                string tabtype = param.tabtype; //Tab类型
                switch (tabtype)
                {
                    case "ghsubject":
                        var findedresultghsubject = GHSubjectService.Find(param.PhId);
                        return DataConverterHelper.SerializeObject(findedresultghsubject);
                    case "subjectmst":
                        var findedresultsubjectmst = GHSubjectService.FindSubjectMstByForeignKey(param.PhId);
                        foreach (var item in findedresultsubjectmst.Data)
                        {
                            if (!string.IsNullOrEmpty(item.FProjCode) || !string.IsNullOrEmpty(item.FProjName))
                            {
                                item.FSubjectCode = "";
                                item.FSubjectName = "";
                            }
                        }
                        return DCHelper.ModelListToJson<SubjectMstModel>(findedresultsubjectmst.Data, findedresultsubjectmst.Data.Count);

                    case "subjectmstbudgetdtl":
                        var findedresultsubjectmstbudgetdtl = GHSubjectService.FindSubjectMstBudgetDtlByForeignKey(param.PhId);
                        return DCHelper.ModelListToJson<SubjectMstBudgetDtlModel>(findedresultsubjectmstbudgetdtl.Data, findedresultsubjectmstbudgetdtl.Data.Count);
                    default:
                        Common.Model.Results.FindedResult findedresultother = new Common.Model.Results.FindedResult();
                        return DataConverterHelper.SerializeObject(findedresultother);
                }
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }

        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpPost]
        public string PostDeleteGHSubject([FromBody]GHSubjectRequestModel param)
        {
            if (param.PhId == 0)
            {
                return DCHelper.ErrorMessage("主键不能为空！");
            }
            try
            {
                long id = param.PhId;  //主表主键
                var data = GHSubjectService.Find(id).Data;
                if (data.FType == "tz" && data.FApproveStatus != "5")//删除年中调整 未作废单据 恢复上一个年中调整单据
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
                            foreach (var a in gHSubjects2)
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
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 取消送审
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string PostUnChecked([FromBody]GHSubjectRequestModel param)
        {
            if (param.PhId == 0)
            {
                return DCHelper.ErrorMessage("主键不能为空！");
            }
            try
            {
                long id = param.PhId;  //主表主键

                var Findresult = GHSubjectService.Find<System.Int64>(id);
                Common.Model.Results.SavedResult<Int64> savedresult = new Common.Model.Results.SavedResult<Int64>();


                Findresult.Data.FApproveStatus = "1";
                Findresult.Data.FApprover = 0;
                Findresult.Data.FApproveDate = new Nullable<DateTime>();
                Findresult.Data.PersistentState = PersistentState.Modified;
                savedresult = GHSubjectService.Save<Int64>(Findresult.Data, "");

                return DataConverterHelper.SerializeObject(savedresult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
           
        }


        #endregion


    }
}
