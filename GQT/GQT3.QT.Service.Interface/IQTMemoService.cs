#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service.Interface
    * 类 名 称：			IQTMemoService
    * 文 件 名：			IQTMemoService.cs
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

namespace GQT3.QT.Service.Interface
{
	/// <summary>
	/// QTMemo服务组装层接口
	/// </summary>
    public partial interface IQTMemoService : IEntServiceBase<QTMemoModel>
    {
        #region IQTMemoService 业务添加的成员

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="adddata"></param>
        /// <param name="updatedata"></param>
        /// <param name="deletedata"></param>
        /// <returns></returns>
        CommonResult Save2(List<QTMemoModel> adddata, List<QTMemoModel> updatedata, List<string> deletedata);
        /// <summary>
        /// 
        /// </summary>
        Hashtable GetFormRights(Int64 userid, Int64 orgid, string userType, string pageName);

        DataTable GetLoadMenu(string product, string suite, bool isusbuser, string usertype, Int64 orgID, Int64 userID, string nodeid, bool rightFlag, bool lazyLoadFlag, string treeFilter);
        #endregion
    }
}
