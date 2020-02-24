#region Summary
/**************************************************************************************
    * 类 名 称：        IExamplesRule
    * 命名空间：        GXM3.XM.Rule.Interface
    * 文 件 名：        IExamplesRule.cs
    * 创建时间：        2018/8/17 9:44:50 
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
using System.Text;
using Enterprise3.NHORM.Interface.EntBase;
using SUP.Common.Base;

using GXM3.XM.Model;

namespace GXM3.XM.Rule.Interface
{
    /// <summary>
    /// Examples业务逻辑层接口
    /// </summary>
    public partial interface IExamplesRule : IEntRuleBase<ExamplesModel>
    {
        #region IExamplesRule 业务添加的成员

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

        /// <summary>
        /// 获取指定Id集合的部门信息(并添加空部门信息PhId=0,OCode="none",OName="")
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <param name="oname">空部门名称(默认空值)</param>
        /// <returns>组织信息</returns>
        IList<ExamplesModel> GetDeptsAddEmptyByIds(List<Int64> ids, string oname = "");

        /// <summary>
        /// 获取指定Id集合的组织信息(并添加空组织信息PhId=0,OCode="none",OName="")
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <param name="oname">空部门名称(默认空值)</param>
        /// <returns>组织信息</returns>
        IList<ExamplesModel> GetOrgsAddEmptyByIds(List<Int64> ids, string oname = "");

        /// <summary>
        /// 构建组织树数据
        /// </summary>
        /// <param name="list">组织集合</param>
        /// <returns></returns>
        IList<TreeJSONBase> GetOrgTreeData(IList<ExamplesModel> list);
        #endregion
    }
}
