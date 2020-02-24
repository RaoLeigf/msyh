#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Facade.Interface
    * 类 名 称：			IQTMemoFacade
    * 文 件 名：			IQTMemoFacade.cs
    * 创建时间：			2019/5/15 
    * 作    者：			刘杭    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GQT3.QT.Model.Domain;
using SUP.Common.Base;

namespace GQT3.QT.Facade.Interface
{
	/// <summary>
	/// QTMemo业务组装层接口
	/// </summary>
    public partial interface IQTMemoFacade : IEntFacadeBase<QTMemoModel>
    {
        #region IQTMemoFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<QTMemoModel> ExampleMethod<QTMemoModel>(string param)


        Hashtable GetFormRights(Int64 userid, Int64 orgid, string userType, string pageName);

        DataTable GetLoadMenu(string product, string suite, bool isusbuser, string usertype, Int64 orgID, Int64 userID, string nodeid, bool rightFlag, bool lazyLoadFlag, string treeFilter);
        #endregion
    }
}
