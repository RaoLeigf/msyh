#region Summary
/**************************************************************************************
    * 类 名 称：        IQtSysCodeSeqService
    * 命名空间：        GQT3.QT.Service.Interface
    * 文 件 名：        IQtSysCodeSeqService.cs
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

namespace GQT3.QT.Service.Interface
{
	/// <summary>
	/// QtSysCodeSeq服务组装层接口
	/// </summary>
    public partial interface IQtSysCodeSeqService : IEntServiceBase<QtSysCodeSeqModel>
    {
        #region IQtSysCodeSeqService 业务添加的成员

        /// <summary>
        /// 返回对应年度、标识代码和表名的系统编码对象
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="code">标识代码</param>
        /// <param name="tname">表名</param>
        /// <returns></returns>
        FindedResults<QtSysCodeSeqModel> GetSysCodeSeq(string year, string code, string tname);
		#endregion
    }
}
