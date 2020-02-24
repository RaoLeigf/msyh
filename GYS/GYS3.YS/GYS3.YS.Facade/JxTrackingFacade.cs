#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Facade
    * 类 名 称：			JxTrackingFacade
    * 文 件 名：			JxTrackingFacade.cs
    * 创建时间：			2019/10/17 
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
using SUP.Common.DataEntity;
using SUP.Common.DataAccess;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Facade;

using GYS3.YS.Facade.Interface;
using GYS3.YS.Rule.Interface;
using GYS3.YS.Model.Domain;

namespace GYS3.YS.Facade
{
	/// <summary>
	/// JxTracking业务组装处理类
	/// </summary>
    public partial class JxTrackingFacade : EntFacadeBase<JxTrackingModel>, IJxTrackingFacade
    {
		#region 类变量及属性
		/// <summary>
        /// JxTracking业务逻辑处理对象
        /// </summary>
		IJxTrackingRule JxTrackingRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IJxTrackingRule;
            }
        }
		#endregion

		#region 重载方法
		/// <summary>
        /// 通过外关联的单主键值，获取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns>实体</returns>
        public override FindedResults<JxTrackingModel> FindByForeignKey<TValType>(TValType id, params string[] sorts)
        {
            FindedResults<JxTrackingModel> findedResults = base.FindByForeignKey(id, sorts);

            #region 明细Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
            //helpdac.CodeToName<JxTrackingModel>(findedResults.Data, "属性名", "注册的帮助标识"
            //helpdac.CodeToName<JxTrackingModel>(findedResults.Data, "Code属性名", "Name属性名", "注册的帮助标识", "");
            helpdac.CodeToName<JxTrackingModel>(findedResults.Data, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist", "");
            #endregion

            return findedResults;
        }

        #endregion

		#region 实现 IJxTrackingFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<JxTrackingModel> ExampleMethod<JxTrackingModel>(string param)
        //{
        //    //编写代码
        //}

        #endregion
    }
}

