#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service.Interface
    * 类 名 称：			IQtTableCustomizeService
    * 文 件 名：			IQtTableCustomizeService.cs
    * 创建时间：			2019/11/26 
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

using GQT3.QT.Model.Domain;

namespace GQT3.QT.Service.Interface
{
	/// <summary>
	/// QtTableCustomize服务组装层接口
	/// </summary>
    public partial interface IQtTableCustomizeService : IEntServiceBase<QtTableCustomizeModel>
    {
        #region IQtTableCustomizeService 业务添加的成员

        /// <summary>
        /// 修改列表自定义集合
        /// </summary>
        /// <param name="qtTableCustomizes">列表自定义集合</param>
        /// <returns></returns>
        SavedResult<long> UpdateQtTableCustomizes(List<QtTableCustomizeModel> qtTableCustomizes);

        /// <summary>
        /// 根据用户与表编码获取该表格所有列是否显示的数据
        /// </summary>
        /// <param name="uid">用户账号</param>
        /// <param name="tableCode">表格编码</param>
        /// <returns></returns>
        IList<QtTableCustomizeModel> GetQtTableCustomizes(long uid, string tableCode);

        #endregion
    }
}
