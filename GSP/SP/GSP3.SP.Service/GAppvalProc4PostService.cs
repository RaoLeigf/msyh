#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Service
    * 类 名 称：			GAppvalProc4PostService
    * 文 件 名：			GAppvalProc4PostService.cs
    * 创建时间：			2019/5/21 
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

using GSP3.SP.Service.Interface;
using GSP3.SP.Facade.Interface;
using GSP3.SP.Model.Domain;

namespace GSP3.SP.Service
{
	/// <summary>
	/// GAppvalProc4Post服务组装处理类
	/// </summary>
    public partial class GAppvalProc4PostService : EntServiceBase<GAppvalProc4PostModel>, IGAppvalProc4PostService
    {
		#region 类变量及属性
		/// <summary>
        /// GAppvalProc4Post业务外观处理对象
        /// </summary>
		IGAppvalProc4PostFacade GAppvalProc4PostFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IGAppvalProc4PostFacade;
            }
        }
		#endregion

		#region 实现 IGAppvalProc4PostService 业务添加的成员


        #endregion
    }
}

