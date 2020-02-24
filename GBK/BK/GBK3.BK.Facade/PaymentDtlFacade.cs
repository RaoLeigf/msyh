#region Summary
/**************************************************************************************
    * 命名空间：			GBK3.BK.Facade
    * 类 名 称：			PaymentDtlFacade
    * 文 件 名：			PaymentDtlFacade.cs
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
using SUP.Common.DataEntity;
using SUP.Common.DataAccess;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Facade;

using GBK3.BK.Facade.Interface;
using GBK3.BK.Rule.Interface;
using GBK3.BK.Model.Domain;
using Enterprise3.Common.Base.Criterion;
using GBK3.BK.Model.Enums;

namespace GBK3.BK.Facade
{
	/// <summary>
	/// PaymentDtl业务组装处理类
	/// </summary>
    public partial class PaymentDtlFacade : EntFacadeBase<PaymentDtlModel>, IPaymentDtlFacade
    {
		#region 类变量及属性
		/// <summary>
        /// PaymentDtl业务逻辑处理对象
        /// </summary>
		IPaymentDtlRule PaymentDtlRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IPaymentDtlRule;
            }
        }
		#endregion

		#region 重载方法
		/// <summary>
        /// 通过外关联的单主键值，获取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns>实体</returns>
        public override FindedResults<PaymentDtlModel> FindByForeignKey<TValType>(TValType id, params string[] sorts)
        {
            FindedResults<PaymentDtlModel> findedResults = base.FindByForeignKey(id, sorts);

            #region 明细Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<PaymentDtlModel>(findedResults.Data, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<PaymentDtlModel>(findedResults.Data, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return findedResults;
        }

        #endregion

        #region 实现 IPaymentDtlFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<PaymentDtlModel> ExampleMethod<PaymentDtlModel>(string param)
        //{
        //    //编写代码
        //}


        /// <summary>
        /// 通过项目主键获取已使用金钱与已冻结金钱的汇总
        /// </summary>
        /// <param name="xmMstPhid">项目主键</param>
        /// <param name="phid">资金拨付项目主键</param>
        /// <returns></returns>
        public Dictionary<string, object> GetSummary(string xmMstPhid, long phid)
        {
            if (string.IsNullOrEmpty(xmMstPhid))
            {
                throw new Exception("参数传递不正确！");
            }
            else
            {
                Dictionary<string, object> keyValue = new Dictionary<string, object>();
                //List<Dictionary<string, object>> keyValues = new List<Dictionary<string, object>>();
                Dictionary<string, object> dic = new Dictionary<string, object>();
                new CreateCriteria(dic)
                    .Add(ORMRestrictions<long>.Eq("XmMstPhid", long.Parse(xmMstPhid)))
                    .Add(ORMRestrictions<long>.NotEq("MstPhid", phid))
                    .Add(ORMRestrictions<byte>.Eq("FDelete", (byte)DeleteType.No));
                var result = this.PaymentDtlRule.Find(dic);
                decimal sum1 = 0, sum2 = 0;
                if (result.Count > 0)
                {
                    foreach (var paymentDtl in result)
                    {
                        if (paymentDtl.FPayment == 9)
                        {
                            sum1 = sum1 + paymentDtl.FAmount;
                        }
                        else
                        {
                            sum2 = sum2 + paymentDtl.FAmount;
                        }
                    }
                }
                keyValue.Add("Use", sum1);
                //keyValues.Add(keyValue);
                //keyValue.Clear();
                keyValue.Add("Frozen", sum2);

                //decimal used = 0;
                //if (result.Count > 0)
                //{
                //    foreach (var res in keyValue)
                //    {
                //        used = used + (decimal)res.Value;
                //    }
                //}
                //var total = this.BudgetMstService.GetDxbzDtl(long.Parse(xmPhid));
                //Type type = total.GetType();
                //decimal sum = (decimal)type.GetProperty("FAmount").GetValue(total);
                //decimal surplus = sum - used;
                //result.Add("sum", sum);
                //result.Add("surplus", surplus);
                //keyValues.Add(keyValue);
                return keyValue;
            }
        }

        /// <summary>
        /// 通过项目主键list获取已使用金钱与已冻结金钱的汇总
        /// </summary>
        /// <param name="xmPhidList">项目主键list</param>
        /// <returns></returns>
        public Dictionary<string, object> GetSummary2(List<long> xmPhidList)
        {
            Dictionary<string, object> keyValue = new Dictionary<string, object>();
            decimal sum1 = 0, sum2 = 0;
            foreach (long xmPhid in xmPhidList)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                new CreateCriteria(dic)
                   .Add(ORMRestrictions<long>.Eq("XmMstPhid", xmPhid))
                   .Add(ORMRestrictions<long>.NotEq("MstPhid", 0));
                var result = this.PaymentDtlRule.Find(dic);
                if (result.Count > 0)
                {
                    foreach (var paymentDtl in result)
                    {
                        if (paymentDtl.FPayment == 9)
                        {
                            sum1 = sum1 + paymentDtl.FAmount;
                        }
                        else
                        {
                            sum2 = sum2 + paymentDtl.FAmount;
                        }
                    }
                }
            }
            
            keyValue.Add("Use", sum1);
            keyValue.Add("Frozen", sum2);
            return keyValue;
        }

        #endregion
    }
}

