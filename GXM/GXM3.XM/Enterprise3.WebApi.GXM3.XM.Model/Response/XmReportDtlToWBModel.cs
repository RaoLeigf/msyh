using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GXM3.XM.Model.Response
{
    /// <summary>
    /// 返回给java网报系统的签报明细对象
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class XmReportDtlToWBModel
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
		/// 费用项目名称
		/// </summary>
		[DataMember]
        public virtual System.String XmName
        {
            get;
            set;
        }
        /// <summary>
		/// 单价
		/// </summary>
		[DataMember]
        public virtual System.Decimal FPrice
        {
            get;
            set;
        }
        /// <summary>
		/// 明细金额
		/// </summary>
		[DataMember]
        public virtual System.Decimal FAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 是否为固定成本
        /// </summary>
        [DataMember]
        public virtual System.Int32 FIsCost
        {
            get;
            set;
        }

        /// <summary>
		/// 费用项目代码
		/// </summary>
		[DataMember]
        public virtual System.String CostitemCode
        {
            get;
            set;
        }

        /// <summary>
		/// 返回金额
		/// </summary>
		[DataMember]
        public virtual System.Decimal FReturnAmount
        {
            get;
            set;
        }

        /// <summary>
		/// 变量1
		/// </summary>
		[DataMember]
        public virtual System.Decimal FVariable1
        {
            get;
            set;
        }
        /// <summary>
		/// 单位1
		/// </summary>
		[DataMember]
        public virtual System.String FUnit1
        {
            get;
            set;
        }
        /// <summary>
		/// 变量2
		/// </summary>
		[DataMember]
        public virtual System.Decimal FVariable2
        {
            get;
            set;
        }
        /// <summary>
		/// 单位2
		/// </summary>
		[DataMember]
        public virtual System.String FUnit2
        {
            get;
            set;
        }
        /// <summary>
        /// 变量3
		/// </summary>
		[DataMember]
        public virtual System.Decimal FVariable3
        {
            get;
            set;
        }
        /// <summary>
		/// 单位3
		/// </summary>
		[DataMember]
        public virtual System.String FUnit3
        {
            get;
            set;
        }
    }
}
