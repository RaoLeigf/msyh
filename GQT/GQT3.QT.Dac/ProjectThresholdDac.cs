#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectThresholdDac
    * 命名空间：        GQT3.QT.Dac
    * 文 件 名：        ProjectThresholdDac.cs
    * 创建时间：        2018/10/17 
    * 作    者：        李长敏琛    
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
	/// ProjectThreshold数据访问处理类
	/// </summary>
    public partial class ProjectThresholdDac : EntDacBase<ProjectThresholdModel>, IProjectThresholdDac
    {
		#region 实现 IProjectThresholdDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<ProjectThresholdModel> ExampleMethod<ProjectThreshold>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

