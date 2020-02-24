#region Summary
/**************************************************************************************
    * 类 名 称：        QtBaseProjectService
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        QtBaseProjectService.cs
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
using System.Linq;
using SUP.Common.DataEntity;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Service;

using GQT3.QT.Service.Interface;
using GQT3.QT.Facade.Interface;
using GQT3.QT.Model.Domain;
using Enterprise3.Common.Base.Criterion;
using SUP.Common.Base;
using GYS3.YS.Model.Domain;
using GYS3.YS.Facade.Interface;

namespace GQT3.QT.Service
{
	/// <summary>
	/// QtBaseProject服务组装处理类
	/// </summary>
    public partial class QtBaseProjectService : EntServiceBase<QtBaseProjectModel>, IQtBaseProjectService
    {
		#region 类变量及属性
		/// <summary>
        /// QtBaseProject业务外观处理对象
        /// </summary>
		IQtBaseProjectFacade QtBaseProjectFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IQtBaseProjectFacade;
            }
        }

        private IQtSysCodeSeqFacade QtSysCodeSeqFacade { get; set; }

        private ISubjectMstFacade SubjectMstFacade { get; set; }

        private IBudgetAccountsFacade BudgetAccountsFacade { get; set; }

        private ISubjectMstBudgetDtlFacade SubjectMstBudgetDtlFacade { get; set; }

        private IGHSubjectFacade GHSubjectFacade { get; set; }


        #endregion

        #region 实现 IQtBaseProjectService 业务添加的成员

        /// <summary>
        /// 获取最大项目库编码
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public string CreateOrGetMaxProjCode(string year)
        {
            string projCode = "";
            QtSysCodeSeqModel seqM = null;

            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", year))
                .Add(ORMRestrictions<string>.Eq("FCode", "ProjCode")).Add(ORMRestrictions<string>.Eq("FTname", "z_QtBaseProject"));

            FindedResults<QtSysCodeSeqModel> sysCodeSeqResult = QtSysCodeSeqFacade.Find(dicWhere);
            // FindedResults<QtSysCodeSeqModel> sysCodeSeqResult = QtSysCodeSeqFacade.Find(t => t.FYear == year && t.FCode== "ProjCode" && t.FTname== "xm3_projectmst");

            if (sysCodeSeqResult.Status == ResponseStatus.Success)
            {
                //插入或更新项目代码编码序号                    
                if (sysCodeSeqResult.Data.Count > 0)
                {
                    seqM = sysCodeSeqResult.Data[0];
                    if (string.IsNullOrWhiteSpace(seqM.FSeqNO))
                    {
                        projCode = year + string.Format("{0:D8}", 1);
                    }
                    else
                    {
                        var max = Int64.Parse(seqM.FSeqNO.Substring(4));
                        max = max + 1;
                        projCode = year + string.Format("{0:D8}", max);
                    }

                    seqM.FSeqNO = projCode; //更新代码，访问一次后就加1，后续不还原，一直累加
                    seqM.PersistentState = PersistentState.Modified;
                }
                else
                {
                    //系统编码不存在 
                    projCode = year + string.Format("{0:D8}", 1);

                    seqM = new QtSysCodeSeqModel
                    {
                        FYear = year,
                        FCode = "ProjCode",
                        FName = "单位对应基本项目代码编码序号",
                        FTname = "z_QtBaseProject",
                        FSeqNO = projCode,
                        PersistentState = SUP.Common.Base.PersistentState.Added
                    };
                }
                SavedResult<Int64> saveResult = QtSysCodeSeqFacade.Save<Int64>(seqM);
            }
            else
            {
                projCode = year + string.Format("{0:D8}", 1);
            }

            return projCode;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="qtBaseProjectModel"></param>
        /// <param name="subjectMstModel"></param>
        /// <returns></returns>
        public SavedResult<Int64> Save2(QtBaseProjectModel qtBaseProjectModel, SubjectMstModel subjectMstModel)
        {
            SavedResult<Int64> saveResult = new SavedResult<Int64>();
            try
            {
                var dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere).
                    Add(ORMRestrictions<string>.Eq("FKmdm", qtBaseProjectModel.FKmdm)).
                    Add(ORMRestrictions<string>.Eq("FDwdm", qtBaseProjectModel.FDwdm)).
                    Add(ORMRestrictions<string>.Eq("FProjName", qtBaseProjectModel.FProjName)).
                    Add(ORMRestrictions<string>.Eq("FYear", qtBaseProjectModel.FYear));
                if (base.Find(dicWhere).Data.Count > 0)
                {
                    saveResult.Status = ResponseStatus.Error;
                    saveResult.Msg = "相同科目下不允许重复的子科目项目名称存在！";
                    return saveResult;
                }
                if (string.IsNullOrEmpty(qtBaseProjectModel.FProjCode))
                {
                    //var time = DateTime.Today;
                    //qtBaseProjectModel.FProjCode = QtBaseProjectService.CreateOrGetMaxProjCode(time.Year.ToString());
                    qtBaseProjectModel.FProjCode = CreateOrGetMaxProjCode(qtBaseProjectModel.FYear);
                }
                saveResult = base.Save<Int64>(qtBaseProjectModel,"");
                //saveResult= SubjectMstFacade.Save<Int64>(subjectMstModel);
            }
            catch (Exception ex)
            {
                saveResult.Status = ResponseStatus.Error;
                saveResult.Msg = ex.Message.ToString();
            }
            return saveResult;
        }

        /// <summary>
        /// 判断是否是末级
        /// </summary>
        /// <param name="kmdm"></param>
        /// <returns></returns>
        public Boolean JudgeIfEnd(string kmdm)
        {
            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).
                Add(ORMRestrictions<string>.LLike("KMDM", kmdm));
            if (BudgetAccountsFacade.Find(dicWhere).Data.Count > 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 修改项目名称和填报部门
        /// </summary>
        /// <param name="qtBaseProjectModel"></param>
        /// <returns></returns>
        public SavedResult<Int64> Update2(QtBaseProjectModel qtBaseProjectModel)
        {
            SavedResult<Int64> saveResult=new SavedResult<Int64>();
            try
            {
                QtBaseProjectModel model = base.Find(qtBaseProjectModel.PhId).Data;
                if(model.FProjName!= qtBaseProjectModel.FProjName)
                {
                    var dicWhere = new Dictionary<string, object>();
                    new CreateCriteria(dicWhere).
                        Add(ORMRestrictions<string>.Eq("FKmdm", model.FKmdm)).
                        Add(ORMRestrictions<string>.Eq("FProjName", qtBaseProjectModel.FProjName)).
                        Add(ORMRestrictions<string>.Eq("FYear", model.FYear));
                    if (base.Find(dicWhere).Data.Count > 0)
                    {
                        saveResult.Status = ResponseStatus.Error;
                        saveResult.Msg = "相同科目下不允许重复的子科目项目名称存在！";
                        return saveResult;
                    }
                }
                model.FProjName = qtBaseProjectModel.FProjName;
                model.FFillDept = qtBaseProjectModel.FFillDept;
                model.FFillDept_Name = qtBaseProjectModel.FFillDept_Name;
                model.PersistentState = PersistentState.Modified;
                saveResult = base.Save<Int64>(model,"");
            }
            catch (Exception ex)
            {
                saveResult.Status = ResponseStatus.Error;
                saveResult.Msg = ex.Message.ToString();
            }
            return saveResult;
        }


        /// <summary>
        /// 判断是否有明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean JudgeHaveDtl(long id)
        {
            QtBaseProjectModel qtBaseProjectModel = base.Find(id).Data;
            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).
                Add(ORMRestrictions<string>.Eq("FProjCode", qtBaseProjectModel.FProjCode));
            if (SubjectMstBudgetDtlFacade.Find(dicWhere).Data.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 有明细时判断是否处于审批流程中
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DeletedResult DeleteIfDtl(long id)
        {
            var deletedresult = new DeletedResult();
            try
            {
                QtBaseProjectModel qtBaseProjectModel = base.Find(id).Data;
                var dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere).
                    Add(ORMRestrictions<string>.Eq("FProjCode", qtBaseProjectModel.FProjCode)).
                    Add(ORMRestrictions<string>.Eq("FYear", qtBaseProjectModel.FYear));
                IList<SubjectMstBudgetDtlModel> subjectMstBudgetDtlModels = SubjectMstBudgetDtlFacade.Find(dicWhere).Data;

                for(var i=0;i< subjectMstBudgetDtlModels.Count; i++)
                {
                    GHSubjectModel gHSubjectModel = GHSubjectFacade.Find(subjectMstBudgetDtlModels[i].Mstphid).Data;
                    if (gHSubjectModel.FApproveStatus != "1")
                    {
                        deletedresult.Status = ResponseStatus.Error;
                        deletedresult.Msg = "有数据处于审批流程当中，无法删除！";
                        return deletedresult;
                    }
                }

                deletedresult = base.Delete(id);
            }
            catch (Exception ex)
            {
                deletedresult.Status = ResponseStatus.Error;
                deletedresult.Msg = ex.Message.ToString();
            }
            return deletedresult;
        }

        /// <summary>
        /// 没有明细时判断SubjectMst表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DeletedResult Delete2(long id)
        {
            var deletedresult = new DeletedResult();
            try
            {
                QtBaseProjectModel qtBaseProjectModel = base.Find(id).Data;
                var dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere).
                    Add(ORMRestrictions<string>.Eq("FProjCode", qtBaseProjectModel.FProjCode)).
                    Add(ORMRestrictions<string>.Eq("FYear", qtBaseProjectModel.FYear));
                IList<SubjectMstModel> subjectMstModels = SubjectMstFacade.Find(dicWhere).Data;

                for(var i=0;i< subjectMstModels.Count; i++)
                {
                    GHSubjectModel gHSubjectModel= GHSubjectFacade.Find(subjectMstModels[i].Mstphid).Data;
                    if (gHSubjectModel.FApproveStatus != "1")
                    {
                        deletedresult.Status = ResponseStatus.Error;
                        deletedresult.Msg = "有数据处于审批流程当中，无法删除！";
                        return deletedresult;
                    }
                }
                deletedresult = base.Delete(id);
                
            }
            catch (Exception ex)
            {
                deletedresult.Status = ResponseStatus.Error;
                deletedresult.Msg = ex.Message.ToString();
            }
            return deletedresult;
        }

        /// <summary>
        /// 查找该单位下全部的预算科目
        /// </summary>
        /// <param name="FDwdm"></param>
        /// <param name="FKmlb"></param>
        /// <param name="FType"></param>
        /// <param name="FYear"></param>
        /// <returns></returns>
        public PagedResult<QtBaseProjectModel> FindSubjectData(string FDwdm,string FKmlb,string FType,string FYear)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();


            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("FDwdm", FDwdm)).Add(ORMRestrictions<string>.Eq("FKMLB", FKmlb));
            if (!string.IsNullOrEmpty(FType))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FType", FType));
            }
            //if (!string.IsNullOrEmpty(FYear))
            //{
            //    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", FYear));
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
            var result = base.ServiceHelper.LoadWithPageInfinity("GYS3.YS.FindSUbjectProject", dicWhere,false, new string[] { "FKmdm", "NgInsertDt" });
            //var result = base.LoadWithPageInfinity("GYS3.YS.FindSUbjectProject", dicWhere, false, new string[] { "FKmdm", "FProjCode" });
            return result;
        }

        #endregion
    }
}

