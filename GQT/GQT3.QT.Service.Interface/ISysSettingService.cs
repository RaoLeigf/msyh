using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GQT3.QT.Model.Domain;
using SUP.Common.Base;

namespace GQT3.QT.Service.Interface
{
    /// <summary>
    /// 系统设置
    /// </summary>
    public partial interface ISysSettingService : IEntServiceBase<QTMemoModel>
    {
        /// <summary>
        /// 
        /// </summary>
        void GetFormRights(string userid, string orgid);

        IList<TreeJSONBase> GetLoadMenu(string product, string suite, bool isusbuser, string usertype, Int64 orgID, Int64 userID, string nodeid, bool rightFlag, bool lazyLoadFlag, string treeFilter);
    }
}
