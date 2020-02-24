#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectDtlPurchaseDtlFacade
    * 命名空间：        GXM3.XM.Facade
    * 文 件 名：        ProjectDtlPurchaseDtlFacade.cs
    * 创建时间：        2018/10/15 
    * 作    者：        李明    
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

using GXM3.XM.Facade.Interface;
using GXM3.XM.Rule.Interface;
using GXM3.XM.Model.Domain;

namespace GXM3.XM.Facade
{
	/// <summary>
	/// ProjectDtlPurchaseDtl业务组装处理类
	/// </summary>
    public partial class ProjectDtlPurchaseDtlFacade : EntFacadeBase<ProjectDtlPurchaseDtlModel>, IProjectDtlPurchaseDtlFacade
    {
		#region 类变量及属性
		/// <summary>
        /// ProjectDtlPurchaseDtl业务逻辑处理对象
        /// </summary>
		IProjectDtlPurchaseDtlRule ProjectDtlPurchaseDtlRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IProjectDtlPurchaseDtlRule;
            }
        }

		/// <summary>
        /// ProjectDtlPurDtl4SOF业务逻辑处理对象
        /// </summary>
		IProjectDtlPurDtl4SOFRule ProjectDtlPurDtl4SOFRule { get; set; }

		#endregion

		#region 重载方法
		/// <summary>
        /// 通过外关联的单主键值，获取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns>实体</returns>
        public override FindedResults<ProjectDtlPurchaseDtlModel> FindByForeignKey<TValType>(TValType id, params string[] sorts)
        {
            FindedResults<ProjectDtlPurchaseDtlModel> findedResults = base.FindByForeignKey(id, sorts);

            #region 明细Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<ProjectDtlPurchaseDtlModel>(findedResults.Data, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<ProjectDtlPurchaseDtlModel>(findedResults.Data, "Code属性名", "Name属性名", "注册的帮助标识", "");
			helpdac.CodeToName<ProjectDtlPurchaseDtlModel>(findedResults.Data, "FCatalogCode", "FCatalogCode_EXName", "GHProcurementCatalog", "");
			helpdac.CodeToName<ProjectDtlPurchaseDtlModel>(findedResults.Data, "FTypeCode", "FTypeCode_EXName", "GHProcurementType", "");
			helpdac.CodeToName<ProjectDtlPurchaseDtlModel>(findedResults.Data, "FProcedureCode", "FProcedureCode_EXName", "GHProcurementProcedures", "");
            #endregion

            return findedResults;
        }

        /// <summary>
        /// 通过id，删除数据
        /// </summary>
        /// <param name="id">单主键id值</param>
        public override DeletedResult Delete<TValType>(TValType id)
        {
			ProjectDtlPurchaseDtlRule.RuleHelper.DeleteByForeignKey(id);
			ProjectDtlPurDtl4SOFRule.RuleHelper.DeleteByForeignKey(id);

			return base.Delete(id);
        }

		/// <summary>
        /// 通过ids，删除数据
        /// </summary>
        /// <param name="ids">单主键id集合</param>
        public override DeletedResult Delete<TValType>(IList<TValType> ids)
        {
			ProjectDtlPurchaseDtlRule.RuleHelper.DeleteByForeignKey(ids);
			ProjectDtlPurDtl4SOFRule.RuleHelper.DeleteByForeignKey(ids);

			return base.Delete(ids);
        }
        #endregion

		#region 实现 IProjectDtlPurchaseDtlFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<ProjectDtlPurchaseDtlModel> ExampleMethod<ProjectDtlPurchaseDtlModel>(string param)
        //{
        //    //编写代码
        //}

        #endregion
    }
}

