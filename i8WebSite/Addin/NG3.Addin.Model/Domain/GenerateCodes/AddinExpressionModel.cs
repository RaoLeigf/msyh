#region Summary
/**************************************************************************************
    * 类 名 称：        AddinExpressionModel
    * 命名空间：        NG3.Addin.Model.Domain
    * 文 件 名：        AddinExpressionModel.cs
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
using System.Runtime.Serialization;
using Enterprise3.Common.Model;
using Enterprise3.Common.Model.NHORM;
using Enterprise3.Common.Model.Enums;

namespace NG3.Addin.Model.Domain
{
    /// <summary>
    /// AddinExpression实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class AddinExpressionModel : EntityBase<AddinExpressionModel>
    {

        /// <summary>
        /// 
        /// </summary>
        public AddinExpressionModel()
        {
            ForeignKeys = new List<PropertyColumnMapperInfo>
            {
                new PropertyColumnMapperInfo
                {
                    PropertyName = "MstPhid",
                    ColumnName = "mstphid",
                    PropertyType = EnumPropertyType.Long,
                    IsPrimary = false
                }

            };
        }

        /// <summary>
        /// Phid
        /// </summary>
        [DataMember]
		public virtual System.Int64 Phid
		{
			get;
			set;
		}

		/// <summary>
		/// 主表phid
		/// </summary>
		[DataMember]
		public virtual System.Int64 MstPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 表达式文本
		/// </summary>
		[DataMember]
		public virtual System.String ExpText
		{
			get;
			set;
		}

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public virtual System.String Msg
        {
            set; get;
        }

        /// <summary>
        /// 
        /// </summary>

        [DataMember]
        public virtual System.Int32 IsContinue
        {
            set; get;
        }
	}

}