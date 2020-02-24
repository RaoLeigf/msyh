#region Summary
/**************************************************************************************
    * 类 名 称：        EnumAddinType
    * 命名空间：        NG3.Addin.Model.Enums
    * 文 件 名：        EnumAddinType.cs
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
    /// EnumAddinType
    /// </summary>
    public enum EnumAddinType
    {
		/// <summary>
		/// 0-SQL语句
		/// </summary>
		Sql = 0,

		/// <summary>
		/// 1-程序集
		/// </summary>
		Assembly = 1,

		/// <summary>
		/// 2-表达式
		/// </summary>
		Expression = 2,

		/// <summary>
		/// 3-Url
		/// </summary>
		Url = 3
    }
}