#region Summary
/**************************************************************************************
    * 类 名 称：        ISubjectMstBudgetDtlFacade
    * 命名空间：        GYS3.YS.Facade.Interface
    * 文 件 名：        ISubjectMstBudgetDtlFacade.cs
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

namespace GYS3.YS.Facade.Interface
{
    /// <summary>
    /// SubjectMstBudgetDtl业务组装层接口
    /// </summary>
    public partial interface ISubjectMstBudgetDtlFacade : IEntFacadeBase<SubjectMstBudgetDtlModel>
    {
        #region ISubjectMstBudgetDtlFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<SubjectMstBudgetDtlModel> ExampleMethod<SubjectMstBudgetDtlModel>(string param)

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="subjectMstBudgetDtlEntity"></param>
        /// <param name="subjectMstEntities"></param>
        /// <param name="subjectMstBudgetDtlEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveSubjectMstBudgetDtl(SubjectMstBudgetDtlModel subjectMstBudgetDtlEntity, List<SubjectMstModel> subjectMstEntities, List<SubjectMstBudgetDtlModel> subjectMstBudgetDtlEntities);

        #endregion
    }
}
