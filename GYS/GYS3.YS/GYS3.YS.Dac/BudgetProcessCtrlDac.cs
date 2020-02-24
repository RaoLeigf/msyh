#region Summary
/**************************************************************************************
    * 类 名 称：        BudgetProcessCtrlDac
    * 命名空间：        GYS3.YS.Dac
    * 文 件 名：        BudgetProcessCtrlDac.cs
    * 创建时间：        2018/9/10 
    * 作    者：        夏华军    
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

using GYS3.YS.Model.Domain;
using GYS3.YS.Dac.Interface;

namespace GYS3.YS.Dac
{
	/// <summary>
	/// BudgetProcessCtrl数据访问处理类
	/// </summary>
    public partial class BudgetProcessCtrlDac : EntDacBase<BudgetProcessCtrlModel>, IBudgetProcessCtrlDac
    {
		#region 实现 IBudgetProcessCtrlDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<BudgetProcessCtrlModel> ExampleMethod<BudgetProcessCtrl>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

