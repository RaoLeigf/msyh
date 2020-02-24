#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service.Interface
    * 类 名 称：			IQtYJKService
    * 文 件 名：			IQtYJKService.cs
    * 创建时间：			2019/4/15 
    * 作    者：			刘杭    
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
	/// QtYJK服务组装层接口
	/// </summary>
    public partial interface IQtYJKService : IEntServiceBase<QtYJKModel>
    {
        #region IQtYJKService 业务添加的成员

        /// <summary>
        /// 更新意见库
        /// </summary>
        /// <param name="DeleteYJPhids"></param>
        /// <param name="Changedatas"></param>
        /// <param name="Insertdatas"></param>
        /// <returns></returns>
        SavedResult<Int64> Update1(List<long> DeleteYJPhids, List<QtYJKModel> Changedatas, List<QtYJKModel> Insertdatas);
        #endregion
    }
}
