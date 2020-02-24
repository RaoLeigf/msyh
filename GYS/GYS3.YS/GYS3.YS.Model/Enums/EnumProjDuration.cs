#region Summary
/**************************************************************************************
    * 类 名 称：        EnumProjDuration
    * 命名空间：        GYS3.YS.Model.Enums
    * 文 件 名：        EnumProjDuration.cs
    * 创建时间：        2018/8/30 
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

namespace GYS3.YS.Model.Enums
{
    /// <summary>
    /// EnumProjDuration
    /// </summary>
    public enum EnumProjDuration
    {
		/// <summary>
		/// 1-一次性项目
		/// </summary>
		OneOff = 1,

		/// <summary>
		/// 2-经常性项目
		/// </summary>
		Frequent = 2,

		/// <summary>
		/// 3-跨年度项目
		/// </summary>
		CrossYear = 3
    }
}