#region Summary
/**************************************************************************************
    * 类 名 称：        ProjLibProjService
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        ProjLibProjService.cs
    * 创建时间：        2018/9/10 
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
	/// ProjLibProj服务组装处理类
	/// </summary>
    public partial class ProjLibProjService : EntServiceBase<ProjLibProjModel>, IProjLibProjService
    {
		#region 类变量及属性
		/// <summary>
        /// ProjLibProj业务外观处理对象
        /// </summary>
		IProjLibProjFacade ProjLibProjFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IProjLibProjFacade;
            }
        }
		#endregion

		#region 实现 IProjLibProjService 业务添加的成员


        #endregion
    }
}

