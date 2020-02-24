#region Summary
/**************************************************************************************
    * 命名空间：			GGK3.GK.Facade
    * 类 名 称：			GKPaymentMstFacade
    * 文 件 名：			GKPaymentMstFacade.cs
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
using GBK3.BK.Rule.Interface;
using Enterprise3.Common.Base.Criterion;
using GData3.Common.Utils;
using GBK3.BK.Model.Domain;
using Enterprise3.WebApi.Client.Models;
using Enterprise3.Common.Base.Helpers;
using Enterprise3.Common.Model;
using SUP.Common.Base;
using GGK3.GK.Model.Enums;
using System.Data;
using NG3.Data.Service;
using GQT3.QT.Rule.Interface;
using GQT3.QT.Model.Domain;
using GData3.Common.Model.Enums;

namespace GGK3.GK.Facade
{
	/// <summary>
	/// GKPaymentMst业务组装处理类
	/// </summary>
    public partial class GKPaymentMstFacade : EntFacadeBase<GKPaymentMstModel>, IGKPaymentMstFacade
    {
		#region 类变量及属性
		/// <summary>
        /// GKPaymentMst业务逻辑处理对象
        /// </summary>
		IGKPaymentMstRule GKPaymentMstRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IGKPaymentMstRule;
            }
        }
		/// <summary>
        /// GKPaymentDtl业务逻辑处理对象
        /// </summary>
		IGKPaymentDtlRule GKPaymentDtlRule { get; set; }

        /// <summary>
        /// 资金拨付单明细对象
        /// </summary>
        IPaymentDtlRule PaymentDtlRule { get; set; }

        /// <summary>
        /// 资金拨付单项目对象
        /// </summary>
        IPaymentXmRule PaymentXmRule { get; set; }

        /// <summary>
        /// 资金拨付单主表对象
        /// </summary>
        IPaymentMstRule PaymentMstRule { get; set; }

        IOrganizationRule OrganizationRule { get; set; }
        /*
        /// <summary>
        /// 调用服务必要的请求参数对象
        /// </summary>
        public AppInfoBase ApiReqParam
        {
            get
            {
                return new AppInfoBase
                {
                    AppKey = ConfigHelper.GetString("AppKey", "D31B7F91-3068-4A49-91EE-F3E13AE5C48C"), //必须
                    AppSecret = ConfigHelper.GetString("AppSecret", "103CB639-840C-4E4F-8812-220ECE3C4E4D"), //必须
                    DbServerName = NG3.AppInfoBase.DbServerName,
                    OCode = NG3.AppInfoBase.OCode,
                    OrgName = NG3.AppInfoBase.OrgName,
                    SessionKey = NG3.AppInfoBase.SessionID,
                    TokenKey = string.Empty,
                    DbName = NG3.AppInfoBase.DbName,
                    UserKey = NG3.AppInfoBase.LoginID,
                    UserName = NG3.AppInfoBase.UserName,
                    UserId = NG3.AppInfoBase.UserID,
                    OrgId = NG3.AppInfoBase.OrgID,
                };
            }
        }
        

        public string BankApiURL
        {
            get
            {
                return ConfigHelper.GetString("BankApiURL", "127.0.0.1");
            }
        }
        */

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
        public override PagedResult<GKPaymentMstModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<GKPaymentMstModel> pageResult = base.LoadWithPage(pageNumber, pageSize, dic, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<GKPaymentMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<GKPaymentMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
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
        public override PagedResult<GKPaymentMstModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, bool isUseInfoRight = false, params string[] sorts)
        {
            PagedResult<GKPaymentMstModel> pageResult = base.FacadeHelper.LoadWithPage(pageNumber, pageSize, nameSqlName, dic, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<GKPaymentMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<GKPaymentMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return pageResult;
        }

        /// <summary>
        /// 通过id，删除数据
        /// </summary>
        /// <param name="id">单主键id值</param>
        public override DeletedResult Delete<TValType>(TValType id)
        {
            //GKPaymentDtlRule.RuleHelper.DeleteByForeignKey(id);
            GKPaymentDtlRule.RuleHelper.DeleteByForeignKey(id);
            return base.Delete(id);
        }

		/// <summary>
        /// 通过ids，删除数据
        /// </summary>
        /// <param name="ids">单主键id集合</param>
        public override DeletedResult Delete<TValType>(IList<TValType> ids)
        {
            GKPaymentDtlRule.RuleHelper.DeleteByForeignKey(ids);
            return base.Delete(ids);
        }
        #endregion

		#region 实现 IGKPaymentMstFacade 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="gKPaymentMstEntity"></param>
		/// <param name="gKPaymentDtlEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveGKPaymentMst(GKPaymentMstModel gKPaymentMstEntity, List<GKPaymentDtlModel> gKPaymentDtlEntities)
        {
            SavedResult<Int64> savedResult = base.Save<Int64>(gKPaymentMstEntity);
            if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
            {
                if (gKPaymentDtlEntities.Count > 0)
                {                    
                    gKPaymentDtlEntities.Sort((a, b) =>
                    {
                        if (!string.IsNullOrEmpty(a.FIseqno) && !string.IsNullOrEmpty(b.FIseqno)) {
                            return int.Parse(a.FIseqno) - int.Parse(b.FIseqno);
                        }
                        else
                        {
                            return 0;
                        }                        
                    });

                    int i = 0;
                    foreach (var dtl in gKPaymentDtlEntities) {
                        if (!string.IsNullOrEmpty(dtl.FIseqno))
                        {
                            i = int.Parse(dtl.FIseqno);
                        }
                        else {
                            i++;
                            dtl.FIseqno = i.ToString();
                        }
                    }

                    GKPaymentDtlRule.Save(gKPaymentDtlEntities, savedResult.KeyCodes[0]);
				}
                UpdateZjbfPaymentPayState(savedResult.KeyCodes[0], 0);
            }

			return savedResult;
        }

        private DataTable GetPaymentNewCodeDataTable(Int64 ref_billphid) {
            DataTable dt = null;
            string sql = String.Format("SELECT mst_phid, phid, f_new_code, ng_insert_dt, old_dtl_phid FROM GK3_PAYMENT_DTL WHERE REFBILL_PHID={0} ORDER BY ng_insert_dt ", 
                ref_billphid);
            DbHelper.Open();
            dt = DbHelper.GetDataTable(sql);

            return dt;
        }

        /// <summary>
        /// 获取资金拨付支付单信息
        /// </summary>
        /// <param name="phid">支付单主键</param>
        /// <returns></returns>
        public GKPayment4ZjbfModel GetPayment4Zjbf(Int64 phid) {
            GKPayment4ZjbfModel ret = null;

            GKPaymentMstModel mst = this.EntRule.Find(phid);
            if (mst != null) {
                ret = new GKPayment4ZjbfModel();
                //去业务单查部门名称
                PaymentMstModel paymentMst = PaymentMstRule.Find(mst.RefbillPhid);
                mst.fdepname = paymentMst.FDepname;
                mst.FOrgname = paymentMst.FOrgname;

                Dictionary<string, object> where = new Dictionary<string, object>();
                new CreateCriteria(where).Add(ORMRestrictions<Int64>.Eq("MstPhid", mst.PhId));
                //查询相关明细数据
                var dtlList = this.GKPaymentDtlRule.Find(where, new string[] { "QtKmdm Asc" }); //PVoucherDelService.Find(where, new string[] { "SortCode Asc" }).Data.ToList();QtKmdm

                if (dtlList.Count > 0) {
                    long mstPhid = mst.RefbillPhid;
                    Dictionary<string, object> where2 = new Dictionary<string, object>();
                    new CreateCriteria(where2).Add(ORMRestrictions<Int64>.Eq("MstPhid", mstPhid));

                    //资金拨付单对应项目信息    
                    var bfXmList = this.PaymentXmRule.Find(where2);

                    //资金拨付单明细表
                    var bfDtlList = this.PaymentDtlRule.Find(where2);


                    var newCodeDt = this.GetPaymentNewCodeDataTable(mst.RefbillPhid);
                    DataRow[] drs = null;
                    Dictionary<string, string> dicNewCode = null;
                    Dictionary<string, string> dicNewCodeMstPhid = null;
                    string fNewCode = string.Empty, fNewCodeMstPhid = string.Empty;
                    string old_phid = string.Empty;

                    //给明细表赋值，写入扩展字段信息

                    ret.Mst = mst;
                    ret.Dtls = new List<GKPaymentDtl4ZjbfModel>();

                    GKPaymentDtl4ZjbfModel dtl4Zjbf = null;
                    PaymentDtlModel bfDtl = null;
                    PaymentXmModel bfXm = null;
                    IEnumerable<PaymentDtlModel> enuDtl = null;
                    IEnumerable<PaymentXmModel> enuXm = null;


                    Dictionary<string, object> dicOrg = new Dictionary<string, object>();
                    new CreateCriteria(dicOrg)
                        .Add(ORMRestrictions<long>.NotEq("PhId", 0));
                    IList<OrganizeModel> OrgList= OrganizationRule.Find2(dicOrg);
                    //补充完善明细信息
                    foreach (GKPaymentDtlModel dtl in dtlList)
                    {
                        dtl4Zjbf = CommonUtils.TransReflection<GKPaymentDtlModel, GKPaymentDtl4ZjbfModel>(dtl);

                        if (bfDtlList.Count > 0)
                        {
                            enuDtl = bfDtlList.Where(x => x.PhId == dtl.RefbillDtlPhid && x.BudgetdtlPhid == dtl.RefbillDtlPhid2);
                            if (enuDtl.Count() > 0 )
                            {
                                bfDtl = enuDtl.First();
                                if (string.IsNullOrEmpty(dtl.QtKmdm))
                                {
                                    dtl4Zjbf.QtKmdm = bfDtl.QtKmdm; //预算科目
                                    dtl4Zjbf.QtKmmc = bfDtl.QtKmmc; //预算科目名称
                                }

                                dtl4Zjbf.FDepartmentcode = bfDtl.FDepartmentcode;   //补助单位/部门代码
                                OrganizeModel Org = OrgList.ToList().Find(x => x.OCode == bfDtl.FDepartmentcode);
                                dtl4Zjbf.FDepartmentphid = Org.PhId;
                                dtl4Zjbf.FDepartmentname = bfDtl.FDepartmentname;   //补助单位/部门名称
                                if (Org.IfCorp== "N")
                                {
                                    OrganizeModel Org2 = OrgList.ToList().Find(x => x.PhId == Org.ParentOrgId);
                                    dtl4Zjbf.FDepartmentParentcode = Org2.OCode;
                                    dtl4Zjbf.FDepartmentParentphid = Org2.PhId;
                                    dtl4Zjbf.FDepartmentParentname = Org2.OName;
                                }

                                dtl4Zjbf.BudgetdtlName = bfDtl.BudgetdtlName;       //预算明细项目名称

                                //预算项目
                                enuXm = bfXmList.Where(x => x.PhId == bfDtl.PayXmPhid);
                                if (enuXm.Count() > 0)
                                {
                                    bfXm = enuXm.First();
                                    dtl4Zjbf.XmProjcode = bfXm.XmProjcode;
                                    dtl4Zjbf.XmProjname = bfXm.XmProjname;
                                    dtl4Zjbf.FSeq = bfXm.FSeq;
                                }
                            }
                        }

                        //写重新支付单的单号
                        if (newCodeDt != null && newCodeDt.Rows.Count > 0) {
                            dicNewCode = new Dictionary<string, string>();
                            dicNewCodeMstPhid = new Dictionary<string, string>();
                            dtl4Zjbf.FNewCodes = "";

                            //drs = newCodeDt.Select("ng_insert_dt>=#" + dtl.NgInsertDt.ToString("yyyy-MM-dd HH:mm:ss") + "#");
                            drs = newCodeDt.Select(string.Format("phid ={0}", dtl.PhId));
                            while (drs.Length > 0) {
                                foreach (var row in drs) {
                                    fNewCode = row["f_new_code"].ToString();
                                    if (!string.IsNullOrEmpty(fNewCode)) {
                                        if (!dicNewCode.ContainsKey(fNewCode))
                                        {
                                            dicNewCode.Add(fNewCode, fNewCode);
                                        }
                                    }
                                }
                                old_phid = drs[0]["phid"].ToString();
                                if (!string.IsNullOrEmpty(old_phid))
                                {
                                    drs = newCodeDt.Select(string.Format("old_dtl_phid={0}", old_phid));
                                    if (drs.Length > 0) {
                                        fNewCodeMstPhid = drs[0]["mst_phid"].ToString();
                                        if (!dicNewCodeMstPhid.ContainsKey( fNewCodeMstPhid))
                                        {
                                            dicNewCodeMstPhid.Add(fNewCodeMstPhid, fNewCodeMstPhid);
                                        }
                                    }
                                }
                                else {
                                    drs = newCodeDt.Select("1=2");
                                }
                            }

                            dtl4Zjbf.FNewCodes = string.Join(",", dicNewCode.Values.ToArray());
                            dtl4Zjbf.FNewCodesMstPhid = string.Join(",", dicNewCodeMstPhid.Values.ToArray());
                        }

                        ret.Dtls.Add(dtl4Zjbf);
                    }

                }
                ret.Dtls = ret.Dtls.OrderBy(t => t.FSeq).ToList();
            }


            return ret;
        }

        /// <summary>
        /// 获取资金拨付支付单列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="dicWhere"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public PagedResult<GKPayment4ZjbfModel> GetPaymentList4Zjbf(int pageIndex, int pageSize = 20, Dictionary<string, object> dicWhere = null, params string[] sorts) {

            var mstResult = base.LoadWithPage(pageIndex, pageSize, dicWhere, sorts);
            if (mstResult.Results == null)
            {
                PagedResult<GKPayment4ZjbfModel> gk = new PagedResult<GKPayment4ZjbfModel>();
                return gk;
                //return null;
            }
            else{
                //加入业务单名称
                List<long> phids = mstResult.Results.Select(t => t.RefbillPhid).ToList();
                Dictionary<string, object> dic = new Dictionary<string, object>();
                new CreateCriteria(dic).Add(ORMRestrictions<List<long>>.In("PhId", phids));
                var payments = this.PaymentMstRule.Find(dic);
                if(payments.Count > 0)
                {
                    foreach(var mstGk in mstResult.Results)
                    {
                        var mstBk = payments.ToList().Find(t => t.PhId == mstGk.RefbillPhid);
                        if (mstBk != null)
                        {
                            mstGk.RefbillName = mstBk.FName;
                        }                        
                    }
                }
            }

            //获得列表对应的明细数据 
            IList<GKPaymentMstModel> mstList = mstResult.Results;
            Dictionary<long, long> mstIds = new Dictionary<long, long>();
            Dictionary<long, long> mstBfIds = new Dictionary<long, long>();
            mstIds.Clear();
            foreach (GKPaymentMstModel mst in mstList)
            {
                //获取phid                
                if (!mstIds.ContainsKey(mst.PhId))
                {
                    mstIds.Add(mst.PhId, mst.PhId);
                }


                if (!mstBfIds.ContainsKey(mst.RefbillPhid))
                {
                    mstBfIds.Add(mst.RefbillPhid, mst.RefbillPhid);
                }
            }
                     
            string[] sorts2 = new string[] { "MstPhid Asc", "PhId Asc" };
            Dictionary<string, object> where = new Dictionary<string, object>();
            new CreateCriteria(where).Add(ORMRestrictions<List<Int64>>.In("MstPhid", mstIds.Keys.ToList()));

            IList<GKPaymentDtlModel> Dtls = this.GKPaymentDtlRule.Find(where, sorts2);

            Dictionary<string, object> where2 = new Dictionary<string, object>();
            new CreateCriteria(where2).Add(ORMRestrictions<List<Int64>>.In("MstPhid", mstBfIds.Keys.ToList()));
            IList<PaymentDtlModel> bfDtls = this.PaymentDtlRule.Find(where2, sorts2);


            //组装数据
            PagedResult<GKPayment4ZjbfModel> mstPageResult = new PagedResult<GKPayment4ZjbfModel>();                      
            
            IList<GKPayment4ZjbfModel> zjbflist = new List<GKPayment4ZjbfModel>();
            GKPayment4ZjbfModel zjbfMo = null;
            GKPaymentDtl4ZjbfModel zjbfDtlMo = null;
            List<GKPaymentDtlModel> payDtls = null;
            PaymentDtlModel bfDtl = null;
            IEnumerable<PaymentDtlModel> enuZjbfDtl = null;
            foreach (GKPaymentMstModel mst in mstList) {
                zjbfMo = new GKPayment4ZjbfModel();
                zjbfMo.Mst = mst;
                zjbfMo.Dtls = new List<GKPaymentDtl4ZjbfModel>();

                if(Dtls != null && Dtls.Count > 0)
                {
                    payDtls = Dtls.Where(x => x.MstPhid == mst.PhId).ToList();
                    if(payDtls != null && payDtls.Count > 0)
                    {
                            foreach (GKPaymentDtlModel payDtl in payDtls) {
                            zjbfDtlMo = CommonUtils.TransReflection<GKPaymentDtlModel, GKPaymentDtl4ZjbfModel>(payDtl);

                            //资金拨付支付明细赋值
                            if(bfDtls != null && bfDtls.Count > 0)
                            {
                                enuZjbfDtl = bfDtls.Where(x => x.PhId == zjbfDtlMo.RefbillDtlPhid && x.BudgetdtlPhid == zjbfDtlMo.RefbillDtlPhid2);
                                if (enuZjbfDtl != null && enuZjbfDtl.Count() > 0) {
                                    bfDtl = enuZjbfDtl.First();
                                    if (string.IsNullOrEmpty(zjbfDtlMo.QtKmdm))
                                    {
                                        zjbfDtlMo.QtKmdm = bfDtl.QtKmdm; //预算科目
                                        zjbfDtlMo.QtKmmc = bfDtl.QtKmmc; //预算科目名称
                                    }

                                    zjbfDtlMo.FDepartmentcode = bfDtl.FDepartmentcode;   //补助单位/部门代码
                                    zjbfDtlMo.FDepartmentname = bfDtl.FDepartmentname;   //补助单位/部门名称
                                    zjbfDtlMo.BudgetdtlName = bfDtl.BudgetdtlName;       //预算明细项目名称
                                }
                            }
                            zjbfMo.Dtls.Add(zjbfDtlMo);
                        }
                    }
                }

                

                zjbflist.Add(zjbfMo);
            }

            mstPageResult.Results = zjbflist;
            mstPageResult.PageIndex = mstResult.PageIndex;
            mstPageResult.PageSize = mstResult.PageSize;
            mstPageResult.TotalItems = mstResult.TotalItems;

            return mstPageResult;
        }
        /// <summary>
        /// 获取资金拨付支付单列表
        /// </summary>
        /// <param name="mstList"></param>
        /// <param name="TotalItems"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagedResult<GKPayment4ZjbfModel> GetPaymentList4Zjbf2(List<GKPaymentMstModel> mstList, int TotalItems,int pageIndex, int pageSize = 20)
        {
            if(mstList == null || mstList.Count == 0)
            {
                PagedResult<GKPayment4ZjbfModel> gk = new PagedResult<GKPayment4ZjbfModel>();
                return gk;
            }
            else
            {
                //加入业务单名称
                List<long> phids = mstList.Select(t => t.RefbillPhid).ToList();
                Dictionary<string, object> dic = new Dictionary<string, object>();
                new CreateCriteria(dic).Add(ORMRestrictions<List<long>>.In("PhId", phids));
                var payments = this.PaymentMstRule.Find(dic);
                if (payments.Count > 0)
                {
                    foreach (var mstGk in mstList)
                    {
                        var mstBk = payments.ToList().Find(t => t.PhId == mstGk.RefbillPhid);
                        if (mstBk != null)
                        {
                            mstGk.RefbillName = mstBk.FName;
                        }
                    }
                }
            }

            //获得列表对应的明细数据 
            Dictionary<long, long> mstIds = new Dictionary<long, long>();
            Dictionary<long, long> mstBfIds = new Dictionary<long, long>();
            mstIds.Clear();
            foreach (GKPaymentMstModel mst in mstList)
            {
                //获取phid                
                if (!mstIds.ContainsKey(mst.PhId))
                {
                    mstIds.Add(mst.PhId, mst.PhId);
                }


                if (!mstBfIds.ContainsKey(mst.RefbillPhid))
                {
                    mstBfIds.Add(mst.RefbillPhid, mst.RefbillPhid);
                }
            }

            string[] sorts2 = new string[] { "MstPhid Asc", "PhId Asc" };
            Dictionary<string, object> where = new Dictionary<string, object>();
            new CreateCriteria(where).Add(ORMRestrictions<List<Int64>>.In("MstPhid", mstIds.Keys.ToList()));

            IList<GKPaymentDtlModel> Dtls = this.GKPaymentDtlRule.Find(where, sorts2);

            Dictionary<string, object> where2 = new Dictionary<string, object>();
            new CreateCriteria(where2).Add(ORMRestrictions<List<Int64>>.In("MstPhid", mstBfIds.Keys.ToList()));
            IList<PaymentDtlModel> bfDtls = this.PaymentDtlRule.Find(where2, sorts2);


            //组装数据
            PagedResult<GKPayment4ZjbfModel> mstPageResult = new PagedResult<GKPayment4ZjbfModel>();

            IList<GKPayment4ZjbfModel> zjbflist = new List<GKPayment4ZjbfModel>();
            GKPayment4ZjbfModel zjbfMo = null;
            GKPaymentDtl4ZjbfModel zjbfDtlMo = null;
            List<GKPaymentDtlModel> payDtls = null;
            PaymentDtlModel bfDtl = null;
            IEnumerable<PaymentDtlModel> enuZjbfDtl = null;
            foreach (GKPaymentMstModel mst in mstList)
            {
                zjbfMo = new GKPayment4ZjbfModel();
                zjbfMo.Mst = mst;
                zjbfMo.Dtls = new List<GKPaymentDtl4ZjbfModel>();
                //加入必要的判断
                if (Dtls != null && Dtls.Count > 0)
                {
                    payDtls = Dtls.Where(x => x.MstPhid == mst.PhId).ToList();
                    if (payDtls != null && payDtls.Count > 0)
                    {
                        foreach (GKPaymentDtlModel payDtl in payDtls)
                        {
                            zjbfDtlMo = CommonUtils.TransReflection<GKPaymentDtlModel, GKPaymentDtl4ZjbfModel>(payDtl);

                            //资金拨付支付明细赋值
                            if (bfDtls != null && bfDtls.Count > 0)
                            {
                                enuZjbfDtl = bfDtls.Where(x => x.PhId == zjbfDtlMo.RefbillDtlPhid && x.BudgetdtlPhid == zjbfDtlMo.RefbillDtlPhid2);
                                if (enuZjbfDtl != null && enuZjbfDtl.Count() > 0)
                                {
                                    bfDtl = enuZjbfDtl.First();
                                    if (string.IsNullOrEmpty(zjbfDtlMo.QtKmdm))
                                    {
                                        zjbfDtlMo.QtKmdm = bfDtl.QtKmdm; //预算科目
                                        zjbfDtlMo.QtKmmc = bfDtl.QtKmmc; //预算科目名称
                                    }

                                    zjbfDtlMo.FDepartmentcode = bfDtl.FDepartmentcode;   //补助单位/部门代码
                                    zjbfDtlMo.FDepartmentname = bfDtl.FDepartmentname;   //补助单位/部门名称
                                    zjbfDtlMo.BudgetdtlName = bfDtl.BudgetdtlName;       //预算明细项目名称
                                }
                            }
                            zjbfMo.Dtls.Add(zjbfDtlMo);
                        }
                    }
                }
                #region
                //payDtls = Dtls.Where(x => x.MstPhid == mst.PhId).ToList();
                //foreach (GKPaymentDtlModel payDtl in payDtls)
                //{
                //    zjbfDtlMo = CommonUtils.TransReflection<GKPaymentDtlModel, GKPaymentDtl4ZjbfModel>(payDtl);

                //    //资金拨付支付明细赋值
                //    enuZjbfDtl = bfDtls.Where(x => x.PhId == zjbfDtlMo.RefbillDtlPhid && x.BudgetdtlPhid == zjbfDtlMo.RefbillDtlPhid2);
                //    if (enuZjbfDtl.Count() > 0)
                //    {
                //        bfDtl = enuZjbfDtl.First();
                //        if (string.IsNullOrEmpty(zjbfDtlMo.QtKmdm))
                //        {
                //            zjbfDtlMo.QtKmdm = bfDtl.QtKmdm; //预算科目
                //            zjbfDtlMo.QtKmmc = bfDtl.QtKmmc; //预算科目名称
                //        }

                //        zjbfDtlMo.FDepartmentcode = bfDtl.FDepartmentcode;   //补助单位/部门代码
                //        zjbfDtlMo.FDepartmentname = bfDtl.FDepartmentname;   //补助单位/部门名称
                //        zjbfDtlMo.BudgetdtlName = bfDtl.BudgetdtlName;       //预算明细项目名称
                //    }

                //    zjbfMo.Dtls.Add(zjbfDtlMo);
                //}
                #endregion
                zjbflist.Add(zjbfMo);
            }

            mstPageResult.Results = zjbflist;
            mstPageResult.PageIndex = pageIndex;
            mstPageResult.PageSize = pageSize;
            mstPageResult.TotalItems = TotalItems;

            return mstPageResult;
        }

        /// <summary>
        /// 更新单据支付状态
        /// </summary>
        /// <param name="phid"></param>
        /// <param name="payState"></param>
        /// <param name="submitterID"></param>
        /// <returns></returns>
        public SavedResult<long> UpdatePaymentState(Int64 phid, byte payState, Int64 submitterID)
        {
            var list = new List<PropertyColumnMapperInfo>();
            list.Add(new PropertyColumnMapperInfo
            {
                PropertyName = "PhId",
                Value = phid
            });

            list.Add(new PropertyColumnMapperInfo
            {
                PropertyName = "FState",
                Value = payState
            });

            if (submitterID>0)
            {
                list.Add(new PropertyColumnMapperInfo
                {
                    PropertyName = "FSubmitterId",
                    Value = submitterID
                });

                list.Add(new PropertyColumnMapperInfo
                {
                    PropertyName = "FSubmitdate",
                    Value = DateTime.Now
                });
            }

            var result = this.FacadeHelper.Update<long>(list);


            //更新明细表
            if (result.IsError == false)
            {
                var find = this.GKPaymentDtlRule.FindByForeignKey(phid);
                if (find!=null && find.Count>0)
                {
                    var list2 = new List<PropertyColumnMapperInfo>();                    
                    
                    Dictionary<long, long> oldMstIds = new Dictionary<long, long>();
                    Dictionary<long, long> oldDtlIds = new Dictionary<long, long>();

                    #region 更新当前明细表支付状态
                    foreach (var dtl in find)
                    {
                        #region 获得原支付单的条件，后面用来同步原支付单状态
                        if (dtl.OldMstPhid > 0) {
                            if (!oldMstIds.ContainsKey(dtl.OldMstPhid)) {
                                oldMstIds.Add(dtl.OldMstPhid, dtl.OldMstPhid);
                            }
                        }

                        if (dtl.OldDtlPhid > 0)
                        {
                            if (!oldDtlIds.ContainsKey(dtl.OldDtlPhid))
                            {
                                oldDtlIds.Add(dtl.OldDtlPhid, dtl.OldDtlPhid);
                            }
                        }
                        #endregion

                        list2.Add(new PropertyColumnMapperInfo
                        {
                            PropertyName = "PhId",
                            Value = dtl.PhId
                        });

                        list2.Add(new PropertyColumnMapperInfo
                        {
                            PropertyName = "FState",
                            Value = payState
                        });

                        //this.GKPaymentDtlFacade.FacadeHelper.Update<long>(list2);
                        this.GKPaymentDtlRule.RuleHelper.Update(list2);
                        list2.Clear();
                    }
                    #endregion

                    #region 更新老支付单的明细数据的支付状态
                    if (oldDtlIds.Count > 0)
                    {
                        this.UpdateDtlPayState(oldDtlIds.Keys.ToList(), payState); //批量更新明细表支付状态
                    }
                    #endregion

                    #region 更新老支付单的主表数据的支付状态
                    if (oldMstIds.Count > 0)
                    {
                        this.UpdateMstPayState(oldMstIds.Keys.ToList(), payState); //批量更新主表支付状态
                    }
                    #endregion

                }

                this.UpdateZjbfPaymentPayState(phid, payState); //更新资金拨付单状态
            }

            return result;
        }

        /// <summary>
        /// 通过支付单phid来更新资金拨付单状态
        /// </summary>
        /// <param name="phid"></param>
        /// <param name="payState"></param>
        public void UpdateZjbfPaymentPayState(long phid, int payState) {
            //更新对应的资金拨付单 1 异常, 9 成功
            if (payState == (int)EnumPaymentState.Paid || payState == (int)EnumPaymentState.AbnormalPayment)
            {
                var mst = this.EntRule.Find(phid);
                if (mst != null && mst.RefbillPhid > 0)
                {
                    if (payState == (int)EnumPaymentState.Paid)
                    {
                        this.PaymentMstRule.UpdatePaymentPay(mst.RefbillPhid, 9); //支付成功
                    }

                    if (payState == (int)EnumPaymentState.AbnormalPayment)
                    {
                        this.PaymentMstRule.UpdatePaymentPay(mst.RefbillPhid, 1); //支付异常
                    }

                    if (payState == (int)EnumPaymentState.DuringPayment)
                    {
                        this.PaymentMstRule.UpdatePaymentPay(mst.RefbillPhid, 2); //支付中
                    }
                    if (payState == (int)EnumPaymentState.PendingPayment)
                    {
                        this.PaymentMstRule.UpdatePaymentPay(mst.RefbillPhid, 0); //待支付
                    }
                }
            }
        }

        /// <summary>
        /// 批量更新明细表支付状态
        /// </summary>
        /// <param name="phIds"></param>
        /// <param name="payState"></param>
        /// <returns></returns>
        public SavedResult<long> UpdateDtlPayState(List<long> phIds, byte payState) {
            Dictionary<string, object> oldDtlWhere = new Dictionary<string, object>();
            new CreateCriteria(oldDtlWhere).Add(ORMRestrictions<List<long>>.In("PhId", phIds));
            var findDtl = this.GKPaymentDtlRule.Find(oldDtlWhere, new string[] { "PhId Asc" });
            if (findDtl != null && findDtl.Count > 0)
            {
                SavedResult<long>  resutlt = null;
                foreach (var dtl in findDtl)
                {
                    dtl.FState = payState;
                    dtl.PersistentState = PersistentState.Modified;
                    resutlt = this.GKPaymentDtlRule.Save<long>(dtl);
                }

                return resutlt;
            }

            return null;
        }


        /// <summary>
        /// 批量更新主表支付状态
        /// </summary>
        /// <param name="phIds"></param>
        /// <param name="payState"></param>
        /// <returns></returns>
        public SavedResult<long> UpdateMstPayState(List<long> phIds, byte payState)
        {
            Dictionary<string, object> oldMstWhere = new Dictionary<string, object>();
            new CreateCriteria(oldMstWhere).Add(ORMRestrictions<List<long>>.In("PhId", phIds));
            var findMst = this.GKPaymentMstRule.Find(oldMstWhere, new string[] { "PhId Asc" });
            if (findMst != null && findMst.Count > 0)
            {
                Dictionary<string, object> dtlsWhere = new Dictionary<string, object>();
                IList<GKPaymentDtlModel> dtls = null;
                int sameCount = 0;
                foreach (var mst in findMst)
                {
                    //查找对应的明细数据
                    sameCount = 0;
                    dtlsWhere.Clear();
                    dtlsWhere.Add("MstPhid", mst.PhId);
                    dtls = this.GKPaymentDtlRule.Find(dtlsWhere);
                    foreach (var dtl in dtls)
                    {
                        if (dtl.FState == payState)
                        {
                            sameCount++;
                        }
                    }

                    //明细表所有状态和要更新的主表状态一致，可更新主表为对应状态
                    if (sameCount == dtls.Count)
                    {
                        /*
                        mst.FState = payState;
                        mst.PersistentState = PersistentState.Modified;
                                                
                        var resutlt =  this.Save<long>(mst);
                        if (resutlt.IsError == false)
                        {
                            this.UpdateZjbfPaymentPayState(mst.PhId, payState); //更新资金拨付单状态
                            
                        }
                        */

                        var resutlt = this.UpdatePaymentState(mst.PhId, payState, 0); //更新嵌套的主表状态

                        return resutlt;
                    }                    
                }
            }

            return null;
        }


        /// <summary>
        /// 批量更新明细表支付状态
        /// </summary>
        /// <param name="phId"></param>
        /// <param name="payState"></param>
        /// <returns></returns>
        public SavedResult<long> UpdateSingleDtlPayState(long phId, byte payState)
        {
            Dictionary<string, object> oldDtlWhere = new Dictionary<string, object>();
            new CreateCriteria(oldDtlWhere).Add(ORMRestrictions<long>.Eq("PhId", phId));
            var findDtl = this.GKPaymentDtlRule.Find(oldDtlWhere, new string[] { "PhId Asc" });
            if (findDtl != null && findDtl.Count > 0)
            {
                foreach (var dtl in findDtl)
                {
                    dtl.FState = payState;
                    dtl.PersistentState = PersistentState.Modified;
                    return this.GKPaymentDtlRule.Save<long>(dtl);
                }
            }

            return null;
        }

        /// <summary>
        /// 更新支付单的审批状态
        /// </summary>
        /// <param name="relbill_phid">关联单据id</param>
        /// <param name="fApproval">审批状态</param>
        /// <returns></returns>
        public SavedResult<long> UpdatePaymentApprovalState(long relbill_phid, byte fApproval)
        {
            IList<GKPaymentMstModel> mstModels = GKPaymentMstRule.Find(t => t.PhId == relbill_phid);
            if (mstModels == null || mstModels.Count == 0)
            {
                return null;
            }

            GKPaymentMstModel mstModel = mstModels[0];
            mstModel.FApproval = fApproval;
            mstModel.PersistentState = PersistentState.Modified;

            return GKPaymentMstRule.Save<Int64>(mstModel);
        }

        /// <summary>
        /// 根据单据号集合作废单据
        /// </summary>
        /// <param name="phids">单据集合</param>
        /// <returns></returns>
        public SavedResult<long> PostCancetGkPaymentList(List<long> phids)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic).
                    Add(ORMRestrictions<List<long>>.In("PhId", phids));
            var payments = this.GKPaymentMstRule.Find(dic);
            if (payments.Count > 0)
            {
                foreach (var payment in payments)
                {
                    if (payment.FApproval != (byte)EnumApproval.NotPass && payment.FApproval != (byte)EnumApproval.PendingSend)
                    {
                        throw new Exception("只有未送审与未通过的单据可以作废！");
                    }
                    payment.FDelete = (byte)1;
                    payment.PersistentState = PersistentState.Modified;
                }
                savedResult = this.GKPaymentMstRule.Save<long>(payments);
            }
            return savedResult;
        }
        #endregion

    }
}

