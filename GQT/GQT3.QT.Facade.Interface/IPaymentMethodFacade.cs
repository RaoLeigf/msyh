#region Summary
/**************************************************************************************
    * 类 名 称：        IPaymentMethodFacade
    * 命名空间：        GQT3.QT.Facade.Interface
    * 文 件 名：        IPaymentMethodFacade.cs
    * 创建时间：        2018/9/7 
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
using System.Text;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GQT3.QT.Model.Domain;

namespace GQT3.QT.Facade.Interface
{
	/// <summary>
	/// PaymentMethod业务组装层接口
	/// </summary>
    public partial interface IPaymentMethodFacade : IEntFacadeBase<PaymentMethodModel>
    {
        #region IPaymentMethodFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<PaymentMethodModel> ExampleMethod<PaymentMethodModel>(string param)

        PagedResult<PaymentMethodModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts);

        /// <summary>
        /// 通过代码找名称
        /// </summary>
        /// <param name="Dm"></param>
        /// <returns></returns>
        string FindMcByDm(string Dm);
        #endregion
    }
}
