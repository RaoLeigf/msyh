#region Summary
/**************************************************************************************
    * 命名空间：			GXM3.XM.Facade.Interface
    * 类 名 称：			IProjectDtlPersonNameFacade
    * 文 件 名：			IProjectDtlPersonNameFacade.cs
    * 创建时间：			2020/1/14 
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

using GXM3.XM.Model.Domain;

namespace GXM3.XM.Facade.Interface
{
	/// <summary>
	/// ProjectDtlPersonName业务组装层接口
	/// </summary>
    public partial interface IProjectDtlPersonNameFacade : IEntFacadeBase<ProjectDtlPersonNameModel>
    {
        #region IProjectDtlPersonNameFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<ProjectDtlPersonNameModel> ExampleMethod<ProjectDtlPersonNameModel>(string param)


        /// <summary>
        /// 保存维护人员集合
        /// </summary>
        /// <param name="projectDtlPersonNames">人员集合</param>
        /// <param name="phid">单据主键</param>
        /// <returns></returns>
        SavedResult<long> SaveMSYHPersonNames(List<ProjectDtlPersonNameModel> projectDtlPersonNames, long phid);
        #endregion
    }
}
