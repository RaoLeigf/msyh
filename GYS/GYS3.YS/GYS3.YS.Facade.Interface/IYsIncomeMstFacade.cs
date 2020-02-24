#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Facade.Interface
    * 类 名 称：			IYsIncomeMstFacade
    * 文 件 名：			IYsIncomeMstFacade.cs
    * 创建时间：			2019/12/31 
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

using GYS3.YS.Model.Domain;

namespace GYS3.YS.Facade.Interface
{
	/// <summary>
	/// YsIncomeMst业务组装层接口
	/// </summary>
    public partial interface IYsIncomeMstFacade : IEntFacadeBase<YsIncomeMstModel>
    {
		#region IYsIncomeMstFacade 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="ysIncomeMstEntity"></param>
		/// <param name="ysIncomeDtlEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveYsIncomeMst(YsIncomeMstModel ysIncomeMstEntity, List<YsIncomeDtlModel> ysIncomeDtlEntities);


        /// <summary>
        /// 根据主键与审批状态修改主表的审批状态
        /// </summary>
        /// <param name="phid">主表主键</param>
        /// <param name="fApproval">审批状态</param>
        /// <returns></returns>
        SavedResult<long> UpdateInCome(long phid, byte fApproval);
        #endregion
    }
}
