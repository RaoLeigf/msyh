#region Summary
/**************************************************************************************
    * 类 名 称：        IProcurementProceduresFacade
    * 命名空间：        GQT3.QT.Facade.Interface
    * 文 件 名：        IProcurementProceduresFacade.cs
    * 创建时间：        2018/10/15 
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
	/// ProcurementProcedures业务组装层接口
	/// </summary>
    public partial interface IProcurementProceduresFacade : IEntFacadeBase<ProcurementProceduresModel>
    {
        #region IProcurementProceduresFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<ProcurementProceduresModel> ExampleMethod<ProcurementProceduresModel>(string param)

        PagedResult<ProcurementProceduresModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts);

        /// <summary>
        /// 通过代码找名称
        /// </summary>
        /// <param name="Dm"></param>
        /// <returns></returns>
        string FindMcByDm(string Dm);
        #endregion
    }
}
