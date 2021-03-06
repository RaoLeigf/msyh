#region Summary
/**************************************************************************************
    * 类 名 称：        SubjectMstBudgetDtlFacade
    * 命名空间：        GYS3.YS.Facade
    * 文 件 名：        SubjectMstBudgetDtlFacade.cs
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
using SUP.Common.DataAccess;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Facade;

using GYS3.YS.Facade.Interface;
using GYS3.YS.Rule.Interface;
using GYS3.YS.Model.Domain;

namespace GYS3.YS.Facade
{
    /// <summary>
    /// SubjectMstBudgetDtl业务组装处理类
    /// </summary>
    public partial class SubjectMstBudgetDtlFacade : EntFacadeBase<SubjectMstBudgetDtlModel>, ISubjectMstBudgetDtlFacade
    {
        #region 类变量及属性
        /// <summary>
        /// SubjectMstBudgetDtl业务逻辑处理对象
        /// </summary>
        ISubjectMstBudgetDtlRule SubjectMstBudgetDtlRule
        {
            get
            {
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as ISubjectMstBudgetDtlRule;
            }
        }
        /// <summary>
        /// SubjectMst业务逻辑处理对象
        /// </summary>
        ISubjectMstRule SubjectMstRule { get; set; }
        /// <summary>
        /// SubjectMstBudgetDtl业务逻辑处理对象
        /// </summary>
        //ISubjectMstBudgetDtlRule SubjectMstBudgetDtlRule { get; set; }
        #endregion

        #region 重载方法
        /// <summary>
        /// 通过外关联的单主键值，获取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns>实体</returns>
        public override FindedResults<SubjectMstBudgetDtlModel> FindByForeignKey<TValType>(TValType id, params string[] sorts)
        {
            FindedResults<SubjectMstBudgetDtlModel> findedResults = base.FindByForeignKey(id, sorts);

            #region 明细Grid代码转名称
            RichHelpDac helpdac = new RichHelpDac();
            //helpdac.CodeToName<SubjectMstBudgetDtlModel>(findedResults.Data, "属性名", "注册的帮助标识"
            //helpdac.CodeToName<SubjectMstBudgetDtlModel>(findedResults.Data, "Code属性名", "Name属性名", "注册的帮助标识", "");
            helpdac.CodeToName<SubjectMstBudgetDtlModel>(findedResults.Data, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist", "");
            helpdac.CodeToName<SubjectMstBudgetDtlModel>(findedResults.Data, "FExpensesChannel", "FExpensesChannel_EXName", "GHExpensesChannel", "");
            helpdac.CodeToName<SubjectMstBudgetDtlModel>(findedResults.Data, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
            #endregion

            return findedResults;
        }

        /// <summary>
        /// 通过id，删除数据
        /// </summary>
        /// <param name="id">单主键id值</param>
        public override DeletedResult Delete<TValType>(TValType id)
        {
            SubjectMstRule.RuleHelper.DeleteByForeignKey(id);
            SubjectMstBudgetDtlRule.RuleHelper.DeleteByForeignKey(id);
            return base.Delete(id);
        }

        /// <summary>
        /// 通过ids，删除数据
        /// </summary>
        /// <param name="ids">单主键id集合</param>
        public override DeletedResult Delete<TValType>(IList<TValType> ids)
        {
            SubjectMstRule.RuleHelper.DeleteByForeignKey(ids);
            SubjectMstBudgetDtlRule.RuleHelper.DeleteByForeignKey(ids);
            return base.Delete(ids);
        }
        #endregion

        #region 实现 ISubjectMstBudgetDtlFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<SubjectMstBudgetDtlModel> ExampleMethod<SubjectMstBudgetDtlModel>(string param)
        //{
        //    //编写代码
        //}

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="subjectMstBudgetDtlEntity"></param>
        /// <param name="subjectMstEntities"></param>
        /// <param name="subjectMstBudgetDtlEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveSubjectMstBudgetDtl(SubjectMstBudgetDtlModel subjectMstBudgetDtlEntity, List<SubjectMstModel> subjectMstEntities, List<SubjectMstBudgetDtlModel> subjectMstBudgetDtlEntities)
        {
            SavedResult<Int64> savedResult = base.Save<Int64>(subjectMstBudgetDtlEntity);
            if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
            {
                if (subjectMstEntities.Count > 0)
                {
                    SubjectMstRule.Save(subjectMstEntities, savedResult.KeyCodes[0]);
                }
                if (subjectMstBudgetDtlEntities.Count > 0)
                {
                    SubjectMstBudgetDtlRule.Save(subjectMstBudgetDtlEntities, savedResult.KeyCodes[0]);
                }
            }

            return savedResult;
        }

        #endregion
    }
}

