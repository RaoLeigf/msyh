#region Summary
/**************************************************************************************
    * 类 名 称：        ExpenseMstService
    * 命名空间：        GYS3.YS.Service
    * 文 件 名：        ExpenseMstService.cs
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
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Service;

using GYS3.YS.Service.Interface;
using GYS3.YS.Facade.Interface;
using GYS3.YS.Model.Domain;
using Enterprise3.Common.Base.Criterion;
using SUP.Common.Base;
using GYS3.YS.Model.Extra;
using SUP.Common.DataAccess;
using GQT3.QT.Model.Domain;
using GQT3.QT.Facade.Interface;

namespace GYS3.YS.Service
{
    /// <summary>
    /// ExpenseMst服务组装处理类
    /// </summary>
    public partial class ExpenseMstService : EntServiceBase<ExpenseMstModel>, IExpenseMstService
    {
        #region 类变量及属性
        /// <summary>
        /// ExpenseMst业务外观处理对象
        /// </summary>
        IExpenseMstFacade ExpenseMstFacade
        {
            get
            {
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IExpenseMstFacade;
            }
        }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
        private IExpenseDtlFacade ExpenseDtlFacade { get; set; }

        private IBudgetDtlBudgetDtlFacade BudgetDtlBudgetDtlFacade { get; set; }

        private IExpenseHxFacade ExpenseHxFacade { get; set; }

        private IBudgetMstFacade BudgetMstFacade { get; set; }

        private IQTSysSetFacade QTSysSetFacade { get; set; }
        #endregion

        #region 实现 IExpenseMstService 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="expenseMstEntity"></param>
        /// <param name="expenseDtlEntities"></param>
        /// <param name="NCmoney">年初预算金额</param>
        /// <param name="beforeSum">本单据初始预计支出金额</param>
        /// <param name="beforeFReturnamount">本单据初始预计返还金额</param>
        /// <param name="Ifreturn">是否额度返还</param>
        /// <returns></returns>
        public SavedResult<Int64> SaveExpenseMst(ExpenseMstModel expenseMstEntity, List<ExpenseDtlModel> expenseDtlEntities, string NCmoney, string beforeSum, string beforeFReturnamount, string Ifreturn)
        {
            return ExpenseMstFacade.SaveExpenseMst(expenseMstEntity, expenseDtlEntities, NCmoney, SumFSurplusamount(expenseMstEntity.FProjcode), beforeSum, beforeFReturnamount, Ifreturn);
        }

        /// <summary>
        /// 通过外键值获取ExpenseDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<ExpenseDtlModel> FindExpenseDtlByForeignKey<TValType>(TValType id)
        {
            return ExpenseDtlFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 通过外键值获取ExpenseHx明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<ExpenseHxModel> FindExpenseHxByForeignKey<Int64>(Int64 id)
        {
            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<Int64>.Eq("MstPhid", id));

            return ExpenseHxFacade.Find(dicWhere);
        }

        /// <summary>
        /// 根据项目代码取预计支出金额的和
        /// </summary>
        /// <param name="FProjCode"></param>
        /// <returns></returns>
        public string SumFSurplusamount(string FProjCode)
        {
            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FProjcode", FProjCode))
                .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
                .Add(ORMRestrictions<System.Int32>.NotEq("FIfpurchase", 1));//是否额度返还
            var findedresultmst = base.Find(dicWhere).Data;
            Decimal Sum = 0;
            for (var i = 0; i < findedresultmst.Count; i++)
            {
                Sum += findedresultmst[i].FSurplusamount;
                Sum -= findedresultmst[i].FReturnamount;
            }
            return Sum.ToString();
        }

        /// <summary>
        /// 额度逆返还
        /// </summary>
        /// <param name="id">额度返还的单据phid</param>
        /// <returns></returns>
        public CommonResult DeleteReturn(long id)
        {
            ExpenseMstModel expenseMstEntity = base.Find(id).Data;//额度返还的单据
            decimal FPlayamount = expenseMstEntity.FProjAmount - decimal.Parse(SumFSurplusamount(expenseMstEntity.FProjcode)) - expenseMstEntity.FReturnamount;
            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FBillno", id.ToString()));
            var findedresultmst = base.Find(dicWhere).Data;
            //if (findedresultmst.Count > 0)
            //{
            ExpenseMstModel expenseMstEntity2 = findedresultmst[0];//额度返还之前的单据
            expenseMstEntity2.FIfpurchase = 0;
            expenseMstEntity2.FBillno = "";
            expenseMstEntity2.FPlayamount = FPlayamount;
            expenseMstEntity2.PersistentState = SUP.Common.Base.PersistentState.Modified;
            //}

            var dicWhere2 = new Dictionary<string, object>();
            new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FProjcode", expenseMstEntity.FProjcode))
                    .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
                    .Add(ORMRestrictions<System.Int32>.NotEq("FIfpurchase", 1))
                    .Add(ORMRestrictions<System.Int64>.NotEq("PhId", expenseMstEntity.PhId));
            IList<ExpenseMstModel> ExpenseMstList = base.Find(dicWhere2).Data;
            //List<ExpenseMstModel> ExpenseMstList = ExpenseMstList2.ToList().FindAll(t => t.FIfpurchase != 1);
            //只剩一条单据的时候
            /*if (ExpenseMstList.Count == 1)
            {
                ExpenseMstList[0].FProjname = ExpenseMstList[0].FProjname.Replace("-" + ExpenseMstList[0].FIfperformanceappraisal.ToString(), "");
                ExpenseMstList[0].FIfperformanceappraisal = 1;
                //删除额度返还的原单据
                if (ExpenseMstList[0].FIfpurchase == 2)
                {
                    List<ExpenseMstModel> ExpenseMstList3 = ExpenseMstList2.ToList().FindAll(t => t.FBillno == ExpenseMstList[0].PhId.ToString());
                    if (ExpenseMstList3.Count > 0)
                    {
                        ExpenseMstList3[0].FProjname.Replace("-" + ExpenseMstList3[0].FIfperformanceappraisal.ToString(), "");
                        ExpenseMstList3[0].FIfperformanceappraisal = 1;
                        ExpenseMstFacade.Save<Int64>(ExpenseMstList3);
                    }
                }
            }*/
            for (var i = 0; i < ExpenseMstList.Count; i++)
            {
                ExpenseMstList[i].FPlayamount = FPlayamount;
                ExpenseMstList[i].PersistentState = SUP.Common.Base.PersistentState.Modified;
                //base.Save<Int64>(ExpenseMstList[i]);
            }
            ExpenseMstFacade.Save<Int64>(ExpenseMstList);
            ExpenseMstFacade.Save<Int64>(expenseMstEntity2);
            CommonResult result = base.Delete<System.Int64>(id);
            return result;
        }

        /// <summary>
        /// 删除正常单据 可编报数变更
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CommonResult Delete2(long id)
        {
            ExpenseMstModel expenseMstEntity = base.Find(id).Data;
            if (expenseMstEntity.FIfpurchase == 0)
            {
                decimal FPlayamount = expenseMstEntity.FPlayamount + expenseMstEntity.FSurplusamount;
                var dicWhere2 = new Dictionary<string, object>();
                new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FProjcode", expenseMstEntity.FProjcode))
                        .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
                        //.Add(ORMRestrictions<System.Int32>.NotEq("FIfpurchase", 1))
                        .Add(ORMRestrictions<System.Int64>.NotEq("PhId", expenseMstEntity.PhId));
                IList<ExpenseMstModel> ExpenseMstList2 = base.Find(dicWhere2).Data;

                List<ExpenseMstModel> ExpenseMstList = ExpenseMstList2.ToList().FindAll(t => t.FIfpurchase != 1);
                //只剩一条单据的时候
                if (ExpenseMstList.Count == 1)
                {
                    ExpenseMstList[0].FProjname = ExpenseMstList[0].FProjname.Replace("-" + ExpenseMstList[0].FIfperformanceappraisal.ToString(), "");
                    ExpenseMstList[0].FIfperformanceappraisal = 1;
                    //删除额度返还的原单据
                    if (ExpenseMstList[0].FIfpurchase == 2)
                    {
                        List<ExpenseMstModel> ExpenseMstList3 = ExpenseMstList2.ToList().FindAll(t => t.FBillno == ExpenseMstList[0].PhId.ToString());
                        if (ExpenseMstList3.Count > 0)
                        {
                            ExpenseMstList3[0].FProjname.Replace("-" + ExpenseMstList3[0].FIfperformanceappraisal.ToString(), "");
                            ExpenseMstList3[0].FIfperformanceappraisal = 1;
                            ExpenseMstList3[0].PersistentState = PersistentState.Modified;
                            ExpenseMstFacade.Save<Int64>(ExpenseMstList3);
                        }
                    }
                }
                for (var i = 0; i < ExpenseMstList.Count; i++)
                {
                    ExpenseMstList[i].FPlayamount = FPlayamount;
                    ExpenseMstList[i].PersistentState = SUP.Common.Base.PersistentState.Modified;
                }
                ExpenseMstFacade.Save<Int64>(ExpenseMstList);
            }
            CommonResult result = base.Delete(id);
            return result;
        }

        /// <summary>
        /// 根据主表phid取明细剩余金额
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public string RestOfAmount(long id, string code)
        {
            List<string> YskmList = new List<string>();
            List<Decimal> AmountList = new List<Decimal>();
            IList<BudgetDtlBudgetDtlModel> dtl = BudgetDtlBudgetDtlFacade.FindByForeignKey(id).Data;
            for (var a = 0; a < dtl.Count; a++)
            {

                if (YskmList.IndexOf(dtl[a].FBudgetAccounts) < 0)
                {
                    YskmList.Add(dtl[a].FBudgetAccounts);
                    AmountList.Add(dtl[a].FAmount + dtl[a].FAmountEdit);
                }
                else
                {
                    AmountList[YskmList.IndexOf(dtl[a].FBudgetAccounts)] += dtl[a].FAmount + dtl[a].FAmountEdit;
                }
            }
            var dicWhere2 = new Dictionary<string, object>();
            new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FProjcode", code))
                    .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
                    .Add(ORMRestrictions<System.Int32>.NotEq("FIfpurchase", 1));
            IList<ExpenseMstModel> ExpenseMstList = ExpenseMstFacade.Find(dicWhere2).Data;
            for (var i = 0; i < ExpenseMstList.Count; i++)
            {
                IList<ExpenseDtlModel> ExpenseDtlList = ExpenseDtlFacade.FindByForeignKey(ExpenseMstList[i].PhId).Data;
                for (var j = 0; j < ExpenseDtlList.Count; j++)
                {
                    var yskmindex = YskmList.IndexOf(ExpenseDtlList[j].FBudgetaccounts);
                    AmountList[yskmindex] = AmountList[yskmindex] - ExpenseDtlList[j].FAmount + ExpenseDtlList[j].FReturnamount;
                }
            }
            var data = new
            {
                YskmList = YskmList,
                AmountList = AmountList
            };

            //return DataConverterHelper.EntityListToJson(findedresultbudgetdtlbudgetdtl.Data, findedresultbudgetdtlbudgetdtl.Data.Count);
            return DataConverterHelper.SerializeObject(data);
        }
        /*
        /// <summary>
        /// 项目支出预算情况查询
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagedResult<ExpenseMstModel> GetXmZcYs(string userID, int pageIndex, int pageSize)
        {
            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", DateTime.Now.Year.ToString()));
            //PagedResult<ExpenseMstModel> expenseMsts= ExpenseMstFacade.LoadWithPageForDept("GYS.YS.getZCbudget", dicWhere);
            PagedResult<ExpenseMstModel> expenseMsts = ExpenseMstFacade.LoadWithPage(pageIndex, pageSize, "GYS.YS.getZCbudget", dicWhere); 
            PagedResult<ExpenseMstModel> expenseMsts2 = ExpenseMstFacade.GetSJFSS(userID, expenseMsts);
            return expenseMsts2;
        }*/

        /// <summary>
        /// 通过项目代码和操作员获取财务实际发生数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetSJFSSbyCode(string userID, string code)
        {
            return ExpenseMstFacade.GetSJFSSbyCode(userID, code);
        }

        /// <summary>
        /// 保存额度核销数据
        /// </summary>
        /// <param name="adddata"></param>
        /// <param name="updatedata"></param>
        /// <param name="deletedata"></param>
        /// <returns></returns>
        public CommonResult SaveHX(List<ExpenseHxModel> adddata, List<ExpenseHxModel> updatedata, List<string> deletedata)
        {
            CommonResult result = new CommonResult();
            if (adddata != null && adddata.Count > 0)
            {
                for (var i = 0; i < adddata.Count; i++)
                {
                    ExpenseHxModel Hx = adddata[i];
                    Hx.PersistentState = PersistentState.Added;
                    ExpenseHxFacade.Save<Int64>(Hx, "");
                }

            }
            if (updatedata != null && updatedata.Count > 0)
            {
                for (var j = 0; j < updatedata.Count; j++)
                {
                    ExpenseHxModel Hx2 = updatedata[j];
                    ExpenseHxModel Hx3 = ExpenseHxFacade.Find(Hx2.PhId).Data;
                    Hx3.FHxdateTime = Hx2.FHxdateTime;
                    Hx3.FAmount = Hx2.FAmount;
                    Hx3.FContentCode = Hx2.FContentCode;
                    Hx3.FRemark = Hx2.FRemark;
                    Hx3.FCode = Hx2.FCode;
                    Hx3.FName = Hx2.FName;
                    Hx3.PersistentState = PersistentState.Modified;
                    ExpenseHxFacade.Save<Int64>(Hx3, "");
                }

            }
            if (deletedata != null && deletedata.Count > 0)
            {
                for (var x = 0; x < deletedata.Count; x++)
                {
                    ExpenseHxFacade.Delete(deletedata[x]);
                }
            }
            return result;
        }

        /// <summary>
        /// 额度核销执行完毕确认
        /// </summary>
        /// <param name="id"></param>
        /// <param name="FPlayamount"></param>
        /// <param name="FReturnamount"></param>
        /// <param name="dtls"></param>
        /// <returns></returns>
        public CommonResult SaveHXgo(long id, Decimal FPlayamount, Decimal FReturnamount, List<ExpenseDtlModel> dtls)
        {
            CommonResult result = new CommonResult();
            ExpenseMstModel mst = ExpenseMstFacade.Find(id).Data;
            mst.FPlayamount = FPlayamount;
            mst.FIfKeyEvaluation = 1;
            mst.FReturnamount = FReturnamount;
            mst.PersistentState = PersistentState.Modified;

            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FProjcode", mst.FProjcode))
                    .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
                    .Add(ORMRestrictions<System.Int32>.NotEq("FIfpurchase", 1))
                    .Add(ORMRestrictions<System.Int64>.NotEq("PhId", mst.PhId));
            IList<ExpenseMstModel> ExpenseMstList = ExpenseMstFacade.Find(dicWhere).Data;
            for (var i = 0; i < ExpenseMstList.Count; i++)
            {
                ExpenseMstList[i].FPlayamount = FPlayamount;
                ExpenseMstList[i].PersistentState = PersistentState.Modified;
            }
            ExpenseMstList.Add(mst);
            result = ExpenseMstFacade.FacadeHelper.Save<long>(ExpenseMstList);

            List<ExpenseDtlModel> ExpenseDtlList=new List<ExpenseDtlModel>();
            for (var j=0;j< dtls.Count; j++)
            {
                ExpenseDtlModel dtl = ExpenseDtlFacade.Find(dtls[j].PhId).Data;
                dtl.FReturnamount = dtls[j].FReturnamount;
                dtl.PersistentState= PersistentState.Modified;
                ExpenseDtlList.Add(dtl);
            }
            result = ExpenseDtlFacade.FacadeHelper.Save<long>(ExpenseDtlList);
            return result;
        }

        /// <summary>
        /// 额度核销撤销
        /// </summary>
        /// <param name="id"></param>
        /// <param name="FPlayamount"></param>
        /// <returns></returns>
        public CommonResult SaveHXreturn(long id, Decimal FPlayamount)
        {
            CommonResult result = new CommonResult();
            ExpenseMstModel mst = ExpenseMstFacade.Find(id).Data;
            mst.FPlayamount = FPlayamount;
            mst.FIfKeyEvaluation = 0;
            mst.FReturnamount = 0;
            mst.PersistentState = PersistentState.Modified;

            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FProjcode", mst.FProjcode))
                    .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
                    .Add(ORMRestrictions<System.Int32>.NotEq("FIfpurchase", 1))
                    .Add(ORMRestrictions<System.Int64>.NotEq("PhId", mst.PhId));
            IList<ExpenseMstModel> ExpenseMstList = ExpenseMstFacade.Find(dicWhere).Data;
            for (var i = 0; i < ExpenseMstList.Count; i++)
            {
                ExpenseMstList[i].FPlayamount = FPlayamount;
                ExpenseMstList[i].PersistentState = PersistentState.Modified;
            }
            ExpenseMstList.Add(mst);
            result = ExpenseMstFacade.FacadeHelper.Save<long>(ExpenseMstList);

            IList<ExpenseDtlModel> ExpenseDtlList = ExpenseDtlFacade.FindByForeignKey(id).Data;
            for(var j = 0; j < ExpenseDtlList.Count; j++)
            {
                ExpenseDtlList[j].FReturnamount = 0;
                ExpenseDtlList[j].PersistentState = PersistentState.Modified;
            }
            result = ExpenseDtlFacade.FacadeHelper.Save<long>(ExpenseDtlList);
            return result;
        }

        /// <summary>
        /// 根据项目代码获取额度核销总金额
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public decimal GetHXsumByCode(string code)
        {
            decimal sum = 0;
            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FProjcode", code))
                    .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
                    .Add(ORMRestrictions<System.Int32>.NotEq("FIfpurchase", 1));
            IList<ExpenseMstModel> ExpenseMstList = ExpenseMstFacade.Find(dicWhere).Data;
            if (ExpenseMstList.Count > 0)
            {
                for(var i=0;i< ExpenseMstList.Count; i++)
                {
                    if (ExpenseMstList[i].FIfKeyEvaluation == 1)
                    {
                        var dicWhere2 = new Dictionary<string, object>();
                        new CreateCriteria(dicWhere2).Add(ORMRestrictions<Int64>.Eq("MstPhid", ExpenseMstList[i].PhId));
                        IList<ExpenseHxModel> ExpenseHxList = ExpenseHxFacade.Find(dicWhere2).Data;
                        if (ExpenseHxList.Count > 0)
                        {
                            for(var j=0;j< ExpenseHxList.Count; j++)
                            {
                                sum += ExpenseHxList[j].FAmount;
                            }
                        }
                    }
                }
            }
            return sum;
        }

        /// <summary>
        /// 根据预算主键获取信息
        /// </summary>
        /// <param name="YsPhid"></param>
        /// <returns></returns>
        public object GetinfoByProjCode(long YsPhid)
        {
            //一些必要数据加入中文名
            RichHelpDac helpdac = new RichHelpDac();
            BudgetMstModel budgetMst = BudgetMstFacade.Find(YsPhid).Data;
            helpdac.CodeToName<BudgetMstModel>(budgetMst, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory");
            string FExpenseCategory = budgetMst.FExpenseCategory;//项目类型
            string FExpenseCategory_EXName = budgetMst.FExpenseCategory_EXName;
            Decimal FProjAmount = budgetMst.FBudgetAmount;//核定预算数
            IList<BudgetDtlBudgetDtlModel> budgetDtls = BudgetDtlBudgetDtlFacade.FacadeHelper.FindByForeignKey(YsPhid).Data;

            helpdac.CodeToName<BudgetDtlBudgetDtlModel>(budgetDtls, "FSourceOfFunds", "FSourceOfFunds_EXName", "GHSourceOfFunds", "");
            helpdac.CodeToName<BudgetDtlBudgetDtlModel>(budgetDtls, "FPaymentMethod", "FPaymentMethod_EXName", "GHPaymentMethod", "");
            helpdac.CodeToName<BudgetDtlBudgetDtlModel>(budgetDtls, "FExpensesChannel", "FExpensesChannel_EXName", "GHExpensesChannel", "");
            helpdac.CodeToName<BudgetDtlBudgetDtlModel>(budgetDtls, "FBudgetAccounts", "FBudgetAccounts_EXName", "GHBudgetAccounts", "");
            helpdac.CodeToName<BudgetDtlBudgetDtlModel>(budgetDtls, "FQtZcgnfl", "FQtZcgnfl_EXName", "GHQtZcgnfl", "");

            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FProjcode", budgetMst.FProjCode))
                .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
                .Add(ORMRestrictions<System.Int32>.NotEq("FIfpurchase", 1));//去除被额度返还的原单据
            var findedresultmst = base.Find(dicWhere).Data;
            
            Decimal FrozenSum = 0;//冻结数
            Decimal UsedSum = 0;//已使用数
           
            for (var i = 0; i < findedresultmst.Count; i++)
            {
                if (findedresultmst[i].FApprovestatus == "3"|| findedresultmst[i].FApprovestatus == "4")
                {
                    UsedSum += findedresultmst[i].FSurplusamount;
                    UsedSum -= findedresultmst[i].FReturnamount;
                }
                else
                {
                    FrozenSum += findedresultmst[i].FSurplusamount;
                    FrozenSum -= findedresultmst[i].FReturnamount;
                }
            }
            Decimal FPlayamount = FProjAmount- FrozenSum- UsedSum;//可编报数

            Dictionary<string, Decimal> Yskm_Amount = new Dictionary<string, Decimal>();
            List<Decimal> AmountList = new List<Decimal>();
            for (var a = 0; a < budgetDtls.Count; a++)
            {
                if (!Yskm_Amount.ContainsKey(budgetDtls[a].FBudgetAccounts))
                {
                    Yskm_Amount.Add(budgetDtls[a].FBudgetAccounts, budgetDtls[a].FBudgetAmount);
                }
                else
                {
                    Yskm_Amount[budgetDtls[a].FBudgetAccounts] += budgetDtls[a].FBudgetAmount;
                }
            }
            //去除已经做的单据
            var dicWhere2 = new Dictionary<string, object>();
            new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FProjcode", budgetMst.FProjCode))
                    .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
                    .Add(ORMRestrictions<System.Int32>.NotEq("FIfpurchase", 1));
            IList<ExpenseMstModel> ExpenseMstList = ExpenseMstFacade.Find(dicWhere2).Data;
            for (var i = 0; i < ExpenseMstList.Count; i++)
            {
                IList<ExpenseDtlModel> ExpenseDtlList = ExpenseDtlFacade.FindByForeignKey(ExpenseMstList[i].PhId).Data;
                for (var j = 0; j < ExpenseDtlList.Count; j++)
                {
                    if (!string.IsNullOrEmpty(ExpenseDtlList[j].FBudgetaccounts) && Yskm_Amount.ContainsKey(ExpenseDtlList[j].FBudgetaccounts))
                    {
                        Yskm_Amount[ExpenseDtlList[j].FBudgetaccounts] = Yskm_Amount[ExpenseDtlList[j].FBudgetaccounts] - ExpenseDtlList[j].FAmount + ExpenseDtlList[j].FReturnamount;
                    }
                }
            }

            object result = new
            {
                FExpenseCategory = FExpenseCategory,
                FExpenseCategory_EXName = FExpenseCategory_EXName,
                FProjAmount = FProjAmount,
                FrozenSum = FrozenSum,
                UsedSum = UsedSum,
                FPlayamount = FPlayamount,
                Yskm_Amount = Yskm_Amount,
                budgetDtls= budgetDtls
            };
            return result;
        }

        /// <summary>
        /// 根据预算部门取项目支出预算申报总数、申报总额、有哪些项目及金额饼图
        /// </summary>
        /// <param name="Dept"></param>
        /// <param name="Year"></param>
        /// <returns></returns>
        public object GetinfoByDept2(string Dept,string Year)
        {
            var dicWhereys = new Dictionary<string, object>();
            new CreateCriteria(dicWhereys)
                .Add(ORMRestrictions<string>.Eq("FBudgetDept", Dept))
                .Add(ORMRestrictions<string>.Eq("FYear", Year))
                .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
                .Add(ORMRestrictions<string>.Eq("FApproveStatus", "3"))
                .Add(ORMRestrictions<String>.Eq("FMidYearChange", "0"));
            var budgetMsts = BudgetMstFacade.Find(dicWhereys).Data;
            Decimal MstsFProjAmount = 0;
            Decimal MstsFrozenSum = 0;
            Decimal MstsUsedSum = 0;
            Decimal MstsFPlayamount = 0;
            List<object> Msts = new List<object>();
            if (budgetMsts.Count > 0)
            {
                foreach (var budgetMst in budgetMsts)
                {
                    Decimal FProjAmount = budgetMst.FBudgetAmount;//核定预算数
                    Decimal FrozenSum = 0;//冻结数
                    Decimal UsedSum = 0;//已使用数
                    var dicWhere = new Dictionary<string, object>();
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FProjcode", budgetMst.FProjCode))
                        .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
                        .Add(ORMRestrictions<System.Int32>.NotEq("FIfpurchase", 1));//去除被额度返还的原单据
                    var findedresultmst = base.Find(dicWhere).Data;

                    for (var i = 0; i < findedresultmst.Count; i++)
                    {
                        if (findedresultmst[i].FApprovestatus == "3" || findedresultmst[i].FApprovestatus == "4")
                        {
                            UsedSum += findedresultmst[i].FSurplusamount;
                            UsedSum -= findedresultmst[i].FReturnamount;
                        }
                        else
                        {
                            FrozenSum += findedresultmst[i].FSurplusamount;
                            FrozenSum -= findedresultmst[i].FReturnamount;
                        }
                    }
                    Decimal FPlayamount = FProjAmount - FrozenSum - UsedSum;//可编报数
                    object Mst = new
                    {
                        FProjAmount = FProjAmount,
                        FrozenSum= FrozenSum,
                        UsedSum= UsedSum,
                        FPlayamount= FPlayamount,
                        data= budgetMst
                    };
                    MstsFProjAmount += FProjAmount;
                    MstsFrozenSum += FrozenSum;
                    MstsUsedSum += UsedSum;
                    MstsFPlayamount += FPlayamount;
                    Msts.Add(Mst);
                }
            }
            object result = new
            {
                Msts= Msts,
                MstsFProjAmount = MstsFProjAmount,
                MstsFrozenSum= MstsFrozenSum,
                MstsUsedSum= MstsUsedSum,
                MstsFPlayamount= MstsFPlayamount,
                Num = budgetMsts.Count
            };
            return result;
        }

        /// <summary>
        /// 根据预算部门取项目支出预算申报总数、申报总额、有哪些项目及金额饼图
        /// </summary>
        /// <param name="Dept"></param>
        /// <param name="Year"></param>
        /// <returns></returns>
        public object GetinfoByDept(string Dept, string Year)
        {
            var dicWhereys = new Dictionary<string, object>();
            new CreateCriteria(dicWhereys)
                .Add(ORMRestrictions<string>.Eq("FBudgetDept", Dept))
                .Add(ORMRestrictions<string>.Eq("FYear", Year))
                .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
                .Add(ORMRestrictions<string>.Eq("FApproveStatus", "3"))
                .Add(ORMRestrictions<String>.Eq("FMidYearChange", "0"));
            var budgetMsts = BudgetMstFacade.Find(dicWhereys).Data;
            Decimal MstsFProjAmount = 0;
            Decimal MstsFrozenSum = 0;
            Decimal MstsUsedSum = 0;
            Decimal MstsFPlayamount = 0;
            List<object> Msts = new List<object>();
            if (budgetMsts != null && budgetMsts.Count > 0)
            {
                //取出所有这些预算对应的用款单据
                List<string> budList = budgetMsts.Select(t => t.FProjCode).Distinct().ToList();
                IList<ExpenseMstModel> findedresultmst = new List<ExpenseMstModel>();
                IList<ExpenseDtlModel> expenseDtls = new List<ExpenseDtlModel>();
                IList<QTSysSetModel> qTSysSets = new List<QTSysSetModel>();
                if (budList != null && budList.Count > 0)
                {
                    var dicWhere = new Dictionary<string, object>();
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<List<string>>.In("FProjcode", budList))
                        .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
                        .Add(ORMRestrictions<System.Int32>.NotEq("FIfpurchase", 1));//去除被额度返还的原单据
                    findedresultmst = base.Find(dicWhere).Data;

                    if(findedresultmst != null && findedresultmst.Count > 0)
                    {
                        //所有用款单据的主键
                        List<long> exPhids = findedresultmst.Select(t => t.PhId).Distinct().ToList();
                        if(exPhids != null && exPhids.Count > 0)
                        {
                            expenseDtls = this.ExpenseDtlFacade.Find(t => exPhids.Contains(t.MstPhid)).Data;
                        }
                        //所有用款单据的申报单位
                        List<string> orgList = findedresultmst.Select(t => t.FDeclarationunit).Distinct().ToList();
                        if(orgList != null && orgList.Count > 0)
                        {
                            qTSysSets = this.QTSysSetFacade.Find(t => t.DicType == "ZjlzCode" && orgList.Contains(t.Orgcode)).Data;
                        }
                    }
                }

                foreach (var budgetMst in budgetMsts)
                {
                    if(budgetMst.FProjCode == "2019101.010192")
                    {
                        var ff = 1;
                    }
                    Decimal FProjAmount = budgetMst.FBudgetAmount;//核定预算数
                    Decimal FrozenSum = 0;//冻结数
                    Decimal UsedSum = 0;//已使用数
                    //先去该项目的用款主表数据
                    if(findedresultmst != null && findedresultmst.Count > 0)
                    {
                        var expMsts = findedresultmst.ToList().FindAll(t => t.FProjcode == budgetMst.FProjCode);
                        if(expMsts != null && expMsts.Count > 0)
                        {
                            var mstPhids = expMsts.Select(t => t.PhId).ToList();
                            if(mstPhids != null && mstPhids.Count > 0 && expenseDtls != null && expenseDtls.Count > 0)
                            {
                                var expDtls = expenseDtls.ToList().FindAll(t => mstPhids.Contains(t.MstPhid));
                                if(expDtls != null && expMsts.Count > 0)
                                {
                                    for (var i = 0; i < expDtls.Count; i++)
                                    {
                                        var mst = findedresultmst.ToList().Find(t => t.PhId == expDtls[i].MstPhid);
                                        if(qTSysSets != null && qTSysSets.Count > 0)
                                        {
                                            var set = qTSysSets.ToList().Find(t => t.Orgcode == mst.FDeclarationunit);
                                            if(set != null)
                                            {
                                                if(set.Value == expDtls[i].FPaymentmethod)
                                                {
                                                    if (mst.FApprovestatus == "3" || mst.FApprovestatus == "4")
                                                    {
                                                        UsedSum += expDtls[i].FAmount + expDtls[i].FAmountedit;
                                                        UsedSum -= expDtls[i].FReturnamount;
                                                    }
                                                    else
                                                    {
                                                        FrozenSum += expDtls[i].FAmount + expDtls[i].FAmountedit;
                                                        FrozenSum -= expDtls[i].FReturnamount;
                                                    }
                                                }

                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                   
                    Decimal FPlayamount = FProjAmount - FrozenSum - UsedSum;//可编报数
                    object Mst = new
                    {
                        FProjAmount = FProjAmount,
                        FrozenSum = FrozenSum,
                        UsedSum = UsedSum,
                        FPlayamount = FPlayamount,
                        data = budgetMst
                    };
                    MstsFProjAmount += FProjAmount;
                    MstsFrozenSum += FrozenSum;
                    MstsUsedSum += UsedSum;
                    MstsFPlayamount += FPlayamount;
                    Msts.Add(Mst);
                }
            }
            object result = new
            {
                Msts = Msts,
                MstsFProjAmount = MstsFProjAmount,
                MstsFrozenSum = MstsFrozenSum,
                MstsUsedSum = MstsUsedSum,
                MstsFPlayamount = MstsFPlayamount,
                Num = budgetMsts.Count
            };
            return result;
        }
        /// <summary>
        /// 根据用款计划的主键获取相关数据集合
        /// </summary>
        /// <param name="phid">主键</param>
        /// <returns></returns>
        public ExpenseAllModel GetExpenseAllModel(long phid)
        {
            return this.ExpenseMstFacade.GetExpenseAllModel(phid);
        }
        #endregion
    }
}

