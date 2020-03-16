using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GQT3.QT.Model.Response
{
    /// <summary>
    /// 返回的项目分发对象
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class XmDistributeModel
    {
        /// <summary>
		/// 项目代码
		/// </summary>
		[DataMember]
        public virtual System.String FProjcode
        {
            get;
            set;
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        [DataMember]
        public virtual System.String FProjname
        {
            get;
            set;
        }
        /// <summary>
		/// 业务条线代码
		/// </summary>
		[DataMember]
        public virtual string FBusiness
        {
            get;
            set;
        }
        /// <summary>
		/// 业务条线名称
		/// </summary>
		[DataMember]
        public virtual string FBusiness_EXName
        {
            get;
            set;
        }
        /// <summary>
		/// 启用组织列表
		/// </summary>
		[DataMember]
        public virtual List<Int64> EnableOrgList
        {
            get;
            set;
        }
        /// <summary>
		/// 启用组织列表带是否能分发
		/// </summary>
		[DataMember]
        public virtual List<object> EnableOrgList2
        {
            get;
            set;
        }
        /// <summary>
		/// 启用组织中文拼接
		/// </summary>
		[DataMember]
        public virtual string EnableOrgStr
        {
            get;
            set;
        }
        /// <summary>
		/// 是否能修改
		/// </summary>
		[DataMember]
        public virtual bool CanUpdate
        {
            get;
            set;
        }

        /// <summary>
		/// 是否能分发
		/// </summary>
		[DataMember]
        public virtual bool CanFF
        {
            get;
            set;
        }

        /// <summary>
        /// 机构Id
        /// </summary>
        [DataMember]
        public virtual System.Int64 CurOrgId
        {
            get;
            set;
        }

        /// <summary>
        /// 启用或停用 true代表启用，false代表停用
        /// </summary>
        [DataMember]
        public virtual byte IfUse
        {
            get;
            set;
        }
        #region 项目分发所需
        /// <summary>
        /// 分发组织id
        /// </summary>
        [DataMember]
        public virtual System.Int64 orgid
        {
            get;
            set;
        }

        /// <summary>
        /// 分发操作员id
        /// </summary>
        [DataMember]
        public virtual System.Int64 userid
        {
            get;
            set;
        }
        #endregion
    }
}
