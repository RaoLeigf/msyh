#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectDtlFundApplDac
    * 命名空间：        GXM3.XM.Dac
    * 文 件 名：        ProjectDtlFundApplDac.cs
    * 创建时间：        2018/8/28 
    * 作    者：        李明    
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

using GXM3.XM.Model.Domain;
using GXM3.XM.Dac.Interface;

namespace GXM3.XM.Dac
{
	/// <summary>
	/// ProjectDtlFundAppl数据访问处理类
	/// </summary>
    public partial class ProjectDtlFundApplDac : EntDacBase<ProjectDtlFundApplModel>, IProjectDtlFundApplDac
    {
		#region 实现 IProjectDtlFundApplDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<ProjectDtlFundApplModel> ExampleMethod<ProjectDtlFundAppl>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

