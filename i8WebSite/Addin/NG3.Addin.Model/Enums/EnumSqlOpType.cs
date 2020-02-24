#region Summary
/**************************************************************************************
    * 类 名 称：        EnumSqlOpType
    * 命名空间：        NG3.Addin.Model.Enums
    * 文 件 名：        EnumSqlOpType.cs
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
    /// EnumSqlOpType
    /// </summary>
    public enum EnumSqlOpType
    {
		/// <summary>
		/// 0-Sql
		/// </summary>
		Sql = 0,

		/// <summary>
		/// 1-存储过程
		/// </summary>
		Sp = 1,

		/// <summary>
		/// 2-函数
		/// </summary>
		Func = 2,

		/// <summary>
		/// 3-内置方法
		/// </summary>
		DataTable = 3
    }
}