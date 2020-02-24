#region Summary
/**************************************************************************************
    * 命名空间：			GGK3.GK.Facade
    * 类 名 称：			GKPaymentDtlFacade
    * 文 件 名：			GKPaymentDtlFacade.cs
    * 创建时间：			2019/5/20 
    * 作    者：			李明    
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

using GGK3.GK.Facade.Interface;
using GGK3.GK.Rule.Interface;
using GGK3.GK.Model.Domain;

namespace GGK3.GK.Facade
{
	/// <summary>
	/// GKPaymentDtl业务组装处理类
	/// </summary>
    public partial class GKPaymentDtlFacade : EntFacadeBase<GKPaymentDtlModel>, IGKPaymentDtlFacade
    {
		#region 类变量及属性
		/// <summary>
        /// GKPaymentDtl业务逻辑处理对象
        /// </summary>
		IGKPaymentDtlRule GKPaymentDtlRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IGKPaymentDtlRule;
            }
        }
		#endregion

		#region 重载方法
		/// <summary>
        /// 通过外关联的单主键值，获取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns>实体</returns>
        public override FindedResults<GKPaymentDtlModel> FindByForeignKey<TValType>(TValType id, params string[] sorts)
        {
            FindedResults<GKPaymentDtlModel> findedResults = base.FindByForeignKey(id, sorts);

            #region 明细Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<GKPaymentDtlModel>(findedResults.Data, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<GKPaymentDtlModel>(findedResults.Data, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return findedResults;
        }

        #endregion

		#region 实现 IGKPaymentDtlFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<GKPaymentDtlModel> ExampleMethod<GKPaymentDtlModel>(string param)
        //{
        //    //编写代码
        //}

        #endregion
    }
}

