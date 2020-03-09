﻿#region Summary
/**************************************************************************************
    * 命名空间：			GXM3.XM.Dac
    * 类 名 称：			XmReportDtlDac
    * 文 件 名：			XmReportDtlDac.cs
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
using Enterprise3.NHORM.Dac;

using GXM3.XM.Model.Domain;
using GXM3.XM.Dac.Interface;

namespace GXM3.XM.Dac
{
    /// <summary>
    /// XmReportReturn数据访问处理类
    /// </summary>
    public partial class XmReportReturnDac : EntDacBase<XmReportReturnModel>, IXmReportReturnDac
    {
        #region 实现 IXmReportReturnDac 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<XmReportReturnModel> ExampleMethod<XmReportReturn>(string param)
        //{
        //    //编写代码
        //}

        #endregion
    }
}
