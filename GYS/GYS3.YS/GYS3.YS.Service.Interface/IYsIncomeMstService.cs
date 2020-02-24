#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Service.Interface
    * 类 名 称：			IYsIncomeMstService
    * 文 件 名：			IYsIncomeMstService.cs
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
using GQT3.QT.Model.Domain;
using GYS3.YS.Model.Domain;

namespace GYS3.YS.Service.Interface
{
	/// <summary>
	/// YsIncomeMst服务组装层接口
	/// </summary>
    public partial interface IYsIncomeMstService : IEntServiceBase<YsIncomeMstModel>
    {
		#region IYsIncomeMstService 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="ysIncomeMstEntity"></param>
		/// <param name="ysIncomeDtlEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveYsIncomeMst(YsIncomeMstModel ysIncomeMstEntity, List<YsIncomeDtlModel> ysIncomeDtlEntities);

        /// <summary>
        /// 通过外键值获取YsIncomeDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<YsIncomeDtlModel> FindYsIncomeDtlByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 保存单个组织的收入预算
        /// </summary>
        /// <param name="ysIncomeMst">收入预算对象</param>
        /// <param name="ysIncomeDtls">收入预算子表集合</param>
        /// <returns></returns>
        SavedResult<long> SaveYsIncome(YsIncomeMstModel ysIncomeMst, IList<YsIncomeDtlModel> ysIncomeDtls);


        /// <summary>
        /// 删除收入预算数据
        /// </summary>
        /// <param name="phid">收入预算主键</param>
        /// <returns></returns>
        DeletedResult SaveDelete(long phid);

        /// <summary>
        /// 根据收入预算主键生成预算
        /// </summary>
        /// <param name="phid">收入预算主键</param>
        /// <param name="userId">人员主键</param>
        /// <returns></returns>
        SavedResult<long> SaveBudget(long phid, long userId);

        /// <summary>
        /// 获取当前所有组织信息
        /// </summary>
        /// <returns></returns>
        IList<OrganizeModel> GetAllOrganize();
        #endregion
    }
}
