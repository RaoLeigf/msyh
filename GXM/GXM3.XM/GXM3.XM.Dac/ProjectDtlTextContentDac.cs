#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectDtlTextContentDac
    * 命名空间：        GXM3.XM.Dac
    * 文 件 名：        ProjectDtlTextContentDac.cs
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
	/// ProjectDtlTextContent数据访问处理类
	/// </summary>
    public partial class ProjectDtlTextContentDac : EntDacBase<ProjectDtlTextContentModel>, IProjectDtlTextContentDac
    {
		#region 实现 IProjectDtlTextContentDac 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<ProjectDtlTextContentModel> ExampleMethod<ProjectDtlTextContent>(string param)
        //{
        //    //编写代码
        //}

		#endregion
    }
}

