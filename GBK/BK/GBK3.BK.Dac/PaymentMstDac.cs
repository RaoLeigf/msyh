#region Summary
/**************************************************************************************
    * 命名空间：			GBK3.BK.Dac
    * 类 名 称：			PaymentMstDac
    * 文 件 名：			PaymentMstDac.cs
    * 创建时间：			2019/5/15 
    * 作    者：			吾丰明    
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
using Enterprise3.NHORM.Dac;

using GBK3.BK.Model.Domain;
using GBK3.BK.Dac.Interface;
using Enterprise3.WebApi.GBK3.BK.Controller.Common;
using System.Data;

namespace GBK3.BK.Dac
{
	/// <summary>
	/// PaymentMst数据访问处理类
	/// </summary>
    public partial class PaymentMstDac : EntDacBase<PaymentMstModel>, IPaymentMstDac
    {
        #region 实现 IPaymentMstDac 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<PaymentMstModel> ExampleMethod<PaymentMst>(string param)
        //{
        //    //编写代码
        //}

        /// <summary>
        /// 获取资金拨付主列表
        /// </summary>
        /// <param name="fPhid"></param>
        /// <returns></returns>
        public DataTable GetPaymentList(long fPhid)
        {
            var paymentSql = new SqlDao();
            DataTable dataTable = paymentSql.GetPaymentList(fPhid.ToString());
            return dataTable;
        }

        #endregion
    }
}

