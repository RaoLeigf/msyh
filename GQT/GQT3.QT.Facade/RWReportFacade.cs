#region Summary
/**************************************************************************************
    * 类 名 称：        RWReportFacade
    * 命名空间：        GQT3.QT.Facade
    * 文 件 名：        RWReportFacade.cs
    * 创建时间：        2018/10/9 
    * 作    者：        夏华军    
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

using GQT3.QT.Facade.Interface;
using GQT3.QT.Rule.Interface;
using GQT3.QT.Model.Domain;

namespace GQT3.QT.Facade
{
	/// <summary>
	/// RWReport业务组装处理类
	/// </summary>
    public partial class RWReportFacade : EntFacadeBase<RWReportModel>, IRWReportFacade
    {
		#region 类变量及属性
		/// <summary>
        /// RWReport业务逻辑处理对象
        /// </summary>
		IRWReportRule RWReportRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IRWReportRule;
            }
        }
		/// <summary>
        /// RWReport业务逻辑处理对象
        /// </summary>
		//IRWReportRule RWReportRule { get; set; }
		#endregion

		#region 重载方法
		/// <summary>
        /// 通过外关联的单主键值，获取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns>实体</returns>
        public override FindedResults<RWReportModel> FindByForeignKey<TValType>(TValType id, params string[] sorts)
        {
            FindedResults<RWReportModel> findedResults = base.FindByForeignKey(id, sorts);

            #region 明细Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<RWReportModel>(findedResults.Data, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<RWReportModel>(findedResults.Data, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return findedResults;
        }

        /// <summary>
        /// 通过id，删除数据
        /// </summary>
        /// <param name="id">单主键id值</param>
        public override DeletedResult Delete<TValType>(TValType id)
        {
			RWReportRule.RuleHelper.DeleteByForeignKey(id);
			return base.Delete(id);
        }

		/// <summary>
        /// 通过ids，删除数据
        /// </summary>
        /// <param name="ids">单主键id集合</param>
        public override DeletedResult Delete<TValType>(IList<TValType> ids)
        {
			RWReportRule.RuleHelper.DeleteByForeignKey(ids);
			return base.Delete(ids);
        }
        #endregion

		#region 实现 IRWReportFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<RWReportModel> ExampleMethod<RWReportModel>(string param)
        //{
        //    //编写代码
        //}

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="rWReportEntity"></param>
		/// <param name="rWReportEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveRWReport(RWReportModel rWReportEntity, List<RWReportModel> rWReportEntities)
        {
            SavedResult<Int64> savedResult = base.Save<Int64>(rWReportEntity);
            if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
            {
				if (rWReportEntities.Count > 0)
				{
					RWReportRule.Save(rWReportEntities, savedResult.KeyCodes[0]);
				}
            }

			return savedResult;
        }

        #endregion
    }
}

