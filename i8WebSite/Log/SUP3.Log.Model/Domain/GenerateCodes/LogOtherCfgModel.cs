#region Summary
/**************************************************************************************
    * 类 名 称：        LogOtherCfgModel
    * 命名空间：        SUP3.Log.Model.Domain
    * 文 件 名：        LogOtherCfgModel.cs
    * 创建时间：        2017/10/16 
    * 作    者：        洪鹏    
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
using System.Runtime.Serialization;
using Enterprise3.Common.Model;
using Enterprise3.Common.Model.NHORM;
using Enterprise3.Common.Model.Enums;

namespace SUP3.Log.Model.Domain
{
    /// <summary>
    /// LogOtherCfg实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class LogOtherCfgModel : EntityBase<LogOtherCfgModel>
    {
		/// <summary>
		/// PK
		/// </summary>
		[DataMember]
		public virtual System.Int64 Phid
		{
			get;
			set;
		}

		/// <summary>
		/// 配置KEY
		/// </summary>
		[DataMember]
		public virtual System.String CfgKey
		{
			get;
			set;
		}

		/// <summary>
		/// 描述说明
		/// </summary>
		[DataMember]
		public virtual System.String CfgName
		{
			get;
			set;
		}

		/// <summary>
		/// 配置值
		/// </summary>
		[DataMember]
		public virtual System.String CfgValue
		{
			get;
			set;
		}

	}

}