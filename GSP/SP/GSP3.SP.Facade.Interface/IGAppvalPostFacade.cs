#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Facade.Interface
    * 类 名 称：			IGAppvalPostFacade
    * 文 件 名：			IGAppvalPostFacade.cs
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
	/// GAppvalPost业务组装层接口
	/// </summary>
    public partial interface IGAppvalPostFacade : IEntFacadeBase<GAppvalPostModel>
    {
		#region IGAppvalPostFacade 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="gAppvalPostEntity"></param>
		/// <param name="gAppvalPost4OperEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveGAppvalPost(GAppvalPostModel gAppvalPostEntity, List<GAppvalPost4OperModel> gAppvalPost4OperEntities);

        /// <summary>
        /// 根据审批流程的id查找审批岗位
        /// </summary>
        /// <param name="phid">审批岗位id</param>
        /// <returns></returns>
        List<GAppvalPostModel> FindAppvalPostByProcID(long phid);

        #endregion
    }
}
