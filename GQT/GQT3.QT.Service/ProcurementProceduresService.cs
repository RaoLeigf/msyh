#region Summary
/**************************************************************************************
    * 类 名 称：        ProcurementProceduresService
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        ProcurementProceduresService.cs
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
using SUP.Common.Base;

namespace GQT3.QT.Service
{
	/// <summary>
	/// ProcurementProcedures服务组装处理类
	/// </summary>
    public partial class ProcurementProceduresService : EntServiceBase<ProcurementProceduresModel>, IProcurementProceduresService
    {
		#region 类变量及属性
		/// <summary>
        /// ProcurementProcedures业务外观处理对象
        /// </summary>
		IProcurementProceduresFacade ProcurementProceduresFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IProcurementProceduresFacade;
            }
        }
        #endregion

        #region 实现 IProcurementProceduresService 业务添加的成员

        /// <summary>
        /// 根据组织获取所有采购程序集合
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        public IList<ProcurementProceduresModel> GetProcurementProcedures(string orgId, string orgCode)
        {
            IList<ProcurementProceduresModel> procurementProcedures = new List<ProcurementProceduresModel>();
            procurementProcedures = this.ProcurementProceduresFacade.Find(t => t.Orgid == long.Parse(orgId) && t.Orgcode == orgCode).Data.OrderBy(t => t.FCode).ToList();
            return procurementProcedures;
        }


        /// <summary>
        /// 保存采购程序集合
        /// </summary>
        /// <param name="procurementProcedures">集合对象</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        public SavedResult<long> UpdateProcurementProcedures(IList<ProcurementProceduresModel> procurementProcedures, string orgId, string orgCode)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            if (procurementProcedures != null && procurementProcedures.Count > 0)
            {
                foreach (var pro in procurementProcedures)
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
                savedResult = this.ProcurementProceduresFacade.Save<long>(procurementProcedures);
            }
            return savedResult;
        }
        #endregion
    }
}

