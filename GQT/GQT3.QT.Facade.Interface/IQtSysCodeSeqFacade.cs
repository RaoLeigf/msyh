#region Summary
/**************************************************************************************
    * 类 名 称：        IQtSysCodeSeqFacade
    * 命名空间：        GQT3.QT.Facade.Interface
    * 文 件 名：        IQtSysCodeSeqFacade.cs
    * 创建时间：        2018/9/10 
    * 作    者：        李明    
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
	/// QtSysCodeSeq业务组装层接口
	/// </summary>
    public partial interface IQtSysCodeSeqFacade : IEntFacadeBase<QtSysCodeSeqModel>
    {
        #region IQtSysCodeSeqFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<QtSysCodeSeqModel> ExampleMethod<QtSysCodeSeqModel>(string param)

        PagedResult<QtSysCodeSeqModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts);
        #endregion
    }
}
