#region Summary
/**************************************************************************************
    * 类 名 称：        PerformanceDtlTextContModel
    * 命名空间：        GJX3.JX.Model.Domain
    * 文 件 名：        PerformanceDtlTextContModel.cs
    * 创建时间：        2018/9/12 
    * 作    者：        吾丰明    
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

namespace GJX3.JX.Model.Domain
{
    /// <summary>
    /// PerformanceDtlTextCont实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class PerformanceDtlTextContModel : EntityBase<PerformanceDtlTextContModel>
    {
		/// <summary>
		/// 构造函数
		/// </summary>
		public PerformanceDtlTextContModel()
		{
			List<PropertyColumnMapperInfo> list = new List<PropertyColumnMapperInfo>();

			PropertyColumnMapperInfo info = new PropertyColumnMapperInfo();
			info.PropertyName = "MstPhid";
			info.ColumnName = "mst_phid";
			info.PropertyType = EnumPropertyType.Long;
			info.IsPrimary = false;
			list.Add(info);

			ForeignKeys = list;//设置外键字段属性
		}

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
		/// 主表主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 MstPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 其他说明问题
		/// </summary>
		[DataMember]
		public virtual System.String FOtherInstructions
		{
			get;
			set;
		}

        /// <summary>
		/// 自评结论
		/// </summary>
		[DataMember]
        public virtual System.String FConclusion
        {
            get;
            set;
        }

        /// <summary>
		/// 抽评结论
		/// </summary>
		[DataMember]
        public virtual System.String FCheckConclusion
        {
            get;
            set;
        }

        /// <summary>
        /// 第三方评价结论
        /// </summary>
        [DataMember]
        public virtual System.String FThirdCheckConclusion
        {
            get;
            set;
        }

    }

}