#region Summary
/**************************************************************************************
    * 类 名 称：        IExamplesFacade
    * 命名空间：        GXM3.XM.Facade.Interface
    * 文 件 名：        IExamplesFacade.cs
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
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GXM3.XM.Model;

namespace GXM3.XM.Facade.Interface
{
    /// <summary>
    /// Examples业务组装层接口
    /// </summary>
    public partial interface IExamplesFacade : IEntFacadeBase<ExamplesModel>
    {
        #region IExamplesFacade 业务添加的成员

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

        #endregion
    }
}
