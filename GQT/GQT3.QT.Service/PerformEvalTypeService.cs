#region Summary
/**************************************************************************************
    * 类 名 称：        PerformEvalTypeService
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        PerformEvalTypeService.cs
    * 创建时间：        2018/10/16 
    * 作    者：        李长敏琛    
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
	/// PerformEvalType服务组装处理类
	/// </summary>
    public partial class PerformEvalTypeService : EntServiceBase<PerformEvalTypeModel>, IPerformEvalTypeService
    {
		#region 类变量及属性
		/// <summary>
        /// PerformEvalType业务外观处理对象
        /// </summary>
		IPerformEvalTypeFacade PerformEvalTypeFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IPerformEvalTypeFacade;
            }
        }
        #endregion

        #region 实现 IPerformEvalTypeService 业务添加的成员

        /// <summary>
        /// 获取绩效评价类型
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        public IList<PerformEvalTypeModel> GetPerformEvalTypes(string orgId, string orgCode)
        {
            IList<PerformEvalTypeModel> performEvalTypes = new List<PerformEvalTypeModel>();
            performEvalTypes = this.PerformEvalTypeFacade.Find(t => t.Orgid == long.Parse(orgId) && t.Orgcode == orgCode).Data.OrderBy(t => t.FCode).ToList();
            return performEvalTypes;
        }

        /// <summary>
        /// 保存绩效评价类型集合
        /// </summary>
        /// <param name="performEvalTypes">集合对象</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        public SavedResult<long> UpdatePerformEvalTypes(IList<PerformEvalTypeModel> performEvalTypes, string orgId, string orgCode)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            if (performEvalTypes != null && performEvalTypes.Count > 0)
            {
                foreach (var pro in performEvalTypes)
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
                savedResult = this.PerformEvalTypeFacade.Save<long>(performEvalTypes);
            }
            return savedResult;
        }
        #endregion
    }
}

