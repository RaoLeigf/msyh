#region Summary
/**************************************************************************************
    * 类 名 称：        EnumProjStatus
    * 命名空间：        GYS3.YS.Model.Enums
    * 文 件 名：        EnumProjStatus.cs
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
    /// EnumProjStatus
    /// </summary>
    public enum EnumProjStatus
    {
		/// <summary>
		/// 1-单位备选
		/// </summary>
		Alternative = 1,

		/// <summary>
		/// 2-纳入预算
		/// </summary>
		InBudget = 2,

		/// <summary>
		/// 3-项目执行
		/// </summary>
		Execute = 3,

		/// <summary>
		/// 4-项目调整
		/// </summary>
		Adjust = 4,

		/// <summary>
		/// 5-项目暂停
		/// </summary>
		Pause = 5,

		/// <summary>
		/// 6-项目终止
		/// </summary>
		Terminated = 6,

		/// <summary>
		/// 7-项目关闭
		/// </summary>
		Closed = 7
    }
}