#region Summary
/**************************************************************************************
    * 类 名 称：        PerformanceMstService
    * 命名空间：        GJX3.JX.Service
    * 文 件 名：        PerformanceMstService.cs
    * 创建时间：        2018/9/12 
    * 作    者：        吾丰明    
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
using Enterprise3.Common.Base.Criterion;

using GJX3.JX.Service.Interface;
using GJX3.JX.Facade.Interface;
using GQT3.QT.Facade.Interface;
using GJX3.JX.Model.Domain;
using GQT3.QT.Model.Domain;
using GYS3.YS.Model.Domain;
using SUP.Common.Base;
using Enterprise3.WebApi.GJX3.JX.Model.Request;
using SUP.Common.DataAccess;

namespace GJX3.JX.Service
{
	/// <summary>
	/// PerformanceMst服务组装处理类
	/// </summary>
    public partial class PerformanceMstService : EntServiceBase<PerformanceMstModel>, IPerformanceMstService
    {
		#region 类变量及属性
		/// <summary>
        /// PerformanceMst业务外观处理对象
        /// </summary>
		IPerformanceMstFacade PerformanceMstFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IPerformanceMstFacade;
            }
        }

		/// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IPerformanceDtlEvalFacade PerformanceDtlEvalFacade { get; set; }

		/// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IPerformanceDtlTextContFacade PerformanceDtlTextContFacade { get; set; }

		/// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IPerformanceDtlBuDtlFacade PerformanceDtlBuDtlFacade { get; set; }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IPerformanceDtlTarImplFacade PerformanceDtlTarImplFacade { get; set; }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
        private IPerformEvalTargetTypeFacade PerformEvalTargetTypeFacade { get; set; }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
        private IPerformEvalTargetClassFacade PerformEvalTargetClassFacade { get; set; }
        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IThirdAttachmentFacade ThirdAttachmentFacade { get; set; }

        private IQtAttachmentFacade QtAttachmentFacade { get; set; }

        private IOrganizationFacade OrganizationFacade { get; set; }
        #endregion

        #region 实现 IPerformanceMstService 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="performanceMstEntity"></param>
        /// <param name="performanceDtlTextContEntities"></param>
        /// <param name="performanceDtlBuDtlEntities"></param>
        /// <param name="performanceDtlTarImplEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SavePerformanceMst(PerformanceMstModel performanceMstEntity, List<PerformanceDtlTextContModel> performanceDtlTextContEntities, List<PerformanceDtlBuDtlModel> performanceDtlBuDtlEntities, List<PerformanceDtlTarImplModel> performanceDtlTarImplEntities)
        {
			return PerformanceMstFacade.SavePerformanceMst(performanceMstEntity, performanceDtlTextContEntities, performanceDtlBuDtlEntities,performanceDtlTarImplEntities);
        }

        /// <summary>
        /// 通过外键值获取PerformanceDtlEval明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<PerformanceDtlEvalModel> FindPerformanceDtlEvalByForeignKey<TValType>(TValType id)
        {
            return PerformanceDtlEvalFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 通过外键值获取PerformanceDtlTextCont明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<PerformanceDtlTextContModel> FindPerformanceDtlTextContByForeignKey<TValType>(TValType id)
        {
            return PerformanceDtlTextContFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 通过外键值获取PerformanceDtlBuDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<PerformanceDtlBuDtlModel> FindPerformanceDtlBuDtlByForeignKey<TValType>(TValType id)
        {
            return PerformanceDtlBuDtlFacade.FindByForeignKey(id);
        }
        
        /// <summary>
        /// 通过外键值获取PerformanceDtlTarImpl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<PerformanceDtlTarImplModel> FindPerformanceDtlTarImplByForeignKey<TValType>(TValType id)
        {
            FindedResults<PerformanceDtlTarImplModel> results = PerformanceDtlTarImplFacade.FindByForeignKey(id);
            if (results != null) {
                var data = results.Data;
                for (int i = 0; i < data.Count; i++) {
                    PerformanceDtlTarImplModel model = data[i];
                    string typeCode = model.FTargetTypeCode;
                    string classCode = model.FTargetClassCode;
                    Dictionary<string, object> dicWhere1 = new Dictionary<string, object>();
                    Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
                    new CreateCriteria(dicWhere1).Add(ORMRestrictions<string>.Eq("FCode", typeCode));
                    FindedResults<PerformEvalTargetTypeModel> typeModel = PerformEvalTargetTypeFacade.Find(dicWhere1);
                    new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FCode", classCode));
                    FindedResults<PerformEvalTargetClassModel> classModel = PerformEvalTargetClassFacade.Find(dicWhere2);
                    if (typeModel != null && typeModel.Data.Count > 0) {
                        model.FTargetTypeCode = typeModel.Data[0].FName;
                    }
                    if (classModel != null && classModel.Data.Count > 0) {
                        model.FTargetClassCode = classModel.Data[0].FName;
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// 通过外键值获取ThirdAttachment明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<ThirdAttachmentModel> FindThirdAttachmentByForeignKey<TValType>(TValType id)
        {
            return ThirdAttachmentFacade.FindByForeignKey(id,new string[] { "FTime" });
        }

        /// <summary>
        /// BudgetDtlPerformTargetModel转换成PerformanceDtlTarImplModel
        /// </summary>
        /// <param name="list">预算绩效目标实现集合</param>
        /// <returns></returns>
        public IList<PerformanceDtlTarImplModel> ConvertData(IList<BudgetDtlPerformTargetModel> list) {
            IList<PerformanceDtlTarImplModel> result = new List<PerformanceDtlTarImplModel>(); 
            if (list != null) {
                for (int i = 0; i < list.Count; i++) {
                    BudgetDtlPerformTargetModel budgetModel = list[i];
                    PerformanceDtlTarImplModel performModel = new PerformanceDtlTarImplModel();
                    performModel.XmPhid = budgetModel.XmPhId;
                    performModel.FTargetCode = budgetModel.FTargetCode;
                    performModel.FTargetName = budgetModel.FTargetName;
                    performModel.FTargetContent = budgetModel.FTargetContent;
                    performModel.FTargetValue = budgetModel.FTargetValue;
                    performModel.FTargetWeight = Convert.ToDecimal(budgetModel.FTargetWeight);
                    performModel.FTargetDescribe = budgetModel.FTargetDescribe;
                    //代码转名称
                    string typeCode = budgetModel.FTargetTypeCode;
                    string classCode = budgetModel.FTargetClassCode;
                    Dictionary<string, object> dicWhere1 = new Dictionary<string, object>();
                    Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
                    new CreateCriteria(dicWhere1).Add(ORMRestrictions<string>.Eq("FCode", typeCode));
                    FindedResults<PerformEvalTargetTypeModel> typeModel = PerformEvalTargetTypeFacade.Find(dicWhere1);
                    new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FCode", classCode));
                    FindedResults<PerformEvalTargetClassModel> classModel = PerformEvalTargetClassFacade.Find(dicWhere2);
                    if (typeModel != null && typeModel.Data.Count > 0)
                    {
                        performModel.FTargetTypeCode = typeModel.Data[0].FName;
                    }
                    if (classModel != null && classModel.Data.Count > 0)
                    {
                        performModel.FTargetClassCode = classModel.Data[0].FName;
                    }
                    performModel.FIfCustom = budgetModel.FIfCustom;

                    result.Add(performModel);
                }
            }
            return result;
        }

        /// <summary>
        /// 考虑到存在指标类别、类型名称转代码的问题
        /// 合并绩效目标实现情况数据，从预算明细表获取表格模板数据，从绩效明细表获取自评完成值和自评得分
        /// </summary>
        /// <param name="budgetDtlPerformTarget">预算绩效目标实现集合</param>
        /// <param name="performanceDtlTarImpl">绩效目标实现情况集合</param>
        /// <returns></returns>
        public List<PerformanceDtlTarImplModel> ConvertSaveData(IList<BudgetDtlPerformTargetModel> budgetDtlPerformTarget, IList<PerformanceDtlTarImplModel> performanceDtlTarImpl) {
            List<PerformanceDtlTarImplModel> result = new List<PerformanceDtlTarImplModel>();
            if (budgetDtlPerformTarget != null && budgetDtlPerformTarget.Count > 0) {
                if (performanceDtlTarImpl != null && performanceDtlTarImpl.Count > 0) {
                    for (int i = 0; i < performanceDtlTarImpl.Count; i++) {
                        PerformanceDtlTarImplModel performanceDtlTar = performanceDtlTarImpl[i];
                        BudgetDtlPerformTargetModel budgetDtlPerform = budgetDtlPerformTarget.First(t => t.FTargetName == performanceDtlTar.FTargetName);
                        performanceDtlTarImpl[i].FTargetTypeCode = budgetDtlPerform.FTargetTypeCode;
                        performanceDtlTarImpl[i].FTargetClassCode = budgetDtlPerform.FTargetClassCode;
                        result.Add(performanceDtlTar);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 通过id获取主表信息
        /// </summary>
        /// <param name="id">主键值</param>
        /// <returns></returns>
        public FindedResult<PerformanceMstModel> Find2<TValType>(TValType id)
        {
            var data = PerformanceMstFacade.Find(id);
            var TarImpl = PerformanceDtlTarImplFacade.FindByForeignKey(id).Data;
            if (TarImpl.Count > 0)
            {
                data.Data.FTargetTypeCode = TarImpl[0].FTargetTypeCode;
                Dictionary<string, object> dicWhere1 = new Dictionary<string, object>();
                new CreateCriteria(dicWhere1).Add(ORMRestrictions<string>.Eq("FCode", TarImpl[0].FTargetTypeCode));
                var typeModel = PerformEvalTargetTypeFacade.Find(dicWhere1).Data;
                if (typeModel.Count > 0)
                {
                    data.Data.FTargetTypeCode_EXName = typeModel[0].FName;
                }
            }
            return data;
        }

        /// <summary>
        /// 保存第三方评价数据
        /// </summary>
        /// <param name="adddata"></param>
        /// <param name="updatedata"></param>
        /// <param name="deletedata"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveThird(List<ThirdAttachmentModel> adddata, List<ThirdAttachmentModel> updatedata, List<string> deletedata)
        {
            SavedResult<Int64> result = new SavedResult<Int64>();
            List<ThirdAttachmentModel> data = new List<ThirdAttachmentModel>();
            if (adddata != null && adddata.Count > 0)
            {
                for (var i = 0; i < adddata.Count; i++) 
                {
                    ThirdAttachmentModel a = adddata[i];
                    a.PersistentState = PersistentState.Added;
                    data.Add(a);
                }
                var Mst = PerformanceMstFacade.Find(adddata[0].MstPhid).Data;
                if (Mst.FThird != "1")
                {
                    Mst.FThird = "1";
                    Mst.PersistentState = PersistentState.Modified;
                    PerformanceMstFacade.Save<Int64>(Mst, "");
                }
            }
            if (updatedata != null && updatedata.Count > 0)
            {
                for (var j = 0; j < updatedata.Count; j++)
                {
                    ThirdAttachmentModel b = updatedata[j];
                    ThirdAttachmentModel c = ThirdAttachmentFacade.Find(b.PhId).Data;
                    c.FTime = b.FTime;
                    c.FText = b.FText;
                    c.FDeclarationUnit = b.FDeclarationUnit;
                    c.FProjName = b.FProjName;
                    c.FAgency = b.FAgency;
                    c.FLeader = b.FLeader;
                    c.PersistentState = PersistentState.Modified;
                    data.Add(c);
                }

            }
            if (deletedata != null && deletedata.Count > 0)
            {
                for (var x = 0; x < deletedata.Count; x++)
                {
                    ThirdAttachmentModel d = ThirdAttachmentFacade.Find(long.Parse(deletedata[x])).Data;
                    d.PersistentState = PersistentState.Deleted;
                    data.Add(d);
                }
            }
            result = ThirdAttachmentFacade.Save<Int64>(data, "");
            return result;
        }

        #endregion

        #region//vue绩效相关

        /// <summary>
        /// 将预算表中的BudgetDtlPerformTargetModel集合组装成绩效的PerformanceDtlTarImplModel集合
        /// </summary>
        /// <param name="list">预算绩效集合</param>
        /// <param name="performanceMst">绩效主对象</param>
        /// <returns></returns>
        public IList<PerformanceDtlTarImplModel> ConvertData2(IList<BudgetDtlPerformTargetModel> list, PerformanceMstModel performanceMst)
        {
            IList<PerformanceDtlTarImplModel> result = new List<PerformanceDtlTarImplModel>();
            if (list != null && list.Count > 0)
            {
                //根据申报组织获取绩效基础数据，为后续数据准备
                var allTypes = this.PerformEvalTargetTypeFacade.Find(t => t.Orgcode == performanceMst.FDeclarationUnit).Data;
                var allClasses = this.PerformEvalTargetClassFacade.Find(t => t.Orgcode == performanceMst.FDeclarationUnit).Data;
                for (int i = 0; i < list.Count; i++)
                {
                    BudgetDtlPerformTargetModel budgetModel = list[i];
                    PerformanceDtlTarImplModel performModel = new PerformanceDtlTarImplModel();
                    performModel.XmPhid = budgetModel.XmPhId;
                    performModel.FTargetCode = budgetModel.FTargetCode;
                    performModel.FTargetName = budgetModel.FTargetName;
                    performModel.FTargetContent = budgetModel.FTargetContent;
                    performModel.FTargetValue = budgetModel.FTargetValue;
                    performModel.FTargetWeight = Convert.ToDecimal(budgetModel.FTargetWeight);
                    performModel.FTargetDescribe = budgetModel.FTargetDescribe;
                    performModel.FTargetTypeCode = budgetModel.FTargetTypeCode;
                    performModel.FTargetClassCode = budgetModel.FTargetClassCode;
                    //代码转名称
                    string typeCode = budgetModel.FTargetTypeCode;
                    string classCode = budgetModel.FTargetClassCode;

                    if (allTypes != null && allTypes.Count > 0)
                    {
                        var typeModel = allTypes.ToList().FindAll(t => t.FCode == typeCode);
                        if (typeModel != null && typeModel.Count > 0)
                        {
                            performModel.FTargetTypeName = typeModel[0].FName;
                        }
                    }
                    if (allClasses != null && allClasses.Count > 0)
                    {
                        var classModel = allClasses.ToList().FindAll(t => t.FCode == classCode);
                        if (classModel != null && classModel.Count > 0)
                        {
                            performModel.FTargetClassName = classModel[0].FName;
                        }
                    }
                    //Dictionary<string, object> dicWhere1 = new Dictionary<string, object>();
                    //Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
                    //new CreateCriteria(dicWhere1).Add(ORMRestrictions<string>.Eq("FCode", typeCode));
                    //FindedResults<PerformEvalTargetTypeModel> typeModel = PerformEvalTargetTypeFacade.Find(dicWhere1);
                    //new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FCode", classCode));
                    //FindedResults<PerformEvalTargetClassModel> classModel = PerformEvalTargetClassFacade.Find(dicWhere2);
                    //if (typeModel != null && typeModel.Data.Count > 0)
                    //{
                    //    performModel.FTargetTypeCode = typeModel.Data[0].FName;
                    //}
                    //if (classModel != null && classModel.Data.Count > 0)
                    //{
                    //    performModel.FTargetClassCode = classModel.Data[0].FName;
                    //}
                    performModel.FIfCustom = budgetModel.FIfCustom;

                    result.Add(performModel);
                }
            }
            return result;
        }

        /// <summary>
        /// 根据绩效主键获取单个绩效的数据
        /// </summary>
        /// <param name="phid">绩效主键</param>
        /// <returns></returns>
        public PerformanceAllData GetPerformanceMst(long phid)
        {
            PerformanceAllData performanceAll = new PerformanceAllData();
            var result = this.PerformanceMstFacade.Find(t => t.PhId == phid).Data;
            if(result != null  && result.Count > 0)
            {
                performanceAll.PerformanceMst = result[0];
            }
            else
            {
                throw new Exception("绩效查询失败！");
            }
            performanceAll.PerformanceDtlBuDtls = this.PerformanceDtlBuDtlFacade.Find(t => t.MstPhid == phid).Data;
            if(performanceAll.PerformanceDtlBuDtls != null && performanceAll.PerformanceDtlBuDtls.Count > 0)
            {
                RichHelpDac helpdac = new RichHelpDac();
                helpdac.CodeToName<PerformanceDtlBuDtlModel>(performanceAll.PerformanceDtlBuDtls, "FSourceOfFunds", "FSourceOfFunds_EXName", "GHSourceOfFunds", "");
            }
            performanceAll.PerformanceDtlTarImpls = this.PerformanceDtlTarImplFacade.Find(t => t.MstPhid == phid).Data;
            if(performanceAll.PerformanceDtlTarImpls != null && performanceAll.PerformanceDtlTarImpls.Count > 0)
            {
                //根据申报组织获取绩效基础数据，为后续数据准备
                var allTypes = this.PerformEvalTargetTypeFacade.Find(t => t.Orgcode == result[0].FDeclarationUnit).Data;
                var allClasses = this.PerformEvalTargetClassFacade.Find(t => t.Orgcode == result[0].FDeclarationUnit).Data;
                //绩效主表的绩效数据
                performanceAll.PerformanceMst.FTargetTypeCode = performanceAll.PerformanceDtlTarImpls[0].FTargetTypeCode;
                if(allTypes != null && allTypes.Count > 0)
                {
                    var typeModel1 = allTypes.ToList().FindAll(t=>t.FCode == performanceAll.PerformanceDtlTarImpls[0].FTargetTypeCode);
                    if (typeModel1 != null && typeModel1.Count > 0)
                    {
                        performanceAll.PerformanceMst.FTargetTypeCode_EXName = typeModel1[0].FName;
                    }
                }

                //绩效单据的绩效数据
                for (int i = 0; i < performanceAll.PerformanceDtlTarImpls.Count; i++)
                {
                    PerformanceDtlTarImplModel model = performanceAll.PerformanceDtlTarImpls[i];
                    string typeCode = model.FTargetTypeCode;
                    string classCode = model.FTargetClassCode;
                    //Dictionary<string, object> dicWhere1 = new Dictionary<string, object>();
                    //Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
                    //new CreateCriteria(dicWhere1).Add(ORMRestrictions<string>.Eq("FCode", typeCode));
                    //FindedResults<PerformEvalTargetTypeModel> typeModel = PerformEvalTargetTypeFacade.Find(dicWhere1);
                    //new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FCode", classCode));
                    //FindedResults<PerformEvalTargetClassModel> classModel = PerformEvalTargetClassFacade.Find(dicWhere2);
                    if(allTypes != null && allTypes.Count > 0)
                    {
                        var typeModel = allTypes.ToList().FindAll(t => t.FCode == typeCode);
                        if (typeModel != null && typeModel.Count > 0)
                        {
                            model.FTargetTypeName = typeModel[0].FName;
                        }
                    }
                    if(allClasses != null && allClasses.Count > 0)
                    {
                        var classModel = allClasses.ToList().FindAll(t => t.FCode == classCode);
                        if (classModel != null && classModel.Count > 0)
                        {
                            model.FTargetClassName = classModel[0].FName;
                        }
                    }
                }
            }
            IList<OrganizeModel> organizes = this.OrganizationFacade.Find(t=>t.OCode ==performanceAll.PerformanceMst.FDeclarationUnit).Data;
            if (organizes != null && organizes.Count > 0)
            {
                OrganizeModel organize = organizes[0];
                performanceAll.PerformanceDtlTarImpls = this.PerformanceMstFacade.GetNewProPerformTargets(performanceAll.PerformanceDtlTarImpls.ToList(), performanceAll.PerformanceMst.FPerformType, organize.PhId, organize.OCode);
            }

            //var contexts = this.PerformanceDtlTextContFacade.Find(t => t.MstPhid == phid).Data;
            //if(contexts != null && contexts.Count > 0)
            //{
            //    performanceAll.PerformanceDtlTextCont = contexts[0];
            //}
            performanceAll.PerformanceDtlTextConts = this.PerformanceDtlTextContFacade.Find(t => t.MstPhid == phid).Data;
            performanceAll.ThirdAttachmentModels = this.ThirdAttachmentFacade.Find(t => t.MstPhid == phid).Data;
            if(performanceAll.ThirdAttachmentModels != null && performanceAll.ThirdAttachmentModels.Count > 0)
            {
                RichHelpDac helpdac = new RichHelpDac();
                helpdac.CodeToName<ThirdAttachmentModel>(performanceAll.ThirdAttachmentModels, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist", "");

                var phids = performanceAll.ThirdAttachmentModels.Select(t => t.PhId).Distinct().ToList();
                var allAttachs = this.QtAttachmentFacade.Find(t => phids.Contains(t.RelPhid) && t.BTable == "JX3_THIRDATTACHMENT").Data;
                if(allAttachs != null && allAttachs.Count > 0)
                {
                    foreach (var third in performanceAll.ThirdAttachmentModels)
                    {
                        third.ThirdQtAttachments = allAttachs.ToList().FindAll(t => t.RelPhid == third.PhId);
                    }
                }
            }
            performanceAll.QtAttachments = this.QtAttachmentFacade.Find(t => t.RelPhid == phid && t.BTable == "JX3_PERFORMANCEMST").Data;
            //performanceAll.ThirdQtAttachments = this.QtAttachmentFacade.Find(t => t.RelPhid == phid && t.BTable == "JX3_THIRDATTACHMENT").Data;
            return performanceAll;
        }

        /// <summary>
        /// 保存绩效抽评的第三方评价
        /// </summary>
        /// <param name="performanceMst">绩效抽评主对象</param>
        /// <param name="thirdAttachments">第三方评价的集合</param>
        /// <returns></returns>
        public SavedResult<long> SaveThird(PerformanceMstModel performanceMst, IList<ThirdAttachmentModel> thirdAttachments)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            if(thirdAttachments != null && thirdAttachments.Count > 0)
            {
                foreach(var third in thirdAttachments)
                {
                    if(third.PhId == 0)
                    {
                        third.PersistentState = PersistentState.Added;
                        third.FTime = DateTime.Now;
                    }
                    else
                    {
                        if(third.PersistentState != PersistentState.Deleted)
                        {
                            third.PersistentState = PersistentState.Modified;
                        }
                    }
                    third.MstPhid = performanceMst.PhId;
                }
                this.ThirdAttachmentFacade.Save<long>(thirdAttachments);
                //if(thirdAttachments.ToList().FindAll(t=>t.PersistentState != PersistentState.Deleted).Count > 0)
                //{
                //    performanceMst.FThird = "1";
                //}
                //else
                //{
                //    performanceMst.FThird = "2";
                //}
            }
            //else
            //{
            //    performanceMst.FThird = "2";
            //}
            performanceMst.PersistentState = PersistentState.Modified;
            savedResult = this.PerformanceMstFacade.Save<long>(performanceMst);
            return savedResult;
        }
        #endregion
    }
}

