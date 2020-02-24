#region Summary
/**************************************************************************************
    * 类 名 称：        AddinExpressionService
    * 命名空间：        NG3.Addin.Service
    * 文 件 名：        AddinExpressionService.cs
    * 创建时间：        2017/7/10 
    * 作    者：        韦忠吉    
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
	/// AddinExpression服务组装处理类
	/// </summary>
    public partial class AddinExpressionService : EntServiceBase<AddinExpressionModel>, IAddinExpressionService
    {
		#region 类变量及属性
		/// <summary>
        /// AddinExpression业务外观处理对象
        /// </summary>
		IAddinExpressionFacade AddinExpressionFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("BaseLanguage", "InitializeObjectFail");

                return CurrentFacade as IAddinExpressionFacade;
            }
        }
		#endregion

		#region 实现 IAddinExpressionService 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public List<AddinExpressionModel> ExampleMethod<AddinExpressionModel>(string param)
        //{
        //    //编写代码
        //}


        #endregion
    }
}

