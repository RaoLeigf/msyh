#region Summary
/**************************************************************************************
    * 类 名 称：        ThirdAttachmentModel
    * 命名空间：        GJX3.JX.Model.Domain
    * 文 件 名：        ThirdAttachmentModel.cs
    * 创建时间：        2019/10/9 
    * 作    者：        刘杭    
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
using GQT3.QT.Model.Domain;

namespace GJX3.JX.Model.Domain
{
    /// <summary>
    /// ThirdAttachment实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class ThirdAttachmentModel : EntityBase<ThirdAttachmentModel>
    {
		/// <summary>
		/// 构造函数
		/// </summary>
		public ThirdAttachmentModel()
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
		/// 外键
		/// </summary>
		[DataMember]
		public virtual System.Int64 MstPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 评价时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FTime
		{
			get;
			set;
		}

		/// <summary>
		/// 评价内容
		/// </summary>
		[DataMember]
		public virtual System.String FText
		{
			get;
			set;
		}

		/// <summary>
		/// 单位名称代码
		/// </summary>
		[DataMember]
		public virtual System.String FDeclarationUnit
		{
			get;
			set;
		}

        /// <summary>
		/// 单位名称
		/// </summary>
		[DataMember]
        public virtual System.String FDeclarationUnit_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 项目代码
        /// </summary>
        [DataMember]
		public virtual System.String FProjCode
		{
			get;
			set;
		}

		/// <summary>
		/// 项目名称
		/// </summary>
		[DataMember]
		public virtual System.String FProjName
		{
			get;
			set;
		}

		/// <summary>
		/// 评价机构
		/// </summary>
		[DataMember]
		public virtual System.String FAgency
		{
			get;
			set;
		}

		/// <summary>
		/// 项目负责人
		/// </summary>
		[DataMember]
		public virtual System.String FLeader
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR1
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR2
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR3
		{
			get;
			set;
		}


        /// <summary>
        /// 相关附件集合
        /// </summary>
        [DataMember]
        public virtual IList<QtAttachmentModel> ThirdQtAttachments
        {
            get;
            set;
        }
    }

}