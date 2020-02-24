#region Summary
/**************************************************************************************
    * 类 名 称：        MethodAroundMstModel
    * 命名空间：        NG3.Addin.Model.Domain
    * 文 件 名：        MethodAroundMstModel.cs
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
    /// MethodAroundMst实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class MethodAroundMstModel : EntityBase<MethodAroundMstModel>
    {
		/// <summary>
		/// phid
		/// </summary>
		[DataMember]
		public virtual System.Int64 Phid
		{
			get;
			set;
		}

        /// <summary>
        /// phid
        /// </summary>
        [DataMember]
        public virtual System.Int64 TargetServiceId
        {
            get;
            set;
        }

        /// <summary>
        /// 服务名
        /// </summary>
        [DataMember]
        public virtual System.String TargetServiceName
        {
            get;
            set;
        }


        /// <summary>
        /// 目标类名
        /// </summary>
        [DataMember]
		public virtual System.String TargetClassName
		{
			get;
			set;
		}

		/// <summary>
		/// 目标程序集名称
		/// </summary>
		[DataMember]
		public virtual System.String TargetAssemblyName
		{
			get;
			set;
		}

		/// <summary>
		/// 目标方法名
		/// </summary>
		[DataMember]
		public virtual System.String TargetMethodName
		{
			get;
			set;
		}

		/// <summary>
		/// 拦截类型
		/// </summary>
		[DataMember]
		public virtual EnumInterceptorType InterceptorType
		{
			get;
			set;
		}

		/// <summary>
		/// 插件类型
		/// </summary>
		[DataMember]
		public virtual EnumAddinType AddinType
		{
			get;
			set;
		}
        /// <summary>
        /// 是否发布
        /// </summary>
        [DataMember]
        public virtual System.Int32 DeployFlag { set; get; }

        /// <summary>
        /// 匹配子句,判断整个服务注入是否能够匹配
        /// </summary>
        [DataMember]
        public virtual System.String MatchClause { set; get; }
    }

}