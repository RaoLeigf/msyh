#region Summary
/**************************************************************************************
    * 命名空间：			GBK3.BK.Rule
    * 类 名 称：			PaymentXmRule
    * 文 件 名：			PaymentXmRule.cs
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
using System.Text;
using Enterprise3.Common.Model.NHORM;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.NHORM.Rule;

using GBK3.BK.Rule.Interface;
using GBK3.BK.Dac.Interface;
using GBK3.BK.Model.Domain;

namespace GBK3.BK.Rule
{
	/// <summary>
	/// PaymentXm业务逻辑处理类
	/// </summary>
    public partial class PaymentXmRule : EntRuleBase<PaymentXmModel>, IPaymentXmRule
    {
        
        #region 类变量及属性
        /// <summary>
        /// PaymentXm数据访问处理对象
        /// </summary>
        IPaymentXmDac PaymentXmDac
        {
            get
            {
                if (CurrentDac == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentDac as IPaymentXmDac;
            }
        }

        ////protected override string BillNoIdentity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //public override bool DataCheck(IList<PaymentXmModel> entities, out string prompt)
        //{
        //    throw new NotImplementedException();
        //}

        //public override void DataHandling<TPrimaryKeyType>(ref IList<PaymentXmModel> entities, TPrimaryKeyType masterId)
        //{
        //    throw new NotImplementedException();
        //}
        #endregion

        #region 数据校验
        /// <summary>
        /// 数据校验(重写方法)
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="prompt">提示信息</param>
        /// <returns>处理成功返回True</returns>
        public override bool DataCheck(IList<PaymentXmModel> entities, out string prompt)
        {
            prompt = null;
            return true;
        }
        #endregion

        #region 数据处理
        /// <summary>
        /// 数据处理
        /// </summary>
        /// <para>
        /// 此方法必须进行重写,执行顺序在DataCheck方法之前。
        /// </para>
        /// <param name="entities">实体集合</param>
        /// <param name="masterId">主表关键字值</param>
        public override void DataHandling<Int64>(ref IList<PaymentXmModel> entities, Int64 masterId)
        {
            //参数idPropertyName：主键Id的属性名称，新增时赋主键值用，若是单主键，则无需传递idPropertyName，
            //参数noPropertyName：编码列的属性名称，新增时赋用户编码用，BillNoReqType不为空时，需赋值
            base.RuleHelper.DataHandlingPrimaryKey(ref entities, string.Empty, string.Empty);

        }
        #endregion


        #region 实现 IPaymentDtlRule 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<PaymentDtlModel> ExampleMethod<PaymentDtlModel>(string param)
        //{
        //    //编写代码
        //}

        #endregion

    }
}
