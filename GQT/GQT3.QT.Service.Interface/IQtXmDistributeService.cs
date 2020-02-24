#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service.Interface
    * 类 名 称：			IQtXmDistributeService
    * 文 件 名：			IQtXmDistributeService.cs
    * 创建时间：			2020/1/6 
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
	/// QtXmDistribute服务组装层接口
	/// </summary>
    public partial interface IQtXmDistributeService : IEntServiceBase<QtXmDistributeModel>
    {
        #region IQtXmDistributeService 业务添加的成员
        /// <summary>
        /// 获取最大项目库编码
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        string CreateOrGetMaxProjCode(string year);

        #endregion
    }
}
