#region Summary
/**************************************************************************************
    * 类 名 称：        ExpenseMstDac
    * 命名空间：        GYS3.YS.Dac
    * 文 件 名：        ExpenseMstDac.cs
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
	/// ExpenseMst数据访问处理类
	/// </summary>
    public partial class ExpenseMstDac : EntDacBase<ExpenseMstModel>, IExpenseMstDac
    {
		#region 实现 IExpenseMstDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<ExpenseMstModel> ExampleMethod<ExpenseMst>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

