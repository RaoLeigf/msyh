#region Summary
/**************************************************************************************
    * 类 名 称：        IExamplesService
    * 命名空间：        GQT3.QT.Service.Interface
    * 文 件 名：        IExamplesService.cs
    * 创建时间：        2018/8/17 9:54:20 
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

using GQT3.QT.Model;

namespace GQT3.QT.Service.Interface
{
    /// <summary>
    /// Examples服务组装处理接口类
    /// </summary>
    public partial interface IExamplesService : IEntServiceBase<ExamplesModel>
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
