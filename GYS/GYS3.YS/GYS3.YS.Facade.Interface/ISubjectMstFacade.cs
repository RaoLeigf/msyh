#region Summary
/**************************************************************************************
    * 类 名 称：        ISubjectMstFacade
    * 命名空间：        GYS3.YS.Facade.Interface
    * 文 件 名：        ISubjectMstFacade.cs
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
    /// SubjectMst业务组装层接口
    /// </summary>
    public partial interface ISubjectMstFacade : IEntFacadeBase<SubjectMstModel>
    {
        #region ISubjectMstFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<SubjectMstModel> ExampleMethod<SubjectMstModel>(string param)

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="subjectMstEntity"></param>
        /// <param name="subjectMstEntities"></param>
        /// <param name="subjectMstBudgetDtlEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveSubjectMst(SubjectMstModel subjectMstEntity, List<SubjectMstModel> subjectMstEntities, List<SubjectMstBudgetDtlModel> subjectMstBudgetDtlEntities);

        #endregion
    }
}
