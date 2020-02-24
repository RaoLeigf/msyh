#region Summary
/**************************************************************************************
    * 命名空间：			GBK3.BK.Facade
    * 类 名 称：			PaymentXmFacade
    * 文 件 名：			PaymentXmFacade.cs
    * 创建时间：			2019/5/23 
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

using GBK3.BK.Facade.Interface;
using GBK3.BK.Rule.Interface;
using GBK3.BK.Model.Domain;

namespace GBK3.BK.Facade
{
	/// <summary>
	/// PaymentXm业务组装处理类
	/// </summary>
    public partial class PaymentXmFacade : EntFacadeBase<PaymentXmModel>, IPaymentXmFacade
    {
		#region 类变量及属性
		/// <summary>
        /// PaymentXm业务逻辑处理对象
        /// </summary>
		IPaymentXmRule PaymentXmRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IPaymentXmRule;
            }
        }
		#endregion

		#region 重载方法
		/// <summary>
        /// 通过外关联的单主键值，获取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns>实体</returns>
        public override FindedResults<PaymentXmModel> FindByForeignKey<TValType>(TValType id, params string[] sorts)
        {
            FindedResults<PaymentXmModel> findedResults = base.FindByForeignKey(id, sorts);

            #region 明细Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<PaymentXmModel>(findedResults.Data, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<PaymentXmModel>(findedResults.Data, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return findedResults;
        }

        #endregion

		#region 实现 IPaymentXmFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<PaymentXmModel> ExampleMethod<PaymentXmModel>(string param)
        //{
        //    //编写代码
        //}

        #endregion
    }
}

