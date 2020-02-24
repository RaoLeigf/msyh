#region Summary
/**************************************************************************************
    * 类 名 称：        IQtOrgDygxRule
    * 命名空间：        GQT3.QT.Rule.Interface
    * 文 件 名：        IQtOrgDygxRule.cs
    * 创建时间：        2019/2/14 
    * 作    者：        刘杭    
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
	/// QtOrgDygx业务逻辑层接口
	/// </summary>
    public partial interface IQtOrgDygxRule : IEntRuleBase<QtOrgDygxModel>
    {
        #region IQtOrgDygxRule 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<QtOrgDygxModel> ExampleMethod<QtOrgDygxModel>(string param)

        /// <summary>
        /// 根据Xmorg找项目库和老g6h对应关系
        /// </summary>
        /// <param name="Xmorg"></param>
        /// <returns></returns>
        IList<QtOrgDygxModel> findByXmorg(string Xmorg);

        /// <summary>
        /// 根据Oldorg找项目库和老g6h对应关系
        /// </summary>
        /// <param name="Oldorg"></param>
        /// <returns></returns>
        IList<QtOrgDygxModel> findByOldorg(string Oldorg);
        #endregion
    }
}
