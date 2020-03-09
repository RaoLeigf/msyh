﻿#region Summary
/**************************************************************************************
    * 命名空间：			GXM3.XM.Facade.Interface
    * 类 名 称：			IXmReportReturnFacade
    * 文 件 名：			IXmReportReturnFacade.cs
    * 创建时间：			2020/1/17 
    * 作    者：			王冠冠    
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

using GXM3.XM.Model.Domain;

namespace GXM3.XM.Facade.Interface
{
    /// <summary>
    /// XmReportReturn业务组装层接口
    /// </summary>
    public partial interface IXmReportReturnFacade : IEntFacadeBase<XmReportReturnModel>
    {
        #region IXmReportReturnFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<XmReportReturnModel> ExampleMethod<XmReportReturnModel>(string param)

        #endregion
    }
}