#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service
    * 类 名 称：			QtAccountService
    * 文 件 名：			QtAccountService.cs
    * 创建时间：			2019/9/18 
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
	/// QtAccount服务组装处理类
	/// </summary>
    public partial class QtAccountService : EntServiceBase<QtAccountModel>, IQtAccountService
    {
		#region 类变量及属性
		/// <summary>
        /// QtAccount业务外观处理对象
        /// </summary>
		IQtAccountFacade QtAccountFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IQtAccountFacade;
            }
        }
		#endregion

		#region 实现 IQtAccountService 业务添加的成员


        #endregion
    }
}

