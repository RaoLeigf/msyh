#region Summary
/**************************************************************************************
    * 类 名 称：        IQtOrgDygxService
    * 命名空间：        GQT3.QT.Service.Interface
    * 文 件 名：        IQtOrgDygxService.cs
    * 创建时间：        2019/2/14 
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
	/// QtOrgDygx服务组装层接口
	/// </summary>
    public partial interface IQtOrgDygxService : IEntServiceBase<QtOrgDygxModel>
    {
        #region IQtOrgDygxService 业务添加的成员

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="adddata"></param>
        /// <param name="updatedata"></param>
        /// <param name="deletedata"></param>
        /// <returns></returns>
        CommonResult Save2(List<QtOrgDygxModel> adddata, List<QtOrgDygxModel> updatedata, List<string> deletedata);

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="fileExtension"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        SavedResult<Int64> ImportData(string fileExtension, string filePath);

        #endregion
    }
}
