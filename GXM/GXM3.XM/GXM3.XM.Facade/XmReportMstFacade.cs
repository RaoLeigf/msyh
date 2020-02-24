#region Summary
/**************************************************************************************
    * 命名空间：			GXM3.XM.Facade
    * 类 名 称：			XmReportMstFacade
    * 文 件 名：			XmReportMstFacade.cs
    * 创建时间：			2020/1/17 
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

using GXM3.XM.Facade.Interface;
using GXM3.XM.Rule.Interface;
using GXM3.XM.Model.Domain;

namespace GXM3.XM.Facade
{
	/// <summary>
	/// XmReportMst业务组装处理类
	/// </summary>
    public partial class XmReportMstFacade : EntFacadeBase<XmReportMstModel>, IXmReportMstFacade
    {
		#region 类变量及属性
		/// <summary>
        /// XmReportMst业务逻辑处理对象
        /// </summary>
		IXmReportMstRule XmReportMstRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IXmReportMstRule;
            }
        }
		/// <summary>
        /// XmReportDtl业务逻辑处理对象
        /// </summary>
		IXmReportDtlRule XmReportDtlRule { get; set; }
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
        public override PagedResult<XmReportMstModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<XmReportMstModel> pageResult = base.LoadWithPage(pageNumber, pageSize, dic, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<XmReportMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<XmReportMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
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
        public override PagedResult<XmReportMstModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, bool isUseInfoRight = false, params string[] sorts)
        {
            PagedResult<XmReportMstModel> pageResult = base.LoadWithPage(pageNumber, pageSize, nameSqlName, dic, isUseInfoRight, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<XmReportMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<XmReportMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return pageResult;
        }

        /// <summary>
        /// 通过id，删除数据
        /// </summary>
        /// <param name="id">单主键id值</param>
        public override DeletedResult Delete<TValType>(TValType id)
        {
			XmReportDtlRule.RuleHelper.DeleteByForeignKey(id);
			return base.Delete(id);
        }

		/// <summary>
        /// 通过ids，删除数据
        /// </summary>
        /// <param name="ids">单主键id集合</param>
        public override DeletedResult Delete<TValType>(IList<TValType> ids)
        {
			XmReportDtlRule.RuleHelper.DeleteByForeignKey(ids);
			return base.Delete(ids);
        }
        #endregion

		#region 实现 IXmReportMstFacade 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="xmReportMstEntity"></param>
		/// <param name="xmReportDtlEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveXmReportMst(XmReportMstModel xmReportMstEntity, List<XmReportDtlModel> xmReportDtlEntities)
        {
            SavedResult<Int64> savedResult = base.Save<Int64>(xmReportMstEntity);
            if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
            {
				if (xmReportDtlEntities.Count > 0)
				{
					XmReportDtlRule.Save(xmReportDtlEntities, savedResult.KeyCodes[0]);
				}
            }

			return savedResult;
        }

        #endregion
    }
}

