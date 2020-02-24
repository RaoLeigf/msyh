#region Summary
/**************************************************************************************
    * 命名空间：			GXM3.XM.Dac
    * 类 名 称：			XmReportMstDac
    * 文 件 名：			XmReportMstDac.cs
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
	/// XmReportMst数据访问处理类
	/// </summary>
    public partial class XmReportMstDac : EntDacBase<XmReportMstModel>, IXmReportMstDac
    {
		#region 实现 IXmReportMstDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<XmReportMstModel> ExampleMethod<XmReportMst>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

