#region Summary
/**************************************************************************************
    * 类 名 称：        PaymentMethodService
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        PaymentMethodService.cs
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
using SUP.Common.DataEntity;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Service;
using Enterprise3.Common.Base.Criterion;

using GQT3.QT.Service.Interface;
using GQT3.QT.Facade.Interface;
using GQT3.QT.Model.Domain;

namespace GQT3.QT.Service
{
	/// <summary>
	/// PaymentMethod服务组装处理类
	/// </summary>
    public partial class PaymentMethodService : EntServiceBase<PaymentMethodModel>, IPaymentMethodService
    {
		#region 类变量及属性
		/// <summary>
        /// PaymentMethod业务外观处理对象
        /// </summary>
		IPaymentMethodFacade PaymentMethodFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IPaymentMethodFacade;
            }
        }
        #endregion

        #region 实现 IPaymentMethodService 业务添加的成员

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <returns></returns>
        public FindedResults<PaymentMethodModel> ExecuteDataCheck(ref List<PaymentMethodModel> paymentMethods,string otype) {

            FindedResults<PaymentMethodModel> results = new FindedResults<PaymentMethodModel>();
            List<string> dm = new List<string>();
            List<string> mc = new List<string>();
            if (paymentMethods == null)
            {
                results.Status = ResponseStatus.Error;
                results.Msg = "保存失败，数据异常！";
            }
            else {
                for (int i = 0; i < paymentMethods.Count; i++) {
                    paymentMethods[i].Dm = paymentMethods[i].Dm.Replace(" ", "");
                    paymentMethods[i].Mc = paymentMethods[i].Mc.Replace(" ", "");
                    dm.Add(paymentMethods[i].Dm);
                    mc.Add(paymentMethods[i].Mc);
                }
                Dictionary<string, object> dicWhere1 = new Dictionary<string, object>();
                Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();

                new CreateCriteria(dicWhere1).Add(ORMRestrictions<IList<string>>.In("Dm",dm));
                new CreateCriteria(dicWhere2).Add(ORMRestrictions<IList<string>>.In("Mc", mc));
                results = base.Find(dicWhere1);
                if (results.Data.Count > 0 && results.Data[0] != null && otype != "edit") {
                    results.Status = ResponseStatus.Error;
                    results.Msg = "保存失败，代码重复！";
                    return results;
                }
                results = base.Find(dicWhere2);
                if (results.Data.Count > 0 && results.Data[0] != null && otype != "edit") {
                    results.Status = ResponseStatus.Error;
                    results.Msg = "保存失败，名称重复！";
                    return results;
                }
            }
            return results;
        }
        #endregion
    }
}

