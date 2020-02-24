#region Summary
/**************************************************************************************
    * 类 名 称：        IProjectThresholdService
    * 命名空间：        GQT3.QT.Service.Interface
    * 文 件 名：        IProjectThresholdService.cs
    * 创建时间：        2018/10/17 
    * 作    者：        李长敏琛    
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

using GQT3.QT.Model.Domain;
using SUP.Common.DataEntity;

namespace GQT3.QT.Service.Interface
{
	/// <summary>
	/// ProjectThreshold服务组装层接口
	/// </summary>
    public partial interface IProjectThresholdService : IEntServiceBase<ProjectThresholdModel>
    {
        #region IProjectThresholdService 业务添加的成员
        bool SaveOrUpdate(string data,IList<OrganizeModel> organizes, IList<ProjectThresholdModel> projectThresholds,DataStoreParam dataStoreParam);
        IList<OrganizeModel> GetSBOrganizes(IList<OrganizeModel> organizes, IList<CorrespondenceSettings2Model> correspondenceSettings2s);
		#endregion
    }
}
