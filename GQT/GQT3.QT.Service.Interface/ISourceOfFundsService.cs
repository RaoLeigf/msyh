#region Summary
/**************************************************************************************
    * 类 名 称：        ISourceOfFundsService
    * 命名空间：        GQT3.QT.Service.Interface
    * 文 件 名：        ISourceOfFundsService.cs
    * 创建时间：        2018/9/3 
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
	/// SourceOfFunds服务组装层接口
	/// </summary>
    public partial interface ISourceOfFundsService : IEntServiceBase<SourceOfFundsModel>
    {
        #region ISourceOfFundsService 业务添加的成员

        /// <summary>
        /// 根据支出类别(项目类型)的code判断是否是末级
        /// </summary>
        /// <returns>返回Json串</returns>
        FindedResults<SourceOfFundsModel> IfLastStage(string code);
        #endregion
    }
}
