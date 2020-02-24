#region Summary
/**************************************************************************************
    * 类 名 称：        EnumUserStatus
    * 命名空间：        GQT3.QT.Model.Enums
    * 文 件 名：        EnumUserStatus.cs
    * 创建时间：        2018/9/13 
    * 作    者：        夏华军    
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

namespace GQT3.QT.Model.Enums
{
    /// <summary>
    /// EnumUserStatus
    /// </summary>
    public enum EnumUserStatus
    {
		/// <summary>
		/// 0-已删除
		/// </summary>
		Deleted = 0,

		/// <summary>
		/// 1-激活
		/// </summary>
		Activate = 1,

		/// <summary>
		/// 2-禁用
		/// </summary>
		Disable = 2,

		/// <summary>
		/// 3-锁定
		/// </summary>
		Locked = 3,

		/// <summary>
		/// 4-过期
		/// </summary>
		Expired = 4
    }
}