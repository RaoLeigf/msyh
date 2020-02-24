#region Summary
/**************************************************************************************
    * 类 名 称：        EnumInterceptorType
    * 命名空间：        NG3.Addin.Model.Enums
    * 文 件 名：        EnumInterceptorType.cs
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
    /// EnumInterceptorType
    /// </summary>
    public enum EnumInterceptorType
    {
		/// <summary>
		/// 0-方法前
		/// </summary>
		Before = 0,

		/// <summary>
		/// 1-方法后
		/// </summary>
		After = 1,
        /// <summary>
        /// 没有设置
        /// </summary>
        None =2
    }
}