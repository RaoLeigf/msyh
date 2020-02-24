#region Summary
/**************************************************************************************
    * 类 名 称：        PerformEvalTargetClassService
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        PerformEvalTargetClassService.cs
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
using SUP.Common.Base;

namespace GQT3.QT.Service
{
	/// <summary>
	/// PerformEvalTargetClass服务组装处理类
	/// </summary>
    public partial class PerformEvalTargetClassService : EntServiceBase<PerformEvalTargetClassModel>, IPerformEvalTargetClassService
    {
		#region 类变量及属性
		/// <summary>
        /// PerformEvalTargetClass业务外观处理对象
        /// </summary>
		IPerformEvalTargetClassFacade PerformEvalTargetClassFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IPerformEvalTargetClassFacade;
            }
        }
		#endregion

		#region 实现 IPerformEvalTargetClassService 业务添加的成员

        /// <summary>
        /// 获取评价指标类别
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        public IList<PerformEvalTargetClassModel> GetPerformEvalTargetClasses(string orgId, string orgCode)
        {
            IList<PerformEvalTargetClassModel> performEvalTargetClasses = new List<PerformEvalTargetClassModel>();
            performEvalTargetClasses = this.PerformEvalTargetClassFacade.Find(t => t.Orgid == long.Parse(orgId) && t.Orgcode == orgCode).Data.OrderBy(t => t.FCode).ToList(); 
            return performEvalTargetClasses;
        }


        /// <summary>
        /// 保存绩效评价指标类别集合
        /// </summary>
        /// <param name="performEvalTargetClasses">集合对象</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        public SavedResult<long> UpdatePerformEvalTargetClasses(IList<PerformEvalTargetClassModel> performEvalTargetClasses, string orgId, string orgCode)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            if (performEvalTargetClasses != null && performEvalTargetClasses.Count > 0)
            {
                foreach (var pro in performEvalTargetClasses)
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
                savedResult = this.PerformEvalTargetClassFacade.Save<long>(performEvalTargetClasses);
            }
            return savedResult;
        }
        #endregion
    }
}

