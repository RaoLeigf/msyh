#region Summary
/**************************************************************************************
    * 类 名 称：        ExpenseDtlDac
    * 命名空间：        GYS3.YS.Dac
    * 文 件 名：        ExpenseDtlDac.cs
    * 创建时间：        2019/1/24 
    * 作    者：        董泉伟    
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
	/// ExpenseDtl数据访问处理类
	/// </summary>
    public partial class ExpenseDtlDac : EntDacBase<ExpenseDtlModel>, IExpenseDtlDac
    {
		#region 实现 IExpenseDtlDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<ExpenseDtlModel> ExampleMethod<ExpenseDtl>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

