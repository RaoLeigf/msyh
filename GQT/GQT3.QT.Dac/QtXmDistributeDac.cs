#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Dac
    * 类 名 称：			QtXmDistributeDac
    * 文 件 名：			QtXmDistributeDac.cs
    * 创建时间：			2020/1/6 
    * 作    者：			刘杭    
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
	/// QtXmDistribute数据访问处理类
	/// </summary>
    public partial class QtXmDistributeDac : EntDacBase<QtXmDistributeModel>, IQtXmDistributeDac
    {
		#region 实现 IQtXmDistributeDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<QtXmDistributeModel> ExampleMethod<QtXmDistribute>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

