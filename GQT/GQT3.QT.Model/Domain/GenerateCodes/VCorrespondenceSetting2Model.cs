#region Summary
/**************************************************************************************
    * 类 名 称：        VCorrespondenceSetting2Model
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        VCorrespondenceSetting2Model.cs
    * 创建时间：        2018/9/13 
    * 作    者：        李长敏琛    
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

namespace GQT3.QT.Model.Domain
{
    /// <summary>
    /// VCorrespondenceSetting2实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class VCorrespondenceSetting2Model : EntityBase<VCorrespondenceSetting2Model>
    {
		/// <summary>
		/// 主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 PhId
		{
			get;
			set;
		}

		/// <summary>
		/// 单位代码
		/// </summary>
		[DataMember]
		public virtual System.String DWDM
		{
			get;
			set;
		}

		/// <summary>
		/// 支付渠道代码
		/// </summary>
		[DataMember]
		public virtual System.String DYDM
		{
			get;
			set;
		}

		/// <summary>
		/// 对应类型
		/// </summary>
		[DataMember]
		public virtual System.String DYLX
		{
			get;
			set;
		}

		/// <summary>
		/// 单位名称
		/// </summary>
		[DataMember]
		public virtual System.String Dwmc
		{
			get;
			set;
		}

		/// <summary>
		/// 渠道名称
		/// </summary>
		[DataMember]
		public virtual System.String Dymc
		{
			get;
			set;
		}

	}

}