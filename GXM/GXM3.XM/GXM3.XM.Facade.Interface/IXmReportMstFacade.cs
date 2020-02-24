#region Summary
/**************************************************************************************
    * 命名空间：			GXM3.XM.Facade.Interface
    * 类 名 称：			IXmReportMstFacade
    * 文 件 名：			IXmReportMstFacade.cs
    * 创建时间：			2020/1/17 
    * 作    者：			王冠冠    
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

using GXM3.XM.Model.Domain;

namespace GXM3.XM.Facade.Interface
{
	/// <summary>
	/// XmReportMst业务组装层接口
	/// </summary>
    public partial interface IXmReportMstFacade : IEntFacadeBase<XmReportMstModel>
    {
		#region IXmReportMstFacade 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="xmReportMstEntity"></param>
		/// <param name="xmReportDtlEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveXmReportMst(XmReportMstModel xmReportMstEntity, List<XmReportDtlModel> xmReportDtlEntities);

		#endregion
    }
}
