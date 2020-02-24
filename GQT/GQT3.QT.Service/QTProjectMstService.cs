#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service
    * 类 名 称：			QTProjectMstService
    * 文 件 名：			QTProjectMstService.cs
    * 创建时间：			2019/9/4 
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
using SUP.Common.DataEntity;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Service;

using GQT3.QT.Service.Interface;
using GQT3.QT.Facade.Interface;
using GQT3.QT.Model.Domain;
using Enterprise3.Common.Base.Criterion;
using GQT3.QT.Model.Extra;

namespace GQT3.QT.Service
{
	/// <summary>
	/// QTProjectMst服务组装处理类
	/// </summary>
    public partial class QTProjectMstService : EntServiceBase<QTProjectMstModel>, IQTProjectMstService
    {
		#region 类变量及属性
		/// <summary>
        /// QTProjectMst业务外观处理对象
        /// </summary>
		IQTProjectMstFacade QTProjectMstFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IQTProjectMstFacade;
            }
        }

		/// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IQTProjectDtlBudgetDtlFacade QTProjectDtlBudgetDtlFacade { get; set; }

		/// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IQTProjectDtlFundApplFacade QTProjectDtlFundApplFacade { get; set; }

		/// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IQTProjectDtlImplPlanFacade QTProjectDtlImplPlanFacade { get; set; }

		/// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IQTProjectDtlPerformTargetFacade QTProjectDtlPerformTargetFacade { get; set; }

		/// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IQTProjectDtlPurchaseDtlFacade QTProjectDtlPurchaseDtlFacade { get; set; }

		/// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IQTProjectDtlPurDtl4SOFFacade QTProjectDtlPurDtl4SOFFacade { get; set; }

		/// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IQTProjectDtlTextContentFacade QTProjectDtlTextContentFacade { get; set; }
		#endregion

		#region 实现 IQTProjectMstService 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="qTProjectMstEntity"></param>
		/// <param name="qTProjectDtlBudgetDtlEntities"></param>
		/// <param name="qTProjectDtlFundApplEntities"></param>
		/// <param name="qTProjectDtlImplPlanEntities"></param>
		/// <param name="qTProjectDtlPerformTargetEntities"></param>
		/// <param name="qTProjectDtlPurchaseDtlEntities"></param>
		/// <param name="qTProjectDtlPurDtl4SOFEntities"></param>
		/// <param name="qTProjectDtlTextContentEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveQTProjectMst(QTProjectMstModel qTProjectMstEntity, List<QTProjectDtlBudgetDtlModel> qTProjectDtlBudgetDtlEntities, List<QTProjectDtlFundApplModel> qTProjectDtlFundApplEntities, List<QTProjectDtlImplPlanModel> qTProjectDtlImplPlanEntities, List<QTProjectDtlPerformTargetModel> qTProjectDtlPerformTargetEntities, List<QTProjectDtlPurchaseDtlModel> qTProjectDtlPurchaseDtlEntities, List<QTProjectDtlPurDtl4SOFModel> qTProjectDtlPurDtl4SOFEntities, List<QTProjectDtlTextContentModel> qTProjectDtlTextContentEntities)
        {
			return QTProjectMstFacade.SaveQTProjectMst(qTProjectMstEntity, qTProjectDtlBudgetDtlEntities, qTProjectDtlFundApplEntities, qTProjectDtlImplPlanEntities, qTProjectDtlPerformTargetEntities, qTProjectDtlPurchaseDtlEntities, qTProjectDtlPurDtl4SOFEntities, qTProjectDtlTextContentEntities);
        }

        /// <summary>
        /// 通过外键值获取QTProjectDtlBudgetDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<QTProjectDtlBudgetDtlModel> FindQTProjectDtlBudgetDtlByForeignKey<TValType>(TValType id)
        {
            return QTProjectDtlBudgetDtlFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 通过外键值获取QTProjectDtlFundAppl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<QTProjectDtlFundApplModel> FindQTProjectDtlFundApplByForeignKey<TValType>(TValType id)
        {
            return QTProjectDtlFundApplFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 通过外键值获取QTProjectDtlImplPlan明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<QTProjectDtlImplPlanModel> FindQTProjectDtlImplPlanByForeignKey<TValType>(TValType id)
        {
            return QTProjectDtlImplPlanFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 通过外键值获取QTProjectDtlPerformTarget明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<QTProjectDtlPerformTargetModel> FindQTProjectDtlPerformTargetByForeignKey<TValType>(TValType id)
        {
            return QTProjectDtlPerformTargetFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 通过外键值获取QTProjectDtlPurchaseDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<QTProjectDtlPurchaseDtlModel> FindQTProjectDtlPurchaseDtlByForeignKey<TValType>(TValType id)
        {
            return QTProjectDtlPurchaseDtlFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 通过外键值获取QTProjectDtlPurDtl4SOF明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<QTProjectDtlPurDtl4SOFModel> FindQTProjectDtlPurDtl4SOFByForeignKey<TValType>(TValType id)
        {
            return QTProjectDtlPurDtl4SOFFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 通过外键值获取QTProjectDtlTextContent明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<QTProjectDtlTextContentModel> FindQTProjectDtlTextContentByForeignKey<TValType>(TValType id)
        {
            return QTProjectDtlTextContentFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 申报部门项目汇总表
        /// </summary>
        /// <returns>返回Json串</returns>
        public List<QTProjectMstHZModel> GetQTProjectMstHZ(Dictionary<string, object> dic,int pageIndex,int pageSize,out int TotalItems)
        {
            TotalItems = 0;
            List<QTProjectMstHZModel> result = new List<QTProjectMstHZModel>();
            new CreateCriteria(dic).Add(ORMRestrictions<System.Int64>.NotEq("PhId", 0));//防止dic为空
            List<QTProjectMstModel> Msts=QTProjectMstFacade.Find(dic,new string[] { "FProjCode" }).Data.ToList();
            var AllProjcodes = Msts.Select(x => x.FProjCode).Distinct().ToList();
            TotalItems = AllProjcodes.Count;
            List<string> Projcodes = AllProjcodes.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            if (Projcodes.Count > 0)
            {
                var Msts2 = new List<QTProjectMstModel>();
                var Top1 = new QTProjectMstModel();
                var Bottom1 = new QTProjectMstModel();
                var Top2 = new QTProjectMstModel();
                var Bottom2 = new QTProjectMstModel();
                var oldbudget=new QTProjectDtlBudgetDtlModel();
                var newbudget = new QTProjectDtlBudgetDtlModel();
                for (var i = 0; i < Projcodes.Count; i++)
                {
                    QTProjectMstHZModel data = new QTProjectMstHZModel();
                    Msts2 = Msts.FindAll(x => x.FProjCode == Projcodes[i]).OrderBy(x=>x.NgInsertDt).ToList();
                    Top1 = Msts2.Find(x => x.FDtlstage_EXName == "一上");
                    Bottom1 = Msts2.Find(x => x.FDtlstage_EXName == "一下");
                    Top2 = Msts2.Find(x => x.FDtlstage_EXName == "二上");
                    Bottom2 = Msts2.Find(x => x.FDtlstage_EXName == "二下");
                    if (Top1 != null)
                    {
                        data.FAmountTop1 = Top1.FProjAmount;
                    }
                    if (Bottom1 != null)
                    {
                        data.FAmountBottom1 = Bottom1.FProjAmount;
                    }
                    if (Top2 != null)
                    {
                        data.FAmountTop2 = Top2.FProjAmount;
                    }
                    if (Bottom2 != null)
                    {
                        data.FAmountBottom2 = Bottom2.FProjAmount;
                    }
                    //Msts2肯定不为空
                    data.FDeclarationDept = Msts2[Msts2.Count-1].FDeclarationDept;
                    data.FDeclarationDept_EXName = Msts2[Msts2.Count-1].FDeclarationDept_EXName;
                    data.FBudgetDept = Msts2[Msts2.Count - 1].FBudgetDept;
                    data.FBudgetDept_EXName = Msts2[Msts2.Count - 1].FBudgetDept_EXName;
                    data.FProjName = Msts2[Msts2.Count - 1].FProjName;
                    data.FExpenseCategory= Msts2[Msts2.Count - 1].FExpenseCategory;
                    data.FExpenseCategory_EXName = Msts2[Msts2.Count - 1].FExpenseCategory_EXName;

                    oldbudget = QTProjectDtlBudgetDtlFacade.FindByForeignKey(Msts2[0].PhId).Data[0];
                    data.FBudgetAccounts = oldbudget.FBudgetAccounts;
                    data.FBudgetAccounts_EXName = oldbudget.FBudgetAccounts_EXName;
                    newbudget = QTProjectDtlBudgetDtlFacade.FindByForeignKey(Msts2[Msts2.Count - 1].PhId).Data[0];
                    data.FBudgetAccounts2 = newbudget.FBudgetAccounts;
                    data.FBudgetAccounts2_EXName = newbudget.FBudgetAccounts_EXName;
                    data.FAccount = Msts2[Msts2.Count - 1].FAccount;
                    data.FAdjustAmount = data.FAmountBottom1 - data.FAmountTop1;
                    data.FAdjustAmount2 = data.FAmountTop2 - data.FAmountBottom1;
                    data.FAdjustAmount3 = data.FAmountBottom2 - data.FAmountTop2;
                    //后续增的
                    data.FAdjustAmountS2S1 = data.FAmountTop2 - data.FAmountTop1;
                    data.FAdjustAmountX2S1 = data.FAmountBottom2 - data.FAmountTop1;
                    data.FAdjustAmountX2X1 = data.FAmountBottom2 - data.FAmountBottom1;

                    result.Add(data);
                }
            }


            
            /*if (Msts.Count > 0)//根据明细维度取表数据时逻辑
            {
                var MstPhids = Msts.Select(x => x.PhId).ToList();
                var dtlDic = new Dictionary<string, object>();
                new CreateCriteria(dtlDic).Add(ORMRestrictions<List<System.Int64>>.In("MstPhid", MstPhids));
                if (!string.IsNullOrEmpty(FAccount))
                {
                    new CreateCriteria(dtlDic).Add(ORMRestrictions<string>.Eq("FAccount", FAccount));
                }
                var Dtls = QTProjectDtlBudgetDtlFacade.Find(dtlDic, new string[] { "FDtlCode" }).Data.ToList();
                var AllDtlCodes = Dtls.Select(x => x.FDtlCode).Distinct().ToList();
                TotalItems = AllDtlCodes.Count;
                List<string> DtlCodes = AllDtlCodes.Skip(pageIndex * pageSize).Take(pageSize).ToList();

                var Dtls2 = new List<QTProjectDtlBudgetDtlModel>(); 
                var MstPhids2= new List<long>();
                var Top1 = new QTProjectMstModel();
                var Bottom1 = new QTProjectMstModel();
                var Top2 = new QTProjectMstModel();
                var Bottom2 = new QTProjectMstModel();
                for (var i=0;i< DtlCodes.Count; i++)
                {
                    Dtls2 = Dtls.FindAll(x => x.FDtlCode == DtlCodes[i]).OrderBy(x=>x.NgInsertDt).ToList();
                    MstPhids2 = Dtls2.Select(x => x.MstPhid).Distinct().ToList();
                    Top1 = Msts.Find(x => MstPhids2.Contains(x.PhId) && x.FDtlstage_EXName == "一上");
                    Bottom1 = Msts.Find(x => MstPhids2.Contains(x.PhId) && x.FDtlstage_EXName == "一下");
                    Top2 = Msts.Find(x => MstPhids2.Contains(x.PhId) && x.FDtlstage_EXName == "二上");
                    Bottom2 = Msts.Find(x => MstPhids2.Contains(x.PhId) && x.FDtlstage_EXName == "二下");
                    QTProjectMstHZModel data = new QTProjectMstHZModel();
                    if (Top1 != null)
                    {
                        data.FAmountTop1 = Dtls2.Find(x => x.MstPhid == Top1.PhId).FAmount;
                        data.FDeclarationDept = Top1.FDeclarationDept;
                        data.FDeclarationDept_EXName = Top1.FDeclarationDept_EXName;
                        data.FBudgetDept = Top1.FBudgetDept;
                        data.FBudgetDept_EXName = Top1.FBudgetDept_EXName;
                        data.FProjName = Top1.FProjName;
                    }
                    if (Bottom1 != null)
                    {
                        data.FAmountBottom1 = Dtls2.Find(x => x.MstPhid == Bottom1.PhId).FAmount;
                        data.FDeclarationDept = Bottom1.FDeclarationDept;
                        data.FDeclarationDept_EXName = Bottom1.FDeclarationDept_EXName;
                        data.FBudgetDept = Bottom1.FBudgetDept;
                        data.FBudgetDept_EXName = Bottom1.FBudgetDept_EXName;
                        data.FProjName = Bottom1.FProjName;
                    }
                    if (Top2 != null)
                    {
                        data.FAmountTop2 = Dtls2.Find(x => x.MstPhid == Top2.PhId).FAmount;
                        data.FDeclarationDept = Top2.FDeclarationDept;
                        data.FDeclarationDept_EXName = Top2.FDeclarationDept_EXName;
                        data.FBudgetDept = Top2.FBudgetDept;
                        data.FBudgetDept_EXName = Top2.FBudgetDept_EXName;
                        data.FProjName = Top2.FProjName;
                    }
                    if (Bottom2 != null)
                    {
                        data.FAmountBottom2 = Dtls2.Find(x => x.MstPhid == Bottom2.PhId).FAmount;
                        data.FDeclarationDept = Bottom2.FDeclarationDept;
                        data.FDeclarationDept_EXName = Bottom2.FDeclarationDept_EXName;
                        data.FBudgetDept = Bottom2.FBudgetDept;
                        data.FBudgetDept_EXName = Bottom2.FBudgetDept_EXName;
                        data.FProjName = Bottom2.FProjName;
                    }
                    //Dtls2肯定不为空
                    data.FBudgetAccounts = Dtls2[0].FBudgetAccounts;
                    data.FBudgetAccounts_EXName = Dtls2[0].FBudgetAccounts_EXName;
                    data.FName1 = Dtls2[0].FName;
                    data.FName2 = Dtls2[Dtls2.Count].FName;
                    data.FAccount = Dtls2[Dtls2.Count].FAccount;
                    data.FAccount_EXName = Dtls2[Dtls2.Count].FAccount_EXName;
                    data.FAdjustAmount = data.FAmountBottom1 - data.FAmountTop1;
                    data.FAdjustAmount2 = data.FAmountTop2 - data.FAmountBottom1;
                    data.FAdjustAmount = data.FAmountBottom2 - data.FAmountTop2;

                    result.Add(data);
                }
            }*/
            return result;
        }
        #endregion
    }
}

