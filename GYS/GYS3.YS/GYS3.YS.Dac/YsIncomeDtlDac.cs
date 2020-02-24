#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Dac
    * 类 名 称：			YsIncomeDtlDac
    * 文 件 名：			YsIncomeDtlDac.cs
    * 创建时间：			2019/12/31 
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
	/// YsIncomeDtl数据访问处理类
	/// </summary>
    public partial class YsIncomeDtlDac : EntDacBase<YsIncomeDtlModel>, IYsIncomeDtlDac
    {
		#region 实现 IYsIncomeDtlDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<YsIncomeDtlModel> ExampleMethod<YsIncomeDtl>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

