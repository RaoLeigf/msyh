#region Summary
/**************************************************************************************
    * 类 名 称：        QTIndividualInfoModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QTIndividualInfoModel.cs
    * 创建时间：        2019/5/14 
    * 作    者：        董泉伟    
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
    /// QTIndividualInfo实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QTIndividualInfoTempleModel : EntityBase<QTIndividualInfoTempleModel>
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
		/// 名称
		/// </summary>
		[DataMember]
        public virtual System.String name
        {
            get;
            set;
        }

        /// <summary>
        /// 自定义表单类型
        /// </summary>
        [DataMember]
		public virtual System.String bustype
        {
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String defaultflg
        {
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String isbackup
        {
			get;
			set;
		}

		/// <summary>
		///  备注
		/// </summary>
		[DataMember]
		public virtual System.String remark
        {
			get;
			set;
		}


	}

}