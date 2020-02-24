#region Summary
/**************************************************************************************
    * 类 名 称：        AddinOperatorModel
    * 命名空间：        NG3.Addin.Model.Domain
    * 文 件 名：        AddinOperatorModel.cs
    * 创建时间：        2017/8/3 
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
using System.Runtime.Serialization;
using Enterprise3.Common.Model;
using Enterprise3.Common.Model.NHORM;
using Enterprise3.Common.Model.Enums;

namespace NG3.Addin.Model.Domain
{
    /// <summary>
    /// AddinOperator实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class AddinOperatorModel : EntityBase<AddinOperatorModel>
    {
		/// <summary>
		/// PHID
		/// </summary>
		[DataMember]
		public virtual System.Int64 Phid
		{
			get;
			set;
		}

		/// <summary>
		/// 人员ID
		/// </summary>
		[DataMember]
		public virtual System.String LoginId
		{
			get;
			set;
		}

	}

}