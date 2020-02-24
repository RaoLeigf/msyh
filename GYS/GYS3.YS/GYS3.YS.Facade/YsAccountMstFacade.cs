#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Facade
    * 类 名 称：			YsAccountMstFacade
    * 文 件 名：			YsAccountMstFacade.cs
    * 创建时间：			2019/9/23 
    * 作    者：			王冠冠    
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

using GYS3.YS.Facade.Interface;
using GYS3.YS.Rule.Interface;
using GYS3.YS.Model.Domain;
using GQT3.QT.Model.Domain;
using GQT3.QT.Rule.Interface;
using SUP.Common.Base;
using Enterprise3.Common.Base.Criterion;
using GYS3.YS.Model.Extend;

namespace GYS3.YS.Facade
{
	/// <summary>
	/// YsAccountMst业务组装处理类
	/// </summary>
    public partial class YsAccountMstFacade : EntFacadeBase<YsAccountMstModel>, IYsAccountMstFacade
    {
		#region 类变量及属性
		/// <summary>
        /// YsAccountMst业务逻辑处理对象
        /// </summary>
		IYsAccountMstRule YsAccountMstRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IYsAccountMstRule;
            }
        }
		/// <summary>
        /// YsAccount业务逻辑处理对象
        /// </summary>
		IYsAccountRule YsAccountRule { get; set; }

        IUserRule UserRule { get; set; }

        IOrganizationRule OrganizationRule { get; set; }

        IBudgetAccountsRule BudgetAccountsRule { get; set; }

        ICorrespondenceSettingsRule CorrespondenceSettingsRule { get; set; }
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
        public override PagedResult<YsAccountMstModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<YsAccountMstModel> pageResult = base.LoadWithPage(pageNumber, pageSize, dic, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<YsAccountMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<YsAccountMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
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
        public override PagedResult<YsAccountMstModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, bool isUseInfoRight = false, params string[] sorts)
        {
            PagedResult<YsAccountMstModel> pageResult = base.LoadWithPage(pageNumber, pageSize, nameSqlName, dic, isUseInfoRight, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<YsAccountMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<YsAccountMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return pageResult;
        }

        /// <summary>
        /// 通过id，删除数据
        /// </summary>
        /// <param name="id">单主键id值</param>
        public override DeletedResult Delete<TValType>(TValType id)
        {
			YsAccountRule.RuleHelper.DeleteByForeignKey(id);
			return base.Delete(id);
        }

		/// <summary>
        /// 通过ids，删除数据
        /// </summary>
        /// <param name="ids">单主键id集合</param>
        public override DeletedResult Delete<TValType>(IList<TValType> ids)
        {
			YsAccountRule.RuleHelper.DeleteByForeignKey(ids);
			return base.Delete(ids);
        }
        #endregion

		#region 实现 IYsAccountMstFacade 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="ysAccountMstEntity"></param>
		/// <param name="ysAccountEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveYsAccountMst(YsAccountMstModel ysAccountMstEntity, List<YsAccountModel> ysAccountEntities)
        {
            SavedResult<Int64> savedResult = base.Save<Int64>(ysAccountMstEntity);
            if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
            {
				if (ysAccountEntities.Count > 0)
				{
					YsAccountRule.Save(ysAccountEntities, savedResult.KeyCodes[0]);
				}
            }

			return savedResult;
        }


        /// <summary>
        /// 保存预决算报表
        /// </summary>
        /// <param name="ysAccountMst">对象</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <param name="year">年份</param>
        /// <param name="uid">用户id</param>
        /// <param name="verify">用来判断年初，年中，年末（1、年初，2、年中，3、年末）</param>
        /// <returns></returns>
        public SavedResult<long> SaveYsAccount(YsAccountMstModel ysAccountMst, long orgId, string orgCode, string year, long uid, string verify)
        {
            //根据用户id获取用户对象
            User2Model user = new User2Model();
            if(this.UserRule.Find(t => t.PhId == uid)!= null && this.UserRule.Find(t => t.PhId == uid).Count > 0)
            {
                user = this.UserRule.Find(t => t.PhId == uid)[0];
            }
            else
            {
                throw new Exception("用户查询失败！");
            }
            //根据组织id获取组织对象
            OrganizeModel org = new OrganizeModel();
            if (this.OrganizationRule.Find(t => t.PhId == orgId) != null && this.OrganizationRule.Find(t => t.PhId == orgId).Count > 0)
            {
                org = this.OrganizationRule.Find(t => t.PhId == orgId)[0];
            }
            else
            {
                throw new Exception("组织查询失败！");
            }
            SavedResult<long> savedResult = new SavedResult<long>();
            IList<YsAccountMstModel> ysAccountMsts = new List<YsAccountMstModel>();
            ysAccountMsts = this.YsAccountMstRule.Find(t => t.Orgid == orgId && t.Uyear == year);
            if(ysAccountMsts != null && ysAccountMsts.Count > 0)
            {
                if(ysAccountMst.PhId == 0)
                {
                    throw new Exception("主表主键传递有误！");
                }
                if ("1".Equals(verify) && ysAccountMsts[0].VerifyStart == 1)
                {
                    throw new Exception("年初申报数据已上报，不能修改！");
                }
                else if ("2".Equals(verify) && ysAccountMsts[0].VerifyMiddle == 1)
                {
                    throw new Exception("年中调整数据已上报，不能修改！");
                }
                else if ("3".Equals(verify) && ysAccountMsts[0].VerifyEnd == 1)
                {
                    throw new Exception("年末决算数据已上报，不能修改！");
                }
                ysAccountMst.PersistentState = PersistentState.Modified;
                if ("1".Equals(verify) && ysAccountMst.VerifyStart == 1)
                {
                    ysAccountMst.VerifyStartTime = DateTime.Now;
                    ysAccountMst.StartReportDate = DateTime.Now;
                    ysAccountMst.StartReportMan = user.UserName;
                }
                else if ("2".Equals(verify) && ysAccountMst.VerifyMiddle == 1)
                {
                    if(ysAccountMsts[0].VerifyStart != 1)
                    {
                        throw new Exception("年初还未上报，不能进行年中的上报！");
                    }
                    ysAccountMst.VerifyMiddleTime = DateTime.Now;
                    ysAccountMst.MiddleReportDate = DateTime.Now;
                    ysAccountMst.MiddleReportMan = user.UserName;
                }
                else if ("3".Equals(verify) && ysAccountMst.VerifyEnd == 1)
                {
                    if (ysAccountMsts[0].VerifyStart != 1 || ysAccountMsts[0].VerifyMiddle != 1)
                    {
                        throw new Exception("年初或者年中还未上报，不能进行年末的上报！");
                    }
                    ysAccountMst.VerifyEndTime = DateTime.Now;
                    ysAccountMst.EndReportDate = DateTime.Now;
                    ysAccountMst.EndReportMan = user.UserName;
                }
                //保存主表数据
                savedResult = this.YsAccountMstRule.Save<long>(ysAccountMst);

                IList<YsAccountModel> ysAccounts = new List<YsAccountModel>();
                //已存在的明细数据
                ysAccounts = this.YsAccountRule.Find(t => t.PHIDMST == ysAccountMst.PhId && t.UYEAR == year && t.ORGID == org.PhId);
                if(ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
                {
                    foreach(var account in ysAccountMst.YsAccounts)
                    {
                        account.PHIDMST = savedResult.KeyCodes[0];
                        if (ysAccounts != null && ysAccounts.Count > 0)
                        {
                            if(ysAccounts.ToList().Find(t=>t.PhId == account.PhId) != null)
                            {
                                account.PersistentState = PersistentState.Modified;
                            }
                            else
                            {
                                account.PersistentState = PersistentState.Added;
                            }
                            if ("1".Equals(verify))
                            {
                                account.VERIFYSTART = ysAccountMst.VerifyStart;
                                account.VERIFYSTARTTIME = ysAccountMst.VerifyStartTime;
                            }
                            else if ("2".Equals(verify))
                            {
                                account.VERIFYMIDDLE = ysAccountMst.VerifyMiddle;
                                account.VERIFYMIDDLETIME = ysAccountMst.VerifyMiddleTime;
                            }
                            else if ("3".Equals(verify))
                            {
                                account.VERIFYEND = ysAccountMst.VerifyEnd;
                                account.VERIFYENDTIME = ysAccountMst.VerifyEndTime;
                            }
                        }
                        else
                        {
                            throw new Exception("原明细数据有误！");
                        }
                    }
                    //保存明细表数据
                    this.YsAccountRule.Save<long>(ysAccountMst.YsAccounts);
                }
                else
                {
                    throw new Exception("明细表数据不能为空！");
                }
            }
            else
            {
                ysAccountMst.PersistentState = PersistentState.Added;
                if ("1".Equals(verify) && ysAccountMst.VerifyStart == 1)
                {
                    ysAccountMst.VerifyStartTime = DateTime.Now;
                    ysAccountMst.StartReportDate = DateTime.Now;
                    ysAccountMst.StartReportMan = user.UserName;
                }
                else if ("2".Equals(verify) && ysAccountMst.VerifyMiddle == 1)
                {
                    throw new Exception("年初还未上报，不能进行年中的上报！");
                    //ysAccountMst.VerifyMiddleTime = DateTime.Now;
                    //ysAccountMst.MiddleReportDate = DateTime.Now;
                    //ysAccountMst.MiddleReportMan = user.UserName;
                }
                else if ("3".Equals(verify) && ysAccountMst.VerifyEnd == 1)
                {
                    throw new Exception("年初或者年中还未上报，不能进行年末的上报！");
                    //ysAccountMst.VerifyEndTime = DateTime.Now;
                    //ysAccountMst.EndReportDate = DateTime.Now;
                    //ysAccountMst.EndReportMan = user.UserName;
                }
                //保存主表数据
                savedResult = this.YsAccountMstRule.Save<long>(ysAccountMst);

                if (ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
                {
                    foreach (var account in ysAccountMst.YsAccounts)
                    {
                        account.PersistentState = PersistentState.Added;
                        account.PHIDMST = savedResult.KeyCodes[0];
                        if ("1".Equals(verify))
                        {
                            account.VERIFYSTART = ysAccountMst.VerifyStart;
                            account.VERIFYSTARTTIME = ysAccountMst.VerifyStartTime;
                        }
                        else if ("2".Equals(verify))
                        {
                            account.VERIFYMIDDLE = ysAccountMst.VerifyMiddle;
                            account.VERIFYMIDDLETIME = ysAccountMst.VerifyMiddleTime;
                        }
                        else if ("3".Equals(verify))
                        {
                            account.VERIFYEND = ysAccountMst.VerifyEnd;
                            account.VERIFYENDTIME = ysAccountMst.VerifyEndTime;
                        }
                    }
                    //保存明细表数据
                    this.YsAccountRule.Save<long>(ysAccountMst.YsAccounts);
                }
                else
                {
                    throw new Exception("明细表数据不能为空！");
                }
            }
            return savedResult;
        }

        /// <summary>
        /// 根据组织与年份获取各个上报组织的数量
        /// </summary>
        /// <param name="orgCode">组织code</param>
        /// <param name="year">对应年份</param>
        /// <returns></returns>
        public Dictionary<string, object> GetYsAccountNum(string orgCode, string year)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            IList<OrganizeModel> organizes = new List<OrganizeModel>();
            organizes = this.OrganizationRule.Find(t => t.OCode.StartsWith(orgCode) && t.IfCorp =="Y");
            if(organizes != null && organizes.Count > 0)
            {
                dic.Add("Sum", organizes.Count);
                IList<YsAccountMstModel> ysAccountMsts = new List<YsAccountMstModel>();
                ysAccountMsts = this.YsAccountMstRule.Find(t => t.PhId > (long)0);//预决算所有数据
                if(ysAccountMsts != null && ysAccountMsts.Count > 0)
                {
                    dic.Add("StartReport", ysAccountMsts.ToList().FindAll(t => t.Orgcode.StartsWith(orgCode) && t.VerifyStart == 1 && t.Uyear == year).Count);//年初已上报
                    dic.Add("StartNoReport", organizes.Count - ysAccountMsts.ToList().FindAll(t => t.Orgcode.StartsWith(orgCode) && t.VerifyStart == 1 && t.Uyear == year).Count);//年初未上报
                    dic.Add("MiddleReport", ysAccountMsts.ToList().FindAll(t => t.Orgcode.StartsWith(orgCode) && t.VerifyMiddle == 1 && t.Uyear == year).Count);//年中已上报
                    dic.Add("MiddleNoReport", organizes.Count - ysAccountMsts.ToList().FindAll(t => t.Orgcode.StartsWith(orgCode) && t.VerifyMiddle == 1 && t.Uyear == year).Count);//年中未上报
                    dic.Add("EndReport", ysAccountMsts.ToList().FindAll(t => t.Orgcode.StartsWith(orgCode) && t.VerifyEnd == 1 && t.Uyear == year).Count);//年末已上报
                    dic.Add("EndNoReport", organizes.Count - ysAccountMsts.ToList().FindAll(t => t.Orgcode.StartsWith(orgCode) && t.VerifyEnd == 1 && t.Uyear == year).Count);//年末未上报
                }
                else
                {
                    dic.Add("StartReport", 0);//年初已上报
                    dic.Add("StartNoReport", organizes.Count);//年初未上报
                    dic.Add("MiddleReport", 0);//年中已上报
                    dic.Add("MiddleNoReport", organizes.Count);//年中未上报
                    dic.Add("EndReport", 0);//年末已上报
                    dic.Add("EndNoReport", organizes.Count);//年末未上报
                }
                //int StartReport = 0, StartNoReport = 0, MiddleReport = 0, MiddleNoReport = 0, EndReport = 0, EndNoReport = 0;
            }
            else
            {
                dic.Add("Sum", 0);//总数
                dic.Add("StartReport", 0);//年初已上报
                dic.Add("StartNoReport", 0);//年初未上报
                dic.Add("MiddleReport", 0);//年中已上报
                dic.Add("MiddleNoReport", 0);//年中未上报
                dic.Add("EndReport", 0);//年末已上报
                dic.Add("EndNoReport", 0);//年末未上报
            }
            return dic;
        }

        /// <summary>
        /// 根据组织和年份获取该组织以及下级组织已上报所有明细数据
        /// </summary>
        /// <param name="orgCode">组织编码</param>
        /// <param name="year">年份</param>
        /// <param name="chooseOwn">是否包含本级组织（0-包含，1-不包含）</param>
        /// <param name="verify">用来筛选年初年中年末</param>
        /// <returns></returns>
        public List<YsAccountModel> GetAllYsAccountList(string orgCode, string year, int chooseOwn, string verify)
        {
            List<YsAccountModel> ysAccounts = new List<YsAccountModel>();
            IList<YsAccountMstModel> ysAccountMsts = new List<YsAccountMstModel>();
            if (chooseOwn > 0)
            {                
                if ("1".Equals(verify))
                {
                    ysAccountMsts = this.YsAccountMstRule.Find(t => t.Orgcode.StartsWith(orgCode) && t.Orgcode != orgCode && t.VerifyStart == 1 && t.Uyear == year);
                }
                else if ("2".Equals(verify))
                {
                    ysAccountMsts = this.YsAccountMstRule.Find(t => t.Orgcode.StartsWith(orgCode) && t.Orgcode != orgCode && t.VerifyMiddle == 1 && t.Uyear == year);
                }
                else if ("3".Equals(verify))
                {
                    ysAccountMsts = this.YsAccountMstRule.Find(t => t.Orgcode.StartsWith(orgCode) && t.Orgcode != orgCode && t.VerifyEnd == 1 && t.Uyear == year);
                }
            }
            else
            {
                if ("1".Equals(verify))
                {
                    ysAccountMsts = this.YsAccountMstRule.Find(t => t.Orgcode.StartsWith(orgCode) && t.VerifyStart == 1 && t.Uyear == year);
                }
                else if ("2".Equals(verify))
                {
                    ysAccountMsts = this.YsAccountMstRule.Find(t => t.Orgcode.StartsWith(orgCode) && t.VerifyMiddle == 1 && t.Uyear == year);
                }
                else if ("3".Equals(verify))
                {
                    ysAccountMsts = this.YsAccountMstRule.Find(t => t.Orgcode.StartsWith(orgCode) && t.VerifyEnd == 1 && t.Uyear == year);
                }
            }
            if(ysAccountMsts != null && ysAccountMsts.Count > 0)
            {
                var phids = ysAccountMsts.ToList().Select(t => t.PhId);
                ysAccounts = this.YsAccountRule.Find(t => phids.Contains(t.PHIDMST)).ToList();
            }
            return ysAccounts;
        }

        /// <summary>
        /// 根据组织获取该组织预算科目对应的初始预决算集合
        /// </summary>
        /// <param name="orgCode">组织Code</param>
        /// <param name="orgId">组织id</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public List<YsAccountModel> GetYsAccountsBySubject(string orgCode, long orgId, string year)
        {
            List<YsAccountModel> ysAccounts = new List<YsAccountModel>();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            //先获取预算所有的科目集合
            dic.Clear();
            new CreateCriteria(dic)
                    .Add(ORMRestrictions<long>.NotEq("PhId", (long)0));
            IList<BudgetAccountsModel> allAccounts = new List<BudgetAccountsModel>();
            allAccounts = this.BudgetAccountsRule.Find(dic);
            if (allAccounts == null || allAccounts.Count <= 0)
            {
                throw new Exception("预算科目数据为空！");
            }
            dic.Clear();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<string>.LLike("DefStr1", orgCode))
                .Add(ORMRestrictions<string>.Eq("Dylx", "02"));
            IList<CorrespondenceSettingsModel> allAccountsByOrg = new List<CorrespondenceSettingsModel>();
            allAccountsByOrg = this.CorrespondenceSettingsRule.Find(dic);
            if (allAccountsByOrg == null || allAccountsByOrg.Count <= 0)
            {
                throw new Exception("该组织及下级组织对应的预算科目数据为空！");
            }
            allAccountsByOrg = allAccountsByOrg.ToList().FindAll(t => (t.Dydm.StartsWith("4") || t.Dydm.StartsWith("5")));
            if (allAccountsByOrg != null && allAccountsByOrg.Count > 0)
            {
                foreach (var acc in allAccountsByOrg)
                {
                    YsAccountModel accountModel = new YsAccountModel();
                    accountModel.SUBJECTCODE = acc.Dydm;
                    accountModel.SUBJECTNAME = allAccounts.ToList().Find(t => t.KMDM == acc.Dydm) == null ? "" : allAccounts.ToList().Find(t => t.KMDM == acc.Dydm).KMMC;
                    accountModel.PHIDSUBJECT = allAccounts.ToList().Find(t => t.KMDM == acc.Dydm) == null ? 0 : allAccounts.ToList().Find(t => t.KMDM == acc.Dydm).PhId;
                    accountModel.ORGID = orgId;
                    accountModel.ORGCODE = orgCode;
                    accountModel.UYEAR = year;
                    if(ysAccounts.FindAll(t=>t.SUBJECTCODE == acc.Dydm).Count == 0)
                    {
                        ysAccounts.Add(accountModel);
                    }                    
                }

                ysAccounts.Add(new YsAccountModel { SUBJECTCODE = "4BNHJSR", SUBJECTNAME = "本年合计收入" , ORGID = orgId, ORGCODE = orgCode, UYEAR = year});
                ysAccounts.Add(new YsAccountModel { SUBJECTCODE = "5BNHJZC", SUBJECTNAME = "本年合计支出", ORGID = orgId, ORGCODE = orgCode, UYEAR = year });
                ysAccounts.Add(new YsAccountModel { SUBJECTCODE = "5QM1", SUBJECTNAME = "本年结余", ORGID = orgId, ORGCODE = orgCode, UYEAR = year });
                ysAccounts.Add(new YsAccountModel { SUBJECTCODE = "5QM2", SUBJECTNAME = "加：上年结余", ORGID = orgId, ORGCODE = orgCode, UYEAR = year });
                ysAccounts.Add(new YsAccountModel { SUBJECTCODE = "5QM3", SUBJECTNAME = "加：本年收回投资", ORGID = orgId, ORGCODE = orgCode, UYEAR = year });
                ysAccounts.Add(new YsAccountModel { SUBJECTCODE = "5QM4", SUBJECTNAME = "减：本年投资", ORGID = orgId, ORGCODE = orgCode, UYEAR = year });
                ysAccounts.Add(new YsAccountModel { SUBJECTCODE = "5QM5", SUBJECTNAME = "减：本年提取后备金", ORGID = orgId, ORGCODE = orgCode, UYEAR = year });
                ysAccounts.Add(new YsAccountModel { SUBJECTCODE = "5QM6", SUBJECTNAME = "期末滚存结余", ORGID = orgId, ORGCODE = orgCode, UYEAR = year });
                ysAccounts = ysAccounts.OrderBy(t => t.SUBJECTCODE).ToList();
            }        
            return ysAccounts;
        }

        /// <summary>
        /// 给所选组织的本级与下级组织打上审批记号
        /// </summary>
        /// <param name="orgCode">组织编码</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public IList<OrganizeModel> GetOrganizeVerifyList(string orgCode, string year)
        {
            IList<OrganizeModel> organizes = new List<OrganizeModel>();
            organizes = this.OrganizationRule.Find(t => t.OCode.StartsWith(orgCode) && t.IfCorp == "Y");
            if(organizes != null && organizes.Count > 0)
            {
                IList<YsAccountMstModel> allYsAccounts = this.YsAccountMstRule.Find(t => t.PhId > (long)0);
                foreach(var org in organizes)
                {
                    if(allYsAccounts != null && allYsAccounts.Count > 0)
                    {
                        org.VerifyStart = allYsAccounts.ToList().Find(t => t.Orgid == org.PhId) == null ? 0 : allYsAccounts.ToList().Find(t => t.Orgid == org.PhId).VerifyStart;
                        org.VerifyMiddle = allYsAccounts.ToList().Find(t => t.Orgid == org.PhId) == null ? 0 : allYsAccounts.ToList().Find(t => t.Orgid == org.PhId).VerifyMiddle;
                        org.VerifyEnd = allYsAccounts.ToList().Find(t => t.Orgid == org.PhId) == null ? 0 : allYsAccounts.ToList().Find(t => t.Orgid == org.PhId).VerifyEnd;
                    }
                }
            }
            return organizes;
        }
        #endregion
    }
}

