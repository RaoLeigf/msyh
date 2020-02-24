#region Summary
/**************************************************************************************
    * 类 名 称：        ProjLibProjModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        ProjLibProjModel.cs
    * 创建时间：        2018/9/10 
    * 作    者：        李明    
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
    /// ProjLibProj实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class ProjLibProjModel : EntityBase<ProjLibProjModel>
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
		/// 项目代码
		/// </summary>
		[DataMember]
		public virtual System.String DM
		{
			get;
			set;
		}

		/// <summary>
		/// 项目名称
		/// </summary>
		[DataMember]
		public virtual System.String MC
		{
			get;
			set;
		}

		/// <summary>
		/// 预算科目
		/// </summary>
		[DataMember]
		public virtual System.String YSKM
		{
			get;
			set;
		}

		/// <summary>
		/// 是否录过数据
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR1
		{
			get;
			set;
		}

		/// <summary>
		/// 父节点代码
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR2
		{
			get;
			set;
		}

		/// <summary>
		/// 申报单位
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR3
		{
			get;
			set;
		}

		/// <summary>
		/// DEF_INT1
		/// </summary>
		[DataMember]
		public virtual System.Int32 DEFINT1
		{
			get;
			set;
		}

		/// <summary>
		/// DEF_INT2
		/// </summary>
		[DataMember]
		public virtual System.Int32 DEFINT2
		{
			get;
			set;
		}

        /// <summary>
		/// 预算部门
		/// </summary>
		[DataMember]
        public virtual System.String DEFSTR4
        {
            get;
            set;
        }

    }

}