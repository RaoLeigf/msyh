#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Dac
    * 类 名 称：			YsAccountMstDac
    * 文 件 名：			YsAccountMstDac.cs
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
	/// YsAccountMst数据访问处理类
	/// </summary>
    public partial class YsAccountMstDac : EntDacBase<YsAccountMstModel>, IYsAccountMstDac
    {
		#region 实现 IYsAccountMstDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<YsAccountMstModel> ExampleMethod<YsAccountMst>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

