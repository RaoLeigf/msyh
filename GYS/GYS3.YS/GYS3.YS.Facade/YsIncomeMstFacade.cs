#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Facade
    * 类 名 称：			YsIncomeMstFacade
    * 文 件 名：			YsIncomeMstFacade.cs
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
using SUP.Common.DataEntity;
using SUP.Common.DataAccess;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Facade;

using GYS3.YS.Facade.Interface;
using GYS3.YS.Rule.Interface;
using GYS3.YS.Model.Domain;
using SUP.Common.Base;

namespace GYS3.YS.Facade
{
	/// <summary>
	/// YsIncomeMst业务组装处理类
	/// </summary>
    public partial class YsIncomeMstFacade : EntFacadeBase<YsIncomeMstModel>, IYsIncomeMstFacade
    {
		#region 类变量及属性
		/// <summary>
        /// YsIncomeMst业务逻辑处理对象
        /// </summary>
		IYsIncomeMstRule YsIncomeMstRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IYsIncomeMstRule;
            }
        }
		/// <summary>
        /// YsIncomeDtl业务逻辑处理对象
        /// </summary>
		IYsIncomeDtlRule YsIncomeDtlRule { get; set; }
		#endregion

		#region 重载方法
        /// <summary>
        /// 分页获取全部集合
        /// </summary>
        /// <param name="pageNumber">页码(从1开始)</param>
        /// <param name="pageSize">每页大小(最大为200)</param>
        /// <param name="dic">查询条件字典（建议使用Enterprise3.Common.Base.CreateCriteria进行创建）</param>
        /// <param name="sorts">排序({属性名 desc,属性名,属性名 asc})</param>
        /// <returns>集合</returns>
        public override PagedResult<YsIncomeMstModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<YsIncomeMstModel> pageResult = base.LoadWithPage(pageNumber, pageSize, dic, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<YsIncomeMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<YsIncomeMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return pageResult;
        }

        /// <summary>
        /// 分页获取全部集合
        /// </summary>
        /// <param name="pageNumber">页码(从1开始)</param>
        /// <param name="pageSize">每页大小(最大为200)</param>
        /// <param name="nameSqlName">命名SQL名称</param>
        /// <param name="dic">查询条件字典（建议使用Enterprise3.Common.Base.CreateCriteria进行创建）</param>
        /// <param name="sorts">排序({属性名 desc,属性名,属性名 asc})</param>
        /// <returns>集合</returns>
        public override PagedResult<YsIncomeMstModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, bool isUseInfoRight = false, params string[] sorts)
        {
            PagedResult<YsIncomeMstModel> pageResult = base.LoadWithPage(pageNumber, pageSize, nameSqlName, dic, isUseInfoRight, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<YsIncomeMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<YsIncomeMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return pageResult;
        }

        /// <summary>
        /// 通过id，删除数据
        /// </summary>
        /// <param name="id">单主键id值</param>
        public override DeletedResult Delete<TValType>(TValType id)
        {
			YsIncomeDtlRule.RuleHelper.DeleteByForeignKey(id);
			return base.Delete(id);
        }

		/// <summary>
        /// 通过ids，删除数据
        /// </summary>
        /// <param name="ids">单主键id集合</param>
        public override DeletedResult Delete<TValType>(IList<TValType> ids)
        {
			YsIncomeDtlRule.RuleHelper.DeleteByForeignKey(ids);
			return base.Delete(ids);
        }
        #endregion

		#region 实现 IYsIncomeMstFacade 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="ysIncomeMstEntity"></param>
		/// <param name="ysIncomeDtlEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveYsIncomeMst(YsIncomeMstModel ysIncomeMstEntity, List<YsIncomeDtlModel> ysIncomeDtlEntities)
        {
            SavedResult<Int64> savedResult = base.Save<Int64>(ysIncomeMstEntity);
            if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
            {
				if (ysIncomeDtlEntities.Count > 0)
				{
					YsIncomeDtlRule.Save(ysIncomeDtlEntities, savedResult.KeyCodes[0]);
				}
            }

			return savedResult;
        }
        #endregion

        #region//审批相关

        /// <summary>
        /// 根据主键与审批状态修改主表的审批状态
        /// </summary>
        /// <param name="phid">主表主键</param>
        /// <param name="fApproval">审批状态</param>
        /// <returns></returns>
        public SavedResult<long> UpdateInCome(long phid, byte fApproval)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            if (phid != 0)
            {
                YsIncomeMstModel ysIncomeMst = this.YsIncomeMstRule.Find(phid);
                if(ysIncomeMst != null)
                {
                    ysIncomeMst.FApproval = fApproval;
                    ysIncomeMst.PersistentState = PersistentState.Modified;
                    savedResult = this.YsIncomeMstRule.Save<Int64>(ysIncomeMst);
                }
            }
            return savedResult;
        }
        #endregion
    }
}

