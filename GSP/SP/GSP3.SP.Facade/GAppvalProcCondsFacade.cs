#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Facade
    * 类 名 称：			GAppvalProcCondsFacade
    * 文 件 名：			GAppvalProcCondsFacade.cs
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
using SUP.Common.DataEntity;
using SUP.Common.DataAccess;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Facade;

using GSP3.SP.Facade.Interface;
using GSP3.SP.Rule.Interface;
using GSP3.SP.Model.Domain;

namespace GSP3.SP.Facade
{
	/// <summary>
	/// GAppvalProcConds业务组装处理类
	/// </summary>
    public partial class GAppvalProcCondsFacade : EntFacadeBase<GAppvalProcCondsModel>, IGAppvalProcCondsFacade
    {
		#region 类变量及属性
		/// <summary>
        /// GAppvalProcConds业务逻辑处理对象
        /// </summary>
		IGAppvalProcCondsRule GAppvalProcCondsRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IGAppvalProcCondsRule;
            }
        }
		#endregion

		#region 重载方法
		/// <summary>
        /// 通过外关联的单主键值，获取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns>实体</returns>
        public override FindedResults<GAppvalProcCondsModel> FindByForeignKey<TValType>(TValType id, params string[] sorts)
        {
            FindedResults<GAppvalProcCondsModel> findedResults = base.FindByForeignKey(id, sorts);

            #region 明细Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<GAppvalProcCondsModel>(findedResults.Data, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<GAppvalProcCondsModel>(findedResults.Data, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return findedResults;
        }

        #endregion

		#region 实现 IGAppvalProcCondsFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<GAppvalProcCondsModel> ExampleMethod<GAppvalProcCondsModel>(string param)
        //{
        //    //编写代码
        //}

        #endregion
    }
}

