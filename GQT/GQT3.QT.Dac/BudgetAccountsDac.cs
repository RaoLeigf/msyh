#region Summary
/**************************************************************************************
    * 类 名 称：        BudgetAccountsDac
    * 命名空间：        GQT3.QT.Dac
    * 文 件 名：        BudgetAccountsDac.cs
    * 创建时间：        2018/8/29 
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
using Enterprise3.NHORM.Dac;

using GQT3.QT.Model.Domain;
using GQT3.QT.Dac.Interface;
using NHibernate.Cfg;
using NHibernate;

namespace GQT3.QT.Dac
{
    /// <summary>
    /// BudgetAccounts数据访问处理类
    /// </summary>
    public partial class BudgetAccountsDac : EntDacBase<BudgetAccountsModel>, IBudgetAccountsDac
    {


        #region 实现 IBudgetAccountsDac 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<BudgetAccountsModel> ExampleMethod<BudgetAccounts>(string param)
        //{
        //    //编写代码
        //}

        
        #endregion
    }
}

