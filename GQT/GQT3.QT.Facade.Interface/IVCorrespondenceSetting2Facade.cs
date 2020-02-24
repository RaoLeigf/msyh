#region Summary
/**************************************************************************************
    * 类 名 称：        IVCorrespondenceSetting2Facade
    * 命名空间：        GQT3.QT.Facade.Interface
    * 文 件 名：        IVCorrespondenceSetting2Facade.cs
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

namespace GQT3.QT.Facade.Interface
{
	/// <summary>
	/// VCorrespondenceSetting2业务组装层接口
	/// </summary>
    public partial interface IVCorrespondenceSetting2Facade : IEntFacadeBase<VCorrespondenceSetting2Model>
    {
        #region IVCorrespondenceSetting2Facade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<VCorrespondenceSetting2Model> ExampleMethod<VCorrespondenceSetting2Model>(string param)

        PagedResult<VCorrespondenceSetting2Model> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, params string[] sorts);
        #endregion
    }
}
