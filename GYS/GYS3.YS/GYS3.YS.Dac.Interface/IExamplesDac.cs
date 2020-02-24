#region Summary
/**************************************************************************************
    * 类 名 称：        IExamplesDac
    * 命名空间：        GYS3.YS.Dac.Interface
    * 文 件 名：        IExamplesDac.cs
    * 创建时间：        2018/8/17 9:53:11 
    * 作    者：        丰立新    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Enterprise3.NHORM.Interface.EntBase;

using GYS3.YS.Model;

namespace GYS3.YS.Dac.Interface
{
    /// <summary>
    /// Examples数据访问层接口
    /// </summary>
    public partial interface IExamplesDac : IEntDacBase<ExamplesModel>
    {
        #region IExamplesDac 业务添加的成员

        /// <summary>
        /// 获取所有有效的组织信息
        /// </summary>
        /// <returns>组织集合</returns>
        IList<ExamplesModel> GetAllEffectiveOrgs();

        /// <summary>
        /// 获取指定组织所有有效的部门信息
        /// </summary>
        /// <param name="ocode">组织编码（目前通过编码设置关系）</param>
        /// <returns></returns>
        IList<ExamplesModel> GetAllEffectiveDepts(string ocode);

        /// <summary>
        /// 获取指定Id集合的部门信息
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns>组织信息</returns>
	    IList<ExamplesModel> GetDeptsByIds(List<Int64> ids);

        /// <summary>
        /// 获取指定Id集合的组织信息
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns>组织信息</returns>
	    IList<ExamplesModel> GetOrgsByIds(List<Int64> ids);

        #endregion
    }
}

