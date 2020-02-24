#region Summary
/**************************************************************************************
    * 类 名 称：        ProcurementTypeService
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        ProcurementTypeService.cs
    * 创建时间：        2018/10/15 
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
    /// ProcurementType服务组装处理类
    /// </summary>
    public partial class ProcurementTypeService : EntServiceBase<ProcurementTypeModel>, IProcurementTypeService
    {
        #region 类变量及属性
        /// <summary>
        /// ProcurementType业务外观处理对象
        /// </summary>
        IProcurementTypeFacade ProcurementTypeFacade
        {
            get
            {
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IProcurementTypeFacade;
            }
        }
        #endregion

        #region 实现 IProcurementTypeService 业务添加的成员

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <returns></returns>
        public FindedResults<ProcurementTypeModel> ExecuteDataCheck(ref List<ProcurementTypeModel> procurementTypes, string otype)
        {
            IList<string> dm = new List<string>();
            IList<string> mc = new List<string>();
            FindedResults<ProcurementTypeModel> results = new FindedResults<ProcurementTypeModel>();
            if (procurementTypes == null)
            {
                results.Status = ResponseStatus.Error;
                results.Msg = "保存失败，数据异常！";
            }
            else
            {
                for (int i = 0; i < procurementTypes.Count; i++)
                {
                    procurementTypes[i].FCode = procurementTypes[i].FCode.Replace(" ", "");
                    procurementTypes[i].FName = procurementTypes[i].FName.Replace(" ", "");
                    procurementTypes[i].FRemark.Trim();
                    dm.Add(procurementTypes[i].FCode);
                    mc.Add(procurementTypes[i].FName);
                }
                var dicWhere1 = new Dictionary<string, object>();
                var dicWhere2 = new Dictionary<string, object>();
                new CreateCriteria(dicWhere1).
                    Add(ORMRestrictions<IList<string>>.In("FCode", dm));
                new CreateCriteria(dicWhere2).
                    Add(ORMRestrictions<IList<string>>.In("FName", mc));
                if (base.Find(dicWhere1) != null && base.Find(dicWhere1).Data.Count > 0 && otype != "edit")
                {
                    results = base.Find(dicWhere1);
                    results.Status = ResponseStatus.Error;
                    results.Msg = "保存失败，代码重复！";
                }
                if (base.Find(dicWhere2) != null && base.Find(dicWhere2).Data.Count > 0)
                {
                    results = base.Find(dicWhere2);
                    results.Status = ResponseStatus.Error;
                    results.Msg = "保存失败，名称重复！";
                }
            }
            return results;
        }


        /// <summary>
        /// 根据组织获取所有采购类型集合
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        public IList<ProcurementTypeModel> GetProcurementTypes(string orgId, string orgCode)
        {
            IList<ProcurementTypeModel> procurementTypes = new List<ProcurementTypeModel>();
            procurementTypes = this.ProcurementTypeFacade.Find(t => t.Orgid == long.Parse(orgId) && t.Orgcode == orgCode).Data.OrderBy(t => t.FCode).ToList();
            return procurementTypes;
        }

        /// <summary>
        /// 保存采购类型集合
        /// </summary>
        /// <param name="procurementTypes">集合对象</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        public SavedResult<long> UpdateProcurementTypes(IList<ProcurementTypeModel> procurementTypes, string orgId, string orgCode)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            if (procurementTypes != null && procurementTypes.Count > 0)
            {
                foreach (var pro in procurementTypes)
                {
                    pro.Orgcode = orgCode;
                    pro.Orgid = long.Parse(orgId);
                    if (pro.PhId == 0)
                    {
                        pro.PersistentState = PersistentState.Added;
                    }
                    else
                    {
                        if (pro.PersistentState != PersistentState.Deleted)
                        {
                            pro.PersistentState = PersistentState.Modified;
                        }
                    }
                }
                savedResult = this.ProcurementTypeFacade.Save<long>(procurementTypes);
            }
            return savedResult;
        }
        #endregion
    }
}

