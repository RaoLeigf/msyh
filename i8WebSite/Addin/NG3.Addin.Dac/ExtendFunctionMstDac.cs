#region Summary
/**************************************************************************************
    * 类 名 称：        ExtendFunctionMstDac
    * 命名空间：        NG3.Addin.Dac
    * 文 件 名：        ExtendFunctionMstDac.cs
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
using System.Text;
using Enterprise3.NHORM.Dac;

using NG3.Addin.Model.Domain;
using NG3.Addin.Dac.Interface;

namespace NG3.Addin.Dac
{
	/// <summary>
	/// ExtendFunctionMst数据访问处理类
	/// </summary>
    public partial class ExtendFunctionMstDac : EntDacBase<ExtendFunctionMstModel>, IExtendFunctionMstDac
    {
		#region 实现 IExtendFunctionMstDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public List<ExtendFunctionMstModel> ExampleMethod<ExtendFunctionMst>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

