#region Summary
/**************************************************************************************
    * 类 名 称：        IOrganizationRule
    * 命名空间：        GQT3.QT.Rule.Interface
    * 文 件 名：        IOrganizationRule.cs
    * 创建时间：        2018/9/13 
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
using System.Text;
using Enterprise3.NHORM.Interface.EntBase;

using GQT3.QT.Model.Domain;

namespace GQT3.QT.Rule.Interface
{
	/// <summary>
	/// Organization业务逻辑层接口
	/// </summary>
    public partial interface IOrganizationRule : IEntRuleBase<OrganizeModel>
    {
        #region IOrganizationRule 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<OrganizationModel> ExampleMethod<OrganizationModel>(string param)

        /// <summary>
        /// 提供给其他facade层调用
        /// </summary>
        /// <param name="dicWhere"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        IList<OrganizeModel> Find2(Dictionary<string, object> dicWhere, params string[] sorts);
        #endregion
    }
}
