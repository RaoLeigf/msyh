#region Summary
/**************************************************************************************
    * 类 名 称：        PerformanceMstFacade
    * 命名空间：        GJX3.JX.Facade
    * 文 件 名：        PerformanceMstFacade.cs
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
using SUP.Common.DataAccess;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Facade;

using GJX3.JX.Facade.Interface;
using GJX3.JX.Rule.Interface;
using GJX3.JX.Model.Domain;
using GQT3.QT.Model.Domain;
using GQT3.QT.Facade.Interface;
using GQT3.QT.Rule.Interface;

namespace GJX3.JX.Facade
{
	/// <summary>
	/// PerformanceMst业务组装处理类
	/// </summary>
    public partial class PerformanceMstFacade : EntFacadeBase<PerformanceMstModel>, IPerformanceMstFacade
    {
		#region 类变量及属性
		/// <summary>
        /// PerformanceMst业务逻辑处理对象
        /// </summary>
		IPerformanceMstRule PerformanceMstRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IPerformanceMstRule;
            }
        }
		/// <summary>
        /// PerformanceDtlEval业务逻辑处理对象
        /// </summary>
		IPerformanceDtlEvalRule PerformanceDtlEvalRule { get; set; }
		/// <summary>
        /// PerformanceDtlTextCont业务逻辑处理对象
        /// </summary>
		IPerformanceDtlTextContRule PerformanceDtlTextContRule { get; set; }
		/// <summary>
        /// PerformanceDtlBuDtl业务逻辑处理对象
        /// </summary>
		IPerformanceDtlBuDtlRule PerformanceDtlBuDtlRule { get; set; }
        /// <summary>
        /// PerformanceDtlTarImpl业务逻辑处理对象
        /// </summary>
		IPerformanceDtlTarImplRule PerformanceDtlTarImplRule { get; set; }
        /// <summary>
        /// ThirdAttachment业务逻辑处理对象
        /// </summary>
		IThirdAttachmentRule ThirdAttachmentRule { get; set; }

        IPerformEvalTargetTypeRule PerformEvalTargetTypeRule { get; set; }
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
        public override PagedResult<PerformanceMstModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<PerformanceMstModel> pageResult = base.LoadWithPage(pageNumber, pageSize, dic, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<PerformanceMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<PerformanceMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
			//helpdac.CodeToName<PerformanceMstModel>(pageResult.Results, "FProjName", "FProjName_EXName", "xm3_xmlist", "");
			helpdac.CodeToName<PerformanceMstModel>(pageResult.Results, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist", "");
			helpdac.CodeToName<PerformanceMstModel>(pageResult.Results, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
            helpdac.CodeToName<PerformanceMstModel>(pageResult.Results, "FDeclarationDept", "FDeclarationDept_EXName", "ys_orglist", "");
            helpdac.CodeToName<PerformanceMstModel>(pageResult.Results, "FEvaluationDept", "FEvaluationDept_EXName", "ys_orglist", "");
            helpdac.CodeToName<PerformanceMstModel>(pageResult.Results, "FInformant", "FInformantName", "fg3_user", "");
            helpdac.CodeToName<PerformanceMstModel>(pageResult.Results, "FPerformType", "FPerformType_EXName", "GHPerformEvalTargetTypeTree", "");
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
        public PagedResult<PerformanceMstModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<PerformanceMstModel> pageResult = base.FacadeHelper.LoadWithPage(pageNumber, pageSize, nameSqlName, dic, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<PerformanceMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<PerformanceMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
			//helpdac.CodeToName<PerformanceMstModel>(pageResult.Results, "FProjName", "FProjName_EXName", "xm3_xmlist", "");
			helpdac.CodeToName<PerformanceMstModel>(pageResult.Results, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist", "");
			helpdac.CodeToName<PerformanceMstModel>(pageResult.Results, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
            helpdac.CodeToName<PerformanceMstModel>(pageResult.Results, "FDeclarationDept", "FDeclarationDept_EXName", "ys_orglist", "");
            helpdac.CodeToName<PerformanceMstModel>(pageResult.Results, "FEvaluationDept", "FEvaluationDept_EXName", "ys_orglist", "");
            helpdac.CodeToName<PerformanceMstModel>(pageResult.Results, "FInformant", "FInformantName", "fg3_user", "");
            helpdac.CodeToName<PerformanceMstModel>(pageResult.Results, "FPerformType", "FPerformType_EXName", "GHPerformEvalTargetTypeTree", "");

            #endregion

            return pageResult;
        }

        /// <summary>
        /// 通过id，删除数据
        /// </summary>
        /// <param name="id">单主键id值</param>
        public override DeletedResult Delete<TValType>(TValType id)
        {
			PerformanceDtlEvalRule.RuleHelper.DeleteByForeignKey(id);
			PerformanceDtlTextContRule.RuleHelper.DeleteByForeignKey(id);
			PerformanceDtlBuDtlRule.RuleHelper.DeleteByForeignKey(id);
            PerformanceDtlTarImplRule.RuleHelper.DeleteByForeignKey(id);
            ThirdAttachmentRule.RuleHelper.DeleteByForeignKey(id);
            return base.Delete(id);
        }

		/// <summary>
        /// 通过ids，删除数据
        /// </summary>
        /// <param name="ids">单主键id集合</param>
        public override DeletedResult Delete<TValType>(IList<TValType> ids)
        {
			PerformanceDtlEvalRule.RuleHelper.DeleteByForeignKey(ids);
			PerformanceDtlTextContRule.RuleHelper.DeleteByForeignKey(ids);
			PerformanceDtlBuDtlRule.RuleHelper.DeleteByForeignKey(ids);
            PerformanceDtlTarImplRule.RuleHelper.DeleteByForeignKey(ids);
            ThirdAttachmentRule.RuleHelper.DeleteByForeignKey(ids);
            return base.Delete(ids);
        }
        #endregion

        #region 实现 IPerformanceMstFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<PerformanceMstModel> ExampleMethod<PerformanceMstModel>(string param)
        //{
        //    //编写代码
        //}

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
            SavedResult<Int64> savedResult = base.Save<Int64>(performanceMstEntity);
            if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
            {
				//if (performanceDtlEvalEntities.Count > 0)
				//{
				//	PerformanceDtlEvalRule.Save(performanceDtlEvalEntities, savedResult.KeyCodes[0]);
				//}
				if (performanceDtlTextContEntities != null && performanceDtlTextContEntities.Count > 0)
				{
					PerformanceDtlTextContRule.Save(performanceDtlTextContEntities, savedResult.KeyCodes[0]);
				}
				if (performanceDtlBuDtlEntities != null && performanceDtlBuDtlEntities.Count > 0)
				{
					PerformanceDtlBuDtlRule.Save(performanceDtlBuDtlEntities, savedResult.KeyCodes[0]);
				}
                if (performanceDtlTarImplEntities != null && performanceDtlTarImplEntities.Count > 0)
                {
                    for (int i = 0; i < performanceDtlTarImplEntities.Count; i++) {
                        performanceDtlTarImplEntities[i].MstPhid = savedResult.KeyCodes[0];
                    }
                    PerformanceDtlTarImplRule.Save(performanceDtlTarImplEntities, savedResult.KeyCodes[0]);
                }
            }

			return savedResult;
        }


        /// <summary>
        /// 获取新的项目绩效集合
        /// </summary>
        /// <param name="projectDtlPerformTargets">项目带的绩效集合</param>
        /// <param name="targetTypeCode">父级节点</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        public List<PerformanceDtlTarImplModel> GetNewProPerformTargets(List<PerformanceDtlTarImplModel> projectDtlPerformTargets, string targetTypeCode, long orgId, string orgCode)
        {
            if (projectDtlPerformTargets != null && projectDtlPerformTargets.Count > 0)
            {
                //该组织下所有的指标类型集合
                IList<PerformEvalTargetTypeModel> performEvalTargetTypes = this.PerformEvalTargetTypeRule.Find(t => t.Orgcode == orgCode && t.Orgid == orgId);

                int max = 0;
                foreach (var proPer in projectDtlPerformTargets)
                {
                    if (proPer.FTargetTypeCode.Length / 2 > max)
                    {
                        max = proPer.FTargetTypeCode.Length / 2;
                    }
                }
                foreach (var per in projectDtlPerformTargets)
                {
                    per.FTargetTypeName = performEvalTargetTypes.ToList().Find(t => t.FCode == per.FTargetTypeCode) == null ? "" : performEvalTargetTypes.ToList().Find(t => t.FCode == per.FTargetTypeCode).FName;
                    per.FTargetTypePerantCode = performEvalTargetTypes.ToList().Find(t => t.FCode == per.FTargetTypeCode) == null ? "" : performEvalTargetTypes.ToList().Find(t => t.FCode == per.FTargetTypeCode).FCode;
                    if (max - per.FTargetTypeCode.Length / 2 == 0)
                    {
                        per.TypeCount = 1;
                        per.FTargetTypeCode1 = per.FTargetTypeCode;
                        per.FTargetTypeName1 = per.FTargetTypeName;
                        GetNewProPerformTarget(performEvalTargetTypes, per, targetTypeCode, 1);
                    }
                    else if (max - per.FTargetTypeCode.Length / 2 == 1)
                    {
                        per.TypeCount = 2;
                        per.FTargetTypeCode2 = per.FTargetTypeCode;
                        per.FTargetTypeName2 = per.FTargetTypeName;
                        GetNewProPerformTarget(performEvalTargetTypes, per, targetTypeCode, 1);
                    }
                    else if (max - per.FTargetTypeCode.Length / 2 == 2)
                    {
                        per.TypeCount = 3;
                        per.FTargetTypeCode3 = per.FTargetTypeCode;
                        per.FTargetTypeName3 = per.FTargetTypeName;
                        GetNewProPerformTarget(performEvalTargetTypes, per, targetTypeCode, 1);
                    }
                    else if (max - per.FTargetTypeCode.Length / 2 == 3)
                    {
                        per.TypeCount = 4;
                        per.FTargetTypeCode4 = per.FTargetTypeCode;
                        per.FTargetTypeName4 = per.FTargetTypeName;
                        GetNewProPerformTarget(performEvalTargetTypes, per, targetTypeCode, 1);
                    }
                    else if (max - per.FTargetTypeCode.Length / 2 == 4)
                    {
                        per.TypeCount = 5;
                        per.FTargetTypeCode5 = per.FTargetTypeCode;
                        per.FTargetTypeName5 = per.FTargetTypeName;
                        GetNewProPerformTarget(performEvalTargetTypes, per, targetTypeCode, 1);
                    }
                    else
                    {
                        continue;
                    }
                }

                projectDtlPerformTargets = projectDtlPerformTargets.OrderBy(t => t.FTargetTypeCode5).OrderBy(t => t.FTargetTypeCode4).OrderBy(t => t.FTargetTypeCode3).OrderBy(t => t.FTargetTypeCode2).OrderBy(t => t.FTargetTypeCode1).ToList();
            }
            return projectDtlPerformTargets;
        }

        /// <summary>
        /// 获取新的绩效明细
        /// </summary>
        /// <param name="performEvalTargetTypes">该组织绩效指标类型集合</param>
        /// <param name="projectDtlPerformTarget">单个绩效明细</param>
        /// <param name="targetTypeCode">父节点</param>
        /// <param name="num">数量</param>
        /// <returns></returns>
        public PerformanceDtlTarImplModel GetNewProPerformTarget(IList<PerformEvalTargetTypeModel> performEvalTargetTypes, PerformanceDtlTarImplModel projectDtlPerformTarget, string targetTypeCode, int num)
        {
            if (projectDtlPerformTarget != null && performEvalTargetTypes != null && performEvalTargetTypes.Count > 0 && projectDtlPerformTarget.FTargetTypePerantCode != targetTypeCode)
            {
                if (projectDtlPerformTarget.FTargetTypeCode.Length - num * 2 >= 0)
                {
                    var type = performEvalTargetTypes.ToList().Find(t => t.FCode == projectDtlPerformTarget.FTargetTypeCode.Substring(0, projectDtlPerformTarget.FTargetTypeCode.Length - num * 2));
                    if (type != null)
                    {
                        if (projectDtlPerformTarget.TypeCount == 1)
                        {
                            projectDtlPerformTarget.FTargetTypeCode2 = type.FCode;
                            projectDtlPerformTarget.FTargetTypeName2 = type.FName;
                            projectDtlPerformTarget.TypeCount++;
                            projectDtlPerformTarget.FTargetTypePerantCode = type.FCode;
                            num++;
                            GetNewProPerformTarget(performEvalTargetTypes, projectDtlPerformTarget, targetTypeCode, num);
                        }
                        else if (projectDtlPerformTarget.TypeCount == 2)
                        {
                            projectDtlPerformTarget.FTargetTypeCode3 = type.FCode;
                            projectDtlPerformTarget.FTargetTypeName3 = type.FName;
                            projectDtlPerformTarget.TypeCount++;
                            projectDtlPerformTarget.FTargetTypePerantCode = type.FCode;
                            num++;
                            GetNewProPerformTarget(performEvalTargetTypes, projectDtlPerformTarget, targetTypeCode, num);
                        }
                        else if (projectDtlPerformTarget.TypeCount == 3)
                        {
                            projectDtlPerformTarget.FTargetTypeCode4 = type.FCode;
                            projectDtlPerformTarget.FTargetTypeName4 = type.FName;
                            projectDtlPerformTarget.TypeCount++;
                            projectDtlPerformTarget.FTargetTypePerantCode = type.FCode;
                            num++;
                            GetNewProPerformTarget(performEvalTargetTypes, projectDtlPerformTarget, targetTypeCode, num);
                        }
                        else if (projectDtlPerformTarget.TypeCount == 4)
                        {
                            projectDtlPerformTarget.FTargetTypeCode5 = type.FCode;
                            projectDtlPerformTarget.FTargetTypeName5 = type.FName;
                            projectDtlPerformTarget.TypeCount++;
                            projectDtlPerformTarget.FTargetTypePerantCode = type.FCode;
                            num++;
                            GetNewProPerformTarget(performEvalTargetTypes, projectDtlPerformTarget, targetTypeCode, num);
                        }
                        else
                        {

                        }
                    }
                }

            }
            return projectDtlPerformTarget;
        }
        #endregion
    }
}

