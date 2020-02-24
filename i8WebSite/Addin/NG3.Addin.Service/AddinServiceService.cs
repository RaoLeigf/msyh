#region Summary
/**************************************************************************************
    * 类 名 称：        AddinServiceService
    * 命名空间：        NG3.Addin.Service
    * 文 件 名：        AddinServiceService.cs
    * 创建时间：        2017/12/13 
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

using NG3.Addin.Service.Interface;
using NG3.Addin.Facade.Interface;
using NG3.Addin.Model.Domain;

namespace NG3.Addin.Service
{
	/// <summary>
	/// AddinService服务组装处理类
	/// </summary>
    public partial class AddinServiceService : EntServiceBase<AddinServiceModel>, IAddinServiceService
    {
		#region 类变量及属性
		/// <summary>
        /// AddinService业务外观处理对象
        /// </summary>
		IAddinServiceFacade AddinServiceFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("BaseLanguage", "InitializeObjectFail");

                return CurrentFacade as IAddinServiceFacade;
            }
        }
		#endregion

		#region 实现 IAddinServiceService 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<AddinServiceModel> ExampleMethod<AddinServiceModel>(string param)
        //{
        //    //编写代码
        //}


        #endregion
    }
}

