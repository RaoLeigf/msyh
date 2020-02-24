#region Summary
/**************************************************************************************
    * 类 名 称：        ISubjectMstBudgetDtlRule
    * 命名空间：        GYS3.YS.Rule.Interface
    * 文 件 名：        ISubjectMstBudgetDtlRule.cs
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
using Enterprise3.NHORM.Interface.EntBase;

using GYS3.YS.Model.Domain;

namespace GYS3.YS.Rule.Interface
{
    /// <summary>
    /// SubjectMstBudgetDtl业务逻辑层接口
    /// </summary>
    public partial interface ISubjectMstBudgetDtlRule : IEntRuleBase<SubjectMstBudgetDtlModel>
    {
        #region ISubjectMstBudgetDtlRule 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<SubjectMstBudgetDtlModel> ExampleMethod<SubjectMstBudgetDtlModel>(string param)

        #endregion
    }
}
