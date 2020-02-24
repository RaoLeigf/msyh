#region Summary
/**************************************************************************************
    * 类 名 称：        BudgetProcessCtrlService
    * 命名空间：        GYS3.YS.Service
    * 文 件 名：        BudgetProcessCtrlService.cs
    * 创建时间：        2018/9/10 
    * 作    者：        夏华军    
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

using GYS3.YS.Service.Interface;
using GYS3.YS.Facade.Interface;
using GYS3.YS.Model.Extend;

namespace GYS3.YS.Service
{
	/// <summary>
	/// BudgetProcessCtrl服务组装处理类
	/// </summary>
    public partial class BuDeptReportService : EntServiceBase<BuDeptReportModel>, IBuDeptReportService
    {
		#region 类变量及属性
		/// <summary>
        /// BudgetProcessCtrl业务外观处理对象
        /// </summary>
		IBuDeptReportFacade BuDeptReportFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IBuDeptReportFacade;
            }
        }
        #endregion


    }
}

