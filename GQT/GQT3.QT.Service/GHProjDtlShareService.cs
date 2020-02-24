#region Summary
/**************************************************************************************
    * 类 名 称：        GHProjDtlShareService
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        GHProjDtlShareService.cs
    * 创建时间：        2018/9/11 
    * 作    者：        李明    
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
	/// GHProjDtlShare服务组装处理类
	/// </summary>
    public partial class GHProjDtlShareService : EntServiceBase<GHProjDtlShareModel>, IGHProjDtlShareService
    {
		#region 类变量及属性
		/// <summary>
        /// GHProjDtlShare业务外观处理对象
        /// </summary>
		IGHProjDtlShareFacade GHProjDtlShareFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IGHProjDtlShareFacade;
            }
        }
		#endregion

		#region 实现 IGHProjDtlShareService 业务添加的成员


        #endregion
    }
}

