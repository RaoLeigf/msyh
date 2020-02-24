#region Summary
/**************************************************************************************
    * 类 名 称：        EnumUIDataSourceType
    * 命名空间：        NG3.Addin.Model.Enums
    * 文 件 名：        EnumUIDataSourceType.cs
    * 创建时间：        2017/7/10 
    * 作    者：        韦忠吉    
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

namespace NG3.Addin.Model.Enums
{
    /// <summary>
    /// EnumUIDataSourceType
    /// </summary>
    public enum EnumUIDataSourceType
    {
		/// <summary>
		/// 0-空
		/// </summary>
		Nul = 0,

		/// <summary>
		/// 1-新增行
		/// </summary>
		New = 1,

		/// <summary>
		/// 2-删除行
		/// </summary>
		Del = 2,

		/// <summary>
		/// 3-修改行
		/// </summary>
		Mod = 3,

		/// <summary>
		/// 4-所有行
		/// </summary>
		All = 4
    }
}