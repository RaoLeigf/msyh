#region Summary
/**************************************************************************************
    * 命名空间：			GGK3.GK.Service
    * 类 名 称：			GK3_BankVauitService
    * 文 件 名：			GK3_BankVauitService.cs
    * 创建时间：			2019/11/18 
    * 作    者：			张宇    
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

using GGK3.GK.Service.Interface;
using GGK3.GK.Facade.Interface;
using GGK3.GK.Model.Domain;

namespace GGK3.GK.Service
{
	/// <summary>
	/// GK3_BankVauit服务组装处理类
	/// </summary>
    public partial class GK3_BankVauitService : EntServiceBase<GK3_BankVauitModel>, IGK3_BankVauitService
    {
		#region 类变量及属性
		/// <summary>
        /// GK3_BankVauit业务外观处理对象
        /// </summary>
		IGK3_BankVauitFacade GK3_BankVauitFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IGK3_BankVauitFacade;
            }
        }
		#endregion

		#region 实现 IGK3_BankVauitService 业务添加的成员


        #endregion
    }
}

