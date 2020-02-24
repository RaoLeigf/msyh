#region Summary
/**************************************************************************************
    * 类 名 称：        IQtZcgnflService
    * 命名空间：        GQT3.QT.Service.Interface
    * 文 件 名：        IQtZcgnflService.cs
    * 创建时间：        2019/1/23 
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
	/// QtZcgnfl服务组装层接口
	/// </summary>
    public partial interface IQtZcgnflService : IEntServiceBase<QtZcgnflModel>
    {
        #region IQtZcgnflService 业务添加的成员
        /// <summary>
        /// 根据code判断是否是末级
        /// </summary>
        /// <returns>返回Json串</returns>
        FindedResults<QtZcgnflModel> IfLastStage(string code);


        /// <summary>
        /// 根据组织获取支出功能分类列表
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        IList<QtZcgnflModel> GetZcgnfls(string orgId, string orgCode);
        #endregion
    }
}
