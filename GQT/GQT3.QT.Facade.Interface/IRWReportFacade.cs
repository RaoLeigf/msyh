#region Summary
/**************************************************************************************
    * 类 名 称：        IRWReportFacade
    * 命名空间：        GQT3.QT.Facade.Interface
    * 文 件 名：        IRWReportFacade.cs
    * 创建时间：        2018/10/9 
    * 作    者：        夏华军    
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
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GQT3.QT.Model.Domain;

namespace GQT3.QT.Facade.Interface
{
	/// <summary>
	/// RWReport业务组装层接口
	/// </summary>
    public partial interface IRWReportFacade : IEntFacadeBase<RWReportModel>
    {
		#region IRWReportFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<RWReportModel> ExampleMethod<RWReportModel>(string param)

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="rWReportEntity"></param>
		/// <param name="rWReportEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveRWReport(RWReportModel rWReportEntity, List<RWReportModel> rWReportEntities);

		#endregion
    }
}
