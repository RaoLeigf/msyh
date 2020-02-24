#region Summary
/**************************************************************************************
    * 类 名 称：        IQTProductUserService
    * 命名空间：        GQT3.QT.Service.Interface
    * 文 件 名：        IQTProductUserService.cs
    * 创建时间：        2018/12/12 
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
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GQT3.QT.Model.Domain;

namespace GQT3.QT.Service.Interface
{
	/// <summary>
	/// QTProductUser服务组装层接口
	/// </summary>
    public partial interface IQTProductUserService : IEntServiceBase<QTProductUserModel>
    {
        #region IQTProductUserService 业务添加的成员
        /// <summary>
        /// 保存时获取产品主键
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        SavedResult<Int64> Save2(IList<QTProductUserModel> entities);

        /// <summary>
        /// 获取产品用户通过产品标识和G6账号
        /// </summary>
        /// <param name="ProductBZ"></param>
        /// <param name="UserNo"></param>
        /// <returns></returns>
        QTProductUserModel getUserByProduct(string ProductBZ, string UserNo);

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <returns></returns>
        SavedResult<Int64> SaveImportData(string fileExtension,string filePath, string clear, string ProductBZ, long ProductPhid);

        #endregion
    }
}
