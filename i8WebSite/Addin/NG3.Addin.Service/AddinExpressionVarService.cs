#region Summary
/**************************************************************************************
    * 类 名 称：        AddinExpressionVarService
    * 命名空间：        NG3.Addin.Service
    * 文 件 名：        AddinExpressionVarService.cs
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
	/// AddinExpressionVar服务组装处理类
	/// </summary>
    public partial class AddinExpressionVarService : EntServiceBase<AddinExpressionVarModel>, IAddinExpressionVarService
    {
		#region 类变量及属性
		/// <summary>
        /// AddinExpressionVar业务外观处理对象
        /// </summary>
		IAddinExpressionVarFacade AddinExpressionVarFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("BaseLanguage", "InitializeObjectFail");

                return CurrentFacade as IAddinExpressionVarFacade;
            }
        }
		#endregion

		#region 实现 IAddinExpressionVarService 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public List<AddinExpressionVarModel> ExampleMethod<AddinExpressionVarModel>(string param)
        //{
        //    //编写代码
        //}


        #endregion
    }
}

