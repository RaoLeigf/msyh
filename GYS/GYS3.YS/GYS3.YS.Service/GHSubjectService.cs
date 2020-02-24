#region Summary
/**************************************************************************************
    * 类 名 称：        GHSubjectService
    * 命名空间：        GYS3.YS.Service
    * 文 件 名：        GHSubjectService.cs
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
using System.Linq;
using SUP.Common.DataEntity;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Service;

using GYS3.YS.Service.Interface;
using GYS3.YS.Facade.Interface;
using GYS3.YS.Model.Domain;

using Enterprise3.Common.Base.Criterion;

namespace GYS3.YS.Service
{
    /// <summary>
    /// GHSubject服务组装处理类
    /// </summary>
    public partial class GHSubjectService : EntServiceBase<GHSubjectModel>, IGHSubjectService
    {
        #region 类变量及属性
        /// <summary>
        /// GHSubject业务外观处理对象
        /// </summary>
        IGHSubjectFacade GHSubjectFacade
        {
            get
            {
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IGHSubjectFacade;
            }
        }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
        private ISubjectMstFacade SubjectMstFacade { get; set; }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
        private ISubjectMstBudgetDtlFacade SubjectMstBudgetDtlFacade { get; set; }
        #endregion

        #region 实现 IGHSubjectService 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="gHSubjectEntity"></param>
        /// <param name="subjectMstEntities"></param>
        /// <param name="subjectMstBudgetDtlEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveGHSubject(GHSubjectModel gHSubjectEntity, List<SubjectMstModel> subjectMstEntities, List<SubjectMstBudgetDtlModel> subjectMstBudgetDtlEntities)
        {
            return GHSubjectFacade.SaveGHSubject(gHSubjectEntity, subjectMstEntities, subjectMstBudgetDtlEntities);
        }

        /// <summary>
        /// 通过外键值获取SubjectMst明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<SubjectMstModel> FindSubjectMstByForeignKey<TValType>(TValType id)
        {
            return SubjectMstFacade.FindByForeignKey(id, new string[] { "FSubjectCode", "PhId" });
        }

        /// <summary>
        /// 通过外键值获取SubjectMstBudgetDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<SubjectMstBudgetDtlModel> FindSubjectMstBudgetDtlByForeignKey<TValType>(TValType id)
        {
            return SubjectMstBudgetDtlFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 获取SubjectMstBudgetDtl明细数据
        /// </summary>
        /// <returns></returns>
        public FindedResults<SubjectMstBudgetDtlModel> FindSubjectMstBudgetDtl()
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<string>.Eq("FBudgetset", "0"));
            return SubjectMstBudgetDtlFacade.Find(dicWhere);
        }

        /// <summary>
        /// 查找各子项目明细项目
        /// </summary>
        /// <param name="dicWhere"></param>
        /// <returns></returns>
        public FindedResults<SubjectMstBudgetDtlModel> FindSubjectMstBudgetDtl(Dictionary<string, object> dicWhere)
        {
            return SubjectMstBudgetDtlFacade.Find(dicWhere);
        }

        /// <summary>
        /// 同步数据到老G6H数据库
        /// </summary>
        /// <returns></returns>
        public string AddData()
        {
            return GHSubjectFacade.AddData();
        }

        /// <summary>
        /// 纳入预算同步数据到老G6H数据库
        /// </summary>
        /// <returns></returns>
        public string AddDataSP()
        {
            return GHSubjectFacade.AddDataSP();
        }
        
        #endregion
    }
}

