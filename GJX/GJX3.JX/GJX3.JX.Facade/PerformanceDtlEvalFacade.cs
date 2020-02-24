#region Summary
/**************************************************************************************
    * 类 名 称：        PerformanceDtlEvalFacade
    * 命名空间：        GJX3.JX.Facade
    * 文 件 名：        PerformanceDtlEvalFacade.cs
    * 创建时间：        2018/9/12 
    * 作    者：        吾丰明    
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
	/// PerformanceDtlEval业务组装处理类
	/// </summary>
    public partial class PerformanceDtlEvalFacade : EntFacadeBase<PerformanceDtlEvalModel>, IPerformanceDtlEvalFacade
    {
		#region 类变量及属性
		/// <summary>
        /// PerformanceDtlEval业务逻辑处理对象
        /// </summary>
		IPerformanceDtlEvalRule PerformanceDtlEvalRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IPerformanceDtlEvalRule;
            }
        }
		#endregion

		#region 重载方法
		/// <summary>
        /// 通过外关联的单主键值，获取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns>实体</returns>
        public override FindedResults<PerformanceDtlEvalModel> FindByForeignKey<TValType>(TValType id, params string[] sorts)
        {
            FindedResults<PerformanceDtlEvalModel> findedResults = base.FindByForeignKey(id, sorts);

            #region 明细Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<PerformanceDtlEvalModel>(findedResults.Data, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<PerformanceDtlEvalModel>(findedResults.Data, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return findedResults;
        }
        #endregion

		#region 实现 IPerformanceDtlEvalFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<PerformanceDtlEvalModel> ExampleMethod<PerformanceDtlEvalModel>(string param)
        //{
        //    //编写代码
        //}

        #endregion
    }
}

