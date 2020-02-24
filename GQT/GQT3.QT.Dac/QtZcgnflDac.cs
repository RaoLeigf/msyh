#region Summary
/**************************************************************************************
    * 类 名 称：        QtZcgnflDac
    * 命名空间：        GQT3.QT.Dac
    * 文 件 名：        QtZcgnflDac.cs
    * 创建时间：        2019/1/23 
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
using Enterprise3.NHORM.Dac;

using GQT3.QT.Model.Domain;
using GQT3.QT.Dac.Interface;

namespace GQT3.QT.Dac
{
	/// <summary>
	/// QtZcgnfl数据访问处理类
	/// </summary>
    public partial class QtZcgnflDac : EntDacBase<QtZcgnflModel>, IQtZcgnflDac
    {
		#region 实现 IQtZcgnflDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<QtZcgnflModel> ExampleMethod<QtZcgnfl>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

