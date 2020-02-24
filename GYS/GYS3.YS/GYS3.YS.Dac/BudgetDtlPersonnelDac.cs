#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Dac
    * 类 名 称：			BudgetDtlPersonnelDac
    * 文 件 名：			BudgetDtlPersonnelDac.cs
    * 创建时间：			2020/1/2 
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

using GYS3.YS.Model.Domain;
using GYS3.YS.Dac.Interface;

namespace GYS3.YS.Dac
{
	/// <summary>
	/// BudgetDtlPersonnel数据访问处理类
	/// </summary>
    public partial class BudgetDtlPersonnelDac : EntDacBase<BudgetDtlPersonnelModel>, IBudgetDtlPersonnelDac
    {
		#region 实现 IBudgetDtlPersonnelDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<BudgetDtlPersonnelModel> ExampleMethod<BudgetDtlPersonnel>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

