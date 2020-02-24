#region Summary
/**************************************************************************************
    * 类 名 称：        ICorrespondenceSettingsFacade
    * 命名空间：        GQT3.QT.Facade.Interface
    * 文 件 名：        ICorrespondenceSettingsFacade.cs
    * 创建时间：        2018/9/3 
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
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GQT3.QT.Model.Domain;

namespace GQT3.QT.Facade.Interface
{
	/// <summary>
	/// CorrespondenceSettings业务组装层接口
	/// </summary>
    public partial interface ICorrespondenceSettingsFacade : IEntFacadeBase<CorrespondenceSettingsModel>
    {
        #region ICorrespondenceSettingsFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<CorrespondenceSettingsModel> ExampleMethod<CorrespondenceSettingsModel>(string param)

        /// <summary>
        /// 根据当前usercode 获取所拥有部门信息
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        List<CorrespondenceSettingsModel> GetUserDepementList(string usercode);

        PagedResult<CorrespondenceSettingsModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts);
        #endregion
    }
}
