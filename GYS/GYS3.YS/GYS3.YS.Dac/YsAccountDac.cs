#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Dac
    * 类 名 称：			YsAccountDac
    * 文 件 名：			YsAccountDac.cs
    * 创建时间：			2019/9/23 
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
	/// YsAccount数据访问处理类
	/// </summary>
    public partial class YsAccountDac : EntDacBase<YsAccountModel>, IYsAccountDac
    {
		#region 实现 IYsAccountDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<YsAccountModel> ExampleMethod<YsAccount>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

