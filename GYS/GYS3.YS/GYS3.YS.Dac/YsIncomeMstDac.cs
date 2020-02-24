#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Dac
    * 类 名 称：			YsIncomeMstDac
    * 文 件 名：			YsIncomeMstDac.cs
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
	/// YsIncomeMst数据访问处理类
	/// </summary>
    public partial class YsIncomeMstDac : EntDacBase<YsIncomeMstModel>, IYsIncomeMstDac
    {
		#region 实现 IYsIncomeMstDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<YsIncomeMstModel> ExampleMethod<YsIncomeMst>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

