#region Summary
/**************************************************************************************
    * 类 名 称：        ProcurementTypeDac
    * 命名空间：        GQT3.QT.Dac
    * 文 件 名：        ProcurementTypeDac.cs
    * 创建时间：        2018/10/15 
    * 作    者：        刘杭    
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

using GQT3.QT.Model.Domain;
using GQT3.QT.Dac.Interface;

namespace GQT3.QT.Dac
{
	/// <summary>
	/// ProcurementType数据访问处理类
	/// </summary>
    public partial class ProcurementTypeDac : EntDacBase<ProcurementTypeModel>, IProcurementTypeDac
    {
		#region 实现 IProcurementTypeDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<ProcurementTypeModel> ExampleMethod<ProcurementType>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

