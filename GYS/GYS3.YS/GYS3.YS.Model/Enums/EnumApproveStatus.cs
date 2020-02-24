#region Summary
/**************************************************************************************
    * 类 名 称：        EnumApproveStatus
    * 命名空间：        GYS3.YS.Model.Enums
    * 文 件 名：        EnumApproveStatus.cs
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
    /// EnumApproveStatus
    /// </summary>
    public enum EnumApproveStatus
    {
		/// <summary>
		/// 1-待上报
		/// </summary>
		ToBeRepored = 1,

		/// <summary>
		/// 2-审批中
		/// </summary>
		IsPending = 2,

		/// <summary>
		/// 3-已审批
		/// </summary>
		Approved = 3
    }
}