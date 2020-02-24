#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Dac
    * 类 名 称：			QTEditMemoDac
    * 文 件 名：			QTEditMemoDac.cs
    * 创建时间：			2019/5/29 
    * 作    者：			董泉伟    
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

using GQT3.QT.Model.Domain;
using GQT3.QT.Dac.Interface;

namespace GQT3.QT.Dac
{
	/// <summary>
	/// QTEditMemo数据访问处理类
	/// </summary>
    public partial class QTEditMemoDac : EntDacBase<QTEditMemoModel>, IQTEditMemoDac
    {
		#region 实现 IQTEditMemoDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<QTEditMemoModel> ExampleMethod<QTEditMemo>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

