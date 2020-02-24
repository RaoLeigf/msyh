#region Summary
/**************************************************************************************
    * 类 名 称：        AddinAssemblyModel
    * 命名空间：        NG3.Addin.Model.Domain
    * 文 件 名：        AddinAssemblyModel.cs
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
using NG3.Addin.Model.Enums;

namespace NG3.Addin.Model.Domain
{
    /// <summary>
    /// AddinAssembly实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class AddinAssemblyModel : EntityBase<AddinAssemblyModel>
    {

        /// <summary>
        /// 
        /// </summary>
        public AddinAssemblyModel()
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
		/// 主表PHID
		/// </summary>
		[DataMember]
		public virtual Int64 MstPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 程序集名称
		/// </summary>
		[DataMember]
		public virtual System.String AssemblyName
		{
			get;
			set;
		}

		/// <summary>
		/// 插件类名
		/// </summary>
		[DataMember]
		public virtual System.String ClassName
		{
			get;
			set;
		}

        /// <summary>
        /// 插件的类别，主要是功能扩展与功能环绕两种
        /// </summary>
        [DataMember]
        public virtual EnumCatalog AssemblyCatalog
        {
            get;
            set;
        }

	}

}