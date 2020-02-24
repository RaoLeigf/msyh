#region Summary
/**************************************************************************************
    * 类 名 称：        PerformEvalTargetService
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        PerformEvalTargetService.cs
    * 创建时间：        2018/10/16 
    * 作    者：        刘杭    
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
using SUP.Common.Base;

namespace GQT3.QT.Service
{
	/// <summary>
	/// PerformEvalTarget服务组装处理类
	/// </summary>
    public partial class PerformEvalTargetService : EntServiceBase<PerformEvalTargetModel>, IPerformEvalTargetService
    {
		#region 类变量及属性
		/// <summary>
        /// PerformEvalTarget业务外观处理对象
        /// </summary>
		IPerformEvalTargetFacade PerformEvalTargetFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IPerformEvalTargetFacade;
            }
        }

        private IPerformEvalTargetClassFacade PerformEvalTargetClassFacade { get; set; }

        private IPerformEvalTargetTypeFacade PerformEvalTargetTypeFacade { get; set; }


        #endregion

        #region 实现 IPerformEvalTargetService 业务添加的成员

        /// <summary>
        /// 指标类别转名称
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public PagedResult<PerformEvalTargetModel> CodeToName(PagedResult<PerformEvalTargetModel> result)
        {
            if (result != null)
            {
                for (var i = 0; i < result.Results.Count; i++)
                {
                    Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                    new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FCode", result.Results[i].FTargetClassCode));
                    FindedResults<PerformEvalTargetClassModel> PerformEvalTargetClassModel = PerformEvalTargetClassFacade.Find(dicWhere);
                    if (PerformEvalTargetClassModel != null && PerformEvalTargetClassModel.Data.Count > 0)
                    {
                        result.Results[i].FTargetClassCode = PerformEvalTargetClassModel.Data[0].FName;
                    }
                    if (result.Results[i].FTargetWeight != null)
                    {
                        result.Results[i].FTargetWeight = result.Results[i].FTargetWeight + "%";
                    }
                }
            }
            return result;
        }


        public FindedResults<PerformEvalTargetModel> FindPerformEvalTargetByAnyCode<TValType>(TValType values, string Pname) {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<TValType>.In(Pname, values));
            return PerformEvalTargetFacade.Find(dicWhere);
        }

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <returns></returns>
        public FindedResults<PerformEvalTargetModel> ExecuteDataCheck(ref List<PerformEvalTargetModel> performEvalTarget, string otype)
        {
            IList<string> dm = new List<string>();
            FindedResults<PerformEvalTargetModel> results = new FindedResults<PerformEvalTargetModel>();
            if (performEvalTarget == null)
            {
                results.Status = ResponseStatus.Error;
                results.Msg = "保存失败，数据异常！";
            }
            else
            {
                for (int i = 0; i < performEvalTarget.Count; i++)
                {
                    performEvalTarget[i].FTargetCode = performEvalTarget[i].FTargetCode.Replace(" ", "");
                    performEvalTarget[i].FTargetName = performEvalTarget[i].FTargetName.Replace(" ", "");

                    if (performEvalTarget[i].FTargetWeight == "")
                    {
                        performEvalTarget[i].FTargetWeight = null;
                    }
                    dm.Add(performEvalTarget[i].FTargetCode);
                }
                var dicWhere1 = new Dictionary<string, object>();
                new CreateCriteria(dicWhere1).
                    Add(ORMRestrictions<IList<string>>.In("FTargetCode", dm));

                if (base.Find(dicWhere1) != null && base.Find(dicWhere1).Data.Count > 0 && otype != "edit")
                {
                    results = base.Find(dicWhere1);
                    results.Status = ResponseStatus.Error;
                    results.Msg = "保存失败，代码重复！";
                }

            }
            return results;
        }

        /// <summary>
        /// 根据指标类型获取指标集合
        /// </summary>
        /// <param name="targetTypeCode">指标类型编码</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        public IList<PerformEvalTargetModel> GetPerformEvalTargetList(string targetTypeCode, string orgId, string orgCode)
        {
            //该组织下所有的指标类型集合
            IList<PerformEvalTargetTypeModel> performEvalTargetTypes = this.PerformEvalTargetTypeFacade.Find(t => t.Orgcode == orgCode && t.Orgid == long.Parse(orgId)).Data;
            //该组织下所有的指标类别集合
            IList<PerformEvalTargetClassModel> performEvalTargetClasses = this.PerformEvalTargetClassFacade.Find(t => t.Orgcode == orgCode && t.Orgid == long.Parse(orgId)).Data;

            IList<PerformEvalTargetModel> performEvalTargets = this.PerformEvalTargetFacade.Find(t => t.Orgcode == orgCode && t.Orgid == long.Parse(orgId) && t.FTargetTypeCode.StartsWith(targetTypeCode)).Data.OrderBy(t => t.FTargetClassCode).ToList();
            if(performEvalTargets != null && performEvalTargets.Count > 0)
            {
                foreach(var per in performEvalTargets)
                {
                    per.FTargetClassName = performEvalTargetClasses.ToList().Find(t => t.FCode == per.FTargetClassCode) == null ? "" : performEvalTargetClasses.ToList().Find(t => t.FCode == per.FTargetClassCode).FName;
                    per.FTargetTypeName = performEvalTargetTypes.ToList().Find(t => t.FCode == per.FTargetTypeCode) == null ? "" : performEvalTargetTypes.ToList().Find(t => t.FCode == per.FTargetTypeCode).FName;
                }
            }
            return performEvalTargets;
        }

        /// <summary>
        /// 根据指标类型获取指标集合(指标类型有多层)
        /// </summary>
        /// <param name="targetTypeCode">指标类型编码</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        public IList<PerformEvalTargetModel> GetPerformEvalTargetList2(string targetTypeCode, string orgId, string orgCode)
        {
            //该组织下所有的指标类型集合
            IList<PerformEvalTargetTypeModel> performEvalTargetTypes = this.PerformEvalTargetTypeFacade.Find(t => t.Orgcode == orgCode && t.Orgid == long.Parse(orgId)).Data;
            //该组织下所有的指标类别集合
            IList<PerformEvalTargetClassModel> performEvalTargetClasses = this.PerformEvalTargetClassFacade.Find(t => t.Orgcode == orgCode && t.Orgid == long.Parse(orgId)).Data;

            IList<PerformEvalTargetModel> performEvalTargets = this.PerformEvalTargetFacade.Find(t => t.Orgcode == orgCode && t.Orgid == long.Parse(orgId) && t.FTargetTypeCode.StartsWith(targetTypeCode)).Data.OrderBy(t => t.FTargetClassCode).ToList();
            if (performEvalTargets != null && performEvalTargets.Count > 0)
            {
                int max = 0;
                foreach (var per in performEvalTargets)
                {
                    if(per.FTargetTypeCode.Length/2 > max)
                    {
                        max = per.FTargetTypeCode.Length / 2;
                    }
                }
                foreach (var per in performEvalTargets)
                {
                    per.FTargetClassName = performEvalTargetClasses.ToList().Find(t => t.FCode == per.FTargetClassCode) == null ? "" : performEvalTargetClasses.ToList().Find(t => t.FCode == per.FTargetClassCode).FName;
                    per.FTargetTypeName = performEvalTargetTypes.ToList().Find(t => t.FCode == per.FTargetTypeCode) == null ? "" : performEvalTargetTypes.ToList().Find(t => t.FCode == per.FTargetTypeCode).FName;
                    per.FTargetTypePerantCode = performEvalTargetTypes.ToList().Find(t => t.FCode == per.FTargetTypeCode) == null ? "" : performEvalTargetTypes.ToList().Find(t => t.FCode == per.FTargetTypeCode).FCode;
                    if(max - per.FTargetTypeCode.Length / 2 == 0)
                    {
                        per.TypeCount = 1;
                        per.FTargetTypeCode1 = per.FTargetTypeCode;
                        per.FTargetTypeName1 = per.FTargetTypeName;
                        GetNewPerformEvalTarget(performEvalTargetTypes, per, targetTypeCode, 1);
                    }
                    else if (max - per.FTargetTypeCode.Length / 2 == 1)
                    {
                        per.TypeCount = 2;
                        per.FTargetTypeCode2 = per.FTargetTypeCode;
                        per.FTargetTypeName2 = per.FTargetTypeName;
                        GetNewPerformEvalTarget(performEvalTargetTypes, per, targetTypeCode, 1);
                    }
                    else if (max - per.FTargetTypeCode.Length / 2 == 2)
                    {
                        per.TypeCount = 3;
                        per.FTargetTypeCode3 = per.FTargetTypeCode;
                        per.FTargetTypeName3 = per.FTargetTypeName;
                        GetNewPerformEvalTarget(performEvalTargetTypes, per, targetTypeCode, 1);
                    }
                    else if (max - per.FTargetTypeCode.Length / 2 == 3)
                    {
                        per.TypeCount = 4;
                        per.FTargetTypeCode4 = per.FTargetTypeCode;
                        per.FTargetTypeName4 = per.FTargetTypeName;
                        GetNewPerformEvalTarget(performEvalTargetTypes, per, targetTypeCode, 1);
                    }
                    else if (max - per.FTargetTypeCode.Length / 2 == 4)
                    {
                        per.TypeCount = 5;
                        per.FTargetTypeCode5 = per.FTargetTypeCode;
                        per.FTargetTypeName5 = per.FTargetTypeName;
                        GetNewPerformEvalTarget(performEvalTargetTypes, per, targetTypeCode, 1);
                    }
                    else
                    {
                        continue;
                    }
                    
                }
            }

            performEvalTargets = performEvalTargets.ToList().OrderBy(t => t.FTargetTypeCode5).OrderBy(t => t.FTargetTypeCode4).OrderBy(t => t.FTargetTypeCode3).OrderBy(t => t.FTargetTypeCode2).OrderBy(t => t.FTargetTypeCode1).ToList();
            return performEvalTargets;
        }

        /// <summary>
        /// 提取多层指标类型
        /// </summary>
        /// <param name="performEvalTargetTypes">该组织所有指标类型</param>
        /// <param name="performEvalTarget">改个明细</param>
        /// <param name="targetTypeCode">选中的指标类型</param>
        /// <param name="num">记录层数</param>
        /// <returns></returns>
        public PerformEvalTargetModel GetNewPerformEvalTarget(IList<PerformEvalTargetTypeModel> performEvalTargetTypes, PerformEvalTargetModel performEvalTarget, string targetTypeCode, int num)
        {
            if (performEvalTarget != null && performEvalTargetTypes != null && performEvalTargetTypes.Count > 0 && performEvalTarget.FTargetTypePerantCode != targetTypeCode)
            {
                if (performEvalTarget.FTargetTypeCode.Length - num * 2 >= 0)
                {
                    var type = performEvalTargetTypes.ToList().Find(t => t.FCode == performEvalTarget.FTargetTypeCode.Substring(0, performEvalTarget.FTargetTypeCode.Length - num * 2));
                    if(type != null)
                    {
                        if(performEvalTarget.TypeCount == 1)
                        {
                            performEvalTarget.FTargetTypeCode2 = type.FCode;
                            performEvalTarget.FTargetTypeName2 = type.FName;
                            performEvalTarget.TypeCount++;
                            performEvalTarget.FTargetTypePerantCode = type.FCode;
                            num++;
                            GetNewPerformEvalTarget(performEvalTargetTypes, performEvalTarget, targetTypeCode, num);
                        }
                        else if (performEvalTarget.TypeCount == 2)
                        {
                            performEvalTarget.FTargetTypeCode3 = type.FCode;
                            performEvalTarget.FTargetTypeName3 = type.FName;
                            performEvalTarget.TypeCount++;
                            performEvalTarget.FTargetTypePerantCode = type.FCode;
                            num++;
                            GetNewPerformEvalTarget(performEvalTargetTypes, performEvalTarget, targetTypeCode, num);
                        }
                        else if (performEvalTarget.TypeCount == 3)
                        {
                            performEvalTarget.FTargetTypeCode4 = type.FCode;
                            performEvalTarget.FTargetTypeName4 = type.FName;
                            performEvalTarget.TypeCount++;
                            performEvalTarget.FTargetTypePerantCode = type.FCode;
                            num++;
                            GetNewPerformEvalTarget(performEvalTargetTypes, performEvalTarget, targetTypeCode, num);
                        }
                        else if (performEvalTarget.TypeCount == 4)
                        {
                            performEvalTarget.FTargetTypeCode5 = type.FCode;
                            performEvalTarget.FTargetTypeName5 = type.FName;
                            performEvalTarget.TypeCount++;
                            performEvalTarget.FTargetTypePerantCode = type.FCode;
                            num++;
                            GetNewPerformEvalTarget(performEvalTargetTypes, performEvalTarget, targetTypeCode, num);
                        }
                        else
                        {
                        
                        }
                    }
                }

            }
            return performEvalTarget;
        }

        /// <summary>
        /// 修改指标明细集合
        /// </summary>
        /// <param name="performEvalTargets">集合</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        public SavedResult<long> UpdateTargets(List<PerformEvalTargetModel> performEvalTargets,string orgId, string orgCode)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            if(performEvalTargets != null && performEvalTargets.Count > 0)
            {
                foreach(var target in performEvalTargets)
                {
                    target.Orgcode = orgCode;
                    target.Orgid = long.Parse(orgId);
                    if(target.PhId == 0)
                    {
                        target.PersistentState = PersistentState.Added;
                    }
                    else
                    {
                        if(target.PersistentState != PersistentState.Deleted)
                        {
                            target.PersistentState = PersistentState.Modified;
                        }
                    }
                }
                savedResult = this.PerformEvalTargetFacade.Save<long>(performEvalTargets);
            }
            return savedResult;
        }

        /// <summary>
        /// 保存指标类型数
        /// </summary>
        /// <param name="targetTypeModel">指标类型对象</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <param name="perCode">父级code</param>
        /// <param name="performEvalTargets">该组织明细集合</param>
        /// <returns></returns>
        public SavedResult<long> UpdateTargetType(PerformEvalTargetTypeModel targetTypeModel, string orgId, string orgCode, string perCode, IList<PerformEvalTargetModel> performEvalTargets)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            if(targetTypeModel != null && targetTypeModel.Children != null && targetTypeModel.Children.Count > 0)
            {
                foreach(var tar in targetTypeModel.Children)
                {
                    tar.Orgcode = orgCode;
                    tar.Orgid = long.Parse(orgId);
                    tar.FParentCode = perCode;
                    if(tar.PhId == 0)
                    {
                        IList<PerformEvalTargetTypeModel> allPerforms = new List<PerformEvalTargetTypeModel>();
                        if (string.IsNullOrEmpty(perCode))
                        {
                            allPerforms = this.PerformEvalTargetTypeFacade.Find(t =>t.Orgid== long.Parse(orgId) && (t.FParentCode == null || t.FParentCode == "")).Data.OrderByDescending(t => t.FCode).ToList();
                        }
                        else
                        {
                            allPerforms = this.PerformEvalTargetTypeFacade.Find(t => t.Orgid == long.Parse(orgId) && t.FParentCode == perCode).Data.OrderByDescending(t => t.FCode).ToList();
                        }
                        if(allPerforms != null && allPerforms.Count > 0)
                        {
                            var len = allPerforms[0].FCode.Length;
                            tar.FCode = (int.Parse(allPerforms[0].FCode) + 1).ToString().PadLeft(len, '0');
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(perCode))
                            {
                                tar.FCode = "01";
                            }
                            else
                            {
                                tar.FCode = perCode + "01";
                            }
                        }
                        tar.PersistentState = PersistentState.Added;
                        //若是新增的下级(判断原上级是否有明细)
                        if(performEvalTargets != null && performEvalTargets.Count > 0)
                        {
                            var per = performEvalTargets.ToList().FindAll(t => t.Orgid == long.Parse(orgId) && t.FTargetTypeCode == perCode);
                            if(per != null && per.Count > 0)
                            {
                                foreach(var pe in per)
                                {
                                    pe.PersistentState = PersistentState.Deleted;
                                }
                                this.PerformEvalTargetFacade.Save<Int64>(per);
                            }
                        }
                    }
                    else
                    {
                        if(tar.PersistentState != PersistentState.Deleted)
                        {
                            tar.PersistentState = PersistentState.Modified;
                        }
                        else
                        {
                            //如是删除根节点，则需删除对应的明细数据
                            if (performEvalTargets != null && performEvalTargets.Count > 0)
                            {
                                var per = performEvalTargets.ToList().FindAll(t => t.Orgid == long.Parse(orgId) && t.FTargetTypeCode == tar.FCode);
                                if (per != null && per.Count > 0)
                                {
                                    foreach (var pe in per)
                                    {
                                        pe.PersistentState = PersistentState.Deleted;
                                    }
                                    this.PerformEvalTargetFacade.Save<Int64>(per);
                                }
                            }
                        }
                    }
                }
                savedResult = this.PerformEvalTargetTypeFacade.Save<long>(targetTypeModel.Children);
                foreach (var tar in targetTypeModel.Children)
                {
                    UpdateTargetType(tar, orgId, orgCode, tar.FCode, performEvalTargets);
                }
            }
            return savedResult;
        }
        #endregion
    }
}

