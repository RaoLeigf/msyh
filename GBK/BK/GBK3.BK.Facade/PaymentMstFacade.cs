#region Summary
/**************************************************************************************
    * 命名空间：			GBK3.BK.Facade
    * 类 名 称：			PaymentMstFacade
    * 文 件 名：			PaymentMstFacade.cs
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
using System.Data;
using GBK3.BK.Model.Extend;
using SUP.Common.Base;
using GSP3.SP.Model.Domain;
using GSP3.SP.Rule;
using GSP3.SP.Rule.Interface;
using GBK3.BK.Model.Enums;
using GQT3.QT.Rule.Interface;
using Enterprise3.Common.Model;
using Enterprise3.WebApi.GSP3.SP.Model.Response;
using Enterprise3.WebApi.GSP3.SP.Model.Common;
using GQT3.QT.Model.Domain;
using GGK3.GK.Rule.Interface;
using GGK3.GK.Model.Domain;

namespace GBK3.BK.Facade
{
	/// <summary>
	/// PaymentMst业务组装处理类
	/// </summary>
    public partial class PaymentMstFacade : EntFacadeBase<PaymentMstModel>, IPaymentMstFacade
    {
		#region 类变量及属性
		/// <summary>
        /// PaymentMst业务逻辑处理对象
        /// </summary>
		IPaymentMstRule PaymentMstRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IPaymentMstRule;
            }
        }
		/// <summary>
        /// PaymentDtl业务逻辑处理对象
        /// </summary>
		IPaymentDtlRule PaymentDtlRule { get; set; }


        /// <summary>
        /// PaymentXm业务逻辑处理对象
        /// </summary>
        IPaymentXmRule PaymentXmRule { get; set; }

        /// <summary>
        /// 附件业务逻辑处理对象
        /// </summary>
        IQtAttachmentRule QtAttachmentRule { get; set; }

        IQTSysSetRule QTSysSetRule { get; set; }

        IGKPaymentMstRule GKPaymentMstRule { get; set; }

        IOrganizationRule OrganizationRule { get; set; }

        #endregion

        #region 重载方法
        /// <summary>
        /// 分页获取全部集合
        /// </summary>
        /// <param name="pageNumber">页码(从1开始)</param>
        /// <param name="pageSize">每页大小(最大为200)</param>
        /// <param name="dic">查询条件字典（建议使用Enterprise3.Common.Base.CreateCriteria进行创建）</param>
        /// <param name="sorts">排序({属性名 desc,属性名,属性名 asc})</param>
        /// <returns>集合</returns>
        public override PagedResult<PaymentMstModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<PaymentMstModel> pageResult = base.LoadWithPage(pageNumber, pageSize, dic, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<PaymentMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<PaymentMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return pageResult;
        }

        /// <summary>
        /// 分页获取全部集合
        /// </summary>
        /// <param name="pageNumber">页码(从1开始)</param>
        /// <param name="pageSize">每页大小(最大为200)</param>
        /// <param name="nameSqlName">命名SQL名称</param>
        /// <param name="dic">查询条件字典（建议使用Enterprise3.Common.Base.CreateCriteria进行创建）</param>
        /// <param name="sorts">排序({属性名 desc,属性名,属性名 asc})</param>
        /// <returns>集合</returns>
        public override PagedResult<PaymentMstModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, bool isUseInfoRight = false, params string[] sorts)
        {
            PagedResult<PaymentMstModel> pageResult = base.FacadeHelper.LoadWithPage(pageNumber, pageSize, nameSqlName, dic, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<PaymentMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<PaymentMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return pageResult;
        }

        /// <summary>
        /// 通过id，删除数据
        /// </summary>
        /// <param name="id">单主键id值</param>
        public override DeletedResult Delete<TValType>(TValType id)
        {
            PaymentDtlRule.RuleHelper.DeleteByForeignKey(id);
            return base.Delete(id);
        }

		/// <summary>
        /// 通过ids，删除数据
        /// </summary>
        /// <param name="ids">单主键id集合</param>
        public override DeletedResult Delete<TValType>(IList<TValType> ids)
        {
            PaymentDtlRule.RuleHelper.DeleteByForeignKey(ids);
            return base.Delete(ids);
        }
        #endregion

		#region 实现 IPaymentMstFacade 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="paymentMstEntity"></param>
		/// <param name="paymentDtlEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SavePaymentMst(PaymentMstModel paymentMstEntity, List<PaymentDtlModel> paymentDtlEntities)
        {
            SavedResult<Int64> savedResult = base.Save<Int64>(paymentMstEntity);
            if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
            {
				if (paymentDtlEntities.Count > 0)
				{
					PaymentDtlRule.Save(paymentDtlEntities, savedResult.KeyCodes[0]);
				}
            }

			return savedResult;
        }

        /// <summary>
        /// 获取资金拨付主列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="payment">对象</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public IList<PaymentMstModel> GetPaymentMstList(int pageIndex, PaymentMstModel payment, int pageSize = 20)
        {
            IList<PaymentMstModel> paymentList = new List<PaymentMstModel>();
            //DataTable dataTable = GetPaymentList(payment.FPhid);
            //List<PaymentMstModel> paymentList = DCHelper.DataTable2List<PaymentMstModel>(dataTable).ToList();

            //Dictionary<string, object> dic = new Dictionary<string, object>();
            //if (!string.IsNullOrEmpty(payment.FName))
            //{
            //    //Dictionary<string, object> dicCode = new Dictionary<string, object>();
            //    //Dictionary<string, object> dicName = new Dictionary<string, object>();
            //    //new CreateCriteria(dicCode).
            //    //        Add(ORMRestrictions<string>.Eq("FCode", payment.FName));
            //    //new CreateCriteria(dicName).
            //    //        Add(ORMRestrictions<string>.Eq("FName", payment.FName));
            //    //new CreateCriteria(dic).
            //    //        Add(ORMRestrictions.Or(dicCode, dicName));
            //    paymentList = paymentList.FindAll(t => (t.FName.IndexOf(payment.FName) > -1 || t.FCode.IndexOf(payment.FName) > -1));
            //}
            //if (!string.IsNullOrEmpty(payment.FApproval.ToString()))
            //{
            //    paymentList = paymentList.FindAll(t => t.FApproval == payment.FApproval);
            //}
            //if (!string.IsNullOrEmpty(payment.IsPay.ToString()))
            //{
            //    paymentList = paymentList.FindAll(t => t.IsPay == payment.IsPay);
            //}
            //if (!string.IsNullOrEmpty(payment.FDate.ToString()))
            //{
            //    paymentList = paymentList.FindAll(t => (t.FDate <= payment.EndDate && t.FDate >= payment.StartDate));
            //}
            //if (!string.IsNullOrEmpty(payment.FAmountTotal.ToString()))
            //{
            //    paymentList = paymentList.FindAll(t => (t.FAmountTotal <= payment.MaxAmount && t.FAmountTotal >= payment.MinAmount));
            //}

            //return paymentList;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(payment.FName))
            {
                Dictionary<string, object> dicCode = new Dictionary<string, object>();
                Dictionary<string, object> dicName = new Dictionary<string, object>();
                new CreateCriteria(dicCode).
                        Add(ORMRestrictions<string>.Like("FCode", payment.FName));
                new CreateCriteria(dicName).
                        Add(ORMRestrictions<string>.Like("FName", payment.FName));
                new CreateCriteria(dic).
                        Add(ORMRestrictions.Or(dicCode, dicName));
            }
            if (payment.ApprovalBzs != null && payment.ApprovalBzs.Count > 0)
            {
                new CreateCriteria(dic).
                        Add(ORMRestrictions<List<byte>>.In("FApproval", payment.ApprovalBzs));
            }
            if (payment.PayBzs != null && payment.PayBzs.Count > 0)
            {
                new CreateCriteria(dic).
                        Add(ORMRestrictions<List<byte>>.In("IsPay", payment.PayBzs));
            }
            //if (!string.IsNullOrEmpty(payment.ApprovalBz))
            //{
            //    new CreateCriteria(dic).
            //            Add(ORMRestrictions<byte>.Eq("FApproval", byte.Parse(payment.ApprovalBz)));
            //}
            //if (!string.IsNullOrEmpty(payment.PayBz))
            //{
            //    new CreateCriteria(dic).
            //            Add(ORMRestrictions<byte>.Eq("IsPay", byte.Parse(payment.PayBz)));
            //}
            if (!string.IsNullOrEmpty(payment.StartDate.ToString()) && !string.IsNullOrEmpty(payment.EndDate.ToString()))
            {
                new CreateCriteria(dic).
                        Add(ORMRestrictions<DateTime>.Ge("FDate", DateTime.Parse(payment.StartDate.ToString()))).
                        Add(ORMRestrictions<DateTime>.Le("FDate", DateTime.Parse(payment.EndDate.Value.AddDays(1).ToString())));
            }
            if(!string.IsNullOrEmpty(payment.MaxAmount) && !string.IsNullOrEmpty(payment.MinAmount))
            {
                decimal max = decimal.Parse(payment.MaxAmount);
                decimal min = decimal.Parse(payment.MinAmount);
                if(max < min)
                {
                    throw new Exception("申请金额传递错误！");
                }
                new CreateCriteria(dic).
                        Add(ORMRestrictions<decimal>.Ge("FAmountTotal", min)).
                        Add(ORMRestrictions<decimal>.Le("FAmountTotal", max));
            }
            //if (payment.FOrgphid > 0)
            //{
            //    new CreateCriteria(dic).
            //        Add(ORMRestrictions<long>.Eq("FOrgphid", payment.FOrgphid));
            //}
            //if (!string.IsNullOrEmpty(payment.FDepphid.ToString()))
            //{
            //    new CreateCriteria(dic).
            //        Add(ORMRestrictions<long>.Eq("FDepphid", payment.FDepphid));
            //}
            new CreateCriteria(dic).
                Add(ORMRestrictions<long>.Eq("FOrgphid", payment.FOrgphid)).
                Add(ORMRestrictions<long>.Eq("FDepphid", payment.FDepphid));
            paymentList = this.PaymentMstRule.Find(dic, new string[] { "FCode desc" });
            if(paymentList.Count > 0)
            {
                var payPhids = paymentList.Select(t => t.PhId).ToList();
                if (payPhids.Count > 0)
                {
                    dic.Clear();
                    new CreateCriteria(dic).
                            Add(ORMRestrictions<List<long>>.In("RefbillPhid", payPhids));
                    var GkPayments = this.GKPaymentMstRule.Find(dic);
                    if (GkPayments.Count > 0)
                    {
                        foreach (var per in paymentList)
                        {
                            var Gks = GkPayments.ToList().FindAll(t => t.RefbillPhid == per.PhId);
                            if (Gks.Count > 0)
                            {
                                per.GkPaymentCode = Gks.OrderByDescending(t => t.FCode).ToList()[0].FCode;
                            }
                        }
                    }
                }
            }

            return paymentList;
        }

        /// <summary>
        /// 获取资金拨付主列表
        /// </summary>
        /// <param name="fPhid"></param>
        /// <returns></returns>
        public DataTable GetPaymentList(long fPhid)
        {
            return this.PaymentMstRule.GetPaymentList(fPhid);
        }

        /// <summary>
        /// 点击申请单显示详情
        /// </summary>
        /// <param name="fCode"></param>
        /// <returns></returns>
        public PaymentMstAndXmModel GetPaymentMst(string fCode)
        {
            PaymentMstAndXmModel paymentMstAndXm = new PaymentMstAndXmModel();           
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<long>.Eq("PhId", long.Parse(fCode)));
            var result = this.PaymentMstRule.Find(dic);
            if(result.Count > 0)
            {
                paymentMstAndXm.PaymentMst = result[0];
                dic.Clear();
                new CreateCriteria(dic)
                    .Add(ORMRestrictions<long>.Eq("MstPhid", result[0].PhId));
                var result2 = this.PaymentXmRule.Find(dic, new string[] { "FSeq asc", "XmProjcode Asc" });
                if(result2.Count > 0)
                {
                    List<PaymentXmAndDtlModel> paymentXmAndDtls = new List<PaymentXmAndDtlModel>();
                    foreach (var paymentXm in result2)
                    {
                        PaymentXmAndDtlModel paymentXmAndDtl = new PaymentXmAndDtlModel();
                        paymentXmAndDtl.PaymentXm = paymentXm;
                        //根据项目获取明细数据
                        dic.Clear();
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<long>.Eq("PayXmPhid", paymentXm.PhId));
                        var result3 = this.PaymentDtlRule.Find(dic);
                        if(result3.Count > 0)
                        {
                            paymentXmAndDtl.PaymentDtls = result3;
                            //foreach (var paymentDtl in result3)
                            //{
                            //    paymentXmAndDtl.PaymentDtls.Add(paymentDtl);
                            //}
                        }
                        //根据项目获取附件信息
                        dic.Clear();
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<long>.Eq("RelPhid", paymentXm.PhId))
                            .Add(ORMRestrictions<string>.Eq("BTable", "BK3_PAYMENT_XM"));
                        var result4 = this.QtAttachmentRule.Find(dic);
                        if (result4.Count > 0)
                        {
                            paymentXmAndDtl.QtAttachments = result4;
                        }
                        paymentXmAndDtls.Add(paymentXmAndDtl);
                    }
                    paymentMstAndXm.PaymentXmDtl = paymentXmAndDtls;
                }
                
            }
            return paymentMstAndXm;
        }

        /// <summary>
        /// 根据多个单据号删除多条单据以及单据明细
        /// </summary>
        /// <param name="fCodes">多个单据号</param>
        /// <returns></returns>
        public int DeleteSignle(List<long> fCodes)
        {
            int deletedResult = 0;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic)
                    .Add(ORMRestrictions<List<long>>.In("PhId", fCodes));
            var result = this.PaymentMstRule.Find(dic);
            if(result != null && result.Count > 0)
            {
                //删除单据主表
                deletedResult = this.PaymentMstRule.Delete(dic);
                var mstIdList = result.ToList().Select(t => t.PhId).ToList();
                dic.Clear();
                new CreateCriteria(dic)
                    .Add(ORMRestrictions<List<long>>.In("MstPhid", mstIdList));
                var result2 = this.PaymentXmRule.Find(dic);
                if(result2 != null && result2.Count > 0)
                {
                    //删除单据集合对应的项目表
                    deletedResult = this.PaymentXmRule.Delete(dic);
                    var xmIdList = result2.ToList().Select(t => t.PhId).ToList();
                    //删除明细
                    dic.Clear();
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<List<long>>.In("PayXmPhid", xmIdList));
                    var result3 = this.PaymentDtlRule.Find(dic);
                    if(result3 != null && result3.Count > 0)
                    {
                        deletedResult = this.PaymentDtlRule.Delete(dic);
                    }
                    //根据项目集合删除附件
                    dic.Clear();
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<List<long>>.In("RelPhid", xmIdList))
                        .Add(ORMRestrictions<string>.Eq("BTable", "BK3_PAYMENT_XM"));
                    var result4 = this.QtAttachmentRule.Find(dic);
                    if (result4 != null && result4.Count > 0)
                    {
                        deletedResult = this.QtAttachmentRule.Delete(dic);
                    }
                }
            }
            return deletedResult;
        }

        /// <summary>
        /// 生成申请单编号
        /// </summary>
        /// <returns></returns>
        public string GetPaymentCode()
        {
            string paymentCode = "";
            string payment = "";
            string code = "";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic).
                Add(ORMRestrictions<long>.Ge("PhId", (long)0));
            var result = this.PaymentMstRule.Find(dic, new string[] { "FCode Desc" });
            if(result != null && result.Count > 0)
            {
                string fCode = result[0].FCode;
                if(fCode.Length == 12)
                {
                    payment = DateTime.Now.ToString("yyyyMMdd");
                    string fcode1 = fCode.Substring(0,8);
                    if (payment.Equals(fcode1))
                    {
                        int fcode2 = int.Parse(fCode.Substring(8)) + 1;
                        if(fcode2 > 999 && fcode2 < 10000)
                        {
                            code = fcode2.ToString();
                        }
                        else if (fcode2>99 && fcode2 <1000)
                        {
                            code = "0" + fcode2;
                        }
                        else if (fcode2 > 9 && fcode2 < 100)
                        {
                            code = "00" + fcode2;
                        }
                        else if(fcode2 > 0 && fcode2 < 10)
                        {
                            code = "000" + fcode2;
                        }
                        else
                        {
                            throw new Exception("申请单号生成失败！");
                        }
                    }
                    else
                    {
                        code = "0001";
                    }
                }
                else
                {
                    throw new Exception("申请单号为："+fCode+"的申请编码生成错误，请联系管理员！");
                }
            }
            else
            {
                payment = DateTime.Now.ToString("yyyyMMdd");
                code = "0001";               
            }
            paymentCode = payment + code;
            return paymentCode;
        }

        /// <summary>
        /// 新增申请单
        /// </summary>
        /// <param name="paymentMstAndXm">申请单对象</param>
        /// <returns></returns>
        public SavedResult<long> AddSignle(PaymentMstAndXmModel paymentMstAndXm)
        {
            SavedResult<long> savedResultMst = new SavedResult<long>();
            SavedResult<long> savedResult = new SavedResult<long>();
            if(paymentMstAndXm.PaymentMst != null)
            {
                paymentMstAndXm.PaymentMst.FDate = DateTime.Now;
                paymentMstAndXm.PaymentMst.PersistentState = PersistentState.Added;
                paymentMstAndXm.PaymentMst.FCode = GetPaymentCode();
                savedResultMst = this.PaymentMstRule.Save<long>(paymentMstAndXm.PaymentMst);
                if (savedResultMst.KeyCodes.Count > 0)
                {
                    long mstPhid = savedResultMst.KeyCodes[0];
                    if(paymentMstAndXm.PaymentXmDtl !=null && paymentMstAndXm.PaymentXmDtl.Count > 0)
                    {
                        int seq = 1;
                        foreach(var paymentXmDtl in paymentMstAndXm.PaymentXmDtl)
                        {
                            //PaymentXmAndDtlModel paymentXmAndDtl = new PaymentXmAndDtlModel();
                            paymentXmDtl.PaymentXm.PersistentState = PersistentState.Added;
                            paymentXmDtl.PaymentXm.MstPhid = mstPhid;
                            paymentXmDtl.PaymentXm.FSeq = seq++;
                            savedResult = this.PaymentXmRule.Save<long>(paymentXmDtl.PaymentXm);
                            if(savedResult.KeyCodes.Count > 0)
                            {
                                long xmPhid = savedResult.KeyCodes[0];
                                //新增明细
                                if(paymentXmDtl.PaymentDtls != null && paymentXmDtl.PaymentDtls.Count > 0)
                                {
                                    foreach(var paymentDtl in paymentXmDtl.PaymentDtls)
                                    {
                                        paymentDtl.PersistentState = PersistentState.Added;
                                        paymentDtl.MstPhid = mstPhid;
                                        paymentDtl.PayXmPhid = xmPhid;
                                    }
                                }
                                //新增项目附件
                                if(paymentXmDtl.QtAttachments != null && paymentXmDtl.QtAttachments.Count > 0)
                                {
                                    foreach(var attachment in paymentXmDtl.QtAttachments)
                                    {
                                        attachment.RelPhid = xmPhid;
                                        attachment.BTable = "BK3_PAYMENT_XM";
                                        attachment.PersistentState = PersistentState.Added;
                                    }
                                }
                                savedResult = this.QtAttachmentRule.Save<long>(paymentXmDtl.QtAttachments);
                                savedResult = this.PaymentDtlRule.Save<long>(paymentXmDtl.PaymentDtls);
                            }
                        }
                    }
                }
                
            }
            return savedResultMst;
        }

        /// <summary>
        /// 修改申请单
        /// </summary>
        /// <param name="paymentMstAndXm">新的申请单</param>
        /// <returns></returns>
        public SavedResult<long> UpdateSignle(PaymentMstAndXmModel paymentMstAndXm)
        {
            SavedResult<long> savedResultMst = new SavedResult<long>();
            SavedResult<long> savedResult = new SavedResult<long>();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if(paymentMstAndXm.PaymentMst != null)
            {
                paymentMstAndXm.PaymentMst.PersistentState = PersistentState.Modified;
                if (string.IsNullOrEmpty(paymentMstAndXm.PaymentMst.FCode))
                {
                    paymentMstAndXm.PaymentMst.FCode = GetPaymentCode();
                }
                savedResultMst = this.PaymentMstRule.Save<long>(paymentMstAndXm.PaymentMst);
                if (savedResultMst.KeyCodes.Count > 0)
                {
                    long mstPhid = savedResultMst.KeyCodes[0];
                    //先删除该申请单下原有的项目与明细(包括附件)
                    int deletedResult = 0;
                    dic.Clear();
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<long>.Eq("MstPhid", mstPhid));
                    var result2 = this.PaymentXmRule.Find(dic);
                    if (result2 != null && result2.Count > 0)
                    {
                        //删除单据对应项目表数据
                        deletedResult = this.PaymentXmRule.Delete(dic);
                        var xmIdList = result2.ToList().Select(t => t.PhId).ToList();
                        //删除明细表数据
                        dic.Clear();
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<List<long>>.In("PayXmPhid", xmIdList));
                        var result3 = this.PaymentDtlRule.Find(dic);
                        if (result3 != null && result3.Count > 0)
                        {
                            deletedResult = this.PaymentDtlRule.Delete(dic);
                        }
                        //删除附件数据
                        dic.Clear();
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<List<long>>.In("RelPhid", xmIdList))
                            .Add(ORMRestrictions<string>.Eq("BTable", "BK3_PAYMENT_XM"));
                        var result4 = this.QtAttachmentRule.Find(dic);
                        if (result4 != null && result4.Count > 0)
                        {
                            deletedResult = this.QtAttachmentRule.Delete(dic);
                        }
                    }

                    //接着新增修改后的项目与明细
                    if (paymentMstAndXm.PaymentXmDtl != null && paymentMstAndXm.PaymentXmDtl.Count > 0)
                    {
                        int seq = 1;
                        foreach (var paymentXmDtl in paymentMstAndXm.PaymentXmDtl)
                        {
                            //新增单据对应项目
                            //PaymentXmAndDtlModel paymentXmAndDtl = new PaymentXmAndDtlModel();
                            paymentXmDtl.PaymentXm.PersistentState = PersistentState.Added;
                            paymentXmDtl.PaymentXm.MstPhid = mstPhid;
                            paymentXmDtl.PaymentXm.FSeq = seq++;

                            savedResult = this.PaymentXmRule.Save<long>(paymentXmDtl.PaymentXm);
                            if (savedResult.KeyCodes.Count > 0)
                            {
                                long xmPhid = savedResult.KeyCodes[0];
                                //新增明细
                                if (paymentXmDtl.PaymentDtls != null && paymentXmDtl.PaymentDtls.Count > 0)
                                {
                                    foreach (var paymentDtl in paymentXmDtl.PaymentDtls)
                                    {
                                        paymentDtl.PersistentState = PersistentState.Added;
                                        paymentDtl.MstPhid = mstPhid;
                                        paymentDtl.PayXmPhid = xmPhid;
                                    }
                                }
                                //新增项目附件
                                if (paymentXmDtl.QtAttachments != null && paymentXmDtl.QtAttachments.Count > 0)
                                {
                                    foreach (var attachment in paymentXmDtl.QtAttachments)
                                    {
                                        attachment.RelPhid = xmPhid;
                                        attachment.BTable = "BK3_PAYMENT_XM";                                        
                                        attachment.PersistentState = PersistentState.Added;
                                    }
                                }
                                savedResult = this.QtAttachmentRule.Save<long>(paymentXmDtl.QtAttachments);
                                savedResult = this.PaymentDtlRule.Save<long>(paymentXmDtl.PaymentDtls);
                            }
                        }
                    }
                }
            }
            
            return savedResultMst;
        }

        ///// <summary>
        ///// 把数据保存到审批记录表中
        ///// </summary>
        ///// <param name="gAppvalRecords">数据列表</param>
        ///// <returns></returns>
        //public SavedResult<long> AddAppvalRecord(List<GAppvalRecordModel> gAppvalRecords)
        //{
        //    SavedResult<long> savedResult = new SavedResult<long>();
        //    if(gAppvalRecords.Count > 0)
        //    {
        //        foreach (var gAppvalRecord in gAppvalRecords)
        //        {
        //            //关联单据不能为空
        //            if (gAppvalRecord.RefbillPhid < 1)
        //            {
        //                throw new Exception("关联单据不能为空！");
        //            }
        //            //关联流程不能为空
        //            if (gAppvalRecord.ProcPhid < 1)
        //            {
        //                throw new Exception("关联流程不能为空！");
        //            }
        //            //关联岗位不能为空
        //            if (gAppvalRecord.PostPhid < 1)
        //            {
        //                throw new Exception("关联岗位不能为空！");
        //            }
        //            //关联审批管理员不能为空
        //            if (gAppvalRecord.OperaPhid < 1)
        //            {
        //                throw new Exception("关联审批管理员不能为空！");
        //            }
        //            //审批状态传递有误
        //            if (gAppvalRecord.FApproval != (byte)ApprovalType.pend)
        //            {
        //                throw new Exception("审批状态传递有误！");
        //            }
        //        }            
        //        Dictionary<string, object> dic = new Dictionary<string, object>();
        //        new CreateCriteria(dic)
        //                .Add(ORMRestrictions<long>.Eq("RefbillPhid", gAppvalRecords[0].RefbillPhid));
        //        var result = this.GAppvalRecordRule.Find(dic);
        //        if(result.Count > 0)
        //        {
        //            byte fApproval = gAppvalRecords[0].FApproval;
        //            foreach (var res in result)
        //            {
        //                res.FApproval = fApproval;
        //                res.PersistentState = PersistentState.Modified;
        //            }
        //            savedResult = this.GAppvalRecordRule.Save<long>(result);
        //        }
        //        else
        //        {
        //            foreach(var gAppvalRecord in gAppvalRecords)
        //            {
        //                gAppvalRecord.PersistentState = PersistentState.Added;
        //            }
        //            savedResult = this.GAppvalRecordRule.Save<long>(gAppvalRecords);
        //        }
        //    }
        //    return savedResult;
        //}

        /// <summary>
        /// 更新资金拨付单的审批状态
        /// </summary>
        /// <param name="phid">单据id</param>
        /// <param name="fApproval">审批状态</param>
        /// <returns></returns>
        public SavedResult<long> UpdatePayment(long phid, byte fApproval) {
            if (phid == 0)
                return null;

            PaymentMstModel paymentMst = PaymentMstRule.Find(phid);

            paymentMst.FApproval = fApproval;
            paymentMst.PersistentState = PersistentState.Modified;

            return PaymentMstRule.Save<Int64>(paymentMst);
        }

        /// <summary>
        /// 根据主表主键查找明细表数据
        /// </summary>
        /// <param name="mstPhid">主表主键</param>
        /// <returns></returns>
        public IList<PaymentDtlModel> GetPaymentDtlsByMstPhid(long mstPhid)
        {
            IList<PaymentDtlModel> paymentDtls = new List<PaymentDtlModel>();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic).
                Add(ORMRestrictions<long>.Eq("MstPhid", mstPhid));
            paymentDtls = this.PaymentDtlRule.Find(dic);
            return paymentDtls;
        }

        /// <summary>
        /// 根据单据主键与支付状态修改单据
        /// </summary>
        /// <param name="phid">单据主键</param>
        /// <param name="isPay">支付状态</param>
        public void UpdatePaymentPay(long phid, int isPay)
        {
            //int result;
            //var list = new List<PropertyColumnMapperInfo>();
            //list.Add(new PropertyColumnMapperInfo
            //{
            //    PropertyName = "PhId",
            //    Value = phid
            //});
            //list.Add(new PropertyColumnMapperInfo
            //{
            //    PropertyName = "IsPay",
            //    Value = (byte)isPay
            //});
            //result = this.PaymentMstRule.RuleHelper.Update(list);
            //return result;
            this.PaymentMstRule.UpdatePaymentPay(phid, isPay);
        }


        /// <summary>
        /// 修改后的获取审批单据列表的接口
        /// </summary>
        /// <param name="payment">参数结合</param>
        /// <returns></returns>
        public IList<PaymentMstModel> GetPaymentList(PaymentMstModel payment)
        {
            IList<PaymentMstModel> paymentList = new List<PaymentMstModel>();

            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(payment.FName))
            {
                Dictionary<string, object> dicCode = new Dictionary<string, object>();
                Dictionary<string, object> dicName = new Dictionary<string, object>();
                new CreateCriteria(dicCode).
                        Add(ORMRestrictions<string>.Like("FCode", payment.FName));
                new CreateCriteria(dicName).
                        Add(ORMRestrictions<string>.Like("FName", payment.FName));
                new CreateCriteria(dic).
                        Add(ORMRestrictions.Or(dicCode, dicName));
            }
            if (payment.ApprovalBzs !=null && payment.ApprovalBzs.Count > 0)
            {
                new CreateCriteria(dic).
                        Add(ORMRestrictions<List<byte>>.In("FApproval", payment.ApprovalBzs));
            }
            if (payment.PayBzs != null && payment.PayBzs.Count > 0)
            {
                new CreateCriteria(dic).
                        Add(ORMRestrictions<List<byte>>.In("IsPay", payment.PayBzs));
            }
            if (!string.IsNullOrEmpty(payment.StartDate.ToString()) && !string.IsNullOrEmpty(payment.EndDate.ToString()))
            {
                new CreateCriteria(dic).
                        Add(ORMRestrictions<DateTime>.Ge("FDate", DateTime.Parse(payment.StartDate.ToString()))).
                        Add(ORMRestrictions<DateTime>.Le("FDate", DateTime.Parse(payment.EndDate.Value.AddDays(1).ToString())));
            }
            if (!string.IsNullOrEmpty(payment.MaxAmount) && !string.IsNullOrEmpty(payment.MinAmount))
            {
                decimal max = decimal.Parse(payment.MaxAmount);
                decimal min = decimal.Parse(payment.MinAmount);
                if (max < min)
                {
                    throw new Exception("申请金额传递错误！");
                }
                new CreateCriteria(dic).
                        Add(ORMRestrictions<decimal>.Ge("FAmountTotal", min)).
                        Add(ORMRestrictions<decimal>.Le("FAmountTotal", max));
            }
            new CreateCriteria(dic).
                Add(ORMRestrictions<long>.Eq("FOrgphid", payment.FOrgphid)).
                Add(ORMRestrictions<long>.Eq("FDepphid", payment.FDepphid)).
                Add(ORMRestrictions<string>.Eq("FYear", payment.FYear));
            //获取符合条件的审批单据
            paymentList = this.PaymentMstRule.Find(dic, new string[] { "IsPay asc", "FCode desc" });

            if(paymentList != null && paymentList.Count > 0)
            {
                //获取单据类型与单据类型主键
                IList<QTSysSetModel> models = QTSysSetRule.RuleHelper.Find(t => t.DicType == "splx" && t.TypeCode=="1");
                List<AppvalRecordVo> appvalRecords = new List<AppvalRecordVo>();
                SqlDao sqlDao = new SqlDao();
                if (models.Count > 0)
                {
                    OrganizeModel Org = this.OrganizationRule.Find(payment.FOrgphid);
                    payment.FOrgcode = Org.OCode;
                    appvalRecords = sqlDao.GetRecords(payment.FYear, long.Parse(payment.UserId), models[0].Value, payment.FOrgcode, "1", models[0].PhId);
                    if(appvalRecords != null && appvalRecords.Count > 0)
                    {
                        appvalRecords = appvalRecords.FindAll(t => t.DepId == payment.FDepphid);
                    }                    
                }
                //该部门下存在自己未审核的数据要放最前面
                if(appvalRecords != null && appvalRecords.Count > 0)
                {
                    var refbillPhids = appvalRecords.Select(t => t.RefbillPhid);
                    var pays1 = paymentList.ToList().FindAll(t => refbillPhids.Contains(t.PhId));
                    if(pays1.Count > 0)
                    {
                        foreach(var pay in pays1)
                        {
                            pay.IsApprovalNow = 1;
                            var appvalRecord = appvalRecords.Find(t => t.RefbillPhid == pay.PhId);
                            if(appvalRecord == null)
                            {
                                throw new Exception("单据审批流查询失败！");
                            }
                            pay.RefbillPhid = appvalRecord.RefbillPhid;
                            pay.FBilltype = appvalRecord.FBilltype;
                            pay.PostPhid = appvalRecord.PostPhid;
                            pay.ProcPhid = appvalRecord.ProcPhid;
                            pay.OperaPhid = appvalRecord.OperaPhid;
                            pay.AppvalPhid = appvalRecord.PhId;
                        }
                    }
                    var pays2 = paymentList.ToList().FindAll(t => (!refbillPhids.Contains(t.PhId)));
                    if (pays2.Count > 0)
                    {
                        foreach (var pay in pays2)
                        {
                            pay.IsApprovalNow = 0;
                        }
                    }
                    pays1.AddRange(pays2);
                    paymentList = pays1;
                }
                var payPhids = paymentList.Select(t => t.PhId).ToList();
                if(payPhids.Count > 0)
                {
                    dic.Clear();
                    new CreateCriteria(dic).
                            Add(ORMRestrictions<List<long>>.In("RefbillPhid", payPhids));
                    var GkPayments = this.GKPaymentMstRule.Find(dic);
                    if(GkPayments.Count > 0)
                    {
                        foreach(var per in paymentList)
                        {
                            var Gks = GkPayments.ToList().FindAll(t => t.RefbillPhid == per.PhId);
                            if (Gks.Count > 0)
                            {
                                per.GkPaymentCode = Gks.OrderByDescending(t => t.FCode).ToList()[0].FCode;
                            }                            
                        }
                    }
                }  
            }               
            return paymentList;
        }
        
        /// <summary>
        /// 批量作废单据
        /// </summary>
        /// <param name="phids">单据集合</param>
        /// <returns></returns>
        public SavedResult<long> PostCancetPaymentList(List<long> phids)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic).
                    Add(ORMRestrictions<List<long>>.In("PhId", phids));
            var payments = this.PaymentMstRule.Find(dic);
            if(payments.Count > 0)
            {
                foreach(var payment in payments)
                {
                    if(payment.FApproval != (byte)ApprovalType.not && payment.FApproval != (byte)ApprovalType.no)
                    {
                        throw new Exception("只有未送审与未通过的单据可以作废！");
                    }
                    payment.FDelete = (byte)1;
                    payment.PersistentState = PersistentState.Modified;
                }
                savedResult = this.PaymentMstRule.Save<long>(payments);
                dic.Clear();
                new CreateCriteria(dic).
                        Add(ORMRestrictions<List<long>>.In("MstPhid", phids));
                var paymentXms = this.PaymentXmRule.Find(dic);
                if(paymentXms.Count > 0)
                {
                    foreach (var paymentXm in paymentXms)
                    {
                        paymentXm.FDelete = (byte)1;
                        paymentXm.PersistentState = PersistentState.Modified;
                    }
                    savedResult = this.PaymentXmRule.Save<long>(paymentXms);
                }
                dic.Clear();
                new CreateCriteria(dic).
                        Add(ORMRestrictions<List<long>>.In("MstPhid", phids));
                var paymentDtls = this.PaymentDtlRule.Find(dic);
                if (paymentDtls.Count > 0)
                {
                    foreach (var paymentDtl in paymentDtls)
                    {
                        paymentDtl.FDelete = (byte)1;
                        paymentDtl.PersistentState = PersistentState.Modified;
                    }
                    savedResult = this.PaymentDtlRule.Save<long>(paymentDtls);
                }
            }
            return savedResult;
        }

        /// <summary>
        /// 修改业务单据的支付状态
        /// </summary>
        /// <param name="GkPayment">支付主表对象</param>
        /// <param name="gKPaymentDtls">支付子表对象</param>
        /// <returns></returns>
        public SavedResult<long> UpdatePayState(GKPaymentMstModel GkPayment, IList<GKPaymentDtlModel> gKPaymentDtls)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            if(GkPayment != null)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                new CreateCriteria(dic).Add(ORMRestrictions<long>.Eq("PhId", GkPayment.RefbillPhid));
                var payment = this.PaymentMstRule.Find(dic);
                if(payment != null && payment.Count > 0)
                {
                    foreach(var pay in payment)
                    {
                        if(GkPayment.FState == (byte)1)
                        {
                            pay.IsPay = (byte)9;
                        }
                        if (GkPayment.FState == (byte)2)
                        {
                            pay.IsPay = (byte)1;
                        }
                        if (GkPayment.FState == (byte)3)
                        {
                            pay.IsPay = (byte)2;
                        }
                        pay.PersistentState = PersistentState.Modified;
                    }
                    savedResult = this.PaymentMstRule.Save<long>(payment);

                    if(gKPaymentDtls != null && gKPaymentDtls.Count > 0)
                    {                        
                        var dtlLists = gKPaymentDtls.Select(t => t.RefbillDtlPhid).ToList();
                        if(dtlLists.Count > 0)
                        {
                            dic.Clear();
                            new CreateCriteria(dic).Add(ORMRestrictions<List<long>>.In("PhId", dtlLists));
                            var paymentDtls = this.PaymentDtlRule.Find(dic);
                            if(paymentDtls != null && paymentDtls.Count > 0)
                            {
                                foreach (var paymentdtl in paymentDtls)
                                {
                                    var payState = gKPaymentDtls.ToList().Find(t => t.RefbillDtlPhid == paymentdtl.PhId) == null ? (byte)0 : gKPaymentDtls.ToList().Find(t => t.RefbillDtlPhid == paymentdtl.PhId).FState;
                                    if (payState == (byte)1)
                                    {
                                        paymentdtl.FPayment = (byte)9;
                                    }
                                    if (payState == (byte)2)
                                    {
                                        paymentdtl.FPayment = (byte)1;
                                    }
                                    if (payState == (byte)3)
                                    {
                                        paymentdtl.FPayment = (byte)2;
                                    }
                                    paymentdtl.PersistentState = PersistentState.Modified;
                                }
                                savedResult = this.PaymentDtlRule.Save<long>(paymentDtls);
                            }
                        }
                    }
                }
            }
            return savedResult;
        }
        #endregion
    }
}

