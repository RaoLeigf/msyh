#region Summary
/**************************************************************************************
    * 类 名 称：        PerformEvalTargetTypeService
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        PerformEvalTargetTypeService.cs
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

namespace GQT3.QT.Service
{
	/// <summary>
	/// PerformEvalTargetType服务组装处理类
	/// </summary>
    public partial class PerformEvalTargetTypeService : EntServiceBase<PerformEvalTargetTypeModel>, IPerformEvalTargetTypeService
    {
		#region 类变量及属性
		/// <summary>
        /// PerformEvalTargetType业务外观处理对象
        /// </summary>
		IPerformEvalTargetTypeFacade PerformEvalTargetTypeFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IPerformEvalTargetTypeFacade;
            }
        }
        #endregion

        #region 实现 IPerformEvalTargetTypeService 业务添加的成员

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <returns></returns>
        public FindedResults<PerformEvalTargetTypeModel> ExecuteDataCheck(ref List<PerformEvalTargetTypeModel> performEvalTargetType, string otype)
        {
            IList<string> dm = new List<string>();
            FindedResults<PerformEvalTargetTypeModel> results = new FindedResults<PerformEvalTargetTypeModel>();
            if (performEvalTargetType == null)
            {
                results.Status = ResponseStatus.Error;
                results.Msg = "保存失败，数据异常！";
            }
            else
            {
                for (int i = 0; i < performEvalTargetType.Count; i++)
                {
                    performEvalTargetType[i].FCode = performEvalTargetType[i].FCode.Replace(" ", "");
                    performEvalTargetType[i].FName = performEvalTargetType[i].FName.Replace(" ", "");
                    performEvalTargetType[i].FRemark.Trim();
                    dm.Add(performEvalTargetType[i].FCode);
                }
                var dicWhere1 = new Dictionary<string, object>();
                new CreateCriteria(dicWhere1).
                    Add(ORMRestrictions<IList<string>>.In("FCode", dm));
                
                if (base.Find(dicWhere1) != null && base.Find(dicWhere1).Data.Count > 0 && otype != "edit")
                {
                    results = base.Find(dicWhere1);
                    results.Status = ResponseStatus.Error;
                    results.Msg = "保存失败，代码重复！";
                }
                
            }
            return results;
        }

        #endregion
    }
}

