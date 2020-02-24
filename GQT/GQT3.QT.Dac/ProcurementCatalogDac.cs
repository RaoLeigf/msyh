#region Summary
/**************************************************************************************
    * 类 名 称：        ProcurementCatalogDac
    * 命名空间：        GQT3.QT.Dac
    * 文 件 名：        ProcurementCatalogDac.cs
    * 创建时间：        2018/10/16 
    * 作    者：        李长敏琛    
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
	/// ProcurementCatalog数据访问处理类
	/// </summary>
    public partial class ProcurementCatalogDac : EntDacBase<ProcurementCatalogModel>, IProcurementCatalogDac
    {
		#region 实现 IProcurementCatalogDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<ProcurementCatalogModel> ExampleMethod<ProcurementCatalog>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

