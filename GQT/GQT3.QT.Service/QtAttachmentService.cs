#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service
    * 类 名 称：			QtAttachmentService
    * 文 件 名：			QtAttachmentService.cs
    * 创建时间：			2019/6/17 
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
	/// QtAttachment服务组装处理类
	/// </summary>
    public partial class QtAttachmentService : EntServiceBase<QtAttachmentModel>, IQtAttachmentService
    {
		#region 类变量及属性
		/// <summary>
        /// QtAttachment业务外观处理对象
        /// </summary>
		IQtAttachmentFacade QtAttachmentFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IQtAttachmentFacade;
            }
        }
		#endregion

		#region 实现 IQtAttachmentService 业务添加的成员


        #endregion
    }
}

