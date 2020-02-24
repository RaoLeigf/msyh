#region Summary
/**************************************************************************************
    * 类 名 称：        ExpenseMstFacade
    * 命名空间：        GYS3.YS.Facade
    * 文 件 名：        ExpenseMstFacade.cs
    * 创建时间：        2019/1/24 
    * 作    者：        董泉伟    
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
using Enterprise3.Common.Base.Criterion;
using NG3.WorkFlow.Interfaces;
using GYS3.YS.Model.Enums;
using NG3.WorkFlow.Rule;
using Newtonsoft.Json.Linq;
using GQT3.QT.Rule.Interface;
using GQT3.QT.Model.Domain;
using System.Data;
using NG3.Data.Service;
using GSP3.SP.Model.Domain;
using SUP.Common.Base;
using GYS3.YS.Model.Extra;

namespace GYS3.YS.Facade
{
	/// <summary>
	/// ExpenseMst业务组装处理类
	/// </summary>
    public partial class ExpenseMstFacade : EntFacadeBase<ExpenseMstModel>, IExpenseMstFacade, IWorkFlowPlugin
    {
		#region 类变量及属性
		/// <summary>
        /// ExpenseMst业务逻辑处理对象
        /// </summary>
		IExpenseMstRule ExpenseMstRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IExpenseMstRule;
            }
        }
		/// <summary>
        /// ExpenseDtl业务逻辑处理对象
        /// </summary>
		IExpenseDtlRule ExpenseDtlRule { get; set; }

        IExpenseHxRule ExpenseHxRule { get; set; }

        ICorrespondenceSettings2Rule CorrespondenceSettings2Rule { get; set; }
        ICorrespondenceSettingsRule CorrespondenceSettingsRule { get; set; }
        IQTControlSetRule QTControlSetRule { get; set; }
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
        public override PagedResult<ExpenseMstModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<ExpenseMstModel> pageResult = base.LoadWithPage(pageNumber, pageSize, dic, sorts);

            pageResult = AddNextApproveName(pageResult, "GHExpense");

            #region 列表Grid代码转名称
            RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<ExpenseMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<ExpenseMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
			helpdac.CodeToName<ExpenseMstModel>(pageResult.Results, "FDeclarationunit", "FDeclarationunit_EXName", "sb_orglist", "");
			helpdac.CodeToName<ExpenseMstModel>(pageResult.Results, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode", "");
			helpdac.CodeToName<ExpenseMstModel>(pageResult.Results, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
			helpdac.CodeToName<ExpenseMstModel>(pageResult.Results, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory", "");
			helpdac.CodeToName<ExpenseMstModel>(pageResult.Results, "FApprover", "FApprover_EXName", "fg3_user", "");
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
        public PagedResult<ExpenseMstModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<ExpenseMstModel> pageResult = base.FacadeHelper.LoadWithPage(pageNumber, pageSize, nameSqlName, dic, sorts);

            pageResult = AddNextApproveName(pageResult, "GHExpense");

            #region 列表Grid代码转名称
            RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<ExpenseMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<ExpenseMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
			helpdac.CodeToName<ExpenseMstModel>(pageResult.Results, "FDeclarationunit", "FDeclarationunit_EXName", "sb_orglist", "");
			helpdac.CodeToName<ExpenseMstModel>(pageResult.Results, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode", "");
			helpdac.CodeToName<ExpenseMstModel>(pageResult.Results, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
			helpdac.CodeToName<ExpenseMstModel>(pageResult.Results, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory", "");
			helpdac.CodeToName<ExpenseMstModel>(pageResult.Results, "FApprover", "FApprover_EXName", "fg3_user", "");
            #endregion

            return pageResult;
        }

        /// <summary>
        /// 通过id，删除数据
        /// </summary>
        /// <param name="id">单主键id值</param>
        public override DeletedResult Delete<TValType>(TValType id)
        {
			ExpenseDtlRule.RuleHelper.DeleteByForeignKey(id);
			return base.Delete(id);
        }

		/// <summary>
        /// 通过ids，删除数据
        /// </summary>
        /// <param name="ids">单主键id集合</param>
        public override DeletedResult Delete<TValType>(IList<TValType> ids)
        {
			ExpenseDtlRule.RuleHelper.DeleteByForeignKey(ids);
			return base.Delete(ids);
        }
        
        #endregion

        #region 实现 IExpenseMstFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<ExpenseMstModel> ExampleMethod<ExpenseMstModel>(string param)
        //{
        //    //编写代码
        //}

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="expenseMstEntity"></param>
        /// <param name="expenseDtlEntities"></param>
        /// <param name="NCmoney">年初预算金额</param>
        /// <param name="SumFSurplusamount">数据库支出和返回的计算值</param>
        /// <param name="beforeSum">本单据初始预计支出金额</param>
        /// <param name="beforeFReturnamount">本单据初始预计返还金额</param>
        /// <param name="Ifreturn">是否额度返还</param>
        /// <returns></returns>
        public SavedResult<Int64> SaveExpenseMst(ExpenseMstModel expenseMstEntity, List<ExpenseDtlModel> expenseDtlEntities, string NCmoney, string SumFSurplusamount, string beforeSum, string beforeFReturnamount, string Ifreturn)
        {
            SavedResult<Int64> savedResult = new SavedResult<Int64>();
            decimal FPlayamount;
            /*if (expenseMstEntity.PersistentState == SUP.Common.Base.PersistentState.Added)
            {
                FPlayamount = decimal.Parse(NCmoney) - decimal.Parse(SumFSurplusamount) - expenseMstEntity.FSurplusamount+ decimal.Parse(beforeSum);
            }
            else
            {
                FPlayamount = decimal.Parse(NCmoney) - (decimal.Parse(SumFSurplusamount)- decimal.Parse(beforeSum))- expenseMstEntity.FSurplusamount;
            }*/


            FPlayamount = decimal.Parse(NCmoney) - decimal.Parse(SumFSurplusamount) - expenseMstEntity.FSurplusamount + decimal.Parse(beforeSum) + expenseMstEntity.FReturnamount - decimal.Parse(beforeFReturnamount);
            if (FPlayamount >= 0)
            {
                if (Ifreturn == "1")
                {
                    /*ExpenseMstModel expenseMstEntity2 = base.Find(expenseMstEntity.PhId).Data;
                    //expenseMstEntity2.PhId = 0;
                    expenseMstEntity2.FIfpurchase = 1;
                    expenseMstEntity2.PersistentState = SUP.Common.Base.PersistentState.Added;
                    SavedResult<Int64> savedResult2 = base.Save<Int64>(expenseMstEntity2);

                    if (savedResult2.Status == ResponseStatus.Success && savedResult2.KeyCodes.Count > 0)
                    {

                        IList<ExpenseDtlModel> expenseDtlEntities2 = ExpenseDtlRule.FindByForeignKey<Int64>(expenseMstEntity.PhId);
                        if (expenseDtlEntities2.Count > 0)
                        {
                            for (var j = 0; j < expenseDtlEntities2.Count; j++)
                            {
                                expenseDtlEntities2[j].PersistentState = SUP.Common.Base.PersistentState.Added;
                            }
                            ExpenseDtlRule.Save(expenseDtlEntities, savedResult2.KeyCodes[0]);
                        }
                    }*/
                    ExpenseMstModel expenseMstEntity2 = base.Find(expenseMstEntity.PhId).Data;
                    expenseMstEntity2.FIfpurchase = 1;
                    expenseMstEntity2.PersistentState = SUP.Common.Base.PersistentState.Modified;

                    expenseMstEntity.FProjname += "-额度返还";
                    expenseMstEntity.PhId = 0;
                    expenseMstEntity.FApprovestatus = "4";
                    expenseMstEntity.FIfpurchase = 2;//判读是否额度返还
                    expenseMstEntity.FPlayamount = FPlayamount;
                    expenseMstEntity.FYear = DateTime.Now.Year.ToString();
                    //单据号要求与原单据不同
                    var dicdjh = new Dictionary<string, object>();
                    new CreateCriteria(dicdjh).Add(ORMRestrictions<string>.Eq("FProjcode", expenseMstEntity.FProjcode))
                            //.Add(ORMRestrictions<System.Int32>.Eq("FIfpurchase", 0));
                            .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0));
                    // .Add(ORMRestrictions<System.Int32>.NotEq("FIfpurchase", 1));
                    IList<ExpenseMstModel> ExdjhList = base.Find(dicdjh, new string[] { "FIfperformanceappraisal Desc" }).Data;
                    //FPerformevaltype作为单据号
                    if (ExdjhList.Count > 0)
                    {
                        List<ExpenseMstModel> ExdjhListSort = ExdjhList.ToList();
                        ExdjhListSort.Sort((ExpenseMstModel Mst1, ExpenseMstModel Mst2) => Mst1.FPerformevaltype.CompareTo(Mst2.FPerformevaltype));
                        if (!string.IsNullOrEmpty(ExdjhListSort[ExdjhListSort.Count - 1].FPerformevaltype))
                        {
                            expenseMstEntity.FPerformevaltype = expenseMstEntity.FProjcode + (long.Parse(ExdjhListSort[ExdjhListSort.Count - 1].FPerformevaltype.Substring(ExdjhListSort[ExdjhListSort.Count - 1].FPerformevaltype.Length - 4)) + 1).ToString().PadLeft(4, '0');
                        }
                        else
                        {
                            expenseMstEntity.FPerformevaltype = expenseMstEntity.FProjcode + "0001";
                        }
                    }
                    else
                    {
                        expenseMstEntity.FPerformevaltype = expenseMstEntity.FProjcode + "0001";
                    }

                    expenseMstEntity.PersistentState = SUP.Common.Base.PersistentState.Added;

                    savedResult = base.Save<Int64>(expenseMstEntity);
                    if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
                    {
                        expenseMstEntity2.FBillno = expenseMstEntity.PhId.ToString();
                        base.Save<Int64>(expenseMstEntity2);
                        if (expenseDtlEntities.Count > 0)
                        {
                            for (var x = 0; x < expenseDtlEntities.Count; x++)
                            {
                                expenseDtlEntities[x].PersistentState = SUP.Common.Base.PersistentState.Added;
                            }
                            ExpenseDtlRule.Save(expenseDtlEntities, savedResult.KeyCodes[0]);
                        }
                    }
                }
                else
                {
                    expenseMstEntity.FPlayamount = FPlayamount;

                    if (expenseMstEntity.PersistentState == SUP.Common.Base.PersistentState.Added)
                    {
                        var dicWhere2 = new Dictionary<string, object>();
                        new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FProjcode", expenseMstEntity.FProjcode))
                                //.Add(ORMRestrictions<System.Int32>.Eq("FIfpurchase", 0));
                                .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0));
                               // .Add(ORMRestrictions<System.Int32>.NotEq("FIfpurchase", 1));
                        IList<ExpenseMstModel> ExpenseMstList3 = base.Find(dicWhere2, new string[] { "FIfperformanceappraisal Desc" }).Data;
                        //筛选出正常单据和额度返还单据
                        List<ExpenseMstModel> ExpenseMstList2 = ExpenseMstList3.ToList().FindAll(t => t.FIfpurchase != 1);

                        expenseMstEntity.FYear = DateTime.Now.Year.ToString();

                        //FPerformevaltype作为单据号
                        if (ExpenseMstList3.Count > 0)
                        {
                            List<ExpenseMstModel> ExpenseMstListSort = ExpenseMstList3.ToList();
                            ExpenseMstListSort.Sort((ExpenseMstModel Mst1, ExpenseMstModel Mst2) => Mst1.FPerformevaltype.CompareTo(Mst2.FPerformevaltype));
                            if (!string.IsNullOrEmpty(ExpenseMstListSort[ExpenseMstListSort.Count - 1].FPerformevaltype))
                            {
                                expenseMstEntity.FPerformevaltype = expenseMstEntity.FProjcode + (long.Parse(ExpenseMstListSort[ExpenseMstListSort.Count - 1].FPerformevaltype.Substring(ExpenseMstListSort[ExpenseMstListSort.Count - 1].FPerformevaltype.Length-4)) + 1).ToString().PadLeft(4,'0');
                            }
                            else
                            {
                                expenseMstEntity.FPerformevaltype = expenseMstEntity.FProjcode + "0001";
                            }
                        }
                        else
                        {
                            expenseMstEntity.FPerformevaltype = expenseMstEntity.FProjcode + "0001";
                        }

                        if (ExpenseMstList2.Count > 0)
                        {
                            //有一条单据时该单据名称+ "-1"
                            if (ExpenseMstList2.Count == 1)
                            {
                                ExpenseMstList2[0].FIfperformanceappraisal = 1;
                                ExpenseMstList2[0].PersistentState = SUP.Common.Base.PersistentState.Modified;
                                if (ExpenseMstList2[0].FIfpurchase == 0)
                                {
                                    ExpenseMstList2[0].FProjname = ExpenseMstList2[0].FProjname + "-" + "1";
                                }
                                else
                                {
                                    ExpenseMstList2[0].FProjname = ExpenseMstList2[0].FProjname.Replace("额度返还", "1-额度返还");
                                    //寻找被额度返还的原单据
                                    List<ExpenseMstModel> ExpenseMstList4 = ExpenseMstList3.ToList().FindAll(t => t.FBillno == ExpenseMstList2[0].PhId.ToString());
                                    if (ExpenseMstList4.Count > 0)
                                    {
                                        ExpenseMstList4[0].FIfperformanceappraisal = 1;
                                        ExpenseMstList4[0].PersistentState = SUP.Common.Base.PersistentState.Modified;
                                        ExpenseMstList4[0].FProjname= ExpenseMstList4[0].FProjname + "-" + "1";
                                        base.Save<Int64>(ExpenseMstList4[0]);
                                    }
                                }
                                base.Save<Int64>(ExpenseMstList2[0]);
                                expenseMstEntity.FIfperformanceappraisal = 2;
                                expenseMstEntity.FProjname = expenseMstEntity.FProjname + "-" + "2";
                            }
                            else {
                                expenseMstEntity.FProjname = expenseMstEntity.FProjname + "-" + (ExpenseMstList2[0].FIfperformanceappraisal + 1).ToString();
                                expenseMstEntity.FIfperformanceappraisal = ExpenseMstList2[0].FIfperformanceappraisal + 1;
                            }
                        }
                        else
                        {
                            //expenseMstEntity.FProjname = expenseMstEntity.FProjname + "-" + "1";
                            expenseMstEntity.FIfperformanceappraisal = 1;
                        }
                    }



                    savedResult = base.Save<Int64>(expenseMstEntity);
                    if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
                    {
                        if (expenseDtlEntities.Count > 0)
                        {
                            ExpenseDtlRule.Save(expenseDtlEntities, savedResult.KeyCodes[0]);
                        }
                    }


                }
                var dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FProjcode", expenseMstEntity.FProjcode))
                        .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
                        .Add(ORMRestrictions<System.Int32>.NotEq("FIfpurchase", 1))
                        .Add(ORMRestrictions<System.Int64>.NotEq("PhId", expenseMstEntity.PhId));
                IList<ExpenseMstModel> ExpenseMstList = base.Find(dicWhere).Data;
                for (var i = 0; i < ExpenseMstList.Count; i++)
                {
                    ExpenseMstList[i].FPlayamount = FPlayamount;
                    ExpenseMstList[i].PersistentState = SUP.Common.Base.PersistentState.Modified;
                    base.Save<Int64>(ExpenseMstList[i]);
                }
            }
            else
            {
                savedResult.Status = "failure";
                savedResult.Msg = "预计支出金额超出项目的预算数，请检查！";
            }



            return savedResult;
        }

        /// <summary>
        /// 部门代码转
        /// </summary>
        /// <param name="nameSqlName"></param>
        /// <param name="dicWhere"></param>
        /// <param name="isUseInfoRight"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public PagedResult<ExpenseMstModel> LoadWithPageForDept(string nameSqlName = "", Dictionary<string, object> dicWhere = null, bool isUseInfoRight = false, params string[] sorts)
        {
            PagedResult<ExpenseMstModel> pageResult = base.FacadeHelper.LoadWithPageInfinity(nameSqlName, dicWhere, false, sorts);
            RichHelpDac helpdac = new RichHelpDac();
            helpdac.CodeToName<ExpenseMstModel>(pageResult.Results, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
            return pageResult;
        }

        /*
        /// <summary>
        /// 获取实际发生数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="expenseMsts"></param>
        /// <returns></returns>
        public PagedResult<ExpenseMstModel> GetSJFSS (string userID, PagedResult<ExpenseMstModel> expenseMsts)
        {
            string FDeclarationUnit="";
            var dicWhereUnit = new Dictionary<string, object>();
            new CreateCriteria(dicWhereUnit).Add(ORMRestrictions<string>.Eq("Dylx", "08"))
                .Add(ORMRestrictions<Int32>.Eq("DefInt1", 1)).Add(ORMRestrictions<string>.Eq("Dwdm", userID));
            var CorrespondenceSettings = CorrespondenceSettingsRule.Find(dicWhereUnit);
            if (CorrespondenceSettings.Count > 0)
            {
                FDeclarationUnit = CorrespondenceSettings[0].Dydm;
            }

            DateTime? beforeDate = new DateTime(DateTime.Now.Year, 1, 1); //账务筛选时间
            var dicWheredate = new Dictionary<string, object>();
            new CreateCriteria(dicWheredate).Add(ORMRestrictions<string>.Eq("BZ", "G6HBLKZExpense"));
            var setModels = QTControlSetRule.Find(dicWheredate);//QTControlSetModel
            if (setModels.Count > 0)
            {
                beforeDate = setModels[0].BEGINFDATE;
            }

            string userConn="";
            string select_DM;
            string Date_Dm;
            Dictionary<string, object> conndic = new Dictionary<string, object>();
            new CreateCriteria(conndic)
                .Add(ORMRestrictions<string>.Eq("Dylx", "config"))
                .Add(ORMRestrictions<string>.Eq("DefStr1", FDeclarationUnit));
            IList<CorrespondenceSettings2Model> CorrespondenceSettings2s = CorrespondenceSettings2Rule.RuleHelper.Find(conndic);
            if (CorrespondenceSettings2s.Count > 0 && CorrespondenceSettings2s[0].DefStr2 != null)
            {
                userConn = CorrespondenceSettings2s[0].DefStr2;
            }
            if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
            {
                select_DM = @"SELECT sum(nvl(j,0))-sum(nvl(d,0)) FROM v_zw_pzhz WHERE zxyt= ";
                Date_Dm = " and PZRQ < to_date('" + Convert.ToDateTime(beforeDate).ToString("yyyy-MM-dd") + "', 'yyyy/mm/dd hh24:mi:ss')";
            }
            else
            {
                select_DM = @"SELECT sum(isnull(j,0))-sum(isnull(d,0)) FROM v_zw_pzhz WHERE zxyt= ";
                Date_Dm = " and PZRQ < '" + Convert.ToDateTime(beforeDate).ToString("yyyy-MM-dd") + "'";
            }
            DataTable dataTable = null;
            DbHelper.Open(userConn);
            for (var i = 0; i < expenseMsts.Results.Count; i++)
            {
                dataTable = DbHelper.GetDataTable(userConn, select_DM+"'"+ expenseMsts.Results[i].FProjcode+"'" + Date_Dm);
                if (dataTable.Rows.Count != 0)
                {
                    expenseMsts.Results[i].FDeclarationDept = dataTable.Rows[0][0].ToString();
                }
            }
            DbHelper.Close(userConn);
            return expenseMsts;
        }*/

        /// <summary>
        /// 通过项目代码和操作员获取财务实际发生数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetSJFSSbyCode(string userID,string code)
        {
            var result="";
            string FDeclarationUnit = "";
            var dicWhereUnit = new Dictionary<string, object>();
            new CreateCriteria(dicWhereUnit).Add(ORMRestrictions<string>.Eq("Dylx", "08"))
                .Add(ORMRestrictions<Int32>.Eq("DefInt1", 1)).Add(ORMRestrictions<string>.Eq("Dwdm", userID));
            var CorrespondenceSettings = CorrespondenceSettingsRule.Find(dicWhereUnit);
            if (CorrespondenceSettings.Count > 0)
            {
                FDeclarationUnit = CorrespondenceSettings[0].Dydm;
            }
            DateTime? beforeDate = new DateTime(DateTime.Now.Year, 1, 1); //账务筛选时间
            var dicWheredate = new Dictionary<string, object>();
            new CreateCriteria(dicWheredate).Add(ORMRestrictions<string>.Eq("BZ", "G6HBLKZExpense"));
            var setModels = QTControlSetRule.Find(dicWheredate);//QTControlSetModel
            if (setModels.Count > 0)
            {
                beforeDate = setModels[0].BEGINFDATE;
            }

            string userConn = "";
            string select_DM;
            string Date_Dm;
            Dictionary<string, object> conndic = new Dictionary<string, object>();
            new CreateCriteria(conndic)
                .Add(ORMRestrictions<string>.Eq("Dylx", "config"))
                .Add(ORMRestrictions<string>.Eq("DefStr1", FDeclarationUnit));
            IList<CorrespondenceSettings2Model> CorrespondenceSettings2s = CorrespondenceSettings2Rule.RuleHelper.Find(conndic);
            if (CorrespondenceSettings2s.Count > 0 && CorrespondenceSettings2s[0].DefStr2 != null)
            {
                userConn = CorrespondenceSettings2s[0].DefStr2;

                if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    select_DM = @"SELECT sum(nvl(j,0))-sum(nvl(d,0)) FROM v_zw_pzhz WHERE zxyt= ";
                    Date_Dm = " and PZRQ < to_date('" + Convert.ToDateTime(beforeDate).ToString("yyyy-MM-dd") + "', 'yyyy/mm/dd hh24:mi:ss')";
                }
                else
                {
                    select_DM = @"SELECT sum(isnull(j,0))-sum(isnull(d,0)) FROM v_zw_pzhz WHERE zxyt= ";
                    Date_Dm = " and PZRQ < '" + Convert.ToDateTime(beforeDate).ToString("yyyy-MM-dd") + "'";
                }
                DataTable dataTable = null;
                DbHelper.Open(userConn);
                dataTable = DbHelper.GetDataTable(userConn, select_DM + "'" + code + "'" + Date_Dm);
                if (dataTable.Rows.Count != 0)
                {
                    result = dataTable.Rows[0][0].ToString();
                }
                DbHelper.Close(userConn);
            }
            else
            {
                return "0.00";
            }
            return result;
        }

        #endregion

        #region 工作流接口
        /// <summary>
        /// 流程发起时调用（一般用于修改表单状态为送审中、或是维护表单已挂工作流）
        /// </summary>
        /// <param name="ec"></param>
        public void FlowStart(WorkFlowExecutionContext ec)
        {
            long phid = Convert.ToInt64(ec.BillInfo.PK1);
            var mst = base.Find(phid);

            //更改状态为：审批中
            mst.Data.FApprovestatus = Convert.ToInt32(EnumApproveStatus.IsPending).ToString();
            mst.Data.FApprovedate = DateTime.Now;
            //mst.Data.FDateofdeclaration = DateTime.Now;
            CurrentRule.Update<Int64>(mst.Data);
        }

        /// <summary>
        /// 在审批任务执行前调用，在这里判断是否允许执行审批操作（现在流程中没有判断杜绝多个审批节点执行，所以单据状态为已审批也允许再次审批）
        /// </summary>
        /// <param name="ec"></param>
        /// <returns></returns>
        public ApproveValidResult CheckApproveValid(WorkFlowExecutionContext ec)
        {
            return NG3.WorkFlow.Interfaces.ApproveValidResult.Create(ApproveValidType.Yes, string.Empty);
        }

        /// <summary>
        ///  审批组件任务办理时调用（现在流程中没有判断杜绝多个审批节点执行，所以如果单据已审批就修改审批人、审批时间）
        /// </summary>
        /// <param name="ec"></param>
        public void Approve(WorkFlowExecutionContext ec)
        {

            long phid = Convert.ToInt64(ec.BillInfo.PK1);
            var mst = base.Find(phid);
            //更新状态为已审批
            if (mst.Data.FApprovestatus != Convert.ToInt32(EnumApproveStatus.Approved).ToString())
            {
                //20190107 审批完成时改为已审批，流程结束时才改项目状态
                //审批完成时,项目状态为预立项,则项目状态改为立项,审批状态改为未审批
                //if (mst.Data.FProjStatus == 1)
                //{
                //    mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.ToBeRepored).ToString();
                //    mst.Data.FProjStatus = 2;
                //}
                //else
                //{
                mst.Data.FApprovestatus = Convert.ToInt32(EnumApproveStatus.Approved).ToString();
                //}

            }
            mst.Data.FApprovedate = DateTime.Today;
            mst.Data.FApprover = base.UserID;
            CurrentRule.Update<Int64>(mst.Data);

            ////用 FlowEnd(), 在流程结束时进行操作(approve 方法 在进行审批节点后就会调用,可能存在多个审批节点)
            //long phid = Convert.ToInt64(ec.BillInfo.PK1);
            //var mst = base.Find(phid);
            ////更新状态为已审批
            //if (mst.Data.FApproveStatus != Convert.ToInt32(EnumApproveStatus.Approved).ToString())
            //{
            //    //审批完成时,项目状态为预立项,则项目状态改为立项,审批状态改为未审批
            //    if (mst.Data.FProjStatus == 1)
            //    {
            //        mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.ToBeRepored).ToString();
            //        mst.Data.FProjStatus = 2;
            //    }
            //    else
            //    {
            //        mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.Approved).ToString();
            //    }

            //    mst.Data.FApproveDate = DateTime.Now;
            //    mst.Data.FApprover = base.UserID;
            //    CurrentRule.Update<Int64>(mst.Data);
            //}

        }

        /// <summary>
        /// 审批节点回退前判断是否允许取消审批
        /// </summary>
        /// <param name="ec"></param>
        /// <returns></returns>
        public ApproveValidResult CheckCancelApproveValid(WorkFlowExecutionContext ec)
        {
            return NG3.WorkFlow.Interfaces.ApproveValidResult.Create(ApproveValidType.Yes, string.Empty);
        }

        /// <summary>
        /// 审批节点回退时调用进行单据取消审批操作
        /// </summary>
        /// <param name="ec"></param>
        public void CancelApprove(WorkFlowExecutionContext ec)
        {
            long phid = Convert.ToInt64(ec.BillInfo.PK1);
            var mst = base.Find(phid);
            //更新状态为审批中
            mst.Data.FApprovestatus = Convert.ToInt32(EnumApproveStatus.IsPending).ToString();
            // mst.Data.FDateofdeclaration = new Nullable<DateTime>();
            mst.Data.FApprovedate = new Nullable<DateTime>();
            CurrentRule.Update<Int64>(mst.Data);

            //throw new NotImplementedException();
        }

        /// <summary>
        /// 流程被终止时调用
        /// </summary>
        /// <param name="ec"></param>
        public void FlowAbort(WorkFlowExecutionContext ec)
        {
            long phid = Convert.ToInt64(ec.BillInfo.PK1);
            var mst = base.Find(phid);
            //更新状态为待上报
            mst.Data.FApprovestatus = Convert.ToInt32(EnumApproveStatus.ToBeRepored).ToString();
            mst.Data.FProjstatus = 1;//项目立项审批驳回-->从预立项从新修改，发起；状态默认预立项；
            //mst.Data.FDateofdeclaration = new Nullable<DateTime>();
            mst.Data.FApprovedate = new Nullable<DateTime>();
            CurrentRule.Update<Int64>(mst.Data);
        }

        /// <summary>
        /// 流程结束时调用
        /// </summary>
        /// <param name="ec"></param>
        public void FlowEnd(WorkFlowExecutionContext ec)
        {

            long phid = Convert.ToInt64(ec.BillInfo.PK1);
            var mst = base.Find(phid);
            //更新状态为已审批
            if (mst.Data.FApprovestatus != Convert.ToInt32(EnumApproveStatus.Approved).ToString())
            {
                mst.Data.FApprovestatus = Convert.ToInt32(EnumApproveStatus.Approved).ToString();

            }

            mst.Data.FApprovedate = DateTime.Today;
            mst.Data.FApprover = base.UserID;
            CurrentRule.Update<Int64>(mst.Data);


        }

        /// <summary>
        /// 新增、编辑\审核类组件任务执行时调用,方法参数中包含组件id
        /// </summary>
        /// <param name="compId"></param>
        /// <param name="ec"></param>
        public void EditUserTaskComplete(string compId, WorkFlowExecutionContext ec)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 获取单据转pdf所需的套打模块、及数据，用于APP端
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public BizToPdfEntity GetBizToPdfEntity(WorkFlowExecutionContext executionContext)
        {
            return null;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 获取单据附用（用于App端）
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public List<BizAttachment> GetBizAttachment(WorkFlowExecutionContext executionContext)
        {
            return new List<BizAttachment>();
        }

        /// <summary>
        /// app办理时如果单据有修改则调用该方法判断是否允许保存修改
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public ApproveValidResult CheckBizSaveByMobileApp(WorkFlowExecutionContext executionContext, Dictionary<string, string> jsonData)
        {
            return ApproveValidResult.DefaultValue;
        }

        /// <summary>
        /// app端办理时如果修改了单据内容则调用该方法进行单据保存。
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public bool SaveBizDataByMobileApp(WorkFlowExecutionContext executionContext, Dictionary<string, string> jsonData)
        {
            return true;
            //throw new NotImplementedException();
        }
        #endregion


        /// <summary>
        /// 获取审批中的单据id
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public List<string> GetApproveList(PagedResult<ExpenseMstModel> pageResult)
        {
            var idList = new List<string>();
            foreach (var item in pageResult.Results)
            {
                if (item.FApprovestatus == "2") 
                {
                    idList.Add(item.PhId.ToString());
                }
            }
            return idList;
        }

        /// <summary>
        /// 增加单据中下一审批节点名称
        /// </summary>
        /// <param name="pageResult"></param>
        /// <param name="BizType"> 审批流业务类型</param>
        /// <returns></returns>
        public PagedResult<ExpenseMstModel> AddNextApproveName(PagedResult<ExpenseMstModel> pageResult, string BizType)
        {
            var approveListId = GetApproveList(pageResult);
            if (approveListId.Count == 0)
            {
                foreach (var item in pageResult.Results)
                {
                    item.FNextApprove = "无";
                }
                return pageResult;
            }

            var Next_approve = WorkFlowHelper.GetFlowInfoByBizList(BizType, approveListId);

            foreach (var item in pageResult.Results)
            {
                if (item.FApprovestatus == "2")
                {
                    for (var i = 0; i < Next_approve.Count; i++)
                    {
                        if (Next_approve[i]["pk1"].ToString() == item.PhId.ToString() && Next_approve[i]["flow_status_name"].ToString() == "运行中")
                        {
                            item.FNextApprove = Next_approve[i]["curnodes"].ToString();
                            break;
                        }
                    }
                }
                else
                {
                    item.FNextApprove = "无";
                }
            }

            return pageResult;
        }

        #region//与审批相关

        /// <summary>
        /// 修改主表审批状态
        /// </summary>
        /// <param name="recordModel">传参对象</param>
        /// <param name="fApproval">审批状态</param>
        /// <returns></returns>
        public SavedResult<long> UpdateExpense(GAppvalRecordModel recordModel, string fApproval)
        {
            if (recordModel.RefbillPhid == 0)
                return null;
            SavedResult<long> savedResult = new SavedResult<long>();
            ExpenseMstModel expense = this.ExpenseMstRule.Find(recordModel.RefbillPhid);
            if(expense != null)
            {
                expense.FApprovestatus = fApproval;
                expense.FApprovedate = DateTime.Now;
                expense.FApprover = recordModel.OperaPhid;
                expense.FApprover_EXName = recordModel.OperaName;
                expense.PersistentState = PersistentState.Modified;
                savedResult = ExpenseMstRule.Save<Int64>(expense);
            }
            return savedResult;
        }

        /// <summary>
        /// 根据用款计划的主键获取相关数据集合
        /// </summary>
        /// <param name="phid">主键</param>
        /// <returns></returns>
        public ExpenseAllModel GetExpenseAllModel(long phid)
        {
            ExpenseAllModel expenseAll = new ExpenseAllModel();
            ExpenseMstModel expenseMst = new ExpenseMstModel();
            List<ExpenseDtlModel> expenseDtls = new List<ExpenseDtlModel>();
            List<ExpenseHxModel> expenseHxes = new List<ExpenseHxModel>();
            var expenses = this.ExpenseMstRule.Find(t => t.PhId == phid);
            if(expenses != null && expenses.Count > 0)
            {
                RichHelpDac helpdac = new RichHelpDac();
                helpdac.CodeToName<ExpenseMstModel>(expenses, "FDeclarationunit", "FDeclarationunit_EXName", "sb_orglist","");
                helpdac.CodeToName<ExpenseMstModel>(expenses, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode","");
                helpdac.CodeToName<ExpenseMstModel>(expenses, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist","");
                helpdac.CodeToName<ExpenseMstModel>(expenses, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory","");
                helpdac.CodeToName<ExpenseMstModel>(expenses, "FApprover", "FApprover_EXName", "fg3_user","");
                expenseMst = expenses[0];
                if(expenseMst.FApprovestatus != "3")
                {
                    throw new Exception("已审批的单据才能进行核销！");
                }
                if(expenseMst.FIfpurchase == 1 || expenseMst.FLifeCycle != 0)
                {
                    throw new Exception("作废据不能进行核销！");
                }
                decimal returnAllAmount = 0;
                decimal payAllAmount = 0;
                expenseDtls = this.ExpenseDtlRule.Find(t => t.MstPhid == phid).ToList(); //明细数据也就是额度返回的数据
                if(expenseDtls != null && expenseDtls.Count > 0)
                {
                    //RichHelpDac helpdac = new RichHelpDac();
                    helpdac.CodeToName<ExpenseDtlModel>(expenseDtls, "FBudgetaccounts", "FBudgetaccounts_EXName", "GHBudgetAccounts", "");
                    helpdac.CodeToName<ExpenseDtlModel>(expenseDtls, "FExpenseschannel", "FExpenseschannel_EXName", "GHExpensesChannel", "");
                    helpdac.CodeToName<ExpenseDtlModel>(expenseDtls, "FSourceoffunds", "FSourceoffunds_EXName", "GHSourceOfFunds", "");
                    foreach (var dtl in expenseDtls)
                    {
                        returnAllAmount = returnAllAmount + dtl.FAmount;
                        payAllAmount = payAllAmount + dtl.FAmount;
                    }
                }         
                expenseHxes = this.ExpenseHxRule.Find(t => t.MstPhid == phid).ToList();//核销明细数据
                if(expenseHxes != null && expenseHxes.Count > 0)
                {
                    foreach (var hx in expenseHxes)
                    {
                        returnAllAmount = returnAllAmount - hx.FAmount;
                    }
                }
                expenseAll.ExpenseMst = expenseMst;
                expenseAll.ExpenseDtls = expenseDtls;
                expenseAll.ExpenseHxs = expenseHxes;
                expenseAll.ReturnAllAmount = returnAllAmount;
                expenseAll.PayAllAmount = payAllAmount;
            }
            else
            {
                throw new Exception("此用款计划数据不存在！");
            }
            return expenseAll;
        }
        #endregion
    }
}

