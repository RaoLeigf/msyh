#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Facade.Interface
    * 类 名 称：			IGAppvalPost4OperFacade
    * 文 件 名：			IGAppvalPost4OperFacade.cs
    * 创建时间：			2019/5/20 
    * 作    者：			李明    
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

using GSP3.SP.Model.Domain;

namespace GSP3.SP.Facade.Interface
{
	/// <summary>
	/// GAppvalPost4Oper业务组装层接口
	/// </summary>
    public partial interface IGAppvalPost4OperFacade : IEntFacadeBase<GAppvalPost4OperModel>
    {
        #region IGAppvalPost4OperFacade 业务添加的成员

        /// <summary>
        /// 根据岗位id查找所有的操作员
        /// </summary>
        /// <param name="postId">岗位id</param>
        /// <returns></returns>
        List<GAppvalPost4OperModel> GetOperatorsByPostID(long postId);
        #endregion
    }
}
