#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Facade
    * 类 名 称：			YsAccountFacade
    * 文 件 名：			YsAccountFacade.cs
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

using GYS3.YS.Rule.Interface;
using GYS3.YS.Model.Domain;
using Enterprise3.Common.Base.Criterion;
using GQT3.QT.Model.Domain;
using GQT3.QT.Rule.Interface;
using System.Data;
using NG3.Data.Service;
using GYS3.YS.Model.Extend;
using SUP.Common.Base;
using GData3.Common.Utils;
using GYS3.YS.Facade.Interface;

namespace GYS3.YS.Facade
{
	/// <summary>
	/// YsAccount业务组装处理类
	/// </summary>
    public partial class YsAccountFacade : EntFacadeBase<YsAccountModel>, IYsAccountFacade
    {
		#region 类变量及属性
		/// <summary>
        /// YsAccount业务逻辑处理对象
        /// </summary>
		IYsAccountRule YsAccountRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IYsAccountRule;
            }
        }


        ICorrespondenceSettings2Rule CorrespondenceSettings2Rule { get; set; }

        IQtOrgDygxRule QtOrgDygxRule { get; set; }

        IBudgetAccountsRule BudgetAccountsRule { get; set; }

        ICorrespondenceSettingsRule CorrespondenceSettingsRule { get; set; }

        IBudgetMstRule BudgetMstRule { get; set; }

        IBudgetDtlBudgetDtlRule BudgetDtlBudgetDtlRule { get; set; }

        IYsAccountMstRule YsAccountMstRule { get; set; }
        #endregion

        #region 重载方法
        /// <summary>
        /// 通过外关联的单主键值，获取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns>实体</returns>
        public override FindedResults<YsAccountModel> FindByForeignKey<TValType>(TValType id, params string[] sorts)
        {
            FindedResults<YsAccountModel> findedResults = base.FindByForeignKey(id, sorts);

            #region 明细Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<YsAccountModel>(findedResults.Data, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<YsAccountModel>(findedResults.Data, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return findedResults;
        }



        #endregion

        #region 实现 IYsAccountFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<YsAccountModel> ExampleMethod<YsAccountModel>(string param)
        //{
        //    //编写代码
        //}


        /// <summary>
        /// 根据组织获取该组织的上年与本年决算
        /// </summary>
        /// <param name="orgId">组织ID</param>
        /// <param name="orgCode">组织Code</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public IList<YsAccountModel> GetYsAccounts(string orgId, string orgCode, string year)
        {
            string userConn = "", select_DM = "", lastUserConn = "";
            string newOrgCode = "", select_KM = "";
            //是否已存在本年数据
            IList<YsAccountModel> ysAccounts = new List<YsAccountModel>();
            //新的返回数据
            IList<YsAccountModel> newYsAccounts = new List<YsAccountModel>();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Clear();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<string>.Eq("ORGCODE", orgCode))
                .Add(ORMRestrictions<string>.Eq("UYEAR", year));
            ysAccounts = this.YsAccountRule.Find(dic);
            dic.Clear();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<string>.Eq("Xmorg", orgCode));
            IList<QtOrgDygxModel> qtOrgDygxes = new List<QtOrgDygxModel>();
            qtOrgDygxes = this.QtOrgDygxRule.Find(dic);
            if (qtOrgDygxes != null && qtOrgDygxes.Count > 0)
            {
                newOrgCode = qtOrgDygxes[0].Oldorg;
            }
            else
            {
                throw new Exception("请完善组织对应关系！");
            }
            dic.Clear();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<string>.Eq("Dylx", "config"))
                .Add(ORMRestrictions<string>.Eq("DefStr1", orgCode));
            IList<CorrespondenceSettings2Model> CorrespondenceSettings2s = CorrespondenceSettings2Rule.RuleHelper.Find(dic);
            if (CorrespondenceSettings2s.Count > 0 && !string.IsNullOrEmpty(CorrespondenceSettings2s[0].DefStr2))
            {
                userConn = CorrespondenceSettings2s[0].DefStr2;
                lastUserConn = GetNewConn(userConn);
            }
            else
            {
                throw new Exception("未进行业务数据库的配置");
            }
            if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
            {
                select_DM = @"SELECT KMDM SubjectCode, sum(nvl(j,0))-sum(nvl(d,0)) SubjectAccount, sum(nvl(j,0)) SubjectJ,sum(nvl(d,0)) SubjectD 
                            FROM V_ZW_PZHZ
                            WHERE ORGCODE like '{0}%'
                            GROUP BY KMDM
                            ORDER BY KMDM";
                select_KM = @"SELECT kmdm SubjectCode,kmmc SubjectName, 0 SubjectAccount, 0 SubjectJ,0 SubjectD FROM km
                             WHERE ORGCODE = '{0}'
                             ORDER BY KMDM";
            }
            else
            {
                select_DM = @"SELECT KMDM SubjectCode, sum(isnull(j,0))-sum(isnull(d,0)) SubjectAccount, sum(isnull(j,0)) SubjectJ,sum(isnull(d,0)) SubjectD
                            FROM V_ZW_PZHZ
                            WHERE ORGCODE like '{0}%'
                            GROUP BY KMDM
                            ORDER BY KMDM";
                select_KM = @"SELECT kmdm SubjectCode,kmmc SubjectName, 0 SubjectAccount, 0 SubjectJ,0 SubjectD  FROM km
                             WHERE ORGCODE = '{0}'
                             ORDER BY KMDM";
            }

            DataTable dataTable1 = null, dataTable2 = null, dataTable3 = null;
            //DbHelper.Open(userConn);
            //DbHelper.Open(lastUserConn);
            //dataTable1 = DbHelper.GetDataTable(userConn, string.Format(select_DM, newOrgCode));
            //dataTable2 = DbHelper.GetDataTable(lastUserConn, string.Format(select_DM, newOrgCode));
            //dataTable3 = DbHelper.GetDataTable(userConn, string.Format(select_KM, newOrgCode));
            //DbHelper.Close(userConn);
            //DbHelper.Close(lastUserConn);
            //本年的金额明细
            IList<SubjectAccountModel> subjectAccounts1 = new List<SubjectAccountModel>();
            //上年的明细
            IList<SubjectAccountModel> subjectAccounts2 = new List<SubjectAccountModel>();
            //本年的科目
            IList<SubjectAccountModel> subjectAccounts3 = new List<SubjectAccountModel>();

            try
            {
                DbHelper.Open(userConn);
                dataTable1 = DbHelper.GetDataTable(userConn, string.Format(select_DM, newOrgCode));
                //dataTable2 = DbHelper.GetDataTable(lastUserConn, string.Format(select_DM, newOrgCode));
                dataTable3 = DbHelper.GetDataTable(userConn, string.Format(select_KM, newOrgCode));

                subjectAccounts1 = DCHelper.DataTable2List<SubjectAccountModel>(dataTable1);
                //subjectAccounts1 = ModelConverHelp<SubjectAccountModel>.DataTableToModel(dataTable1);
                //subjectAccounts2 = ModelConverHelp<SubjectAccountModel>.DataTableToModel(dataTable2);
                //subjectAccounts3 = ModelConverHelp<SubjectAccountModel>.DataTableToModel(dataTable3);
                subjectAccounts3 = DCHelper.DataTable2List<SubjectAccountModel>(dataTable3);
                DbHelper.Close(userConn);
                DbHelper.Close(userConn);
            }
            catch (Exception ex)
            {

            }
            try
            {
                DbHelper.Open(lastUserConn);
                dataTable2 = DbHelper.GetDataTable(lastUserConn, string.Format(select_DM, newOrgCode));
                //subjectAccounts2 = ModelConverHelp<SubjectAccountModel>.DataTableToModel(dataTable2);
                subjectAccounts2 = DCHelper.DataTable2List<SubjectAccountModel>(dataTable2);
                DbHelper.Close(lastUserConn);
            }
            catch (Exception ex)
            {

            }
            //取预算对应科目
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
                .Add(ORMRestrictions<string>.Eq("DefStr1", orgCode))
                .Add(ORMRestrictions<string>.Eq("Dylx", "02"));
            IList<CorrespondenceSettingsModel> allAccountsByOrg = new List<CorrespondenceSettingsModel>();
            allAccountsByOrg = this.CorrespondenceSettingsRule.Find(dic);
            if (allAccountsByOrg == null || allAccountsByOrg.Count <= 0)
            {
                throw new Exception("该组织对应的预算科目数据为空！");
            }
            IList<SubjectAccountModel> newSubjectAccounts = new List<SubjectAccountModel>();
            foreach (var acc in allAccountsByOrg)
            {
                SubjectAccountModel accountModel = new SubjectAccountModel();
                accountModel.SubjectCode = acc.Dydm;
                accountModel.SubjectName = allAccounts.ToList().Find(t => t.KMDM == acc.Dydm) == null ? "" : allAccounts.ToList().Find(t => t.KMDM == acc.Dydm).KMMC;
                accountModel.SubjectAccount = 0;
                accountModel.SubjectD = 0;
                accountModel.SubjectJ = 0;
                newSubjectAccounts.Add(accountModel);
            }
            //subjectAccounts3 = newSubjectAccounts;


            if (newSubjectAccounts != null && newSubjectAccounts.Count > 0)
            {
                IList<SubjectAccountModel> subject1 = newSubjectAccounts.ToList().FindAll(t => (t.SubjectCode.StartsWith("4") || t.SubjectCode.StartsWith("5"))).OrderBy(t => t.SubjectCode).ToList();
                SubjectAccountModel subject4 = new SubjectAccountModel();  //表示本年合计收入
                SubjectAccountModel subject5 = new SubjectAccountModel();  //表示本年合计支出
                SubjectAccountModel subject6 = new SubjectAccountModel();  //表示本年结余
                SubjectAccountModel subject7 = new SubjectAccountModel();  //表示上年结余
                SubjectAccountModel subject8 = new SubjectAccountModel(); //表示本年收回投资
                SubjectAccountModel subject9 = new SubjectAccountModel(); //表示本年投资
                SubjectAccountModel subject10 = new SubjectAccountModel(); // 表示提取后备金
                SubjectAccountModel subject11 = new SubjectAccountModel(); //表示期末滚存结余
                subject4.SubjectCode = "4BNHJSR"; subject4.SubjectName = "本年合计收入"; subject4.SubjectAccount = 0; subject4.SubjectJ = 0; subject4.SubjectD = 0;
                subject5.SubjectCode = "5BNHJZC"; subject5.SubjectName = "本年合计支出"; subject5.SubjectAccount = 0; subject5.SubjectJ = 0; subject5.SubjectD = 0;
                subject6.SubjectCode = "5QM1"; subject6.SubjectName = "本年结余"; subject6.SubjectAccount = 0; subject6.SubjectJ = 0; subject6.SubjectD = 0;
                subject7.SubjectCode = "5QM2"; subject7.SubjectName = "加：上年结余"; subject7.SubjectAccount = 0; subject7.SubjectJ = 0; subject7.SubjectD = 0;
                subject8.SubjectCode = "5QM3"; subject8.SubjectName = "加：本年收回投资"; subject8.SubjectAccount = 0; subject8.SubjectJ = 0; subject8.SubjectD = 0;
                subject9.SubjectCode = "5QM4"; subject9.SubjectName = "减：本年投资"; subject9.SubjectAccount = 0; subject9.SubjectJ = 0; subject9.SubjectD = 0;
                subject10.SubjectCode = "5QM5"; subject10.SubjectName = "减：本年提取后备金"; subject10.SubjectAccount = 0; subject10.SubjectJ = 0; subject10.SubjectD = 0;
                subject11.SubjectCode = "5QM6"; subject11.SubjectName = "期末滚存结余"; subject11.SubjectAccount = 0; subject11.SubjectJ = 0; subject11.SubjectD = 0;

                if (subject1.Count > 0)
                {
                    subject1.Add(subject4);
                    subject1.Add(subject5);
                    subject1.Add(subject6);
                    subject1.Add(subject7);
                    subject1.Add(subject8);
                    subject1.Add(subject9);
                    subject1.Add(subject10);
                    subject1.Add(subject11);

                    foreach (var sub in subject1)
                    {
                        YsAccountModel ysAccount = new YsAccountModel();
                        //数据库是否存在该科目的记录
                        if (ysAccounts.ToList().Find(t => t.SUBJECTCODE == sub.SubjectCode) != null)
                        {
                            ysAccount = ysAccounts.ToList().Find(t => t.SUBJECTCODE == sub.SubjectCode);
                            ysAccount.SUBJECTNAME = sub.SubjectName;
                            ysAccount.FINALACCOUNTSTOTAL = subjectAccounts2.ToList().FindAll(t => t.SubjectCode.StartsWith(sub.SubjectCode)) == null ? 0 : subjectAccounts2.ToList().FindAll(t => t.SubjectCode.StartsWith(sub.SubjectCode)).Sum(t => t.SubjectAccount);
                            ysAccount.ThisaccountsTotal = subjectAccounts1.ToList().FindAll(t => t.SubjectCode.StartsWith(sub.SubjectCode)) == null ? 0 : subjectAccounts1.ToList().FindAll(t => t.SubjectCode.StartsWith(sub.SubjectCode)).Sum(t => t.SubjectAccount);
                        }
                        else
                        {
                            ysAccount.ORGID = long.Parse(orgId);
                            ysAccount.ORGCODE = orgCode;
                            ysAccount.UYEAR = year;
                            ysAccount.SUBJECTCODE = sub.SubjectCode;
                            ysAccount.SUBJECTNAME = sub.SubjectName;
                            ysAccount.FINALACCOUNTSTOTAL = subjectAccounts2.ToList().FindAll(t => t.SubjectCode.StartsWith(sub.SubjectCode)) == null ? 0 : subjectAccounts2.ToList().FindAll(t => t.SubjectCode.StartsWith(sub.SubjectCode)).Sum(t => t.SubjectAccount);
                            ysAccount.ThisaccountsTotal = subjectAccounts1.ToList().FindAll(t => t.SubjectCode.StartsWith(sub.SubjectCode)) == null ? 0 : subjectAccounts1.ToList().FindAll(t => t.SubjectCode.StartsWith(sub.SubjectCode)).Sum(t => t.SubjectAccount);
                        }
                        if (sub.SubjectCode == "4BNHJSR")
                        {
                            ysAccount.FINALACCOUNTSTOTAL = subjectAccounts2.ToList().FindAll(t => t.SubjectCode.StartsWith("4")) == null ? 0 : subjectAccounts2.ToList().FindAll(t => t.SubjectCode.StartsWith("4")).Sum(t => t.SubjectAccount);
                            ysAccount.ThisaccountsTotal = subjectAccounts1.ToList().FindAll(t => t.SubjectCode.StartsWith("4")) == null ? 0 : subjectAccounts1.ToList().FindAll(t => t.SubjectCode.StartsWith("4")).Sum(t => t.SubjectAccount);
                        }
                        if (sub.SubjectCode == "5BNHJZC")
                        {
                            ysAccount.FINALACCOUNTSTOTAL = subjectAccounts2.ToList().FindAll(t => t.SubjectCode.StartsWith("5")) == null ? 0 : subjectAccounts2.ToList().FindAll(t => t.SubjectCode.StartsWith("5")).Sum(t => t.SubjectAccount);
                            ysAccount.ThisaccountsTotal = subjectAccounts1.ToList().FindAll(t => t.SubjectCode.StartsWith("5")) == null ? 0 : subjectAccounts1.ToList().FindAll(t => t.SubjectCode.StartsWith("5")).Sum(t => t.SubjectAccount);
                        }
                        if (sub.SubjectCode == "5QM1")
                        {
                            ysAccount.FINALACCOUNTSTOTAL = (newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").FINALACCOUNTSTOTAL) + (newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").FINALACCOUNTSTOTAL);
                            ysAccount.ThisaccountsTotal = (newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").ThisaccountsTotal) + (newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").ThisaccountsTotal);
                        }
                        if (sub.SubjectCode == "5QM2")
                        {
                            ysAccount.FINALACCOUNTSTOTAL = subjectAccounts2.ToList().FindAll(t => t.SubjectCode.StartsWith("331")) == null ? 0 : subjectAccounts2.ToList().FindAll(t => t.SubjectCode.StartsWith("331")).Sum(t => t.SubjectAccount);
                            ysAccount.ThisaccountsTotal = subjectAccounts1.ToList().FindAll(t => t.SubjectCode.StartsWith("331")) == null ? 0 : subjectAccounts1.ToList().FindAll(t => t.SubjectCode.StartsWith("331")).Sum(t => t.SubjectAccount);
                        }
                        if (sub.SubjectCode == "5QM3")
                        {
                            ysAccount.FINALACCOUNTSTOTAL = subjectAccounts2.ToList().FindAll(t => t.SubjectCode.StartsWith("311")) == null ? 0 : subjectAccounts2.ToList().FindAll(t => t.SubjectCode.StartsWith("311")).Sum(t => t.SubjectJ);
                            ysAccount.ThisaccountsTotal = subjectAccounts1.ToList().FindAll(t => t.SubjectCode.StartsWith("311")) == null ? 0 : subjectAccounts1.ToList().FindAll(t => t.SubjectCode.StartsWith("311")).Sum(t => t.SubjectJ);
                        }
                        if (sub.SubjectCode == "5QM4")
                        {
                            ysAccount.FINALACCOUNTSTOTAL = subjectAccounts2.ToList().FindAll(t => t.SubjectCode.StartsWith("311")) == null ? 0 : subjectAccounts2.ToList().FindAll(t => t.SubjectCode.StartsWith("311")).Sum(t => t.SubjectD);
                            ysAccount.ThisaccountsTotal = subjectAccounts1.ToList().FindAll(t => t.SubjectCode.StartsWith("311")) == null ? 0 : subjectAccounts1.ToList().FindAll(t => t.SubjectCode.StartsWith("311")).Sum(t => t.SubjectD);
                        }
                        if (sub.SubjectCode == "5QM5")
                        {
                            ysAccount.FINALACCOUNTSTOTAL = 0;
                            ysAccount.ThisaccountsTotal = 0;
                        }
                        if (sub.SubjectCode == "5QM6")
                        {
                            ysAccount.FINALACCOUNTSTOTAL = (newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").FINALACCOUNTSTOTAL) + (newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").FINALACCOUNTSTOTAL)
                                    + (newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").FINALACCOUNTSTOTAL) - (newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").FINALACCOUNTSTOTAL)
                                    - (newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").FINALACCOUNTSTOTAL);
                            ysAccount.ThisaccountsTotal = (newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").ThisaccountsTotal) + (newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").ThisaccountsTotal)
                                    + (newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").ThisaccountsTotal) - (newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").ThisaccountsTotal)
                                    - (newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : newYsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").ThisaccountsTotal);
                        }
                        newYsAccounts.Add(ysAccount);
                    }
                }
            }
            else
            {
                throw new Exception("该组织对应的预算科目数据为空！");
            }
            //ysAccounts = ModelConverHelp<YsAccountModel>.DataTableToModel(dataTable1);
            return newYsAccounts;
        }

        /// <summary>
        /// 根据今年账套的数据库连接串，获取上年账套的数据库连接串
        /// </summary>
        /// <param name="oldConn">今年账套的数据库连接串</param>
        /// <returns></returns>
        public string GetNewConn(string oldConn)
        {
            string newConn = "";
            int begin = 0;
            if (!string.IsNullOrEmpty(oldConn))
            {
                if (oldConn.IndexOf("User ID=", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    begin = oldConn.IndexOf("User ID=", StringComparison.OrdinalIgnoreCase);
                    string conn1 = oldConn.Substring(begin);
                    string conn2 = oldConn.Remove(begin);
                    if (conn1.IndexOf("=", StringComparison.OrdinalIgnoreCase) >= 0 && conn1.IndexOf(";", StringComparison.OrdinalIgnoreCase) >= 0 && conn1.IndexOf(";", StringComparison.OrdinalIgnoreCase) > conn1.IndexOf("=", StringComparison.OrdinalIgnoreCase))
                    {
                        string conn3 = conn1.Substring(conn1.IndexOf("=", StringComparison.OrdinalIgnoreCase) + 1).Substring(0, conn1.IndexOf(";", StringComparison.OrdinalIgnoreCase) - conn1.IndexOf("=", StringComparison.OrdinalIgnoreCase) - 1);
                        if (conn3.Length > 4)
                        {
                            string conn4 = conn3.Substring(conn3.Length - 4);
                            string conn5 = conn3.Remove(conn3.Length - 4);
                            string conn6 = conn5 + (long.Parse(conn4) - 1).ToString();
                            string conn7 = "User ID=" + conn6 + ";Password=" + conn6 + ";";
                            newConn = conn2 + conn7;
                        }
                    }
                }
            }
            return newConn;
        }


        /// <summary>
        /// 修改预决算数据
        /// </summary>
        /// <param name="ysAccounts">列表集合</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public SavedResult<long> SaveAccountList(List<YsAccountModel> ysAccounts, string orgId, string orgCode, string year)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            if (ysAccounts != null && ysAccounts.Count > 0)
            {
                foreach (var account in ysAccounts)
                {
                    if (account.PhId == 0)
                    {
                        account.PersistentState = PersistentState.Added;
                    }
                    else
                    {
                        account.PersistentState = PersistentState.Modified;
                    }
                }
                savedResult = this.YsAccountRule.Save<long>(ysAccounts);
            }
            return savedResult;
        }
        #endregion

        #region//与年中，年末决算相关

        /// <summary>
        /// 得到年初上报的数据（单个组织）
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public YsAccountMstModel GetBegineAccounts(long orgId, string orgCode, string year)
        {
            //取本年预算的所有明细数据
            IList<BudgetMstModel> budgetMsts = new List<BudgetMstModel>();//用来存主表数据
            IList<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtls = new List<BudgetDtlBudgetDtlModel>();//用来存明细数据
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic).Add(ORMRestrictions<string>.Eq("FYear", year))
               .Add(ORMRestrictions<string>.Eq("FDeclarationUnit", orgCode))
               .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
               .Add(ORMRestrictions<string>.Eq("FApproveStatus", "3"))
               .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));
            budgetMsts = this.BudgetMstRule.Find(dic);
            if (budgetMsts != null && budgetMsts.Count > 0)
            {
                var phids = budgetMsts.Select(t => t.PhId).Distinct().ToList();
                if (phids != null && phids.Count > 0)
                {
                    dic.Clear();
                    new CreateCriteria(dic).Add(ORMRestrictions<List<long>>.In("MstPhid", phids));
                    budgetDtlBudgetDtls = this.BudgetDtlBudgetDtlRule.Find(dic);
                }
            }
            

            //先判断该组织年中数据是否上报（若上报直接取数据库存好的数据）
            YsAccountMstModel ysAccountMst = new YsAccountMstModel();
            IList<YsAccountMstModel> ysAccountMsts = new List<YsAccountMstModel>();
            ysAccountMsts = this.YsAccountMstRule.Find(t => t.Orgid == orgId && t.Uyear == year);
            //已经上报过
            if (ysAccountMsts != null && ysAccountMsts.Count > 0)
            {
                ysAccountMst = ysAccountMsts[0];
                IList<YsAccountModel> ysAccounts = new List<YsAccountModel>();
                ysAccounts = this.YsAccountRule.Find(t => t.PHIDMST == ysAccountMst.PhId && t.ORGID == orgId && t.UYEAR == year);
                ysAccountMst.YsAccounts = ysAccounts;
                //年初已经上报
                if (ysAccountMst.VerifyStart > 0)
                {

                    //IList<YsAccountModel> ysAccounts = new List<YsAccountModel>();
                    //ysAccounts = this.YsAccountRule.Find(t => t.PHIDMST == ysAccountMst.PhId && t.ORGID == orgId && t.UYEAR == year);
                    //ysAccountMst.YsAccounts = ysAccounts;
                }
                else
                {
                    //年初也没上报(只要有数据，那么表示年初一定是上报过了，所以预算数直接去数据库的数据)
                    if (ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
                    {
                        foreach (var ysAcc in ysAccountMst.YsAccounts)
                        {
                            if (ysAccountMst.SaveStart > 0 && ysAcc.SUBJECTCODE.StartsWith("4"))
                            {
                                continue;
                            }
                            else
                            {
                                if (budgetDtlBudgetDtls != null && budgetDtlBudgetDtls.Count > 0)
                                {
                                
                                    ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)) == null?0: budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)).Sum(t => t.FAmount);
                                    //ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts == ysAcc.SUBJECTCODE).Sum(t => t.FAmountEdit);
                                    //ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts == ysAcc.SUBJECTCODE).Sum(t => t.FAmountAfterEdit);
                                    if (ysAcc.SUBJECTCODE == "4BNHJSR")
                                    {
                                        ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmount);
                                        //ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmountEdit);
                                        //ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmountAfterEdit);
                                        //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")));
                                    }
                                    if (ysAcc.SUBJECTCODE == "5BNHJZC")
                                    {
                                        ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmount);
                                        //ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmountEdit);
                                        //ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmountAfterEdit);
                                        //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")));
                                    }
                                    if (ysAcc.SUBJECTCODE == "5QM1")
                                    {
                                        ysAcc.BUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").BUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").BUDGETTOTAL);
                                        //ysAcc.ADJUSTMENT = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").ADJUSTMENT) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").ADJUSTMENT);
                                        //ysAcc.APPROVEDBUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").APPROVEDBUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").APPROVEDBUDGETTOTAL);
                                    }
                                    if (ysAcc.SUBJECTCODE == "5QM2")
                                    {

                                        ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmount);
                                        //ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountEdit);
                                        //ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountAfterEdit);
                                        //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")));
                                    }
                                    if (ysAcc.SUBJECTCODE == "5QM3")
                                    {
                                        ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("311")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("311")).Sum(t => t.FAmount);
                                        //ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("311")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("311")).Sum(t => t.FAmountEdit);
                                        //ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountAfterEdit);
                                        //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")));
                                    }
                                    if (ysAcc.SUBJECTCODE == "5QM6")
                                    {
                                        ysAcc.BUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").BUDGETTOTAL) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").BUDGETTOTAL)
                                                + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").BUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").BUDGETTOTAL)
                                                - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").BUDGETTOTAL);
                                        //ysAcc.ADJUSTMENT = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").ADJUSTMENT) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").ADJUSTMENT)
                                        //        + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").ADJUSTMENT) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").ADJUSTMENT)
                                        //        - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").ADJUSTMENT);
                                        //ysAcc.APPROVEDBUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").APPROVEDBUDGETTOTAL) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").APPROVEDBUDGETTOTAL)
                                        //        + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").APPROVEDBUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").APPROVEDBUDGETTOTAL)
                                        //        - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").APPROVEDBUDGETTOTAL);
                                    }
                                }
                                else
                                {
                                    ysAcc.BUDGETTOTAL = 0;
                                    //ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts == ysAcc.SUBJECTCODE).Sum(t => t.FAmount);
                                    //ysAcc.ADJUSTMENT = 0;
                                    //ysAcc.APPROVEDBUDGETTOTAL = 0;
                                }
                            }
                            
                        }
                    }
                }

                if (ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
                {
                    foreach (var ysA in ysAccountMst.YsAccounts)
                    {
                        if (ysA.BUDGETTOTAL == 0)
                        {
                            ysA.BudgetComplete = 100;
                        }
                        else
                        {
                            ysA.BudgetComplete = ysA.APPROVEDBUDGETTOTAL / ysA.BUDGETTOTAL * 100;
                        }
                    }
                }
                
            }
            else
            {
                IList<YsAccountModel> ysAccounts = new List<YsAccountModel>();
                ysAccounts = GetYsAccounts(orgId.ToString(), orgCode, year);
                ysAccountMst.Orgid = orgId;
                ysAccountMst.Orgcode = orgCode;
                ysAccountMst.Uyear = year;
                ysAccountMst.YsAccounts = ysAccounts;
                //年中,年初，年末都没上报
                if (ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
                {
                    foreach (var ysAcc in ysAccountMst.YsAccounts)
                    {
                        if (budgetDtlBudgetDtls != null && budgetDtlBudgetDtls.Count > 0)
                        {
                            ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)).Sum(t => t.FAmount);
                            ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)).Sum(t => t.FAmountEdit);
                            ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)).Sum(t => t.FAmountAfterEdit);
                            if (ysAcc.SUBJECTCODE == "4BNHJSR")
                            {
                                ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmount);
                                ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmountEdit);
                                ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmountAfterEdit);
                                //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")));
                            }
                            if (ysAcc.SUBJECTCODE == "5BNHJZC")
                            {
                                ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmount);
                                ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmountEdit);
                                ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmountAfterEdit);
                                //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")));
                            }
                            if (ysAcc.SUBJECTCODE == "5QM1")
                            {
                                ysAcc.BUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").BUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").BUDGETTOTAL);
                                ysAcc.ADJUSTMENT = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").ADJUSTMENT) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").ADJUSTMENT);
                                ysAcc.APPROVEDBUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").APPROVEDBUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").APPROVEDBUDGETTOTAL);
                            }
                            if (ysAcc.SUBJECTCODE == "5QM2")
                            {

                                ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmount);
                                ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountEdit);
                                ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountAfterEdit);
                                //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")));
                            }
                            if (ysAcc.SUBJECTCODE == "5QM3")
                            {
                                ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("311")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("311")).Sum(t => t.FAmount);
                                ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("311")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("311")).Sum(t => t.FAmountEdit);
                                ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountAfterEdit);
                                //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")));
                            }
                            if (ysAcc.SUBJECTCODE == "5QM6")
                            {
                                ysAcc.BUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").BUDGETTOTAL) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").BUDGETTOTAL)
                                        + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").BUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").BUDGETTOTAL)
                                        - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").BUDGETTOTAL);
                                ysAcc.ADJUSTMENT = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").ADJUSTMENT) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").ADJUSTMENT)
                                        + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").ADJUSTMENT) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").ADJUSTMENT)
                                        - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").ADJUSTMENT);
                                ysAcc.APPROVEDBUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").APPROVEDBUDGETTOTAL) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").APPROVEDBUDGETTOTAL)
                                        + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").APPROVEDBUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").APPROVEDBUDGETTOTAL)
                                        - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").APPROVEDBUDGETTOTAL);
                            }
                        }
                        else
                        {
                            ysAcc.BUDGETTOTAL = 0;
                            ysAcc.ADJUSTMENT = 0;
                            ysAcc.APPROVEDBUDGETTOTAL = ysAcc.BUDGETTOTAL;
                        }
                    }
                }

                if (ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
                {
                    foreach (var ysA in ysAccountMst.YsAccounts)
                    {
                        if (ysA.BUDGETTOTAL == 0)
                        {
                            ysA.BudgetComplete = 100;
                        }
                        else
                        {
                            ysA.BudgetComplete = ysA.APPROVEDBUDGETTOTAL / ysA.BUDGETTOTAL * 100;
                        }
                    }
                }
            }

            ysAccountMst.YsAccounts = GetSubjectNameForYs(ysAccountMst.YsAccounts);
            ysAccountMst.YsAccounts = ysAccountMst.YsAccounts.OrderBy(t => t.SUBJECTCODE).ToList();
            return ysAccountMst;
        }


        /// <summary>
        /// 得到年中上报的数据（单个组织）
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public YsAccountMstModel GetMiddleAccounts(long orgId, string orgCode, string year)
        {
            //取本年预算的所有明细数据
            IList<BudgetMstModel> budgetMsts = new List<BudgetMstModel>();//用来存主表数据
            IList<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtls = new List<BudgetDtlBudgetDtlModel>();//用来存明细数据
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic).Add(ORMRestrictions<string>.Eq("FYear", year))
               .Add(ORMRestrictions<string>.Eq("FDeclarationUnit", orgCode))
               .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
               .Add(ORMRestrictions<string>.Eq("FApproveStatus", "3"))
               .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));
            budgetMsts = this.BudgetMstRule.Find(dic);
            if (budgetMsts != null && budgetMsts.Count > 0)
            {
                var phids = budgetMsts.Select(t => t.PhId).Distinct().ToList();
                if (phids != null && phids.Count > 0)
                {
                    dic.Clear();
                    new CreateCriteria(dic).Add(ORMRestrictions<List<long>>.In("MstPhid", phids));
                    budgetDtlBudgetDtls = this.BudgetDtlBudgetDtlRule.Find(dic);
                }
            }
            ////先获取预算所有的科目集合
            //dic.Clear();
            //new CreateCriteria(dic)
            //        .Add(ORMRestrictions<long>.NotEq("PhId", (long)0));
            //IList<BudgetAccountsModel> allAccounts = new List<BudgetAccountsModel>();
            //allAccounts = this.BudgetAccountsRule.Find(dic);
            //if (allAccounts == null || allAccounts.Count <= 0)
            //{
            //    throw new Exception("预算科目数据为空！");
            //}
            //dic.Clear();
            //new CreateCriteria(dic)
            //    .Add(ORMRestrictions<string>.Eq("DefStr1", orgCode))
            //    .Add(ORMRestrictions<string>.Eq("Dylx", "02"));
            //IList<CorrespondenceSettingsModel> allAccountsByOrg = new List<CorrespondenceSettingsModel>();
            //allAccountsByOrg = this.CorrespondenceSettingsRule.Find(dic);
            //if (allAccountsByOrg == null || allAccountsByOrg.Count <= 0)
            //{
            //    throw new Exception("该组织对应的预算科目数据为空！");
            //}

            //先判断该组织年中数据是否上报（若上报直接取数据库存好的数据）
            YsAccountMstModel ysAccountMst = new YsAccountMstModel();
            IList<YsAccountMstModel> ysAccountMsts = new List<YsAccountMstModel>();
            ysAccountMsts = this.YsAccountMstRule.Find(t => t.Orgid == orgId && t.Uyear == year);
            //已经上报过
            if (ysAccountMsts != null && ysAccountMsts.Count > 0)
            {
                ysAccountMst = ysAccountMsts[0];
                IList<YsAccountModel> ysAccounts = new List<YsAccountModel>();
                ysAccounts = this.YsAccountRule.Find(t => t.PHIDMST == ysAccountMst.PhId && t.ORGID == orgId && t.UYEAR == year);
                ysAccountMst.YsAccounts = ysAccounts;
                //年中已经上报
                if (ysAccountMst.VerifyMiddle > 0)
                {
                    
                    //IList<YsAccountModel> ysAccounts = new List<YsAccountModel>();
                    //ysAccounts = this.YsAccountRule.Find(t => t.PHIDMST == ysAccountMst.PhId && t.ORGID == orgId && t.UYEAR == year);
                    //ysAccountMst.YsAccounts = ysAccounts;
                }
                else
                {
                    if(ysAccountMst.VerifyStart > 0)
                    {
                        //年中没上报，年初已上报(只要有数据，那么表示年初一定是上报过了，所以预算数直接去数据库的数据)
                        if(ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
                        {
                            foreach (var ysAcc in ysAccountMst.YsAccounts)
                            {
                                if (ysAccountMst.SaveMiddle > 0 && ysAcc.SUBJECTCODE.StartsWith("4"))
                                {
                                    continue;
                                }
                                if (budgetDtlBudgetDtls != null && budgetDtlBudgetDtls.Count > 0)
                                {
                                    //ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts == ysAcc.SUBJECTCODE).Sum(t => t.FAmount);
                                    ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)).Sum(t => t.FAmountEdit);
                                    ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)).Sum(t => t.FAmountAfterEdit);
                                    if (ysAcc.SUBJECTCODE == "4BNHJSR")
                                    {
                                        ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmountEdit);
                                        ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmountAfterEdit);
                                        //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")));
                                    }
                                    if (ysAcc.SUBJECTCODE == "5BNHJZC")
                                    {
                                        ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmountEdit);
                                        ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmountAfterEdit);
                                        //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")));
                                    }
                                    if (ysAcc.SUBJECTCODE == "5QM1")
                                    {
                                        ysAcc.ADJUSTMENT = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").ADJUSTMENT) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").ADJUSTMENT);
                                        ysAcc.APPROVEDBUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").APPROVEDBUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").APPROVEDBUDGETTOTAL);
                                    }
                                    if (ysAcc.SUBJECTCODE == "5QM2")
                                    {
                                        ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountEdit);
                                        ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountAfterEdit);
                                        //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")));
                                    }
                                    if (ysAcc.SUBJECTCODE == "5QM3")
                                    {
                                        ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")).Sum(t => t.FAmountEdit);
                                        ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountAfterEdit);
                                        //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")));
                                    }
                                    if (ysAcc.SUBJECTCODE == "5QM6")
                                    {
                                        ysAcc.ADJUSTMENT = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").ADJUSTMENT) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").ADJUSTMENT)
                                                + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").ADJUSTMENT) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").ADJUSTMENT)
                                                - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").ADJUSTMENT);
                                        ysAcc.APPROVEDBUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").APPROVEDBUDGETTOTAL) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").APPROVEDBUDGETTOTAL)
                                                + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").APPROVEDBUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").APPROVEDBUDGETTOTAL)
                                                - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").APPROVEDBUDGETTOTAL);
                                    }
                                }
                                else
                                {
                                    //ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts == ysAcc.SUBJECTCODE).Sum(t => t.FAmount);
                                    ysAcc.ADJUSTMENT = 0;
                                    ysAcc.APPROVEDBUDGETTOTAL = ysAcc.BUDGETTOTAL;
                                }
                            }
                        }
                    }
                    else
                    {
                        //年初也没上报(只要有数据，那么表示年初一定是上报过了，所以预算数直接去数据库的数据)
                        if (ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
                        {
                            foreach (var ysAcc in ysAccountMst.YsAccounts)
                            {
                                if (ysAccountMst.SaveStart > 0 && ysAcc.SUBJECTCODE.StartsWith("4"))
                                {
                                    continue;
                                }
                                if (budgetDtlBudgetDtls != null && budgetDtlBudgetDtls.Count > 0)
                                {
                                    ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)).Sum(t => t.FAmount);
                                    ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)).Sum(t => t.FAmountEdit);
                                    ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)).Sum(t => t.FAmountAfterEdit);
                                    if (ysAcc.SUBJECTCODE == "4BNHJSR")
                                    {
                                        ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmount);
                                        ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmountEdit);
                                        ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmountAfterEdit);
                                        //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")));
                                    }
                                    if (ysAcc.SUBJECTCODE == "5BNHJZC")
                                    {
                                        ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmount);
                                        ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmountEdit);
                                        ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmountAfterEdit);
                                        //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")));
                                    }
                                    if (ysAcc.SUBJECTCODE == "5QM1")
                                    {
                                        ysAcc.BUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").BUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").BUDGETTOTAL);
                                        ysAcc.ADJUSTMENT = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").ADJUSTMENT) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").ADJUSTMENT);
                                        ysAcc.APPROVEDBUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").APPROVEDBUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").APPROVEDBUDGETTOTAL);
                                    }
                                    if (ysAcc.SUBJECTCODE == "5QM2")
                                    {

                                        ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmount);
                                        ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountEdit);
                                        ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountAfterEdit);
                                        //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")));
                                    }
                                    if (ysAcc.SUBJECTCODE == "5QM3")
                                    {
                                        ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")).Sum(t => t.FAmount);
                                        ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")).Sum(t => t.FAmountEdit);
                                        ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountAfterEdit);
                                        //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")));
                                    }
                                    if (ysAcc.SUBJECTCODE == "5QM6")
                                    {
                                        ysAcc.BUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").BUDGETTOTAL) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").BUDGETTOTAL)
                                                + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").BUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").BUDGETTOTAL)
                                                - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").BUDGETTOTAL);
                                        ysAcc.ADJUSTMENT = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").ADJUSTMENT) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").ADJUSTMENT)
                                                + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").ADJUSTMENT) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").ADJUSTMENT)
                                                - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").ADJUSTMENT);
                                        ysAcc.APPROVEDBUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").APPROVEDBUDGETTOTAL) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").APPROVEDBUDGETTOTAL)
                                                + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").APPROVEDBUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").APPROVEDBUDGETTOTAL)
                                                - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").APPROVEDBUDGETTOTAL);
                                    }
                                }
                                else
                                {
                                    ysAcc.BUDGETTOTAL = 0;
                                    //ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts == ysAcc.SUBJECTCODE).Sum(t => t.FAmount);
                                    ysAcc.ADJUSTMENT = 0;
                                    ysAcc.APPROVEDBUDGETTOTAL = 0;
                                }
                            }
                        }
                    }

                    if (ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
                    {
                        foreach(var ysA in ysAccountMst.YsAccounts)
                        {
                            if(ysA.BUDGETTOTAL == 0)
                            {
                                ysA.BudgetComplete = 100;
                            }
                            else
                            {
                                ysA.BudgetComplete = ysA.APPROVEDBUDGETTOTAL / ysA.BUDGETTOTAL * 100;
                            }
                        }
                    }
                }
            }
            else{
                IList<YsAccountModel> ysAccounts = new List<YsAccountModel>();
                ysAccounts = GetYsAccounts(orgId.ToString(), orgCode, year);
                ysAccountMst.Orgid = orgId;
                ysAccountMst.Orgcode = orgCode;
                ysAccountMst.Uyear = year;
                ysAccountMst.YsAccounts = ysAccounts;
                //年中,年初，年末都没上报
                if (ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
                {
                    foreach (var ysAcc in ysAccountMst.YsAccounts)
                    {
                        if (budgetDtlBudgetDtls != null && budgetDtlBudgetDtls.Count > 0)
                        {
                            ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)).Sum(t => t.FAmount);
                            ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)).Sum(t => t.FAmountEdit);
                            ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)).Sum(t => t.FAmountAfterEdit);
                            if (ysAcc.SUBJECTCODE == "4BNHJSR")
                            {
                                ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmount);
                                ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmountEdit);
                                ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmountAfterEdit);
                                //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")));
                            }
                            if (ysAcc.SUBJECTCODE == "5BNHJZC")
                            {
                                ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmount);
                                ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmountEdit);
                                ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmountAfterEdit);
                                //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")));
                            }
                            if (ysAcc.SUBJECTCODE == "5QM1")
                            {
                                ysAcc.BUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").BUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").BUDGETTOTAL);
                                ysAcc.ADJUSTMENT = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").ADJUSTMENT) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").ADJUSTMENT);
                                ysAcc.APPROVEDBUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").APPROVEDBUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").APPROVEDBUDGETTOTAL);
                            }
                            if (ysAcc.SUBJECTCODE == "5QM2")
                            {

                                ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmount);
                                ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountEdit);
                                ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountAfterEdit);
                                //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")));
                            }
                            if (ysAcc.SUBJECTCODE == "5QM3")
                            {
                                ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")).Sum(t => t.FAmount);
                                ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")).Sum(t => t.FAmountEdit);
                                ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountAfterEdit);
                                //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")));
                            }
                            if (ysAcc.SUBJECTCODE == "5QM6")
                            {
                                ysAcc.BUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").BUDGETTOTAL) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").BUDGETTOTAL)
                                        + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").BUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").BUDGETTOTAL)
                                        - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").BUDGETTOTAL);
                                ysAcc.ADJUSTMENT = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").ADJUSTMENT) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").ADJUSTMENT)
                                        + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").ADJUSTMENT) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").ADJUSTMENT)
                                        - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").ADJUSTMENT);
                                ysAcc.APPROVEDBUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").APPROVEDBUDGETTOTAL) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").APPROVEDBUDGETTOTAL)
                                        + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").APPROVEDBUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").APPROVEDBUDGETTOTAL)
                                        - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").APPROVEDBUDGETTOTAL);
                            }
                        }
                        else
                        {
                            ysAcc.BUDGETTOTAL = 0;
                            ysAcc.ADJUSTMENT = 0;
                            ysAcc.APPROVEDBUDGETTOTAL = ysAcc.BUDGETTOTAL;
                        }
                    }
                }

                if (ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
                {
                    foreach (var ysA in ysAccountMst.YsAccounts)
                    {
                        if (ysA.BUDGETTOTAL == 0)
                        {
                            ysA.BudgetComplete = 100;
                        }
                        else
                        {
                            ysA.BudgetComplete = ysA.APPROVEDBUDGETTOTAL / ysA.BUDGETTOTAL * 100;
                        }
                    }
                }
            }

            ysAccountMst.YsAccounts = GetSubjectNameForYs(ysAccountMst.YsAccounts);
            ysAccountMst.YsAccounts = ysAccountMst.YsAccounts.OrderBy(t => t.SUBJECTCODE).ToList();
            return ysAccountMst;
        }

        /// <summary>
        /// 给科目附上名称 (比例保留四位小数)
        /// </summary>
        /// <param name="ysAccounts"></param>
        /// <returns></returns>
        public IList<YsAccountModel> GetSubjectNameForYs(IList<YsAccountModel> ysAccounts)
        {
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
            if (ysAccounts != null && ysAccounts.Count > 0)
            {
                foreach (var ysA in ysAccounts)
                {
                    ysA.COMPLETE = Math.Round(ysA.COMPLETE, 4);
                    ysA.BudgetComplete = Math.Round(ysA.BudgetComplete, 4);
                    if (ysA.SUBJECTCODE == "4BNHJSR")
                    {
                        ysA.SUBJECTNAME = "本年收入合计";
                    }
                    else if (ysA.SUBJECTCODE == "5BNHJZC")
                    {
                        ysA.SUBJECTNAME = "本年支出合计";
                    }
                    else if (ysA.SUBJECTCODE == "5QM1")
                    {
                        ysA.SUBJECTNAME = "本年结余";
                    }
                    else if (ysA.SUBJECTCODE == "5QM2")
                    {
                        ysA.SUBJECTNAME = "加：上年结余";
                    }
                    else if (ysA.SUBJECTCODE == "5QM3")
                    {
                        ysA.SUBJECTNAME = "加：本年收回投资";
                    }
                    else if (ysA.SUBJECTCODE == "5QM4")
                    {
                        ysA.SUBJECTNAME = "减：本年投资";
                    }
                    else if (ysA.SUBJECTCODE == "5QM5")
                    {
                        ysA.SUBJECTNAME = "减：本年提取后备金";
                    }
                    else if (ysA.SUBJECTCODE == "5QM6")
                    {
                        ysA.SUBJECTNAME = "期末滚存结余";
                    }
                    else
                    {
                        ysA.SUBJECTNAME = allAccounts.ToList().Find(t => t.KMDM == ysA.SUBJECTCODE) == null ? "" : allAccounts.ToList().Find(t => t.KMDM == ysA.SUBJECTCODE).KMMC;
                    }
                    
                }
            }
            return ysAccounts;
        }


        /// <summary>
        /// 得到年末上报的数据（单个组织）
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public YsAccountMstModel GetEndAccounts(long orgId, string orgCode, string year)
        {
            //取本年预算的所有明细数据
            IList<BudgetMstModel> budgetMsts = new List<BudgetMstModel>();//用来存主表数据
            IList<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtls = new List<BudgetDtlBudgetDtlModel>();//用来存明细数据
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic).Add(ORMRestrictions<string>.Eq("FYear", year))
               .Add(ORMRestrictions<string>.Eq("FDeclarationUnit", orgCode))
               .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
               .Add(ORMRestrictions<string>.Eq("FApproveStatus", "3"))
               .Add(ORMRestrictions<string>.Eq("FMidYearChange", "0"));
            budgetMsts = this.BudgetMstRule.Find(dic);
            if (budgetMsts != null && budgetMsts.Count > 0)
            {
                var phids = budgetMsts.Select(t => t.PhId).Distinct().ToList();
                if (phids != null && phids.Count > 0)
                {
                    dic.Clear();
                    new CreateCriteria(dic).Add(ORMRestrictions<List<long>>.In("MstPhid", phids));
                    budgetDtlBudgetDtls = this.BudgetDtlBudgetDtlRule.Find(dic);
                }
            }

            //先判断该组织年中数据是否上报（若上报直接取数据库存好的数据）
            YsAccountMstModel ysAccountMst = new YsAccountMstModel();
            IList<YsAccountMstModel> ysAccountMsts = new List<YsAccountMstModel>();
            ysAccountMsts = this.YsAccountMstRule.Find(t => t.Orgid == orgId && t.Uyear == year);
            //已经上报过
            if (ysAccountMsts != null && ysAccountMsts.Count > 0)
            {
                ysAccountMst = ysAccountMsts[0];
                IList<YsAccountModel> ysAccounts = new List<YsAccountModel>();
                ysAccounts = this.YsAccountRule.Find(t => t.PHIDMST == ysAccountMst.PhId && t.ORGID == orgId && t.UYEAR == year);
                ysAccountMst.YsAccounts = ysAccounts;
                //年末已经上报
                if (ysAccountMst.VerifyEnd > 0)
                {

                    //IList<YsAccountModel> ysAccounts = new List<YsAccountModel>();
                    //ysAccounts = this.YsAccountRule.Find(t => t.PHIDMST == ysAccountMst.PhId && t.ORGID == orgId && t.UYEAR == year);
                    //ysAccountMst.YsAccounts = ysAccounts;
                }
                else
                {
                    //年中已经上报了(只要加入完成率即可)
                    if(ysAccountMst.VerifyMiddle > 0)
                    {
                        if (ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
                        {
                            foreach (var ysAcc in ysAccountMst.YsAccounts)
                            {
                                if(ysAcc.APPROVEDBUDGETTOTAL == 0)
                                {
                                    ysAcc.COMPLETE = 100;
                                }
                                else
                                {
                                    ysAcc.COMPLETE = ysAcc.ThisaccountsTotal / ysAcc.APPROVEDBUDGETTOTAL * 100;
                                }
                            }
                        }
                    }
                    else
                    {
                        if(ysAccountMst.VerifyStart > 0)
                        {
                            //年初已经上报，年中与年末都没有上报，年初已上报(只要有数据，那么表示年初一定是上报过了，所以预算数直接去数据库的数据)
                            if (ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
                            {
                                foreach (var ysAcc in ysAccountMst.YsAccounts)
                                {
                                    if (ysAccountMst.SaveMiddle > 0 && ysAcc.SUBJECTCODE.StartsWith("4"))
                                    {
                                        continue;
                                    }
                                    if (budgetDtlBudgetDtls != null && budgetDtlBudgetDtls.Count > 0)
                                    {
                                        //ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts == ysAcc.SUBJECTCODE).Sum(t => t.FAmount);
                                        ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)).Sum(t => t.FAmountEdit);
                                        ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)).Sum(t => t.FAmountAfterEdit);
                                        if (ysAcc.SUBJECTCODE == "4BNHJSR")
                                        {
                                            //ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmount);
                                            ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmountEdit);
                                            ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmountAfterEdit);
                                            //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")));
                                        }
                                        if (ysAcc.SUBJECTCODE == "5BNHJZC")
                                        {
                                            //ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmount);
                                            ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmountEdit);
                                            ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmountAfterEdit);
                                            //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")));
                                        }
                                        if (ysAcc.SUBJECTCODE == "5QM1")
                                        {
                                            //ysAcc.BUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").ADJUSTMENT) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").BUDGETTOTAL);
                                            ysAcc.ADJUSTMENT = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").ADJUSTMENT) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").ADJUSTMENT);
                                            ysAcc.APPROVEDBUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").APPROVEDBUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").APPROVEDBUDGETTOTAL);
                                        }
                                        if (ysAcc.SUBJECTCODE == "5QM2")
                                        {

                                            //ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmount);
                                            ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountEdit);
                                            ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountAfterEdit);
                                            //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")));
                                        }
                                        if (ysAcc.SUBJECTCODE == "5QM3")
                                        {
                                            //ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")).Sum(t => t.FAmount);
                                            ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")).Sum(t => t.FAmountEdit);
                                            ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountAfterEdit);
                                            //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")));
                                        }
                                        if (ysAcc.SUBJECTCODE == "5QM6")
                                        {
                                            //ysAcc.BUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").BUDGETTOTAL) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").BUDGETTOTAL)
                                                   // + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").BUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").BUDGETTOTAL)
                                                    //- (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").BUDGETTOTAL);
                                            ysAcc.ADJUSTMENT = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").ADJUSTMENT) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").ADJUSTMENT)
                                                    + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").ADJUSTMENT) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").ADJUSTMENT)
                                                    - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").ADJUSTMENT);
                                            ysAcc.APPROVEDBUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").APPROVEDBUDGETTOTAL) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").APPROVEDBUDGETTOTAL)
                                                    + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").APPROVEDBUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").APPROVEDBUDGETTOTAL)
                                                    - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").APPROVEDBUDGETTOTAL);
                                        }
                                    }
                                    else
                                    {
                                        //ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts == ysAcc.SUBJECTCODE).Sum(t => t.FAmount);
                                        ysAcc.ADJUSTMENT = 0;
                                        ysAcc.APPROVEDBUDGETTOTAL = ysAcc.BUDGETTOTAL;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //年初都没有上报，年中与年末都没有上报，年初已上报(只要有数据，那么表示年初一定是上报过了，所以预算数直接去数据库的数据)
                            if (ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
                            {
                                foreach (var ysAcc in ysAccountMst.YsAccounts)
                                {
                                    if (ysAccountMst.SaveStart > 0 && ysAcc.SUBJECTCODE.StartsWith("4"))
                                    {
                                        continue;
                                    }
                                    if (budgetDtlBudgetDtls != null && budgetDtlBudgetDtls.Count > 0)
                                    {
                                        ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)).Sum(t => t.FAmount);
                                        ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)).Sum(t => t.FAmountEdit);
                                        ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)).Sum(t => t.FAmountAfterEdit);
                                        if (ysAcc.SUBJECTCODE == "4BNHJSR")
                                        {
                                            ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmount);
                                            ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmountEdit);
                                            ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmountAfterEdit);
                                            //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")));
                                        }
                                        if (ysAcc.SUBJECTCODE == "5BNHJZC")
                                        {
                                            ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmount);
                                            ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmountEdit);
                                            ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmountAfterEdit);
                                            //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")));
                                        }
                                        if (ysAcc.SUBJECTCODE == "5QM1")
                                        {
                                            ysAcc.BUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").BUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").BUDGETTOTAL);
                                            ysAcc.ADJUSTMENT = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").ADJUSTMENT) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").ADJUSTMENT);
                                            ysAcc.APPROVEDBUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").APPROVEDBUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").APPROVEDBUDGETTOTAL);
                                        }
                                        if (ysAcc.SUBJECTCODE == "5QM2")
                                        {

                                            ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmount);
                                            ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountEdit);
                                            ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountAfterEdit);
                                            //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")));
                                        }
                                        if (ysAcc.SUBJECTCODE == "5QM3")
                                        {
                                            ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")).Sum(t => t.FAmount);
                                            ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")).Sum(t => t.FAmountEdit);
                                            ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountAfterEdit);
                                            //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")));
                                        }
                                        if (ysAcc.SUBJECTCODE == "5QM6")
                                        {
                                            ysAcc.BUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").BUDGETTOTAL) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").BUDGETTOTAL)
                                                    + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").BUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").BUDGETTOTAL)
                                                    - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").BUDGETTOTAL);
                                            ysAcc.ADJUSTMENT = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").ADJUSTMENT) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").ADJUSTMENT)
                                                    + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").ADJUSTMENT) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").ADJUSTMENT)
                                                    - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").ADJUSTMENT);
                                            ysAcc.APPROVEDBUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").APPROVEDBUDGETTOTAL) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").APPROVEDBUDGETTOTAL)
                                                    + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").APPROVEDBUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").APPROVEDBUDGETTOTAL)
                                                    - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").APPROVEDBUDGETTOTAL);
                                        }
                                    }
                                    else
                                    {
                                        ysAcc.BUDGETTOTAL = 0;
                                        ysAcc.ADJUSTMENT = 0;
                                        ysAcc.APPROVEDBUDGETTOTAL = ysAcc.BUDGETTOTAL;
                                    }
                                }
                            }
                        }

                        if (ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
                        {
                            foreach (var ysA in ysAccountMst.YsAccounts)
                            {
                                if (ysA.BUDGETTOTAL == 0)
                                {
                                    ysA.BudgetComplete = 100;
                                }
                                else
                                {
                                    ysA.BudgetComplete =  ysA.APPROVEDBUDGETTOTAL / ysA.BUDGETTOTAL * 100;
                                }
                                if (ysA.APPROVEDBUDGETTOTAL == 0)
                                {
                                    ysA.COMPLETE = 100;
                                }
                                else
                                {
                                    ysA.COMPLETE = ysA.ThisaccountsTotal / ysA.APPROVEDBUDGETTOTAL * 100;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                IList<YsAccountModel> ysAccounts = new List<YsAccountModel>();
                ysAccounts = GetYsAccounts(orgId.ToString(), orgCode, year);
                ysAccountMst.Orgid = orgId;
                ysAccountMst.Orgcode = orgCode;
                ysAccountMst.Uyear = year;
                ysAccountMst.YsAccounts = ysAccounts;
                //年中,年初，年末都没上报
                if (ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
                {
                    foreach (var ysAcc in ysAccountMst.YsAccounts)
                    {
                        if (budgetDtlBudgetDtls != null && budgetDtlBudgetDtls.Count > 0)
                        {
                            ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)).Sum(t => t.FAmount);
                            ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)).Sum(t => t.FAmountEdit);
                            ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ? "" : t.FBudgetAccounts).StartsWith(ysAcc.SUBJECTCODE)).Sum(t => t.FAmountAfterEdit);
                            if (ysAcc.SUBJECTCODE == "4BNHJSR")
                            {
                                ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmount);
                                ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmountEdit);
                                ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")).Sum(t => t.FAmountAfterEdit);
                                //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("4")));
                            }
                            if (ysAcc.SUBJECTCODE == "5BNHJZC")
                            {
                                ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmount);
                                ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmountEdit);
                                ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")).Sum(t => t.FAmountAfterEdit);
                                //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("5")));
                            }
                            if (ysAcc.SUBJECTCODE == "5QM1")
                            {
                                ysAcc.BUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").BUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").BUDGETTOTAL);
                                ysAcc.ADJUSTMENT = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").ADJUSTMENT) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").ADJUSTMENT);
                                ysAcc.APPROVEDBUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "4BNHJSR").APPROVEDBUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5BNHJZC").APPROVEDBUDGETTOTAL);
                            }
                            if (ysAcc.SUBJECTCODE == "5QM2")
                            {

                                ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmount);
                                ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountEdit);
                                ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountAfterEdit);
                                //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")));
                            }
                            if (ysAcc.SUBJECTCODE == "5QM3")
                            {
                                ysAcc.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")).Sum(t => t.FAmount);
                                ysAcc.ADJUSTMENT = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")).Sum(t => t.FAmountEdit);
                                ysAcc.APPROVEDBUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")) == null ? 0 : budgetDtlBudgetDtls.ToList().FindAll(t => (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("331")).Sum(t => t.FAmountAfterEdit);
                                //res.BUDGETTOTAL = budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")) == null ? 0 : GetSumAmount(budgetDtlBudgetDtls.ToList().FindAll(t => t.FBudgetAccounts != null && (t.FBudgetAccounts == null ?"": t.FBudgetAccounts).StartsWith("311")));
                            }
                            if (ysAcc.SUBJECTCODE == "5QM6")
                            {
                                ysAcc.BUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").BUDGETTOTAL) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").BUDGETTOTAL)
                                        + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").BUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").BUDGETTOTAL)
                                        - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").BUDGETTOTAL);
                                ysAcc.ADJUSTMENT = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").ADJUSTMENT) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").ADJUSTMENT)
                                        + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").ADJUSTMENT) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").ADJUSTMENT)
                                        - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").ADJUSTMENT);
                                ysAcc.APPROVEDBUDGETTOTAL = (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM1").APPROVEDBUDGETTOTAL) + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM2").APPROVEDBUDGETTOTAL)
                                        + (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM3").APPROVEDBUDGETTOTAL) - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM4").APPROVEDBUDGETTOTAL)
                                        - (ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5") == null ? 0 : ysAccountMst.YsAccounts.ToList().Find(t => t.SUBJECTCODE == "5QM5").APPROVEDBUDGETTOTAL);
                            }
                        }
                        else
                        {
                            ysAcc.BUDGETTOTAL = 0;
                            ysAcc.ADJUSTMENT = 0;
                            ysAcc.APPROVEDBUDGETTOTAL = ysAcc.BUDGETTOTAL;
                        }
                    }
                }

                if (ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
                {
                    foreach (var ysA in ysAccountMst.YsAccounts)
                    {
                        if (ysA.BUDGETTOTAL == 0)
                        {
                            ysA.BudgetComplete = 100;
                        }
                        else
                        {
                            ysA.BudgetComplete = ysA.APPROVEDBUDGETTOTAL / ysA.BUDGETTOTAL * 100;
                        }
                        if (ysA.APPROVEDBUDGETTOTAL == 0)
                        {
                            ysA.COMPLETE = 100;
                        }
                        else
                        {
                            ysA.COMPLETE = ysA.ThisaccountsTotal / ysA.APPROVEDBUDGETTOTAL * 100;
                        }
                    }
                }
            }

            ysAccountMst.YsAccounts = GetSubjectNameForYs(ysAccountMst.YsAccounts);
            ysAccountMst.YsAccounts = ysAccountMst.YsAccounts.OrderBy(t => t.SUBJECTCODE).ToList();
            return ysAccountMst;
        }
        #endregion
    }
}

