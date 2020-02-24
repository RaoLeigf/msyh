#region Summary
/**************************************************************************************
    * 类 名 称：        QtSysCodeSeqService
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        QtSysCodeSeqService.cs
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
using SUP.Common.DataEntity;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Service;

using GQT3.QT.Service.Interface;
using GQT3.QT.Facade.Interface;
using GQT3.QT.Model.Domain;

namespace GQT3.QT.Service
{
	/// <summary>
	/// QtSysCodeSeq服务组装处理类
	/// </summary>
    public partial class QtSysCodeSeqService : EntServiceBase<QtSysCodeSeqModel>, IQtSysCodeSeqService
    {
		#region 类变量及属性
		/// <summary>
        /// QtSysCodeSeq业务外观处理对象
        /// </summary>
		IQtSysCodeSeqFacade QtSysCodeSeqFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IQtSysCodeSeqFacade;
            }
        }

        /// <summary>
        /// 返回对应年度、标识代码和表名的系统编码对象
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="code">标识代码</param>
        /// <param name="tname">表名</param>
        /// <returns></returns>
        public FindedResults<QtSysCodeSeqModel> GetSysCodeSeq(string year, string code, string tname)
        {
            FindedResults<QtSysCodeSeqModel> result = base.Find(t => t.FYear == year && t.FCode == code && t.FTname == tname,  "FCode" );
            return result;
        }
        #endregion

        #region 实现 IQtSysCodeSeqService 业务添加的成员


        #endregion
    }
}

