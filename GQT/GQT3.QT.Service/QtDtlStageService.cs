#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service
    * 类 名 称：			QtDtlStageService
    * 文 件 名：			QtDtlStageService.cs
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

namespace GQT3.QT.Service
{
	/// <summary>
	/// QtDtlStage服务组装处理类
	/// </summary>
    public partial class QtDtlStageService : EntServiceBase<QtDtlStageModel>, IQtDtlStageService
    {
		#region 类变量及属性
		/// <summary>
        /// QtDtlStage业务外观处理对象
        /// </summary>
		IQtDtlStageFacade QtDtlStageFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IQtDtlStageFacade;
            }
        }
		#endregion

		#region 实现 IQtDtlStageService 业务添加的成员


        #endregion
    }
}

