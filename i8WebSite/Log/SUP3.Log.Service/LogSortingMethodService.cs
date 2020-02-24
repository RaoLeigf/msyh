#region Summary
/**************************************************************************************
    * 类 名 称：        LogSortingMethodService
    * 命名空间：        SUP3.Log.Service
    * 文 件 名：        LogSortingMethodService.cs
    * 创建时间：        2017/11/4 
    * 作    者：        洪鹏    
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

using SUP3.Log.Service.Interface;
using SUP3.Log.Facade.Interface;
using SUP3.Log.Model.Domain;

namespace SUP3.Log.Service
{
	/// <summary>
	/// LogSortingMethod服务组装处理类
	/// </summary>
    public partial class LogSortingMethodService : EntServiceBase<LogSortingMethodModel>, ILogSortingMethodService
    {
		#region 类变量及属性
		/// <summary>
        /// LogSortingMethod业务外观处理对象
        /// </summary>
		ILogSortingMethodFacade LogSortingMethodFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("BaseLanguage", "InitializeObjectFail");

                return CurrentFacade as ILogSortingMethodFacade;
            }
        }
		#endregion

		#region 实现 ILogSortingMethodService 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public List<LogSortingMethodModel> ExampleMethod<LogSortingMethodModel>(string param)
        //{
        //    //编写代码
        //}


        #endregion
    }
}

