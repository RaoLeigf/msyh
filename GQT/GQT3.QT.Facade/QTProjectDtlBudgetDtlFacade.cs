#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Facade
    * 类 名 称：			QTProjectDtlBudgetDtlFacade
    * 文 件 名：			QTProjectDtlBudgetDtlFacade.cs
    * 创建时间：			2019/9/4 
    * 作    者：			刘杭    
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

using GQT3.QT.Facade.Interface;
using GQT3.QT.Rule.Interface;
using GQT3.QT.Model.Domain;

namespace GQT3.QT.Facade
{
	/// <summary>
	/// QTProjectDtlBudgetDtl业务组装处理类
	/// </summary>
    public partial class QTProjectDtlBudgetDtlFacade : EntFacadeBase<QTProjectDtlBudgetDtlModel>, IQTProjectDtlBudgetDtlFacade
    {
		#region 类变量及属性
		/// <summary>
        /// QTProjectDtlBudgetDtl业务逻辑处理对象
        /// </summary>
		IQTProjectDtlBudgetDtlRule QTProjectDtlBudgetDtlRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IQTProjectDtlBudgetDtlRule;
            }
        }
        #endregion

        #region 重载方法
        /// <summary>
        /// 通过外关联的单主键值，获取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns>实体</returns>
        public override FindedResults<QTProjectDtlBudgetDtlModel> FindByForeignKey<TValType>(TValType id, params string[] sorts)
        {
            FindedResults<QTProjectDtlBudgetDtlModel> findedResults = base.FindByForeignKey(id, sorts);

            #region 明细Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
            //helpdac.CodeToName<QTProjectDtlBudgetDtlModel>(findedResults.Data, "属性名", "注册的帮助标识"
            //helpdac.CodeToName<QTProjectDtlBudgetDtlModel>(findedResults.Data, "Code属性名", "Name属性名", "注册的帮助标识", "");
            helpdac.CodeToName<QTProjectDtlBudgetDtlModel>(findedResults.Data, "FBudgetAccounts", "FBudgetAccounts_EXName", "GHBudgetAccounts","");
            #endregion

            return findedResults;
        }

        #endregion

		#region 实现 IQTProjectDtlBudgetDtlFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<QTProjectDtlBudgetDtlModel> ExampleMethod<QTProjectDtlBudgetDtlModel>(string param)
        //{
        //    //编写代码
        //}

        #endregion
    }
}

