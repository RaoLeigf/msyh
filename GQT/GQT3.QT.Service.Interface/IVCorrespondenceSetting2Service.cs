#region Summary
/**************************************************************************************
    * 类 名 称：        IVCorrespondenceSetting2Service
    * 命名空间：        GQT3.QT.Service.Interface
    * 文 件 名：        IVCorrespondenceSetting2Service.cs
    * 创建时间：        2018/9/13 
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

namespace GQT3.QT.Service.Interface
{
	/// <summary>
	/// VCorrespondenceSetting2服务组装层接口
	/// </summary>
    public partial interface IVCorrespondenceSetting2Service : IEntServiceBase<VCorrespondenceSetting2Model>
    {
        #region IVCorrespondenceSetting2Service 业务添加的成员

        List<OrganizeModel> GetOrange();
		#endregion
    }
}
