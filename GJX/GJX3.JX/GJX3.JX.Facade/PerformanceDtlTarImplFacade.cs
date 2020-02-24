#region Summary
/**************************************************************************************
    * 类 名 称：        PerformanceDtlTarImplFacade
    * 命名空间：        GJX3.JX.Facade
    * 文 件 名：        PerformanceDtlTarImplFacade.cs
    * 创建时间：        2018/10/16 
    * 作    者：        夏华军    
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

using GJX3.JX.Facade.Interface;
using GJX3.JX.Rule.Interface;
using GJX3.JX.Model.Domain;

namespace GJX3.JX.Facade
{
	/// <summary>
	/// PerformanceDtlTarImpl业务组装处理类
	/// </summary>
    public partial class PerformanceDtlTarImplFacade : EntFacadeBase<PerformanceDtlTarImplModel>, IPerformanceDtlTarImplFacade
    {
		#region 类变量及属性
		/// <summary>
        /// PerformanceDtlTarImpl业务逻辑处理对象
        /// </summary>
		IPerformanceDtlTarImplRule PerformanceDtlTarImplRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IPerformanceDtlTarImplRule;
            }
        }
		/// <summary>
        /// PerformanceDtlEval业务逻辑处理对象
        /// </summary>
		IPerformanceDtlEvalRule PerformanceDtlEvalRule { get; set; }
		/// <summary>
        /// PerformanceDtlTextCont业务逻辑处理对象
        /// </summary>
		IPerformanceDtlTextContRule PerformanceDtlTextContRule { get; set; }
		/// <summary>
        /// PerformanceDtlBuDtl业务逻辑处理对象
        /// </summary>
		IPerformanceDtlBuDtlRule PerformanceDtlBuDtlRule { get; set; }
		#endregion

		#region 重载方法
		/// <summary>
        /// 通过外关联的单主键值，获取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns>实体</returns>
        public override FindedResults<PerformanceDtlTarImplModel> FindByForeignKey<TValType>(TValType id, params string[] sorts)
        {
            FindedResults<PerformanceDtlTarImplModel> findedResults = base.FindByForeignKey(id, sorts);

            #region 明细Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<PerformanceDtlTarImplModel>(findedResults.Data, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<PerformanceDtlTarImplModel>(findedResults.Data, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return findedResults;
        }

        /// <summary>
        /// 通过id，删除数据
        /// </summary>
        /// <param name="id">单主键id值</param>
        public override DeletedResult Delete<TValType>(TValType id)
        {
			PerformanceDtlEvalRule.RuleHelper.DeleteByForeignKey(id);
			PerformanceDtlTextContRule.RuleHelper.DeleteByForeignKey(id);
			PerformanceDtlTarImplRule.RuleHelper.DeleteByForeignKey(id);
			PerformanceDtlBuDtlRule.RuleHelper.DeleteByForeignKey(id);
			return base.Delete(id);
        }

		/// <summary>
        /// 通过ids，删除数据
        /// </summary>
        /// <param name="ids">单主键id集合</param>
        public override DeletedResult Delete<TValType>(IList<TValType> ids)
        {
			PerformanceDtlEvalRule.RuleHelper.DeleteByForeignKey(ids);
			PerformanceDtlTextContRule.RuleHelper.DeleteByForeignKey(ids);
			PerformanceDtlTarImplRule.RuleHelper.DeleteByForeignKey(ids);
			PerformanceDtlBuDtlRule.RuleHelper.DeleteByForeignKey(ids);
			return base.Delete(ids);
        }
        #endregion

		#region 实现 IPerformanceDtlTarImplFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<PerformanceDtlTarImplModel> ExampleMethod<PerformanceDtlTarImplModel>(string param)
        //{
        //    //编写代码
        //}

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="performanceDtlTarImplEntity"></param>
		/// <param name="performanceDtlEvalEntities"></param>
		/// <param name="performanceDtlTextContEntities"></param>
		/// <param name="performanceDtlTarImplEntities"></param>
		/// <param name="performanceDtlBuDtlEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SavePerformanceDtlTarImpl(PerformanceDtlTarImplModel performanceDtlTarImplEntity, List<PerformanceDtlEvalModel> performanceDtlEvalEntities, List<PerformanceDtlTextContModel> performanceDtlTextContEntities, List<PerformanceDtlTarImplModel> performanceDtlTarImplEntities, List<PerformanceDtlBuDtlModel> performanceDtlBuDtlEntities)
        {
            SavedResult<Int64> savedResult = base.Save<Int64>(performanceDtlTarImplEntity);
            if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
            {
				if (performanceDtlEvalEntities.Count > 0)
				{
					PerformanceDtlEvalRule.Save(performanceDtlEvalEntities, savedResult.KeyCodes[0]);
				}
				if (performanceDtlTextContEntities.Count > 0)
				{
					PerformanceDtlTextContRule.Save(performanceDtlTextContEntities, savedResult.KeyCodes[0]);
				}
				if (performanceDtlTarImplEntities.Count > 0)
				{
					PerformanceDtlTarImplRule.Save(performanceDtlTarImplEntities, savedResult.KeyCodes[0]);
				}
				if (performanceDtlBuDtlEntities.Count > 0)
				{
					PerformanceDtlBuDtlRule.Save(performanceDtlBuDtlEntities, savedResult.KeyCodes[0]);
				}
            }

			return savedResult;
        }

        #endregion
    }
}

