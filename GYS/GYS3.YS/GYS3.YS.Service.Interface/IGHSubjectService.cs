#region Summary
/**************************************************************************************
    * 类 名 称：        IGHSubjectService
    * 命名空间：        GYS3.YS.Service.Interface
    * 文 件 名：        IGHSubjectService.cs
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
using System.Text;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GYS3.YS.Model.Domain;

namespace GYS3.YS.Service.Interface
{
    /// <summary>
    /// GHSubject服务组装层接口
    /// </summary>
    public partial interface IGHSubjectService : IEntServiceBase<GHSubjectModel>
    {
        #region IGHSubjectService 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="gHSubjectEntity"></param>
        /// <param name="subjectMstEntities"></param>
        /// <param name="subjectMstBudgetDtlEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveGHSubject(GHSubjectModel gHSubjectEntity, List<SubjectMstModel> subjectMstEntities, List<SubjectMstBudgetDtlModel> subjectMstBudgetDtlEntities);

        /// <summary>
        /// 通过外键值获取SubjectMst明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<SubjectMstModel> FindSubjectMstByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取SubjectMstBudgetDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<SubjectMstBudgetDtlModel> FindSubjectMstBudgetDtlByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 获取SubjectMstBudgetDtl明细数据
        /// </summary>
        /// <returns></returns>
        FindedResults<SubjectMstBudgetDtlModel> FindSubjectMstBudgetDtl();

        /// <summary>
        /// 查找各子项目明细项目
        /// </summary>
        /// <param name="dicWhere"></param>
        /// <returns></returns>
        FindedResults<SubjectMstBudgetDtlModel> FindSubjectMstBudgetDtl(Dictionary<string, object> dicWhere);

        /// <summary>
        /// 同步数据到老G6H数据库
        /// </summary>
        /// <returns></returns>
        string AddData();

        /// <summary>
        /// 纳入预算同步数据到老G6H数据库
        /// </summary>
        /// <returns></returns>
        string AddDataSP();
        
        #endregion
    }
}
